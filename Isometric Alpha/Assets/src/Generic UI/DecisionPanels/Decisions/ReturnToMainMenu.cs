using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMainMenu : IDecision
{
	public const string returnToMainMenuMessage = "Are you sure you want to return to the Main Menu? Any unsaved progress will be lost.";
	
	public ReturnToMainMenu()
	{

	}
	
	public string getMessage()
	{
		return returnToMainMenuMessage;
	}
 
	public void execute()
	{
        Flags.resetAllFlags();
        PartyManager.resetPartyMembers();

        CombatStateManager.inCombat = false;
        CombatStateManager.useReturnCell();

        AudioManager.endAmbience();

        EscapeStack.instantiateEscapeStack();

        SceneChange.changeSceneToStartMenu();
    }
 
	public void backOut()
	{
		
	}
}
