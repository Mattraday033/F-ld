using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartySlotTracker : MonoBehaviour
{
	public TextMeshProUGUI slotTrackerText;
	
	public AdjustPartyRosterManager adjustPartyRosterManager;
	
	public UnassignedPartyMemberGridManager unassignedPartyMemberGridManager;
	
    void Start()
    {
        updateSlotTrackerText();
    }	
	
	public void updateSlotTrackerText()
	{
		int usedSlots = adjustPartyRosterManager.getInterimUsedSlots();

        int maxSlots = PartyStats.getPartySizeMaximum();
		
		slotTrackerText.text = usedSlots + "/" + maxSlots;
	}
}
