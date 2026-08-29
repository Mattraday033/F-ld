using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class CunningManager : SkillManager
{
    public const int cunningRange = 9;
    public const int playerCunningCoords = (cunningRange - 1) / 2;

    public static CunningManager getInstance()
    {
        return new CunningManager();
    } 

    public static int cunningsRemaining;

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        cunningsRemaining = -1;

        LoadSaveFile.OnLoadReadBlueprint.AddListener(readSaveBlueprint);
        PlayerOOCStateManager.OnStateChangeFromSkill.AddListener(enableAllHoverColliders);
    }

    public override ContactFilter2D getCollisionFilter()
    {
        ContactFilter2D filterCollider = new ContactFilter2D();
        filterCollider.useTriggers = true;
        filterCollider.SetLayerMask(LayerAndTagManager.blocksCunningLayerMask);

        return filterCollider;
    }

    private static void enableAllHoverColliders()
    {
        EnemyMovement.ToggleHoverColliders.Invoke(true);
    }

    private static void readSaveBlueprint(SaveBlueprint blueprint)
    {
        setCunningsRemaining(blueprint.cunningsRemaining);
    }

    public static int getCunningsRemaining()
    {
        if(PartyStats.inTutorialArea() && Flags.getFlag(FlagNameList.startedTaborCunningTutorial))
        {
            return 2;
        }

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

    public static void addCunningsRemaining(int charges)
    {
        if (cunningsRemaining + charges <= PartyStats.getMaxCunningCount())
        {
            cunningsRemaining += charges;
            OOCUIManager.updateOOCUI();
        }
    }

    public static void removeCunningsRemaining(int charges)
    {
        if (cunningsRemaining - charges >= 0)
        {
            cunningsRemaining -= charges;
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

    public static void enterCunningMode()
    {
        destroyAllSkillGrids();
        getInstance().createSkillArea();
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.cunning);
        EnemyMovement.ToggleHoverColliders.Invoke(false);
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
        skillGrid = new SkillIndicator[range, range];

        for (int row = 0; row < range; row++)
        {
            for (int col = 0; col < range; col++)
            {
                if (coordsWithinRange(row, col))
                {
                    skillGrid[row, col] = instantiateTile(playerCoords, row, col).GetComponent<SkillIndicator>();
                    skillGrid[row, col].updateColliderPosition();
                    skillGrid[row, col].allowHover = allowHovers();
                    skillGrid[row, col].coords = new Vector2Int(row, col);
                }
                else
                {
                    continue;
                }

                setTileColor(skillGrid[row, col]);
            }
        }

        cullSkillArea();
        setSelectorOriginTile();
    }

    public override bool targetIsValid(ISkillTarget skillTarget)
    {
        return skillTarget != null && skillTarget.validTarget(SkillType.Cunning);
    }

    protected virtual bool allowHovers()
    {
        return true;
    }

    public void cullSkillArea()
    {
        List<Vector2Int> snake;
        List<Vector2Int> colliderIndicators = new List<Vector2Int>();

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
            destroyTileAt(coords);
        }

        if(!allowSolitaryTiles())
        {
            for (int i = 0; i < getRange(); i++)
            {
                for (int j = 0; j < getRange(); j++)
                {
                    bool nothingAbove = i == 0 || skillGrid[i-1,j] == null;
                    bool nothingBelow = i == getRange()-1 || skillGrid[i+1,j] == null;
                    bool nothingLeft = j == 0 || skillGrid[i,j-1] == null;
                    bool nothingRight = j == getRange()-1 || skillGrid[i,j+1] == null;
                    
                    if( nothingAbove && 
                        nothingBelow && 
                        nothingLeft && 
                        nothingRight)
                    {
                        destroyTileAt(i,j);
                    }
                }
            }
        }
    }

    public virtual bool allowSolitaryTiles()
    {
        return false;
    }

    private List<Vector2Int> snakeTowardCenter(Vector2Int start)
    {
        Vector2Int currentCoords = start;

        List<Vector2Int> snake = new List<Vector2Int>();

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

    private List<Vector2Int> cullSnake(List<Vector2Int> snake)
    {
        bool foundCollider = false;
        List<Vector2Int> colliderIndicators = new List<Vector2Int>();

        for (int i = snake.Count - 1; i >= 0; i--)
        {
            Vector2Int currentCoords = snake[i];
            snake.RemoveAt(i);

            if (skillGrid[currentCoords.x, currentCoords.y] == null)
            {
                continue; //ignore the null

            }
            Collider2D currentCollider = skillGrid[currentCoords.x, currentCoords.y].collider;

            if (skillGrid[currentCoords.x, currentCoords.y] != null &&
                        Helpers.hasCollision(currentCollider) &&
                        cullFromCollision(Helpers.getCollisions(currentCollider)))
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
                GameObject.Destroy(skillGrid[currentCoords.x, currentCoords.y].gameObject);
                skillGrid[currentCoords.x, currentCoords.y] = null;   // destroy the indicator because it's on the other side of a wall
            }
        }

        return colliderIndicators;
    }

    public virtual bool cullFromCollision(Collider2D[] collisions)
    {
        foreach (Collider2D collision in collisions)
        {
            if (collision != null && 
                (collision.gameObject.layer == LayerAndTagManager.colliderLayer || 
                 collision.gameObject.layer == LayerAndTagManager.observableLayer || 
                 collision.gameObject.layer == LayerAndTagManager.npcLayer))
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
            return current - 1;
        }

        if (current < getMiddleOfRange())
        {
            return current + 1;
        }

        return current;
    }

    public virtual void handleWASDMovement()
    {
        if (Input.GetKey(KeyBindingList.moveNorthKey.getCurrentKeyCode()))
        {
            moveCurrentSelectorNorthEast();
        }
        else if (Input.GetKey(KeyBindingList.moveWestKey.getCurrentKeyCode()))
        {
            moveCurrentSelectorNorthWest();
        }
        else if (Input.GetKey(KeyBindingList.moveSouthKey.getCurrentKeyCode()))
        {
            moveCurrentSelectorSouthWest();
        }
        else if (Input.GetKey(KeyBindingList.moveEastKey.getCurrentKeyCode()))
        {
            moveCurrentSelectorSouthEast();
        }

        PlayerObject.setButtonPromptVisibility();
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

    public static void setCurrentSelector(Vector2Int newPosition)
    {
        if (newPosition.x < 0 ||
            newPosition.x >= getInstance().getRange() ||
            newPosition.y < 0 ||
            newPosition.y >= getInstance().getRange())
        {
            return;
        }

        if (skillGrid[newPosition.x, newPosition.y] == null)
        {
            return;
        }

        getInstance().setTileColor(skillGrid[selectorPosition.x, selectorPosition.y]);
        skillGrid[selectorPosition.x, selectorPosition.y].currentCursor = false;
        // skillGrid[selectorPosition.x, selectorPosition.y].setToNoTargetFoundSelector();

        selectorPosition = newPosition;

        skillGrid[selectorPosition.x, selectorPosition.y].setColor(Color.green);
        skillGrid[selectorPosition.x, selectorPosition.y].setToTargetFoundSelector();
        skillGrid[selectorPosition.x, selectorPosition.y].currentCursor = true;

        PlayerObject.setButtonPromptVisibility();
        AudioManager.playSelectorMovedSFX();
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

        setTileColor(skillGrid[selectorPosition.x, selectorPosition.y]);
        skillGrid[selectorPosition.x, selectorPosition.y].currentCursor = false;
        // skillGrid[selectorPosition.x, selectorPosition.y].setToNoTargetFoundSelector();

        selectorPosition = selectorPosition + directionalModifier;

        skillGrid[selectorPosition.x, selectorPosition.y].setColor(Color.green);
        skillGrid[selectorPosition.x, selectorPosition.y].setToTargetFoundSelector();
        skillGrid[selectorPosition.x, selectorPosition.y].currentCursor = true;

        AudioManager.playSelectorMovedSFX();
    }

    protected virtual void setSelectorOriginTile()
    {
        selectorPosition = getClosestStartingTileToFacingCoords(State.playerFacing.getFacing());

        skillGrid[selectorPosition.x, selectorPosition.y].setColor(Color.green);
        skillGrid[selectorPosition.x, selectorPosition.y].setToTargetFoundSelector();
        skillGrid[selectorPosition.x, selectorPosition.y].currentCursor = true;
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

    public override Color getTileBaseColor()
    {
        return ColorList.cunningTileBaseColor;
    }

    public override Color getTileTargetColor()
    {
        return ColorList.skillIndicatorTargetableColor;
    }

    public override int getRange()
    {
        return cunningRange;
    }

    public bool hasEnoughChargesForTarget(ISkillTarget target)
    {
        return target.getChargeCost(SkillType.Cunning) <= getCunningsRemaining();
    }

    public virtual bool canUseSkill()
    {
        if(skillGrid == null)
        {
            return false;
        }

        ISkillTarget target = getTargetFromTile(skillGrid[selectorPosition.x, selectorPosition.y]);

        return canUseSkill(target);
    }

    public bool hasTooExpensiveTarget()
    {
        if(skillGrid == null)
        {
            return false;
        }

        ISkillTarget target = getTargetFromTile(skillGrid[selectorPosition.x, selectorPosition.y]);

        return target != null && target.validTarget(SkillType.Cunning) && !hasEnoughChargesForTarget(target);
    }

    protected bool canUseSkill(ISkillTarget target)
    {
        return target != null && target.validTarget(SkillType.Cunning) && hasEnoughChargesForTarget(target);
    }

    public override bool executeSkill()
    {
        ISkillTarget target = getTargetFromTile(skillGrid[selectorPosition.x, selectorPosition.y]);

        if (canUseSkill(target))
        {
            target.cunning();
            removeCunningsRemaining(target.getChargeCost(SkillType.Cunning));
            destroySkillArea();
            OnSkillUse.Invoke();
            // AudioManager.playSmokebombSFX();
            createEffect(target.getTargetPosition());
            return true;
        }
        else
        {
            return false;
        }
    }

    public ISkillTarget getTargetFromTile(SkillIndicator tile)
    {
        if(tile == null)
        {
            return null;
        }

        EnemyMovement.ToggleHoverColliders.Invoke(true);
        Collider2D selectorCollider = tile.collider;
        ISkillTarget target = null;

        if (!Helpers.hasCollision(selectorCollider))
        {

            EnemyMovement.ToggleHoverColliders.Invoke(false);
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

            EnemyMovement.ToggleHoverColliders.Invoke(false);
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
