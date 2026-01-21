using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneOfInfluenceTrait : Trait
{
	private const string zoneOfInfluenceTraitType = "Influence";
	
	public ZoneOfInfluenceTrait(string traitName, string traitDescription, string iconBackgroundName): base(traitName, zoneOfInfluenceTraitType, traitDescription, iconBackgroundName, Color.black)
    {
        
    }
	
	public override bool fromZoneOfInfluence()
	{
		return true;
	}

    public override string getIconName()
    {
        string companionName = getName().Replace(Stats.zoiTraitName, "");

        switch(companionName)
        {
            case NPCNameList.thatch:
            case NPCNameList.nandor:
            case NPCNameList.carter:
                return companionName + "-" + HoverMessageList.zoneOfInfluenceKey;
            default:
                return base.getIconName();
        }
    }
}
