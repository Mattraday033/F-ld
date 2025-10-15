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
	
	public static int trainLayerMask = 27;
	
    void Start()
    {
        
    }

    void Update() //here for Animation
    {
		Collider2D[] collisions = Helpers.getCollisions(collider);
		
		if(collisions == null || collisions is null)
		{
			sprite.enabled = true;
			return;
		}
		
		foreach(Collider2D collision in collisions)
		{
			if(collision == null || collision is null)
			{
				continue;
			}
			
			int layerOfCollision = collision.gameObject.layer;
			string tagOfCollision = collision.gameObject.tag;
			
			if(layerOfCollision != trainLayerMask)
			{
				continue;
			}
			
			if(!String.Equals(tagOfCollision, LayerAndTagManager.npcTag, StringComparison.OrdinalIgnoreCase))
			{
				sprite.enabled = false;
				return;
			}
			
			int priorityOfCollision =  collision.transform.parent.GetComponent<PartyMemberTrainPriority>().partyMemberPriority;
			
			if(priorityOfCollision < partyMemberPriority)	
			{
				sprite.enabled = false;
				return;
			} 
		}
		
		sprite.enabled = true;
    }
}
