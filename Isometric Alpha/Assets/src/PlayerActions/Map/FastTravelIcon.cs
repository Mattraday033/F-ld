using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastTravelIcon : MonoBehaviour
{

    public Image interiorImage;
    public Image arrowIconImage;

    public void disableFastTravelIcon()
    {
        gameObject.SetActive(false);
    }

    public void setToFastTravelAllowed()
    {
        interiorImage.color = ColorList.surpriseIconGreen;
        // arrowIconImage.color = ColorList.grey25;
    }

    public void setToFastTravelNotAllowed()
    {
        interiorImage.color = ColorList.surpriseIconRed;
        // arrowIconImage.color = ColorList.grey25;
    }

}
