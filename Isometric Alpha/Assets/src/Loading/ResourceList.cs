using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loads every asset of one type out of Resources and keys it by a generated enum, using the
/// manifest/enum pair that ResourceEnumGenerator writes. Line N of the manifest is enum member
/// N + reservedKeyCount, so no name parsing is needed to rebuild the mapping at runtime.
///
/// One instance per asset type, held by a thin static wrapper (AudioClipList, InkAssetList).
/// The wrapper is what game code talks to; this class only owns the loading and the validation.
/// </summary>
public class ResourceList<TKey, TAsset> where TKey : struct, Enum where TAsset : UnityEngine.Object
{

    private readonly static string[] lineSeperators = new string[] { "\r\n", "\n", "\r" };

    private readonly Dictionary<TKey, TAsset> assets = new Dictionary<TKey, TAsset>();

    // Resources path of the generated manifest, with no extension - the form Resources.Load wants.
    private readonly string manifestFileName;

    // Enum members emitted before the first asset, so default(TKey) means "nothing". They have no
    // manifest line, which is what shifts every asset by this much.
    private readonly int reservedKeyCount;

    private readonly string logPrefix;
    private readonly string regenerateHint;

    private bool initialized;

    public ResourceList(string manifestFileName, int reservedKeyCount, string logPrefix, string regenerateHint)
    {
        this.manifestFileName = manifestFileName;
        this.reservedKeyCount = reservedKeyCount;
        this.logPrefix = logPrefix;
        this.regenerateHint = regenerateHint;
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
            assets[toKey(i + reservedKeyCount)] = Resources.Load<TAsset>(assetPaths[i].Trim());
        }
    }

    public TAsset getAsset(TKey key)
    {
        // Reserved members are a legitimate "nothing here" value, not a lookup failure, so they
        // must not log - and answering them needs no dictionary, so this comes before the load.
        if(Convert.ToInt32(key) < reservedKeyCount)
        {
            return null;
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
            return null;
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
