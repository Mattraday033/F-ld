using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum DescriptionPanelParent {HoverPanel = 1, CombatUI = 2};

public class NestedDescriptionPanelMouseListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public DescriptionPanelParent parentType;
	public DescriptionPanel inputDescriptionPanel; //descriptionPanel that when moused over needs to be described in more detail
	public ArrayList relatedDescriptionPanels;
	private DescriptionPanelBuilder outputDescriptionPanel;
	
	public Transform getDescriptionPanelParent()
	{
		switch(parentType)
		{
			case DescriptionPanelParent.HoverPanel:
			
				return HoverPanel.getTraitDescriptionPanelParent();
				
			case DescriptionPanelParent.CombatUI:
			
				return CombatUI.getDescriptionPanelParent();
				
			default:
				throw new IOException("Unknown DescriptionPanelParent: " + parentType.ToString());
		}
	}
	
	public void OnPointerEnter(PointerEventData eventData)
    {
        if (CombatStateManager.currentActivity == CurrentActivity.Waiting ||
			CombatStateManager.currentActivity == CurrentActivity.Retreating)
        {
            return;
        }

        instantiateAllDescriptionPanels();
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        if (CombatStateManager.currentActivity == CurrentActivity.Waiting ||
			CombatStateManager.currentActivity == CurrentActivity.Retreating)
        {
            return;
        }

        destroyAllDescriptionPanels();
    }
 
	public void instantiateAllDescriptionPanels()
	{
		Transform descriptionPanelParent = getDescriptionPanelParent(); 
		
		descriptionPanelParent.gameObject.SetActive(true);
		
		IDescribable describable = inputDescriptionPanel.getObjectBeingDescribed();
		
		outputDescriptionPanel = setUpDescriptionPanel(describable, descriptionPanelParent);

		relatedDescriptionPanels = setUpDescriptionPanels(describable.getRelatedDescribables(), descriptionPanelParent);
	}
 
	public void destroyAllDescriptionPanels()
	{
		if (outputDescriptionPanel != null)
		{
			Destroy(outputDescriptionPanel.gameObject);
		}

		if (relatedDescriptionPanels == null)
		{
			return;
		}

		foreach (DescriptionPanelBuilder builder in relatedDescriptionPanels)
		{
			if (builder == null)
			{
				continue;
			}

			Destroy(builder.gameObject);
		}
		
		relatedDescriptionPanels = new ArrayList();
		getDescriptionPanelParent().gameObject.SetActive(false);
	}
 
	private ArrayList setUpDescriptionPanels(ArrayList listOfDescribables, Transform parent)
	{
		ArrayList listOfDescriptionPanels = new ArrayList();
		
		foreach(IDescribable describable in listOfDescribables)
		{
			DescriptionPanelBuilder panel = setUpDescriptionPanel(describable, parent);
			
			listOfDescriptionPanels.Add(panel);
		}
	
		return listOfDescriptionPanels;
	}
 
	private DescriptionPanelBuilder setUpDescriptionPanel(IDescribable describable, Transform parent)
	{
		DescriptionPanelBuilder descriptionPanelBuilder = Instantiate(Resources.Load<GameObject>(PrefabNames.combatActionHoverDescriptionPanelBuilder), parent).GetComponent<DescriptionPanelBuilder>();
		
		descriptionPanelBuilder.buildDescriptionPanel(describable as IDescribableInBlocks);
		
		return descriptionPanelBuilder;
	}
 

}
