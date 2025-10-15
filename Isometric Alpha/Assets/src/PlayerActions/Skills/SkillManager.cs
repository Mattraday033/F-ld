using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using UnityEngine;

public interface ISkillTarget
{
    public void cunning();
    public void intimidate();
}
public class SkillManager
{
    public Vector2Int selectorPosition;
    public Color oldColor;
    public GameObject[,] skillGrid;

    public const int skillUnlockLevel = 2;
    public const int skillImprovedLevel = 5;
    public const int skillExtraordinaryLevel = 8;

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

        foreach (GameObject tile in skillGrid)
        {
            if (tile != null)
            {
                GameObject.Destroy(tile);
            }
        }
    }

    public virtual bool executeSkill()
    {
        return false;
    }

    public static Vector3Int getPlayerCoords()
    {
        return MovementManager.getInstance().grid.LocalToCell(MovementManager.endingPositions[MovementManager.playerSpriteIndex]);
    }

    private bool coordsWithinRange(int row, int col)
    {
        return (Math.Abs(row - getCurrentPlayerSkillGridCoords().row) + Math.Abs(col - getCurrentPlayerSkillGridCoords().col)) <= getCurrentPlayerSkillGridCoords().row;
    }

    public virtual string getTilePrefabName()
    {
        throw new IOException("Base version of getTilePrefabName() called erroneously");
    }

    private Transform getTileParent()
    {
        return PlayerMovement.getInstance().gameObject.transform.parent;
    }
    public virtual Color getTileBaseColor()
    {
        throw new IOException("Base version of getTileBaseColor() called erroneously");
    }
    public virtual Color getTileTargetColor()
    {
        throw new IOException("Base version of getTileTargetColor() called erroneously");
    }

    public Color getTileColor(GameObject tile)
    {
        if (collidedWithTarget(tile))
        {
            return getTileTargetColor();
        }
        else
        {
            return getTileBaseColor();
        }
    }

    public virtual bool collidedWithTarget(GameObject tile)
    {
        if (!Helpers.hasCollision(tile.GetComponent<Collider2D>()))
        {
            return false;
        }
        else
        {
            Collider2D[] collisions = Helpers.getCollisions(tile.GetComponent<Collider2D>());

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
    }

    public GameObject instantiateTile(Vector3Int playerCoords, int skillGridRow, int skillGridCol)
    {
        GameObject tile = GameObject.Instantiate(Resources.Load<GameObject>(getTilePrefabName()), getTileParent(), false);

        tile.transform.localPosition = MovementManager.getGrid().GetCellCenterLocal(playerCoords + new Vector3Int(skillGridRow - getCurrentPlayerSkillGridCoords().row, skillGridCol - getCurrentPlayerSkillGridCoords().col, 0));

        return tile;
    }

    public void destroyTileAt(int row, int col)
    {
        if (skillGrid[row, col] == null)
        {
            return;
        }

        GameObject.Destroy(skillGrid[row, col]);
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

    /*
    private string getGridCellCoords(Vector2Int currentCoords)
    {
        Vector3Int gridCellCoords = PlayerMovement.getInstance().getMovementGridCoordsLocal();

        return "(" + ((currentCoords.x + gridCellCoords.x) - playerCunningCoords) + "," + ((currentCoords.y + gridCellCoords.y) - playerCunningCoords) + ")";
    }
    */
}
