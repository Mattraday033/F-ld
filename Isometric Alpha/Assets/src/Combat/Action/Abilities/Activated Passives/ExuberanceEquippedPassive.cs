using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public enum MultiStackProcType { RedKnife = 0, BlueShield = 1, YellowThorn = 2, GreenLeaf = 3 }
public class ExuberanceEquippedPassive : EquippedPassive
{
    private const int redStackIndex = 0;
    private const int blueStackIndex = 1;
    private const int yellowStackIndex = 2;
    private const int greenStackIndex = 3;

	private int[] numberOfFreeStacks;
	private MultiStackProcType[] procTypes;


	public override bool autoApply => false;

	public ExuberanceEquippedPassive(CombatActionSettings settings, MultiStackProcType procType, int numberOfFreeStacks, List<TargetingLogic> targetingLogics = null) :
	base(settings, targetingLogics)
	{
		this.numberOfFreeStacks = new int[] { numberOfFreeStacks };
		this.procTypes = new MultiStackProcType[] { procType };
	}

	public ExuberanceEquippedPassive(CombatActionSettings settings, MultiStackProcType[] procTypes, int[] numberOfFreeStacks, List<TargetingLogic> targetingLogics = null) :
	base(settings, targetingLogics)
	{
		this.numberOfFreeStacks = numberOfFreeStacks;
		this.procTypes = procTypes;
	}

	public override string getUseDescription()
    {
        string useDescription = "This Equipped Passive adds the following Exuberances to the Party's total at the beginning of Combat:\n\n";

        if(redStacksAtStart > 0)
        {
            useDescription += redStacksAtStart + " Red Knife stack(s).\n";
        }

        if(blueStacksAtStart > 0)
        {
            useDescription += blueStacksAtStart + " Blue Shield stack(s).\n";
        }

        if(yellowStacksAtStart > 0)
        {
            useDescription += yellowStacksAtStart + " Yellow Thorn stack(s).\n";
        }

        if(greenStacksAtStart > 0)
        {
            useDescription += greenStacksAtStart + " Green Leaf stack(s).";
        }

        if(useDescription[useDescription.Length-1] == '\n')
        {
            useDescription = useDescription.Substring(0,useDescription.Length-1);
        }

        return useDescription;
    }

    public override int redStacksAtStart => getFreeStacks(redStackIndex);
    public override int blueStacksAtStart => getFreeStacks(blueStackIndex);
    public override int yellowStacksAtStart => getFreeStacks(yellowStackIndex);
    public override int greenStacksAtStart => getFreeStacks(greenStackIndex);

    private int getFreeStacks(int stackIndex)
    {
        if (numberOfFreeStacks == null || numberOfFreeStacks.Length <= stackIndex)
        {
            return 0;
        }

        return numberOfFreeStacks[stackIndex];
    }
}
