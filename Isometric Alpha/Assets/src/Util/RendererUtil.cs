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

        //the physics shape is in unflipped sprite space, while flipping mirrors the rendered sprite around its pivot (local origin)
        if(spriteRenderer.flipX || spriteRenderer.flipY)
        {
            for(int index = 0; index < pointsList.Count; index++)
            {
                Vector2 point = pointsList[index];

                pointsList[index] = new Vector2(spriteRenderer.flipX ? -point.x : point.x,
                                                spriteRenderer.flipY ? -point.y : point.y);
            }
        }

        polygonCollider2D.points = pointsList.ToArray();
    }
}
