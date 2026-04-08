using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct InventoryWrapper
{
    public string key;
    public string[] inventory;
}


[System.Serializable]
public struct PositionWrapper
{
    public float x;
    public float y;
    public float z;

    public PositionWrapper(Vector3 position)
    {
        x = position.x;
        y = position.y;
        z = 0f;
    }

    public Vector3 getPosition()
    {
        return new Vector3(x, y, z);
    }
}

[System.Serializable]
public struct EnemyStatWrapper
{
    public PositionWrapper positionWrapper;

    public Facing facing;

    public int intimidateCounter;
    public int cunningCounter;
    public int retreatCounter;

    public EnemyStatWrapper(Vector3 position, Facing facing, int intimidateCounter, int cunningCounter, int retreatCounter)
    {
        positionWrapper = new PositionWrapper(position);
        this.facing = facing;
        this.intimidateCounter = intimidateCounter;
        this.cunningCounter = cunningCounter;
        this.retreatCounter = retreatCounter;
    }

    public Vector3 getPosition()
    {
        return positionWrapper.getPosition();
    }
}

[System.Serializable]
public struct FlagWrapper
{

    public string flagName;
    public bool flagStatus;

    public FlagWrapper(string flagName, bool flagStatus)
    {
        this.flagName = flagName;
        this.flagStatus = flagStatus;
    }

    public FlagWrapper(KeyValuePair<string, bool> kvp)
    {
        this.flagName = kvp.Key;
        this.flagStatus = kvp.Value;
    }

    public static FlagWrapper[] getAllFlagsInDictionary(Dictionary<string, bool> dict)
    {
        List<FlagWrapper> flagWrappers = new List<FlagWrapper>();

        foreach (KeyValuePair<string, bool> kvp in dict)
        {
            flagWrappers.Add(new FlagWrapper(kvp));
        }

        return flagWrappers.ToArray();
    }

    public static Dictionary<string, bool> convertFlagWrapperListToDictionary(FlagWrapper[] flagWrappers)
    {
        Dictionary<string, bool> dict = new Dictionary<string, bool>();

        foreach (FlagWrapper wrapper in flagWrappers)
        {
            dict[wrapper.flagName] = wrapper.flagStatus;
        }

        return dict;
    }

}

[System.Serializable]
public struct StatsWrapper
{
    public string key;

    public int strength;
    public int dexterity;
    public int wisdom;
    public int charisma;

    public int level;
    public int xp;
    public int totalHealth;
    public int currentHealth;

    public bool canJoinParty;

    public bool placed;

    public string partyMemberPlacedPosition;
    public GridCoords partyMemberFormationCoords;

    public string[] currentEquipment;
    public string[] combatActions;

    public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        if (!canJoinParty)
        {
            return new List<DescriptionPanelBuildingBlock>();
        }

        List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, key.Replace(PartyManager.playerMarker, "")));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Level: " + level));
        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Health: " + currentHealth + "/" + totalHealth));
        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Experience: " + xp));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, ""));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Strength: " + strength));
        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Dexterity: " + dexterity));
        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Wisdom: " + wisdom));
        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Charisma: " + charisma));


        return buildingBlocks;
    }

}

[System.Serializable]
public struct QuestWrapper
{
    public string title;
    public bool active;
    public bool finished;
    public bool succeeded;
    public QuestStepWrapper[] steps;
    public DeathStepWrapper[] deathSteps;

    public QuestWrapper(Quest quest)
    {
        this.title = quest.title;
        this.active = quest.active;
        this.finished = quest.finished;
        this.succeeded = quest.succeeded;

        this.steps = new QuestStepWrapper[quest.steps.Count];

        int index = 0;
        foreach(KeyValuePair<string, QuestStep> kvp in quest.steps)
        {
            this.steps[index] = new QuestStepWrapper(kvp.Value);
            index++;
        }

        this.deathSteps = new DeathStepWrapper[quest.deathSteps.Count];
        index = 0;
        foreach(KeyValuePair<string, DeathStep> kvp in quest.deathSteps)
        {
            this.deathSteps[index] = new DeathStepWrapper(kvp.Value);
            index++;
        }
    }

    public Quest unwrapQuest(Quest quest)
    {
        quest.active = active;
        quest.finished = finished;
        quest.succeeded = succeeded;

        for(int stepIndex = 0; stepIndex < quest.steps.Count && stepIndex < steps.Length; stepIndex++)
        {
            steps[stepIndex].stepName = stepNameEdits(steps[stepIndex].stepName);

            quest.steps[steps[stepIndex].stepName] = steps[stepIndex].unwrapQuestStep(quest.steps[steps[stepIndex].stepName]);
        }

        for(int stepIndex = 0; stepIndex < quest.deathSteps.Count && stepIndex < deathSteps.Length; stepIndex++)
        {
            quest.deathSteps[deathSteps[stepIndex].stepName] = deathSteps[stepIndex].unwrapDeathStep(quest.deathSteps[deathSteps[stepIndex].stepName]);
        }

        return quest;
    }

    private static string stepNameEdits(string stepName)
    {
        switch(stepName)
        {
            case "Find the key to the Director's office.":
                return QuestNameList.thePlanStepTitleFifteen;

            case "Recompense, finally.":
                return QuestNameList.dealWithThePrisonersStepTitleTwo;

            case "Márcos, free.":
                return QuestNameList.dealWithThePrisonersStepTitleThree;
            case "Márcos, punished.":
                return QuestNameList.dealWithThePrisonersStepTitleFour;
            case "Márcos, executed.":
                return QuestNameList.dealWithThePrisonersStepTitleFive;
            case "Márcos, mobbed.":
                return QuestNameList.dealWithThePrisonersStepTitleSix;
            
            case "András, free.":
                return QuestNameList.dealWithThePrisonersStepTitleSeven;
            case "András, punished.":
                return QuestNameList.dealWithThePrisonersStepTitleEight;
            case "András, executed.":
                return QuestNameList.dealWithThePrisonersStepTitleNine;
            case "András, mobbed.":
                return QuestNameList.dealWithThePrisonersStepTitleTen;
            
            case "Réka, free.":
                return QuestNameList.dealWithThePrisonersStepTitleEleven;
            case "Réka, punished.":
                return QuestNameList.dealWithThePrisonersStepTitleTwelve;
            case "Réka, executed.":
                return QuestNameList.dealWithThePrisonersStepTitleThirteen;
            case "Réka, mobbed.":
                return QuestNameList.dealWithThePrisonersStepTitleFourteen;
            
            case "Pázmán, free.":
                return QuestNameList.dealWithThePrisonersStepTitleFifteen;
            case "Pázmán, punished.":
                return QuestNameList.dealWithThePrisonersStepTitleSixteen;
            case "Pázmán, executed.":
                return QuestNameList.dealWithThePrisonersStepTitleSeventeen;
            case "Pázmán, mobbed.":
                return QuestNameList.dealWithThePrisonersStepTitleEighteen;

            default:
                return stepName;
        }
    }
}

[System.Serializable]
public struct QuestStepWrapper
{
    public string stepName;
    public bool active;
    public int activationIndex;

    public QuestStepWrapper(QuestStep step)
    {
        this.stepName = step.stepName;
        this.active = step.active;
        this.activationIndex = step.activationIndex;
    }

    public QuestStep unwrapQuestStep(QuestStep questStep)
    {
        questStep.setActiveStatus(active, activationIndex);

        return questStep;
    }
}

[System.Serializable]
public struct DeathStepWrapper
{
    public string stepName;
    public bool active;
    public int activationIndex;
    public int currentStepOnDeath;

    public DeathStepWrapper(DeathStep deathStep)
    {
        
        this.stepName = deathStep.stepName;
        this.active = deathStep.active;
        this.activationIndex = deathStep.activationIndex;
        this.currentStepOnDeath = deathStep.currentStepOnDeath;
    }

    public DeathStep unwrapDeathStep(DeathStep deathStep)
    {
        deathStep.setActiveStatus(active, activationIndex);
        deathStep.currentStepOnDeath = currentStepOnDeath;

        return deathStep;
    }
}