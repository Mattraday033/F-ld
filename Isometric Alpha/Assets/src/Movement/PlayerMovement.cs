using System.IO;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
	public static bool hasCustomPromptMessage = false;

	private float debugMessageTimer = 0f;
	public bool useMapStartLocation;
	public bool loadingStartLocationFromTransition;

	public GameObject pressButtonPrompt;
	public TextMeshProUGUI pressButtonPromptText;

	public Vector3Int directionalModifierGrid;

	public bool colliderNotDetected = false;

	public float timeToMove = 0.2f;
	public static float detectionSize = .05f;
	public Transform UIParent;

	public DialogueManager dialogueManager;
	public DialogueTrigger dialogueTrigger;
	public LessonUIManager lessonUIManager;

	public TutorialPopUpButton tutorialPopUpButton;
	public MapPopUpButton mapPopUpButton;

	public Animator animator;

	public CapsuleCollider2D terrainCollider;
	public SpriteMask spriteMask;

	public TilemapRenderer[] terrainTilemaps;
	public SpriteRenderer[] terrainSprites;

	public Chest currentChest;
	public Collider2D transitionCollider;

	private bool enteredCombat = false;
	private NPCCombatInfo npcCombatInfo = null;
	private static PlayerMovement instance;

	public SpriteRenderer playerSpriteRenderer;

	public ObservationManager observationManager;
	public CunningManager cunningManager;
	public IntimidateManager intimidateManager;

	private ArrayList barredMovementKeyCodes = new ArrayList(); //key codes that are not able to be used because you have selected
																//multiple keys at once. Pressing W, then while W is pressed also
																//pressing A should stop accepting W as an input and allow A.

	private KeyCode currentMovementKeyCode = KeyCode.None;

	private const string animationStatePrefix = "MC"; //Main Character
	private const string baseLayerName = "Base Layer";
	private const string run = "Run";
	private const string idle = "Idle";

	private string direction = "SW";
	private string runOrIdle = "Idle";

	public static int testCounter = 1;

	private void Awake()
	{
		instance = this;

        setAsCameraTarget();

        findTerrainObjects();

		if (State.terrainHidden)
		{
			setTerrainActive(false);
		}

		if (State.playerFacing == null)
		{
			State.playerFacing = new CharacterFacing();
		}
	}

	private void setAsCameraTarget()
	{
		CinemachineVirtualCamera mainCM = GameObject.FindWithTag("MainVirtualCamera").GetComponent<CinemachineVirtualCamera>();
        mainCM.m_Follow = PlayerMovement.getInstance().gameObject.transform;
	}


    public static PlayerMovement getInstance()
    {
        return instance;
    }

	public static Transform getTransform()
	{
		if (instance == null)
		{
			return null;
		}

		return instance.gameObject.transform;
	}

	public static Transform getUIParentTransform()
	{
		return instance.UIParent;
	}

	void Start()
	{
        // if (TransitionManager.hasASourceTransition())
        // {
        // 	transform.position = translateTransitionDestinationToWorldPosition();

        // 	if (playerSpriteRenderer != null && TransitionManager.hasASortingLayer())
        // 	{
        // 		playerSpriteRenderer.sortingLayerName = State.currentSourceTransitionInfo.sortingLayerName;
        // 	}

        // 	TransitionManager.resetCurrentSourceTransition();
        // }
        // else
        // {
        // 	loadingStartLocationFromTransition = SceneTransitionPosition.hasPositionToUse;


        // 	if (loadingStartLocationFromTransition)
        // 	{
        // 		transform.position = SceneTransitionPosition.bankedPosition.oldPosition;

        // 		if (SceneTransitionPosition.sortingLayer != null && SceneTransitionPosition.sortingLayer.Length > 0)
        // 		{
        // 			playerSpriteRenderer.sortingLayerName = SceneTransitionPosition.sortingLayer;
        // 			SceneTransitionPosition.sortingLayer = null;
        // 		}

        // 		//if(SceneTransitionPosition.characterFacing != null) //commented out when removing CharacterFacing scriptable object and transitioning to soley
        // 		//{													  //using State.playerFacing because this set the facing to something random
        // 		//	State.playerFacing.setFacing(SceneTransitionPosition.characterFacing.getFacing());
        // 		//}

        // 	}
        // 	else if (Flags.flags["newGame"])
        // 	{
        // 		useMapStartLocation = true;
        // 	}
        // 	else if (!useMapStartLocation && !loadingStartLocationFromTransition)
        // 	{
        // 		transform.localPosition = startingPosition.oldPosition;
        // 	}
        // 	else
        // 	{
        // 		useMapStartLocation = false;
        // 	}
        // }

        // loadingStartLocationFromTransition = false;

        MovementManager.getInstance().addPlayerSprite(transform);

		// transform.localPosition = MovementManager.getInstance().grid.GetCellCenterLocal(MovementManager.getInstance().grid.LocalToCell(transform.localPosition));

		if (dialogueManager == null)
		{
			dialogueManager = DialogueManager.getInstance();
		}

		if (lessonUIManager == null)
		{
			lessonUIManager = LessonUIManager.getInstance();
		}

		transitionCollider = GetComponent<CircleCollider2D>();

		adjustDirectionalModifierGrid();

		intimidateManager = new IntimidateManager();
		cunningManager = new CunningManager();
		observationManager = new ObservationManager();

		adjustAnimator(true);

		if (PlayerOOCStateManager.currentActivity != OOCActivity.inDialogue &&
            PlayerOOCStateManager.currentActivity != OOCActivity.inTutorialSequence)
		{
			PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
		}
	}

	void Update() //here for Key Input
	{
		// printDebugMessages();

		testCounter++;

		KeyPressManager.updateKeyBools();

		if (onTopOfTransitionOrTutorial())
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

		if (enteredCombat && FadeToBlackManager.isBlack())
		{
			prepCombat();
		}

		if ((KeyPressManager.handlingPrimaryKeyPress && PlayerOOCStateManager.currentActivity != OOCActivity.inChestUI &&
												PlayerOOCStateManager.currentActivity != OOCActivity.inTutorialSequence)
		|| !FadeToBlackManager.isTransparent())
		//|| AdjustPartyRosterManager.isInPartySelection || lessonUIManager.isLearningLesson)
		{
			return;
		}

		//for checking whether you need to display behind a building
		if (Helpers.hasCollision(terrainCollider, LayerAndTagManager.terrainLayerMask))
		{
			hideTerrain();
		}
		else
		{
			displayTerrain();
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
				default:
					Debug.LogError("Unrecognized OOCActivity: " + PlayerOOCStateManager.currentActivity.ToString());
					break;
			}
		}

		adjustAnimator();
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
			OOCKeyBindingListDisplayManager.toggleKeyBindingList();
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

		if (Input.GetKey(KeyBindingList.hideTerrainKey) && !KeyPressManager.handlingSecondaryKeyPress)
		{
			if (State.terrainHidden)
			{
				setTerrainActive(true);
			}
			else
			{
				setTerrainActive(false);
			}

			KeyPressManager.handlingSecondaryKeyPress = true;
		}

		if (Input.GetKey(KeyBindingList.interactKey) && !KeyPressManager.handlingPrimaryKeyPress)
		{
			KeyPressManager.handlingPrimaryKeyPress = true;

			if (canInteract())
			{
				interact();
			}

		}

		if (Input.GetKey(KeyBindingList.observationKey) && !KeyPressManager.handlingPrimaryKeyPress && PartyManager.getPlayerStats().getWisdom() >= 2)
		{
			ObservationManager.enterObservationMode();
			KeyPressManager.handlingPrimaryKeyPress = true;
			return;
		}

		if (Input.GetKey(KeyBindingList.cunningKey) && !KeyPressManager.handlingPrimaryKeyPress && CunningManager.getCunningsRemaining() > 0)
		{
			CunningManager.enterCunningMode();
			KeyPressManager.handlingPrimaryKeyPress = true;
			return;
		}

		if (Input.GetKey(KeyBindingList.intimidateKey) && !KeyPressManager.handlingPrimaryKeyPress && IntimidateManager.getIntimidatesRemaining() > 0)
		{
			IntimidateManager.enterIntimidateMode();
			KeyPressManager.handlingPrimaryKeyPress = true;	
			return;
		}

		if (Input.GetKey(KeyBindingList.mapKey) && !KeyPressManager.handlingPrimaryKeyPress)
		{
			mapPopUpButton.spawnPopUp();

			KeyPressManager.handlingPrimaryKeyPress = true;
			return;
		}

		if (Input.GetKey(KeyBindingList.transcriptKey) && !KeyPressManager.handlingPrimaryKeyPress)
		{
			dialogueManager.spawnDialogueTrackerWindowWithoutChoices();

			KeyPressManager.handlingPrimaryKeyPress = true;
		}

		if (Input.GetKey(KeyBindingList.placeCompanionKey) && !KeyPressManager.handlingPrimaryKeyPress && PartyMemberPlacer.getPlacedPartyMemberCount() < PartyStats.getMaxPlacablePartyMembers())
		{
			PartyMemberPlacer.placeNextPartyMember();
			KeyPressManager.handlingPrimaryKeyPress = true;
		}

		if (Input.GetKey(KeyBindingList.removePlacedCompanionMoveableObjectKey) && !KeyPressManager.handlingPrimaryKeyPress)
		{
			if (Physics2D.OverlapCircle(colliderWorldPosition(), detectionSize, LayerAndTagManager.npcLayerMask))
			{
				GameObject npcGameObject = Physics2D.OverlapCircle(colliderWorldPosition(), detectionSize, LayerAndTagManager.npcLayerMask).gameObject;

				if (npcGameObject.tag.Equals(LayerAndTagManager.partyMemberTag))
				{
					string partyMemberName = npcGameObject.GetComponent<PartyMemberTrainPriority>().partyMemberName;
					PartyMemberPlacer.removePlacedPartyMember(partyMemberName);
				}
				else
				{
					return; //took out "fight anyone" so this code doesn't need to be run. keeping it here in case it's needed again

					npcCombatInfo = npcGameObject.GetComponent<NPCCombatInfo>();

					if (npcCombatInfo == null || !npcCombatInfo.canBeFought())
					{
						return;
					}

					FadeToBlackManager.getInstance().setAndStartFadeToBlack();
					enteredCombat = true;
				}

			}
			else if (Physics2D.OverlapCircle(colliderWorldPosition(), detectionSize, LayerAndTagManager.movableObjectLayerMask))
			{
				GameObject movableObject = Physics2D.OverlapCircle(colliderWorldPosition(), detectionSize, LayerAndTagManager.movableObjectLayerMask).gameObject;
				EnemyMovement enemyMovement = movableObject.GetComponent<EnemyMovement>();
				enemyMovement.putBackToStartingPosition();
				MovementManager.getInstance().evaluateAllButtonScripts();
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

		if (KeyBindingList.saveLoadScreenKeyIsPressed() && !KeyPressManager.handlingPrimaryKeyPress)
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
			&& dialogueManager.storyCanContinue())
		{
			if (!dialogueManager.getDialogue().random)
			{
				dialogueManager.continueStory();
			}

			KeyPressManager.handlingPrimaryKeyPress = true;
			return;
		}


		if (Input.GetKey(KeyCode.Alpha1))
		{
			KeyPressManager.handlingPrimaryKeyPress = true;
			dialogueManager.makeChoice(0);
			return;
		}
		else if (Input.GetKey(KeyCode.Alpha2))
		{
			KeyPressManager.handlingPrimaryKeyPress = true;
			dialogueManager.makeChoice(1);
			return;
		}
		else if (Input.GetKey(KeyCode.Alpha3))
		{
			KeyPressManager.handlingPrimaryKeyPress = true;
			dialogueManager.makeChoice(2);
			return;
		}
		else if (Input.GetKey(KeyCode.Alpha4))
		{
			KeyPressManager.handlingPrimaryKeyPress = true;
			dialogueManager.makeChoice(3);
			return;
		}
		else if (Input.GetKey(KeyCode.Alpha5))
		{
			KeyPressManager.handlingPrimaryKeyPress = true;
			dialogueManager.makeChoice(4);
			return;
		}
		else if (Input.GetKey(KeyCode.Alpha6))
		{
			KeyPressManager.handlingPrimaryKeyPress = true;
			dialogueManager.makeChoice(5);
			return;
		}
		else if (Input.GetKey(KeyCode.Alpha7))
		{
			KeyPressManager.handlingPrimaryKeyPress = true;
			dialogueManager.makeChoice(6);
			return;
		}
		else if (Input.GetKey(KeyCode.Alpha8))
		{
			KeyPressManager.handlingPrimaryKeyPress = true;
			dialogueManager.makeChoice(7);
			return;
		}
	}

	private void handleUIStateKeyPresses()
	{
		showFormulaToggleCheck();

		if (KeyBindingList.eitherBackoutKeyIsPressed() && EscapeStack.getEscapableObjectsCount() > 0 && !KeyPressManager.handlingPrimaryKeyPress)
		{
			EscapeStack.handleEscapePress();

			KeyPressManager.handlingPrimaryKeyPress = true;
			return;
		}
		else if ((Input.GetKey(KeyBindingList.lastScreenKey) || KeyBindingList.eitherBackoutKeyIsPressed())
					&& !KeyPressManager.handlingPrimaryKeyPress)
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

		bool passedbackOutCheck = false;

		switch (OverallUIManager.lastScreenType)
		{
			case ScreenType.Character:
				passedbackOutCheck = backOutCheck(KeyBindingList.characterScreenKey);
				break;
			case ScreenType.Inventory:
				passedbackOutCheck = backOutCheck(KeyBindingList.inventoryScreenKey);
				break;
			case ScreenType.Party:
				passedbackOutCheck = backOutCheck(KeyBindingList.partyScreenKey);
				break;
			case ScreenType.Journal:
				passedbackOutCheck = backOutCheck(KeyBindingList.journalScreenKey);
				break;
			case ScreenType.SaveAndLoad:
				if (backOutCheck(KeyBindingList.saveScreenKey) ||
					backOutCheck(KeyBindingList.loadScreenKey))
				{
					return;
				}
				break;
			default:
				break;
		}

		if (passedbackOutCheck)
		{
			return;
		}

		if (Input.GetKey(KeyBindingList.moveLeftKey) && !SaveHandler.ignoreNavigationKeyPresseDuringInputFieldSelection() && !KeyPressManager.handlingPrimaryKeyPress)
		{
			OverallUIManager.moveToScreenToTheLeft();

			KeyPressManager.handlingPrimaryKeyPress = true;
			return;
		}

		if (Input.GetKey(KeyBindingList.moveRightKey) && !SaveHandler.ignoreNavigationKeyPresseDuringInputFieldSelection() && !KeyPressManager.handlingPrimaryKeyPress)
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
			if ((keyCode == KeyBindingList.saveScreenKey || keyCode == KeyBindingList.loadScreenKey) && SaveHandler.ignoreNavigationKeyPresseDuringInputFieldSelection())
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
		if (MapPopUpWindow.hasFastTravelTarget() && KeyBindingList.eitherBackoutKeyIsPressed() && !KeyPressManager.handlingPrimaryKeyPress)
		{
			MapPopUpWindow.fastTravelPanelCloseButtonPress();

			KeyPressManager.handlingPrimaryKeyPress = true;
			return;
		}
		else if (!MapPopUpWindow.hasFastTravelTarget() && Input.GetKey(KeyBindingList.mapKey) || KeyBindingList.eitherBackoutKeyIsPressed() && !KeyPressManager.handlingPrimaryKeyPress)
		{

			mapPopUpButton.destroyPopUp();

			KeyPressManager.handlingPrimaryKeyPress = true;
		}
	}

	private void handleCunningStateKeyPresses()
	{
		if ((KeyBindingList.eitherBackoutKeyIsPressed() || Input.GetKey(KeyBindingList.cunningKey)) && !KeyPressManager.handlingPrimaryKeyPress)
		{
			CunningManager.leaveCunningMode();
			KeyPressManager.handlingPrimaryKeyPress = true;
			return;
		}

		if (KeyBindingList.movementKeyPressed() && !KeyPressManager.handlingPrimaryKeyPress)
		{
			cunningManager.handleWASDMovement();
			KeyPressManager.handlingPrimaryKeyPress = true;
		}

		if (Input.GetKey(KeyBindingList.interactKey) && !KeyPressManager.handlingPrimaryKeyPress)
		{
			KeyPressManager.handlingPrimaryKeyPress = true;

			if (cunningManager.executeSkill())
			{
				PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
			}

			return;
		}
	}

	private void handleObservingStateKeyPresses()
	{
		if ((KeyBindingList.eitherBackoutKeyIsPressed() || Input.GetKey(KeyBindingList.observationKey)) && !KeyPressManager.handlingPrimaryKeyPress)
		{
			ObservationManager.leaveObservationMode();
			KeyPressManager.handlingPrimaryKeyPress = true;
			return;
		}
	}

	private void handleIntimidateStateKeyPresses()
	{
		if ((KeyBindingList.eitherBackoutKeyIsPressed() || Input.GetKey(KeyBindingList.intimidateKey)) && !KeyPressManager.handlingPrimaryKeyPress)
		{
			IntimidateManager.leaveIntimidateMode();
			KeyPressManager.handlingPrimaryKeyPress = true;
			return;
		}

		if (Input.GetKey(KeyBindingList.interactKey) && !KeyPressManager.handlingPrimaryKeyPress)
		{
			KeyPressManager.handlingPrimaryKeyPress = true;

			if (intimidateManager.executeSkill())
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
            currentChest.destroyUI();
            currentChest.setSpriteToOpenEmpty();

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

	private void readBook(GameObject bookGameObject)
	{
		WorldBookInfo bookInfo = bookGameObject.GetComponent<WorldBookInfo>();

		NotificationManager.OnDeleteAllNotifications.Invoke();

		bookInfo.setUpBookManager(WorldBookInfo.giveCopyOfBook, OOCActivity.walking);
	}

	private void speakToNPC(GameObject npcGameObject)
	{
		dialogueTrigger = npcGameObject.GetComponent<DialogueTrigger>();

		if (dialogueTrigger == null || dialogueTrigger is null)
		{
			return;
		}

		NotificationManager.OnDeleteAllNotifications.Invoke();

		dialogueManager = DialogueManager.getInstance();

		dialogueTrigger.TriggerDialogue();
	}

	private bool canMove()
	{
		return PlayerOOCStateManager.currentActivity == OOCActivity.walking;
	}

	public void adjustAnimator()
	{
		adjustAnimator(false);
	}

	public void adjustAnimator(bool moveOverride)
	{
		if (!canMove() && !moveOverride)
		{
			return;
		}

		switch (State.playerFacing.getFacing())
		{
			case Facing.NorthEast:
				direction = "NE";
				break;
			case Facing.SouthEast:
				direction = "SE";
				break;
			case Facing.SouthWest:
				direction = "SW";
				break;
			case Facing.NorthWest:
				direction = "NW";
				break;
		}

		setButtonPromptVisibility();

		if (currentMovementKeyCode != KeyCode.None && !animator.GetBool("playWalkCycle") && FadeToBlackManager.isTransparent())
		{
			runOrIdle = "Run";
		}
		else if (!FadeToBlackManager.isTransparent() || currentMovementKeyCode == KeyCode.None || PlayerOOCStateManager.currentActivity != OOCActivity.walking)
		{
			runOrIdle = "Idle";
		}

		string newStateName = animationStatePrefix + runOrIdle + direction;

		if (!animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Base Layer")).IsName(newStateName))
		{
			animator.Play(baseLayerName + "." + newStateName);
		}
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
			barredMovementKeyCodes = new ArrayList();
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
					barredMovementKeyCodes = new ArrayList();
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

		if (currentMovementKeyCode != KeyCode.None && Input.GetKey(currentMovementKeyCode) && !MovementManager.getInstance().isBetweenTiles(0))
		{
			switch (currentMovementKeyCode)
			{
				case KeyBindingList.moveNorthKey:
					State.playerFacing.setFacing(Facing.NorthEast);
					adjustDirectionalModifierGrid();
					break;

				case KeyBindingList.moveWestKey:
					State.playerFacing.setFacing(Facing.NorthWest);
					adjustDirectionalModifierGrid();
					break;

				case KeyBindingList.moveSouthKey:
					State.playerFacing.setFacing(Facing.SouthWest);
					adjustDirectionalModifierGrid();
					break;

				case KeyBindingList.moveEastKey:
					State.playerFacing.setFacing(Facing.SouthEast);
					adjustDirectionalModifierGrid();
					break;
			}

			if (Physics2D.OverlapCircle(colliderWorldPosition(), detectionSize, LayerAndTagManager.movableObjectLayerMask))
			{

				if (!Helpers.checkPositionForColliders(colliderWorldPosition(2), detectionSize, LayerAndTagManager.blocksMovableObjectLayerMask))
				{
					MovementManager.getInstance().moveAllSprites(directionalModifierGrid);
				}
				else
				{
					MovementManager.getInstance().moveAllSprites(new Vector3Int(0, 0, 0));
				}

			}
			else if (!Helpers.checkPositionForColliders(colliderWorldPosition(), detectionSize, LayerAndTagManager.blocksPlayerMovementLayerMask))
			{
				MovementManager.getInstance().moveAllSprites(directionalModifierGrid);
			}
			else
			{
				MovementManager.getInstance().moveAllSprites(new Vector3Int(0, 0, 0));
			}

			return true;
		}

		return false;
	}

	public void adjustDirectionalModifierGrid()
	{
		if (State.playerFacing.getFacing() == Facing.NorthEast)
		{
			directionalModifierGrid = MovementManager.distance1TileNorthEastGrid;
			return;

		}
		else if (State.playerFacing.getFacing() == Facing.NorthWest)
		{
			directionalModifierGrid = MovementManager.distance1TileNorthWestGrid;
			return;

		}
		else if (State.playerFacing.getFacing() == Facing.SouthWest)
		{
			directionalModifierGrid = MovementManager.distance1TileSouthWestGrid;
			return;

		}
		else if (State.playerFacing.getFacing() == Facing.SouthEast)
		{
			directionalModifierGrid = MovementManager.distance1TileSouthEastGrid;
			return;
		}

		throw new IOException("State.playerFacing isn't set to any direction");

	}

	public void interact()
	{
		// Debug.LogError("interact called");
		if (Physics2D.OverlapCircle(colliderWorldPosition(), detectionSize, LayerAndTagManager.npcLayerMask))
		{
			GameObject currentGameObject = Physics2D.OverlapCircle(colliderWorldPosition(), detectionSize, LayerAndTagManager.npcLayerMask).gameObject;

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
		// else if (Physics2D.OverlapCircle(colliderWorldPosition(), detectionSize, LayerAndTagManager.openableDoorLayer))
		// {
		// 	Physics2D.OverlapCircle(colliderWorldPosition(), detectionSize, LayerAndTagManager.openableDoorLayer).gameObject.GetComponent<OpenableDoor>().open();
		// }
		else if (Physics2D.OverlapCircle(colliderWorldPosition(), detectionSize, LayerAndTagManager.chestLayerMask))
		{

			currentChest = Physics2D.OverlapCircle(colliderWorldPosition(), detectionSize, LayerAndTagManager.chestLayerMask).gameObject.GetComponent<Chest>();

			if (!currentChest.hasBeenOpened())
			{
				currentChest.playerOpensChest();
				PlayerOOCStateManager.setCurrentActivity(OOCActivity.inChestUI);
				return;
			}
		}
	}

	public static bool canInteract()
	{
		PlayerMovement player = getInstance();

		if (player == null)
		{
			return false;
		}

		// Debug.LogError("interact called");
		if (Physics2D.OverlapCircle(player.colliderWorldPosition(), detectionSize, LayerAndTagManager.npcLayerMask))
		{
			GameObject currentGameObject = Physics2D.OverlapCircle(player.colliderWorldPosition(), detectionSize, LayerAndTagManager.npcLayerMask).gameObject;

			if (currentGameObject.tag.Equals(LayerAndTagManager.npcTag) ||
				currentGameObject.tag.Equals(LayerAndTagManager.observableTag) ||
				currentGameObject.tag.Equals(LayerAndTagManager.transitionTag)) //added transition tag for Ladders, normal transitions shouldn't be interactable
			{                                                                   //If a transition is interactable (it would throw an error when interacted with)
																				//then it has it's layer set to NPC erroneously
				return true;
			}
			else if (currentGameObject.tag.Equals(LayerAndTagManager.bookTag))
			{
				return true;
			}

		}
		// else if (Physics2D.OverlapCircle(colliderWorldPosition(), detectionSize, LayerAndTagManager.openableDoorLayer))
		// {
		// 	Physics2D.OverlapCircle(colliderWorldPosition(), detectionSize, LayerAndTagManager.openableDoorLayer).gameObject.GetComponent<OpenableDoor>().open();
		// }
		else if (Physics2D.OverlapCircle(player.colliderWorldPosition(), detectionSize, LayerAndTagManager.chestLayerMask))
		{
			player.currentChest = Physics2D.OverlapCircle(player.colliderWorldPosition(), detectionSize, LayerAndTagManager.chestLayerMask).gameObject.GetComponent<Chest>();

			if (!player.currentChest.hasBeenOpened())
			{
				return true;
			}
		}

		return false;
	}

	private void OnDrawGizmos()
	{

		if (MovementManager.getInstance() == null)
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
				Gizmos.DrawWireSphere(colliderWorldPosition(), detectionSize);
				Gizmos.DrawWireSphere(colliderWorldPosition(2), detectionSize);
				return;
			case Facing.SouthEast:
				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere(colliderWorldPosition(), detectionSize);
				Gizmos.DrawWireSphere(colliderWorldPosition(2), detectionSize);
				return;
			case Facing.SouthWest:
				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere(colliderWorldPosition(), detectionSize);
				Gizmos.DrawWireSphere(colliderWorldPosition(2), detectionSize);
				return;
			case Facing.NorthWest:
				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere(colliderWorldPosition(), detectionSize);
				Gizmos.DrawWireSphere(colliderWorldPosition(2), detectionSize);
				return;
			default:
				throw new IOException("Unknown facing: " + State.playerFacing.getFacing().ToString());
		}
	}

	public void hideTerrain()
	{
		for (int i = 0; i < terrainTilemaps.Length; i++)
		{
			if (terrainTilemaps[i].maskInteraction == SpriteMaskInteraction.VisibleOutsideMask)
			{
				continue;
			}

			terrainTilemaps[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		}

		for (int i = 0; i < terrainSprites.Length; i++)
		{
			if (terrainSprites[i].maskInteraction == SpriteMaskInteraction.VisibleOutsideMask)
			{
				continue;
			}

			terrainSprites[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		}
	}

	public void displayTerrain()
	{
		for (int i = 0; i < terrainTilemaps.Length; i++)
		{
			if (terrainTilemaps[i].maskInteraction == SpriteMaskInteraction.None)
			{
				continue;
			}

			terrainTilemaps[i].maskInteraction = SpriteMaskInteraction.None;
		}

		for (int i = 0; i < terrainSprites.Length; i++)
		{
			if (terrainSprites[0].maskInteraction == SpriteMaskInteraction.None)
			{
				continue;
			}

			terrainSprites[i].maskInteraction = SpriteMaskInteraction.None;
		}
	}

	//doesn't just mask terrain objects but turns them completely off.
	public void setTerrainActive(bool terrainStatus)
	{
		foreach (TilemapRenderer tilemap in terrainTilemaps)
		{
			tilemap.enabled = terrainStatus;
		}

		foreach (SpriteRenderer sprite in terrainSprites)
		{
			sprite.enabled = terrainStatus;
		}

		State.terrainHidden = !terrainStatus;
	}

    public bool onTopOfTransitionOrTutorial()
    {
        if (Helpers.hasCollision(transitionCollider) && !FadeToBlackManager.getInstance().currentlyFadingToBlack())
        {
            if (Helpers.hasCollision(transitionCollider, LayerAndTagManager.transitionLayerMask))
            {
                // NewSceneTransition transition = Helpers.getCollision(transitionCollider, LayerAndTagManager.transitionLayerMask).transform.parent.GetComponent<NewSceneTransition>();
                // TransitionManager.changeScene(transition.getTransitionInfo());

                Transition transition = Helpers.getCollision(transitionCollider, LayerAndTagManager.transitionLayerMask).transform.GetComponent<TransitionSpace>().transition;
                TransitionManager.changeScene(transition);
                return true;
            }
            
            if (PlayerOOCStateManager.currentActivity != OOCActivity.inTutorialSequence && Helpers.hasCollision(transitionCollider, LayerAndTagManager.tutorialLayerMask))
            {
                Collider2D tutorialCollider = Helpers.getCollision(transitionCollider, LayerAndTagManager.tutorialLayerMask);

                if (TutorialSequence.startTutorialSequence(tutorialCollider.gameObject))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void prepCombat()
    {
        State.playerPosition = transform.position;

        State.enemyPackInfo = npcCombatInfo.getEnemyInfo(0);

        if (!npcCombatInfo.hasDeadNames())
        {
            Dialogue dialogue = npcCombatInfo.gameObject.GetComponent<DialogueTrigger>().dialogue;
            npcCombatInfo.deadNameList = new DeadNameList[1];
            npcCombatInfo.deadNameList[0] = new DeadNameList(new string[] { dialogue.names[1] });
        }

        npcCombatInfo.addAllDeadNames(0);

        QuestList.checkForDeadNames();

        AreaList.addHostility();

        State.enemyFacing = npcCombatInfo.gameObject.GetComponent<NPCMovement>().getFacing();
        CombatStateManager.locationBeforeCombat = AreaManager.locationName;

        SceneChange.changeSceneToCombat();
    }

	private void findTerrainObjects()
    {
		GameObject[] terrainObjects = GameObject.FindGameObjectsWithTag(LayerAndTagManager.terrainTag);

		terrainTilemaps = new TilemapRenderer[0];
		terrainSprites = new SpriteRenderer[0];

		foreach (GameObject terrainObject in terrainObjects)
		{
			TilemapRenderer terrainTilemap = terrainObject.GetComponent<TilemapRenderer>();
			SpriteRenderer terrainSprite = terrainObject.GetComponent<SpriteRenderer>();

			if (terrainTilemap != null && !(terrainTilemap is null))
			{
				terrainTilemaps = Helpers.appendArray<TilemapRenderer>(terrainTilemaps, terrainTilemap);
			}

			if (terrainSprite != null && !(terrainSprite is null))
			{
				terrainSprites = Helpers.appendArray<SpriteRenderer>(terrainSprites, terrainSprite);
			}
		}
	}

	public static void createCustomButtonPrompt(string promptMessage)
	{
		PlayerMovement player = getInstance();

		if (player == null)
		{
			return;
		}

		if (promptMessage.Length > 0)
		{
			player.pressButtonPrompt.SetActive(true);
			player.pressButtonPromptText.text = promptMessage;

			hasCustomPromptMessage = true;
		}
		else
		{
			player.pressButtonPrompt.SetActive(false);
		}
	}



	public static void setButtonPromptVisibility()
	{
		PlayerMovement player = getInstance();

		if (player == null)
		{
			return;
		}

		if (PlayerOOCStateManager.currentActivity == OOCActivity.walking)
		{
			string promptMessage = player.getPromptMessage();

			if (promptMessage.Length > 0)
			{

				player.pressButtonPrompt.SetActive(true);
				player.pressButtonPromptText.text = promptMessage;

				hasCustomPromptMessage = false;
			}
			else if (hasCustomPromptMessage)
			{
				return;
			}
			else
			{
				player.pressButtonPrompt.SetActive(false);
			}
		}
		else
		{
			player.pressButtonPrompt.SetActive(false);
		}
	}

	private string getPromptMessage()
	{
		if (canInteract())
		{
			return "E: Interact";
		}


		Collider2D collider = Physics2D.OverlapCircle(instance.colliderWorldPosition(), detectionSize, LayerAndTagManager.npcLayerMask);

		if (collider != null && collider.gameObject.tag.Equals(LayerAndTagManager.partyMemberTag))
		{
			return "Z: Remove";
		}

		collider = Physics2D.OverlapCircle(instance.colliderWorldPosition(), detectionSize, LayerAndTagManager.movableObjectLayerMask);

		if (collider != null)
		{
			EnemyMovement movableObject = collider.gameObject.GetComponent<EnemyMovement>();

			if (movableObject != null && movableObject.canBePutBackToStartingPosition())
			{
				return "Z: Return";
			}
		}

		return "";
	}

	private void printDebugMessages()
	{
		debugMessageTimer += Time.deltaTime;

		if (debugMessageTimer > 1f)
		{
			Debug.LogError("PlayerOOCStateManager.currentActivity = " + PlayerOOCStateManager.currentActivity.ToString());

			debugMessageTimer = 0f;
		}
	}

	private Vector3 colliderWorldPosition() //world used for checking for colliders and drawing gizmos
	{
		return MovementManager.getInstance().grid.GetCellCenterWorld(MovementManager.getInstance().grid.WorldToCell(transform.position) + directionalModifierGrid) - new Vector3(0f, .2f, 0);
	}

	private Vector3 colliderWorldPosition(int multiplier) //world used for checking for colliders and drawing gizmos, with multiplier
	{
		return MovementManager.getInstance().grid.GetCellCenterWorld(MovementManager.getInstance().grid.WorldToCell(transform.position) + (directionalModifierGrid) * multiplier) - new Vector3(0f, .2f, 0);
	}

	public Vector3Int getMovementGridCoordsLocal()
	{
		return MovementManager.getInstance().grid.LocalToCell(transform.localPosition);
	}

    public Vector3 convertGridCoordsToLocalPos(Vector3Int gridSquareCoords)
    {
        return MovementManager.getInstance().grid.GetCellCenterLocal(gridSquareCoords);
    }
    
    // public Vector3 convertGridCoordsToWorldPos(Vector3Int gridSquareCoords)
	// {
	// 	return MovementManager.getInstance().grid.GetCellCenterWorld(gridSquareCoords);
	// }

	private void showFormulaToggleCheck()
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
