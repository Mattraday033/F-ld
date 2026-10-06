using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlacedPartyMember : MonoBehaviour, INameSource, INameTagSuppressor
{
    public readonly static UnityEvent<PlacedPartyMember> PartyMemberLocationRequest = new UnityEvent<PlacedPartyMember>();

    public List<MovementTracker> movementTrackers = new List<MovementTracker>();

    private SpriteLayerRendererList rendererList;
    private OverHeadIconManager iconManager;

    public PartyMember partyMember;
    public Vector3Int currentCell;

    public string displayName { get { return partyMember != null ? partyMember.displayName : Constants.emptyString; } }
    public string uniqueName { get { return partyMember != null ? partyMember.uniqueName : Constants.emptyString; } }

    private bool _Hidden;
    public bool hidden { get { return _Hidden; } }

    //a hidden party member is standing under the player or a follower, so it shows no name tag or outline either
    public bool suppressNameTag { get { return _Hidden; } }

    //both arrive with the prefab and the aesthetic spawn behaviours, so they are there before OnEnable needs them
    private void Awake()
    {
        rendererList = GetComponent<SpriteLayerRendererList>();
        iconManager = GetComponent<OverHeadIconManager>();
    }

    public void checkIfVisible(int i)
    {
        if(PlayerStateManager.currentActivity != CurrentActivity.Walking &&
            PlayerStateManager.currentActivity != CurrentActivity.InTutorialSequence)
        {
            return;
        }

        PartyMemberLocationRequest.Invoke(this);

        foreach(MovementTracker movementTracker in movementTrackers)
        {
            if(currentCell.Equals(movementTracker.getCell()))
            {
                hideSelf();
                movementTrackers = new List<MovementTracker>();
                return;
            }
        }

        revealSelf();
        movementTrackers = new List<MovementTracker>();
    }

    private void OnEnable()
    {
        PartyMemberPlacer.DestroyAllFollowers.AddListener(destroySelf);
        PartyMemberPlacer.HideAllFollowers.AddListener(hideSelf);
        PartyMemberPlacer.RevealAllFollowers.AddListener(revealSelf);

        //OnStepFinished checks after every step, since OnMoveFinished only fires once a mover stops.
        //OnMoveFinished is still needed for the places that invoke it by hand, like a party member being removed
        MovementManager.OnStepFinished.AddListener(checkIfVisible);
        MovementManager.OnMoveFinished.AddListener(checkIfVisible);

        currentCell = AreaManager.getMasterGrid().WorldToCell(transform.position);

        if(currentCell.Equals(PlayerMovement.getInstance().getCell()))
        {
            hideSelf();
        }
    }

    private void OnDisable()
    {
        PartyMemberPlacer.DestroyAllFollowers.RemoveListener(destroySelf);
        PartyMemberPlacer.HideAllFollowers.RemoveListener(hideSelf);
        PartyMemberPlacer.RevealAllFollowers.RemoveListener(revealSelf);

        MovementManager.OnStepFinished.RemoveListener(checkIfVisible);
        MovementManager.OnMoveFinished.RemoveListener(checkIfVisible);
    }

    //covers the ways of being destroyed that do not go through destroySelf, like the area being wiped
    private void OnDestroy()
    {
        forgetSelf();
    }

    private void destroySelf()
    {
        //Destroy waits for the end of the frame, so the list has to be right before anything counts it
        forgetSelf();

        Destroy(gameObject);
        SkillManager.OnSkillUse.Invoke();
    }

    private void forgetSelf()
    {
        PartyMemberPlacer.placedPartyMembers.Remove(this);
    }

    private void hideSelf()
    {
        setHidden(true);
    }

    private void revealSelf()
    {
        setHidden(false);
    }

    //checkIfVisible calls in after every step, so only a change of state may touch the outline and name tag a hover put up
    private void setHidden(bool hide)
    {
        if(_Hidden == hide)
        {
            return;
        }

        _Hidden = hide;

        if(hide)
        {
            rendererList.disableAllLayers();
        } else
        {
            rendererList.enableAllLayers();
        }

        //left on, the body collider would take the mouse hover from whatever shares the cell
        rendererList.setBodyColliderEnabled(!hide);

        if(iconManager != null)
        {
            iconManager.onReveal(!hide && RevealManager.currentlyRevealed);
        }
    }
}
