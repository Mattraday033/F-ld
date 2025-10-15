using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SideStatsPanel : MonoBehaviour, IStatsPanel
{
    public TextMeshProUGUI levelText;

    public TextMeshProUGUI currentHPText;
	public TextMeshProUGUI totalHPText;
		
	public TextMeshProUGUI XPText;
	public TextMeshProUGUI GPText;
	public TextMeshProUGUI affinityText;

	public void updateStatsPanel()
	{
        levelText.text = "Level " + PartyManager.getPlayerStats().getLevel();

        currentHPText.text = PartyManager.getPlayerStats().currentHealth + "";
		totalHPText.text = PartyManager.getPlayerStats().getTotalHealth() + "";
		
		XPText.text = PartyManager.getPlayerStats().xp + "";
		GPText.text = Purse.getCoinsInPurse() + Purse.moneySymbol;
		affinityText.text = "" + AffinityManager.getTotalAffinity();
	}
}
