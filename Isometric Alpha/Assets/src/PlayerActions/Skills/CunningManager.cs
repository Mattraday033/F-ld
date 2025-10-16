using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using UnityEngine;

public class CunningManager : SkillManager
{
    public const int cunningRange = 9;
    public const int playerCunningCoords = (cunningRange - 1) / 2;

    public static int cunningsRemaining = -1;

    public static int getCunningsRemaining()
    {
        if (cunningsRemaining < 0)
        {
            resetCunningsRemaining();
        }

        return cunningsRemaining;
    }

    public static void setCunningsRemaining(int newCunningsRemaining)
    {
        cunningsRemaining = newCunningsRemaining;
    }

    public static void incrementCunningsRemaining()
    {
        if (cunningsRemaining + 1 <= PartyStats.getMaxCunningCount())
        {
            cunningsRemaining++;
            OOCUIManager.updateOOCUI();
        }
    }

    public static void decrementCunningsRemaining()
    {
        if (cunningsRemaining > 0)
        {
            cunningsRemaining--;
            OOCUIManager.updateOOCUI();
        }
    }

    public static void resetCunningsRemaining()
    {
        cunningsRemaining = PartyStats.getMaxCunningCount();

        if (State.oocUIManager != null)
        {
            OOCUIManager.updateOOCUI();
        }

    }

    public static CunningManager getInstance()
    {
        return PlayerMovement.getInstance().cunningManager;
    }

    public static void enterCunningMode()
    {
        SkillManager.destroyAllSkillGrids();
        getInstance().createSkillArea();
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.cunning);
    }

    public static void leaveCunningMode()
    {
        getInstance().destroySkillArea();
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
    }

    public override void createSkillArea()
    {
        Vector3Int playerCoords = getPlayerCoords();

        int range = getRange();
        skillGrid = new GameObject[range, range];

        for (int row = 0; row < range; row++)
        {
            for (int col = 0; col < range; col++)
            {
                if (coordsWithinRange(row, col))
                {
                    skillGrid[row, col] = instantiateTile(playerCoords, row, col);
                    Helpers.updateColliderPosition(skillGrid[row, col]);
                }
                else
                {
                    continue;
                }

                skillGrid[row, col].GetComponent<SpriteRenderer>().color = getTileColor(skillGrid[row, col]);
            }
        }

        cullSkillArea();
        setSelectorOriginTile();
    }

    public void cullSkillArea()
    {
        ArrayList snake;
        ArrayList colliderIndicators = new ArrayList();

        for (int i = 0; i < getRange(); i++)
        {
            for (int j = 0; j < getRange(); j++)
            {
                if (i == 0 || i == (getRange() - 1) || j == 0 || j == (getRange() - 1))
                {
                    snake = snakeTowardCenter(new Vector2Int(i, j));
                    colliderIndicators.AddRange(cullSnake(snake));

                }
            }
        }

        foreach (Vector2Int coords in colliderIndicators)
        {
            if (skillGrid[coords.x, coords.y] != null)
            {
                GameObject.Destroy(skillGrid[coords.x, coords.y]);
                skillGrid[coords.x, coords.y] = null;
            }
        }
    }

    private ArrayList snakeTowardCenter(Vector2Int start)
    {
        Vector2Int currentCoords = start;

        ArrayList snake = new ArrayList();

        while (currentCoords.x != getMiddleOfRange() || currentCoords.y != getMiddleOfRange())
        {
            snake.Add(currentCoords);

            if (midpointDistance(currentCoords.x) != 0)
            {
                currentCoords.x = nextSquare(currentCoords.x);
            }
            if (midpointDistance(currentCoords.y) != 0)
            {
                currentCoords.y = nextSquare(currentCoords.y);
            }
        }

        return snake;
    }

    private ArrayList cullSnake(ArrayList snake)
    {
        bool foundCollider = false;
        ArrayList colliderIndicators = new ArrayList();

        for (int i = (snake.Count - 1); i >= 0; i--)
        {
            Vector2Int currentCoords = (Vector2Int)snake[i];
            snake.Remove(i);

            if (skillGrid[currentCoords.x, currentCoords.y] == null)
            {
                continue; //ignore the null

            }
            Collider2D currentCollider = skillGrid[currentCoords.x, currentCoords.y].GetComponent<Collider2D>();


            if (skillGrid[currentCoords.x, currentCoords.y] != null &&
                        Helpers.hasCollision(currentCollider) &&
                        collisionsContainsColliderLayer(Helpers.getCollisions(currentCollider)))
            {
                foundCollider = true;
                colliderIndicators.Add(currentCoords);  // store the collider for later

            }
            else if (!foundCollider && skillGrid[currentCoords.x, currentCoords.y] != null)
            {
                continue; //keep the indicator

            }
            else if (foundCollider && skillGrid[currentCoords.x, currentCoords.y] != null)
            {
                GameObject.Destroy(skillGrid[currentCoords.x, currentCoords.y]);
                skillGrid[currentCoords.x, currentCoords.y] = null;   // destroy the indicator because it's on the other side of a wall
            }
        }

        return colliderIndicators;
    }

    private bool collisionsContainsColliderLayer(Collider2D[] collisions)
    {
        foreach (Collider2D collision in collisions)
        {
            if (collision != null && collision.gameObject.layer == LayerMask.NameToLayer("Collider"))
            {
                return true;
            }
        }

        return false;
    }

    private int nextSquare(int current)
    {
        if (current > getMiddleOfRange())
        {
            return (current - 1);
        }

        if (current < getMiddleOfRange())
        {
            return (current + 1);
        }

        return current;
    }

    public virtual void handleWASDMovement()
    {
        if (Input.GetKey(KeyBindingList.moveNorthKey))
        {
            moveCurrentSelectorNorthEast();
        }
        else if (Input.GetKey(KeyBindingList.moveWestKey))
        {
            moveCurrentSelectorNorthWest();
        }
        else if (Input.GetKey(KeyBindingList.moveSouthKey))
        {
            moveCurrentSelectorSouthWest();
        }
        else if (Input.GetKey(KeyBindingList.moveEastKey))
        {
            moveCurrentSelectorSouthEast();
        }
    }


    public void moveCurrentSelectorNorthEast()
    {
        moveCurrentSelector((Vector2Int)MovementManager.distance1TileNorthEastGrid);
    }
    public void moveCurrentSelectorNorthWest()
    {
        moveCurrentSelector((Vector2Int)MovementManager.distance1TileNorthWestGrid);
    }
    public void moveCurrentSelectorSouthWest()
    {
        moveCurrentSelector((Vector2Int)MovementManager.distance1TileSouthWestGrid);
    }
    public void moveCurrentSelectorSouthEast()
    {
        moveCurrentSelector((Vector2Int)MovementManager.distance1TileSouthEastGrid);
    }

    private void moveCurrentSelector(Vector2Int directionalModifier)
    {
        if (selectorPosition.x + directionalModifier.x < 0 ||
            selectorPosition.x + directionalModifier.x >= getRange() ||
            selectorPosition.y + directionalModifier.y < 0 ||
            selectorPosition.y + directionalModifier.y >= getRange())
        {
            return;
        }

        if (skillGrid[selectorPosition.x + directionalModifier.x, selectorPosition.y + directionalModifier.y] == null)
        {
            return;
        }

        skillGrid[selectorPosition.x, selectorPosition.y].GetComponent<SpriteRenderer>().color = oldColor;

        selectorPosition = selectorPosition + directionalModifier;

        oldColor = skillGrid[selectorPosition.x, selectorPosition.y].GetComponent<SpriteRenderer>().color;

        skillGrid[selectorPosition.x, selectorPosition.y].GetComponent<SpriteRenderer>().color = Color.green;
    }

    public void setSelectorOriginTile()
    {
        selectorPosition = getClosestStartingTileToFacingCoords(State.playerFacing.getFacing());

        oldColor = skillGrid[selectorPosition.x, selectorPosition.y].GetComponent<SpriteRenderer>().color;
        skillGrid[selectorPosition.x, selectorPosition.y].GetComponent<SpriteRenderer>().color = Color.green;
    }

    private Vector2Int getClosestStartingTileToFacingCoords(Facing direction)
    {
        for (int i = 0; i < 4; i++)
        {
            switch (direction)
            {
                case Facing.NorthEast:
                    if (skillGrid[getMiddleOfRange() + 1, getMiddleOfRange()] != null)
                    {
                        return new Vector2Int(getMiddleOfRange() + 1, getMiddleOfRange());
                    }
                    else
                    {
                        direction = Facing.NorthWest;
                    }
                    break;
                case Facing.NorthWest:
                    if (skillGrid[getMiddleOfRange(), getMiddleOfRange() + 1] != null)
                    {
                        return new Vector2Int(getMiddleOfRange(), getMiddleOfRange() + 1);
                    }
                    else
                    {
                        direction = Facing.SouthWest;
                    }
                    break;
                case Facing.SouthWest:
                    if (skillGrid[getMiddleOfRange() - 1, getMiddleOfRange()] != null)
                    {
                        return new Vector2Int(getMiddleOfRange() - 1, getMiddleOfRange());
                    }
                    else
                    {
                        direction = Facing.SouthEast;
                    }
                    break;
                case Facing.SouthEast:
                    if (skillGrid[getMiddleOfRange(), getMiddleOfRange() - 1] != null)
                    {
                        return new Vector2Int(getMiddleOfRange(), getMiddleOfRange() - 1);
                    }
                    else
                    {
                        direction = Facing.NorthEast;
                    }
                    break;
                default:
                    throw new IOException("Player isn't facing anywhere?");
            }
        }

        throw new IOException("Player isn't facing anywhere?");
    }

    public override string getTilePrefabName()
    {
        return PrefabNames.cunningTileName;
    }

    public override Color getTileBaseColor()
    {
        return Color.yellow;
    }

    public override Color getTileTargetColor()
    {
        return Color.red;
    }

    private bool coordsWithinRange(int row, int col)
    {
        return (Math.Abs(row - getCurrentPlayerSkillGridCoords().row) + Math.Abs(col - getCurrentPlayerSkillGridCoords().col)) <= getCurrentPlayerSkillGridCoords().row;
    }

    public override int getRange()
    {
        return cunningRange;
    }

    public override bool executeSkill()
    {
        ISkillTarget cunningTarget = getTargetFromTile(skillGrid[selectorPosition.x, selectorPosition.y]);

        if (cunningTarget != null)
        {
            cunningTarget.cunning();
            decrementCunningsRemaining();
            destroySkillArea();
            return true;
        }
        else
        {
            return false;
        }
    }

    public ISkillTarget getTargetFromTile(GameObject tile)
    {
        Collider2D selectorCollider = tile.GetComponent<Collider2D>();
        ISkillTarget target = null;

        if (!Helpers.hasCollision(selectorCollider))
        {

            return target;
        }
        else
        {
            Collider2D[] allCollisions = Helpers.getCollisions(selectorCollider);

            foreach (Collider2D collision in allCollisions)
            {
                if (collision != null && !(collision is null))
                {
                    target = collision.gameObject.GetComponent<ISkillTarget>();

                    if (target != null && !(target is null))
                    {
                        break;
                    }
                }
            }

            return target;
        }
    }

    private string getGridCellCoords(Vector2Int currentCoords)
    {
        Vector3Int gridCellCoords = PlayerMovement.getMovementGridCoords();

        return "(" + ((currentCoords.x + gridCellCoords.x) - getMiddleOfRange()) + "," + ((currentCoords.y + gridCellCoords.y) - getMiddleOfRange()) + ")";
    }
    
    public static void destroyCunningSkillArea()
    {
        if (getInstance() != null)
        {
            getInstance().destroySkillArea();
        }
    }
}
