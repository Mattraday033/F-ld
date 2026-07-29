using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class OverHeadIconManager : MonoBehaviour
{
    public Canvas canvas;
    public Transform iconParent;
    private Dictionary<OverHeadIconType, GameObject> icons = new Dictionary<OverHeadIconType, GameObject>();


    public void createOverHeadIcon(OverHeadIconType iconType, IOverHeadIconSource source = null, string nameOfNPC = null)
    {
        if(icons.ContainsKey(iconType))
        {
            return;
        }

        switch(iconType)
        {
            case OverHeadIconType.NameTag:

                if(hoveringOverIcon())
                {
                    return;
                }

                NPCNameTag nameTag = Instantiate(Resources.Load<GameObject>(PrefabNames.overHeadNameTag), iconParent).GetComponent<NPCNameTag>();

                nameTag.labelNPC(nameOfNPC);
                nameTag.transform.SetAsFirstSibling();

                if(iconParent.childCount > 1)
                {
                    nameTag.orientTransformSide();
                } else
                {
                    nameTag.orientTransformMiddle();
                }

                icons[iconType] = nameTag.gameObject;
                return;
            default:
                OverHeadIcon icon = Instantiate(Resources.Load<GameObject>(PrefabNames.overHeadIcon), iconParent).GetComponent<OverHeadIcon>();
                icon.source = source;
                icon.canvas = canvas;

                icon.setDisplay(iconType);
                icon.iconManager = this;

                icons[iconType] = icon.gameObject;

                if(icons.ContainsKey(OverHeadIconType.NameTag))
                {
                    icons[OverHeadIconType.NameTag].GetComponent<NPCNameTag>().orientTransformSide();
                }
                return;
        }
    }

    public bool hoveringOverIcon()
    {
        foreach(GameObject gameObject in icons.Values)
        {
            OverHeadIcon icon = gameObject.GetComponent<OverHeadIcon>();

            if(icon != null && icon.hovered)
            {
                return true;
            }
        }

        return false;
    }

    public void removeAllDestroyedIcons()
    {
        List<OverHeadIconType> iconKeys = new List<OverHeadIconType>(icons.Keys);

        foreach(OverHeadIconType key in iconKeys)
        {
            if(icons[key] == null)
            {
                icons.Remove(key);
            }
        }
    }

    public void destroyIcon(OverHeadIconType iconType)
    {
        if(icons.ContainsKey(iconType))
        {
            DestroyImmediate(icons[iconType]);
            removeAllDestroyedIcons();
        }
    }

}
