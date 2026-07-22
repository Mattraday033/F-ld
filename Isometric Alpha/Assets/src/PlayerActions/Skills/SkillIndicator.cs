using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillIndicator : MonoBehaviour
{
	public Collider2D collider;

    public EffectAnimationManager frontSelector;
    public EffectAnimationManager backSelector;

    public GameObject tileMapGameObject;

    private void Awake()
    {        
        frontSelector.loops = true;
        frontSelector.setAnimations(EffectAnimationType.FrontSelector);

        backSelector.loops = true;
        backSelector.setAnimations(EffectAnimationType.BackSelector);
    }

	public void disableSelf(bool deactivate)
	{
		if(deactivate)
		{
			gameObject.SetActive(false);
		} else
		{
			enabled = false;
		}
	}
	
    public void updateColliderPosition()
    {
        Helpers.updateColliderPosition(tileMapGameObject);
    }

    public void setColor(Color color)
    {
        frontSelector.spriteRenderer.color = color;
        backSelector.spriteRenderer.color = color;
    }

}
