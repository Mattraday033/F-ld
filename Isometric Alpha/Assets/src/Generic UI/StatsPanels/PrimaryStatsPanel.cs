using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrimaryStatsPanel : MonoBehaviour, IStatsPanel
{
	public TextMeshProUGUI strengthStatText;
	
	public TextMeshProUGUI dexterityStatText;
	
	public TextMeshProUGUI wisdomStatText;
	
	public TextMeshProUGUI charismaStatText;

	public void updateStatsPanel()
	{
		updateStatsPanel(PartyManager.getPlayerStats());
    }

    public void updateStatsPanel(AllyStats playerStats)
    {
        strengthStatText.text = "" + playerStats.totalStrength;

        dexterityStatText.text = "" + playerStats.totalDexterity;

        wisdomStatText.text = "" + playerStats.totalWisdom;

        charismaStatText.text = "" + playerStats.totalCharisma;
    }
	
}
