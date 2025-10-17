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

    // public override bool determineSpriteAtSpawn()
    // {
    //     return false;
    // }

    public void spawnActions(EnemyMovement enemyMovement)
    {
        if (monsterDefeatKeyExists(enemyMovement.getMonsterPackIndex()))
        {
            enemyMovement.gameObject.SetActive(false);
        } else
        {
            enemyMovement.setEnemyFacing(facing);
            enemyMovement.followsPlayer = chasesPlayer;
            MovementManager.getInstance().addEnemySprite(enemyMovement.transform, enemyMovement.getMonsterPackIndex() + 1);
        }
    }


    public override void spawnActions(GameObject interactable)
    {
        // base.spawnActions(interactable);
        
    }

    private bool monsterDefeatKeyExists(int index)
    {
        if (State.monsterDefeatKeys.ContainsKey(getMonsterDefeatKey(index)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private string getMonsterDefeatKey(int index)
    {
        return AreaManager.locationName + "-" + index;
    }

}
