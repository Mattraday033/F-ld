using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using UnityEngine;

public class SkillTile : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;

    private void OnDisable()
    {
        if(spriteRenderer.color.Equals(Color.red))
        {
            IntimidateManager.decrementIntimidateTargets();
        }
    }
}