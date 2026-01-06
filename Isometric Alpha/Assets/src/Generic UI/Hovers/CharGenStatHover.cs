using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CharGenStatHover : SlotIconHover
{
    public PrimaryStat primaryStat;
    
    public override void spawnHoverIcon()
    {
        MouseHoverManager.spawnCustomHover(this, transform, getHoverPrefabName());
    }

    private string getHoverPrefabName()
    {
        return primaryStat.ToString() + "";
    }

}
