#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// Sprite layers' binding into ResourceEnumGenerator: every importable texture under
/// Resources/Sprites/SpriteLayers becomes a line in SpriteLayerFilePaths.txt and a member of
/// SpritePath. SpriteList reads the pair back at runtime.
///
/// Unlike SFXType and DialogueKey this definition opts into full-path member names. The leaf file
/// names here are animation types drawn from a small fixed vocabulary - OOC_Idle_Front, Run_Back -
/// that repeats across every body, face, hair and weapon variant by design, so naming a member after
/// the file alone would collide constantly, and the default collision fallback's single folder
/// prefix (LovashiArmor_Run_Back) throws away the layer that variant belongs to. The path is the
/// identity here, so the path is the name.
///
/// One member is one texture, not one sprite: a sliced sheet is a single member whose key answers
/// with every frame. That is SpriteList's half of the arrangement, via ResourceList's loadAll.
/// </summary>
public static class SpritePathGenerator
{
    public static readonly ResourceEnumDefinition Definition = new ResourceEnumDefinition
    {
        SourceFolderPath = "Assets/Resources/Sprites/SpriteLayers",
        ManifestAssetPath = "Assets/Resources/SpriteLayerFilePaths.txt",
        EnumAssetPath = "Assets/src/Art/SpritePath.cs",
        EnumName = "SpritePath",
        GeneratorName = "SpritePathGenerator",
        RegenerateHint = "Tools > Sprites > Regenerate SpritePath",

        // SpriteList.reservedSpritePathCount must match this length.
        ReservedMemberNames = new string[] { "NoSprite" },

        // Every texture format Unity imports through the TextureImporter, matched case-insensitively
        // - which is the only reason Run_Front.PNG sits beside Run_Back.png instead of dropping out
        // of both the enum and the postprocessor's change detection.
        //
        // .psb is deliberately absent: com.unity.2d.psdimporter is installed, so a .psb imports as a
        // prefab with sprite sub-assets rather than as a sprite sheet, and it does not belong in a
        // list whose contract is "this key is one texture's frames".
        Extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".png", ".jpg", ".jpeg", ".tga", ".tif", ".tiff", ".bmp", ".gif", ".exr", ".psd"
        },

        UseFullPathMemberNames = true
    };

    [MenuItem("Tools/Sprites/Regenerate SpritePath")]
    public static void Generate()
    {
        ResourceEnumGenerator.Generate(Definition);
    }
}
#endif
