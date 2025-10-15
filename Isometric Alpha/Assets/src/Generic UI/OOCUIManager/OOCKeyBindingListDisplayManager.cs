using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOCKeyBindingListDisplayManager : MonoBehaviour
{
    private static bool keyBindingListShowing = true;

    private static OOCKeyBindingListDisplayManager instance;
    
    public GameObject keyBindingsParent;
    public GameObject keyBindingsShowingBackground;
    public GameObject keyBindingsHiddenBackground;

    private void Awake()
    {
        if (instance != null)
        {
            instance.destroyAllGameObject();
            DestroyImmediate(instance);
        }

        instance = this;

        setKeyBindingListToPreviousSetting();

        if (PartyManager.getPlayerStats() == null)
        {
            setKeyBindingsListToInvisibile();
        }
    }

    public static OOCKeyBindingListDisplayManager getInstance()
    {
        return instance;
    }

    private void OnEnable()
    {
        PlayerOOCStateManager.OnStateChangeToWalking.AddListener(setKeyBindingsListToVisibile);
        PlayerOOCStateManager.OnStateChangeFromWalking.AddListener(setKeyBindingsListToInvisibile);
    }

    private void OnDisable()
    {
        PlayerOOCStateManager.OnStateChangeToWalking.RemoveListener(setKeyBindingsListToVisibile);
        PlayerOOCStateManager.OnStateChangeFromWalking.RemoveListener(setKeyBindingsListToInvisibile);
    }

    public static void toggleKeyBindingList()
    {
        if (keyBindingListShowing)
        {
            instance.revealShowKeyBindingsPrompt();
            keyBindingListShowing = false;
        }
        else
        {
            instance.revealFullKeyBindingsList();
            keyBindingListShowing = true;
        }
    }

    private void setKeyBindingListToPreviousSetting()
    {
        if (keyBindingListShowing)
        {
            revealFullKeyBindingsList();
        }
        else
        {
            revealShowKeyBindingsPrompt();
        }
    }

    private void revealFullKeyBindingsList()
    {
        if (Flags.isInNewGameMode())
        {
            setKeyBindingsListToInvisibile(); ;
        }

        setKeyBindingsListVisibility(true);
        keyBindingsShowingBackground.SetActive(true);
        keyBindingsHiddenBackground.SetActive(false);
    }

    private void revealShowKeyBindingsPrompt()
    {
        if (Flags.isInNewGameMode())
        {
            return;
        }

        setKeyBindingsListVisibility(true);
        keyBindingsShowingBackground.SetActive(false);
        keyBindingsHiddenBackground.SetActive(true);
    }

    public void setKeyBindingsListToVisibile()
    {
        if (Flags.isInNewGameMode())
        {
            return;
        }
        
        keyBindingsParent.SetActive(true);
    }

    public void setKeyBindingsListToInvisibile()
    {
        keyBindingsParent.SetActive(false);
    }

    private void setKeyBindingsListVisibility(bool activate)
    {
        keyBindingsParent.SetActive(activate);
    }

    private void destroyAllGameObject()
    {
        DestroyImmediate(keyBindingsParent);
        DestroyImmediate(keyBindingsShowingBackground);
        DestroyImmediate(keyBindingsHiddenBackground);
    }

}
