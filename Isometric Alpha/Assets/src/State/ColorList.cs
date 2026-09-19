using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorList
{
    public const float hoverSelectorAlpha = .65f;

    #region Black Fadeouts

    public readonly static Color32 blackFadeOut75 = new Color32(0, 0, 0, 75);

    public readonly static Color32 cutOutFade = new Color32(0,0,0,120);

    #endregion

    #region White Fadeouts

    public readonly static Color whiteFadeOut75 = new Color32(255, 255, 255, 75);

    #endregion

    #region Greys
    public readonly static Color grey25 = new Color32(25, 25, 25, 255);
    public readonly static Color grey35 = new Color32(35, 35, 35, 255);
    public readonly static Color grey55 = new Color32(55, 55, 55, 255);
    public readonly static Color grey75 = new Color32(75, 75, 75, 255);    
    public readonly static Color greyedOutBackgroundColor = grey75;
    public readonly static Color grey100 = new Color32(100, 100, 100, 255);
    public readonly static Color grey100Transparent = new Color32(100, 100, 100, 125);
    public readonly static Color grey125 = new Color32(125, 125, 125, 255);
    public readonly static Color grey155 = new Color32(155, 155, 155, 255);
    public readonly static Color grey175 = new Color32(175, 175, 175, 255);
    public readonly static Color grey215 = new Color32(215, 215, 215, 255);
    public readonly static Color grey225 = new Color32(225, 225, 225, 255);
    public readonly static Color grey240 = new Color32(240, 240, 240, 255);
    public readonly static Color grey245 = new Color32(245, 245, 245, 255);
    #endregion

    #region UI Colors


    public readonly static Color grey25Transparent = new Color32(25, 25, 25, 175);

    public readonly static Color questCounterCyan = new Color32(200,232,240,255);
    public readonly static Color lightUICyan = new Color32(160,183,188,255);
    public readonly static Color darkUICyan = new Color32(100,125,130,255);

    public readonly static Color bubbleOutlineColor = new Color32(60, 35, 25, 255);
    public readonly static Color bubbleBackgroundColor = new Color32(35, 25, 15, 255);

    #endregion

    #region Skill Indicator Colors


    public readonly static Color intimidateIndicatorOrange = new Color32(255, 175, 25, 255);
    public readonly static Color skillIndicatorTargetableColor = new Color32(235,0,0,235);
    public readonly static Color cunningTileBaseColor = new Color32(235,235,0,235);
    public readonly static Color observationColor = new Color32(225,0,245,235);

    #endregion

    #region Outline Colors
	public readonly static Color attacksOnSight = Color.red;
	public readonly static Color canBeInteractedWith = Color.green;
	public readonly static Color canBePushed = Color.blue;
	public readonly static Color canBeCunninged = Color.yellow;
	public readonly static Color defaultWhenNotRevealed = Color.clear;
	public readonly static Color tutorialDefault = questCounterCyan;
    #endregion

    #region Rubble Colors
    public readonly static Color32 mineLvl3RubbleColor = new Color32(179, 175, 192, 255);
    public readonly static Color32 mineLvl2RubbleColor = new Color32(175, 170, 160, 255);
    public readonly static Color32 shackRubbleColor = new Color32(225, 205, 175, 255);
    #endregion

    #region Surprise Icon Colors

    public readonly static Color surpriseIconGrey = blackFadeOut75;
    public readonly static Color surpriseIconGreen = new Color32(25,185,25,255);
    public readonly static Color surpriseIconYellow = new Color32(255,230,30,255);
    public readonly static Color surpriseIconRed = new Color32(225,35,35,255);

    #endregion

    #region Misc

    public readonly static Color skillButtonOutlineHighlight = Color.yellow;

    public readonly static Color availableEquipmentIcon = lightUICyan;
    public readonly static Color unavailableEquipmentIcon = grey35;
    public readonly static Color availableIconFadeOutLevel = blackFadeOut75;
    public readonly static Color unavailableIconFadeOutLevel = grey75;
    public readonly static Color filledIconFadeOutLevel = grey125;

    public readonly static Color shopUnbuyable = grey125;

    public readonly static Color ineligibleColor = grey175;
    public readonly static Color alternateRowColor = grey215;

    public readonly static Color colorIndicatingChosenBefore = grey155; 

    public readonly static Color combatHoverOutlineGrey = grey175;

    public readonly static Color blueShieldTextColor = new Color32(25, 100, 255, 255); // color is a lighter blue than default Color.blue
	public readonly static Color greenLeafTextColor = new Color32(25, 255, 0, 255); // color is a lighter green than default Color.green


    #region HealthBarManager colors
    public readonly static Color healthyGreen = new Color32(0,175,55,255);
    public readonly static Color buffedBlue = new Color32(0,225,225,255);
    public readonly static Color debuffedPurple = new Color32(135,15,175,255);
    public readonly static Color buffedDebuffed = new Color32(230,190,186,255);
    #endregion

    #endregion
    
    #region Sprite Colors

    #region Skin Tones

    public readonly static Color Skin_PaleWhite = parseColor("#FDD9BC");
    public readonly static Color Skin_Asian = parseColor("#F4C999");
    public readonly static Color Skin_MiddleEastern = parseColor("#C49B78");
    public readonly static Color Skin_Tan = parseColor("#BE9562");
    public readonly static Color Skin_LightBrown = parseColor("#A2755F");
    public readonly static Color Skin_Brown = parseColor("#936745");
    public readonly static Color Skin_DeepBrown = parseColor("#774D26");
    public readonly static Color Skin_Black = parseColor("#634934");

    public readonly static Color Hair_BrandedScar = parseColor("#702626");

    #endregion

    #region Hair Tones

    public readonly static Color Hair_White = parseColor("#D7D7D7");
    public readonly static Color Hair_Grey2 = parseColor("#959595");
    public readonly static Color Hair_Grey1 = parseColor("#757575");
    public readonly static Color Hair_Blonde1 = parseColor("#FFC964");
    public readonly static Color Hair_TanBlonde = parseColor("#9E8051");
    public readonly static Color Hair_GrayishBrown = parseColor("#796960");
    public readonly static Color Hair_Clay = parseColor("#563122");
    public readonly static Color Hair_Brown = parseColor("#382A1B");
    public readonly static Color Hair_DarkBrown = parseColor("#2E231C");
    public readonly static Color Hair_LightBlack = parseColor("#2C2C2C");
    public readonly static Color Hair_DarkBlack = parseColor("#201D1B");
    public readonly static Color Hair_Orange = parseColor("#CB5A20");
    public readonly static Color Hair_Ginger = parseColor("#B3492B");
    public readonly static Color Hair_DeepRed = parseColor("#522E2E");

    #endregion

    #region Cloth

    public readonly static Color Cloth_PatchBrown = parseColor("#A9614A");
    public readonly static Color Cloth_TerraCottaBrown = parseColor("#975434");
    public readonly static Color Cloth_Brown = parseColor("#775F55");
    public readonly static Color Cloth_DeepBrown = parseColor("#3A2C1D");
    public readonly static Color Cloth_FadedBrown = parseColor("#554137");

    public readonly static Color Cloth_Blood = parseColor("#AA3333");
    public readonly static Color Cloth_DeepRed = parseColor("#6E2323");
    public readonly static Color Cloth_DesaturatedRed = parseColor("#9A635C");
    public readonly static Color Cloth_FadedRed = parseColor("#9D4B42"); //Red of Branded Clothes

    public readonly static Color Cloth_RoyalPurple = parseColor("#3B2365");
    public readonly static Color Cloth_FadedPurple = parseColor("#605F84");

    public readonly static Color Cloth_SuppressedBlue = parseColor("#182E61");
    public readonly static Color Cloth_PaleBlue = parseColor("#5E83AE");    
    public readonly static Color Cloth_DesaturatedBlue = parseColor("#42628B");

    public readonly static Color Cloth_MossGreen = parseColor("#516243");
    public readonly static Color Cloth_DarkOliveGreen = parseColor("#30520D");

    public readonly static Color Cloth_TunicCuffAndCollarYellow = parseColor("#FEC662");

    public readonly static Color Cloth_White = parseColor("#F9F9F9");
    public readonly static Color Cloth_ServantsTunicWhite = parseColor("#ECE7E2");
    public readonly static Color Cloth_BandageWhite = parseColor("#EAE5E0");
    public readonly static Color Cloth_AltBandageWhite = parseColor("#FEFEFE");

    public readonly static Color Cloth_LightGrey = parseColor("#A6A6A6");
    public readonly static Color Cloth_Grey = parseColor("#7E7E7E");
    public readonly static Color Cloth_DarkerGrey = parseColor("#5C5B5B");

    public readonly static Color Cloth_LightBlack = parseColor("#272727");

    #endregion

    #region Leather

    public readonly static Color Leather_Blue = parseColor("#224B84");
    public readonly static Color Leather_Red = parseColor("#AA3333");
    public readonly static Color Leather_BeltBrown = parseColor("#674330");
    public readonly static Color Leather_DullGrey = parseColor("#2B2B2B");

    #endregion

    #region Wood

    public readonly static Color Wood_WeaponShaft = parseColor("#674330");

    #endregion

    #region Metals
    public readonly static Color Metal_Brass = parseColor("#FFC763");

    public readonly static Color Metal_Bronze = parseColor("#FEBB8E");
    public readonly static Color Metal_BronzeShadow = parseColor("#EA8A68");

    public readonly static Color Metal_Iron = parseColor("#A7A6A6");
    public readonly static Color Metal_IronShadow = parseColor("#313131");

    public readonly static Color Metal_Shine = parseColor("#F9F9F9");

    #endregion

    #endregion

    private static Color parseColor(string colorString)
    {    
        if(ColorUtility.TryParseHtmlString(colorString, out Color color))
        {
            return color;
        } else
        {
            return Color.white;
        }
    }

    private static Dictionary<string, Color> rubbleColorDict;
    public static Color getRubbleColorFromLocationName()
    {
        string locationNamePrefix = AreaManager.locationName.Split("-")[0];

        if(!rubbleColorDict.ContainsKey(locationNamePrefix))
        {
            return Color.white; 
        }

        return rubbleColorDict[locationNamePrefix];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        rubbleColorDict = new Dictionary<string, Color>();

        rubbleColorDict.Add(LocationNameList.slaveShackOne, shackRubbleColor);
        rubbleColorDict.Add(LocationNameList.slaveShackTwo, shackRubbleColor);
        rubbleColorDict.Add(LocationNameList.slaveShackThree, shackRubbleColor);
        rubbleColorDict.Add(LocationNameList.slaveShackFour, shackRubbleColor);
        rubbleColorDict.Add(LocationNameList.slaveShackFive, shackRubbleColor);
        rubbleColorDict.Add(LocationNameList.slaveShackSix, shackRubbleColor);
        rubbleColorDict.Add(LocationNameList.slaveShackSeven, shackRubbleColor);
        rubbleColorDict.Add(LocationNameList.slaveShackEight, shackRubbleColor);
        rubbleColorDict.Add(LocationNameList.slaveShackNine, shackRubbleColor);

        rubbleColorDict.Add(LocationNameList.campNorthWest, shackRubbleColor);
        rubbleColorDict.Add(LocationNameList.bodyPile, shackRubbleColor);

        rubbleColorDict.Add(ZoneKeyList.mineLvl1, shackRubbleColor);

        rubbleColorDict.Add(ZoneKeyList.mineLvl2, mineLvl2RubbleColor);

        rubbleColorDict.Add(ZoneKeyList.mineLvl3, mineLvl3RubbleColor);
    }

}

public enum ColorReplacementSlot
{
    R,
    G,
    B,
    C,
    M,
    Y,
    O,
    V,
    T,
    S,
    P
}

public enum SpriteLayer
{
    Shield_Back,
    Weapon,
    Body,
    Cloak,
    Face,
    Hair,
    Shield_Front
}

public class ColorReplaceSchema
{
    private readonly Dictionary<ColorReplacementSlot, Color> shieldBack;
    private readonly Dictionary<ColorReplacementSlot, Color> weapon;
    private readonly Dictionary<ColorReplacementSlot, Color> body;
    private readonly Dictionary<ColorReplacementSlot, Color> cloak;
    private readonly Dictionary<ColorReplacementSlot, Color> face;
    private readonly Dictionary<ColorReplacementSlot, Color> hair;
    private readonly Dictionary<ColorReplacementSlot, Color> shieldFront;

    public Color getColor(SpriteLayer section, ColorReplacementSlot slot)
    {
        Dictionary<ColorReplacementSlot, Color> dict;

        switch(section)
        {
            case SpriteLayer.Shield_Back:
                dict = shieldBack;
                break;
            case SpriteLayer.Weapon:
                dict = weapon;
                break;
            case SpriteLayer.Body:
                dict = body;
                break;
            case SpriteLayer.Cloak:
                dict = cloak;
                break;
            case SpriteLayer.Face:
                dict = face;
                break;
            case SpriteLayer.Hair:
                dict = hair;
                break;
            case SpriteLayer.Shield_Front:
                dict = shieldFront;
                break;

            default:
                return Color.black;
        }

        if(dict.ContainsKey(slot))
        {
            return dict[slot];
        } else
        {
            return Color.black;
        }
    }

    public ColorReplaceSchema(  Dictionary<ColorReplacementSlot, Color> shieldBack = null,
                                Dictionary<ColorReplacementSlot, Color> weapon = null,
                                Dictionary<ColorReplacementSlot, Color> body = null,
                                Dictionary<ColorReplacementSlot, Color> cloak = null,
                                Dictionary<ColorReplacementSlot, Color> face = null,
                                Dictionary<ColorReplacementSlot, Color> hair = null,
                                Dictionary<ColorReplacementSlot, Color> shieldFront = null)
    {

        if(shieldBack != null)
        {
            this.shieldBack = shieldBack;
        } else
        {
            this.shieldBack = getDefaultReplaceSchema();
        }

        if(weapon != null)
        {
            this.weapon = weapon;
        } else
        {
            this.weapon = getDefaultReplaceSchema();
        }

        if(body != null)
        {
            this.body = body;
        } else
        {
            this.body = getDefaultReplaceSchema();
        }
        
        if(cloak != null)
        {
            this.cloak = cloak;
        } else
        {
            this.cloak = getDefaultReplaceSchema();
        }

        if(face != null)
        {
            this.face = face;
        } else
        {
            this.face = getDefaultReplaceSchema();
        }

        if(hair != null)
        {
            this.hair = hair;
        } else
        {
            this.hair = getDefaultReplaceSchema();
        }

        if(shieldFront != null)
        {
            this.shieldFront = shieldFront;
        } else
        {
            this.shieldFront = getDefaultReplaceSchema();
        }
    }

    private static Dictionary<ColorReplacementSlot, Color> getDefaultReplaceSchema()
    {
        return new Dictionary<ColorReplacementSlot, Color>();
    }
}

public static class ColorSchemaList
{
    private static Dictionary<string, ColorReplaceSchema> colorSchemaDict;

    public static ColorReplaceSchema getSchema(string name)
    {
        if(!colorSchemaDict.ContainsKey(name))
        {
            return new ColorReplaceSchema(); 
        }

        return colorSchemaDict[name];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        colorSchemaDict = new Dictionary<string, ColorReplaceSchema>();

        colorSchemaDict.Add(MonsterNameList.spearman, new ColorReplaceSchema(
                                                                                weapon: new Dictionary<ColorReplacementSlot, Color>()
                                                                                {
                                                                                    [ColorReplacementSlot.R] = ColorList.Metal_Bronze,
                                                                                    [ColorReplacementSlot.G] = ColorList.Cloth_PaleBlue,
                                                                                    [ColorReplacementSlot.B] = ColorList.Wood_WeaponShaft
                                                                                },
                                                                                body: new Dictionary<ColorReplacementSlot, Color>()
                                                                                {
                                                                                    [ColorReplacementSlot.R] = ColorList.Skin_LightBrown,
                                                                                    [ColorReplacementSlot.G] = ColorList.Skin_LightBrown,
                                                                                    [ColorReplacementSlot.B] = ColorList.Metal_Bronze,
                                                                                    [ColorReplacementSlot.C] = ColorList.Metal_BronzeShadow,            
                                                                                    [ColorReplacementSlot.M] = ColorList.Metal_Bronze,
                                                                                    [ColorReplacementSlot.Y] = ColorList.Metal_BronzeShadow,
                                                                                    [ColorReplacementSlot.O] = ColorList.Metal_Brass,
                                                                                    [ColorReplacementSlot.V] = ColorList.Cloth_SuppressedBlue,
                                                                                    [ColorReplacementSlot.T] = ColorList.Cloth_PaleBlue,
                                                                                    [ColorReplacementSlot.S] = ColorList.Leather_DullGrey,
                                                                                    [ColorReplacementSlot.P] = ColorList.Leather_BeltBrown,
                                                                                },
                                                                                hair: new Dictionary<ColorReplacementSlot, Color>()
                                                                                {
                                                                                    [ColorReplacementSlot.G] = ColorList.Hair_DarkBrown
                                                                                },
                                                                                face: new Dictionary<ColorReplacementSlot, Color>()
                                                                                {
                                                                                    [ColorReplacementSlot.R] = ColorList.Skin_LightBrown,
                                                                                    [ColorReplacementSlot.G] = ColorList.Hair_DarkBrown
                                                                                }
                                                                            ));
    }
}