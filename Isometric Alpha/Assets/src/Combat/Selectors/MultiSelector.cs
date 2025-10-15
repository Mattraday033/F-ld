using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class MultiSelector : ScriptableObject
{
	public string name;
	
	public int startRow;
	public int startCol;
	
	public int currentRow;
	public int currentCol;
	
	public int maxRow;
	public int minRow;
	
	private GameObject selector;
	private GameObject selectorTiles;
	private CapsuleCollider2D collider;
	
	void Start()
	{
		selector = Resources.Load<GameObject>(name);
		collider = selector.GetComponent<CapsuleCollider2D>();
	}
	
	public GameObject getSelector()
	{
		if(selector == null)
		{
			selector = Instantiate(Resources.Load<GameObject>(name), CombatGrid.fullCombatGrid[startRow][startCol],Quaternion.identity);
		}
		
		return selector;
	}
	
	public CapsuleCollider2D getCollider()
	{
		if(selector == null)
		{ //Instantiate(CombatGrid.getCombatantAtCoords(x,y).combatSprite, CombatGrid.fullCombatGrid[x][y] + CombatGrid.getCombatantAtCoords(x,y).adjustment, Quaternion.identity);
			selector = Instantiate(Resources.Load<GameObject>(name), CombatGrid.fullCombatGrid[startRow][startCol],Quaternion.identity);
			collider = selector.GetComponent<CapsuleCollider2D>();
		} else if(collider == null)
		{
			collider = selector.GetComponent<CapsuleCollider2D>();
		} 
		
		return collider;
	}
	
	public void setToStartLocation()
	{
		
		currentRow = startRow;
		currentCol = startCol;
		
		selector.transform.position = CombatGrid.fullCombatGrid[currentRow][currentCol];
	}
	
	//always returns lowercase for easier comparisons
	public string getTag()
	{
		return selector.tag.ToLower();
	}
	
	public void SetActive(bool active)
	{
		if(selector == null)
		{
			selector = Instantiate(Resources.Load<GameObject>(name), CombatGrid.fullCombatGrid[startRow][startCol],Quaternion.identity);
			collider = selector.GetComponent<CapsuleCollider2D>();
		} 
		
		selector.SetActive(active);
	}
	
	/*
	public void setToCurrentSelector()
	{
		SetActive(true);
		SelectorManager.currentSelector = this;
	}
	*/
} 
