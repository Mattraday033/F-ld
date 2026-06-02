using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum CombatActionSlot
{
	FirstSlot = 0,
	SecondSlot = 1,
	ThirdSlot = 2,
	FourthSlot = 3,
	FifthSlot = 4,
	SixthSlot = 5,
	SeventhSlot = 6,
	EighthSlot = 7,
	FirstPassiveSlot = 8,
	SecondPassiveSlot = 9,
	ThirdPassiveSlot = 10,
	FourthPassiveSlot = 11,
}

public class CombatActionArray : StatBoostSourceCombiner
{
    public const int numberOfActivatablePlayerCombatActions = 8;
    public const int maxPlayerCombatActions = 12;

    public readonly static UnityEvent OnCombatActionArrayChange = new UnityEvent();

    private Stats actor;
    private CombatAction[] combatActions;

    public CombatActionArray(AllyStats actor)
    {
        this.actor = actor;
        combatActions = new CombatAction[maxPlayerCombatActions];
    }

    public CombatActionArray(AllyStats actor, CombatAction[] combatActions)
    {
        this.actor = actor;
        this.combatActions = new CombatAction[maxPlayerCombatActions];

        for (int index = 0; index < this.combatActions.Length &&
                            index < combatActions.Length; index++)
        {
            if (combatActions[index] != null)
            {
                equipCombatAction(index, combatActions[index], removeItem: false);
            }

        }
    }

    public CombatAction[] getActions()
    {
        return combatActions;
    }

    public CombatAction getActionInSlot(int slotIndex)
    {
        return combatActions[slotIndex];
    }

    public int findFirstAttackSlot()
    {
        int index = 0;

        foreach(CombatAction action in combatActions)
        {
            if(action is Attack)
            {
                return index;
            }

            index++;
        }

        return -1;
    }

    public int calculateBonusAbilityDamage()
    {
        int highestBonusAbilityDamage = 0;

        int offHandBonusDamage = calculateAllStatFormulas(getStatSource().getAllStatBoosts(),
                                                          b => b.getBonusDamageFormula());

        foreach (CombatAction action in combatActions)
        {
            if (action != null && action.getSourceItem() != null && action.getSourceItem().isEquippable())
            {
                int currentBonusDamage = DamageCalculator.calculateBonusDamage(action.getDamageFormula());

                Weapon sourceWeapon = action.getSourceItem() as Weapon;

                if(sourceWeapon != null && sourceWeapon.getIsTwoHanded() && currentBonusDamage > highestBonusAbilityDamage)
                {
                    highestBonusAbilityDamage = currentBonusDamage;
                } else if(currentBonusDamage + offHandBonusDamage > highestBonusAbilityDamage)
                {
                    highestBonusAbilityDamage = currentBonusDamage + offHandBonusDamage;
                }
            }
        }

        if(highestBonusAbilityDamage < offHandBonusDamage)
        {
            return offHandBonusDamage;
        } else
        {
            return highestBonusAbilityDamage;
        }
    }

    public void unequipCombatAction(string actionName)
    {
        int slotIndex = 0;
        foreach (CombatAction action in combatActions)
        {
            if (action != null && action.getName().Equals(actionName))
            {
                unequipCombatAction(slotIndex);
                return;
            }

            slotIndex++;
        }

    }

    public void unequipCombatAction(int slotIndex)
    {
        if (slotIndex >= combatActions.Length)
        {
            Debug.LogError("SlotIndex(" + slotIndex + ") >= " + combatActions.Length);
            return;
        }

        if (combatActions[slotIndex] == null)
        {
            return;
        }

        Item oldItem = combatActions[slotIndex].getSourceItem();

        if (oldItem != null && oldItem.getQuantity() > 0 && !oldItem.usableInCombat())
        {
            Inventory.addItem(oldItem, ignoreEvent: true);
            // EquippedItems.OnEquipmentChange.Invoke();
        }

        if(!Flags.isInNewGameMode() && !LoadSaveFile.midLoad && 
            PlayerOOCStateManager.currentActivity == OOCActivity.inUI)
        {
            combatActions[slotIndex].playUnequipSFX();
        }

        combatActions[slotIndex] = null;

        if(!LoadSaveFile.midLoad)
        {
            MouseHoverManager.OnHoverPanelCreation.Invoke();
            OnCombatActionArrayChange.Invoke();
        }
    }

    public void unequipAllActions()
    {
        for(int index = 0; index < maxPlayerCombatActions; index++)
        {
            unequipCombatAction(index);
        }
    }

    private bool actionIsAttack(CombatAction action)
    {
        return action.getSourceItem() != null && action.getSourceItem().isEquippable();
    }

    private bool indexIsUnlocked(int index)
    {
        if (index < numberOfActivatablePlayerCombatActions + actor.getPassiveSlotsUnlocked())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void equipCombatAction(CombatAction newCombatAction)
    {
        if (newCombatAction == null)
        {
            return;
        }

        int firstAvailableSlotIndex = -1;

        if (newCombatAction.canBePlacedInPassiveSlot() && !actionIsAttack(newCombatAction))
        {
            for (int index = (combatActions.Length - 1); index >= 0; index--)
            {
                if (indexIsUnlocked(index) && combatActions[index] == null)
                {
                    firstAvailableSlotIndex = index;
                    break;
                }
            }
        }
        else
        {
            int maxIndex = numberOfActivatablePlayerCombatActions;

            if (actionIsAttack(newCombatAction))
            {
                maxIndex += actor.getPassiveSlotsUnlocked();
            }

            for (int index = 0; index < maxIndex; index++)
            {
                if (combatActions[index] == null)
                {
                    firstAvailableSlotIndex = index;
                    break;
                }
            }
        }

        equipCombatAction(newCombatAction, firstAvailableSlotIndex);
    }

    public void equipCombatAction(int slotIndex, CombatAction newCombatAction, bool removeItem = true)
    {
        equipCombatAction(newCombatAction, slotIndex, removeItem);
    }

    public void equipCombatAction(CombatAction newCombatAction, int slotIndex, bool removeItem = true)
    {
        if (slotIndex >= combatActions.Length || slotIndex < 0)
        {
            return;
        }

        if (combatActions[slotIndex] != null)
        {
            unequipCombatAction(slotIndex);
        }

        if (newCombatAction != null)
        {
            Item newItem = newCombatAction.getSourceItem();

            if (removeItem && newItem != null && !newItem.usableInCombat() &&
                !newItem.getKey().Equals(ItemList.getMainHandFist(actor as AllyStats).getKey()) &&
                !newItem.getKey().Equals(ItemList.getOffHandFist().getKey()))
            {
                Inventory.removeItem(newItem, 1, ignoreEvent: true);
            }

            combatActions[slotIndex] = newCombatAction.clone();
            combatActions[slotIndex].setActor(actor);
        }
        else
        {
            combatActions[slotIndex] = null;
        }

        if(!LoadSaveFile.midLoad)
        {
            if(!Flags.isInNewGameMode() &&
                PlayerOOCStateManager.currentActivity == OOCActivity.inUI && 
                OverallUIManager.lastScreenType != ScreenType.SaveAndLoad)
            {
                newCombatAction.playEquipSFX();
            }

            MouseHoverManager.OnHoverPanelCreation.Invoke();
            OnCombatActionArrayChange.Invoke();
        }
    }

    public bool hasAvailableSlots(CombatAction newAction)
    {
        int actionSlots = 0;

        foreach (CombatAction action in combatActions)
        {
            if (action == null)
            {
                continue;
            }

            if (String.Equals(newAction.getKey(), action.getKey(), StringComparison.OrdinalIgnoreCase))
            {
                actionSlots++;
            }

            if (actionSlots >= newAction.getMaximumSlots())
            {
                return false;
            }
        }

        return true;
    }

    public int getSlotsUsedOnAction(CombatAction newAction)
    {
        int actionSlots = 0;

        foreach (CombatAction action in combatActions)
        {
            if (action == null)
            {
                continue;
            }

            if (String.Equals(newAction.getKey(), action.getKey(), StringComparison.OrdinalIgnoreCase))
            {
                actionSlots++;
            }
        }

        return actionSlots;
    }

    public bool hasAvailableWeaponSlots()
    {
        return getAmountOfWeaponCombatActions() < actor.getWeaponSlots();
    }

    public int getAmountOfWeaponCombatActions()
    {
        int amountOfWeapons = 0;

        foreach (CombatAction action in combatActions)
        {
            if (action == null)
            {
                continue;
            }

            if (action.takesAWeaponSlot())
            {
                amountOfWeapons++;
            }
        }

        return amountOfWeapons;
    }

    public bool everyAvailableCombatActionSlotFilled()
    {
        for (int index = 0; index < numberOfActivatablePlayerCombatActions + actor.getPassiveSlotsUnlocked(); index++)
        {
            if (combatActions[index] == null)
            {
                return false;
            }
        }

        return true;
    }

    public List<Trait> getAllEquippedPassiveTraits()
    {
        List<Trait> equippedPassiveTraits = new List<Trait>();

        foreach (CombatAction action in combatActions)
        {
            if (action == null)
            {
                continue;
            }

            if (action.autoApply())
            {
                action.getAppliedTrait().resetStacksToStartingAmount();

                equippedPassiveTraits.Add(action.getAppliedTrait());
            }
        }

        return equippedPassiveTraits;
    }

    public bool alreadyHasStance()
    {
        foreach (CombatAction action in combatActions)
        {
            if (action != null && action.getName().Contains(Stance.stanceNameFragment))
            {
                return true;
            }
        }

        return false;
    }

    public int getActionIndex(CombatAction action)
    {
        for (int index = 0; index < maxPlayerCombatActions; index++)
        {
            if (combatActions[index] != null && combatActions[index].getKey().Equals(action.getKey()))
            {
                return index;
            }
        }

        return -1;
    }

    public void resetAllCooldowns()
    {
        foreach (CombatAction action in this)
        {
            if (action != null)
            {
                action.takeOffCooldown();;
            }
        }
    }

    public int getTotalRedStacksAtStart()
    {
        return Helpers.sum<CombatAction>(this, t => t.getRedStacksAtStart()) + actor.getBonusExuberances();
    }

    public int getTotalBlueStacksAtStart()
    {
        return Helpers.sum<CombatAction>(this, t => t.getBlueStacksAtStart()) + actor.getBonusExuberances();
    }

    public int getTotalYellowStacksAtStart()
    {
        return Helpers.sum<CombatAction>(this, t => t.getYellowStacksAtStart()) + actor.getBonusExuberances();
    }

    public int getTotalGreenStacksAtStart()
    {
        return Helpers.sum<CombatAction>(this, t => t.getGreenStacksAtStart()) + actor.getBonusExuberances();
    }

    public override string getName()
    {
        return actor.getName() + "'s Combat Action Array";
    }

    public override Stats getStatSource()
    {
        return actor;
    }

    public override IEnumerator GetEnumerator()
    {
        return combatActions.GetEnumerator();
    }

    public string getCombinedWeaponFormula(FormulaDelegate<StatBoostSource> getFormula)
    {
        List<CombatAction> attacksInCombatArray = Helpers.getAllObjectsWithQuality<CombatAction>(combatActions, a => a as Attack != null);

        string combinedFormula = Constants.zeroBonus;

        foreach(Attack attack in attacksInCombatArray)
        {
            combinedFormula = DamageCalculator.combineFormulas(combinedFormula, getFormula(attack));
        }

        return combinedFormula;
    }

    public bool allActionSlotsFull()
    {
        for (int index = 0; index < numberOfActivatablePlayerCombatActions + actor.getPassiveSlotsUnlocked(); index++)
        {
            if(getActions()[index] == null)
            {
                return false;
            }
        }

        return true;
    }

}
