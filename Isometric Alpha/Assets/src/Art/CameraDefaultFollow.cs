using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraDefaultFollow : MonoBehaviour
{
	public CinemachineVirtualCamera mainCM;
	
    private void Awake()
    {
        if(mainCM == null)
        {
            return;
        }

        Debug.LogError("Screen.height = " + Screen.height);
        Debug.LogError("mainCM.m_Lens.OrthographicSize = " + mainCM.m_Lens.OrthographicSize);
        Debug.LogError("PPU = " + Screen.height / (mainCM.m_Lens.OrthographicSize*2));
        

        // mainCM.m_Lens.OrthographicSize = Screen.height/

        // Vertical Screen Resolution / PPU / 2
    }

    void Start()
    {
        if(PlayerMovement.getInstance() != null && PlayerOOCStateManager.currentActivity != OOCActivity.inDialogue)
		{
			mainCM.m_Follow = PlayerMovement.getInstance().gameObject.transform;
		}
    }
}
