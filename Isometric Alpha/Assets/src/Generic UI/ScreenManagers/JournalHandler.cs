using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class JournalHandler : ScreenManager
{
    public static Dictionary<string, string> subcategoryDictionary = new Dictionary<string, string>();


    public override void updateCounter()
    {
        // updateAllStatsPanels();
        // populateAllGrids();
    }

    public override List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(AbilityGridSideTab.OnSideTabChosen);
        listOfEvents.Add(OnScreenInteriorUpdate);

        return listOfEvents;
    }

    public override bool requiresPartyMemberSelectionGrid()
    {
        return false;
    }

    public override DescribableList getDefaultDescribableList()
    {
        return DescribableList.Quests;
    }

    public override KeyCode getExitKeyCode()
    {
        return KeyBindingList.journalScreenKey;
    }

}
