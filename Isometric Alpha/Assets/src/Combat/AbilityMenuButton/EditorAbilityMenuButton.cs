using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorAbilityMenuButton : AbilityMenuButton
{

    public bool isPassiveSlot;

    public void setPlayerCombatActionAtIndex(CombatAction combatAction)
    {
        if (isPassiveSlot && !combatAction.canBePlacedInPassiveSlot())
        {
            return;
        }

        if (combatAction.hasAvailableSlots(abilityMenuManager))
        {
            insertCombatAction(combatAction);
            return;
        }

        CombatActionArray combatActionArray = abilityMenuManager.getStoredCombatActionArray();

        CombatAction oldAction = combatActionArray.getActionInSlot(index);
        combatActionArray.unequipCombatAction(index);
        abilityMenuManager.populateAbilityMenuFromCombatActionArray();

        if (combatAction.hasAvailableSlots(abilityMenuManager))
        {
            insertCombatAction(combatAction);
            return;
        }

        insertCombatAction(oldAction);
    }

    private void insertCombatAction(CombatAction combatAction)
    {
        if (combatAction == null)
        {
            return;
        }

        abilityMenuManager.getStoredCombatActionArray().equipCombatAction(combatAction, index);

        OnPointerEnter(null);
        populateUI();
    }

    public void removeAbility()
    {
        abilityMenuManager.getStoredCombatActionArray().unequipCombatAction(index);

        OnPointerExit(null);
        populateUI();
    }

    private void populateUI()
    {
        abilityMenuManager.populateAbilityMenuFromCombatActionArray();
        OverallUIManager.currentScreenManager.populateAllGrids();
    }

    public override void enable()
    {
        enabled = true;
        abilityIcon.enabled = true;
        iconOutline.enabled = true;

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

        iconOutline.enabled = false;
        iconBackground.color = Color.black;

        loadedCombatAction = null;
    }

}
