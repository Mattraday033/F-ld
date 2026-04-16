using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferTargetPriorityTrait : TargetPriorityTrait
{
	private const string initialName = "Support";
	private const string initialTraitDescription = "This creature does not attack but aids it's allies in other ways.";
	private const string initialTraitIconName = "Support";

    protected override Stats traitHolder
    {
        get
        {
            return targetPriorityTrait.getTraitHolder();
        }
        set
        {
            targetPriorityTrait.setTraitHolder(value);
        }
    }

	private TargetPriorityTrait targetPriorityTrait;

	public BufferTargetPriorityTrait(TargetPriorityTrait targetPriorityTrait): 
	base(initialName, initialTraitDescription, initialTraitIconName)
	{
		this.targetPriorityTrait = targetPriorityTrait;
	}
	
	public override Selector findTargetLocation(Selector selector, List<Stats> listOfTargets)
	{
		return targetPriorityTrait.findTargetLocation(selector, listOfTargets);
	}
	
}
