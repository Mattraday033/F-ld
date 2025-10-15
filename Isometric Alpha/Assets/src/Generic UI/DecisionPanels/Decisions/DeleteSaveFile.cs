using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSaveFile : IDecision
{
	private const string deleteMessageStart = "Are you sure you want to delete '";
	private const string deleteMessageEnd = "'? This can't be undone.";
	
	public string saveName;
	
	public DeleteSaveFile(string saveName)
	{
		this.saveName = saveName;
	}
	
	public string getMessage()
	{
		return deleteMessageStart + saveName + deleteMessageEnd;
	}
 
	public void execute()
	{
		SaveHandler.deleteSaveFile(saveName);
		
		OverallUIManager.currentScreenManager.populateAllGridsEnableAllRows();
		OverallUIManager.currentScreenManager.hideCurrentDescriptionPanel();
		
		EscapeStack.handleEscapePress();
	}
 
	public void backOut()
	{
		
	}
}
