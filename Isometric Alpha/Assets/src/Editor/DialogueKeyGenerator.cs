#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// Dialogue's binding into ResourceEnumGenerator: every compiled ink story under
/// Resources/Dialogue becomes a line in InkFilePaths.txt and a member of DialogueKey.
/// InkAssetList reads the pair back at runtime.
///
/// The collected asset is the .json, not the .ink, because the .json is what Unity imports as a
/// TextAsset and therefore what Resources.Load resolves. Ink is set to compile automatically
/// (see ProjectSettings/InkSettings.asset), so writing a story is enough to make it appear here;
/// any .ink that failed to compile is reported rather than quietly dropped.
/// </summary>
public static class DialogueKeyGenerator
{
    public static readonly ResourceEnumDefinition Definition = new ResourceEnumDefinition
    {
        SourceFolderPath = "Assets/Resources/Dialogue",
        ManifestAssetPath = "Assets/Resources/InkFilePaths.txt",
        EnumAssetPath = "Assets/src/PlayerActions/Dialogue/DialogueKey.cs",
        EnumName = "DialogueKey",
        GeneratorName = "DialogueKeyGenerator",
        RegenerateHint = "Tools > Dialogue > Regenerate DialogueKey",

        // InkAssetList.reservedDialogueKeyCount must match this length.
        ReservedMemberNames = new string[] { "NoDialogue" },

        Extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".json"
        },

        CompanionSourceExtensions = new string[] { ".ink" }
    };

    [MenuItem("Tools/Dialogue/Regenerate DialogueKey")]
    public static void Generate()
    {
        ResourceEnumGenerator.Generate(Definition);
    }
}
#endif
