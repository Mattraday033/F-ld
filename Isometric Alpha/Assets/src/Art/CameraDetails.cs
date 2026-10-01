using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraDetails : MonoBehaviour
{
	public CinemachineVirtualCamera mainCM;

    void Awake()
    {
        Camera.main.eventMask = LayerAndTagManager.mouseHoverMask;
    }

    void Start()
    {
        if(PlayerMovement.getInstance() != null && PlayerStateManager.currentActivity != CurrentActivity.InDialogue)
		{
			mainCM.Follow = PlayerMovement.getInstance().gameObject.transform;
		}
    }
}
