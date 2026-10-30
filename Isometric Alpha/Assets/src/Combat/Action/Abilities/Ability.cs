using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Ability: CombatAction, IJSONConvertable
{
	//the save file key an ability's json is stored under. CombatAction.extractFromJson looks for it to tell abilities apart from item actions
	public const string sourceAbilityElementName = "sourceAbility";

	private int requiredStatLevel = 1;
	public string statKey = "";
	
	private string _Key;

	private string name;

	private string _UseDescription;

	private string _LoreDescription;

	private string damageFormula;
	
	private string critFormula;
	
	private SelectorTemplate rangeTemplate;
	
	private int _MaximumSlots;
	
	private int maximumCooldown;
	
	public EnemyStats spawnType;

    public bool targetsOnlyAllies;
    
    private bool _OnlyUsableDuringSurpriseRound; 
    public override bool onlyUsableDuringSurpriseRound
    {
        get => _OnlyUsableDuringSurpriseRound;
        set
        {
            _OnlyUsableDuringSurpriseRound = value;
        }
    }

    public override bool targetsAllySection => targetsOnlyAllies;

    public override bool meetsStatRequirement
    {
        get
        {
            if (statKey.Length <= 0 || getRequiredStatLevel() < 0)
            {
                return true;
            }

            int currentStat = 0;

            char statKeyChar = statKey[0];

            switch(statKeyChar)
            {
                case AbilityList.levelKeyChar:
                    currentStat = OverallUIManager.getCurrentPartyMember().level;
                    break;
                case AbilityList.strengthKeyChar:
                    currentStat = OverallUIManager.getCurrentPartyMember().totalStrength;
                    break;
                case AbilityList.dexterityKeyChar:
                    currentStat = OverallUIManager.getCurrentPartyMember().totalDexterity;
                    break;
                case AbilityList.wisdomKeyChar:
                    currentStat = OverallUIManager.getCurrentPartyMember().totalWisdom;
                    break;
                case AbilityList.charismaKeyChar:
                    currentStat = OverallUIManager.getCurrentPartyMember().totalCharisma;
                    break;
            }

            return getRequiredStatLevel() <= currentStat;
        }
    }

    //abilities don't come from an item
    public override Item sourceItem => null;

    public Ability(CombatActionSettings settings, Catalog<IQueuingUnit> queuingUnits = null, List<TargetingLogic> targetingLogics = null) :
    base(null, SelectorFactory.buildByTemplate(settings.targetParams.rangeTemplate), queuingUnits, targetingLogics)
    {
        applySettings(settings);
    }

	public override void applySettings(CombatActionSettings settings)
	{
		_Key = settings.key;

		name = settings.descriptionParams.name;
		_UseDescription = settings.descriptionParams.useDescription;
		_LoreDescription = settings.descriptionParams.loreDescription;
		iconName = settings.descriptionParams.iconName;

		damageFormula = settings.damageParams.damageFormula;
		critFormula = settings.damageParams.critFormula;
		cannotDealDamage = settings.damageParams.cannotDealDamage;

        rangeTemplate = settings.targetParams.rangeTemplate;

        _SelfTargeting = settings.targetParams.selfTargeting;
        targetsOnlyAllies = settings.targetParams.targetsOnlyAllies;

		_MaximumSlots = settings.frequencyParams.maximumSlots;
		maximumCooldown = settings.frequencyParams.maximumCooldown;
		_OnlyUsableDuringSurpriseRound = settings.frequencyParams.onlyUsableDuringSurpriseRound;

		for (int index = 0; index < settings.costParams.costTypes.Length && index < settings.costParams.actionCosts.Length; index++)
		{
			this[settings.costParams.costTypes[index]] = settings.costParams.actionCosts[index];
		}

        _EffectAnimationType = settings.animationParams.effectAnimationType;
        useSpecialAttackAnimation = settings.animationParams.useSpecialAttackAnimation;

        _AppliedTrait = settings.appliedTrait;

        base.applySettings(settings);
    }


	public override string displayName { get { return name; } }
	public override string uniqueName { get { return _Key; } }

	public override string key => _Key;

	public override string useDescription
	{
		get
		{
	        if(appliedTrait != null)
	        {
	            if(_UseDescription.Length <= 0)
	            {
	                return appliedTrait.getApplicationDescription();
	            } else
	            {
	                return _UseDescription + " " + appliedTrait.getApplicationDescription();
	            }
	        } else
	        {
			    return _UseDescription;
	        }
		}
	}
	
	public override string loreDescription
	{
		get
		{
			if(_LoreDescription.Length <= 0)
			{
				return "";
			}

			return "<i>" + _LoreDescription + "</i>";
		}
	}

	public override string getRangeTitle()
	{
		return rangeTemplate.ToFriendlyString();
	}

    //convertToJson is for save files, you will never need to save an actions coords so actor/target coords are not saved
    public override string convertToJson()
    {
        return "{\"" + sourceAbilityElementName + "\":\"" + key + "\"}";
    }

    public override EffectAnimationType effectAnimationType
    {
        get
        {
            if(_EffectAnimationType != EffectAnimationType.Default)
            {
                return _EffectAnimationType ;
            }

            if(healsTarget)
            {
                return EffectAnimationType.Healing;
            }

            if(appliedTrait != null && appliedTrait.isDebuff())
            {
                return EffectAnimationType.Negative;
            }

            if(appliedTrait != null && appliedTrait.isBuff())
            {
                return EffectAnimationType.Positive;
            }

            return EffectAnimationType.Slash;
        }
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

        return DamageCalculator.combineFormulas(gatherAllNonActionFormulas(a => a.getDamageFormula()), "+" + actorStats.bonusAbilityDamage);
    }

    protected override string gatherAllNonActionFormulas(FormulaDelegate<StatBoostSource> getFormula)
    {
        string allStats = getAllOfOneStatFormula<StatBoostSource>(getStatSource().allStatBoosts, t => getFormula(t));

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
	
	public override SelectorTemplate getRangeTemplate()
	{
		return rangeTemplate;
	}
	
	public override int maximumSlots => _MaximumSlots;
	
	public override string getDisplayType()
	{
		return AbilityList.abilityActionTypeName;
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
        _AppliedTrait = trait;
    }

    //IDescribable Methods

	public override void describeSelfRow(DescriptionPanel panel)
	{
		base.describeSelfRow(panel);
		
		panel.setObjectBeingDescribed(this);
		
        DescriptionPanel.setText(panel.nameText, displayName);

        DescriptionPanel.setText(panel.statText, "" + getRequiredStatLevelForDisplay());

        addSlotsTextToRow(panel);
	}
	
	public override bool ineligible()
	{
		//Debug.LogError("Checking if " + uniqueName + " is ineligible");

		if (CombatStateManager.inCombat)
		{
			if(actorStats.isStunned)
			{
                return true;
            } else
			{
                return false;
            }
		}
		else
		{
            return !meetsStatRequirement;
		}
	}

    public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> buildingBlocks = base.getDescriptionBuildingBlocks();

        if(getCostDescription().Length > 0)
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getCostDescription()));
        }

        if (loreDescription.Length > 0)
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(loreDescription));
        }

        return buildingBlocks;
    }

    public override List<IDescribable> getRelatedDescribables()
    {
        List<IDescribable> relatedDescribables = base.getRelatedDescribables();

        foreach(ActionCostType costType in actionCostTypes)
        {
            Trait costTrait = costType.getCostTrait();

            if(costTrait != null)
            {
                costTrait.setTraitHolder(getStatSource());
                relatedDescribables.Add(costTrait);
            }
        }

        return relatedDescribables;
    }

    public string getCostDescription()
    {
        return string.Join("\n", actionCostTypes.Select(costType => costType.getCostDescription(this[costType])));
    }

    //ISortable Methods

    public override int getLevel()
    {
		return requiredStatLevel;
    }
}
