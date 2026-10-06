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
        base.removeListeners();

        ScreenManager.OnScreenDeclaration.RemoveListener(setVisibility);
    }

    public override void updateCounter()
    {
        if(PlayerStateManager.inMainMenu())
        {
            return;
        }

        //switched off by setVisibility for a screen with no use for the strip. The next screen that wants it switches it on before its update goes out.
        //A fill here would also fetch the party list, which a journal that is showing takes as its own list and titles itself after
        if (!gameObject.activeSelf)
        {
            return;
        }

        base.updateCounter();

        // if(ScreenManager.currentPartyMember == null)
        // {
        //     grid.disableGridRowAndClick(0);
        // } else
        // {
        //     grid.disableGridRow(ScreenManager.currentPartyMember.uniqueName);
        // }
    }

    public override List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(PartyManager.OnPartyChange);
        listOfEvents.Add(ScreenManager.OnScreenInteriorUpdate);

        return listOfEvents;
    }

    private void setVisibility(ScreenManager screenManager)
    {
        gameObject.SetActive(screenManager.requiresPartyMemberSelectionGrid());
    }

    public override DescribableList getDescribableListType()
    {
        return DescribableList.PartyMembers;
    }

}
