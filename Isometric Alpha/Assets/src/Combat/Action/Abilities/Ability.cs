using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Ability: CombatAction, IJSONConvertable
{
	private int requiredStatLevel = -1;
	private string statKey = "";
	
	private string key;
	
	private string name;
	
	private string iconName;
	
	private string useDescription;
	
	private string damageFormula;
	
	private string critFormula;
	
	private int rangeIndex;
	
	private int maximumSlots;
	
	private int maximumCooldown;
	
	public EnemyStats spawnType;

    public bool selfTargeting;
    
    private bool _OnlyUsableDuringSurpriseRound; 
    public override bool onlyUsableDuringSurpriseRound
    {
        get => _OnlyUsableDuringSurpriseRound;
        set
        {
            _OnlyUsableDuringSurpriseRound = value;
        }
    }

	private Trait appliedTrait;

	private ActionCostType[] costTypes = new ActionCostType[] { ActionCostType.None };
    private int[] actionCosts = new int[] { 0 };

    private CombatAnimationType animationType;
    private EffectAnimationType effectAnimationType;

    public Ability(CombatActionSettings settings) :
    base(null, null)
    {
        applySettings(settings);
    }

	public override void applySettings(CombatActionSettings settings)
	{
		key = settings.key;
		
		name = settings.descriptionParams.name;
		useDescription = settings.descriptionParams.useDescription;
		iconName = settings.descriptionParams.iconName;

		damageFormula = settings.damageParams.damageFormula;
		critFormula = settings.damageParams.critFormula;
		cannotDealDamage = settings.damageParams.cannotDealDamage;

        rangeIndex = settings.targetParams.rangeIndex;

        selfTargeting = settings.targetParams.selfTargeting;

		maximumSlots = settings.frequencyParams.maximumSlots;
		maximumCooldown = settings.frequencyParams.maximumCooldown;
		_OnlyUsableDuringSurpriseRound = settings.frequencyParams.onlyUsableDuringSurpriseRound;

		costTypes = settings.costParams.costTypes;
		actionCosts = settings.costParams.actionCosts;

        animationType = settings.animationParams.animationType;
        effectAnimationType = settings.animationParams.effectAnimationType;

        appliedTrait = settings.appliedTrait;
    }


	public override string getName()
	{
		return name;
	}
	
	public override string getIconName()
	{
		return iconName;
	}
	
	public override string getKey()
	{
		return key;
	}
	
	public override string getUseDescription()
	{
		return useDescription;
	}
	
	public override void setCooldownToMax()
	{
        setCooldownRemaining(getMaximumCooldown());
	}
	
	public override int getMaximumCooldown()
	{
		return maximumCooldown;
	}
	
	public override string getRangeTitle()
	{
		return Range.getRangeTitle(rangeIndex);
	}

    public override int[] getActionCosts()
    {
        return actionCosts;
    }

    public override ActionCostType[] getActionCostTypes()
    {
        return costTypes;
    }

    //convertToJson is for save files, you will never need to save an actions coords so actor/target coords are not saved
    public override string convertToJson()
    {
        return "{sourceAbility\":\"" + getKey() + "\",\"CombatActionSaveType\":\"" + getSaveType() + "}";
    }
	
    public override CombatAnimationType getCombatAnimationType()
    {
        return CombatAnimationType.Effect;
    }

    public override string getEffectAnimationType()
    {
        if(effectAnimationType != EffectAnimationType.Default)
        {
            return effectAnimationType.ToString();
        }

        if(healsTarget())
        {
            return EffectAnimationType.Healing.ToString();
        }

        if(getAppliedTrait() != null && getAppliedTrait().isDebuff())
        {
            return EffectAnimationType.Negative.ToString();
        }

        if(getAppliedTrait() != null && getAppliedTrait().isBuff())
        {
            return EffectAnimationType.Positive.ToString();
        }

        return EffectAnimationType.Slash.ToString();
    }

	public override string getDamageFormula()
	{
		if(cannotDealDamage)
		{
			return Constants.zeroRating;
		}

        return damageFormula;
	}
	
    public override string getFinalDamageFormula()
    {
		if(cannotDealDamage)
		{
			return Constants.zeroRating;
		}

        return DamageCalculator.combineFormulas(gatherAllNonActionFormulas(a => a.getDamageFormula()), "+" + getActorStats().getBonusAbilityDamage());
    }

    protected override string gatherAllNonActionFormulas(FormulaDelegate<StatBoostSource> getFormula)
    {
        string allStats = getAllOfOneStatFormula<StatBoostSource>(getStatSource().getAllStatBoosts(), t => getFormula(t));

        AllyStats statSource = getStatSource() as AllyStats;

        if(statSource !=  null)
        {
            string invertedOffHandWeaponFormula = DamageCalculator.invertFormula(getFormula(statSource.equippedItems.getOffHand()));

            allStats = DamageCalculator.combineFormulas(invertedOffHandWeaponFormula, allStats);
        }

        return DamageCalculator.combineFormulas(getFormula(this), allStats);
    }

	public override string getCritFormula()
	{
		if(cannotDealDamage)
		{
			return Constants.zeroRating;
		}

        return critFormula;
	}
	
	public override int getRangeIndex()
	{
		return rangeIndex;
	}
	
	public override int getMaximumSlots()
	{
		return maximumSlots;
	}
	
    public override void setActor(Stats actor)
    {
        base.setActor(actor);

        if(appliedTrait != null && actor != null)
        {
            appliedTrait.traitApplier = actor;
        }
    }

	public override Trait getAppliedTrait()
	{
		if (appliedTrait == null)
		{
			return null;
		}
		else
		{
			return appliedTrait.clone();	
		}
	}
	
	public override bool isSelfTargeting()
	{
		return selfTargeting;
	}
	
	public override string getDisplayType()
	{
		return "Ability";
	}
	
	public override int getSaveType()
	{
		return (int) CombatActionSaveType.Ability;
	}
	
	public override int getRequiredStatLevel()
	{
		//returns -1 if it doesn't require a stat level
		return requiredStatLevel;
	}
	
	public void setStatRequirements(string statKey)
	{
		this.statKey = statKey;
		this.requiredStatLevel = int.Parse(statKey.Split("-")[1]);
	}

    public virtual void setAppliedTrait(Trait trait)
    {
        appliedTrait = trait;
    }

    //IDescribable Methods

    public override GameObject getRowType(RowType rowType)
	{
		string rowTypeName = "";
		
		switch(rowType)
		{
			case RowType.Standard:
				rowTypeName = PrefabNames.actionRow;
				break;
			case RowType.AbilityEditor:
				rowTypeName = PrefabNames.actionEditorRow;
				break;
			default:
				return base.getRowType(rowType);
		}
		
		return Resources.Load<GameObject>(rowTypeName);
	}
	
	public override void describeSelfRow(DescriptionPanel panel)
	{
		base.describeSelfRow(panel);
		
		panel.setObjectBeingDescribed(this);
		
		if(panel.nameText != null && !(panel.nameText is null))
		{
			panel.nameText.text = getName();
		}
		
		if(panel.statText != null && !(panel.statText is null) && getRequiredStatLevel() >= 0)
		{
			panel.statText.text = "" + getRequiredStatLevel();
		}
	}
	
	public override bool ineligible()
	{
		//Debug.LogError("Checking if " + getName() + " is ineligible");

		if (CombatStateManager.inCombat)
		{
			if(getActorStats().isStunned())
			{
                return true;
            } else
			{
                return false;
            }
		}
		else
		{
			if (statKey.Length <= 0 || getRequiredStatLevel() < 0)
			{
				return false;
			}

			int currentStat = 0;

			if (statKey.Contains(AbilityList.strengthKeyChar))
			{
				currentStat = OverallUIManager.getCurrentPartyMember().getStrength();

			}
			else if (statKey.Contains(AbilityList.dexterityKeyChar))
			{
				currentStat = OverallUIManager.getCurrentPartyMember().getDexterity();

			}
			else if (statKey.Contains(AbilityList.wisdomKeyChar))
			{
				currentStat = OverallUIManager.getCurrentPartyMember().getWisdom();

			}
			else if (statKey.Contains(AbilityList.charismaKeyChar))
			{
				currentStat = OverallUIManager.getCurrentPartyMember().getCharisma();
			}
			else
			{
				throw new IOException("Unknown statKey: " + statKey);
			}

			if (getRequiredStatLevel() > currentStat)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

    //ISortable Methods

    public override int getLevel()
    {
		return requiredStatLevel;
    }
}
