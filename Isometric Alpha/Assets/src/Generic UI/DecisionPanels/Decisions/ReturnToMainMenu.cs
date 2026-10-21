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
 
	//true while the screen fades out ahead of the return, so a second press doesn't start it twice
	private static bool fadingOut = false;

	//statics survive between play sessions with domain reload disabled
	[RuntimeInitializeOnLoadMethod]
	private static void init()
	{
		fadingOut = false;
	}

	//The scene change blocks for a few frames, and the last frame drawn before it would otherwise be the UI half torn down,
	//so the screen goes black first. It stays black: the start menu fades back in from black as it loads.
	//The game is reset after the fade, so nothing on screen changes while it's fading
	public void execute()
	{
		if(fadingOut)
		{
			return;
		}

		fadingOut = true;

		FadeToBlackManager.quickFadeToBlackThen(() =>
		{
			fadingOut = false;
			returnToMainMenu();
		});
	}

	private static void returnToMainMenu()
	{
        PlayerStateManager.setCurrentActivity(CurrentActivity.MainMenu);

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
