using UnityEngine;

/// <summary>
/// Every compiled ink story under Resources/Dialogue, keyed by DialogueKey. Filled from the
/// generated manifest; see DialogueKeyGenerator.
/// </summary>
public static class InkAssetList
{

    private const int reservedDialogueKeyCount = 1;
    public const string inkFilePathsFileName = "InkFilePaths";

    private readonly static ResourceList<DialogueKey, TextAsset> inkAssets =
        new ResourceList<DialogueKey, TextAsset>(inkFilePathsFileName,
                                                 reservedDialogueKeyCount,
                                                 "[InkAssetList]",
                                                 "Tools > Dialogue > Regenerate DialogueKey");

    public static void init()
    {
        inkAssets.init();
    }

    /// <summary>
    /// The compiled ink JSON for a story, or null for DialogueKey.NoDialogue. Callers hand this
    /// straight to a Dialogue, which passes its text to a new Ink.Runtime.Story.
    /// </summary>
    public static TextAsset getInkJSON(DialogueKey dialogueKey)
    {
        return inkAssets.getAsset(dialogueKey);
    }
}
