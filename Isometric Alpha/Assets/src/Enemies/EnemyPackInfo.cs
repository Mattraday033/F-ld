using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CreatureAmount
{
    public int amount;
    public Stats enemyStats;

    public CreatureAmount(int amount, Stats enemyStats)
    {
        this.amount = amount;

        this.enemyStats = enemyStats;
    }
}

public delegate void BeforeCombatAction();
public delegate void AfterCombatAction();

//info about a pack of enemies on the overworld, such as how many of them there are and of what type. Stored in State
public class EnemyPackInfo : IDescribableInBlocks, ICreatureSpawnPackage
{
    private string _TutorialSequenceKey;

    public string tutorialSequenceKey
    {
        get
        {
            if(!TutorialFlags.getFlag(TutorialSequenceList.mandatoryTargetTutorialSeenFlag))
            {
                foreach(CreatureAmount creatureAmount in FoeTypes)
                {
                    if(creatureAmount.enemyStats.isMandatoryTarget())
                    {
                        return TutorialSequenceList.mandatoryTargetTutorialSequenceKey;
                    }
                }
            }
            
            return _TutorialSequenceKey;
        } 
        private set
        {
            _TutorialSequenceKey = value;
        }
    }

    public CreatureAmount[] FoeTypes;

    public string dropTableName;

    public string dialogueUponSceneLoadKey;

    public ItemListID[] guaranteedDrops;

    public List<SpawnDetails> spawnDetailsList;
    private int currentSpawnDetailsIndex = 0;

    public int numberOfDrops = 1; //number of rolls on their drop table

    public bool alwaysSurprised;
    public WinCondition winCon;
    public List<BeforeCombatAction> beforeCombatActions;
    public List<AfterCombatAction> afterCombatActions;

    public EnemyPackInfo(CreatureAmount[] FoeTypes, 
                            string dropTableName,
                            ItemListID[] guaranteedDrops = null,
                            string tutorialSequenceKey = "", 
                            List<SpawnDetails> spawnDetailsList = null, 
                            bool alwaysSurprised = false, 
                            WinCondition winCon = null,
                            List<BeforeCombatAction> beforeCombatActions = null,
                            List<AfterCombatAction> afterCombatActions = null,
                            string dialogueUponSceneLoadKey = null)
    {
        this.FoeTypes = FoeTypes;

        this.dropTableName = dropTableName;

        this.dialogueUponSceneLoadKey = dialogueUponSceneLoadKey;

        this.tutorialSequenceKey = tutorialSequenceKey;

        this.guaranteedDrops = guaranteedDrops;

        this.spawnDetailsList = spawnDetailsList;

        if(this.spawnDetailsList != null)
        {
            CombatStateManager.OnCombatStart.AddListener(() => currentSpawnDetailsIndex = 0);
            LoadSaveFile.OnLoadResetData.AddListener(() => currentSpawnDetailsIndex = 0);
        }

        this.alwaysSurprised = alwaysSurprised;

        this.winCon = winCon ?? WinLoseConditionList.defeatAllEnemies;
        this.beforeCombatActions = beforeCombatActions ?? new List<BeforeCombatAction>();
        this.afterCombatActions = afterCombatActions ?? new List<AfterCombatAction>();
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

            blocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, " x"+enemyNumber + " "+FoeTypes[enemyIndex].enemyStats.getName() + " "));
        }

        return blocks;
    }

    public bool requiresInspectNode()
    {
        return false;
    }

    public SpawnDetails getNextSpawnDetails()
    {
        if(spawnDetailsList == null || spawnDetailsList.Count == 0 || currentSpawnDetailsIndex >= spawnDetailsList.Count)
        {
            return null;
        }

        if(currentSpawnDetailsIndex < 0)
        {
            currentSpawnDetailsIndex = 0;
        }

        SpawnDetails spawnDetails = spawnDetailsList[currentSpawnDetailsIndex];

        currentSpawnDetailsIndex++;

        return spawnDetails;
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

    public BossPackInfo(CreatureAmount[] FoeTypes, 
                        string dropTableName,
                        string killFlagKey = "",
                        string dialogueUponSceneLoadKey = null,
                        ItemListID[] guaranteedDrops = null, 
                        QuestStepActivationScript script = null, 
                        int xpDrop = 0, 
                        List<SpawnDetails> spawnDetailsList = null):
    base(FoeTypes, dropTableName, guaranteedDrops, spawnDetailsList: spawnDetailsList)
    {
        this.FoeTypes = FoeTypes;

        this.dropTableName = dropTableName;
        this.killFlagKey = killFlagKey;
        
        this.dialogueUponSceneLoadKey = dialogueUponSceneLoadKey;
        this.script = script;

        this.xpDrop = xpDrop;
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
