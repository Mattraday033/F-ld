using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PartyStats
{

    #region Party Wide Secondary Stats

    public static float getPartyRegenAmount()
    {
        int totalWis = getTotalWisdom() / 2;
        int totalStr = getTotalStrength() / 2;

        int totalMedicinePoints = totalWis + totalStr;

        switch (totalMedicinePoints)
        {
            case < 5:
                return 0f;
            case < 10:
                return .02f;
            case < 15:
                return .04f;
            case < 20:
                return .06f;
            case < 25:
                return .08f;
            case < 30:
                return .10f;
            case < 35:
                return .12f;
            case < 45:
                return .14f;
            case < 50:
                return .16f;
            case < 55:
                return .18f;
            case >= 55:
                return .20f;
        }
    }

    public static string getPartyRegenAmountForDisplay()
    {
        float regenMult = getPartyRegenAmount();

        return regenMult * 100f + "%";
    }

    public static int getPartyMemberCombatActionSlots()
    {
        int highestLevel = getHighestLevel();
        int totalDex = getTotalDexterity() / 2;
        int totalCha = getTotalCharisma() / 2;

        int totalActionPoints = highestLevel + totalDex + totalCha;

        switch (totalActionPoints)
        {
            case < 10:
                return 2;
            case < 35:
                return 3;
            case < 55:
                return 4;
            case >= 55:
                return 5;
        }
    }

    public static int getPartySizeMaximum(StatsWrapper[] partyMemberStats)
    {
        return getPartySizeMaximum(new Formation(partyMemberStats));
    }

    public static int getPartySizeMaximum()
    {
        return getPartySizeMaximum(State.formation);
    }

    public static int getPartySizeMaximum(Formation formation)
    {
        int highestLevel = getHighestLevel();
        int totalWis = getTotalWisdom() / 2;
        int totalCha = getTotalCharisma() / 2;

        int totalSizePoints = highestLevel + totalWis + totalCha;

        switch (totalSizePoints)
        {
            case < 10:
                return 3;
            case < 35:
                return 4;
            case < 55:
                return 5;
            case >= 55:
                return 6;
        }
    }

    public static int getVolleyAccuracy()
    {
        int totalWis = getTotalWisdom() / 2;
        int totalCha = getTotalCharisma() / 2;

        return totalWis + totalCha;
    }

    public static int getPartySurpriseRounds()
    {
        int totalDex = getTotalDexterity();

        switch (totalDex)
        {
            case < 30:
                return 1;
            case < 50:
                return 2;
            case >= 50:
                return 3;
        }
    }

    public static int getRetreatChanceBonus()
    {
        int totalWis = getTotalWisdom() / 2;
        int totalDex = getTotalDexterity() / 2;

        return totalWis + totalDex;
    }

    public static float calculateDiscount()
    {
        float totalCha = getTotalCharisma();

        return totalCha * .01f;
    }

    public static float getDiscountMultiplier()
    {
        return 1f - calculateDiscount();
    }

    public static string getDiscountForDisplay()
    {
        return (int)(calculateDiscount() * 100f) + "%";
    }

    public static double getGoldMultiplier()
    {
        double goldMultiplier = 1.0;

        foreach (AllyStats ally in State.formation)
        {
            if (ally == null)
            {
                continue;
            }

            goldMultiplier += StatBoostManager.calculateAllStatFormulasAsPercentageDouble(ally, ally.getAllStatBoosts(), b => b.getBonusGoldMultiplierFormula());
        }

        return goldMultiplier;
    }

    public static string getGoldMultiplierForDisplay()
    {
        return "+" + Mathf.Round((float) ((getGoldMultiplier()-1.0) * 100.0)) + "%";
    }

    #endregion

    #region Exuberances
    public static bool partyHasAccessToExuberances()
    {
        foreach (Stats stats in State.formation)
        {
            if (stats != null && stats.getCharisma() >= 2)
            {
                return true;
            }
        }

        return false;
    }

    public static int getStartingRedKnife()
    {
        if (!partyHasAccessToExuberances())
        {
            return 0;
        }

        return Helpers.sum<CombatActionArray>(State.formation.getAllCombatActionArrays(), t => t.getTotalRedStacksAtStart());
    }

    public static int getStartingBlueShield()
    {
        if (!partyHasAccessToExuberances())
        {
            return 0;
        }

        return Helpers.sum<CombatActionArray>(State.formation.getAllCombatActionArrays(), t => t.getTotalBlueStacksAtStart());
    }


    public static int getStartingYellowThorn()
    {
        if (!partyHasAccessToExuberances())
        {
            return 0;
        }

        return Helpers.sum<CombatActionArray>(State.formation.getAllCombatActionArrays(), t => t.getTotalYellowStacksAtStart());
    }

    public static int getStartingGreenLeaf()
    {
        if (!partyHasAccessToExuberances())
        {
            return 0;
        }

        return Helpers.sum<CombatActionArray>(State.formation.getAllCombatActionArrays(), t => t.getTotalGreenStacksAtStart());
    }

    #endregion

    #region Total/Highest Stat 

    public static int getTotalStrength()
    {
        return State.formation.getTotalStrength();
    }

    public static int getTotalDexterity()
    {
        return State.formation.getTotalDexterity();
    }

    public static int getTotalWisdom()
    {
        return State.formation.getTotalWisdom();
    }

    public static int getTotalCharisma()
    {
        return State.formation.getTotalCharisma();
    }

    public static int getHighestLevel()
    {
        return State.formation.getHighestLevel();
    }

    public static bool partyMemberCanLevelUp()
    {
        List<PartyMember> partyMembers = PartyManager.getAllJoinablePartyMembers();

        foreach (PartyMember partyMember in partyMembers)
        {
            if (partyMember.stats.xp >= AllyStats.xpNeededToLevelUp)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region Skills

    public static int getMaxIntimidateCount()
    {
        int playerStrength = PartyManager.getPlayerStats().getStrength();

        if (playerStrength >= SkillManager.skillUnlockLevel)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public static int getMaxCunningCount()
    {
        int playerDexterity = PartyManager.getPlayerStats().getDexterity();
        int baseDexCharges;

        if (playerDexterity >= SkillManager.skillExtraordinaryLevel)
        {
            baseDexCharges = 4;
        }
        else if (playerDexterity >= SkillManager.skillImprovedLevel)
        {
            baseDexCharges = 3;
        }
        else if (playerDexterity >= SkillManager.skillUnlockLevel)
        {
            baseDexCharges = 2;
        }
        else
        {
            baseDexCharges = 0;
        }

        if (baseDexCharges == 0)
        {
            return baseDexCharges;
        }
        else
        {
            int bonusFormulas = StatBoostManager.calculateAllStatFormulas(PartyManager.getPlayerStats(), PartyManager.getPlayerStats().getAllStatBoosts(), b => b.getBonusCunningChargesFormula());

            return baseDexCharges + bonusFormulas;
        }
    }

    public static int getObservationLevel()
    {
        int playerWisdom = PartyManager.getPlayerStats().getWisdom();

        if (playerWisdom < 2)
        {
            return 0;
        }
        else
        {
            return playerWisdom + StatBoostManager.calculateAllStatFormulas(PartyManager.getPlayerStats(), PartyManager.getPlayerStats().getAllStatBoosts(), b => b.getBonusObservationLevelFormula());
        }
    }

    public static int getMaxPlacablePartyMembers()
    {
        int playerCharisma = PartyManager.getPlayerStats().getCharisma();
        int skillLevelFromCharisma;

        if (playerCharisma >= SkillManager.skillExtraordinaryLevel)
        {
            skillLevelFromCharisma = 3;
        }
        else if (playerCharisma >= SkillManager.skillImprovedLevel)
        {
            skillLevelFromCharisma = 2;
        }
        else if (playerCharisma >= SkillManager.skillUnlockLevel)
        {
            skillLevelFromCharisma = 1;
        }
        else
        {
            return 0;
        }

        return skillLevelFromCharisma;
    }

    #endregion

}
