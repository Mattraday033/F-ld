using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillIndicator : MonoBehaviour
{
	public Collider2D collider;

    public EffectAnimationManager frontSelector;
    public EffectAnimationManager backSelector;

    public GameObject tileMapCollider;

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
        Helpers.updateColliderPosition(tileMapCollider);
    }

    public bool collidedWithTarget(GameObject tile, ContactFilter2D filterCollider)
    {
        Collider2D[] collisions = Helpers.getCollisions(tile.GetComponent<Collider2D>(), filterCollider);

        foreach (Collider2D collision in collisions)
        {
            if (collision == null || collision is null)
            {
                continue;
            }

            if (collision.GetComponent<ISkillTarget>() != null)
            {
                return true;
            }
        }

        return false;
    }

    protected abstract ContactFilter2D getFilterCollider();

    public abstract void setColor();
    
}
