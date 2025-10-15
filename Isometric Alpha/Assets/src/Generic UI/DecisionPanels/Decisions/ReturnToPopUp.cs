using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToPopUp : IDecision
{
	public const string ReturnToPopUpMessage = "Do you wish to close this window? Any changes you made will be lost.";

	public ISaveChangesPromptable popUpWindow;


	public ReturnToPopUp(ISaveChangesPromptable popUpWindow)
	{
		this.popUpWindow = popUpWindow;
	}
	
	public string getMessage()
	{
		return ReturnToPopUpMessage;
	}

	public void execute()
	{
		EscapeStack.removeTopObjectFromStack();
		popUpWindow.bypassChangesMadePromptAndClose();
	}
	
 
	public void backOut()
	{

	}
}
