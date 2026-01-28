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

    public CreatureAmount[] FoeTypes;

    public string dropTableName;

    public string dialogueUponSceneLoadKey;

    public ItemListID[] guaranteedDrops;

    public int numberOfDrops = 1; //number of rolls on their drop table


    public EnemyPackInfo(CreatureAmount[] FoeTypes, string dropTableName)
    {
        this.FoeTypes = FoeTypes;

        this.dropTableName = dropTableName;
    }

    public EnemyPackInfo(CreatureAmount[] FoeTypes, string dropTableName, string tutorialSequenceKey)
    {
        this.FoeTypes = FoeTypes;

        this.dropTableName = dropTableName;

        this.tutorialSequenceKey = tutorialSequenceKey;
    }

    public EnemyPackInfo(CreatureAmount[] FoeTypes, string dropTableName, ItemListID[] guaranteedDrops)
    {
        this.FoeTypes = FoeTypes;

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
        return MonsterNameList.getPackName(FoeTypes[Constants.indexZero].enemyStats.getName());
    }

    public virtual void markBossAsKilled()
    {
        //empty on purpose
    }

    public int determineEnemyCount(int index)
    {
        return FoeTypes[index].amount;
    }


    //IDescribableInBlocks methods
    public string getName()
    {
        return "";
    }

    public virtual IEnumerator GetEnumerator()
    {
        List<Stats> allStatsInPack = new List<Stats>();

        //CreatureAmount[] FoeTypes
        foreach (CreatureAmount amount in FoeTypes)
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

        for (int enemyIndex = 0; enemyIndex < FoeTypes.Length; enemyIndex++)
        {
            string enemyNumber = FoeTypes[enemyIndex].amount.ToString();

            blocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "  x"+enemyNumber + " "+FoeTypes[enemyIndex].enemyStats.getName() + " "));
        }

        return blocks;
    }

    public bool requiresInspectNode()
    {
        return false;
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

    public BossPackInfo(CreatureAmount[] FoeTypes, string dropTableName, string killFlagKey):
    base(FoeTypes, dropTableName)
    {
        this.FoeTypes = FoeTypes;

        this.dropTableName = dropTableName;
        this.killFlagKey = killFlagKey;
    }

    public BossPackInfo(CreatureAmount[] FoeTypes, string dropTableName, string killFlagKey, string dialogueUponSceneLoadKey):
    base(FoeTypes, dropTableName)
    {
        this.FoeTypes = FoeTypes;

        this.dropTableName = dropTableName;
        this.killFlagKey = killFlagKey;
        
        this.dialogueUponSceneLoadKey = dialogueUponSceneLoadKey;
    }

    public BossPackInfo(CreatureAmount[] FoeTypes, string dropTableName, ItemListID[] guaranteedDrops, string killFlagKey):
    base(FoeTypes, dropTableName, guaranteedDrops)
    {
        this.FoeTypes = FoeTypes;

        this.dropTableName = dropTableName;
        this.killFlagKey = killFlagKey;
    }

    public BossPackInfo(CreatureAmount[] FoeTypes, string dropTableName, string killFlagKey, QuestStepActivationScript script):
    base(FoeTypes, dropTableName)
    {
        this.FoeTypes = FoeTypes;

        this.dropTableName = dropTableName;

        this.killFlagKey = killFlagKey;

        this.script = script;
    }

    public BossPackInfo(CreatureAmount[] FoeTypes, string dropTableName, ItemListID[] guaranteedDrops, string killFlagKey, QuestStepActivationScript script):
    base(FoeTypes, dropTableName, guaranteedDrops)
    {
        this.FoeTypes = FoeTypes;

        this.dropTableName = dropTableName;

        this.killFlagKey = killFlagKey;

        this.script = script;
    }

    public BossPackInfo(CreatureAmount[] FoeTypes, string dropTableName, QuestStepActivationScript script):
    base(FoeTypes, dropTableName)
    {
        this.FoeTypes = FoeTypes;

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
