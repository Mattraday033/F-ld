using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MovementManager : MonoBehaviour
{
    public readonly static Vector3Int distance1TileNorthEastGrid = new Vector3Int(1, 0, 0);
    public readonly static Vector3Int distance1TileNorthWestGrid = new Vector3Int(0, 1, 0);
    public readonly static Vector3Int distance1TileSouthWestGrid = new Vector3Int(-1, 0, 0);
    public readonly static Vector3Int distance1TileSouthEastGrid = new Vector3Int(0, -1, 0);

    public readonly static Vector3Int[] allDirectionVectors = new Vector3Int[]{distance1TileNorthEastGrid,
                                                                                distance1TileNorthWestGrid,
                                                                                distance1TileSouthWestGrid,
                                                                                distance1TileSouthEastGrid};

    public readonly static UnityEvent OnMoveFinished = new UnityEvent();

    public Grid grid;

    public bool smallWaitAfterMoving = false;

    public static List<MovementTracker> allMovementTrackers;
    public static Dictionary<MovementTracker, Coroutine> currentMovements;

    private const float timeToMove = .2f;

    public const int playerSpriteIndex = 0;

    public static Grid getGrid()
    {
        return AreaManager.getMasterGrid();
    }

    void Awake()
    {
        TransitionManager.BeforeTransition.AddListener(initializeMovementManager);
    }

    private void OnDestroy()
    {
        TransitionManager.BeforeTransition.RemoveListener(initializeMovementManager);
    }

    [RuntimeInitializeOnLoadMethod]
    public static void initializeMovementManager()
    {
        allMovementTrackers = new List<MovementTracker>();
        currentMovements = new Dictionary<MovementTracker, Coroutine>();
    }

    public static Vector3Int getPlayerCell()
    {
        return MovementTracker.getCurrentCell(PlayerMovement.getInstance());
    }

    public void addFloorButton(IFloorButton button)
    {
        // floorButtons.Add(button);
    }

    //something keeps setting sprites' Z position to 25.5 and this messes with positioning. 
    //this sets them all back to 0 before doing movement stuff
    private void setAllZPositionsToZero()
    {
        foreach (MovementTracker movement in allMovementTrackers)
        {
            if (movement == null || movement.getTransform() == null)
            {
                continue;
            }

            Vector3 newposition = movement.getWorldPosition();
            newposition.z = 0;
            movement.getTransform().position = newposition;
        }
    }

    public void moveAllSprites()
    {
        setAllZPositionsToZero();

        if (PlayerMovement.getInstance().isMoving())
        {
            return;
        }

        foreach (MovementTracker movement in allMovementTrackers)
        {
            if (movement == null) { continue; }

            Vector3Int coords = MovementTracker.getCurrentCell(movement);

            if (!movement.isMoving())
            {
                movement.determineDirection();
            }
        }

        preventCollidingEndingPositions();

        preventMonstersFromMovingAwayFromPlayer();

        //movement loop

        foreach (MovementTracker movement in allMovementTrackers)
        {
            if (movement == null) { continue; }

            if (!movement.isMoving())
            {
                currentMovements.Add(movement, StartCoroutine(moveSprite(movement)));

                movement.updateFacing();

                if (!PlayerMovement.getInstance().directionMod.Equals(Vector3Int.zero))
                {
                    PartyMemberMovement.showAllPartyMembers();
                    movePartyMembers(0, grid.CellToWorld(getFirstPartyMemberEndingPosition()));
                }
            }
        }

        smallWaitAfterMoving = true;

        changeFooting();
    }

    public static bool onLeftFoot()
    {
        return State.onLeftFoot;
    }

    public static void changeFooting()
    {
        StepCountScriptManager.incrementStepCount();

        State.onLeftFoot = !State.onLeftFoot;
        OOCUIManager.getInstance().updateFooting();
    }

    public static void setFooting(bool onLeftFoot)
    {
        State.onLeftFoot = onLeftFoot;

        if (OOCUIManager.getInstance() != null)
        {
            OOCUIManager.getInstance().updateFooting();
        }
    }

    public static bool colliderInCell(Vector3Int cellCoords, LayerMask layerMask)
    {
        Vector3 worldPosition = AreaManager.getMasterGrid().CellToWorld(cellCoords);

        return Physics2D.OverlapCircle(worldPosition, .05f, layerMask);
    }

    public static Vector3 getColliderInCellPosition(Vector3Int cellCoords)
    {
        return AreaManager.getMasterGrid().CellToWorld(cellCoords);
    }

    public static void addMovementTracker(MovementTracker movement)
    {
        movement.startingPosition = movement.getWorldPosition();
        movement.endingPosition = movement.startingPosition;

        if (allMovementTrackers.Count == movement.getMovementIndex())
        {
            allMovementTrackers.Add(movement);
        }
        else if (allMovementTrackers.Count > movement.getMovementIndex())
        {
            if (allMovementTrackers[movement.getMovementIndex()] == null)
            {
                allMovementTrackers[movement.getMovementIndex()] = movement;
            }
        }
        else
        {
            MovementTracker[] movements = new MovementTracker[movement.getMovementIndex() - allMovementTrackers.Count + 1];

            movements[movements.Length - 1] = movement;

            allMovementTrackers.AddRange(movements);
        }      
    }

    public static void replaceMovementTracker(MovementTracker movement)
    {
        movement.startingPosition = movement.getWorldPosition();
        movement.endingPosition = movement.startingPosition;

        allMovementTrackers[movement.getMovementIndex()] = movement;
    }

    private IEnumerator prepCombatAfterMovesFinish(MovementTracker movement)
    {
        do
        {
            yield return null;
        } while (PlayerMovement.getInstance().isMoving());

        EnemyMovement enemyMovement = movement as EnemyMovement;

        if (enemyMovement != null)
        {
            enemyMovement.prepCombat();
        }
    }

    private IEnumerator moveSprite(MovementTracker movement)
    {
        if (movement.getMovementIndex() != playerSpriteIndex && MonsterDefeatKeysList.monsterIsDefeated(movement.getMovementIndex() - 1))
        {
            yield return null;
            currentMovements.Remove(movement);
            yield break;
        }

        float elapsedTime = 0;

        while (elapsedTime <= timeToMove)
        {
            movement.getTransform().position = Vector3.Lerp(movement.startingPosition, movement.endingPosition, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;  
            yield return null;
        }

        movement.getTransform().position = movement.endingPosition;
        movement.startingPosition = movement.getTransform().position;

        PartyMemberMovement.hideOverlappingPartyMembers();

        currentMovements.Remove(movement);

        movement.updateFacing();

        OnMoveFinished.Invoke();
    }

    private void preventCollidingEndingPositions()
    {

        List<MovementTracker> previousMovements = new List<MovementTracker>();

        foreach (MovementTracker movement in allMovementTrackers)
        {
            if (previousMovements.Count <= 0 && movement != null)
            {
                previousMovements.Add(movement);
                continue;
            }else if (movement == null)
            {
                continue;
            }

            Vector3Int currentMonstersCellCoords = MovementTracker.getCurrentCell(movement);
            Vector3Int currentMonstersEndingCellCoords = MovementTracker.getEndingCell(movement);

            foreach (MovementTracker previousMovement in previousMovements)
            {
                Vector3Int previousMonstersCellCoords = MovementTracker.getCurrentCell(previousMovement);
                Vector3Int previousMonstersEndingCellCoords = MovementTracker.getEndingCell(previousMovement);

                if (previousMonstersCellCoords.Equals(currentMonstersCellCoords))
                {
                    movement.cancelMovement();
                } else if (previousMonstersEndingCellCoords.Equals(currentMonstersEndingCellCoords))
                {
                    movement.cancelMovement();
                }
            }

            previousMovements.Add(movement);
        }

    }

    private Vector3Int getFirstPartyMemberEndingPosition()
    {
        return grid.WorldToCell(PlayerMovement.getInstance().startingPosition);
    }

    public void movePartyMembers(int partyMemberIndex, Vector3 endingPosition)
    {

        // if (PartyMemberMovement.partyMemberTrain == null)
        // {
        //     return;
        // }

        // if (partyMemberIndex < PartyMemberMovement.partyMemberTrain.Length &&
        //     partyMemberIndex < (PartyMemberMovement.stepCounter))
        // {
        //     Vector3 startingPosition = PartyMemberMovement.partyMemberTrain[partyMemberIndex].position;

        //     StartCoroutine(moveSprite(PartyMemberMovement.partyMemberTrain[partyMemberIndex]));

        //     endingPosition = startingPosition;

        //     movePartyMembers(partyMemberIndex + 1, endingPosition);
        // }
        // else
        // {
        //     PartyMemberMovement.stepCounter++;
        //     return;
        // }
    }

    private void preventMonstersFromMovingAwayFromPlayer()
    {
        for (int positionIndex = 1; positionIndex < allMovementTrackers.Count; positionIndex++)
        {
            if (allMovementTrackers[positionIndex].movableObject)
            {
                continue;
            }

            if (cellsAreAdjacent(PlayerMovement.getInstance().endingPosition, allMovementTrackers[positionIndex].startingPosition))
            {
                allMovementTrackers[positionIndex].cancelMovement();
            }

            if (allMovementTrackers[positionIndex].gameObject.activeSelf == true &&
                cellsAreAdjacent(PlayerMovement.getInstance().endingPosition, allMovementTrackers[positionIndex].endingPosition))
            {
                StartCoroutine(prepCombatAfterMovesFinish(allMovementTrackers[positionIndex]));
            }
        }
    }

    public bool cellsAreAdjacent(Vector3 first, Vector3 second)
    {
        first.z = 0f;
        second.z = 0f;

        Vector3Int firstCellPosition = getCellWorld(first);
        Vector3Int secondCellPosition = getCellWorld(second);

        int xDistance = firstCellPosition.x - secondCellPosition.x;
        int yDistance = firstCellPosition.y - secondCellPosition.y;

        if (xDistance <= 1 && xDistance >= -1 && yDistance == 0)
        {
            return true;
        }

        if (yDistance <= 1 && yDistance >= -1 && xDistance == 0)
        {
            return true;
        }

        return false;
    }

    public static EnemyStatWrapper[] getAllMonsterStats()
    {
        EnemyStatWrapper[] statWrappers = new EnemyStatWrapper[allMovementTrackers.Count - 1];

        for (int index = 0; index < statWrappers.Length; index++)
        {
            EnemyMovement enemyMovement = allMovementTrackers[index + 1] as EnemyMovement;

            statWrappers[index] = new EnemyStatWrapper(enemyMovement.transform.position,
                                                        enemyMovement.enemyFacing.getFacing(),
                                                        enemyMovement.intimidateCounter,
                                                        enemyMovement.cunningStunCounter,
                                                        enemyMovement.retreatStunnedCounter);
        }

        return statWrappers;
    }



    public static SurpriseState determineSurprisedParty(Vector3 playerPosition, Vector3 enemyPosition, Facing enemyFacing)
    {
        Grid grid = AreaManager.getMasterGrid();

        Vector3Int playerCell = grid.WorldToCell(new Vector3(playerPosition.x, playerPosition.y, 0f));
        Vector3Int enemyCell = grid.WorldToCell(new Vector3(enemyPosition.x, enemyPosition.y, 0f));

        PlayerDirectionFromEnemy playerDirectionFromEnemy;
        SurpriseState surpriseState;

        if (playerCell.x > enemyCell.x && playerCell.y == enemyCell.y)
        {
            playerDirectionFromEnemy = PlayerDirectionFromEnemy.NorthEast;

        }
        else if (playerCell.x == enemyCell.x && playerCell.y > enemyCell.y)
        {
            playerDirectionFromEnemy = PlayerDirectionFromEnemy.NorthWest;

        }
        else if (playerCell.x == enemyCell.x && playerCell.y < enemyCell.y)
        {
            playerDirectionFromEnemy = PlayerDirectionFromEnemy.SouthEast;
        }
        else if (playerCell.x < enemyCell.x && playerCell.y == enemyCell.y)
        {
            playerDirectionFromEnemy = PlayerDirectionFromEnemy.SouthWest;
        }
        else
        {
            Debug.LogError("Could not determine PlayerDirectionFromEnemy:");
            Debug.LogError("playerCell = " + playerCell);
            Debug.LogError("enemyCell = " + enemyCell);

            return SurpriseState.NoOneSurprised;
        }

        if (playerDirectionFromEnemy == PlayerDirectionFromEnemy.NorthEast)
        {
            if ((State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.SouthWest) ||
               (State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.SouthEast) ||
               (State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.NorthWest) ||
               (State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.SouthWest) ||
               (State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.SouthWest))
            {
                surpriseState = SurpriseState.EnemySurprised;

            }
            else if ((State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.NorthEast) ||
                      (State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.NorthWest) ||
                      (State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.SouthWest) ||
                      (State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.NorthEast) ||
                      (State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.NorthEast))
            {
                surpriseState = SurpriseState.PlayerSurprised;

            }
            else
            {
                surpriseState = SurpriseState.NoOneSurprised;
            }
        }
        else if (playerDirectionFromEnemy == PlayerDirectionFromEnemy.NorthWest)
        {
            if ((State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.SouthEast) ||
                (State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.NorthEast) ||
                (State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.SouthWest) ||
                (State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.SouthEast) ||
                (State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.SouthEast))
            {
                surpriseState = SurpriseState.EnemySurprised;

            }
            else if ((State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.NorthWest) ||
                     (State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.NorthEast) ||
                     (State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.SouthWest) ||
                     (State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.NorthWest) ||
                     (State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.NorthWest))
            {
                surpriseState = SurpriseState.PlayerSurprised;
            }
            else
            {
                surpriseState = SurpriseState.NoOneSurprised;
            }
        }
        else if (playerDirectionFromEnemy == PlayerDirectionFromEnemy.SouthWest)
        {
            if ((State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.NorthEast) ||
                (State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.NorthWest) ||
                (State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.SouthEast) ||
                (State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.NorthEast) ||
                (State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.NorthEast))
            {
                surpriseState = SurpriseState.EnemySurprised;

            }
            else if ((State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.SouthWest) ||
                     (State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.NorthWest) ||
                     (State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.SouthEast) ||
                     (State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.SouthWest) ||
                     (State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.SouthWest))
            {
                surpriseState = SurpriseState.PlayerSurprised;

            }
            else
            {
                surpriseState = SurpriseState.NoOneSurprised;
            }
        }
        else if (playerDirectionFromEnemy == PlayerDirectionFromEnemy.SouthEast)
        {
            if ((State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.NorthWest) ||
                (State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.NorthEast) ||
                (State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.SouthWest) ||
                (State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.NorthWest) ||
                (State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.NorthWest))
            {
                surpriseState = SurpriseState.EnemySurprised;

            }
            else if ((State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.SouthEast) ||
                     (State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.NorthEast) ||
                     (State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.SouthWest) ||
                     (State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.SouthEast) ||
                     (State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.SouthEast))
            {
                surpriseState = SurpriseState.PlayerSurprised;

            }
            else
            {
                surpriseState = SurpriseState.NoOneSurprised;
            }
        }
        else
        {
            throw new IOException("Unable to determine who is surprised.");
        }

        //Debug.LogError("surpriseState = " + Helpers.enumToString(surpriseState));

        return surpriseState;
    }

    public static Vector3Int getCellWorld(Vector3 position)
    {
        return AreaManager.getMasterGrid().WorldToCell(position);
    }
}

    // public static Vector3Int[] directionMod;

    // public static Transform[] allSpritesToMove;
    // public static bool[] isSpritemovableObject;

    // public static Vector3[] startingPositions;
    // public static Vector3[] endingPositions;
    // public static bool[] isMoving;

        // return (allSpritesToMove[spriteID].position.x < startingPositions[spriteID].x && allSpritesToMove[spriteID].position.x > endingPositions[spriteID].x) ||
        //         (allSpritesToMove[spriteID].position.x > startingPositions[spriteID].x && allSpritesToMove[spriteID].position.x < endingPositions[spriteID].x) ||
        //         (allSpritesToMove[spriteID].position.y < startingPositions[spriteID].y && allSpritesToMove[spriteID].position.y > endingPositions[spriteID].y) ||
        //         (allSpritesToMove[spriteID].position.y > startingPositions[spriteID].y && allSpritesToMove[spriteID].position.y < endingPositions[spriteID].y) ||
        //          isMoving.Contains(true);

        // foreach (Transform sprite in allSpritesToMove)
        // {
        //     if (sprite == null || sprite is null)
        //     {
        //         continue;
        //     }

        //     Vector3 newposition = sprite.position;
        //     newposition.z = 0;
        //     sprite.position = newposition;
        // }

        // for (int i = 0; i < allSpritesToMove.Length; i++)
        // {
        //     if (allSpritesToMove[i] == null)
        //     {
        //         continue;
        //     }

        //     Vector3Int coords = grid.WorldToCell(allSpritesToMove[i].position);

        //     if (!isMoving[i])
        //     {
        //         directionMod[i] = determineDirection(i, coords);

        //         startingPositions[i] = allSpritesToMove[i].position;
        //         endingPositions[i] = grid.GetCellCenterWorld(coords + directionMod[i]);
        //     }
        // }

        // for (int i = playerSpriteIndex + 1; i < endingPositions.Length; i++)
        // {

        //     if (allSpritesToMove[i] == null)
        //     {
        //         continue;
        //     }

        //     Vector3Int currentMonstersCellCoords = grid.WorldToCell(new Vector3(endingPositions[i].x, endingPositions[i].y, 0f));

        //     for (int j = (i - 1); j >= 0; j--)
        //     {
        //         if (allSpritesToMove[j] == null)
        //         {
        //             continue;
        //         }

        //         Vector3Int previousMonstersCellCoords = grid.WorldToCell(new Vector3(endingPositions[j].x, endingPositions[j].y, 0f));

        //         if (currentMonstersCellCoords.Equals(previousMonstersCellCoords) &&
        //             (endingPositions[i].x != startingPositions[i].x &&
        //                 endingPositions[i].y != startingPositions[i].y))
        //         {
        //             endingPositions[i] = startingPositions[i];
        //             i = playerSpriteIndex;  //restart loop
        //         }
        //     }
        // }
        
        // for (int positionIndex = 1; positionIndex < positions.Length; positionIndex++)
        // {
        //     if (isSpritemovableObject[positionIndex])
        //     {
        //         continue;
        //     }

        //     if (cellsAreAdjacent(endingPositions[playerSpriteIndex], positions[positionIndex]) &&
        //         !MonsterDefeatKeysList.monsterIsDefeated(positionIndex - 1))
        //     {
        //         return positionIndex;
        //     }
        // }

        // for (int i = 0; i < allSpritesToMove.Length; i++)
        // {
        //     if (allSpritesToMove[i] == null)
        //     {
        //         continue;
        //     }

        //     if (!isMoving[i])
        //     {
        //         isMoving[i] = true;

        //         if (i > playerSpriteIndex && endingPositions[i] != startingPositions[i])
        //         {
        //             setEnemyFacing(directionMod[i], allSpritesToMove[i].GetComponent<EnemyMovement>());
        //         }

        //         StartCoroutine(moveSprite(i));

        //         if (!playerDirection.Equals(Vector3Int.zero))
        //         {
        //             PartyMemberMovement.showAllPartyMembers();
        //             movePartyMembers(0, grid.CellToWorld(getFirstPartyMemberEndingPosition()));
        //         }
        //     }
        // }
