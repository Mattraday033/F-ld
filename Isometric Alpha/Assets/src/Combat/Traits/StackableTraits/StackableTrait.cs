using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public delegate bool ReapplicationLogicDelegate<T>(T t);
public enum ActionCostType { None = 1, Stance = 2, Bloodlust = 3, Predation = 4, 
                             RedKnife = 5, BlueShield = 6, YellowThorn = 7, GreenLeaf = 8 }

public class StackableTrait: Trait
{
    private int startingStacks;
	private int numberOfStacks;
    private int maximumStacks = 99;
    private int stacksAppliedPerApplication;

	private UnityEvent[] reapplicationEvents;

	private Trait[] baseTraits;
    private ActionCostType costType; 
	
	public StackableTrait(int startingStacks, int stacksAppliedPerApplication, Trait baseTrait) :
	base(baseTrait.getName(), baseTrait.getType(), baseTrait.getDescription(), baseTrait.getIconName(), baseTrait.getTraitIconBackgroundColor())
	{
        this.startingStacks = startingStacks;
        resetStacksToStartingAmount();

        this.stacksAppliedPerApplication = stacksAppliedPerApplication;

        this.costType = ActionCostType.None;
        this.baseTraits = new Trait[] { baseTrait };
    }

    public StackableTrait(int startingStacks, int stacksAppliedPerApplication, ActionCostType costType, Trait baseTrait) :
    base(baseTrait.getName(), baseTrait.getType(), baseTrait.getDescription(), baseTrait.getIconName(), baseTrait.getTraitIconBackgroundColor())
    {
        this.startingStacks = startingStacks;
        resetStacksToStartingAmount();

        this.stacksAppliedPerApplication = stacksAppliedPerApplication;

        this.costType = costType;
        this.baseTraits = new Trait[] { baseTrait };
    }

    public StackableTrait(UnityEvent reapplicationEvent, int startingStacks, int stacksAppliedPerApplication, Trait baseTrait) :
	base(baseTrait.getName(), baseTrait.getType(), baseTrait.getDescription(), baseTrait.getIconName(), baseTrait.getTraitIconBackgroundColor())
    {
        this.reapplicationEvents = new UnityEvent[] { reapplicationEvent };
        //this.stackChangeAction += reapply; removed so that the base version of the method in TraitList isn't subscribed. Each StackableTrait is subscribed in clone()

        this.startingStacks = startingStacks;
        resetStacksToStartingAmount();

        this.stacksAppliedPerApplication = stacksAppliedPerApplication;

        this.costType = ActionCostType.None;
        this.baseTraits = new Trait[] { baseTrait };
    }

    public StackableTrait(UnityEvent reapplicationEvent, int startingStacks, int stacksAppliedPerApplication, ActionCostType costType, Trait baseTrait) :
    base(baseTrait.getName(), baseTrait.getType(), baseTrait.getDescription(), baseTrait.getIconName(), baseTrait.getTraitIconBackgroundColor())
    {
        this.reapplicationEvents = new UnityEvent[] { reapplicationEvent };
        //this.stackChangeAction += reapply; removed so that the base version of the method in TraitList isn't subscribed. Each StackableTrait is subscribed in clone()

        this.startingStacks = startingStacks;
        resetStacksToStartingAmount();

        this.stacksAppliedPerApplication = stacksAppliedPerApplication;

        this.costType = costType;
        this.baseTraits = new Trait[] { baseTrait };
    }
    public StackableTrait(UnityEvent reapplicationEvent, int startingStacks, int stacksAppliedPerApplication, ActionCostType costType, Trait[] baseTraits) :
    base(baseTraits[0].getName(), baseTraits[0].getType(), baseTraits[0].getDescription(), baseTraits[0].getIconName(), baseTraits[0].getTraitIconBackgroundColor())
    {
        this.reapplicationEvents = new UnityEvent[] { reapplicationEvent };
        //this.stackChangeAction += reapply; removed so that the base version of the method in TraitList isn't subscribed. Each StackableTrait is subscribed in clone()

        this.startingStacks = startingStacks;
        resetStacksToStartingAmount();

        this.stacksAppliedPerApplication = stacksAppliedPerApplication;

        this.costType = costType;
        this.baseTraits = baseTraits;
    }

    public StackableTrait(UnityEvent[] reapplicationEvents, int startingStacks, int stacksAppliedPerApplication, int maximumStacks, ActionCostType costType, Trait baseTraits) :
    base(baseTraits.getName(), baseTraits.getType(), baseTraits.getDescription(), baseTraits.getIconName(), baseTraits.getTraitIconBackgroundColor())
    {
        this.reapplicationEvents = reapplicationEvents;
        //this.stackChangeAction += reapply; removed so that the base version of the method in TraitList isn't subscribed. Each StackableTrait is subscribed in clone()

        this.startingStacks = startingStacks;
        resetStacksToStartingAmount();
        this.maximumStacks = maximumStacks;

        this.stacksAppliedPerApplication = stacksAppliedPerApplication;

        this.costType = costType;
        this.baseTraits = new Trait[1] { baseTraits };
    }

    private void setStackChangeActions()
    {
        if (reapplicationEvents == null)
        {
            return;
        }

        foreach (UnityEvent unityEvent in reapplicationEvents)
        {
            unityEvent.AddListener(onReapplicationEvent);
        }
    }

    public void addStackChangeActions(UnityEvent reapplicationEvent)
    {
        this.reapplicationEvents = Helpers.appendArray<UnityEvent>(this.reapplicationEvents, reapplicationEvent);
        reapplicationEvent.AddListener(onReapplicationEvent);
    }

    public virtual void onReapplicationEvent()
    {
        if(getTraitHolder() == CombatActionManager.currentActor)
        {
            reapply();
        }
    }

    public override void onApplication()
    {
        setStackChangeActions();
    }

    public override void resetStacksToStartingAmount()
    {
        numberOfStacks = startingStacks;
    }

    public override void reapply()
	{
		base.reapply();

        if(numberOfStacks + stacksAppliedPerApplication < maximumStacks)
        {
            numberOfStacks += stacksAppliedPerApplication;
        } else
        {
            numberOfStacks = maximumStacks;
        }

        //Debug.LogError("reapply() was called, current numberOfStacks = " + numberOfStacks);
    }
	
	public override int getNumberOfStacks()
	{
		return numberOfStacks;
	}
    public override void removeStacks(ActionCostType costType, int stacksToRemove)
    {
        if (numberOfStacks - stacksToRemove >= 0)
        {
            numberOfStacks -= stacksToRemove;
        } else
        {
            numberOfStacks = 0;
        }
    }

    public override int getBonusDamageDealt()
    {
        return Helpers.sum<Trait>(baseTraits, t => t.getBonusDamageDealt() * getNumberOfStacks());
    }

    public override int getBonusDamageTaken()
	{
        return Helpers.sum<Trait>(baseTraits, t => t.getBonusDamageTaken() * getNumberOfStacks());
    }

    public override bool hasActionCostType(ActionCostType typeToCheckFor)
    {
        return costType == typeToCheckFor;
    }

    public override Trait clone()
    {
        StackableTrait cloneOfTrait = (StackableTrait) Clone();

        return (Trait) cloneOfTrait;
    }

    //IDescribable Methods
    public override GameObject getRowType(RowType rowType)
    {
        return Resources.Load<GameObject>(PrefabNames.stackableTraitSquareRowPanel);
    }

    public override void describeSelfRow(DescriptionPanel panel)
    {
        base.describeSelfRow(panel);

        DescriptionPanel.setText(panel.amountText, getNumberOfStacks());
    }
}
