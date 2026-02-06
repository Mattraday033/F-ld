using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class FormationHandler : ScreenManager, IPartyEditor, ICounter
{
    public FormationDisplayUI formationDisplayUI;

    public TextMeshProUGUI slotTracker;

    public DescriptionPanelSlot primaryStatSlot;

    public override bool enableSpriteRowDragAndDrop()
    {
        return true;
    }

    //IPartyEditor methods

    public void updateSlotTracker()
    {
        slotTracker.text = State.formation.getSizeOfFormation() + " / " + PartyStats.getPartySizeMaximum();
    }

    public void addCharacterToFormation(AllyStats characterToAdd, int row, int col)
    {
        if (State.formation.canWriteToSlot(row, col) && !State.formation.contains(characterToAdd))
        {
            State.formation.setCharacterAtCoords(characterToAdd, row, col);
            OnScreenInteriorUpdate.Invoke();
        }
    }

    public void removeCharacter(AllyStats characterToRemove)
    {
        State.formation.removeCharacter(characterToRemove);

        OnScreenInteriorUpdate.Invoke();
    }

    public AllyStats getSelectedPartyMember()
    {
        if (PartyMemberDragAndDrop.getInstance() == null)
        {
            return null;
        }
        else
        {
            return (AllyStats)PartyMemberDragAndDrop.getInstance().getObjectBeingDragged();
        }
    }

    public Formation getFormation()
    {
        return State.formation;
    }

    //ICounter Methods

    private void OnEnable()
    {
        addListeners();
    }

    private void OnDestroy()
    {
        removeListeners();
        MouseHoverManager.destroyMouseHoverBase();
    }

    public override void addListeners()
    {
        base.addListeners();

        PlayerOOCStateManager.OnStateChangeFromInUI.AddListener(MouseHoverManager.destroyMouseHoverBase);
    }
    public override void removeListeners()
    {
        base.removeListeners();
        
        PlayerOOCStateManager.OnStateChangeFromInUI.RemoveListener(MouseHoverManager.destroyMouseHoverBase);
    }

    public override void updateCounter()
    {
        updateSlotTracker();
    }

    public override List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(Formation.OnFormationChange);
        listOfEvents.Add(PartySpriteGridRow.OnPartyMemberSelected);
        listOfEvents.Add(OnScreenInteriorUpdate);

        return listOfEvents;
    }

    public override bool requiresPartyMemberSelectionGrid()
    {
        return true;
    }

    public override DescribableList getDefaultDescribableList()
    {
        return DescribableList.Unnecessary;
    }

    public override KeyCode getExitKeyCode()
    {
        return KeyBindingList.partyScreenKey;
    }
}
