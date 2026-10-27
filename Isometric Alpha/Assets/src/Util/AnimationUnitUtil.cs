using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class AnimationUnitUtil
{

    public readonly static UnityEvent haltAllAnimationUnits = new UnityEvent();

    public static void retireAllAnimationUnits()
    {
        haltAllAnimationUnits.RemoveAllListeners();
    }

}
