using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionParams
{
    public string name;
    public string useDescription;
    public string loreDescription;
    public string iconName;

    public static DescriptionParams build()
    {
        return build("", "", "");
    }

    public static DescriptionParams build(string name, string useDescription = "", string loreDescription = "")
    {
        return build(name, iconName: name, useDescription: useDescription, loreDescription: loreDescription);
    }

    public static DescriptionParams build(string name, string iconName, string useDescription = "", string loreDescription = "")
    {
        DescriptionParams parameters = new DescriptionParams();

        parameters.name = name;
        parameters.useDescription = useDescription;
        parameters.loreDescription = loreDescription;
        parameters.iconName = iconName;

        return parameters;
    }
}

public class DamageParams
{
    private const bool dealsDamage = false;
    private const bool dealsNoDamage = true;

    public string damageFormula;
    public string critFormula;
    public bool cannotDealDamage;

    public static DamageParams build()
    {
        return build("0", "" + DamageCalculator.critAutoFailureThreshold, dealsNoDamage);
    }

    public static DamageParams build(string damageFormula)
    {
        return build(damageFormula, "" + DamageCalculator.critAutoFailureThreshold, dealsDamage);
    }

    public static DamageParams build(string damageFormula, string critFormula)
    {
        return build(damageFormula, critFormula, dealsDamage);
    }

    public static DamageParams build(string damageFormula, string critFormula, bool cannotDealDamage)
    {
        DamageParams parameters = new DamageParams();

        parameters.damageFormula = damageFormula;
        parameters.critFormula = critFormula;
        parameters.cannotDealDamage = cannotDealDamage;

        return parameters;
    }
}

public class TargetParams
{
    private const bool notSelfTargeting = false;

    public SelectorTemplate rangeTemplate;
    public bool selfTargeting;
    public bool targetsOnlyAllies;

    public static TargetParams build()
    {
        return build(SelectorTemplate.Single, notSelfTargeting);
    }

    public static TargetParams build(SelectorTemplate rangeTemplate, bool selfTargeting = false, bool targetsOnlyAllies = false)
    {
        TargetParams parameters = new TargetParams();

        parameters.rangeTemplate = rangeTemplate;
        parameters.selfTargeting = selfTargeting;
        parameters.targetsOnlyAllies = targetsOnlyAllies;

        return parameters;
    }
}

public class FrequencyParams
{
    public const bool usableOutsideSurpriseRound = false;

    public int maximumSlots;
    public int maximumCooldown;
    public bool onlyUsableDuringSurpriseRound;

    public static FrequencyParams build()
    {
        return build(1, 1);
    }

    public static FrequencyParams build(int maximumSlots, int maximumCooldown)
    {
        return build(maximumSlots,maximumCooldown, usableOutsideSurpriseRound);
    }

    public static FrequencyParams build(int maximumSlots, int maximumCooldown, bool onlyUsableDuringSurpriseRound)
    {
        FrequencyParams parameters = new FrequencyParams();

        parameters.maximumSlots = maximumSlots;
        parameters.maximumCooldown = maximumCooldown;
        parameters.onlyUsableDuringSurpriseRound = onlyUsableDuringSurpriseRound;

        return parameters;
    }
}

public class CostParams
{
    public int[] actionCosts;
    public ActionCostType[] costTypes;

    public static CostParams build()
    {
        return build(ActionCostType.None, 0);
    }

    public static CostParams build(ActionCostType costType)
    {
        return build(new ActionCostType[] { costType }, new int[] { 0 });
    }

    public static CostParams build(int actionCost, ActionCostType costType)
    {
        CostParams parameters = new CostParams();

        parameters.costTypes = new ActionCostType[] { costType };
        parameters.actionCosts = new int[] { actionCost };

        return parameters;
    }

    public static CostParams build(ActionCostType costType, int actionCost)
    {
         return build(actionCost, costType);
    }

    public static CostParams build(int[] actionCosts, ActionCostType[] costTypes)
    {
        return build(costTypes, actionCosts);
    }

    public static CostParams build(ActionCostType[] costTypes, int[] actionCosts)
    {
        CostParams parameters = new CostParams();

        parameters.costTypes = costTypes;
        parameters.actionCosts = actionCosts;

        return parameters;
    }
}

public class AnimationParams
{
    public const bool useSpecialAttack = true;

    public CombatAnimationType animationType;
    public EffectAnimationType effectAnimationType;
    public bool useSpecialAttackAnimation = false;


    public static AnimationParams build()
    {
        return build(CombatAnimationType.Effect);
    }

    public static AnimationParams build(bool useSpecialAttackAnimation)
    {
        AnimationParams animationParams = build();

        animationParams.useSpecialAttackAnimation = useSpecialAttackAnimation;

        return animationParams;
    }

    public static AnimationParams build(CombatAnimationType animationType, EffectAnimationType effectAnimationType = EffectAnimationType.Default)
    {
        AnimationParams animationParams = new AnimationParams();

        animationParams.animationType = animationType;
        animationParams.effectAnimationType = effectAnimationType;

        return animationParams;
    }

    public static AnimationParams build(EffectAnimationType effectAnimationType, bool useSpecialAttackAnimation = false)
    {
        AnimationParams animationParams = new AnimationParams();

        animationParams.animationType = CombatAnimationType.Effect;
        animationParams.effectAnimationType = effectAnimationType;
        animationParams.useSpecialAttackAnimation = useSpecialAttackAnimation;

        return animationParams;
    }
}

public class CombatActionSettings
{
    public string key;

    public DescriptionParams descriptionParams;
    public DamageParams damageParams;
    public TargetParams targetParams;
    public FrequencyParams frequencyParams;
    public CostParams costParams;
    public AnimationParams animationParams;

    public Trait appliedTrait;

    public Trait[] relatedTraits;

    public static CombatActionSettings build(DescriptionParams descriptionParams, 
                                                DamageParams damageParams = null, 
                                                TargetParams targetParams = null, 
                                                FrequencyParams frequencyParams = null, 
                                                CostParams costParams = null, 
                                                AnimationParams animationParams = null, 
                                                Trait appliedTrait = null, 
                                                Trait[] relatedTraits = null)
    {
        return build(descriptionParams.name,
                        descriptionParams,
                        damageParams,
                        targetParams,
                        frequencyParams,
                        costParams,
                        animationParams,
                        appliedTrait,
                        relatedTraits);
    }

    public static CombatActionSettings build(string key, 
                                                DescriptionParams descriptionParams = null, 
                                                DamageParams damageParams = null, 
                                                TargetParams targetParams = null, 
                                                FrequencyParams frequencyParams = null, 
                                                CostParams costParams = null, 
                                                AnimationParams animationParams = null, 
                                                Trait appliedTrait = null, 
                                                Trait[] relatedTraits = null)
    {
        if(descriptionParams == null) { descriptionParams = DescriptionParams.build(); }

        if(damageParams == null) { damageParams = DamageParams.build(); }

        if(targetParams == null) { targetParams = TargetParams.build(); }

        if(frequencyParams == null) { frequencyParams = FrequencyParams.build(); }

        if(costParams == null) { costParams = CostParams.build(); }

        if(animationParams == null) { animationParams = AnimationParams.build(); }

        CombatActionSettings parameters = new CombatActionSettings();

        parameters.key = key;

        parameters.descriptionParams = descriptionParams;
        parameters.damageParams = damageParams;
        parameters.targetParams = targetParams;
        parameters.frequencyParams = frequencyParams;
        parameters.costParams = costParams;
        parameters.animationParams = animationParams;

        parameters.appliedTrait = appliedTrait;
        parameters.relatedTraits = relatedTraits;

        return parameters;
    }
}
