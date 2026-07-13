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
    public SlotIconHover parentHover;

    public int index;
    public bool greyedOut = false;
    public static bool hoveringOverAbilityMenuButton = false;

    [SerializeField]
    public AbilityMenuManager abilityMenuManager;

    public GameObject cooldownParent;
    public TextMeshProUGUI cooldownCostText;

    public Transform costParent;

    public Button abilityMenuButton;
    public Image lockedIcon;
    public Image abilityIcon;
    public Image iconBackground;
    public Image iconOutline;

    public bool disableHover;

    public CombatAction loadedCombatAction;
    public GameObject previewSelectorObject;

    private void Awake()
    {
        if(CombatStateManager.inCombat)
        {
            CombatStateManager.OnActivityChangeToTutorial.AddListener(disableButtonComponent);
            CombatStateManager.OnActivityChangeFromTutorial.AddListener(enableButtonComponent);
        }

        InspectNode.OnInspect.AddListener(preventParentHoverOnInspect);
    }

    private void OnDestroy()
    {
        CombatStateManager.OnActivityChangeToTutorial.RemoveListener(disableButtonComponent);
        CombatStateManager.OnActivityChangeFromTutorial.RemoveListener(enableButtonComponent);

        InspectNode.OnInspect.RemoveListener(preventParentHoverOnInspect);
    }

    public void handleCombatMouseClick()
    {
        if(CombatStateManager.currentActivity == CurrentActivity.Tutorial)
        {
            AbilityWheelChooseAbility chooseAbilityScript = new AbilityWheelChooseAbility();

            chooseAbilityScript.runScript();

            TutorialSequence.currentTutorialSequence.moveToNextStep();
            return;
        }

        abilityMenuManager.setCurrentlySelectedAbilityIndex(index);

        abilityMenuManager.selectAction();
        OnPointerExit(null);
    }

    private void enableButtonComponent()
    {
        if (CombatStateManager.inCombat && abilityMenuButton != null && !abilityMenuManager.displayOnly && loadedCombatAction != null && !loadedCombatAction.unactivatable())
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
        if(loadedCombatAction == null)
        {
            return;
        }

        setDisplay(loadedCombatAction.getIconSprite(), ColorList.grey25);
    }

    public void setDisplay(Sprite abilityIconSprite, Color iconBackgroundColor)
    {
        DescriptionPanel.setImage(abilityIcon, abilityIconSprite);
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


    public void selectButton(bool playSFX = true)
    {
        iconOutline.color = Color.yellow;
        Helpers.updateSpritePosition(iconOutline.gameObject);

        if(playSFX)
        {
            AudioManager.playChangeSelectedActionFX();
        }
    }

    public void deselectButton()
    {
        iconOutline.color = ColorList.grey25;
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

        if (!CombatStateManager.isPlayerSurpriseRound() && loadedCombatAction != null && loadedCombatAction.onlyUsableDuringSurpriseRound)
        {
            greyOutAbility();
        }
    }

    public void populateWithoutEnabling(Sprite abilityIconSprite, Color iconBackgroundColor, CombatAction action)
    {
        setDisplay(abilityIconSprite, iconBackgroundColor);
        loadCombatAction(action);

        if (!CombatStateManager.isPlayerSurpriseRound() && loadedCombatAction != null && loadedCombatAction.onlyUsableDuringSurpriseRound)
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
        iconBackground.color = ColorList.grey25;

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

            Selector selectorClone = SelectorList.getByName(loadedCombatAction.getRangeName());

            // selectorClone.SetActive(true);

            // previewSelectorObject = selectorClone.getSelectorObject();

            if (loadedCombatAction.isSelfTargeting())
            {
                selectorClone.setToLocation(SelectorList.playerCursor.getCoords());
            }
            else if (loadedCombatAction.targetsAllySection())
            {
                selectorClone.setToLocation(Range.getRangeAllyStartingPosition(loadedCombatAction.getRangeName()));
            }
            else
            {
                selectorClone.setToStartLocation();
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

        loadedCombatAction.setSelector(SelectorList.getByName(loadedCombatAction.getRangeName()));

        SelectorManager.currentSelector = loadedCombatAction.getSelector();

        loadedCombatAction = setCombatActionSelectorStartingPosition(loadedCombatAction);
        loadedCombatAction.setActor(CombatGrid.getCombatantAtCoords(SelectorList.playerCursor.getCoords()));

        getDescriptionPanelSlot().revertToPrimaryDescribable();
        getDescriptionPanelSlot().setPrimaryDescribable(loadedCombatAction);

        SelectorManager.updateAllDamagePreviews();

        // loadedCombatAction.getSelectorObject().SetActive(true);

        SelectorManager.displayCurrentHoverUI();
    }

    private CombatAction setCombatActionSelectorStartingPosition(CombatAction action)
    {

        SelectorManager selectorManager = SelectorManager.getInstance();

        if (loadedCombatAction.isSelfTargeting())
        {
            loadedCombatAction.getSelector().setToLocation(SelectorList.playerCursor.getCoords());
            loadedCombatAction.getSelector().selfTargeting = true;
            return action;
        }

        loadedCombatAction.getSelector().selfTargeting = false;

        if (loadedCombatAction.targetsAllySection())
        {
            if (loadedCombatAction.getRangeName() == SelectorList.boxThreeName)
            {
                loadedCombatAction.getSelector().setToLocation(Range.getRangeAllyStartingPosition(loadedCombatAction.getRangeName()));
            }
            else
            {
                loadedCombatAction.getSelector().setToLocation(SelectorList.playerCursor.getCoords());
            }

            return action;
        }
        else if (!loadedCombatAction.targetsAllySection() && loadedCombatAction.getRangeName() == SelectorList.boxThreeName)
        {
            loadedCombatAction.getSelector().setToStartLocation();

            return action;
        }

        //loadedCombatAction.getSelector().setToStartLocation();

        Stats mandatoryTarget = CombatGrid.enemyHasMandatoryTarget();

        if (mandatoryTarget != null && !loadedCombatAction.getSelector().hasAtLeastOneMandatoryTarget() && loadedCombatAction.getSelector().singleTile())
        {
            GridCoords mandatoryTargetCoords = mandatoryTarget.positions.Count > 0 ? mandatoryTarget.positions[0] : GridCoords.getDefaultCoords();

            loadedCombatAction.getSelector().setToClosestLegalLocation(new GridCoords(mandatoryTargetCoords.row,
                                                                    mandatoryTargetCoords.col));

            return action;
        }

        List<Stats> allAliveEnemies = CombatGrid.getAllAliveEnemyCombatants();

        Stats closestTarget = (Stats)allAliveEnemies[allAliveEnemies.Count - 1];

        if (closestTarget.positions.Count > 0)
        {
            loadedCombatAction.getSelector().setToClosestLegalLocation(closestTarget.positions[0]);
        }

        return action;
    }

    public void greyOutAbility()
    {
        enabled = false;
        greyedOut = true;
        iconOutline.enabled = false;

        abilityIcon.color = ColorList.greyedOutIconColor;
        iconBackground.color = ColorList.greyedOutBackgroundColor;

        disableButtonComponent();

        Helpers.updateGameObjectPosition(abilityIcon.gameObject);
        Helpers.updateGameObjectPosition(iconBackground.gameObject);
    }

    public void resetGreyOutStatus()
    {
        enabled = true;
        greyedOut = false;
        iconOutline.enabled = true;

        abilityIcon.color = Color.white;
        iconBackground.color = ColorList.grey25;

        enableButtonComponent();

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
                cooldownParent.SetActive(false);

                if (isSelected() && !loadedCombatAction.getActionCostTypes().Contains(ActionCostType.None))
                {
                    setAllActionCosts(loadedCombatAction.getActionCostTypes(), loadedCombatAction.getActionCosts());
                }
                else
                {
                    destroyAllCostIcons();
                }
            }
            else
            {
                cooldownParent.SetActive(true);
                cooldownCostText.text = "" + loadedCombatAction.getCooldownRemaining();

                destroyAllCostIcons();
            }
        }
    }

    private void destroyAllCostIcons()
    {
        foreach(Transform child in costParent)
        {
            Destroy(child.gameObject);
        }

        costParent.gameObject.SetActive(false);
    }

    private void setAllActionCosts(ActionCostType[] costTypes, int[] actionCosts)
    {
        destroyAllCostIcons();

        for (int index = 0; index < costTypes.Length && index < actionCosts.Length; index++)
        {
            setActionCostText(costTypes[index], actionCosts[index]);
        }
    }

    private void setActionCostText(ActionCostType costType, int actionCost)
    {
        costParent.gameObject.SetActive(true);

        CostIcon costIcon = Instantiate(Resources.Load<GameObject>(PrefabNames.costIcon), costParent).GetComponent<CostIcon>();

        costIcon.setCostType(costType);
        costIcon.setCostText(actionCost.ToString());

        // switch (costType)
        // {
        //     case ActionCostType.RedKnife:
        //         redKnifeCostText.text = "" + actionCost;
        //         break;
        //     case ActionCostType.BlueShield:
        //         blueShieldCostText.text = "" + actionCost;
        //         break;
        //     case ActionCostType.YellowThorn:
        //         yellowThornCostText.text = "" + actionCost;
        //         break;
        //     case ActionCostType.GreenLeaf:
        //         greenLeafCostText.text = "" + actionCost;
        //         break;

        //     default:
        //         cooldownCostText.text = "" + actionCost;
        //         return;
        // }
    }

    public void setToLockedStatus()
    {
        lockedIcon.gameObject.SetActive(true);

        abilityIcon.gameObject.SetActive(false);

        abilityMenuButton.enabled = false;
    }

    public void setToUnlockedStatus()
    {
        lockedIcon.gameObject.SetActive(false);

        abilityIcon.gameObject.SetActive(true);

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

        hoveringOverAbilityMenuButton = true;

        if (CombatStateManager.inCombat)
        {
            getDescriptionPanelSlot().setTempDescribable(loadedCombatAction);

            if (!loadedCombatAction.getActionCostTypes().Contains(ActionCostType.None))
            {
                setAllActionCosts(loadedCombatAction.getActionCostTypes(), loadedCombatAction.getActionCosts());
            }

            CombatHoverTileManager.GetHoverSelector.RemoveAllListeners();
            SelectorManager.declareSelectors();
        }
        else
        {
            MouseHoverManager.startCoroutine(this,MouseHoverManager.waitToHandleDescriptionPanel(this, MouseHoverManager.shouldSpawnHoverIcon));
        }

    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        // MouseHoverManager.OnHoverPanelCreation.Invoke();

        if (disableHover || loadedCombatAction == null || !enabled)
        {
            return;
        }

        if (CombatStateManager.inCombat)
        {
            getDescriptionPanelSlot().revertToPrimaryDescribable();
            destroyAllCostIcons();
            SelectorManager.declareSelectors();
        }
        else
        {
            MouseHoverManager.startCoroutine(this, MouseHoverManager.waitToHandleDescriptionPanel(this, MouseHoverManager.shouldDestroyHoverIcon));

            if(parentHover != null && hoveringOverAbilityMenuButton)
            {
                parentHover.OnPointerEnter(eventData);
            }
        }

        hoveringOverAbilityMenuButton = false;
    }

    public void preventParentHoverOnInspect()
    {
        hoveringOverAbilityMenuButton = false;
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

    private void OnDisable()
    {
        hoveringOverAbilityMenuButton = false;

        if(CombatStateManager.inCombat)
        {
            DescriptionPanelSlot slot = getDescriptionPanelSlot();

            if(slot != null && slot.hasTempDescribable())
            {
                slot.revertToPrimaryDescribable();
            }
        }
    }
}