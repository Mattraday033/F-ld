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

    public StoryStatRequirementVariableSource(Dictionary<string, int> newStatReqs)
    {
        this.statRequirements = new List<KeyValuePair<string, int>>();

        foreach(KeyValuePair<string, int> statReq in newStatReqs)
        {
            statRequirements.Add(statReq);
        }
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

public class StoryFlagList : IStoryVariableSource
{
    private List<KeyValuePair<string, string>> flagList = new List<KeyValuePair<string, string>>();

    public StoryFlagList(string variableName = null,
                            string variableContents = null, 
                            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(), 
                            List<KeyValuePair<string, string>> kvps = null)
    {
        if(variableName != null && variableContents != null)
        {
            flagList.Add(new KeyValuePair<string, string>(variableName, variableContents));
        }

        if(kvp.Key != null && kvp.Key.Length > 0 && kvp.Value != null && kvp.Value.Length > 0)
        {
            flagList.Add(kvp);
        }

        if(kvps != null && kvps.Count > 0)
        {
            flagList.AddRange(kvps);
        }
    }

    public Story addVariables(Story story)
    {
        foreach(KeyValuePair<string, string> flag in flagList)
        {
            if(story.variablesState[flag.Key] != null)
            {
                story.variablesState[flag.Key] = flag.Value;
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

    public Dialogue(string[] names, TextAsset inkJSON, NPCCombatInfo npcCombatInfo, IStoryVariableSource variableSource = null)
	{
        this.names = createNameArray(names);

        this.cameraFoci = new GameObject[this.names.Length];
		this.inkJSON = inkJSON;
		this.npcCombatInfo = npcCombatInfo;

        if(variableSource != null)
        {
            this.variableSources.Add(variableSource);
        }
	}

	public Dialogue(string[] names, TextAsset inkJSON, NPCCombatInfo npcCombatInfo, TextAsset[] secondaryInkJSONs)
	{
        this.names = createNameArray(names);

        this.cameraFoci = new GameObject[this.names.Length];
		this.inkJSON = inkJSON;
		this.npcCombatInfo = npcCombatInfo;
        this.secondaryInkJSONs = secondaryInkJSONs;
	}

    public virtual bool findNPCGameObjectsInScene()
    {
        return true;
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

    protected static Dialogue addInfoToClone(Dialogue original, Dialogue clone)
    {
        for (int index = 0; index < clone.names.Length; index++)
        {
            clone.names[index] = original.names[index];
        }

        clone.npcCombatInfo = original.npcCombatInfo;

        clone.variableSources = new List<IStoryVariableSource>();

        foreach(IStoryVariableSource variableSource in original.variableSources)
        {
            clone.variableSources.Add(variableSource);
        }

        if(original.secondaryInkJSONs == null)
        {
            original.secondaryInkJSONs = new TextAsset[0];
            clone.secondaryInkJSONs = new TextAsset[0];
        } else
        {
            clone.secondaryInkJSONs = new TextAsset[original.secondaryInkJSONs.Length];

            int index = 0;
            foreach(TextAsset secondaryInkJSON in original.secondaryInkJSONs)
            {
                clone.secondaryInkJSONs[index] = secondaryInkJSON;
                index++;
            }
        }

        return clone;
    }

    public virtual Dialogue clone()
    {
        Dialogue clone = new Dialogue(new string[names.Length], new GameObject[cameraFoci.Length], inkJSON, variableSources);

        return addInfoToClone(this, clone);
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

public class SingleCharacterDialogue : Dialogue
{
    public SingleCharacterDialogue( string name, 
                                    TextAsset inkJSON, 
                                    NPCCombatInfo npcCombatInfo = null, 
                                    IStoryVariableSource variableSource = null) : 
    base(new string[]{name}, inkJSON, npcCombatInfo, variableSource)
    {
        
    }

    public override bool findNPCGameObjectsInScene()
    {
        return false;
    }

    public override Dialogue clone()
    {
        SingleCharacterDialogue clone = new SingleCharacterDialogue(names[Constants.indexOne], inkJSON);

        return addInfoToClone(this, clone);
    }
}

public class GenericDialogue : SingleCharacterDialogue, IStoryVariableSource
{
    private string dialogueContents;

    public GenericDialogue(string name, string dialogueContents) : 
    base(name, InkAssetList.getInkJSON(DialogueKey.GenericDialogue))
    {
        this.dialogueContents = dialogueContents;
        
        variableSources.Add(this);
    }

    public override Dialogue clone()
    {
        return new GenericDialogue(names[Constants.indexOne], dialogueContents);
    }

    #region IStoryVariableSource methods


    public Story addVariables(Story story)
    {
        return InkVariableNameList.setStoryVariable(story, InkVariableNameList.description, dialogueContents);
    }

    #endregion
}