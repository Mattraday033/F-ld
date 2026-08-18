using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TakeHostageAbility : Ability
{

    public TakeHostageAbility(CombatActionSettings settings) :
    base(settings)
    {

    }

    public override int findFinalDamage(Stats target, bool isCrit)
    {
        int finalDamage = 0;

        if(target != null)
        {
            finalDamage = target.currentHealth * 3;
        }

        return finalDamage;
    }


    public override void performCombatAction(List<Stats> targets)
    {
        int projectileNumber = 1;

        playActivationAnimation();
        Dictionary<Stats, int> skipLog = new Dictionary<Stats, int>();

        foreach (Stats target in targets)
        {
            if(target == null || target.isDead() || !(target.hasTrait(TraitList.summoned) || target.hasTrait(TraitList.minion)))
            {
                continue;
            }

            int skips = 0;

            if (skipLog.ContainsKey(target))
            {
                skips = skipLog[target];
                skipLog[target]++; 
            }
            else
            {
                skipLog.Add(target, 1);
            }

            projectileNumber += sendProjectileAt(target.getPositionToHit(getSelector(), skips), target, projectileNumber);

            // target.playAnimationOnDamage();

            if (target.isDead() && !target.isLarge())
            {
                List<GridCoords> emptySpaces = new List<GridCoords>(CombatGrid.getAllEmptySpacesInEnemyZone());

                if(emptySpaces.Count <= 0)
                {
                    return;
                }

                EnemyStats conscript = (EnemyStats) EnemyStatsList.getEnemyStats(MonsterNameList.brandedConscript).clone();

                CreatureSpawner.spawn(conscript, new List<GridCoords> { emptySpaces.OrderBy(a => Guid.NewGuid()).ToList()[0] });
            }
        }
    }

}
