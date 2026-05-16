using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnapSelectorToMaster : TutorialSequenceStepScript
{
    public override void runScript(GameObject target = null) 
    {
        // snaps to master with shielded trait
        List<Stats> allEnemies = CombatGrid.getAllAliveEnemyCombatants();

        Stats shieldedEnemy;

        foreach (EnemyStats enemy in allEnemies)
        {
            if (enemy.isMaster() && enemy.hasTrait(TraitList.shielded))
            {
                shieldedEnemy = enemy;

                if (shieldedEnemy.positions.Count > 0)
                {
                    SelectorManager.currentSelector.setToLocation(shieldedEnemy.positions[0]);
                }

                SelectorManager.declareSelectors();

                SpawnHoverPanel.runInstanceOfScript();
                return;
            }
        }
    }

}

public class SnapSelectorToMandatoryTarget : TutorialSequenceStepScript
{
    public override void runScript(GameObject target = null) 
    {
        // snaps to master with shielded trait
        List<Stats> allEnemies = CombatGrid.getAllAliveEnemyCombatants();

        Stats mandatoryTarget;

        foreach (EnemyStats enemy in allEnemies)
        {
            if (enemy.isMandatoryTarget())
            {
                mandatoryTarget = enemy;

                if (mandatoryTarget.positions.Count > 0)
                {
                    SelectorManager.currentSelector.setToLocation(mandatoryTarget.positions[0]);
                }

                SelectorManager.declareSelectors();

                SpawnHoverPanel.runInstanceOfScript();
                return;
            }
        }
    }

}
