using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RetreatUIManager : SlotIconHover
{

    private static Color lessThan50Color = Color.red;
    private static Color from50To74Color = Color.yellow; //may add orange in later between yellow and red
    private static Color from75To100Color = Color.green;

    public Button retreatButton;
    public TextMeshProUGUI retreatChanceText;

    private static RetreatUIManager instance;
    public static RetreatUIManager getInstance()
    {
        return instance;
    }
    public override void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Duplicate instances of RetreatUIManager exist erroneously");
        }

        instance = this;

        hoverMessageKey = HoverMessageList.retreatButtonKey; 

        base.Awake();
    }

    void Start()
    {
        setRetreatChanceDisplay();
    }

    void Update()
    {
        if (CombatStateManager.currentActivity == CurrentActivity.Retreating)
        {
            KeyPressManager.updateKeyBools();

            if (KeyBindingList.eitherBackoutKeyIsPressed() && !KeyPressManager.handlingPrimaryKeyPress)
            {
                EscapeStack.handleEscapePress();

                CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingActor);

                KeyPressManager.handlingPrimaryKeyPress = true;
            }
        }
    }

    public void setCurrentActivityToRetreating()
    {
        CombatStateManager.setCurrentActivity(CurrentActivity.Retreating);
    }

    private void setRetreatChanceDisplay()
    {
        setRetreatChanceTextColor(Retreat.calculateRetreatChance());

        retreatChanceText.text = Retreat.getRetreatChanceForDisplay();
    }

    private void setRetreatChanceTextColor(float retreatChance)
    {
        switch (retreatChance)
        {
            case <= .5f:
                retreatChanceText.color = lessThan50Color;
                break;
            case <= .75f:
                retreatChanceText.color = from50To74Color;
                break;
            default:
                retreatChanceText.color = from75To100Color;
                break;
        }
    }

    public static void setRetreatButtonInteractibility()
    {
        if (Retreat.calculateRetreatChance() <= 0f)
        {
            getInstance().retreatButton.interactable = false;
            return;
        }

        if (CombatStateManager.currentActivity == CurrentActivity.ChoosingActor)
        {
            getInstance().retreatButton.interactable = true;
        }
        else
        {
            getInstance().retreatButton.interactable = false;
        }
    }
    
}
