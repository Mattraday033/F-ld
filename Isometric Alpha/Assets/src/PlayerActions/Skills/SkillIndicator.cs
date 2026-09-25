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

    public void Awake()
    {
        frontSelectorTwo.Awake();
        frontSelectorTwo.loops = true;
        frontSelectorTwo.animationData = AnimationDataList.frontSelector2;
        frontSelectorTwo.startAnimation();

        backSelectorTwo.Awake();
        backSelectorTwo.loops = true;
        backSelectorTwo.animationData = AnimationDataList.backSelector2;
        backSelectorTwo.startAnimation();

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
        GameObjectUtil.updateGameObjectPosition(tileMapGameObject);
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

    private void setSelectorTwoColor(EffectAnimationManager selectorTwo, Color newColor)
    {
        if(selectorTwo.rendererList == null)
        {
            return;
        }

        foreach(SpriteLayer layer in EnumUtil.SpriteLayers)
        {
            selectorTwo.rendererList[layer].color = newColor;
        }
    }

    private Color getColorWithTransparency()
    {
        return new Color(color.r, color.g, color.b, ColorList.hoverSelectorAlpha);
    }

    public void setToTargetFoundSelector()
    {
        backSelector.color = Color.clear;
        frontSelector.color = Color.clear;

        setSelectorTwoColor(backSelectorTwo, color);
        setSelectorTwoColor(frontSelectorTwo, color);
    }

    public void setToNoTargetFoundSelector()
    {
        setColorWithTransparency(backSelector);
        setColorWithTransparency(frontSelector);

        setSelectorTwoColor(backSelectorTwo, Color.clear);
        setSelectorTwoColor(frontSelectorTwo, Color.clear);
    }

    public void OnMouseEnter()
    {
        if(allowHover && !currentCursor)
        {
            stateBeforeHover = new SkillIndicatorState(this);

            setColor(Color.green);
            setToTargetFoundSelector();
            setSelectorTwoColor(backSelectorTwo, getColorWithTransparency());
            setSelectorTwoColor(frontSelectorTwo, getColorWithTransparency());
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
