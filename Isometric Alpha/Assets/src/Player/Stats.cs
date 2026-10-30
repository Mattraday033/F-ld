using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public abstract class Stats : ICloneable, IDescribable, IDescribableInBlocks, IAppearanceSource
{

    #region Constants

    public const bool failedToResist = false;

    private const int traitApplicationDamageFrameDelay = 2;
    private const bool doesNotHealTarget = false;
    private const bool isNotACrit = false;

    #endregion

    #region Unity Events

    public readonly static UnityEvent OnHealthChange = new UnityEvent();
    public readonly static UnityEvent OnStatsChange = new UnityEvent();
    public readonly static UnityEvent PredationProc = new UnityEvent();

    #endregion

    #region Global Variables

    //the player's characterName carries the player marker, which displayName leaves out
    private string _CharacterName;
    public string characterName
    {
        get
        {
            return _CharacterName;
        }
        set
        {
            _CharacterName = value;
            _NPCName = value == null ? null : value.Replace(PartyManager.playerMarker, "");
        }
    }

    private string _NPCName;

    private List<GridCoords> _Positions = new List<GridCoords>();

    public List<GridCoords> positions
    {
        get
        {
            return _Positions;
        }
        set
        {
            _Positions = value;
        }
    }


    public virtual IAppearance appearance
    {
        get;
    }

    public bool inPreviewMode = false;
    public bool inOnDeathEffect = false;

    //true when the player picks this combatant's targets, false when its actions find their own targets
    public virtual bool usePlayerTargeting => false;

    public Stats repositionClone;
    //used to track if Reposition Ability
    //is already moving creature

    public int currentHealth;

    public TraitContainer traitContainer;

    protected Dictionary<CharacterAnimationType, SFXType> animationAudioClipDictionary;

    #endregion

    #region Boolean Properties

    public virtual bool multiSpaceEnemy => false;
    public bool isAlive => currentHealth > 0;
    public bool isDead => currentHealth <= 0;
    public bool queuedToMove => repositionClone != null;
    public bool isRepositionClone => Helpers.hasQuality<Trait>(traitContainer, t => t.Equals(TraitList.repositioningInvulnerability));
    public virtual bool isPriorityAttacker => false;
    public virtual bool isLowPriorityAttacker => false;
    public virtual bool costsPartyCombatActions => false;
    public bool shouldTargetEnemy => positions.Any(p => CombatGrid.positionIsOnAlliedSide(p));
    public bool isMandatoryTarget => Helpers.hasQuality<Trait>(traitContainer, t => t.isMandatoryTarget());
    public bool isTargetable => !Helpers.hasQuality<Trait>(traitContainer, t => t.isUntargetable());
    public virtual bool isPartOfVolley => false;
    public bool isMaster => hasTrait(TraitList.master);
    public bool isMinion => hasTrait(TraitList.minion);
    public bool isSummon => hasTrait(TraitList.summoned);
    public virtual bool isLarge => hasTrait(TraitList.large);
    public bool isSlowed => Helpers.hasQuality<Trait>(traitContainer, t => t.slowsTraitHolder());
    public bool isImmobile => isLarge || isMultiTile;


    public bool isStunned
    {
        get
        {
            if(Helpers.hasQuality<Trait>(traitContainer, t => t.immuneToStun()))
            {
                return false;
            }

            return Helpers.hasQuality<Trait>(traitContainer, t => t.preventsCombatAction());
        }
    }

    public bool hasUnusedDeathEffect => Helpers.hasQuality<Trait>(traitContainer, t => t.hasUnusedOnDeathEffect()) || inOnDeathEffect;
    public virtual bool notResurrectable => Helpers.hasQuality<Trait>(traitContainer, t => t.preventsResurrection());
    public bool isBuffed => Helpers.hasQuality<Trait>(traitContainer, t => t.isBuff());
    public bool isDebuffed => Helpers.hasQuality<Trait>(traitContainer, t => t.isDebuff());
    public bool isFrontline => hasTrait(TraitList.frontLine);
    public bool isBackline => hasTrait(TraitList.backLine);
    public virtual bool hasAvailableWeaponSlots => false;
    public bool isMultiTile => positions.Count > Constants.sizeOne;
    public virtual bool removableFromFormation => true;

    #endregion

    #region Properties

    #region Combatant

    public virtual Color outlineColor => ColorList.canBeInteractedWith;

    #endregion

    #region Tutorial

    public string tutorialTargetHash
    {
        get
        {
            if(uniqueName.Equals(PartyManager.getPlayerStats().uniqueName))
            {
                return TutorialSequenceList.playerCombatSpriteTargetHash;
            }

            if(isMandatoryTarget)
            {
                return TutorialSequenceList.mandatoryTargetMonsterTargetHash;
            }

            //characterName, since an enemy's uniqueName carries its clone key
            switch(characterName)
            {
                case MonsterNameList.armoredBat:
                    return TutorialSequenceList.traitMonsterTargetHash;
                default:
                    return characterName;
            }
        }
    }

    #endregion

    #region Health

    public abstract int totalHealth { get; }

    public int missingHealth => totalHealth - currentHealth;

    #endregion

    #region Primary Stats

    //the total* stats include every boost; AllyStats keeps the unboosted values in strength, dexterity, wisdom and charisma
    public virtual int totalStrength => 1;

    public virtual int totalDexterity => 1;
    public virtual int extraArmorFromDexterity => 0;

    public virtual int totalWisdom => 1;
    public virtual int passiveSlotsUnlocked => 0;
    public virtual int armorPenetration => StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getBonusArmorPenetrationFormula());

    public virtual int totalCharisma => 1;
    public virtual int zoiStat => totalCharisma;
    public virtual int synergyCoefficient => 0;

    public int synergyModifier
    {
        get
        {
            int sum = 0;
            int synergyBonus = synergyCoefficient;

            foreach (Trait trait in traitContainer)
            {
                if (trait.fromZoneOfInfluence())
                {
                    sum += synergyBonus;
                }
            }

            return sum;
        }
    }

    public virtual int bonusExuberances => 0;

    #endregion

    #region Tertiary Stats

    public virtual string invulnerability => "" + StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getInvulnerableFormula());
    public virtual string vulnerability => "" + StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getVulnerableFormula());
    public virtual string healingBoost => "" + StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getHealingBoostFormula());

    #endregion

    #region Combat and Action Arrays

    public abstract int totalArmorRating { get; }
    public virtual int totalArmorShred => StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getArmorShredFormula());

    public string totalArmorRatingForDisplay
    {
        get
        {
            int armorAfterShred = totalArmorRating - totalArmorShred;

            if(armorAfterShred < 0)
            {
                return Constants.zeroRating + "%";
            } else
            {
                return armorAfterShred.ToString() + "%";
            }
        }
    }

    public virtual double critDamageMultiplier => 1.5;
    public virtual float surpriseDamageMultiplier => 1f;

    public virtual CombatActionArray actionArray => new CombatActionArray(null);

    public virtual int bonusAbilityDamage => 0;

    public virtual string bonusCritChance
    {
        get
        {
            string critChance = "" + StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getCritFormula());

            if (critChance == null ||
                critChance.Length <= 0 ||
                critChance.Equals(Constants.zeroRating) ||
                critChance.Equals(Constants.zeroBonus))
            {
                return "";
            }

            return "+" + critChance;
        }
    }

    public virtual float devastatingCriticalPercentage => 0f;

    #region Volley

    public virtual int volleyAccuracy => Constants.perfectVolleyAccuracy;
    public virtual int bonusVolleyAccuracy => Constants.sizeZero;
    public abstract EffectAnimationType volleyAnimationType { get; }

    #endregion

    #endregion

    #region Traits

    public virtual ZoneOfInfluenceTrait zoneOfInfluenceTrait => null;

    #endregion

    #region Equipment

    public virtual EquippedItems equippedItems => null;
    public virtual int weaponSlots => 0;

    #endregion

    #region Miscellaneous

    public virtual string genderMarker => "";
    public virtual List<StatBoostSource> allStatBoosts => StatBoostSource.getAllStatBoosts(traitContainer);

    private Sprite headSprite => EnemyTypeFolderPathList.getHeadSprite(uniqueName);

    #endregion

    #endregion

    #region Constructors

    public Stats(string name)
    {
        this.characterName = name;
        traitContainer = new TraitContainer(this);
    }

    #endregion

    #region Combatant

    //the GameObject side lives on the Combatant, which sets these when it's spawned and listens to the events below.
    //one per GameObject, so several for a MultiAnimationEnemyStats; the primary one carries the health bar (see getCombatant)
    [NonSerialized]
    public List<Combatant> combatants = new List<Combatant>();

    public event Action OnPositionsChanged;
    public event Action OnHealthBarUpdate;
    //true when the combatant can't be brought back, so its GameObjects are removed along with it
    public event Action<bool> OnDeath;
    public event Action OnRevive;
    public event Action<Trait> OnTraitAdded;
    public event Action<Trait> OnTraitRemoved;

    //clones share no GameObject with the Stats they came from, so the Combatant never hears from them
    private void detachCombatant()
    {
        combatants = new List<Combatant>();

        OnPositionsChanged = null;
        OnHealthBarUpdate = null;
        OnDeath = null;
        OnRevive = null;
        OnTraitAdded = null;
        OnTraitRemoved = null;
    }

    public virtual void setToDeadSprite()
    {
        if (CombatStateManager.whoseTurn != WhoseTurn.TickDown &&
            CombatStateManager.whoseTurn != WhoseTurn.Resolving &&
            CombatStateManager.whoseTurn != WhoseTurn.Start )
        {
            return;
        }

        bool removedFromCombat = notResurrectable && !hasUnusedDeathEffect;

        if (removedFromCombat)
        {
            removeFromGrid();
        }

        OnDeath?.Invoke(removedFromCombat);

        if(CombatStateManager.whoseTurn != WhoseTurn.Start)
        {
            prepareOnDeathEffects();
        }
    }

    public virtual void bringBackFromDeath()
    {
        OnRevive?.Invoke();
    }

    public virtual GridCoords getPositionToHit(Selector selector, int skips)
    {
        GridCoords[] allSelectorCoords = selector.getAllSelectorCoords();
        List<GridCoords> overlapping = new List<GridCoords>();

        foreach (GridCoords coords in allSelectorCoords)
        {
            if (positions.Contains(coords))
            {
                overlapping.Add(coords);
            }
        }

        if (overlapping.Count == 0 || skips >= overlapping.Count)
        {
            return positions.Count > 0 ? positions[0].clone() : GridCoords.getDefaultCoords();
        }

        return overlapping[skips];
    }

    public virtual void spawningActions()
    {
        //Empty on Purpose
    }

    public void instateEnvironmentalCombatAction()
    {
        EnvironmentalCombatActionManager.instateEnvironmentalCombatAction(this);
    }

    public virtual void removeFromGrid()
    {
        foreach (GridCoords coords in positions)
        {
            if (CombatGrid.tryGetCombatantAt(coords, out Combatant combatant)
                && combatant.stats.Equals(this))
            {
                CombatGrid.removeCombatantAtCoords(coords);
            }
        }
    }

    public virtual bool isInsideCoordinates(GridCoords coords)
    {
        return positions.Contains(coords);
    }

    public virtual bool isInsideCoordinates(GridCoords[] coords)
    {
        return positions.Any(p => coords.Contains(p));
    }

    #endregion

    #region Audio

    public virtual void playAnimationSFX(CharacterAnimationType animationType)
    {
        if(animationAudioClipDictionary == null ||
            !animationAudioClipDictionary.ContainsKey(animationType))
        {
            return;
        }

        AudioManager.playAudioClipAsSingleton(AudioClipList.getAudioClip(animationAudioClipDictionary[animationType]));
    }

    #endregion

    #region HealthBar

    //the health bar belongs to the Combatant, which redraws it from these Stats
    public void updateHealthBar()
    {
        OnHealthBarUpdate?.Invoke();
    }

    #endregion

    #region Health

    public void fullHeal()
    {
        modifyCurrentHealth(totalHealth, true);
    }

    // public void modifyCurrentHealth(int changeInHealth)
    // {
    //     modifyCurrentHealth(changeInHealth, false);
    // }

    public void modifyCurrentHealth(int changeInHealth, bool healing = false, bool playAnimation = true)
    {
        int maxHealth = totalHealth;

        if (!inPreviewMode && changeInHealth >= maxHealth && !healing && isMaster)
        {
            PredationProc.Invoke();
        }

        if (changeInHealth >= 0 && !healing)
        {
            removeTraitsRemovedByDamage();
        }

        if (changeInHealth != 0)
        {
            if (!healing) //handle dealing damage
            {
                if (changeInHealth >= currentHealth)
                {

                    currentHealth = 0; // killed actor, set health to 0
                }
                else
                {
                    currentHealth -= changeInHealth; //hurt actor, decrement by changeInHealth
                }
            }
            else
            { //handle Healing

                if ((changeInHealth + currentHealth) >= maxHealth)
                {
                    currentHealth = maxHealth; //healed actor to full, set to total to not go over;
                }
                else
                {
                    currentHealth += changeInHealth; //healed enemy, increment by changeInHealth
                }
            }
        }

        if (CombatStateManager.inCombat && !inPreviewMode)
        {
            if (!healing && PartyManager.getPlayerStats().currentHealth > 0)
            {
                harmAllLinkedTargets(changeInHealth);
            }

            updateHealthBar();
        }

        OnHealthChange.Invoke();
    }

    #endregion

    #region Primary Stats

    #region Strength + Secondaries

    public virtual bool rollAgainstWoundResistance()
    {
        return failedToResist;
    }

    #endregion

    #region Wisdom + Secondaries

    public virtual bool rollAgainstMentalResistance()
    {
        return failedToResist;
    }

    #endregion

    #endregion

    #region Combat and Action Arrays

    public void moveTo(List<GridCoords> newCoords, bool moveSprite = true)
    {
        List<GridCoords> oldCoords = positions.Select(p => p.clone()).ToList();

        CombatGrid.removeCombatantFromGrid(this);

        positions = newCoords.Select(p => p.clone()).ToList();

        CombatGrid.addCombatantToGrid(this);

        if (moveSprite && positions.Count > 0)
        {
            OnPositionsChanged?.Invoke();
        }
    }

    public bool canPayActionCost(CombatAction action)
    {
        List<bool> costsPayable = new List<bool>();
        List<ActionCostType> actionCostTypesUnpayable = new List<ActionCostType>();

        foreach (ActionCostType costType in action.actionCostTypes)
        {
            int actionCost = action[costType];
            bool costPayable = false;

            if (costType == ActionCostType.RedKnife ||
                costType == ActionCostType.BlueShield ||
                costType == ActionCostType.YellowThorn ||
                costType == ActionCostType.GreenLeaf
                )
            {
                costPayable = Exuberances.canPayCost(costType, actionCost);

                if(!costPayable)
                {
                    actionCostTypesUnpayable.Add(costType);
                }

                costsPayable.Add(costPayable);
                continue;
            }

            foreach (Trait trait in traitContainer)
            {
                if (trait == null)
                {
                    continue;
                }

                if (trait.getNumberOfStacks(costType) >= actionCost)
                {
                    costPayable = true;

                    if(!costPayable)
                    {
                        actionCostTypesUnpayable.Add(costType);
                    }
                }
            }

            costsPayable.Add(costPayable);
        }

        ExuberanceTracker.ActionCostCannotBePaid.Invoke(actionCostTypesUnpayable);

        return !costsPayable.Contains(false);
    }

    public int modifyIncomingDamage(int baseDamage, int armorPen)
    {
        // int modifiedDamage = baseDamage;
        int modifiedDamage = (int)(((double)baseDamage) * (1.0 - Armor.getDamageReduction(totalArmorRating - (totalArmorShred + armorPen))));

        int vulnInvulnMod = int.Parse(vulnerability) - int.Parse(invulnerability);

        modifiedDamage += vulnInvulnMod;

        modifiedDamage -= synergyModifier;

        if (modifiedDamage < 1)
        {
            modifiedDamage = 1;
        }

        return modifiedDamage;
    }

    public int modifyIncomingHealing(int baseDamage)
    {
        string boost = healingBoost;

        if(boost.Length <= 0)
        {
            return baseDamage;
        }

        return baseDamage + int.Parse(boost);
    }

    public void payActionCost(CombatAction action)
    {
        foreach (ActionCostType costType in action.actionCostTypes)
        {
            switch(costType)
            {
                case ActionCostType.RedKnife:
                case ActionCostType.BlueShield:
                case ActionCostType.YellowThorn:
                case ActionCostType.GreenLeaf:
                    Exuberances.payCost(costType, action[costType]);
                    continue;
                default:
                    Trait costTrait = Helpers.getObjectWithQuality<Trait>(traitContainer, t => t.hasActionCostType(costType));

                    if (costTrait != null)
                    {
                        costTrait.removeStacks(costType, action[costType]);
                    }
                    break;
            }
        }
    }

    public void refundActionCost(CombatAction action)
    {
        foreach (ActionCostType costType in action.actionCostTypes)
        {
            switch(costType)
            {
                case ActionCostType.RedKnife:
                case ActionCostType.BlueShield:
                case ActionCostType.YellowThorn:
                case ActionCostType.GreenLeaf:
                    Exuberances.addExuberance(costType, action[costType]);
                    continue;
                default:
                    Trait costTrait = Helpers.getObjectWithQuality<Trait>(traitContainer, t => t.hasActionCostType(costType));

                    if (costTrait != null)
                    {
                        costTrait.removeStacks(costType, action[costType]);
                    }
                    break;
            }
        }
    }

    public bool mandatoryTargetForTargetingType(Trait targetTrait)
    {
        if(targetTrait == null)
        {
            return false;
        }

        switch(targetTrait.uniqueName)
        {
            case TerritorialTargetPriorityTrait.initialName:
                return hasTrait(TraitList.intimidatingPressence);
            case PredatoryTargetPriorityTrait.initialName:
                return hasTrait(TraitList.protectTheWeak);
            case ChaoticTargetPriorityTrait.initialName:
                return hasTrait(TraitList.protectTheWeak);
            default:
                return false;
        }
    }

    public virtual void evolve()
    {
        //left intentionally blank
    }

    public virtual void devolve()
    {
        //left intentionally blank
    }

    #endregion

    #region Traits

    public void addTrait(Trait newTrait)
    {
        if (newTrait == null || (isDead && CombatStateManager.whoseTurn != WhoseTurn.Start))
        {
            return;
        }

        newTrait = newTrait.clone();
        newTrait.onApplication();
        newTrait.setTraitHolder(this);

        OnTraitAdded?.Invoke(newTrait);

        if(!newTrait.isHiddenTrait())
        {
            dealTraitApplicationDamage(newTrait);
        }

        traitContainer.addTrait(newTrait);

        Trait.OnTraitApplication.Invoke(newTrait);
    }

    public Trait getTraitOfType(TraitType traitType)
    {
        return Helpers.getObjectWithQuality<Trait>(traitContainer, t => t.traitType == traitType);
    }

    private void dealTraitApplicationDamage(Trait newTrait)
    {
        int traitApplicationDamage = getTraitApplicationDamage(newTrait);

        if (traitApplicationDamage > 0)
        {
            modifyCurrentHealth(traitApplicationDamage, doesNotHealTarget);

            if (CombatStateManager.whoseTurn == WhoseTurn.Resolving)
            {
                foreach (GridCoords coords in positions)
                {
                    DamageNumberPopup.create(coords,
                                             traitApplicationDamage,
                                         CombatGrid.getPositionAt(coords),
                                         DamageNumberPopup.getDirectionByTargetCoords(coords),
                                         AnimationTracker.getInstance().damageNumberCanvas,
                                         isNotACrit,
                                         doesNotHealTarget,
                                         traitApplicationDamageFrameDelay);
                }
            }
        }
    }

    private int getTraitApplicationDamage(Trait newTrait)
    {
        if (newTrait.isDebuff())
        {
            return Helpers.sum<Trait>(traitContainer, t => t.damageOnDebuffApplication());
        }
        else if (newTrait.isBuff())
        {
            return Helpers.sum<Trait>(traitContainer, t => t.damageOnBuffApplication());
        }
        else
        {
            return 0;
        }
    }

    public void addTraits(IEnumerable newTraits)
    {
        foreach (Trait trait in newTraits)
        {
            addTrait(trait);
        }
    }

    public void removeTrait(Trait traitToRemove)
    {
        if(traitContainer.removeTrait(traitToRemove))
        {
            OnTraitRemoved?.Invoke(traitToRemove);

            Trait.OnTraitRemoval.Invoke(traitToRemove);
        }
    }

    public void removeFirstTraitOfType(TraitType traitType)
    {
        traitContainer.removeFirstTraitOfType(traitType);
    }

    public void removeAllTraits()
    {
        traitContainer = new TraitContainer(this);
    }

    public void removeAllTraitsOfType(TraitType traitType)
    {
        traitContainer.removeAllTraitsOfType(traitType);
    }

    public virtual void addEquippedPassiveTraits()
    {
        //empty on purpose
    }

    private void removeTraitsRemovedByDamage()
    {
        traitContainer.removeAllTraitsRemovedByDamage();
    }

    public void prepareOnDeathEffects()
    {
        removeAllTraitsOfType(TraitType.Mental);
        removeAllTraitsOfType(TraitType.Wound);

        foreach (Trait trait in traitContainer)
        {
            if (trait != null)
            {
                trait.onDeathEffect(this);
            }
        }
    }

    public bool hasTrait(Trait trait)
    {
        return Helpers.hasQuality<Trait>(traitContainer, t => t.Equals(trait));
    }

    public bool hasTraitOfType(TraitType traitType)
    {
        return Helpers.hasQuality<Trait>(traitContainer, t => t.traitType == traitType);
    }

    private void harmAllLinkedTargets(int damage)
    {
        foreach (Trait trait in traitContainer)
        {
            if (trait != null)
            {
                trait.harmAllLinkedTargets(damage);
            }
        }
    }

    #region Zone of Influence

    public void removeAllZoneOfInfluenceTraits()
    {
        traitContainer.removeAllTraitsOfType(TraitType.Influence);
    }

    #endregion

    #endregion

    #region Miscellaneous

    public abstract List<GridCoords> findLocationToSpawn();

    public override bool Equals(object obj)
    {
        Stats stats = obj as Stats;

        if (stats == null)
        {
            return false;
        }

        if(CombatStateManager.inCombat  &&
            !(stats.positions.Count <= 0 && positions.Count <= 0))
        {

            return stats.positions.Any(p => positions.Contains(p)) && stats.uniqueName.Equals(uniqueName);
        } else
        {
            return stats.uniqueName.Equals(uniqueName);
        }
    }

    public override int GetHashCode()
    {
        return uniqueName.GetHashCode();
    }

    public static AllyStats convertIDescribableToStats(IDescribable describable)
    {
        if (describable as PartyMember != null)
        {
            return (describable as PartyMember).stats;
        }
        else
        {
            return describable as AllyStats;
        }
    }

    public bool isInParty(Formation formation)
    {
        return formation.isInParty(this);
    }

    public void setHeadSprite(DescriptionPanel panel)
    {
        Costume costume = appearance as Costume;

        if(costume != null)
        {
            costume.applyHeadshot(panel?.headshotDisplay);
        }
    }

    public void setHeadSprite(Image image)
    {
        if(image != null)
        {
            image.sprite = headSprite;
        }
    }

    public void setHeadSprite(SpriteRenderer spriteRenderer)
    {
        if(spriteRenderer != null)
        {
            spriteRenderer.sprite = headSprite;
        }
    }

    #endregion

    #region ICloneable

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public virtual Stats clone()
    {
        Stats clone = (Stats)Clone();

        clone.repositionClone = null;
        clone.positions = positions.Select(p => p.clone()).ToList();

        clone.detachCombatant();

        clone.traitContainer = traitContainer.clone(clone);

        return clone;
    }

    //a method rather than a property, since it builds a new clone each time
    public Stats getPreviewClone()
    {
        Stats previewClone = clone();

        previewClone.inPreviewMode = true;

        return previewClone;
    }

    #endregion

    #region IDescribable

    //not scrubbed of end numbers, since a player's chosen name can legitimately end in one
    public virtual string displayName { get { return _NPCName; } }
    public virtual string uniqueName { get { return _CharacterName; } }

    public virtual bool ineligible()
    {
        return false;
    }

    public virtual GameObject getRowType(RowType rowType)
    {
        return null;
    }

    public virtual GameObject getDescriptionPanelFull()
    {
        return getDescriptionPanelFull(PanelType.Standard);
    }

    public virtual GameObject getDescriptionPanelFull(PanelType type)
    {
        return null;
    }

    public virtual GameObject getDecisionPanel()
    {
        return null;
    }

    public virtual bool withinFilter(string[] filterParameters)
    {
        return true;
    }

    public virtual void describeSelfFull(DescriptionPanel panel)
    {
        panel.setObjectBeingDescribed(this);

        DescriptionPanel.setText(panel.nameText, displayName);
        DescriptionPanel.setText(panel.hpText, currentHealth + " / " + totalHealth);
        DescriptionPanel.setText(panel.armorRatingText, totalArmorRatingForDisplay);
    }

    public virtual void describeSelfRow(DescriptionPanel panel)
    {
        describeSelfFull(panel);
    }

    public virtual void setUpDecisionPanel(IDecisionPanel descisionPanel)
    {

    }

	public virtual List<IDescribable> getRelatedDescribables()
	{
		return new List<IDescribable>();
	}

    public virtual bool buildableWithBlocks()
    {
        return true;
    }

    public virtual bool buildableWithBlocksRows()
    {
        return true;
    }

    #endregion

    #region IDescribableInBlocks

    public virtual List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {

        List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(displayName));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getHealthBlock(currentHealth, totalHealth));

        int armorShred = totalArmorShred;

        if(armorShred > 0)
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getArmorBlock(totalArmorRatingForDisplay, totalArmorRating + " - " + armorShred));
        } else
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getArmorBlock(totalArmorRatingForDisplay));
        }

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getInvulnerableBlock(invulnerability));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getVulnerableBlock(vulnerability));

        return buildingBlocks;
    }

    public bool requiresInspectNode()
    {
        return false;
    }

    #endregion

}
