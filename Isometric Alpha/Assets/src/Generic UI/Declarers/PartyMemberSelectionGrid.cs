using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PartyMemberSelectionGrid : UIListenerGrid
{

    public override void addListeners()
    {
        base.addListeners();

        ScreenManager.OnScreenDeclaration.AddListener(setVisibility);
    }
    
    public override void removeListeners()
    {
        base.addListeners();

        ScreenManager.OnScreenDeclaration.RemoveListener(setVisibility);
    }

    public override void updateCounter()
    {
        if(Flags.isInNewGameMode())
        {
            return;
        }

        base.updateCounter();

        if(ScreenManager.currentPartyMember == null)
        {
            grid.disableGridRowAndClick(0);
        } else
        {
            grid.disableGridRowAndClick(ScreenManager.currentPartyMember.getName());
        }
    }

    public override List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        // listOfEvents.Add(PartySpriteGridRow.OnPartyMemberSelected); listening to PartyGridRow.OnPartyMemberSelected creates infinite loop
        listOfEvents.Add(PartyManager.OnPartyChange);
        listOfEvents.Add(ScreenManager.OnScreenInteriorUpdate);

        return listOfEvents;
    }

    private void setVisibility(ScreenManager screenManager)
    {
        gameObject.SetActive(screenManager.requiresPartyMemberSelectionGrid());
    }

    public override DescribableList getDescribableList()
    {
        return DescribableList.PartyMembers;
    }

}
