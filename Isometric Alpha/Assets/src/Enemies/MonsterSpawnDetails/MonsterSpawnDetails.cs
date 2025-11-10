using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnDetails : OOCSpawnDetails
{
    public const bool followsPlayer = true;

    public Facing facing;
    public bool chasesPlayer;

    public MonsterSpawnDetails(string npcName, Vector3Int cellCoords) :
    base(npcName, cellCoords)
    {
        this.facing = Facing.Random;
        this.chasesPlayer = false;
    }

    public MonsterSpawnDetails(string npcName, Vector3Int cellCoords, Facing facing) :
    base(npcName, cellCoords)
    {
        this.facing = facing;
        this.chasesPlayer = false;
    }

    public MonsterSpawnDetails(string npcName, Vector3Int cellCoords, Facing facing, string tutorialTargetHash) :
    base(npcName, cellCoords)
    {
        this.facing = facing;
        this.chasesPlayer = false;
        this.tutorialTargetHash = tutorialTargetHash;
    }

    public MonsterSpawnDetails(string npcName, Vector3Int cellCoords, bool chasesPlayer) :
    base(npcName, cellCoords)
    {
        this.facing = Facing.Random;
        this.chasesPlayer = chasesPlayer;
    }

    public MonsterSpawnDetails(string npcName, Vector3Int cellCoords, bool chasesPlayer, Facing facing) :
    base(npcName, cellCoords)
    {
        this.facing = facing;
        this.chasesPlayer = chasesPlayer;
    }

    public override string getSpriteName()
    {
        return null;
    }

    public override string getPrefabName()
    {
        return PrefabNames.oocMonster;
    }

    public override Transform getParent()
    {
        return AreaManager.getMonsterParent();
    }

    // public override bool determineSpriteAtSpawn()
    // {
    //     return false;
    // }

    public virtual void spawnActions(EnemyMovement enemyMovement)
    {
        if (hasTutorialTargetHash())
        {
            addTutorialTargetComponent(enemyMovement, tutorialTargetHash);
        }

        MovementManager.addMovementTracker(enemyMovement);
        enemyMovement.initializeAnimationManager();
        enemyMovement.setEnemyFacing(facing);
        enemyMovement.followsPlayer = chasesPlayer;
    }


    public override void spawnActions(GameObject interactable)
    {
        // base.spawnActions(interactable); commented out until animations are implemented
    }
}

public class MovableObjectSpawnDetails: MonsterSpawnDetails
{
    private string spritePath;

    public MovableObjectSpawnDetails(string npcName, Vector3Int cellCoords, string spritePath) :
    base(npcName, cellCoords)
    {
        this.facing = Facing.Random;
        this.chasesPlayer = false;
        this.spritePath = spritePath;
    }

    public override string getPrefabName()
    {
        return PrefabNames.movableObject;
    }

    public override string getSpriteName()
    {
        return spritePath;
    }

    public override Transform getParent()
    {
        return AreaManager.getMovableObjectParent();
    }

    public override void spawnActions(EnemyMovement enemyMovement)
    {
        MovementManager.addMovementTracker(enemyMovement);

        enemyMovement.getSpriteRenderer().sprite = Helpers.loadSpriteFromResources(getSpriteName());
    }


    public override void spawnActions(GameObject interactable)
    {
        // base.spawnActions(interactable);
        
    }
}