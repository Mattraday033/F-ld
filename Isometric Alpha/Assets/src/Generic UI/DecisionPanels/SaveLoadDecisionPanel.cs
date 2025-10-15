using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadDecisionPanel : MonoBehaviour, IDecisionPanel
{
	public SaveBlueprint saveBlueprint;

	public int collectionIndex;
	public ScrollableUIElement grid;

	//IDecisionPanel methods
	public GameObject getGameObject()
	{
		return gameObject;
	}

	public void setObjectToBeDecidedOn(IDescribable describable)
	{
		this.saveBlueprint = (SaveBlueprint)describable;
	}

	public void setScrollableUIElement(ScrollableUIElement grid)
	{
		this.grid = grid;
	}

	public void setCollectionIndex(int collectionIndex)
	{
		this.collectionIndex = collectionIndex;
	}

	public void updateEnabledButtons()
	{
		//empty on purpose
	}

	public string getDescribableRowKey()
	{
		return saveBlueprint.getName();
	}
}
