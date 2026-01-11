using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AbilityGridSideTab : MonoBehaviour
{
    private static Dictionary<ScreenManager,DescribableList> currentTabDict;

    public int tabIndex;

    public GameObject openTabPanel;
    public Button closedButton;

    public DescribableList listToChoose;

    public readonly static UnityEvent OnSideTabChosen = new UnityEvent();


    static AbilityGridSideTab()
    {
        LoadSaveFile.OnLoad.AddListener(initializeAbilityGridSideTab);
    }

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

    public static DescribableList getDescribableList()
    {
        if(!currentTabDict.ContainsKey(OverallUIManager.currentScreenManager))
        {
            return OverallUIManager.currentScreenManager.getDefaultDescribableList();
        }

        return currentTabDict[OverallUIManager.currentScreenManager];
    }

    public virtual void setToOpen()
    {
        currentTabDict[OverallUIManager.currentScreenManager] = listToChoose;
        
        OnSideTabChosen.Invoke();
        openTabPanel.SetActive(true);

        // OverallUIManager.currentScreenManager.setCurrentTabCollection(getCurrentScreenAbilityGridIndex());
        // OverallUIManager.currentScreenManager.setCurrentTab(tabIndex);

        // OverallUIManager.currentScreenManager.populateGrid(getCurrentScreenAbilityGridIndex());
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

    private void setToDefaultState()
    {
        if((currentTabDict.ContainsKey(OverallUIManager.currentScreenManager) && listToChoose == currentTabDict[OverallUIManager.currentScreenManager]) || 
            OverallUIManager.currentScreenManager.getDefaultDescribableList() == listToChoose)
        {
            closedButton.onClick.Invoke();
        } else
        {
            setToClosed();
        }
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeAbilityGridSideTab()
    {
        currentTabDict = new Dictionary<ScreenManager,DescribableList>();
    }

}
