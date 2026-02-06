using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    private List<KeyCode> barredMovementKeyCodes = new List<KeyCode>(); //key codes that are not able to be used because you have selected
                                                                //multiple keys at once. Pressing W, then while W is pressed also
                                                                //pressing A should stop accepting W as an input and allow A.

    private KeyCode currentMovementKeyCode = KeyCode.None;

    void Update() 
    {
        KeyPressManager.updateKeyBools();

        if (PlayerObject.onTopOfTransitionOrTutorial())
        {
            return;
        }

        if (KeyBindingList.eitherBackoutKeyIsPressed() && PlayerOOCStateManager.currentActivity != OOCActivity.inChestUI)
        {
            if (NotificationManager.getCurrentNotificationPopUpWindowGameObject() != null &&
                 !KeyPressManager.handlingPrimaryKeyPress)
            {
                NotificationManager.OnDeleteAllNotifications.Invoke();
                KeyPressManager.handlingPrimaryKeyPress = true;
            }
        }

        if ((KeyPressManager.handlingPrimaryKeyPress && PlayerOOCStateManager.currentActivity != OOCActivity.inChestUI &&
                                                PlayerOOCStateManager.currentActivity != OOCActivity.inTutorialSequence)
        || FadeToBlackManager.isMidFade())
        {
            return;
        }

        if (!KeyPressManager.handlingPrimaryKeyPress || PlayerOOCStateManager.currentActivity == OOCActivity.inTutorialSequence)
        {
            switch (PlayerOOCStateManager.currentActivity)
            {
                case OOCActivity.walking:
                    handleWalkingStateKeyPresses();
                    break;
                case OOCActivity.inDialogue:
                    handleDialogueStateKeyPresses();
                    break;
                case OOCActivity.inUI:
                    handleUIStateKeyPresses();
                    break;
                case OOCActivity.inMap:
                    handleMapStateKeyPresses();
                    break;
                case OOCActivity.cunning:
                    handleCunningStateKeyPresses();
                    break;
                case OOCActivity.observing:
                    handleObservingStateKeyPresses();
                    break;
                case OOCActivity.intimidating:
                    handleIntimidateStateKeyPresses();
                    break;
                case OOCActivity.inChestUI:
                    handleChestStateKeyPresses();
                    break;
                case OOCActivity.inBookUI:
                    handleBookStateKeyPresses();
                    break;
                case OOCActivity.inShopUI:
                    handleShopStateKeyPresses();
                    break;
                case OOCActivity.inDialoguePopUp:
                    handleDialoguePopUpStateKeyPresses();
                    break;
                case OOCActivity.inLevelUpPopUp:
                    handleLevelUpPopUpStateKeyPresses();
                    break;
                case OOCActivity.inTutorialPopUp:
                    handleTutorialPopUpStateKeyPresses();
                    break;
                case OOCActivity.inTutorialSequence:
                    handleTutorialSequenceStateKeyPresses();
                    break;
                case OOCActivity.inWorldMap:
                    handleWorldMapStateKeyPresses();
                    break;
                case OOCActivity.inFade:
                case OOCActivity.preCombat:
                    return;
                default:
                    Debug.LogError("Unrecognized OOCActivity: " + PlayerOOCStateManager.currentActivity.ToString());
                    break;
            }
        }
    }

    private void handleWalkingStateKeyPresses()
    {
        if (handleWASDMovement())
        {
            return;
        }

        if (Input.GetKey(KeyBindingList.showHideKeyBindingsListKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            CombatInputManager.OnHideKeyBindingsList.Invoke();
        }

        if (KeyBindingList.quickLoadKeysPressed() && !KeyPressManager.handlingPrimaryKeyPress)
        {
            SaveHandler.quickLoadTopSave();

            KeyPressManager.handlingPrimaryKeyPress = true;
        }

        if (Input.GetKey(KeyBindingList.quicksaveKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            SaveHandler.quickSave();

            KeyPressManager.handlingPrimaryKeyPress = true;
        }

        toggleTerrainKeyCheck();

        if (Input.GetKey(KeyBindingList.interactKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.handlingPrimaryKeyPress = true;

            if (PlayerMovement.canInteract())
            {
                interact();
            }

        }

        if (Input.GetKey(KeyBindingList.skillKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            SkillButtonManager.useSkill();
            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        if (Input.GetKey(KeyBindingList.cycleSkillAscendingKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            SkillButtonManager.changeSkill(false);
            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        if (Input.GetKey(KeyBindingList.cycleSkillDecendingKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            SkillButtonManager.changeSkill(true);
            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        if (Input.GetKey(KeyBindingList.mapKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            PlayerObject.getMapPopUpButton().spawnPopUp();

            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        if (Input.GetKey(KeyBindingList.worldMapKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            PlayerObject.getWorldMapPopUpButton().spawnPopUp();

            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        if (Input.GetKey(KeyBindingList.transcriptKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            DialogueManager.getInstance().spawnDialogueTrackerWindowWithoutChoices();

            KeyPressManager.handlingPrimaryKeyPress = true;
        }

        if (Input.GetKey(KeyBindingList.removePlacedCompanionMovableObjectKey) && !KeyPressManager.handlingPrimaryKeyPress)
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

        if (KeyBindingList.revealKeyIsPressed() && !KeyPressManager.handlingSecondaryKeyPress)
        {
            RevealManager.toggleReveal();
            KeyPressManager.handlingSecondaryKeyPress = true;
        }

        if (Input.GetKey(KeyBindingList.lastScreenKey) && !KeyPressManager.handlingPrimaryKeyPress)
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
        if (Input.GetKey(KeyBindingList.characterScreenKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.escapeAll();

            SideScreenButtonManager.getInstance().setCurrentScreenType(ScreenType.Character);

            KeyPressManager.handlingPrimaryKeyPress = true;

            return;
        }

        if (Input.GetKey(KeyBindingList.inventoryScreenKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.escapeAll();

            SideScreenButtonManager.getInstance().setCurrentScreenType(ScreenType.Inventory);

            KeyPressManager.handlingPrimaryKeyPress = true;

            return;
        }

        if (Input.GetKey(KeyBindingList.partyScreenKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.escapeAll();

            SideScreenButtonManager.getInstance().setCurrentScreenType(ScreenType.Party);

            KeyPressManager.handlingPrimaryKeyPress = true;

            return;
        }

        if (Input.GetKey(KeyBindingList.journalScreenKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.escapeAll();

            SideScreenButtonManager.getInstance().setCurrentScreenType(ScreenType.Journal);

            KeyPressManager.handlingPrimaryKeyPress = true;

            return;
        }

        if (Input.GetKey(KeyBindingList.loadScreenKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.escapeAll();

            SideScreenButtonManager.getInstance().setCurrentScreenType(ScreenType.SaveAndLoad);

            KeyPressManager.handlingPrimaryKeyPress = true;

            return;
        }

        if (KeyBindingList.settingsScreenKeyKeyIsPressed() && !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.escapeAll();

            SideScreenButtonManager.getInstance().setCurrentScreenType(ScreenType.Settings);

            KeyPressManager.handlingPrimaryKeyPress = true;

            return;
        }
    }

    private void handleDialogueStateKeyPresses()
    {
        if (KeyBindingList.continueStoryKeyIsPressed()
            && DialogueManager.getInstance().storyCanContinue())
        {
            if (!DialogueManager.getInstance().getDialogue().random)
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

        if (KeyBindingList.eitherBackoutKeyIsPressed() && 
            EscapeStack.getEscapableObjectsCount() > 0 &&
            !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.handleEscapePress();

            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }
        else if ((((Input.GetKey(OverallUIManager.getCurrentScreenExitKey()) || KeyBindingList.eitherBackoutKeyIsPressed()) && 
                    !SaveHandler.saveNameFieldIsSelected()) || 
                    Input.GetKey(KeyBindingList.lastScreenKey)) && 
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

        if(KeyBindingList.screenNavigationbuttonIsPressed() && !KeyPressManager.handlingPrimaryKeyPress)
        {
            handleScreenSelection();
            return;
        }

        if(SaveHandler.saveNameFieldIsSelected() && 
            Input.GetKey(KeyBindingList.backOutKey1) && 
            !KeyPressManager.handlingPrimaryKeyPress)
        {
            EventSystem.current.SetSelectedGameObject(null);
            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        // bool passedbackOutCheck = false;

        // switch (OverallUIManager.lastScreenType)
        // {
        //     case ScreenType.Character:
        //         passedbackOutCheck = backOutCheck(KeyBindingList.characterScreenKey);
        //         break;
        //     case ScreenType.Inventory:
        //         passedbackOutCheck = backOutCheck(KeyBindingList.inventoryScreenKey);
        //         break;
        //     case ScreenType.Party:
        //         passedbackOutCheck = backOutCheck(KeyBindingList.partyScreenKey);
        //         break;
        //     case ScreenType.Journal:
        //         passedbackOutCheck = backOutCheck(KeyBindingList.journalScreenKey);
        //         break;
        //     case ScreenType.SaveAndLoad:
        //         if (  backOutCheck(KeyBindingList.loadScreenKey))
        //         {
        //             return;
        //         }
        //         break;
        //     default:
        //         break;
        // }

        // if (passedbackOutCheck)
        // {
        //     return;
        // }

        if (Input.GetKey(KeyBindingList.moveLeftKey) && !SaveHandler.saveNameFieldIsSelected() && !KeyPressManager.handlingPrimaryKeyPress)
        {
            OverallUIManager.moveToScreenToTheLeft();

            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        if (Input.GetKey(KeyBindingList.moveRightKey) && !SaveHandler.saveNameFieldIsSelected() && !KeyPressManager.handlingPrimaryKeyPress)
        {
            OverallUIManager.moveToScreenToTheRight();

            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

    }


    private bool backOutCheck(KeyCode keyCode)
    {
        ScreenType correspondingScreen = KeyBindingList.getScreenType(keyCode);

        if (Input.GetKey(keyCode) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            if ((keyCode == KeyBindingList.loadScreenKey) && SaveHandler.saveNameFieldIsSelected())
            {
                return false;
            }

            backOutOfUI();
            return true;
        }

        return false;
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
        if (MapPopUpWindow.hasFastTravelTarget() && KeyBindingList.eitherBackoutKeyIsPressed())
        {
            MapPopUpWindow.fastTravelPanelCloseButtonPress();

            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }
        else if (!MapPopUpWindow.hasFastTravelTarget() && Input.GetKey(KeyBindingList.mapKey) || KeyBindingList.eitherBackoutKeyIsPressed())
        {

            PlayerObject.getMapPopUpButton().destroyPopUp();

            KeyPressManager.handlingPrimaryKeyPress = true;
        }else if(!MapPopUpWindow.hasFastTravelTarget() && Input.GetKey(KeyBindingList.worldMapKey))
        {
            PlayerObject.getMapPopUpButton().destroyPopUp();
            PlayerObject.getWorldMapPopUpButton().spawnPopUp();

            KeyPressManager.handlingPrimaryKeyPress = true;
        }
    }

    private void handleWorldMapStateKeyPresses()
    {
        if (Input.GetKey(KeyBindingList.worldMapKey) || KeyBindingList.eitherBackoutKeyIsPressed())
        {
            PlayerObject.getWorldMapPopUpButton().destroyPopUp();

            KeyPressManager.handlingPrimaryKeyPress = true;
        } else if(Input.GetKey(KeyBindingList.mapKey))
        {
            PlayerObject.getWorldMapPopUpButton().destroyPopUp();
            PlayerObject.getMapPopUpButton().spawnPopUp();

            KeyPressManager.handlingPrimaryKeyPress = true;
        }
    }

    private void handleCunningStateKeyPresses()
    {
        if ((KeyBindingList.eitherBackoutKeyIsPressed() || Input.GetKey(KeyBindingList.skillKey)) && !KeyPressManager.handlingPrimaryKeyPress)
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

        if (Input.GetKey(KeyBindingList.interactKey) && !KeyPressManager.handlingPrimaryKeyPress)
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
        if ((KeyBindingList.eitherBackoutKeyIsPressed() || Input.GetKey(KeyBindingList.skillKey)) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            ObservationManager.leaveObservationMode();
            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }
    }

    private void handleIntimidateStateKeyPresses()
    {
        if ((KeyBindingList.eitherBackoutKeyIsPressed() || Input.GetKey(KeyBindingList.skillKey)) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            IntimidateManager.leaveIntimidateMode();
            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

        if (Input.GetKey(KeyBindingList.interactKey) && !KeyPressManager.handlingPrimaryKeyPress)
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
        if ((KeyBindingList.eitherBackoutKeyIsPressed() || Input.GetKey(KeyBindingList.interactKey)) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
            return;
        }
    }

    private void handleBookStateKeyPresses()
    {
        if ((KeyBindingList.eitherBackoutKeyIsPressed() || KeyBindingList.continueUIKeyIsPressed())
                && !KeyPressManager.handlingPrimaryKeyPress)
        {
            //BookManager.getInstance().deactivate();

            EscapeStack.escapeAll();

            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }
    }

    private void handleShopStateKeyPresses()
    {
        showFormulaToggleCheck();

        if (KeyBindingList.eitherBackoutKeyIsPressed() && !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.escapeAll();

            KeyPressManager.handlingPrimaryKeyPress = true;
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
            return;
        }
    }

    private void handleDialoguePopUpStateKeyPresses()
    {
        if ((Input.GetKey(KeyBindingList.transcriptKey) || KeyBindingList.eitherBackoutKeyIsPressed()) && !KeyPressManager.handlingPrimaryKeyPress)
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
        if (KeyBindingList.eitherBackoutKeyIsPressed() && !KeyPressManager.handlingPrimaryKeyPress)
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

    public bool handleWASDMovement()
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
            switch (currentMovementKeyCode)
            {
                case KeyBindingList.moveNorthKey:
                    PlayerMovement.adjustPlayerDirectionalMod(MovementManager.distance1TileNorthEastGrid);
                    break;

                case KeyBindingList.moveWestKey:
                    PlayerMovement.adjustPlayerDirectionalMod(MovementManager.distance1TileNorthWestGrid);
                    break;

                case KeyBindingList.moveSouthKey:
                    PlayerMovement.adjustPlayerDirectionalMod(MovementManager.distance1TileSouthWestGrid);
                    break;

                case KeyBindingList.moveEastKey:
                    PlayerMovement.adjustPlayerDirectionalMod(MovementManager.distance1TileSouthEastGrid);
                    break;
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
                PlayerOOCStateManager.setCurrentActivity(OOCActivity.inDialogue);
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
            Chest currentChest = chestCollider.gameObject.GetComponent<Chest>();

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
    
    public static void toggleTerrainKeyCheck()
    {
        if (Input.GetKey(KeyBindingList.hideTerrainKey) && !KeyPressManager.handlingSecondaryKeyPress)
        {
            TerrainVisibilityManager.toggleTerrainVisibility();

            KeyPressManager.handlingSecondaryKeyPress = true;
        }
    }

    public static void showFormulaToggleCheck()
    {
        if (KeyBindingList.eitherAltKeyIsPressed() && !OverallUIManager.showFormula)
        {
            OverallUIManager.showFormula = true;
            DescriptionPanelBuilder.OnFormulaSwap.Invoke();
            KeyPressManager.handlingPrimaryKeyPress = true;
        }
        else if (!KeyBindingList.eitherAltKeyIsPressed() && OverallUIManager.showFormula)
        {
            OverallUIManager.showFormula = false;
            DescriptionPanelBuilder.OnFormulaSwap.Invoke();
        }
    }
}
