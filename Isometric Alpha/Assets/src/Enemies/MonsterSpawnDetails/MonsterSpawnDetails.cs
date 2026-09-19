using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterMovementType { Random, Stationary, Chases }

public class MonsterSpawnDetails : OOCSpawnDetails
{
    public const bool followsPlayer = true;

    public Facing facing;
    public MonsterMovementType movementType;

    public MonsterSpawnDetails(string npcName, 
                                Vector3Int cellCoords, 
                                Facing facing = Facing.Random, 
                                MonsterMovementType movementType = MonsterMovementType.Random, 
                                string tutorialTargetHash = "",
                                IAppearance appearance = null) :
    base(npcName, appearance: appearance, cellCoords: cellCoords, tutorialTargetHash: tutorialTargetHash)
    {
        this.facing =  facing;
        this.movementType = movementType;

        if(tutorialTargetHash.Length > 0)
        {
            this.movementType = MonsterMovementType.Stationary;
        }
    }

    // public override string getPrefabName()
    // {
    //     return PrefabNames.oocMonster;
    // }

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
        enemyMovement.characterFacing.currentFacing = facing;
        enemyMovement.movementType = movementType;
    }


    public override void spawnActions(GameObject interactable)
    {
        
    }
}

public class MovableObjectSpawnDetails: MonsterSpawnDetails
{
    private string spritePath;

    public MovableObjectSpawnDetails(string npcName, Vector3Int cellCoords, string spritePath, IAppearance appearance = null) :
    base(npcName, cellCoords, appearance: appearance)
    {
        this.facing = Facing.Random;
        this.movementType = MonsterMovementType.Random;
        this.spritePath = spritePath;
    }

    public MovableObjectSpawnDetails(string npcName, Vector3Int cellCoords, string spritePath, string tutorialTargetHash, IAppearance appearance = null) :
    base(npcName, cellCoords, appearance: appearance)
    {
        this.facing = Facing.Random;
        this.movementType = MonsterMovementType.Random;
        this.spritePath = spritePath;
        this.tutorialTargetHash = tutorialTargetHash;
    }

    // public override string getPrefabName()
    // {
    //     return PrefabNames.movableObject;
    // }

    public override Transform getParent()
    {
        return AreaManager.getMovableObjectParent();
    }

    public override void spawnActions(EnemyMovement enemyMovement)
    {
        // if (hasTutorialTargetHash())
        // {
        //     addTutorialTargetComponent(enemyMovement, tutorialTargetHash);
        // }

        // MovementManager.addMovementTracker(enemyMovement);

        // enemyMovement.getSpriteRenderer().sprite = SpriteUtil.loadSpriteFromResources(getSpriteName());
    }


    public override void spawnActions(GameObject interactable)
    {
        // base.spawnActions(interactable);
        
    }
}