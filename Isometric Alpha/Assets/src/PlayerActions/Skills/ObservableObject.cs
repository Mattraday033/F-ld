using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservableObject : MonoBehaviour
{
	public bool observed = false;
	private SpriteRenderer sprite;
	
	public static int npcLayer = 16; // NPC layer
	
	void Start()
	{
		sprite = GetComponent<SpriteRenderer>();
	}
	
	public void markAsObserved()
	{
		observed = true;
		gameObject.layer = npcLayer;
		
		sprite.color = Color.magenta;
	}

}
