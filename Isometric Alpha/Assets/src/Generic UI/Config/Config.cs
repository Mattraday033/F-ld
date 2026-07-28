using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public struct TutorialFlagWrapper
{

    public bool equippableItemTutorialSeen;
    public bool formationTutorialSeen;

    public bool combatTutorialSeen;
    public bool traitTutorialSeen;
    public bool mandatoryTargetTutorialSeen;

    public bool skipThatchShackTutorialsFlag;
	public bool intimidateTutorialSeen;
	public bool cunningTutorialSeen;
	public bool secondCunningTutorialSeen;
	public bool thirdCunningTutorialSeen;
	public bool observationTutorialSeen;
	public bool secondObservationTutorialSeen;
	public bool leadershipTutorialSeen;
	public bool secondLeadershipTutorialSeen;
	public bool interactableObjectTutorialSeen;
    public bool hiddenObjectsTutorialSeen;
    public bool firstHostilityTutorialSeen;
    public bool secondHostilityTutorialSeen;

    public bool movableObjectTutorialSeen;

    public bool questCounterTutorialSeen;

    public bool playerLevelUpTutorialSeen;
    public bool addingAbilitiesTutorialSeen;
    public bool companionSpecificAbilitiesTutorialSeen;

    public bool exuberanceCostTutorialSeen;
    public bool traitCostTutorialSeen;

    public bool winConUITutorialSeen;

    public static TutorialFlagWrapper buildFromCurrentSettings()
    {
        return new TutorialFlagWrapper()
        {
            equippableItemTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.equippableItemTutorialSeenFlag, ignoreTutorialDisabledSetting: true),
            formationTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.formationTutorialSeenFlag, ignoreTutorialDisabledSetting: true),

            combatTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.combatTutorialSeenFlag, ignoreTutorialDisabledSetting: true),
            traitTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.traitTutorialSeenFlag, ignoreTutorialDisabledSetting: true),
            mandatoryTargetTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.mandatoryTargetTutorialSeenFlag, ignoreTutorialDisabledSetting: true),

            skipThatchShackTutorialsFlag = TutorialFlags.getFlag(TutorialSequenceList.skipThatchShackTutorialsFlag, ignoreTutorialDisabledSetting: true),
            intimidateTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.intimidateTutorialSeenFlag, ignoreTutorialDisabledSetting: true),
            cunningTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.cunningTutorialSeenFlag, ignoreTutorialDisabledSetting: true),
            secondCunningTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.secondCunningTutorialSeenFlag, ignoreTutorialDisabledSetting: true),
            thirdCunningTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.thirdCunningTutorialSeenFlag, ignoreTutorialDisabledSetting: true),
            observationTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.observationTutorialSeenFlag, ignoreTutorialDisabledSetting: true),
            secondObservationTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.secondObservationTutorialSeenFlag, ignoreTutorialDisabledSetting: true),
            leadershipTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.leadershipTutorialSeenFlag, ignoreTutorialDisabledSetting: true),
            secondLeadershipTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.secondLeadershipTutorialSeenFlag, ignoreTutorialDisabledSetting: true),
            interactableObjectTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.interactableObjectTutorialSeenFlag, ignoreTutorialDisabledSetting: true),
            hiddenObjectsTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.hiddenObjectsTutorialSeenFlag, ignoreTutorialDisabledSetting: true),
            firstHostilityTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.firstHostilityTutorialSeenFlag, ignoreTutorialDisabledSetting: true),
            secondHostilityTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.secondHostilityTutorialSeenFlag, ignoreTutorialDisabledSetting: true),

            movableObjectTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.movableObjectTutorialSeenFlag, ignoreTutorialDisabledSetting: true),

            questCounterTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.questCounterTutorialSeenFlag, ignoreTutorialDisabledSetting: true),

            playerLevelUpTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.playerLevelUpTutorialSeenFlag, ignoreTutorialDisabledSetting: true),
            addingAbilitiesTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.addingAbilitiesTutorialSeenFlag, ignoreTutorialDisabledSetting: true),
            companionSpecificAbilitiesTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.companionSpecificAbilitiesTutorialSeenFlag, ignoreTutorialDisabledSetting: true),

            exuberanceCostTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.exuberanceCostTutorialSeenFlag, ignoreTutorialDisabledSetting: true),
            traitCostTutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.traitCostTutorialSeenFlag, ignoreTutorialDisabledSetting: true),

            winConUITutorialSeen = TutorialFlags.getFlag(TutorialSequenceList.winConUITutorialSeenFlag, ignoreTutorialDisabledSetting: true),
        };
    }

    public void useForSettings()
    {
        Dictionary<string, bool> newTutorialFlags = new Dictionary<string, bool>()
        {
            [TutorialSequenceList.equippableItemTutorialSeenFlag] = equippableItemTutorialSeen,
            [TutorialSequenceList.formationTutorialSeenFlag] = formationTutorialSeen,

            [TutorialSequenceList.combatTutorialSeenFlag] = combatTutorialSeen,
            [TutorialSequenceList.traitTutorialSeenFlag] = traitTutorialSeen,
            [TutorialSequenceList.mandatoryTargetTutorialSeenFlag] = mandatoryTargetTutorialSeen,
            
            [TutorialSequenceList.skipThatchShackTutorialsFlag] = skipThatchShackTutorialsFlag,
            [TutorialSequenceList.intimidateTutorialSeenFlag] = intimidateTutorialSeen,
            [TutorialSequenceList.cunningTutorialSeenFlag] = cunningTutorialSeen,
            [TutorialSequenceList.secondCunningTutorialSeenFlag] = secondCunningTutorialSeen,
            [TutorialSequenceList.thirdCunningTutorialSeenFlag] = thirdCunningTutorialSeen,
            [TutorialSequenceList.observationTutorialSeenFlag] = observationTutorialSeen,
            [TutorialSequenceList.secondObservationTutorialSeenFlag] = secondObservationTutorialSeen,
            [TutorialSequenceList.leadershipTutorialSeenFlag] = leadershipTutorialSeen,
            [TutorialSequenceList.secondLeadershipTutorialSeenFlag] = secondLeadershipTutorialSeen,
            [TutorialSequenceList.interactableObjectTutorialSeenFlag] = interactableObjectTutorialSeen,
            [TutorialSequenceList.hiddenObjectsTutorialSeenFlag] = hiddenObjectsTutorialSeen,

            [TutorialSequenceList.firstHostilityTutorialSeenFlag] = firstHostilityTutorialSeen,
            [TutorialSequenceList.secondHostilityTutorialSeenFlag] = secondHostilityTutorialSeen,

            [TutorialSequenceList.movableObjectTutorialSeenFlag] = movableObjectTutorialSeen,

            [TutorialSequenceList.questCounterTutorialSeenFlag] = questCounterTutorialSeen,

            [TutorialSequenceList.playerLevelUpTutorialSeenFlag] = playerLevelUpTutorialSeen,
            [TutorialSequenceList.addingAbilitiesTutorialSeenFlag] = addingAbilitiesTutorialSeen,
            [TutorialSequenceList.companionSpecificAbilitiesTutorialSeenFlag] = companionSpecificAbilitiesTutorialSeen,

            [TutorialSequenceList.exuberanceCostTutorialSeenFlag] = exuberanceCostTutorialSeen,
            [TutorialSequenceList.traitCostTutorialSeenFlag] = traitCostTutorialSeen,

            [TutorialSequenceList.winConUITutorialSeenFlag] = winConUITutorialSeen
        };

        TutorialFlags.overwriteFlags(newTutorialFlags);

    }

}

public struct AudioSettingsWrapper
{
    public float masterVolumePlayerSetting;

    public float musicVolumePlayerSetting;
    public float sfxVolumePlayerSetting;
    public float voiceVolumePlayerSetting;
    public float footstepVolumePlayerSetting;    

    public AudioSettingsWrapper(float masterVolumePlayerSetting, float musicVolumePlayerSetting, float sfxVolumePlayerSetting, float voiceVolumePlayerSetting, float footstepVolumePlayerSetting)
    {
        this.masterVolumePlayerSetting = masterVolumePlayerSetting;
        this.musicVolumePlayerSetting = musicVolumePlayerSetting;
        this.sfxVolumePlayerSetting = sfxVolumePlayerSetting;
        this.voiceVolumePlayerSetting = voiceVolumePlayerSetting;        
        this.footstepVolumePlayerSetting = footstepVolumePlayerSetting;    
    }    

    public void useForSettings()
    {
        AudioManager.masterVolumePlayerSetting = masterVolumePlayerSetting;

        AudioManager.musicVolumePlayerSetting = musicVolumePlayerSetting;     
        AudioManager.sfxVolumePlayerSetting = sfxVolumePlayerSetting;        
        AudioManager.voiceVolumePlayerSetting = voiceVolumePlayerSetting;        
        AudioManager.footstepVolumePlayerSetting = footstepVolumePlayerSetting;   
    }

}

public struct GameplaySettingsWrapper
{
    public string combatAnimationSpeed;
    public string autoTarget;
    public string healthBarsAlwaysVisible;

    public string transitionIndicatorsAlwaysVisible;

    public string boldImportantQuestText;
    public string showOnlyImportantQuestText;

    public string tutorialsEnabled;

    public static GameplaySettingsWrapper buildFromCurrentSettings()
    {
        return new GameplaySettingsWrapper()
        {
            combatAnimationSpeed = GameplaySettingsList.combatAnimationSpeed.getCurrentOptionTitle(),
            autoTarget = GameplaySettingsList.autoTarget.getCurrentOptionTitle(),
            healthBarsAlwaysVisible = GameplaySettingsList.healthBarsAlwaysVisible.getCurrentOptionTitle(),

            transitionIndicatorsAlwaysVisible = GameplaySettingsList.transitionIndicatorsAlwaysVisible.getCurrentOptionTitle(),

            boldImportantQuestText = GameplaySettingsList.boldImportantQuestText.getCurrentOptionTitle(),
            showOnlyImportantQuestText = GameplaySettingsList.showOnlyImportantQuestText.getCurrentOptionTitle(),

            tutorialsEnabled = GameplaySettingsList.tutorialsEnabled.getCurrentOptionTitle()
        };
    }

    public void useForSettings()
    {
        setOptionIfPresent(GameplaySettingsList.combatAnimationSpeed, combatAnimationSpeed);
        setOptionIfPresent(GameplaySettingsList.autoTarget, autoTarget);
        setOptionIfPresent(GameplaySettingsList.healthBarsAlwaysVisible, healthBarsAlwaysVisible);

        setOptionIfPresent(GameplaySettingsList.transitionIndicatorsAlwaysVisible, transitionIndicatorsAlwaysVisible);

        setOptionIfPresent(GameplaySettingsList.boldImportantQuestText, boldImportantQuestText);
        setOptionIfPresent(GameplaySettingsList.showOnlyImportantQuestText, showOnlyImportantQuestText);

        setOptionIfPresent(GameplaySettingsList.tutorialsEnabled, tutorialsEnabled);
    }

    private static void setOptionIfPresent(GameplaySetting setting, string optionTitle)
    {
        if(!String.IsNullOrEmpty(optionTitle))
        {
            setting.setOption(optionTitle);
        }
    }

}

public struct KeyBindingSettingsWrapper
{
    public KeyCode? moveNorthKey;
    public KeyCode? moveWestKey;
    public KeyCode? moveSouthKey;
    public KeyCode? moveEastKey;

    public KeyCode? interactKey;
    public KeyCode? backOutKey;
    public KeyCode? hideTerrainKey;
    public KeyCode? revealKey;
    public KeyCode? removePlacedCompanionMovableObjectKey;
    public KeyCode? quicksaveKey;
    public KeyCode? transcriptKey;
    public KeyCode? showHideKeyBindingsListKey;

    public KeyCode? skillKey;
    public KeyCode? cycleSkillAscendingKey;
    public KeyCode? cycleSkillDescendingKey;

    public KeyCode? acceptKey;
    public KeyCode? acceptInputKey;
    public KeyCode? lastScreenKey;
    public KeyCode? characterScreenKey;
    public KeyCode? inventoryScreenKey;
    public KeyCode? partyScreenKey;
    public KeyCode? journalScreenKey;
    public KeyCode? loadScreenKey;
    public KeyCode? settingsScreenKey;
    public KeyCode? moveLeftKey;
    public KeyCode? moveRightKey;
    public KeyCode? inspectKey;
    public KeyCode? mapKey;
    public KeyCode? worldMapKey;
    public KeyCode? showFormulaKey;
    public KeyCode? maxAmountKey;
    public KeyCode? multiplyByTenAmountKey;

    public KeyCode? combatSelectKey;
    public KeyCode? combatDeselectKey;
    public KeyCode? resolveTurnKey;
    public KeyCode? jumpMoveKey;
    public KeyCode? combatSettingsScreenKey;

    public KeyCode? moveCounterClockwiseKey;
    public KeyCode? moveClockwiseKey;

    public static KeyBindingSettingsWrapper buildFromCurrentSettings()
    {
        return new KeyBindingSettingsWrapper()
        {
            moveNorthKey = KeyBindingList.moveNorthKey.getCurrentKeyCode(),
            moveWestKey = KeyBindingList.moveWestKey.getCurrentKeyCode(),
            moveSouthKey = KeyBindingList.moveSouthKey.getCurrentKeyCode(),
            moveEastKey = KeyBindingList.moveEastKey.getCurrentKeyCode(),

            interactKey = KeyBindingList.interactKey.getCurrentKeyCode(),
            backOutKey = KeyBindingList.backOutKey.getCurrentKeyCode(),
            hideTerrainKey = KeyBindingList.hideTerrainKey.getCurrentKeyCode(),
            revealKey = KeyBindingList.revealKey.getCurrentKeyCode(),
            removePlacedCompanionMovableObjectKey = KeyBindingList.removePlacedCompanionMovableObjectKey.getCurrentKeyCode(),
            quicksaveKey = KeyBindingList.quicksaveKey.getCurrentKeyCode(),
            transcriptKey = KeyBindingList.transcriptKey.getCurrentKeyCode(),
            showHideKeyBindingsListKey = KeyBindingList.showHideKeyBindingsListKey.getCurrentKeyCode(),

            skillKey = KeyBindingList.skillKey.getCurrentKeyCode(),
            cycleSkillAscendingKey = KeyBindingList.cycleSkillAscendingKey.getCurrentKeyCode(),
            cycleSkillDescendingKey = KeyBindingList.cycleSkillDescendingKey.getCurrentKeyCode(),

            acceptKey = KeyBindingList.acceptKey.getCurrentKeyCode(),
            acceptInputKey = KeyBindingList.acceptInputKey.getCurrentKeyCode(),
            lastScreenKey = KeyBindingList.lastScreenKey.getCurrentKeyCode(),
            characterScreenKey = KeyBindingList.characterScreenKey.getCurrentKeyCode(),
            inventoryScreenKey = KeyBindingList.inventoryScreenKey.getCurrentKeyCode(),
            partyScreenKey = KeyBindingList.partyScreenKey.getCurrentKeyCode(),
            journalScreenKey = KeyBindingList.journalScreenKey.getCurrentKeyCode(),
            loadScreenKey = KeyBindingList.loadScreenKey.getCurrentKeyCode(),
            settingsScreenKey = KeyBindingList.settingsScreenKey.getCurrentKeyCode(),
            moveLeftKey = KeyBindingList.moveLeftKey.getCurrentKeyCode(),
            moveRightKey = KeyBindingList.moveRightKey.getCurrentKeyCode(),
            inspectKey = KeyBindingList.inspectKey.getCurrentKeyCode(),
            mapKey = KeyBindingList.mapKey.getCurrentKeyCode(),
            worldMapKey = KeyBindingList.worldMapKey.getCurrentKeyCode(),
            showFormulaKey = KeyBindingList.showFormulaKey.getCurrentKeyCode(),
            maxAmountKey = KeyBindingList.maxAmountKey.getCurrentKeyCode(),
            multiplyByTenAmountKey = KeyBindingList.multiplyByTenAmountKey.getCurrentKeyCode(),

            combatSelectKey = KeyBindingList.combatSelectKey.getCurrentKeyCode(),
            combatDeselectKey = KeyBindingList.combatDeselectKey.getCurrentKeyCode(),
            resolveTurnKey = KeyBindingList.resolveTurnKey.getCurrentKeyCode(),
            jumpMoveKey = KeyBindingList.jumpMoveKey.getCurrentKeyCode(),
            combatSettingsScreenKey = KeyBindingList.combatSettingsScreenKey.getCurrentKeyCode(),

            moveCounterClockwiseKey = KeyBindingList.moveCounterClockwiseKey.getCurrentKeyCode(),
            moveClockwiseKey = KeyBindingList.moveClockwiseKey.getCurrentKeyCode()
        };
    }

    public void useForSettings()
    {
        setKeyCodeIfPresent(KeyBindingList.moveNorthKey, moveNorthKey);
        setKeyCodeIfPresent(KeyBindingList.moveWestKey, moveWestKey);
        setKeyCodeIfPresent(KeyBindingList.moveSouthKey, moveSouthKey);
        setKeyCodeIfPresent(KeyBindingList.moveEastKey, moveEastKey);
        setKeyCodeIfPresent(KeyBindingList.interactKey, interactKey);
        setKeyCodeIfPresent(KeyBindingList.backOutKey, backOutKey);
        setKeyCodeIfPresent(KeyBindingList.hideTerrainKey, hideTerrainKey);
        setKeyCodeIfPresent(KeyBindingList.revealKey, revealKey);
        setKeyCodeIfPresent(KeyBindingList.removePlacedCompanionMovableObjectKey, removePlacedCompanionMovableObjectKey);
        setKeyCodeIfPresent(KeyBindingList.quicksaveKey, quicksaveKey);
        setKeyCodeIfPresent(KeyBindingList.transcriptKey, transcriptKey);
        setKeyCodeIfPresent(KeyBindingList.showHideKeyBindingsListKey, showHideKeyBindingsListKey);
        setKeyCodeIfPresent(KeyBindingList.skillKey, skillKey);
        setKeyCodeIfPresent(KeyBindingList.cycleSkillAscendingKey, cycleSkillAscendingKey);
        setKeyCodeIfPresent(KeyBindingList.cycleSkillDescendingKey, cycleSkillDescendingKey);
        setKeyCodeIfPresent(KeyBindingList.acceptKey, acceptKey);
        setKeyCodeIfPresent(KeyBindingList.acceptInputKey, acceptInputKey);
        setKeyCodeIfPresent(KeyBindingList.lastScreenKey, lastScreenKey);
        setKeyCodeIfPresent(KeyBindingList.characterScreenKey, characterScreenKey);
        setKeyCodeIfPresent(KeyBindingList.inventoryScreenKey, inventoryScreenKey);
        setKeyCodeIfPresent(KeyBindingList.partyScreenKey, partyScreenKey);
        setKeyCodeIfPresent(KeyBindingList.journalScreenKey, journalScreenKey);
        setKeyCodeIfPresent(KeyBindingList.loadScreenKey, loadScreenKey);
        setKeyCodeIfPresent(KeyBindingList.settingsScreenKey, settingsScreenKey);
        setKeyCodeIfPresent(KeyBindingList.moveLeftKey, moveLeftKey);
        setKeyCodeIfPresent(KeyBindingList.moveRightKey, moveRightKey);
        setKeyCodeIfPresent(KeyBindingList.inspectKey, inspectKey);
        setKeyCodeIfPresent(KeyBindingList.mapKey, mapKey);
        setKeyCodeIfPresent(KeyBindingList.worldMapKey, worldMapKey);
        setKeyCodeIfPresent(KeyBindingList.showFormulaKey, showFormulaKey);
        setKeyCodeIfPresent(KeyBindingList.maxAmountKey, maxAmountKey);
        setKeyCodeIfPresent(KeyBindingList.multiplyByTenAmountKey, multiplyByTenAmountKey);
        setKeyCodeIfPresent(KeyBindingList.combatSelectKey, combatSelectKey);
        setKeyCodeIfPresent(KeyBindingList.combatDeselectKey, combatDeselectKey);
        setKeyCodeIfPresent(KeyBindingList.resolveTurnKey, resolveTurnKey);
        setKeyCodeIfPresent(KeyBindingList.jumpMoveKey, jumpMoveKey);
        setKeyCodeIfPresent(KeyBindingList.combatSettingsScreenKey, combatSettingsScreenKey);
        setKeyCodeIfPresent(KeyBindingList.moveCounterClockwiseKey, moveCounterClockwiseKey);
        setKeyCodeIfPresent(KeyBindingList.moveClockwiseKey, moveClockwiseKey);
    }

    private static void setKeyCodeIfPresent(KeyBind keyBind, KeyCode? keyCode)
    {
        if(keyCode.HasValue)
        {
            keyBind.setCurrentKeyCode(keyCode.Value);
        }
    }

}

public class ConfigFile
{
    public TutorialFlagWrapper tutorialFlags;
    public AudioSettingsWrapper audioSettings;

    public KeyBindingSettingsWrapper keybindSettings;

    public GameplaySettingsWrapper gameplaySettings;

    public void useForSettings()
    {
        tutorialFlags.useForSettings();

        audioSettings.useForSettings();

        keybindSettings.useForSettings();

        gameplaySettings.useForSettings();
    }

    public static ConfigFile build()
    {
        ConfigFile config = new ConfigFile();

        config.tutorialFlags = TutorialFlagWrapper.buildFromCurrentSettings();

        config.audioSettings = new AudioSettingsWrapper(AudioManager.masterVolumePlayerSetting,
                                                        AudioManager._MusicVolumePlayerSetting,
                                                        AudioManager._SFXVolumePlayerSetting,
                                                        AudioManager._VoiceVolumePlayerSetting,
                                                        AudioManager._FootstepVolumePlayerSetting);

        config.keybindSettings = KeyBindingSettingsWrapper.buildFromCurrentSettings();

        config.gameplaySettings = GameplaySettingsWrapper.buildFromCurrentSettings();

        return config;
    }

    public static ConfigFile buildDefault()
    {
        ConfigFile config = new ConfigFile();

        config.tutorialFlags = TutorialFlagWrapper.buildFromCurrentSettings();

        config.audioSettings = new AudioSettingsWrapper(AudioManager.volumeMaximum,
                                                        AudioManager.volumeMaximum,
                                                        AudioManager.volumeMaximum,
                                                        AudioManager.volumeMaximum,
                                                        AudioManager.volumeMaximum);

        config.keybindSettings = KeyBindingSettingsWrapper.buildFromCurrentSettings();

        config.gameplaySettings = GameplaySettingsWrapper.buildFromCurrentSettings();


        string json = JsonConvert.SerializeObject(config);

        if(!Directory.Exists(PrefabNames.configFolder))
        {
            Directory.CreateDirectory(PrefabNames.configFolder);
        }

		File.WriteAllText(PrefabNames.configFile, json);

        return config;
    }

}

public static class Config
{
    private static ConfigFile config = null;

    public static void initializeSettingsFromConfigFile()
    {
        if(config == null)
        {
            readConfig();
        } else
        {
            return;
        }
    }

	public static void readConfig() 
	{
        if(!Directory.Exists(PrefabNames.configFolder))
        {
            Directory.CreateDirectory(PrefabNames.configFolder);
        }

		string[] filesInConfigFolder = Directory.GetFiles(PrefabNames.configFolder);

        foreach(string filePath in filesInConfigFolder)
        {
            if (!String.Equals(filePath.Split(".")[1], Constants.jsonFileExtensionWithoutPeriod, StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}

            config = Json.getObjectFromJSON<ConfigFile>(filePath);            

            if(config != null)
            {
                break;
            }
        }

        if(config == null)
        {
            config = ConfigFile.buildDefault();
        } 

        config.useForSettings();
	}

    public static void writeConfig()
    {
        if(File.Exists(PrefabNames.configFile))
        {
            File.Delete(PrefabNames.configFile);
        }

        config = ConfigFile.build();

        Json.writeObjectToJSON(PrefabNames.configFile, config);
    }

}
