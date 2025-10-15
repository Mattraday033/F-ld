using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

//[System.Serializable]
public class Dialogue : ICloneable
{
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
        if (names[0].Length > 0)
        {
            this.names = Helpers.appendArray<string>(stringArrayWithPlayerSpace, names);
        }
        else
        {
            this.names = names;
        }

        this.cameraFoci = new GameObject[this.names.Length];
        this.inkJSON = inkJSON;
    }

    public Dialogue(string[] names, GameObject[] cameraFoci, TextAsset inkJSON)
    {
        this.names = names;
        this.cameraFoci = cameraFoci;
        this.inkJSON = inkJSON;
    }

	public Dialogue(string[] names, GameObject[] cameraFoci, TextAsset inkJSON, TextAsset[] secondaryInkJSONs)
	{
		this.names = names;
		this.cameraFoci = cameraFoci;
		this.inkJSON = inkJSON;
		this.secondaryInkJSONs = secondaryInkJSONs;
	}

	public Dialogue(string[] names, GameObject[] cameraFoci, TextAsset inkJSON, NPCCombatInfo npcCombatInfo)
	{
		this.names = names;
		this.cameraFoci = cameraFoci;
		this.inkJSON = inkJSON;
		this.npcCombatInfo = npcCombatInfo;
	}

	public string getMainNPCName()
	{
		return names[1];
	}

	public TutorialSequenceListTrigger GetTutorialSequenceListTrigger()
	{
		if (cameraFoci == null || cameraFoci.Length < 2 || cameraFoci[1] == null)
		{
			return null;
		}

		return cameraFoci[1].GetComponent<TutorialSequenceListTrigger>();
	}

	public object Clone()
	{
		return this.MemberwiseClone();
	}
	
	public Dialogue clone()
    {
		Dialogue clone = new Dialogue(new string[names.Length], new GameObject[cameraFoci.Length], inkJSON);

		for (int index = 0; index < clone.names.Length; index++)
		{
			clone.names[index] = names[index];
		}

		clone.npcCombatInfo = npcCombatInfo;

		return clone;
    }
}
