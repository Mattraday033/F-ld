using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class FormationHandler : ScreenWithGeneratedPartyTabs, IPartyEditor, ICounter
{
    private const int party2x3GridIndex = 1;

    public FormationDisplayUI formationDisplayUI;

    public TextMeshProUGUI slotTracker;

    public DescriptionPanelSlot skillSlot;
    public DescriptionPanelSlot exuberanceSlot;
    public DescriptionPanelSlot primaryStatSlot;
    public DescriptionPanelSlot secondaryStatSlot;

    public override void Awake()
    {
        base.Awake();

        formationDisplayUI.setToReadOnly();
        updateAllStatsPanels();
        populateAllGrids();
    }

    public override void updateAllStatsPanels()
    {
        skillSlot.setPrimaryDescribable(State.formation);
        exuberanceSlot.setPrimaryDescribable(State.formation);
        primaryStatSlot.setPrimaryDescribable(State.formation);
        secondaryStatSlot.setPrimaryDescribable(State.formation);
    }

    public override void populateAllGrids()
    {
        grids[party2x3GridIndex].populatePanels(State.formation.getAllPartyStatsInFormation()); 
        readInCurrentFormation();
    }

    public void removeAllPartyMembersFromCurrentPartyAndDisplay()
    {
        PartyManager.removeAllPartyMembersFromCurrentParty();

        readInCurrentFormation();
    }

    public override void revealDescriptionPanelSet(IDescribable objectToDescribe)
    {
        //empty on purpose
    }

    public override void populateObjectAttachedToSpriteRowButton(PartyMember partyMember)
    {
        // grids[2].populatePanels(partyMember.getCombatActionsForDisplay());
    }

    public override bool enableSpriteRowDragAndDrop()
    {
        return true;
    }

    //IPartyEditor methods

    public void updateSlotTracker()
    {
        slotTracker.text = State.formation.getSizeOfFormation() + " / " + PartyStats.getPartySizeMaximum();
    }

    public void populateFormationGrid()
    {
        formationDisplayUI.populate(State.formation);
        updateSlotTracker();
    }

    public void readInCurrentFormation()
    {
        populateFormationGrid();
    }

    public void writeFormation()
    {
        populateFormationGrid();
    }

    public void addCharacterToFormation(AllyStats characterToAdd, int row, int col)
    {
        Debug.LogError("characterToAdd = " + characterToAdd.getName());

        if (State.formation.canWriteToSlot(row, col) && !State.formation.contains(characterToAdd))
        {
            State.formation.setCharacterAtCoords(characterToAdd, row, col);
            populateFormationGrid();
        }
    }

    public void removeCharacter(AllyStats characterToRemove)
    {
        State.formation.removeCharacter(characterToRemove);

        populateFormationGrid();
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

    public override AllyStats getCurrentPartyMember()
    {
        return Stats.convertIDescribableToStats(grids[0].getDisabledRowDescribable());
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

    public void addListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.AddListener(updateCounter);
        }

        PlayerOOCStateManager.OnStateChangeFromInUI.AddListener(MouseHoverManager.destroyMouseHoverBase);
    }
    public void removeListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.RemoveListener(updateCounter);
        }
        
        PlayerOOCStateManager.OnStateChangeFromInUI.RemoveListener(MouseHoverManager.destroyMouseHoverBase);
    }

    public void updateCounter()
    {
        updateAllStatsPanels();
        populateAllGrids();
    }

    public List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(Formation.OnFormationChange);

        return listOfEvents;
    }
}
