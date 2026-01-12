using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSaveFile : IDecision
{
	private const string deleteMessageStart = "Are you sure you want to delete '";
	private const string deleteMessageEnd = "'? This can't be undone.";
	
	public SaveBlueprint save;
	
	public DeleteSaveFile(SaveBlueprint save)
	{
		this.save = save;
	}
	
	public string getMessage()
	{
		return deleteMessageStart + save.getName() + deleteMessageEnd;
	}
 
	public void execute()
	{
		SaveHandler.deleteSaveFile(save.getName());
		
		ScreenManager.OnScreenInteriorUpdate.Invoke();
		
		EscapeStack.handleEscapePress();
	}
 
	public void backOut()
	{
		
	}
}
