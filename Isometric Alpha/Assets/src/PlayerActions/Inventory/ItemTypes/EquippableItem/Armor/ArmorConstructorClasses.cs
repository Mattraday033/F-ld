using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class OffHandWeapon : Armor
{
    private string iconName;

	public OffHandWeapon(ItemListID listID, string key, string loreDescription, string damageFormula, string critFormula, string iconName)  : 
    base(listID, key, loreDescription, offHandSlotIndex, Constants.tierZero, damageFormula, critFormula) 
    {
        this.iconName = iconName;
    }

    public override string getIconName()
    {
        return iconName;
    }

    public override string getInvulnerableFormula()
    {
        return Constants.zeroRating;
    }

    public override string getBonusDamageFormula()
    {
        return DamageCalculator.calculateBonusDamage(getDamageFormula()).ToString();
    }
}

public class OffHandFist : OffHandWeapon
{
	public OffHandFist(ItemListID listID, string key, string loreDescription, string damageFormula, string critFormula)  : 
    base(listID, key, loreDescription, damageFormula, critFormula, "FistIcon") 
    {

    }

	public override bool isEquipped(AllyStats target)
	{
		if (target.equippedItems.getOffHand() == null)
		{
			return true;
		}

		return false;
	}

	public override bool isUnequippable()
	{
		return false;
	}
}

public class TierZeroShield : Armor
{
    public TierZeroShield(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, offHandSlotIndex, Constants.tierZero, damageFormula, critFormula) 
    {

    }
}

public class TierZeroHelmet : Armor
{
    public TierZeroHelmet(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, headSlotIndex, Constants.tierZero, damageFormula, critFormula) 
    {

    }
}

public class TierZeroBody : Armor
{
    public TierZeroBody(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, bodySlotIndex, Constants.tierZero, damageFormula, critFormula) 
    {

    }
}

public class TierZeroHands : Armor
{
    public TierZeroHands(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, handsSlotIndex, Constants.tierZero, damageFormula, critFormula) 
    {

    }
}

public class TierZeroFeet : Armor
{
    public TierZeroFeet(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, feetSlotIndex, Constants.tierZero, damageFormula, critFormula) 
    {

    }
}

public class Trinket : Armor
{
    public Trinket(ItemListID listID, string key, string loreDescription = Constants.zeroRating, string damageFormula = Constants.zeroRating, 
                                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, trinketSlotIndex, Constants.tierZero, damageFormula, critFormula) 
    {

    }
}

public class TierOneShield : Armor
{
    public TierOneShield(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, offHandSlotIndex, Constants.tierOne, damageFormula, critFormula) 
    {

    }
}

public class TierOneHelmet : Armor
{
    public TierOneHelmet(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, headSlotIndex, Constants.tierOne, damageFormula, critFormula) 
    {

    }
}

public class TierOneBody : Armor
{
    public TierOneBody(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, bodySlotIndex, Constants.tierOne, damageFormula, critFormula) 
    {

    }
}

public class TierOneHands : Armor
{
    public TierOneHands(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, handsSlotIndex, Constants.tierOne, damageFormula, critFormula) 
    {

    }
}

public class TierOneFeet : Armor
{
    public TierOneFeet(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, feetSlotIndex, Constants.tierOne, damageFormula, critFormula) 
    {

    }
}

public class TierTwoShield : Armor
{
    public TierTwoShield(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, offHandSlotIndex, Constants.tierTwo, damageFormula, critFormula) 
    {

    }
}

public class TierTwoHelmet : Armor
{
    public TierTwoHelmet(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, headSlotIndex, Constants.tierTwo, damageFormula, critFormula) 
    {

    }
}

public class TierTwoBody : Armor
{
    public TierTwoBody(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, bodySlotIndex, Constants.tierTwo, damageFormula, critFormula) 
    {

    }
}

public class TierTwoHands : Armor
{
    public TierTwoHands(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, handsSlotIndex, Constants.tierTwo, damageFormula, critFormula) 
    {

    }
}

public class TierTwoFeet : Armor
{
    public TierTwoFeet(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, feetSlotIndex, Constants.tierTwo, damageFormula, critFormula) 
    {

    }
}

public class TierThreeShield : Armor
{
    public TierThreeShield(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, offHandSlotIndex, Constants.tierThree, damageFormula, critFormula) 
    {

    }
}

public class TierThreeHelmet : Armor
{
    public TierThreeHelmet(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, headSlotIndex, Constants.tierThree, damageFormula, critFormula) 
    {

    }
}

public class TierThreeBody : Armor
{
    public TierThreeBody(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, bodySlotIndex, Constants.tierThree, damageFormula, critFormula) 
    {

    }
}

public class TierThreeHands : Armor
{
    public TierThreeHands(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, handsSlotIndex, Constants.tierThree, damageFormula, critFormula) 
    {

    }
}

public class TierThreeFeet : Armor
{
    public TierThreeFeet(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, feetSlotIndex, Constants.tierThree, damageFormula, critFormula) 
    {

    }
}

public class TierFourShield : Armor
{
    public TierFourShield(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, offHandSlotIndex, Constants.tierFour, damageFormula, critFormula) 
    {

    }
}

public class TierFourHelmet : Armor
{
    public TierFourHelmet(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, headSlotIndex, Constants.tierFour, damageFormula, critFormula) 
    {

    }
}

public class TierFourBody : Armor
{
    public TierFourBody(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, bodySlotIndex, Constants.tierFour, damageFormula, critFormula) 
    {

    }
}

public class TierFourHands : Armor
{
    public TierFourHands(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, handsSlotIndex, Constants.tierFour, damageFormula, critFormula) 
    {

    }
}

public class TierFourFeet : Armor
{
    public TierFourFeet(ItemListID listID, string key, string loreDescription, string damageFormula = Constants.zeroRating, 
                                                                                                      string critFormula = Constants.zeroRating)  : 
    base(listID, key, loreDescription, feetSlotIndex, Constants.tierFour, damageFormula, critFormula) 
    {

    }
}