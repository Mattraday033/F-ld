using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepetitionAbility : Ability
{
    private int maxRepetitions;
    private int completedRepetitions = 0;

    public RepetitionAbility(CombatActionSettings settings) :
    base(settings)
    {
        this.maxRepetitions = 1;
    }

    public RepetitionAbility(CombatActionSettings settings, int maxRepetitions) :
    base(settings)
    {
        this.maxRepetitions = maxRepetitions;
    }

    public override void performCombatAction()
    {
        base.performCombatAction();

        completedRepetitions++;

        if (hasRepetitionsRemaining() && CombatActionManager.lockedInCombatActionQueue != null)
        {
            CombatActionManager.lockedInCombatActionQueue.Insert(0, this);
        }
    }

    public virtual int getRepetitions()
    {
        return maxRepetitions;
    }

    public override bool hasRepetitionsRemaining()
    {
        return completedRepetitions < maxRepetitions;
    }

    public override CombatAction clone()
    {
        completedRepetitions = 0;

        return base.clone();
	}

}
