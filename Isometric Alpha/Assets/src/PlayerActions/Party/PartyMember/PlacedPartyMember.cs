using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlacedPartyMember : MonoBehaviour
{
    public readonly static UnityEvent<PlacedPartyMember> PartyMemberLocationRequest = new UnityEvent<PlacedPartyMember>();

    public List<MovementTracker> movementTrackers = new List<MovementTracker>();

    public SpriteRenderer sprite;
    public AnimationManager animationManager;
    private PartyMember _PartyMember;
    public PartyMember partyMember
    {
        get
        {
            return _PartyMember;
        }
        set
        {
            _PartyMember = value;
            animationManager.setAnimations(_PartyMember.getName());
            animationManager.setFacing(State.playerFacing.getFacing());
        }
    }
    public Vector3Int currentCell;
    
    public void checkIfVisible(int i)
    {
        if(PlayerOOCStateManager.currentActivity != OOCActivity.walking && 
            PlayerOOCStateManager.currentActivity != OOCActivity.inTutorialSequence)
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

        MovementManager.OnMoveFinished.RemoveListener(checkIfVisible);        
    }

    private void destroySelf()
    {
        DestroyImmediate(gameObject);
        SkillManager.OnSkillUse.Invoke();
    }

    private void hideSelf()
    {
        sprite.color = Color.clear;
        animationManager.disableExtras();
    }

    private void revealSelf()
    {
        sprite.color = Color.white;    
        animationManager.enableExtras();    
    }
}
