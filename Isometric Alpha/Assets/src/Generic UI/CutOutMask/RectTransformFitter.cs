using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformFitter : MonoBehaviour
{
    public RectTransform rectTransform;


    void Start()
    {
        rectTransform.offsetMin = new Vector2(0f, rectTransform.offsetMin.y);
        rectTransform.offsetMax = new Vector2(0f, rectTransform.offsetMax.y);

        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 0f);
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 0f);
    }

}

