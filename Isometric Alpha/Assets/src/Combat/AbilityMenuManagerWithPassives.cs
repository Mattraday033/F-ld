using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class AbilityMenuManagerWithPassives : AbilityMenuManager
{

    public AbilityMenuButton[] passiveButtons;

    public void disableLockedPassiveButtons()
    {
        int unlockedSlots = actionArraySource.getPassiveSlotsUnlocked();

        for (int index = 0; index < passiveButtons.Length; index++)
        {
            if (index < unlockedSlots)
            {
                passiveButtons[index].setToUnlockedStatus();
            }
            else
            {
                passiveButtons[index].setToLockedStatus();
            }
        }
    }

    public override void populateAbilityMenuFromCombatActionArray(CombatAction[] actions)
    {
        base.populateAbilityMenuFromCombatActionArray(actions);

        disableLockedPassiveButtons();
    }

}
