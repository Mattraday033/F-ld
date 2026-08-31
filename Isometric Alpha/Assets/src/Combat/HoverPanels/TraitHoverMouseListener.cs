using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TraitHoverMouseListener : GridRow, IPointerEnterHandler, IPointerExitHandler
{
    private const int maxPanels = 4;

	public DescriptionPanelBuilder traitHoverDescriptionPanel;
    public List<DescriptionPanelBuilder> relatedDescriptionPanelBuilders;

    public BoxCollider2D hoverCollider;

    private void disableHoverCollider()
    {
        if (hoverCollider != null)
        {
            hoverCollider.enabled = false;
        }
    }

    private void enableHoverCollider()
    {
        if (hoverCollider != null)
        {
            hoverCollider.enabled = true;
        }
    }

    private void OnMouseEnter()
    {
        OnPointerEnter(null);
    }

    private void OnMouseExit()
    {
        OnPointerExit(null);
    }


    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(eventData != null && eventData.used)
        {
            return;
        }

        Transform descriptionPanelParent = HoverPanel.getTraitDescriptionPanelParent();

        descriptionPanelParent.gameObject.SetActive(true);

        List<IDescribable> relatedDescribables = descriptionPanel.getObjectBeingDescribed().getRelatedDescribables();

        if (relatedDescribables.Count < maxPanels)
        {
            traitHoverDescriptionPanel = setUpDescriptionPanelBuilder(descriptionPanel.getObjectBeingDescribed() as IDescribableInBlocks, descriptionPanelParent);
        }

        relatedDescriptionPanelBuilders = setUpRelatedDescriptionPanelBuilders(relatedDescribables, descriptionPanelParent);

        if (relatedDescribables.Count >= 3)
        {
            CurrentActionHoverPanelManager.hidePanels();
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if(InspectNode.inspecting || (eventData != null && eventData.used))
        {
            return;
        }

        destroyAllDescriptionPanels();

        CurrentActionHoverPanelManager.showPanels();
    }

    public void destroyAllDescriptionPanels()
    {
        if(InspectNode.inspecting)
        {
            return;
        }

        if (traitHoverDescriptionPanel != null)
        {
            Destroy(traitHoverDescriptionPanel.gameObject);
        }

        foreach (DescriptionPanelBuilder panel in relatedDescriptionPanelBuilders)
        {
            if (panel == null)
            {
                continue;
            }

            Destroy(panel.gameObject);
        }

        relatedDescriptionPanelBuilders = new List<DescriptionPanelBuilder>();
        HoverPanel.getTraitDescriptionPanelParent().gameObject.SetActive(false);
    }

    private List<DescriptionPanelBuilder> setUpRelatedDescriptionPanelBuilders(List<IDescribable> listOfDescribables, Transform parent)
    {
        List<DescriptionPanelBuilder> listOfDescriptionPanelBuilders = new List<DescriptionPanelBuilder>();

        foreach (IDescribable describable in listOfDescribables)
        {
            DescriptionPanelBuilder blockBuilder = setUpDescriptionPanelBuilder(describable as IDescribableInBlocks, parent);

            listOfDescriptionPanelBuilders.Add(blockBuilder);

            if (listOfDescriptionPanelBuilders.Count >= maxPanels)
            {
                break;
            }
        }

        return listOfDescriptionPanelBuilders;
    }

    private DescriptionPanelBuilder setUpDescriptionPanelBuilder(IDescribableInBlocks describable, Transform parent)
    {
        DescriptionPanelBuilder descriptionPanelBuilder = Instantiate(Resources.Load<GameObject>(PrefabNames.combatActionHoverDescriptionPanelBuilder), parent).GetComponent<DescriptionPanelBuilder>();

        descriptionPanelBuilder.buildDescriptionPanel(describable);

        return descriptionPanelBuilder;
    }

    private void OnEnable()
    {
        MouseHoverManager.OnHoverPanelCreation.AddListener(destroyAllDescriptionPanels);

        CombatStateManager.OnActivityChangeToInEscapeMenu.AddListener(disableHoverCollider);
        CombatStateManager.OnActivityChangeFromInEscapeMenu.AddListener(enableHoverCollider);
        // InspectNode.OnInspect.AddListener(disableDestroyHoverOnPanelCreation);

        if(CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        {
            disableHoverCollider();
        }
    }

    private void OnDisable()
    {
        MouseHoverManager.OnHoverPanelCreation.RemoveListener(destroyAllDescriptionPanels);

        CombatStateManager.OnActivityChangeToInEscapeMenu.RemoveListener(disableHoverCollider);
        CombatStateManager.OnActivityChangeFromInEscapeMenu.RemoveListener(enableHoverCollider);
        // InspectNode.OnInspect.AddListener(disableDestroyHoverOnPanelCreation);
    }

}
