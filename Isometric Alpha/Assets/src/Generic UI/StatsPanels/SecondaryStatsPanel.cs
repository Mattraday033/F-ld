using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SecondaryStatsPanel : MonoBehaviour, IStatsPanel
{
	public TextMeshProUGUI[] secondaryStatsTexts;

    public void updateStatsPanel()
    {
		updateStatsPanel(PartyManager.getPlayerStats());
    }

    public void updateStatsPanel(AllyStats stats)
	{
		// string[][] allStatsForDisplay = new string[][]{ Strength.getAllSecondaryStatsForDisplay(stats.getStrength()),
		// 												Dexterity.getAllSecondaryStatsForDisplay(stats.getDexterity()),
		// 												Wisdom.getAllSecondaryStatsForDisplay(stats.getWisdom()),
		// 												Charisma.getAllSecondaryStatsForDisplay(stats.getCharisma())
		// 											  };
		// int row = 0;
		// int col = 0;
		
		// foreach(TextMeshProUGUI textBox in secondaryStatsTexts)
		// {
		// 	textBox.text = allStatsForDisplay[row][col];
			
		// 	col++;
			
		// 	if(col >= allStatsForDisplay[row].Length)
		// 	{
		// 		row++;
		// 		col = 0;
		// 	}
		// }
		
	}
	
	
	
}
