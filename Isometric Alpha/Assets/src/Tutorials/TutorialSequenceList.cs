using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TutorialSequenceList
{
    #region Flags and Hash's
    private const bool createPopUpScreenBlocker = true;
    private const bool enableAllDisabledRowButtons = true;
    private const bool skipHighlight = true;
    private const bool skipUnhighlight = true;
    private const bool highlight = false;
    private const bool unhighlight = false;
    private const bool allowsMovementKeys = true;

    private const string itemTutorialSequenceKey = "Item Tutorial";
    public const string equippableItemTutorialSeenFlag = "equippableItemTutorialSequenceEntered";

    private const string formationTutorialSequenceKey = "Formation Tutorial";
    public const string formationTutorialSeenFlag = "formationTutorialSequenceEntered";

    public const string addingAbilitiesTutorialSequenceKey = "Adding Abilities Tutorial";
    public const string addingAbilitiesTutorialSeenFlag = "addingAbilitiesTutorialSequenceEntered";

    public const string combatTutorialSequenceKey = "Combat Tutorial";
    public const string combatTutorialSeenFlag = "combatTutorialSequenceEntered";
    
    public const string traitTutorialSequenceKey = "Trait Tutorial";
    public const string traitTutorialSeenFlag = "traitTutorialSequenceEntered";


    public const string mandatoryTargetMonsterTargetHash = "Mandatory Target Monster";
    public const string mandatoryTargetTraitIconTargetHash = "Mandatory Target Trait Icon";
    public const string mandatoryTargetTutorialSequenceKey = "Mandatory Target Tutorial";
    public const string mandatoryTargetTutorialSeenFlag = "mandatoryTargetTutorialSequenceEntered";

    public const string skipThatchShackTutorialsFlag = "skipThatchShackTutorials";
    public const string firstHostilityTutorialSeenFlag = "firstHostilityTutorialSequenceEntered";
	public const string intimidateTutorialSeenFlag = "intimidateTutorialSequenceEntered";
	public const string cunningTutorialSeenFlag = "cunningTutorialSequenceEntered";
	public const string secondCunningTutorialSeenFlag = "secondCunningTutorialSequenceEntered";
	public const string observationTutorialSeenFlag = "observationTutorialSequenceEntered";
	public const string leadershipTutorialSeenFlag = "leadershipTutorialSequenceEntered";
	public const string interactableObjectTutorialSeenFlag = "interactableObjectTutorialSequenceEntered";
    public const string hiddenObjectsTutorialSeenFlag = "hiddenObjectsTutorialSequenceEntered";
    public const string secondHostilityTutorialSeenFlag = "secondHostilityTutorialSequenceEntered";

    private const string characterScreenStatsTargetHash = "Character Screen Stats";

    private const string characterScreenButtonTargetHash = "Character Screen Button";
    private const string inventoryButtonTargetHash = "Inventory Button";
    private const string equipmentGridTargetHash = "Equipment Grid";
    private const string itemGridTargetHash = "Item Grid";
    private const string inventoryScreenTargetHash = "Inventory Screen";

    
    private const string companionAbilityButtonTargetHash = "Companion Ability Button";

    private const string characterScreenBackground = "Character Screen Background";
    private const string characterScreenAbilityList = "Character Screen Ability List";

    private const string partyScreenButton = "Party Screen Button";
    private const string partyMemberList = "Party Member List";
    private const string partySlotTracker = "Party Slot Tracker";
    private const string formationGrid = "Formation Grid";
    private const string partyScreen = "Party Screen";

    private const string descriptionPanelNameText = "Name Text";
    private const string descriptionPanelDamageText = "Damage Text";
    private const string descriptionPanelCritText = "Crit Text";
    private const string descriptionPanelRangeText = "Range Text";

    public const string firstTutorialEnemyTargetHash = "First Tutorial Enemy";
    public const string secondTutorialEnemyTargetHash = "Second Tutorial Enemy";
    public const string statusEffectDisplayTargetHash = "Status Effect Display";
    private const string hostilityUITargetHash = "Hostility UI Panel";

    public const string interactableRubbleTargetHash = "Interactable Rubble";

    public const string vaultableBarrelsTargetHash = "Vaultable Barrels";
    public const string tutorialCunningObjectTargetHash = "Tutorial Cunning Object";
    public const string skillUIPanelTargetHash = "Skill UI Panel";

    public const string secretDoorTargetHash = "Secret Door";

    public const string fallenBeamTargetHash = "Fallen Beam";
    public const string tutorialButtonOneTargetHash = "Tutorial Button 1";
    public const string tutorialButtonTwoTargetHash = "Tutorial Button 2";
    public const string placedCharacterTargetHash = "Placed Character";

    public const string questCounterUIPanel = "OOCUI Quest Counter";
    public const string mapPopUpWindow = "Map PopUp Window";
    public const string mapTileQuestCounter = "Map Tile Quest Counter";
    public const string mapQuestList = "Map Quest List";

    public const string playerCombatSpriteTargetHash = "Player Combat";
    private const string allyZoneTargetHash = "Ally Zone";
    private const string enemyZoneTargetHash = "Enemy Zone";
    private const string surpriseIconTargetHash = "Surprise Icon";
    private const string singleTargetSelectorTargetHash = "Single Target Selector";
    private const string bottomRightHoverPanelTargetHash = "Bottom Right Hover Panel";
    private const string traitDisplayTargetHash = "Trait Display";
    private const string masterTraitIconTargetHash = "Master Trait Icon";
    private const string minionTraitIconTargetHash = "Minion Trait Icon";
    private const string topThirdOfCombatUITargetHash = "Top Third Of Combat UI";
    private const string combatActionWheelTargetHash = "Combat Action Wheel";
    private const string actionOrderTargetHash = "Action Order";
    private const string actionSlotIconsTargetHash = "Action Slot Icons";
    public const string traitMonsterTargetHash = "Trait Monster";
    public const string exuberancesParentTargetHash = "Exuberances Parent";
    public const string combatActionDescriptionPanelTargetHash = "Combat Action Description Panel";


    public const string movableObjectTutorialSequenceKey = "Movable Object Tutorial";
    public const string movableObjectTutorialSeenFlag = "movableObjectTutorialSequenceEntered";
    public const string tutorialCrateTargetHash = "Tutorial Crate";

    public const string questCounterTutorialSeenFlag = "questCounterTutorialSequenceEntered";

    public const string firstHostilityTutorialSequenceKey = "First Hostility Tutorial";
    public const string secondHostilityTutorialSequenceKey = "Second Hostility Tutorial";
    public const string intimidateTutorialSequenceKey = "Intimidate Tutorial";
    public const string interactableRubbleTutorialSequenceKey = "Interactable Rubble Tutorial";
    public const string vaultableObjectTutorialSequenceKey = "Vaultable Object Tutorial";
    public const string firstCunningTutorialSequenceKey = "First Cunning Tutorial";
    public const string secondCunningTutorialSequenceKey = "Second Cunning Tutorial";
    public const string observationTutorialSequenceKey = "Observation Tutorial";
    public const string leadershipTutorialSequenceKey = "Leadership Tutorial";
    public const string questCounterTutorialSequenceKey = "Quest Counter Tutorial";
    public const string hiddenObjectsTutorialSequenceKey = "Hidden Object Tutorial";
    // public const string partyMemberUpgradeTutorialSequenceKey = "Party Member Upgrade Tutorial";
    // public const string partyMemberUpgradeTutorialSeenFlag = "partyMemberUpgradeTutorialSequenceEntered";
    public const string playerLevelUpTutorialSequenceKey = "Player Level Up Tutorial";
    public const string playerLevelUpTutorialSeenFlag = "playerLevelUpTutorialSequenceEntered";
    public const string playerSpriteOOCTargetHash = "Player";
    public const string playerSpriteOOCNoArrowTargetHash = "PlayerNoArrow";

    public const string companionSpecificAbilitiesTutorialSequenceKey = "Companion Specific Abilities Tutorial";
    public const string companionSpecificAbilitiesTutorialSeenFlag = "companionSpecificAbilitiesTutorialSequenceEntered";

    public const string exuberanceCostTutorialSequenceKey = "Exuberance Cost Tutorial";
    public const string exuberanceCostTutorialSeenFlag = "exuberanceCostTutorialSequenceEntered";
    public const string traitCostTutorialSequenceKey = "Trait Cost Tutorial";
    public const string traitCostTutorialSeenFlag = "traitCostTutorialSequenceEntered";

    #endregion

    private const bool doNoSkipCurrentActivityChange = false;

    private static Dictionary<string, TutorialSequence> tutorialSequenceDictionary;

    [RuntimeInitializeOnLoadMethod]
    public static void initializeTutorials()
    {
        tutorialSequenceDictionary = new Dictionary<string, TutorialSequence>();

        initializeFirstHostilityTutorial();
        initializeSecondHostilityTutorial();

        initializeLiftableRubbleTutorial();
        initializeIntimidateTutorial();
        
        initializeVaultableObjectTutorial();
        initializeFirstCunningTutorial();
        initializeSecondCunningTutorial();

        initializeObservationTutorial();

        initializeLeadershipTutorial();

        initializeQuestSymbolTutorial();

        initializeHiddenObjectTutorial();

        initializeEquippableItemTutorial();
        initializeFormationPopUpItemTutorial();
        
        initializeTraitTutorial();
        initializeMandatoryTargetTutorial();
        initializeMovableObjectTutorial();
        
        initializePlayerLevelUpTutorial();
        initializeAddingAbilitiesTutorial();
        initializeCompanionSpecificAbilitiesTutorial();

        initializeExuberanceCostTutorial();
        initializeTraitCostTutorial();
    }

    public static void initializeFirstHostilityTutorial()
    {
        TutorialSequenceStep stepOne = new TutorialSequenceStep(TutorialMessageList.hostileTargetTutorialMessagePrefix + 1, 
                                                                firstTutorialEnemyTargetHash, 
                                                                ArrowDirection.Top, 
                                                                KeyBindingList.revealKey, 
                                                                skipHighlight: highlight, 
                                                                skipUnhighlight: skipUnhighlight,  
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker, 
                                                                scriptAtEnd: new HighlightTargetScript());
        TutorialSequenceStep stepTwo = new TutorialSequenceStep(TutorialMessageList.hostileTargetTutorialMessagePrefix + 2, 
                                                                firstTutorialEnemyTargetHash, 
                                                                ArrowDirection.Top, 
                                                                KeyBindingList.revealKey, 
                                                                skipHighlight: skipHighlight, 
                                                                skipUnhighlight: unhighlight,  
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker, 
                                                                scriptAtEnd: new UnhighlightTargetScript());
        TutorialSequenceStep stepThree = new TutorialSequenceStep(TutorialMessageList.hostileTargetTutorialMessagePrefix + 3, 
                                                                    hostilityUITargetHash, 
                                                                    ArrowDirection.Left, 
                                                                    skipHighlight: skipHighlight, 
                                                                    skipUnhighlight: skipUnhighlight, 
                                                                    createPopUpScreenBlocker: createPopUpScreenBlocker);
        TutorialSequenceStep stepFour = new TutorialSequenceStep(TutorialMessageList.hostileTargetTutorialMessagePrefix + 4, 
                                                                  firstTutorialEnemyTargetHash, 
                                                                  ArrowDirection.Top, 
                                                                  skipHighlight: highlight, 
                                                                  skipUnhighlight: skipUnhighlight, 
                                                                  createPopUpScreenBlocker: createPopUpScreenBlocker);
        TutorialSequenceStep stepFive = new TutorialSequenceStep(TutorialMessageList.hostileTargetTutorialMessagePrefix + 5, 
                                                                 firstTutorialEnemyTargetHash, 
                                                                 ArrowDirection.Top, 
                                                                 skipHighlight: highlight, 
                                                                 skipUnhighlight: skipUnhighlight, 
                                                                 createPopUpScreenBlocker: createPopUpScreenBlocker);
        TutorialSequenceStep stepSix = new TutorialSequenceStep(TutorialMessageList.hostileTargetTutorialMessagePrefix + 6, 
                                                                firstTutorialEnemyTargetHash, 
                                                                ArrowDirection.Top, 
                                                                KeyBindingList.moveEastKey, 
                                                                skipHighlight: highlight, 
                                                                skipUnhighlight: unhighlight,  
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker, 
                                                                scriptAtEnd: new MovePlayerSouthEastScript());

        TutorialSequence firstHostilityTutorialSequence = new TutorialSequence(OOCActivity.walking, 
                                                                              doNoSkipCurrentActivityChange, 
                                                                              firstHostilityTutorialSeenFlag, 
                                                                              new TutorialSequenceStep[] { 
                                                                                                            stepOne, 
                                                                                                            stepTwo, 
                                                                                                            stepThree,
                                                                                                            stepFour,
                                                                                                            stepFive, 
                                                                                                            stepSix 
                                                                                                          });

        firstHostilityTutorialSequence.setSkipScript(new SkipTutorialScript());
        tutorialSequenceDictionary.Add(firstHostilityTutorialSequenceKey, firstHostilityTutorialSequence); 
    }

    public static void initializeSecondHostilityTutorial()
    {
        TutorialSequenceStep stepOne = new TutorialSequenceStep(TutorialMessageList.hostilityBarsTutorialMessagePrefix + 1,
                                                                hostilityUITargetHash,
                                                                ArrowDirection.Left,
                                                                skipHighlight: highlight,
                                                                skipUnhighlight: skipUnhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker);
        TutorialSequenceStep stepTwo = new TutorialSequenceStep(TutorialMessageList.hostilityBarsTutorialMessagePrefix + 2,
                                                                hostilityUITargetHash,
                                                                ArrowDirection.Left,
                                                                skipHighlight: skipHighlight,
                                                                skipUnhighlight: unhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker);

        TutorialSequence secondHostilityTutorialSequence = new TutorialSequence(OOCActivity.walking, doNoSkipCurrentActivityChange, secondHostilityTutorialSeenFlag, new TutorialSequenceStep[] { stepOne, stepTwo});

        secondHostilityTutorialSequence.setSkipScript(new SkipTutorialScript());
        tutorialSequenceDictionary.Add(secondHostilityTutorialSequenceKey, secondHostilityTutorialSequence);
    }

    public static void initializeLiftableRubbleTutorial()
    {
        TutorialSequenceStep stepOne = new TutorialSequenceStep(TutorialMessageList.interactableObjectTutorialMessagePrefix + 1,
                                                                interactableRubbleTargetHash,
                                                                ArrowDirection.Left,
                                                                KeyBindingList.interactKey,
                                                                skipHighlight: highlight,
                                                                skipUnhighlight: unhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                scriptAtStart: new FaceNorthEastScript(),
                                                                scriptAtEnd: new PlayerInteractScript());

        TutorialSequence interactableObjectTutorialSequence = new TutorialSequence(OOCActivity.inDialogue, doNoSkipCurrentActivityChange, interactableObjectTutorialSeenFlag, new TutorialSequenceStep[] { stepOne });

        interactableObjectTutorialSequence.setSkipScript(new SkipInteractionTutorialScript());
        tutorialSequenceDictionary.Add(interactableRubbleTutorialSequenceKey, interactableObjectTutorialSequence);
    }

    public static void initializeVaultableObjectTutorial()
    {
        TutorialSequenceStep stepOne = new TutorialSequenceStep(TutorialMessageList.interactableObjectTutorialMessagePrefix + 2,
                                                                vaultableBarrelsTargetHash,
                                                                ArrowDirection.Top,
                                                                KeyBindingList.interactKey,
                                                                skipHighlight: highlight,
                                                                skipUnhighlight: unhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                scriptAtStart: new FaceNorthEastScript(),
                                                                scriptAtEnd: new PlayerInteractScript());

        TutorialSequence vaultableObjectTutorialSequence = new TutorialSequence(OOCActivity.inDialogue, doNoSkipCurrentActivityChange, interactableObjectTutorialSeenFlag, new TutorialSequenceStep[] { stepOne });

        vaultableObjectTutorialSequence.setSkipScript(new SkipInteractionTutorialScript());
        tutorialSequenceDictionary.Add(vaultableObjectTutorialSequenceKey, vaultableObjectTutorialSequence);
    }

    public static void initializeIntimidateTutorial()
    {
        TutorialSequenceStep stepOne = new TutorialSequenceStep(TutorialMessageList.intimidateTutorialMessagePrefix + 1,
                                                                secondTutorialEnemyTargetHash,
                                                                ArrowDirection.Left,
                                                                skipHighlight: highlight,
                                                                skipUnhighlight: unhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                scriptAtStart: new ReplenishIntimidateChargesScript(),
                                                                scriptAtEnd: new ShowIntimidateRangeScript());
        TutorialSequenceStep stepTwo = new TutorialSequenceStep(TutorialMessageList.intimidateTutorialMessagePrefix + 2,
                                                                secondTutorialEnemyTargetHash,
                                                                ArrowDirection.Left,
                                                                skipHighlight: highlight,
                                                                skipUnhighlight: unhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker);
        TutorialSequenceStep stepThree = new TutorialSequenceStep(TutorialMessageList.intimidateTutorialMessagePrefix + 3,
                                                                  skillUIPanelTargetHash,
                                                                  ArrowDirection.Left,
                                                                  KeyBindingList.interactKey,
                                                                  skipHighlight: highlight,
                                                                  skipUnhighlight: unhighlight,
                                                                  createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                  scriptAtEnd: new ActivateIntimidateScript());
        TutorialSequenceStep stepFour = new TutorialSequenceStep(TutorialMessageList.intimidateTutorialMessagePrefix + 4,
                                                                 statusEffectDisplayTargetHash,
                                                                 ArrowDirection.Left,
                                                                 KeyBindingList.moveWestKey,
                                                                 skipHighlight: highlight,
                                                                 skipUnhighlight: unhighlight,
                                                                 createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                 scriptAtEnd: new MovePlayerNorthWestScript());

        TutorialSequence intimidateTutorialSequence = new TutorialSequence(OOCActivity.walking, doNoSkipCurrentActivityChange, intimidateTutorialSeenFlag, new TutorialSequenceStep[] { stepOne, stepTwo, stepThree, stepFour });

        intimidateTutorialSequence.setSkipScript(new SkipTutorialScript());
        tutorialSequenceDictionary.Add(intimidateTutorialSequenceKey, intimidateTutorialSequence);
    }

    public static void initializeFirstCunningTutorial()
    {
        TutorialSequenceStep stepOne = new TutorialSequenceStep(TutorialMessageList.cunningTutorialMessagePrefix + 1,
                                                                secondTutorialEnemyTargetHash,
                                                                ArrowDirection.Top,
                                                                skipHighlight: highlight,
                                                                skipUnhighlight: unhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                scriptAtStart: new ReplenishCunningChargesScript(),
                                                                scriptAtEnd: new ShowCunningRangeScript());
        TutorialSequenceStep stepTwo = new TutorialSequenceStep(TutorialMessageList.cunningTutorialMessagePrefix + 2,
                                                                secondTutorialEnemyTargetHash,
                                                                ArrowDirection.Top,
                                                                KeyBindingList.moveSouthKey,
                                                                skipHighlight: highlight,
                                                                skipUnhighlight: unhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                scriptAtEnd: new MoveCunningTargetSouthWestScript());
        TutorialSequenceStep stepThree = new TutorialSequenceStep(TutorialMessageList.cunningTutorialMessagePrefix + 3,
                                                                  secondTutorialEnemyTargetHash,
                                                                  ArrowDirection.Top,
                                                                  KeyBindingList.interactKey,
                                                                  skipHighlight: highlight,
                                                                  skipUnhighlight: unhighlight,
                                                                  createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                  scriptAtEnd: new ActivateCunningScript());
        TutorialSequenceStep stepFour = new TutorialSequenceStep(TutorialMessageList.cunningTutorialMessagePrefix + 4,
                                                                 statusEffectDisplayTargetHash,
                                                                 ArrowDirection.Left,
                                                                 KeyBindingList.moveWestKey,
                                                                 skipHighlight: highlight,
                                                                 skipUnhighlight: unhighlight,
                                                                 createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                 scriptAtEnd: new MovePlayerNorthWestScript());

        TutorialSequence firstCunningTutorialSequence = new TutorialSequence(OOCActivity.walking, doNoSkipCurrentActivityChange, cunningTutorialSeenFlag, new TutorialSequenceStep[] { stepOne, stepTwo, stepThree, stepFour });

        firstCunningTutorialSequence.setSkipScript(new SkipTutorialScript());
        tutorialSequenceDictionary.Add(firstCunningTutorialSequenceKey, firstCunningTutorialSequence);
    }

    public static void initializeSecondCunningTutorial()
    {
        TutorialSequenceStep stepOne = new TutorialSequenceStep(TutorialMessageList.cunningTutorialMessagePrefix + 5,
                                                                tutorialCunningObjectTargetHash,
                                                                ArrowDirection.Left,
                                                                skipHighlight: highlight,
                                                                skipUnhighlight: unhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                scriptAtStart: new ReplenishCunningChargesScript(),
                                                                scriptAtEnd: new ShowCunningRangeScript());
        TutorialSequenceStep stepTwo = new TutorialSequenceStep(TutorialMessageList.cunningTutorialMessagePrefix + 6,
                                                                skillUIPanelTargetHash,
                                                                ArrowDirection.Left,
                                                                KeyBindingList.moveNorthKey,
                                                                skipHighlight: highlight,
                                                                skipUnhighlight: unhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                scriptAtEnd: new MoveCunningTargetNorthEastScript());
        TutorialSequenceStep stepThree = new TutorialSequenceStep(TutorialMessageList.cunningTutorialMessagePrefix + 7,
                                                                  tutorialCunningObjectTargetHash,
                                                                  ArrowDirection.Left,
                                                                  KeyBindingList.interactKey,
                                                                  scriptAtEnd: new ActivateCunningScript());

        TutorialSequence secondCunningTutorialSequence = new TutorialSequence(OOCActivity.walking, doNoSkipCurrentActivityChange, secondCunningTutorialSeenFlag, new TutorialSequenceStep[] { stepOne, stepTwo, stepThree });

        secondCunningTutorialSequence.setSkipScript(new SkipTutorialScript());
        tutorialSequenceDictionary.Add(secondCunningTutorialSequenceKey, secondCunningTutorialSequence);
    }

    public static void initializeObservationTutorial()
    {
        TutorialSequenceStep stepOne = new TutorialSequenceStep(TutorialMessageList.observationTutorialMessagePrefix + 1,
                                                                secretDoorTargetHash,
                                                                ArrowDirection.Right,
                                                                KeyBindingList.moveNorthKey,
                                                                skipHighlight: skipHighlight,
                                                                skipUnhighlight: skipUnhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                scriptAtStart: new SetToSkillScript(SkillType.Observation),
                                                                scriptAtEnd: new MovePlayerNorthEastScript());
        TutorialSequenceStep stepTwo = new TutorialSequenceStep(TutorialMessageList.observationTutorialMessagePrefix + 2,
                                                                secretDoorTargetHash,
                                                                ArrowDirection.Right,
                                                                skipHighlight: skipHighlight,
                                                                skipUnhighlight: skipUnhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                scriptAtEnd: new ShowObservationRangeScript());
        TutorialSequenceStep stepThree = new TutorialSequenceStep(TutorialMessageList.observationTutorialMessagePrefix + 3,
                                                                  secretDoorTargetHash,
                                                                  ArrowDirection.Right,
                                                                  skipHighlight: skipHighlight,
                                                                  skipUnhighlight: skipUnhighlight,
                                                                  createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                  scriptAtEnd: new HideObservationRangeScript());
        // TutorialSequenceStep stepFour = new TutorialSequenceStep(TutorialMessageList.observationTutorialMessagePrefix + 4,
        //                                                          skillUIPanelTargetHash,
        //                                                          ArrowDirection.Left,
        //                                                          new KeyCode[] { KeyCode.Space },
        //                                                          skipHighlight: highlight,
        //                                                          skipUnhighlight: unhighlight,
        //                                                          createPopUpScreenBlocker: createPopUpScreenBlocker);
        TutorialSequenceStep stepFive = new TutorialSequenceStep(TutorialMessageList.observationTutorialMessagePrefix + 5,
                                                                 secretDoorTargetHash,
                                                                 ArrowDirection.Right,
                                                                 KeyBindingList.moveNorthKey,
                                                                 skipHighlight: skipHighlight,
                                                                 skipUnhighlight: skipUnhighlight,
                                                                 createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                 scriptAtEnd: new MovePlayerNorthEastScript());
        TutorialSequenceStep stepSix = new TutorialSequenceStep(TutorialMessageList.observationTutorialMessagePrefix + 6,
                                                                secretDoorTargetHash,
                                                                ArrowDirection.Right,
                                                                KeyBindingList.interactKey,
                                                                skipHighlight: skipHighlight,
                                                                skipUnhighlight: skipUnhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                scriptAtEnd: new PlayerInteractScript());

        TutorialSequence observationTutorialSequence = new TutorialSequence(OOCActivity.inDialogue,
                                                                             doNoSkipCurrentActivityChange, 
                                                                             observationTutorialSeenFlag, 
                                                                             new TutorialSequenceStep[] { 
                                                                                                            stepOne, 
                                                                                                            stepTwo,
                                                                                                            stepThree, 
                                                                                                            // stepFour, 
                                                                                                            stepFive,
                                                                                                            stepSix
                                                                                                        });

        observationTutorialSequence.setSkipScript(new SkipTutorialScript());
        tutorialSequenceDictionary.Add(observationTutorialSequenceKey, observationTutorialSequence);
    }

    public static void initializeLeadershipTutorial()
    {
        TutorialSequenceStep stepOne = new TutorialSequenceStep(TutorialMessageList.leadershipTutorialMessagePrefix + 1,
                                                                fallenBeamTargetHash,
                                                                ArrowDirection.Right,
                                                                KeyBindingList.moveWestKey,
                                                                skipHighlight: highlight,
                                                                skipUnhighlight: unhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                scriptAtStart: new RemoveAllFollowersScript(),
                                                                scriptAtEnd: new MovePlayerNorthWestScript());
        TutorialSequenceStep stepTwo = new TutorialSequenceStep(TutorialMessageList.leadershipTutorialMessagePrefix + 2,
                                                                tutorialButtonOneTargetHash,
                                                                ArrowDirection.Left,
                                                                skipHighlight: highlight,
                                                                skipUnhighlight: unhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker);
        TutorialSequenceStep stepThree = new TutorialSequenceStep(TutorialMessageList.leadershipTutorialMessagePrefix + 3,
                                                                  tutorialButtonOneTargetHash,
                                                                  ArrowDirection.Left,
                                                                  skipHighlight: highlight,
                                                                  skipUnhighlight: unhighlight,
                                                                  createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                  scriptAtEnd: new PlaceFollowerScript());
        TutorialSequenceStep stepFour = new TutorialSequenceStep(TutorialMessageList.leadershipTutorialMessagePrefix + 4,
                                                                 skillUIPanelTargetHash,
                                                                 ArrowDirection.Left,
                                                                 KeyBindingList.moveNorthKey,
                                                                 skipHighlight: highlight,
                                                                 skipUnhighlight: unhighlight,
                                                                 createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                 scriptAtEnd: new MovePlayerNorthEastScript());
        TutorialSequenceStep stepFive = new TutorialSequenceStep(TutorialMessageList.leadershipTutorialMessagePrefix + 5,
                                                                 placedCharacterTargetHash,
                                                                 ArrowDirection.Right,
                                                                 skipHighlight: highlight,
                                                                 skipUnhighlight: unhighlight,
                                                                 createPopUpScreenBlocker: createPopUpScreenBlocker);
        TutorialSequenceStep stepSix = new TutorialSequenceStep(TutorialMessageList.leadershipTutorialMessagePrefix + 6,
                                                                tutorialButtonTwoTargetHash,
                                                                ArrowDirection.Right,
                                                                KeyBindingList.moveNorthKey,
                                                                skipHighlight: highlight,
                                                                skipUnhighlight: unhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                scriptAtEnd: new MovePlayerNorthEastScript());

        TutorialSequence leadershipTutorialSequence = new TutorialSequence(OOCActivity.walking, doNoSkipCurrentActivityChange, leadershipTutorialSeenFlag, new TutorialSequenceStep[] { stepOne, stepTwo, stepThree, stepFour, stepFive, stepSix });

        leadershipTutorialSequence.setSkipScript(new SkipTutorialScript());
        tutorialSequenceDictionary.Add(leadershipTutorialSequenceKey, leadershipTutorialSequence);
    }

    public static void initializeQuestSymbolTutorial()
    {
        TutorialSequenceStep stepOne = new TutorialSequenceStep(TutorialMessageList.questCounterTutorialMessagePrefix + 1,
                                                                playerSpriteOOCNoArrowTargetHash,
                                                                ArrowDirection.Top,
                                                                KeyBindingList.mapKey,
                                                                skipHighlight: skipHighlight,
                                                                skipUnhighlight: skipUnhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                scriptAtEnd: new OpenMap());
        TutorialSequenceStep stepTwo = new TutorialSequenceStep(TutorialMessageList.questCounterTutorialMessagePrefix + 2,
                                                                  mapPopUpWindow,
                                                                  ArrowDirection.Right,
                                                                  skipHighlight: skipHighlight,
                                                                  skipUnhighlight: skipUnhighlight,
                                                                  createPopUpScreenBlocker: createPopUpScreenBlocker);
        stepTwo.blockInternalRaycastsOnCutOutMask = true;
        TutorialSequenceStep stepThree = new TutorialSequenceStep(TutorialMessageList.questCounterTutorialMessagePrefix + 3,
                                                                  mapTileQuestCounter,
                                                                  ArrowDirection.Top,
                                                                  skipHighlight: skipHighlight,
                                                                  skipUnhighlight: skipUnhighlight,
                                                                  createPopUpScreenBlocker: createPopUpScreenBlocker);
        stepThree.blockInternalRaycastsOnCutOutMask = true;
        TutorialSequenceStep stepFour = new TutorialSequenceStep(TutorialMessageList.questCounterTutorialMessagePrefix + 4,
                                                                    mapQuestList,
                                                                    ArrowDirection.Center,
                                                                    skipHighlight: skipHighlight,
                                                                    skipUnhighlight: skipUnhighlight,
                                                                    createPopUpScreenBlocker: createPopUpScreenBlocker);

        TutorialSequence questCounterTutorialSequence = new TutorialSequence(OOCActivity.inMap, doNoSkipCurrentActivityChange, questCounterTutorialSeenFlag, new TutorialSequenceStep[] { stepOne, stepTwo, stepThree, stepFour});

        questCounterTutorialSequence.setSkipScript(new SkipMapTutorialScript());
        tutorialSequenceDictionary.Add(questCounterTutorialSequenceKey, questCounterTutorialSequence);
    }

    public static void initializeHiddenObjectTutorial()
    {
        TutorialSequenceStep stepOne = new TutorialSequenceStep(TutorialMessageList.hiddenObjectTutorialMessagePrefix + 1,
                                                                questCounterUIPanel,
                                                                ArrowDirection.BottomLeft,
                                                                skipHighlight: skipHighlight,
                                                                skipUnhighlight: skipUnhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker);

        TutorialSequenceStep stepTwo = new TutorialSequenceStep(TutorialMessageList.hiddenObjectTutorialMessagePrefix + 2,
                                                                questCounterUIPanel,
                                                                ArrowDirection.BottomLeft,
                                                                skipHighlight: skipHighlight,
                                                                skipUnhighlight: skipUnhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker);

        TutorialSequenceStep stepThree = new TutorialSequenceStep(TutorialMessageList.hiddenObjectTutorialMessagePrefix + 3,
                                                                playerSpriteOOCNoArrowTargetHash,
                                                                ArrowDirection.Top,
                                                                KeyBindingList.hideTerrainKey,
                                                                skipHighlight: skipHighlight,
                                                                skipUnhighlight: skipUnhighlight,
                                                                createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                scriptAtEnd: new HideTerrain());

        TutorialSequence hiddenObjectsTutorialSequence = new TutorialSequence(OOCActivity.walking, doNoSkipCurrentActivityChange, hiddenObjectsTutorialSeenFlag, new TutorialSequenceStep[] { stepOne, stepTwo, stepThree });

        hiddenObjectsTutorialSequence.setSkipScript(new SkipTutorialScript());
        tutorialSequenceDictionary.Add(hiddenObjectsTutorialSequenceKey, hiddenObjectsTutorialSequence);
    }
    
    public static void initializePlayerLevelUpTutorial()
    {
        TutorialSequenceStep playerLevelUpStepOne = new TutorialSequenceStep(TutorialMessageList.playerLevelUpTutorialMessagePrefix + 1,
                                                                             characterScreenButtonTargetHash,
                                                                             ArrowDirection.Left,
                                                                             KeyBindingList.characterScreenKey,
                                                                             scriptAtStart: new PickFirstCharacterWithLevelUpScript());
        TutorialSequenceStep playerLevelUpStepTwo = new TutorialSequenceStep(TutorialMessageList.playerLevelUpTutorialMessagePrefix + 2,
                                                                             characterScreenStatsTargetHash,
                                                                             ArrowDirection.Top);
        TutorialSequenceStep playerLevelUpStepThree = new TutorialSequenceStep(TutorialMessageList.playerLevelUpTutorialMessagePrefix + 3,
                                                                               characterScreenStatsTargetHash,
                                                                               ArrowDirection.Top,
                                                                               useButtonPress: true,
                                                                               scriptAtStart: new EnableButtonsScript());

        TutorialSequence playerLevelUpTutorialSequence = new TutorialSequence(OOCActivity.inUI, doNoSkipCurrentActivityChange, playerLevelUpTutorialSeenFlag, new TutorialSequenceStep[] { playerLevelUpStepOne, playerLevelUpStepTwo, playerLevelUpStepThree });

        playerLevelUpTutorialSequence.endOfSequenceEvent = PrimaryStatIncreaseButton.PrimaryStatsIncreaseButtonPressed;

        playerLevelUpTutorialSequence.setSkipScript(new SkipUpgradingPartyMemberTutorialScript());
        tutorialSequenceDictionary.Add(playerLevelUpTutorialSequenceKey, playerLevelUpTutorialSequence);
    }

    public static void initializeCompanionSpecificAbilitiesTutorial()
    {
        TutorialSequenceStep companionSpecificAbilitiesStepOne = new TutorialSequenceStep(TutorialMessageList.companionSpecificAbiltiesTutorialMessagePrefix + 1,
                                                                             companionAbilityButtonTargetHash,
                                                                             ArrowDirection.Right);

        TutorialSequence companionSpecificAbilitiesTutorialSequence = new TutorialSequence(OOCActivity.inUI, doNoSkipCurrentActivityChange, companionSpecificAbilitiesTutorialSeenFlag, new TutorialSequenceStep[] { 
                                                                                                                                                                                            companionSpecificAbilitiesStepOne
                                                                                                                                                                                        });

        companionSpecificAbilitiesTutorialSequence.endOfSequenceEvent = AbilityGridSideTab.OnSideTabChosen;

        companionSpecificAbilitiesTutorialSequence.setSkipScript(new SkipUpgradingPartyMemberTutorialScript());
        tutorialSequenceDictionary.Add(companionSpecificAbilitiesTutorialSequenceKey, companionSpecificAbilitiesTutorialSequence);
    }

    public static void initializeMovableObjectTutorial()
    {
        TutorialSequenceStep movableObjectStepOne = new TutorialSequenceStep(TutorialMessageList.movableObjectTutorialMessagePrefix + 1,
                                                                             tutorialCrateTargetHash,
                                                                             ArrowDirection.Left,
                                                                             skipHighlight: skipHighlight,
                                                                             skipUnhighlight: skipUnhighlight,
                                                                             createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                             scriptAtStart: new HighlightTargetScript());
        TutorialSequenceStep movableObjectStepTwo = new TutorialSequenceStep(TutorialMessageList.movableObjectTutorialMessagePrefix + 2,
                                                                             tutorialCrateTargetHash,
                                                                             ArrowDirection.Left,
                                                                             skipHighlight: skipHighlight,
                                                                             skipUnhighlight: skipUnhighlight,
                                                                             createPopUpScreenBlocker: createPopUpScreenBlocker);
        TutorialSequenceStep movableObjectStepThree = new TutorialSequenceStep(TutorialMessageList.movableObjectTutorialMessagePrefix + 3,
                                                                               tutorialCrateTargetHash,
                                                                               ArrowDirection.Left,
                                                                               skipHighlight: skipHighlight,
                                                                               skipUnhighlight: unhighlight,
                                                                               createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                               scriptAtEnd: new UnhighlightTargetScript());

        TutorialSequence movableObjectTutorialSequence = new TutorialSequence(OOCActivity.walking, doNoSkipCurrentActivityChange, movableObjectTutorialSeenFlag, new TutorialSequenceStep[] { movableObjectStepOne, movableObjectStepTwo, movableObjectStepThree });

        movableObjectTutorialSequence.setSkipScript(new SkipTutorialScript());
        tutorialSequenceDictionary.Add(movableObjectTutorialSequenceKey, movableObjectTutorialSequence);
    }

    public static void initializeEquippableItemTutorial()
    {
        TutorialSequenceStep itemStepOne = new TutorialSequenceStep(TutorialMessageList.equippableItemTutorialMessagePrefix + 1,
                                                                    inventoryButtonTargetHash,
                                                                    ArrowDirection.Left,
                                                                    KeyBindingList.inventoryScreenKey,
                                                                    scriptAtEnd: new OpenScreenScript(ScreenType.Inventory));
        TutorialSequenceStep itemStepTwo = new TutorialSequenceStep(TutorialMessageList.equippableItemTutorialMessagePrefix + 2,
                                                                    equipmentGridTargetHash,
                                                                    ArrowDirection.Top,
                                                                    createPopUpScreenBlocker: createPopUpScreenBlocker);
        TutorialSequenceStep itemStepThree = new TutorialSequenceStep(TutorialMessageList.equippableItemTutorialMessagePrefix + 3,
                                                                      itemGridTargetHash,
                                                                      ArrowDirection.Top,
                                                                      createPopUpScreenBlocker: createPopUpScreenBlocker);
        TutorialSequenceStep itemStepFour = new TutorialSequenceStep(TutorialMessageList.equippableItemTutorialMessagePrefix + 4,
                                                                     inventoryScreenTargetHash,
                                                                     ArrowDirection.Top,
                                                                     skipHighlight: skipHighlight,
                                                                     skipUnhighlight: skipUnhighlight,
                                                                     useButtonPress: true);
        itemStepFour.dragWeaponContinueMessage = true;

        TutorialSequence itemTutorialSequence = new TutorialSequence(OOCActivity.inUI, doNoSkipCurrentActivityChange, equippableItemTutorialSeenFlag, new TutorialSequenceStep[] { itemStepOne, itemStepTwo, itemStepThree, itemStepFour });

        itemTutorialSequence.endOfSequenceEvent = EquippedItems.OnEquipmentChange;

        itemTutorialSequence.setSkipScript(new SkipEquippingItemsTutorialScript());
        tutorialSequenceDictionary.Add(itemTutorialSequenceKey, itemTutorialSequence);
    }

    public static void initializeFormationPopUpItemTutorial()
    {
        TutorialSequenceStep formationStepOne = new TutorialSequenceStep(TutorialMessageList.formationTutorialMessagePrefix + 1,
                                                                              partyScreenButton,
                                                                              ArrowDirection.Left,
                                                                              nextStepKey: KeyBindingList.partyScreenKey);
        TutorialSequenceStep formationStepTwo = new TutorialSequenceStep(TutorialMessageList.formationTutorialMessagePrefix + 2,
                                                                              formationGrid, 
                                                                              ArrowDirection.Right,
                                                                              skipHighlight: true,
                                                                              skipUnhighlight: true);
        TutorialSequenceStep formationStepThree = new TutorialSequenceStep(TutorialMessageList.formationTutorialMessagePrefix + 3,
                                                                                partyMemberList, 
                                                                                ArrowDirection.Bottom,
                                                                                skipHighlight: true,
                                                                                skipUnhighlight: true);
        TutorialSequenceStep formationStepFour = new TutorialSequenceStep(TutorialMessageList.formationTutorialMessagePrefix + 4,
                                                                                partySlotTracker,
                                                                                ArrowDirection.Right,
                                                                                skipHighlight: true,
                                                                                skipUnhighlight: true);
        TutorialSequenceStep formationStepFive = new TutorialSequenceStep(TutorialMessageList.formationTutorialMessagePrefix + 5,
                                                                                partyScreen,
                                                                                ArrowDirection.Center,
                                                                                skipHighlight: true,
                                                                                skipUnhighlight: true);

        TutorialSequence formationTutorialSequence = new TutorialSequence(OOCActivity.inUI, doNoSkipCurrentActivityChange, formationTutorialSeenFlag, new TutorialSequenceStep[] { formationStepOne, formationStepTwo, formationStepThree,
                                                                                                                                                                                             formationStepFour, formationStepFive });

        formationTutorialSequence.endOfSequenceEvent = Formation.OnFormationChange;

        formationTutorialSequence.setSkipScript(new SkipFormationTutorialScript());
        tutorialSequenceDictionary.Add(formationTutorialSequenceKey, formationTutorialSequence);
    }

    public static void initializeAddingAbilitiesTutorial()
    {
        // TutorialSequenceStep addingAbilitiesStepOne = new TutorialSequenceStep(TutorialMessageList.addingAbilitiesTutorialMessagePrefix + 1, characterScreenButtonTargetHash, ArrowDirection.Left, new KeyCode[] { KeyCode.C });
        // TutorialSequenceStep addingAbilitiesStepTwo = new TutorialSequenceStep(TutorialMessageList.addingAbilitiesTutorialMessagePrefix + 2, screenBackground, ArrowDirection.Center, new KeyCode[] { KeyCode.Space }, skipHighlight: skipHighlight, skipUnhighlight: skipUnhighlight,  createPopUpScreenBlocker: createPopUpScreenBlocker);
        TutorialSequenceStep addingAbilitiesStepThree = new TutorialSequenceStep(TutorialMessageList.addingAbilitiesTutorialMessagePrefix + 3,
                                                                                 characterScreenAbilityList,
                                                                                 ArrowDirection.Top,
                                                                                 scriptAtStart: new OpenRelevantAbilityTabScript());
        TutorialSequenceStep addingAbilitiesStepFour = new TutorialSequenceStep(TutorialMessageList.addingAbilitiesTutorialMessagePrefix + 4,
                                                                                characterScreenBackground,
                                                                                ArrowDirection.Top,
                                                                                useButtonPress: true,
                                                                                skipHighlight: skipHighlight,
                                                                                skipUnhighlight: skipUnhighlight,
                                                                                scriptAtStart: new EnableButtonsScript());
        addingAbilitiesStepFour.dragActionContinueMessage = true;

        TutorialSequence addingAbilitiesTutorialSequence = new TutorialSequence(OOCActivity.inUI, doNoSkipCurrentActivityChange, addingAbilitiesTutorialSeenFlag, new TutorialSequenceStep[] { addingAbilitiesStepThree,
                                                                                                                                                                                                 addingAbilitiesStepFour });

        addingAbilitiesTutorialSequence.endOfSequenceEvent = CombatActionArray.OnCombatActionArrayChange;

        addingAbilitiesTutorialSequence.setSkipScript(new SkipAddingAbilitiesTutorialScript());

        tutorialSequenceDictionary.Add(addingAbilitiesTutorialSequenceKey, addingAbilitiesTutorialSequence);
    }

    public static void initializeTraitTutorial()
    {
        List<TutorialSequenceStep> traitTutorialSteps = new List<TutorialSequenceStep>();

        TutorialSequenceStep traitTutorialStepOne = new TutorialSequenceStep(TutorialMessageList.combatTraitTutorialMessagePrefix + 1,
                                                                             traitMonsterTargetHash,
                                                                             ArrowDirection.Left,
                                                                             KeyBindingList.moveNorthKey,
                                                                             createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                             scriptAtEnd: new SnapSelectorToMaster());
        traitTutorialStepOne.addShiftToKeyCodeMessage = true;
        traitTutorialSteps.Add(traitTutorialStepOne);

        traitTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTraitTutorialMessagePrefix + 2,
                                                        bottomRightHoverPanelTargetHash,
                                                        ArrowDirection.Top));

        TutorialSequenceStep traitTutorialStepThree = new TutorialSequenceStep(TutorialMessageList.combatTraitTutorialMessagePrefix + 3,
                                                                               traitDisplayTargetHash,
                                                                               ArrowDirection.Left,
                                                                               KeyBindingList.moveSouthKey,
                                                                               scriptAtEnd: new SnapSelectorToPlayer());
        traitTutorialStepThree.addShiftToKeyCodeMessage = true;
        traitTutorialSteps.Add(traitTutorialStepThree);


        TutorialSequence traitTutorialSequence = new TutorialSequence(CurrentActivity.ChoosingActor, doNoSkipCurrentActivityChange, traitTutorialSeenFlag, traitTutorialSteps);
        traitTutorialSequence.preventMouseHovers = true;

        traitTutorialSequence.setSkipScript(new SkipCombatTutorialScript());

        tutorialSequenceDictionary.Add(traitTutorialSequenceKey, traitTutorialSequence);
    }

    public static void initializeMandatoryTargetTutorial()
    {
        List<TutorialSequenceStep> mandatoryTargetTutorialSteps = new List<TutorialSequenceStep>();

        TutorialSequenceStep mandatoryTargetTutorialStepOne = new TutorialSequenceStep(TutorialMessageList.mandatoryTargetTutorialMessagePrefix + 1,
                                                                             mandatoryTargetMonsterTargetHash,
                                                                             ArrowDirection.Left,
                                                                             KeyBindingList.moveNorthKey,
                                                                             skipHighlight: false,
                                                                             skipUnhighlight: true,
                                                                             createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                                             scriptAtEnd: new SnapSelectorToMandatoryTarget());
        mandatoryTargetTutorialStepOne.addShiftToKeyCodeMessage = true;
        mandatoryTargetTutorialSteps.Add(mandatoryTargetTutorialStepOne);

        mandatoryTargetTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.mandatoryTargetTutorialMessagePrefix + 2,
                                                        mandatoryTargetTraitIconTargetHash,
                                                        ArrowDirection.Left));

        TutorialSequenceStep mandatoryTargetTutorialStepThree = new TutorialSequenceStep(TutorialMessageList.mandatoryTargetTutorialMessagePrefix + 3,
                                                                               mandatoryTargetTraitIconTargetHash,
                                                                               ArrowDirection.Left,
                                                                               KeyBindingList.moveSouthKey,
                                                                               scriptAtEnd: new SnapSelectorToPlayer());
        mandatoryTargetTutorialStepThree.addShiftToKeyCodeMessage = true;
        mandatoryTargetTutorialSteps.Add(mandatoryTargetTutorialStepThree);


        TutorialSequence mandatoryTargetTutorialSequence = new TutorialSequence(CurrentActivity.ChoosingActor, doNoSkipCurrentActivityChange, mandatoryTargetTutorialSeenFlag, mandatoryTargetTutorialSteps);
        mandatoryTargetTutorialSequence.preventMouseHovers = true;

        mandatoryTargetTutorialSequence.setSkipScript(new SkipCombatTutorialScript());

        tutorialSequenceDictionary.Add(mandatoryTargetTutorialSequenceKey, mandatoryTargetTutorialSequence);
    }

    public static TutorialSequence getCombatTutorialSequence()
    {
        TutorialSequenceAdditionalScript[] combatTutorialStepFiveAndSevenAdditionalScripts = new TutorialSequenceAdditionalScript[] { new TutorialSequenceAdditionalScript(KeyCode.W, new MoveCurrentSelector()),
                                                                                                                                          new TutorialSequenceAdditionalScript(KeyCode.A, new MoveCurrentSelector()),
                                                                                                                                          new TutorialSequenceAdditionalScript(KeyCode.S, new MoveCurrentSelector()),
                                                                                                                                          new TutorialSequenceAdditionalScript(KeyCode.D, new MoveCurrentSelector()) };

        TutorialSequenceAdditionalScript[] combatTutorialStepFourteenAdditionalScripts = new TutorialSequenceAdditionalScript[] { new TutorialSequenceAdditionalScript(KeyCode.A, new AbilityWheelCycleCounterClockwise()),
                                                                                                                                  new TutorialSequenceAdditionalScript(KeyCode.D, new AbilityWheelCycleClockwise()) };

        List<TutorialSequenceStep> combatTutorialSteps = new List<TutorialSequenceStep>();

        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 1,
                                                         playerCombatSpriteTargetHash,
                                                         ArrowDirection.Top,
                                                         createPopUpScreenBlocker: createPopUpScreenBlocker));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 2,
                                                         allyZoneTargetHash,
                                                         ArrowDirection.Right,
                                                         createPopUpScreenBlocker: createPopUpScreenBlocker));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 3,
                                                         enemyZoneTargetHash,
                                                         ArrowDirection.Left,
                                                         createPopUpScreenBlocker: createPopUpScreenBlocker));

        switch (CombatStateManager.whoIsSurprised)
        {
            case SurpriseState.EnemySurprised:
                combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 4 + " PlayerGetsSurpriseRound",
                                                                 surpriseIconTargetHash,
                                                                 ArrowDirection.BottomRight));
                break;
            case SurpriseState.NoOneSurprised:
                combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 4 + " NoSurpriseRound",
                                                                 surpriseIconTargetHash,
                                                                 ArrowDirection.BottomRight));
                break;
            case SurpriseState.PlayerSurprised:
                combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 4 + " EnemyGetsSurpriseRound",
                                                                 surpriseIconTargetHash,
                                                                 ArrowDirection.BottomRight,
                                                                 scriptAtEnd: new ResolveTurn()));
                break;
        }

        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 5,
                                                         topThirdOfCombatUITargetHash,
                                                         ArrowDirection.Center,
                                                         KeyBindingList.combatSelectKey,
                                                         createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                         scriptAtEnd: new SelectCurrentActor(),
                                                         additionalScripts: combatTutorialStepFiveAndSevenAdditionalScripts,
                                                         allowsMovementKeys: allowsMovementKeys,
                                                         condition: () => SelectCurrentActor.hasActorTarget()));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 6,
                                                         combatActionWheelTargetHash,
                                                         ArrowDirection.BottomRight,
                                                         KeyBindingList.combatSelectKey,
                                                         createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                         scriptAtEnd: new AbilityWheelChooseAbility(),
                                                         additionalScripts: combatTutorialStepFourteenAdditionalScripts,
                                                         condition: () => SelectTarget.canPayCost()));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 7,
                                                         topThirdOfCombatUITargetHash,
                                                         ArrowDirection.Center,
                                                         KeyBindingList.combatSelectKey,
                                                         createPopUpScreenBlocker: createPopUpScreenBlocker,
                                                         scriptAtStart: new DestroyHoverPanel(),
                                                         scriptAtEnd: new SelectTarget(),
                                                         additionalScripts: combatTutorialStepFiveAndSevenAdditionalScripts,
                                                         allowsMovementKeys: allowsMovementKeys,
                                                         condition: () => SelectTarget.hasTargets()));

        combatTutorialSteps = getFinalCombatTutorialSteps(combatTutorialSteps);

        TutorialSequence combatTutorialSequence = new TutorialSequence(CurrentActivity.ChoosingActor, doNoSkipCurrentActivityChange, combatTutorialSeenFlag, combatTutorialSteps);

        combatTutorialSequence.setSkipScript(new SkipCombatTutorialScript());

        return combatTutorialSequence;
    }

    public static TutorialSequence getCombatTutorialSequenceForReposition()
    {
        TutorialSequenceAdditionalScript[] combatTutorialRepositionStepOneAdditionalScripts = new TutorialSequenceAdditionalScript[] {  new TutorialSequenceAdditionalScript(KeyCode.W, new MoveCurrentSelector()),
                                                                                                                                        new TutorialSequenceAdditionalScript(KeyCode.A, new MoveCurrentSelector()),
                                                                                                                                        new TutorialSequenceAdditionalScript(KeyCode.S, new MoveCurrentSelector()),
                                                                                                                                        new TutorialSequenceAdditionalScript(KeyCode.D, new MoveCurrentSelector()) };

        List<TutorialSequenceStep> combatTutorialSteps = new List<TutorialSequenceStep>();

        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialRepositionMessagePrefix + 1,
                                                         topThirdOfCombatUITargetHash,
                                                         ArrowDirection.Center,
                                                         KeyBindingList.combatSelectKey,
                                                         scriptAtEnd: new SelectTertiaryTarget(),
                                                         additionalScripts: combatTutorialRepositionStepOneAdditionalScripts,
                                                         allowsMovementKeys: allowsMovementKeys));

        combatTutorialSteps = getFinalCombatTutorialSteps(combatTutorialSteps);

        TutorialSequence combatTutorialSequence = new TutorialSequence(CurrentActivity.ChoosingActor, doNoSkipCurrentActivityChange, combatTutorialSeenFlag, combatTutorialSteps);

        combatTutorialSequence.setSkipScript(new SkipCombatTutorialScript());

        return combatTutorialSequence;
    }

    private static List<TutorialSequenceStep> getFinalCombatTutorialSteps(List<TutorialSequenceStep> combatTutorialSteps)
    {
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 8,
                                                         actionOrderTargetHash,
                                                         ArrowDirection.Right));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 9,
                                                         actionSlotIconsTargetHash,
                                                         ArrowDirection.BottomRight));

        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 10,
                                                         topThirdOfCombatUITargetHash,
                                                         ArrowDirection.Center,
                                                         KeyBindingList.combatSelectKey));

        return combatTutorialSteps;
    }

    public static void initializeExuberanceCostTutorial()
    {
        List<TutorialSequenceStep> exuberanceCostTutorialSteps = new List<TutorialSequenceStep>();

        TutorialSequenceStep exuberanceCostTutorialStepOne = new TutorialSequenceStep(TutorialMessageList.exuberanceCostTutorialMessagePrefix + 1,
                                                                             combatActionWheelTargetHash,
                                                                             ArrowDirection.Right,
                                                                             createPopUpScreenBlocker: createPopUpScreenBlocker);
        exuberanceCostTutorialSteps.Add(exuberanceCostTutorialStepOne);

        TutorialSequenceStep exuberanceCostTutorialStepTwo = new TutorialSequenceStep(TutorialMessageList.exuberanceCostTutorialMessagePrefix + 2,
                                                                             exuberancesParentTargetHash,
                                                                             ArrowDirection.Right);
        exuberanceCostTutorialSteps.Add(exuberanceCostTutorialStepTwo);

        TutorialSequenceStep exuberanceCostTutorialStepThree = new TutorialSequenceStep(TutorialMessageList.exuberanceCostTutorialMessagePrefix + 3,
                                                                             combatActionDescriptionPanelTargetHash,
                                                                             ArrowDirection.Bottom);
        exuberanceCostTutorialSteps.Add(exuberanceCostTutorialStepThree);

        TutorialSequenceStep exuberanceCostTutorialStepFour = new TutorialSequenceStep(TutorialMessageList.exuberanceCostTutorialMessagePrefix + 4,
                                                                             combatActionWheelTargetHash,
                                                                             ArrowDirection.Right,
                                                                             createPopUpScreenBlocker: createPopUpScreenBlocker);
        exuberanceCostTutorialSteps.Add(exuberanceCostTutorialStepFour);

        TutorialSequence exuberanceCostTutorialSequence = new TutorialSequence(CurrentActivity.ChoosingAbility, doNoSkipCurrentActivityChange, exuberanceCostTutorialSequenceKey, exuberanceCostTutorialSteps);
        exuberanceCostTutorialSequence.preventMouseHovers = true;

        exuberanceCostTutorialSequence.setSkipScript(new SkipCombatTutorialScript());

        tutorialSequenceDictionary.Add(exuberanceCostTutorialSequenceKey, exuberanceCostTutorialSequence);
    }

    public static void initializeTraitCostTutorial()
    {
        List<TutorialSequenceStep> traitCostTutorialSteps = new List<TutorialSequenceStep>();

        TutorialSequenceStep traitCostTutorialStepOne = new TutorialSequenceStep(TutorialMessageList.traitCostTutorialMessagePrefix + 1,
                                                                             combatActionWheelTargetHash,
                                                                             ArrowDirection.Right,
                                                                             createPopUpScreenBlocker: createPopUpScreenBlocker);
        traitCostTutorialSteps.Add(traitCostTutorialStepOne);

        TutorialSequenceStep traitCostTutorialStepTwo = new TutorialSequenceStep(TutorialMessageList.traitCostTutorialMessagePrefix + 2,
                                                                             traitDisplayTargetHash,
                                                                             ArrowDirection.Top);
        traitCostTutorialSteps.Add(traitCostTutorialStepTwo);


        TutorialSequenceStep traitCostTutorialStepThree = new TutorialSequenceStep(TutorialMessageList.traitCostTutorialMessagePrefix + 3,
                                                                             traitDisplayTargetHash,
                                                                             ArrowDirection.Top);
        traitCostTutorialSteps.Add(traitCostTutorialStepThree);

        TutorialSequenceStep traitCostTutorialStepFour = new TutorialSequenceStep(TutorialMessageList.traitCostTutorialMessagePrefix + 4,
                                                                             combatActionDescriptionPanelTargetHash,
                                                                             ArrowDirection.Bottom);
        traitCostTutorialSteps.Add(traitCostTutorialStepFour);

        TutorialSequence traitCostTutorialSequence = new TutorialSequence(CurrentActivity.ChoosingAbility, doNoSkipCurrentActivityChange, traitCostTutorialSequenceKey, traitCostTutorialSteps);
        traitCostTutorialSequence.preventMouseHovers = true;

        traitCostTutorialSequence.setSkipScript(new SkipCombatTutorialScript());

        tutorialSequenceDictionary.Add(traitCostTutorialSequenceKey, traitCostTutorialSequence);
    }

    public static TutorialSequence getTutorialSequence(string key)
    {
        return tutorialSequenceDictionary[key];
    }

    public static string getDescriptionPanelRowTutorialHash(DescriptionPanelBuildingBlock block)
    {
        switch (block.type)
        {
            case DescriptionPanelBuildingBlockType.Name:
                return descriptionPanelNameText;
            case DescriptionPanelBuildingBlockType.DamageText:
                return descriptionPanelDamageText;
        }


        if(block.iconName != null && block.iconName.Equals(IconList.critIconName))
        {
            return descriptionPanelCritText;
        }

        if(block.iconName != null && block.iconName.Equals(IconList.rangeIconName))
        {
            return descriptionPanelRangeText;
        }

        return "";
    }
}


public class EnableButtonsScript : TutorialSequenceStepScript
{
    public override void runScript(GameObject targetObject)
    {
        TutorialSequence.OnEnableButtons.Invoke();
    }
}

public class PickFirstCharacterWithLevelUpScript : TutorialSequenceStepScript
{
    public override void runScript(GameObject targetObject)
    {
        
        if(PartyManager.getPlayerStats().canLevelUp())
        {
            ScreenManager.currentPartyMember = PartyManager.getPlayerStats();
            return;
        }

        List<PartyMember> joinablePartyMembers = PartyManager.getAllJoinablePartyMembers();

        foreach(PartyMember partyMember in joinablePartyMembers)
        {
            if(partyMember != null && partyMember.stats != null && 
                partyMember.stats.canLevelUp())
            {
                ScreenManager.currentPartyMember = partyMember.stats;
                return;
            }
        }

    }
}

public class OpenRelevantAbilityTabScript : TutorialSequenceStepScript
{
    public override void runScript(GameObject targetObject)
    {
        PrimaryStat type = PrimaryStatIncreaseButton.currentPrimaryStat;

        switch (type)
        {
            case PrimaryStat.Strength:
                AbilityGridSideTab.chooseTab(DescribableList.Strength);
                return;
            case PrimaryStat.Dexterity:
                AbilityGridSideTab.chooseTab(DescribableList.Dexterity);
                return;
            case PrimaryStat.Wisdom:
                AbilityGridSideTab.chooseTab(DescribableList.Wisdom);
                return;
            case PrimaryStat.Charisma:
                AbilityGridSideTab.chooseTab(DescribableList.Charisma);
                return;
        }
    }
}

public class OpenScreenScript : TutorialSequenceStepScript
{
    private ScreenType screenType;

    public OpenScreenScript(ScreenType screenType)
    {
        this.screenType = screenType;
    }

    public override void runScript(GameObject targetObject)
    {
        SideScreenButtonManager.getInstance().setCurrentScreenType(screenType);
    }
}