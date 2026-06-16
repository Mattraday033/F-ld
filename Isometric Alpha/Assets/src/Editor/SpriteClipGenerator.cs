#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

public static class SpriteClipGenerator
{
    // Default sample rate for generated clips. Animancer-driven clips don't rely on
    // frame rate for playback speed, but a sane value keeps the Animation window usable.
    private const float DefaultFrameRate = 60f;

    // Cached set of tempAnimationType names so each imported asset is a cheap O(1) lookup.
    private static readonly HashSet<string> tempAnimationNames =
        new HashSet<string>(AnimationManager.tempAnimationTypes.Select(type => type.ToString()));

    // Records every sprite path we've applied a preset to, so the preset is applied exactly
    // once. Lives under Library/ (not Assets/) so writing it doesn't trigger another import,
    // and so it stays out of source control. Without this, every re-import — including the
    // one Unity triggers when you edit import settings — would re-stamp the preset and revert
    // your manual changes.
    private static readonly string PresetLogPath =
        Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Library", "SpriteClipGeneratorPresetLog.txt"));

    /// <summary>
    /// Listens for asset imports and generates a matching .anim whenever a sprite sheet
    /// is imported whose name matches one of AnimationManager.tempAnimationTypes.
    /// </summary>
    private class Postprocessor : AssetPostprocessor
    {
        // Runs before the texture is sliced, so any matching import preset (e.g. a
        // sprite-sheet slicing preset) takes effect on the sprites the clip is built from.
        private void OnPreprocessTexture()
        {
            TryApplyImportPreset(assetPath, assetImporter);
        }

        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (string assetPath in importedAssets)
            {
                TryGenerateClipForSpriteSheet(assetPath);
            }
        }
    }

    /// <summary>
    /// If the asset at <paramref name="assetPath"/> is a sprite sheet whose name matches
    /// one of AnimationManager.tempAnimationTypes, creates a .anim of the same name beside it.
    /// </summary>
    private static void TryGenerateClipForSpriteSheet(string assetPath)
    {
        if (!IsSpriteSheet(assetPath))
        {
            return;
        }

        string spriteSheetName = Path.GetFileNameWithoutExtension(assetPath);

        if (!tempAnimationNames.Contains(spriteSheetName))
        {
            return;
        }

        string directory = Path.GetDirectoryName(assetPath);
        string clipPath = Path.Combine(directory, spriteSheetName + ".anim").Replace('\\', '/');

        // Don't clobber an existing clip (also prevents re-generating on every re-import).
        if (File.Exists(clipPath))
        {
            return;
        }

        CreateClip(assetPath, clipPath);
    }

    /// <summary>Returns true when the asset is a texture imported as a sprite sheet.</summary>
    private static bool IsSpriteSheet(string assetPath)
    {
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

        return importer != null && importer.textureType == TextureImporterType.Sprite;
    }

    /// <summary>
    /// If a Preset asset exists whose name matches the sprite sheet and which can be applied
    /// to the given importer (i.e. it targets the appropriate importer type), applies it so
    /// the import settings — including any sprite slicing — are in place before the clip is built.
    /// </summary>
    private static void TryApplyImportPreset(string assetPath, AssetImporter importer)
    {
        if (importer == null)
        {
            return;
        }

        string spriteSheetName = Path.GetFileNameWithoutExtension(assetPath);

        if (!tempAnimationNames.Contains(spriteSheetName))
        {
            return;
        }

        // Apply the preset only the first time we ever see this sprite. After that the artist
        // owns the import settings — re-imports must not stomp manual changes back to the preset.
        if (HasPresetAlreadyBeenApplied(assetPath))
        {
            return;
        }

        Preset preset = FindMatchingPreset(spriteSheetName, importer);

        if (preset == null)
        {
            return;
        }

        preset.ApplyTo(importer);
        RecordPresetApplied(assetPath);
    }

    /// <summary>
    /// Returns true if a preset has previously been applied to the sprite at
    /// <paramref name="assetPath"/>, according to the preset log file.
    /// </summary>
    private static bool HasPresetAlreadyBeenApplied(string assetPath)
    {
        return File.Exists(PresetLogPath) &&
               File.ReadLines(PresetLogPath).Contains(assetPath);
    }

    /// <summary>Appends the sprite path to the preset log so it is never re-stamped.</summary>
    private static void RecordPresetApplied(string assetPath)
    {
        File.AppendAllText(PresetLogPath, assetPath + "\n");
    }

    /// <summary>
    /// Finds a Preset asset named exactly <paramref name="presetName"/> that can be applied to
    /// <paramref name="target"/> (matching the appropriate target type), or null if none exists.
    /// </summary>
    private static Preset FindMatchingPreset(string presetName, UnityEngine.Object target)
    {
        foreach (string guid in AssetDatabase.FindAssets($"t:Preset {presetName}"))
        {
            string presetPath = AssetDatabase.GUIDToAssetPath(guid);

            // FindAssets matches names fuzzily; require an exact name match against the sheet.
            if (Path.GetFileNameWithoutExtension(presetPath) != presetName)
            {
                continue;
            }

            Preset preset = AssetDatabase.LoadAssetAtPath<Preset>(presetPath);

            if (preset != null && preset.CanBeAppliedTo(target))
            {
                return preset;
            }
        }

        return null;
    }

    // The curve binding used when no preset dictates one: the SpriteRenderer's sprite.
    private static readonly EditorCurveBinding DefaultSpriteBinding = new EditorCurveBinding
    {
        type = typeof(SpriteRenderer),
        path = string.Empty,
        propertyName = "m_Sprite"
    };

    /// <summary>
    /// Creates a new AnimationClip at <paramref name="clipPath"/> and populates it with the
    /// sprite frames found in the sheet at <paramref name="spriteSheetPath"/>. If a matching
    /// clip preset exists, its sprite-swap timing is mirrored (using the freshly imported
    /// sprites in place of the preset's own); otherwise the frames are spaced evenly.
    /// </summary>
    private static void CreateClip(string spriteSheetPath, string clipPath)
    {
        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheetPath)
                                        .OfType<Sprite>()
                                        .OrderBy(sprite => sprite.name, new NaturalNameComparer())
                                        .ToArray();

        AnimationClip clip = new AnimationClip { frameRate = DefaultFrameRate };

        if (!TryApplyClipPreset(clip, clipPath, sprites))
        {
            // No preset to mirror — fall back to evenly spaced frames on a SpriteRenderer.
            SetSpriteCurve(clip, DefaultSpriteBinding, sprites, EvenlySpacedTimes(sprites.Length, clip.frameRate));
        }

        AssetDatabase.CreateAsset(clip, clipPath);
        AssetDatabase.SaveAssets();

        Debug.Log($"[SpriteClipGenerator] Generated animation clip '{clipPath}' from sprite sheet '{spriteSheetPath}'.");
    }

    /// <summary>
    /// Applies a matching AnimationClip preset to <paramref name="clip"/> — at most once per clip
    /// path, tracked in the same preset log as sprite sheets — then rebinds the preset's sprite
    /// curve so it shows the freshly imported <paramref name="sprites"/> at the preset's existing
    /// keyframe times. Returns false when there is no matching preset, or one was already applied.
    /// </summary>
    private static bool TryApplyClipPreset(AnimationClip clip, string clipPath, Sprite[] sprites)
    {
        if (HasPresetAlreadyBeenApplied(clipPath))
        {
            return false;
        }

        Preset preset = FindMatchingPreset(Path.GetFileNameWithoutExtension(clipPath), clip);

        if (preset == null)
        {
            return false;
        }

        preset.ApplyTo(clip);

        if (TryGetSpriteBinding(clip, out EditorCurveBinding binding))
        {
            // Keep the preset's timing AND its sprite-reuse pattern, swapping in the new sheet's
            // sprites. A preset frame that repeats an earlier sprite (e.g. an 8th frame that
            // reuses the 1st sprite) repeats the matching new sprite at the same slot.
            ObjectReferenceKeyframe[] presetKeyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);

            AnimationUtility.SetObjectReferenceCurve(clip, binding, RemapSpritesPreservingReuse(presetKeyframes, sprites, clip.name));
        }
        else
        {
            // Preset carried no sprite curve to mirror; space the frames evenly instead.
            SetSpriteCurve(clip, DefaultSpriteBinding, sprites, EvenlySpacedTimes(sprites.Length, clip.frameRate));
        }

        RecordPresetApplied(clipPath);
        return true;
    }

    /// <summary>Finds the sprite-swap curve binding on the clip, if one exists.</summary>
    private static bool TryGetSpriteBinding(AnimationClip clip, out EditorCurveBinding spriteBinding)
    {
        foreach (EditorCurveBinding binding in AnimationUtility.GetObjectReferenceCurveBindings(clip))
        {
            if (binding.propertyName == "m_Sprite")
            {
                spriteBinding = binding;
                return true;
            }
        }

        spriteBinding = default;
        return false;
    }

    /// <summary>
    /// Writes an object-reference curve that shows <paramref name="sprites"/> at the given
    /// <paramref name="times"/>, paired by index (bounded by the shorter of the two).
    /// </summary>
    private static void SetSpriteCurve(AnimationClip clip, EditorCurveBinding binding, Sprite[] sprites, float[] times)
    {
        int count = Mathf.Min(sprites.Length, times.Length);

        if (count <= 0)
        {
            return;
        }

        if (sprites.Length != times.Length)
        {
            Debug.LogWarning($"[SpriteClipGenerator] Sprite count ({sprites.Length}) and keyframe time count ({times.Length}) differ for '{clip.name}'; pairing the first {count}.");
        }

        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[count];

        for (int i = 0; i < count; i++)
        {
            keyframes[i] = new ObjectReferenceKeyframe
            {
                time = times[i],
                value = sprites[i]
            };
        }

        AnimationUtility.SetObjectReferenceCurve(clip, binding, keyframes);
    }

    /// <summary>
    /// Rebuilds <paramref name="presetKeyframes"/> so each keyframe keeps its time but shows a
    /// sprite from <paramref name="sprites"/>. Distinct preset sprites are assigned to new sprites
    /// in order of first appearance, so whenever the preset repeats an earlier sprite the result
    /// repeats the same new sprite — preserving holds/loops instead of dropping or misaligning them.
    /// </summary>
    private static ObjectReferenceKeyframe[] RemapSpritesPreservingReuse(
        ObjectReferenceKeyframe[] presetKeyframes, Sprite[] sprites, string clipName)
    {
        Dictionary<UnityEngine.Object, int> slotByPresetSprite = new Dictionary<UnityEngine.Object, int>();
        List<ObjectReferenceKeyframe> keyframes = new List<ObjectReferenceKeyframe>(presetKeyframes.Length);

        foreach (ObjectReferenceKeyframe presetKeyframe in presetKeyframes)
        {
            if (presetKeyframe.value == null)
            {
                continue;
            }

            // A "Blank" sprite is an intentional gap in the preset (e.g. a hold on nothing).
            // Keep the preset's own value untouched and don't let it consume a new-sprite slot,
            // so it neither gets overridden nor shifts the remapping of the real frames.
            if (presetKeyframe.value.name.IndexOf("Blank", System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                keyframes.Add(presetKeyframe);
                continue;
            }

            // First time we see a given preset sprite, it claims the next new-sprite slot;
            // a repeat of that preset sprite resolves to the same slot again.
            if (!slotByPresetSprite.TryGetValue(presetKeyframe.value, out int slot))
            {
                slot = slotByPresetSprite.Count;
                slotByPresetSprite.Add(presetKeyframe.value, slot);
            }

            if (slot >= sprites.Length)
            {
                Debug.LogWarning($"[SpriteClipGenerator] '{clipName}' preset references more distinct sprites ({slotByPresetSprite.Count}) than the sheet provides ({sprites.Length}); skipping keyframe at {presetKeyframe.time}s.");
                continue;
            }

            keyframes.Add(new ObjectReferenceKeyframe
            {
                time = presetKeyframe.time,
                value = sprites[slot]
            });
        }

        return keyframes.ToArray();
    }

    /// <summary>Returns <paramref name="count"/> keyframe times spaced one frame apart.</summary>
    private static float[] EvenlySpacedTimes(int count, float frameRate)
    {
        float[] times = new float[count];

        for (int i = 0; i < count; i++)
        {
            times[i] = i / frameRate;
        }

        return times;
    }

    /// <summary>
    /// Orders sprite names so that "Frame_2" sorts before "Frame_10" (numeric-aware),
    /// keeping generated keyframes in the order an artist would expect.
    /// </summary>
    private class NaturalNameComparer : IComparer<string>
    {
        public int Compare(string left, string right)
        {
            if (left == right)
            {
                return 0;
            }

            string[] leftChunks = Regex.Split(left ?? string.Empty, "([0-9]+)");
            string[] rightChunks = Regex.Split(right ?? string.Empty, "([0-9]+)");

            int count = Mathf.Min(leftChunks.Length, rightChunks.Length);

            for (int i = 0; i < count; i++)
            {
                if (leftChunks[i] == rightChunks[i])
                {
                    continue;
                }

                if (int.TryParse(leftChunks[i], out int leftNumber) &&
                    int.TryParse(rightChunks[i], out int rightNumber))
                {
                    return leftNumber.CompareTo(rightNumber);
                }

                return string.Compare(leftChunks[i], rightChunks[i], System.StringComparison.Ordinal);
            }

            return leftChunks.Length.CompareTo(rightChunks.Length);
        }
    }
}
#endif
