using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class LevelUpPopUpButton : PopUpButton
{
    public static Coroutine waitingLevelUpPopUp = null;

    public LevelUpPopUpButton() :
    base(PopUpType.LevelUp)
    {

    }

    public override void spawnPopUp()
    {
        if(waitingLevelUpPopUp == null && PlayerMovement.getInstance() != null)
        {
            waitingLevelUpPopUp = PlayerMovement.getInstance().StartCoroutine(waitToPopUp()); //used PlayerMovement because it should always exist when out of combat
        }
    }

    public override void destroyPopUp()
    {
        base.destroyPopUp();

        PlayerStateManager.setCurrentActivity(CurrentActivity.InUI);
    }

    private IEnumerator waitToPopUp()
    {
        while(CombatStateManager.inCombat || PlayerStateManager.currentActivity != CurrentActivity.Walking)
        {
            yield return null;
        }
        
        PopUpScreenBlockerManager.spawnPopUpScreenBlocker();

        Instantiate(Resources.Load<GameObject>(getPopUpPrefabName(type)), PopUpScreenBlockerManager.getPopUpParent());

        setPopUpWindow(getCurrentPopUpGameObject().GetComponent<PopUpWindow>());

        getPopUpWindow().setProgenitor(this);

        PlayerStateManager.setCurrentActivity(CurrentActivity.InLevelUpPopUp);

        waitingLevelUpPopUp = null;
    }
    public override GameObject getCurrentPopUpGameObject()
    {
        // if (LevelUpWindow.getInstance() != null && !(LevelUpWindow.getInstance() is null))
        // {
        //     return LevelUpWindow.getInstance().gameObject;
        // }
        // else
        // {
            return null;
        // }
    }
}
