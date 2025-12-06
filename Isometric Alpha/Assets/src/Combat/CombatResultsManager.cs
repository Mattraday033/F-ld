using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatResultsManager
{
	public static bool rerolled = false;

	public static List<Item> determineItemDrops(DropTable dropTable, int numberOfDrops, ItemListID[] guaranteedDrops)
    {

        if (dropTable.items.Length < dropTable.dropChances.Length)
        {
            throw new IOException("Each Item in drop table does not have a drop chance");
        }

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

        if (State.enemyPackInfo.isBossMonster)
        {
            return itemDrops;
        }

        float currentDieRoll = 0f;
        float currentTableProbability = 0f;
        int index = 0;

        for (int itemDropNumber = 1; itemDropNumber <= numberOfDrops; itemDropNumber++)
        {
            currentDieRoll = Random.Range(0.001f, 1f);
            currentTableProbability = 0f;


            foreach (Item item in dropTable.items)
            {
                if (currentDieRoll > currentTableProbability && currentDieRoll <= (currentTableProbability + dropTable.dropChances[index]))
                {
                    if (item == null)
                    {
                        if (!rerolled)
                        {
                            itemDropNumber += 2;
                            rerolled = true;
                        }

                        break;
                    }

                    itemDrops.Add(item.clone());
                    Inventory.addItem(item.clone());
                }

                currentTableProbability += dropTable.dropChances[index];
                index++;
            }
            index = 0;
        }


        rerolled = false;
        return itemDrops;
    }
	
	public static int determineGoldDrops(DropTable dropTable, int numberOfDrops)
	{
		if(numberOfDrops <= 0)
		{
			return 0;
		}
		
		int goldDropped = 0;
		
		for(int goldDropNumber = 1; goldDropNumber <= numberOfDrops; goldDropNumber++)
		{
			goldDropped += Random.Range(dropTable.goldMin, dropTable.goldMax);
		}			
		
		int finalGoldDropped = (int) (((double) goldDropped) * PartyStats.getGoldMultiplier());
		
		Purse.addCoins(finalGoldDropped);
		
		return finalGoldDropped;
	}

}
