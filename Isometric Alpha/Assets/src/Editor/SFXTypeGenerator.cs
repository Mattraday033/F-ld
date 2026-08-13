#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Walks Assets/Resources/Audio and derives two generated files from what it finds: a manifest of
/// Resources-relative clip paths, and an SFXType enum with one member per clip. Line N of the
/// manifest corresponds to enum member N, which is what lets AudioClipList.init() build its
/// dictionary without having to parse names back out of the paths.
/// </summary>
public static class SFXTypeGenerator
{
    private const string AudioFolderPath = "Assets/Resources/Audio";
    private const string ResourcesPrefix = "Assets/Resources/";

    // Must be .txt or Unity won't import it as a TextAsset, and Resources.Load would return null.
    private const string ManifestAssetPath = "Assets/Resources/AudioClipFilePaths.txt";
    private const string EnumAssetPath = "Assets/src/Audio/SFXType.cs";

    private const string EnumName = "SFXType";

    // Members emitted before any clip, so default(SFXType) means "no sound" and callers have a
    // value to represent silence. These have no clip behind them and therefore no manifest line,
    // which shifts every clip one place: manifest line N is enum member N + ReservedMemberNames.Length.
    // AudioClipList.reservedSFXTypeCount must match this length.
    private static readonly string[] ReservedMemberNames = { "NoSFX" };

    // Extensions Unity actually imports as an AudioClip. Filtering on these drops .meta files and
    // the one .flac under Hit/Blunt/, which Unity cannot import and so must not enter the enum.
    private static readonly HashSet<string> AudioExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".wav", ".mp3", ".ogg", ".aif", ".aiff", ".xm", ".it", ".mod", ".s3m"
    };

    // Folders under Audio/ that exist on disk but aren't part of the game. The footstep pack is
    // ~120 unused vendor files that would otherwise triple the size of the enum.
    private static readonly string[] ExcludedFolders =
    {
        "Audio/Sound Effects/Footsteps/WAV - 44100 Hz - 16 Bit/"
    };

    // Set while a postprocessor-triggered run is queued, so importing fifty clips at once queues
    // one regeneration rather than fifty.
    private static bool regenerationQueued;

    /// <summary>
    /// Re-runs the generator whenever an audio file under Resources/Audio is added, moved or
    /// deleted. The audio-extension check is also what stops this from looping forever: the
    /// generator's own outputs land inside Assets/ and would otherwise re-trigger the import.
    /// </summary>
    private class Postprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            bool audioChanged =
                importedAssets.Any(IsAudioAssetPath) ||
                deletedAssets.Any(IsAudioAssetPath) ||
                movedAssets.Any(IsAudioAssetPath) ||
                movedFromAssetPaths.Any(IsAudioAssetPath);

            if (!audioChanged || regenerationQueued)
            {
                return;
            }

            // Deferred rather than run inline: Generate() writes assets and calls
            // AssetDatabase.Refresh, which must not happen while the import that triggered this
            // callback is still in flight.
            regenerationQueued = true;

            EditorApplication.delayCall += () =>
            {
                regenerationQueued = false;
                Generate();
            };
        }
    }

    [MenuItem("Tools/Audio/Regenerate SFXType")]
    public static void Generate()
    {
        if (!Directory.Exists(AudioFolderPath))
        {
            Debug.LogError($"[SFXTypeGenerator] Audio folder not found at {AudioFolderPath}. Nothing generated.");
            return;
        }

        List<string> clipPaths = CollectClipPaths();

        if (clipPaths.Count == 0)
        {
            Debug.LogWarning($"[SFXTypeGenerator] No audio clips found under {AudioFolderPath}. Nothing generated.");
            return;
        }

        List<string> memberNames = BuildEnumMemberNames(clipPaths);

        bool manifestWritten = WriteIfChanged(ManifestAssetPath, BuildManifestText(clipPaths));
        bool enumWritten = WriteIfChanged(EnumAssetPath, BuildEnumText(clipPaths, memberNames));

        if (!manifestWritten && !enumWritten)
        {
            return;
        }

        if (manifestWritten)
        {
            AssetDatabase.ImportAsset(ManifestAssetPath);
        }

        AssetDatabase.Refresh();

        Debug.Log($"[SFXTypeGenerator] Wrote {clipPaths.Count} audio clip paths to {ManifestAssetPath} and regenerated {EnumName} at {EnumAssetPath}.");
    }

    /// <summary>
    /// Every importable audio file under Audio/, as Resources-relative paths with no extension
    /// (e.g. "Audio/Sound Effects/Misc/Splash") — the exact form AudioClipList.getAudioClip wants.
    /// Sorted ordinally so reruns and other machines produce identical output; without a stable
    /// order the enum ordinals would shuffle and the manifest's line alignment would silently start
    /// pointing at the wrong clips.
    /// </summary>
    private static List<string> CollectClipPaths()
    {
        return Directory.GetFiles(AudioFolderPath, "*", SearchOption.AllDirectories)
            .Select(path => path.Replace('\\', '/'))
            .Where(path => AudioExtensions.Contains(Path.GetExtension(path)))
            .Select(ToResourcesRelativePath)
            .Where(path => !IsExcluded(path))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// One enum member name per clip path, positionally matched to <paramref name="clipPaths"/>.
    /// Unique file names stay bare (Splash); names shared by several folders take their folder as a
    /// prefix (MaleHuman_Death, Cave_FS1), since an enum can't hold duplicate members.
    /// </summary>
    private static List<string> BuildEnumMemberNames(List<string> clipPaths)
    {
        List<string> fileNames = clipPaths
            .Select(path => SanitizeIdentifier(Path.GetFileName(path)))
            .ToList();

        Dictionary<string, int> fileNameCounts = new Dictionary<string, int>(StringComparer.Ordinal);

        foreach (string fileName in fileNames)
        {
            fileNameCounts.TryGetValue(fileName, out int count);
            fileNameCounts[fileName] = count + 1;
        }

        List<string> memberNames = new List<string>(clipPaths.Count);

        // Seeded with the reserved names so a clip file literally called NoSFX.wav is renamed
        // rather than silently duplicating the sentinel.
        HashSet<string> usedNames = new HashSet<string>(ReservedMemberNames, StringComparer.Ordinal);

        for (int i = 0; i < clipPaths.Count; i++)
        {
            string memberName = fileNames[i];

            if (fileNameCounts[memberName] > 1)
            {
                string folderName = SanitizeIdentifier(Path.GetFileName(Path.GetDirectoryName(clipPaths[i])));
                memberName = folderName + "_" + memberName;
            }

            memberNames.Add(MakeUnique(memberName, usedNames));
        }

        return memberNames;
    }

    /// <summary>
    /// Strips everything that can't appear in a C# identifier — spaces, dashes and parens, so
    /// "Camp Interior (Old)" becomes "CampInteriorOld" — and guards against a leading digit.
    /// </summary>
    private static string SanitizeIdentifier(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return "_";
        }

        StringBuilder sanitized = new StringBuilder(name.Length);

        foreach (char character in name)
        {
            if (char.IsLetterOrDigit(character) || character == '_')
            {
                sanitized.Append(character);
            }
        }

        if (sanitized.Length == 0)
        {
            return "_";
        }

        if (char.IsDigit(sanitized[0]))
        {
            sanitized.Insert(0, '_');
        }

        return sanitized.ToString();
    }

    /// <summary>
    /// Last-resort deduplication for names that still collide after the folder prefix. Not expected
    /// for the current asset set, but it keeps the generated file compiling whatever lands in Audio/.
    /// </summary>
    private static string MakeUnique(string memberName, HashSet<string> usedNames)
    {
        if (usedNames.Add(memberName))
        {
            return memberName;
        }

        int suffix = 2;

        while (!usedNames.Add(memberName + "_" + suffix))
        {
            suffix++;
        }

        return memberName + "_" + suffix;
    }

    private static string BuildManifestText(List<string> clipPaths)
    {
        return string.Join("\n", clipPaths) + "\n";
    }

    private static string BuildEnumText(List<string> clipPaths, List<string> memberNames)
    {
        StringBuilder generated = new StringBuilder();

        generated.Append("// AUTO-GENERATED BY SFXTypeGenerator. DO NOT EDIT.\n");
        generated.Append("// Regenerate with Tools > Audio > Regenerate SFXType.\n");
        generated.Append($"// The first {ReservedMemberNames.Length} member(s) are reserved and have no clip; after them,\n");
        generated.Append("// member order matches the line order of Assets/Resources/AudioClipFilePaths.txt.\n");
        generated.Append("\n");
        generated.Append($"public enum {EnumName}\n");
        generated.Append("{\n");

        foreach (string reservedName in ReservedMemberNames)
        {
            generated.Append($"    {reservedName}, // reserved: no audio clip\n");
        }

        for (int i = 0; i < memberNames.Count; i++)
        {
            generated.Append($"    {memberNames[i]}, // {clipPaths[i]}\n");
        }

        generated.Append("}\n");

        return generated.ToString();
    }

    /// <summary>
    /// Skips the write when nothing changed, so an unrelated audio reimport doesn't touch
    /// SFXType.cs and force a script recompile.
    /// </summary>
    private static bool WriteIfChanged(string assetPath, string contents)
    {
        if (File.Exists(assetPath) && File.ReadAllText(assetPath) == contents)
        {
            return false;
        }

        string directory = Path.GetDirectoryName(assetPath);

        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(assetPath, contents);

        return true;
    }

    private static string ToResourcesRelativePath(string assetPath)
    {
        string withoutExtension = assetPath.Substring(0, assetPath.Length - Path.GetExtension(assetPath).Length);

        return withoutExtension.Substring(ResourcesPrefix.Length);
    }

    private static bool IsExcluded(string resourcesRelativePath)
    {
        return ExcludedFolders.Any(folder => resourcesRelativePath.StartsWith(folder, StringComparison.Ordinal));
    }

    private static bool IsAudioAssetPath(string assetPath)
    {
        string normalizedPath = assetPath.Replace('\\', '/');

        return normalizedPath.StartsWith(AudioFolderPath + "/", StringComparison.Ordinal)
            && AudioExtensions.Contains(Path.GetExtension(normalizedPath));
    }
}
#endif
