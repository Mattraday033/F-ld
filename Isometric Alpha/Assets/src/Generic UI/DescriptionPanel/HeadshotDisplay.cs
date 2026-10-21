using System;
using UnityEngine;
using UnityEngine.UI;

public class HeadshotDisplay : MonoBehaviour
{
	public Image hairImage;
    public Image faceImage;
    public Image bodyImage;


    //head sprites live in SpriteLayers/Head and are named after their HeadType, e.g. Head_Human
    public static Sprite getHeadSprite(BodyType bodyType)
    {
        HeadType headType = bodyType.getHeadType();

        if(!Enum.TryParse("Head_" + headType.ToString(), out SpritePath spritePath))
        {
            Debug.LogWarning("[HeadshotDisplay] No head sprite for HeadType." + headType + " (BodyType." + bodyType + "), so the headshot uses a blank texture.");
            spritePath = SpritePath.NoSprite;
        }

        return SpriteList.getSprite(spritePath);
    }
}
