using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpritePivotAdjuster : MonoBehaviour
{

    public Vector2 newPivot;
    public RectTransform rectTransform;


    void Start()
    {
        rectTransform.pivot = newPivot;
        Helpers.updateGameObjectPosition(gameObject);
    }
}
