using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyPositionAcceptButton : MonoBehaviour
{

	public Button acceptButton;
	
	public void activate()
	{
		acceptButton.interactable = true;
	}
	
	public void deactivate()
	{
		acceptButton.interactable = false;
	}
}
