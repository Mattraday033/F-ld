using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverwriteSaveFile : IDecision
{
	private const string overwriteMessageStart = "Are you sure you want to overwrite '";
	private const string overwriteMessageEnd = "'? This can't be undone.";
	
	public SaveBlueprint save;
	
	public OverwriteSaveFile(SaveBlueprint save)
	{
		this.save = save;
	}
	
	public string getMessage()
	{
		return overwriteMessageStart + save.getName() + overwriteMessageEnd;
	}
 
	public void execute()
	{
		SaveHandler.deleteSaveFile(save.getName());
		
		SaveHandler.save(save.getName());
		
		ScreenManager.OnScreenInteriorUpdate.Invoke();
		
		EscapeStack.handleEscapePress();
	}
 
	public void backOut()
	{
		
	}
}
