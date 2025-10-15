using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Strength
{
	public const string symbolChar = "S";

	public const int minorRegenerationLevel = 2;
	public const int majorRegenerationLevel = 7;

	public const int healthPerStrength = 10;
    public const int critDamMultPerStrength = 5;
    public const double critDamMultPerStrengthDouble = .05;
    public const int physResistBase = 10;
    public const double physResistBaseDouble = .1;
    public const int physResistPerStrength = 2;
    public const double physResistPerStrengthDouble = .02;

	public static int getHealthFromStrength(int strength)
    {
        return strength * healthPerStrength;
    }

    public static int getCurrentRegenerationAmount(AllyStats stats)
    {
        int amountToHeal = (int)(stats.getTotalHealth() * PartyStats.getPartyRegenAmount()) + StatBoostManager.calculateAllStatFormulas(stats, stats.getAllStatBoosts(), b => b.getBonusStrengthFormula());

        int missingHealth = stats.getMissingHealth();

        if (missingHealth < amountToHeal)
        {
            return missingHealth;
        }

        return amountToHeal;
    }

    public static void applyRegeneration(AllyStats targetStats)
    {
        if (targetStats == null || !State.formation.contains(targetStats))
        {
            return;
        }

        int amountToHeal = getCurrentRegenerationAmount(targetStats);
        bool isHealing = true;

        targetStats.modifyCurrentHealth(amountToHeal, isHealing);
    }

	// public static string[] getAllSecondaryStatsForDisplay()
	// {
	// 	return getAllSecondaryStatsForDisplay(State.playerStats.getStrength());
	// }

	// public static string[] getAllSecondaryStatsForDisplay(int stat)
	// {
	// 	PlayerStats stats = new PlayerStats();

	// 	stats.strength = stat;

	// 	string[] displayStats = {""+stats.getExtraCritDamageForDisplay(),
	// 							 ""+getHealthFromStrength(stat),
	// 							 ""+stats.getPhysicalResistanceForDisplay(),
	// 							 ""+stats.getWeaponSlots()};

	// 	return displayStats;
	// }

	public static string getDescription()
	{
		string startingDescription = "Strength is the Primary Stat of physical prowess, furiosity, and intimidation. " +
									 "Strong characters can take powerful attacks on the chin and then hit back harder. " +
									 "They tend to throw their weight around, using threats both implicit and explicit to get their way. " +
									 "And if things turn violent anyways? Perhaps that is what they wanted all along.\n\n";

		string combatDescription = "Combat: Characters that rely on their Strength in combat have access to more weapons than other characters, and can hit more enemies at a time. " +
								   "They also have more health and heal after every combat, allowing them to rarely need to avoid battle. " +
								   "Strength also improves the damage caused by critical hits, allowing strong characters to deliver truly devastating attacks.\n\n";

		string dialogueDescription = "Dialogue: Those that train their Strength will find opportunities to use it in dialogue. " +
									 "Threats are good motivators, and can convince others to help you or even force an enemy to surrender before weapons are ever drawn. " +
									 "Strength has an average amount of dialogue opportunities.\n\n";

		string movementDescription = "Movement: Your Primary Stats can be used to open new paths unavailable to others. " +
									 "In the case of Strength, use your powerful muscles to clear obstacles, lift gates, force doors, and climb walls, among other uses. " +
									 "Strength has an average number of movement opportunities.\n\n";

		string skillDescription = "Skill (Intimidate): Challenge enemies to combat, alerting them to your presence but preventing them from ambushing you in turn.";


		return startingDescription + combatDescription + dialogueDescription + movementDescription + skillDescription;
	}

	public static CombatAction[] getStartingActions()
	{
		return new CombatAction[] { new FistAttack(), AbilityList.getAbility("s-2-1"), AbilityList.getAbility("s-2-1"), null, null, null, null, null, null, null, null, null };
	}

	public static int getPartyMemberStrengthAtLevel(string partyMemberName, int level)
	{
		switch (partyMemberName)
		{
			case NPCNameList.thatch:
				return PrimaryStats.getStatAtLevelFastProgression(level);
			case NPCNameList.carter:
				return PrimaryStats.getStatAtLevelSlowProgression(level);
			case NPCNameList.nandor:
				return 1;
			default:
				return 1;
		}
	}
}
/*
Secondary Stats:

	Health
	Weapon Slots
	Extra Crit Damage
	Physical Resistance Chance

Abilities / Toggles: 
	Toggle: Intimidating Presence (All enemy attack patterns must include the target, even if they wouldn't otherwise)
*/