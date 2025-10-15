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
    public static PrimaryStat currentStatType;

    public static UnityEvent PrimaryStatsIncreaseButtonPressed = new UnityEvent();

    public Button button;
    public TextMeshProUGUI attachedIconText;

    private void Awake()
    {
        if (PlayerOOCStateManager.currentActivity == OOCActivity.inTutorialSequence)
        {
            button.enabled = false;
            TutorialSequence.OnEnableButtons.AddListener(enableButton);
        }
    }

    public void enableButton()
    {
        button.enabled = true;
        TutorialSequence.OnEnableButtons.RemoveListener(enableButton);
    }

    public void setCurrentButton()
    {
        currentButton = this;
        currentStatType = getPrimaryStatType();
        PrimaryStatsIncreaseButtonPressed.Invoke();
    }

    public string getStatName()
    {
        if (attachedIconText != null)
        {
            switch (attachedIconText.text)
            {
                case Strength.symbolChar:
                    return "Strength";
                case Dexterity.symbolChar:
                    return "Dexterity";
                case Wisdom.symbolChar:
                    return "Wisdom";
                case Charisma.symbolChar:
                    return "Charisma";
            }
        }

        return noAttachedIconMessage;
    }

    public string getStatSymbol()
    {
        if (attachedIconText != null)
        {
            switch (attachedIconText.text)
            {
                case Strength.symbolChar:
                    return Strength.symbolChar;
                case Dexterity.symbolChar:
                    return Dexterity.symbolChar;
                case Wisdom.symbolChar:
                    return Wisdom.symbolChar;
                case Charisma.symbolChar:
                    return Charisma.symbolChar;
            }
        }

        return noAttachedIconMessage;
    }

    public PrimaryStat getPrimaryStatType()
    {
        if (attachedIconText != null)
        {
            switch (attachedIconText.text)
            {
                case Strength.symbolChar:
                    return PrimaryStat.Strength;
                case Dexterity.symbolChar:
                    return PrimaryStat.Dexterity;
                case Wisdom.symbolChar:
                    return PrimaryStat.Wisdom;
                case Charisma.symbolChar:
                    return PrimaryStat.Charisma;
            }
        }

        return PrimaryStat.None;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CharacterScreen.upgradeDescriptionPanelSlot.setPrimaryDescribable(new AllyStatsUpgradeDifference(OverallUIManager.getCurrentPartyMember(), getPrimaryStatType())); 
    }

	public void OnPointerExit(PointerEventData eventData)
	{
		CharacterScreen.upgradeDescriptionPanelSlot.removePrimaryDescribable();
	}

}
