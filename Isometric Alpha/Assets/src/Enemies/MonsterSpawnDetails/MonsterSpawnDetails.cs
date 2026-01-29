using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterMovementType { Random, Stationary, Chases }

public class MonsterSpawnDetails : OOCSpawnDetails
{
    public const bool followsPlayer = true;

    public Facing facing;
    public MonsterMovementType movementType;

    public MonsterSpawnDetails(string npcName, Vector3Int cellCoords) :
    base(npcName, cellCoords)
    {
        this.facing = Facing.Random;
        this.movementType = MonsterMovementType.Random;
    }

    public MonsterSpawnDetails(string npcName, Vector3Int cellCoords, Facing facing) :
    base(npcName, cellCoords)
    {
        this.facing = facing;
        this.movementType = MonsterMovementType.Random;
    }

    public MonsterSpawnDetails(string npcName, Vector3Int cellCoords, Facing facing, string tutorialTargetHash) :
    base(npcName, cellCoords)
    {
        this.facing = facing;
        this.movementType = MonsterMovementType.Stationary;
        this.tutorialTargetHash = tutorialTargetHash;
    }

    public MonsterSpawnDetails(string npcName, Vector3Int cellCoords, MonsterMovementType movementType) :
    base(npcName, cellCoords)
    {
        this.facing = Facing.Random;
        this.movementType = movementType;
    }

    public MonsterSpawnDetails(string npcName, Vector3Int cellCoords, MonsterMovementType movementType, Facing facing) :
    base(npcName, cellCoords)
    {
        this.facing = facing;
        this.movementType = movementType;
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

    public virtual void spawnActions(EnemyMovement enemyMovement)
    {
        if (hasTutorialTargetHash())
        {
            addTutorialTargetComponent(enemyMovement, tutorialTargetHash);
        }

        MovementManager.addMovementTracker(enemyMovement);
        enemyMovement.initializeAnimationManager();
        enemyMovement.setFacing(facing);
        enemyMovement.movementType = movementType;
    }


    public override void spawnActions(GameObject interactable)
    {
        
    }
}

public class MovableObjectSpawnDetails: MonsterSpawnDetails
{
    private string spritePath;

    public MovableObjectSpawnDetails(string npcName, Vector3Int cellCoords, string spritePath) :
    base(npcName, cellCoords)
    {
        this.facing = Facing.Random;
        this.movementType = MonsterMovementType.Random;
        this.spritePath = spritePath;
    }

    public MovableObjectSpawnDetails(string npcName, Vector3Int cellCoords, string spritePath, string tutorialTargetHash) :
    base(npcName, cellCoords)
    {
        this.facing = Facing.Random;
        this.movementType = MonsterMovementType.Random;
        this.spritePath = spritePath;
        this.tutorialTargetHash = tutorialTargetHash;
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
        if (hasTutorialTargetHash())
        {
            addTutorialTargetComponent(enemyMovement, tutorialTargetHash);
        }

        MovementManager.addMovementTracker(enemyMovement);

        enemyMovement.getSpriteRenderer().sprite = Helpers.loadSpriteFromResources(getSpriteName());
        // enemyMovement.packName = npcName;
    }


    public override void spawnActions(GameObject interactable)
    {
        // base.spawnActions(interactable);
        
    }
}

public class BossPackSpawnDetails: MonsterSpawnDetails
{
    public BossPackSpawnDetails(string npcName, Vector3Int cellCoords) :
    base(npcName, cellCoords, MonsterMovementType.Stationary, Facing.Random)
    {
        
    }
}