using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SettingOption : ICloneable
{
    public GameplaySetting setting;
    public string optionTitle;
    public bool set;

    public SettingOption(string optionTitle, bool set = false)
    {
        this.optionTitle = optionTitle;
        this.set = set;
    }

	public object Clone()
    {
        return this.MemberwiseClone();
    }

    public SettingOption clone()
    {
        SettingOption option = Clone() as SettingOption;

        option.setting = setting;

        return option;
    }
}

public delegate void OnGameplaySettingChangeBehaviour();

public class GameplaySetting
{
    public readonly static UnityEvent<GameplaySetting> OnGameplaySettingChange = new UnityEvent<GameplaySetting>();

    public string title;

    public SettingOption[] settingOptions;
    private SettingOption defaultOption;

    private OnGameplaySettingChangeBehaviour behaviour;

    public GameplaySetting(string title, SettingOption[] settingOptions, OnGameplaySettingChangeBehaviour behaviour = null)
    {
        this.title = title;
        this.settingOptions = settingOptions;

        foreach(SettingOption option in this.settingOptions)
        {
            option.setting = this;

            if(option.set)
            {
                defaultOption = option.clone();
            }
        }

        this.behaviour = behaviour;

        GameplaySettingsManager.ReturnAllGameplayOptionsToDefault.AddListener(returnToDefault);
    }

    public void setOption(SettingOption settingOption, bool invoke = true, bool runOnChangeBehaviour = true)
    {
        for(int i = 0; i < settingOptions.Length; i++)
        {
            settingOptions[i].set = settingOptions[i].optionTitle.Equals(settingOption.optionTitle);
        }

        if(invoke)
        {
            OnGameplaySettingChange.Invoke(this);
        }

        if(runOnChangeBehaviour && behaviour != null)
        {
            behaviour();
        }
    }

    public void setOption(string optionTitle)
    {
        foreach(SettingOption option in settingOptions)
        {
            if(option.optionTitle.Equals(optionTitle))
            {
                setOption(option, invoke:false, runOnChangeBehaviour:false);
                return;
            }
        }
    }

    public string getCurrentOptionTitle()
    {
        foreach(SettingOption option in settingOptions)
        {
            if(option.set)
            {
                return option.optionTitle;
            }
        }

        return null;
    }

    private void returnToDefault()
    {
        setOption(defaultOption, invoke: false);
    }
}

public static class GameplaySettingsList
{

    #region On Gameplay Setting Change Events (Per Setting)

    public static readonly UnityEvent OnTransitionIndicatorVisibilitySettingChange = new UnityEvent();

    #endregion

    // Combat
    public readonly static GameplaySetting combatAnimationSpeed = new GameplaySetting("Animation Speed", 
                                                                  new SettingOption[] { new SettingOption("Slow", true), new SettingOption("Fast"), new SettingOption("Very Fast") });

    public readonly static GameplaySetting autoTarget = new GameplaySetting("Auto-Targeting", 
                                                        new SettingOption[] { new SettingOption("On", true), new SettingOption("Off")});

    public readonly static GameplaySetting healthBarsAlwaysVisible = new GameplaySetting("Health Bars Always Visible", 
                                                                     new SettingOption[] { new SettingOption("On"), new SettingOption("Off", true)});

    // Overworld
    public readonly static GameplaySetting transitionIndicatorsAlwaysVisible =  new GameplaySetting("Transition Indicators Always Visible", 
                                                                                new SettingOption[] { new SettingOption("On"), new SettingOption("Off", true)},
                                                                                () =>
                                                                                {
                                                                                    OnTransitionIndicatorVisibilitySettingChange.Invoke();
                                                                                });

    // Journal
    public readonly static GameplaySetting boldImportantQuestText = new GameplaySetting("Bold Important Quest Text", 
                                                                    new SettingOption[] { new SettingOption("On", true), new SettingOption("Off")});

    public readonly static GameplaySetting showOnlyImportantQuestText = new GameplaySetting("Show Important Quest Text Only", 
                                                                    new SettingOption[] { new SettingOption("On"), new SettingOption("Off", true)});

    // Tutorials
    public readonly static GameplaySetting tutorialsEnabled = new GameplaySetting("Enable Tutorials", 
                                                              new SettingOption[] { new SettingOption("On", true), new SettingOption("Off")});

}
