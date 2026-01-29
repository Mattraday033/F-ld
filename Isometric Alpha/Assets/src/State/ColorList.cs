using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorList
{
    #region Private Colors

    #region Rubble Colors
    public readonly static Color32 mineLvl3RubbleColor = new Color32(179, 175, 192, 255);
    public readonly static Color32 mineLvl2RubbleColor = new Color32(175, 170, 160, 255);
    public readonly static Color32 shackRubbleColor = new Color32(225, 205, 175, 255);
    #endregion

    #endregion

    #region Public Colors

    #region Black Fadeouts

    public readonly static Color32 blackFadeOut75 = new Color32(0, 0, 0, 75);

    public readonly static Color32 cutOutFade = new Color32(0,0,0,120);

    #endregion

    #region White Fadeouts

    #endregion

    #region Greys
    public readonly static Color32 grey25 = new Color32(25, 25, 25, 255);
    public readonly static Color32 grey35 = new Color32(35, 35, 35, 255);
    public readonly static Color32 grey55 = new Color32(55, 55, 55, 255);
    public readonly static Color32 grey75 = new Color32(75, 75, 75, 255);
    public readonly static Color32 grey100 = new Color32(100, 100, 100, 255);
    public readonly static Color32 grey100Transparent = new Color32(100, 100, 100, 125);
    public readonly static Color32 grey125 = new Color32(125, 125, 125, 255);
    public readonly static Color32 grey155 = new Color32(155, 155, 155, 255);
    public readonly static Color32 grey215 = new Color32(215, 215, 215, 255);
    public readonly static Color32 grey245 = new Color32(245, 245, 245, 255);
    #endregion

    #region Outline Colors
	public readonly static Color attacksOnSight = Color.red;
	public readonly static Color canBeInteractedWith = Color.green;
	public readonly static Color canBePushed = Color.blue;
	public readonly static Color canBeCunninged = Color.yellow;
	public readonly static Color defaultWhenNotRevealed = Color.clear;
	public readonly static Color tutorialDefault = Color.cyan;
    #endregion

    public readonly static Color surpriseIconGrey = grey155;

    public readonly static Color skillButtonOutlineHighlight = Color.yellow;

    public readonly static Color availableEquipmentIcon = grey100;
    public readonly static Color unavailableEquipmentIcon = grey35;
    public readonly static Color availableIconFadeOutLevel = blackFadeOut75;
    public readonly static Color unavailableIconFadeOutLevel = grey75;
    public readonly static Color filledIconFadeOutLevel = grey125;

    public readonly static Color shopUnbuyable = grey125;

    public readonly static Color ineligibleColor = grey125; //Should be a light grey/red for now
    public readonly static Color alternateRowColor = grey215;

    public readonly static Color colorIndicatingChosenBefore = grey100Transparent; //turns choice text gray and a bit transparent if it has been chosen before

    public readonly static Color combatHoverOutlineGrey = grey155;

    public readonly static Color blueShieldTextColor = new Color32(25, 100, 255, 255); // color is a lighter blue than default Color.blue
	public readonly static Color greenLeafTextColor = new Color32(25, 255, 0, 255); // color is a lighter green than default Color.green

	public readonly static Color cunningStunnedColor = Color.red;
	public readonly static Color intimidatedColor = Color.magenta;
	public readonly static Color retreatStunnedColor = Color.cyan;

    public readonly static Color intimidateIndicatorOrange = new Color32(225, 115, 0, 255);

    public readonly static Color greyedOutIconColor = new Color32(255, 255, 255, 75);
    public readonly static Color greyedOutBackgroundColor = grey75;

    public readonly static Color usedCombatActionSlotColor = Color.green;
	public readonly static Color unusedCombatActionSlotColor = Color.red;
    public readonly static Color dormantCombatActionSlotColor = grey75;

    public readonly static Color lockedBackgroundColor = grey55;

    public readonly static Color costPayableColor = Color.green;
    public readonly static Color costNotPayableColor = Color.red;
    public readonly static Color cooldownColor = Color.yellow;

    //HealthBarManager colors
    public readonly static Color healthyGreen = new Color32(0,175,55,255);
    public readonly static Color buffedBlue = new Color32(0,225,225,255);
    public readonly static Color debuffedPurple = new Color32(135,15,175,255);
    public readonly static Color buffedDebuffed = new Color32(230,190,186,255);
    #endregion

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
    private static void initializeDictionaries()
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

        rubbleColorDict.Add(ZoneKeyList.mineLvl2, mineLvl2RubbleColor);

        rubbleColorDict.Add(ZoneKeyList.mineLvl3, mineLvl3RubbleColor);
    }

}
