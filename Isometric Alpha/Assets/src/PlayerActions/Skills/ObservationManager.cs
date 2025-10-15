using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObservationManager : SkillManager
{
    public const int observeRange = 15;
    public const int playerObserveCoords = (observeRange - 1) / 2;

    public ContactFilter2D filterCollider;

    public static ObservationManager getInstance()
    {
        return PlayerMovement.getInstance().observationManager;
    }

    public static void enterObservationMode()
    {
        SkillManager.destroyAllSkillGrids();
        getInstance().createSkillArea();
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.observing);
    }

    public static void leaveObservationMode()
    {
        getInstance().destroySkillArea();
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
    }

    public ObservationManager()
    {
        filterCollider = new ContactFilter2D();
        filterCollider.useTriggers = true;
        filterCollider.SetLayerMask(LayerAndTagManager.blocksObservationLayerMask);
    }

    public override void createSkillArea()
    {
        skillGrid = new GameObject[getRange(), getRange()];
        Vector3Int playerCoords = getPlayerCoords();

        for (int row = 0; row < observeRange; row++)
        {
            for (int col = 0; col < observeRange; col++)
            {

                if (gridTileIsInSight(row, col))
                {
                    skillGrid[row, col] = instantiateTile(playerCoords, row, col);
                    Helpers.updateColliderPosition(skillGrid[row, col]);
                }

                /*if(skillGrid[row,col] != null && 
					((Helpers.hasCollision(skillGrid[row,col].GetComponent<Collider2D>()) && !Helpers.hasCollision(skillGrid[row,col].GetComponent<Collider2D>(), layersToIgnore)) 
					|| (row == playerObserveCoords && col == playerObserveCoords)))
				{
					skillGrid[row,col].GetComponent<SpriteRenderer>().color = Color.cyan;
				}*/

            }
        }

        cullSkillArea();
        observeWithinRange();
    }

    private void cullSkillArea()
    {
        ArrayList snake;
        ArrayList colliderIndicators = new ArrayList();

        for (int i = 0; i < observeRange; i++)
        {
            for (int j = 0; j < observeRange; j++)
            {
                if (i == 0 || i == (observeRange - 1) || j == 0 || j == (observeRange - 1))
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

        while (currentCoords.x != playerObserveCoords || currentCoords.y != playerObserveCoords)
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
                        Helpers.hasCollision(currentCollider, filterCollider) &&
                        !Helpers.getCollision(currentCollider).gameObject.CompareTag("Observable"))
            {
                if (!foundCollider)
                {
                    foundCollider = true;
                    colliderIndicators.Add(currentCoords);  // store the collider for later
                }
                else
                {
                    destroyTileAt(currentCoords.x, currentCoords.y);
                }
            }
            else if (skillGrid[currentCoords.x, currentCoords.y] != null &&
                        Helpers.hasCollision(currentCollider) &&
                        Helpers.getCollision(currentCollider).gameObject.CompareTag("Observable"))
            {
                if (!foundCollider)
                {
                    foundCollider = true;
                }
                else
                {
                    destroyTileAt(currentCoords.x, currentCoords.y);
                }

            }
            else if (!foundCollider && skillGrid[currentCoords.x, currentCoords.y] != null)
            {
                continue; //keep the indicator

            }
            else if (foundCollider && skillGrid[currentCoords.x, currentCoords.y] != null)
            {
                destroyTileAt(currentCoords.x, currentCoords.y);  // destroy the indicator because it's on the other side of a wall
            }
        }

        return colliderIndicators;
    }



    private int nextSquare(int current)
    {
        if (current > getCurrentPlayerSkillGridCoords().row)
        {
            return (current - 1);
        }

        if (current < getCurrentPlayerSkillGridCoords().row)
        {
            return (current + 1);
        }

        return current;
    }
    public void observeWithinRange()
    {
        foreach (GameObject tile in skillGrid)
        {
            if (tile == null || tile is null)
            {
                continue;
            }

            tile.GetComponent<MarkObservableObject>().detectObservableObject();
        }
    }

    public override string getTilePrefabName()
    {
        return PrefabNames.observationTileName;
    }

    public override Color getTileBaseColor()
    {
        return Color.magenta;
    }
    public override Color getTileTargetColor()
    {
        return Color.cyan;
    }

    public override int getRange()
    {
        return observeRange;
    }

    public override bool executeSkill()
    {
        return false;
    }

    private bool gridTileIsInSight(int row, int col)
    {

        if (State.playerFacing.getFacing().Equals(Facing.NorthEast))
        {
            return (row >= playerObserveCoords && col >= (observeRange - (row + 1)) && col <= (observeRange - (observeRange - row)));
        }

        if (State.playerFacing.getFacing().Equals(Facing.NorthWest))
        {
            return ((col >= (observeRange - (row + 1)) && row <= playerObserveCoords) || (col >= row && row >= playerObserveCoords));
        }

        if (State.playerFacing.getFacing().Equals(Facing.SouthWest))
        {
            return (col >= row && col < (observeRange - row));
        }

        if (State.playerFacing.getFacing().Equals(Facing.SouthEast))
        {
            return (col <= playerObserveCoords && (col <= row && (col + row) < observeRange));
        }

        throw new IOException("Character isn't facing any direction");
    }
    
        
    public static void destroyObservationSkillArea()
    {
        if (getInstance() != null)
        {
            getInstance().destroySkillArea();
        }
    }
}
