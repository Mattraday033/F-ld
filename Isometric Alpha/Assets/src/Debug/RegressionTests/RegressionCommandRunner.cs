using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Runs a regression test's .commands file, one command at a time, headless only (started by
/// RegressionCommandBootstrap). Commands only act; they never judge. The runner's own log lines
/// (Script.*, Command.*) mark where each step starts and ends and are never evidence that a step worked:
/// the game's own instrumentation logs what happened and the .queries file judges it afterwards.
///
/// The per-frame logging rule covers this class too: every HeadlessLogger call is in a small one-shot helper,
/// called once per command (or once per press), never from inside a wait loop.
/// </summary>
public class RegressionCommandRunner : MonoBehaviour
{
    public const float testTimeScale = 10f;

    private const float defaultClickTimeout = 10f;
    private const float defaultWaitForTimeout = 30f;
    private const float defaultDialogueTimeout = 600f;
    private const float defaultDialogueStepTimeout = 30f;
    private const int maximumForwardedWarnings = 200;
    private const int maximumForwardedErrors = 500;
    private const string headlessLoggerPrefix = "[HeadlessLogger]";

    private const string dialogueAwaitingInputEvent = "Dialogue.AwaitingInput";
    private const string dialogueEndedEvent = "Dialogue.Ended";
    private const string dialogueChoiceIgnoredEvent = "Dialogue.ChoiceIgnored";

    #region Parsed commands

    private class Command
    {
        public int lineNumber;
        public string label;
        public string verb;
        public string rawArguments;
        public List<string> positional = new List<string>();
        public Dictionary<string, string> options = new Dictionary<string, string>();

        public int startLogIndex = -1;

        public string getOption(string key, string fallback = null)
        {
            return options.TryGetValue(key, out string value) ? value : fallback;
        }

        public float getFloatOption(string key, float fallback)
        {
            string value = getOption(key);

            if(value == null)
            {
                return fallback;
            }

            return float.Parse(value, CultureInfo.InvariantCulture);
        }
    }

    private class CommandResult
    {
        public bool failed;
        public string reason;

        public void fail(string failReason)
        {
            failed = true;
            reason = failReason;
        }
    }

    private class ParseException : Exception
    {
        public ParseException(string message) : base(message)
        {
        }
    }

    #endregion

    #region Log lines seen

    private class LoggedEvent
    {
        public int index;
        public string level;
        public string eventName;
        public Dictionary<string, string> keys = new Dictionary<string, string>();

        public bool matches(string name, Dictionary<string, string> requiredKeys)
        {
            if(eventName != name)
            {
                return false;
            }

            foreach(KeyValuePair<string, string> required in requiredKeys)
            {
                if(!keys.TryGetValue(required.Key, out string value) || value != required.Value)
                {
                    return false;
                }
            }

            return true;
        }
    }

    private readonly List<LoggedEvent> loggedEvents = new List<LoggedEvent>();
    private readonly object loggedEventsLock = new object();

    #endregion

    //Bypasses for input simulated keys and the mouse can't reach. Listed by hand, not found by reflection, so a
    //stripped build can't lose entries. Empty for now: every screen the tests use so far takes simulated input.
    private readonly Dictionary<string, Func<string[], IEnumerator>> bypasses = new Dictionary<string, Func<string[], IEnumerator>>();

    private static readonly HashSet<string> waitForReservedKeys = new HashSet<string>() { "timeout", "since" };

    private List<Command> commands = new List<Command>();
    private readonly Dictionary<string, Command> commandsByLabel = new Dictionary<string, Command>();
    private Command currentCommand = null;
    private bool runFinished = false;
    private bool forwardingUnityLog = false;
    private int forwardedWarnings = 0;
    private int forwardedErrors = 0;

    public void begin(string commandsPath)
    {
        HeadlessLogger.LineWritten += onLineWritten;
        Application.logMessageReceived += onUnityLogMessage;
        Application.quitting += onQuitting;

        try
        {
            commands = parseFile(commandsPath);
        }
        catch(ParseException e)
        {
            logParseError(e.Message);
            finishRun("parseError", 1);
            return;
        }
        catch(Exception e) when(e is IOException || e is UnauthorizedAccessException)
        {
            logParseError("could not read " + commandsPath + ": " + e.Message);
            finishRun("parseError", 1);
            return;
        }

        StartCoroutine(runAllCommands());
    }

    private void OnDestroy()
    {
        HeadlessLogger.LineWritten -= onLineWritten;
        Application.logMessageReceived -= onUnityLogMessage;
        Application.quitting -= onQuitting;
    }

    #region Running

    private IEnumerator runAllCommands()
    {
        Time.timeScale = testTimeScale;
        VirtualInputDriver.ensureDevices();

        foreach(Command command in commands)
        {
            currentCommand = command;
            logCommandStarted(command);

            CommandResult result = new CommandResult();
            Stack<IEnumerator> routines = new Stack<IEnumerator>();

            try
            {
                routines.Push(createRoutine(command, result));
            }
            catch(Exception e)
            {
                result.fail("exception " + e.GetType().Name + ": " + e.Message);
            }

            //Each command is stepped by hand inside try/catch (a try block can't hold a yield), flattening any
            //nested IEnumerator it yields, so an exception anywhere in a command becomes Command.Failed.
            while(routines.Count > 0 && !result.failed)
            {
                Time.timeScale = testTimeScale;

                IEnumerator top = routines.Peek();
                bool moved;

                try
                {
                    moved = top.MoveNext();
                }
                catch(Exception e)
                {
                    result.fail("exception " + e.GetType().Name + ": " + e.Message);
                    break;
                }

                if(result.failed)
                {
                    break;
                }

                if(!moved)
                {
                    routines.Pop();
                    continue;
                }

                if(top.Current is IEnumerator nested)
                {
                    routines.Push(nested);
                    continue;
                }

                yield return top.Current;
            }

            if(result.failed)
            {
                logCommandFailed(command, result.reason);
                finishRun("commandFailed", 1);
                yield break;
            }

            logCommandFinished(command, null);
        }

        currentCommand = null;
        finishRun("completed", 0);
    }

    private IEnumerator createRoutine(Command command, CommandResult result)
    {
        switch(command.verb)
        {
            case "click":
                return runClick(command, result);
            case "mouseClick":
                return runMouseClick(command, result);
            case "mouseMove":
                return runMouseMove(command, result);
            case "mouseDown":
                return runMouseButton(command, true);
            case "mouseUp":
                return runMouseButton(command, false);
            case "scroll":
                return runScroll(command);
            case "press":
                return runPress(command);
            case "hold":
                return runHold(command, true);
            case "release":
                return runHold(command, false);
            case "releaseAll":
                return runReleaseAll();
            case "typeText":
                return runTypeText(command, result);
            case "wait":
                return runWait(command);
            case "waitFor":
                return runWaitFor(command, result);
            case "playDialogue":
                return runPlayDialogue(command, result);
            case "call":
                return bypasses[command.positional[0]](command.positional.GetRange(1, command.positional.Count - 1).ToArray());
            default:
                throw new InvalidOperationException("no routine for " + command.verb);
        }
    }

    private void finishRun(string reason, int exitCode)
    {
        if(runFinished)
        {
            return;
        }

        runFinished = true;

        logScriptFinished(reason);
        VirtualInputDriver.releaseAll();

        //The exit code only says whether every command ran; the queries decide pass or fail.
        Application.Quit(exitCode);
    }

    //The game can quit itself mid-command (Settings > Quit Game calls Application.Quit()), so the run is closed
    //off here instead.
    private void onQuitting()
    {
        if(runFinished)
        {
            return;
        }

        runFinished = true;

        if(currentCommand != null)
        {
            logCommandFinished(currentCommand, "gameQuit");
        }

        logScriptFinished("gameQuit");
    }

    #endregion

    #region Commands

    private IEnumerator runClick(Command command, CommandResult result)
    {
        string targetName = command.positional[0];
        float deadline = Time.realtimeSinceStartup + command.getFloatOption("timeout", defaultClickTimeout);
        Button button = null;

        while(button == null)
        {
            button = findInteractableButton(targetName);

            if(button != null)
            {
                break;
            }

            if(Time.realtimeSinceStartup > deadline)
            {
                result.fail("timed out waiting for an active, interactable Button named \"" + targetName + "\"");
                yield break;
            }

            yield return null;
        }

        //DialogueChoice and others cache references in Start(), so a button pressed on the frame it spawns can throw
        yield return null;

        if(button == null || !button.isActiveAndEnabled || !button.interactable)
        {
            result.fail("Button \"" + targetName + "\" stopped being active and interactable before it was clicked");
            yield break;
        }

        if(EventSystem.current != null)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                button = PointerEventData.InputButton.Left
            };

            ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerDownHandler);
            ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerUpHandler);
            ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
        }
        else
        {
            logCommandNote(command, "no EventSystem, invoked onClick");
            button.onClick.Invoke();
        }

        yield return null;
    }

    private IEnumerator runMouseClick(Command command, CommandResult result)
    {
        string targetName = command.positional[0];
        int mouseButton = parseMouseButton(command.getOption("button", "left"));
        float deadline = Time.realtimeSinceStartup + command.getFloatOption("timeout", defaultClickTimeout);
        GameObject target = null;

        while(target == null)
        {
            target = findActiveTarget(targetName);

            if(target != null)
            {
                break;
            }

            if(Time.realtimeSinceStartup > deadline)
            {
                result.fail("timed out waiting for an active, interactable object named \"" + targetName + "\"");
                yield break;
            }

            yield return null;
        }

        yield return null;

        if(target == null || !VirtualInputDriver.tryGetScreenPoint(target, out Vector2 point))
        {
            result.fail("no screen point for \"" + targetName + "\"");
            yield break;
        }

        VirtualInputDriver.moveMouse(point);
        yield return null;

        VirtualInputDriver.setMouseButton(mouseButton, true);
        yield return null;

        VirtualInputDriver.setMouseButton(mouseButton, false);
        yield return null;
    }

    private IEnumerator runMouseMove(Command command, CommandResult result)
    {
        string targetName = command.positional[0];
        float deadline = Time.realtimeSinceStartup + command.getFloatOption("timeout", defaultClickTimeout);
        GameObject target = null;

        while(target == null)
        {
            target = findActiveTarget(targetName);

            if(target != null)
            {
                break;
            }

            if(Time.realtimeSinceStartup > deadline)
            {
                result.fail("timed out waiting for an active object named \"" + targetName + "\"");
                yield break;
            }

            yield return null;
        }

        if(!VirtualInputDriver.tryGetScreenPoint(target, out Vector2 point))
        {
            result.fail("no screen point for \"" + targetName + "\"");
            yield break;
        }

        VirtualInputDriver.moveMouse(point);
        yield return null;
    }

    private IEnumerator runMouseButton(Command command, bool down)
    {
        string buttonName = command.positional.Count > 0 ? command.positional[0] : command.getOption("button", "left");

        VirtualInputDriver.setMouseButton(parseMouseButton(buttonName), down);
        yield return null;
    }

    private IEnumerator runScroll(Command command)
    {
        VirtualInputDriver.scroll(float.Parse(command.positional[0], CultureInfo.InvariantCulture));
        yield return null;

        VirtualInputDriver.scroll(0f);
        yield return null;
    }

    private IEnumerator runPress(Command command)
    {
        int frames = (int) command.getFloatOption("frames", VirtualInputDriver.minimumTapFrames);

        yield return VirtualInputDriver.tap(resolveKey(command.positional[0]), frames);
    }

    private IEnumerator runHold(Command command, bool down)
    {
        KeyCode keyCode = resolveKey(command.positional[0]);

        if(down)
        {
            VirtualInputDriver.hold(keyCode);
        }
        else
        {
            VirtualInputDriver.release(keyCode);
        }

        yield return null;
    }

    private IEnumerator runReleaseAll()
    {
        VirtualInputDriver.releaseAll();
        yield return null;
    }

    private IEnumerator runTypeText(Command command, CommandResult result)
    {
        string fieldName = command.positional[0];
        string text = command.positional[1];
        float deadline = Time.realtimeSinceStartup + command.getFloatOption("timeout", defaultClickTimeout);
        TMP_InputField field = null;

        while(field == null)
        {
            field = findInputField(fieldName);

            if(field != null)
            {
                break;
            }

            if(Time.realtimeSinceStartup > deadline)
            {
                result.fail("timed out waiting for an active TMP_InputField named \"" + fieldName + "\"");
                yield break;
            }

            yield return null;
        }

        //TMP_InputField reads typed characters from IMGUI events, which virtual keys can't produce
        field.text = text;
        yield return null;
    }

    private IEnumerator runWait(Command command)
    {
        float end = Time.realtimeSinceStartup + float.Parse(command.positional[0], CultureInfo.InvariantCulture);

        while(Time.realtimeSinceStartup < end)
        {
            yield return null;
        }
    }

    //Pauses until the log gains a matching event. Only paces the commands; the queries still decide success.
    //Matches anything logged since the previous step started (an event set off by the step before isn't missed),
    //or since the step named by since=.
    private IEnumerator runWaitFor(Command command, CommandResult result)
    {
        string eventName = command.positional[0];
        Dictionary<string, string> requiredKeys = getMatchKeys(command);
        int searchFrom = getWindowStart(command);
        float timeout = command.getFloatOption("timeout", defaultWaitForTimeout);
        float deadline = Time.realtimeSinceStartup + timeout;

        while(true)
        {
            int found = findEvent(searchFrom, eventName, requiredKeys);

            if(found >= 0)
            {
                yield break;
            }

            if(Time.realtimeSinceStartup > deadline)
            {
                result.fail("timed out after " + timeout + "s waiting for " + eventName + describeKeys(requiredKeys));
                yield break;
            }

            yield return null;
        }
    }

    //Plays a conversation through simulated keys. Each Dialogue.AwaitingInput is answered once: count=0 (only a
    //"Continue..." line) presses the interact key; otherwise the next index from choices= is pressed as its
    //number key (Alpha1 for index 0). Ends at Dialogue.Ended. A number key is never sent for count=0, since
    //makeChoice with no choices continues the story without checking for a fade.
    private IEnumerator runPlayDialogue(Command command, CommandResult result)
    {
        List<int> choices = parseChoiceList(command.getOption("choices", ""));
        float overallDeadline = Time.realtimeSinceStartup + command.getFloatOption("timeout", defaultDialogueTimeout);
        float stepTimeout = command.getFloatOption("stepTimeout", defaultDialogueStepTimeout);
        float stepDeadline = Time.realtimeSinceStartup + stepTimeout;
        int searchFrom = getWindowStart(command);
        int nextChoice = 0;

        while(true)
        {
            LoggedEvent dialogueEvent = findNextDialogueEvent(searchFrom);

            if(dialogueEvent == null)
            {
                if(Time.realtimeSinceStartup > overallDeadline)
                {
                    result.fail("conversation did not end within its timeout; " + nextChoice + " of " + choices.Count + " choices used");
                    yield break;
                }

                if(Time.realtimeSinceStartup > stepDeadline)
                {
                    result.fail("no Dialogue.AwaitingInput or Dialogue.Ended within " + stepTimeout + "s; " + nextChoice + " of " + choices.Count + " choices used");
                    yield break;
                }

                yield return null;
                continue;
            }

            searchFrom = dialogueEvent.index + 1;

            if(dialogueEvent.eventName == dialogueEndedEvent)
            {
                if(nextChoice < choices.Count)
                {
                    result.fail("conversation ended with " + (choices.Count - nextChoice) + " choices unused");
                }

                yield break;
            }

            if(dialogueEvent.eventName == dialogueChoiceIgnoredEvent)
            {
                result.fail("the game ignored choice index " + dialogueEvent.keys.GetValueOrDefault("index", "?") + " (" + dialogueEvent.keys.GetValueOrDefault("reason", "?") + ")");
                yield break;
            }

            int choiceCount = int.Parse(dialogueEvent.keys.GetValueOrDefault("count", "0"), CultureInfo.InvariantCulture);
            KeyCode keyToPress;
            string pressDescription;

            if(choiceCount == 0)
            {
                keyToPress = KeyBindingList.interactKey.getCurrentKeyCode();
                pressDescription = "continue";
            }
            else
            {
                if(nextChoice >= choices.Count)
                {
                    result.fail("ran out of choices: a " + choiceCount + "-choice prompt came after all " + choices.Count + " were used");
                    yield break;
                }

                int choiceIndex = choices[nextChoice];

                if(choiceIndex >= choiceCount)
                {
                    result.fail("choice " + (nextChoice + 1) + " is index " + choiceIndex + " but only " + choiceCount + " choices are shown");
                    yield break;
                }

                nextChoice++;
                keyToPress = KeyCode.Alpha1 + choiceIndex;
                pressDescription = "choice " + nextChoice + " index=" + choiceIndex;
            }

            //the choice rows were just built; give them a frame before pressing
            yield return null;

            logDialoguePress(command, keyToPress, pressDescription, choiceCount);

            yield return VirtualInputDriver.tap(keyToPress);

            stepDeadline = Time.realtimeSinceStartup + stepTimeout;
        }
    }

    #endregion

    #region Finding things

    private static Button findInteractableButton(string objectName)
    {
        foreach(Button button in FindObjectsByType<Button>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            if(button.gameObject.name == objectName && button.isActiveAndEnabled && button.interactable)
            {
                return button;
            }
        }

        return null;
    }

    //An active object with the name; when it's a Selectable, it must also be interactable.
    private static GameObject findActiveTarget(string objectName)
    {
        foreach(Transform candidate in FindObjectsByType<Transform>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            if(candidate.gameObject.name != objectName || !candidate.gameObject.activeInHierarchy)
            {
                continue;
            }

            Selectable selectable = candidate.GetComponent<Selectable>();

            if(selectable != null && (!selectable.isActiveAndEnabled || !selectable.IsInteractable()))
            {
                continue;
            }

            return candidate.gameObject;
        }

        return null;
    }

    private static TMP_InputField findInputField(string objectName)
    {
        foreach(TMP_InputField field in FindObjectsByType<TMP_InputField>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            if(field.gameObject.name == objectName && field.isActiveAndEnabled)
            {
                return field;
            }
        }

        return null;
    }

    private int getWindowStart(Command command)
    {
        string sinceLabel = command.getOption("since");

        if(sinceLabel != null)
        {
            return commandsByLabel[sinceLabel].startLogIndex;
        }

        int position = commands.IndexOf(command);

        if(position <= 0)
        {
            return 0;
        }

        return commands[position - 1].startLogIndex;
    }

    private int findEvent(int fromIndex, string eventName, Dictionary<string, string> requiredKeys)
    {
        lock(loggedEventsLock)
        {
            for(int index = Mathf.Max(0, fromIndex); index < loggedEvents.Count; index++)
            {
                if(loggedEvents[index].matches(eventName, requiredKeys))
                {
                    return index;
                }
            }
        }

        return -1;
    }

    private LoggedEvent findNextDialogueEvent(int fromIndex)
    {
        lock(loggedEventsLock)
        {
            for(int index = Mathf.Max(0, fromIndex); index < loggedEvents.Count; index++)
            {
                string name = loggedEvents[index].eventName;

                if(name == dialogueAwaitingInputEvent || name == dialogueEndedEvent || name == dialogueChoiceIgnoredEvent)
                {
                    return loggedEvents[index];
                }
            }
        }

        return null;
    }

    private static Dictionary<string, string> getMatchKeys(Command command)
    {
        Dictionary<string, string> keys = new Dictionary<string, string>();

        foreach(KeyValuePair<string, string> option in command.options)
        {
            if(!waitForReservedKeys.Contains(option.Key))
            {
                keys[option.Key] = option.Value;
            }
        }

        return keys;
    }

    private static KeyCode resolveKey(string keyName)
    {
        if(VirtualInputDriver.tryGetKeyBind(keyName, out KeyBind keyBind))
        {
            return keyBind.getCurrentKeyCode();
        }

        return (KeyCode) Enum.Parse(typeof(KeyCode), keyName, true);
    }

    private static bool isKnownKey(string keyName)
    {
        return VirtualInputDriver.tryGetKeyBind(keyName, out _) || Enum.TryParse(keyName, true, out KeyCode _);
    }

    private static int parseMouseButton(string buttonName)
    {
        switch(buttonName.ToLowerInvariant())
        {
            case "right":
                return 1;
            case "middle":
                return 2;
            default:
                return 0;
        }
    }

    private static List<int> parseChoiceList(string choiceList)
    {
        List<int> choices = new List<int>();

        foreach(string part in choiceList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            choices.Add(int.Parse(part.Trim(), CultureInfo.InvariantCulture));
        }

        return choices;
    }

    private static string describeKeys(Dictionary<string, string> keys)
    {
        StringBuilder builder = new StringBuilder();

        foreach(KeyValuePair<string, string> pair in keys)
        {
            builder.Append(' ').Append(pair.Key).Append('=').Append(pair.Value);
        }

        return builder.ToString();
    }

    #endregion

    #region Reading log lines

    private void onLineWritten(string line)
    {
        LoggedEvent loggedEvent = parseLogLine(line);

        if(loggedEvent == null)
        {
            return;
        }

        lock(loggedEventsLock)
        {
            loggedEvent.index = loggedEvents.Count;
            loggedEvents.Add(loggedEvent);
        }
    }

    //"[14:05:11.204] [INFO] Area.Event key=value key2="quoted value""
    private static LoggedEvent parseLogLine(string line)
    {
        int levelStart = line.IndexOf("] [", StringComparison.Ordinal);

        if(levelStart < 0)
        {
            return null;
        }

        int levelEnd = line.IndexOf("] ", levelStart + 3, StringComparison.Ordinal);

        if(levelEnd < 0)
        {
            return null;
        }

        List<string> tokens = tokenize(line.Substring(levelEnd + 2));

        if(tokens.Count == 0)
        {
            return null;
        }

        LoggedEvent loggedEvent = new LoggedEvent
        {
            level = line.Substring(levelStart + 3, levelEnd - levelStart - 3),
            eventName = tokens[0]
        };

        for(int index = 1; index < tokens.Count; index++)
        {
            int equals = tokens[index].IndexOf('=');

            if(equals > 0)
            {
                loggedEvent.keys[tokens[index].Substring(0, equals)] = tokens[index].Substring(equals + 1);
            }
        }

        return loggedEvent;
    }

    //Splits on spaces outside quotes. Quotes are removed and \" and \\ unescaped, so key="a b" becomes key=a b.
    private static List<string> tokenize(string text)
    {
        List<string> tokens = new List<string>();
        StringBuilder current = new StringBuilder();
        bool inQuotes = false;
        bool hasToken = false;

        for(int index = 0; index < text.Length; index++)
        {
            char character = text[index];

            if(inQuotes && character == '\\' && index + 1 < text.Length)
            {
                current.Append(text[index + 1]);
                index++;
            }
            else if(character == '"')
            {
                inQuotes = !inQuotes;
                hasToken = true;
            }
            else if(character == ' ' && !inQuotes)
            {
                if(hasToken)
                {
                    tokens.Add(current.ToString());
                    current.Clear();
                    hasToken = false;
                }
            }
            else
            {
                current.Append(character);
                hasToken = true;
            }
        }

        if(hasToken)
        {
            tokens.Add(current.ToString());
        }

        return tokens;
    }

    #endregion

    #region Parsing the command file

    private List<Command> parseFile(string path)
    {
        List<Command> parsed = new List<Command>();
        string[] lines = File.ReadAllLines(path);

        for(int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
        {
            int lineNumber = lineIndex + 1;
            string line = stripComment(lines[lineIndex]).Trim();

            if(line.Length == 0)
            {
                continue;
            }

            Command command = parseCommandLine(line, lineNumber);

            if(commandsByLabel.ContainsKey(command.label))
            {
                throw new ParseException("line " + lineNumber + ": step label \"" + command.label + "\" is used twice");
            }

            validate(command);

            commandsByLabel[command.label] = command;
            parsed.Add(command);
        }

        if(parsed.Count == 0)
        {
            throw new ParseException("no commands in " + path);
        }

        return parsed;
    }

    private static string stripComment(string line)
    {
        bool inQuotes = false;

        for(int index = 0; index < line.Length; index++)
        {
            if(line[index] == '"')
            {
                inQuotes = !inQuotes;
            }
            else if(line[index] == '#' && !inQuotes)
            {
                return line.Substring(0, index);
            }
        }

        return line;
    }

    private static Command parseCommandLine(string line, int lineNumber)
    {
        int colon = line.IndexOf(':');

        if(colon <= 0)
        {
            throw new ParseException("line " + lineNumber + ": expected \"<step label>: <command>\"");
        }

        string label = line.Substring(0, colon).Trim();

        foreach(char character in label)
        {
            if(!char.IsLetterOrDigit(character) && character != '-' && character != '_')
            {
                throw new ParseException("line " + lineNumber + ": step label \"" + label + "\" may only use letters, digits, - and _");
            }
        }

        string rest = line.Substring(colon + 1).Trim();
        List<string> tokens = tokenize(rest);

        if(tokens.Count == 0)
        {
            throw new ParseException("line " + lineNumber + ": step \"" + label + "\" has no command");
        }

        Command command = new Command
        {
            lineNumber = lineNumber,
            label = label,
            verb = tokens[0],
            rawArguments = rest.Substring(tokens[0].Length).Trim()
        };

        for(int index = 1; index < tokens.Count; index++)
        {
            int equals = tokens[index].IndexOf('=');
            int space = tokens[index].IndexOf(' ');

            //key=value, or key="quoted value"; a quoted positional such as "Exit Game Button" has no = before its first space
            if(equals > 0 && (space < 0 || space > equals))
            {
                command.options[tokens[index].Substring(0, equals)] = tokens[index].Substring(equals + 1);
            }
            else
            {
                command.positional.Add(tokens[index]);
            }
        }

        return command;
    }

    private void validate(Command command)
    {
        string where = "line " + command.lineNumber + " (" + command.label + "): ";

        switch(command.verb)
        {
            case "click":
            case "mouseClick":
            case "mouseMove":
                requirePositional(command, 1, where + command.verb + " needs a GameObject name");
                break;
            case "mouseDown":
            case "mouseUp":
            case "releaseAll":
                break;
            case "scroll":
            case "wait":
                requirePositional(command, 1, where + command.verb + " needs a number");
                requireNumber(command.positional[0], where);
                break;
            case "press":
            case "hold":
            case "release":
                requirePositional(command, 1, where + command.verb + " needs a KeyBind field name or KeyCode");

                if(!isKnownKey(command.positional[0]))
                {
                    throw new ParseException(where + "unknown key \"" + command.positional[0] + "\"");
                }

                break;
            case "typeText":
                requirePositional(command, 2, where + "typeText needs a field name and the text");
                break;
            case "waitFor":
                requirePositional(command, 1, where + "waitFor needs an event name");
                validateSince(command, where);
                break;
            case "playDialogue":
                if(command.getOption("choices") == null)
                {
                    throw new ParseException(where + "playDialogue needs choices=i,j,... (it may be empty: choices=)");
                }

                try
                {
                    parseChoiceList(command.getOption("choices"));
                }
                catch(FormatException)
                {
                    throw new ParseException(where + "choices= must be comma-separated whole numbers");
                }

                foreach(int choice in parseChoiceList(command.getOption("choices")))
                {
                    if(choice < 0 || choice > 7)
                    {
                        throw new ParseException(where + "choice index " + choice + " has no number key (0-7 map to 1-8)");
                    }
                }

                validateSince(command, where);
                break;
            case "call":
                requirePositional(command, 1, where + "call needs a bypass name");

                if(!bypasses.ContainsKey(command.positional[0]))
                {
                    throw new ParseException(where + "unknown bypass \"" + command.positional[0] + "\"");
                }

                break;
            default:
                throw new ParseException(where + "unknown command \"" + command.verb + "\"");
        }

        foreach(string numericOption in new[] { "timeout", "stepTimeout", "frames" })
        {
            if(command.options.ContainsKey(numericOption))
            {
                requireNumber(command.options[numericOption], where);
            }
        }
    }

    private void validateSince(Command command, string where)
    {
        string sinceLabel = command.getOption("since");

        if(sinceLabel != null && !commandsByLabel.ContainsKey(sinceLabel))
        {
            throw new ParseException(where + "since=" + sinceLabel + " must name an earlier step");
        }
    }

    private static void requirePositional(Command command, int count, string message)
    {
        if(command.positional.Count < count)
        {
            throw new ParseException(message);
        }
    }

    private static void requireNumber(string value, string where)
    {
        if(!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
        {
            throw new ParseException(where + "\"" + value + "\" is not a number");
        }
    }

    #endregion

    #region Unity output

    private void onUnityLogMessage(string message, string stackTrace, LogType type)
    {
        if(forwardingUnityLog || type == LogType.Log || message == null || message.StartsWith(headlessLoggerPrefix, StringComparison.Ordinal))
        {
            return;
        }

        forwardingUnityLog = true;

        try
        {
            if(type == LogType.Warning)
            {
                forwardedWarnings++;

                if(forwardedWarnings <= maximumForwardedWarnings)
                {
                    HeadlessLogger.LogWarningEvent("Unity.Warning", "message", message);
                }
            }
            else
            {
                forwardedErrors++;

                if(forwardedErrors <= maximumForwardedErrors)
                {
                    string eventName = type == LogType.Exception ? "Unity.Exception" : "Unity.Error";
                    string firstStackLine = stackTrace == null ? "" : stackTrace.Split('\n')[0].Trim();

                    HeadlessLogger.LogErrorEvent(eventName, "message", message, "at", firstStackLine);
                }
            }
        }
        finally
        {
            forwardingUnityLog = false;
        }
    }

    #endregion

    #region Runner log lines (one-shot helpers only)

    private void logCommandStarted(Command command)
    {
        //the Command.Started line is the next event, so its index marks where this step's window begins
        lock(loggedEventsLock)
        {
            command.startLogIndex = loggedEvents.Count;
        }

        HeadlessLogger.LogEvent("Command.Started", "step", command.label, "command", command.verb, "args", command.rawArguments);
    }

    private static void logCommandFinished(Command command, string note)
    {
        if(note == null)
        {
            HeadlessLogger.LogEvent("Command.Finished", "step", command.label);
        }
        else
        {
            HeadlessLogger.LogEvent("Command.Finished", "step", command.label, "note", note);
        }
    }

    private static void logCommandFailed(Command command, string reason)
    {
        HeadlessLogger.LogErrorEvent("Command.Failed", "step", command.label, "reason", reason);
    }

    private static void logCommandNote(Command command, string note)
    {
        HeadlessLogger.LogEvent("Command.Note", "step", command.label, "note", note);
    }

    private static void logDialoguePress(Command command, KeyCode key, string description, int choiceCount)
    {
        HeadlessLogger.LogEvent("Command.Note", "step", command.label, "pressed", key, "for", description, "count", choiceCount);
    }

    private static void logParseError(string reason)
    {
        HeadlessLogger.LogErrorEvent("Command.ParseError", "reason", reason);
    }

    private static void logScriptFinished(string reason)
    {
        HeadlessLogger.LogEvent("Script.Finished", "reason", reason);
    }

    #endregion
}
