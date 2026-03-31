using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DropTableEntry
{
    public Item item;
    public float dropChance;

    public DropTableEntry(Item item, float dropChance)
    {
        this.item = item;
        this.dropChance = dropChance;
    }
}

public class DropTable
{
	public int goldMin;
	public int goldMax;

    public DropTableEntry[] entries;

	public DropTable(int goldMin, int goldMax, DropTableEntry[] entries)
	{
		this.goldMin = goldMin;
		this.goldMax = goldMax;
		
		this.entries = entries;
	}

}
