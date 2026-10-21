# AnimationManager → CreatureAnimationManager porting list

Every place where code that used the old `AnimationManager` was commented out and still has to be ported to `CreatureAnimationManager`.

- **Old class:** [AnimationManager.cs](../Assets/src/Art/Animations/AnimationManager.cs). Almost all of its body is commented out and serves as the reference for the old behaviour.
- **New class:** [CreatureAnimationManager.cs](../Assets/src/Art/Animations/CreatureAnimationManager.cs).

Last audited 2026-10-21. Line numbers drift as files are edited, so search for the quoted code if a line doesn't match. When something is ported, move it to [Done](#done) instead of deleting it.

## What CreatureAnimationManager can already do

- **`playAnimation(type)`:** only handles `Run_*`, `Idle_*`, `OOC_Idle_*` and `Attack_Normal_Front/Back`. Every other type falls into `default: return` and does nothing. That includes `Secondary_Idle_*`, `Death_*`, `Wounded_*`, `StandUp`, `Spawn` and `Vertical_Falling`.
- **`currentIdle` setter:** accepts `Idle_*`, `OOC_Idle_*`, `Death_Front/Back`, `Secondary_Idle_Front/Back` and `Secondary_Death`. It silently ignores `Secondary_Idle`, `Death`, `Death_*_Weaponless` and `Vertical_Falling`.
- **Working:** `handleMovementAnimation()`, `setAppearanceSource()`, `characterFacing`, `playAttackAnimation()`, `playAttackIntoFrontIdleAnimation()` and `playAttackIntoSecondaryIdleAnimation()`.
- **`shadowExtras`** (nested class `ShadowExtras`): shadow creation and facing, `disableExtras()`/`enableExtras()`, `showShadowOutline()`/`removeShadowOutline()`, and the static `RemoveAllShadowOutlines` event.
- **Empty TODO stubs:** `playSpawnAnimation`, `playSpecialAttackAnimation`, `playWoundedAnimation`, `playDeathAnimation`, `setToDefaultIdle`, `setToDeadIdle`, `setIdleAnimationOnTraitApplication`/`Removal`, `setHeartBeatRow` and `playAnimationSFX`. `getAnimationLength` always returns 0. Live code calls these stubs and silently gets nothing.
- **Missing:** a by-name lookup or broadcast. The old static events `SetIdleByNPCName`, `PlayAnimationByNPCName`, `CreateEffectByNPCName`, `ShowStapledEffectByNPCName` and `HideAllStapledEffects` have no replacement.
- **Getting the manager:**
  - In combat: `stats.getAnimationManager()` or `Combatant.animationManager`.
  - Out of combat: `MovementTracker.animationManager`.
  - For the player: `PlayerMovement.getInstance().animationManager`.
  - From any object: `GetComponent<CreatureAnimationManager>()`.

## Not ported

### Dialogue: [DialogueManager.cs](../Assets/src/PlayerActions/Dialogue/DialogueManager.cs)
| Lines | Old behaviour | Notes |
|---|---|---|
| 1119-1190 | `playanimation` command: attack front/back, every idle variant, death (+ hide), secondary death, weaponless death idles, StandUp, Wounded_* / OOC_Wounded_* | Now only calls `continueStory()`. The attack and supported idle cases could go straight through `playAnimation`/`currentIdle`. |
| 1249 | `resetidledictentry`: `AnimationManager.resetIdleDictionaryEntry(name)` reloaded the cached idle sprites | `IdleDictionary` has no reset. |
| 1262 | `createstapledeffectbyname`: `CreateEffectByNPCName` | There is no stapled-effect system. |
| 1271 | `hideallstapledeffects`: `HideAllStapledEffects` | |
| 1284 | `showstapledeffectbyname`: `ShowStapledEffectByNPCName` | |
| 1298 | `setidleofnpcsbyname`: `SetIdleByNPCName` | There is no by-name lookup. |
| 1315-1332 | `playanimationthenfade`: play the animation, then fade to black | It always just continues now. |
| 1345-1359 | `playdelayedanimation` / `playanimationwithdelay`: the wounded animation after a delay | Needs `playWoundedAnimation`, which is a stub. |
| 2370-2378 | Execution with `leaveDeadBodies`: left the body in the `Death_Front_Weaponless` idle | The target is always `SetActive(false)`, so the flag does nothing. |
| 2512-2523 | `playAnimationThenFadeToBlack` coroutine (whole method) | Also needs `getAnimationLength`. |
| 2525-2537 | `waitThenPlayAnimation` coroutine (whole method) | `AnimationDelegate<T>` at line 11 has no users now. |

### Player, movement and transitions
| Location | Old behaviour | Notes |
|---|---|---|
| [PlayerObject.cs:103](../Assets/src/Movement/PlayerObject.cs#L103) | `toggleHover` also turned the player's hover polygon collider on and off | Could use `SpriteLayerRendererList.setBodyColliderEnabled`, the way enemies do at EnemyMovement.cs:804-815. |
| [PlayerObject.cs:411-418](../Assets/src/Movement/PlayerObject.cs#L411-L418) | `playDeathAnimation` | The body is empty but it is still called from DialogueManager (jelly misfire / defeat). Needs the `playDeathAnimation` stub filled in. |
| [PlayerInput.cs:304-328](../Assets/src/Movement/PlayerInput.cs#L304-L328) | `drawSpriteSortPoint` debug gizmo | `SpriteLayerRendererList.bodyRenderer` is private. |
| [DropInFromAboveTransitionScript.cs](../Assets/src/Scripts/TransitionScripts/DropInFromAboveTransitionScript.cs), lines 58-60 and 93-104 | Falling pose (`Vertical_Falling`) while dropping in | `Vertical_Falling` isn't supported. The NPC fallers in OOCSpawnDetailsList.cs:419-421 are ignored for the same reason. |
| Same file, lines 80-83 and 106-128 | Stand-up animation after landing | Needs StandUp support and `getAnimationLength`. |
| Same file, lines 85 and 130-145 | Forced `Idle_Front` / `OOC_Idle_Front` after landing | Directly portable. |
| [CunningBlocker.cs:100-112](../Assets/src/PlayerActions/Skills/CunningBlocker.cs#L100-L112) | `playDeathAnimationThenHide` on everything a blocker kills | The whole file is commented out. |

### Combat and traits
| Location | Old behaviour | Notes |
|---|---|---|
| [Trait.cs:461-469](../Assets/src/Combat/Traits/Trait.cs#L461-L469) | Virtual hooks that set the idle when a trait is added or removed | Combatant.onTraitAdded/Removed call the empty stubs on CreatureAnimationManager. |
| [ShieldTrait.cs:18-26](../Assets/src/Combat/Traits/ShieldTrait.cs#L18-L26) | ShieldTrait: `Secondary_Idle` on, `Idle_Front` off | `Secondary_Idle` isn't accepted by `currentIdle` or `playAnimation`. |
| [ShieldTrait.cs:43-51](../Assets/src/Combat/Traits/ShieldTrait.cs#L43-L51) | HiddenShieldTrait: deliberately empty overrides | Intent: a hidden shield doesn't change the idle. |
| [ShieldTrait.cs:68-77](../Assets/src/Combat/Traits/ShieldTrait.cs#L68-L77) | CaveMatronShieldTrait: `Idle_Front` if she's the last master with no minions, otherwise `Secondary_Idle` | |
| [TraitContainer.cs:97-100](../Assets/src/Combat/Traits/TraitContainer.cs#L97-L100) and [112-115](../Assets/src/Combat/Traits/TraitContainer.cs#L112-L115) | Reset the idle after removing traits | These paths call `traits.Remove` directly and never raise `Stats.OnTraitRemoved`. `removeFirstTraitOfType` (82-86) skips it too. |
| [RepositionAbility.cs:30-39](../Assets/src/Combat/Action/Abilities/RepositionAbilities/RepositionAbility.cs#L30-L39) | Waited for the actor's attack animation before moving the target | Easy to port: `getActorStats().getAnimationManager()`, and attacks are tracked. Lines 41-48 (move, applyTrait, BlueShield) are commented out as well. **The guard at lines 24-28 is inverted**: it returns when the combatant *does* exist. |
| [RepositionEnemyAbility.cs:25-27](../Assets/src/Combat/Action/Abilities/RepositionAbilities/RepositionEnemyAbility.cs#L25-L27) | The same wait, as an inline `/* */` parameter | The live `playAnimationOnDamage()` at line 31 lands on the wounded/death stubs. |
| [WinCondition.cs:235-238](../Assets/src/Combat/WinCondition.cs#L235-L238) | End-of-combat cutscene: `PlayAnimationByNPCName("Puppeted " + name, Attack_Normal_Front)` | `Attack_Normal_Front` is supported. A port only needs a way to find the puppeted combatants. `cutSceneSpriteNames` is unused. |
| [EffectAnimationManager.cs:304-316](../Assets/src/Art/Animations/EffectAnimationManager.cs#L304-L316) | `handleDamageNumbersAndWoundedAnim`: the target's wounded animation | Partial: damage numbers were ported (`createDamagePopUp`), the wounded animation wasn't. |
| [SpriteClipGenerator.cs:17](../Assets/src/Editor/SpriteClipGenerator.cs#L17) | `AnimationManager.tempAnimationTypes` filled the set of sheets to generate `.anim` clips for | The set is empty now, so no clips are generated. The doc comments at lines 29 and 55 still refer to it. |

### Inside commented-out AnimationManager.cs (old features with no home yet)
Search by method name, since the line numbers in this file have moved:
- Heartbeat idle: `updateIdleAnimation`, `setSpriteToCurrentIdle`, `spriteSetByHeartBeat`, `setHeartBeatRow`, `addIdleSprites`, `resetIdleDictionaryEntry`
- Idle selection and fallbacks: `setCurrentIdle`, `setToDefaultIdle`, `getFallBackIdleType`, `getFallBackAnimationType`
- Clip-based animations: `playSpawnAnimation`, `playDeathAnimation`, `playDeathAnimationThenHide`, `playStandUpAnimation`, `getAnimationLength`, the `createClipTransition*` helpers
- By-name events and stapled effects: `playAnimationByNPCName`, `createEffectByNPCName`, `showStapledEffect`, `hideAllStapledEffects`
- Collider toggling across combat activities: `disablePolygonCollider`, `enablePolygonCollider`, `handleActivityChangeFrom*`
- `enableExtras()`/`disableExtras()` calls are still in the commented `removeAnimation`, `playIdleAnimation`, `playCurrentIdleAnimation`, `playSpawnAnimation` and `playDeathAnimation`. When porting those, call `shadowExtras.enableExtras()`/`disableExtras()` at the same points.

### Related leftovers (not AnimationManager, same migration)
- `PlayerObject.setSpriteSortingLayer` is fully commented out (it used `playerSpriteRenderer`). DropInFromAboveTransitionScript still calls it at lines 30 and 90.
- `DropInFromAboveTransitionScript.player_spriteHeight` always returns 0.

## Done
| Old code | Replacement |
|---|---|
| `PlayerObject.animationManager` field (line 24), `setAnimations` in Awake (63), `getAnimationManager()` (359-363) | `MovementTracker.animationManager`; the appearance is set in SpawnInfoManager.cs:116,128-129 |
| MovementManager.cs:259-262, `haltAllAnimations` after each step | CreatureAnimationManager subscribes `handleMovementAnimation` to `MovementManager.AfterMoveStarted`/`OnMoveFinished` |
| EnemyMovement.cs:285, `linkedStats` for out-of-combat enemies | The appearance comes from the spawn details (`AnimationManagerSpawnBehaviour`) |
| EffectAnimationManager.cs:328-331, `instantiatePrefab` | `EffectAnimationManager.createEffect` |
| All shadow/extras code (`instantiateShadow`, `setAllShadowDirections`, `flipShadowByName`, `canChangeShadowFacing`, `disableExtras`, `enableExtras`, `showShadowOutline`, `removeShadowOutline`, `getOutlineSize`, `RemoveAllShadowOutlines`) | `CreatureAnimationManager.ShadowExtras` (2026-10-21) |
| DialogueManager.cs:50 (`RemoveAllShadowOutlines`) and 64-69 (`highlightSpeaker`) | `ShadowExtras.RemoveAllShadowOutlines` / `shadowExtras.showShadowOutline()` |
| DialogueManager.cs:1214-1219 (`startfall`) and 1233-1238 (`disableextras`/`hideextras`) | `shadowExtras.disableExtras()` |
| PlayerObject.hidePlayerSprite / showPlayerSprite | `PlayerMovement.hideSprite()/showSprite()` + `shadowExtras.disableExtras()/enableExtras()` |
