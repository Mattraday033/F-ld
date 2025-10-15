using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnChecker : MonoBehaviour
{
	public string boolKey;

	public bool spawnOnFlag = true; //if true, turn on if the flag is true
									//if false, turn off if flag is true

	public int strReq = -1;
	public int dexReq = -1;
	public int wisReq = -1;
	public int chaReq = -1;

	// Start is called before the first frame update 
	void Start()
	{
		if (spawnOnFlag)
		{
			if (Flags.getFlag(boolKey))
			{
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
				return;
			}
		}
		else
		{
			if (Flags.getFlag(boolKey))
			{
				gameObject.SetActive(false);
				return;
			}
			else
			{
				gameObject.SetActive(true);
			}
		}

		if (strReq > 0 && strReq > PartyManager.getPlayerStats().getStrength())
		{
			gameObject.SetActive(false);
		}

		if (dexReq > 0 && dexReq > PartyManager.getPlayerStats().getDexterity())
		{
			gameObject.SetActive(false);
		}

		if (wisReq > 0 && wisReq > PartyManager.getPlayerStats().getWisdom())
		{
			gameObject.SetActive(false);
		}

		if (chaReq > 0 && chaReq > PartyManager.getPlayerStats().getCharisma())
		{
			gameObject.SetActive(false);
		}
	}
	
	private void OnEnable()
	{
		createListeners();
	}

	private void OnDisable()
	{
		destroyListeners();
	}

	//IRevealable interface methods

	public void createListeners()
	{
		RevealManager.OnReveal.AddListener(onReveal);
	}

	public void destroyListeners()
	{
		RevealManager.OnReveal.RemoveListener(onReveal);
	}

	public void onReveal()
	{
		RevealManager.setRevealForGameObject(gameObject, getRevealColor());
	}

	public Color getRevealColor()
	{
		return RevealManager.canBeInteractedWith;
	}
}
