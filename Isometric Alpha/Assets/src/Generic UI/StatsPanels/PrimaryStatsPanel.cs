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
        strengthStatText.text = "" + playerStats.getStrength();

        dexterityStatText.text = "" + playerStats.getDexterity();

        wisdomStatText.text = "" + playerStats.getWisdom();

        charismaStatText.text = "" + playerStats.getCharisma();
    }
	
}
