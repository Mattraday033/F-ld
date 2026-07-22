using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public enum SkillType {Intimidate = 0, Cunning = 1, Observation = 2, Leadership = 3}

public interface ISkillTarget
{
    public int getChargeCost(SkillType skillType);
    public bool validTarget(SkillType skillType);
    public void cunning();
    public void intimidate();
    public Vector3 getTargetPosition();
}
public abstract class SkillManager
{
    public readonly static UnityEvent OnSkillUse = new UnityEvent();
    public readonly static UnityEvent OnSkillTargetFound = new UnityEvent();

    public static Vector2Int selectorPosition;
    public static Color oldColor;
    protected static SkillIndicator[,] skillGrid;

    public const int skillUnlockLevel = 2;
    public const int skillImprovedLevel = 5;
    public const int skillExtraordinaryLevel = 8;

    public virtual ContactFilter2D getCollisionFilter()
    {
        ContactFilter2D filterCollider = new ContactFilter2D();
        filterCollider.useTriggers = true;
        filterCollider.SetLayerMask(LayerAndTagManager.blocksSkillsLayerMask);

        return filterCollider;
    }

    public virtual void createSkillArea()
    {
        throw new IOException("Base version of createSkillArea called erroneously");
    }

    public int midpointDistance(int current)
    {
        return Math.Abs(getCurrentPlayerSkillGridCoords().row - current);
    }

    public void destroySkillArea()
    {
        if (skillGrid == null)
        {
            return;
        }

        foreach (SkillIndicator tile in skillGrid)
        {
            if (tile != null)
            {
                GameObject.Destroy(tile.gameObject);
            }
        }
    }

    public virtual bool executeSkill()
    {
        return false;
    }

    public static Vector3Int getPlayerCoords()
    {
        return AreaManager.getMasterGrid().WorldToCell(PlayerMovement.getInstance().endingPosition);
    }

    public bool coordsWithinRange(int row, int col)
    {
        return (Math.Abs(row - getCurrentPlayerSkillGridCoords().row) + Math.Abs(col - getCurrentPlayerSkillGridCoords().col)) <= getCurrentPlayerSkillGridCoords().row;
    }

    public abstract string getTilePrefabName();

    private Transform getTileParent()
    {
        return PlayerMovement.getInstance().gameObject.transform.parent;
    }
    public abstract Color getTileBaseColor();
    public abstract Color getTileTargetColor();

    public Color getTileColor(SkillIndicator tile)
    {
        if (collidedWithTarget(tile))
        {
            OnSkillTargetFound.Invoke();
            return getTileTargetColor();
        }
        else
        {
            return getTileBaseColor();
        }
    }

    public virtual bool collidedWithTarget(SkillIndicator tile)
    {
        Collider2D[] collisions = Helpers.getCollisions(tile.collider, getCollisionFilter());

        foreach (Collider2D collision in collisions)
        {
            if (collision == null || collision is null)
            {
                continue;
            }

            if (collision.GetComponent<ISkillTarget>() != null)
            {
                return true;
            }
        }

        return false;
    }

    public GameObject instantiateTile(Vector3Int playerCoords, int skillGridRow, int skillGridCol)
    {
        GameObject tile = GameObject.Instantiate(Resources.Load<GameObject>(getTilePrefabName()), getTileParent(), false);

        tile.transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(playerCoords + new Vector3Int(skillGridRow - getCurrentPlayerSkillGridCoords().row, skillGridCol - getCurrentPlayerSkillGridCoords().col));

        return tile;
    }

    public void destroyTileAt(int row, int col)
    {
        if (skillGrid[row, col] == null)
        {
            return;
        }

        GameObject.Destroy(skillGrid[row, col].gameObject);
        skillGrid[row, col] = null;
    }

    public virtual int getRange()
    {
        throw new IOException("Base version of getRange() called erroneously");
    }

    public int getMiddleOfRange()
    {
        return getRange() / 2;
    }

    public GridCoords getCurrentPlayerSkillGridCoords()
    {
        return new GridCoords(getMiddleOfRange(), getMiddleOfRange());
    }

    public static void destroyAllSkillGrids()
    {
        IntimidateManager.destroyIntimdiateSkillArea();
        CunningManager.destroyCunningSkillArea();
        ObservationManager.destroyObservationSkillArea();
    }

    public static SkillType getHighestSkillType(AllyStats stats)
    {
        switch(stats.getHighestStat())
        {
            case PrimaryStat.Dexterity:
                return SkillType.Cunning;
            case PrimaryStat.Wisdom:
                return SkillType.Observation;
            case PrimaryStat.Charisma:
                return SkillType.Leadership;
            default:
                return SkillType.Intimidate;
        }
    }

    public void createEffect(Vector3 targetWorldPos)
    {
        EffectAnimationManager effect = EffectAnimationManager.instantiatePrefab();
        Transform effectTransform = effect.transform;
        effectTransform.position = targetWorldPos;

        effect.waitBeforeSFX = false;
        effect.setAnimations(getEffectType());
    }

    public virtual string getEffectType()
    {
        return EffectAnimationType.SmokeBomb.ToString();
    }

    /*
    private string getGridCellCoords(Vector2Int currentCoords)
    {
        Vector3Int gridCellCoords = PlayerMovement.getMovementGridCoords();

        return "(" + ((currentCoords.x + gridCellCoords.x) - playerCunningCoords) + "," + ((currentCoords.y + gridCellCoords.y) - playerCunningCoords) + ")";
    }
    */
}
