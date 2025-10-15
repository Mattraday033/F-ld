using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradePartyMemberDecisionPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private const string upgradeTitleWord = "Upgrade";
	private const string affinityTitleWord = "Affinity";
	private const string maxLevelTitleWords = "Max Level";

	public static UnityEvent OnPartyMemberUpgraded = new UnityEvent();

	public DescriptionPanelSlot hoverDescriptionPanelSlot;

	public DescriptionPanel descriptionPanel;
	public Button upgradeButton;
	public TextMeshProUGUI buttonText;

	private void Awake()
	{
		StartCoroutine(waitToUpdateButtonText());
	}

	public void levelUpCurrentPartyMember()
	{
		// PartyMember currentPartyMember = (PartyMember)descriptionPanel.getObjectBeingDescribed();

		// if (currentPartyMember.stats.getLevel() < AllyStats.partyMemberLevelMaximum &&
		// 	AffinityManager.getTotalAffinity() >= PartyMember.getNextUpgradeCost(currentPartyMember.stats.getLevel()))
		// {
		// 	AffinityManager.addAffinity(-PartyMember.getNextUpgradeCost(currentPartyMember.stats.getLevel()));
		// 	currentPartyMember.stats.incrementLevel();
		// 	currentPartyMember.stats.currentHealth = currentPartyMember.stats.getTotalHealth();

		// 	currentPartyMember.describeSelfFull(descriptionPanel);
		// 	OverallUIManager.currentScreenManager.populateGrid(0);
		// 	OverallUIManager.currentScreenManager.updateAllStatsPanels();
		// 	updateButton();
        //     OnPartyMemberUpgraded.Invoke();
		// }
	}

	public void updateButton()
	{
		PartyMember currentPartyMember = (PartyMember)descriptionPanel.getObjectBeingDescribed();

		int currentUpgradeCost = PartyMember.getNextUpgradeCost(currentPartyMember.stats.getLevel());

		if (currentUpgradeCost > 0)
		{
			buttonText.text = upgradeTitleWord + ": " + currentUpgradeCost + " " + affinityTitleWord;

		}
		else
		{
			buttonText.text = maxLevelTitleWords;
		}

		if (currentUpgradeCost > 0 &&
			AffinityManager.getTotalAffinity() >= PartyMember.getNextUpgradeCost(currentPartyMember.stats.getLevel()))
		{
			upgradeButton.interactable = true;
		}
		else
		{
			upgradeButton.interactable = false;
		}
	}

	private IEnumerator waitToUpdateButtonText()
	{
		while (descriptionPanel.getObjectBeingDescribed() == null)
		{
			yield return null;
		}

		updateButton();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		// PartyMemberStats stats = Stats.convertIDescribableToStats(descriptionPanel.getObjectBeingDescribed()) as PartyMemberStats;

		// if (eventData.used || stats.getLevel() >= stats.getLevelMaximum())
		// {
		// 	return;
		// }

		// hoverDescriptionPanelSlot.setPrimaryDescribable(new StatsUpgradeDifference(Stats.convertIDescribableToStats(descriptionPanel.getObjectBeingDescribed()) as PartyMemberStats));
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		// PartyMemberStats stats = Stats.convertIDescribableToStats(descriptionPanel.getObjectBeingDescribed()) as PartyMemberStats;

		// if (eventData.used || stats.getLevel() >= stats.getLevelMaximum())
		// {
		// 	return;
		// }

		// hoverDescriptionPanelSlot.removePrimaryDescribable();
	}
}
