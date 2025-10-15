using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnusedAbilityPanel : GridRow
{
	public override void displayDescribable() 
	{
		FullEditAbilityWheelPopUpWindow.getFullEditInstance().revealDescriptionPanelSet(descriptionPanel.getObjectBeingDescribed() as CombatAction); 
	}
	
	public void instantiateDragAndDropIcon()
	{
		CombatAction actionBeingDescribed = (CombatAction) descriptionPanel.getObjectBeingDescribed();
		
		GameObject dragAndDropIcon = Instantiate(Resources.Load<GameObject>(PrefabNames.dragAndDropCombatActionIcon), DragAndDropParentDeclarer.dragAndDropParent);
		
		AbilityMenuButton dragAndDropIconAbilityMenuButton = dragAndDropIcon.GetComponent<AbilityMenuButton>();
		
		dragAndDropIconAbilityMenuButton.populateWithoutEnabling(Helpers.loadSpriteFromResources(actionBeingDescribed.getIconName()), Color.clear, actionBeingDescribed);
		
		dragAndDropIcon.SetActive(true);
	}
}
