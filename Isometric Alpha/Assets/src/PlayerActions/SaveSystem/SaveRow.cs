using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveRow : MonoBehaviour
{
	public TextMeshProUGUI saveNameText;
	public TextMeshProUGUI saveDateText;
	public TextMeshProUGUI saveNumberText;
	
	private SaveBlueprint saveBlueprint;
	
	public void setSaveBlueprint(SaveBlueprint saveBlueprint)
	{
		this.saveBlueprint = saveBlueprint;
	}
	
	public SaveBlueprint getSaveBlueprint()
	{
		return saveBlueprint;
	}
}
