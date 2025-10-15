using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Dexterity
{
	public const string symbolChar = "D";

	public const int extraSurpriseRoundLevel = 10;

	public const int extraArmorMultiplier = 4;

    public const float surpriseDamMultCoefficient = .1f; 
    public const float surpriseDamMultBase = 1f; 

    public const int devastatingCriticalLevel = 3;

	public const int exitStrategy2RoundLevel = 3;
	public const int exitStrategy3RoundLevel = 10;

	public const int retreatChancePerDexterity = 5;
	public const float baseRetreatChance = .45f;

    // public static string[] getAllSecondaryStatsForDisplay()
    // {
    //     return getAllSecondaryStatsForDisplay(PartyManager.getPlayerStats().getDexterity());
    // }

	// public static string[] getAllSecondaryStatsForDisplay(int stat)
	// {
	// 	PlayerStats stats = new PlayerStats();

	// 	stats.dexterity = stat;

	// 	string[] displayStats = {  stats.getMaxCunningCount() + "",
	// 							   stats.getExtraArmorFromDexterity() + "",
	// 							   stats.getSurpriseDamageMultiplierForDisplay(),
	// 							   stats.getNumberOfSurpriseRounds() + ""
	// 							   };

	// 	return displayStats;
	// }

	public static void addExitStrategy(Stats target)
	{
        
        Debug.LogError("Dexterity.addExitStrategy() not implemented");
        return;

		if (!CombatStateManager.isPlayerSurpriseRound() || CombatStateManager.whoIsSurprised != SurpriseState.EnemySurprised)
        {
            return;
        }

        Debug.LogError("Dexterity.addExitStrategy(Stats) does not check all characters");

        if (PartyManager.getPlayerStats().getDexterity() >= Dexterity.exitStrategy3RoundLevel)
        {
            target.addTrait(TraitList.exitStrategy3Round);
        }
        else if (PartyManager.getPlayerStats().getDexterity() >= Dexterity.exitStrategy2RoundLevel)
        {
            target.addTrait(TraitList.exitStrategy2Round);
        }
	}

	public static string getDescription()
	{
		string startingDescription = "Dexterity is the Primary Stat of agility, stealth, and trickery. " +
									 "Dextrous characters stalk the shadows, patiently striking when their opponents are most vulnerable. " +
									 "Clever solutions and dirty tactics allow these pragmatists to succeed where other's falter, winning the day through wits over brawn.\n\n";

		string combatDescription = "Combat: Characters that rely on their Dexterity in combat are able to more easily ambush their opponents, executing abilities before their targets can even react. " +
									"Dexterity will also allow your attacks to critically hit more often, and many Dexterity abilities contribute to or additionally benefit from critical hits.\n\n";

		string dialogueDescription = "Dialogue: Dexterity can be used to deceive in many ways, dialogue included. " +
									 "Underhanded explanations, quick deliveries, and half-truths will allow a Dextrous character to fool their marks into providing aid; whether it would be in their best interest to do so or not. " +
									 "Dexterity has the least amount of dialogue opportunities of any stat.\n\n";

		string movementDescription = "Movement: Your Primary Stats can be used to open new paths unavailable to others. " +
									 "A Dextrous character can climb over objects, slip through cracks, pick locks, and slip by guards, to name but a few examples. " +
									 "Dexterity has the most movement opportunities of any stat.\n\n";

		string skillDescription = "Skill (Cunning): Distract enemies, allowing you to either ambush or avoid them at your discretion. " +
								  "Some objects in the world may also be activated by Cunning, opening up new areas or discoveries.";


		return startingDescription + combatDescription + dialogueDescription + movementDescription + skillDescription;
	}

	public static CombatAction[] getStartingActions()
	{
		return new CombatAction[] { new FistAttack(), AbilityList.getAbility("d-2-1"), null, null, null, null, null, null, AbilityList.getAbility("d-2-2"), null, null, null, };
	}

	public static int getPartyMemberDexterityAtLevel(string partyMemberName, int level)
	{
		switch (partyMemberName)
		{
			case NPCNameList.carter:
				return PrimaryStats.getStatAtLevelFastProgression(level);
			case NPCNameList.thatch:
			case NPCNameList.nandor:
				return 1;
			default:
				return 1;
		}
	}

}

/*
Secondary Stats: 
	Extra Surprise Round Damage
	Number of Surprise Rounds
	Defense
	Cunning Uses
	
Abilities / Toggles: 
	Passive: Devastating Criticals: Whenever the player deals a crit, the recipient of that crit takes (Dex/2)% of it's total health as damage as well. Extra Surprise Round Damage is not applied to this damage, but the percentage of health lost is doubled during the surprise round.
	Ability: Waylay: Can only be used in the surprise round. Guarenteed crit.
	
	Dex flavored Quest Reward/Item? Smoke Screen: Blanket the battleground in smoke, confusing and debilitating the enemy. No actions can be taken by the enemy for one turn.
	
*/