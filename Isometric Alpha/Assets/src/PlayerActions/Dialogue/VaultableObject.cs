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

    public readonly static VaultableObject diffTwoVaultableBarrelsOneTile = new VaultableObject(Constants.difficultyTwo, Constants.sizeOne, isPlural, barrelName);
    public readonly static VaultableObject diffTwoVaultableBarrelsTwoTiles = new VaultableObject(Constants.difficultyTwo, Constants.sizeTwo, isPlural, barrelName);

    public readonly static VaultableObject diffThreeVaultableBarrelsOneTile = new VaultableObject(Constants.difficultyThree, Constants.sizeOne, isPlural, barrelName);

    public readonly static VaultableObject diffTwoVaultableGap = new VaultableObject(Constants.difficultyTwo, Constants.sizeThree, notPlural, gapName);
    public readonly static VaultableObject diffThreeVaultableGap = new VaultableObject(Constants.difficultyThree, Constants.sizeThree, notPlural, gapName);

    public int dexDifficulty;

    public int size;

    public bool plural;

    public string objectName;

    public VaultableObject(int dexDifficulty, int size, bool plural, string objectName)
    {
        this.dexDifficulty = dexDifficulty;
        this.size = size;
        this.plural = plural;
        this.objectName = objectName;
    }

    public virtual Dialogue getDialogue(string name)
    {
        return new Dialogue(new string[] { Constants.emptyString, name }, Resources.Load<TextAsset>(DialogueNameList.vaultableObjectPath));
    }

    public virtual Story addVariables(Story story)
    {
        story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.size, size);
        story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.plural, plural);
        story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.objectName, objectName);
        story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.dexDiffVarName, dexDifficulty);

        return story;
    }

}

public class VaultableOrDestroyableObject : VaultableObject
{
    
    public const string gateKey = "-VaultableOrDestroyable-";
    private const string hastilyBuiltBarricadeExplanation = "This barricade was built in a hurry.";
    public readonly static VaultableOrDestroyableObject diffThreeVaultableBarricadeOneTileIndexZero = new VaultableOrDestroyableObject(Constants.difficultyThree, Constants.difficultyThree, Constants.sizeOne, notPlural, NPCNameList.barricade, hastilyBuiltBarricadeExplanation, 0);

    public int strDifficulty;
    public string explanation;
    public int index;

    public VaultableOrDestroyableObject(int dexDifficulty, int strDifficulty, int size, bool plural, string objectName, string explanation, int index):
    base(dexDifficulty, size, plural, objectName)
    {
        this.strDifficulty = strDifficulty;
        this.explanation = explanation;
        this.index = index;
    }

    public override Dialogue getDialogue(string name)
    {
        return new Dialogue(new string[] { Constants.emptyString, name }, Resources.Load<TextAsset>(DialogueNameList.vaultableOrDestroyableObjectPath));
    }

    public override Story addVariables(Story story)
    {
        story = base.addVariables(story);

        story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.strDiffVarName, strDifficulty);
        story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.explanation, explanation);
        story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.gateKey, gateKey+index);

        return story;
    }

}

public class Ladder : IStoryVariableSource
{
    public const string barracksLadderDescription = "This ladder is old and weather-worn. One false step could alert the guards, or prove fatal.";

    public Facing facing;
    public int dexDifficulty;

    public string locationName;
    public string destinationName;
    public string description;

    public Ladder(int dexDifficulty, string locationName, string destinationName, string description, Facing facing)
    {
        this.dexDifficulty = dexDifficulty;
        this.locationName = locationName;
        this.destinationName = destinationName;
        this.description = description;
        this.facing = facing;
    }

    public static Dialogue getDialogue()
    {
        return new SingleCharacterDialogue(NPCNameList.ladder, Resources.Load<TextAsset>(DialogueNameList.ladderPath));
    }

    public virtual Story addVariables(Story story)
    {
        story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.description, description);
        story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.destinationName, destinationName);
        story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.dexDiffVarName, dexDifficulty);

        return story;
    }

}