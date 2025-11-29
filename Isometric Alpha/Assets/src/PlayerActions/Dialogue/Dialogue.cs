using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class StoryStatRequirementVariableSource : IStoryVariableSource
{
    private List<KeyValuePair<string, int>> statRequirements = new List<KeyValuePair<string, int>>();

    public StoryStatRequirementVariableSource(string variableName, int statLevel)
    {
        statRequirements.Add(new KeyValuePair<string, int>(variableName, statLevel));
    }

    public StoryStatRequirementVariableSource(List<KeyValuePair<string, int>> statRequirements)
    {
        this.statRequirements = statRequirements;
    }

    public Story addVariables(Story story)
    {
        foreach(KeyValuePair<string, int> statRequirement in statRequirements)
        {
            if(story.variablesState[statRequirement.Key] != null)
            {
                story.variablesState[statRequirement.Key] = statRequirement.Value;
            }
        }

        return story;
    }
}

[System.Serializable]
public class Dialogue : ICloneable
{
    public const int mainNPCIndex = 1;
    public static string[] stringArrayWithPlayerSpace = new string[] { "" };

	public string[] names; //to keep names[] index the same as cameraFoci index, names[0] will always be blank
    public GameObject[] cameraFoci; //cameraFoci[0] is always the player. cameraFoci[1] should be the first person the player speaks too. Order after that doesn't matter.

    public List<IStoryVariableSource> variableSources = new List<IStoryVariableSource>();

    public bool random;
	public bool convoEndableAtStart;
	public bool isVaultable;
	public TextAsset inkJSON;

	public bool startWithUIDisabled;


    public TextAsset[] secondaryInkJSONs;

	public NPCCombatInfo npcCombatInfo;

	public Dialogue(string name, GameObject npc)
    {
        this.names = Helpers.appendArray<string>(stringArrayWithPlayerSpace, new string[] { name });

		this.cameraFoci = new GameObject[2] {null, npc };
		this.inkJSON = null;
	}

    public Dialogue(string[] names, TextAsset inkJSON)
    {
        this.names = createNameArray(names);

        this.cameraFoci = new GameObject[this.names.Length];
        this.inkJSON = inkJSON;
    }

    public Dialogue(string[] names, TextAsset inkJSON, IStoryVariableSource variableSource)
    {
        this.names = createNameArray(names);

        this.cameraFoci = new GameObject[this.names.Length];
        this.inkJSON = inkJSON;

        variableSources.Add(variableSource);
    }

	public Dialogue(string[] names, GameObject[] cameraFoci, TextAsset inkJSON, TextAsset[] secondaryInkJSONs)
	{
        this.names = createNameArray(names);

		this.cameraFoci = cameraFoci;
		this.inkJSON = inkJSON;
		this.secondaryInkJSONs = secondaryInkJSONs;
	}

	public Dialogue(string[] names, TextAsset inkJSON, TextAsset[] secondaryInkJSONs)
	{
        this.names = createNameArray(names);

        this.cameraFoci = new GameObject[this.names.Length];

		this.inkJSON = inkJSON;
		this.secondaryInkJSONs = secondaryInkJSONs;
	}


	public Dialogue(string[] names, TextAsset inkJSON, NPCCombatInfo npcCombatInfo)
	{
        this.names = createNameArray(names);

        this.cameraFoci = new GameObject[this.names.Length];
		this.inkJSON = inkJSON;
		this.npcCombatInfo = npcCombatInfo;
	}

	public Dialogue(string[] names, TextAsset inkJSON, NPCCombatInfo npcCombatInfo, TextAsset[] secondaryInkJSONs)
	{
        this.names = createNameArray(names);

        this.cameraFoci = new GameObject[this.names.Length];
		this.inkJSON = inkJSON;
		this.npcCombatInfo = npcCombatInfo;
        this.secondaryInkJSONs = secondaryInkJSONs;
	}

    private string[] createNameArray(string[] npcNames)
    {
        if (npcNames[0] == null || npcNames[0].Length > 0)
        {
            return Helpers.appendArray<string>(stringArrayWithPlayerSpace, npcNames);
        }
        else
        {
            return npcNames;
        }
    }

    public string getName()
    {
        return names[mainNPCIndex];
    }

	public object Clone()
	{
		return this.MemberwiseClone();
	}

    public Dialogue clone()
    {
        Dialogue clone = new Dialogue(new string[names.Length], new GameObject[cameraFoci.Length], inkJSON, variableSources);

        for (int index = 0; index < clone.names.Length; index++)
        {
            clone.names[index] = names[index];
        }

        clone.npcCombatInfo = npcCombatInfo;

        return clone;
    }
    
    //Clone Constructor
    public Dialogue(string[] names, GameObject[] cameraFoci, TextAsset inkJSON, List<IStoryVariableSource> variableSources)
    {
        this.names = names;

        this.cameraFoci = cameraFoci;
        this.inkJSON = inkJSON;

        this.variableSources = variableSources;
    }
}
