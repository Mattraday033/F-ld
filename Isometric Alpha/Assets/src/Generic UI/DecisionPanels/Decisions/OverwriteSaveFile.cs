using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverwriteSaveFile : IDecision
{
	private const string overwriteMessageStart = "Are you sure you want to overwrite '";
	private const string overwriteMessageEnd = "'? This can't be undone.";
	
	public string saveName;
	
	public OverwriteSaveFile(string saveName)
	{
		this.saveName = saveName;
	}
	
	public string getMessage()
	{
		return overwriteMessageStart + saveName + overwriteMessageEnd;
	}
 
	public void execute()
	{
		// Debug.LogError("Deleting " + saveName);
		
		SaveHandler.deleteSaveFile(saveName);
		
		SaveHandler.save(saveName);
		
		OverallUIManager.currentScreenManager.populateAllGridsEnableAllRows();
		OverallUIManager.currentScreenManager.hideCurrentDescriptionPanel();
		
		EscapeStack.handleEscapePress();
	}
 
	public void backOut()
	{
		
	}
}
