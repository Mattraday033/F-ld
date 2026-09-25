using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RendererUtil
{
    public static void updatePolygonCollider(SpriteRenderer spriteRenderer, PolygonCollider2D polygonCollider2D)
    {
        if(spriteRenderer.sprite == null || spriteRenderer.sprite.GetPhysicsShapeCount() <= 0)
        {
            return;
        }

        List<Vector2> pointsList = new List<Vector2>();

        spriteRenderer.sprite.GetPhysicsShape(0, pointsList);

        polygonCollider2D.points = pointsList.ToArray();
    }
}
