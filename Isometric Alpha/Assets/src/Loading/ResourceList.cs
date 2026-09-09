using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Loads every asset of one type out of Resources and keys it by a generated enum, using the
/// manifest/enum pair that ResourceEnumGenerator writes. Line N of the manifest is enum member
/// N + reservedKeyCount, so no name parsing is needed to rebuild the mapping at runtime.
///
/// One instance per asset type, held by a thin static wrapper (AudioClipList, InkAssetList).
/// The wrapper is what game code talks to; this class only owns the loading and the validation.
///
/// A manifest line can stand for one asset or for several - a sliced sprite sheet is one line and N
/// frames - so the mapping always holds arrays. Lists that were never sliced hold one-element arrays
/// and getAsset() unwraps them, which is why AudioClipList and InkAssetList are untouched by this.
/// </summary>
public class ResourceList<TKey, TAsset> where TKey : struct, Enum where TAsset : UnityEngine.Object
{

    private readonly static string[] lineSeperators = new string[] { "\r\n", "\n", "\r" };

    private readonly Dictionary<TKey, TAsset[]> assets = new Dictionary<TKey, TAsset[]>();

    // Resources path of the generated manifest, with no extension - the form Resources.Load wants.
    private readonly string manifestFileName;

    // Enum members emitted before the first asset, so default(TKey) means "nothing". They have no
    // manifest line, which is what shifts every asset by this much.
    private readonly int reservedKeyCount;

    private readonly string logPrefix;
    private readonly string regenerateHint;

    // True when one manifest line means every sub-asset at that path rather than the single asset
    // Resources.Load would pick - sprite sheets, where the frames are the whole point.
    private readonly bool loadAll;

    private bool initialized;

    public ResourceList(string manifestFileName, int reservedKeyCount, string logPrefix, string regenerateHint, bool loadAll = false)
    {
        this.manifestFileName = manifestFileName;
        this.reservedKeyCount = reservedKeyCount;
        this.logPrefix = logPrefix;
        this.regenerateHint = regenerateHint;
        this.loadAll = loadAll;
    }

    /// <summary>
    /// Rebuilds the mapping from the manifest. Safe to call repeatedly; callers that only read
    /// don't need to call it at all, since getAsset() initializes on first use.
    /// </summary>
    public void init()
    {
        // Statics survive between play sessions when domain reload is disabled, so clear rather
        // than trust these to be empty. Stale references would point at objects the previous
        // session already unloaded.
        assets.Clear();

        // Set before the early returns: a missing or stale manifest is a permanent condition, and
        // retrying the failed load on every single getAsset() call would bury the console.
        initialized = true;

        TextAsset manifest = Resources.Load<TextAsset>(manifestFileName);

        if(manifest == null)
        {
            Debug.LogError($"{logPrefix} Could not load {manifestFileName} from Resources. Run {regenerateHint}.");
            return;
        }

        string[] assetPaths = manifest.text.Split(lineSeperators, StringSplitOptions.RemoveEmptyEntries);

        int expectedPathCount = Enum.GetValues(typeof(TKey)).Length - reservedKeyCount;

        if(assetPaths.Length != expectedPathCount)
        {
            Debug.LogError($"{logPrefix} {manifestFileName} has {assetPaths.Length} paths but {typeof(TKey).Name} expects {expectedPathCount}. Run {regenerateHint}.");
            return;
        }

        for(int i = Constants.indexZero; i < assetPaths.Length; i++)
        {
            assets[toKey(i + reservedKeyCount)] = loadAssets(assetPaths[i].Trim());
        }
    }

    /// <summary>
    /// The assets behind one manifest line.
    ///
    /// The single-asset case deliberately wraps whatever Resources.Load returned - null included -
    /// instead of collapsing a failed load to an empty array, so getAsset() still answers null for a
    /// path that didn't resolve and still doesn't log for it, exactly as it did before this class
    /// held arrays.
    ///
    /// The loadAll case is sorted by name because Unity documents no order for sub-assets: an
    /// animation whose frames arrive in import order is an animation that can silently reverse
    /// itself the next time the sheet is re-sliced. NaturalNameComparer rather than a plain string
    /// sort is what keeps Run_Back_2 ahead of Run_Back_10, and it is the same comparer
    /// SpriteClipGenerator lays out a generated .anim's keyframes with, so a clip and a getAssets()
    /// call agree about frame order.
    /// </summary>
    private TAsset[] loadAssets(string assetPath)
    {
        if(!loadAll)
        {
            return new TAsset[] { Resources.Load<TAsset>(assetPath) };
        }

        // LoadAll omits anything it fails to load rather than returning a null entry, so reading
        // .name off each result is safe.
        return Resources.LoadAll<TAsset>(assetPath)
                        .OrderBy(asset => asset.name, NaturalNameComparer.instance)
                        .ToArray();
    }

    /// <summary>
    /// The single asset behind a key - for a line that loaded several, its first. Null for a
    /// reserved key, for a path that failed to load, and for a line that produced nothing.
    /// </summary>
    public TAsset getAsset(TKey key)
    {
        TAsset[] loaded = getAssets(key);

        return loaded.Length > Constants.indexZero ? loaded[Constants.indexZero] : null;
    }

    /// <summary>
    /// Every asset behind a key, in name order. Empty rather than null for a reserved key or a
    /// lookup failure, so callers can foreach the result unguarded. The array is the list's own -
    /// callers must not sort into it or overwrite its elements.
    /// </summary>
    public TAsset[] getAssets(TKey key)
    {
        // Reserved members are a legitimate "nothing here" value, not a lookup failure, so they
        // must not log - and answering them needs no dictionary, so this comes before the load.
        if(Convert.ToInt32(key) < reservedKeyCount)
        {
            return Array.Empty<TAsset>();
        }

        // init() is the reset that a new play session needs; this covers callers that run before
        // it, notably static field initializers in the *List classes, which execute ahead of every
        // [RuntimeInitializeOnLoadMethod].
        if(!initialized)
        {
            init();
        }

        if(!assets.ContainsKey(key))
        {
            Debug.LogError($"{logPrefix} No asset for {typeof(TKey).Name}.{key}. Run {regenerateHint}.");
            return Array.Empty<TAsset>();
        }

        return assets[key];
    }

    /// <summary>
    /// Boxed conversion rather than a cast: TKey is an unbounded generic, so (TKey)(object)ordinal
    /// is the only conversion the compiler will accept here.
    /// </summary>
    private static TKey toKey(int ordinal)
    {
        return (TKey) Enum.ToObject(typeof(TKey), ordinal);
    }
}
