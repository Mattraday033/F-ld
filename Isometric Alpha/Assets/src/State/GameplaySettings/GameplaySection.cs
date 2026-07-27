using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplaySection : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Transform settingParent;

    public void createGamepaySettingsPrompts(string title, List<GameplaySetting> settings)
    {
        this.title.text = title;

        foreach(GameplaySetting setting in settings)
        {
            GameplaySettingParent gameplaySettingParent = Instantiate(Resources.Load<GameObject>(PrefabNames.gameplaySettingParent), settingParent).GetComponent<GameplaySettingParent>();
            gameplaySettingParent.createGameplaySettingsPrompts(setting);

            // KeybindingButton keybindingButton = keybindingButtonGO.GetComponent<KeybindingButton>();

            // keybindingButton.populate(keyBind);
        }
    }

}
