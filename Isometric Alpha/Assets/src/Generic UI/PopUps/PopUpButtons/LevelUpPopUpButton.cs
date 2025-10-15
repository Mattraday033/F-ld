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

        PlayerOOCStateManager.setCurrentActivity(OOCActivity.inUI);
    }

    private IEnumerator waitToPopUp()
    {
        while(CombatStateManager.inCombat || PlayerOOCStateManager.currentActivity != OOCActivity.walking)
        {
            yield return null;
        }
        
        PopUpBlocker.spawnPopUpScreenBlocker();

        Instantiate(Resources.Load<GameObject>(getPopUpPrefabName(type)), PopUpBlocker.getPopUpParent());

        setPopUpWindow(getCurrentPopUpGameObject().GetComponent<PopUpWindow>());

        getPopUpWindow().setProgenitor(this);

        PlayerOOCStateManager.setCurrentActivity(OOCActivity.inLevelUpPopUp);

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
