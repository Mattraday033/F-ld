using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceGridRow : GridRow
{
	public override void setToIneligible()
	{
		//base.setToIneligible();
		
		foreach(TextMeshProUGUI buttonText in buttonTexts)
		{
			buttonText.color = ColorList.colorIndicatingChosenBefore;
		}
		
	}
}
