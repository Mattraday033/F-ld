using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterMovementType { Random, Stationary, Chases }

public class MonsterSpawnDetails : OOCSpawnDetails
{
    public const bool followsPlayer = true;

    public override Transform parent { get { return AreaManager.getMonsterParent(); } }
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

    public virtual void spawnActions(EnemyMovement enemyMovement)
    {
        // if (hasTutorialTargetHash())
        // {
        //     addTutorialTargetComponent(enemyMovement, tutorialTargetHash);
        // }

        MovementManager.addMovementTracker(enemyMovement);
        enemyMovement.initializeAnimationManager();
        enemyMovement.characterFacing.currentFacing = facing;
        enemyMovement.movementType = movementType;
    }
}

public class MovableObjectSpawnDetails: MonsterSpawnDetails
{

    public override Transform parent { get { return AreaManager.getMovableObjectParent(); } }

    public MovableObjectSpawnDetails(string npcName, Vector3Int cellCoords, string tutorialTargetHash = "", IAppearance appearance = null) :
    base(npcName, cellCoords, appearance: appearance, tutorialTargetHash: tutorialTargetHash)
    {
        this.facing = Facing.Random;
        this.movementType = MonsterMovementType.Random;
    }

    // public override string getPrefabName()
    // {
    //     return PrefabNames.movableObject;
    // }

    public override void spawnActions(EnemyMovement enemyMovement)
    {
        // if (hasTutorialTargetHash())
        // {
        //     addTutorialTargetComponent(enemyMovement, tutorialTargetHash);
        // }

        // MovementManager.addMovementTracker(enemyMovement);

        // enemyMovement.getSpriteRenderer().sprite = SpriteUtil.loadSpriteFromResources(getSpriteName());
    }
}