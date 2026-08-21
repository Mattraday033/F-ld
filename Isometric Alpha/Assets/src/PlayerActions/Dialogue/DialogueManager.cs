using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ink.Runtime;
using Cinemachine;
using System.Linq;

public delegate void AnimationDelegate<T>(T t);

public class DialogueManager : MonoBehaviour
{
    private string quantitySymbol = " x";

    private List<Choice> currentChoiceInkObjects;

    [Header("Cameras")] 

    [SerializeField] 
    public Camera mainCamera;
	[SerializeField] 
    public CinemachineVirtualCamera mainCM;
	private CinemachineFramingTransposer framingTransposer;

    [Header("Managers")]
	[SerializeField] 
    public OOCUIManager oocUIManager;
	private FadeToBlackManager fadeToBlackManager;

	private static bool returnToRevealAfterDialogue;


    #region _NameText/Speaker Highlighting
	private string _NameText;

    private string getNameTextWithoutColon()
    {
        return _NameText.Replace(":", "");
    }

    private void resetNameText()
    {
        _NameText = "";
    }

    private void setNameText(int targetIndex)
    {
        AnimationManager.RemoveAllShadowOutlines.Invoke();

        _NameText = DialogueList.scrubNameOfEndNumbers(currentDialogue.names[targetIndex]) + ":";

        if(targetIndex > 0)
        {
            highlightSpeaker(currentDialogue.cameraFoci[targetIndex]);
        }
    }

    private void highlightSpeaker(GameObject cameraFoci)
    {
        if(GameplaySettingsList.highlightSpeakerInDialogue.settingOptions[Constants.onSettingIndex].set)
        {
            AnimationManager animationManager = cameraFoci.GetComponent<AnimationManager>();
        
            if(animationManager != null)
            {
                animationManager.showShadowOutline();
            }
        }
    }

    #endregion

	private string buffer;
	private bool keepOldDialogue = false;
	private bool combineOldDialogue = false;
	private Story currentStory;
	private string storyName;
	private string defaultChoiceText = "Continue...";
	private ChoiceKey previousChoice;

	private Conversation currentConversation;
	private Dialogue currentDialogue;
	private static DialogueManager instance;

	private bool addItemText = false;
	private bool atConvoEndPoint = true;
	private bool waitingOnFadeToBlack = false;
	private bool waitingOnFadeBackIn = false;

	private const float defaultXDamping = 0f;
	private const float defaultYDamping = 0f;
	private const float dialogueXDamping = 1.25f;
	private const float dialogueYDamping = 1.25f;
	private int frames = 60;
	private int framesToWait = 60;

	public DialogueTrackerButton dialogueTrackerButton;
	private DialogueTrackerWindow dialogueTrackerWindow;

	private IEnumerator tutorialWaitCoroutine;

	void Start()
	{
		setCameras();

		oocUIManager = OOCUIManager.getInstance();
		fadeToBlackManager = FadeToBlackManager.getInstance();

		if (!LoadSaveFile.midLoad && State.dialogueUponSceneLoadKey != null && State.dialogueUponSceneLoadKey.Length > 0)
		{
			dialogueTrackerButton = new DialogueTrackerButton(true);

            startDialogue(DialogueList.getDialogue(AreaManager.locationName, State.dialogueUponSceneLoadKey));
			State.dialogueUponSceneLoadKey = null;
			return;
		}

		dialogueTrackerButton = new DialogueTrackerButton(true);
	}

	void Update() //here for Animation
	{
		if (waitingOnFadeToBlack && !FadeToBlackManager.isMidScreenFade())
		{
			waitingOnFadeToBlack = false;
			continueStory();
		}

		if (waitingOnFadeBackIn && !FadeToBlackManager.isMidScreenFade())
		{
			waitingOnFadeBackIn = false;

			if (dialogueTrackerWindow != null && !(dialogueTrackerWindow is null)) 
			{
				continueStory();
			}
		}

		if (frames < framesToWait)
		{
			frames++;
		}
	}

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Found more than one Dialogue Manager in the scene.");
		}

		instance = this;
	}

	public void setCameras()
	{
		mainCamera = Camera.main;
		mainCM = GameObject.FindWithTag("MainVirtualCamera").GetComponent<CinemachineVirtualCamera>();
		framingTransposer = mainCM.GetCinemachineComponent<CinemachineFramingTransposer>();
        //mainCM.Follow = PlayerMovement.getInstance().gameObject.transform;
		//dialogueCanvas.worldCamera = mainCamera;
	}

	public bool storyCanContinue()
	{
		return currentStory.canContinue && dialogueUIVisible();
	}

	public bool dialogueUIVisible()
	{
		return dialogueTrackerWindow != null && dialogueTrackerWindow.gameObject.activeSelf && dialogueTrackerWindow.gameObject.activeInHierarchy;
	}

	public Dialogue getDialogue()
	{
		return currentDialogue;
	}

	public bool convoAtEndPoint()
	{
		return atConvoEndPoint;
	}

	public static DialogueManager getInstance()
	{
		return instance;
	}

	public void startDialogue(Dialogue dialogue, bool midDialogue = false)
	{
		//Flags.printAll();

        if(dialogue == null)
        {
            Debug.LogError("dialogue == null");
            return;
        } else if(dialogue.inkJSON == null)
        {
            Debug.LogError("dialogue.inkJSON == null");
            return;
        }

		if (RevealManager.currentlyRevealed)
		{
			returnToRevealAfterDialogue = true;
			RevealManager.toggleReveal();
		}

		if (framingTransposer == null)
		{
			Start();
		}

		PlayerOOCStateManager.setCurrentActivity(OOCActivity.inDialogue);

        setCameraToDialogueSpeed();
		oocUIManager.disableOOCUI();

		currentDialogue = dialogue;

        PartyMemberPlacer.HideAllFollowers.Invoke();
        
        if(currentDialogue.findNPCGameObjectsInScene())
        {
            findNPCGameObject();
        } else
        {
            currentDialogue.cameraFoci[Constants.indexOne] = PlayerMovement.getCurrentInteractableBeforePlayer();
        }

        NPCCombatInfo combatInfo = currentDialogue.npcCombatInfo;

		setNameText(Dialogue.mainNPCIndex);
		currentStory = addAllVariables(new Story(dialogue.inkJSON.text), dialogue.variableSources);

		storyName = dialogue.inkJSON.name;
		atConvoEndPoint = dialogue.convoEndableAtStart;

        if (currentDialogue.isVaultable)
        {
            VaultableObject vaultableObject = currentDialogue.cameraFoci[Dialogue.mainNPCIndex].GetComponent<VaultableObject>();

            if (currentStory.variablesState[InkVariableNameList.objectName] != null)
            {
                currentStory.variablesState[InkVariableNameList.objectName] = vaultableObject.objectName;
            }

            if (currentStory.variablesState[InkVariableNameList.plural] != null)
            {
                currentStory.variablesState[InkVariableNameList.plural] = vaultableObject.plural;
            }
        }

		if (currentStory.variablesState[InkVariableNameList.attitude] != null)
		{
			currentStory.variablesState[InkVariableNameList.attitude] = 0;
		}

        if(!midDialogue)
        {
            if (!dialogue.startWithUIDisabled)
            {
                dialogueTrackerButton.spawnEmptyPopUp();
                dialogueTrackerWindow = (DialogueTrackerWindow)dialogueTrackerButton.getPopUpWindow();

                currentConversation = new Conversation(dialogueTrackerWindow);
            }
            else
            {
                currentConversation = new Conversation();
            }

		    continueStory();
		    PlayerOOCStateManager.OnStateChangeToWalking.AddListener(onStateChangeToWalkingEvent);

            AudioManager.duckMusicForDialogue();
        }
	}

	public void endDialogue()
	{
		mainCM.Follow = PlayerMovement.getInstance().gameObject.transform;

		setCameraToDefaultSpeed();

		oocUIManager.enableOOCUI();

		currentDialogue = null;
		currentChoiceInkObjects = null;

		currentConversation.addEndOfDialogueLine();

        resetNameText();
		storyName = "";
		previousChoice = null;
		QuestList.checkForDeadNames();

        PartyMemberPlacer.RevealAllFollowers.Invoke();

        if (Flags.getFlag(FlagNameList.newGameFlagName))
        {
            Flags.setFlag(FlagNameList.newGameFlagName, false);
        }

		if (returnToRevealAfterDialogue)
		{
			returnToRevealAfterDialogue = false;
			RevealManager.toggleReveal();
		}

		SpeechLog.appendConversation(currentConversation);
		dialogueTrackerButton.destroyPopUp();
		dialogueTrackerWindow = null;
		currentConversation = new Conversation();

		if (PlayerOOCStateManager.currentActivity == OOCActivity.inDialogue)
		{
			PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
		}

        AudioManager.unduckMusicAfterDialogue();
	}

	public void onStateChangeToWalkingEvent()
	{
		EscapeStack.escapeAll();
		PlayerOOCStateManager.OnStateChangeToWalking.RemoveListener(onStateChangeToWalkingEvent);
	}

	private ChoiceDescription getDefaultChoice()
	{
		return new ChoiceDescription(1, defaultChoiceText, null);
	}

	private void displayChoices()
	{
		List<ChoiceDescription> choiceDescriptions = new List<ChoiceDescription>();

		if (currentStory.currentChoices.Count <= 0)
		{
			choiceDescriptions.Add(getDefaultChoice());
		}
		else
		{
			for (int choiceIndex = 0; choiceIndex < currentStory.currentChoices.Count; choiceIndex++)
			{
				choiceDescriptions.Add(new ChoiceDescription(choiceIndex + 1,
															 currentStory.currentChoices[choiceIndex].text,
															 new ChoiceKey(storyName, currentStory.currentChoices[choiceIndex].sourcePath)));
			}
		}

		if (dialogueTrackerWindow != null && !(dialogueTrackerWindow is null))
		{
			dialogueTrackerWindow.populateChoices(choiceDescriptions);
			currentChoiceInkObjects = currentStory.currentChoices;
		}
	}

	public void makeChoice(int choiceIndex)
	{
		if (currentChoiceInkObjects == null || choiceIndex >= currentChoiceInkObjects.Count || currentChoiceInkObjects.Count == 0)
        {
            continueStory();

			return;
		}

		if(currentChoiceInkObjects[choiceIndex].text.Contains(ChoiceDescription.unimplementedTag))
		{
			return;
		}

		ChoiceKey currentChoice = new ChoiceKey(storyName, currentChoiceInkObjects[choiceIndex].sourcePath);

		if (previousChoice == null || !currentChoice.Equals(previousChoice))
		{
			previousChoice = currentChoice;

			string newLine = PartyManager.getPlayerNameForDisplay() + ": " + currentChoiceInkObjects[choiceIndex].text;

			currentConversation.addDialogueLine(PartyManager.getPlayerNameForDisplay(), currentChoiceInkObjects[choiceIndex].text);

			ChoiceManager.addChoice(storyName, currentChoiceInkObjects[choiceIndex].sourcePath);

			currentStory.ChooseChoiceIndex(choiceIndex);

			currentStory.Continue();

			continueStory();
		}

	}

	public static void setCameraToDefaultSpeed()
	{
		instance.framingTransposer.m_XDamping = defaultXDamping;
		instance.framingTransposer.m_YDamping = defaultYDamping;
	}

	public static void setCameraToDialogueSpeed()
	{
		instance.framingTransposer.m_XDamping = dialogueXDamping;
		instance.framingTransposer.m_YDamping = dialogueYDamping;
	}

	public void spawnDialogueTrackerWindowWithoutChoices()
	{
		dialogueTrackerButton.withChoices = false;
		dialogueTrackerButton.spawnPopUp();
		dialogueTrackerButton.withChoices = true;
	}

    public static GameObject findNPCGameObject(string npcName)
    {
        List<Transform> children = new List<Transform>();
        children.AddRange(AreaManager.getNPCParentWithScale().Cast<Transform>().ToList());
        children.AddRange(AreaManager.getNPCParentWithoutScale().Cast<Transform>().ToList());

        foreach (Transform child in children)
        {
            if(child.gameObject.name.Contains(NPCSpawnDetails.extraSpaceNameSuffix))
            {
                continue;
            }

            IDialogueParticipant npcDialogueTrigger = child.GetComponent<IDialogueParticipant>();

            if (npcDialogueTrigger != null)
            {
                if (npcName.Equals(npcDialogueTrigger.getName()) || 
                    child.gameObject.name.Replace(OOCSpawnDetails.gameObjectNameSuffix, "").Equals(npcName))
                {
                    return child.gameObject;
                }
            }
        }

        return null;
    }

    public void findNPCGameObject()
    {
        currentDialogue.names[0] = PartyManager.getPlayerStats().getName();
        currentDialogue.cameraFoci[0] = PlayerMovement.getInstance().gameObject;

        for (int nameIndex = 1; nameIndex < currentDialogue.names.Length; nameIndex++)
        {
            GameObject npcObject = findNPCGameObject(currentDialogue.names[nameIndex]);

            if(npcObject != null)
            {
                currentDialogue.names[nameIndex] = currentDialogue.names[nameIndex];
                currentDialogue.cameraFoci[nameIndex] = npcObject;
            } else
            {
                Debug.LogError(currentDialogue.names[nameIndex] + " was not found");
            }
        }
    }

    public void continueStory()
    {

        string newLine;
        Item itemToGive;
        string partyMemberName = "";
        string tutorialKey = "";
        int camTargetIndex = 0;
        int intParameter = 0;
        string parameter = "";
        string[] args = new string[0];
        AnimationManager targetAnimationManager = null;

        if (currentStory.canContinue)
        {
            currentChoiceInkObjects = null;

            if (combineOldDialogue)
            {
                buffer = currentStory.Continue();
                buffer += " " + currentStory.Continue();
                buffer = buffer.Replace("\n", "").Replace("\r", "");
            }
            else if (!keepOldDialogue)
            {
                buffer = currentStory.Continue();
            }
            else
            { //if this executes, this means the previous continueStory() run was a keepDialogue() call
                buffer = currentStory.Continue(); //keeps the dialogue it was told to keep
                currentStory.Continue(); //skips next dialogue after the kept dialogue. Choices will then be displayed
            }

            //Debug.LogError(buffer);

            switch (buffer.Split("(")[0].ToLower())
            {

                case "autosave":

                    SaveHandler.autosave(MovementManager.getPlayerCell(), State.playerFacing.getFacing());

                    continueStory();
                    break;
                case "keepdialogue":

                    keepOldDialogue = true;
                    continueStory();
                    keepOldDialogue = false;

                    break;

                case "duckmusic":

                    AudioManager.duckMusicForDialogue();

                    continueStory();
                    break;

                case "activate":

                    intParameter = getArgumentInt(buffer, Constants.indexZero);

                    if(intParameter == Constants.indexZero)
                    {
                        PlayerObject.showPlayerSprite();
                    } else
                    {
                        currentDialogue.cameraFoci[intParameter].SetActive(true);
                    }

                    continueStory();

                    break;

                case "deactivate":

                    intParameter = getArgumentInt(buffer, Constants.indexZero);

                    if(intParameter == Constants.indexZero)
                    {
                        PlayerObject.hidePlayerSprite();
                    } else
                    {
                        currentDialogue.cameraFoci[intParameter].SetActive(false);
                    }

                    continueStory();

                    break;

                case "setsecretdoorsobservable":

                    ObservableObject.SetAllSecretDoorsObservable.Invoke();

                    continueStory();

                    break;

                case "revealnondefeatedenemies":
                case "revealallnondefeatedenemies":

                    if(AreaManager.getMonsterParent() != null && 
                        AreaManager.getMonsterParent().childCount <= 0)
                    {
                        SpawnInfoManager.spawnAllMonsters();
                    }

                    EnemyMovement.RevealAllNonDefeatedEnemies.Invoke();

                    continueStory();

                    break;

                case "activatequeststep":

                    string questTitle3 = getArgument(buffer);
                    string questStepTitle = Helpers.removeSpacesOnEnds(getArgument(buffer, Constants.indexOne));

                    checkForHaltNotificationBoolArg(buffer, Constants.indexTwo);

                    QuestList.activateQuestStep(questTitle3, questStepTitle);

                    continueStory();

                    break;

                case "finishedquest":
                case "finishquest":

                    string questTitle = getArgument(buffer);
                    bool questSuccessful = getArgumentBool(buffer, Constants.indexOne);
                    string finalQuestStep = Helpers.removeSpacesOnEnds(getArgument(buffer, Constants.indexTwo));

                    checkForHaltNotificationBoolArg(buffer, Constants.indexThree);

                    QuestList.finishQuest(questTitle, finalQuestStep, questSuccessful);

                    OOCUIManager.updateQuestCounter();

                    continueStory();

                    break;

                case "changecamtarget":
                case "changecameratarget":

                    int k = getArgumentInt(buffer, Constants.indexZero);

                    changeCameraTarget(k);
                    continueStory();

                    break;

                case "removechoice":
                case "updatechoice":

                    string storyName = getArgument(buffer, Constants.indexZero);
                    string storyPath = getArgument(buffer, Constants.indexOne);

                    ChoiceManager.removeChoice(storyName, storyPath);

                    continueStory();

                    break;

                case "settotrue":

                    buffer = getArgument(buffer);

                    Flags.setFlag(buffer, true);

                    if (currentStory.variablesState.Contains(buffer))
                    {
                        currentStory.variablesState[buffer] = true;
                    }

                    continueStory();

                    break;

                case "settofalse":

                    buffer = getArgument(buffer);

                    Flags.setFlag(buffer, false);

                    if (currentStory.variablesState.Contains(buffer))
                    {
                        currentStory.variablesState[buffer] = false;
                    }
                    continueStory();

                    break;

                case "settutorialtoseen":

                    buffer = getArgument(buffer);

                    TutorialFlags.setFlag(buffer, true);

                    continueStory();

                    break;

                case "resettutorial":

                    buffer = getArgument(buffer);

                    TutorialFlags.setFlag(buffer, false);

                    continueStory();

                    break;

                case "replenishskills":
                case "replenishallskills":
                case "resetskills":
                case "resetallskills":

                    PartyStats.resetAllSkills();

                    continueStory();

                    break;

                case "addsecretdoorflag":

                    string secretDoorKey = getArgument(buffer);

                    SecretDoorFlags.addSecretDoorFlag(secretDoorKey);

                    continueStory();

                    break;

                case "updatenpcvisibility":

                    SecretDoorFlags.addSecretDoorFlag("");

                    continueStory();

                    break;

                case "combinedialogue":

                    combineOldDialogue = true;

                    continueStory();

                    combineOldDialogue = false;

                    break;

                case "prepitem":
                case "prepforitem": //prepForItem() calls must come before the dialogue directly before a addXP()/giveCoins()/takeCoins()/giveItem()/giveItems()/takeAllOfItem() call
                                    //to account for the additional dialogue explaining what item was taken/received

                    addItemText = true;
                    continueStory();

                    break;

                case "givecoin":
                case "givecoins":

                    string coinsToGive = getArgument(buffer);

                    Purse.addCoins(coinsToGive);

                    newLine = "Gold Coins x" + coinsToGive;

                    currentConversation.addObtainedLine(newLine);

                    break;

                case "takecoin":
                case "takecoins":

                    string coinsToTake = getArgument(buffer);

                    Purse.removeCoins(coinsToTake);

                    newLine = "Gold Coins x" + coinsToTake;

                    currentConversation.addRemovedLine(newLine);

                    break;

                case "giveitem":

                    givePlayerItemFromDialogue(buffer);

                    break;

                case "giveitems":

                    string[] argGroups = getAllArgumentGroups(buffer);

                    foreach (string argGroup in argGroups)
                    {
                        givePlayerItemFromDialogue(argGroup);
                    }

                    break;

                case "takeallofitem":

                    string itemName = getArgument(buffer);

                    int quantity = 0;

                    Item itemInInv = Inventory.removeItem(itemName);

                    if(itemInInv != null)
                    {
                        quantity += itemInInv.getQuantity();
                    }

                    Item itemInJunk = Inventory.removeItem(itemName, State.junkPocket);

                    if(itemInJunk != null)
                    {
                        quantity += itemInJunk.getQuantity();
                    }

                    List<PartyMember> partyMembers = PartyManager.getAllPartyMembers();

                    foreach (PartyMember partyMember in partyMembers)
                    {
                        EquippedItems equippedItems = partyMember.stats.getEquippedItems();

                        for (int index = 0; index < EquippedItems.totalEquipmentSlots; index++)
                        {
                            if (equippedItems.getItemInSlot(index) != null && equippedItems.getItemInSlot(index).getKey().Equals(itemName))
                            {
                                equippedItems.unequipItem(index);
                                quantity++;
                            }
                        }
                    }

                    newLine = itemName + quantitySymbol + quantity;

                    currentConversation.addRemovedLine(newLine);

                    break;

                case "takeitem":
                    //Item(string key, string loreDescription, string type, string subtype)

                    string itemKey = getArgument(buffer);
                    int itemQuantity = getArgumentInt(buffer, Constants.indexOne);

                    takeItem(itemKey, itemQuantity);

                    break;

                case "addxp":

                    int earnedXP = getArgumentInt(buffer, Constants.indexZero);

                    if(getNumberOfArgs(buffer) > 1)
                    {
                        int levelReq = getArgumentInt(buffer, Constants.indexOne);
                        
                        if(PartyStats.getPartyLevel() > levelReq)
                        {
                            earnedXP /= 2;
                        }
                    }

                    PartyManager.addXP(earnedXP);

                    newLine = earnedXP + " experience points";

                    currentConversation.addEarnedLine(newLine);

                    break;

                case "exchangeitemforxp":

                    itemKey = getArgument(buffer);
                    itemQuantity = getArgumentInt(buffer, Constants.indexOne);
                    earnedXP = getArgumentInt(buffer, Constants.indexTwo);

                    takeItem(itemKey, itemQuantity);

                    PartyManager.addXP(earnedXP);

                    newLine = earnedXP + " experience points";

                    currentConversation.addEarnedLine(newLine);

                    break;

                case "searchinventoryfor":

                    playerHasItem(buffer);

                    continueStory();

                    break;

                case "healparty":

                    PartyManager.healAllPartyMembersToFull();

                    continueStory();

                    break;

                case "restparty":

                    endDialogue();

                    // State.playerStats.modifyCurrentHealth(State.playerStats.getTotalHealth(), true);

                    PartyManager.healAllPartyMembersToFull();

                    AudioManager.playRestSFX();

                    if (!FadeToBlackManager.isMidScreenFade())
                    {
                        fadeToBlackManager.setAndStartFadeBackIn();
                    }
                    else
                    {
                        fadeToBlackManager.setAndStartFadeToBlack();
                    }

                    return;

                case "setpartymemberhealth":                    
                    partyMemberName = getArgument(buffer, Constants.indexZero);
                    int newHealth = getArgumentInt(buffer, Constants.indexOne);

                    PartyMember companion = PartyManager.getPartyMember(partyMemberName);

                    companion.stats.currentHealth = newHealth;

                    continueStory();

                    break;
                case "dealdamagesafe":       

                    partyMemberName = getArgument(buffer, Constants.indexZero);
                    int damage = getArgumentInt(buffer, Constants.indexOne);

                    if(partyMemberName.Equals(PartyManager.getPlayerNameForDisplay()))
                    {
                        companion = PartyManager.getPlayer();
                    } else
                    {
                        companion = PartyManager.getPartyMember(partyMemberName);
                    }

                    if(companion.stats.currentHealth > damage)
                    {
                        companion.stats.modifyCurrentHealth(damage);
                    } else
                    {
                        companion.stats.currentHealth = Constants.sizeOne;
                    }

                    continueStory();

                    break;
                case "quickfadetoblack":
                    fadeToBlackCommand(quickFade: true);
                    return;
                case "fadetoblack":

                    fadeToBlackCommand();
                    return;

                case "manualfadetoblack":

                    fadeToBlackCommand(manual: true);
                    return;


                case "fadebackin": //fadeBackIn(int framesToWait), 
                                   //fadeBackIn(int framesToWait, bool continueAfterTransparent)

                    string[] fadeBackInArgs = getAllArgs(buffer);


                    if (fadeBackInArgs.Length == 0 || fadeBackInArgs[Constants.indexZero] == "")
                    {
                        framesToWait = 0;
                    }
                    else
                    {
                        framesToWait = int.Parse(fadeBackInArgs[Constants.indexZero]);
                    }

                    frames = 0;

                    bool continueAfterTransparent = false;

                    if (fadeBackInArgs.Length > 1)
                    {
                        continueAfterTransparent = bool.Parse(fadeBackInArgs[Constants.indexOne]);
                    }

                    StartCoroutine(fadeBackIn(continueAfterTransparent));

                    return;


                case "slowfadebackin": //slowFadeBackIn(int fadeTimeMultiplier) 

                    int multiplier = getArgumentInt(buffer);

                    StartCoroutine(slowFadeBackIn(multiplier));

                    return;

                case "wait": //wait(float seconds) 

                    float seconds = getArgumentFloat(buffer);

                    StartCoroutine(waitInSeconds(seconds));

                    return;

                case "stopfades": 
                case "stopallfades": 

                    FadeToBlackManager.StopFade(FadeType.Screen);
                    
                    continueStory();

                    break;

                case "movepos":
                case "moveposition":
                case "movetopos":
                case "movetoposition":
                case "moveplayer":
                case "moveplayerpos":
                case "moveplayerposition":
                case "changepos":
                case "changeposition":
                case "changeplayerpos":
                case "changeplayerposition":

                    int xPos = getArgumentInt(buffer, Constants.indexZero);
                    int yPos = getArgumentInt(buffer, Constants.indexOne);

                    Vector3Int targetCellCoords = new Vector3Int(xPos, yPos);

                    PlayerObject.getInstanceTransform().position = AreaManager.getMasterGrid().GetCellCenterWorld(targetCellCoords);

                    PlayerMovement.updateStartEndPosition();

                    Helpers.updateColliderPosition(PlayerObject.getInstanceTransform());

                    PartyMemberTrainManager.createPartyMemberTrain();

                    continueStory();

                    break;
                case "setfacing":
                case "setplayerfacing":
                case "changefacing":
                case "changeplayerfacing":

                    string facingArgs = getArgument(buffer);

                    switch (facingArgs.ToLower().Replace(" ",""))
                    {
                        case "ne":
                        case "northeast":
                            PlayerMovement.getInstance().getAnimationManager().setFacing(Facing.NorthEast);
                            break;
                        case "nw":
                        case "northwest":
                            PlayerMovement.getInstance().getAnimationManager().setFacing(Facing.NorthWest);
                            break;
                        case "se":
                        case "southeast":
                            PlayerMovement.getInstance().getAnimationManager().setFacing(Facing.SouthEast);
                            break;
                        case "sw":
                        case "southwest":
                            PlayerMovement.getInstance().getAnimationManager().setFacing(Facing.SouthWest);
                            break;
                    }

                    continueStory();

                    break;

                case "setnpcfacing":
                case "changenpcfacing":

                    camTargetIndex = getArgumentInt(buffer, Constants.indexZero);
                    string npcFacingArgs = getArgument(buffer, Constants.indexOne);

                    targetAnimationManager = currentDialogue.cameraFoci[camTargetIndex].GetComponent<AnimationManager>();

                    if(targetAnimationManager != null)
                    {
                        switch (npcFacingArgs.ToLower().Replace(" ",""))
                        {
                            case "ne":
                            case "northeast":
                                targetAnimationManager.setFacing(Facing.NorthEast);
                                break;
                            case "nw":
                            case "northwest":
                                targetAnimationManager.setFacing(Facing.NorthWest);
                                break;
                            case "se":
                            case "southeast":
                                targetAnimationManager.setFacing(Facing.SouthEast);
                                break;
                            case "sw":
                            case "southwest":
                                targetAnimationManager.setFacing(Facing.SouthWest);
                                break;
                        }
                    }

                    continueStory();

                    break;

                case "faceplayer":
                case "faceoppositeplayer":

                    camTargetIndex = getArgumentInt(buffer, Constants.indexZero);
                    DialogueTrigger dialogueTrigger = currentDialogue.cameraFoci[camTargetIndex].GetComponent<DialogueTrigger>();

                    if(dialogueTrigger != null)
                    {
                        dialogueTrigger.setFacing();
                    }

                    continueStory();

                    break;
                case "playanimation":

                    camTargetIndex = getArgumentInt(buffer, Constants.indexZero);
                    string npcAnimationArgs = getArgument(buffer, Constants.indexOne);

                    targetAnimationManager = currentDialogue.cameraFoci[camTargetIndex].GetComponent<AnimationManager>();

                    if(targetAnimationManager != null && Enum.TryParse(npcAnimationArgs, ignoreCase: true, out CharacterAnimationType animationType))
                    {
                        switch (animationType)
                        {
                            case CharacterAnimationType.Attack_Normal:
                            case CharacterAnimationType.Attack_Normal_Front:
                                targetAnimationManager.playAttackFrontAnimation();
                                break;
                            case CharacterAnimationType.Attack_Normal_Back:
                                targetAnimationManager.playAttackBackAnimation();
                                break;
                            case CharacterAnimationType.Idle_Back:
                                targetAnimationManager.setCurrentIdle(CharacterAnimationType.Idle_Back);
                                break;
                            case CharacterAnimationType.Idle_Front:
                                targetAnimationManager.setCurrentIdle(CharacterAnimationType.Idle_Front);
                                break;
                            case CharacterAnimationType.OOC_Idle_Back:
                                targetAnimationManager.setCurrentIdle(CharacterAnimationType.OOC_Idle_Back);
                                break;
                            case CharacterAnimationType.OOC_Idle_Front:
                                targetAnimationManager.setCurrentIdle(CharacterAnimationType.OOC_Idle_Front);
                                break;
                            case CharacterAnimationType.Secondary_Idle:
                                targetAnimationManager.setCurrentIdle(CharacterAnimationType.Secondary_Idle);
                                break;
                            case CharacterAnimationType.Secondary_Idle_Back:
                                targetAnimationManager.setCurrentIdle(CharacterAnimationType.Secondary_Idle_Back);
                                break;
                            case CharacterAnimationType.Secondary_Idle_Front:
                                targetAnimationManager.setCurrentIdle(CharacterAnimationType.Secondary_Idle_Front);
                                break;
                          case CharacterAnimationType.Death:
                                targetAnimationManager.setCurrentIdle(CharacterAnimationType.Death);
                                targetAnimationManager.playDeathAnimationThenHide();
                                break;
                          case CharacterAnimationType.Secondary_Death:
                                targetAnimationManager.setCurrentIdle(CharacterAnimationType.Secondary_Death);
                                targetAnimationManager.playSecondaryDeathAnimation();
                                break;
                            case CharacterAnimationType.Death_Back:
                                targetAnimationManager.setCurrentIdle(CharacterAnimationType.Death_Back);
                                break;
                            case CharacterAnimationType.Death_Front:
                                targetAnimationManager.setCurrentIdle(CharacterAnimationType.Death_Front);
                                targetAnimationManager.playDeathAnimationThenHide();
                                break;
                            case CharacterAnimationType.StandUp:
                                targetAnimationManager.playAnimation(CharacterAnimationType.StandUp);
                                break;
                            case CharacterAnimationType.Death_Front_Weaponless:
                                targetAnimationManager.setCurrentIdle(CharacterAnimationType.Death_Front_Weaponless);
                                break;
                            case CharacterAnimationType.Death_Back_Weaponless:
                                targetAnimationManager.setCurrentIdle(CharacterAnimationType.Death_Back_Weaponless);
                                break;
                            case CharacterAnimationType.Wounded_Back:
                                targetAnimationManager.playAnimation(CharacterAnimationType.Wounded_Back);
                                break;
                            case CharacterAnimationType.Wounded_Front:
                                targetAnimationManager.playAnimation(CharacterAnimationType.Wounded_Front);
                                break;
                            case CharacterAnimationType.OOC_Wounded_Back:
                                targetAnimationManager.playAnimation(CharacterAnimationType.OOC_Wounded_Back);
                                break;
                            case CharacterAnimationType.OOC_Wounded_Front:
                                targetAnimationManager.playAnimation(CharacterAnimationType.OOC_Wounded_Front);
                                break;
                        }
                    } 

                    continueStory();

                    break;

                case "startfall":

                    camTargetIndex = getArgumentInt(buffer, Constants.indexZero);
                    Vector2Int startCoords = new Vector2Int(getArgumentInt(buffer, Constants.indexOne), getArgumentInt(buffer, Constants.indexTwo));
                    Vector2Int endCoords = new Vector2Int(getArgumentInt(buffer, Constants.indexThree), getArgumentInt(buffer, Constants.indexFour));
                    float durationSeconds = getArgumentFloat(buffer, Constants.indexFive);
                    bool disableAtEnd = getArgumentBool(buffer, Constants.indexSix);
                    string effectName = getArgument(buffer, Constants.indexSeven);

                    currentDialogue.cameraFoci[camTargetIndex].SetActive(true);

                    FallingNPCMovement targetFallingNPC = currentDialogue.cameraFoci[camTargetIndex].GetComponent<FallingNPCMovement>();
                    
                    if(targetFallingNPC == null)
                    {
                        targetFallingNPC = currentDialogue.cameraFoci[camTargetIndex].AddComponent<FallingNPCMovement>();
                    }

                    targetAnimationManager = currentDialogue.cameraFoci[camTargetIndex].GetComponent<AnimationManager>();

                    if(targetAnimationManager != null)
                    {
                        targetAnimationManager.disableExtras();
                    }

                    targetFallingNPC.moveBetweenCells(startCoords, endCoords, durationSeconds, disableAtEnd, effectAtEndName: effectName);

                    continueStory();

                    break;

                case "disableextras":
                case "hideextras":
                case "removespriteextras":

                    camTargetIndex = getArgumentInt(buffer, Constants.indexZero);

                    AnimationManager animationManager = currentDialogue.cameraFoci[camTargetIndex].GetComponent<AnimationManager>();

                    if(animationManager != null)
                    {
                        animationManager.disableExtras();
                    }

                    continueStory();

                    break;

                case "resetidledictentry":
                case "resetidledictionaryentry":

                    camTargetIndex = getArgumentInt(buffer, Constants.indexZero);

                    AnimationManager.resetIdleDictionaryEntry(currentDialogue.names[camTargetIndex]);

                    continueStory();
                    break;

                case "createstapledeffectbyname":
                case "createeffectonnpcsbyname":

                    string npcName = getArgument(buffer, Constants.indexZero);
                    effectName = getArgument(buffer, Constants.indexOne);

                    if(Enum.TryParse(effectName, ignoreCase: true, out EffectAnimationType effectType))
                    {
                        AnimationManager.CreateEffectByNPCName.Invoke(npcName, effectType);
                    }

                    continueStory();

                    break;

                case "hideallstapledeffects":

                    AnimationManager.HideAllStapledEffects.Invoke();

                    continueStory();

                    break;

                case "showstapledeffectbyname":

                    npcName = getArgument(buffer, Constants.indexZero);
                    effectName = getArgument(buffer, Constants.indexOne);

                    if(Enum.TryParse(effectName, ignoreCase: true, out effectType))
                    {
                        AnimationManager.ShowStapledEffectByNPCName.Invoke(npcName, effectType);
                    }

                    continueStory();

                    break;

                case "setidleofnpcsbyname":

                    npcName = getArgument(buffer, Constants.indexZero);
                    string idleName = getArgument(buffer, Constants.indexOne);

                    if(Enum.TryParse(idleName, ignoreCase: true, out CharacterAnimationType idleType))
                    {
                        AnimationManager.SetIdleByNPCName.Invoke(npcName, idleType);
                    }

                    continueStory();

                    break;

                case "playanimationthenfade":

                    if (dialogueTrackerWindow != null)
                    {
                        dialogueTrackerWindow.gameObject.SetActive(false);
                    }

                    camTargetIndex = getArgumentInt(buffer, Constants.indexZero);
                    string animThenFadeArg = getArgument(buffer, Constants.indexOne);

                    targetAnimationManager = currentDialogue.cameraFoci[camTargetIndex].GetComponent<AnimationManager>();

                    if(targetAnimationManager != null && 
                        Enum.TryParse(animThenFadeArg, ignoreCase: true, out CharacterAnimationType animThenFadeType))
                    {
                        if(animThenFadeType != CharacterAnimationType.None)
                        {
                            StartCoroutine(playAnimationThenFadeToBlack(targetAnimationManager, animThenFadeType));
                        }
                        else
                        {
                            continueStory();
                        }
                    }
                    else
                    {
                        continueStory();
                    }

                    return;

                case "playdelayedanimation":
                case "playanimationwithdelay":

                    camTargetIndex = getArgumentInt(buffer, Constants.indexZero);
                    parameter = getArgument(buffer, Constants.indexOne);
                    intParameter = getArgumentInt(buffer, Constants.indexTwo);

                    float secondsToWait = ((float) intParameter)/1000f;

                    targetAnimationManager = currentDialogue.cameraFoci[camTargetIndex].GetComponent<AnimationManager>();

                    AnimationDelegate<AnimationManager> animationDelegate = null;

                    switch (parameter.ToLower().Replace(" ",""))
                    {
                        case "wounded":
                            animationDelegate = t => t.playWoundedAnimation();
                            break;
                    }

                    if(animationDelegate != null && targetAnimationManager != null)
                    {
                        StartCoroutine(waitThenPlayAnimation(secondsToWait, animationDelegate, targetAnimationManager));
                    }

                    continueStory();

                    break;

                case "playdelayedsfx":
                case "playsfxwithdelay":

                    string npcSFXArgs = getArgument(buffer, Constants.indexZero);
                    intParameter = getArgumentInt(buffer, Constants.indexOne);
                    secondsToWait = ((float) intParameter)/1000f;

                    if(Enum.TryParse(npcSFXArgs, ignoreCase: true, out SFXType sfxType))
                    {
                        StartCoroutine(waitThenPlaySFX(secondsToWait, sfxType));
                    }

                    continueStory();

                    break;

                case "playcrowdambience":

                    AudioManager.playCrowdAmbience();

                    continueStory();

                    break;

                case "endambience":

                    AudioManager.endAmbience();

                    continueStory();

                    break;

                case "adjustgridsquare":

                    Facing facingDirection = State.playerFacing.getFacing();
                    int adjustmentMagnitude = getArgumentInt(buffer, Constants.indexZero) + 1;
                    Vector3Int gridSquareAdjustment = Vector3Int.zero;

                    if (facingDirection == Facing.NorthEast)
                    {
                        gridSquareAdjustment.x = adjustmentMagnitude;

                    }
                    else if (facingDirection == Facing.NorthWest)
                    {
                        gridSquareAdjustment.y = adjustmentMagnitude;

                    }
                    else if (facingDirection == Facing.SouthWest)
                    {
                        gridSquareAdjustment.x = adjustmentMagnitude * -1;

                    }
                    else if (facingDirection == Facing.SouthEast)
                    {
                        gridSquareAdjustment.y = adjustmentMagnitude * -1;
                    }

                    PlayerMovement player = PlayerMovement.getInstance();

                    Vector3Int newPlayerGridSquare = PlayerMovement.getMovementGridCoords() + gridSquareAdjustment;

                    PlayerObject.getInstanceTransform().position = player.convertGridCoordsToWorldPos(newPlayerGridSquare);

                    PlayerMovement.updateStartEndPosition();

                    Helpers.updateColliderPosition(PlayerObject.getInstanceTransform());

                    PartyMemberTrainManager.createPartyMemberTrain();

                    continueStory();

                    break;

                case "enabledialogueui":

                    if (dialogueTrackerWindow == null)
                    {
                        // Debug.LogError("DialogueTrackerWindow is null in enableDialogueUI()");
                        dialogueTrackerButton.spawnEmptyPopUp();
                        dialogueTrackerWindow = (DialogueTrackerWindow)dialogueTrackerButton.getPopUpWindow();
                    }

                    currentConversation.setAttachedWindow(dialogueTrackerWindow);

                    dialogueTrackerWindow.gameObject.SetActive(true);

                    continueStory();

                    break;

                case "disabledialogueui":

                    if (dialogueTrackerWindow == null)
                    {
                        // Debug.LogError("DialogueTrackerWindow is null in enableDialogueUI()");
                        dialogueTrackerButton.spawnEmptyPopUp();
                        dialogueTrackerWindow = (DialogueTrackerWindow) dialogueTrackerButton.getPopUpWindow();
                    }

                    currentConversation.setAttachedWindow(dialogueTrackerWindow);

                    dialogueTrackerWindow.gameObject.SetActive(false);

                    continueStory();

                    break;

                case "entershopmode":

                    EscapeStack.escapeAll();

                    ShopPopUpButton shopPopUpButton = new ShopPopUpButton();
                    Shopkeeper shopkeeper = currentDialogue.cameraFoci[Dialogue.mainNPCIndex].GetComponent<Shopkeeper>();

                    endDialogue();

                    shopPopUpButton.spawnPopUp(shopkeeper);
                    shopPopUpButton.getCurrentPopUpGameObject().SetActive(true);

                    return;

                case "execute":

                    execute(buffer);

                    return;

                case "executeleavebody":
                case "executeleavebodies":

                    execute(buffer, leaveDeadBodies: true);

                    return;

                case "mob":

                    int mobDeadNameIndex = getArgumentInt(buffer, Constants.indexZero);

                    GameObject mobTarget = changeCameraTarget(mobDeadNameIndex);

                    DeathFlagManager.addName(currentDialogue.names[mobDeadNameIndex]);

                    StartCoroutine(handleMob(mobTarget));

                    return;

                case "kill":

                    int deadNameIndex = getArgumentInt(buffer, Constants.indexZero);

                    DeathFlagManager.addName(currentDialogue.names[deadNameIndex]);

                    currentDialogue.cameraFoci[deadNameIndex].SetActive(false);

                    continueStory();

                    break;

                case "killwithoutdeactivation":

                    DeathFlagManager.addName(currentDialogue.names[getArgumentInt(buffer, Constants.indexZero)]);

                    continueStory();

                    break;

                case "adddeathflag":

                    string deadName = getArgument(buffer);

                    DeathFlagManager.addName(deadName);

                    continueStory();

                    break;
                case "addtoparty":
                case "addtopartywithoutpopup":          

                    partyMemberName = DialogueList.scrubNameOfEndNumbers(currentDialogue.names[getArgumentInt(buffer, Constants.indexZero)]);

                    PartyMember newPartyMember = PartyManager.getPartyMember(partyMemberName);

                    newPartyMember.canJoinParty = true;
                    newPartyMember.stats.setXPToPlayerLevel();

                    Formation formation = State.formation;

                    if (!formation.isFull())
                    {
                        AllyStats partyMemberStats = PartyManager.getPartyMember(partyMemberName).stats;

                        if (formation.getStatsAtCoords(0, 2) == null)
                        {
                            formation.setCharacterAtCoords(0, 2, partyMemberStats);
                        }
                        else
                        {
                            formation.addAllyInFirstOpenSpace(partyMemberStats);
                        }
                    }

                    continueStory();

                    break;
            
                case "addtopartybutnotformation":

                    partyMemberName = DialogueList.scrubNameOfEndNumbers(currentDialogue.names[getArgumentInt(buffer, Constants.indexZero)]);


                    newPartyMember = PartyManager.getPartyMember(partyMemberName);

                    newPartyMember.canJoinParty = true;
                    newPartyMember.stats.setXPToPlayerLevel();

                    continueStory();
                    break;

                case "removefromparty":

                    int nameIndex = getArgumentInt(buffer, Constants.indexZero);
                    bool preventInventoryLoss = getArgumentBool(buffer, Constants.indexOne);

                    partyMemberName = currentDialogue.names[nameIndex];

                    if(partyMemberName.Contains(NPCNameList.overseer) || 
                        partyMemberName.Contains(NPCNameList.chief))
                    {
                        partyMemberName = partyMemberName.Split(" ")[1];
                    }

                    PartyMember partyMemberToRemove = PartyManager.getPartyMember(partyMemberName);

                    if(!partyMemberToRemove.canJoinParty)
                    {
                        continueStory();
                        break;
                    }

                    partyMemberToRemove.canJoinParty = false;

                    if(!preventInventoryLoss)
                    {
                        partyMemberToRemove.stats.equippedItems.unequipAllItems();
                        partyMemberToRemove.stats.combatActionArray.unequipAllActions();
                    }

                    if (State.formation != null)
                    {
                        State.formation.removePartyMember(partyMemberName);
                    }

                    currentConversation.addLeftPartyLine(partyMemberName);

                    PartyMemberTrainManager.createPartyMemberTrain();

                    continueStory();

                    break;
                case "opengate":

                    Gate gate = currentDialogue.cameraFoci[getArgumentInt(buffer, Constants.indexZero)].GetComponent<Gate>();

                    GateAndChestManager.addKey(gate.getGateKey());

                    continueStory();

                    break;
                case "opengatewithkey":
                case "opengatefromkey":

                    string gateKey = getArgument(buffer, Constants.indexZero);
                    GateAndChestManager.preventSFX = getArgumentBool(buffer, Constants.indexOne);

                    GateAndChestManager.addKey(AreaManager.locationName + gateKey);

                    GateAndChestManager.preventSFX = false;

                    continueStory();

                    break;

                case "setgatetoopenwithoutopening":

                    gateKey = getArgument(buffer, Constants.indexZero);

                    GateAndChestManager.addKey(AreaManager.locationName + gateKey, invoke: false);

                    continueStory();

                    break;

                case "opengatewithkeyanylocation":
                case "opengatefromkeyanylocation":

                    gateKey = getArgument(buffer, Constants.indexZero);

                    GateAndChestManager.addKey(gateKey);

                    continueStory();

                    break;

                case "explodeanddie":

                    GameObject effectGO = Instantiate(Resources.Load<GameObject>(PrefabNames.effect), PlayerObject.getInstanceTransform());

                    EffectAnimationManager effect = effectGO.GetComponent<EffectAnimationManager>();

                    effect.setAnimations(EffectAnimationType.BlastingJelly);
                    
                    PlayerObject.playDeathAnimation();

                    AudioManager.playJellyMisfireSFX();

                    AudioManager.playDefeatMusic();

                    NotificationManager.purgeNotifications();
                    
                    PlayerOOCStateManager.setCurrentActivity(OOCActivity.Defeat);
                    
                    continueStory();
                    
                    PlayerObject.spawnGameOverPopUp();
                    break;

                case "createeffect":

                    effectName = getArgument(buffer, Constants.indexZero);
                    xPos = getArgumentInt(buffer, Constants.indexOne);
                    yPos = getArgumentInt(buffer, Constants.indexTwo);
                    bool loopEffect = getArgumentBool(buffer, Constants.indexThree);

                    targetCellCoords = new Vector3Int(xPos, yPos);

                    effectGO = Instantiate(Resources.Load<GameObject>(PrefabNames.effect), PlayerObject.getInstanceTransform());

                    effectGO.transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(targetCellCoords);

                    effect = effectGO.GetComponent<EffectAnimationManager>();
                    effect.loops = loopEffect;
                    
                    effect.setAnimations(effectName);
                    
                    continueStory();
                    
                    break;

                case "destroyeffect":
                case "destroyeffects":
                case "destroyalleffects":

                    effectName = getArgument(buffer, Constants.indexZero);
                    
                    if(Enum.TryParse(effectName, ignoreCase: true, out effectType))
                    {
                        EffectAnimationManager.DestroyAllEffectsOfType.Invoke(effectType);
                    }

                    continueStory();

                    break;

                case "defeatmonster":

                    string locationName = getArgument(buffer, Constants.indexZero);
                    int monsterIndex = getArgumentInt(buffer, Constants.indexOne);
                    bool defeated = getArgumentBool(buffer, Constants.indexTwo);

			        MonsterDefeatKeysList.setDefeatKey(locationName + "-" + monsterIndex, defeated);
                    EnemyMovement.OnOOCMonsterDefeat.Invoke();

                    continueStory();
                    break;
                case "swapinkfile": //swapInkFiles(int secondaryInkFileIndex, string startingBoolName)
                case "swapinkfiles": //swapInkFiles(int secondaryInkFileIndex, string startingBoolName, bool safeToSwapDialogueObjects)

                    int secondaryInkFileIndex = getArgumentInt(buffer, Constants.indexZero);
                    string startingBoolName = getArgument(buffer, Constants.indexOne);
                                                      //if you want to start at the correct knot, 
                                                       //you need to create/give a bool that tells 
                                                       //the dialogue at the start to move to that 
                                                       //knot. see the transition between MinersDialogue
                                                       // and MarcosDialoge in MineLvl_3-Miners Camp
                    bool safeToSwapDialogueObjects = getArgumentBool(buffer, Constants.indexTwo);

                    currentStory = new Story(getSecondaryStoryJSON(secondaryInkFileIndex).text);

                    currentStory = addAllVariables(currentStory, currentDialogue.variableSources);
                    currentStory.variablesState[startingBoolName.Replace(" ", "")] = true;

                    storyName = getSecondaryStoryJSON(secondaryInkFileIndex).name;
                    atConvoEndPoint = currentDialogue.convoEndableAtStart;

                    if (safeToSwapDialogueObjects)
                    {
                        currentDialogue = currentDialogue.cameraFoci[1].GetComponent<DialogueTrigger>().dialogue;
                    }

                    continueStory();

                    break;
                case "getnewdialoguefromlist":

                    int argAmount = getNumberOfArgs(buffer);
                    string dialogueKey = getArgument(buffer, Constants.indexZero);

                    if(argAmount > 1)
                    {
                        bool wipeConversation = getArgumentBool(buffer, Constants.indexOne);
                        if(wipeConversation)
                        {
                            DialogueTrackerWindow.wipeDialogue();
                        }
                    }

                    startDialogue(DialogueList.getDialogue(dialogueKey), midDialogue: true); 
                    
                    if(argAmount > 2)
                    {
                        startingBoolName = getArgument(buffer, Constants.indexTwo).Replace(" ", "");

                        if(currentStory.variablesState[startingBoolName] != null)
                        {
                            currentStory.variablesState[startingBoolName] = true;
                        }
                    }

                    continueStory();

                    return;
                case "setdialogueuponsceneloadkey":

                    dialogueKey = getArgument(buffer, Constants.indexZero);

                    State.dialogueUponSceneLoadKey = dialogueKey;

                    StartCoroutine(waitForNewLocationThenStartDialogueOnFadeIn());

                    continueStory();

                    return;
                case "entercombat":

                    int enemyPackInfoIndex = getArgumentInt(buffer, Constants.indexZero);

                    NPCCombatInfo npcCombatInfo = currentDialogue.npcCombatInfo;

                    State.enemyPackInfo = npcCombatInfo.getEnemyInfo(enemyPackInfoIndex);
                    State.allyPackInfo = AllyPackInfoList.defaultAllyPackInfoByZone();

                    AudioManager.playBattleMusic();

                    NotificationManager.skipNextNotificationSpawn();
                    CombatStateManager.whoIsSurprised = SurpriseState.NoOneSurprised;

                    if (!currentDialogue.npcCombatInfo.ignoreDeathFlags)
                    {

                        if (!currentDialogue.npcCombatInfo.hasDeadNames())
                        {
                            currentDialogue.npcCombatInfo.deadNameList = new DeadNameList[1];
                            currentDialogue.npcCombatInfo.deadNameList[0] = new DeadNameList(new string[] { currentDialogue.names[1] });
                        }

                        currentDialogue.npcCombatInfo.addAllDeadNames(enemyPackInfoIndex);
                    }

                    QuestList.checkForDeadNames();

                    CombatStateManager.locationBeforeCombat = AreaManager.locationName;

                    endDialogue();

                    if (getAllArgs(buffer).Length > 1)
                    {
                        State.dialogueUponSceneLoadKey = getArgument(buffer, Constants.indexOne);
                    }

                    State.enteredCombatFromDialogue = true;

                    framingTransposer.m_XDamping = dialogueXDamping * 1.75f;
                    framingTransposer.m_YDamping = dialogueYDamping * 1.75f;
                    mainCM.Follow = PlayerObject.getInstanceTransform();
                    StartCoroutine(waitForCombatTransitionThenLoadCombat());
                    break;

                case "activatehostilityscript":

                    HostilityScriptList.runScript(getArgument(buffer));

                    continueStory();
                    break;


                case "setareatosafe":
                    string locationToBecomeSafe = getArgument(buffer);

                    AreaList.setAreaToSafe(locationToBecomeSafe);

                    continueStory();
                    break;

                case "setareatohostile":
                    string locationToBecomeHostile = getArgument(buffer);

                    AreaList.setAreaToHostile(locationToBecomeHostile);

                    continueStory();
                    break;

                case "setareatopassive":
                    string sceneToBecomePassive = getArgument(buffer);

                    AreaList.setAreaToPassive(sceneToBecomePassive);

                    continueStory();
                    break;

                case "addhostilitytocurrentarea":
                    AreaList.incrementHostility();

                    continueStory();
                    break;

                case "hidetrain":

                    PartyMemberTrainManager.createPartyMemberTrain();

                    continueStory();
                    break;

                case "starttutorial":

                    tutorialKey = getArgument(buffer);

                    if (tutorialWaitCoroutine != null)
                    {
                        StopCoroutine(tutorialWaitCoroutine);
                    }

                    tutorialWaitCoroutine = startTutorialAtDialogueEnd(tutorialKey, OOCActivity.walking);
                    StartCoroutine(tutorialWaitCoroutine);

                    NotificationManager.skipNextNotificationSpawn();

                    continueStory();
                    break;

                case "startuitutorial": //start UI tutorial

                    tutorialKey = getArgument(buffer);

                    if (tutorialWaitCoroutine != null)
                    {
                        StopCoroutine(tutorialWaitCoroutine);
                    }

                    tutorialWaitCoroutine = startTutorialAtDialogueEnd(tutorialKey, OOCActivity.inUI);
                    StartCoroutine(tutorialWaitCoroutine);

                    NotificationManager.skipNextNotificationSpawn();

                    continueStory();
                    break;

                case "addfloatingpromptmessage":
                case "setfloatingpromptmessage":
                case "createfloatingpromptmessage":
                case "addpromptmessage":
                case "setpromptmessage":
                case "createpromptmessage":

                    string promptMessage = getArgument(buffer);

                    PlayerObject.createCustomButtonPrompt(promptMessage);
                    WASDPromptStepCounter.createStepCounter();

                    continueStory();
                    break;

                case "changelocation":

                    string destinationName = getArgument(buffer);

                    endDialogue();

                    TransitionManager.changeLocation(new Transition(AreaManager.locationName, destinationName, Transition.ladderTransition));

                    return;

                case "close":

                    endDialogue();

                    break;

                default:
                    currentConversation.addDialogueLine(getNameTextWithoutColon(), buffer);
                    break;
            }

            if (addItemText)
            {
                addItemText = false;
                continueStory();

            }
            else if (!addItemText && buffer.Split(")").Length > 1 && buffer.Split(")")[1].Contains("&"))
            {
                continueStory();
            }

            displayChoices();
        }
    }

    private void takeItem(string itemKey, int itemQuantity)
    {
        Inventory.removeItem(itemKey, itemQuantity);
        Inventory.removeItem(itemKey, itemQuantity, State.junkPocket);

        currentConversation.addRemovedLine(itemKey + quantitySymbol + itemQuantity);
    }

    private GameObject changeCameraTarget(int targetIndex)
    {
        setNameText(targetIndex);
        
        if (targetIndex == 0)
        {
            mainCM.Follow = PlayerMovement.getInstance().gameObject.transform;
        }
        else
        {
            mainCM.Follow = currentDialogue.cameraFoci[targetIndex].transform;

        }

        return mainCM.Follow.gameObject;
    }

    private void fadeToBlackCommand(bool quickFade = false, bool manual = false)
    {
        string[] fadeToBlackArgs = getAllArgs(buffer);

        bool setDialogueUIActiveAfterFadeIn = true;
        bool continueAfterTransparent = true;

        if (fadeToBlackArgs.Length > 0 && fadeToBlackArgs[Constants.indexZero].Length > 0)
        {
            setDialogueUIActiveAfterFadeIn = bool.Parse(fadeToBlackArgs[Constants.indexZero]);
        }

        if (fadeToBlackArgs.Length > 1 && fadeToBlackArgs[Constants.indexOne].Length > 1)
        {
            continueAfterTransparent = bool.Parse(fadeToBlackArgs[Constants.indexOne]);
        }

        setCameraToDefaultSpeed();

        if(quickFade)
        {
            fadeToBlackManager.quickFadeToBlack();
        } else
        {
            fadeToBlackManager.setAndStartFadeToBlack();
        }
        waitingOnFadeToBlack = true;

        if(!manual)
        {
            StartCoroutine(handleDialogueUIDuringFadeOut(setDialogueUIActiveAfterFadeIn, continueAfterTransparent));
        }
    }

    private void playerHasItem(string buffer)
    {
        string[] args = getAllArgs(buffer);

        bool flagStatus = false;

        if (args.Length == 2)
        {
            string itemName = args[Constants.indexOne];
            flagStatus = Inventory.inventoryContainsItem(itemName) ||
                         Inventory.equipmentContainsItem(itemName) ||
                         Inventory.junkContainsItem(itemName);
        }
        else if (args.Length == 3)
        {
            string itemSubtype = args[Constants.indexOne];
            string itemID = args[Constants.indexTwo];
            flagStatus = Inventory.inventoryContainsItem(itemSubtype, int.Parse(itemID));
        }

        string flagName = args[Constants.indexZero];

        Flags.setFlag(flagName, flagStatus);
        currentStory.variablesState[flagName] = flagStatus;
    }

    private void execute(string buffer, bool leaveDeadBodies = false)
    {
        string [] args = getAllArgs(buffer);

        List<int> allIndexes = new List<int>();
        List<GameObject> executionTargets = new List<GameObject>();

        foreach(string arg in args)
        {
            allIndexes.Add(int.Parse(arg));
        }

        changeCameraTarget(allIndexes[0]);

        foreach(int index in allIndexes)
        {
            executionTargets.Add(currentDialogue.cameraFoci[index]);    
            DeathFlagManager.addName(currentDialogue.names[index]);
        }

        StartCoroutine(handleExecution(executionTargets, leaveDeadBodies));
    }

    private Item getItemFromArgs(string[] args)
    {
        return ItemList.getItem(int.Parse(args[Constants.indexZero]),
                                int.Parse(args[Constants.indexOne]),
                                int.Parse(args[Constants.indexTwo]));
    }

    private void givePlayerItemFromDialogue(string buffer)
    {
        givePlayerItemFromDialogue(getAllArgs(buffer));
    }

    private void givePlayerItemFromDialogue(string[] args)
    {
        Item itemToGive = getItemFromArgs(args);

        Inventory.addItem(itemToGive);

        string newLine = itemToGive.getKey() + quantitySymbol + itemToGive.getQuantity();

        currentConversation.addObtainedLine(newLine);
    }

    private int getNumberOfArgs(string buffer)
    {
        return getAllArgs(buffer).Length;
    }

    private string[] getAllArgs(string buffer)
    {
        return buffer.Split("(")[1].Split(")")[0].Split(",");
    }

    private string getArgument(string buffer, int argIndex = Constants.indexZero)
    {
        string[] allArgs = getAllArgs(buffer);

        if(allArgs.Length <= argIndex)
        {
            return null;
        }

        return allArgs[argIndex];
    }

    private int getArgumentInt(string buffer, int argIndex = 0)
    {
        string[] args = getAllArgs(buffer);

        if(args.Length <= argIndex || args[argIndex].Length <= 0)
        {
            return Constants.indexOne;
        }
        
        return int.Parse(getAllArgs(buffer)[argIndex]);
    }

    private float getArgumentFloat(string buffer, int argIndex = 0)
    {
        string[] args = getAllArgs(buffer);

        if(args.Length <= argIndex || args[argIndex].Length <= 0)
        {
            return Constants.indexOne;
        }
        
        return float.Parse(getAllArgs(buffer)[argIndex]);
    }

    private bool getArgumentBool(string buffer, int argIndex)
    {
        string[] args = getAllArgs(buffer);

        if (args.Length <= argIndex)
        {
            return false;
        }

        return bool.Parse(args[argIndex]);
    }

    private string[] getAllArgumentGroups(string buffer)
    {
        return buffer.Split("(")[1].Split(")")[0].Split("|");
    }

    // private string getArgument(string buffer, int argIndex)
    // {
    //     return buffer.Split("(")[1].Split(")")[0].Split(",")[argIndex];
    // }

    // private string getArgument(string buffer, int argIndex)
    // {
    //     return buffer.Split("(")[1].Split(")")[0].Split(",")[argIndex];
    // }

    private TextAsset getSecondaryStoryJSON(int secondaryInkFileIndex)
    {
        // Helpers.debugNullCheck("currentDialogue.secondaryInkJSONs",currentDialogue.secondaryInkJSONs);
        // Debug.LogError("currentDialogue.secondaryInkJSONs.Length = " + currentDialogue.secondaryInkJSONs.Length);
        // Debug.LogError("secondaryInkFileIndex = " + secondaryInkFileIndex);
        // Helpers.debugNullCheck("currentDialogue.secondaryInkJSONs[secondaryInkFileIndex]", currentDialogue.secondaryInkJSONs[secondaryInkFileIndex]);

        if (currentDialogue.secondaryInkJSONs == null || currentDialogue.secondaryInkJSONs.Length <= secondaryInkFileIndex || currentDialogue.secondaryInkJSONs[secondaryInkFileIndex] == null)
        {
            GameObject tabor = currentDialogue.cameraFoci[1];
            DialogueTrigger trigger = tabor.GetComponent<DialogueTrigger>();
            Dialogue dialogue = trigger.getDialogue();
            TextAsset[] secondaryInkJSONs = dialogue.secondaryInkJSONs;

            return secondaryInkJSONs[secondaryInkFileIndex];
        }
        else
        {
            return currentDialogue.secondaryInkJSONs[secondaryInkFileIndex];
        }
    }

	private void checkForHaltNotificationBoolArg(string buffer, int boolIndex)
	{
        if (getArgumentBool(buffer, boolIndex))
        {
            NotificationManager.skipNextNotificationSpawn();
        }
	}

	private IEnumerator startTutorialAtDialogueEnd(string tutorialSequenceKey, OOCActivity stateToWaitFor)
	{

		while (PlayerOOCStateManager.currentActivity != stateToWaitFor)
		{
			if (PlayerOOCStateManager.currentActivity == OOCActivity.inTutorialSequence)
			{
				yield break;
			}
			else
			{
				yield return null;
			}
		}

		if (!TutorialSequence.currentlyInTutorialSequence() && TutorialSequence.startTutorialSequence(tutorialSequenceKey))
		{
			PlayerOOCStateManager.setCurrentActivity(OOCActivity.inTutorialSequence);
		} else
        {
            PlayerOOCStateManager.OnLeavingTutorialSequenceState.Invoke();
        }
	}

	public static void stopTutorials()
	{
		if(getInstance() == null)
		{
			return;
		}

		getInstance().stopAllTutorials();
		
	}

	public void stopAllTutorials()
	{
		if(getInstance() == null || getInstance().tutorialWaitCoroutine == null)
		{
			return;
		}

		StopCoroutine(getInstance().tutorialWaitCoroutine);
	}

	private IEnumerator waitForCombatTransitionThenLoadCombat()
	{
		Transform playerTransform = PlayerObject.getInstanceTransform();
		Vector3 cameraPos = mainCamera.transform.position;
		Vector3 playerPos = playerTransform.position;

		while(Mathf.Abs(cameraPos.x - playerPos.x) > 0.23f || Mathf.Abs(cameraPos.y - playerPos.y) > 0.23f)
		{
			yield return null;
			cameraPos = mainCamera.transform.position;
			playerPos = playerTransform.position;
		}

		yield return new WaitForSeconds(0.05f);

		FadeToBlackManager.startCombatTransition(playerTransform);

		while(PlayerOOCStateManager.currentActivity != OOCActivity.preCombat)
		{
			yield return null;
		}

		setCameraToDefaultSpeed();
		PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
		SceneChange.changeSceneToCombat();
	}

	private IEnumerator waitForNewLocationThenStartDialogueOnFadeIn()
	{
		string locationBeforeTransition = AreaManager.locationName;

		// Wait for the changeLocation transition to swap in the new area.
		yield return new WaitUntil(() => AreaManager.locationName != locationBeforeTransition);

		// The area swap happens while the screen is fully black; once the fade
		// back in from that black screen begins, isBlack() flips false again.
		yield return new WaitUntil(() => FadeToBlackManager.isBlack());
		yield return new WaitUntil(() => !FadeToBlackManager.isBlack());

		Start();
	}

	private IEnumerator handleDialogueUIDuringFadeOut(bool setDialogueUIActiveAfterFadeIn, bool continueAfterTransparent)
	{

		if (dialogueTrackerWindow != null)
		{
			dialogueTrackerWindow.gameObject.SetActive(false);
		}

		if (!setDialogueUIActiveAfterFadeIn)
		{
			yield break;
		}

		while (!FadeToBlackManager.isMidScreenFade())
		{
			yield return null;
		}

		while (FadeToBlackManager.isMidScreenFade())
		{
			yield return null;
		}

        if(PlayerOOCStateManager.currentActivity == OOCActivity.walking)
        {
            yield break;
        }

		if (dialogueTrackerWindow == null)
		{
			dialogueTrackerButton.spawnEmptyPopUp();
			dialogueTrackerWindow = (DialogueTrackerWindow)dialogueTrackerButton.getPopUpWindow();
		}

		currentConversation.setAttachedWindow(dialogueTrackerWindow);

		dialogueTrackerWindow.gameObject.SetActive(true);

		if (continueAfterTransparent)
		{
			// Debug.LogError("continueStory() inside handleDialogueUIDuringFadeOut");
			continueStory();
		}
	}

	private IEnumerator handleExecution(List<GameObject> targets, bool leaveDeadBodies)
	{
		if (dialogueTrackerWindow != null)
		{
			dialogueTrackerWindow.gameObject.SetActive(false);
		}

		fadeToBlackManager.setAndStartFadeToBlack();

		while (FadeToBlackManager.isMidScreenFade())
		{
			yield return null;
		}

		AudioClip executionClip = AudioClipList.getAudioClip(SFXType.Execution);
		AudioManager.playExecutionSFX();

        foreach(GameObject target in targets)
        {
            if(target == null)
            {
                continue;
            }

            if(leaveDeadBodies)
            {
                AnimationManager animationManager = target.GetComponent<AnimationManager>();

                if(animationManager == null)
                {
                    target.SetActive(false);
                    continue;
                }

                animationManager.setCurrentIdle(CharacterAnimationType.Death_Front_Weaponless);

            } else
            {
                target.SetActive(false);
            }
        }
        
		yield return new WaitForSeconds(executionClip.length);

		fadeToBlackManager.setAndStartFadeBackIn();

		while (FadeToBlackManager.isMidScreenFade())
		{
			yield return null;
		}

		if (dialogueTrackerWindow != null)
		{
		    dialogueTrackerWindow.gameObject.SetActive(true);
		}

		continueStory();
	}

	private IEnumerator handleMob(GameObject target)
	{
		bool alreadyBlack = FadeToBlackManager.isBlack();

		if (!alreadyBlack)
		{
			if (dialogueTrackerWindow != null)
			{
				dialogueTrackerWindow.gameObject.SetActive(false);
			}

			fadeToBlackManager.setAndStartFadeToBlack();

			while (FadeToBlackManager.isMidScreenFade())
			{
				yield return null;
			}
		}

        target.SetActive(false);

		AudioClip whipClip = AudioClipList.getAudioClip(SFXType.Whip);

		for (int i = 0; i < 4; i++)
		{
			AudioManager.playWhipSFX();
			yield return new WaitForSeconds(whipClip.length);
		}

		fadeToBlackManager.setAndStartFadeBackIn();

		while (FadeToBlackManager.isMidScreenFade())
		{
			yield return null;
		}

		if (dialogueTrackerWindow != null)
		{
		    dialogueTrackerWindow.gameObject.SetActive(true);
		}

		continueStory();
	}

	private IEnumerator fadeBackIn(bool continueAfterTransparent)
	{

		yield return new WaitUntil(() => frames >= framesToWait);

		fadeToBlackManager.setAndStartFadeBackIn();
		waitingOnFadeBackIn = true;

		setCameraToDialogueSpeed();

		if (continueAfterTransparent)
		{
			while (FadeToBlackManager.isMidScreenFade())
			{
				yield return null;
			}

			// Debug.LogError("inside fadeBackIn");

			continueStory();
		}
	}
	
	private IEnumerator slowFadeBackIn(int multiplier)
	{
		fadeToBlackManager.setAndStartFadeBackIn(multiplier);

        yield return null;

		yield return new WaitUntil(() => !FadeToBlackManager.isMidScreenFade());

		setCameraToDialogueSpeed();

        continueStory();
	}

	
	private IEnumerator waitInSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);

        continueStory();
	}

	private static Story addAllVariables(Story story, List<IStoryVariableSource> variableSources)
	{
		story = Flags.addAllVariables(story);
        story = MetaFlags.addAllVariables(story);
		story = DeathFlagManager.addAllVariables(story);
		story = PartyManager.addAllVariables(story);
		story = GateAndChestManager.addAllVariables(story);
		story = Purse.addCoinsToStory(story);
		story = PartyManager.getPlayerStats().addAllStats(story);
		story = PlayerMovement.addAllVariables(story);

        foreach(IStoryVariableSource source in variableSources)
        {
            story = source.addVariables(story);
        }

        return story;
	}

    private IEnumerator playAnimationThenFadeToBlack(AnimationManager targetAnimationManager, CharacterAnimationType animationType)
    {
        float animLength = targetAnimationManager.getAnimationLength(animationType);
        targetAnimationManager.playAnimation(animationType);

        yield return new WaitForSeconds(animLength + 1f);

        fadeToBlackManager.setAndStartFadeToBlack();
        waitingOnFadeToBlack = true;

        StartCoroutine(handleDialogueUIDuringFadeOut(true, true));
    }

    private static IEnumerator waitThenPlayAnimation(float secondsToWait, AnimationDelegate<AnimationManager> playAnimation, AnimationManager animationManager)
    {
        float timeWaited = 0f;

        while(timeWaited <= secondsToWait)
        {
            yield return null;

            timeWaited += Time.deltaTime;
        }

        playAnimation(animationManager);
    }

    private static IEnumerator waitThenPlaySFX(float secondsToWait, SFXType sfxType)
    {
        float timeWaited = 0f;

        while(timeWaited <= secondsToWait)
        {
            yield return null;

            timeWaited += Time.deltaTime;
        }

        AudioManager.playAudioClipAsSingleton(sfxType);
    }
}

public class Conversation
{	
	public const int maxLineCount = 500;
	public const string infoName = "Info";
	public const string earnedName = "Earned";
	public const string obtainedName = "Obtained";
	public const string removedName = "Removed";
	public const string endOfDialogueMessage = "End of Dialogue";
    public const string leftPartyMessage = " has left your party.";

    private DialogueTrackerWindow attachedWindow;
	
	private List<DialogueLine> dialogueList = new List<DialogueLine>();
	
	public Conversation()
	{
		
	}
	
	public Conversation(DialogueTrackerWindow attachedWindow)
	{
		this.attachedWindow = attachedWindow;
	}
	
	//can only use if attachedWindow == null, otherwise you may have another window out there somewhere which you shouldn't have
	public void setAttachedWindow(DialogueTrackerWindow attachedWindow)
	{
		if(this.attachedWindow == null)
		{
			this.attachedWindow = attachedWindow;
        }
	}

	public List<DialogueLine> getDialogueList()
	{
		return dialogueList;
	}
	
	public void addEndOfDialogueLine()
	{
		addDialogueLine(endOfDialogueMessage);
	}
    public void addLeftPartyLine(string partyMemberName)
    {
        addDialogueLine(DialogueList.scrubNameOfEndNumbers(partyMemberName) + leftPartyMessage);
    }

    public void addEarnedLine(string contents)
	{
		addDialogueLine(earnedName, contents);
	}
	
	public void addObtainedLine(string contents)
	{
		addDialogueLine(obtainedName, contents);
	}

	public void addRemovedLine(string contents)
	{
		addDialogueLine(removedName, contents);
	}
	
	public void addDialogueLine(string contents)
	{
		addDialogueLine(infoName, contents);
	}
	
	public void addDialogueLine(string speakerName, string contents)
	{
		addDialogueLine(new DialogueLine(speakerName, contents));
	}
	
	public void addDialogueLine(DialogueLine line)
	{
		if(dialogueList.Count == maxLineCount)
		{
			dialogueList.RemoveAt(0);
		}
		
		dialogueList.Add(line);
		showLastLine();
	}
	
	public void showLastLine()
	{
		if(attachedWindow != null && !(attachedWindow is null))
		{
			List<DialogueLine> listOfDialogue = new List<DialogueLine>();
			listOfDialogue.Add(getLastLine());
			
			attachedWindow.appendDialogue(listOfDialogue);
		}
	}
	
	public void appendConversation(Conversation newConversation)
	{
		List<DialogueLine> dialogueToAppend = newConversation.getDialogueList();
		
		if(dialogueToAppend.Count >= maxLineCount)
		{
			dialogueList = dialogueToAppend;
		} else
		{
			foreach(DialogueLine line in dialogueToAppend)
			{
				addDialogueLine(line);
			}
		}
	}
	
	public DialogueLine getLastLine()
	{
		return dialogueList[dialogueList.Count - 1];
	}
	
    public static bool nameIsUpdate(string name)
    {
        switch(name)
        {
            case infoName:
            case obtainedName:
            case removedName:
            case earnedName:
                return true;
            default:
                return false;
        }
    }
	
}

public static class SpeechLog
{	
	private static Conversation allDialogues = new Conversation();
	
	public static void appendConversation(Conversation newConversation)
	{
		allDialogues.appendConversation(newConversation);
	}
	
	public static List<DialogueLine> getDialogueList()
	{
		return allDialogues.getDialogueList();
	}
	
	public static void cleanSpeechLog()
	{
		allDialogues = new Conversation();
	}

	[RuntimeInitializeOnLoadMethod]
	private static void init()
	{
		LoadSaveFile.OnLoadResetData.AddListener(cleanSpeechLog);
	}
}
