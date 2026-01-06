using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PositionQuery
{

    public static Collider2D npcAtPosition(Vector3 position)
    {
        return Physics2D.OverlapCircle(position, Constants.detectionSize, LayerAndTagManager.npcLayerMask);
    }

    public static Collider2D chestAtPosition(Vector3 position)
    {
        return Physics2D.OverlapCircle(position, Constants.detectionSize, LayerAndTagManager.chestLayerMask);
    }

    public static Collider2D moveableObjectAtPosition(Vector3 position)
    {
        return Physics2D.OverlapCircle(position, Constants.detectionSize, LayerAndTagManager.moveableObjectLayerMask);
    }

    public static Collider2D moveableObjectBlockerAtPosition(Vector3 position)
    {
        return Physics2D.OverlapCircle(position, Constants.detectionSize, LayerAndTagManager.blocksMoveableObjectLayerMask);
    }


}
