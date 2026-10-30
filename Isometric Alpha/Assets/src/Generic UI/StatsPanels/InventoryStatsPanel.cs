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
        bonusAbilityDamageText.text = "+" + PartyManager.getPlayerStats().bonusAbilityDamage;

        totalArmorText.text = PartyManager.getPlayerStats().totalArmorRating + "";

        currentHPText.text = PartyManager.getPlayerStats().currentHealth + "";

        totalHPText.text = PartyManager.getPlayerStats().totalHealth + "";

        totalGoldText.text = Purse.getCoinsInPurseForDisplay();
    }
}
