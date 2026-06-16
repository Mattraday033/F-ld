using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class ExuberanceEquippedPassive : EquippedPassive
{
    private const int redStackIndex = 0;
    private const int blueStackIndex = 1;
    private const int yellowStackIndex = 2;
    private const int greenStackIndex = 3;

	private int[] numberOfFreeStacks;
	private MultiStackProcType[] procTypes;


	public ExuberanceEquippedPassive(CombatActionSettings settings, MultiStackProcType procType, int numberOfFreeStacks) :
	base(settings)
	{
		this.numberOfFreeStacks = new int[] { numberOfFreeStacks };
		this.procTypes = new MultiStackProcType[] { procType };
	}

	public ExuberanceEquippedPassive(CombatActionSettings settings, MultiStackProcType[] procTypes, int[] numberOfFreeStacks) :
	base(settings)
	{
		this.numberOfFreeStacks = numberOfFreeStacks;
		this.procTypes = procTypes;
	}

	public override bool autoApply()
	{
		return false;
	}

	public override string getUseDescription()
    {
        string useDescription = "This Equipped Passive adds the following Exuberances to the Party's total at the beginning of Combat:\n\n";

        if(getRedStacksAtStart() > 0)
        {
            useDescription += getRedStacksAtStart() + " Red Knife stack(s).\n";
        }

        if(getBlueStacksAtStart() > 0)
        {
            useDescription += getBlueStacksAtStart() + " Blue Shield stack(s).\n";
        }

        if(getYellowStacksAtStart() > 0)
        {
            useDescription += getYellowStacksAtStart() + " Yellow Thorn stack(s).\n";
        }

        if(getGreenStacksAtStart() > 0)
        {
            useDescription += getGreenStacksAtStart() + " Green Leaf stack(s).";
        }

        if(useDescription[useDescription.Length-1] == '\n')
        {
            useDescription = useDescription.Substring(0,useDescription.Length-1);
        }

        return useDescription;
    }

    public override int getRedStacksAtStart()
    {
        if (numberOfFreeStacks == null || numberOfFreeStacks.Length <= redStackIndex)
        {
            return 0;
        }

        return numberOfFreeStacks[redStackIndex];
    }

    public override int getBlueStacksAtStart()
    {
        if (numberOfFreeStacks == null || numberOfFreeStacks.Length <= blueStackIndex)
        {
            return 0;
        }

        return numberOfFreeStacks[blueStackIndex];
    }

    public override int getYellowStacksAtStart()
    {
        if (numberOfFreeStacks == null || numberOfFreeStacks.Length <= yellowStackIndex)
        {
            return 0;
        }

        return numberOfFreeStacks[yellowStackIndex];
    }

    public override int getGreenStacksAtStart()
    {
        if (numberOfFreeStacks == null || numberOfFreeStacks.Length <= greenStackIndex)
        {
            return 0;
        }

        return numberOfFreeStacks[greenStackIndex];
    }
}
