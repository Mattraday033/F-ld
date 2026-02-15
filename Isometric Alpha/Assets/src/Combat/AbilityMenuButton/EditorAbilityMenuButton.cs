using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorAbilityMenuButton : AbilityMenuButton
{

    public bool isPassiveSlot;

    public bool setPlayerCombatActionAtIndex(CombatAction combatAction)
    {
        if (isPassiveSlot && !combatAction.canBePlacedInPassiveSlot())
        {
            return false;
        }

        if (combatAction.hasAvailableSlots(abilityMenuManager))
        {
            insertCombatAction(combatAction);
            return true;
        }

        CombatActionArray combatActionArray = abilityMenuManager.getStoredCombatActionArray();

        CombatAction oldAction = combatActionArray.getActionInSlot(index);
        combatActionArray.unequipCombatAction(index);
        abilityMenuManager.populateAbilityMenuFromCombatActionArray();

        if (combatAction.hasAvailableSlots(abilityMenuManager))
        {
            insertCombatAction(combatAction);
            return true;
        }

        insertCombatAction(oldAction);
        return false;
    }

    private void insertCombatAction(CombatAction combatAction)
    {
        if (combatAction == null)
        {
            return;
        }

        abilityMenuManager.getStoredCombatActionArray().equipCombatAction(combatAction, index);

        OnPointerEnter(null);
    }

    public void removeAbility()
    {
        abilityMenuManager.getStoredCombatActionArray().unequipCombatAction(index);

        OnPointerExit(null);
    }

    public override void enable()
    {
        enabled = true;
        abilityIcon.enabled = true;

        if (!abilityMenuManager.displayOnly)
        {
            abilityMenuButton.enabled = true;
        }
    }

    public override void disable()
    {
        abilityMenuButton.enabled = false;

        abilityIcon.sprite = null;
        abilityIcon.enabled = false;

        iconBackground.color = ColorList.lightUICyan;

        loadedCombatAction = null;
    }

}
