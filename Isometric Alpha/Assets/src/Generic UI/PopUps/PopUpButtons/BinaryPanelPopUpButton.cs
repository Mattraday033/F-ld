using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum BinaryDescisionType {LoadSaveFile = 1, OverwriteSaveFile = 2, DeleteSaveFile = 3, QuitToDesktop = 4, ReturnToMainMenu = 5, Retreat = 6, FastTravel = 7, RaiseStats = 8}

public class BinaryPanelPopUpButton : PopUpButton
{
	[SerializeField]
	private BinaryDescisionType decisionType;
	
	public BinaryPanelPopUpButton():
	base(PopUpType.BinaryPanel)
	{
		
	}

	public void spawnPopUp(IDecision decision)
	{
		base.spawnPopUp();
		
		BinaryDescisionPanel binaryDecisionPanel = (BinaryDescisionPanel) getPopUpWindow();
		
		binaryDecisionPanel.populate(decision);
	}

	public override void spawnPopUp()
	{
		base.spawnPopUp();
		
		BinaryDescisionPanel binaryDecisionPanel = (BinaryDescisionPanel) getPopUpWindow();
		
		binaryDecisionPanel.populate(getDescisionType());
	}
	
	public void executeDecisionWithoutPopUp()
	{
		getDescisionType().execute();
	}
	
	private IDecision getDescisionType()
	{
		switch(decisionType)
		{
			case BinaryDescisionType.LoadSaveFile:
				return new LoadSaveFile(SaveHandler.getCurrentSaveName());
			case BinaryDescisionType.OverwriteSaveFile:
				return new OverwriteSaveFile(SaveHandler.getCurrentSaveName());
			case BinaryDescisionType.DeleteSaveFile:
				return new DeleteSaveFile(SaveHandler.getCurrentSaveName());
			case BinaryDescisionType.QuitToDesktop:
				return new QuitToDesktop();
            case BinaryDescisionType.ReturnToMainMenu:
                return new ReturnToMainMenu();
            case BinaryDescisionType.Retreat:
                return new Retreat();
            case BinaryDescisionType.FastTravel:
                return new FastTravel(MapPopUpWindow.getInstance().fastTravelTarget);
            case BinaryDescisionType.RaiseStats:
                return new AddStatPoint(OverallUIManager.getCurrentPartyMember());
            default:
				throw new IOException("Unknown BinaryDescisionType: " + decisionType.ToString());
		}
	}
    public override GameObject getCurrentPopUpGameObject()
    {
        if (BinaryDescisionPanel.getInstance() != null && !(BinaryDescisionPanel.getInstance() is null))
        {
            return BinaryDescisionPanel.getInstance().gameObject;
        }
        else
        {
            return null;
        }
    }
}
