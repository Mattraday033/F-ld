using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WinLoseConditionList
{
    #region Loss Behaviour

    public readonly static LossBehaviour gameOver = () =>
    {
        CombatStateManager.getInstance().gameOverPopUpButton.spawnPopUp();
    };

    #endregion

    #region Win Condition
    
    public static readonly WinConditionCheck defeatAllEnemiesLogic =         
        () => CombatGrid.getTotalAliveEnemyCount() == 0 ||
              CombatGrid.getEnemyMasterCount() == 0;

    public static readonly WinConditionCheck surviveFourRoundsLogic =  
        () => {
            return CombatStateManager.turnNumber >= Constants.sizeFive;
        };


    public readonly static WinCondition defeatAllEnemies = new WinCondition(
        "Defeat all Master Creatures to Win. Minion/Summoned Creatures will flee after the last Master Creature is defeated."
    );

    public readonly static WinCondition surival = new WinCondition(
        "If the party is defeated, they will be moved to another location instead of dying. Survive for four rounds to receive a bonus reward at the end of combat.\n\n<i>The enemy is without end.</i>",
        surviveFourRoundsLogic
    );

    #endregion

}
