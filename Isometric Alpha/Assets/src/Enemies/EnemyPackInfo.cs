using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct EnemyAmount
{
    public int amount;
    public EnemyStats enemyStats;

    public EnemyAmount(int amount, EnemyStats enemyStats)
    {
        this.amount = amount;

        this.enemyStats = enemyStats;
    }
}

//info about a pack of enemies on the overworld, such as how many of them there are and of what type. Stored in State
public class EnemyPackInfo : MonoBehaviour, IDescribableInBlocks
{

    //[SerializeField]
    private string[] flagsToCheckForAllies;

    public string tutorialSequenceKey;

    public EnemyAmount[] enemyTypes;

    public string dropTableName;

    public string killFlagKey;

    public string dialogueUponSceneLoadKey;

    //[SerializeField]
    public ItemListID[] guaranteedDrops;

    public int numberOfDrops = 1;
    public int xpDrop = -1;

    public bool isBossMonster = false;

    public string questName;
    public int questStep;

    public QuestStepActivationScript script;

    public EnemyPackInfo(EnemyAmount[] enemyTypes, string dropTableName)
    {
        this.enemyTypes = enemyTypes;

        this.dropTableName = dropTableName;
    }

    public EnemyPackInfo(EnemyAmount[] enemyTypes, string[] flagsToCheckForAllies, string dropTableName)
    {
        this.enemyTypes = enemyTypes;

        this.flagsToCheckForAllies = flagsToCheckForAllies;

        this.dropTableName = dropTableName;
    }

    public EnemyPackInfo(EnemyAmount[] enemyTypes, string[] flagsToCheckForAllies, string dropTableName, QuestStepActivationScript script)
    {
        this.enemyTypes = enemyTypes;

        this.flagsToCheckForAllies = flagsToCheckForAllies;

        this.dropTableName = dropTableName;

        this.script = script;
    }

    public void markBossAsKilled()
    {
        if (isBossMonster)
        {
            Flags.setFlag(killFlagKey, true);
        }
    }

    public int determineEnemyCount(int index)
    {
        return enemyTypes[index].amount;
    }

    public bool hasSummonsToSpawn()
    {

        if (flagsToCheckForAllies == null || flagsToCheckForAllies is null || flagsToCheckForAllies.Length == 0)
        {
            return false;
        }

        foreach (string flag in flagsToCheckForAllies)
        {
            if (flag == null || flag is null || flag.Length == 0)
            {
                continue;
            }
            else
            {
                if (Flags.getFlag(flag))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public string getAllyGroupingKey()
    {
        for (int keyIndex = 0; keyIndex < flagsToCheckForAllies.Length; keyIndex++)
        {
            if (Flags.getFlag(flagsToCheckForAllies[keyIndex]))
            {
                return flagsToCheckForAllies[keyIndex];
            }
        }

        throw new IOException("No key to use");
    }


    //IDescribableInBlocks methods
    public string getName()
    {
        return "";
    }

    public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> blocks = new List<DescriptionPanelBuildingBlock>();

        for (int enemyIndex = 0; enemyIndex < enemyTypes.Length; enemyIndex++)
        {
            string enemyNumber = enemyTypes[enemyIndex].amount.ToString();

            blocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, enemyNumber + "   " + enemyTypes[enemyIndex].enemyStats.getName()));
        }

        return blocks;
    }
}
