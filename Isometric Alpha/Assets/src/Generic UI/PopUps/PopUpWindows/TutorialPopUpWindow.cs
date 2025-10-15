using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopUpWindow : PageReaderPopUpWindow
{
    private static TutorialPopUpWindow instance;

    public List<string> pageKeys = new List<string>();

    public RectTransform windowRectTransform;

    public static TutorialPopUpWindow getInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Duplicate instances of TutorialPopUpWindow exist erroneously.");
        }

        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyBindingList.moveRightKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            populateNextPage();
        }

        if (Input.GetKeyDown(KeyBindingList.moveLeftKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            populatePreviousPage();
        }

        if (!CombatStateManager.inCombat)
        {
            return;
        }

        KeyPressManager.updateKeyBools();

        if (KeyBindingList.eitherBackoutKeyIsPressed() && !KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            closeButtonPress();
        }
    }

    private void OnDisable()
    {
        instance = null;
    }

    public void addTutorialPages(string tutorialKey)
    {
        int currentPageNum = 1;
        GameObject pageObject = Resources.Load<GameObject>(tutorialKey + pageBridge + currentPageNum);

        while (pageObject != null)
        {
            pageKeys.Add(tutorialKey + pageBridge + currentPageNum);

            currentPageNum++;
            pageObject = Resources.Load<GameObject>(tutorialKey + pageBridge + currentPageNum);
        }
    }

    public override GameObject getNextPageObject()
    {
        if (getCurrentPageNum() + 1 >= pageKeys.Count)
        {
            return null;
        } else 
        {
           return Resources.Load<GameObject>(getPageName(getCurrentPageNum() + 1));
        }   
    }

    public override void setCurrentPageObject(GameObject newPageObject)
    {
        base.setCurrentPageObject(newPageObject);

        LayoutRebuilder.ForceRebuildLayoutImmediate(windowRectTransform);
    }

    public override string getPageName(int pageIndex)
    {
        return pageKeys[pageIndex];
    }

    public override void closeButtonPress()
    {
        base.closeButtonPress();

        PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
	}
}
