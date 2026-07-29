using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolveTurnWithNoActions : IDecision
{
    public ResolveTurnWithNoActions()
    {
        
    }

	public string getMessage()
    {
        return "Are you certain you want to resolve the turn? You have not selected any actions.";
    }
 
	public void execute()
    {
		EscapeStack.handleEscapePress();
        
        CombatStateManager.resolveTurn(skipNoActionCheck: true);
    }
 
	public void backOut()
    {
        CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingActor);
    }

    public static void executeCurrentDecision()
    {
        IDecision resolveTurn = new ResolveTurnWithNoActions();

        resolveTurn.execute();
    }

    public static void backOutOfCurrentDecision()
    {
        IDecision resolveTurn = new ResolveTurnWithNoActions();

        resolveTurn.backOut();
    }


}
