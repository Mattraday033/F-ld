using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostilityBarManager : MonoBehaviour
{
    public Image[] hostilityBars;

    public GameObject[] flowerIcons;
    public GameObject[] alertIcons;
    public GameObject[] skullIcons;
    
    public void setUpHostilityBars()
    {
        int lowestGreenIndex = AreaList.getCurrentAreaHostility();

        if (lowestGreenIndex >= Area.hostilityThreshold)
        {
            for(int i = 0; i < hostilityBars.Length; i++)
            {
                setBarToHostile(i);
            }
            return;
        }
        else
        {
            for (int barIndex = 0; barIndex < hostilityBars.Length; barIndex++)
            {
                if (barIndex < lowestGreenIndex)
                {
                    setBarToAlerted(barIndex);
                }
                else
                {
                    if(defaultBarStateIsHostile())
                    {
                        setBarToHostile(barIndex);
                    } else
                    {
                        setBarToPeaceful(barIndex);
                    }
                }
            }
        }
    }

    private void setBarToPeaceful(int i)
    {
        hostilityBars[i].color = ColorList.surpriseIconGreen;

        flowerIcons[i].SetActive(true);
        alertIcons[i].SetActive(false);
        skullIcons[i].SetActive(false);
    }

    private void setBarToAlerted(int i)
    {
        hostilityBars[i].color = ColorList.surpriseIconYellow;

        flowerIcons[i].SetActive(false);
        alertIcons[i].SetActive(true);
        skullIcons[i].SetActive(false);
    }

    private void setBarToHostile(int i)
    {
        hostilityBars[i].color = ColorList.surpriseIconRed;

        flowerIcons[i].SetActive(false);
        alertIcons[i].SetActive(false);
        skullIcons[i].SetActive(true);
    }

    private bool defaultBarStateIsHostile()
    {
        return AreaList.locationAlwaysHostile(AreaManager.locationName);
    }
}
