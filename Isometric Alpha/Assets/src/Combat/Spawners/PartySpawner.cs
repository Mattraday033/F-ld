using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartySpawner : MonoBehaviour
{
	public Canvas combatantInfoCanvas;
	
	public static PartySpawner instance;
	
	public static PartySpawner getInstance()
	{
		return instance;
	}
	
	private void Awake()
	{
		if(instance != null)
		{
			throw new IOException("Instance of PartySpawner already exists");
		}
		
		instance = this;
	}
	
	public void spawnFormation()
	{
		for(int row = 0; row < State.formation.getGrid().Length; row++)
		{
			for(int col = 0; col < State.formation.getGrid()[row].Length; col++)
			{

				spawn(row+4, col, State.formation.getStatsAtCoords(row,col));
			}
		}
	}

    public void spawn(GridCoords coords, Stats partyMember)
    {
		spawn(coords.row, coords.col, partyMember);
	}
	
    public void spawn(int row, int col, Stats partyMember)
    {
		
		if(partyMember == null || partyMember is null)
		{
			return;
		}

		CombatGrid.combatantStatsGrid[row].setCol(col, partyMember);
		partyMember.position = new GridCoords(row,col);
		
		partyMember.instantiateCombatSprite();

		partyMember.combatSprite.transform.position = CombatGrid.getPositionAt(row, col);
		
		if(partyMember.currentHealth <= 0)
		{
			partyMember.setToDeadSprite();
		}

		Dexterity.addExitStrategy(partyMember);
    }

}
