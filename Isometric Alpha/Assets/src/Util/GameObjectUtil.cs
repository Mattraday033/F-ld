using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

public static class GameObjectUtil
{
    public static void updateGameObjectPosition(GameObject gObj)
    {
        gObj.SetActive(false);
        gObj.SetActive(true);
    }

    public static void updateGameObjectPosition(Transform transform)
    {
        transform.gameObject.SetActive(false);
        transform.gameObject.SetActive(true);
    }

    public static bool tagMatchesCriteria(GameObject combatSprite, string[] tagCriteria)
    {
        foreach (string tag in tagCriteria)
        {
            if (combatSprite.gameObject.tag.Equals(tag))
            {
                return true;
            }
        }

        return false;
    }

    public static GameObject createBlankGameObject(GameObject parent, string name = "New Object")
    {
        return createBlankGameObject(parent.transform, name);
    }

    public static GameObject createBlankGameObject(Transform parent, string name = "New Object")
    {
        GameObject template = new GameObject(name);

        GameObject blank = UnityObject.Instantiate(template, parent);
        
        UnityObject.Destroy(template);

        blank.transform.localPosition = Vector3.zero;

        updateGameObjectPosition(blank);

        return blank;
    }

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        
    }

}