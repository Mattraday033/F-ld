using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnDetails : OOCSpawnDetails
{
    public const bool followsPlayer = true;

    public Facing facing;
    public bool chasesPlayer;

    private string tutorialTargetHash = "";

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
        if (tutorialTargetHash.Length > 0)
        {
            enemyMovement.tutorialHash = tutorialTargetHash;
            enemyMovement.gameObject.AddComponent<RectTransform>();
        }

        string key = MonsterDefeatKeysList.generateMonsterDefeatKey(enemyMovement.getMonsterPackIndex());

        if (MonsterDefeatKeysList.monsterIsDefeated(key))
        {
            enemyMovement.gameObject.SetActive(false);
        }
        else
        {
            enemyMovement.setEnemyFacing(facing);
            enemyMovement.followsPlayer = chasesPlayer;
            AreaManager.getMovementManager().addEnemySprite(enemyMovement.transform, enemyMovement.getMonsterPackIndex() + 1);
        }
    }


    public override void spawnActions(GameObject interactable)
    {
        // base.spawnActions(interactable);
        
    }
}

public class MovableObjectSpawnDetails: MonsterSpawnDetails
{
    public MovableObjectSpawnDetails(string npcName, Vector3Int cellCoords) :
    base(npcName, cellCoords)
    {
        this.facing = Facing.Random;
        this.chasesPlayer = false;
    }

    public override string getSpriteName()
    {
        return null;
    }

    public override string getPrefabName()
    {
        return PrefabNames.movableObject;
    }

    public override Transform getParent()
    {
        return AreaManager.getMovableObjectParent();
    }

    public override void spawnActions(EnemyMovement enemyMovement)
    {
        enemyMovement.setEnemyFacing(Facing.Random);
        enemyMovement.followsPlayer = false;
        AreaManager.getMovementManager().addEnemySprite(enemyMovement.transform, enemyMovement.getMonsterPackIndex() + 1);
    }


    public override void spawnActions(GameObject interactable)
    {
        // base.spawnActions(interactable);
        
    }
}