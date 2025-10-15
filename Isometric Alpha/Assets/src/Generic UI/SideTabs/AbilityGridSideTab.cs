using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AbilityGridSideTab : MonoBehaviour
{

    public int tabIndex;

    public GameObject openTabPanel;
    public Button closedButton;

    public static UnityEvent OnSideTabChosen = new UnityEvent();


    private void Awake()
    {
        setToClosed();

        OnSideTabChosen.AddListener(setToClosed);
    }

    private void OnDestroy()
    {
        OnSideTabChosen.RemoveListener(setToClosed);
    }

    private int getCurrentScreenAbilityGridIndex()
    {
        return OverallUIManager.currentScreenManager.getAbilityGridIndex();
    }

    public virtual void setToOpen()
    {
        OnSideTabChosen.Invoke();
        openTabPanel.SetActive(true);

        OverallUIManager.currentScreenManager.setCurrentTabCollection(getCurrentScreenAbilityGridIndex());
        OverallUIManager.currentScreenManager.setCurrentTab(tabIndex);

        OverallUIManager.currentScreenManager.populateGrid(getCurrentScreenAbilityGridIndex());
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

}
