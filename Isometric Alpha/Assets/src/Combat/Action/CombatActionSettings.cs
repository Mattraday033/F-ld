using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionParams
{
    public string name;
    public string useDescription;
    public string iconName;

    public static DescriptionParams build()
    {
        return build("", "", "");
    }

    public static DescriptionParams build(string name, string useDescription)
    {
        return build(name, useDescription, name);
    }

    public static DescriptionParams build(string name, string useDescription, string iconName)
    {
        DescriptionParams parameters = new DescriptionParams();

        parameters.name = name;
        parameters.useDescription = useDescription;
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

    public int rangeIndex;
    public bool selfTargeting;

    public static TargetParams build()
    {
        return build(Range.singleTargetIndex, notSelfTargeting);
    }

    public static TargetParams build(int rangeIndex)
    {
        return build(rangeIndex, notSelfTargeting);
    }

    public static TargetParams build(int rangeIndex, bool selfTargeting)
    {
        TargetParams parameters = new TargetParams();

        parameters.rangeIndex = rangeIndex;
        parameters.selfTargeting = selfTargeting;

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

public class CombatActionSettings
{
    public string key;

    public DescriptionParams descriptionParams;
    public DamageParams damageParams;
    public TargetParams targetParams;
    public FrequencyParams frequencyParams;
    public CostParams costParams;

    public Trait appliedTrait;

    public Trait[] relatedTraits;

    // 0 Param Builds (E.g. Some Activated Passives)

    public static CombatActionSettings build(string key)
    {
        return CombatActionSettings.build(key, DescriptionParams.build(), DamageParams.build(), TargetParams.build(), FrequencyParams.build(), CostParams.build(), null, null);
    }

    public static CombatActionSettings build(string key, Trait appliedTrait)
    {
        return CombatActionSettings.build(key, DescriptionParams.build(), DamageParams.build(), TargetParams.build(), FrequencyParams.build(), CostParams.build(), appliedTrait, null);
    }

    public static CombatActionSettings build(string key, Trait appliedTrait, Trait[] relatedTraits)
    {
        return CombatActionSettings.build(key, DescriptionParams.build(), DamageParams.build(), TargetParams.build(), FrequencyParams.build(), CostParams.build(), appliedTrait, null);
    }

    // 1 Param Builds
    public static CombatActionSettings build(DescriptionParams descriptionParams)
    {
        return CombatActionSettings.build(descriptionParams.name, descriptionParams, DamageParams.build(), TargetParams.build(), FrequencyParams.build(), CostParams.build(), null, null);
    }
    public static CombatActionSettings build(string key, DescriptionParams descriptionParams)
    {
        return CombatActionSettings.build(key, descriptionParams, DamageParams.build(), TargetParams.build(), FrequencyParams.build(), CostParams.build(), null, null);
    }

    public static CombatActionSettings build(DescriptionParams descriptionParams, Trait appliedTrait)
    {
        return CombatActionSettings.build(descriptionParams.name, descriptionParams, DamageParams.build(), TargetParams.build(), FrequencyParams.build(), CostParams.build(), appliedTrait, null);
    }

    public static CombatActionSettings build(string key, DescriptionParams descriptionParams, Trait appliedTrait)
    {
        return CombatActionSettings.build(key, descriptionParams, DamageParams.build(), TargetParams.build(), FrequencyParams.build(), CostParams.build(), appliedTrait, null);
    }

    public static CombatActionSettings build(string key, CostParams costParams, Trait appliedTrait)
    {
        return CombatActionSettings.build(key, DescriptionParams.build(), DamageParams.build(), TargetParams.build(), FrequencyParams.build(), costParams, appliedTrait, null);
    }

    // 2 Param Builds
    public static CombatActionSettings build(DescriptionParams descriptionParams, TargetParams targetParams)
    {
        return CombatActionSettings.build(descriptionParams.name, descriptionParams, DamageParams.build(), targetParams, FrequencyParams.build(), CostParams.build(), null, null);
    }

    public static CombatActionSettings build(DescriptionParams descriptionParams, DamageParams damageParams, Trait appliedTrait)
    {
        return CombatActionSettings.build(descriptionParams.name, descriptionParams, damageParams, TargetParams.build(), FrequencyParams.build(), CostParams.build(), appliedTrait, null);
    }

    public static CombatActionSettings build(DescriptionParams descriptionParams, DamageParams damageParams)
    {
        return CombatActionSettings.build(descriptionParams.name, descriptionParams, damageParams, TargetParams.build(), FrequencyParams.build(), CostParams.build(), null);
    }

    public static CombatActionSettings build(string key, DescriptionParams descriptionParams, DamageParams damageParams)
    {
        return CombatActionSettings.build(key, descriptionParams, damageParams, TargetParams.build(), FrequencyParams.build(), CostParams.build(), null, null);
    }

    public static CombatActionSettings build(DescriptionParams descriptionParams, TargetParams TargetParams, Trait appliedTrait)
    {
        return CombatActionSettings.build(descriptionParams.name, descriptionParams, DamageParams.build(), TargetParams, FrequencyParams.build(), CostParams.build(), appliedTrait, null);
    }

    public static CombatActionSettings build(string key, DescriptionParams descriptionParams, TargetParams targetParams, Trait appliedTrait)
    {
        return CombatActionSettings.build(key, descriptionParams, DamageParams.build(), targetParams, FrequencyParams.build(), CostParams.build(), appliedTrait, null);
    }

    public static CombatActionSettings build(string key, DescriptionParams descriptionParams, FrequencyParams frequencyParams)
    {
        return CombatActionSettings.build(key, descriptionParams, DamageParams.build(), TargetParams.build(), frequencyParams, CostParams.build(), null, null);
    }

    public static CombatActionSettings build(string key, DescriptionParams descriptionParams, FrequencyParams frequencyParams, Trait appliedTrait)
    {
        return CombatActionSettings.build(key, descriptionParams, DamageParams.build(), TargetParams.build(), frequencyParams, CostParams.build(), appliedTrait, null);
    }

    //3 Param Builds
    public static CombatActionSettings build(string key, DescriptionParams descriptionParams, DamageParams damageParams, FrequencyParams frequencyParams)
    {
        return CombatActionSettings.build(key, descriptionParams, damageParams, TargetParams.build(), frequencyParams, CostParams.build(), null, null);
    }

    public static CombatActionSettings build(DescriptionParams descriptionParams, DamageParams damageParams, FrequencyParams frequencyParams)
    {
        return CombatActionSettings.build(descriptionParams.name, descriptionParams, damageParams, TargetParams.build(), frequencyParams, CostParams.build(), null, null);
    }

    public static CombatActionSettings build(string key, DescriptionParams descriptionParams, DamageParams damageParams, TargetParams targetParams)
    {
        return CombatActionSettings.build(key, descriptionParams, damageParams, targetParams, FrequencyParams.build(), CostParams.build(), null, null);
    }

    public static CombatActionSettings build(DescriptionParams descriptionParams, DamageParams damageParams, TargetParams targetParams)
    {
        return CombatActionSettings.build(descriptionParams.name, descriptionParams, damageParams, targetParams, FrequencyParams.build(), CostParams.build(), null, null);
    }

    public static CombatActionSettings build(DescriptionParams descriptionParams, DamageParams damageParams, TargetParams targetParams, Trait appliedTrait)
    {
        return CombatActionSettings.build(descriptionParams.name, descriptionParams, damageParams, targetParams, FrequencyParams.build(), CostParams.build(), appliedTrait, null);
    }

    public static CombatActionSettings build(string key, DescriptionParams descriptionParams, DamageParams damageParams, FrequencyParams frequencyParams, Trait appliedTrait)
    {
        return CombatActionSettings.build(key, descriptionParams, damageParams, TargetParams.build(), frequencyParams, CostParams.build(), appliedTrait, null);
    }

    public static CombatActionSettings build(string key, DescriptionParams descriptionParams, TargetParams targetParams, FrequencyParams frequencyParams, Trait appliedTrait)
    {
        return CombatActionSettings.build(key, descriptionParams, DamageParams.build(), targetParams, frequencyParams, CostParams.build(), appliedTrait, null);
    }

    //4 Param Builds
    public static CombatActionSettings build(DescriptionParams descriptionParams, DamageParams damageParams, TargetParams targetParams, FrequencyParams frequencyParams)
    {
        return CombatActionSettings.build(descriptionParams.name, descriptionParams, damageParams, targetParams, frequencyParams, CostParams.build(), null, null);
    }

    public static CombatActionSettings build(string key, DescriptionParams descriptionParams, DamageParams damageParams, TargetParams targetParams, FrequencyParams frequencyParams)
    {
        return CombatActionSettings.build(key, descriptionParams, damageParams, targetParams, frequencyParams, CostParams.build(), null, null);
    }

    public static CombatActionSettings build(string key, DescriptionParams descriptionParams, DamageParams damageParams, TargetParams targetParams, FrequencyParams frequencyParams, Trait appliedTrait)
    {
        return CombatActionSettings.build(key, descriptionParams, damageParams, targetParams, frequencyParams, CostParams.build(), appliedTrait, null);
    }

    public static CombatActionSettings build(string key, DescriptionParams descriptionParams, DamageParams damageParams, FrequencyParams frequencyParams, CostParams costParams)
    {
        return CombatActionSettings.build(key, descriptionParams, damageParams, TargetParams.build(), frequencyParams, costParams, null, null);
    }

    public static CombatActionSettings build(string key, DescriptionParams descriptionParams, DamageParams damageParams, FrequencyParams frequencyParams, CostParams costParams, Trait appliedTrait)
    {
        return CombatActionSettings.build(key, descriptionParams, damageParams, TargetParams.build(), frequencyParams, costParams, appliedTrait, null);
    }

    public static CombatActionSettings build(string key, DescriptionParams descriptionParams, TargetParams targetParams, FrequencyParams frequencyParams, CostParams costParams, Trait appliedTrait)
    {
        return CombatActionSettings.build(key, descriptionParams, DamageParams.build(), targetParams, frequencyParams, costParams, appliedTrait, null);
    }

    //5 Param Builds
    public static CombatActionSettings build(string key, DescriptionParams descriptionParams, DamageParams damageParams, TargetParams targetParams, FrequencyParams frequencyParams, CostParams costParams)
    {
        return CombatActionSettings.build(key, descriptionParams, damageParams, targetParams, frequencyParams, costParams, null, null);
    }

    public static CombatActionSettings build(string key, DescriptionParams descriptionParams, DamageParams damageParams, TargetParams targetParams, FrequencyParams frequencyParams, CostParams costParams, Trait appliedTrait)
    {
        return CombatActionSettings.build(key, descriptionParams, damageParams, targetParams, frequencyParams, costParams, appliedTrait, null);
    }

    public static CombatActionSettings build(string key, DescriptionParams descriptionParams, DamageParams damageParams, TargetParams targetParams, FrequencyParams frequencyParams, CostParams costParams, Trait appliedTrait, Trait[] relatedTraits)
    {
        CombatActionSettings parameters = new CombatActionSettings();

        parameters.key = key;

        parameters.descriptionParams = descriptionParams;
        parameters.damageParams = damageParams;
        parameters.targetParams = targetParams;
        parameters.frequencyParams = frequencyParams;
        parameters.costParams = costParams;

        parameters.appliedTrait = appliedTrait;
        parameters.relatedTraits = relatedTraits;

        return parameters;
    }
}
