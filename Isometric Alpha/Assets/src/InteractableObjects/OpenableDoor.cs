using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableDoor : MonoBehaviour
{

	public GameObject openDoor;
	
	public void open()
	{
		openDoor.SetActive(true);
		gameObject.SetActive(false);
	}
}
