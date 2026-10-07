using System;
using UnityEngine;

//where a layer's sprite sits on one frame of the body it's drawn over, worked out ahead of time.
//a layer smaller than the body frame (a 48x48 head canvas) has its renderer scaled up to cover the frame, and the
//tiling and offset then shrink its texture back to size and slide it to the canvas's position
public readonly struct CanvasPlacement : IEquatable<CanvasPlacement>
{
    //a layer that fills the body frame, which is every layer that isn't a small canvas
    public static readonly CanvasPlacement identity = new CanvasPlacement(Vector3.one, Vector2.one, Vector2.zero, Vector2.one, Vector2.zero);

    public readonly Vector3 scale;

    public readonly Vector2 layerTiling;
    public readonly Vector2 layerOffset;

    //the outline samples every layer at the body frame's place in the body's sheet, so these also undo the frame's position in that sheet
    public readonly Vector2 outlineTiling;
    public readonly Vector2 outlineOffset;

    private CanvasPlacement(Vector3 scale, Vector2 layerTiling, Vector2 layerOffset, Vector2 outlineTiling, Vector2 outlineOffset)
    {
        this.scale = scale;
        this.layerTiling = layerTiling;
        this.layerOffset = layerOffset;
        this.outlineTiling = outlineTiling;
        this.outlineOffset = outlineOffset;
    }

    //only a sprite that fits inside the body frame is a canvas. Anything else is drawn the way it always was
    public static bool isCanvas(Sprite bodyFrame, Sprite layerSprite)
    {
        if(bodyFrame == null || layerSprite == null)
        {
            return false;
        }

        return layerSprite.rect.width < bodyFrame.rect.width && layerSprite.rect.height < bodyFrame.rect.height;
    }

    //topLeft is the canvas's top-left pixel inside the body frame, counted from the frame's top-left the way an art program shows it.
    //the canvas has to be the whole of its texture, and share the body frame's pivot in proportion, for the scaled renderer to land on the frame
    public static CanvasPlacement calculate(Sprite bodyFrame, Sprite canvas, Vector2Int topLeft)
    {
        if(!isCanvas(bodyFrame, canvas))
        {
            return identity;
        }

        Rect frame = bodyFrame.rect;
        Vector2 canvasSize = canvas.rect.size;

        //textures count their rows from the bottom
        float left = topLeft.x;
        float bottom = frame.height - topLeft.y - canvasSize.y;

        Vector2 frameTiling = new Vector2(frame.width/canvasSize.x, frame.height/canvasSize.y);

        return new CanvasPlacement(
            new Vector3(frameTiling.x, frameTiling.y, 1f),
            frameTiling,
            new Vector2(-left/canvasSize.x, -bottom/canvasSize.y),
            new Vector2(bodyFrame.texture.width/canvasSize.x, bodyFrame.texture.height/canvasSize.y),
            new Vector2(-(frame.x + left)/canvasSize.x, -(frame.y + bottom)/canvasSize.y));
    }

    public bool Equals(CanvasPlacement other)
    {
        return scale == other.scale &&
                layerTiling == other.layerTiling &&
                layerOffset == other.layerOffset &&
                outlineTiling == other.outlineTiling &&
                outlineOffset == other.outlineOffset;
    }
}
