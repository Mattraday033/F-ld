using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class GlossaryCategoryList
{

	public readonly static GlossaryCategory actionTypes = new GlossaryCategory("Action Types", CombatAction.getAllActionTypeGlossaryEntries());
	public readonly static GlossaryCategory partyStats = new GlossaryCategory("Party Stats", getAllPartyStatGlossaryEntries());
	public readonly static GlossaryCategory primaryStats = new GlossaryCategory("Primary Stats", getAllPrimaryStatGlossaryEntries());
	public readonly static GlossaryCategory ranges = new GlossaryCategory("Ranges", Range.getAllRangesGlossaryEntries());
	public readonly static GlossaryCategory secondaryStats = new GlossaryCategory("Secondary Stats", getAllSecondaryStatGlossaryEntries());
	public readonly static GlossaryCategory skills = new GlossaryCategory("Skills", getAllSkillGlossaryEntries());
	public readonly static GlossaryCategory traitTypes = new GlossaryCategory("Trait Types", Trait.getAllTraitTypeGlossaryEntries());

	public static List<GlossaryCategory> allGlossaryCategories;

    [RuntimeInitializeOnLoadMethod]
	private static void instantiateGlossaryCategoryList()
	{
		allGlossaryCategories = new List<GlossaryCategory>();

		allGlossaryCategories.Add(actionTypes);
		allGlossaryCategories.Add(partyStats);
		allGlossaryCategories.Add(primaryStats);
		allGlossaryCategories.Add(ranges);
		allGlossaryCategories.Add(skills);
		allGlossaryCategories.Add(secondaryStats);
		allGlossaryCategories.Add(traitTypes);
	}

	public static List<GlossaryCategory> getAllGlossaryCategories()
	{
		return allGlossaryCategories;
	}

    public static List<GlossaryEntry> getAllPrimaryStatGlossaryEntries()
	{
        List<GlossaryEntry> primaryStatEntries = new List<GlossaryEntry>();

        primaryStatEntries.Add(new StatGlossaryEntry(PrimaryStat.Strength.ToString(), "Primary Stat", HoverMessageList.strengthMessage, IconList.strengthIconName));
        primaryStatEntries.Add(new StatGlossaryEntry(PrimaryStat.Dexterity.ToString(), "Primary Stat", HoverMessageList.dexterityMessage, IconList.dexterityIconName));
        primaryStatEntries.Add(new StatGlossaryEntry(PrimaryStat.Wisdom.ToString(), "Primary Stat", HoverMessageList.wisdomMessage, IconList.wisdomIconName));
        primaryStatEntries.Add(new StatGlossaryEntry(PrimaryStat.Charisma.ToString(), "Primary Stat", HoverMessageList.charismaMessage, IconList.charismaIconName));

		return primaryStatEntries;
	}

    public static List<GlossaryEntry> getAllSecondaryStatGlossaryEntries()
	{
        List<GlossaryEntry> secondaryStatEntries = new List<GlossaryEntry>();

        // Strength
        secondaryStatEntries.Add(new StatGlossaryEntry(IconList.healthIconName, "Secondary Stat", HoverMessageList.healthMessage + " Determined by a Character's Level, and Strength."));
        secondaryStatEntries.Add(new StatGlossaryEntry(IconList.criticalHitDamageIconName, "Secondary Stat", HoverMessageList.criticalHitDamageMessage));
        secondaryStatEntries.Add(new StatGlossaryEntry(IconList.woundResistIconName, "Secondary Stat", HoverMessageList.woundResistMessage));

        // Dexterity
        secondaryStatEntries.Add(new StatGlossaryEntry(IconList.armorScoreIconName, "Secondary Stat", HoverMessageList.armorScoreMessage));
        secondaryStatEntries.Add(new StatGlossaryEntry(IconList.surpriseRoundDamageMultiplierIconName, "Secondary Stat", HoverMessageList.surpriseRoundDamageMultiplierMessage));
        secondaryStatEntries.Add(new StatGlossaryEntry(IconList.armorPenetrationIconName, "Secondary Stat", HoverMessageList.armorPenetrationMessage));

        // Wisdom
        secondaryStatEntries.Add(new StatGlossaryEntry(IconList.mentalResistIconName, "Secondary Stat", HoverMessageList.mentalResistMessage));
        secondaryStatEntries.Add(new StatGlossaryEntry(IconList.passiveSlotsIconName, "Secondary Stat", HoverMessageList.passiveSlotsMessage));
        secondaryStatEntries.Add(new StatGlossaryEntry(IconList.weaponSlotsIconName, "Secondary Stat", HoverMessageList.weaponSlotMessage));

        // Charisma
        secondaryStatEntries.Add(new StatGlossaryEntry(IconList.synergyIconName, "Secondary Stat", HoverMessageList.synergyMessage));
        secondaryStatEntries.Add(new StatGlossaryEntry(IconList.allExuberancesIconName, "Secondary Stat", HoverMessageList.bonusExuberancesMessage));
        secondaryStatEntries.Add(new StatGlossaryEntry(IconList.ZOIIconName, "Secondary Stat", HoverMessageList.zoiMessage));

		return secondaryStatEntries;
	}

    public static List<GlossaryEntry> getAllPartyStatGlossaryEntries()
	{
        List<GlossaryEntry> partyStatEntries = new List<GlossaryEntry>();

        partyStatEntries.Add(new StatGlossaryEntry(IconList.discountIconName, "Party Stat", HoverMessageList.discountMessage));
        partyStatEntries.Add(new StatGlossaryEntry(IconList.goldMultiplierIconName, "Party Stat", HoverMessageList.goldMultiplierMessage));
        partyStatEntries.Add(new StatGlossaryEntry(IconList.partyActionsIconName, "Party Stat", HoverMessageList.partyActionsMessage));
        partyStatEntries.Add(new StatGlossaryEntry(IconList.partySlotsIconName, "Party Stat", HoverMessageList.partySlotsMessage));
        partyStatEntries.Add(new StatGlossaryEntry(IconList.regenIconName, "Party Stat", HoverMessageList.regenMessage));
        partyStatEntries.Add(new StatGlossaryEntry(IconList.retreatChanceIconName, "Party Stat", HoverMessageList.retreatChanceMessage));
        partyStatEntries.Add(new StatGlossaryEntry(IconList.surpriseRoundAmountIconName, "Party Stat", HoverMessageList.surpriseRoundAmountMessage));
        partyStatEntries.Add(new StatGlossaryEntry(IconList.volleyIconName, "Party Stat", HoverMessageList.volleyAccuracyMessage));

		return partyStatEntries;
	}

    public static List<GlossaryEntry> getAllSkillGlossaryEntries()
	{
        List<GlossaryEntry> skillStatEntries = new List<GlossaryEntry>();

        skillStatEntries.Add(new WrittenGlossaryEntry("How to obtain Skills.", "Skill", "Skills are Actions that can be used outside of Combat to overcome certain obstacles or gain advantages against packs of Enemies. To be able to use a specific Skill, your Party must have a member with at least a 2 in that Skill's governing Primary Stat. If multiple Party Members have access to a Skill, only the Party Member with the highest governing Primary Stat will contribute when using that Skill."));
        skillStatEntries.Add(new StatGlossaryEntry(IconList.intimidateIconName, "Skill", HoverMessageList.intimidateMessage));
        skillStatEntries.Add(new StatGlossaryEntry(IconList.cunningIconName, "Skill", HoverMessageList.cunningMessage));
        skillStatEntries.Add(new StatGlossaryEntry(IconList.observationIconName, "Skill", HoverMessageList.observationMessage));
        skillStatEntries.Add(new StatGlossaryEntry(IconList.leadershipIconName, "Skill", HoverMessageList.leadershipMessage));

		return skillStatEntries;
	}

}
