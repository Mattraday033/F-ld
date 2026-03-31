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
    public GameObject invulnerabilityIcon;
    public GameObject armorIcon;
    public GameObject damageIcon;
    public GameObject critIcon;

    public void hideArmorColumn()
    {
        invulnerabilityIcon.SetActive(false);
        armorIcon.SetActive(false);
    }

    public void showArmorColumn()
    {
        invulnerabilityIcon.SetActive(true);
        armorIcon.SetActive(true);
    }

    public void hideOffHandColumns()
    {
        critIcon.SetActive(false);
        damageIcon.SetActive(false);
    }

    public void showOffHandColumns()
    {
        critIcon.SetActive(true);
        damageIcon.SetActive(true);
    }

}
