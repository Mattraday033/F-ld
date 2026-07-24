using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIndicator : MonoBehaviour
{
	public Collider2D collider;

    private Color oldColor;

    public GameObject selectorOne;
    public GameObject selectorTwo;

    public Material defaultSprite;
    public Material frontSelectorOneMat;
    public Material backSelectorOneMat;

    public EffectAnimationManager frontSelector;
    public EffectAnimationManager backSelector;

    public EffectAnimationManager frontSelectorTwo;
    public EffectAnimationManager backSelectorTwo;

    public GameObject tileMapGameObject;

    private void Awake()
    {        
        frontSelector.loops = true;
        frontSelector.setAnimations(EffectAnimationType.FrontSelector);

        backSelector.loops = true;
        backSelector.setAnimations(EffectAnimationType.BackSelector);

        frontSelectorTwo.loops = true;
        frontSelectorTwo.setAnimations(EffectAnimationType.FrontSelector2);

        backSelectorTwo.loops = true;
        backSelectorTwo.setAnimations(EffectAnimationType.BackSelector2);
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
        oldColor = color;

        frontSelectorTwo.spriteRenderer.color = color;
        backSelectorTwo.spriteRenderer.color = color;
    }

    public void setToTargetFoundSelector()
    {
        backSelector.spriteRenderer.color = Color.clear;
        backSelector.spriteRenderer.material = defaultSprite;
        
        frontSelector.spriteRenderer.color = Color.clear;
        frontSelector.spriteRenderer.material = defaultSprite;

        backSelectorTwo.spriteRenderer.color = oldColor;
        frontSelectorTwo.spriteRenderer.color = oldColor;
    }

    public void setToNoTargetFoundSelector()
    {
        backSelector.spriteRenderer.color = oldColor;
        backSelector.spriteRenderer.material = backSelectorOneMat;
        
        frontSelector.spriteRenderer.color = oldColor;
        frontSelector.spriteRenderer.material = frontSelectorOneMat;

        backSelectorTwo.spriteRenderer.color = Color.clear;
        frontSelectorTwo.spriteRenderer.color = Color.clear;
    }

	public void detectObservableObject()
    {
		if(Helpers.hasCollision(collider, LayerAndTagManager.observableLayerMask))
		{
            GameObject observedObj = Helpers.getCollision(collider, LayerAndTagManager.observableLayerMask).gameObject;

			if(observedObj.CompareTag(LayerAndTagManager.observableTag))
			{
				observedObj.GetComponent<ObservableObject>().markAsObserved();
				disableSelf(true);
			} 			
			
		} else
		{
			disableSelf(false);
		}
    }

}
