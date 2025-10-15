using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSecondaryStatsPanelButton : MonoBehaviour
{

    public GameObject showButton;
    public GameObject hideButton;
    public GameObject secondaryStatsPanelGameObject;

    public SecondaryStatsPanel secondaryStatsPanel;

    public void showPanel()
    {
        showButton.SetActive(false);

        secondaryStatsPanelGameObject.SetActive(true);
        hideButton.SetActive(true);
    }

    public void hidePanel()
    {
        showButton.SetActive(true);

        secondaryStatsPanelGameObject.SetActive(false);
        hideButton.SetActive(false);
    }


}
