using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private static List<KeyCode> barredMovementKeyCodes = new List<KeyCode>(); //key codes that are not able to be used because you have selected
                                                                //multiple keys at once. Pressing W, then while W is pressed also
                                                                //pressing A should stop accepting W as an input and allow A.

    private static KeyCode currentMovementKeyCode = KeyCode.None;

    //The enabled set is owned by PlayerOOCStateManager.updateEnabledInputActions, which runs on every
    //activity change and once at startup, so the movement actions no longer need enabling here.

    void Update()
    {

        if(KeyBindingSettingsManager.listeningForKeyBinding() || 
            PlayerOOCStateManager.currentActivity == OOCActivity.inAnimation)
        {
            return;
        }

        if(InspectNode.inspecting)
        {
            showFormulaToggleCheck();
            return;
        }

        KeyPressManager.updateKeyBools();

        if (PlayerObject.onTopOfTransitionOrTutorial())
        {
            return;
        }

        if (KeyBindingList.settingsScreenOrBackKeyPressed() && PlayerOOCStateManager.currentActivity != OOCActivity.inChestUI)
        {
            if (NotificationManager.getCurrentNotificationPopUpWindowGameObject() != null &&
                 !KeyPressManager.handlingPrimaryKeyPress)
            {
                NotificationManager.OnDeleteAllNotifications.Invoke();
                KeyPressManager.handlingPrimaryKeyPress = true;
            }
        }

        // if ((KeyPressManager.handlingPrimaryKeyPress && PlayerOOCStateManager.currentActivity != OOCActivity.inChestUI &&
        //                                         PlayerOOCStateManager.currentActivity != OOCActivity.inTutorialSequence)
        // || FadeToBlackManager.isMidScreenFade() || !FadeToBlackManager.getInstance().fadeToBlackImage.color.Equals(Color.clear))
        // {
        //     return;
        // }

        // if (Input.GetKey(KeyBindingList.showHideKeyBindingsListKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        // {
        //     KeyPressManager.handlingPrimaryKeyPress = true;
        //     CombatInputManager.OnHideKeyBindingsList.Invoke();
        // }

        // if (!KeyPressManager.handlingPrimaryKeyPress || PlayerOOCStateManager.currentActivity == OOCActivity.inTutorialSequence)
        // {
        //     switch (PlayerOOCStateManager.currentActivity)
        //     {
        //         case OOCActivity.walking:
        //             handleWalkingStateKeyPresses();
        //             break;
        //         case OOCActivity.inDialogue:
        //             handleDialogueStateKeyPresses();
        //             break;
        //         case OOCActivity.inUI:
        //             handleUIStateKeyPresses();
        //             break;
        //         case OOCActivity.inMap:
        //             handleMapStateKeyPresses();
        //             break;
        //         case OOCActivity.cunning:
        //             handleCunningStateKeyPresses();
        //             break;
        //         case OOCActivity.observing:
        //             handleObservingStateKeyPresses();
        //             break;
        //         case OOCActivity.intimidating:
        //             handleIntimidateStateKeyPresses();
        //             break;
        //         case OOCActivity.inChestUI:
        //             handleChestStateKeyPresses();
        //             break;
        //         case OOCActivity.inBookUI:
        //             handleBookStateKeyPresses();
        //             break;
        //         case OOCActivity.inShopUI:
        //             handleShopStateKeyPresses();
        //             break;
        //         case OOCActivity.inDialoguePopUp:
        //             handleDialoguePopUpStateKeyPresses();
        //             break;
        //         case OOCActivity.inLevelUpPopUp:
        //             handleLevelUpPopUpStateKeyPresses();
        //             break;
        //         case OOCActivity.inTutorialPopUp:
        //             handleTutorialPopUpStateKeyPresses();
        //             break;
        //         case OOCActivity.inTutorialSequence:
        //             handleTutorialSequenceStateKeyPresses();
        //             break;
        //         case OOCActivity.inWorldMap:
        //             handleWorldMapStateKeyPresses();
        //             break;
        //         case OOCActivity.inFade:
        //         case OOCActivity.preCombat:
        //         case OOCActivity.Defeat:
        //         case OOCActivity.Loading:
        //         case OOCActivity.inAnimation:
        //             return;
        //         default:
        //             Debug.LogError("Unrecognized OOCActivity: " + PlayerOOCStateManager.currentActivity.ToString());
        //             break;
        //     }
        // }
    }

    private void handleWalkingStateKeyPresses()
    {
        if (handleWASDMovement())
        {
            return;
        }

        if (KeyBindingList.quickLoadKeysPressed() && !KeyPressManager.handlingPrimaryKeyPress)
        {
            SaveHandler.quickLoadTopSave();

            KeyPressManager.handlingPrimaryKeyPress = true;
        }

        if (Input.GetKey(KeyBindingList.quicksaveKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            SaveHandler.quickSave();

            KeyPressManager.handlingPrimaryKeyPress = true;
        }

        toggleTerrainKeyCheck();

        if (Input.GetKey(KeyBindingList.interactKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.handlingPrimaryKeyPress = true;

            if (PlayerMovement.canInteract())
            {
                interact();
            }
        }

        if (Input.GetKey(KeyBindingList.skillKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            SkillButtonManager.useSkill();
            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        if (Input.GetKey(KeyBindingList.cycleSkillAscendingKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            SkillButtonManager.changeSkill(false);
            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        if (Input.GetKey(KeyBindingList.cycleSkillDescendingKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            SkillButtonManager.changeSkill(true);
            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        if (Input.GetKey(KeyBindingList.mapKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            PlayerObject.getMapPopUpButton().spawnPopUp();

            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        if (Input.GetKey(KeyBindingList.worldMapKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            PlayerObject.getWorldMapPopUpButton().spawnPopUp();

            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        if (Input.GetKey(KeyBindingList.transcriptKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            DialogueBookItem dialogueBook = new DialogueBookItem();
            dialogueBook.use(PartyManager.getPlayerStats());

            PlayerOOCStateManager.setCurrentActivity(OOCActivity.inDialoguePopUp);

            KeyPressManager.handlingPrimaryKeyPress = true;
        }

        if (Input.GetKey(KeyBindingList.removePlacedCompanionMovableObjectKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            Collider2D npcCollider = PositionQuery.npcAtPosition(PlayerMovement.getColliderWorldPosition());
            Collider2D moveableObjectCollider = PositionQuery.moveableObjectAtPosition(PlayerMovement.getColliderWorldPosition());

            if (npcCollider != null)
            {
                GameObject npcGameObject = npcCollider.gameObject;

                if (npcGameObject.tag.Equals(LayerAndTagManager.partyMemberTag))
                {
                    string partyMemberName = npcGameObject.GetComponent<PlacedPartyMember>().partyMember.getName();
                    PartyMemberPlacer.removePlacedPartyMember(partyMemberName);
                }
                else
                {
                    return;
                }

            }
            else if (moveableObjectCollider != null)
            {
                GameObject movableObject = moveableObjectCollider.gameObject;
                EnemyMovement enemyMovement = movableObject.GetComponent<EnemyMovement>();
                enemyMovement.putBackToStartingPosition();
                MovementManager.OnMoveFinished.Invoke(PlayerMovement.getPlayerMovementIndex());
            }

            OOCUIManager.updateOOCUI();
            KeyPressManager.handlingPrimaryKeyPress = true;
        }

        if (Input.GetKey(KeyBindingList.revealKey.getCurrentKeyCode()) && !KeyPressManager.handlingSecondaryKeyPress)
        {
            RevealManager.toggleReveal();
            KeyPressManager.handlingSecondaryKeyPress = true;
        }

        if (Input.GetKey(KeyBindingList.lastScreenKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.escapeAll();

            OverallUIManager.changeScreen(OverallUIManager.lastScreenType);

            KeyPressManager.handlingPrimaryKeyPress = true;
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.inUI);
            return;
        }

        handleScreenSelection();
    }

    private void handleScreenSelection()
    {
        if (Input.GetKey(KeyBindingList.characterScreenKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            SideScreenButtonManager.getInstance().setCurrentScreenType(ScreenType.Character);

            KeyPressManager.handlingPrimaryKeyPress = true;

            return;
        }

        if (Input.GetKey(KeyBindingList.inventoryScreenKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            SideScreenButtonManager.getInstance().setCurrentScreenType(ScreenType.Inventory);

            KeyPressManager.handlingPrimaryKeyPress = true;

            return;
        }

        if (Input.GetKey(KeyBindingList.partyScreenKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            SideScreenButtonManager.getInstance().setCurrentScreenType(ScreenType.Party);

            KeyPressManager.handlingPrimaryKeyPress = true;

            return;
        }

        if (Input.GetKey(KeyBindingList.journalScreenKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            SideScreenButtonManager.getInstance().setCurrentScreenType(ScreenType.Journal);

            KeyPressManager.handlingPrimaryKeyPress = true;

            return;
        }

        if (Input.GetKey(KeyBindingList.loadScreenKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            SideScreenButtonManager.getInstance().setCurrentScreenType(ScreenType.SaveAndLoad);

            KeyPressManager.handlingPrimaryKeyPress = true;

            return;
        }

        if (Input.GetKey(KeyBindingList.settingsScreenKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            SideScreenButtonManager.getInstance().setCurrentScreenType(ScreenType.Settings);

            KeyPressManager.handlingPrimaryKeyPress = true;

            return;
        }
    }

    private void handleDialogueStateKeyPresses()
    {
        if(!DialogueManager.getInstance().dialogueUIVisible())
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        if (KeyBindingList.continueStoryKeyIsPressed()
            && DialogueManager.getInstance().storyCanContinue() && 
                !FadeToBlackManager.isMidScreenFade())
        {
            if (!DialogueManager.getInstance().dialogue.random)
            {
                DialogueManager.getInstance().continueStory();
            }

            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        toggleTerrainKeyCheck();

        if (Input.GetKey(KeyCode.Alpha1))
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            DialogueManager.getInstance().makeChoice(0);
            return;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            DialogueManager.getInstance().makeChoice(1);
            return;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            DialogueManager.getInstance().makeChoice(2);
            return;
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            DialogueManager.getInstance().makeChoice(3);
            return;
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            DialogueManager.getInstance().makeChoice(4);
            return;
        }
        else if (Input.GetKey(KeyCode.Alpha6))
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            DialogueManager.getInstance().makeChoice(5);
            return;
        }
        else if (Input.GetKey(KeyCode.Alpha7))
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            DialogueManager.getInstance().makeChoice(6);
            return;
        }
        else if (Input.GetKey(KeyCode.Alpha8))
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            DialogueManager.getInstance().makeChoice(7);
            return;
        }
    }

    private void handleUIStateKeyPresses()
    {
        showFormulaToggleCheck();

        if (KeyBindingList.settingsScreenOrBackKeyPressed() && 
            EscapeStack.getEscapableObjectsCount() > 0 &&
            !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.handleEscapePress();

            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }
        else if ((((Input.GetKey(OverallUIManager.getCurrentScreenExitKey()) || 
                    KeyBindingList.settingsScreenOrBackKeyPressed()) && 
                    !SaveHandler.saveNameFieldIsSelected()) || 
                    Input.GetKey(KeyBindingList.lastScreenKey.getCurrentKeyCode())) && 
                    !KeyPressManager.handlingPrimaryKeyPress)
        {
            if (backOutOfUI())
            {
                return;
            }
        }

        if (EscapeStack.getEscapableObjectsCount() > 0)
        {
            return;
        }

        if(KeyBindingList.screenNavigationButtonIsPressed() && !KeyPressManager.handlingPrimaryKeyPress)
        {
            handleScreenSelection();
            return;
        }

        if(SaveHandler.saveNameFieldIsSelected() && 
            Input.GetKey(KeyBindingList.settingsScreenKey.getCurrentKeyCode()) && 
            !KeyPressManager.handlingPrimaryKeyPress)
        {
            EventSystem.current.SetSelectedGameObject(null);
            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        if (Input.GetKey(KeyBindingList.moveLeftKey.getCurrentKeyCode()) && !SaveHandler.saveNameFieldIsSelected() && !KeyPressManager.handlingPrimaryKeyPress)
        {
            OverallUIManager.moveToScreenToTheLeft();

            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        if (Input.GetKey(KeyBindingList.moveRightKey.getCurrentKeyCode()) && !SaveHandler.saveNameFieldIsSelected() && !KeyPressManager.handlingPrimaryKeyPress)
        {
            OverallUIManager.moveToScreenToTheRight();

            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

    }

    public static bool backOutOfUI()
    {
        if (EscapeStack.getEscapableObjectsCount() > 0)
        {
            return false;
        }

        OverallUIManager.leaveUI();
        EscapeStack.escapeAll();

        KeyPressManager.handlingPrimaryKeyPress = true;
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);

        return true;
    }

    private void handleMapStateKeyPresses()
    {
        if (MapPopUpWindow.hasFastTravelTarget() && KeyBindingList.settingsScreenOrBackKeyPressed())
        {
            MapPopUpWindow.fastTravelPanelCloseButtonPress();

            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }
        else if (!MapPopUpWindow.hasFastTravelTarget() && Input.GetKey(KeyBindingList.mapKey.getCurrentKeyCode()) || KeyBindingList.settingsScreenOrBackKeyPressed())
        {

            PlayerObject.getMapPopUpButton().destroyPopUp();

            KeyPressManager.handlingPrimaryKeyPress = true;
        }else if(!MapPopUpWindow.hasFastTravelTarget() && Input.GetKey(KeyBindingList.worldMapKey.getCurrentKeyCode()))
        {
            PlayerObject.getMapPopUpButton().destroyPopUp();
            PlayerObject.getWorldMapPopUpButton().spawnPopUp();

            KeyPressManager.handlingPrimaryKeyPress = true;
        }
    }

    private void handleWorldMapStateKeyPresses()
    {
        if (Input.GetKey(KeyBindingList.worldMapKey.getCurrentKeyCode()) || KeyBindingList.settingsScreenOrBackKeyPressed())
        {
            PlayerObject.getWorldMapPopUpButton().destroyPopUp();

            KeyPressManager.handlingPrimaryKeyPress = true;
        } else if(Input.GetKey(KeyBindingList.mapKey.getCurrentKeyCode()))
        {
            PlayerObject.getWorldMapPopUpButton().destroyPopUp();
            PlayerObject.getMapPopUpButton().spawnPopUp();

            KeyPressManager.handlingPrimaryKeyPress = true;
        }
    }

    private void handleCunningStateKeyPresses()
    {
        if ((KeyBindingList.settingsScreenOrBackKeyPressed() || Input.GetKey(KeyBindingList.skillKey.getCurrentKeyCode())) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            CunningManager.leaveCunningMode();
            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        if (KeyBindingList.movementKeyPressed() && !KeyPressManager.handlingPrimaryKeyPress)
        {
            CunningManager.getInstance().handleWASDMovement();
            KeyPressManager.handlingPrimaryKeyPress = true;
        }

        if (Input.GetKey(KeyBindingList.interactKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.handlingPrimaryKeyPress = true;

            if (CunningManager.getInstance().executeSkill())
            {
                PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
            }

            return;
        }
    }

    private void handleObservingStateKeyPresses()
    {
        if ((KeyBindingList.settingsScreenOrBackKeyPressed() || Input.GetKey(KeyBindingList.skillKey.getCurrentKeyCode())) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            ObservationManager.leaveObservationMode();
            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }
    }

    private void handleIntimidateStateKeyPresses()
    {
        if ((KeyBindingList.settingsScreenOrBackKeyPressed() || Input.GetKey(KeyBindingList.skillKey.getCurrentKeyCode())) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            IntimidateManager.leaveIntimidateMode();
            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        if (Input.GetKey(KeyBindingList.interactKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.handlingPrimaryKeyPress = true;

            if (IntimidateManager.getInstance().executeSkill())
            {
                PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
            }

            return;
        }
    }

    private void handleChestStateKeyPresses()
    {
        showFormulaToggleCheck();

        if ((KeyBindingList.settingsScreenOrBackKeyPressed() || KeyBindingList.continueUIKeyIsPressed()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
            return;
        }
    }

    private void handleBookStateKeyPresses()
    {
        if ((KeyBindingList.settingsScreenOrBackKeyPressed() || KeyBindingList.continueUIKeyIsPressed())
                && !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.escapeAll();

            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }
    }

    private void handleShopStateKeyPresses()
    {
        showFormulaToggleCheck();

        if (KeyBindingList.settingsScreenOrBackKeyPressed() && !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.escapeAll();

            KeyPressManager.handlingPrimaryKeyPress = true;
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
            return;
        }
    }

    private void handleDialoguePopUpStateKeyPresses()
    {
        if ((Input.GetKey(KeyBindingList.transcriptKey.getCurrentKeyCode()) || KeyBindingList.settingsScreenOrBackKeyPressed()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.escapeAll();

            KeyPressManager.handlingPrimaryKeyPress = true;
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
            return;
        }
    }

    private void handleLevelUpPopUpStateKeyPresses()
    {
        showFormulaToggleCheck();
    }

    private void handleTutorialPopUpStateKeyPresses()
    {
        if (KeyBindingList.settingsScreenOrBackKeyPressed() && !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.escapeAll();

            KeyPressManager.handlingPrimaryKeyPress = true;
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
            return;
        }
    }

    private void handleTutorialSequenceStateKeyPresses()
    {
        if (!TutorialSequence.currentlyInTutorialSequence())
        {
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
            return;
        }

        TutorialSequenceInput.handleCombatTutorialInput();
    }

    private static void readBook(GameObject bookGameObject)
    {
        WorldBookInfo bookInfo = bookGameObject.GetComponent<WorldBookInfo>();

        NotificationManager.OnDeleteAllNotifications.Invoke();

        bookInfo.setUpBookManager(WorldBookInfo.giveCopyOfBook, OOCActivity.walking);
    }

    private static void speakToNPC(GameObject npcGameObject)
    {
        DialogueTrigger dialogueTrigger = npcGameObject.GetComponent<DialogueTrigger>();

        if (dialogueTrigger == null || dialogueTrigger is null)
        {
            return;
        }

        NotificationManager.OnDeleteAllNotifications.Invoke();

        dialogueTrigger.triggerDialogue();
    }

    public static bool canMove()
    {
        return PlayerOOCStateManager.currentActivity == OOCActivity.walking;
    }

    public static bool handleWASDMovement()
    {
        if (!canMove())
        {
            return false;
        }

        if (!Input.anyKey)
        {
            currentMovementKeyCode = KeyCode.None;
        }

        if (currentMovementKeyCode == KeyCode.None && barredMovementKeyCodes.Count != 0)
        {
            barredMovementKeyCodes = new List<KeyCode>();
        }

        int numberOfMovementKeysPressed = KeyPressManager.numberOfMovementKeysPressed();

        switch (numberOfMovementKeysPressed)
        {
            case <= 0:
                currentMovementKeyCode = KeyCode.None;
                return false;
            case 1:
                if (!Input.GetKeyDown(currentMovementKeyCode) || currentMovementKeyCode == KeyCode.None)
                {
                    currentMovementKeyCode = KeyPressManager.getFirstMovementKeyPressedDetectedInWASDOrder();
                }

                if (barredMovementKeyCodes.Count != 0)
                {
                    barredMovementKeyCodes = new List<KeyCode>();
                }
                break;
            case 2:
                KeyCode otherKeyCode = KeyPressManager.getFirstMovementKeyPressedDetectedInWASDOrderSkippingGivenKey(currentMovementKeyCode);

                if (Input.GetKey(currentMovementKeyCode) && !barredMovementKeyCodes.Contains(otherKeyCode))
                {
                    barredMovementKeyCodes.Add(currentMovementKeyCode);

                    currentMovementKeyCode = otherKeyCode;
                }
                else if (Input.GetKey(currentMovementKeyCode) && barredMovementKeyCodes.Contains(otherKeyCode) && barredMovementKeyCodes.Contains(currentMovementKeyCode))
                {
                    return false;
                }
                else
                {
                    currentMovementKeyCode = KeyPressManager.getFirstNonBarredMovementKeyPressedDetectedInWASDOrder(barredMovementKeyCodes);

                    if (currentMovementKeyCode == KeyCode.None)
                    {
                        return false;
                    }
                }
                break;
            case >= 3:
                return false;
        }

        if (currentMovementKeyCode != KeyCode.None && Input.GetKey(currentMovementKeyCode) && !PlayerMovement.playerIsMoving() && !KeyPressManager.handlingPrimaryKeyPress)
        {
            if(currentMovementKeyCode == KeyBindingList.moveNorthKey.getCurrentKeyCode())
            {
                PlayerMovement.adjustPlayerDirectionalMod(MovementManager.distance1TileNorthEastGrid);

            } else if(currentMovementKeyCode == KeyBindingList.moveWestKey.getCurrentKeyCode())
            {
                PlayerMovement.adjustPlayerDirectionalMod(MovementManager.distance1TileNorthWestGrid);
                
            } else if(currentMovementKeyCode == KeyBindingList.moveSouthKey.getCurrentKeyCode())
            {
                PlayerMovement.adjustPlayerDirectionalMod(MovementManager.distance1TileSouthWestGrid);
                
            } else if(currentMovementKeyCode == KeyBindingList.moveEastKey.getCurrentKeyCode())
            {
                PlayerMovement.adjustPlayerDirectionalMod(MovementManager.distance1TileSouthEastGrid);
            }

            PlayerMovement.updatePlayerFacing();

            if (PositionQuery.moveableObjectAtPosition(PlayerMovement.getColliderWorldPosition()) != null)
            {

                if (!Helpers.checkPositionForColliders(PlayerMovement.getColliderWorldPosition(Constants.sizeTwo), LayerAndTagManager.blocksMoveableObjectLayerMask))
                {
                    AreaManager.getMovementManager().moveAllSprites();
                }
                else
                {
                    PlayerMovement.cancelPlayerMovement();
                    AreaManager.getMovementManager().moveAllSprites();
                }

            }
            else if (!Helpers.checkPositionForColliders(PlayerMovement.getColliderWorldPosition(), LayerAndTagManager.blocksMoveableObjectLayerMask))
            {
                AreaManager.getMovementManager().moveAllSprites();
            }
            else
            {
                PlayerMovement.cancelPlayerMovement();
                AreaManager.getMovementManager().moveAllSprites();
            }

            PlayerMovement.pollMovementAtEndOfMove();

            return true;
        }

        return false;
    }



    public static void interact()
    {
        Collider2D npcCollider = PositionQuery.npcAtPosition(PlayerMovement.getColliderWorldPosition());
        Collider2D chestCollider = PositionQuery.chestAtPosition(PlayerMovement.getColliderWorldPosition());

        if (npcCollider != null)
        {
            GameObject currentGameObject = npcCollider.gameObject;

            if (currentGameObject.tag.Equals(LayerAndTagManager.npcTag) ||
                currentGameObject.tag.Equals(LayerAndTagManager.observableTag) ||
                currentGameObject.tag.Equals(LayerAndTagManager.transitionTag)) //added transition tag for Ladders, normal transitions shouldn't be interactable
            {                                                                   //If a transition is interactable (it would throw an error when interacted with)
                                                                                //then it has it's layer set to NPC erroneously
                speakToNPC(currentGameObject);
                return;
            }
            else if (currentGameObject.tag.Equals(LayerAndTagManager.bookTag))
            {

                readBook(currentGameObject);
                PlayerOOCStateManager.setCurrentActivity(OOCActivity.inBookUI);
                return;
            }

        }
        else if (chestCollider != null)
        {
            Container currentChest = chestCollider.gameObject.GetComponent<Container>();

            if (!currentChest.hasBeenOpened())
            {
                currentChest.playerOpensChest();
                PlayerOOCStateManager.setCurrentActivity(OOCActivity.inChestUI);
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(!Application.isPlaying)
        {
            return;
        }

        drawSpriteSortPoint();

        if (AreaManager.getMovementManager() == null)
        {
            return;
        }

        if (State.playerFacing == null)
        {
            return;
        }

        switch (State.playerFacing.getFacing())
        {
            case Facing.NorthEast:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(PlayerMovement.getColliderWorldPosition(), Constants.detectionSize);
                Gizmos.DrawWireSphere(PlayerMovement.getColliderWorldPosition(Constants.sizeTwo), Constants.detectionSize);
                return;
            case Facing.SouthEast:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(PlayerMovement.getColliderWorldPosition(), Constants.detectionSize);
                Gizmos.DrawWireSphere(PlayerMovement.getColliderWorldPosition(Constants.sizeTwo), Constants.detectionSize);
                return;
            case Facing.SouthWest:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(PlayerMovement.getColliderWorldPosition(), Constants.detectionSize);
                Gizmos.DrawWireSphere(PlayerMovement.getColliderWorldPosition(Constants.sizeTwo), Constants.detectionSize);
                return;
            case Facing.NorthWest:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(PlayerMovement.getColliderWorldPosition(), Constants.detectionSize);
                Gizmos.DrawWireSphere(PlayerMovement.getColliderWorldPosition(Constants.sizeTwo), Constants.detectionSize);
                return;
            default:
                throw new IOException("Unknown facing: " + State.playerFacing.getFacing().ToString());
        }
    }

    //Draws a cyan horizontal line at the height of the sort point Unity uses to order the player's
    //current sprite against everything else in the scene.
    private static void drawSpriteSortPoint()
    {
        // AnimationManager animationManager = PlayerObject.getAnimationManager();

        // if (animationManager == null || animationManager.spriteRenderer == null)
        // {
        //     return;
        // }

        // SpriteRenderer spriteRenderer = animationManager.spriteRenderer;

        // if (spriteRenderer.sprite == null)
        // {
        //     return;
        // }

        // //Pivot sorting uses the renderer's transform position, center sorting uses the middle of its bounds.
        // Bounds bounds = spriteRenderer.bounds;
        // Vector3 sortPoint = spriteRenderer.spriteSortPoint == SpriteSortPoint.Pivot
        //     ? spriteRenderer.transform.position
        //     : bounds.center;

        // float halfWidth = Mathf.Max(bounds.size.x, 1f) / 2f;

        // Gizmos.color = Color.cyan;
        // Gizmos.DrawLine(new Vector3(sortPoint.x - halfWidth, sortPoint.y, sortPoint.z),
        //                 new Vector3(sortPoint.x + halfWidth, sortPoint.y, sortPoint.z));
    }

    public static void toggleTerrainKeyCheck()
    {
        if (Input.GetKey(KeyBindingList.hideTerrainKey.getCurrentKeyCode()) && !KeyPressManager.handlingSecondaryKeyPress)
        {
            TerrainVisibilityManager.toggleTerrainVisibility();

            KeyPressManager.handlingSecondaryKeyPress = true;
        }
    }

    public static void showFormulaToggleCheck()
    {
        if (Input.GetKey(KeyBindingList.showFormulaKey.getCurrentKeyCode()) && !OverallUIManager.showFormula)
        {
            OverallUIManager.showFormula = true;
            KeyPressManager.handlingPrimaryKeyPress = true;
        }
        else if (!Input.GetKey(KeyBindingList.showFormulaKey.getCurrentKeyCode()) && OverallUIManager.showFormula)
        {
            OverallUIManager.showFormula = false;
        }
    }
}


public static class PlayerInputList
{

    #region Shared State Effects

    //notifications and does nothing else that press, in every state but inChestUI.
    private static bool notificationsCleared()
    {
        if (PlayerOOCStateManager.currentActivity == OOCActivity.inChestUI ||
            NotificationManager.getCurrentNotificationPopUpWindowGameObject() == null)
        {
            return false;
        }

        NotificationManager.OnDeleteAllNotifications.Invoke();

        return true;
    }

    //PlayerInput.backOutOfUI (PlayerInput.cs:453-467) plus the escape-stack branch that precedes it in
    //handleUIStateKeyPresses (:394-402): a live escapable consumes the press before the UI itself closes.
    private static void backOutOfUI()
    {
        if (EscapeStack.getEscapableObjectsCount() > 0)
        {
            EscapeStack.handleEscapePress();
            return;
        }

        OverallUIManager.leaveUI();
        EscapeStack.escapeAll();

        if(PlayerOOCStateManager.currentActivity != OOCActivity.MainMenu)
        {
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
        }
    }

    //PlayerInput.handleDialogueStateKeyPresses (PlayerInput.cs:317-336). A random dialogue waits on its
    //choice buttons rather than continuing, and a hidden dialogue UI swallows the press entirely. The fade
    //check is the polled version's (:327) - without it a press during a screen fade skips a line.
    private static void continueDialogue()
    {
        DialogueManager dialogueManager = DialogueManager.getInstance();

        if (!dialogueManager.dialogueUIVisible() ||
            !dialogueManager.storyCanContinue() ||
            dialogueManager.dialogue.random ||
            FadeToBlackManager.isMidScreenFade())
        {
            return;
        }

        dialogueManager.continueStory();
    }

    //The six screen keys plus walkingOpenSettings. The save name field takes priority over every screen key
    //while it is selected (handleUIStateKeyPresses, PlayerInput.cs:403-413), which is why the guard lives here
    //rather than being repeated in each handler.
    private static void openScreen(ScreenType screenType)
    {
        if (SaveHandler.saveNameFieldIsSelected())
        {
            return;
        }

        SideScreenButtonManager.getInstance().setCurrentScreenType(screenType);
    }

    //The shared tail of every pop-up state that closes straight back to walking: inShopUI, inDialoguePopUp
    //and inTutorialPopUp (handleBackOut, PlayerInput.cs:1012-1017) plus the transcript key closing the
    //transcript it opened (handleDialoguePopUpStateKeyPresses, :607-617).
    private static void closePopUpToWalking()
    {
        EscapeStack.escapeAll();

        PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
    }

    private static void backOutToWalking()
    {
        if (notificationsCleared())
        {
            return;
        }

        closePopUpToWalking();
    }

    //handleMapStateKeyPresses (PlayerInput.cs:469-491): an open fast travel panel is closed first and owns
    //the press, so the map itself stays up.
    private static void backOutOfMap()
    {
        if (notificationsCleared())
        {
            return;
        }

        if (MapPopUpWindow.hasFastTravelTarget())
        {
            MapPopUpWindow.fastTravelPanelCloseButtonPress();
            return;
        }

        PlayerObject.getMapPopUpButton().destroyPopUp();
    }

    private static void backOutOfWorldMap()
    {
        if (notificationsCleared())
        {
            return;
        }

        PlayerObject.getWorldMapPopUpButton().destroyPopUp();
    }

    private static void cancelCunning()
    {
        if (notificationsCleared())
        {
            return;
        }

        CunningManager.leaveCunningMode();
    }

    private static void cancelObserving()
    {
        if (notificationsCleared())
        {
            return;
        }

        ObservationManager.leaveObservationMode();
    }

    private static void cancelIntimidating()
    {
        if (notificationsCleared())
        {
            return;
        }

        IntimidateManager.leaveIntimidateMode();
    }

    //No notificationsCleared call: it returns false in inChestUI by its own test above, so guarding here would
    //read as if the chest swallowed a press to clear notifications when it never does.
    private static void closeChestUI()
    {
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
    }

    //The book's own escapable sets the state back on its way out, so this does not set walking itself
    //(handleBookStateKeyPresses, PlayerInput.cs:581-591).
    private static void closeBookUI()
    {
        EscapeStack.escapeAll();
    }

    private static void backOutOfBookUI()
    {
        if (notificationsCleared())
        {
            return;
        }

        closeBookUI();
    }

    #endregion

    #region State Independent KeyBinds

    //Keys whose effect does not depend on the current OOCActivity, so one action serves every state that
    //accepts them. Three of them - toggleTerrain, toggleKeyBindingsList and showFormula - are the only
    //actions in this file that appear in more than one enable set, which is safe because none of their
    //handlers can reach setCurrentActivity and so trigger the disable/re-enable cycle described below.

    #region Movement KeyBinds

    #region Move North
    public static readonly CustomInputAction moveNorth = new CustomInputAction(KeyBindingList.moveNorthKey.getCurrentKeyCode(),
                                                                                    onStarted: onMoveNorthKeyPress);

    private static void onMoveNorthKeyPress(InputAction.CallbackContext context = default)
    {
        PlayerInput.handleWASDMovement();
    }
    #endregion

    #region Move West
    public static readonly CustomInputAction moveWest = new CustomInputAction(KeyBindingList.moveWestKey.getCurrentKeyCode(),
                                                                                    onStarted: onMoveWestKeyPress);

    private static void onMoveWestKeyPress(InputAction.CallbackContext context = default)
    {
        PlayerInput.handleWASDMovement();
    }
    #endregion

    #region Move South
    public static readonly CustomInputAction moveSouth = new CustomInputAction(KeyBindingList.moveSouthKey.getCurrentKeyCode(),
                                                                                    onStarted: onMoveSouthKeyPress);

    private static void onMoveSouthKeyPress(InputAction.CallbackContext context = default)
    {
        PlayerInput.handleWASDMovement();
    }
    #endregion

    #region Move East
    public static readonly CustomInputAction moveEast = new CustomInputAction(KeyBindingList.moveEastKey.getCurrentKeyCode(),
                                                                                    onStarted: onMoveEastKeyPress);

    private static void onMoveEastKeyPress(InputAction.CallbackContext context = default)
    {
        PlayerInput.handleWASDMovement();
    }
    #endregion

    // private static void stepPlayer(Vector3Int directionalMod)
    // {
    //     if (PlayerMovement.playerIsMoving())
    //     {
    //         return;
    //     }

    //     PlayerMovement.adjustPlayerDirectionalMod(directionalMod);
    //     PlayerMovement.updatePlayerFacing();

    //     if (PositionQuery.moveableObjectAtPosition(PlayerMovement.getColliderWorldPosition()) != null)
    //     {
    //         if (Helpers.checkPositionForColliders(PlayerMovement.getColliderWorldPosition(Constants.sizeTwo),
    //                                                 LayerAndTagManager.blocksMoveableObjectLayerMask))
    //         {
    //             PlayerMovement.cancelPlayerMovement();
    //         }
    //     }
    //     else if (Helpers.checkPositionForColliders(PlayerMovement.getColliderWorldPosition(),
    //                                                 LayerAndTagManager.blocksMoveableObjectLayerMask))
    //     {
    //         PlayerMovement.cancelPlayerMovement();
    //     }

    //     AreaManager.getMovementManager().moveAllSprites();
    //     PlayerMovement.pollMovementAtEndOfMove();
    // }

    #endregion

    #region Overworld Keys

    #region Hide Terrain
    public static readonly CustomInputAction toggleTerrain = new CustomInputAction(KeyBindingList.hideTerrainKey.getCurrentKeyCode(),
                                                                                    onStarted: onHideTerrainKeyPress);

    private static void onHideTerrainKeyPress(InputAction.CallbackContext context)
    {
        TerrainVisibilityManager.toggleTerrainVisibility();
    }
    #endregion

    #region Reveal Interactable Objects
    public static readonly CustomInputAction toggleReveal = new CustomInputAction(KeyBindingList.revealKey.getCurrentKeyCode(),
                                                                                    onStarted: onRevealKeyPress);

    private static void onRevealKeyPress(InputAction.CallbackContext context)
    {
        RevealManager.toggleReveal();
    }
    #endregion

    #region Remove Object
    public static readonly CustomInputAction removePlacedObject = new CustomInputAction(KeyBindingList.removePlacedCompanionMovableObjectKey.getCurrentKeyCode(),
                                                                                    onStarted: onRemovePlacedCompanionMovableObjectKeyPress);

    private static void onRemovePlacedCompanionMovableObjectKeyPress(InputAction.CallbackContext context)
    {
        Collider2D npcCollider = PositionQuery.npcAtPosition(PlayerMovement.getColliderWorldPosition());
        Collider2D moveableObjectCollider = PositionQuery.moveableObjectAtPosition(PlayerMovement.getColliderWorldPosition());

        if (npcCollider != null)
        {
            GameObject npcGameObject = npcCollider.gameObject;

            if (!npcGameObject.tag.Equals(LayerAndTagManager.partyMemberTag))
            {
                return;
            }

            string partyMemberName = npcGameObject.GetComponent<PlacedPartyMember>().partyMember.getName();
            PartyMemberPlacer.removePlacedPartyMember(partyMemberName);
        }
        else if (moveableObjectCollider != null)
        {
            EnemyMovement enemyMovement = moveableObjectCollider.gameObject.GetComponent<EnemyMovement>();
            enemyMovement.putBackToStartingPosition();
            MovementManager.OnMoveFinished.Invoke(PlayerMovement.getPlayerMovementIndex());
        }

        OOCUIManager.updateOOCUI();
    }
    #endregion

    #region Quick Save
    public static readonly CustomInputAction quickSave = new CustomInputAction(KeyBindingList.quicksaveKey.getCurrentKeyCode(),
                                                                                    onStarted: onQuicksaveKeyPress);

    private static void onQuicksaveKeyPress(InputAction.CallbackContext context)
    {
        SaveHandler.quickSave();
    }
    #endregion

    #region Show/Hide Keybindings
    public static readonly CustomInputAction toggleKeyBindingsList = new CustomInputAction(KeyBindingList.showHideKeyBindingsListKey.getCurrentKeyCode(),
                                                                                    onStarted: onShowHideKeyBindingsListKeyPress);

    private static void onShowHideKeyBindingsListKeyPress(InputAction.CallbackContext context)
    {
        CombatInputManager.OnHideKeyBindingsList.Invoke();
    }
    #endregion

    #endregion

    #region Skill Keys

    #region Next Skill
    public static readonly CustomInputAction cycleSkillAscending = new CustomInputAction(KeyBindingList.cycleSkillAscendingKey.getCurrentKeyCode(),
                                                                                    onStarted: onCycleSkillAscendingKeyPress);

    private static void onCycleSkillAscendingKeyPress(InputAction.CallbackContext context)
    {
        SkillButtonManager.changeSkill(false);
    }
    #endregion

    #region Previous Skill
    public static readonly CustomInputAction cycleSkillDescending = new CustomInputAction(KeyBindingList.cycleSkillDescendingKey.getCurrentKeyCode(),
                                                                                    onStarted: onCycleSkillDescendingKeyPress);

    private static void onCycleSkillDescendingKeyPress(InputAction.CallbackContext context)
    {
        SkillButtonManager.changeSkill(true);
    }
    #endregion

    #endregion

    #region UI Keybinds

    #region Left Screen
    public static readonly CustomInputAction moveScreenLeft = new CustomInputAction(KeyBindingList.moveLeftKey.getCurrentKeyCode(),
                                                                                    onStarted: onMoveLeftKeyPress);

    private static void onMoveLeftKeyPress(InputAction.CallbackContext context)
    {
        if (SaveHandler.saveNameFieldIsSelected())
        {
            return;
        }

        OverallUIManager.moveToScreenToTheLeft();
    }
    #endregion

    #region Right Screen
    public static readonly CustomInputAction moveScreenRight = new CustomInputAction(KeyBindingList.moveRightKey.getCurrentKeyCode(),
                                                                                    onStarted: onMoveRightKeyPress);

    private static void onMoveRightKeyPress(InputAction.CallbackContext context)
    {
        if (SaveHandler.saveNameFieldIsSelected())
        {
            return;
        }

        OverallUIManager.moveToScreenToTheRight();
    }
    #endregion

    #region Inspect
    public static readonly CustomInputAction toggleInspect = new CustomInputAction(KeyBindingList.inspectKey.getCurrentKeyCode(),
                                                                                    onStarted: onInspectKeyPress);

    private static void onInspectKeyPress(InputAction.CallbackContext context)
    {
        //No effect available yet. InspectNode.setToInspectingMode and exitInspectingMode are private instance
        //methods (InspectNode.cs:76-90), and entering inspect mode also needs the private SerializeField hover.
        //This needs a public toggle on InspectNode before it can be written.
    }
    #endregion

    #region Show Formulas
    //Hold to show rather than a toggle (showFormulaToggleCheck, PlayerInput.cs:975-986), so the same handler
    //takes both edges and reads the button state off the context - started gives true, canceled gives false.
    public static readonly CustomInputAction showFormula = new CustomInputAction(KeyBindingList.showFormulaKey.getCurrentKeyCode(),
                                                                                    onStarted: onShowFormulaKeyPress,
                                                                                    onCanceled: onShowFormulaKeyPress);

    private static void onShowFormulaKeyPress(InputAction.CallbackContext context)
    {
        OverallUIManager.showFormula = context.ReadValueAsButton();
    }
    #endregion

    #region Shop Max Amount
    public static readonly CustomInputAction maxAmount = new CustomInputAction(KeyBindingList.maxAmountKey.getCurrentKeyCode(),
                                                                                    onStarted: onMaxAmountKeyPress);

    private static void onMaxAmountKeyPress(InputAction.CallbackContext context)
    {
        //No effect on its own. This is a held modifier read inside AmountPanel.incrementAmount and
        //decrementAmount (AmountPanel.cs:143, 157), both driven by UI button clicks rather than by this key.
    }
    #endregion

    #region Shop Amount x10
    public static readonly CustomInputAction multiplyByTenAmount = new CustomInputAction(KeyBindingList.multiplyByTenAmountKey.getCurrentKeyCode(),
                                                                                    onStarted: onMultiplyByTenAmountKeyPress);

    private static void onMultiplyByTenAmountKeyPress(InputAction.CallbackContext context)
    {
        //No effect on its own. This is a held modifier read inside AmountPanel.incrementAmount and
        //decrementAmount (AmountPanel.cs:146, 161), both driven by UI button clicks rather than by this key.
    }
    #endregion

    #endregion

    #region Combat Keys

    #region Select
    public static readonly CustomInputAction combatSelect = new CustomInputAction(KeyBindingList.combatSelectKey.getCurrentKeyCode(),
                                                                                    onStarted: onCombatSelectKeyPress);

    private static void onCombatSelectKeyPress(InputAction.CallbackContext context)
    {
        SelectorManager.handleAllySelection();
    }
    #endregion

    #region Deselect
    public static readonly CustomInputAction combatDeselect = new CustomInputAction(KeyBindingList.combatDeselectKey.getCurrentKeyCode(),
                                                                                    onStarted: onCombatDeselectKeyPress);

    private static void onCombatDeselectKeyPress(InputAction.CallbackContext context)
    {
        SelectorManager.deselectAlly();
        SelectorManager.displayCurrentHoverUI();
    }
    #endregion

    #region Resolve Turn
    public static readonly CustomInputAction resolveTurn = new CustomInputAction(KeyBindingList.resolveTurnKey.getCurrentKeyCode(),
                                                                                    onStarted: onResolveTurnKeyPress);

    private static void onResolveTurnKeyPress(InputAction.CallbackContext context)
    {
        CombatStateManager.resolveTurn();
    }
    #endregion

    #region Jump Move
    public static readonly CustomInputAction jumpMove = new CustomInputAction(KeyBindingList.jumpMoveKey.getCurrentKeyCode(),
                                                                                    onStarted: onJumpMoveKeyPress);

    private static void onJumpMoveKeyPress(InputAction.CallbackContext context)
    {
        //The polled version is really a chord - it also requires KeyBindingList.movementKeyPressed()
        //(SelectorManager.cs:514), so as a standalone press this snaps without a direction.
        SelectorManager.moveCurrentSelectorToNextSingleTileTarget();
        SelectorManager.displayCurrentHoverUI();
    }
    #endregion

    #region Settings Menu
    public static readonly CustomInputAction openCombatSettings = new CustomInputAction(KeyBindingList.combatSettingsScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onCombatSettingsScreenKeyPress);

    private static void onCombatSettingsScreenKeyPress(InputAction.CallbackContext context)
    {
        if(CombatEscapeMenuPopUpButton.getInstance() != null)
        {
            CombatEscapeMenuPopUpButton.getInstance().spawnPopUp();
        }
    }
    #endregion

    #region Fast Forward
    public static readonly CustomInputAction fastForwardAnimation = new CustomInputAction(KeyBindingList.combatFastForwardAnimationKey.getCurrentKeyCode(),
                                                                                    onStarted: onCombatFastForwardAnimationKeyPress);

    private static void onCombatFastForwardAnimationKeyPress(InputAction.CallbackContext context)
    {
        //No effect available yet. The only consumer is CombatStateManager.setTimeScale
        //(CombatStateManager.cs:889-910), which re-reads this key every frame, and the time scale constants it
        //needs are private. This needs a public setFastForward on CombatStateManager before it can be written.
    }
    #endregion

    #endregion

    #region Action Wheel

    #region Action Wheel Counter Clockwise
    public static readonly CustomInputAction moveWheelCounterClockwise = new CustomInputAction(KeyBindingList.moveCounterClockwiseKey.getCurrentKeyCode(),
                                                                                    onStarted: onMoveCounterClockwiseKeyPress);

    private static void onMoveCounterClockwiseKeyPress(InputAction.CallbackContext context)
    {
        AbilityMenuManager.getInstance().moveSelectedButtonCounterClockwise();
    }
    #endregion

    #region Action Wheel Clockwise
    public static readonly CustomInputAction moveWheelClockwise = new CustomInputAction(KeyBindingList.moveClockwiseKey.getCurrentKeyCode(),
                                                                                    onStarted: onMoveClockwiseKeyPress);

    private static void onMoveClockwiseKeyPress(InputAction.CallbackContext context)
    {
        AbilityMenuManager.getInstance().moveSelectedButtonClockwise();
    }
    #endregion

    #endregion

   #region Unchangable Keybinds

    public static readonly CustomInputAction chooseDialogueOption1 = new CustomInputAction(KeyCode.Alpha1,
                                                                                            onStarted: chooseChoiceOne);

    private static void chooseChoiceOne(InputAction.CallbackContext context)
    {
        DialogueManager.getInstance().makeChoice(0);
    }

    public static readonly CustomInputAction chooseDialogueOption2 = new CustomInputAction(KeyCode.Alpha2,
                                                                                            onStarted: chooseChoiceTwo);

    private static void chooseChoiceTwo(InputAction.CallbackContext context)
    {
        DialogueManager.getInstance().makeChoice(1);
    }

    public static readonly CustomInputAction chooseDialogueOption3 = new CustomInputAction(KeyCode.Alpha3,
                                                                                            onStarted: chooseChoiceThree);

    private static void chooseChoiceThree(InputAction.CallbackContext context)
    {
        DialogueManager.getInstance().makeChoice(2);
    }

    public static readonly CustomInputAction chooseDialogueOption4 = new CustomInputAction(KeyCode.Alpha4,
                                                                                            onStarted: chooseChoiceFour);

    private static void chooseChoiceFour(InputAction.CallbackContext context)
    {
        DialogueManager.getInstance().makeChoice(3);
    }

    public static readonly CustomInputAction chooseDialogueOption5 = new CustomInputAction(KeyCode.Alpha5,
                                                                                            onStarted: chooseChoiceFive);

    private static void chooseChoiceFive(InputAction.CallbackContext context)
    {
        DialogueManager.getInstance().makeChoice(4);
    }

    public static readonly CustomInputAction chooseDialogueOption6 = new CustomInputAction(KeyCode.Alpha6,
                                                                                            onStarted: chooseChoiceSix);

    private static void chooseChoiceSix(InputAction.CallbackContext context)
    {
        DialogueManager.getInstance().makeChoice(5);
    }

    public static readonly CustomInputAction chooseDialogueOption7 = new CustomInputAction(KeyCode.Alpha7,
                                                                                            onStarted: chooseChoiceSeven);

    private static void chooseChoiceSeven(InputAction.CallbackContext context)
    {
        DialogueManager.getInstance().makeChoice(6);
    }

    public static readonly CustomInputAction chooseDialogueOption8 = new CustomInputAction(KeyCode.Alpha8,
                                                                                            onStarted: chooseChoiceEight);

    private static void chooseChoiceEight(InputAction.CallbackContext context)
    {
        DialogueManager.getInstance().makeChoice(7);
    }


    #endregion

    #endregion

    #region Per-State KeyBinds

    //One CustomInputAction per key per OOCActivity, so the enable set is the only thing that decides which
    //state a key is live in. A handler here never tests currentActivity - if it runs, its state is current.
    //
    //This is also what stops a press firing twice. setCurrentActivity ends in updateEnabledInputActions,
    //which disables every action and re-enables the new state's set, so a handler that changes state does
    //that from inside its own started callback. An action that is in both the outgoing and the incoming set
    //gets Disable()d and Enable()d mid-press, and Enable() rewinds its phase to Waiting - the Input System
    //then reaches the Performed transition for the same press, sees Waiting, and "detours via Started",
    //calling the handler a second time. One action per state means the outgoing one is never re-enabled, so
    //the disabled check swallows the rest of that press instead.
    //
    //The three keys below that appear in more than one enable set (toggleTerrain, toggleKeyBindingsList,
    //showFormula) are safe to share because their handlers cannot reach setCurrentActivity.
    //
    //  backOutKey / settingsScreenKey   the two disjuncts of settingsScreenOrBackKeyPressed, so outside
    //                                   walking, inUI and MainMenu both call one shared helper
    //  interactKey / acceptKey /        the three disjuncts of continueUIKeyIsPressed, so inDialogue,
    //  acceptInputKey                   inChestUI and inBookUI treat all three alike

    #region Walking

    public static readonly CustomInputAction walkingInteract = new CustomInputAction(KeyBindingList.interactKey.getCurrentKeyCode(),
                                                                                    onStarted: onWalkingInteractKeyPress);

    private static void onWalkingInteractKeyPress(InputAction.CallbackContext context)
    {
        if (PlayerMovement.canInteract())
        {
            PlayerInput.interact();
        }
    }

    //Clearing notifications is the whole effect of the back key while walking - handleBackOut ran the sweep
    //and then fell through to its default arm (PlayerInput.cs:1018-1019).
    public static readonly CustomInputAction walkingBackOut = new CustomInputAction(KeyBindingList.backOutKey.getCurrentKeyCode(),
                                                                                    onStarted: onWalkingBackOutKeyPress);

    private static void onWalkingBackOutKeyPress(InputAction.CallbackContext context)
    {
        notificationsCleared();
    }

    public static readonly CustomInputAction walkingOpenTranscript = new CustomInputAction(KeyBindingList.transcriptKey.getCurrentKeyCode(),
                                                                                    onStarted: onWalkingOpenTranscriptKeyPress);

    private static void onWalkingOpenTranscriptKeyPress(InputAction.CallbackContext context)
    {
        DialogueBookItem dialogueBook = new DialogueBookItem();
        dialogueBook.use(PartyManager.getPlayerStats());

        PlayerOOCStateManager.setCurrentActivity(OOCActivity.inDialoguePopUp);
    }

    public static readonly CustomInputAction walkingUseSkill = new CustomInputAction(KeyBindingList.skillKey.getCurrentKeyCode(),
                                                                                    onStarted: onWalkingUseSkillKeyPress);

    private static void onWalkingUseSkillKeyPress(InputAction.CallbackContext context)
    {
        SkillButtonManager.useSkill();
    }

    public static readonly CustomInputAction walkingOpenLastScreen = new CustomInputAction(KeyBindingList.lastScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onWalkingOpenLastScreenKeyPress);

    private static void onWalkingOpenLastScreenKeyPress(InputAction.CallbackContext context)
    {
        if (SaveHandler.saveNameFieldIsSelected())
        {
            return;
        }

        EscapeStack.escapeAll();

        OverallUIManager.changeScreen(OverallUIManager.lastScreenType);

        PlayerOOCStateManager.setCurrentActivity(OOCActivity.inUI);
    }

    public static readonly CustomInputAction walkingOpenCharacter = new CustomInputAction(KeyBindingList.characterScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onWalkingOpenCharacterKeyPress);

    private static void onWalkingOpenCharacterKeyPress(InputAction.CallbackContext context)
    {
        openScreen(ScreenType.Character);
    }

    public static readonly CustomInputAction walkingOpenInventory = new CustomInputAction(KeyBindingList.inventoryScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onWalkingOpenInventoryKeyPress);

    private static void onWalkingOpenInventoryKeyPress(InputAction.CallbackContext context)
    {
        openScreen(ScreenType.Inventory);
    }

    public static readonly CustomInputAction walkingOpenParty = new CustomInputAction(KeyBindingList.partyScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onWalkingOpenPartyKeyPress);

    private static void onWalkingOpenPartyKeyPress(InputAction.CallbackContext context)
    {
        openScreen(ScreenType.Party);
    }

    public static readonly CustomInputAction walkingOpenJournal = new CustomInputAction(KeyBindingList.journalScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onWalkingOpenJournalKeyPress);

    private static void onWalkingOpenJournalKeyPress(InputAction.CallbackContext context)
    {
        openScreen(ScreenType.Journal);
    }

    public static readonly CustomInputAction walkingOpenSaveAndLoad = new CustomInputAction(KeyBindingList.loadScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onWalkingOpenSaveAndLoadKeyPress);

    private static void onWalkingOpenSaveAndLoadKeyPress(InputAction.CallbackContext context)
    {
        openScreen(ScreenType.SaveAndLoad);
    }

    public static readonly CustomInputAction walkingOpenSettings = new CustomInputAction(KeyBindingList.settingsScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onWalkingOpenSettingsKeyPress);

    private static void onWalkingOpenSettingsKeyPress(InputAction.CallbackContext context)
    {
        if (notificationsCleared())
        {
            return;
        }

        openScreen(ScreenType.Settings);
    }

    public static readonly CustomInputAction walkingOpenMap = new CustomInputAction(KeyBindingList.mapKey.getCurrentKeyCode(),
                                                                                    onStarted: onWalkingOpenMapKeyPress);

    private static void onWalkingOpenMapKeyPress(InputAction.CallbackContext context)
    {
        PlayerObject.getMapPopUpButton().spawnPopUp();
    }

    public static readonly CustomInputAction walkingOpenWorldMap = new CustomInputAction(KeyBindingList.worldMapKey.getCurrentKeyCode(),
                                                                                    onStarted: onWalkingOpenWorldMapKeyPress);

    private static void onWalkingOpenWorldMapKeyPress(InputAction.CallbackContext context)
    {
        PlayerObject.getWorldMapPopUpButton().spawnPopUp();
    }

    #endregion

    #region In Dialogue

    public static readonly CustomInputAction inDialogueInteract = new CustomInputAction(KeyBindingList.interactKey.getCurrentKeyCode(),
                                                                                    onStarted: onInDialogueInteractKeyPress);

    private static void onInDialogueInteractKeyPress(InputAction.CallbackContext context)
    {
        continueDialogue();
    }

    public static readonly CustomInputAction inDialogueAccept = new CustomInputAction(KeyBindingList.acceptKey.getCurrentKeyCode(),
                                                                                    onStarted: onInDialogueAcceptKeyPress);

    private static void onInDialogueAcceptKeyPress(InputAction.CallbackContext context)
    {
        continueDialogue();
    }

    public static readonly CustomInputAction inDialogueAcceptInput = new CustomInputAction(KeyBindingList.acceptInputKey.getCurrentKeyCode(),
                                                                                    onStarted: onInDialogueAcceptInputKeyPress);

    private static void onInDialogueAcceptInputKeyPress(InputAction.CallbackContext context)
    {
        continueDialogue();
    }

    #endregion

    #region In UI

    public static readonly CustomInputAction inUIBackOut = new CustomInputAction(KeyBindingList.backOutKey.getCurrentKeyCode(),
                                                                                    onStarted: onInUIBackOutKeyPress);

    private static void onInUIBackOutKeyPress(InputAction.CallbackContext context)
    {
        if (notificationsCleared())
        {
            return;
        }

        backOutOfUI();
    }

    //The same key that opened the UI from walking closes it again (onLastScreenKeyPress, PlayerInput.cs:1384).
    public static readonly CustomInputAction inUIOpenLastScreen = new CustomInputAction(KeyBindingList.lastScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onInUIOpenLastScreenKeyPress);

    private static void onInUIOpenLastScreenKeyPress(InputAction.CallbackContext context)
    {
        if (SaveHandler.saveNameFieldIsSelected())
        {
            return;
        }

        backOutOfUI();
    }

    public static readonly CustomInputAction inUIOpenCharacter = new CustomInputAction(KeyBindingList.characterScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onInUIOpenCharacterKeyPress);

    private static void onInUIOpenCharacterKeyPress(InputAction.CallbackContext context)
    {
        openScreen(ScreenType.Character);
    }

    public static readonly CustomInputAction inUIOpenInventory = new CustomInputAction(KeyBindingList.inventoryScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onInUIOpenInventoryKeyPress);

    private static void onInUIOpenInventoryKeyPress(InputAction.CallbackContext context)
    {
        openScreen(ScreenType.Inventory);
    }

    public static readonly CustomInputAction inUIOpenParty = new CustomInputAction(KeyBindingList.partyScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onInUIOpenPartyKeyPress);

    private static void onInUIOpenPartyKeyPress(InputAction.CallbackContext context)
    {
        openScreen(ScreenType.Party);
    }

    public static readonly CustomInputAction inUIOpenJournal = new CustomInputAction(KeyBindingList.journalScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onInUIOpenJournalKeyPress);

    private static void onInUIOpenJournalKeyPress(InputAction.CallbackContext context)
    {
        openScreen(ScreenType.Journal);
    }

    public static readonly CustomInputAction inUIOpenSaveAndLoad = new CustomInputAction(KeyBindingList.loadScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onInUIOpenSaveAndLoadKeyPress);

    private static void onInUIOpenSaveAndLoadKeyPress(InputAction.CallbackContext context)
    {
        openScreen(ScreenType.SaveAndLoad);
    }

    public static readonly CustomInputAction inUIOpenSettings = new CustomInputAction(KeyBindingList.settingsScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onInUIOpenSettingsKeyPress);

    private static void onInUIOpenSettingsKeyPress(InputAction.CallbackContext context)
    {
        if (notificationsCleared())
        {
            return;
        }

        //A selected save name field takes this key to deselect itself rather than backing out
        //(handleUIStateKeyPresses, PlayerInput.cs:426-433), which is also why it never opens Settings here.
        if (SaveHandler.saveNameFieldIsSelected())
        {
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }

        backOutOfUI();
    }

    #endregion

    #region In Map

    public static readonly CustomInputAction inMapBackOut = new CustomInputAction(KeyBindingList.backOutKey.getCurrentKeyCode(),
                                                                                    onStarted: onInMapBackOutKeyPress);

    private static void onInMapBackOutKeyPress(InputAction.CallbackContext context)
    {
        backOutOfMap();
    }

    public static readonly CustomInputAction inMapOpenSettings = new CustomInputAction(KeyBindingList.settingsScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onInMapOpenSettingsKeyPress);

    private static void onInMapOpenSettingsKeyPress(InputAction.CallbackContext context)
    {
        backOutOfMap();
    }

    //An open fast travel panel owns the map keys until a back key closes it (handleMapStateKeyPresses,
    //PlayerInput.cs:469-491).
    public static readonly CustomInputAction inMapOpenMap = new CustomInputAction(KeyBindingList.mapKey.getCurrentKeyCode(),
                                                                                    onStarted: onInMapOpenMapKeyPress);

    private static void onInMapOpenMapKeyPress(InputAction.CallbackContext context)
    {
        if (MapPopUpWindow.hasFastTravelTarget())
        {
            return;
        }

        PlayerObject.getMapPopUpButton().destroyPopUp();
    }

    public static readonly CustomInputAction inMapOpenWorldMap = new CustomInputAction(KeyBindingList.worldMapKey.getCurrentKeyCode(),
                                                                                    onStarted: onInMapOpenWorldMapKeyPress);

    private static void onInMapOpenWorldMapKeyPress(InputAction.CallbackContext context)
    {
        if (MapPopUpWindow.hasFastTravelTarget())
        {
            return;
        }

        PlayerObject.getMapPopUpButton().destroyPopUp();
        PlayerObject.getWorldMapPopUpButton().spawnPopUp();
    }

    #endregion

    #region Cunning

    public static readonly CustomInputAction cunningBackOut = new CustomInputAction(KeyBindingList.backOutKey.getCurrentKeyCode(),
                                                                                    onStarted: onCunningBackOutKeyPress);

    private static void onCunningBackOutKeyPress(InputAction.CallbackContext context)
    {
        cancelCunning();
    }

    public static readonly CustomInputAction cunningOpenSettings = new CustomInputAction(KeyBindingList.settingsScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onCunningOpenSettingsKeyPress);

    private static void onCunningOpenSettingsKeyPress(InputAction.CallbackContext context)
    {
        cancelCunning();
    }

    //No notification sweep: the skill key cancels through the second disjunct of the polled check
    //(handleCunningStateKeyPresses, PlayerInput.cs:511), not through the back-key predicate.
    public static readonly CustomInputAction cunningUseSkill = new CustomInputAction(KeyBindingList.skillKey.getCurrentKeyCode(),
                                                                                    onStarted: onCunningUseSkillKeyPress);

    private static void onCunningUseSkillKeyPress(InputAction.CallbackContext context)
    {
        CunningManager.leaveCunningMode();
    }

    public static readonly CustomInputAction cunningInteract = new CustomInputAction(KeyBindingList.interactKey.getCurrentKeyCode(),
                                                                                    onStarted: onCunningInteractKeyPress);

    private static void onCunningInteractKeyPress(InputAction.CallbackContext context)
    {
        if (CunningManager.getInstance().executeSkill())
        {
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
        }
    }

    //These do not go through CunningManager.handleWASDMovement: that method re-reads Input.GetKey to work out
    //which way to go (CunningManager.cs:300-315), so driving it from an event would make all four keys
    //interchangeable. Calling the matching mover directly keeps the key that fired as the one that decides.
    //The setButtonPromptVisibility call is the part handleWASDMovement did afterwards (CunningManager.cs:317)
    //that moveCurrentSelector does not do for itself.

    public static readonly CustomInputAction cunningMoveNorth = new CustomInputAction(KeyBindingList.moveNorthKey.getCurrentKeyCode(),
                                                                                    onStarted: onCunningMoveNorthKeyPress);

    private static void onCunningMoveNorthKeyPress(InputAction.CallbackContext context)
    {
        CunningManager.getInstance().moveCurrentSelectorNorthEast();
        PlayerObject.setButtonPromptVisibility();
    }

    public static readonly CustomInputAction cunningMoveWest = new CustomInputAction(KeyBindingList.moveWestKey.getCurrentKeyCode(),
                                                                                    onStarted: onCunningMoveWestKeyPress);

    private static void onCunningMoveWestKeyPress(InputAction.CallbackContext context)
    {
        CunningManager.getInstance().moveCurrentSelectorNorthWest();
        PlayerObject.setButtonPromptVisibility();
    }

    public static readonly CustomInputAction cunningMoveSouth = new CustomInputAction(KeyBindingList.moveSouthKey.getCurrentKeyCode(),
                                                                                    onStarted: onCunningMoveSouthKeyPress);

    private static void onCunningMoveSouthKeyPress(InputAction.CallbackContext context)
    {
        CunningManager.getInstance().moveCurrentSelectorSouthWest();
        PlayerObject.setButtonPromptVisibility();
    }

    public static readonly CustomInputAction cunningMoveEast = new CustomInputAction(KeyBindingList.moveEastKey.getCurrentKeyCode(),
                                                                                    onStarted: onCunningMoveEastKeyPress);

    private static void onCunningMoveEastKeyPress(InputAction.CallbackContext context)
    {
        CunningManager.getInstance().moveCurrentSelectorSouthEast();
        PlayerObject.setButtonPromptVisibility();
    }

    #endregion

    #region Observing

    public static readonly CustomInputAction observingBackOut = new CustomInputAction(KeyBindingList.backOutKey.getCurrentKeyCode(),
                                                                                    onStarted: onObservingBackOutKeyPress);

    private static void onObservingBackOutKeyPress(InputAction.CallbackContext context)
    {
        cancelObserving();
    }

    public static readonly CustomInputAction observingOpenSettings = new CustomInputAction(KeyBindingList.settingsScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onObservingOpenSettingsKeyPress);

    private static void onObservingOpenSettingsKeyPress(InputAction.CallbackContext context)
    {
        cancelObserving();
    }

    public static readonly CustomInputAction observingUseSkill = new CustomInputAction(KeyBindingList.skillKey.getCurrentKeyCode(),
                                                                                    onStarted: onObservingUseSkillKeyPress);

    private static void onObservingUseSkillKeyPress(InputAction.CallbackContext context)
    {
        ObservationManager.leaveObservationMode();
    }

    #endregion

    #region Intimidating

    public static readonly CustomInputAction intimidatingBackOut = new CustomInputAction(KeyBindingList.backOutKey.getCurrentKeyCode(),
                                                                                    onStarted: onIntimidatingBackOutKeyPress);

    private static void onIntimidatingBackOutKeyPress(InputAction.CallbackContext context)
    {
        cancelIntimidating();
    }

    public static readonly CustomInputAction intimidatingOpenSettings = new CustomInputAction(KeyBindingList.settingsScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onIntimidatingOpenSettingsKeyPress);

    private static void onIntimidatingOpenSettingsKeyPress(InputAction.CallbackContext context)
    {
        cancelIntimidating();
    }

    public static readonly CustomInputAction intimidatingUseSkill = new CustomInputAction(KeyBindingList.skillKey.getCurrentKeyCode(),
                                                                                    onStarted: onIntimidatingUseSkillKeyPress);

    private static void onIntimidatingUseSkillKeyPress(InputAction.CallbackContext context)
    {
        IntimidateManager.leaveIntimidateMode();
    }

    public static readonly CustomInputAction intimidatingInteract = new CustomInputAction(KeyBindingList.interactKey.getCurrentKeyCode(),
                                                                                    onStarted: onIntimidatingInteractKeyPress);

    private static void onIntimidatingInteractKeyPress(InputAction.CallbackContext context)
    {
        if (IntimidateManager.getInstance().executeSkill())
        {
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
        }
    }

    #endregion

    #region In Chest UI

    public static readonly CustomInputAction inChestUIBackOut = new CustomInputAction(KeyBindingList.backOutKey.getCurrentKeyCode(),
                                                                                    onStarted: onInChestUIBackOutKeyPress);

    private static void onInChestUIBackOutKeyPress(InputAction.CallbackContext context)
    {
        closeChestUI();
    }

    public static readonly CustomInputAction inChestUIOpenSettings = new CustomInputAction(KeyBindingList.settingsScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onInChestUIOpenSettingsKeyPress);

    private static void onInChestUIOpenSettingsKeyPress(InputAction.CallbackContext context)
    {
        closeChestUI();
    }

    public static readonly CustomInputAction inChestUIInteract = new CustomInputAction(KeyBindingList.interactKey.getCurrentKeyCode(),
                                                                                    onStarted: onInChestUIInteractKeyPress);

    private static void onInChestUIInteractKeyPress(InputAction.CallbackContext context)
    {
        closeChestUI();
    }

    public static readonly CustomInputAction inChestUIAccept = new CustomInputAction(KeyBindingList.acceptKey.getCurrentKeyCode(),
                                                                                    onStarted: onInChestUIAcceptKeyPress);

    private static void onInChestUIAcceptKeyPress(InputAction.CallbackContext context)
    {
        closeChestUI();
    }

    public static readonly CustomInputAction inChestUIAcceptInput = new CustomInputAction(KeyBindingList.acceptInputKey.getCurrentKeyCode(),
                                                                                    onStarted: onInChestUIAcceptInputKeyPress);

    private static void onInChestUIAcceptInputKeyPress(InputAction.CallbackContext context)
    {
        closeChestUI();
    }

    #endregion

    #region In Book UI

    public static readonly CustomInputAction inBookUIBackOut = new CustomInputAction(KeyBindingList.backOutKey.getCurrentKeyCode(),
                                                                                    onStarted: onInBookUIBackOutKeyPress);

    private static void onInBookUIBackOutKeyPress(InputAction.CallbackContext context)
    {
        backOutOfBookUI();
    }

    public static readonly CustomInputAction inBookUIOpenSettings = new CustomInputAction(KeyBindingList.settingsScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onInBookUIOpenSettingsKeyPress);

    private static void onInBookUIOpenSettingsKeyPress(InputAction.CallbackContext context)
    {
        backOutOfBookUI();
    }

    public static readonly CustomInputAction inBookUIInteract = new CustomInputAction(KeyBindingList.interactKey.getCurrentKeyCode(),
                                                                                    onStarted: onInBookUIInteractKeyPress);

    private static void onInBookUIInteractKeyPress(InputAction.CallbackContext context)
    {
        closeBookUI();
    }

    public static readonly CustomInputAction inBookUIAccept = new CustomInputAction(KeyBindingList.acceptKey.getCurrentKeyCode(),
                                                                                    onStarted: onInBookUIAcceptKeyPress);

    private static void onInBookUIAcceptKeyPress(InputAction.CallbackContext context)
    {
        closeBookUI();
    }

    public static readonly CustomInputAction inBookUIAcceptInput = new CustomInputAction(KeyBindingList.acceptInputKey.getCurrentKeyCode(),
                                                                                    onStarted: onInBookUIAcceptInputKeyPress);

    private static void onInBookUIAcceptInputKeyPress(InputAction.CallbackContext context)
    {
        closeBookUI();
    }

    #endregion

    #region In Shop UI

    public static readonly CustomInputAction inShopUIBackOut = new CustomInputAction(KeyBindingList.backOutKey.getCurrentKeyCode(),
                                                                                    onStarted: onInShopUIBackOutKeyPress);

    private static void onInShopUIBackOutKeyPress(InputAction.CallbackContext context)
    {
        backOutToWalking();
    }

    public static readonly CustomInputAction inShopUIOpenSettings = new CustomInputAction(KeyBindingList.settingsScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onInShopUIOpenSettingsKeyPress);

    private static void onInShopUIOpenSettingsKeyPress(InputAction.CallbackContext context)
    {
        backOutToWalking();
    }

    #endregion

    #region In Dialogue PopUp

    public static readonly CustomInputAction inDialoguePopUpBackOut = new CustomInputAction(KeyBindingList.backOutKey.getCurrentKeyCode(),
                                                                                    onStarted: onInDialoguePopUpBackOutKeyPress);

    private static void onInDialoguePopUpBackOutKeyPress(InputAction.CallbackContext context)
    {
        backOutToWalking();
    }

    public static readonly CustomInputAction inDialoguePopUpOpenSettings = new CustomInputAction(KeyBindingList.settingsScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onInDialoguePopUpOpenSettingsKeyPress);

    private static void onInDialoguePopUpOpenSettingsKeyPress(InputAction.CallbackContext context)
    {
        backOutToWalking();
    }

    //No notification sweep: the transcript key closes the transcript it opened through its own disjunct
    //(handleDialoguePopUpStateKeyPresses, PlayerInput.cs:609), not through the back-key predicate.
    public static readonly CustomInputAction inDialoguePopUpOpenTranscript = new CustomInputAction(KeyBindingList.transcriptKey.getCurrentKeyCode(),
                                                                                    onStarted: onInDialoguePopUpOpenTranscriptKeyPress);

    private static void onInDialoguePopUpOpenTranscriptKeyPress(InputAction.CallbackContext context)
    {
        closePopUpToWalking();
    }

    #endregion

    #region In Level Up PopUp

    //No per-state actions - showFormula is the only key this state accepts, and it is state independent
    //(handleLevelUpPopUpStateKeyPresses, PlayerInput.cs:619-622).

    #endregion

    #region In Tutorial PopUp

    //Dead on arrival: nothing in Assets/src calls setCurrentActivity(OOCActivity.inTutorialPopUp), so this
    //state is currently unreachable. Written from handleTutorialPopUpStateKeyPresses (PlayerInput.cs:624-634)
    //so the set is already right when a tutorial pop-up button starts setting the state.

    public static readonly CustomInputAction inTutorialPopUpBackOut = new CustomInputAction(KeyBindingList.backOutKey.getCurrentKeyCode(),
                                                                                    onStarted: onInTutorialPopUpBackOutKeyPress);

    private static void onInTutorialPopUpBackOutKeyPress(InputAction.CallbackContext context)
    {
        backOutToWalking();
    }

    public static readonly CustomInputAction inTutorialPopUpOpenSettings = new CustomInputAction(KeyBindingList.settingsScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onInTutorialPopUpOpenSettingsKeyPress);

    private static void onInTutorialPopUpOpenSettingsKeyPress(InputAction.CallbackContext context)
    {
        backOutToWalking();
    }

    #endregion

    #region In World Map

    public static readonly CustomInputAction inWorldMapBackOut = new CustomInputAction(KeyBindingList.backOutKey.getCurrentKeyCode(),
                                                                                    onStarted: onInWorldMapBackOutKeyPress);

    private static void onInWorldMapBackOutKeyPress(InputAction.CallbackContext context)
    {
        backOutOfWorldMap();
    }

    public static readonly CustomInputAction inWorldMapOpenSettings = new CustomInputAction(KeyBindingList.settingsScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onInWorldMapOpenSettingsKeyPress);

    private static void onInWorldMapOpenSettingsKeyPress(InputAction.CallbackContext context)
    {
        backOutOfWorldMap();
    }

    public static readonly CustomInputAction inWorldMapOpenWorldMap = new CustomInputAction(KeyBindingList.worldMapKey.getCurrentKeyCode(),
                                                                                    onStarted: onInWorldMapOpenWorldMapKeyPress);

    private static void onInWorldMapOpenWorldMapKeyPress(InputAction.CallbackContext context)
    {
        PlayerObject.getWorldMapPopUpButton().destroyPopUp();
    }

    public static readonly CustomInputAction inWorldMapOpenMap = new CustomInputAction(KeyBindingList.mapKey.getCurrentKeyCode(),
                                                                                    onStarted: onInWorldMapOpenMapKeyPress);

    private static void onInWorldMapOpenMapKeyPress(InputAction.CallbackContext context)
    {
        PlayerObject.getWorldMapPopUpButton().destroyPopUp();
        PlayerObject.getMapPopUpButton().spawnPopUp();
    }

    #endregion

    #region Main Menu

    public static readonly CustomInputAction mainMenuOpenSettings = new CustomInputAction(KeyBindingList.settingsScreenKey.getCurrentKeyCode(),
                                                                                    onStarted: onMainMenuOpenSettingsKeyPress);

    private static void onMainMenuOpenSettingsKeyPress(InputAction.CallbackContext context)
    {
        backOutOfUI();

        if (StartingMenuManager.getInstance() != null)
        {
            StartingMenuManager.getInstance().revertToMainMenu();
        }
    }

    #endregion

    #endregion

    #region State Input Sets

    public static void ensureInitialized()
    {
    }

    //One method per OOCActivity, in the order PlayerOOCStateManager.updateEnabledInputActions switches on
    //them. Every action named below belongs to exactly one of these methods - the only exceptions are
    //toggleTerrain, toggleKeyBindingsList and showFormula, whose handlers cannot reach setCurrentActivity
    //and so cannot be disabled and re-enabled from inside their own callback.

    public static void enableWalkingInputActions()
    {
        moveNorth.enabled = true;
        moveWest.enabled = true;
        moveSouth.enabled = true;
        moveEast.enabled = true;

        walkingInteract.enabled = true;
        walkingBackOut.enabled = true;
        toggleTerrain.enabled = true;
        toggleReveal.enabled = true;
        removePlacedObject.enabled = true;
        quickSave.enabled = true;
        walkingOpenTranscript.enabled = true;
        toggleKeyBindingsList.enabled = true;

        walkingUseSkill.enabled = true;
        cycleSkillAscending.enabled = true;
        cycleSkillDescending.enabled = true;

        walkingOpenLastScreen.enabled = true;
        walkingOpenCharacter.enabled = true;
        walkingOpenInventory.enabled = true;
        walkingOpenParty.enabled = true;
        walkingOpenJournal.enabled = true;
        walkingOpenSaveAndLoad.enabled = true;
        walkingOpenSettings.enabled = true;
        walkingOpenMap.enabled = true;
        walkingOpenWorldMap.enabled = true;
    }

    public static void enableDialogueInputActions()
    {
        toggleTerrain.enabled = true;

        inDialogueInteract.enabled = true;
        inDialogueAccept.enabled = true;
        inDialogueAcceptInput.enabled = true;

        chooseDialogueOption1.enabled = true;
        chooseDialogueOption2.enabled = true;
        chooseDialogueOption3.enabled = true;
        chooseDialogueOption4.enabled = true;
        chooseDialogueOption5.enabled = true;
        chooseDialogueOption6.enabled = true;
        chooseDialogueOption7.enabled = true;
        chooseDialogueOption8.enabled = true;
    }

    public static void enableUIInputActions()
    {
        showFormula.enabled = true;
        inUIBackOut.enabled = true;
        toggleKeyBindingsList.enabled = true;

        inUIOpenLastScreen.enabled = true;
        inUIOpenCharacter.enabled = true;
        inUIOpenInventory.enabled = true;
        inUIOpenParty.enabled = true;
        inUIOpenJournal.enabled = true;
        inUIOpenSaveAndLoad.enabled = true;
        inUIOpenSettings.enabled = true;

        moveScreenLeft.enabled = true;
        moveScreenRight.enabled = true;
    }

    public static void enableMapInputActions()
    {
        inMapBackOut.enabled = true;
        inMapOpenSettings.enabled = true;
        inMapOpenMap.enabled = true;
        inMapOpenWorldMap.enabled = true;
    }

    public static void enableCunningInputActions()
    {
        cunningMoveNorth.enabled = true;
        cunningMoveWest.enabled = true;
        cunningMoveSouth.enabled = true;
        cunningMoveEast.enabled = true;

        cunningInteract.enabled = true;
        cunningBackOut.enabled = true;
        cunningOpenSettings.enabled = true;
        cunningUseSkill.enabled = true;
    }

    public static void enableObservingInputActions()
    {
        observingBackOut.enabled = true;
        observingOpenSettings.enabled = true;
        observingUseSkill.enabled = true;
    }

    //No movement actions: IntimidateManager has no selector to move - executeSkill sweeps the whole grid and
    //setSelectorOriginTile is empty (IntimidateManager.cs:143-146).
    public static void enableIntimidatingInputActions()
    {
        intimidatingInteract.enabled = true;
        intimidatingBackOut.enabled = true;
        intimidatingOpenSettings.enabled = true;
        intimidatingUseSkill.enabled = true;
    }

    public static void enableChestInputActions()
    {
        showFormula.enabled = true;

        inChestUIBackOut.enabled = true;
        inChestUIOpenSettings.enabled = true;
        inChestUIInteract.enabled = true;
        inChestUIAccept.enabled = true;
        inChestUIAcceptInput.enabled = true;
    }

    public static void enableBookInputActions()
    {
        inBookUIBackOut.enabled = true;
        inBookUIOpenSettings.enabled = true;
        inBookUIInteract.enabled = true;
        inBookUIAccept.enabled = true;
        inBookUIAcceptInput.enabled = true;
    }

    public static void enableShopInputActions()
    {
        showFormula.enabled = true;

        inShopUIBackOut.enabled = true;
        inShopUIOpenSettings.enabled = true;

        //maxAmount and multiplyByTenAmount stay off: their handlers are no-ops, and AmountPanel reads those
        //keys directly while handling a button click.
    }

    public static void enableDialoguePopUpInputActions()
    {
        inDialoguePopUpBackOut.enabled = true;
        inDialoguePopUpOpenSettings.enabled = true;
        inDialoguePopUpOpenTranscript.enabled = true;
    }

    public static void enableLevelUpPopUpInputActions()
    {
        showFormula.enabled = true;
    }

    //Dead until something sets OOCActivity.inTutorialPopUp - no call site in Assets/src does. Kept wired so
    //the set is already correct when a tutorial pop-up button starts setting the state.
    public static void enableTutorialPopUpInputActions()
    {
        inTutorialPopUpBackOut.enabled = true;
        inTutorialPopUpOpenSettings.enabled = true;
    }

    public static void enableWorldMapInputActions()
    {
        inWorldMapBackOut.enabled = true;
        inWorldMapOpenSettings.enabled = true;
        inWorldMapOpenMap.enabled = true;
        inWorldMapOpenWorldMap.enabled = true;
    }

    //No mainMenuBackOut: StartingMenuManager.Update (StartingMenuManager.cs:44) still polls
    //settingsScreenOrBackKeyPressed and serves the back key itself.
    public static void enableMainMenuInputActions()
    {
        mainMenuOpenSettings.enabled = true;
    }

    #endregion

}

public class CustomInputAction
{
    public readonly static UnityEvent DisableAllInputActions = new UnityEvent();

    private InputAction input;

    public bool enabled
    {
        set
        {
            if(value && !input.enabled)
            {
                input.Enable();

                if(onStarted != null){ input.started += onStarted; }
                if(onCanceled != null){ input.canceled += onCanceled; }
                if(onPerformed != null){ input.performed += onPerformed; }
                
            } else if(!value && input.enabled)
            {
                input.Disable();

                input.started -= onStarted;
                input.canceled -= onCanceled;
                input.performed -= onPerformed;
            }
        }
        get
        {
            return input.enabled;
        }
    }

    private Action<InputAction.CallbackContext> onStarted;
    private Action<InputAction.CallbackContext> onCanceled;
    private Action<InputAction.CallbackContext> onPerformed;

    public CustomInputAction(KeyCode keyCode,
                                Action<InputAction.CallbackContext> onStarted = null,
                                Action<InputAction.CallbackContext> onCanceled = null,
                                Action<InputAction.CallbackContext> onPerformed = null)
    {
        input = buildInputAction(keyCode);
        this.onStarted = onStarted;
        this.onCanceled = onCanceled;
        this.onPerformed = onPerformed;

        DisableAllInputActions.AddListener(() => enabled = false);
    }

    public void rebind(KeyCode keyCode)
    {
        bool enable = input.enabled;

        input.started -= onStarted;
        input.canceled -= onCanceled;
        input.performed -= onPerformed;
        input.Disable();
        input.Dispose();

        input = buildInputAction(keyCode);
        input.started += onStarted;
        input.canceled += onCanceled;
        input.performed += onPerformed;
        
        if(enable)
        {
            input.Enable();
        }
    }

    //An InputAction whose binding path resolves to no controls never fires and never complains, so an
    //unmappable KeyCode is reported here rather than turning into an input that silently does nothing.
    private static InputAction buildInputAction(KeyCode keyCode)
    {
        string keyboardPath = keyCode.ToKeyboardPath();

        if (keyboardPath == null)
        {
            Debug.LogError("KeyCode has no Input System equivalent: " + keyCode.ToString());
        }

        return new InputAction(type: InputActionType.Button, binding: keyboardPath);
    }
}