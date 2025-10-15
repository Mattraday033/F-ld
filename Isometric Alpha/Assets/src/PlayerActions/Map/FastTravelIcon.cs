using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastTravelIcon : MonoBehaviour
{

    public Image backgroundImage;
    public Image arrowIconImage;

    public void disableFastTravelIcon()
    {
        gameObject.SetActive(false);
    }

    public void setToFastTravelAllowed()
    {
        backgroundImage.color = Color.black;
        arrowIconImage.color = Color.green;
    }

    public void setToFastTravelNotAllowed()
    {
        backgroundImage.color = Color.red;
        arrowIconImage.color = Color.black;
    }

}
