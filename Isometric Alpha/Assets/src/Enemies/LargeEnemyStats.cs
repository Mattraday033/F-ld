using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public class SpawnDetails
{
    public readonly static SpawnDetails topLeft2x2 = new SpawnDetails(new GridCoords[]{ new GridCoords(1,1), new GridCoords(1,0),
                                                                                        new GridCoords(0,1), new GridCoords(0,0) });
    public readonly static SpawnDetails topRight2x2 =  new SpawnDetails(new GridCoords[]{ new GridCoords(1,3), new GridCoords(0,3),
                                                                                        new GridCoords(1,2), new GridCoords(0,2) });
    public readonly static SpawnDetails bottomLeft2x2 = new SpawnDetails(new GridCoords[]{ new GridCoords(3,1), new GridCoords(3,0),
                                                                                        new GridCoords(2,1), new GridCoords(2,0) });
    public readonly static SpawnDetails bottomRight2x2 =  new SpawnDetails(new GridCoords[]{ new GridCoords(3,3), new GridCoords(2,3),
                                                                                        new GridCoords(3,2), new GridCoords(2,2) });

    public readonly static SpawnDetails middle2x2 =  new SpawnDetails(new GridCoords[]{ new GridCoords(2,2), new GridCoords(2,1),
                                                                                        new GridCoords(1,2), new GridCoords(1,1) });

	public bool hasSpawnDetails;
	public bool dontSpawnWhenSurprised;
	
	public GridCoords[] allSpawnPositions; //every coords that has a reference to the enemy's stats
	public GridCoords baseStatsPosition;   //the coords put into the "position" of the base class
	public GridCoords spritePosition;      //the coords that the sprite is placed at on the grid
	
	public SpawnDetails(GridCoords[] allSpawnPositions, bool dontSpawnWhenSurprised = false)
	{
		this.allSpawnPositions = allSpawnPositions;
		this.baseStatsPosition = allSpawnPositions[0];
		this.spritePosition = allSpawnPositions[0];
		this.dontSpawnWhenSurprised = dontSpawnWhenSurprised;
		
		this.hasSpawnDetails = true;
	}

    public List<Vector3> getAllSpawnWorldPositions()
    {
        List<Vector3> worldPositions = new List<Vector3>();

        foreach(GridCoords coords in allSpawnPositions)
        {
            worldPositions.Add(CombatGrid.getPositionAt(coords));
        }

        return worldPositions;
    }
}

public class LargeEnemyStats : EnemyStats
{

    #region Global Variables
    public SpawnDetails spawnDetails;

    #endregion

    #region Unity Events
    public readonly static UnityEvent OnLargeEnemySpawn = new UnityEvent();
    #endregion


    #region Constructors

    public LargeEnemyStats(string key, int armor, int tHP, Trait[] traits, CombatAction combatAction = null, Dictionary<CharacterAnimationType, SFXType> animationAudioClipDictionary = null) :
    base(key, armor, tHP, traits: traits, combatAction: combatAction, animationAudioClipDictionary: animationAudioClipDictionary)
    {
        if(!traits.Contains(TraitList.large) && !traits.Contains(TraitList.immobile))
        {
            traitContainer.addTrait(TraitList.large);
        }
    }

    #endregion

    #region Sprite and GameObject

    public override GameObject instantiateCombatSprite(List<GridCoords> coords)
    {
        return instantiateCombatSprite();
    }

    protected virtual GameObject instantiateCombatSprite()
    {
        if(spawnDetails == null)
        {
            spawnDetails = State.enemyPackInfo.getNextSpawnDetails();
            if(spawnDetails == null)
            {
                return null;
            }
        }

        positions = spawnDetails.allSpawnPositions.Select(p => p.clone()).ToList();

        CombatGrid.addCombatantToGrid(this);

        GameObject combatSpriteGameObject = base.instantiateCombatSprite(positions);

        setHealthBarToAverageWorldPosition();

        setHeartBeatRowByName();

        OnLargeEnemySpawn.Invoke();

        return combatSpriteGameObject;
    }

    public void setHealthBarToAverageWorldPosition()
    {
        healthBarManager.setPosition(Helpers.getAveragePosition(spawnDetails.getAllSpawnWorldPositions()));
    }

    public override bool multiSpaceEnemy()
    {
        return true;
    }

    #endregion

    #region AnimationManager

    public void setHeartBeatRowByName()
    {
        int heartBeatRow = 0;

        switch(getName())
        {
            case MonsterNameList.hiveHeraldNest:
                heartBeatRow = 0;
                break;
            case MonsterNameList.martyrWormNest:
                heartBeatRow = 1;
                break;
            case MonsterNameList.toxicWormNest:
                heartBeatRow = 2;
                break;
            case MonsterNameList.wormNest:
                heartBeatRow = 3;
                break;
            default:
                return;
        }

        animationManager.heartBeatRow = heartBeatRow;
    }

    #endregion

    #region Health

    #endregion

    #region Combat and Actions

    #endregion

    #region Traits

    public override bool isLarge()
    {
        return true;
    }

    #endregion

}
