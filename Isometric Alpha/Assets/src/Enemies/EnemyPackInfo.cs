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

    protected string[] flagsToCheckForAllies;

    public string tutorialSequenceKey;

    public EnemyAmount[] enemyTypes;

    public string dropTableName;

    public string dialogueUponSceneLoadKey;

    public ItemListID[] guaranteedDrops;

    public int numberOfDrops = 1; //number of rolls on their drop table


    public EnemyPackInfo(EnemyAmount[] enemyTypes, string dropTableName)
    {
        this.enemyTypes = enemyTypes;

        this.dropTableName = dropTableName;
    }

    public EnemyPackInfo(EnemyAmount[] enemyTypes, string dropTableName, string tutorialSequenceKey)
    {
        this.enemyTypes = enemyTypes;

        this.dropTableName = dropTableName;

        this.tutorialSequenceKey = tutorialSequenceKey;
    }

    public EnemyPackInfo(EnemyAmount[] enemyTypes, string dropTableName, ItemListID[] guaranteedDrops)
    {
        this.enemyTypes = enemyTypes;

        this.dropTableName = dropTableName;

        this.guaranteedDrops = guaranteedDrops;
    }

    public EnemyPackInfo(EnemyAmount[] enemyTypes, string[] flagsToCheckForAllies, string dropTableName)
    {
        this.enemyTypes = enemyTypes;

        this.flagsToCheckForAllies = flagsToCheckForAllies;

        this.dropTableName = dropTableName;
    }

    public virtual string getQuestStep()
    {
        return "";
    }

    public virtual QuestStepActivationScript getQuestScript()
    {
        return null;
    }


    public virtual int getXPDrops()
    {
        return 0;
    }

    public virtual bool isBossMonster()
    {
        return false;
    }

    public virtual string getQuestName()
    {
        return null;
    }

    public string getPackName()
    {
        return MonsterNameList.getPackName(enemyTypes[Constants.indexZero].enemyStats.getName());
    }

    public virtual void markBossAsKilled()
    {
        //empty on purpose
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

//info about a pack of enemies on the overworld, such as how many of them there are and of what type. Stored in State
public class BossPackInfo : EnemyPackInfo
{

    public string killFlagKey;
    public int xpDrop = 0;

    public string questName;
    public string questStepName;

    public QuestStepActivationScript script;

    public BossPackInfo(EnemyAmount[] enemyTypes, string dropTableName, string killFlagKey):
    base(enemyTypes, dropTableName)
    {
        this.enemyTypes = enemyTypes;

        this.dropTableName = dropTableName;
        this.killFlagKey = killFlagKey;
    }

    public BossPackInfo(EnemyAmount[] enemyTypes, string dropTableName, string killFlagKey, string dialogueUponSceneLoadKey):
    base(enemyTypes, dropTableName)
    {
        this.enemyTypes = enemyTypes;

        this.dropTableName = dropTableName;
        this.killFlagKey = killFlagKey;
        
        this.dialogueUponSceneLoadKey = dialogueUponSceneLoadKey;
    }

    public BossPackInfo(EnemyAmount[] enemyTypes, string dropTableName, ItemListID[] guaranteedDrops, string killFlagKey):
    base(enemyTypes, dropTableName, guaranteedDrops)
    {
        this.enemyTypes = enemyTypes;

        this.dropTableName = dropTableName;
        this.killFlagKey = killFlagKey;
    }

    public BossPackInfo(EnemyAmount[] enemyTypes, string dropTableName, string killFlagKey, QuestStepActivationScript script):
    base(enemyTypes, dropTableName)
    {
        this.enemyTypes = enemyTypes;

        this.dropTableName = dropTableName;

        this.killFlagKey = killFlagKey;

        this.script = script;
    }

    public BossPackInfo(EnemyAmount[] enemyTypes, string dropTableName, ItemListID[] guaranteedDrops, string killFlagKey, QuestStepActivationScript script):
    base(enemyTypes, dropTableName, guaranteedDrops)
    {
        this.enemyTypes = enemyTypes;

        this.dropTableName = dropTableName;

        this.killFlagKey = killFlagKey;

        this.script = script;
    }

    public BossPackInfo(EnemyAmount[] enemyTypes, string[] flagsToCheckForAllies, string dropTableName, QuestStepActivationScript script):
    base(enemyTypes, flagsToCheckForAllies, dropTableName)
    {
        this.enemyTypes = enemyTypes;

        this.flagsToCheckForAllies = flagsToCheckForAllies;

        this.dropTableName = dropTableName;

        this.script = script;
    }

    public override string getQuestStep()
    {
        return questStepName;
    }

    public override string getQuestName()
    {
        return questName;
    }

    public override QuestStepActivationScript getQuestScript()
    {
        return script;
    }

    public override void markBossAsKilled()
    {
        Flags.setFlag(killFlagKey, true);
    }

    public override bool isBossMonster()
    {
        return true;
    }

    public override int getXPDrops()
    {
        return xpDrop;
    }
}
