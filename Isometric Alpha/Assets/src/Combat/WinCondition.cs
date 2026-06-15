using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate bool WinConditionCheck();
public delegate void LossBehaviour();

public class WinCondition
{

    private string winConMessage = "";

    private WinConditionCheck winLogic;
    private LossBehaviour lossBehaviour;

    public WinCondition(string winConMessage = "", WinConditionCheck winLogic = null, LossBehaviour lossBehaviour = null)
    {
        this.winConMessage = winConMessage;
        this.winLogic = winLogic ?? WinLoseConditionList.defeatAllEnemiesLogic;
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

    public void performLossBehaviour()
    {
        lossBehaviour();
    }

}
