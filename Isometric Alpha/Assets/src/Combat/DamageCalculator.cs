using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Formula
{
	public const char strChar = 'S';
	public const char dexChar = 'D';
	public const char wisChar = 'W';
	public const char chaChar = 'C';
	public const char plusChar = '+';
	public const char minusChar = '-';

    public Dictionary<PrimaryStat, int> formulaDict = new Dictionary<PrimaryStat, int>();

    public Formula(string formula)
    {
        formula = formula.ToUpper().Replace(" ","");

        if(formula.Length <= 0 ||
           formula.Equals(Constants.zeroBonus)||
           formula.Equals(Constants.zeroRating))
        {
            return;
        }

        List<string> sections = new List<string>();

        for(int i = 0; i < formula.Length; i++)
        {
            if(characterIsStatMarker(formula[i]) || 
                ((i+1) < formula.Length && characterIsSectionBreak(formula[i+1])))
            {
                sections.Add(formula.Substring(0, i+1));
                formula = formula.Substring(i+1, formula.Length-(i+1));
                i = -1;
            }
        }

        if(formula.Length > 0) //Capturing final portion if there is anything left
        {
            sections.Add(formula);
        }

        foreach(string section in sections)
        {
            if(section.Length <= 0)
            {
                continue;
            }

            PrimaryStat key = PrimaryStat.None;
            int value = 0;

            switch(section[section.Length-1])
            {
                case strChar:
                    key = PrimaryStat.Strength;
                    break;
                case dexChar:
                    key = PrimaryStat.Dexterity;
                    break;
                case wisChar:
                    key = PrimaryStat.Wisdom;
                    break;
                case chaChar:
                    key = PrimaryStat.Charisma;
                    break;
            }

            if(key == PrimaryStat.None)
            {
                try
                {
                    value = int.Parse(section);
                } catch(Exception e)
                {
                    Debug.LogError("Exception found: " + section);
                }
                
            } else if(section.Length == 1 || (section.Split(plusChar).Length > 1 && 
                        section.Split(plusChar)[Constants.indexOne].Length == 1))
            {
                value = 1;
            } else if(section.Split(minusChar).Length > 1 && 
                        section.Split(minusChar)[Constants.indexOne].Length == 1)
            {
                value = -1;
            } else
            {
                value = int.Parse(section.Substring(0, section.Length-1));
            }

            addSectionToDict(key, value);
        }
    }
    
    private void addSectionToDict(PrimaryStat key, int value)
    {
        if(!formulaDict.ContainsKey(key))
        {
            formulaDict.Add(key, value);
            return;
        }

        formulaDict[key] += value;
    }

    public void combine(Formula other)
    {
        foreach(KeyValuePair<PrimaryStat, int> kvp in other.formulaDict)
        {
            addSectionToDict(kvp.Key, kvp.Value);
        }
    }

    public string getFormula()
    {
        string output = Constants.emptyString;

        output += getSectionOfFormula(PrimaryStat.Strength, output.Length > 0);
        output += getSectionOfFormula(PrimaryStat.Dexterity, output.Length > 0);
        output += getSectionOfFormula(PrimaryStat.Wisdom, output.Length > 0);
        output += getSectionOfFormula(PrimaryStat.Charisma, output.Length > 0);
        output += getSectionOfFormula(PrimaryStat.None, output.Length > 0);

        if(output.Equals(Constants.emptyString))
        {
            output = Constants.zeroRating;
        }
        
        return output;
    }

    public string getFormulaInverted()
    {
        string output = Constants.emptyString;

        output += getSectionOfFormula(PrimaryStat.Strength, output.Length > 0);

        if(output.Length > 0 && !output.Contains(plusChar) && !output.Contains(minusChar))
        {
            output = minusChar + output;
        } else if(output.Contains(minusChar))
        {
            output = output.Replace(minusChar+"", "");
        }

        string dexSection = getSectionOfFormula(PrimaryStat.Dexterity, output.Length > 0);

        if((dexSection.Length > 0 && !dexSection.Contains(plusChar) && !dexSection.Contains(minusChar)) || dexSection.Contains(plusChar))
        {
            output += minusChar + dexSection.Replace(plusChar+"", "");
        } else if(dexSection.Contains(minusChar) && output.Length > 0)
        {
            output += plusChar + dexSection.Replace(minusChar+"", "");
        } else if(dexSection.Contains(minusChar) && output.Length <= 0)
        {
            output += dexSection.Replace(minusChar+"", "");
        }

        string wisSection = getSectionOfFormula(PrimaryStat.Wisdom, output.Length > 0);

        if(wisSection.Length > 0 && ((!wisSection.Contains(plusChar) && !wisSection.Contains(minusChar)) || wisSection.Contains(plusChar)))
        {
            output += minusChar + wisSection.Replace(plusChar+"", "");
        } else if(wisSection.Contains(minusChar) && output.Length > 0)
        {
            output += plusChar + wisSection.Replace(minusChar+"", "");
        } else if(wisSection.Contains(minusChar) && output.Length <= 0)
        {
            output += wisSection.Replace(minusChar+"", "");
        }

        string chaSection = getSectionOfFormula(PrimaryStat.Charisma, output.Length > 0);

        if(chaSection.Length > 0 && ((!chaSection.Contains(plusChar) && !chaSection.Contains(minusChar)) || chaSection.Contains(plusChar)))
        {
            output += minusChar + chaSection.Replace(plusChar+"", "");
        } else if(chaSection.Contains(minusChar) && output.Length > 0)
        {
            output += plusChar + chaSection.Replace(minusChar+"", "");
        } else if(chaSection.Contains(minusChar) && output.Length <= 0)
        {
            output += chaSection.Replace(minusChar+"", "");
        }

        string bonusSection = getSectionOfFormula(PrimaryStat.None, output.Length > 0);

        if(bonusSection.Length > 0 && ((!bonusSection.Contains(plusChar) && !bonusSection.Contains(minusChar)) || bonusSection.Contains(plusChar)))
        {
            output += minusChar + bonusSection.Replace(plusChar+"", "");
        } else if(bonusSection.Contains(minusChar) && output.Length > 0)
        {
            output += plusChar + bonusSection.Replace(minusChar+"", "");
        } else if(bonusSection.Contains(minusChar) && output.Length <= 0)
        {
            output += bonusSection.Replace(minusChar+"", "");
        }

        if(output.Equals(Constants.emptyString))
        {
            output = Constants.zeroRating;
        }

        return output;
    }

    private string getSectionOfFormula(PrimaryStat key, bool sectionAdded)
    {
        string output = Constants.emptyString;

        if(formulaDict.ContainsKey(key) && 
            formulaDict[key] != 0)
        {
            if(formulaDict[key] < 0)
            {
                // output += minusChar;
            } else if(sectionAdded && formulaDict[key] > 0)
            {
                output += plusChar;
            }

            if((formulaDict[key] != 1 && 
                formulaDict[key] != -1) || key == PrimaryStat.None)
            {
                output += formulaDict[key];
            }

            output += convertKeyToChar(key);
        }

        return output;
    }

    private string convertKeyToChar(PrimaryStat stat)
    {
        switch(stat)
        {
            case PrimaryStat.Strength:
                return strChar.ToString();
            case PrimaryStat.Dexterity:
                return dexChar.ToString();
            case PrimaryStat.Wisdom:
                return wisChar.ToString();
            case PrimaryStat.Charisma:
                return chaChar.ToString();
            default:
                return "";
        }
    }


    public int calculateFormula(Stats source)
    {
        int output = 0;

        if(source == null)
        {
            return output;
        }

        foreach(KeyValuePair<PrimaryStat, int> kvp in formulaDict)
        {
            switch(kvp.Key)
            {
                case PrimaryStat.Strength:
                    output += kvp.Value * source.getStrength();
                    break;
                case PrimaryStat.Dexterity:
                    output += kvp.Value * source.getDexterity();
                    break;
                case PrimaryStat.Wisdom:
                    output += kvp.Value * source.getWisdom();
                    break;
                case PrimaryStat.Charisma:
                    output += kvp.Value * source.getCharisma();
                    break;
                case PrimaryStat.None:
                    output += kvp.Value;
                    break;
            }
        }

        return output;
    }

    public int calculateBonusDamage()
    {
        if(!formulaDict.ContainsKey(PrimaryStat.None))
        {
            return 0;
        }

        return formulaDict[PrimaryStat.None];
    }

    public string multiplyFormula(int multiplier)
    {
        Dictionary<PrimaryStat, int> newFormulaDict = new Dictionary<PrimaryStat, int>();

        foreach(KeyValuePair<PrimaryStat, int> kvp in formulaDict)
        {
            newFormulaDict[kvp.Key] = formulaDict[kvp.Key] * multiplier;
        }

        formulaDict = newFormulaDict;

        return getFormula();
    }

    private static bool characterIsStatMarker(char character)
    {
        switch(character)
        {
            case strChar:
            case dexChar:
            case wisChar:
            case chaChar:
                return true;
            default:
                return false;
        }
    }

    private static bool characterIsSectionBreak(char character)
    {
        switch(character)
        {
            case plusChar:
            case minusChar:
                return true;
            default:
                return false;
        }
    }
}

public static class DamageCalculator
{	
	public const int critAutoSuccessThreshold = 100;
	public const int critAutoFailureThreshold = 0;

	public const double baseCriticalDamage = 1.5;

    public static Stats noStatsSource
    {
        get;
        private set;
    }
    private static Stats currentStatSource;

    static DamageCalculator()
    {

    }

	public static string combineFormulas(string f1, string f2)
	{
        Formula formula = new Formula(f1);
        
        formula.combine(new Formula(f2));

		return formula.getFormula();
	}

    private static int calculateFormula(string damageFormula)
    {
        return calculateFormula(damageFormula, currentStatSource);
    }

    public static int calculateFormula(string damageFormula, Stats statSource)
    {
        Formula formula = new Formula(damageFormula);

        return formula.calculateFormula(statSource);
    }
	
	public static int calculateBonusDamage(string damageFormula)
    {
        Formula formula = new Formula(damageFormula);

        return formula.calculateBonusDamage();
	}
	
	public static bool isACrit(string critFormula, string critKey)
	{
		bool isCrit = false;

		if(isGuaranteedCrit(critKey))
		{
            isCrit = true;
		} else
		{
            int critChance = calculateFormula(critFormula);

			if (critChance >= critAutoSuccessThreshold)
			{
				isCrit = true;
			}
			else if (critChance <= critAutoFailureThreshold)
			{
                isCrit = false;
			}
			else
			{
                int critRoll = UnityEngine.Random.Range(1, 100);

                if (critRoll <= critChance)
                {
                    isCrit = true;
                }
                else
                {
                    isCrit = false;
                }
            }
        }

		if(isCrit)
		{
			queueCritCombatAction(critKey);

        }

		return isCrit;
    }

    public static bool isGuaranteedCrit(string key)
    {
        bool critStatus = false;

        switch (key)
        {
            case AbilityList.waylayName:
                if (CombatStateManager.isPlayerSurpriseRound())
                {
                    critStatus = true;
                }
                break;
            default:
                critStatus = false;
                break;
        }

        return critStatus;
    }

	private static void queueCritCombatAction(string critKey)
	{
        switch (critKey)
        {
            case AbilityList.crippleName:

				CombatAction fear = AbilityList.getAbility(currentStatSource, AbilityList.fearName);

				Selector targetSelector = TraitList.chaotic.findTargetLocation(SelectorManager.getInstance().selectors[fear.getRangeIndex()].clone(), CombatGrid.getAllAliveEnemyCombatants());

				fear.setActorCoords(currentStatSource.position);
				fear.setSelector(targetSelector);
				//fear.setTargetCoords(targetSelector.getCoords());

				CombatActionManager.addCritCombatAction(fear);

				return;
        }
    }
	
    public static string invertFormula(string f1)
    {
        Formula formula = new Formula(f1);

        return formula.getFormulaInverted();
    }

    public static string multiplyFormula(string f1, int multiplier)
    {
        Formula formula = new Formula(f1);

        return formula.multiplyFormula(multiplier);
    }

	/* idea for universal findFinalDamage, may not use
    public static int[] findFinalDamage(Stats actor, Stats target, string damageFormula, bool isCrit)
    {
        if (actor == null)
        {
			throw new IOException("actor == null");
        } else if(target == null)
		{
            throw new IOException("target == null");
        }

		int baseDamage = DamageCalculator.calculateFormula(damageFormula);
		int damageAfterActorMods = actor.modifyOutgoingDamage(baseDamage);

		int damageAfterCritCalc;

        if (isCrit)
        {
            damageAfterCritCalc = (int)(damageAfterActorMods * actor.getCritDamageMultiplier());
            damageAfterCritCalc += (int)((float)target.getTotalHealth() * actor.getDevastatingCriticalPercentage()); //will return 0f if not a devastatingCritical
        } else
		{
			damageAfterCritCalc = damageAfterActorMods;
        }

		int damageAfterSurpriseModifier;

		if (CombatStateManager.isPlayerSurpriseRound() && actor == currentStatSource)
		{
            damageAfterSurpriseModifier = (int)((float)damageAfterCritCalc * actor.getSurpriseDamageMultiplier());
		} else
		{
			damageAfterSurpriseModifier = damageAfterCritCalc;
        }

		int finalDamage = target.modifyIncomingDamage(damageAfterSurpriseModifier);

        return new int[] { finalDamage };
    } */
}
