using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatResultsManager
{
	public static List<Item> determineItemDrops(DropTable dropTable, ItemListID[] guaranteedDrops)
    {
        List<Item> itemDrops = new List<Item>();

        if (guaranteedDrops != null)
        {
            foreach (ItemListID guaranteedDropID in guaranteedDrops)
            {
                Item item = ItemList.getItem(guaranteedDropID);

                itemDrops.Add(item.clone());
                Inventory.addItem(item.clone());
            }
        }

        if (State.enemyPackInfo.isBossMonster())
        {
            return itemDrops;
        }

        foreach (DropTableEntry entry in dropTable.entries)
        {
            float currentDieRoll = Random.Range(0.001f, 1f);

            if (currentDieRoll <= entry.dropChance)
            {
                itemDrops.Add(entry.item.clone());
                Inventory.addItem(entry.item.clone());
            }
        }

        return itemDrops;
    }
	
	public static int determineGoldDrops(DropTable dropTable)
	{
		int goldDropped = Random.Range(dropTable.goldMin, dropTable.goldMax);
		
		int finalGoldDropped = (int) (((double) goldDropped) * PartyStats.getGoldMultiplier());
		
		Purse.addCoins(finalGoldDropped);
		
		return finalGoldDropped;
	}

	public static int determineCunningChargesReplenished()
	{
        if(CunningManager.getCunningsRemaining() < PartyStats.getMaxCunningCount())
        {
            CunningManager.addCunningsRemaining(1);
            return 1;
        } else
        {
            return 0;
        }
	}

	public static int determineIntimidateChargesReplenished()
	{
        if(IntimidateManager.getIntimidatesRemaining() < PartyStats.getMaxIntimidateCount())
        {
            IntimidateManager.incrementIntimidatesRemaining();
            return 1;
        } else
        {
            return 0;
        }
	}

}
