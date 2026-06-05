using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class PrimaryStatIncreaseButton : BinaryPanelPopUpButton, IPointerEnterHandler, IPointerExitHandler
{
    private const string noAttachedIconMessage = "No Attached Icon";
    public static PrimaryStatIncreaseButton currentButton;
    public static PrimaryStat currentPrimaryStat;

    public static UnityEvent PrimaryStatsIncreaseButtonPressed = new UnityEvent();

    public Button button;
    public Image attachedIcon;

    private void Awake()
    {
        if (PlayerOOCStateManager.currentActivity == OOCActivity.inTutorialSequence)
        {
            button.enabled = false;
            TutorialSequence.OnEnableButtons.AddListener(enableButton);
        }
    }

	public override void spawnPopUp()
	{
        StartCoroutine(waitTwoFrameThenSpawnPopUp());
	}

    private IEnumerator waitTwoFrameThenSpawnPopUp()
    {
        CharacterScreen.getUpgradeDescriptionPanelSlot().removePrimaryDescribable();

        yield return null;
        yield return null;

        base.spawnPopUp();
    }

    public void enableButton()
    {
        button.enabled = true;
        TutorialSequence.OnEnableButtons.RemoveListener(enableButton);
    }

    public void setCurrentButton()
    {
        currentButton = this;
        currentPrimaryStat = getPrimaryPrimaryStat();
        PrimaryStatsIncreaseButtonPressed.Invoke();
    }

    public string getStatName()
    {
        if (attachedIcon != null && attachedIcon.sprite != null)
        {
            return attachedIcon.sprite.name;
        }

        return noAttachedIconMessage;
    }

    public PrimaryStat getPrimaryPrimaryStat()
    {
        if (attachedIcon != null && attachedIcon.sprite != null)
        {
            switch (attachedIcon.sprite.name)
            {
                case IconList.strengthIconName:
                    return PrimaryStat.Strength;
                case IconList.dexterityIconName:
                    return PrimaryStat.Dexterity;
                case IconList.wisdomIconName:
                    return PrimaryStat.Wisdom;
                case IconList.charismaIconName:
                    return PrimaryStat.Charisma;
            }
        }

        return PrimaryStat.None;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(CharacterScreen.getUpgradeDescriptionPanelSlot() != null)
        {
            CharacterScreen.getUpgradeDescriptionPanelSlot().setPrimaryDescribable(new AllyStatsUpgradeDifference(OverallUIManager.getCurrentPartyMember(), getPrimaryPrimaryStat())); 
        }
    }

	public void OnPointerExit(PointerEventData eventData)
	{
        if(CharacterScreen.getUpgradeDescriptionPanelSlot() != null)
        {
            CharacterScreen.getUpgradeDescriptionPanelSlot().removePrimaryDescribable();
        }
	}

}
