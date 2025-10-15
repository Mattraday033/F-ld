using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditAbilityWheelButton : PopUpButton
{
	public EditAbilityWheelButton(PopUpType type):
	base(type)
	{
		
	}

    public override GameObject getCurrentPopUpGameObject() 
    {
        if (EditAbilityWheelPopUpWindow.getInstance() != null && !(EditAbilityWheelPopUpWindow.getInstance() is null))
        {
            return EditAbilityWheelPopUpWindow.getInstance().gameObject;
        }
        else
        {
            return null;
        }
    }
}
