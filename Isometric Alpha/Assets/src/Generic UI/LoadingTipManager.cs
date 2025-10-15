using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingTip
{
    public bool used;
    public string tip;

    public LoadingTip(string tip)
    {
        this.tip = tip;
        this.used = false;
    }

}

public static class LoadingTipList
{
    public static ArrayList loadingTips;

    static LoadingTipList()
    {
        loadingTips = new ArrayList();

        loadingTips.Add(new LoadingTip("\"There is only one thing you must know here: the guards cannot break you. They can only trick you into breaking yourself.\" - Bálint the Sage"));

        loadingTips.Add(new LoadingTip("An Autosave is taken every time you enter a new area. You can have up to three Autosaves."));

        loadingTips.Add(new LoadingTip("Press '"+KeyBindingList.hideTerrainKey.ToString() + "' to remove terrain that may be blocking your view."));

        loadingTips.Add(new LoadingTip("Don't forget to level up your followers by spending Affinity on the Party screen."));

        loadingTips.Add(new LoadingTip("Press 'Shift' to highlight interactable people and objects."));

        loadingTips.Add(new LoadingTip("You only need to kill every Master creature to win in Combat. All Minion creatures will flee after the last Master creature is slain."));

        loadingTips.Add(new LoadingTip("You gain Affinity for every creature you slay in combat, including Minions. Affinity can only be gained when at least one companion is deployed in combat."));

        loadingTips.Add(new LoadingTip("\"Brittle is any peace built with human hands.\" - Craft Folk proverb"));

        loadingTips.Add(new LoadingTip("You cannot retreat from combat that was entered from dialogue."));
    }
}


public class LoadingTipManager : MonoBehaviour
{
    public TextMeshProUGUI loadingTipMessage;

    private void Awake()
    {
        setLoadingScreenTip();
    }
    private bool noTipsLeft()
    {
        foreach (LoadingTip tip in LoadingTipList.loadingTips)
        {
            if(!tip.used)
            {
                return false;
            }
        }

        return true;
    }

    private bool noTipsUsed()
    {
        foreach (LoadingTip tip in LoadingTipList.loadingTips)
        {
            if (tip.used)
            {
                return false;
            }
        }

        return true;
    }

    private void resetTipUsage()
    {
        for(int index = 0; index < LoadingTipList.loadingTips.Count; index++)
        {
            LoadingTip tip = (LoadingTip) LoadingTipList.loadingTips[index];
            tip.used = false;
        }
    }

    public void setLoadingScreenTip()
    {
        if(noTipsUsed())
        {
            //Debug.LogError("No Tips Have Been Used"); 
        }

        if(noTipsLeft())
        {
            resetTipUsage();
        }

        int index = UnityEngine.Random.Range(0, LoadingTipList.loadingTips.Count-1);
        LoadingTip tip = ((LoadingTip) LoadingTipList.loadingTips[index]);

        if (!tip.used)
        {
            loadingTipMessage.text = tip.tip;
            tip.used = true;
            return;
        }

        for(int currentIndex = index + 1; currentIndex < LoadingTipList.loadingTips.Count; currentIndex++)
        {
            tip = ((LoadingTip) LoadingTipList.loadingTips[currentIndex]);

            if(!tip.used)
            {
                loadingTipMessage.text = tip.tip;
                tip.used = true;
                return;
            }
        }

        for (int currentIndex = index - 1; currentIndex >= 0; currentIndex--)
        {
            tip = ((LoadingTip) LoadingTipList.loadingTips[currentIndex]);

            if (!tip.used)
            {
                loadingTipMessage.text = tip.tip;
                tip.used = true;
                return;
            }
        }

        Debug.LogError("Made it to the end of setLoadingScreenTip() somehow without setting a loading screen tip.");
    }
}
