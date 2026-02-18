using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatActionCounterIcon : MonoBehaviour
{

	
	public GameObject greenCheck;
	public GameObject redX;

    public void setToGreen()
    {
        greenCheck.SetActive(true);
        redX.SetActive(false);
    }

    public void setToRed()
    {
        greenCheck.SetActive(false);
        redX.SetActive(true);
    }

    public void setToInvisble()
    {
        greenCheck.SetActive(false);
        redX.SetActive(false);
    }

}
