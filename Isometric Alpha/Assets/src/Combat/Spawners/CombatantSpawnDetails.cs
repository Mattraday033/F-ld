using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//builds a combatant's GameObjects from its Stats, the same way OOCSpawnDetails builds an NPC.
//Also doubles as a spawn layout: built from just a set of cells, it's a template an EnemyPackInfo hands out to its enemies
public class CombatantSpawnDetails : OOCSpawnDetails
{
    #region Layouts

    public readonly static CombatantSpawnDetails topLeft2x2 = new CombatantSpawnDetails(new GridCoords[]{ new GridCoords(1,1), new GridCoords(1,0),
                                                                                                        new GridCoords(0,1), new GridCoords(0,0) });
    public readonly static CombatantSpawnDetails topRight2x2 = new CombatantSpawnDetails(new GridCoords[]{ new GridCoords(1,3), new GridCoords(0,3),
                                                                                                        new GridCoords(1,2), new GridCoords(0,2) });
    public readonly static CombatantSpawnDetails bottomLeft2x2 = new CombatantSpawnDetails(new GridCoords[]{ new GridCoords(3,1), new GridCoords(3,0),
                                                                                                        new GridCoords(2,1), new GridCoords(2,0) });
    public readonly static CombatantSpawnDetails bottomRight2x2 = new CombatantSpawnDetails(new GridCoords[]{ new GridCoords(3,3), new GridCoords(2,3),
                                                                                                        new GridCoords(3,2), new GridCoords(2,2) });
    public readonly static CombatantSpawnDetails middle2x2 = new CombatantSpawnDetails(new GridCoords[]{ new GridCoords(2,2), new GridCoords(2,1),
                                                                                                        new GridCoords(1,2), new GridCoords(1,1) });

    public readonly static CombatantSpawnDetails barricade = new CombatantSpawnDetails(new GridCoords[] {
                                                                                        new GridCoords(Constants.indexThree, Constants.indexZero),
                                                                                        new GridCoords(Constants.indexThree, Constants.indexOne),
                                                                                        new GridCoords(Constants.indexThree, Constants.indexTwo),
                                                                                        new GridCoords(Constants.indexThree, Constants.indexThree)
                                                                                      }, true);

    //every cell that points at the combatant's Stats on the CombatGrid; the first is the one that carries the health bar
    public readonly GridCoords[] allSpawnPositions;
    public readonly bool dontSpawnWhenSurprised;

    public CombatantSpawnDetails(GridCoords[] allSpawnPositions, bool dontSpawnWhenSurprised = false)
    {
        this.allSpawnPositions = allSpawnPositions;
        this.dontSpawnWhenSurprised = dontSpawnWhenSurprised;
    }

    #endregion

    public readonly Stats stats;
    private readonly bool placeholder;
    private readonly GridCoords primaryCell;

    //filled in as each GameObject is placed, so CombatantSpawnBehaviour can tell which cell a GameObject stands on
    private readonly Dictionary<GameObject, GridCoords> cellsByGameObject = new Dictionary<GameObject, GridCoords>();

    //positions must already be resolved, see forSpawn. A placeholder is the see-through stand in shown where a combatant is about to reposition to
    public CombatantSpawnDetails(Stats stats, List<GridCoords> positions, bool placeholder = false) :
    base(stats.displayName,
         appearance: stats.appearance,
         cellCoords: getPrimaryCell(stats, positions).toVector3Int(),
         facing: getFacing(positions),
         tutorialTargetHash: placeholder ? "" : stats.getTutorialTargetHash(),
         extraSpaces: getExtraSpaces(stats, positions))
    {
        this.stats = stats;
        this.placeholder = placeholder;
        this.allSpawnPositions = positions.Select(p => p.clone()).ToArray();
        this.primaryCell = getPrimaryCell(stats, positions);

        //the stats are the appearance source, so a change of equipment shows on the combatant
        aestheticSpawnBehaviours[typeof(AnimationManagerSpawnBehaviour)] = new AnimationManagerSpawnBehaviour(stats, facing, getIdleType(positions));

        //universal, since only universal behaviours can be the ActivationListener's name source
        universalSpawnBehaviours[typeof(CombatantSpawnBehaviour)] = new CombatantSpawnBehaviour(this);
    }

    #region OOCSpawnDetails Overrides

    public override string uniqueName { get { return stats != null ? stats.uniqueName : base.uniqueName; } }

    public override string prefabName { get { return PrefabNames.creaturePrefab; } }

    public override Transform parent { get { return CombatStateManager.getCreatureParent(); } }

    public override Vector3 localScale
    {
        get
        {
            if(stats != null && stats.uniqueName.Contains(NPCNameList.barricade))
            {
                return Constants.reverseScaleChange;
            }

            return Vector3.one;
        }
    }

    //Selector and SelectionInfo tell allies, enemies and allied summons apart by these tags
    protected override string tag
    {
        get
        {
            if(stats is AlliedSummonStats)
            {
                return LayerAndTagManager.npcTag;
            }

            if(stats is AllyStats)
            {
                return LayerAndTagManager.playerTag;
            }

            return LayerAndTagManager.enemyTag;
        }
    }

    //a MultiAnimationEnemyStats is drawn on every cell it covers, while any other multi cell combatant is one sprite
    protected override bool extraSpacesAreVisible { get { return stats is MultiAnimationEnemyStats; } }

    protected override void setBlankPrefabPosition(Vector3Int cell, Transform transform)
    {
        GridCoords coords = GridCoords.fromVector3Int(cell);

        transform.position = CombatGrid.getPositionAt(coords);

        cellsByGameObject[transform.gameObject] = coords;
    }

    #endregion

    #region Spawning

    //works out where the Stats should stand, since an enemy pack's layout overrides the coords the Stats asked for.
    //Returns null when a large enemy has no layout to spawn into
    public static CombatantSpawnDetails forSpawn(Stats stats, List<GridCoords> coords)
    {
        List<GridCoords> positions = resolvePositions(stats, coords);

        if(positions == null || positions.Count == 0)
        {
            return null;
        }

        return new CombatantSpawnDetails(stats, positions);
    }

    private static List<GridCoords> resolvePositions(Stats stats, List<GridCoords> coords)
    {
        if(stats is MultiAnimationEnemyStats multiAnimationEnemy)
        {
            if(multiAnimationEnemy.spawnLayout == null)
            {
                multiAnimationEnemy.spawnLayout = multiAnimationEnemy.uniqueName.Contains(NPCNameList.barricade) ? barricade : State.enemyPackInfo.getNextSpawnDetails();
            }

            return layoutPositions(multiAnimationEnemy.spawnLayout);
        }

        if(stats is LargeEnemyStats largeEnemy)
        {
            if(largeEnemy.spawnLayout == null)
            {
                largeEnemy.spawnLayout = obtainLayout(largeEnemy);
            } else
            {
                largeEnemy.obtainedSpawnLayout = true;
            }

            return layoutPositions(largeEnemy.spawnLayout);
        }

        if(stats is EnemyStats enemy)
        {
            CombatantSpawnDetails layout = obtainLayout(enemy);

            if(layout != null)
            {
                return layoutPositions(layout);
            }
        }

        return coords;
    }

    //an enemy only ever takes one layout from its pack, however many times it's spawned
    private static CombatantSpawnDetails obtainLayout(EnemyStats enemy)
    {
        CombatantSpawnDetails layout = null;

        if(!enemy.obtainedSpawnLayout)
        {
            layout = State.enemyPackInfo.getNextSpawnDetails();
        }

        enemy.obtainedSpawnLayout = true;

        return layout;
    }

    private static List<GridCoords> layoutPositions(CombatantSpawnDetails layout)
    {
        if(layout == null)
        {
            return null;
        }

        return layout.allSpawnPositions.Select(p => p.clone()).ToList();
    }

    public List<Combatant> spawnCombatant()
    {
        if(!placeholder)
        {
            //positions are replaced before touching the grid, since a party member's Stats still hold the cells of its last combat.
            //The sprite is placed by the spawn itself, so only the grid needs updating here
            stats.positions = allSpawnPositions.Select(p => p.clone()).ToList();
            CombatGrid.addCombatantToGrid(stats);
        }

        List<Combatant> combatants = spawnInteractables().Select(g => g.GetComponent<Combatant>())
                                                         .Where(c => c != null)
                                                         .ToList();

        if(stats is LargeEnemyStats largeEnemy && !placeholder)
        {
            int heartBeatRow = largeEnemy.getHeartBeatRow();

            if(heartBeatRow != LargeEnemyStats.noHeartBeatRow)
            {
                foreach(Combatant combatant in combatants)
                {
                    if(combatant.animationManager != null)
                    {
                        combatant.animationManager.setHeartBeatRow(heartBeatRow);
                    }
                }
            }

            LargeEnemyStats.OnLargeEnemySpawn.Invoke();
        }

        return combatants;
    }

    public GridCoords getCell(GameObject gameObject)
    {
        return cellsByGameObject.TryGetValue(gameObject, out GridCoords cell) ? cell : primaryCell;
    }

    public bool isPrimaryCell(GridCoords cell)
    {
        return cell.Equals(primaryCell);
    }

    public bool isPlaceholder { get { return placeholder; } }

    #endregion

    #region Cells and Facing

    //the sprite of a single sprite combatant stands on its lowest cell on screen, which has the highest row + col
    public static GridCoords getSpriteCell(List<GridCoords> positions)
    {
        int coordsSum = 0;
        GridCoords spriteCell = positions[0];

        foreach(GridCoords position in positions)
        {
            if(position.sum() > coordsSum)
            {
                coordsSum = position.sum();
                spriteCell = position;
            }
        }

        return spriteCell;
    }

    private static GridCoords getPrimaryCell(Stats stats, List<GridCoords> positions)
    {
        if(stats is MultiAnimationEnemyStats)
        {
            return positions[0];
        }

        return getSpriteCell(positions);
    }

    private static Vector3Int[] getExtraSpaces(Stats stats, List<GridCoords> positions)
    {
        if(!(stats is MultiAnimationEnemyStats))
        {
            return new Vector3Int[0];
        }

        return positions.Skip(1).Select(p => p.toVector3Int()).ToArray();
    }

    private static bool onAlliedSide(List<GridCoords> positions)
    {
        return CombatGrid.positionIsOnAlliedSide(positions[0]);
    }

    //allies have their backs to the camera and face the enemies across the grid
    private static Facing getFacing(List<GridCoords> positions)
    {
        return onAlliedSide(positions) ? Facing.NorthEast : Facing.SouthWest;
    }

    private static CharacterAnimationType getIdleType(List<GridCoords> positions)
    {
        return onAlliedSide(positions) ? CharacterAnimationType.Idle_Back : CharacterAnimationType.Idle_Front;
    }

    #endregion
}
