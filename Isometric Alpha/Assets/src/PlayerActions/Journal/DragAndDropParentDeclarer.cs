using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropParentDeclarer : MonoBehaviour
{

    public static Transform dragAndDropParent;

    private void Awake()
    {
        dragAndDropParent = transform;
    }

}
