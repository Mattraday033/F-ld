using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneOfInfluenceTrait : Trait
{
	
    public const string zoiTraitName = "'s Influence";
    private const string zoiTraitDescription = "The benefits of a Zone of Influence are being applied to this creature.";


	public ZoneOfInfluenceTrait(AllyStats zoneOwner): 
    base(zoneOwner.getNameWithoutPlayerMarker() + zoiTraitName, TraitType.Influence, iconName: HoverMessageList.zoneOfInfluenceKey)
    {
        traitApplier = zoneOwner;
    }
	
    public override string getDescription()
    {
        switch (traitApplier.getName())
        {
            case NPCNameList.thatch:
                return "Creature's in this companion's Zone of Influence gain extra armor.";
            case NPCNameList.carter:
                return "Creature's in this companion's Zone of Influence gain extra damage during a surprise round.";
            case NPCNameList.nandor:
                return "Creature's in this companion's Zone of Influence gain extra Mental Resistance.";
            default:
                return zoiTraitDescription;
        }
    }

	public override bool fromZoneOfInfluence()
	{
		return true;
	}

    public override string getIconName()
    {
        string companionName = getName().Replace(zoiTraitName, "");

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
