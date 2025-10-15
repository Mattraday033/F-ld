using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferTargetPriorityTrait : TargetPriorityTrait
{
	private const string initialName = "Support";
	private const string initialTraitDescription = "This creature does not attack but aids it's allies in other ways.";
	private const string initialTraitIconName = "Support";
	private static Color initialIconBackgroundColor = Color.blue;

	private TargetPriorityTrait targetPriorityTrait;

	public BufferTargetPriorityTrait(TargetPriorityTrait targetPriorityTrait): 
	base(initialName, initialTraitDescription, initialTraitIconName, initialIconBackgroundColor)
	{
		this.targetPriorityTrait = targetPriorityTrait;
	}
	
	public override Selector findTargetLocation(Selector selector, ArrayList listOfTargets)
	{
		return targetPriorityTrait.findTargetLocation(selector, listOfTargets);
	}
	
}
