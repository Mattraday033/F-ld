using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatAdjustmentManager : MonoBehaviour
{
	private int str = 0;
	private int dex = 1;
	private int wis = 2;
	private int cha = 3;
	
	public bool newGame;
	private bool needsUpdating = true;
	
	public TextMeshProUGUI[] statText;
	
	public TextMeshProUGUI pointsToSpendText;
	
	public Button[] upButtons;
	public Button[] downButtons;
	
	public int[] currentStats = new int[4];
	
	private int[] originalStats = new int[4];
	
	public int pointsToSpend;
	
	void Start()
	{
		
		if(newGame) 
		{
			for(int i = 0; i < 4; i++) 
			{
				currentStats[i] = 1;
			}
			
			for(int i = 0; i < 4; i++) 
			{
				originalStats[i] = 1;
			}
			
			pointsToSpend = 1;
			
		} else {
			currentStats[str] = PartyManager.getPlayerStats().getStrength();
			currentStats[dex] = PartyManager.getPlayerStats().getDexterity();
			currentStats[wis] = PartyManager.getPlayerStats().getWisdom();
			currentStats[cha] = PartyManager.getPlayerStats().getCharisma();
			
			originalStats[str] = PartyManager.getPlayerStats().getStrength();
			originalStats[dex] = PartyManager.getPlayerStats().getDexterity();
			originalStats[wis] = PartyManager.getPlayerStats().getWisdom();
			originalStats[cha] = PartyManager.getPlayerStats().getCharisma();
			
			pointsToSpend = 1;
		}
		
	}

	private void updateScreen()
	{
		for(int i = 0; i < 4; i++)
		{
			statText[i].text = "" + currentStats[i];
		
			downButtons[i].interactable = !(currentStats[i] == originalStats[i]);
		}

		activateUpButtons(!(pointsToSpend == 0));
		
		pointsToSpendText.text = "" + pointsToSpend;
	} 
	
	public void activateUpButtons(bool setButtonTo)
	{
		foreach(Button upButton in upButtons)
		{
			upButton.interactable = setButtonTo;
		}
	}
	
	public void addPoint(int index)
	{
		pointsToSpend--;
		currentStats[index]++;

		updateScreen();
	}
	
	public void removePoint(int index)
	{
		pointsToSpend++;
		currentStats[index]--;

		updateScreen();
	}
}
