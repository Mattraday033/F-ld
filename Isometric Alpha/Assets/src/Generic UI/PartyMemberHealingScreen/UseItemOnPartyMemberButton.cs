using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UseItemOnPartyMemberButton : PopUpButton
{
	public UsableItem itemBeingDescribed;
	
	public UseItemOnPartyMemberButton():
	base(PopUpType.UseItemOnPartyMember)
	{
		
	}
	
	public void spawnPopUp(UsableItem itemBeingDescribed)
	{
		this.itemBeingDescribed = itemBeingDescribed;
		
		spawnPopUp();
	}
	
	public override void spawnPopUp()
	{
		base.spawnPopUp();
		
		PartyMemberSelectionScreen partyMemberSelectionScreen = (PartyMemberSelectionScreen) getPopUpWindow();
		
		partyMemberSelectionScreen.itemBeingDescribed = itemBeingDescribed;
		partyMemberSelectionScreen.populate();
	}

    public override GameObject getCurrentPopUpGameObject()
    {
		if(PartyMemberSelectionScreen.getInstance() == null || PartyMemberSelectionScreen.getInstance() is null)
		{
			return null;
		}

		return PartyMemberSelectionScreen.getInstance().gameObject;
    }

}
