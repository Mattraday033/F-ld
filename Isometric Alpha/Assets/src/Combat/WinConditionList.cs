using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WinLoseConditionList
{
    #region Loss Behaviour

    public readonly static CombatEndBehaviour showCombatResults = () =>
    {
		CombatUI.combatResultsPopUpButton.spawnPopUp();
    };

    public readonly static CombatEndBehaviour gameOver = () =>
    {
        CombatStateManager.getInstance().gameOverPopUpButton.spawnPopUp();
    };

    public readonly static CombatEndBehaviour takacsCutScene = () =>
    {
        EndOfCombatCutSceneScript script = new EndOfCombatCutSceneScript();

        script.startCutScene();
    };

    #endregion

    #region Win Condition 
    
    public static readonly WinConditionCheck defeatAllEnemiesLogic =         
        () => CombatGrid.getTotalAliveEnemyCount() == 0 ||
              CombatGrid.getEnemyMasterCount() == 0;
    public readonly static WinCondition defeatAllEnemies = new DefaultWinCondition(
        "Defeat Master Creatures",
        IconList.masterIcon,
        "Defeat all Master Creatures to Win. Minion/Summoned Creatures will flee after the last Master Creature is defeated."
    );

    public readonly static WavesWinCondition surival = new WavesWinCondition(
        Constants.sizeThree,
        IconList.waves,
        winBehaviour: takacsCutScene,
        lossBehaviour: takacsCutScene
    );

    #endregion

}
