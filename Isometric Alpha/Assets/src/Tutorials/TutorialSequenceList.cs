using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TutorialSequenceList
{
    private const bool createPopUpScreenBlocker = true;
    private const bool doNotCreatePopUpScreenBlocker = false;
    private const bool enableAllDisabledRowButtons = true;
    private const bool skipHighlight = true;
    private const bool skipUnhighlight = true;
    private const bool highlight = false;
    private const bool unhighlight = false;

    private static TutorialSequenceStepScript noScript = null;
    private const string itemTutorialSequenceKey = "Item Tutorial";
    public const string equippableItemTutorialSeenFlag = "equippableItemTutorialSequenceEntered";

    private const string formationPopUpTutorialSequenceKey = "Formation Tutorial";
    public const string formationPopUpTutorialSeenFlag = "formationPopUpTutorialSequenceEntered";

    public const string addingAbilitiesTutorialSequenceKey = "Adding Abilities Tutorial";
    public const string addingAbilitiesTutorialSeenFlag = "addingAbilitiesTutorialSequenceEntered";

    public const string combatTutorialSequenceKey = "Combat Tutorial";
    public const string combatTutorialSeenFlag = "combatTutorialSequenceEntered";
    
    public const string traitTutorialSequenceKey = "Trait Tutorial";
    public const string traitTutorialSeenFlag = "traitTutorialSequenceEntered";

    public const string skipThatchShackTutorialsFlag = "skipThatchShackTutorials";
    public const string firstHostitilityTutorialSeenFlag = "firstHostitilityTutorialSequenceEntered";
	public const string intimidateTutorialSeenFlag = "intimidateTutorialSequenceEntered";
	public const string cunningTutorialSeenFlag = "cunningTutorialSequenceEntered";
	public const string secondCunningTutorialSeenFlag = "secondCunningTutorialSequenceEntered";
	public const string observationTutorialSeenFlag = "observationTutorialSequenceEntered";
	public const string leadershipTutorialSeenFlag = "leadershipTutorialSequenceEntered";
	public const string interactableObjectTutorialSeenFlag = "interactableObjectTutorialSequenceEntered";
    public const string hiddenObjectsTutorialSeenFlag = "hiddenObjectsTutorialSequenceEntered";

    private const string characterScreenStatsTargetHash = "Character Screen Stats";

    private const string characterScreenButtonTargetHash = "Character Screen Button";
    private const string inventoryButtonTargetHash = "Inventory Button";
    private const string equipmentGridTargetHash = "Equipment Grid";
    private const string itemGridTargetHash = "Item Grid";
    private const string inventoryScreenTargetHash = "Inventory Screen";

    private const string screenTabsSection = "Screen Tabs";
    private const string screenBackground = "Screen Background";
    private const string characterScreenBackground = "Character Screen Background";
    private const string characterScreenAbilityWheel = "Character Screen Ability Wheel";
    private const string characterScreenAbilityList = "Character Screen Ability List";
    private const string fullEditActionWheelButton = "Edit Action Wheel Button";
    private const string fullEditActionWheelActionList = "Full Action List";
    private const string actionDescriptionPanel = "Action Description Panel";
    private const string bonusDamagePanel = "Bonus Damage Panel";
    private const string fullActionWheelEditorPopUp = "Full Action Wheel Editor Pop Up";

    private const string formationPopUpTitleTargetHash = "Formation Pop Up Title";
    private const string partySlotTracker = "Party Slot Tracker";
    private const string partyMemberList = "Party Member List";
    private const string partyMemberRow = "Party Member Row";
    private const string formationGrid = "Formation Grid";
    private const string acceptButton = "Accept Button";

    private const string descriptionPanelNameText = "Name Text";
    private const string descriptionPanelDamageText = "Damage Text";
    private const string descriptionPanelCritText = "Crit Text";
    private const string descriptionPanelRangeText = "Range Text";

    private const string combatUIParentTargetHash = "Combat UI Parent";
    private const string playerCombatSpriteTargetHash = "Player Combat";
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
    private const string retreatUITargetHash = "Retreat UI";
    private const string traitMonsterTargetHash = "Trait Monster";

    private const string movableObjectTutorialSequenceKey = "Movable Object Tutorial";
    public const string movableObjectTutorialSeenFlag = "movableObjectTutorialSequenceEntered";
    private const string tutorialCrateTargetHash = "Tutorial Crate";

    public const string questCounterTutorialSeenFlag = "questCounterTutorialSequenceEntered";

    public const string partyMemberUpgradeTutorialSequenceKey = "Party Member Upgrade Tutorial";
    public const string partyMemberUpgradeTutorialSeenFlag = "partyMemberUpgradeTutorialSequenceEntered";
    public const string playerLevelUpTutorialSequenceKey = "Player Level Up Tutorial";
    public const string playerLevelUpTutorialSeenFlag = "playerLevelUpTutorialSequenceEntered";
    public const string playerSpriteOOCTargetHash = "Player";
    public const string partyScreenButtonTargetHash = "Party Screen Button";
    public const string affinityCounterTargetHash = "Affinity Counter";

    private const bool doNoSkipCurrentActivityChange = false;

    private static Dictionary<string, TutorialSequence> tutorialSequenceDictionary;

    static TutorialSequenceList()
    {
        initializeTutorials();
    }

    public static void initializeTutorials()
    {
        tutorialSequenceDictionary = new Dictionary<string, TutorialSequence>();

        initializeEquippableItemTutorial();
        initializeFormationPopUpItemTutorial();
        initializeAddingAbilitiesTutorial();
        initializeTraitTutorial();
        initializeMovableObjectTutorial();
        initializePartyMemberUpgradeTutorial();
        initializePlayerLevelUpTutorial();
    }

    public static void initializePartyMemberUpgradeTutorial()
    {
        TutorialSequenceStep partyMemberUpgradeStepOne = new TutorialSequenceStep(TutorialMessageList.partyMemberUpgradeTutorialMessagePrefix + 1, playerSpriteOOCTargetHash, ArrowDirection.Top, new KeyCode[] { KeyCode.Space }, skipHighlight, skipUnhighlight, createPopUpScreenBlocker);
        TutorialSequenceStep partyMemberUpgradeStepTwo = new TutorialSequenceStep(TutorialMessageList.partyMemberUpgradeTutorialMessagePrefix + 2, partyScreenButtonTargetHash, ArrowDirection.Left, new KeyCode[] { KeyCode.P }, createPopUpScreenBlocker);
        TutorialSequenceStep partyMemberUpgradeStepThree = new TutorialSequenceStep(TutorialMessageList.partyMemberUpgradeTutorialMessagePrefix + 3, affinityCounterTargetHash, ArrowDirection.Top, new KeyCode[] { KeyCode.Space });
        TutorialSequenceStep partyMemberUpgradeStepFour = new TutorialSequenceStep(TutorialMessageList.partyMemberUpgradeTutorialMessagePrefix + 4, affinityCounterTargetHash, ArrowDirection.Top, new KeyCode[] { KeyCode.Space });

        TutorialSequence partyMemberUpgradeTutorialSequence = new TutorialSequence(OOCActivity.inUI, doNoSkipCurrentActivityChange, partyMemberUpgradeTutorialSeenFlag, new TutorialSequenceStep[] { partyMemberUpgradeStepOne, partyMemberUpgradeStepTwo, partyMemberUpgradeStepThree, partyMemberUpgradeStepFour });

        partyMemberUpgradeTutorialSequence.setSkipScript(new SkipUpgradingPartyMemberTutorialScript());
        tutorialSequenceDictionary.Add(partyMemberUpgradeTutorialSequenceKey, partyMemberUpgradeTutorialSequence);
    }

    public static void initializePlayerLevelUpTutorial()
    {
        TutorialSequenceStep playerLevelUpStepOne = new TutorialSequenceStep(TutorialMessageList.playerLevelUpTutorialMessagePrefix + 1, characterScreenButtonTargetHash, ArrowDirection.Left, new KeyCode[] { KeyCode.C });
        TutorialSequenceStep playerLevelUpStepTwo = new TutorialSequenceStep(TutorialMessageList.playerLevelUpTutorialMessagePrefix + 2, characterScreenStatsTargetHash, ArrowDirection.Top, new KeyCode[] { KeyCode.Space });
        TutorialSequenceStep playerLevelUpStepThree = new TutorialSequenceStep(TutorialMessageList.playerLevelUpTutorialMessagePrefix + 3, characterScreenStatsTargetHash, new EnableButtonsScript(), noScript, ArrowDirection.Top, new KeyCode[] { });

        TutorialSequence playerLevelUpTutorialSequence = new TutorialSequence(OOCActivity.inUI, doNoSkipCurrentActivityChange, partyMemberUpgradeTutorialSeenFlag, new TutorialSequenceStep[] { playerLevelUpStepOne, playerLevelUpStepTwo, playerLevelUpStepThree });

        playerLevelUpTutorialSequence.endOfSequenceEvent = PrimaryStatIncreaseButton.PrimaryStatsIncreaseButtonPressed;

        playerLevelUpTutorialSequence.setSkipScript(new SkipUpgradingPartyMemberTutorialScript());
        tutorialSequenceDictionary.Add(playerLevelUpTutorialSequenceKey, playerLevelUpTutorialSequence);
    }

    public static void initializeMovableObjectTutorial()
    {
        TutorialSequenceStep movableObjectStepOne = new TutorialSequenceStep(TutorialMessageList.movableObjectTutorialMessagePrefix + 1, tutorialCrateTargetHash, new HighlightTargetScript(), noScript, ArrowDirection.Left, new KeyCode[] { KeyCode.Space }, skipHighlight, skipUnhighlight, createPopUpScreenBlocker);
        TutorialSequenceStep movableObjectStepTwo = new TutorialSequenceStep(TutorialMessageList.movableObjectTutorialMessagePrefix + 2, tutorialCrateTargetHash, ArrowDirection.Left, new KeyCode[] { KeyCode.Space }, skipHighlight, skipUnhighlight, createPopUpScreenBlocker);
        TutorialSequenceStep movableObjectStepThree = new TutorialSequenceStep(TutorialMessageList.movableObjectTutorialMessagePrefix + 3, tutorialCrateTargetHash, noScript, new UnhighlightTargetScript(), ArrowDirection.Left, new KeyCode[] { KeyCode.Space }, skipHighlight, unhighlight, createPopUpScreenBlocker);

        TutorialSequence movableObjectTutorialSequence = new TutorialSequence(OOCActivity.walking, doNoSkipCurrentActivityChange, movableObjectTutorialSeenFlag, new TutorialSequenceStep[] { movableObjectStepOne, movableObjectStepTwo, movableObjectStepThree });

        movableObjectTutorialSequence.setSkipScript(new SkipTutorialScript());
        tutorialSequenceDictionary.Add(movableObjectTutorialSequenceKey, movableObjectTutorialSequence);
    }

    public static void initializeEquippableItemTutorial()
    {
        TutorialSequenceStep itemStepOne = new TutorialSequenceStep(TutorialMessageList.equippableItemTutorialMessagePrefix + 1, inventoryButtonTargetHash, ArrowDirection.Left, new KeyCode[] { KeyCode.I });
        TutorialSequenceStep itemStepTwo = new TutorialSequenceStep(TutorialMessageList.equippableItemTutorialMessagePrefix + 2, equipmentGridTargetHash, ArrowDirection.Top, new KeyCode[] { KeyCode.Space }, createPopUpScreenBlocker);
        TutorialSequenceStep itemStepThree = new TutorialSequenceStep(TutorialMessageList.equippableItemTutorialMessagePrefix + 3, itemGridTargetHash, ArrowDirection.Top, new KeyCode[] {KeyCode.Space }, createPopUpScreenBlocker);
        TutorialSequenceStep itemStepFour = new TutorialSequenceStep(TutorialMessageList.equippableItemTutorialMessagePrefix + 4, inventoryScreenTargetHash, ArrowDirection.Top, new KeyCode[] { }, skipHighlight, skipUnhighlight);
        itemStepFour.dragWeaponContinueMessage = true;

        TutorialSequence itemTutorialSequence = new TutorialSequence(OOCActivity.inUI, doNoSkipCurrentActivityChange, equippableItemTutorialSeenFlag, new TutorialSequenceStep[] { itemStepOne, itemStepTwo, itemStepThree, itemStepFour });

        itemTutorialSequence.endOfSequenceEvent = EquippedItems.OnEquipmentChange;

        itemTutorialSequence.setSkipScript(new SkipEquippingItemsTutorialScript());
        tutorialSequenceDictionary.Add(itemTutorialSequenceKey, itemTutorialSequence);
    }

    public static void initializeFormationPopUpItemTutorial()
    {
        TutorialSequenceStep formationPopUpStepOne = new TutorialSequenceStep(TutorialMessageList.formationPopUpTutorialMessagePrefix + 1, formationPopUpTitleTargetHash, ArrowDirection.Left, new KeyCode[] { KeyCode.Space });
        TutorialSequenceStep formationPopUpStepTwo = new TutorialSequenceStep(TutorialMessageList.formationPopUpTutorialMessagePrefix + 2, partySlotTracker, ArrowDirection.Right, new KeyCode[] { KeyCode.Space });
        TutorialSequenceStep formationPopUpStepThree = new TutorialSequenceStep(TutorialMessageList.formationPopUpTutorialMessagePrefix + 3, partyMemberList, ArrowDirection.Right, new KeyCode[] {  });
        TutorialSequenceStep formationPopUpStepFour = new TutorialSequenceStep(TutorialMessageList.formationPopUpTutorialMessagePrefix + 4, formationGrid, ArrowDirection.Right, new KeyCode[] {  });
        TutorialSequenceStep formationPopUpStepFive = new TutorialSequenceStep(TutorialMessageList.formationPopUpTutorialMessagePrefix + 5, acceptButton, noScript, new AcceptFormation(), ArrowDirection.Right, new KeyCode[] { KeyCode.E });

        TutorialSequence formationPopUpTutorialSequence = new TutorialSequence(OOCActivity.walking, doNoSkipCurrentActivityChange, formationPopUpTutorialSeenFlag, new TutorialSequenceStep[] { formationPopUpStepOne, formationPopUpStepTwo, formationPopUpStepThree,
                                                                                                                                                                                             formationPopUpStepFour, formationPopUpStepFive });

        formationPopUpTutorialSequence.setSkipScript(new SkipFormationTutorialScript());
        tutorialSequenceDictionary.Add(formationPopUpTutorialSequenceKey, formationPopUpTutorialSequence);
    }

    public static void initializeAddingAbilitiesTutorial()
    {
        // TutorialSequenceStep addingAbilitiesStepOne = new TutorialSequenceStep(TutorialMessageList.addingAbilitiesTutorialMessagePrefix + 1, characterScreenButtonTargetHash, ArrowDirection.Left, new KeyCode[] { KeyCode.C });
        // TutorialSequenceStep addingAbilitiesStepTwo = new TutorialSequenceStep(TutorialMessageList.addingAbilitiesTutorialMessagePrefix + 2, screenBackground, ArrowDirection.Center, new KeyCode[] { KeyCode.Space }, skipHighlight, skipUnhighlight, createPopUpScreenBlocker);
        TutorialSequenceStep addingAbilitiesStepThree = new TutorialSequenceStep(TutorialMessageList.addingAbilitiesTutorialMessagePrefix + 3, characterScreenAbilityList, new OpenRelevantAbilityTabScript(), noScript, ArrowDirection.Top, new KeyCode[] { KeyCode.Space });
        TutorialSequenceStep addingAbilitiesStepFour = new TutorialSequenceStep(TutorialMessageList.addingAbilitiesTutorialMessagePrefix + 4, characterScreenBackground, new EnableButtonsScript(), noScript, ArrowDirection.Left, new KeyCode[] { }, skipHighlight, skipUnhighlight, doNotCreatePopUpScreenBlocker);
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

        TutorialSequenceStep traitTutorialStepOne = new TutorialSequenceStep(TutorialMessageList.combatTraitTutorialMessagePrefix + 1, traitMonsterTargetHash, noScript, new SnapSelectorToMaster(), ArrowDirection.Left, new KeyCode[] { KeyCode.W }, createPopUpScreenBlocker);
        traitTutorialStepOne.addShiftToKeyCodeMessage = true;
        traitTutorialSteps.Add(traitTutorialStepOne);
        

        traitTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTraitTutorialMessagePrefix + 2, bottomRightHoverPanelTargetHash, ArrowDirection.Left, new KeyCode[] { KeyCode.Space }));

        TutorialSequenceStep traitTutorialStepThree = new TutorialSequenceStep(TutorialMessageList.combatTraitTutorialMessagePrefix + 3, traitDisplayTargetHash, noScript, new SnapSelectorToPlayer(), ArrowDirection.Left, new KeyCode[] { KeyCode.S });
        traitTutorialStepThree.addShiftToKeyCodeMessage = true;
        traitTutorialSteps.Add(traitTutorialStepThree);


        TutorialSequence traitTutorialSequence = new TutorialSequence(CurrentActivity.ChoosingActor, doNoSkipCurrentActivityChange, traitTutorialSequenceKey, traitTutorialSteps);

        traitTutorialSequence.setSkipScript(new SkipCombatTutorialScript());

        tutorialSequenceDictionary.Add(traitTutorialSequenceKey, traitTutorialSequence);
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

        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 1, playerCombatSpriteTargetHash, ArrowDirection.Top, new KeyCode[] { KeyCode.Space }, createPopUpScreenBlocker));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 2, allyZoneTargetHash, ArrowDirection.Right, new KeyCode[] { KeyCode.Space }, createPopUpScreenBlocker));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 3, enemyZoneTargetHash, ArrowDirection.Left, new KeyCode[] { KeyCode.Space }, createPopUpScreenBlocker));

        switch (CombatStateManager.whoIsSurprised)
        {
            case SurpriseState.EnemySurprised:
                combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 4 + " PlayerGetsSurpriseRound", surpriseIconTargetHash, ArrowDirection.BottomRight, new KeyCode[] { KeyCode.Space }));
                break;
            case SurpriseState.NoOneSurprised:
                combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 4 + " NoSurpriseRound", surpriseIconTargetHash, ArrowDirection.BottomRight, new KeyCode[] { KeyCode.Space }));
                break;
            case SurpriseState.PlayerSurprised:
                combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 4 + " EnemyGetsSurpriseRound", surpriseIconTargetHash, noScript, new ResolveTurn(), ArrowDirection.BottomRight, new KeyCode[] { KeyCode.Space }));
                break;
        }

        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 5, topThirdOfCombatUITargetHash, noScript, new SelectCurrentActor(), combatTutorialStepFiveAndSevenAdditionalScripts, ArrowDirection.Center, new KeyCode[] { KeyCode.E }, createPopUpScreenBlocker));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 6, combatActionWheelTargetHash, noScript, new AbilityWheelChooseAbility(), combatTutorialStepFourteenAdditionalScripts, ArrowDirection.Right, new KeyCode[] { KeyCode.E }, createPopUpScreenBlocker));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 7, topThirdOfCombatUITargetHash, new DestroyHoverPanel(), new SelectTarget(), combatTutorialStepFiveAndSevenAdditionalScripts, ArrowDirection.Center, new KeyCode[] { KeyCode.E }, createPopUpScreenBlocker));

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

        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialRepositionMessagePrefix + 1, topThirdOfCombatUITargetHash, noScript, new SelectTertiaryTarget(), combatTutorialRepositionStepOneAdditionalScripts, ArrowDirection.Center, new KeyCode[] { KeyCode.E }));

        combatTutorialSteps = getFinalCombatTutorialSteps(combatTutorialSteps);

        TutorialSequence combatTutorialSequence = new TutorialSequence(CurrentActivity.ChoosingActor, doNoSkipCurrentActivityChange, combatTutorialSeenFlag, combatTutorialSteps);

        combatTutorialSequence.setSkipScript(new SkipCombatTutorialScript());

        return combatTutorialSequence;
    }

    private static List<TutorialSequenceStep> getFinalCombatTutorialSteps(List<TutorialSequenceStep> combatTutorialSteps)
    {
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 8, actionOrderTargetHash, ArrowDirection.Right, new KeyCode[] { KeyCode.Space }));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 9, actionSlotIconsTargetHash, ArrowDirection.BottomRight, new KeyCode[] { KeyCode.Space }));

        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 10, topThirdOfCombatUITargetHash, ArrowDirection.Center, new KeyCode[] { KeyCode.E }));

        return combatTutorialSteps;
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

public class OpenRelevantAbilityTabScript : TutorialSequenceStepScript
{
    public override void runScript(GameObject targetObject)
    {
        PrimaryStat type = PrimaryStatIncreaseButton.currentStatType;
        int abilityGridIndex = OverallUIManager.currentScreenManager.getAbilityGridIndex();
        TabCollection abilityGridTabCollection = OverallUIManager.currentScreenManager.tabCollections[abilityGridIndex];

        switch (type)
        {
            case PrimaryStat.Strength:
                abilityGridTabCollection.selectAndClickTab(2);
                return;
            case PrimaryStat.Dexterity:
                abilityGridTabCollection.selectAndClickTab(3);
                return;
            case PrimaryStat.Wisdom:
                abilityGridTabCollection.selectAndClickTab(4);
                return;
            case PrimaryStat.Charisma:
                abilityGridTabCollection.selectAndClickTab(5);
                return;
        }
    }
}

/* Old Combat Tutorial
    public static TutorialSequence getCombatTutorialSequence()
    {
        TutorialSequenceAdditionalScript[] combatTutorialStepElevenAndFifteenAdditionalScripts = new TutorialSequenceAdditionalScript[] { new TutorialSequenceAdditionalScript(KeyCode.W, new MoveCurrentSelector()),
                                                                                                                                          new TutorialSequenceAdditionalScript(KeyCode.A, new MoveCurrentSelector()),
                                                                                                                                          new TutorialSequenceAdditionalScript(KeyCode.S, new MoveCurrentSelector()),
                                                                                                                                          new TutorialSequenceAdditionalScript(KeyCode.D, new MoveCurrentSelector()) };

        TutorialSequenceAdditionalScript[] combatTutorialStepFourteenAdditionalScripts = new TutorialSequenceAdditionalScript[] { new TutorialSequenceAdditionalScript(KeyCode.A, new AbilityWheelCycleCounterClockwise()),
                                                                                                                                  new TutorialSequenceAdditionalScript(KeyCode.D, new AbilityWheelCycleClockwise()) };

        List<TutorialSequenceStep> combatTutorialSteps = new List<TutorialSequenceStep>();

        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 1, topThirdOfCombatUITargetHash, ArrowDirection.Center, new KeyCode[] { KeyCode.Space }, createPopUpScreenBlocker));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 2, playerCombatSpriteTargetHash, ArrowDirection.Top, new KeyCode[] { KeyCode.Space }, createPopUpScreenBlocker));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 3, allyZoneTargetHash, ArrowDirection.Right, new KeyCode[] { KeyCode.Space }, createPopUpScreenBlocker));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 4, enemyZoneTargetHash, ArrowDirection.Left, new KeyCode[] { KeyCode.Space }, createPopUpScreenBlocker));

        switch(CombatStateManager.whoIsSurprised)
        {
            case SurpriseState.EnemySurprised:
                Debug.LogError("CombatStateManager.whoIsSurprised = " + CombatStateManager.whoIsSurprised.ToString());
                combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 5 + " PlayerGetsSurpriseRound", surpriseIconTargetHash, ArrowDirection.BottomRight, new KeyCode[] { KeyCode.Space }));
                break;
            case SurpriseState.NoOneSurprised:
                Debug.LogError("CombatStateManager.whoIsSurprised = " + CombatStateManager.whoIsSurprised.ToString());
                combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 5 + " NoSurpriseRound", surpriseIconTargetHash, ArrowDirection.BottomRight, new KeyCode[] { KeyCode.Space }));
                break;
            case SurpriseState.PlayerSurprised:
                Debug.LogError("CombatStateManager.whoIsSurprised = " + CombatStateManager.whoIsSurprised.ToString());
                combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 5 + " EnemyGetsSurpriseRound", surpriseIconTargetHash, noScript, new ResolveTurn(), ArrowDirection.BottomRight, new KeyCode[] { KeyCode.Space }));
                break;
        }

        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 6, singleTargetSelectorTargetHash, noScript, new SnapSelectorToMaster(), ArrowDirection.Right, new KeyCode[] { KeyCode.W }, createPopUpScreenBlocker));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 7, bottomRightHoverPanelTargetHash, ArrowDirection.Left, new KeyCode[] { KeyCode.Space }));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 8, traitDisplayTargetHash, ArrowDirection.Left, new KeyCode[] { KeyCode.Space }));

        if (CombatGrid.actualEnemyMinionCombatActionCount() > 0)
        {
            combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 9, masterTraitIconTargetHash, noScript, new SnapSelectorToMinion(),ArrowDirection.Left, new KeyCode[] { KeyCode.Space }));
            combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 10 + " MinionTraitIcon", minionTraitIconTargetHash, noScript, new SnapSelectorToPlayer(), ArrowDirection.Left, new KeyCode[] { KeyCode.S }));
        }
        else
        {
            combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 9 + " SnapToPlayer", masterTraitIconTargetHash,  noScript, new SnapSelectorToPlayer(), ArrowDirection.Left, new KeyCode[] { KeyCode.S }));
        }
 
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 11, topThirdOfCombatUITargetHash, noScript, new SelectCurrentActor(), combatTutorialStepElevenAndFifteenAdditionalScripts, ArrowDirection.Center, new KeyCode[] { KeyCode.E }, createPopUpScreenBlocker));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 12, combatActionWheelTargetHash, new DestroyHoverPanel(), noScript, ArrowDirection.Right, new KeyCode[] { KeyCode.Space }, createPopUpScreenBlocker));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 13, combatActionWheelTargetHash, ArrowDirection.Right, new KeyCode[] { KeyCode.Space }, createPopUpScreenBlocker));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 14, combatActionWheelTargetHash, noScript, new AbilityWheelChooseAbility(), combatTutorialStepFourteenAdditionalScripts, ArrowDirection.Right, new KeyCode[] { KeyCode.E }, createPopUpScreenBlocker));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 15, topThirdOfCombatUITargetHash, noScript, new SelectTarget(), combatTutorialStepElevenAndFifteenAdditionalScripts, ArrowDirection.Center, new KeyCode[] { KeyCode.E }, createPopUpScreenBlocker));

        combatTutorialSteps = getFinalCombatTutorialSteps(combatTutorialSteps);

        TutorialSequence combatTutorialSequence = new TutorialSequence(CurrentActivity.ChoosingActor, doNoSkipCurrentActivityChange, combatTutorialSeenFlag, combatTutorialSteps);

        return combatTutorialSequence;
    }

    public static TutorialSequence getCombatTutorialSequenceForReposition()
    {
        TutorialSequenceAdditionalScript[] combatTutorialRepositionStepOneAdditionalScripts = new TutorialSequenceAdditionalScript[] {  new TutorialSequenceAdditionalScript(KeyCode.W, new MoveCurrentSelector()),
                                                                                                                                        new TutorialSequenceAdditionalScript(KeyCode.A, new MoveCurrentSelector()),
                                                                                                                                        new TutorialSequenceAdditionalScript(KeyCode.S, new MoveCurrentSelector()),
                                                                                                                                        new TutorialSequenceAdditionalScript(KeyCode.D, new MoveCurrentSelector()) };

        List<TutorialSequenceStep> combatTutorialSteps = new List<TutorialSequenceStep>();

        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialRepositionMessagePrefix + 1, topThirdOfCombatUITargetHash, noScript, new SelectTertiaryTarget(), combatTutorialRepositionStepOneAdditionalScripts, ArrowDirection.Center, new KeyCode[] { KeyCode.E }));

        combatTutorialSteps = getFinalCombatTutorialSteps(combatTutorialSteps);

        TutorialSequence combatTutorialSequence = new TutorialSequence(CurrentActivity.ChoosingActor, doNoSkipCurrentActivityChange, combatTutorialSeenFlag, combatTutorialSteps);

        return combatTutorialSequence;
    }

    private static List<TutorialSequenceStep> getFinalCombatTutorialSteps(List<TutorialSequenceStep> combatTutorialSteps)
    {
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 16, actionOrderTargetHash, ArrowDirection.Right, new KeyCode[] { KeyCode.Space }));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 17, actionSlotIconsTargetHash, ArrowDirection.BottomRight, new KeyCode[] { KeyCode.Space }));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 18, actionSlotIconsTargetHash, ArrowDirection.BottomRight, new KeyCode[] { KeyCode.Space }));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 19, retreatUITargetHash, ArrowDirection.BottomRight, new KeyCode[] { KeyCode.Space }, createPopUpScreenBlocker));
        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 20, retreatUITargetHash, ArrowDirection.BottomRight, new KeyCode[] { KeyCode.Space }, createPopUpScreenBlocker));

        combatTutorialSteps.Add(new TutorialSequenceStep(TutorialMessageList.combatTutorialMessagePrefix + 21, topThirdOfCombatUITargetHash, ArrowDirection.Center, new KeyCode[] { KeyCode.E }));

        return combatTutorialSteps;
    }
    */