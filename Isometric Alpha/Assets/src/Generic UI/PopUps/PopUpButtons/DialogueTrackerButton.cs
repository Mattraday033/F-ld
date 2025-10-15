using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueTrackerButton : PopUpButton
{
	public bool withChoices;
	
	public DialogueTrackerButton():
	base(PopUpType.DialogueTrackerWithoutChoices)
	{
		
	}	

	public DialogueTrackerButton(bool withChoices):
	base(PopUpType.DialogueTrackerWithoutChoices)
	{
		this.withChoices = withChoices;
	}

	public void spawnEmptyPopUp()
	{
		if(withChoices)
		{
			type = PopUpType.DialogueTrackerWithChoices;
		}
		
		base.spawnPopUp();

		DialogueTrackerWindow currentWindow = (DialogueTrackerWindow) getPopUpWindow();
		
		PlayerOOCStateManager.setCurrentActivity(OOCActivity.inDialogue);
	}

	public override void spawnPopUp()
	{
		if (withChoices)
		{
			type = PopUpType.DialogueTrackerWithChoices;
		}
		else
		{
			type = PopUpType.DialogueTrackerWithoutChoices;
		}
		
		base.spawnPopUp();
		
		DialogueTrackerWindow currentWindow = (DialogueTrackerWindow) getPopUpWindow();
		
		PlayerOOCStateManager.setCurrentActivity(OOCActivity.inDialoguePopUp);
		
		currentWindow.populateDialogue();
	}

    public void showPopUpWindow()
    {
		GameObject popUpWindowGameObject = getCurrentPopUpGameObject();

		if(popUpWindowGameObject != null & !(popUpWindowGameObject is null))
		{
			popUpWindowGameObject.SetActive(true);
        }
    }

    public void hidePopUpWindow()
    {
        GameObject popUpWindowGameObject = getCurrentPopUpGameObject();

        if (popUpWindowGameObject != null & !(popUpWindowGameObject is null))
        {
            popUpWindowGameObject.SetActive(false);
        }
    }

    public override GameObject getCurrentPopUpGameObject()
    {	
		if(DialogueTrackerWindow.getInstance() != null && !(DialogueTrackerWindow.getInstance() is null))
		{
            return DialogueTrackerWindow.getInstance().gameObject;
        } else
		{
			return null;
		}
    }

}
