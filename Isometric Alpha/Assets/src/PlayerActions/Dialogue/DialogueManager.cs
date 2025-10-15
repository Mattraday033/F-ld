using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ink.Runtime;
using Cinemachine;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
	private const int activateQuestStepHaltNotificationBoolIndex = 2;
	private const int finishQuestHaltNotificationBoolIndex = 3;

	private List<Choice> currentChoiceInkObjects;

    [Header("Cameras")]

    //[SerializeField] 
    public Camera mainCamera;
	//[SerializeField] 
    public CinemachineVirtualCamera mainCM;
	private CinemachineFramingTransposer framingTransposer;

    [Header("Managers")]
	//[SerializeField] 
    public OOCUIManager oocUIManager;
	private FadeToBlackManager fadeToBlackManager;

	public Transform npcParent;

	private static bool returnToRevealAfterDialogue;

	private string nameText;
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

	private const float defaultXDamping = .5f;
	private const float defaultYDamping = .5f;
	private const float dialogueXDamping = 1f;
	private const float dialogueYDamping = 1f;
	private const float fadeXDamping = 0f;
	private const float fadeYDamping = 0f;
	private int frames = 60;
	private int framesToWait = 60;

	public DialogueTrackerButton dialogueTrackerButton;
	private DialogueTrackerWindow dialogueTrackerWindow;

	private TutorialPopUpButton tutorialPopUpButton;

	private IEnumerator tutorialWaitCoroutine;

	private static bool hasQueuedPopUpCoroutine;

	void Start()
	{
		setCameras();

		oocUIManager = OOCUIManager.getInstance();
		fadeToBlackManager = FadeToBlackManager.getInstance();

		if (State.dialogueUponSceneLoadKey != null && State.dialogueUponSceneLoadKey.Length > 0)
		{
			npcParent = NPCParent.getInstance().transform;
			dialogueTrackerButton = new DialogueTrackerButton(true);

			startDialogue(DialogueList.getDialogue(State.dialogueUponSceneLoadKey));
			State.dialogueUponSceneLoadKey = null;
			return;
		}

		dialogueTrackerButton = new DialogueTrackerButton(true);
	}

	void Update() //here for Animation
	{
		if (waitingOnFadeToBlack && FadeToBlackManager.isBlack())
		{
			waitingOnFadeToBlack = false;
			continueStory();
		}

		if (waitingOnFadeBackIn && FadeToBlackManager.isTransparent())
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
//mainCM.m_Follow = PlayerMovement.getInstance().gameObject.transform;
		//dialogueCanvas.worldCamera = mainCamera;
	}

	public bool storyCanContinue()
	{
		return currentStory.canContinue;
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

	public void startDialogue(Dialogue dialogue)
	{
		//Flags.printAll();

		if (RevealManager.currentlyRevealed)
		{
			returnToRevealAfterDialogue = true;
			RevealManager.toggleReveal();
		}

		if (framingTransposer == null)
		{
			Start();
		}

		setCameraToDialogueSpeed();
		oocUIManager.disableOOCUI();

		currentDialogue = dialogue;

        findNPCGameObject();

        NPCCombatInfo combatInfo = currentDialogue.npcCombatInfo;

		nameText = DialogueList.scrubNameOfEndNumbers(currentDialogue.names[1]) + ":";
		currentStory = addAllVariables(new Story(dialogue.inkJSON.text), dialogue.variableSources);

		storyName = dialogue.inkJSON.name;
		atConvoEndPoint = dialogue.convoEndableAtStart;

        if (currentDialogue.isVaultable)
        {
            VaultableObject vaultableObject = currentDialogue.cameraFoci[1].GetComponent<VaultableObject>();

            if (currentStory.variablesState["objectName"] != null)
            {
                currentStory.variablesState["objectName"] = vaultableObject.objectName;
            }

            if (currentStory.variablesState["plural"] != null)
            {
                currentStory.variablesState["plural"] = vaultableObject.plural;
            }
        }

		if (currentStory.variablesState["attitude"] != null)
		{
			currentStory.variablesState["attitude"] = 0;
		}

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

		PlayerOOCStateManager.setCurrentActivity(OOCActivity.inDialogue);
	}

	public void endDialogue()
	{
		mainCM.m_Follow = PlayerMovement.getInstance().gameObject.transform;

		setCameraToDefaultSpeed();

		oocUIManager.enableOOCUI();

		currentDialogue = null;
		currentChoiceInkObjects = null;

		nameText = "";

		currentConversation.addEndOfDialogueLine();

		storyName = "";
		previousChoice = null;
		QuestList.checkForDeadNames();

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
		ArrayList choiceDescriptions = new ArrayList();

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

	private IEnumerator waitToSpawnPopUp(PopUpButton popUpButton)
	{
		if (hasQueuedPopUpCoroutine)
		{
			yield break;
		}
		else
		{
			hasQueuedPopUpCoroutine = true;
		}

		while (PlayerOOCStateManager.currentActivity != OOCActivity.walking)
		{
			yield return null;
		}

		popUpButton.spawnPopUp();

		PlayerOOCStateManager.setCurrentActivity(OOCActivity.inUI);

		hasQueuedPopUpCoroutine = false;
	}

	public void makeChoice(int choiceIndex)
	{
		if (currentChoiceInkObjects == null || choiceIndex >= currentChoiceInkObjects.Count)
		{
			if (choiceIndex == 0 && currentChoiceInkObjects.Count == 0)
			{
				continueStory();
			}

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

			string newLine = PartyManager.getPlayerStats().getName() + ": " + currentChoiceInkObjects[choiceIndex].text;

			currentConversation.addDialogueLine(PartyManager.getPlayerStats().getName(), currentChoiceInkObjects[choiceIndex].text);

			ChoiceManager.addChoice(storyName, currentChoiceInkObjects[choiceIndex].sourcePath);

			currentStory.ChooseChoiceIndex(choiceIndex);

			currentStory.Continue();

			continueStory();
		}

	}
	/*
		public void disableCanvas()
		{
			dialogueCanvas.gameObject.SetActive(false);
		}

		public void enableCanvas()
		{
			dialogueCanvas.gameObject.SetActive(true);
		}
	*/
	private void setCameraToDefaultSpeed()
	{
		framingTransposer.m_XDamping = defaultXDamping;
		framingTransposer.m_YDamping = defaultYDamping;
	}

	private void setCameraToDialogueSpeed()
	{
		framingTransposer.m_XDamping = dialogueXDamping;
		framingTransposer.m_YDamping = dialogueYDamping;
	}

	private void setCameraToFadeSpeed()
	{
		framingTransposer.m_XDamping = fadeXDamping;
		framingTransposer.m_YDamping = fadeYDamping;
	}

	public void spawnDialogueTrackerWindowWithoutChoices()
	{
		dialogueTrackerButton.withChoices = false;
		dialogueTrackerButton.spawnPopUp();
		dialogueTrackerButton.withChoices = true;
	}

    public void findNPCGameObject()
    {
        DialogueTrigger npcDialogueTrigger;
        Dialogue npcDialogue;

        currentDialogue.names[0] = PartyManager.getPlayerStats().getName();
        currentDialogue.cameraFoci[0] = PlayerMovement.getInstance().gameObject;

        for (int nameIndex = 1; nameIndex < currentDialogue.names.Length; nameIndex++)
        {
            foreach (Transform child in AreaManager.getNPCParent())
            {
                npcDialogueTrigger = child.GetComponent<DialogueTrigger>();

                if (npcDialogueTrigger != null)
                {
                    npcDialogue = npcDialogueTrigger.dialogue;

                    if (npcDialogue != null && currentDialogue.names[nameIndex].Equals(npcDialogue.getMainNPCName()))
                    {
                        currentDialogue.names[nameIndex] = currentDialogue.names[nameIndex];
                        currentDialogue.cameraFoci[nameIndex] = child.gameObject;
                        break;
                    }
                    else
                    {
                        // Debug.LogError(currentDialogue.names[nameIndex] + " was not found");
                    }
                }
            }
        }
    }

    public void continueStory()
    {

        string newLine;
        string[] args = new string[0];
        Item itemToGive;
        string partyMemberName = "";
        string tutorialKey = "";
        bool continueAfterTransparent = false;

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
                case "keepdialogue":

                    keepOldDialogue = true;
                    continueStory();
                    keepOldDialogue = false;

                    break;

                case "deactivate":

                    buffer = buffer.ToLower().Replace("deactivate(", "").Replace(")", "").Replace(" ", "").Replace("\n", "").Replace("\r", "");

                    int i = int.Parse(buffer);

                    currentDialogue.cameraFoci[i].SetActive(false);

                    continueStory();

                    break;

                case "activatequeststep":

                    //activateQuestStep(string questTitle, int questStepIndex)
                    //activateQuestStep(string questTitle, int questStepIndex, bool haltNotificationQueue)
                    args = buffer.Split("(")[1].Split(")")[0].Split(",");

                    string questTitle3 = args[0];
                    int questStepIndex3 = int.Parse(args[1]);

                    checkForHaltNotificationBoolArg(args, activateQuestStepHaltNotificationBoolIndex);

                    QuestList.activateQuestStep(questTitle3, questStepIndex3);

                    OOCUIManager.updateQuestCounter();

                    continueStory();

                    break;

                case "finishedquest":
                case "finishquest":

                    //finishQuest(string questTitle, bool succeeded, int questStepIndex) if you're activating a quest step
                    //finishQuest(string questTitle, bool succeeded, int questStepIndex, bool haltNotificationQueue)
                    args = buffer.Split("(")[1].Split(")")[0].Split(",");

                    string questTitle = args[0];
                    bool questSuccessful = bool.Parse(args[1]);
                    int finalQuestStep = int.Parse(args[2]);

                    checkForHaltNotificationBoolArg(args, finishQuestHaltNotificationBoolIndex);

                    QuestList.finishQuest(questTitle, finalQuestStep, questSuccessful);

                    OOCUIManager.updateQuestCounter();

                    continueStory();

                    break;

                case "activate":

                    buffer = buffer.ToLower().Replace("activate(", "").Replace(")", "").Replace(" ", "");

                    int j = int.Parse(buffer);

                    NPCSpawnChecker npcSpawnChecker = currentDialogue.cameraFoci[j].GetComponent<NPCSpawnChecker>();

                    if (npcSpawnChecker != null && !(npcSpawnChecker is null))
                    {
                        npcSpawnChecker.ignoreInPartyForSpawning = true;
                    }

                    currentDialogue.cameraFoci[j].SetActive(true);

                    continueStory();

                    break;

                case "changecamtarget":
                case "changecameratarget":

                    buffer = buffer.ToLower().Replace("changecamtarget(", "").Replace(")", "").Replace(" ", "");

                    int k = int.Parse(buffer);

                    if (k == 0)
                    {
                        mainCM.m_Follow = PlayerMovement.getInstance().gameObject.transform;
                    }
                    else
                    {
                        mainCM.m_Follow = currentDialogue.cameraFoci[k].transform;
                    }
                    nameText = DialogueList.scrubNameOfEndNumbers(currentDialogue.names[k]) + ":";
                    continueStory();

                    break;

                case "settotrue":

                    buffer = buffer.Replace("setToTrue(", "").Replace(")", "").Replace(" ", "").Replace("\n", "");

                    Flags.setFlag(buffer, true);

                    if (currentStory.variablesState.Contains(buffer))
                    {
                        currentStory.variablesState[buffer] = true;
                    }

                    continueStory();

                    break;

                case "settofalse":

                    buffer = buffer.Replace("setToFalse(", "").Replace(")", "").Replace(" ", "").Replace("\n", "");

                    Flags.setFlag(buffer, false);

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

                    string coinsToGive = buffer.Split("(")[1].Split(")")[0];

                    Purse.addCoins(coinsToGive);

                    newLine = "Gold Coins x" + coinsToGive;

                    currentConversation.addObtainedLine(newLine);

                    break;

                case "takecoin":
                case "takecoins":

                    string coinsToTake = buffer.Split("(")[1].Split(")")[0];

                    Purse.removeCoins(coinsToTake);

                    newLine = "Gold Coins x" + coinsToTake;

                    currentConversation.addRemovedLine(newLine);

                    break;

                case "giveitem":

                    args = buffer.Split("(")[1].Split(")")[0].Split(",");

                    itemToGive = ItemList.getItem(args[0], args[1], args[2]);

                    Inventory.addItem(itemToGive);

                    newLine = itemToGive.getKey() + " x" + itemToGive.getQuantity();

                    currentConversation.addObtainedLine(newLine);

                    break;

                case "giveitems":

                    string[] argGroups = buffer.Split("(")[1].Split(")")[0].Split("|");

                    foreach (string argGroup in argGroups)
                    {

                        args = argGroup.Split(",");

                        itemToGive = ItemList.getItem(args[0], args[1], args[2]);

                        Inventory.addItem(itemToGive);

                        newLine = itemToGive.getKey() + " x" + itemToGive.getQuantity();

                        currentConversation.addObtainedLine(newLine);
                    }

                    //commented out to test if the \n in the loop was sufficient
                    //dialogueText.text += "\n"; // '\n' at end to simulate coming from ink. Ink has \n at the end of all dialogue 

                    break;

                case "takeallofitem":

                    string itemName14 = buffer.Split("(")[1].Split(")")[0];
                    //Item(string key, string loreDescription, string type, string subtype)

                    int quantity = 0;

                    try
                    {
                        quantity += Inventory.removeItem(itemName14).getQuantity();
                    }
                    catch (IOException e)
                    {

                    }

                    List<PartyMember> partyMembers = PartyManager.getAllPartyMembers();

                    foreach (PartyMember partyMember in partyMembers)
                    {
                        EquippedItems equippedItems = partyMember.stats.getEquippedItems();

                        for (int index = 0; index < EquippedItems.totalEquipmentSlots; index++)
                        {
                            if (equippedItems.getItemInSlot(index) != null && equippedItems.getItemInSlot(index).getKey().Equals(itemName14))
                            {
                                equippedItems.unequipItem(index);
                                quantity++;
                            }
                        }
                    }

                    newLine = itemName14 + " x" + quantity;

                    currentConversation.addRemovedLine(newLine);

                    break;

                case "takejunk":


                    args = buffer.Split("(")[1].Split(")")[0].Split(",");
                    //Item(string key, string loreDescription, string type, string subtype)

                    Inventory.removeItem(args[0], int.Parse(args[1]), State.junkPocket);

                    newLine = args[0] + " x" + int.Parse(args[1]);

                    currentConversation.addRemovedLine(newLine);

                    break;

                case "addxp":

                    string earnedXP = buffer.Split("(")[1].Split(")")[0];

                    PartyManager.addXP(earnedXP);
                    LessonUIManager.lastEarnedXPBonus = int.Parse(earnedXP);

                    newLine = earnedXP + " experience points";

                    currentConversation.addEarnedLine(newLine);

                    break;

                case "searchinventoryfor":

                    args = buffer.Split("(")[1].Split(")")[0].Split(",");

                    //args needs to include case sensitive name of flag in dialogue file to update 
                    //so it knows whether the player has the item you're searching for or not
                    //args should be in order: string flagName, string key || or || string flagName, string subtype, int ID

                    if (args.Length == 2)
                    {



                        bool flagStatus = Inventory.inventoryContainsItem(args[1]) ||
                                                                 Inventory.equipmentContainsItem(args[1]);
                        Flags.setFlag(args[0], flagStatus);
                        currentStory.variablesState[args[0]] = flagStatus;
                    }
                    else if (args.Length == 3)
                    {
                        bool flagStatus = Inventory.inventoryContainsItem(args[1], int.Parse(args[2]));
                        Flags.setFlag(args[0], flagStatus);
                        currentStory.variablesState[args[0]] = flagStatus;
                        //only for keys/questitems which aren't equippable
                        //so don't need to check equipmentContainsItem()
                    }

                    continueStory();

                    break;

                case "searchjunkfor":


                    args = buffer.Split("(")[1].Split(")")[0].Split(",");

                    //args needs to include case sensitive name of flag in dialogue file to update 
                    //so it knows whether the player has the item you're searching for or not
                    //args should be in order: string flagName, string key || or  || string flagName, string subtype, int ID
                    if (args.Length == 2)
                    {
                        currentStory.variablesState[args[0]] = Inventory.pocketContainsItem(args[1], State.junkPocket);
                    }
                    else if (args.Length == 3)
                    {
                        currentStory.variablesState[args[0]] = Inventory.pocketContainsItem(args[1], int.Parse(args[2]), State.junkPocket);
                    }

                    continueStory();

                    break;

                case "healparty":
                    PartyManager.healFullAllPartyMembers();

                    continueStory();

                    break;

                case "restparty":

                    endDialogue();

                    // State.playerStats.modifyCurrentHealth(State.playerStats.getTotalHealth(), true);

                    PartyManager.healFullAllPartyMembers();

                    if (FadeToBlackManager.isBlack())
                    {
                        fadeToBlackManager.setAndStartFadeBackIn();
                    }
                    else
                    {
                        fadeToBlackManager.setAndStartFadeToBlack();
                    }

                    return;

                case "fadetoblack":
                    args = buffer.Split("(")[1].Split(")")[0].Split(",");

                    bool setDialogueUIActiveAfterFadeIn = true;
                    continueAfterTransparent = true;

                    if (args.Length > 0 && args[0].Length > 0)
                    {
                        setDialogueUIActiveAfterFadeIn = bool.Parse(args[0]);
                    }

                    if (args.Length > 1 && args[1].Length > 1)
                    {
                        continueAfterTransparent = bool.Parse(args[1]);
                    }

                    setCameraToFadeSpeed();

                    fadeToBlackManager.setAndStartFadeToBlack();
                    waitingOnFadeToBlack = true;

                    FadeToBlackManager.delayFadingIn();

                    StartCoroutine(handleDialogueUIDuringFadeOut(setDialogueUIActiveAfterFadeIn, continueAfterTransparent));

                    return;
                case "fadebackin": //fadeBackIn(int framesToWait), fadeBackIn(int framesToWait, bool continueAfterTransparent)

                    args = buffer.Split("(")[1].Split(")")[0].Split(",");


                    if (args.Length == 0 || args[0] == "")
                    {
                        framesToWait = 0;
                    }
                    else
                    {
                        framesToWait = int.Parse(args[0]);
                    }

                    frames = 0;

                    continueAfterTransparent = false;

                    if (args.Length > 1)
                    {
                        continueAfterTransparent = bool.Parse(args[1]);
                    }

                    StartCoroutine(fadeBackIn(continueAfterTransparent));

                    return;

                case "movepos":
                case "moveposition":
                case "moveplayer":
                case "moveplayerpos":
                case "moveplayerposition":
                case "movelocalpos":
                case "movelocalposition":
                case "movetolocalpos":
                case "movetolocalposition":
                case "changepos":
                case "changeposition":
                case "changeplayerpos":
                case "changeplayerposition":
                case "changelocalpos":
                case "changelocalposition":

                    args = buffer.Split("(")[1].Split(")")[0].Split(",");

                    Vector3 newLocalPos = new Vector3(float.Parse(args[0].Replace("f", "")), float.Parse(args[1].Replace("f", "")), 0);

                    PlayerMovement.getInstance().gameObject.transform.localPosition = newLocalPos;

                    Helpers.updateColliderPosition(PlayerMovement.getInstance().gameObject.transform);

                    PartyMemberMovement.instantiatePartyMemberTrain();

                    continueStory();

                    break;
                case "setfacing":
                case "setplayerfacing":
                case "changefacing":
                case "changeplayerfacing":

                    string facingArgs = buffer.Split("(")[1].Split(")")[0];

                    switch (facingArgs.ToLower())
                    {
                        case "ne":
                        case "northeast":
                            State.playerFacing.setFacing(Facing.NorthEast);
                            break;
                        case "nw":
                        case "northwest":
                            State.playerFacing.setFacing(Facing.NorthWest);
                            break;
                        case "se":
                        case "southeast":
                            State.playerFacing.setFacing(Facing.SouthEast);
                            break;
                        case "sw":
                        case "southwest":
                            State.playerFacing.setFacing(Facing.SouthWest);
                            break;
                    }

                    PlayerMovement.getInstance().adjustAnimator(true);

                    continueStory();

                    break;

                case "adjustgridsquare":

                    args = buffer.Split("(")[1].Split(")")[0].Split(",");

                    Facing facingDirection = State.playerFacing.getFacing();
                    int adjustmentMagnitude = int.Parse(args[0]) + 1;
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

                    Vector3Int newPlayerGridSquare = player.getMovementGridCoordsLocal() + gridSquareAdjustment;

                    player.gameObject.transform.localPosition = player.convertGridCoordsToLocalPos(newPlayerGridSquare);

                    Helpers.updateColliderPosition(player.gameObject.transform);

                    PartyMemberMovement.instantiatePartyMemberTrain();

                    PlayerMovement.getInstance().adjustAnimator(true);

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

                case "entershopmode":

                    EscapeStack.escapeAll();

                    ShopPopUpButton shopPopUpButton = new ShopPopUpButton();
                    Shopkeeper shopkeeper = currentDialogue.cameraFoci[1].GetComponent<Shopkeeper>();

                    endDialogue();

                    shopPopUpButton.spawnPopUp(shopkeeper);
                    shopPopUpButton.getCurrentPopUpGameObject().SetActive(true);

                    return;

                case "kill":

                    int deadNameIndex = int.Parse(buffer.Split("(")[1].Split(")")[0]);

                    DeathFlagManager.addName(currentDialogue.names[deadNameIndex]);

                    currentDialogue.cameraFoci[deadNameIndex].SetActive(false);

                    continueStory();

                    break;

                case "adddeathflag":

                    string deadName = buffer.Split("(")[1].Split(")")[0];

                    DeathFlagManager.addName(deadName);

                    continueStory();

                    break;
                case "addtoparty":

                // int nameIndex = int.Parse(buffer.Split("(")[1].Split(")")[0]);  //use to be index of party member in the partyMember arrayList in State
                // 																//but after swapping to a dictionary that is depricated. Now uses same index
                // 																//as changeCamTarget

                // partyMemberName = currentDialogue.names[nameIndex];

                // PartyManager.getPartyMember(partyMemberName).canJoinParty = true;

                // // StartCoroutine(waitToSpawnPopUp(formationEditorButton));

                // continueStory();

                // break;

                case "addtopartywithoutpopup":

                    args = buffer.Split("(")[1].Split(")")[0].Split(",");           //use to be index of party member in the partyMember arrayList in State
                                                                                    //but after swapping to a dictionary that is depricated. Now uses same index
                                                                                    //as changeCamTarget

                    partyMemberName = currentDialogue.names[int.Parse(args[0])];

                    PartyManager.getPartyMember(partyMemberName).canJoinParty = true;

                    Formation formation = State.formation;

                    if (formation.getSizeOfFormation() < PartyStats.getPartySizeMaximum())
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

                case "removefromparty":

                    int nameIndex2 = int.Parse(buffer.Split("(")[1].Split(")")[0]);

                    partyMemberName = currentDialogue.names[nameIndex2];

                    PartyManager.getPartyMember(partyMemberName).canJoinParty = false;

                    if (State.formation != null)
                    {
                        State.formation.removePartyMember(partyMemberName);
                    }

                    currentConversation.addLeftPartyLine(partyMemberName);

                    continueStory();

                    break;
                case "opengate":

                    string gateKey = buffer.Split("(")[1].Split(")")[0].Split(",")[0];

                    if (gateKey.Length <= 0)
                    {
                        gateKey = PlayerMovement.getInstance().dialogueTrigger.gameObject.GetComponent<GateSpawnChecker>().gateKey;
                    }

                    GateAndChestManager.addKey(gateKey);

                    continueStory();

                    break;

                case "swapinkfile": //swapInkFiles(int secondaryInkFileIndex, string startingBoolName)
                case "swapinkfiles": //swapInkFiles(int secondaryInkFileIndex, string startingBoolName, bool safeToSwapDialogueObjects)

                    args = buffer.Split("(")[1].Split(")")[0].Split(",");

                    int secondaryInkFileIndex = int.Parse(args[0]);
                    string startingBoolName = args[1]; //if you want to start at the correct knot, 
                                                       //you need to create/give a bool that tells 
                                                       //the dialogue at the start to move to that 
                                                       //knot. see the transition between MinersDialogue
                                                       // and MarcosDialoge in MineLvl_3-Miners Camp
                    bool safeToSwapDialogueObjects = false;

                    if (args.Length == 3)
                    {
                        safeToSwapDialogueObjects = bool.Parse(buffer.Split("(")[1].Split(")")[0].Split(",")[2]);
                    }



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
                case "learnlesson":

                    LessonUIManager lessonUIManager = LessonUIManager.getInstance();
                    lessonUIManager.addLessonKey(buffer.Split("(")[1].Split(")")[0]);

                    if (!LessonUIManager.waitingToActivate)
                    {
                        StartCoroutine(LessonUIManager.activateAfterDialogue());
                    }

                    continueStory();
                    break;
                case "entercombat":

                    args = buffer.Split("(")[1].Split(")")[0].Split(",");
                    int enemyPackInfoIndex = int.Parse(args[0]);

                    NPCCombatInfo npcCombatInfo = currentDialogue.npcCombatInfo;

                    State.enemyPackInfo = npcCombatInfo.getEnemyInfo(enemyPackInfoIndex);

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
                    AreaList.addHostility();

                    CombatStateManager.locationBeforeCombat = AreaManager.locationName;

                    endDialogue();

                    if (args.Length > 1)
                    {
                        State.dialogueUponSceneLoadKey = args[1];
                    }

                    State.enteredCombatFromDialogue = true;

                    SceneChange.changeSceneToCombat();
                    break;

                case "setareatohostile":
                    string sceneToBecomeHostile = buffer.Split("(")[1].Split(")")[0].Split(",")[0];

                    AreaList.setAreaToHostile(sceneToBecomeHostile);

                    continueStory();
                    break;

                case "setareatopassive":
                    string sceneToBecomePassive = buffer.Split("(")[1].Split(")")[0].Split(",")[0];

                    AreaList.setAreaToPassive(sceneToBecomePassive);

                    continueStory();
                    break;

                case "hidetrain":

                    PartyMemberMovement.instantiatePartyMemberTrain();

                    continueStory();
                    break;

                case "starttutorial":

                    tutorialKey = buffer.Split("(")[1].Split(")")[0];

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

                    tutorialKey = buffer.Split("(")[1].Split(")")[0];

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

                    string promptMessage = buffer.Split("(")[1].Split(")")[0];

                    PlayerMovement.createCustomButtonPrompt(promptMessage);
                    WASDPromptStepCounter.createStepCounter();

                    continueStory();
                    break;

                case "changescene":

                    NewSceneTransition transition = currentDialogue.cameraFoci[1].GetComponent<NewSceneTransition>();

                    if (transition == null)
                    {
                        Debug.LogError("NewSceneTransition component not found on " + currentDialogue.cameraFoci[1].name);
                    }
                    else
                    {
                        TransitionManager.getInstance().changeSceneWithoutTrigger(transition.getTransitionInfo());
                    }

                    continueStory();
                    break;

                case "close":

                    endDialogue();

                    break;

                default:
                    currentConversation.addDialogueLine(nameText.Replace(":", ""), buffer);
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

	private TextAsset getSecondaryStoryJSON(int secondaryInkFileIndex)
	{
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

	private void checkForHaltNotificationBoolArg(string[] args, int boolIndex)
	{
		if (args.Length > boolIndex)
		{
			if (bool.Parse(args[boolIndex]))
			{
				NotificationManager.skipNextNotificationSpawn();
			}
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

		while (FadeToBlackManager.isTransparent())
		{
			yield return null;
		}

		while (!FadeToBlackManager.isTransparent())
		{
			yield return null;
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

	private IEnumerator fadeBackIn(bool continueAfterTransparent)
	{

		// Debug.LogError("inside fadeBackIn");

		yield return new WaitUntil(() => frames >= framesToWait);

		FadeToBlackManager.allowFadingIn();
		fadeToBlackManager.setAndStartFadeBackIn();
		waitingOnFadeBackIn = true;

		setCameraToDialogueSpeed();

		if (continueAfterTransparent)
		{
			while (!FadeToBlackManager.isTransparent())
			{
				yield return null;
			}

			// Debug.LogError("inside fadeBackIn");

			continueStory();
		}
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

        foreach(IStoryVariableSource source in variableSources)
        {
            story = source.addVariables(story);
        }

        return story;
	}
}

public class Conversation
{	
	private const int maxLineCount = 500;
	private const string infoName = "Info";
	private const string earnedName = "Earned";
	private const string obtainedName = "Obtained";
	private const string removedName = "Removed";
	private const string endOfDialogueMessage = "End of Dialogue";
    private const string leftPartyMessage = " has left your party.";

    private DialogueTrackerWindow attachedWindow;
	
	private ArrayList dialogueList = new ArrayList();
	
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

	public ArrayList getDialogueList()
	{
		return dialogueList;
	}
	
	public void addEndOfDialogueLine()
	{
		addDialogueLine(endOfDialogueMessage);
	}
    public void addLeftPartyLine(string partyMemberName)
    {
        addDialogueLine(partyMemberName + leftPartyMessage);
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
			ArrayList listOfDialogue = new ArrayList();
			listOfDialogue.Add(getLastLine());
			
			attachedWindow.appendDialogue(listOfDialogue);
		}
	}
	
	public void appendConversation(Conversation newConversation)
	{
		ArrayList dialogueToAppend = newConversation.getDialogueList();
		
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
		return (DialogueLine) dialogueList[dialogueList.Count - 1];
	}
	
	
}

public static class SpeechLog
{	
	private static Conversation allDialogues = new Conversation();
	
	public static void appendConversation(Conversation newConversation)
	{
		allDialogues.appendConversation(newConversation);
	}
	
	public static ArrayList getDialogueList()
	{
		return allDialogues.getDialogueList();
	}
	
	public static void cleanSpeechLog()
	{
		allDialogues = new Conversation();
	}
}
