using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class GameplayOptionManager : MonoBehaviour
{
    public readonly static UnityEvent ManualGameplayOptionUpdate = new UnityEvent();

    private static Sprite boxOutlineEmpty;
    private static Sprite boxOutlineFull;

    public TextMeshProUGUI title;
    public Image boxOutline;
    public Image boxInterior;

    public SettingOption option;

    public void setDisplay(SettingOption option)
    {
        title.text = option.optionTitle + ":";

        if(option.set)
        {
            boxOutline.sprite = boxOutlineFull;
        } else
        {
            boxOutline.sprite = boxOutlineEmpty;
        }

        boxInterior.enabled = option.set;
        
        this.option = option;
    }

    public void updateDisplay()
    {
        setDisplay(option);
    }

    public void flipOption()
    {
        option.setting.setOption(option);
    }

    private void OnEnable()
    {
        ManualGameplayOptionUpdate.AddListener(updateDisplay);
    }

    private void OnDisable()
    {
        ManualGameplayOptionUpdate.RemoveListener(updateDisplay);
    }

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        boxOutlineEmpty = Helpers.loadSpriteFromResources(IconList.settingBoxEmpty);
        boxOutlineFull = Helpers.loadSpriteFromResources(IconList.settingBoxFull);
    }

}
