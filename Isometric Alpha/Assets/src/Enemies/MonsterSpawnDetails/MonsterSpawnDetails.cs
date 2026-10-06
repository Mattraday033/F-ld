using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterMovementType { Random, Stationary, Chases }

public class MonsterSpawnDetails : OOCSpawnDetails
{
    public const bool followsPlayer = true;

    public override Transform parent { get { return AreaManager.getMonsterParent(); } }
    protected override string tag { get { return LayerAndTagManager.enemyTag; } }
    protected override int layer { get { return LayerAndTagManager.enemyLayer; } }

    //monsters are keyed by their pack index, which MonsterSpawnDetailsList assigns from their place in the area's list
    public override SpawnParams spawnParams { get { return SpawnParamsList.getMonsterSpawnParams(AreaManager.locationName, index.ToString()); } }

    public MonsterMovementType movementType;

    public MonsterSpawnDetails(string displayName,
                                Vector3Int cellCoords,
                                Facing facing = Facing.Random,
                                MonsterMovementType movementType = MonsterMovementType.Random,
                                string tutorialTargetHash = "",
                                IAppearance appearance = null) :
    base(displayName, appearance: appearance, cellCoords: cellCoords, facing: facing, tutorialTargetHash: tutorialTargetHash)
    {
        this.movementType = movementType;

        if(tutorialTargetHash.Length > 0)
        {
            this.movementType = MonsterMovementType.Stationary;
        }

        aestheticSpawnBehaviours[typeof(AnimationManagerSpawnBehaviour)] = new AnimationManagerSpawnBehaviour(this, facing);
        aestheticSpawnBehaviours[typeof(NPCMouseHoverSpawnBehaviour)] = new NPCMouseHoverSpawnBehaviour();

        //the icon manager's spawn params check looks the monster up as an NPC, which would bring a defeated monster back when a secret door is found
        aestheticSpawnBehaviours[typeof(OverHeadIconManagerSpawnBehaviour)] = new OverHeadIconManagerSpawnBehaviour(ignoresSecretDoors: true);

        universalSpawnBehaviours[typeof(EnemyMovementSpawnBehaviour)] = new EnemyMovementSpawnBehaviour(this);
    }
}

public class MovableObjectSpawnDetails: MonsterSpawnDetails
{

    public override Transform parent { get { return AreaManager.getMovableObjectParent(); } }
    protected override string tag { get { return LayerAndTagManager.untaggedTag; } }
    protected override int layer { get { return LayerAndTagManager.movableObjectLayer; } }

    public MovableObjectSpawnDetails(string displayName, Vector3Int cellCoords, string tutorialTargetHash = "", IAppearance appearance = null) :
    base(displayName, cellCoords, appearance: appearance, tutorialTargetHash: tutorialTargetHash)
    {
        this.facing = Facing.Random;
        this.movementType = MonsterMovementType.Random;

        //a movable object is a static sprite: an animation manager would flip it to match the direction it was pushed in
        aestheticSpawnBehaviours.Remove(typeof(AnimationManagerSpawnBehaviour));

        //movable objects get no mouse hover or overhead icons
        aestheticSpawnBehaviours.Remove(typeof(NPCMouseHoverSpawnBehaviour));
        aestheticSpawnBehaviours.Remove(typeof(OverHeadIconManagerSpawnBehaviour));

        universalSpawnBehaviours[typeof(EnemyMovementSpawnBehaviour)] = new MovableObjectMovementSpawnBehaviour(this);
    }
}
