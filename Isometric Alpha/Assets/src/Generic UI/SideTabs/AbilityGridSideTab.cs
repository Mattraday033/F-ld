using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface ITabParent : ICounter
{
    public DescribableList getDefaultDescribableList();
}

public class AbilityGridSideTab : MonoBehaviour
{
    private static Dictionary<ITabParent,DescribableList> currentTabDict;

    public GameObject openTabPanel;
    public Button closedButton;

    public DescribableList listToChoose;

    public readonly static UnityEvent OnSideTabChosen = new UnityEvent();

    //the screen this tab sits in, if it sits in one. The shop's tabs have none
    private ScreenManager owningScreen;

    private void Awake()
    {
        owningScreen = GetComponentInParent<ScreenManager>(true);

        setToClosed();

        OnSideTabChosen.AddListener(setToClosed);
        ScreenManager.OnScreenInteriorUpdate.AddListener(setToDefaultState);
    }

    private void OnDestroy()
    {
        OnSideTabChosen.RemoveListener(setToClosed);
        ScreenManager.OnScreenInteriorUpdate.RemoveListener(setToDefaultState);
    }

    public static DescribableList getDescribableListType()
    {
        if(getCurrentDictKey() != null && !currentTabDict.ContainsKey(getCurrentDictKey()))
        {
            return getCurrentDictKey().getDefaultDescribableList();
        }

        return currentTabDict[getCurrentDictKey()];
    }

    public virtual void setToOpen()
    {
        if(getCurrentDictKey() == null)
        {
            return;
        }

        setCurrentTabDict(getCurrentDictKey(), listToChoose);

        closedButton.interactable = false;

        if (openTabPanel == null || openTabPanel is null)
        {
            return;
        }

        openTabPanel.SetActive(true);
    }

    public virtual void setToClosed()
    {
        //a tab on a hidden screen is set up again when its screen is next shown
        if (ScreenManager.isHidden(owningScreen))
        {
            return;
        }

        closedButton.interactable = true;

        if (openTabPanel == null || openTabPanel is null)
        {
            return;
        }

        openTabPanel.SetActive(false);
    }

    public virtual void setToDefaultState()
    {
        //the screen that is showing is the one the tab dictionary is being asked about, so a tab on a hidden screen has no say
        if (ScreenManager.isHidden(owningScreen))
        {
            return;
        }

        if((currentTabDict.ContainsKey(getCurrentDictKey()) && listToChoose == currentTabDict[getCurrentDictKey()]) || 
            (!currentTabDict.ContainsKey(getCurrentDictKey()) && getCurrentDictKey().getDefaultDescribableList() == listToChoose))
        {
            closedButton.onClick.Invoke();
        } else
        {
            setToClosed();
        }

        //no tab has been chosen for this screen until the default one's click above has gone through
        if(currentTabDict.TryGetValue(getCurrentDictKey(), out DescribableList chosenList) && chosenList == listToChoose)
        {
            closedButton.interactable = false;
        }
    }

    public static void chooseTab(DescribableList list)
    {
        currentTabDict[getCurrentDictKey()] = list;
        ScreenManager.OnScreenInteriorUpdate.Invoke();
    }

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        currentTabDict = new Dictionary<ITabParent,DescribableList>();

        LoadSaveFile.OnLoadResetData.RemoveListener(init);
        LoadSaveFile.OnLoadResetData.AddListener(init);
    }

    //a screen that is shown again opens on its default tab, as one that was just made does
    public static void forgetTab(ITabParent tabParent)
    {
        currentTabDict.Remove(tabParent);
    }

    public static void setCurrentTabDict(ITabParent tabParent, DescribableList newList)
    {
        currentTabDict[tabParent] = newList;

        OnSideTabChosen.Invoke();
    }

    public static ITabParent getCurrentDictKey()
    {
        switch(PlayerStateManager.currentActivity)
        {
            case CurrentActivity.InShopUI:
                return ShopPopUpWindow.getInstance();
            default:
                return OverallUIManager.currentScreenManager;
        }
    }

}
