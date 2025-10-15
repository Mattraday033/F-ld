using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public interface IStoryVariableSource
{
    public Story addVariables(Story story);
}

public class VaultableObject : MonoBehaviour, IStoryVariableSource
{
    public const bool isPlural = true;
    public const string barrelName = "barrels";
    public const int oneTile = 1;

    public static VaultableObject vaultableBarrels = new VaultableObject(VaultableObject.oneTile, VaultableObject.isPlural, VaultableObject.barrelName);

    public int size;

    public bool plural;

    public string objectName;

    public VaultableObject(int size, bool plural, string objectName)
    {
        this.size = size;
        this.plural = plural;
        this.objectName = objectName;
    }

    public Story addVariables(Story story)
    {
        if (story.variablesState[nameof(size)] != null)
        {
            story.variablesState[nameof(size)] = size;
        }

        if (story.variablesState[nameof(plural)] != null)
        {
            story.variablesState[nameof(plural)] = size;
        }

        if (story.variablesState[nameof(objectName)] != null)
        {
            story.variablesState[nameof(objectName)] = objectName;
        }

        return story;
    }

}
