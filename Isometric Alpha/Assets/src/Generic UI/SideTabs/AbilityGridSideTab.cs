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

    private void Awake()
    {
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
        closedButton.interactable = true;

        if (openTabPanel == null || openTabPanel is null)
        {
            return;
        }

        openTabPanel.SetActive(false);
    }

    public virtual void setToDefaultState()
    {
        if((currentTabDict.ContainsKey(getCurrentDictKey()) && listToChoose == currentTabDict[getCurrentDictKey()]) || 
            (!currentTabDict.ContainsKey(getCurrentDictKey()) && getCurrentDictKey().getDefaultDescribableList() == listToChoose))
        {
            closedButton.onClick.Invoke();
        } else
        {
            setToClosed();
        }

        if(currentTabDict[getCurrentDictKey()] == listToChoose)
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

    public static void setCurrentTabDict(ITabParent tabParent, DescribableList newList)
    {
        currentTabDict[tabParent] = newList;

        OnSideTabChosen.Invoke();
    }

    public static ITabParent getCurrentDictKey()
    {
        switch(PlayerOOCStateManager.currentActivity)
        {
            case OOCActivity.inShopUI:
                return ShopPopUpWindow.getInstance();
            default:
                return OverallUIManager.currentScreenManager;
        }
    }

}
