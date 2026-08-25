#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// Audio's binding into ResourceEnumGenerator: every importable clip under Resources/Audio becomes
/// a line in AudioClipFilePaths.txt and a member of SFXType. AudioClipList reads the pair back at
/// runtime.
/// </summary>
public static class SFXTypeGenerator
{
    public static readonly ResourceEnumDefinition Definition = new ResourceEnumDefinition
    {
        SourceFolderPath = "Assets/Resources/Audio",
        ManifestAssetPath = "Assets/Resources/AudioClipFilePaths.txt",
        EnumAssetPath = "Assets/src/Audio/SFXType.cs",
        EnumName = "SFXType",
        GeneratorName = "SFXTypeGenerator",
        RegenerateHint = "Tools > Audio > Regenerate SFXType",

        // AudioClipList.reservedSFXTypeCount must match this length.
        ReservedMemberNames = new string[] { "NoSFX" },

        Extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".wav", ".mp3", ".ogg", ".aif", ".aiff", ".xm", ".it", ".mod", ".s3m"
        },

        // The footstep pack is ~100 unused vendor files that would otherwise double the enum.
        ExcludedFolders = new string[]
        {
            "Audio/Sound Effects/Footsteps/WAV - 44100 Hz - 16 Bit/"
        }
    };

    [MenuItem("Tools/Audio/Regenerate SFXType")]
    public static void Generate()
    {
        ResourceEnumGenerator.Generate(Definition);
    }
}
#endif
