#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Everything one asset type needs to be turned into a generated manifest/enum pair. Each concrete
/// generator (SFXTypeGenerator, DialogueKeyGenerator) supplies one of these and does nothing else.
/// </summary>
public class ResourceEnumDefinition
{
    // Folder to walk, as an Assets-relative path. Must live under Assets/Resources/.
    public string SourceFolderPath;

    // Must be .txt or Unity won't import it as a TextAsset, and Resources.Load would return null.
    public string ManifestAssetPath;

    public string EnumAssetPath;
    public string EnumName;

    // Used for the [Prefix] on log lines and in the generated file's DO NOT EDIT banner.
    public string GeneratorName;

    // Menu path, quoted back to the user in every error the runtime ResourceList can raise.
    public string RegenerateHint;

    // Members emitted before any asset, so default(enum) means "nothing" and callers have a value
    // to represent absence. These have no asset behind them and therefore no manifest line, which
    // shifts every asset one place per reserved member: manifest line N is enum member
    // N + ReservedMemberNames.Length. The matching runtime reservedKeyCount must equal this length.
    public string[] ReservedMemberNames = Array.Empty<string>();

    // Extensions Unity actually imports as the target asset type. Filtering on these drops .meta
    // files and anything Unity cannot import, which must not enter the enum.
    public HashSet<string> Extensions;

    // Folders under SourceFolderPath that exist on disk but aren't part of the game.
    public string[] ExcludedFolders = Array.Empty<string>();

    // Authored source files whose importable output is what actually gets collected - .ink next to
    // the compiled .json. Any of these without a collected sibling is reported, since an
    // uncompiled source would otherwise drop out of the enum with no indication.
    public string[] CompanionSourceExtensions = Array.Empty<string>();
}

/// <summary>
/// Walks a folder under Assets/Resources and derives two generated files from what it finds: a
/// manifest of Resources-relative paths, and an enum with one member per asset. Line N of the
/// manifest corresponds to enum member N, which is what lets ResourceList.init() build its
/// dictionary without having to parse names back out of the paths.
/// </summary>
public static class ResourceEnumGenerator
{
    private const string ResourcesPrefix = "Assets/Resources/";

    /// <summary>
    /// Every asset type wired into this system. The postprocessor iterates these, so a new
    /// generator only becomes automatic once it is listed here.
    /// </summary>
    public static IEnumerable<ResourceEnumDefinition> Definitions
    {
        get
        {
            yield return SFXTypeGenerator.Definition;
            yield return DialogueKeyGenerator.Definition;
        }
    }

    // Definitions with a postprocessor-triggered run already queued, so importing fifty assets at
    // once queues one regeneration rather than fifty.
    private static readonly HashSet<string> queuedDefinitions = new HashSet<string>(StringComparer.Ordinal);

    /// <summary>
    /// Re-runs the affected generator whenever a relevant file is added, moved or deleted. The
    /// extension check is also what stops this from looping forever: a generator's own outputs land
    /// inside Assets/ and would otherwise re-trigger the import.
    /// </summary>
    private class Postprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (ResourceEnumDefinition definition in Definitions)
            {
                bool assetsChanged =
                    importedAssets.Any(path => IsRelevantAssetPath(definition, path)) ||
                    deletedAssets.Any(path => IsRelevantAssetPath(definition, path)) ||
                    movedAssets.Any(path => IsRelevantAssetPath(definition, path)) ||
                    movedFromAssetPaths.Any(path => IsRelevantAssetPath(definition, path));

                if (!assetsChanged || !queuedDefinitions.Add(definition.EnumName))
                {
                    continue;
                }

                // Deferred rather than run inline: Generate() writes assets and calls
                // AssetDatabase.Refresh, which must not happen while the import that triggered this
                // callback is still in flight.
                ResourceEnumDefinition queued = definition;

                EditorApplication.delayCall += () =>
                {
                    queuedDefinitions.Remove(queued.EnumName);
                    Generate(queued);
                };
            }
        }
    }

    public static void Generate(ResourceEnumDefinition definition)
    {
        if (!Directory.Exists(definition.SourceFolderPath))
        {
            Debug.LogError($"[{definition.GeneratorName}] Source folder not found at {definition.SourceFolderPath}. Nothing generated.");
            return;
        }

        List<string> assetPaths = CollectAssetPaths(definition);

        if (assetPaths.Count == 0)
        {
            Debug.LogWarning($"[{definition.GeneratorName}] No assets found under {definition.SourceFolderPath}. Nothing generated.");
            return;
        }

        ReportMissingCompanions(definition, assetPaths);

        List<string> memberNames = BuildEnumMemberNames(definition, assetPaths);

        bool manifestWritten = WriteIfChanged(definition.ManifestAssetPath, BuildManifestText(assetPaths), writeByteOrderMark: false);
        bool enumWritten = WriteIfChanged(definition.EnumAssetPath, BuildEnumText(definition, assetPaths, memberNames), writeByteOrderMark: true);

        if (!manifestWritten && !enumWritten)
        {
            return;
        }

        if (manifestWritten)
        {
            AssetDatabase.ImportAsset(definition.ManifestAssetPath);
        }

        AssetDatabase.Refresh();

        Debug.Log($"[{definition.GeneratorName}] Wrote {assetPaths.Count} asset paths to {definition.ManifestAssetPath} and regenerated {definition.EnumName} at {definition.EnumAssetPath}.");
    }

    /// <summary>
    /// Every importable asset under the source folder, as Resources-relative paths with no
    /// extension (e.g. "Audio/Sound Effects/Misc/Splash") - the exact form Resources.Load wants.
    /// Sorted ordinally so reruns and other machines produce identical output; without a stable
    /// order the enum ordinals would shuffle and the manifest's line alignment would silently start
    /// pointing at the wrong assets.
    /// </summary>
    private static List<string> CollectAssetPaths(ResourceEnumDefinition definition)
    {
        return Directory.GetFiles(definition.SourceFolderPath, "*", SearchOption.AllDirectories)
            .Select(path => path.Replace('\\', '/'))
            .Where(path => definition.Extensions.Contains(Path.GetExtension(path)))
            .Select(ToResourcesRelativePath)
            .Where(path => !IsExcluded(definition, path))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// Warns about authored sources whose importable output is missing - an .ink that never
    /// compiled to .json, say. Without this the story simply wouldn't appear in the enum, and the
    /// first sign of trouble would be a null TextAsset at runtime.
    /// </summary>
    private static void ReportMissingCompanions(ResourceEnumDefinition definition, List<string> assetPaths)
    {
        if (definition.CompanionSourceExtensions.Length == 0)
        {
            return;
        }

        HashSet<string> collected = new HashSet<string>(assetPaths, StringComparer.Ordinal);

        List<string> missing = Directory.GetFiles(definition.SourceFolderPath, "*", SearchOption.AllDirectories)
            .Select(path => path.Replace('\\', '/'))
            .Where(path => definition.CompanionSourceExtensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase))
            .Select(ToResourcesRelativePath)
            .Where(path => !IsExcluded(definition, path) && !collected.Contains(path))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToList();

        if (missing.Count == 0)
        {
            return;
        }

        Debug.LogWarning($"[{definition.GeneratorName}] {missing.Count} source file(s) have no importable output and are absent from {definition.EnumName}:\n{string.Join("\n", missing)}");
    }

    /// <summary>
    /// One enum member name per asset path, positionally matched to <paramref name="assetPaths"/>.
    /// Unique file names stay bare (Splash); names shared by several folders take their folder as a
    /// prefix (MaleHuman_Death, SECamp_Carter), since an enum can't hold duplicate members.
    /// </summary>
    private static List<string> BuildEnumMemberNames(ResourceEnumDefinition definition, List<string> assetPaths)
    {
        List<string> fileNames = assetPaths
            .Select(path => SanitizeIdentifier(Path.GetFileName(path)))
            .ToList();

        Dictionary<string, int> fileNameCounts = new Dictionary<string, int>(StringComparer.Ordinal);

        foreach (string fileName in fileNames)
        {
            fileNameCounts.TryGetValue(fileName, out int count);
            fileNameCounts[fileName] = count + 1;
        }

        List<string> memberNames = new List<string>(assetPaths.Count);

        // Seeded with the reserved names so an asset file literally called NoSFX is renamed rather
        // than silently duplicating the sentinel.
        HashSet<string> usedNames = new HashSet<string>(definition.ReservedMemberNames, StringComparer.Ordinal);

        for (int i = 0; i < assetPaths.Count; i++)
        {
            string memberName = fileNames[i];

            if (fileNameCounts[memberName] > 1)
            {
                string folderName = SanitizeIdentifier(Path.GetFileName(Path.GetDirectoryName(assetPaths[i])));
                memberName = folderName + "_" + memberName;
            }

            memberNames.Add(MakeUnique(memberName, usedNames));
        }

        return memberNames;
    }

    /// <summary>
    /// Strips everything that can't appear in a C# identifier - spaces, dashes and parens, so
    /// "Camp Interior (Old)" becomes "CampInteriorOld" - and guards against a leading digit.
    /// Accented letters are kept: they are legal in C# identifiers, which is why the enum file is
    /// written with a byte order mark.
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
    /// for the current asset sets, but it keeps the generated file compiling whatever lands in them.
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

    private static string BuildManifestText(List<string> assetPaths)
    {
        return string.Join("\n", assetPaths) + "\n";
    }

    private static string BuildEnumText(ResourceEnumDefinition definition, List<string> assetPaths, List<string> memberNames)
    {
        StringBuilder generated = new StringBuilder();

        generated.Append($"// AUTO-GENERATED BY {definition.GeneratorName}. DO NOT EDIT.\n");
        generated.Append($"// Regenerate with {definition.RegenerateHint}.\n");
        generated.Append($"// The first {definition.ReservedMemberNames.Length} member(s) are reserved and have no asset; after them,\n");
        generated.Append($"// member order matches the line order of {definition.ManifestAssetPath}.\n");
        generated.Append("\n");
        generated.Append($"public enum {definition.EnumName}\n");
        generated.Append("{\n");

        foreach (string reservedName in definition.ReservedMemberNames)
        {
            generated.Append($"    {reservedName}, // reserved: no asset\n");
        }

        for (int i = 0; i < memberNames.Count; i++)
        {
            generated.Append($"    {memberNames[i]}, // {assetPaths[i]}\n");
        }

        generated.Append("}\n");

        return generated.ToString();
    }

    /// <summary>
    /// Skips the write when nothing changed, so an unrelated reimport doesn't touch the generated
    /// enum and force a script recompile.
    ///
    /// The enum is written with a byte order mark so the compiler reads accented member names
    /// (Géza, László) as UTF-8. The manifest is written without one: TextAsset.text would hand the
    /// mark to the runtime as a leading character on line 0 and break that path's Resources.Load.
    /// </summary>
    private static bool WriteIfChanged(string assetPath, string contents, bool writeByteOrderMark)
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

        File.WriteAllText(assetPath, contents, new UTF8Encoding(writeByteOrderMark));

        return true;
    }

    private static string ToResourcesRelativePath(string assetPath)
    {
        string withoutExtension = assetPath.Substring(0, assetPath.Length - Path.GetExtension(assetPath).Length);

        return withoutExtension.Substring(ResourcesPrefix.Length);
    }

    private static bool IsExcluded(ResourceEnumDefinition definition, string resourcesRelativePath)
    {
        return definition.ExcludedFolders.Any(folder => resourcesRelativePath.StartsWith(folder, StringComparison.Ordinal));
    }

    private static bool IsRelevantAssetPath(ResourceEnumDefinition definition, string assetPath)
    {
        string normalizedPath = assetPath.Replace('\\', '/');

        return normalizedPath.StartsWith(definition.SourceFolderPath + "/", StringComparison.Ordinal)
            && definition.Extensions.Contains(Path.GetExtension(normalizedPath));
    }
}
#endif
