using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceGridRow : GridRow
{
	private static Color colorIndicatingChosenBefore = new Color(0.5f, 0.5f, 0.5f, .8f); //turns choice text gray and a bit transparent if it has been chosen before
	
	public override void setToIneligible()
	{
		//base.setToIneligible();
		
		foreach(TextMeshProUGUI buttonText in buttonTexts)
		{
			buttonText.color = colorIndicatingChosenBefore;
		}
		
	}
}
