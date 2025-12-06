using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class Stance: EquippedPassive
{
    public const string stanceNameFragment = "Stance";
	public static UnityEvent OnStanceApplyingWeaponAttack = new UnityEvent();

    public Stance(CombatActionSettings settings) :
    base(settings)
    {

    }

    public override bool hasAvailableSlots(CombatActionArray combatActionArray)
    {
        return !combatActionArray.alreadyHasStance();
    }
}
