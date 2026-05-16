using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnapSelectorToMinion : TutorialSequenceStepScript
{
    public override void runScript(GameObject target = null)
    {

        List<Stats> allEnemies = CombatGrid.getAllAliveEnemyCombatants();

        List<Stats> allMasterEnemies = new List<Stats>();

        foreach (EnemyStats enemy in allEnemies)
        {
            if (enemy.isMinion())
            {
                allMasterEnemies.Add(enemy);
            }
        }

        List<int> allDistancesFromPlayer = new List<int>();

        Stats playerStats = PartyManager.getPlayerStats();
        GridCoords playerPosition = playerStats != null && playerStats.positions.Count > 0 ? playerStats.positions[0] : GridCoords.getDefaultCoords();

        foreach(Stats master in allMasterEnemies)
        {
            int minDistance = master.positions.Count > 0 ? master.positions.Min(p => playerPosition.distanceTo(p)) : int.MaxValue;
            allDistancesFromPlayer.Add(minDistance);
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

        Stats closestMaster = allMasterEnemies[closestIndex];
        if (closestMaster.positions.Count > 0)
        {
            SelectorManager.currentSelector.setToLocation(closestMaster.positions[0]);
        }
        
        SpawnHoverPanel.runInstanceOfScript();
    }
}
