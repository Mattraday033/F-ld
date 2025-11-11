using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberTrainPriority : MonoBehaviour
{
	public int partyMemberPriority;
    public string partyMemberName;
    public SpriteRenderer sprite;
	public Collider2D collider;
	

    // void Update() //here for Animation
    // {
    //     Collider2D[] collisions = Helpers.getCollisions(collider);

    //     if (collisions == null || collisions is null)
    //     {
    //         sprite.enabled = true;
    //         return;
    //     }

    //     foreach (Collider2D collision in collisions)
    //     {
    //         if (collision == null || collision is null)
    //         {
    //             continue;
    //         }

    //         int layerOfCollision = collision.gameObject.layer;
    //         string tagOfCollision = collision.gameObject.tag;

    //         if (layerOfCollision != trainLayerMask)
    //         {
    //             continue;
    //         }

    //         if (!String.Equals(tagOfCollision, LayerAndTagManager.npcTag, StringComparison.OrdinalIgnoreCase))
    //         {
    //             sprite.enabled = false;
    //             return;
    //         }

    //         int priorityOfCollision = collision.transform.parent.GetComponent<PartyMemberTrainPriority>().partyMemberPriority;

    //         if (priorityOfCollision < partyMemberPriority)
    //         {
    //             sprite.enabled = false;
    //             return;
    //         }
    //     }

    //     sprite.enabled = true;
    // }
    
    private void OnEnable()
    {
        PartyMemberPlacer.DestroyAllFollowers.AddListener(destroySelf);
        PartyMemberPlacer.HideAllFollowers.AddListener(hideSelf);
        PartyMemberPlacer.RevealAllFollowers.AddListener(revealSelf);        
    }

    private void OnDisable()
    {
        PartyMemberPlacer.DestroyAllFollowers.RemoveListener(destroySelf);
        PartyMemberPlacer.HideAllFollowers.RemoveListener(hideSelf);        
        PartyMemberPlacer.RevealAllFollowers.RemoveListener(revealSelf);        
    }

    private void destroySelf()
    {
        DestroyImmediate(gameObject);
        SkillManager.OnSkillUse.Invoke();
    }

    private void hideSelf()
    {
        sprite.color = Color.clear;
    }

    private void revealSelf()
    {
        sprite.color = Color.white;        
    }
}
