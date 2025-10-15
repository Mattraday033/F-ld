using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MonsterPackListManager : MonoBehaviour
{
	private static MonsterPackListManager instance;
	
	public static bool justLoaded = false;
	
	public MovementManager movementManager;
	
	public string[] flagsToShutOffSpawning;
	
    void Start()
    {
		if(!AreaList.currentSceneIsHostile() || shouldShutOffSpawning() || State.debugStopMonsterSpawning)
		{
			return;
		}
		
		if(State.currentMonsterPackList == null) //|| State.currentMonsterPackList.shouldReset
		{
			//AllMonsterPackLists.markAllPacksAsUndefeated();

			State.currentMonsterPackList = AllMonsterPackLists.getMonsterPackList(AreaManager.locationName);

			if (State.currentMonsterPackList != null)
			{
				State.currentMonsterPackList.setMonsterPackList();

				instantiateAllMonsterSprites();

				setAllMonstersToStartPositions();
			} 
		} else 
		{ 
			instantiateAllMonsterSprites();
		}

		deactivateAllDefeatedPacks();
		
		justLoaded = false;
    }
	
	public void deactivateAllDefeatedPacks()
	{
		if(State.currentMonsterPackList == null)
		{
			return;
		}
		
		for(int i = 0; i < State.currentMonsterPackList.monsterPacks.Length; i++)
		{
			MonsterPack pack = State.currentMonsterPackList.monsterPacks[i];

			if (monsterDefeatKeyExists(i) && pack.monsterSprite != null)
			{
				pack.monsterSprite.SetActive(false);
			}
			else if (pack.monsterSprite != null)
			{
				pack.monsterSprite.SetActive(true);
			}
		}
	
	}


	public void setAllMonstersToStartPositions()
	{
		for (int i = 0; i < State.currentMonsterPackList.monsterPacks.Length; i++)
		{
			MonsterPack pack = State.currentMonsterPackList.monsterPacks[i];

			pack.monsterSprite.transform.position = pack.startPosition;
		}
	}

    public void instantiateAllMonsterSprites()
	{
		for(int i = 0; i < State.currentMonsterPackList.monsterPacks.Length && i < getNumberOfSpawners(); i++)
		{
			State.currentMonsterPackList.monsterPacks[i] = instantiateMonsterSprite(i, State.currentMonsterPackList.monsterPacks[i]);
		}
	}

	public MonsterPack instantiateMonsterSprite(int i, MonsterPack mp)
	{
		GameObject monsterGameObject = Resources.Load<GameObject>(mp.spriteName);

		if(monsterGameObject == null || monsterGameObject is null)
		{
			return mp;
		}

        mp.monsterSprite = Instantiate(monsterGameObject, movementManager.grid.gameObject.transform, false);

		mp.setMovementManager(movementManager);
		
		EnemyMovement enemyMovement = mp.monsterSprite.GetComponent<EnemyMovement>();
		
		enemyMovement.setMonsterPackIndex(i);

		if(CharacterFacing.withinRange(mp.facingDirection))
		{
            enemyMovement.startingFacing = (Facing)mp.facingDirection;
        }
		else
		{
			enemyMovement.startingFacing = Facing.Random;
        }

		if (i >= State.currentMonsterPackList.monsterPacks.Length)
		{
			State.currentMonsterPackList.monsterPacks = Helpers.appendArray<MonsterPack>(State.currentMonsterPackList.monsterPacks, mp);
		}

		enemyMovement.setCunningCounter(mp.cunningCounter);
        enemyMovement.setIntimidateCounter(mp.intimidateCounter);
        enemyMovement.setRetreatStunCounter(mp.retreatCounter);

        mp = setSpritePosition(mp);
		
		return mp;
	}

	public MonsterPack instantiateMonsterSpriteAtStartingPosition(int i, MonsterPack mp)
	{
		mp.currentPosition = mp.startPosition;

		return instantiateMonsterSprite(i, mp);
	}

	private MonsterPack setSpritePosition(MonsterPack monsterPack)
	{

		//if State.currentMonsterPackList.monsterPacks[i].currentPosition isn't equal to 0,0,0 (default empty value)
		if (Vector3.Distance(monsterPack.currentPosition, new Vector3(0f, 0f, 0f)) != 0f)
		{
			//then load this monster at their current position (because it's not empty the player is loading in and expecting the monster to still be where they were when they saved)
			monsterPack.monsterSprite.transform.localPosition = monsterPack.currentPosition;
		}
		else
		{
			//using the spawner position needs to use .position instead of .localPosition because the spawner and monster gameObject will not be siblings

			monsterPack.startPosition = getSpawnerPosition(monsterPack.index);
			monsterPack.monsterSprite.transform.position = monsterPack.startPosition;
			monsterPack.currentPosition = monsterPack.monsterSprite.transform.localPosition;
		}

		Helpers.updateGameObjectPosition(monsterPack.monsterSprite);

		return monsterPack;
	}

	private Vector3 getSpawnerPosition(int index)
	{	
		return movementManager.grid.GetCellCenterWorld(movementManager.grid.WorldToCell(transform.GetChild(index).position));
	}
	
	private int getNumberOfSpawners()
	{	
		return transform.childCount;
	}
	
	private bool shouldShutOffSpawning()
	{
		foreach(string flag in flagsToShutOffSpawning)
		{
			if(Flags.getFlag(flag))
			{
				return true;
			}
		}
		
		return false;
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

	public static MonsterPackListManager getInstance()
	{
		return instance;
	}

	private void Awake()
	{
		if(instance != null)
		{
			throw new IOException("There are 2 instances of MonsterPackListManager");
		}
		
		instance = this;
	}

}
