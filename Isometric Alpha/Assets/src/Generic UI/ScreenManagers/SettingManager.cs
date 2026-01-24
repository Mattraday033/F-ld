using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SettingsManager : ScreenManager, IEscapable
{
    private static SettingsManager instance;

    public GameObject quitMenu;

    public static SettingsManager getInstance()
    {
        return instance;
    }

    public override void Awake()
    {
        base.Awake();

        instance = this;

        if(quitMenu != null && CombatStateManager.inCombat)
        {
            quitMenu.SetActive(false);
        }

        if (CombatStateManager.inCombat)
        {
            TutorialSequenceStepTargetUIObject.createCutOutMask(transform);
        }
    }

    public override bool requiresPartyMemberSelectionGrid()
    {
        return false;
    }

    public override List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();
        listOfEvents.Add(OnScreenInteriorUpdate);

        return listOfEvents;
    }
    public override DescribableList getDefaultDescribableList()
    {
        return DescribableList.Unnecessary;
    }

    public override void updateCounter()
    {
        //Empty on Purpose
    }
    public void handleEscapePress()
    {
        EscapeStack.removeTopObjectFromStack();
        Destroy(gameObject);
    }
}