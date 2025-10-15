using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnapSelectorToMinion : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {

        ArrayList allEnemies = CombatGrid.getAllAliveEnemyCombatants();

        List<Stats> allMasterEnemies = new List<Stats>();

        foreach (EnemyStats enemy in allEnemies)
        {
            if (enemy.traits.Contains(TraitList.minion))
            {
                allMasterEnemies.Add(enemy);
            }
        }

        List<int> allDistancesFromPlayer = new List<int>();

        GridCoords playerPosition = PartyManager.getPlayerStats().position;
       
        foreach(Stats master in allMasterEnemies)
        {
            allDistancesFromPlayer.Add(playerPosition.distanceTo(master.position));
        }

        int closestDistance = 16;
        int closestIndex = 0;

        for(int index = 0; index < allDistancesFromPlayer.Count; index++)
        {
            if (allDistancesFromPlayer[index] < closestDistance)
            {
                closestDistance = allDistancesFromPlayer[index];
                closestIndex = index;
            }
        }

        SelectorManager.currentSelector.setToLocation(allMasterEnemies[closestIndex].position);
        
        SpawnHoverPanel.runInstanceOfScript();
    }
}
