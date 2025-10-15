using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnapSelectorToMaster : TutorialSequenceStepScript
{
    public override void runScript(GameObject target) 
    {
        // snaps to master with shielded trait
        ArrayList allEnemies = CombatGrid.getAllAliveEnemyCombatants();

        Stats shieldedEnemy;

        foreach (EnemyStats enemy in allEnemies)
        {
            if (enemy.traits.Contains(TraitList.master) && enemy.traits.Contains(TraitList.shielded))
            {
                shieldedEnemy = enemy;

                SelectorManager.currentSelector.setToLocation(shieldedEnemy.position);

                SpawnHoverPanel.runInstanceOfScript();
                return;
            }
        }
    }
}
