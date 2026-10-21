using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CanvasPositionList
{
    //the top-left pixel of the head canvas inside each frame of a body sheet, counted from the frame's top-left.
    //Face and Hair are drawn on the same canvas, so one position per frame places both.
    //a body sheet with no entry, or a frame past the end of its entry, gets the default position for the way the sheet faces.
    //these place a 48x48 canvas. The position is the canvas's corner rather than the head's, so a canvas of another size needs its own
    private readonly static Dictionary<SpritePath, Vector2Int[]> headPositions = new Dictionary<SpritePath, Vector2Int[]>()
    {
        [SpritePath.Body_LovashiArmor_TwoHandedStab_Idle_Front] = new Vector2Int[]
        {
            new Vector2Int(60, 32), new Vector2Int(60, 33), new Vector2Int(60, 34), new Vector2Int(59, 32)
        },
        [SpritePath.Body_LovashiArmor_TwoHandedStab_Attack_Normal_Front] = new Vector2Int[]
        {
            new Vector2Int(59, 32), new Vector2Int(59, 32), new Vector2Int(51, 36),
            new Vector2Int(40, 45), new Vector2Int(51, 36), new Vector2Int(59, 32)
        },
        [SpritePath.Body_LovashiArmor_TwoHandedStab_Attack_Normal_Back] = new Vector2Int[]
        {
            new Vector2Int(56, 29), new Vector2Int(56, 29), new Vector2Int(65, 31),
            new Vector2Int(71, 28), new Vector2Int(65, 31), new Vector2Int(56, 29)
        },

        [SpritePath.Body_LovashiArmor_NoWeapon_OOC_Idle_Front] = new Vector2Int[]
        {
            new Vector2Int(60, 32), new Vector2Int(60, 33), new Vector2Int(60, 34), new Vector2Int(60, 33)
        },
        [SpritePath.Body_LovashiArmor_Polearm_OOC_Idle_Front] = new Vector2Int[]
        {
            new Vector2Int(59, 32), new Vector2Int(59, 33), new Vector2Int(59, 34), new Vector2Int(59, 33)
        },
        [SpritePath.Body_LovashiArmor_NoWeapon_Run_Front_Left] = new Vector2Int[] { new Vector2Int(56, 33) },
        [SpritePath.Body_LovashiArmor_NoWeapon_Run_Front_Right] = new Vector2Int[] { new Vector2Int(56, 34) },

        [SpritePath.Body_LovashiArmor_NoWeapon_OOC_Idle_Back] = new Vector2Int[]
        {
            new Vector2Int(56, 29), new Vector2Int(56, 30), new Vector2Int(56, 31), new Vector2Int(56, 30)
        },
        [SpritePath.Body_LovashiArmor_NoWeapon_Run_Back_Left] = new Vector2Int[] { new Vector2Int(56, 28) },
        [SpritePath.Body_LovashiArmor_NoWeapon_Run_Back_Right] = new Vector2Int[] { new Vector2Int(56, 30) },
    };

    //the first idle frame's position on each side, which fits a 164x164 body frame
    private readonly static Vector2Int defaultFrontHeadPosition = new Vector2Int(60, 32);
    private readonly static Vector2Int defaultBackHeadPosition = new Vector2Int(56, 29);

    private readonly static CanvasPlacement[] noPlacements = new CanvasPlacement[0];

    //one placement per body frame, worked out the first time a canvas is worn over a body sheet
    private readonly static Dictionary<(SpritePath, SpritePath), CanvasPlacement[]> placements = new();
    private readonly static HashSet<SpritePath> reportedBodyPaths = new();

    //statics survive between play sessions with domain reload disabled, and a sheet can be resliced in between
    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        placements.Clear();
        reportedBodyPaths.Clear();
    }

    //only the head layers are ever drawn on a canvas. Every other layer fills the body frame
    public static bool usesCanvas(SpriteLayer layer)
    {
        switch(layer)
        {
            case SpriteLayer.Face:
            case SpriteLayer.Hair:
                return true;
            default:
                return false;
        }
    }

    public static CanvasPlacement getPlacement(SpriteLayer layer, SpritePath bodyPath, SpritePath layerPath, int frame)
    {
        CanvasPlacement[] framePlacements = getPlacements(layer, bodyPath, layerPath);

        if(frame < 0 || frame >= framePlacements.Length)
        {
            return CanvasPlacement.identity;
        }

        return framePlacements[frame];
    }

    //the array is the list's own, so callers must not write into it
    public static CanvasPlacement[] getPlacements(SpriteLayer layer, SpritePath bodyPath, SpritePath layerPath)
    {
        if(!usesCanvas(layer) || bodyPath == SpritePath.NoSprite || layerPath == SpritePath.NoSprite)
        {
            return noPlacements;
        }

        if(placements.TryGetValue((bodyPath, layerPath), out CanvasPlacement[] framePlacements))
        {
            return framePlacements;
        }

        Sprite[] bodyFrames = SpriteList.getSprites(bodyPath);
        Sprite canvas = SpriteList.getSprite(layerPath);

        headPositions.TryGetValue(bodyPath, out Vector2Int[] positions);

        framePlacements = new CanvasPlacement[bodyFrames.Length];

        for(int frame = 0; frame < bodyFrames.Length; frame++)
        {
            if(!CanvasPlacement.isCanvas(bodyFrames[frame], canvas))
            {
                framePlacements[frame] = CanvasPlacement.identity;
            } else if(positions == null || frame >= positions.Length)
            {
                framePlacements[frame] = CanvasPlacement.calculate(bodyFrames[frame], canvas, getDefaultHeadPosition(bodyPath));
                reportMissingPosition(bodyPath, frame);
            } else
            {
                framePlacements[frame] = CanvasPlacement.calculate(bodyFrames[frame], canvas, positions[frame]);
            }
        }

        placements[(bodyPath, layerPath)] = framePlacements;

        return framePlacements;
    }

    //every back facing body sheet has _Back in its name. Anything else is treated as facing front
    private static Vector2Int getDefaultHeadPosition(SpritePath bodyPath)
    {
        if(bodyPath.ToString().Contains("_Back"))
        {
            return defaultBackHeadPosition;
        }

        return defaultFrontHeadPosition;
    }

    private static void reportMissingPosition(SpritePath bodyPath, int frame)
    {
        if(reportedBodyPaths.Add(bodyPath))
        {
            Debug.LogWarning("[CanvasPositionList] No head position for frame " + frame + " of SpritePath." + bodyPath + ", so a canvas worn over it uses the default position " + getDefaultHeadPosition(bodyPath) + ".");
        }
    }
}
