using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCloseButton : MonoBehaviour
{

    private void OnEnable()
    {
        if(PlayerStateManager.inMainMenu() && SaveHandler.getInstance() != null)
        {
            SaveHandler.getInstance().redCloseButton = gameObject;
        }
    }

    public void closeUI()
    {
        if(PlayerStateManager.inMainMenu() && StartingMenuManager.getInstance() != null)
        {
            StartingMenuManager.getInstance().handleESCPress();
        } else
        {
            PlayerInput.backOutOfUI();
        }
    }

}
