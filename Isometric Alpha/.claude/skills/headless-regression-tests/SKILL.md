---
name: headless-regression-tests
description: Use when asked to build, write, extend, run or debug regression tests, smoke tests or automated test scripts that run the game headless (-batchmode / -nographics), including scripts that click through menus, dialogue, loading screens or combat with no player. Covers the three-part test design (command file, HeadlessLogger log produced by instrumentation in the game's own source, query file evaluated against that log), the event format, simulated keyboard and mouse input through VirtualInputDriver, protecting the player's saves, and running tests through HeadlessBuildRunner. Also use when updating this skill after such work.
---

# Headless regression tests

A regression test has **three parts**, and they stay strictly separate:

| Part | What it is | Contains | Never contains |
|---|---|---|---|
| **1. Command file** | `RegressionTests/<Name>/<Name>.commands` | Actions only: press this key, click this object, wait for this event | HeadlessLogger calls, checks, expectations |
| **2. Log file** | `HeadlessLogs/<Name>.log`, written while the commands run | Events logged by `HeadlessLogger` calls that live **in the game's source code** | Anything test-specific |
| **3. Query file** | `RegressionTests/<Name>/<Name>.queries` | Queries run over the log afterwards; together they decide whether each step of the command file succeeded | Actions |

The command file only *does* things. The game's own instrumented code *reports* what happened. The queries *judge* the report. A test passes only when the log proves every step worked; a command finishing without an error proves nothing.

These are not Unity Test Framework tests. `com.unity.test-framework` is installed but unused, and the game has no asmdefs.

## Existing pieces (read before writing anything)

| File | What it gives you |
|---|---|
| [HeadlessLogger.cs](../../../Assets/src/Debug/HeadlessLogger.cs) | Static. `IsHeadless` (cached `Application.isBatchMode`), `CreateLogFile(name)`, `Log`, `LogWarning`, `LogError`, `LogEvent(eventName, key, value, ...)` (+ `LogWarningEvent`, `LogErrorEvent`; quotes values with spaces, `"` or `=`), `event Action<string> LineWritten` (raised after each line, outside the file lock), `CurrentLogFilePath`, `DefaultFolder` (`<exe dir>/HeadlessLogs`). Every call checks headless mode itself and does nothing in a normal run. Lines look like `[14:05:11.204] [ERROR] msg` (time of day only, no date). |
| [HeadlessGameEvents.cs](../../../Assets/src/Debug/HeadlessGameEvents.cs) | Game-wide events with no owning class: `Scene.Loaded`, `Scene.Unloaded`, `App.Quitting`, `Transition.Finished`. Subscribes at BeforeSceneLoad, headless only. |
| [RegressionCommandBootstrap.cs](../../../Assets/src/Debug/RegressionTests/RegressionCommandBootstrap.cs) / [RegressionCommandRunner.cs](../../../Assets/src/Debug/RegressionTests/RegressionCommandRunner.cs) | The command runner (see [Shared infrastructure](#shared-infrastructure)). |
| [RegressionTests/run_test.py](../../../RegressionTests/run_test.py) | `python RegressionTests/run_test.py <Name> [--graphics] [--timeout 900] [--exe ...]`: wipes `<exe dir>/HeadlessData`, runs the game with the test's commands, waits for it to quit, then evaluates. The one command to run a test. |
| [RegressionTests/evaluate_queries.py](../../../RegressionTests/evaluate_queries.py) | `python evaluate_queries.py <queries> <log> [--commands ...]`: the part 3 evaluator. `run_evaluation()` is importable. |
| [HeadlessBuildRunner.cs](../../../Assets/src/Editor/HeadlessBuildRunner.cs) | Editor menu `Tools/Build/Run Headless Build` and `Rebuild And Run Headless`. Rebuilds `D:\Fold Builds\Windows\Isometric Alpha.exe` if the build isn't from today, launches it with `-batchmode -nographics -logFile headless-player.log`, kills it after 15/30/60/120 s, reports exceptions in the player log. Also `Tools/Build/Regression Tests/Run <Name>` and `Rebuild And Run <Name>`, which run `run_test.py` with no kill timer and print its report to the Console (error on fail). Add a pair of menu items per new test. |
| [VirtualInputDriver.cs](../../../Assets/src/Debug/RegressionTests/VirtualInputDriver.cs) | Static, batch mode only. `ensureDevices()` adds a virtual keyboard and mouse (a headless run has no input devices at all). `hold`/`release`/`tap` take a `KeyBind` (its current key, so tests survive rebinding) or a `KeyCode`; `releaseAll()`; `moveMouse`, `setMouseButton`, `scroll`, `click(GameObject)`, `tryGetScreenPoint(GameObject)`; `tryGetKeyBind(fieldName)` looks a bind up by its `KeyBindingList` field name. |
| [InputReader.cs](../../../Assets/src/State/KeyPresses/InputReader.cs) | How the game reads held keys, modifiers and the mouse (all through the Input System, so the virtual devices drive them). Discrete presses are `CustomInputAction`s in `PlayerInputList`, enabled per activity or per input overlay by `PlayerStateManager.updateEnabledInputActions()`. |
| `~/.claude/scripts/unity_compile_check.py` | Headless compile check on a mirror copy. Run it after every code change; exit 0 = clean. |

The shared infrastructure below exists (built 2026-10-24). Reuse and extend it instead of duplicating it.

## Rules

1. **No logger calls in command files.** A command file never contains, triggers or names a HeadlessLogger call meant to prove its own result. If a step can't be proven, add instrumentation to the game code; never add a "log that it worked" command.
2. **Instrumentation lives in the game's source and is test-agnostic.** HeadlessLogger calls go in the gameplay or UI class that does the work, at the moment it happens (for example `CharacterCreationPopUpWindow` logs that it opened). They describe game behaviour only. They never mention a test, a command file, a step label or a query, and they fire the same way whichever test is running, or during a plain headless smoke run.
3. **Each instrumentation call is self-contained.** It relies on HeadlessLogger's own headless check and needs no outside setup or flags: no `if(testRunning)` and no test-runner references. When building the message costs something (string building, collection walks), wrap the call in `if(HeadlessLogger.IsHeadless)` so a normal game pays nothing.
4. **No HeadlessLogger calls in per-frame methods, ever, without the user's approval.** If one seems necessary, stop and ask first (see [Per-frame methods](#per-frame-methods-never-log-there-ask-first)).
5. **Tests start only when asked.** The command runner starts only when `HeadlessLogger.IsHeadless` is true **and** `-regressionCommands <path>` is on the command line.
6. **Name targets by object, never by raw screen coordinates.** Mouse commands name a GameObject and the driver works out its screen point, so a layout change doesn't silently send the click somewhere else.
7. **Simulate the player's input; bypass only as a last resort.** Press keys and click objects through `VirtualInputDriver`, so the real input path is tested. A `call` bypass is only for what simulated input can't reach (see [Input](#input-simulate-it)). If a bypass needs gameplay code changed, add the smallest public hook possible and tell the user what you added and why.
8. **Never touch the player's real saves or config** without the user's OK (see [Protecting player data](#protecting-player-data)).
9. **Every headless regression script runs at `Time.timeScale = 10`.** The runner sets it before the first command and keeps it there (see the runner notes), so scaled-time fades and animations finish 10× faster.
10. **Every wait has a real-time timeout** (`Time.realtimeSinceStartup`), because `Time.timeScale` is 10 during tests and combat changes it.
11. Follow the shell rules in the user's global CLAUDE.md for every command.

## Part 2 first: instrumenting the game

Instrument before writing commands or queries. A query can only check events the game actually logs.

**Event format.** One event per line, written through HeadlessLogger so it gets the time and level prefix:
```
[14:05:11.204] [INFO] CharacterCreation.Opened
[14:05:20.877] [INFO] Scene.Loaded scene=OpeningMonologue mode=Single
[14:05:21.003] [INFO] UI.ButtonPressed name="New Game Button" path="Canvas/Main Menu Background/Main Menu/New Game Button"
[14:06:02.441] [ERROR] Dialogue.ChoiceRejected index=3 reason=unimplemented
```
- Name events `Area.Event` in PascalCase, such as `Scene.Loaded`, `Dialogue.ChoiceMade` or `Combat.TurnStarted`.
- Follow the name with `key=value` pairs. Quote values that contain spaces, and escape `"` inside them.
- Use `LogError` for a failure the game itself detects, `LogWarning` for a recoverable oddity and `Log` for everything else.
- Always log through `HeadlessLogger.LogEvent(eventName, key, value, ...)`, so every call site writes the same format. Enum and bool values log by name (`to=Walking`, `midDialogue=False`).

**What to instrument:**
- State changes a query could ask about: scene loads, popups opening and closing, activity changes (`PlayerStateManager`), dialogue lines and choices, combat start, turns and end, saves and loads, transitions and fades finishing.
- Reusable points beat one-off ones. One `UI.ButtonPressed` in a shared spot, or a `Scene.Loaded` on `SceneManager.sceneLoaded`, serves every test.
- Log the outcome, not the intent. `CharacterCreation.Opened` belongs where the window has actually opened, not in the button handler that asks for it.
- Never log inside the command runner on the game's behalf. The runner's own lines (part 1 below) show which step was running; they are never evidence that a step worked.

### Per-frame methods: never log there, ask first

A HeadlessLogger call must never be placed in a method that runs every frame. That means any of these:
- `Update`, `LateUpdate`, `FixedUpdate` or `OnGUI`
- `OnTriggerStay*`, `OnCollisionStay*` and `OnMouseOver`
- **any line of** a coroutine that yields each frame (`yield return null`, `WaitForEndOfFrame` or `WaitForFixedUpdate` in a loop), including lines before or after that loop
- any method called every frame from one of the above, directly or through other methods

This holds even when the call is guarded so it only fires on a state change (`if(stateChanged) HeadlessLogger...`), and even inside `if(HeadlessLogger.IsHeadless)`.

Look for a one-shot place first:
- the setter or method that changes the state, such as `PlayerStateManager.setCurrentActivity`
- an existing event or callback, such as `OnFadeBackInFinished`, `OnStateChange*` or `SceneManager.sceneLoaded`, subscribed to from a one-shot method like `Awake`, `OnEnable` or `Start`
- the method that starts the coroutine, or the one-shot method a coroutine calls when it finishes

**If no one-shot place works, don't add the call.** Stop and ask the user with the AskUserQuestion tool, before writing any code for that event. Explain:
1. which event you need to log, and which test step or query needs it;
2. the method it would go in, and why that method runs every frame;
3. which one-shot alternatives you checked, and why each one doesn't work;
4. the options. For example:
   - add a small one-shot hook to the gameplay code, such as an event raised when the state changes, and log from its subscriber;
   - log in the per-frame method behind a state-change guard;
   - drop the event and prove the step another way;
   - drop that query.

Wait for the answer. Record what the user approves under [Approved per-frame exceptions](#approved-per-frame-exceptions), and don't ask again for that same exception.

## Part 1: the command file

A plain-text file, one command per line, with `#` for comments. Every command starts with a unique **step label**, which the queries refer to:
```
# StartMenuToCharacterCreation
# New Game opens character creation from a cold start.

boot:       waitFor Scene.Loaded scene=StartMenu timeout=30
new-game:   mouseClick "New Game Button" timeout=10
settle:     wait 2
```

| Command | Effect |
|---|---|
| `press <KeyBind field \| KeyCode> [frames=n]` | Tap a key through the virtual keyboard: down, held `n` frames (at least 2, the default), released. A KeyBind field name such as `interactKey` presses whatever the player has it bound to. |
| `hold <KeyBind field \| KeyCode>` / `release <...>` / `releaseAll` | Keep a key down across later commands (walking, Shift-jump, shop modifiers), then let go. The runner calls `releaseAll` when the run ends. |
| `mouseClick "<GameObject name>" [button=left\|right] [timeout=s]` | Wait for an active object with that name, move the virtual mouse onto it, press and release. Goes through the UI input module's raycasts, so hover, blocking and `IPointer*` handlers are all exercised. |
| `mouseMove "<GameObject name>"` / `mouseDown [button]` / `mouseUp [button]` | The parts of a click, for hovers and drags. |
| `scroll <amount>` | Scroll the virtual mouse; positive is up, about 120 per notch. |
| `typeText "<field name>" "<text>"` | Set a `TMP_InputField`'s text directly. Virtual keys can't type (TMP reads IMGUI events). |
| `click "<GameObject name>" [timeout=s]` | Press a `Button` through `ExecuteEvents`, skipping the pointer and raycast path. Use `mouseClick` unless the target can't be reached by the mouse (see the runner notes). |
| `call <bypass> [args...]` | Run a named bypass the runner implements in C#, only for what simulated input can't reach (see [Input](#input-simulate-it)). The bypass table is empty so far. |
| `wait <seconds>` | Real-time pause. |
| `waitFor <Event> [key=value...] [since=<step>] timeout=s` | Pause until the log has a matching event. It searches from the **previous** step's `Command.Started`, so an event set off by the step before isn't missed; `since=<earlier step>` searches from that step instead. This only paces the commands; the queries still decide success. Default timeout 30 s. |
| `playDialogue choices=i,j,... [since=<step>] [timeout=s] [stepTimeout=s]` | Play a conversation by simulated keys. Answers each `Dialogue.AwaitingInput` once: `count=0` presses `interactKey` (continue), otherwise the next 0-based index from `choices=` is pressed as its number key (index 0 → `Alpha1`). Ends at `Dialogue.Ended`. Fails on a `Dialogue.ChoiceIgnored`, when the list runs out or has leftovers at the end, or after `stepTimeout` (default 30 s) with no new event. Writes `Command.Note pressed=... for=...` per press. Never sends a number key for `count=0`: `makeChoice` with no choices continues the story without a fade check. |

Add new commands to the runner's command table, and to this table, only when an existing one can't do the job. Keep them as actions; no command may judge a result.

## Part 3: the query file

One query per line, `#` for comments. Each query belongs to a step label, or to `all`. `[Named Test]` lines group the queries that follow into one named test; the summary prints one PASS/FAIL line per group, and a group passes when all its queries pass:
```
[Reached Character Creation Screen]
boot:      expect Scene.Loaded scene=StartMenu
new-game:  expect UI.ButtonPressed name="New Game Button"
new-game:  expect CharacterCreation.Opened by=settle
all:       expectNone level=ERROR
all:       order Scene.Loaded scene=StartMenu before CharacterCreation.Opened
```

| Query | Passes when |
|---|---|
| `<step>: expect <Event> [key=value...] [by=<later step>]` | A matching event appears in the step's window. By default the window runs from that step's `Command.Started` to the next step's. `by=` widens it to the end of a later step, for effects that land late (fades, scene loads). |
| `<step>: expectNone <Event or level=LEVEL> [by=...]` | Nothing matching appears in that window. |
| `<step>: count <Event> [key=value...] == n` | Exactly n matches in the window (`>=` and `<=` also work). |
| `all: expectNone ...` / `all: order <A> before <B>` | The same checks across the whole log, or event A first appears before event B. |

The evaluator prints each named test as **PASS** or **FAIL** (with the failing query, its window and the nearest lines with the same event name). Every step label in the `.commands` file must appear in at least one query, as the query's step or its `by=` target; a step that doesn't is reported as `PROBLEM: UNVERIFIED step` and fails the run. A `Command.Failed` or `Command.ParseError` line also fails the run. Later steps never start, so their queries fail with "step X never started".

The last line is `RESULT: PASSED|FAILED (n/m tests passed)  Duration: ...  (Script.Started <date time> -> summary <date time>)`. The duration is the time from the `Script.Started` line (its `date=` key plus the line's time of day) to the moment the summary is built. Before it, `Run notes:` lists `Script.Finished reason=`, the count of forwarded Unity errors and exceptions (informational only), runner notes such as the `onClick` fallback, and any problems.

**Window gotcha:** a step's window starts at its own `Command.Started`. An event caused by step A often lands while A's command is still running or just after the next step started, so write the query on A with `by=<next step>` (for example `new-game: expect CharacterCreation.Opened by=creation-open`), not on the waitFor step that paced it.

## Shared infrastructure

Write the C# in the house style (`if(`, braces on new lines, lowerCamel private methods), then compile-check.

**Test files, in `RegressionTests/` at the project root.** It sits outside `Assets`, so Unity doesn't import it and it doesn't ship:
```
RegressionTests/
  run_test.py                  # wipe HeadlessData, run the game, evaluate
  evaluate_queries.py          # part 3 evaluator
  <Name>/<Name>.commands       # part 1
  <Name>/<Name>.queries        # part 3
```

**The command runner, in `Assets/src/Debug/RegressionTests/`:**
- `RegressionCommandBootstrap`
  - Uses `[RuntimeInitializeOnLoadMethod(BeforeSceneLoad)]`, so the log exists before the first scene's events.
  - If `HeadlessLogger.IsHeadless` and `-regressionCommands <path>` are both present, it calls `HeadlessLogger.CreateLogFile(<file name without extension>)`.
  - It then creates a `DontDestroyOnLoad` host with the `RegressionCommandRunner` component.
- `RegressionCommandRunner` (MonoBehaviour):
  - **Parsing:** reads and parses the whole file first. A parse error logs `Command.ParseError line=n` and quits with 1.
  - **Runner lines and the per-frame rule:** commands run as coroutines, so the per-frame rule covers the runner too. Write its lines from small one-shot helpers, such as `logCommandStarted(command)`, that are called once per command and never from inside a per-frame wait loop.
  - **Runner lines:** writes its own structural lines, which mark where each step starts and ends and are never proof of success:
    - `Script.Started date=yyyy-MM-dd file=... build=<Build-Timestamp.txt>`, written by the bootstrap right after `CreateLogFile`. The evaluator times the run from it.
    - `Command.Started step=<label> command=<verb> args="..."`
    - `Command.Finished step=<label> [note=gameQuit]`
    - `Command.Failed step=<label> reason="..."` (level ERROR)
    - `Command.Note step=<label> note="..."` or `pressed=... for=...`
    - `Command.ParseError reason="..."`
    - `Script.Finished reason=completed|commandFailed|parseError|gameQuit`
  - **The game quitting itself:** the runner subscribes to `Application.quitting`. If the game quits mid-command (Settings > Quit Game calls `Application.Quit()`, exit code 0), it writes `Command.Finished ... note=gameQuit` and `Script.Finished reason=gameQuit`. Exit codes are never used for judging.
  - **Forwarding Unity output:** subscribes to `Application.logMessageReceived` and forwards warnings, errors and exceptions as `Unity.Warning`, `Unity.Error` and `Unity.Exception message="..." at="<first stack line>"`. Capped at 200 warnings and 500 errors; HeadlessLogger's own messages are skipped to avoid loops.
  - **Nested coroutines:** the stepping loop flattens any `IEnumerator` a command yields (such as `VirtualInputDriver.tap`), so exceptions inside it are caught too.
  - **Time scale:** sets `Time.timeScale = 10` when the run starts, before the first command. `CombatStateManager` overwrites it on every turn change (`normalTimeScale = 1` or the resolve-turn speed, around lines 390–408, and in `setTimeScale()`), so the runner re-applies 10 at the start of every command and on each pass of its wait loops. Setting the value inside a wait loop is fine; the per-frame rule only bans HeadlessLogger calls.
  - **Exceptions in commands:** runs each command as a coroutine stepped through `MoveNext()` inside `try`/`catch`. A `try` block can't contain `yield`. An exception becomes `Command.Failed`.
  - **Timeouts:** a `click` or `waitFor` that times out logs `Command.Failed` and stops the run, so later steps aren't run against the wrong screen.
  - **Clicking:**
    - It waits one extra frame first. `DialogueChoice` caches its manager in `Start()`, so a row clicked on the frame it spawns throws a NullReferenceException.
    - With `EventSystem.current` set, it sends pointer down, up and click through `ExecuteEvents` with a `PointerEventData` whose `button = Left`. This keeps the `ThreeRingButton` and `CheckboxButton` visuals and Button's interactable check.
    - With no EventSystem, it calls `button.onClick.Invoke()` and logs `Command.Note step=<label> note="no EventSystem, invoked onClick"`.
  - **Simulated input:** calls `VirtualInputDriver.ensureDevices()` when the run starts, before the first command, and `releaseAll()` before it quits. Key commands resolve a name with `VirtualInputDriver.tryGetKeyBind` first and fall back to parsing a `KeyCode`; an unknown name is a parse error.
  - **Bypasses:** `call` names come from a `Dictionary<string, Func<string[], IEnumerator>>` of bypass actions. Keep it explicit, not reflection, so a stripped build can't lose entries.
  - **Exit:** quits with `Application.Quit(anyCommandFailed ? 1 : 0)`. The exit code only says whether every command ran; the queries decide pass or fail.

**`evaluate_queries.py <queries file> <log file>`:**
- Parses the log lines (time, level, event name, keys) and builds the step windows from the `Command.Started` lines.
- Runs every query and prints one line per named test, run notes, then the `RESULT:` line with the duration.
- Exits with 0 for a pass and 1 for a fail.
- Lines that aren't events count for `level=` checks only.
- Self-test: `selftest_evaluator.py` (kept in the session scratchpad, not the repo) built a synthetic NewGameToFirstSave log. The complete log passed 15/15 and printed a duration; with one page event removed, exactly "Reached Intro Page 3" failed with its nearest lines. Re-create a similar check after changing the evaluator.

## Game facts tests depend on

Verified 2026-10-24. Re-check line numbers before relying on them.

**Scene flow** goes through `SceneChange` (methods) and `SceneNameList` (scene name constants, e.g. `SceneNameList.startMenu`) in `Assets/src/State/SceneNameList.cs`. All loads use synchronous `LoadScene`.
- **Boot:** a `[RuntimeInitializeOnLoadMethod]` forces `StartMenu`.
- **StartMenu has no EventSystem** until `OOC UI` loads additively. One loads when the Load Game screen opens.
- **Start menu buttons** are UGUI Buttons inside `Main Menu Background.prefab`: `New Game Button`, `Load Game Button`, `Credits Button` and `Quit Button`. There is no Continue button, and **never click Quit Button** in a test.
- **New Game:** `CharacterCreationPopUpWindow` opens with one stat point to spend, and its "Start Button" stays non-interactable until that point is spent (click "Plus Top Ring"). The name is optional ("Brandon" by default). `acceptButtonPress()` waits about 10 s of fades (scaled time), then loads `OpeningMonologue`.
- **New game start:** area `2SlaveShack` in zone `Camp`; `IntroDialogue` starts by itself once `LoadSaveFile.midLoad` clears (after the loading screen continue). It ends with the player at (4,1). The hut's only exit is a transition tile at (1,1) to `NECamp` (hold `moveSouthKey`), and that transition writes an autosave.
- **Quit Game:** the Settings screen's quit button is named **"Exit Game Button"** (label "Quit to Desktop"). It opens the Binary Decision Panel; its **"Confirm Button"** runs `QuitToDesktop.execute()`, which writes the config and calls `Application.Quit()` (exit code 0). Settings opens with `settingsScreenKey` (Escape) while Walking, unless a `NotificationPopUpWindow` is up, in which case Escape only clears it.
- **Load:** `SaveHandler.quickLoadTopSave()`, or click a save row. In the main menu a load runs with no confirm dialog.

**Good instrumentation points.** There is no single "game ready" event. These are where the game's state changes, so log events near them. Several are driven by coroutines or `Update` (fades, loading-screen stages, `endWait`). For those, log from the one-shot method or event next to them, never from inside the per-frame code; see [Per-frame methods](#per-frame-methods-never-log-there-ask-first).
- `LoadSaveFile.execute()` and `performOutro()`, and the `midLoad` / `beforeSecondStageLoad` flags
- `LoadingBarProgressTracker` stages and `endWait`
- `PrebuiltScreenManager.buildFinished`
- `FadeToBlackManager`: fade start and end, and `OnFadeBackInFinished`
- `PlayerStateManager.setCurrentActivity` and its `OnStateChange*` events
- `SceneManager.sceneLoaded`
- `DialogueManager.makeChoice` and `continueStory`
- `CombatStateManager` turn changes
- `SaveHandler` writes

### Input: simulate it

Verified 2026-10-24. The game's own code no longer reads legacy `UnityEngine.Input` (the input migration). Keys and the mouse go through the Input System, which the virtual devices drive, so the screens that used to need a bypass are driven like a player would:

| Screen | Commands |
|---|---|
| Opening monologue: Space finishes the page being written, then turns it, 7 pages after ~17 s of fades | `press Space` per page. The `OpeningMonologue` input overlay only takes it once the fades end, so `waitFor` an event for that first. |
| Loading screen "press any key" | `press interactKey` (or `acceptKey`, `acceptInputKey`, or `mouseDown`/`mouseUp`) after the prompt shows. A key already held when it shows continues straight away. |
| Walking | `hold moveNorthKey` … `release moveNorthKey`. Walking continues while held, as for a player. |
| Interact, open screens, dialogue choices 1–8, combat keys | `press <bind>`; dialogue choices are the fixed keys `Alpha1`–`Alpha8`. |
| Shop max / ×10, combat jump-move | `hold maxAmountKey` (or `multiplyByTenAmountKey`, `jumpMoveKey`) while clicking or moving, then `release`. |
| Character creation name | `typeText "<field name>" "<name>"`, then `press acceptInputKey` to leave the field. |
| Key binding screen | `mouseClick` a bind button, then `press <KeyCode>`; the capture takes the physical key. |

**What still needs a `call` bypass or `click`:** world objects that react through `OnMouseEnter/Down/Up` (NPC and skill-indicator hover, combat tiles and combatants, world-map landmarks). The virtual mouse doesn't trigger `OnMouse*` (see [Known gotchas](#known-gotchas)), so call the method the callback calls. Bypassed input is untested ground. Mention every bypass a test uses when reporting its results.

**Keys the game reads by state, for reference:** discrete presses are `CustomInputAction`s in `PlayerInputList` (`Assets/src/Movement/PlayerInput.cs`), one set per `CurrentActivity`. Screens that can't own an activity use an **input overlay** (`PlayerStateManager.addInputOverlay`), which replaces the activity's set while it's up: `Inspecting`, `ListeningForKeyBind`, `ConfirmingKeyBindOverwrite`, `CombatResults`, `GameOver`, `LoadingScreenContinue` and `OpeningMonologue`. A press only reaches a set that is enabled at that moment, so `waitFor` the screen before pressing its keys.

**Global state for instrumentation (and the remaining bypasses).**
- Static classes: `State`, `PlayerStateManager`, `PartyManager`, `Flags`, `QuestList`, `ChoiceManager` and `EscapeStack`.
- Static fields: `CombatStateManager.inCombat`, `turnNumber` and `whoseTurn`; `AreaManager.locationName`.
- About 55 classes expose `static getInstance()`.

## `-nographics` risks

- **`WaitForEndOfFrame` may never resume without rendering.** It is used in `ScrollableUIElement` (every list, including dialogue choices), `SelectorManager`, `HoverIconDescriptionPanel`, `DescriptionPanelBuilder`, `PartyMemberSpritePanel`, `SliderScrollRectConnector` and `IExtraSpawnBehaviour`. If a command times out right after one of these runs, re-run with `-batchmode` alone. That mode still has a graphics device but no window. Record the result under [Known gotchas](#known-gotchas).
- **GPU readback:** `AlphaDeterminedRaycastTarget` (`ReadPixels` in `Awake`) and `FlattenedFade` (`RenderTexture`, `Camera.Render`, `Graphics.CopyTexture`; used by `Combatant`).
- **Screen and camera maths:** `CursorController`, `MouseHoverBase`, `AbilityMenuFollower`, `TutorialSequenceStepTargetUIObject` and `HoverIconDescriptionPanel` (static `Screen` sizes).
- **Frame rate:** vSync does nothing headless, so frames run uncapped. At `Time.timeScale = 10`, fades that use scaled time take about a tenth as long, but anything based on unscaled or real time (`WaitForSecondsRealtime`, `Time.unscaledDeltaTime`, `realtimeSinceStartup`) still takes real seconds. Budget for the full 10–17 s fades until a run confirms which ones speed up.

## Protecting player data

Saves and config live under `Application.persistentDataPath`. `PrefabNames.rootGameFolder` is `public readonly static`, so it can't be redirected at runtime. These write to it:
- `SaveHandler` save, quickSave and autosave
- the autosave on zone transitions (`TransitionManager`)
- the autosave in `DialogueManager`
- `EnemyMovement.prepCombat`
- **`Config.writeConfig` on `Application.quitting`**, so every test run rewrites the real config. Key bindings live there too, and a test that rebinds a key through simulated input would change the player's bindings for good.

Before the first test run, ask the user which protection they want, then record their choice here:
- **(a)** Make `rootGameFolder` resolve to a `HeadlessData` folder when `Application.isBatchMode`. This changes gameplay code, so it needs the user's approval.
- **(b)** Back up `Saves/` and `Config/` before the run, and restore them afterwards from outside the game.

Decision (user, 2026-10-24): **(a)**. `PrefabNames.rootGameFolder` is now `<exe dir>/HeadlessData/` when `Application.isBatchMode`, so saves (`Saves/`) and config (`Config/Config.json`) of every headless run go there. `run_test.py` deletes that folder before each run, so tests start with no saves and default key bindings. The path must contain no dots: `SaveHandler.createSavedGameList` and `Config.readConfig` find the extension with `Split(".")`.

## Running tests

1. Compile-check: `python "C:/Users/mattr/.claude/scripts/unity_compile_check.py"`.
2. Make sure the build is current. Claude can't build while the user's editor holds the project lock. Read `D:\Fold Builds\Windows\Isometric Alpha_Data\StreamingAssets\Build-Timestamp.txt`; if it's older than the last code change, ask the user to run **Tools > Build > Rebuild And Run Headless**. Command and query files are read from disk, so changing only those needs no rebuild.
3. **Run it:** `python "RegressionTests/run_test.py" <Name>`. This runs parts 1–3 in one go and blocks until the game quits, so set a Bash `timeout` above the run (up to 600000 ms; `--timeout` is the script's own kill cap, default 900 s). The test log is `D:/Fold Builds/Windows/HeadlessLogs/<Name>.log`, and the player log is `<Name>-player.log` beside it. To re-judge an existing log without re-running: `python "RegressionTests/evaluate_queries.py" "RegressionTests/<Name>/<Name>.queries" "D:/Fold Builds/Windows/HeadlessLogs/<Name>.log"`.
4. From the editor, use **Tools > Build > Regression Tests > Run <Name>** (or **Rebuild And Run <Name>**). The report goes to the Console.
5. Report the `RESULT:` line, every failed test with the log lines around it, and every bypass or `click` fallback the test used. Read the player log only for crashes.

## Existing tests

| Test | Purpose | Instrumentation it relies on | Added |
|---|---|---|---|
| `NewGameToFirstSave` | Start menu → character creation (one stat point, Start) → quote and 7 monologue pages → loading screen → `IntroDialogue` (18 choices: `0,0,0,1,0,3,0,…`) → walk south out of 2SlaveShack into NECamp → quicksave → Settings > Quit Game. 15 named tests. Uses `click` (`onClick` fallback, no EventSystem) for the three start menu and character creation buttons; everything else is simulated input. **Not yet run against a build.** | Scene, CharacterCreation, NewGame, Monologue, Input.Overlay, LoadingScreen, Dialogue, Activity, Area, Transition, Save, Screen, DecisionPanel, Game.QuitRequested, App.Quitting | 2026-10-24 |

## Instrumented events

List every event the game logs, so later tests reuse them instead of adding duplicates.

| Event | Keys | Logged in |
|---|---|---|
| `Scene.Loaded` / `Scene.Unloaded` | `scene`, `mode` | `HeadlessGameEvents` (`SceneManager` callbacks) |
| `App.Quitting` | | `HeadlessGameEvents` (`Application.quitting`) |
| `Transition.Finished` | `area` (the new location) | `HeadlessGameEvents` (`TransitionManager.AfterTransition`; fires before the fade back in) |
| `CharacterCreation.Opened` | | `CharacterCreationPopUpWindow.Awake` |
| `CharacterCreation.StatIncreased` | `stat`, `pointsLeft` | `CharacterCreationPopUpWindow.incrementStat` |
| `CharacterCreation.Accepted` | `name` (empty = default "Brandon"), `pointsLeft` | `newGameSetCharacterNameAndStats`, after its guard |
| `NewGame.Started` | `player` | `applyCharacterChoicesAndStartMonologue`, before `LoadSaveFile.execute` |
| `Monologue.QuoteShown` / `Monologue.QuoteFinished` | | `OpeningMonologueManager.FadeQuote` (after the first fade-in; when the scroll replaces the quote) |
| `Monologue.PageShown` / `PageWaiting` / `PageAdvanced` | `page` (1–7) | `OpeningMonologueManager.FeedPages` (before the page is fed; before `WaitForSpacePress`; after it) |
| `Monologue.Finished` | | `FeedPages`, before the loading screen scene change |
| `Input.OverlayAdded` / `Input.OverlayRemoved` | `overlay` | `PlayerStateManager.addInputOverlay` / `removeInputOverlay` |
| `Activity.Changed` | `from`, `to` (+ `combat=True` in a fight) | `PlayerStateManager.setOOCActivity` / `setCombatActivity`, after the assignment and before the events |
| `LoadingScreen.Opened` / `Continued` / `Closed` | | `LoadingBarProgressTracker.Awake` / `continueFromKey` / `OnDestroy` |
| `Dialogue.Started` | `story`, `midDialogue` | `DialogueManager.startDialogue` |
| `Dialogue.AwaitingInput` | `story`, `count` (0 = continue-only line) | `DialogueManager.displayChoices`, once per input point (see Known gotchas) |
| `Dialogue.ChoiceMade` | `story`, `index`, `text` | `DialogueManager.makeChoice` |
| `Dialogue.ChoiceIgnored` (WARNING) | `story`, `index`, `reason=unimplemented\|repeat` | `DialogueManager.makeChoice` |
| `Dialogue.Ended` | `story` | `DialogueManager.endDialogue` |
| `Area.Changed` | `from`, `to` | `AreaManager.changeArea` |
| `Save.Written` | `name`, `type=manual\|quicksave\|autosave`, `file` | `SaveHandler.createSave`, after the write |
| `Notification.WindowOpened` / `WindowClosed` | `count` | `NotificationPopUpWindow.Awake` / `OnDestroy` |
| `Screen.Opened` | `screen` (`ScreenType`) | `OverallUIManager.changeScreen` |
| `DecisionPanel.Opened` | `decision` (`BinaryDescisionType` or decision class name) | `BinaryPanelPopUpButton.spawnPopUp` (both overloads) |
| `Game.QuitRequested` | `source=QuitToDesktop` | `QuitToDesktop.execute`, before `Application.Quit()` |

## Approved per-frame exceptions

HeadlessLogger calls the user has approved inside per-frame methods. Anything not listed here needs a prompt first.

| Event | Method | Why no one-shot place works | Approved |
|---|---|---|---|
| *(none)* | | | |

## Known gotchas

- **2026-10-24, input spike** (temporary build run with `-batchmode -nographics`, with focus settings changed, and with `-batchmode` alone; all three behaved the same):
  - A headless player starts with **no input devices**. `VirtualInputDriver.ensureDevices()` must run before any input command.
  - No focus or background settings are needed: `Application.isFocused` is true and queued events are processed under the default `backgroundBehavior`.
  - A queued key press fires a `CustomInputAction`'s `started` (and `canceled` on release), and the key reads as held inside that callback. `wasPressedThisFrame` is true in the next `Update`.
  - `wantsInitialStateCheck` fires `started` for a key already held when the action is enabled. Disabling an action while its key is held fires `canceled`.
  - `InputSystem.onAnyButtonPress` reports both virtual devices, so the key binding screen's capture works.
  - With `InputSystemUIInputModule`, a virtual-mouse move and click reach a UI `Button`: pointer enter, down and `onClick` all fire.
  - `OnMouseEnter/Down/Up` **do not fire** from the virtual mouse with Active Input Handling on "Both". `IPointer*` handlers on the same collider do fire once the camera has a `Physics2DRaycaster`, but no scene has one.
  - `TMP_InputField` ignores virtual key presses and `InputSystem.QueueTextEvent`; set `.text` directly.
  - `Screen.width/height` under `-nographics` reported 2858×1535, not a real display size.
- **Mouse button order:** the game and `VirtualInputDriver` number buttons in legacy order (0 left, 1 right, 2 middle, 3 back, 4 forward). The Input System's own `MouseButton` enum puts forward before back; the driver converts.
- **2026-10-24, from code reading (not yet confirmed by a run):**
  - **Dialogue de-duplication:** `continueStory` calls itself for command lines, and every level that reaches its end calls `displayChoices`, so one key press displays the same choices several times. Command lines that wait (`fadeToBlack`, `fadeBackIn`, `slowFadeBackIn`, `wait`, scene changes) `return` early, yet the outer levels still display a `count=0` while the story is about to move on by itself. `DialogueManager` now counts nesting depth: a level that could continue but ended without displaying marks the pass as paused. `Dialogue.AwaitingInput` is logged only at depth 1, in an unpaused pass, with the window up. Keep this if `continueStory` changes.
  - **Number keys skip the fade check:** `makeChoice` with no choices calls `continueStory`, so a number key pressed during a fade advances the story twice. That's why `playDialogue` uses `interactKey` for `count=0`.
  - **10× transition race:** `TransitionManager.waitForBlackScreenThenTransition` waits 10 frames, then waits while `isMidScreenFade()`. At `timeScale` 10 the fade back in can already have started, so the player is back in Walking (`Activity.Changed from=InFade to=Walking`) before `Area.Changed` is logged. Wait for it with `since=<the step that started walking>`.
  - **Transition autosave:** walking through a transition writes `Save.Written type=autosave` (`Autosave 1`). Check a manual save by `type=quicksave` or `type=manual`.

## Keeping this skill current

This file is a living record. At the end of every task that uses it, edit it before reporting back:
- Add new tests to [Existing tests](#existing-tests) and new events to [Instrumented events](#instrumented-events).
- Add new commands, query types or bypass actions to their tables.
- Add anything you confirmed by running a test to [Known gotchas](#known-gotchas). Examples: whether `WaitForEndOfFrame` resumes under `-nographics`, real wait times, flaky steps.
- Record the player-data decision once the user makes it.
- Record every per-frame exception the user approves, with the date.
- Correct or delete facts that turned out to be wrong, and update the "Verified" date on any section you re-checked.
- If you built or changed the shared infrastructure, update its section to describe what actually exists, not the plan.

Keep entries short and factual. Don't add guesses.
