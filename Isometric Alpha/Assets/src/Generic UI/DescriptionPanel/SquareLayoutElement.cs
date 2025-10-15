using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SquareLayoutElement : MonoBehaviour
{

    public LayoutElement layoutElement;

    public Transform test;
    //[SerializeField]
    public float singleDimension;

    private void OnValidate()
    {
        if (test == null)
        {
            return;
        }

        layoutElement.minWidth = singleDimension;
        layoutElement.preferredWidth = singleDimension;
        layoutElement.minHeight = singleDimension;
        layoutElement.preferredHeight = singleDimension;
    }

}
