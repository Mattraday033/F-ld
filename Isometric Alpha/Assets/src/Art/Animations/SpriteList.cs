using UnityEngine;

/// <summary>
/// Every sprite under Resources/Sprites/SpriteLayers, keyed by SpritePath. Filled from the generated
/// manifest; see SpritePathGenerator.
///
/// A key stands for one texture, not one sprite: a sliced sheet (Body_LovashiArmor_Run_Front)
/// answers with all of its frames in order, an unsliced image (Hair_SpearmanHair) answers with the
/// one. That is why this list is built with loadAll where AudioClipList and InkAssetList take one
/// asset per manifest line.
///
/// Never persist a SpritePath by its ordinal - in a save file, or in a [SerializeField] on a prefab
/// or scene component. Members are ordered by the manifest, which is sorted by path, so adding one
/// sprite renumbers every member after it and a stored number silently comes back as a different
/// sprite. Persist the member name or the manifest path string instead, the way
/// SaveBlueprint.playerSpriteName already does, and resolve it with Enum.TryParse.
/// </summary>
public static class SpriteList
{

    private const int reservedSpritePathCount = 1;
    public const string spriteLayerFilePathsFileName = "SpriteLayerFilePaths";

    private readonly static Sprite blankTexture = Resources.Load<Sprite>(PrefabNames.blankTexture);
    private readonly static Sprite[] blankTextureArray = new Sprite[]{ Resources.Load<Sprite>(PrefabNames.blankTexture) };

    private readonly static ResourceList<SpritePath, Sprite> sprites =
        new ResourceList<SpritePath, Sprite>(spriteLayerFilePathsFileName,
                                             reservedSpritePathCount,
                                             "[SpriteList]",
                                             "Tools > Sprites > Regenerate SpritePath",
                                             loadAll: true);

    /// <summary>
    /// Statics survive between play sessions with domain reload disabled, so last session's sprites
    /// have to be dropped rather than handed out as references to objects Unity already unloaded.
    ///
    /// Self-hooked rather than called by an owning manager because sprite layers have no manager to
    /// hang it off - neither AnimationManager nor NewAnimationManager carries a
    /// [RuntimeInitializeOnLoadMethod] of its own, so hooking into one would be an arbitrary
    /// coupling. Ordering against the other initializers doesn't matter: ResourceList loads on first
    /// read anyway, so an initializer that reads sprites before this one runs still gets the right
    /// sprites, and this call then simply reloads them.
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        sprites.init();
    }

    /// <summary>
    /// Every frame of a sprite layer, in frame order - or an empty array for SpritePath.NoSprite,
    /// which is a legitimate "no layer here" value rather than a lookup failure and so does not log.
    /// </summary>
    public static Sprite[] getSprites(SpritePath spritePath)
    {
        if(spritePath == SpritePath.NoSprite)
        {
            return blankTextureArray;
        }

        Sprite[] spritesAtPath = sprites.getAssets(spritePath);

        if(spritesAtPath.Length <= 0)
        {
            return blankTextureArray;
        }

        return spritesAtPath;
    }

    /// <summary>
    /// The first frame of a sprite layer - the whole image, for the unsliced ones - or null for
    /// SpritePath.NoSprite.
    /// </summary>
    public static Sprite getSprite(SpritePath spritePath)
    {
        if(spritePath == SpritePath.NoSprite)
        {
            return blankTexture;
        }

        Sprite spriteAtPath = sprites.getAsset(spritePath);

        if(spriteAtPath == null)
        {
            return blankTexture;
        }

        return spriteAtPath;
    }
}
