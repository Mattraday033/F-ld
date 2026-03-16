using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

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

}

public struct KeyBindingSettingsWrapper
{
    public KeyCode interactKey;

    public KeyBindingSettingsWrapper(KeyCode interactKey = KeyCode.E)
    {
        this.interactKey = interactKey;
    }    

}

public class ConfigFile
{
    public AudioSettingsWrapper audioSettings;

    public KeyBindingSettingsWrapper keybindSettings;

    public void useForSettings()
    {
        AudioManager.masterVolumePlayerSetting = audioSettings.masterVolumePlayerSetting;

        AudioManager.musicVolumePlayerSetting = audioSettings.musicVolumePlayerSetting;        
        AudioManager.sfxVolumePlayerSetting = audioSettings.sfxVolumePlayerSetting;        
        AudioManager.voiceVolumePlayerSetting = audioSettings.voiceVolumePlayerSetting;        
        AudioManager.footstepVolumePlayerSetting = audioSettings.footstepVolumePlayerSetting;        

        KeyBindingList.interactKey.setCurrentKeyCode(keybindSettings.interactKey);
    }

    public static ConfigFile build()
    {
        ConfigFile config = new ConfigFile();

        config.audioSettings = new AudioSettingsWrapper(AudioManager.masterVolumePlayerSetting,
                                                        AudioManager._MusicVolumePlayerSetting,
                                                        AudioManager._SFXVolumePlayerSetting,
                                                        AudioManager._VoiceVolumePlayerSetting,
                                                        AudioManager._FootstepVolumePlayerSetting);

        config.keybindSettings = new KeyBindingSettingsWrapper(KeyBindingList.interactKey.getCurrentKeyCode());

        return config;
    }

    public static ConfigFile buildDefault()
    {
        ConfigFile config = new ConfigFile();

        config.audioSettings = new AudioSettingsWrapper(AudioManager.volumeMaximum,
                                                        AudioManager.volumeMaximum,
                                                        AudioManager.volumeMaximum,
                                                        AudioManager.volumeMaximum,
                                                        AudioManager.volumeMaximum);

        config.keybindSettings = new KeyBindingSettingsWrapper();

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
