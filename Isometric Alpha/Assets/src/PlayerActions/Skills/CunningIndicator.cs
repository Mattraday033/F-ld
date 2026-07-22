using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CunningIndicator : SkillIndicator
{
    protected abstract ContactFilter2D getFilterCollider()
    {
        
    }

    public override void setColor()
    {
        // if (collidedWithTarget(tile))
        // {
        //     OnSkillTargetFound.Invoke();
        //     return getTileTargetColor();
        // }
        // else
        // {
        //     return getTileBaseColor();
        // }
    }

}
