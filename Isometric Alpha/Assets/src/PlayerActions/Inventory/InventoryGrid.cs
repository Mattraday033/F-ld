using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class InventoryGrid : MonoBehaviour
{
    public GameObject gridLine1;
    public GameObject gridLine2;

    public GameObject armorIcon;
    public GameObject damageIcon;
    public GameObject critIcon;

    public void hideArmorColumn()
    {
        gridLine2.SetActive(false);
        armorIcon.SetActive(false);
    }

    public void showArmorColumn()
    {
        gridLine2.SetActive(true);
        armorIcon.SetActive(true);
    }

    public void hideOffHandColumns()
    {
        gridLine1.SetActive(false);
        gridLine2.SetActive(false);

        critIcon.SetActive(false);
        damageIcon.SetActive(false);
    }

    public void showOffHandColumns()
    {
        gridLine1.SetActive(true);
        gridLine2.SetActive(true);

        critIcon.SetActive(true);
        damageIcon.SetActive(true);
    }

}
