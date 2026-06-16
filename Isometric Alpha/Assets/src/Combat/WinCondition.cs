using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate bool WinConditionCheck();
public delegate void CombatEndBehaviour();

public class WinCondition
{

    private string winConMessage = "";

    private WinConditionCheck winLogic;
    private CombatEndBehaviour winBehaviour;
    private CombatEndBehaviour lossBehaviour;

    public WinCondition(string winConMessage = "", WinConditionCheck winLogic = null, CombatEndBehaviour winBehaviour = null, CombatEndBehaviour lossBehaviour = null)
    {
        this.winConMessage = winConMessage;
        this.winLogic = winLogic ?? WinLoseConditionList.defeatAllEnemiesLogic;
        this.winBehaviour = winBehaviour ?? WinLoseConditionList.gameOver;
        this.lossBehaviour = lossBehaviour ?? WinLoseConditionList.gameOver;
    }

    public string getWinConDescription()
    {
        return winConMessage;
    }

    public bool playerHasWon()
    {
        return winLogic();
    }

    public void performWinBehaviour()
    {
        CombatUIModule.OnHideCombatUI.Invoke();
        winBehaviour();
    }

    public void performLossBehaviour()
    {
        CombatUIModule.OnHideCombatUI.Invoke();
        lossBehaviour();
    }

}

public class EndOfCombatCutSceneScript
{
    private const float waitBeforeResultsScreen = 5f;

    private static readonly string[] cutSceneSpriteNames = new string[]
    {
        "Javelineer", "Disciplinarian", "Spearman", "Axeman"
    };

    public void startCutScene()
    {
        CombatStateManager.instance.StartCoroutine(playCutScene());
    }

    private IEnumerator playCutScene()
    {
        foreach (string spriteName in cutSceneSpriteNames)
        {
            AnimationManager.SetIdleByNPCName.Invoke(MonsterNameList.puppetedPrefix + spriteName, CharacterAnimationType.OOC_Idle_Front);
        }

        yield return new WaitForSeconds(waitBeforeResultsScreen);

		CombatUI.combatResultsPopUpButton.spawnPopUp();
    }
}
