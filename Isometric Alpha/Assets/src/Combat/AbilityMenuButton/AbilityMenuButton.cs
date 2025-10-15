using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using UnityEngine.EventSystems;

public class AbilityMenuButton : MonoBehaviour, IPointerEnterHandler,
    IPointerExitHandler, IHoverIconSource
{
    private const int hexadupleBoxStartingPositionEnemySideRow = 2;
    private const int hexadupleBoxStartingPositionAllySideRow = 6;
    private const int hexadupleBoxStartingPositionCol = 2;

    private const float maximumAlpha = 1f;
    private const float greyedOutAlpha = .3f;
    private static Color greyedOutIconColor = new Color(maximumAlpha, maximumAlpha, maximumAlpha, greyedOutAlpha);
    private static Color greyedOutBackgroundColor = new Color(greyedOutAlpha, greyedOutAlpha, greyedOutAlpha, maximumAlpha);

    private static Color lockedBackgroundColor = new Color32(55, 55, 55, 255);

    private static Color costPayableColor = Color.green;
    private static Color costNotPayableColor = Color.red;
    private static Color cooldownColor = Color.yellow;

    public int index;
    public bool greyedOut = false;

    //[SerializeField]
    public AbilityMenuManager abilityMenuManager;

    public TextMeshProUGUI cooldownCostText;

    public TextMeshProUGUI redKnifeCostText;
    public TextMeshProUGUI blueShieldCostText;
    public TextMeshProUGUI yellowThornCostText;
    public TextMeshProUGUI greenLeafCostText;

    public Button abilityMenuButton;
    public Image lockedIcon;
    public Image abilityIcon;
    public Image iconBackground;
    public Image iconOutline;

    public bool disableHover;

    public CombatAction loadedCombatAction;
    public GameObject previewSelectorObject;

    // private void Awake()
    // {
    //     loadedCombatAction = null;
    // }

    public void handleCombatMouseClick()
    {
        abilityMenuManager.setCurrentlySelectedAbilityIndex(index);

        abilityMenuManager.selectAction();
    }

    private void enableButtonComponent()
    {
        if (CombatStateManager.inCombat && abilityMenuButton != null && !abilityMenuManager.displayOnly)
        {
            abilityMenuButton.enabled = true;
        }
    }

    public void disableButtonComponent()
    {
        if (abilityMenuButton != null)
        {
            abilityMenuButton.enabled = false;
        }
    }

    public void updateAppearance()
    {
        setDisplay(loadedCombatAction.getIconSprite(), Color.black);
    }

    public void setDisplay(Sprite abilityIconSprite, Color iconBackgroundColor)
    {
        abilityIcon.sprite = abilityIconSprite;
        iconBackground.color = iconBackgroundColor;
    }

    public void loadCombatAction(CombatAction action)
    {
        if (action == null)
        {
            return;
        }
        else
        {
            // Debug.LogError("action is " + action.getName());
        }

        loadedCombatAction = action;

        if (CombatStateManager.inCombat)
        {
            loadedCombatAction.setActor(CombatGrid.getCombatantAtCoords(SelectorManager.currentSelector.getCoords()));
        }
    }

    public bool isSelectable()
    {
        return enabled && loadedCombatAction != null &&
                loadedCombatAction.getCooldownRemaining() <= 0 &&
                (loadedCombatAction.usableWithoutItemsInInventory() ||
                    (loadedCombatAction.getSourceItem() != null &&
                        loadedCombatAction.getSourceItem().getQuantity() > 0));
    }


    public void selectButton()
    {
        iconOutline.color = Color.yellow;
        Helpers.updateSpritePosition(iconOutline.gameObject);
    }

    public void deselectButton()
    {
        iconOutline.color = Color.black;
        Helpers.updateSpritePosition(iconOutline.gameObject);
    }

    public virtual void enable()
    {
        enabled = true;
        abilityIcon.enabled = true;
        iconOutline.enabled = true;
        enableButtonComponent();
    }

    public void enable(Sprite abilityIconSprite, Color iconBackgroundColor, CombatAction action)
    {

        enable();
        setDisplay(abilityIconSprite, iconBackgroundColor);

        loadCombatAction(action);

        if (!CombatStateManager.isPlayerSurpriseRound() && loadedCombatAction != null && loadedCombatAction.getOnlyUsableDuringSurpriseRound())
        {
            greyOutAbility();
        }
    }

    public void populateWithoutEnabling(Sprite abilityIconSprite, Color iconBackgroundColor, CombatAction action)
    {
        setDisplay(abilityIconSprite, iconBackgroundColor);
        loadCombatAction(action);

        if (!CombatStateManager.isPlayerSurpriseRound() && loadedCombatAction != null && loadedCombatAction.getOnlyUsableDuringSurpriseRound())
        {
            greyOutAbility();
        }
    }


    public virtual void disable()
    {
        enabled = false;

        disableButtonComponent();

        abilityIcon.sprite = null;
        abilityIcon.enabled = false;

        iconOutline.enabled = false;
        iconBackground.color = Color.black;

        loadedCombatAction = null;
    }

    public void enableCombatActionSelectorPreview()
    {
        setCombatActionSelectorPreviewActive(true);
    }

    public void disableCombatActionSelectorPreview()
    {
        setCombatActionSelectorPreviewActive(false);
    }

    private void setCombatActionSelectorPreviewActive(bool active)
    {
        if (loadedCombatAction == null)
        {
            return;
        }

        if (active)
        {
            if (previewSelectorObject != null)
            {
                Destroy(previewSelectorObject);
            }

            Selector selectorClone = SelectorManager.getInstance().selectors[loadedCombatAction.getRangeIndex()].clone();
            selectorClone.deselectSelectorGameObject();

            selectorClone.SetActive(true);

            previewSelectorObject = selectorClone.getSelectorObject();

            if (loadedCombatAction.isSelfTargeting())
            {
                selectorClone.setToLocation(new GridCoords(SelectorManager.getInstance().selectors[0].currentRow,
                                                            SelectorManager.getInstance().selectors[0].currentCol));
            }
            else if (loadedCombatAction.targetsAllySection())
            {
                selectorClone.setToLocation(Range.getRangeAllyStartingPosition(loadedCombatAction.getRangeIndex()));
            }
            else
            {
                selectorClone.setToLocation(Range.getRangeEnemyStartingPosition(loadedCombatAction.getRangeIndex()));
            }
        }
        else
        {
            Destroy(previewSelectorObject);
        }
    }

    public void enableCombatActionSelector()
    {
        SelectorManager selectorManager = SelectorManager.getInstance();

        loadedCombatAction.setSelector(selectorManager.selectors[loadedCombatAction.getRangeIndex()].clone());

        SelectorManager.currentSelector = loadedCombatAction.getSelector();
        selectorManager.updateCurrentSelectorPosition();

        loadedCombatAction = setCombatActionSelectorStartingPosition(loadedCombatAction);
        loadedCombatAction.setActor(CombatGrid.getCombatantAtCoords(SelectorManager.getInstance().selectors[0].getCoords()));

        DamagePreviewManager.getInstance().setupDamagePreviews(loadedCombatAction);

        loadedCombatAction.getSelectorObject().SetActive(true);
    }

    private CombatAction setCombatActionSelectorStartingPosition(CombatAction action)
    {

        SelectorManager selectorManager = SelectorManager.getInstance();

        if (loadedCombatAction.isSelfTargeting())
        {
            loadedCombatAction.getSelector().setToLocation(new GridCoords(selectorManager.selectors[0].currentRow,
                                                                    selectorManager.selectors[0].currentCol));
            loadedCombatAction.getSelector().selfTargeting = true;
            return action;
        }

        loadedCombatAction.getSelector().selfTargeting = false;

        if (loadedCombatAction.targetsAllySection())
        {
            if (loadedCombatAction.getRangeIndex() == Range.hexadecupleBoxIndex)
            {
                loadedCombatAction.getSelector().setToLocation(Range.getRangeAllyStartingPosition(loadedCombatAction.getRangeIndex()));
            }
            else
            {
                loadedCombatAction.getSelector().setToLocation(new GridCoords(selectorManager.selectors[0].currentRow,
                                                                                selectorManager.selectors[0].currentCol));
            }

            return action;
        }
        else if (!loadedCombatAction.targetsAllySection() && loadedCombatAction.getRangeIndex() == Range.hexadecupleBoxIndex)
        {
            loadedCombatAction.getSelector().setToLocation(Range.getRangeEnemyStartingPosition(loadedCombatAction.getRangeIndex()));

            return action;
        }

        //loadedCombatAction.getSelector().setToStartLocation();

        Stats mandatoryTarget = CombatGrid.enemyHasMandatoryTarget();

        if (mandatoryTarget != null && !loadedCombatAction.getSelector().hasAtLeastOneMandatoryTarget() && loadedCombatAction.getSelector().singleTile)
        {
            GridCoords mandatoryTargetCoords = mandatoryTarget.position;

            loadedCombatAction.getSelector().setToClosestLegalLocation(new GridCoords(mandatoryTargetCoords.row,
                                                                    mandatoryTargetCoords.col));

            return action;
        }

        ArrayList allAliveEnemies = CombatGrid.getAllAliveEnemyCombatants();

        Stats closestTarget = (Stats)allAliveEnemies[allAliveEnemies.Count - 1];

        loadedCombatAction.getSelector().setToClosestLegalLocation(closestTarget.position);

        return action;
    }

    public void greyOutAbility()
    {
        enabled = false;
        greyedOut = true;
        iconOutline.enabled = false;

        abilityIcon.color = greyedOutIconColor;
        iconBackground.color = greyedOutBackgroundColor;

        Helpers.updateGameObjectPosition(abilityIcon.gameObject);
        Helpers.updateGameObjectPosition(iconBackground.gameObject);
    }

    public void resetGreyOutStatus()
    {
        enabled = true;
        greyedOut = false;
        iconOutline.enabled = true;

        abilityIcon.color = Color.white;
        iconBackground.color = Color.black;

        Helpers.updateGameObjectPosition(abilityIcon.gameObject);
        Helpers.updateGameObjectPosition(iconBackground.gameObject);
    }

    public bool isSelected()
    {
        return abilityMenuManager.getCurrentlySelectedAbilityMenuButton() == this;
    }

    public bool casterCanPayActionCost()
    {
        return loadedCombatAction.canPayActionCost(abilityMenuManager.getActionArraySource());
    }

    public void updateCooldownCostText()
    {
        if (loadedCombatAction != null)
        {
            if (loadedCombatAction.getCooldownRemaining() == 0)
            {
                if (isSelected() && !loadedCombatAction.getActionCostTypes().Contains(ActionCostType.None))
                {
                    setAllActionCosts(loadedCombatAction.getActionCostTypes(), loadedCombatAction.getActionCosts());

                    if (casterCanPayActionCost())
                    {
                        cooldownCostText.color = costPayableColor;
                    }
                    else
                    {
                        cooldownCostText.color = costNotPayableColor;
                    }
                }
                else
                {
                    cooldownCostText.text = "";

                    redKnifeCostText.text = "";
                    blueShieldCostText.text = "";
                    yellowThornCostText.text = "";
                    greenLeafCostText.text = "";
                }
            }
            else
            {
                cooldownCostText.color = cooldownColor;
                cooldownCostText.text = "" + loadedCombatAction.getCooldownRemaining();

                redKnifeCostText.text = "";
                blueShieldCostText.text = "";
                yellowThornCostText.text = "";
                greenLeafCostText.text = "";
            }
        }
    }

    private void setAllActionCosts(ActionCostType[] costTypes, int[] actionCosts)
    {
        for (int index = 0; index < costTypes.Length && index < actionCosts.Length; index++)
        {
            setActionCostText(costTypes[index], actionCosts[index]);
        }
    }

    private void setActionCostText(ActionCostType costType, int actionCost)
    {
        switch (costType)
        {
            case ActionCostType.RedKnife:
                redKnifeCostText.text = "" + actionCost;
                break;
            case ActionCostType.BlueShield:
                blueShieldCostText.text = "" + actionCost;
                break;
            case ActionCostType.YellowThorn:
                yellowThornCostText.text = "" + actionCost;
                break;
            case ActionCostType.GreenLeaf:
                greenLeafCostText.text = "" + actionCost;
                break;

            default:
                cooldownCostText.text = "" + actionCost;
                return;
        }
    }

    public void setToLockedStatus()
    {
        lockedIcon.gameObject.SetActive(true);

        abilityIcon.gameObject.SetActive(false);

        iconOutline.color = Color.black;
        iconBackground.color = lockedBackgroundColor;
        abilityMenuButton.enabled = false;
    }

    public void setToUnlockedStatus()
    {
        lockedIcon.gameObject.SetActive(false);

        abilityIcon.gameObject.SetActive(true);

        iconOutline.color = Color.black;
        iconBackground.color = Color.black;

        if (!abilityMenuManager.displayOnly)
        {
            abilityMenuButton.enabled = true;
        }
    }

    public DescriptionPanelSlot getDescriptionPanelSlot()
    {
        return AbilityMenuManager.getInstance().descriptionPanelSlot;
    }

    public void setAbilityMenuManager(AbilityMenuManager abilityMenuManager)
    {
        this.abilityMenuManager = abilityMenuManager;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (disableHover || loadedCombatAction == null || !enabled)
        {
            return;
        }

        if (CombatStateManager.inCombat)
        {
            getDescriptionPanelSlot().setTempDescribable(loadedCombatAction);
        }
        else
        {
            MouseHoverManager.startCoroutine(this,MouseHoverManager.waitToHandleDescriptionPanel(this, MouseHoverManager.shouldSpawnHoverIcon));
        }

    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        MouseHoverManager.OnHoverPanelCreation.Invoke();

        if (disableHover || loadedCombatAction == null || !enabled)
        {
            return;
        }

        if (CombatStateManager.inCombat)
        {
            getDescriptionPanelSlot().revertToPrimaryDescribable();
        }
        else
        {
            MouseHoverManager.startCoroutine(this, MouseHoverManager.waitToHandleDescriptionPanel(this, MouseHoverManager.shouldDestroyHoverIcon));
        }
    }

    public void spawnHoverIcon()
    {
        MouseHoverManager.spawnHoverIcon(this, transform);
    }

    public void destroyHoverIcon()
    {
        MouseHoverManager.destroyHoverIcon();
    }

    public GameObject getDescriptionPanelType()
    {
        return Resources.Load<GameObject>(PrefabNames.hoverIconCombatActionDescriptionPanel);
    }
    public IDescribable getObjectBeingDescribed()
    {
        return loadedCombatAction;
    }
}

/*

	private CombatAction setCombatActionSelectorStartingPosition(CombatAction action)
	{
		
		SelectorManager selectorManager = SelectorManager.getInstance();
		
		if(loadedCombatAction.isSelfTargeting())
		{
			loadedCombatAction.getSelector().setToLocation(new GridCoords(selectorManager.selectors[0].currentRow,
																	selectorManager.selectors[0].currentCol));
			loadedCombatAction.getSelector().selfTargeting = true;
			return action;
		}

		loadedCombatAction.getSelector().selfTargeting = false;

		if(loadedCombatAction.targetsAllySection())
		{
			if (loadedCombatAction.getRangeIndex() == Range.hexadecupleBoxIndex)
			{
				loadedCombatAction.getSelector().setToLocation(Range.getRangeAllyStartingPosition(loadedCombatAction.getRangeIndex()));
			}
			else
			{
				loadedCombatAction.getSelector().setToLocation(new GridCoords(selectorManager.selectors[0].currentRow,
																				selectorManager.selectors[0].currentCol));
			}

			return action;
		} else if(!loadedCombatAction.targetsAllySection() && loadedCombatAction.getSelector().singleTile)
		{
			if (loadedCombatAction.getRangeIndex() == Range.hexadecupleBoxIndex)
			{
				loadedCombatAction.getSelector().setToLocation(Range.getRangeEnemyStartingPosition(loadedCombatAction.getRangeIndex()));
			}
			else
			{
				loadedCombatAction.getSelector().setToLocation(new GridCoords(0, 2));

				selectorManager.isMoving = true;
				selectorManager.moveCurrentSelectorToNextSingleTileTarget();
			}

			return action;
		}
		
		loadedCombatAction.getSelector().setToStartLocation();
		
		Stats mandatoryTarget = CombatGrid.enemyHasMandatoryTarget();
		
		if(mandatoryTarget != null && !loadedCombatAction.getSelector().hasAtLeastOneMandatoryTarget() && loadedCombatAction.getSelector().singleTile)
		{
			GridCoords mandatoryTargetCoords = mandatoryTarget.position;
			
			loadedCombatAction.getSelector().setToLocation(new GridCoords(mandatoryTargetCoords.row,
																	mandatoryTargetCoords.col));
																	
			return action;														
		}
		
		return action;
	}
	*/