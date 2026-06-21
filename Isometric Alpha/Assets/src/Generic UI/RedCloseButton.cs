using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCloseButton : MonoBehaviour
{

    private void OnEnable()
    {
        if(Flags.isInNewGameMode() && SaveHandler.getInstance() != null)
        {
            SaveHandler.getInstance().redCloseButton = gameObject;
        }
    }

    public void closeUI()
    {
        if(Flags.isInNewGameMode() && StartingMenuManager.getInstance() != null)
        {
            StartingMenuManager.getInstance().handleESCPress();
        } else
        {
            PlayerInput.backOutOfUI();
        }
    }

}
