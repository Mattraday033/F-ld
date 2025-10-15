using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HoverPanel : PopUpWindow, IEscapable
{
	public Transform descriptionPanelParent;
	public ScrollableUIElement traitDisplay;
	public Transform traitDescriptionPanelParent;
	
	private static HoverPanel instance;
	
	public static HoverPanel getInstance()
	{
		return instance;
	}
	
	private void Awake()
	{
		if(instance != null && !(instance is null) && !(instance.gameObject is null))
		{
			Destroy(instance.gameObject);
		}
	
		instance = this;
	}
	
	public void populate(Stats combatant)
	{
		descriptionPanelSlot.setPrimaryDescribable(combatant);

		ArrayList traitList = new ArrayList();
		traitList.AddRange(combatant.traits);

		traitDisplay.populatePanels(traitList);
		/*
			deactivate();
			combatant.describeStats(this);
			
			traitPanelManager.populateTraitPanels(combatant.getTraits());
			traitPanelManager.gameObject.SetActive(true);
		*/
	}
	
	public static Transform getTraitDescriptionPanelParent()
	{
		if(getInstance() == null)
		{
			return null;
		}

		return getInstance().traitDescriptionPanelParent;
	}	
}
