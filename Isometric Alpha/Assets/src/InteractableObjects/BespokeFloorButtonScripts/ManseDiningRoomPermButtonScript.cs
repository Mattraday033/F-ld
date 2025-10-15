using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManseDiningRoomPermButtonScript : MonoBehaviour, IFloorButtonScript
{
	public GameObject northWestDiningRoomDoors;
	public const string gateKey = "ManseDiningRoomDoors";
	private const string activatedFlag = "manseDiningRoomPermButtonScriptActivated";
	public MonsterPack monsterToSpawn;

	private void Awake()
	{
		if (hasBeenActivated())
		{
			gameObject.GetComponent<SpriteRenderer>().color = Color.green;
		}
    }

    public void activate()
	{
		if (hasBeenActivated())
		{
			return;
		}

		FadeToBlackManager.getInstance().setAndStartFadeToBlack();

		StartCoroutine(handleCombatActionsAfterScreenIsBlack());

		declareHasBeenActivated();
	}
	
	private IEnumerator handleCombatActionsAfterScreenIsBlack()
	{
		while(!FadeToBlackManager.isBlack())
        {
            yield return null;
        }
		
		GateAndChestManager.addKey(gateKey);
		northWestDiningRoomDoors.SetActive(false);
		

		monsterToSpawn = MonsterPackListManager.getInstance().instantiateMonsterSprite(monsterToSpawn.index, monsterToSpawn);

		MovementManager.getInstance().addEnemySprite(monsterToSpawn.monsterSprite.transform, monsterToSpawn.index+1);
		
		FadeToBlackManager.getInstance().setAndStartFadeBackIn();
	}
	
	public bool hasBeenActivated()
	{
		return Flags.getFlag(activatedFlag);
	}
	
	public void declareHasBeenActivated()
	{
		Flags.setFlag(activatedFlag, true);
	}
	
}
