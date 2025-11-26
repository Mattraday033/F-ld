using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public interface IDecisionPanel
{
	public GameObject getGameObject();

	public void setObjectToBeDecidedOn(IDescribable describable);

	public void setScrollableUIElement(ScrollableUIElement grid);

	public void setCollectionIndex(int currentTabCollection);

	public void updateEnabledButtons();

	public string getDescribableRowKey();
}


public class LoadSaveFile : IDecision
{
    private const string loadLostProgressMessageStart = "Are you sure you want to load '";
    private const string loadLostProgressMessageEnd = "'? Any unsaved progress will be lost.";

    public string saveName;

    private SaveBlueprint saveBlueprint;


    public LoadSaveFile()
    {
        this.saveName = null;
    }

    public LoadSaveFile(string saveName)
    {
        this.saveName = saveName;
    }

    public string getMessage()
    {
        return loadLostProgressMessageStart + saveName + loadLostProgressMessageEnd;
    }

    public void execute()
    {
        NotificationManager.purgeNotifications();
        StepCountScriptManager.reset();
        SaveHandler.createSavedGameList();
        OverallUIManager.setCurrentScreenType(null);
        TutorialSequenceList.initializeTutorials();
        MovementManager.initializeMovementManager();
        State.dialogueUponSceneLoadKey = null;

        if (!SceneManager.GetActiveScene().name.Equals(SceneNameList.loadingScreen))
        {
            LoadingBarProgressTracker.loadSaveFile = this;
            SceneChange.changeSceneToLoadingScreen();
        }
        else
        {
            ChoiceManager.resetChoices();
            QuestList.buildQuestListFromScratch();

            if (CombatStateManager.inCombat)
            {
                CombatStateManager.resetCombat();
                CombatStateManager.inCombat = false;
            }

            if (saveName == null)
            {
                saveBlueprint = SaveHandler.getCleanSlateSave();
            }
            else
            {
                saveBlueprint = SaveHandler.getDataFromSaveFile(saveName);
                Flags.exitNewGameMode();
            }

            Flags.resetAllFlags();
            Flags.overwriteFlags(saveBlueprint.currentFlags);

            MovementManager.setFooting(saveBlueprint.onLeftFoot);

            State.playerFacing = new CharacterFacing();
            State.playerFacing.setFacing((Facing)saveBlueprint.playerFacing);

            AreaManager.locationName = saveBlueprint.currentLocation;

            State.terrainHidden = saveBlueprint.terrainHidden;

            if (!Flags.isInNewGameMode()) //if in newgame mode, this is handled in CharacterCreationPopUpWindow 
            {
                saveBlueprint.extractPartyMemberDetails();
                State.formation.implementGridFromCoordSet(saveBlueprint.partyMemberStats);
                AreaManager.saveBlueprint = saveBlueprint;
            }
            else
            {
                Flags.setStatTutorialFlag();
                Flags.exitNewGameMode();
            }

            State.inventory = SaveBlueprint.extractInventoryItemsFromJson(saveBlueprint.currentInventory);
            State.junkPocket = SaveBlueprint.extractInventoryItemsFromJson(saveBlueprint.currentJunk);
            State.lessonsLearned = saveBlueprint.extractAllLessonKeysFromJson();

            ChoiceManager.choices = saveBlueprint.extractChoicesFromJson();
            DeathFlagManager.deadNames = saveBlueprint.extractArrayListOfStringsFromJson(saveBlueprint.currentDeathFlags);
            MetFlagManager.metNames = saveBlueprint.extractArrayListOfStringsFromJson(saveBlueprint.currentMetFlags);
            GateAndChestManager.resetGatesAndChests(FlagWrapper.convertFlagWrapperListToDictionary(saveBlueprint.currentChestAndGateFlags));
            TrapAndButtonStateManager.resetTrapKeys(saveBlueprint.currentActivatedTrapsAndButtons);

            IntimidateManager.setIntimidatesRemaining(saveBlueprint.intimidatesRemaining);
            CunningManager.setCunningsRemaining(saveBlueprint.cunningsRemaining);

            Dictionary<string, Dictionary<string, Item>> newShopkeeperInventories = SaveBlueprint.extractShopkeeperInventoriesFromJson(saveBlueprint.currentShopkeeperInventories);
            Dictionary<string, Dictionary<string, Item>> newBuyBackInventories = SaveBlueprint.extractShopkeeperInventoriesFromJson(saveBlueprint.currentBuyBackInventories);

            ShopkeeperInventoryList.setShopkeeperInventoryList(newShopkeeperInventories, newBuyBackInventories);

            // State.currentMonsterPackList = saveBlueprint.extractMonsterPackListFromJson();

            SaveBlueprint.resetAndOverwriteQuestDictionary(saveBlueprint.extractQuestListFromJson());
            State.allKnownMapData = saveBlueprint.extractAllKnownMapDataFromJson();
            saveBlueprint.extractAllAreaHostilitiesFromJson();

            SecretDoorFlags.setFromSaveData(saveBlueprint.secretDoors);

            Purse.setCoinsInPurse(saveBlueprint.gold);

            MonsterDefeatKeysList.extractAllMonsterDefeatKeys(saveBlueprint);

            CombatStateManager.inCombat = false;

            SpeechLog.cleanSpeechLog();

            JournalHandler.wipeLastOpened();

            TestScript.addTestVariables();

            EscapeStack.escapeAll();

            FadeToBlackManager.setToMaxOpacity();

            if (saveBlueprint.overworldSpriteSortingLayer != null && saveBlueprint.overworldSpriteSortingLayer.Length > 0)
            {
                SceneTransitionPosition.sortingLayer = saveBlueprint.overworldSpriteSortingLayer;
            }

            skipTutorials();

            SpawnInfoManager.lastSaveBlueprint = saveBlueprint;

            SceneChange.changeSceneToOverworld();

        }
    }

    public void backOut()
    {

    }

    public static void loadCleanSlateSaveFile()
    {
        LoadSaveFile loadCleanSlateSaveFile = new LoadSaveFile();

        loadCleanSlateSaveFile.execute();
    }

    private void skipTutorials()
    {
        Flags.setFlag("seenAbilityWheelTutorial", true);

        // Flags.flags[TutorialSequenceList.equippableItemTutorialSeenFlag] = true;
        Flags.setFlag(TutorialSequenceList.formationPopUpTutorialSeenFlag, true);
        // Flags.flags[TutorialSequenceList.addingAbilitiesTutorialSeenFlag] = true;
    }
}
