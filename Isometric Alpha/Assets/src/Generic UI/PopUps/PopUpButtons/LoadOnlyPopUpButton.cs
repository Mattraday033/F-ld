using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadOnlyPopUpButton : BinaryPanelPopUpButton
{
	public LoadOnlyPopUpButton():
	base()
	{
		
	}

    private void Awake()
    {
        GridRow.OnDescribableToDisplay.AddListener(destroySelf);
        SaveHandler.OnSaveCreated.AddListener(destroySelf);
    }

    private void OnDestroy()
    {
        GridRow.OnDescribableToDisplay.RemoveListener(destroySelf);
        SaveHandler.OnSaveCreated.RemoveListener(destroySelf);
    }

    private void destroySelf(IDescribable describable)
    {
        if(describable as SaveBlueprint != null)
        {
            Destroy(gameObject);
        }
    }

}
