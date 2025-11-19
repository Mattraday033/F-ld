using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public interface IStoryVariableSource
{
    public Story addVariables(Story story);
}

public class VaultableObject : IStoryVariableSource
{
    public const bool isPlural = true;
    public const bool notPlural = false;
    public const string barrelName = "barrels";
    public const string rockName = "rocks";
    public const string gapName = "gap";

    public readonly static VaultableObject vaultableBarrelsOneTile = new VaultableObject(Constants.sizeOne, isPlural, barrelName);
    public readonly static VaultableObject vaultableBarrelsTwoTiles = new VaultableObject(Constants.sizeTwo, isPlural, barrelName);

    public readonly static VaultableObject vaultableGap = new VaultableObject(Constants.sizeThree, notPlural, gapName);

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
            story.variablesState[nameof(plural)] = plural;
        }

        if (story.variablesState[nameof(objectName)] != null)
        {
            story.variablesState[nameof(objectName)] = objectName;
        }

        return story;
    }

}
