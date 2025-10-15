using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public interface IStatsPanel
{
	public void updateStatsPanel();
}

public class InventoryStatsPanel : MonoBehaviour, IStatsPanel
{
	public TextMeshProUGUI bonusAbilityDamageText;
	public TextMeshProUGUI totalArmorText;

    public TextMeshProUGUI currentHPText;
    public TextMeshProUGUI totalHPText;

    public TextMeshProUGUI totalGoldText;

    public void updateStatsPanel()
    {
        bonusAbilityDamageText.text = "+" + PartyManager.getPlayerStats().getBonusAbilityDamage();

        totalArmorText.text = PartyManager.getPlayerStats().getTotalArmorRating() + "";

        currentHPText.text = PartyManager.getPlayerStats().currentHealth + "";

        totalHPText.text = PartyManager.getPlayerStats().getTotalHealth() + "";

        totalGoldText.text = Purse.getCoinsInPurseForDisplay();
    }
}
