using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnapSelectorToMaster : TutorialSequenceStepScript
{
    public override void runScript(GameObject target) 
    {
        // snaps to master with shielded trait
        List<Stats> allEnemies = CombatGrid.getAllAliveEnemyCombatants();

        Stats shieldedEnemy;

        foreach (EnemyStats enemy in allEnemies)
        {
            Debug.LogError("Enemy = " + enemy.getName());
            foreach(Trait trait in enemy.traits)
            {
                Debug.LogError("trait = " + trait.getName());
            }

            if (enemy.traits.Contains(TraitList.master) && enemy.traits.Contains(TraitList.shielded))
            {
                shieldedEnemy = enemy;

                SelectorManager.currentSelector.setToLocation(shieldedEnemy.position);

                SelectorManager.declareSelectors();

                SpawnHoverPanel.runInstanceOfScript();
                return;
            }
        }
    }

}
