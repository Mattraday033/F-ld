using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class DamageCalculator
{	
	public const int critAutoSuccessThreshold = 100;
	public const int critAutoFailureThreshold = 0;

	public const double baseCriticalDamage = 1.5;

	private const char strChar = 's';
	private const char dexChar = 'd';
	private const char wisChar = 'w';
	private const char chaChar = 'c';
	private const char plusChar = '+';
	private const char minusChar = '-';

	private static string[] allUpperCaseLetters = new string[]{"S", "D", "W", "C", ""};

    public static Stats noStatsSource
    {
        get;
        private set;
    }
    private static Stats currentStatSource;

    static DamageCalculator()
    {

    }

    public static string combineFormulas(string[] damageFormulas)
    {
        if (damageFormulas == null || damageFormulas.Length == 0)
        {
            return "0";
        }

        string mainFormula = damageFormulas[0];

        for (int index = 1; index < damageFormulas.Length; index++)
        {
            mainFormula = combineFormulas(mainFormula, damageFormulas[index]);
        }

        return mainFormula;
    }

	public static string combineFormulas(string formula1, string formula2)
	{
		string newFormula = "";

		if((formula1 == null || formula1.Length <= 0) && formula2 != null)
		{
			return formula2;
		} else if((formula2 == null || formula2.Length <= 0) && formula1 != null)
		{
			return formula1;
		} else if((formula2 == null || formula2.Length <= 0) && (formula1 == null || formula1.Length <= 0))
		{
			return "0";
		}

		formula1 = formula1.Replace(" ", "").ToLower();
		formula2 = formula2.Replace(" ", "").ToLower();

		string[] allSections = new string[]{"", "", "", "", ""};

		string[] formula1Sections = formula1.Split(plusChar);
		string[] formula2Sections = formula2.Split(plusChar);

		foreach(string formula1Section in formula1Sections)
		{
			// Debug.LogError("allSections: (" + string.Join("), (", allSections)+ ")");
			allSections = addSectionToFormula(allSections, formula1Section);
		}

		foreach (string formula2Section in formula2Sections)
		{
			// Debug.LogError("allSections: (" + string.Join("), (", allSections)+ ")");
			allSections = addSectionToFormula(allSections, formula2Section);
		}

			// Debug.LogError("allSections: (" + string.Join("), (", allSections)+ ")");


		allSections = addCharactersToFormulaAndRemoveDeadSections(allSections);

		// Debug.LogError("allSections after adding characters: (" + string.Join("), (", allSections)+ ")");

		if (allSections.Length == 0)
		{
			return "0";
		}

		newFormula = allSections[0];

		for(int index = 1; index < allSections.Length; index++)
		{
			newFormula += plusChar + allSections[index];
		}

		return newFormula;
	}

	private static string[] addCharactersToFormulaAndRemoveDeadSections(string[] allSections)
	{
		for(int index = 0; index < allSections.Length; index++)
		{
			if (allSections[index].Length > 0)
			{
				if (allSections[index].Equals("1"))
				{
					allSections[index] = allUpperCaseLetters[index];
				}
				else
				{
					allSections[index] += allUpperCaseLetters[index];
				}
			}
		}

		return allSections.Where(section => section.Length > 0).ToArray();
	}

	public static string[] addSectionToFormula(string[] formula, string section)
	{

		if (section.Length <= 0)
		{
			return formula;
		}

		// Debug.LogError("1Formula = (" + string.Join("), (", formula)+ ")");
		// Debug.LogError("1Section = " + section);

		char finalChar = section[section.Length - 1];
		int sectionIndex = 0;

		switch (finalChar)
		{
			case strChar:
				sectionIndex = 0;
				section = section.Replace(strChar + "", "");
				break;
			case dexChar:
				sectionIndex = 1;
				section = section.Replace(dexChar + "", "");
				break;
			case wisChar:
				sectionIndex = 2;
				section = section.Replace(wisChar + "", "");
				break;	
			case chaChar:
				sectionIndex = 3;
				section = section.Replace(chaChar + "", "");
				break;
			default:
				sectionIndex = 4;
				break;
		}

		if(section.Length <= 0)
		{
			section = "1";
		}

		if (formula[sectionIndex].Length <= 0)
		{
			formula[sectionIndex] = section;
		}
		else
		{
			formula[sectionIndex] = combineNumbers(formula[sectionIndex], section).Replace("+", "").Replace(" ", "");
		}

		// Debug.LogError("2Formula = (" + string.Join("), (", formula)+ ")");
		// Debug.LogError("2Section = " + section);

		return formula;
	}

    private static int calculateFormula(string damageFormula)
    {
        return calculateFormula(damageFormula, currentStatSource);
    }

    public static int calculateFormula(string damageFormula, Stats statSource)
    {
        if (damageFormula == null)
        {
            return -1;
        }

        currentStatSource = statSource;

        int output = 0;

        damageFormula = damageFormula.Replace(" ", "").ToLower();

        string[] formulaSections = damageFormula.Split(plusChar);

        foreach (string formulaSection in formulaSections)
        {
            output += calculateSection(formulaSection);
        }

        if (output <= 0)
        {
            return 0;
        }
        else
        {
            return output;
        }
    }
	
	public static int calculateBonusDamage(string damageFormula)
    {
		if(damageFormula == null)
		{
			return -1;
		}
		
		int output = 0;
		
		damageFormula = damageFormula.Replace(" ", "").ToLower();
		
		string[] formulaSections = damageFormula.Split(plusChar);
		
		foreach(string formulaSection in formulaSections)
		{
			output += calculateSection(formulaSection, true);
		}
		
		if(output <= 0)
		{
			return 0;
		} else 
		{
			return output;
		}
	}
	
	public static int calculateSection(string formulaSection)
	{
		return calculateSection(formulaSection, false);
	}
	
	public static int calculateSection(string formulaSection, bool onlyBonusDamage)
	{
		int output = 0;
		
		try
		{
			output = int.Parse(formulaSection);
			return output;
		} catch(FormatException e)
		{
					
			if(formulaSection.Contains(minusChar))
			{
				
				string[] formulaSections = formulaSection.Split(minusChar);
				
				output = calculateSection(formulaSections[0]);
				
				for(int i = 1; i < formulaSections.Length; i++)
				{
					output -= calculateSection(formulaSections[i]);
				}
				
				return output;
			}
			
			if(formulaSection.Contains(strChar))
			{
				if(onlyBonusDamage)
				{
					return 0;
				}
				
				if(formulaSection.Length == 1)
				{
					return currentStatSource.getStrength();
				} else
				{
					output = int.Parse(formulaSection.Split(strChar)[0]) * currentStatSource.getStrength();
				
					return output;
				}

				
			} else if(formulaSection.Contains(dexChar))
			{
				if(onlyBonusDamage)
				{
					return 0;
				}
				
				if(formulaSection.Length == 1)
				{
					return currentStatSource.getDexterity();
				} else
				{
					output = int.Parse(formulaSection.Split(dexChar)[0]) * currentStatSource.getDexterity();
				
					return output;
				}
			} else if(formulaSection.Contains(wisChar))
			{
				if(onlyBonusDamage)
				{
					return 0;
				}
				
				if(formulaSection.Length == 1)
				{
					return currentStatSource.getWisdom();
				} else
				{
					output = int.Parse(formulaSection.Split(wisChar)[0]) * currentStatSource.getWisdom();
				
					return output;
				}
			} else if(formulaSection.Contains(chaChar))
			{
				if(onlyBonusDamage)
				{
					return 0;
				}
				
				if(formulaSection.Length == 1)
				{
					return currentStatSource.getCharisma();
				} else
				{
					output = int.Parse(formulaSection.Split(chaChar)[0]) * currentStatSource.getCharisma();
				
					return output;
				}
			} 
			
			
		}
		
		return output;
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

				CombatAction fear = AbilityList.getAbility(AbilityList.fearName);

				Selector targetSelector = TraitList.chaotic.findTargetLocation(SelectorManager.getInstance().selectors[fear.getRangeIndex()].clone(), CombatGrid.getAllAliveEnemyCombatants());

				fear.setActorCoords(currentStatSource.position);
				fear.setSelector(targetSelector);
				//fear.setTargetCoords(targetSelector.getCoords());

				CombatActionManager.addCritCombatAction(fear);

				return;
        }
    }

    public static string combineNumbers(string firstNum, string secondNum)
	{
		return combineNumbers(new string[]{firstNum, secondNum});
	}
	
	public static string combineNumbers(string[] numbers)
	{
		int output = 0;

		foreach (string number in numbers)
		{
			if (number.Length <= 0)
			{
				continue;
			}

            // Debug.LogError("number = " + number);
            output += int.Parse(number.Replace("+",""));
		}

		// Debug.LogError("output = " + output);
		
		if (output >= 0)
		{
			return "+ " + output;
		}
		else
		{
			return "- " + Math.Abs(output);
		}
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
