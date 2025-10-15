using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenTrait : Trait
{
	private bool untargetable;

	public HiddenTrait(string traitName, bool untargetable):
	base(traitName)
	{
		this.untargetable = untargetable;
	}

	public override bool isUntargetable()
	{
		return untargetable;
	}

}
