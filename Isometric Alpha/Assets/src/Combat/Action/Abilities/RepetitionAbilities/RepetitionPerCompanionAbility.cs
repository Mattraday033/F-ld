using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepetitionPerCompanionAbility : RepetitionAbility
{
    public RepetitionPerCompanionAbility(CombatActionSettings settings) :
    base(settings)
    {

    }

    public override int getRepetitions()
    {
        return getNumberOfCompanions();
    }

    private int getNumberOfCompanions()
    {
        return CombatGrid.getAllNonsummonedAllyCombatants().Count;
    }
}
