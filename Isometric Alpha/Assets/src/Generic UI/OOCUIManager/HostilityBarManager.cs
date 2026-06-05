using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostilityBarManager : MonoBehaviour
{
    public Image zoneHostilityIcon;
    public Image locationHostilityIcon;

    public GameObject alertParent;

    public GameObject[] alertIcons;
    
    public void setUpHostilityBars()
    {
        if(locationShowsAsHostile())
        {
            setIconToHostile(locationHostilityIcon);
        } else
        {
            setIconToPeaceful(locationHostilityIcon);
        }

        if(zoneShowsAsHostile())
        {
            setIconToHostile(zoneHostilityIcon);
        } else
        {
            setIconToPeaceful(zoneHostilityIcon);
        }

        revealAlertIcons();
    }

    private bool zoneShowsAsHostile()
    {
        return AreaList.getCurrentAreaHostility() >= Area.hostilityThreshold;
    }

    private bool locationShowsAsHostile()
    {
        return AreaList.locationAlwaysHostile(AreaManager.locationName) || AreaList.areaIsHostile(AreaManager.locationName);
    }

    private void setIconToPeaceful(Image icon)
    {
        icon.color = ColorList.surpriseIconGreen;
        icon.sprite = Helpers.loadSpriteFromResources(IconList.flowerIcon);
    }

    private void setIconToHostile(Image icon)
    {
        icon.color = ColorList.surpriseIconRed;
        icon.sprite = Helpers.loadSpriteFromResources(IconList.hostileSkullIcon);
    }

    private void revealAlertIcons()
    {
        int alertIconsToShow = 0;

        if(!zoneShowsAsHostile())
        {
            alertIconsToShow = AreaList.getCurrentAreaHostility();
        }

        alertParent.SetActive(alertIconsToShow > 0);

        for(int i = 0; i < alertIcons.Length; i++)
        {
            alertIcons[i].SetActive(i < alertIconsToShow);
        }
    }

}
