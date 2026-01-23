using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct CreatureAmount
{
    public int amount;
    public Stats enemyStats;

    public CreatureAmount(int amount, Stats enemyStats)
    {
        this.amount = amount;

        this.enemyStats = enemyStats;
    }
}

//info about a pack of enemies on the overworld, such as how many of them there are and of what type. Stored in State
public class EnemyPackInfo : MonoBehaviour, IDescribableInBlocks, ICreatureSpawnPackage
{

    public string tutorialSequenceKey;

    public CreatureAmount[] creatureTypes;

    public string dropTableName;

    public string dialogueUponSceneLoadKey;

    public ItemListID[] guaranteedDrops;

    public int numberOfDrops = 1; //number of rolls on their drop table


    public EnemyPackInfo(CreatureAmount[] creatureTypes, string dropTableName)
    {
        this.creatureTypes = creatureTypes;

        this.dropTableName = dropTableName;
    }

    public EnemyPackInfo(CreatureAmount[] creatureTypes, string dropTableName, string tutorialSequenceKey)
    {
        this.creatureTypes = creatureTypes;

        this.dropTableName = dropTableName;

        this.tutorialSequenceKey = tutorialSequenceKey;
    }

    public EnemyPackInfo(CreatureAmount[] creatureTypes, string dropTableName, ItemListID[] guaranteedDrops)
    {
        this.creatureTypes = creatureTypes;

        this.dropTableName = dropTableName;

        this.guaranteedDrops = guaranteedDrops;
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

    public virtual bool hasCreaturesToSpawn()
    {
        return true;
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
        return MonsterNameList.getPackName(creatureTypes[Constants.indexZero].enemyStats.getName());
    }

    public virtual void markBossAsKilled()
    {
        //empty on purpose
    }

    public int determineEnemyCount(int index)
    {
        return creatureTypes[index].amount;
    }


    //IDescribableInBlocks methods
    public string getName()
    {
        return "";
    }

    public virtual IEnumerator GetEnumerator()
    {
        List<Stats> allStatsInPack = new List<Stats>();

        //CreatureAmount[] creatureTypes
        foreach (CreatureAmount amount in creatureTypes)
        {
            for(int index = 0; index < amount.amount; index++)
            {
                allStatsInPack.Add(amount.enemyStats);
            }
        }

        foreach (Stats stats in allStatsInPack)
        {
            if(stats == null)
            {
                continue;
            }

            yield return stats.clone();
        }
    }

    public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> blocks = new List<DescriptionPanelBuildingBlock>();

        for (int enemyIndex = 0; enemyIndex < creatureTypes.Length; enemyIndex++)
        {
            string enemyNumber = creatureTypes[enemyIndex].amount.ToString();

            blocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, enemyNumber + "   " + creatureTypes[enemyIndex].enemyStats.getName()));
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

    public BossPackInfo(CreatureAmount[] creatureTypes, string dropTableName, string killFlagKey):
    base(creatureTypes, dropTableName)
    {
        this.creatureTypes = creatureTypes;

        this.dropTableName = dropTableName;
        this.killFlagKey = killFlagKey;
    }

    public BossPackInfo(CreatureAmount[] creatureTypes, string dropTableName, string killFlagKey, string dialogueUponSceneLoadKey):
    base(creatureTypes, dropTableName)
    {
        this.creatureTypes = creatureTypes;

        this.dropTableName = dropTableName;
        this.killFlagKey = killFlagKey;
        
        this.dialogueUponSceneLoadKey = dialogueUponSceneLoadKey;
    }

    public BossPackInfo(CreatureAmount[] creatureTypes, string dropTableName, ItemListID[] guaranteedDrops, string killFlagKey):
    base(creatureTypes, dropTableName, guaranteedDrops)
    {
        this.creatureTypes = creatureTypes;

        this.dropTableName = dropTableName;
        this.killFlagKey = killFlagKey;
    }

    public BossPackInfo(CreatureAmount[] creatureTypes, string dropTableName, string killFlagKey, QuestStepActivationScript script):
    base(creatureTypes, dropTableName)
    {
        this.creatureTypes = creatureTypes;

        this.dropTableName = dropTableName;

        this.killFlagKey = killFlagKey;

        this.script = script;
    }

    public BossPackInfo(CreatureAmount[] creatureTypes, string dropTableName, ItemListID[] guaranteedDrops, string killFlagKey, QuestStepActivationScript script):
    base(creatureTypes, dropTableName, guaranteedDrops)
    {
        this.creatureTypes = creatureTypes;

        this.dropTableName = dropTableName;

        this.killFlagKey = killFlagKey;

        this.script = script;
    }

    public BossPackInfo(CreatureAmount[] creatureTypes, string dropTableName, QuestStepActivationScript script):
    base(creatureTypes, dropTableName)
    {
        this.creatureTypes = creatureTypes;

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
