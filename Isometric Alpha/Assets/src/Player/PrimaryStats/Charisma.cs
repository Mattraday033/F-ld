using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Charisma
{
	public const string symbolChar = "C";

    public const int playerSynergyModifierCoefficient = 2;

    public const int partyMemberSlotPlusOneLevel = 2;
	public const int partyMemberSlotPlusTwoLevel = 4;
	public const int partyMemberSlotPlusThreeLevel = 6;
	public const int partyMemberSlotPlusFourLevel = 8;
	public const int partyMemberSlotNoLimitLevel = 10;

	public const int partyMemberCombatActionPlusOneLevel = 3;
	public const int partyMemberCombatActionPlusTwoLevel = 5;
	public const int partyMemberCombatActionPlusThreeLevel = 7;
	public const int partyMemberCombatActionPlusFourLevel = 9;

	public const int charismaEnergiesActivatedPassiveLevel = 2;

	// public static string[] getAllSecondaryStatsForDisplay(AllyStats targetStats)
	// {
	// 	return getAllSecondaryStatsForDisplay(targetStats.getCharisma());
	// }

	// public static string[] getAllSecondaryStatsForDisplay(int stat)
	// {
	// 	PlayerStats stats = new PlayerStats();

	// 	stats.charisma = stat;

	// 	string[] displayStats = {""+stats.getPartyMemberCombatActionSlots(),
	// 							 ""+stats.getMaximumPartyMemberSlots(),
	// 							 stats.getDiscountForDisplay()};

	// 	return displayStats;
	// }

    public static string getDescription()
    {
        string startingDescription = "Charisma is the Primary Stat of oration, barter, and leadership. " +
                                     "Charismatic characters are recognized by their peers as standing above the rest, and attract followers to their cause more easily. " +
                                     "Use your words to enhance others or degrade them, to befriend or belittle; calm your enemies' passions, or provoke them to folly.\n\n";

        string combatDescription = "Combat: Characters that rely on their Charisma in combat can use their natural leadership skills to make the most of their followers. " +
                                   "Followers deal more damage with higher Charisma, and Charisma allows you to coordinate more allies in combat as well as have them take more actions. " +
                                   "True leaders exude a magnetic energy that your followers will pick up on, called Exuberances. " +
                                   "Earn these energies by performing, or having your followers perform, specific actions and spend them to achieve great feats in battle.\n\n";

        string dialogueDescription = "Dialogue: Your obvious Charisma allows you to persuade others to help you or hinder your enemies, willingly. " +
                                     "Charismatic characters also gain a discount at shops. " +
                                     "Charisma has the most dialogue opportunities of any stat. \n\n";

        string movementDescription = "Movement: Your Primary Stats can be used to open new paths unavailable to others. " +
                                     "A Charismatic character can use their powerful methods of elocution to talk their way past those that block their way, or direct their followers to help remove obstacles. " +
                                     "Charisma has the least movement opportunities of any stat. \n\n";

        string skillDescription = "Skill (Leadership): Use this skill to give orders to your followers, using them to help with obstacles that would impede a solitary traveler.";


        return startingDescription + combatDescription + dialogueDescription + movementDescription + skillDescription;
    }

	public static CombatAction[] getStartingActions()
	{
		return new CombatAction[] { new FistAttack(), AbilityList.getAbility("c-2-3"), AbilityList.getAbility("c-2-3"), null, null, null, null, null, AbilityList.getAbility("c-2-2"), null, null, null, };
	}
	
	public static int getPartyMemberCharismaAtLevel(string partyMemberName, int level)
	{
		switch (partyMemberName)
		{
			case NPCNameList.carter:
			case NPCNameList.thatch:
				return 1;
			case NPCNameList.nandor:
				return PrimaryStats.getStatAtLevelSlowProgression(level);
			default:
				return 1;
		}
	}
}
/*
	Party Member Limit
	Party Member CombatAction Number
	Sales Price
	
	Abilities / Toggles: 
		Ability: Focus Fire: All friendly actors that damage this opponent deal an additional 40 + (2*Cha)% damage. Give early (lvl 2-3)
		Passive: Fight For the Fallen: Whenever a friendly actor dies, All other friendly actors heal (Cha/2)% health
		Ability: Reposition (reassign Squares for all party members during combat)
*/