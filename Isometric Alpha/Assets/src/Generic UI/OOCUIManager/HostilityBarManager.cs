using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostilityBarManager : MonoBehaviour
{
    public Image[] hostilityBars;
    
    public void setUpHostilityBars()
    {
        int lowestGreenIndex = AreaList.getCurrentAreaHostility();

        if (lowestGreenIndex >= Area.hostilityThreshold)
        {
            setAllHostilityBarsToRed();
            return;
        }
        else
        {
            for (int barIndex = 0; barIndex < hostilityBars.Length; barIndex++)
            {
                if (barIndex < lowestGreenIndex)
                {
                    hostilityBars[barIndex].color = Color.yellow;
                }
                else
                {
                    hostilityBars[barIndex].color = getMainBarColor();
                }
            }
        }
    }

    private void setAllHostilityBarsToRed()
    {
        foreach (Image bar in hostilityBars)
        {
            bar.color = Color.red;
        }
    }

    private Color getMainBarColor()
    {
        if (AreaList.locationAlwaysHostile(AreaManager.locationName))
		{
			return Color.red;
		} else
        {
            return Color.green;
        }
    }
}
