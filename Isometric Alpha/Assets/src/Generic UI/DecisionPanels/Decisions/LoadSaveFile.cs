using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


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

    public static bool midLoad = false;

    public readonly static UnityEvent OnLoad = new UnityEvent();

    public SaveBlueprint saveBlueprint;


    public LoadSaveFile()
    {
        this.saveBlueprint = null;
    }

    public LoadSaveFile(SaveBlueprint saveBlueprint)
    {
        this.saveBlueprint = saveBlueprint;
    }

    public string getMessage()
    {
        return loadLostProgressMessageStart + saveBlueprint.getName() + loadLostProgressMessageEnd;
    }

    public void execute()
    {
        midLoad = true;

        NotificationManager.purgeNotifications();
        StepCountScriptManager.reset();
        SaveHandler.createSavedGameList(true);
        OverallUIManager.setCurrentScreenType(null);
        TutorialSequenceList.initializeTutorials();
        MovementManager.initializeMovementManager();
        State.dialogueUponSceneLoadKey = null;
        ScreenManager.currentPartyMember = null;

        string currentSceneName = SceneManager.GetActiveScene().name;

        if(!currentSceneName.Equals(SceneNameList.openingMonologue) && 
            CharacterCreationPopUpWindow.goToMonologue)
        {
            SceneChange.changeSceneToOpeningMonologue();
            CharacterCreationPopUpWindow.goToMonologue = false;
            LoadingBarProgressTracker.loadSaveFile = this;
            return;
        } else if (!currentSceneName.Equals(SceneNameList.loadingScreen))
        {
            LoadingBarProgressTracker.loadSaveFile = this;
            SceneChange.changeSceneToLoadingScreen();
        }
        else
        {
            ChoiceManager.resetChoices();
            QuestList.buildQuestListFromScratch();

            if (saveBlueprint == null)
            {
                saveBlueprint = SaveHandler.getCleanSlateSave();
            }
                
            Flags.exitNewGameMode();

            if (CombatStateManager.inCombat)
            {
                CombatStateManager.resetCombat();
                CombatStateManager.inCombat = false;
                TransitionManager.ChangeAreaMusic.Invoke(saveBlueprint.currentLocation);
            } else
            {
                TransitionManager.ChangeAreaMusic.Invoke(saveBlueprint.currentLocation);
            }
            
            Flags.resetAllFlags();
            Flags.overwriteFlags(saveBlueprint.currentFlags);
            // TutorialFlags.checkForTutorialFlagsInNormalFlags();

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
                State.playerPortraitName = saveBlueprint.playerPortraitName;
                State.playerSpriteName = saveBlueprint.playerSpriteName;
            }
            else
            {
                Flags.setStatTutorialFlag();
                Flags.exitNewGameMode();
            }

            State.inventory = SaveBlueprint.extractInventoryItemsFromJson(saveBlueprint.currentInventory);
            State.junkPocket = SaveBlueprint.extractInventoryItemsFromJson(saveBlueprint.currentJunk);

            ChoiceManager.choices = saveBlueprint.extractChoicesFromJson();
            DeathFlagManager.resetAllDeadNpcs(saveBlueprint.extractListOfStringsFromJson(saveBlueprint.currentDeathFlags));
            MetFlagManager.resetAllMetNpcs(saveBlueprint.extractListOfStringsFromJson(saveBlueprint.currentMetFlags));
            GateAndChestManager.resetGatesAndChests(FlagWrapper.convertFlagWrapperListToDictionary(saveBlueprint.currentChestAndGateFlags));
            TrapAndButtonStateManager.resetTrapKeys(saveBlueprint.currentActivatedTrapsAndButtons);

            IntimidateManager.setIntimidatesRemaining(saveBlueprint.intimidatesRemaining);
            CunningManager.setCunningsRemaining(saveBlueprint.cunningsRemaining);

            Dictionary<string, Dictionary<string, Item>> newShopkeeperInventories = SaveBlueprint.extractShopkeeperInventoriesFromJson(saveBlueprint.currentShopkeeperInventories);
            Dictionary<string, Dictionary<string, Item>> newBuyBackInventories = SaveBlueprint.extractShopkeeperInventoriesFromJson(saveBlueprint.currentBuyBackInventories);

            PuzzleFlags.currentPuzzleIndex = saveBlueprint.currentPuzzleIndex;

            ShopkeeperInventoryList.setShopkeeperInventoryList(newShopkeeperInventories, newBuyBackInventories);

            QuestList.resetAndOverwriteQuestDictionary(saveBlueprint.currentQuestList);
            State.allKnownMapData = saveBlueprint.extractAllKnownMapDataFromJson();
            saveBlueprint.extractAllAreaHostilitiesFromJson();

            SecretDoorFlags.setFromSaveData(saveBlueprint.secretDoors);

            Purse.setCoinsInPurse(saveBlueprint.gold);

            MonsterDefeatKeysList.extractAllMonsterDefeatKeys(saveBlueprint);

            NewAbilityManager.resetNewAbilityManager(saveBlueprint.newAbilityWrappers);  
            NewPartyMemberManager.resetNewPartyMemberManager(saveBlueprint.newPartyMemberNames);    
            
            CombatStateManager.inCombat = false;

            State.currentSkillType = SkillManager.getHighestSkillType(PartyManager.getPlayerStats());

            SpeechLog.cleanSpeechLog();

            TestScript.addTestVariables();

            EscapeStack.escapeAll();

            FadeToBlackManager.setToMaxOpacity();

            if (saveBlueprint.overworldSpriteSortingLayer != null && saveBlueprint.overworldSpriteSortingLayer.Length > 0)
            {
                SceneTransitionPosition.sortingLayer = saveBlueprint.overworldSpriteSortingLayer;
            }

            skipTutorials();

            SpawnInfoManager.lastSaveBlueprint = saveBlueprint;

            CombatStateManager.whoseTurn = WhoseTurn.Start;

			PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);

            OnLoad.Invoke();
            
            SceneChange.changeSceneToOverworld();

            midLoad = false;
        }
    }

    public string getPlayerSpriteNameInSave()
    {
        if(saveBlueprint == null)
        {
            return State.playerSpriteName;
        }

        return saveBlueprint.playerSpriteName;
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
        Flags.setFlag(TutorialSequenceList.formationTutorialSeenFlag, true);
        // Flags.flags[TutorialSequenceList.addingAbilitiesTutorialSeenFlag] = true;
    }
}
