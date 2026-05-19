using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class FormationHandler : ScreenManager, IPartyEditor, ICounter
{
    public FormationDisplayUI formationDisplayUI;

    public TextMeshProUGUI slotTracker;

    public DescriptionPanelSlot primaryStatSlot;

    public DescriptionPanel[] portraitPanels;

    public override bool enableSpriteRowDragAndDrop()
    {
        return true;
    }

    //IPartyEditor methods

    public void updateSlotTracker()
    {
        slotTracker.text = State.formation.getSizeOfFormation() + " / " + PartyStats.getPartySizeMaximum();
    }

    public void updatePortraits()
    {
        List<AllyStats> partyMembers = State.formation.getAllPartyStatsInFormation();

        for(int index = 0; index < portraitPanels.Length; index++)
        {
            if(index >= partyMembers.Count)
            {
                portraitPanels[index].iconPanel.sprite = Resources.Load<Sprite>(PrefabNames.blankTexture);
                portraitPanels[index].setObjectBeingDescribed(null);
            } else
            {
                if(partyMembers[index] == null)
                {
                    continue;
                }

                portraitPanels[index].iconPanel.sprite = PartyMember.getPortrait(partyMembers[index].getName());
                portraitPanels[index].setObjectBeingDescribed(partyMembers[index]);
            }
        }
    }

    public void addCharacterToFormation(AllyStats characterToAdd, int row, int col)
    {
        if (State.formation.canWriteToSlotWithoutOverride(row, col) && !State.formation.contains(characterToAdd))
        {
            State.formation.setCharacterAtCoords(row, col, characterToAdd);
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
        updatePortraits();
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
        return KeyBindingList.partyScreenKey.getCurrentKeyCode();
    }
}
