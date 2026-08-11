using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraDefaultFollow : MonoBehaviour
{
	public CinemachineVirtualCamera mainCM;
	
    void Start()
    {
        if(PlayerMovement.getInstance() != null && PlayerOOCStateManager.currentActivity != OOCActivity.inDialogue)
		{
			mainCM.Follow = PlayerMovement.getInstance().gameObject.transform;
		}
    }
}
