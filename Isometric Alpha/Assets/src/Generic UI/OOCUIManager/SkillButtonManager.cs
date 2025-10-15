using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButtonManager : MonoBehaviour
{

    public void useIntimidate()
    {
        if (PlayerOOCStateManager.currentActivity != OOCActivity.intimidating)
        {
            IntimidateManager.enterIntimidateMode();
        }
        else
        {
            IntimidateManager.leaveIntimidateMode();            
        }
    }

    public void useCunning()
    {
        if (PlayerOOCStateManager.currentActivity != OOCActivity.cunning)
        {
            CunningManager.enterCunningMode(); 
        }
        else
        {
            CunningManager.leaveCunningMode();            
        }
    }

    public void useObservation()
    {
        if (PlayerOOCStateManager.currentActivity != OOCActivity.observing)
        {
            ObservationManager.enterObservationMode();            
        }
        else
        {
            ObservationManager.leaveObservationMode();            
        }
    }


    public void useLeadership()
    {
        if (PartyMemberPlacer.getPlacedPartyMemberCount() < PartyStats.getMaxPlacablePartyMembers())
		{
			PartyMemberPlacer.placeNextPartyMember();
		}
    }
}
