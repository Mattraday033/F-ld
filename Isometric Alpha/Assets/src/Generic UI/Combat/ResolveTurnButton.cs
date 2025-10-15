using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolveTurnButton : MonoBehaviour
{

	public void resolveTurn()
	{
		CombatStateManager.getInstance().resolveTurn();
	}

}
