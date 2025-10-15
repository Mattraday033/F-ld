using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Wisdom
{
	public const string symbolChar = "W";

	public const int improvedStrikesLevel = 3;
	public const int greaterStrikesLevel = 6;
	public const int ruinousStrikesLevel = 10;

	public const int minorArmorPenetrationLevel = 2;
	public const int lesserArmorPenetrationLevel = 4;
	public const int improvedArmorPenetrationLevel = 6;
	public const int greaterArmorPenetrationLevel = 8;
	public const int majorArmorPenetrationLevel = 10;

	public const int oneRepositionLevel = 2;
	public const int twoRepositionLevel = 4;
	public const int threeRepositionLevel = 6;
	public const int fourRepositionLevel = 8;
	public const int fiveRepositionLevel = 10;

	public const int firstPassiveSlotUnlockLevel = 3;
	public const int secondPassiveSlotUnlockLevel = 6;
	public const int thirdPassiveSlotUnlockLevel = 9;

    public const int mentalResistBase = 10;
    public const double mentalResistBaseDouble = .1;
    public const int mentalResistPerWisdom = 2;
    public const double mentalResistPerWisdomDouble = .02;

    public const int maxNumberOfWeaponSlots = 3;
    public const int maximumPassiveSlots = 4;

    // public static string[] getAllSecondaryStatsForDisplay()
    // {
    //     return getAllSecondaryStatsForDisplay(State.playerStats.getWisdom());
    // }

    // public static string[] getAllSecondaryStatsForDisplay(int stat)
    // {
    // 	PlayerStats stats = new PlayerStats();

    // 	stats.wisdom = stat;

    // 	string[] displayStats = {stats.getArmorPenetrationForDisplay(),
    // 							 stats.getMentalResistanceForDisplay(),
    // 							 "" + stats.getMaximumRepositionsPerCombat(),
    // 							 stats.getRetreatChanceBonusForDisplay()};

    // 	return displayStats;
    // }

    public static string getDescription()
    {
        string startingDescription = "Wisdom is the Primary Stat of knowledge, curiousity, and introspection. " +
                                     "Wise characters have an inherent appetite for mystery which propels them to always be on the hunt for ways to better themselves. " +
                                     "Their tendancy for self betterment leads them to hone their bodies as well as the mind, keeping fit and ready for when conflict becomes inevitable. " +
                                     "Will you use your knowledge to better those around you? " +
                                     "Or will you teach the ignorant that they are right to fear the Wise.\n\n";

        string combatDescription = "Combat: Characters that rely on their Wisdom in combat are capable of maneuvering their allies into more advantageous positions, or choraling their enemies into easily handled pockets. " +
                                   "Wisdom contributes to armor penetration, circumventing your enemy's defenses. " +
                                   "Wise combatants are also able to disrupt their enemies with interruptions or stuns, preventing them from attacking.\n\n";

        string dialogueDescription = "Dialogue: Your hard-earned Wisdom allows for a clarity of speech and thought which others lack. " +
                                     "Put this to use by finding the perfect wording to turn the tide of an argument, or exposing the faulty logic of others. " +
                                     "Wisdom has an average number of dialogue opportunities.\n\n";

        string movementDescription = "Movement: Your Primary Stats can be used to open new paths unavailable to others. " +
                                     "A Wise character can find new routes by observing the terrain around them, finding hidden locations or overcoming obstacles designed to befuddle less astute adversaries. " +
                                     "Wisdom has a below average number of movement opportunies, but they will have greater rewards for the effort.\n\n";

        string skillDescription = "Skill (Observation): Use this skill to reveal hidden doors, breakable walls, and secrets others wish left alone.";


        return startingDescription + combatDescription + dialogueDescription + movementDescription + skillDescription;
    }

	public static CombatAction[] getStartingActions()
	{
		return new CombatAction[] { new FistAttack(), AbilityList.getAbility("w-2-2"), AbilityList.getAbility("w-2-2"), null, null, null, null, null, AbilityList.getAbility("w-2-1"), null, null, null, };
	}
	
	public static int getPartyMemberWisdomAtLevel(string partyMemberName, int level)
	{
		switch (partyMemberName)
		{
			case NPCNameList.carter:
				return 1;
			case NPCNameList.thatch:
				return PrimaryStats.getStatAtLevelSlowProgression(level);
			case NPCNameList.nandor:
				return PrimaryStats.getStatAtLevelFastProgression(level);
			default:
				return 1;
		}
	}
}

/*

Secondary Stats: 
	Repositions Per Battle
	Retreat Chance Bonus
	Mental Resistance Chance
	Armor Pen
	
Abilities / Toggles Ideas: 
	Ability: Take Cover! (The character and all party members take (45 + (2*Wisdom))% less damage this turn. Summons are not affected by this ability. Always goes first
	Ability: Divide and Conquer: Reposition the enemy forces how the player sees fit
	Ability: Protect Them! Down a party member (not a summon) to get a party member that is down back up, and then either give a buff to defense to all party members or heal them, or both
	Ability: Flurry of Feints: Decreases damage, reduces armor, and changes attack priority to random. Lasts forever because it changes attack priority?
	
Goals for Wisdom:

	Make the character feel smart/wise
	Make the character utilize their intellect both in and out of combat
	Wisdom contains the purview of tactics/strategy.
	Wisdom attacks are typically small in area (with the exception of higher levels of unarmed strikes) to illicit the tactical choice of who to hit with them.
	Wisdom and Charisma differ in their approach to followers in the following ways:
	Wisdom is more about directing your followers in new and better ways, where as Charisma is about making your followers want to follow you/work together.
	Wisdom gives your followers new actions, Charisma gives you more followers and lets more of them act per turn and makes the actions they already know better.
*/