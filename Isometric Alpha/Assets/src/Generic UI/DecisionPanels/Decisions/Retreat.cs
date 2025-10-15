using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retreat : IDecision
{
	private const string retreatWithChanceOfFailureMessageStart = "Are you sure you want to retreat? You have a ";
    private const string retreatWithChanceOfFailureMessageEnd = " chance of success. Should you fail, your party will skip their turn and the enemy will take it's entire turn before you get to act. If you succeed, the enemy will be fully restored when you return.";
    private const string retreatAlwaysSucceedMessage = "Are you sure you want to retreat? Your retreat attempt cannot fail, but the enemy will be fully restored when you return.";
	private const float automaticRetreatSuccessThreshold = 1f;

    private const bool retreatedFromEnemy = false; 

    public Retreat()
	{

	}
	
	public string getMessage()
	{
		float retreatChance = calculateRetreatChance();

		if(retreatChance >= automaticRetreatSuccessThreshold)
		{
			return retreatAlwaysSucceedMessage;
        } else
		{
            return retreatWithChanceOfFailureMessageStart + getRetreatChanceForDisplay() + retreatWithChanceOfFailureMessageEnd;
        }
	}

    public static string getRetreatChanceForDisplay()
    {
        float retreatChance = calculateRetreatChance();

        if (retreatChance < 0f)
        {
            retreatChance = 0f;
        }
        else if (retreatChance > 1f)
        {
            retreatChance = 1f;
        }

        retreatChance = retreatChance * 100f;

        string retreatChanceForDisplay = "" + retreatChance;

        retreatChanceForDisplay = retreatChanceForDisplay.Split(".")[0];

        return retreatChanceForDisplay + "%";
    }

    public static float calculateRetreatChance()
    {
        if (State.enteredCombatFromDialogue)
        {
            return 0f;
        }
        else if (State.debugRetreatAutoSucceed)
        {
            return 1f;
        }

        float playerRetreatChanceBonus = ((float) PartyStats.getRetreatChanceBonus())/100f;
        float totalRetreatChance = Dexterity.baseRetreatChance + playerRetreatChanceBonus;

        return totalRetreatChance; 
    }

    public void execute()
	{
        bool retreatSucceeded = rollAgainstRetreatChance();

        if (retreatSucceeded)
        {
            CombatStateManager.returnToOverworld(retreatedFromEnemy);
        } else
        {
            PlayerCombatActionManager.removeAllPlayerActions();

            EscapeStack.handleEscapePress();

            CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingActor); 

            CombatStateManager.getInstance().resolveTurn();
        }
    }
 
    private bool rollAgainstRetreatChance()
    {
        float dieRoll = UnityEngine.Random.Range(0f, 1f);
        float successThreshold = (1f - calculateRetreatChance());

        if(dieRoll >= successThreshold)
        {
            return true;
        } else
        {
            return false;
        }
    }

	public void backOut()
	{
        CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingActor);
    }
}
