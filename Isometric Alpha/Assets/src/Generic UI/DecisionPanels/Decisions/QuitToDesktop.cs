using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitToDesktop : IDecision
{
	public const string quitGameMessage = "Are you sure you want to quit? Any unsaved progress will be lost.";
	
	public QuitToDesktop()
	{

	}
	
	public string getMessage()
	{
		return quitGameMessage;
	}
 
	public void execute()
	{
        HeadlessLogger.LogEvent("Game.QuitRequested", "source", nameof(QuitToDesktop));

        EscapeStack.instantiateEscapeStack();

        Config.writeConfig();

        Application.Quit();
    }
 
	public void backOut()
	{
		
	}
}
