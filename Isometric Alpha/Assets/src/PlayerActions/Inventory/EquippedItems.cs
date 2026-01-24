using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;

public class EquippedItems : StatBoostSource, IEnumerable, ICloneable
{
    public const int totalEquipmentSlots = 6;
    public readonly static UnityEvent OnEquipmentChange = new UnityEvent();

    public Stats owner;
    public EquippableItem[] equippedItems = new EquippableItem[totalEquipmentSlots];

    public EquippedItems(Stats owner)
    {
        this.owner = owner;
    }

    public EquippedItems(Stats owner, EquippableItem[] equippedItems)
    {
        this.owner = owner;
        this.equippedItems = equippedItems;

        foreach (EquippableItem equippedItem in equippedItems)
        {
            if (equippedItem != null)
            {
                equippedItem.equipTarget = owner;
            }
        }
    }

    public EquippableItem getItemInSlot(int index)
    {
        return equippedItems[index];
    }

    public EquippableItem getOffHand()
    {
        if (equippedItems[Weapon.offHandSlotIndex] != null &&
            !(equippedItems[Weapon.offHandSlotIndex] is null))
        {
            return equippedItems[Weapon.offHandSlotIndex];
        }

        return ItemList.getOffHandFist();
    }

    public void equipItem(EquippableItem item)
    {

        Dictionary<string, Item> currentPocket;

        if (item.isJunk())
        {
            currentPocket = State.junkPocket;
        }
        else
        {
            currentPocket = State.inventory;
        }

        if (equippedItems[item.getSlotID()] != null && equippedItems[item.getSlotID()].isJunk())
        {
            Inventory.addItem(equippedItems[item.getSlotID()], State.junkPocket);
        }
        else if (equippedItems[item.getSlotID()] != null)
        {
            Inventory.addItem(equippedItems[item.getSlotID()], State.inventory);
        }

        EquippableItem itemToEquip = (EquippableItem) Inventory.removeItem(item, 1, currentPocket);

        itemToEquip.equipTarget = owner;

        equippedItems[item.getSlotID()] = itemToEquip;

        OnEquipmentChange.Invoke();
    }

    public void unequipItem(int index)
    {
        if (index >= 0 && index < equippedItems.Length)
        {
            EquippableItem item = equippedItems[index];

            if (item != null)
            {
                unequipItem(item);
            }
        }
    }

    public void unequipItem(EquippableItem item)
    {
        if (!item.isUnequippable())
        {
            return;
        }
        
        if (item.getSlotID() == Weapon.mainHandSlotIndex ||
                item.getSlotID() < Weapon.offHandSlotIndex)
            {
                owner.getActionArray().unequipCombatAction(item.getKey());
            }
            else
            {
                Dictionary<string, Item> currentPocket;

                if (item.isJunk())
                {
                    currentPocket = State.junkPocket;
                }
                else
                {
                    currentPocket = State.inventory;
                }

                if (equippedItems[item.getSlotID()] != null)
                {
                    Inventory.addItem(equippedItems[item.getSlotID()], currentPocket);
                }


                equippedItems[item.getSlotID()].equipTarget = null;
                equippedItems[item.getSlotID()] = null;
            }

        OnEquipmentChange.Invoke();
    }

    public IEnumerable<IDescribable> createEquippedItemList()
    {
        List<EquippableItem> allEquippedItems = new List<EquippableItem>();
        List<EquippableItem> equippedMainHandWeapons = new List<EquippableItem>();
        List<EquippableItem> equippedArmorPlusOffHand = new List<EquippableItem>();

        foreach (CombatAction action in owner.getActionArray().getActions())
        {
            if (action != null && action.getSourceItem() != null && action.getSourceItem().isEquippable())
            {
                equippedMainHandWeapons.Add(action.getSourceItem() as EquippableItem);
            }
        }

        while (equippedMainHandWeapons.Count < Wisdom.maxNumberOfWeaponSlots)
        {
            equippedMainHandWeapons.Add(null);
        }

        equippedArmorPlusOffHand.Add(getOffHand());

        foreach (EquippableItem item in equippedItems)
        {
            equippedArmorPlusOffHand.Add(item);
        }

        allEquippedItems.AddRange(equippedMainHandWeapons);
        allEquippedItems.AddRange(equippedArmorPlusOffHand);

        return allEquippedItems;
    }

    //IEnumerable methods

    public IEnumerator GetEnumerator()
    {
        return equippedItems.GetEnumerator();
    }

    /*
        private static bool isDualWielding()
        {
            if(equippedItems[Weapon.offHandSlotIndex] == null || 
                !equippedItems[Weapon.offHandSlotIndex].getSubtype().Equals(Weapon.subtype))
            {
                return false;
            } else
            {
                return true;
            }
        }
    */

    #region ICloneable

    public object Clone()
    {
        return MemberwiseClone();
    }

    public EquippedItems clone()
    {
        EquippedItems clonedObject = Clone() as EquippedItems;
        
        clonedObject.equippedItems = equippedItems;
        clonedObject.owner = owner;

        if(clonedObject.equippedItems[Weapon.offHandSlotIndex] == null)
        {
            clonedObject.equippedItems[Weapon.offHandSlotIndex] = ItemList.getOffHandFist();
        }

        return clonedObject;
    }

    #endregion

    #region StatBoostSource

    public delegate string FormulaDelegate<T>(T t);
    public string getAllOfOneStatFormula<T>(FormulaDelegate<T> getFormula)
    {
        string totalFormula = "+0";

        foreach(T source in this)
        {
            if(source != null)
            {
                totalFormula = DamageCalculator.combineFormulas(totalFormula, getFormula(source));
            }
        }

        return totalFormula;   
    }

    #region Generic Stats
    
    public override string getBonusCritFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusCritFormula());
    }
    public override string getBonusDamageFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusDamageFormula());
    }

    public override string getDamageFormula()
    {
        return clone().getAllOfOneStatFormula<StatBoostSource>(t => t.getDamageFormula());
    }

    #endregion

    #region PrimaryStats

    public override string getBonusStrengthFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusStrengthFormula());
    }
    public override string getBonusDexterityFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusDexterityFormula());
    }
    public override string getBonusWisdomFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusWisdomFormula());
    }
    public override string getBonusCharismaFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusCharismaFormula());
    }

    #endregion

    #region Secondary Stats

    //Strength Stats
    public override string getBonusPhysicalResistanceFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusPhysicalResistanceFormula());
    }
    public override string getBonusCriticalDamageMultiplierFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusCriticalDamageMultiplierFormula());
    }
    public override string getBonusHealthFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusHealthFormula());
    }

    //Dexterity Stats
    public override string getBonusSurpriseRoundDamageFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusSurpriseRoundDamageFormula());
    }
    public override string getBonusArmorFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusArmorFormula());
    }
    public override string getBonusArmorPenetrationFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusArmorPenetrationFormula());
    }

    //Wisdom Stats
    public override string getBonusPassiveSlotsFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusPassiveSlotsFormula());
    }
    public override string getBonusWeaponSlotsFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusWeaponSlotsFormula());
    }
    public override string getBonusMentalResistanceFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusMentalResistanceFormula());
    }

    //Charisma Stats
    public override string getBonusSynergyFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusSynergyFormula());
    }
    public override string getBonusExuberancesFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusExuberancesFormula());
    }
    public override string getBonusZOIPotencyFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusZOIPotencyFormula());
    }

    #endregion

    #region Party Stats

    public override string getBonusRegenFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusRegenFormula());
    }

    public override string getBonusSurpriseRoundsFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusSurpriseRoundsFormula());
    }
    public override string getBonusRetreatChanceFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusRetreatChanceFormula());
    }

    public override string getBonusPartyActionsFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusPartyActionsFormula());
    }
    public override string getBonusPartySlotsFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusPartySlotsFormula());
    }

    public override string getBonusGoldMultiplierFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusGoldMultiplierFormula());
    }
    public override string getBonusDiscountFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusDiscountFormula());
    }

    public override string getBonusVolleyAccuracyFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusVolleyAccuracyFormula());
    }

    #endregion

    #region Skills
    public override string getBonusIntimidateChargesFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusIntimidateChargesFormula());
    }
    public override string getBonusCunningChargesFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusCunningChargesFormula());
    }
    public override string getBonusObservationLevelFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusObservationLevelFormula());
    }
    public override string getBonusLeadershipUsesFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(t => t.getBonusLeadershipUsesFormula());
    }
    #endregion

    public override Stats getStatSource()
    {
        return owner;
    }

    #endregion


    #region IDescribable (Unimplemented)

    public override string getName()
    {
        return owner.getName() + "'s Equipped Items";
    }

	public bool ineligible()
    {
        return false;
    }

	public GameObject getRowType(RowType rowType)
    {
        return null;
    }
	public GameObject getDescriptionPanelFull()
    {
        return null;
    }
	public GameObject getDescriptionPanelFull(PanelType type)
    {
        return null;
    }
    public GameObject getDecisionPanel()
    {
        return null;
    }

	public bool withinFilter(string[] filterParameters)
    {
        return false;
    }

	public void describeSelfFull(DescriptionPanel panel)
    {
        
    }

	public void describeSelfRow(DescriptionPanel panel)
    {
        
    }

	public void setUpDecisionPanel(IDecisionPanel descisionPanel)
    {
        
    }

	public List<IDescribable> getRelatedDescribables()
    {
        return new List<IDescribable>();
    }

	public bool buildableWithBlocks()
    {
        return false;
    }
	public bool buildableWithBlocksRows()
    {
        return false;
    }

    #endregion

}
