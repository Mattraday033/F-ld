using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIndicator : MonoBehaviour
{
	public Collider2D collider;

    public PolygonCollider2D mouseHoverCollider;

    public Vector2Int coords;

    private Color color;

    public SpriteRenderer frontSelector;
    public SpriteRenderer backSelector;

    public EffectAnimationManager frontSelectorTwo;
    public EffectAnimationManager backSelectorTwo;

    public GameObject tileMapGameObject;

    public bool currentCursor = false;
    public bool collidedWithSkillTarget = false;
    public bool allowHover = false;

    private SkillIndicatorState stateBeforeHover;

    #region Awake/OnEnable/OnDisable

    private void Awake()
    {        
        frontSelectorTwo.loops = true;
        frontSelectorTwo.setAnimations(EffectAnimationType.FrontSelector2);

        backSelectorTwo.loops = true;
        backSelectorTwo.setAnimations(EffectAnimationType.BackSelector2);

        mouseHoverCollider.enabled = PlayerOOCStateManager.currentActivity != OOCActivity.inTutorialSequence;
    }

    private void OnEnable()
    {
        EnemyMovement.ToggleHoverColliders.AddListener(toggleTileMapCollider);

        IntimidateManager.GetAllIntimidateTargets.AddListener(declareSkillTarget);
    }

    private void OnDisable()
    {
        EnemyMovement.ToggleHoverColliders.RemoveListener(toggleTileMapCollider);

        IntimidateManager.GetAllIntimidateTargets.RemoveListener(declareSkillTarget);
    }
    #endregion

    public bool hadPreviousCollision()
    {
        return collidedWithSkillTarget;
    }

    private void toggleTileMapCollider(bool active)
    {
        tileMapGameObject.SetActive(active);
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
        this.color = color;
    }

    public Color getColor()
    {
        return color;
    }

    private void setColorWithTransparency(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.color = new Color(color.r, color.g, color.b, ColorList.hoverSelectorAlpha);
    }

    public void setToTargetFoundSelector()
    {
        backSelector.color = Color.clear;
        frontSelector.color = Color.clear;

        backSelectorTwo.spriteRenderer.color = color;
        frontSelectorTwo.spriteRenderer.color = color;
    }

    public void setToNoTargetFoundSelector()
    {
        setColorWithTransparency(backSelector);
        setColorWithTransparency(frontSelector);

        backSelectorTwo.spriteRenderer.color = Color.clear;
        frontSelectorTwo.spriteRenderer.color = Color.clear;
    }

    public void OnMouseEnter()
    {
        if(allowHover && !currentCursor)
        {
            stateBeforeHover = new SkillIndicatorState(this);

            setColor(Color.green);
            setToTargetFoundSelector();
            setColorWithTransparency(backSelectorTwo.spriteRenderer);
            setColorWithTransparency(frontSelectorTwo.spriteRenderer);
        }
    }

    public void OnMouseExit()
    {
        if(stateBeforeHover != null)
        {
            stateBeforeHover.restore(this);
            stateBeforeHover = null;
        }
    }

    public void OnMouseUp()
    {
        if(allowHover)
        {
            CunningManager.setCurrentSelector(coords);
            stateBeforeHover = null;
        }
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

    private void declareSkillTarget(IntBus bus)
    {   
        if(collidedWithSkillTarget)
        {
            bus.amount++;
        }
    }

}
