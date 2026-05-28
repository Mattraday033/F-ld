using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneOfInfluenceTrait : Trait
{
	
    public const string zoiTraitName = "'s Influence";
    private const string zoiTraitDescription = "This Character's Zone of Influence provides no benefits... yet.";


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
                return "Allies in this Party Member's Zone of Influence gain extra Armor.";
            case NPCNameList.carter:
                return "Allies in this Party Member's Zone of Influence gain extra Damage during a Surprise Round.";
            case NPCNameList.nandor:
                return "Allies in this Party Member's Zone of Influence gain extra Mental Resistance.";
            case NPCNameList.weft:
                return "Allies in this Party Member's Zone of Influence gain extra Healing.";
            case NPCNameList.gaspar:
                return "Allies in this Party Member's Zone of Influence gain extra Wound Resistance.";
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
            case NPCNameList.gaspar:
            case NPCNameList.weft:
                return companionName + "-" + HoverMessageList.zoneOfInfluenceKey;
            default:
                return base.getIconName();
        }
    }

    public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> buildingBlocks = base.getDescriptionBuildingBlocks();

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: IconList.ZOIIconName));

        return buildingBlocks;
    }
}
