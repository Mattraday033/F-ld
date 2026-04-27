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
        if(KeyBindingSettingsManager.listeningForKeyBinding() || InspectNode.inspecting)
        {
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

        if ((KeyPressManager.handlingPrimaryKeyPress && PlayerOOCStateManager.currentActivity != OOCActivity.inChestUI &&
                                                PlayerOOCStateManager.currentActivity != OOCActivity.inTutorialSequence)
        || FadeToBlackManager.isMidScreenFade() || !FadeToBlackManager.getInstance().fadeToBlackImage.color.Equals(Color.clear))
        {
            return;
        }

        if (Input.GetKey(KeyBindingList.showHideKeyBindingsListKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            CombatInputManager.OnHideKeyBindingsList.Invoke();
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
                case OOCActivity.Defeat:
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
            EscapeStack.escapeAll();

            SideScreenButtonManager.getInstance().setCurrentScreenType(ScreenType.Character);

            KeyPressManager.handlingPrimaryKeyPress = true;

            return;
        }

        if (Input.GetKey(KeyBindingList.inventoryScreenKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.escapeAll();

            SideScreenButtonManager.getInstance().setCurrentScreenType(ScreenType.Inventory);

            KeyPressManager.handlingPrimaryKeyPress = true;

            return;
        }

        if (Input.GetKey(KeyBindingList.partyScreenKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.escapeAll();

            SideScreenButtonManager.getInstance().setCurrentScreenType(ScreenType.Party);

            KeyPressManager.handlingPrimaryKeyPress = true;

            return;
        }

        if (Input.GetKey(KeyBindingList.journalScreenKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.escapeAll();

            SideScreenButtonManager.getInstance().setCurrentScreenType(ScreenType.Journal);

            KeyPressManager.handlingPrimaryKeyPress = true;

            return;
        }

        if (Input.GetKey(KeyBindingList.loadScreenKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            EscapeStack.escapeAll();

            SideScreenButtonManager.getInstance().setCurrentScreenType(ScreenType.SaveAndLoad);

            KeyPressManager.handlingPrimaryKeyPress = true;

            return;
        }

        if (Input.GetKey(KeyBindingList.settingsScreenKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
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
            DescriptionPanelBuilder.OnFormulaSwap.Invoke();
            KeyPressManager.handlingPrimaryKeyPress = true;
        }
        else if (!Input.GetKey(KeyBindingList.showFormulaKey.getCurrentKeyCode()) && OverallUIManager.showFormula)
        {
            OverallUIManager.showFormula = false;
            DescriptionPanelBuilder.OnFormulaSwap.Invoke();
        }
    }
}
