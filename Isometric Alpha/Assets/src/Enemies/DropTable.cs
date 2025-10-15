using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTable
{
	public string name;

	public int goldMin;
	public int goldMax;

	public Item[] items;
	
	public float[] dropChances; //all drop chances should add up to 1

	public DropTable(string name, int goldMin, int goldMax, Item[] items, float[] dropChances)
	{
		this.name = name;
		
		this.goldMin = goldMin;
		this.goldMax = goldMax;
		
		this.items = items;
		
		this.dropChances = dropChances;
	}

}
