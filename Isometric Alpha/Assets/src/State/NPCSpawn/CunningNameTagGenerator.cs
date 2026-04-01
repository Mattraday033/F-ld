using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CunningNameTagGenerator : NameTagGenerator
{
	public override Color getRevealColor()
	{
		return ColorList.canBeCunninged;
	}
}
