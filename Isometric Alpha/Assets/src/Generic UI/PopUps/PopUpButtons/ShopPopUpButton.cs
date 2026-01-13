using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShopPopUpButton : PopUpButton
{
    public ShopPopUpButton() :
    base(PopUpType.Shop)
    {

    }

    public void spawnPopUp(Shopkeeper currentShopkeeper)
    {
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.inShopUI);

        spawnPopUp();

        ShopPopUpWindow popUpWindow = (ShopPopUpWindow) getPopUpWindow();

        popUpWindow.setCurrentShopkeeper(currentShopkeeper);
        ScreenManager.OnScreenInteriorUpdate.Invoke();

        getCurrentPopUpGameObject().SetActive(true);
    }

    public override void spawnPopUp()
    {
        base.spawnPopUp();
    }

    public override void destroyPopUp()
    {
        base.destroyPopUp();

        PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
    }

    public override GameObject getCurrentPopUpGameObject()
    {
        if (ShopPopUpWindow.getInstance() != null && !(ShopPopUpWindow.getInstance() is null))
        {
            return ShopPopUpWindow.getInstance().gameObject;
        }
        else
        {
            return null;
        }
    }
}
