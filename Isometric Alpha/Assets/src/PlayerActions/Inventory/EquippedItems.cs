using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;

public class EquippedItems : IEnumerable, IStatBoostSource, ICloneable
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

    #region IStatBoostSource

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
    
    public string getBonusCritFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusCritFormula());
    }
    public string getBonusDamageFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusDamageFormula());
    }

    public string getDamageFormula()
    {
        return clone().getAllOfOneStatFormula<IStatBoostSource>(t => t.getDamageFormula());
    }

    #endregion

    #region PrimaryStats

    public string getBonusStrengthFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusStrengthFormula());
    }
    public string getBonusDexterityFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusDexterityFormula());
    }
    public string getBonusWisdomFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusWisdomFormula());
    }
    public string getBonusCharismaFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusCharismaFormula());
    }

    #endregion

    #region Secondary Stats

    //Strength Stats
    public string getBonusPhysicalResistanceFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusPhysicalResistanceFormula());
    }
    public string getBonusCriticalDamageMultiplierFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusCriticalDamageMultiplierFormula());
    }
    public string getBonusHealthFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusHealthFormula());
    }

    //Dexterity Stats
    public string getBonusSurpriseRoundDamageFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusSurpriseRoundDamageFormula());
    }
    public string getBonusArmorFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusArmorFormula());
    }
    public string getBonusArmorPenetrationFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusArmorPenetrationFormula());
    }

    //Wisdom Stats
    public string getBonusPassiveSlotsFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusPassiveSlotsFormula());
    }
    public string getBonusWeaponSlotsFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusWeaponSlotsFormula());
    }
    public string getBonusMentalResistanceFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusMentalResistanceFormula());
    }

    //Charisma Stats
    public string getBonusSynergyFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusSynergyFormula());
    }
    public string getBonusExuberancesFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusExuberancesFormula());
    }
    public string getBonusZOIPotencyFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusZOIPotencyFormula());
    }

    #endregion

    #region Party Stats

    public string getBonusRegenFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusRegenFormula());
    }

    public string getBonusSurpriseRoundsFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusSurpriseRoundsFormula());
    }
    public string getBonusRetreatChanceFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusRetreatChanceFormula());
    }

    public string getBonusPartyActionsFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusPartyActionsFormula());
    }
    public string getBonusPartySlotsFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusPartySlotsFormula());
    }

    public string getBonusGoldMultiplierFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusGoldMultiplierFormula());
    }
    public string getBonusDiscountFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusDiscountFormula());
    }

    public string getBonusVolleyAccuracyFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusVolleyAccuracyFormula());
    }

    #endregion

    #region Skills
    public string getBonusIntimidateChargesFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusIntimidateChargesFormula());
    }
    public string getBonusCunningChargesFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusCunningChargesFormula());
    }
    public string getBonusObservationLevelFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusObservationLevelFormula());
    }
    public string getBonusLeadershipUsesFormula()
    {
        return getAllOfOneStatFormula<IStatBoostSource>(t => t.getBonusLeadershipUsesFormula());
    }
    #endregion

    public Stats getStatSource()
    {
        return owner;
    }

    #endregion


    #region IDescribable (Unimplemented)

    public string getName()
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
