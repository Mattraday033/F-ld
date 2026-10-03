using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Events;

public class HoverPanelPopUpButton : PopUpButton
{
    private static readonly ProfilerMarker prewarmMarker = new ProfilerMarker("HoverPanel.prewarm");

    public readonly static UnityEvent HoverPriorityRequest = new UnityEvent();

    public static Stats currentCombatantWithPriority;

	public HoverPanelPopUpButton():
	base(PopUpType.HoverPanel)
	{

	}

    private Stats findCurrentCombatant()
    {
        Stats currentCombatant = null;
        HoverPriorityRequest.Invoke();

        if(currentCombatantWithPriority != null)
        {
            currentCombatant = currentCombatantWithPriority;
            currentCombatantWithPriority = null;
        } else
        {
            CombatGrid.combatantExistsAtCoords(SelectorManager.getCurrentSelector().getCoords(), out currentCombatant);
        }

        //a reposition placeholder stands in for its original on either path, so the panel sees them as the same combatant
        if(currentCombatant != null && currentCombatant.isRepositionClone())
        {
            Stats originalCombatant = CombatGrid.findOriginalCombatant(currentCombatant);

            if(originalCombatant != null)
            {
                return originalCombatant;
            }
        }

        return currentCombatant;
    }

    //the panel is created once and then kept, so this only instantiates it the first time
    private HoverPanel getOrCreatePanel()
    {
        if(HoverPanel.getInstance() == null)
        {
		    Instantiate(Resources.Load<GameObject>(getPopUpPrefabName(type)), PopUpScreenBlockerManager.getPopUpParent());

		    setPopUpWindow(getCurrentPopUpGameObject().GetComponent<PopUpWindow>());

		    getPopUpWindow().setProgenitor(this);
        }

        return HoverPanel.getInstance();
    }

	public override void spawnPopUp()
	{
        Stats currentCombatant = findCurrentCombatant();

        if(currentCombatant == null)
        {
            destroyPopUp();
            return;
        }

		HoverPanel currentWindow = getOrCreatePanel();

        currentWindow.show();
		currentWindow.populate(currentCombatant);
	}

    //hides rather than destroys, so the same panel is used for the rest of the combat. The side effects are the ones
    //destroying it had, with "the panel was showing" in place of "the panel existed"
	public override void destroyPopUp()
	{
        HoverPanel panel = HoverPanel.getInstance();

		if (panel != null && !(panel is null) && panel.isShowing)
		{
			panel.hide();
			EscapeStack.removeTopObjectFromStack();
		}
		else
		{
			EscapeStack.removeAllNullObjectsFromStack();
		}

		PopUpScreenBlockerManager.destroyPopUpScreenBlocker();

		if(shouldReturnToWalkingMode())
		{
			PlayerStateManager.setCurrentActivity(CurrentActivity.Walking);
		}
	}

    //builds the panel and both of its row layouts while it isn't drawn, so the first look at an ally or an enemy doesn't hitch
    public void prewarm()
    {
        using(prewarmMarker.Auto())
        {
            Stats ally = getFirstCombatant(CombatGrid.getAllAliveAllyCombatants());
            Stats enemy = getFirstCombatant(CombatGrid.getAllAliveNonsummonedEnemies());

            if(ally == null && enemy == null)
            {
                return;
            }

            HoverPanel panel = getOrCreatePanel();

            //a tutorial step may already have shown the panel, in which case it's put back the way it was
            bool wasShowing = panel.isShowing;
            Stats combatantShown = panel.getDisplayedCombatant();

            panel.setRendered(false);
            panel.show();

            panel.populate(enemy);
            panel.populate(ally);

            if(wasShowing)
            {
                panel.populate(combatantShown);
            } else
            {
                //hidden directly rather than through destroyPopUp, which would pop the escape stack
                panel.hide();
            }

            panel.setRendered(true);
        }
    }

    private static Stats getFirstCombatant(List<Stats> combatants)
    {
        return combatants != null && combatants.Count > 0 ? combatants[0] : null;
    }

    public override GameObject getCurrentPopUpGameObject()
    {
        if (HoverPanel.getInstance() != null && !(HoverPanel.getInstance() is null))
        {
            return HoverPanel.getInstance().gameObject;
        }
        else
        {
            return null;
        }
    }

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateHoverPanelPopUpButton()
    {
        currentCombatantWithPriority = null;
    }
}
