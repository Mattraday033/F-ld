using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortLayerManager : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;
	
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer playerSpriteRenderer = PlayerMovement.getInstance().gameObject.GetComponent<SpriteRenderer>();
		
		spriteRenderer.sortingOrder = playerSpriteRenderer.sortingOrder;
    }
}
