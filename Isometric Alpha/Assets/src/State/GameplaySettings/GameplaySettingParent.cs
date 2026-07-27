using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplaySettingParent : MonoBehaviour
{
    private GameplaySetting setting;

    public TextMeshProUGUI title;
    public Transform optionParent;

    public List<GameplayOptionManager> optionManagers;

    public void createGameplaySettingsPrompts(GameplaySetting setting)
    {
        this.setting = setting;

        title.text = setting.title + ":";

        foreach(SettingOption option in setting.settingOptions)
        {
            GameplayOptionManager gameplayOption = Instantiate(Resources.Load<GameObject>(PrefabNames.gameplayOption), optionParent).GetComponent<GameplayOptionManager>();
            gameplayOption.setDisplay(option);

            optionManagers.Add(gameplayOption);
        }
    }

    public void updateGameplaySettingsPrompts(GameplaySetting setting)
    {
        if(!this.setting.Equals(setting))
        {
            return;
        }

        foreach(GameplayOptionManager optionManager in optionManagers)
        {
            optionManager.updateDisplay();
        }
    }

    private void OnEnable()
    {
        GameplaySetting.OnGameplaySettingChange.AddListener(updateGameplaySettingsPrompts);
    }

    private void OnDisable()
    {
        GameplaySetting.OnGameplaySettingChange.RemoveListener(updateGameplaySettingsPrompts);
    }
}
