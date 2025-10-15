using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTarget : MonoBehaviour
{
	public bool active;
	public GateSpawnChecker gateSpawnChecker;

	private void Awake()
	{
		if (gateSpawnChecker == null)
		{
			gateSpawnChecker = GetComponent<GateSpawnChecker>();
		}
	}

	public void activate()
	{
		gameObject.SetActive(true);

		active = true;
	}

	public void deactivate()
	{
		gameObject.SetActive(false);
		
		active = false;
	}
	
	public void setToOpened()
	{
		gateSpawnChecker.setToOpened();
	}

	public void setToOpenedPermanently()
	{
		gateSpawnChecker.setToOpenedPermanently();
	}

	public void flip()
	{
		gameObject.SetActive(!active);

		active = !active;
	}

}
