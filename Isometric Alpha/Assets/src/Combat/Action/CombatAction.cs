using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Text.RegularExpressions;

public enum CombatActionState { NoAction, Calculation, Application, Animation }

//a single thing that the player has selected themself or a party member to do during combat
//Or a single thing the enemy has elected to do during their turn, typically based on logic explained in their trait descriptions
[System.Serializable]
public abstract class CombatAction : StatBoostSource, ICloneable, IJSONConvertable, IDescribable, ISortable, IDescribableInBlocks
{

    #region Constants

    private const string noSlotsRequiredMessage = "No Slots Required";
    private const string noCooldownMessage = "No Cooldown";
    private const string harmlessMessage = "Harmless";
    private const string cannotCritMessage = "Cannot Crit";
    protected const int noDamage = -1;
    protected const int singleExuberanceStack = 1;

    #endregion

    #region Global Variables

    public virtual bool onlyUsableDuringSurpriseRound
    {
        get => false;
        set { }
    }

    public List<GridCoords> previousTargets = new List<GridCoords>();

    private Stats previewActor;
    public bool inPreviewMode { get; private set; } = false;

    public bool cannotDealDamage;
    public bool useSpecialAttackAnimation = false;

    #region Stacks At Start

    public virtual int redStacksAtStart => 0;
    public virtual int blueStacksAtStart => 0;
    public virtual int yellowStacksAtStart => 0;
    public virtual int greenStacksAtStart => 0;

    #endregion

    #endregion

    #region Boolean Properties

    public virtual bool secondaryCoordsRequiresEmptySpace => false;
    public virtual bool requiresSecondaryCoords => false;
    public virtual bool requiresTertiaryCoords => false;
    public virtual bool resetCoordsWhenChoosingTertiary => false; // if true, when the player backs out of choosing the tertiary target, then the previous selector will snap to it's default position
    public virtual bool resetCoordsOnBackOutOfTertiary => false; // if true, when the player backs out of choosing the tertiary target, then the previous selector will snap to it's default position
    public virtual bool targetsAllySection => false;
    public virtual bool targetsOnlyEmptySpace => false;
    public virtual bool usableWithoutItemsInInventory => true;

    public bool actorIsPriorityAttacker
    {
        get
        {
            if (actorCoords.Equals(GridCoords.getDefaultCoords()) || actorStats == null)
            {
                return false;
            }

            return actorStats.isPriorityAttacker;
        }
    }

    public bool actorIsLowPriorityAttacker
    {
        get
        {
            if (actorCoords.Equals(GridCoords.getDefaultCoords()) || actorStats == null)
            {
                return false;
            }

            return actorStats.isPriorityAttacker;
        }
    }

    public virtual bool hasRepetitionsRemaining => false;
    public virtual bool takesAWeaponSlot => false;
    public virtual bool requiresAnAction => true;
    public virtual bool unactivatable => false;
    public virtual bool canBePlacedInActionSlot => true;

    public virtual bool autoApply => false;
    public virtual bool healsTarget => false;
    public virtual bool targetMustBeDead => false;
    public virtual bool repositionsCaster => false;
    public virtual bool killsCaster => false;
    public virtual bool canBePlacedInPassiveSlot => false;
    public virtual bool needsItemQuantityPanel => false;
    public virtual bool movesTarget => false;
    public virtual bool multiActorAction => false;
    public virtual bool meetsStatRequirement => true;

    #endregion

    #region Actor Stats

    private Stats _ActorStats;
    public virtual Stats actorStats
    {
        get
        {
            if(hasAssignedActor(out Stats actor))
            {
                return actor;
            }

            return null;
        }
        set
        {
            _ActorStats = value;

            if(_ActorStats?.usePlayerTargeting ?? false)
            {
                _Selector = null;
            }
        }
    }

    public bool actorIsAlly => actorStats.positions.Any(p => CombatGrid.positionIsOnAlliedSide(p));
    public virtual GridCoords actorCoords => actorStats.positions.Count > 0 ? actorStats.positions[0].clone() : GridCoords.getDefaultCoords();

    public bool actorIsSlowed
    {
        get
        {
            if (actorStats == null)
            {
                return false;
            }
            else
            {
                return actorStats.isSlowed;
            }
        }
    }

    public virtual bool hasAssignedActor(out Stats currentActor)
    {
        if (inPreviewMode)
        {
            currentActor = previewActor;
        }
        else if (_ActorStats != null)
        {
            currentActor = _ActorStats;
        }
        else if (!CombatStateManager.inCombat)
        {
            currentActor = OverallUIManager.getCurrentPartyMember();
        } else
        {
            currentActor = null;
        }

        return currentActor != null;
    }

    #endregion

    #region Selector

    protected bool _SelfTargeting;
    public virtual bool selfTargeting => _SelfTargeting;

    protected Selector _Selector;
    public virtual Selector selector
    {
        get
        {
            if (selfTargeting)
            {
                _Selector = SelectorFactory.buildByTemplate(getRangeTemplate());
                _Selector.setToLocation(actorCoords);
                return _Selector;
            }

            //the actorStats setter clears the selector for player-targeted actors, so build a fresh one on demand
            if(actorStats?.usePlayerTargeting ?? false)
            {
                return _Selector ??= SelectorFactory.buildByTemplate(getRangeTemplate());
            } else if(_Selector != null)
            {
                return _Selector;
            }

            List<Stats> listOfTargets;

            if (actorStats?.shouldTargetEnemy ?? false)
            {
                listOfTargets = CombatGrid.getAllAliveEnemyCombatants();
            }
            else
            {
                listOfTargets = CombatGrid.getAllAliveAllyCombatants();
            }

            return actorStats?.traitContainer.findTargetLocation(SelectorFactory.buildByTemplate(getRangeTemplate()), listOfTargets);
        }
        set
        {
            _Selector = value;
        }
    }

    public void addPreviousTarget(GridCoords coords)
    {
        previousTargets.Add(coords);
    }

    public GridCoords getPreviousTarget()
    {
        GridCoords previousTarget = previousTargets[previousTargets.Count - 1];

        previousTargets.RemoveAt(previousTargets.Count - 1);

        return previousTarget;
    }

    public virtual GridCoords getSecondaryCoords()
    {
        throw new IOException("Base version of getSecondaryCoords() was called erroneously");
    }

    public virtual void setSecondaryCoords(GridCoords coords)
    {
        throw new IOException("Base version of setSecondaryCoords() was called erroneously");
    }

    public virtual bool tertiaryCoordsRequiresEmptySpace()
    {
        throw new IOException("The base class version of tertiaryCoordsRequiresEmptySpace() was called extraneously");
    }

    public virtual void setTertiaryCoords(GridCoords coords)
    {
        throw new IOException("The base class version of setTertiaryCoords() was called extraneously");
    }

    public virtual SelectorTemplate getRangeTemplate()
    {
        return SelectorTemplate.Single;
    }

	public virtual string getRangeTitle()
	{
		return getRangeTemplate().ToFriendlyString();
	}

    public virtual GridCoords getTargetCoords()
    {
        if (selector != null)
        {
            return selector.getCoords();
        }
        else
        {
            return GridCoords.getDefaultCoords();
        }
    }

    public virtual Vector3 getTertiaryPosition()
    {
        throw new IOException("Called base version of getTertiaryPosition");
    }

    public virtual Selector getTertiarySelector()
    {
        Selector tertiarySelector = SelectorFactory.buildByTemplate(SelectorTemplate.Single);

        //if (!tertiaryCoords.Equals(GridCoords.getDefaultCoords()))
        //{
        //    tertiarySelector.setToLocation(tertiaryCoords); 
        //}
 
        return tertiarySelector;
    }

    #endregion

    #region Targeting Logic

    protected List<TargetingLogic> _TargetingLogics;
    public List<TargetingLogic> targetingLogics => _TargetingLogics;

    #endregion

    #region Cooldown

    protected readonly int _MaximumCooldown;
    public virtual int maximumCooldown => _MaximumCooldown;

    public int cooldownRemaining { get; protected set; } = 0;

    public string getMaximumCooldownForDisplay()
    {
        if (maximumCooldown - 1 <= 0)
        {
            return noCooldownMessage;
        }
        else if (maximumCooldown - 1 == 1)
        {
            return (maximumCooldown - 1) + " Round";
        }
        else
        {
            return (maximumCooldown - 1) + " Rounds";
        }
    }

    public void setCooldownToMax() { cooldownRemaining = maximumCooldown; }

    public void takeOffCooldown()
    {
        cooldownRemaining = 0;
    }

    public void tickDown()
    {
        if (cooldownRemaining > 0)
        {
            cooldownRemaining--;
        }
    }

    #endregion

    #region Sprite and GameObject

    protected string _IconName;
    public virtual string iconName
    {
        get
        {
            return _IconName;
        }
        protected set
        {
            _IconName = value;
        }
    }

    public Sprite iconSprite => SpriteUtil.loadSpriteFromResources(iconName);

    #endregion

    #region Key and Descriptions

    public abstract string key { get; }

    public abstract string useDescription { get; }

    //flavour text shown in italics on the action's description panel; empty for an action with none
    public virtual string loreDescription => "";

    #endregion

    #region Slots

    public abstract int maximumSlots { get; }

    #endregion

    #region Source Item

    //the item the action comes from, such as an attack's weapon or a usable item. Null for an action with no item, which ignores sets
    public virtual Item sourceItem
    {
        get
        {
            return null;
        }
        set
        {
            //empty on purpose
        }
    }

    #endregion

    #region Constructors

    public CombatAction(Stats actor, Selector selector, Catalog<IQueuingUnit> queuingUnits = null, List<TargetingLogic> targetingLogics = null)
    {
        _ActorStats = actor;
        _Selector = selector;
        _TargetingLogics = targetingLogics ?? new List<TargetingLogic>();

        //copied, so adding a ChargeActionCost in applySettings can't change a catalog the caller also gave to other actions
        _QueuingUnits = queuingUnits != null ? new Catalog<IQueuingUnit>(queuingUnits.Values) : new Catalog<IQueuingUnit>();
    }

    #endregion

    #region Queueing Actions

    protected Catalog<IQueuingUnit> _QueuingUnits;
    public IEnumerable<IQueuingUnit> queuingUnits => _QueuingUnits.Values;

    #endregion

    #region Damage/Crit/Armor

    public override string getDamageTotalForDisplay()
    {
        if (cannotDealDamage)
        {
            return harmlessMessage;
        }

        int damageFormulaTotal = getDamageFormulaTotal();

        if (damageFormulaTotal <= 0)
        {
            return harmlessMessage;
        }

        return "" + damageFormulaTotal;
    }

    public override string getDamageFormulaForDisplay()
    {
        if (cannotDealDamage || getDamageFormulaTotal() <= 0)
        {
            return harmlessMessage;
        }

        return getFinalDamageFormula();
    }

    public override int getDamageFormulaTotal()
    {
        return DamageCalculator.calculateFormula(getFinalDamageFormula(), actorStats);
    }

    public override string getFinalDamageFormula()
    {
		if(cannotDealDamage)
		{
			return Constants.zeroRating;
		}

        return gatherAllNonActionFormulas(a => a.getDamageFormula());
    }

    public override string getFinalCritFormula()
    {
        string finalCritFormula = gatherAllNonActionFormulas(a => a.getCritFormula());
        //DamageCalculator.combineFormulas(getCritFormula(), );

        //the crit chance only matters to a preview, so it's only worked out for one
        if (cannotDealDamage || 
            (inPreviewMode && DamageCalculator.calculateFormula(finalCritFormula, getStatSource()) < DamageCalculator.critAutoSuccessThreshold))
		{
			return Constants.zeroRating;
		} 

        return finalCritFormula;
    }

    public override int getCritFormulaTotal()
    {
        return DamageCalculator.calculateFormula(getFinalCritFormula(), actorStats);
    }

    public override string getCritTotalForDisplay()
    {
        int critFormulaTotal = getCritFormulaTotal();

        if (critFormulaTotal <= 0)
        {
            return cannotCritMessage;
        }

        return critFormulaTotal + "%";
    }

    public override string getCritFormulaForDisplay()
    {
        if (cannotDealDamage || getCritFormulaTotal() <= 0)
        {
            return cannotCritMessage;
        }

        return "(" + getFinalCritFormula() + ")%";
    }

    protected virtual string gatherAllNonActionFormulas(FormulaDelegate<StatBoostSource> getFormula)
    {
        string allStats = getAllOfOneStatFormula<StatBoostSource>(getStatSource().allStatBoosts, t => getFormula(t));

        return DamageCalculator.combineFormulas(getFormula(this), allStats);
    }

    #endregion

    #region Perform Combat Action

    //only Clone replaces these, so each clone gets catalogs of its own
    public Catalog<IActionUnit> actionCatalog { get; private set; } = new Catalog<IActionUnit>();
    public Catalog<IAnimationUnit> animationCatalog { get; private set; } = new Catalog<IAnimationUnit>();

    #endregion

    #region Animations

    #region Animation Effects

    public virtual ScriptOnLanding landingScript => null;

    protected EffectAnimationType _EffectAnimationType = EffectAnimationType.Slash;
    public virtual EffectAnimationType effectAnimationType
    {
        get
        {
            return _EffectAnimationType;
        }
        protected set
        {
            _EffectAnimationType = value;
        }
    }

    #endregion

    #endregion

    #region Traits

    protected Trait _AppliedTrait;

    public virtual Trait appliedTrait
    {
        get
        {
            if (_AppliedTrait == null)
            {
                return null;
            }

            Trait appliedTraitClone = _AppliedTrait.clone();
            appliedTraitClone.traitApplier = actorStats;

            return appliedTraitClone;
        }
    }

    //sometimes you want an applied trait, but you don't want to describe it as in the case of activated passives which
    //apply a trait that is basically a copy of their ability description. In this case it would be redundant to describe
    //both, and in these cases the action should reimplement getAppliedTraitForDescription() to return null or another trait
    public virtual Trait getAppliedTraitForDescription()
    {
        return appliedTrait;
    }

    //nonapplied related traits are traits that may be related to an action, but aren't directly applied by it, such as the
    //afraid trait with the activated passive Devastating Criticals. It's applied by a random chance on crit, not applied by the
    //activated passive ability itself.
    public virtual List<Trait> getNonAppliedRelatedTraits()
    {
        return new List<Trait>();
    }

    public virtual List<Trait> getTraitsToDescribe()
    {
        List<Trait> listOfTraits = new List<Trait>();

        listOfTraits.AddRange(getNonAppliedRelatedTraits());

        if (getAppliedTraitForDescription() != null)
        {
            listOfTraits.Add(getAppliedTraitForDescription());
        }

        return listOfTraits;
    }

    #endregion

    #region Action Costs

    //what the action costs to use, by cost type. An action with no entries is free.
    //A cost of 0 is still a cost: the actor must have that cost type's trait, and the cost shows on the action's button
    private Dictionary<ActionCostType, int> actionCosts = new Dictionary<ActionCostType, int>();

    //0 for a cost type the action doesn't charge. Writing ActionCostType.None does nothing, as it isn't a cost
    public int this[ActionCostType costType]
    {
        get
        {
            return actionCosts.TryGetValue(costType, out int actionCost) ? actionCost : 0;
        }
        set
        {
            if (costType == ActionCostType.None)
            {
                return;
            }

            actionCosts[costType] = value;
        }
    }

    public IEnumerable<ActionCostType> actionCostTypes => actionCosts.Keys;

    public bool hasActionCost => actionCosts.Count > 0;

    #endregion

    #region Boolean Getters

    public bool hasAvailableSlots(IActionArrayStorage actionArrayStorage)
    {
        return hasAvailableSlots(actionArrayStorage.getStoredCombatActionArray());
    }

    public virtual bool hasAvailableSlots(CombatActionArray combatActionArray)
    {
        return combatActionArray.hasAvailableSlots(this);
    }

    #endregion

    #region Miscellaneous

    //played through CombatActionUtil.playEquipSFX/playUnequipSFX
    public virtual SFXType equipSFX => SFXType.ActionEquip;
    public virtual SFXType unequipSFX => SFXType.ActionUnequip;


    //overrides call this after their own settings, so the costs it checks are already set
    public virtual void applySettings(CombatActionSettings settings)
    {
        //an action with costs has to charge them when it's queued, unless it was given its own ChargeActionCost
        if(_QueuingUnits[typeof(ChargeActionCost)].Count == 0 && hasActionCost)
        {
            _QueuingUnits.Add(new ChargeActionCost());
        }
    }

	public override bool Equals(object obj)
	{
        CombatAction action = obj as CombatAction;

        if(action == null)
        {
            return false;
        }

        if(CombatStateManager.inCombat && actorStats != null)
        {
		    return action.uniqueName.Equals(uniqueName) && actorStats.Equals(action.actorStats);
        } else
        {
		    return action.uniqueName.Equals(uniqueName);
        }
	}

    public override int GetHashCode()
    {
        return uniqueName.GetHashCode();
    }

    public virtual void onAddToAbilityMenu() //for updating things like checking for source item quantity
    {
        // Empty on purpose
    }

    public void setToPreviewMode()
    {
        previewActor = actorStats.clone();

        inPreviewMode = true;
    }

    public abstract string getDisplayType();

    public virtual void addSlotsTextToRow(DescriptionPanel panel)
    {
        CombatActionArray combatActionArray = OverallUIManager.getCurrentActionArray();

        if(!ineligible() && maximumSlots > 0 && combatActionArray != null)
        {
            DescriptionPanel.setText(panel.slotsUsedText, combatActionArray.getSlotsUsedOnAction(this) + "/" + maximumSlots);
        }
    }

    public virtual string getMaximumSlotsForDisplay()
    {
        if (maximumSlots <= 0)
        {
            return noSlotsRequiredMessage;
        }
        else
        {
            return maximumSlots + " Slot(s)";
        }
    }

    public virtual CombatAction alternateActionWhenPlacedInActionSlot()
    {
        return null;
    }

    public virtual int getRequiredStatLevel()
    {
        //returns -1 if it doesn't require a stat level
        return -1;
    }

    public string getRequiredStatLevelForDisplay()
    {
        int reqLevel = getRequiredStatLevel();

        if(reqLevel < 1)
        {
            return Constants.emptyString;
        }

        return reqLevel.ToString();
    }

    //convertToJson is for save files, you will never need to save an actions coords so actor/target coords are not saved
    public abstract string convertToJson();

    //saves written before getSaveType() was retired end each action with a "CombatActionSaveType" tag, sometimes missing
    //its closing quote. The type is now read from the json itself, so the tag is stripped and ignored
    private static readonly Regex legacySaveTypeTag = new Regex(",\"CombatActionSaveType\":\"\\d\"?");

    public static CombatAction extractFromJson(Stats statsSource, string json)
    {
        if (json == null || json.ToLower().Equals("null"))
        {
            return null;
        }

        json = legacySaveTypeTag.Replace(json, "");

        if (json.Contains(Ability.sourceAbilityElementName))
        {
            //older ability json is missing its opening quote, so it's read by hand rather than as json
            string abilityKey = json.Replace("{", "").Replace("}", "").Replace("\"", "").Split(":")[1];

            return AbilityList.getAbility(statsSource, abilityKey);
        }

        //checked by type rather than through getCombatAction, so a weapon always loads as an Attack as it did before
        switch (SaveBlueprint.convertJsonToItem(json))
        {
            case Weapon weapon:
                return new Attack(statsSource, weapon);
            case UsableItem usableItem:
                return new ItemCombatAction(statsSource, usableItem);
            default:
                return null;
        }
    }

    public static List<GlossaryEntry> getAllActionTypeGlossaryEntries()
    {
        List<GlossaryEntry> allActionTypesGlossaryEntries = new List<GlossaryEntry>();
        
        allActionTypesGlossaryEntries.Add(new WrittenGlossaryEntry("What is an Action?", "Action Types", "Actions are maneuvers your Character can perform, usually during Combat. An Action must be placed on the Action Wheel for it to be available for use during Combat. The Action Wheel can be found on the Character Screen. Many Actions cannot be used each turn, and take time to cool down before they can be used again. Some Actions can be placed on the Action Wheel multiple times, with separate cool down periods for each slot. A Character can learn new Actions by leveling up, increasing their Primary Stats, equipping Weapons and Items, and by completing certain Quests."));
        
        allActionTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Ability", "Action Types", "Abilities are Actions a Character can learn by reaching certain Character Level or Primary Stat thresholds.\n\nAbilities are the only type of Action that benefits from your Bonus Damage.\n\nEvery time you level up and raise your Primary Stats, you will gain at least two new Abilities.\n\nRemember to add your new Abilities to your Action Wheel after leveling up, or else you won't be able to use them in Combat."));
        allActionTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Attack", "Action Types", "Attacks are gained by equipping Weapons to the Action Wheel.\n\nAttacks never have a cooldown, and the amount of Action Wheel Slots a Character can fill with Attacks is determined by their Wisdom.\n\nAn Attack acquired from a One-Handed Weapon will add any equipped Off-Hand Weapon's damage to its damage.\n\nUsing an Attack from a Two-Handed Weapon will not add a Character's Off-Hand Weapon's damage, and will remove any benefits gained from an Item held in the Off-Hand for the rest of the turn."));
        allActionTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Item", "Action Types", "Item Actions are gained by equipping Usable Items to the Action Wheel.\n\nSome Item Actions can be performed before the end of a Round, such as Bandages, allowing you to use them and still use another Action that turn.\n\nMost Item Actions destroy the Item after use.\n\nItem Actions rarely have a cooldown, but some do."));
        allActionTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Passive", "Action Types", "Passive Abilities are Abilities that provide a constant effect without requiring activation.\n\nUnlike all other Action Types, Passive Abilities do not need to be equipped to the Action Wheel for a Character to receive their benefits."));
        allActionTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Equipped Passive", "Action Types", "An Equipped Passive is an Ability that generates a unique Trait at the start of Combat.\n\nUnlike normal Passive Abilities, Equipped Passives must be added to your Action Wheel to benefit from them.\n\nThe Traits Equipped Passives generate are permanent, so long as the Equipped Passive remains on your Action Wheel."));
        allActionTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Skill", "Action Types", "Skills are Actions that are used out of combat.\n\nThey cannot be added to the Action Wheel, but instead can be activated while moving around the overworld.\n\nA Skill's Range is measured in tiles away from your Character, and Skills often have a limited number of uses, measured in Skill Charges.\n\n" + Constants.skillChargeDescription));

        return allActionTypesGlossaryEntries;
    }

    #endregion

    #region ICloneable

    public object Clone()
    {
        CombatAction clone = (CombatAction)MemberwiseClone();

        //costs can be written through the indexer, so a clone gets its own copy rather than sharing the original's
        clone.actionCosts = new Dictionary<ActionCostType, int>(actionCosts);

        //units belong to the run that made them, so a clone starts with empty catalogs rather than sharing the original's
        clone.actionCatalog = new Catalog<IActionUnit>();
        clone.animationCatalog = new Catalog<IAnimationUnit>();

        //same entries in new lists, so adding or removing an entry on a clone leaves the original alone
        clone._TargetingLogics = new List<TargetingLogic>(_TargetingLogics);
        clone._QueuingUnits = new Catalog<IQueuingUnit>(_QueuingUnits.Values);

        return clone;
    }

    public virtual CombatAction clone()
    {
        CombatAction clone = (CombatAction)Clone();

        clone.actorStats = actorStats;

        if (selector != null)
        {
            clone.selector = selector.clone();
        }

        if (previousTargets != null)
        {
            clone.previousTargets = new List<GridCoords>();

            foreach (GridCoords previousTarget in previousTargets)
            {
                clone.previousTargets.Add(previousTarget.clone());
            }
        }

        return clone;
    }

    public CombatAction clone(Stats statsSource)
    {
        CombatAction actionClone = clone();

        actionClone.actorStats = statsSource;

        return actionClone;
    }

    #endregion

    #region ISortable

    public virtual int getQuantity()
    {
        return 0;
    }
    public int getWorth()
    {
        throw new NotImplementedException("CombatActions cannot be sorted by Worth.");
    }
    public virtual string getType()
    {
        return getDisplayType();
    }

    public string getSubtype()
    {
        throw new NotImplementedException("CombatActions cannot be sorted by Subtype.");
    }
    public virtual int getLevel()
    {
        return 1;
    }
    public int getNumber()
    {
        throw new NotImplementedException("CombatActions cannot be sorted by Number.");
    }

    #endregion

    #region StatBoostSource Methods

    public override Stats getStatSource()
    {
        return actorStats;
    }

    #endregion

    #region IDescribable

    public virtual GameObject getRowType(RowType rowType)
    {
        string rowTypeName = "";

        switch (rowType)
        {
            case RowType.JournalCategory:
                rowTypeName = PrefabNames.glossaryCategoryRow;
                break;
            case RowType.Standard:
            case RowType.StatRequirements:
                rowTypeName = PrefabNames.playerAbilityRow;
                break;
            case RowType.CombatActionOrder:
                rowTypeName = PrefabNames.combatCombatActionOrderRow;
                break;
            default:
                throw new IOException("Incompatible RowType: " + rowType);
        }

        return Resources.Load<GameObject>(rowTypeName);
    }

    public virtual GameObject getDescriptionPanelFull()
    {
        return getDescriptionPanelFull(PanelType.Standard);
    }

    public virtual GameObject getDescriptionPanelFull(PanelType panelType)
    {
        string panelTypeName = "";

        switch (panelType)
        {
            case PanelType.Builder:
                panelTypeName = PrefabNames.descriptionPanelBuilder;
                break;
            default:
                throw new IOException("Unknown PanelType: " + panelType);
        }

        Debug.LogError("rowTypeName = " + panelTypeName);

        return DescriptionPanel.getDescriptionPanel(panelTypeName);
    }

    public virtual GameObject getDecisionPanel()
    {
        return null;
    }

    public virtual bool withinFilter(string[] filterParameters)
    {
        return false;
    }

    public virtual void describeSelfFull(DescriptionPanel panel)
    {
        panel.setObjectBeingDescribed(this);

        DescriptionPanel.setText(panel.nameText, displayName);

        DescriptionPanel.setText(panel.damageText, getDamageTotalForDisplay());
        DescriptionPanel.setText(panel.critRatingText, getCritTotalForDisplay());
        DescriptionPanel.setText(panel.rangeText, getRangeTitle());
        DescriptionPanel.setText(panel.typeText, getDisplayType());
        DescriptionPanel.setText(panel.amountText, getMaximumSlotsForDisplay());
        DescriptionPanel.setText(panel.timerText, getMaximumCooldownForDisplay());
        DescriptionPanel.setText(panel.useDescriptionText, useDescription);

        DescriptionPanel.setImage(panel.iconPanel, iconSprite);

        /* commented out because apparently getIconBackgroundColor() doesn't exist (was thinking fo traits
			may implement later
		if(panel.iconBackgroundPanel != null && !(panel.iconBackgroundPanel is null))
		{
			panel.iconBackgroundPanel.color = getIconBackgroundColor();
		}*/
    }

    public virtual void describeSelfRow(DescriptionPanel panel)
    {
        panel.setObjectBeingDescribed(this);

        DescriptionPanel.setText(panel.nameText, displayName);

        DescriptionPanel.setImage(panel.iconPanel, iconSprite);

        if(actorStats != null)
        {
            actorStats.setHeadSprite(panel);
        }
    }

    public virtual void setUpDecisionPanel(IDecisionPanel descisionPanel)
    {

    }

    public virtual bool ineligible()
    {
        if (CombatStateManager.inCombat && actorStats.isStunned)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual List<IDescribable> getRelatedDescribables()
    {
        return getTraitsToDescribe().Cast<IDescribable>().ToList();
    }

    public bool canPayActionCost(Stats caster)
    {
        return caster.canPayActionCost(this);
    }

    public bool buildableWithBlocks()
    {
        return true;
    }

    public bool buildableWithBlocksRows()
    {
        return true;
    }


    #endregion

    #region IDescribableInBlocks

    public virtual List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {

        List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(displayName));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getActionTypeBlock(getType(), HoverMessageList.actionTypePrefix + getType()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDamageBlock(getDamageTotalForDisplay(), getDamageFormulaForDisplay()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getCritBlock(getCritTotalForDisplay(), getCritFormulaForDisplay()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getRangeBlock(getRangeTitle()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getCooldownBlock(getMaximumCooldownForDisplay()));

        // if (appliedTrait != null)
        // {
        //     buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(appliedTrait.getMaxRoundsLeftForDisplay()));
        // }

        if (!CombatStateManager.inCombat)
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getSlotsBlock(getMaximumSlotsForDisplay()));
        }

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(useDescription));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: iconName));

        if(!requiresAnAction)
        {
            buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: IconList.noActionRequiredIconName));
        }

        //buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(getCritTotalForDisplay()));

        return buildingBlocks;

    }

    public bool requiresInspectNode()
    {
        return true;
    }

    #endregion

}