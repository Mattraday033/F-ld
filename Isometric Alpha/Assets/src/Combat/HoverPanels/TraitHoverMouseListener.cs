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
    public ArrayList relatedDescriptionPanelBuilders;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Transform descriptionPanelParent = HoverPanel.getTraitDescriptionPanelParent();

        descriptionPanelParent.gameObject.SetActive(true);

        ArrayList relatedDescribables = descriptionPanel.getObjectBeingDescribed().getRelatedDescribables();

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
        destroyAllDescriptionPanels();

        CurrentActionHoverPanelManager.showPanels();
    }

    public void destroyAllDescriptionPanels()
    {
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

        relatedDescriptionPanelBuilders = new ArrayList();
        HoverPanel.getTraitDescriptionPanelParent().gameObject.SetActive(false);
    }

    private ArrayList setUpRelatedDescriptionPanelBuilders(ArrayList listOfDescribables, Transform parent)
    {
        ArrayList listOfDescriptionPanelBuilders = new ArrayList();

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

}
