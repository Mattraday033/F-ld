using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public struct GridCoords
{
	public readonly static GridCoords incrementNorth = new GridCoords(-1, 0);
	public readonly static GridCoords incrementNorthEast = new GridCoords(-1, 1);
	public readonly static GridCoords incrementEast = new GridCoords(0, 1);
	public readonly static GridCoords incrementSouthEast = new GridCoords(1, 1);
	public readonly static GridCoords incrementSouth = new GridCoords(1, 0);
	public readonly static GridCoords incrementSouthWest = new GridCoords(1, -1);
	public readonly static GridCoords incrementWest = new GridCoords(0, -1);
	public readonly static GridCoords incrementNorthWest = new GridCoords(-1, -1);

	public readonly static GridCoords[] increments = new GridCoords[]{incrementNorth, incrementNorthEast, incrementEast, incrementSouthEast,
																incrementSouth, incrementSouthWest, incrementWest, incrementNorthWest};

	public int row;
	public int col;

	public GridCoords(int r, int c)
	{
		row = r;
		col = c;
	}

	public override string ToString()
	{
		return "(Row: " + row + ", Col: " + col + ")";
	}

	public Vector3 toVector3()
	{
        return CombatGrid.getPositionAt(row, col);
	}

	public Vector3Int toVector3Int()
	{
		return new Vector3Int(col, row, 0);
	}

	public bool Equals(GridCoords coords)
	{
		return ((coords.row == row) && (coords.col == col));
	}

	public bool isWithinAllySection()
	{
		if (col < CombatGrid.colLeftBounds || col > CombatGrid.colRightBounds)
		{
			return false;
		}

		if (row > CombatGrid.allyRowLowerBounds || row < CombatGrid.allyRowUpperBounds)
		{
			return false;
		}

		return true;
	}

	public bool isWithinEnemySection()
	{
		if (col < CombatGrid.colLeftBounds || col > CombatGrid.colRightBounds)
		{
			return false;
		}

		if (row > CombatGrid.enemyRowLowerBounds || row < CombatGrid.enemyRowUpperBounds)
		{
			return false;
		}

		return true;
	}

	public GridCoords clone()
	{
		return new GridCoords(row, col);
	}

	public static GridCoords getDefaultCoords()
	{
		return new GridCoords(-1, -1);
	}

	public int distanceTo(GridCoords other)
	{
		return Math.Abs(row - other.row) + Math.Abs(col - other.col);
	}
}

public enum CombatActionSaveType { Attack = 0, Ability = 1, ItemCombatAction = 2}

public enum CombatAnimationType{ None, Projectile, Effect }

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
    private const int noDamage = -1;
    protected const int singleExuberanceStack = 1;

    #endregion

    #region Global Variables

    public virtual bool onlyUsableDuringSurpriseRound
    {
        get => false;
        set { }
    }

    public Stats actorStats;

    private Selector selector; //the selector the player used to identify the spaces affected on the grid
                               //selector should be a snapshot of where the selector was when choosing the target
                               //for this action. The selector may change locations between choosing the target of
                               //this action and when the action is resolved, so be sure to use selector.clone() when
                               //instantiating an action.

    public int cooldownRemaining { get; private set; } = 0;

    public List<GridCoords> previousTargets = new List<GridCoords>();

    private Stats previewActor;
    public bool inPreviewMode { get; private set; } = false;

    public bool cannotDealDamage;
    public bool useSpecialAttackAnimation = false;

    #endregion

    #region Constructors

    public CombatAction(Stats actor, Selector selector)
    {
        this.actorStats = actor;
        this.selector = selector;
    }

    #endregion

    #region Damage/Crit/Armor

    public virtual string getDamageTotalForDisplay()
    {
        if (cannotDealDamage || getDamageFormulaTotal() <= 0)
        {
            return harmlessMessage;
        }

        return "" + getDamageFormulaTotal();
    }

    public virtual string getDamageFormulaForDisplay()
    {
        if (cannotDealDamage || getDamageFormulaTotal() <= 0)
        {
            return harmlessMessage;
        }

        return getFinalDamageFormula();
    }

    public virtual int getDamageFormulaTotal()
    {
        return DamageCalculator.calculateFormula(getFinalDamageFormula(), getActorStats());
    }

    public virtual string getFinalDamageFormula()
    {
		if(cannotDealDamage)
		{
			return Constants.zeroRating;
		}

        return gatherAllNonActionFormulas(a => a.getDamageFormula());
    }

    public virtual string getFinalCritFormula()
    {
        string finalCritFormula = DamageCalculator.combineFormulas(getCritFormula(), gatherAllNonActionFormulas(a => a.getCritFormula()));

        int critChance = DamageCalculator.calculateFormula(finalCritFormula, getStatSource());

        if (cannotDealDamage || 
            (inPreviewMode && critChance < DamageCalculator.critAutoSuccessThreshold))
		{
			return Constants.zeroRating;
		} 

        return finalCritFormula;
    }

    protected virtual string gatherAllNonActionFormulas(FormulaDelegate<StatBoostSource> getFormula)
    {
        string allStats = getAllOfOneStatFormula<StatBoostSource>(getStatSource().getAllStatBoosts(), t => getFormula(t));

        return DamageCalculator.combineFormulas(getFormula(this), allStats);
    }

    public virtual int getCritFormulaTotal()
    {
        return DamageCalculator.calculateFormula(getFinalCritFormula(), getActorStats());
    }

    public string getCritTotalForDisplay()
    {
        if (getCritFormulaTotal() <= 0)
        {
            return cannotCritMessage;
        }

        return getCritFormulaTotal() + "%";
    }

    public virtual string getCritFormulaForDisplay()
    {
        if (cannotDealDamage || getCritFormulaTotal() <= 0)
        {
            return cannotCritMessage;
        }

        return "(" + getFinalCritFormula() + ")%";
    }

    #endregion

    #region Perform Combat Action

    public virtual int[] findFinalDamage(Stats targetCombatant, bool isCrit)
    {
        int baseDamage = getDamageFormulaTotal();

        if (targetCombatant == null || baseDamage == 0)
        {
            return new int[] { -1 };
        }

        Stats actor = getActorStats();

        if (isCrit)
        {
            baseDamage = (int)(baseDamage * actor.getCritDamageMultiplier());
            baseDamage += (int)((float)targetCombatant.getTotalHealth() * actor.getDevastatingCriticalPercentage()); //will return 0f if not a devastatingCritical
        }

        if (CombatStateManager.isPlayerSurpriseRound())
        {
            baseDamage = (int)((float)baseDamage * actor.getSurpriseDamageMultiplier());
        }

        int finalDamage = targetCombatant.modifyIncomingDamage(baseDamage, actor.getArmorPenetration());

        return new int[] { finalDamage };
    }

    public virtual void performCombatAction() //virtual because some abilities target the ground below their targets, such as GroundEffectAbility
    {
        playActivationAnimation();

        performCombatAction(getSelector().getAllTargets());
    }

    public virtual void performCombatAction(List<Stats> targets)
    {
        int projectileNumber = 1;

        Dictionary<Stats, int> skipLog = new Dictionary<Stats, int>();

        foreach (Stats targetCombatant in targets)
        {
            int skips = 0;

            if (skipLog.ContainsKey(targetCombatant))
            {
                skips = skipLog[targetCombatant];
                skipLog[targetCombatant]++;
            }
            else
            {
                skipLog.Add(targetCombatant, 1);
            }

            projectileNumber += sendProjectileAt(targetCombatant.getPositionToHit(getSelector(), skips), targetCombatant, projectileNumber);

            applyTrait(targetCombatant);
        }
    }

    #endregion

    #region Animations

    public virtual void playActivationAnimation()
    {
        if(useSpecialAttackAnimation)
        {
            getActorStats().playSpecialAttackAnimation();
        } else
        {
            getActorStats().playAttackAnimation();
        }
    }

    public virtual string getEffectAnimationType()
    {
        return EffectAnimationType.Slash.ToString();
    }

    #region Projectiles and Effects

    public void createEffectAnimation(GridCoords targetCoords)
    {
        createEffectAnimation(targetCoords, false, noDamage, healsTarget(), targetMustBeDead());
    }

    public void createEffectAnimation(GridCoords targetCoords, bool crit, int damageNumber)
    {
        createEffectAnimation(targetCoords, crit, damageNumber, healsTarget(), targetMustBeDead());
    }

    public virtual void createEffectAnimation(GridCoords targetCoords, bool crit, int damageNumber, bool healsTarget, bool targetCanBeDead)
    {
        switch (getCombatAnimationType())
        {
            case CombatAnimationType.None:
                CombatAnimationManager.loadInvisibleProjectile(getActorCoords(), targetCoords, crit, damageNumber, healsTarget, targetCanBeDead);
                break;
            case CombatAnimationType.Projectile:
                CombatAnimationManager.loadProjectile(getActorCoords(), targetCoords, crit, damageNumber, healsTarget, targetCanBeDead, getLandingScript());
                break;
            case CombatAnimationType.Effect:
                CombatAnimationManager.loadInstantEffect(getEffectAnimationType(), targetCoords, crit, damageNumber, healsTarget, targetCanBeDead, getLandingScript(), getActorCoords());
                break;
        }
    }

    public virtual ScriptOnLanding getLandingScript()
    {
        return null;
    }

    public virtual int sendProjectileAt(GridCoords coords, Stats targetCombatant, int projectileNumber)
    {
        if (targetCombatant != null && getDamageFormula() != null &&
            ((!targetMustBeDead() && targetCombatant.isAlive()) || (targetMustBeDead() && !targetCombatant.isAlive())))
        {
            bool crit = DamageCalculator.isACrit(getCritFormula(), getName());
            int finalDamage = findFinalDamage(targetCombatant, crit)[0];

            if (!inPreviewMode)
            {
                if (crit && actorIsAlly())
                {
                    Exuberances.addExuberance(MultiStackProcType.YellowThorn, singleExuberanceStack);
                }

                if (healsTarget() && actorIsAlly())
                {
                    Exuberances.addExuberance(MultiStackProcType.GreenLeaf, singleExuberanceStack);
                }

                createEffectAnimation(coords, crit, finalDamage);
            }

            if (finalDamage >= 0)
            {
                targetCombatant.modifyCurrentHealth(finalDamage, healsTarget());
            } 

            return 1;
        }

        return 0;
    }

    public virtual int sendSecondaryProjectileAt(GridCoords coords, Stats targetCombatant, int projectileNumber)
    {
        if (targetCombatant != null && getDamageFormula() != null &&
            ((!targetMustBeDead() && targetCombatant.isAlive()) || (targetMustBeDead() && !targetCombatant.isAlive())))
        {
            bool crit = DamageCalculator.isACrit(getCritFormula(), getName());
            int finalDamage = findFinalDamage(targetCombatant, crit)[0];

            if (finalDamage >= 0)
            {
                if (!inPreviewMode)
                {
                    if (crit && actorIsAlly())
                    {
                        Exuberances.addExuberance(MultiStackProcType.YellowThorn, singleExuberanceStack);
                    }

                    if (healsTarget() && actorIsAlly())
                    {
                        Exuberances.addExuberance(MultiStackProcType.GreenLeaf, singleExuberanceStack);
                    }

                    createEffectAnimation(coords, crit, finalDamage);
                }

                targetCombatant.modifyCurrentHealth(finalDamage, healsTarget());

                return 1;
            }
        }

        return 0;
    }

    public int sendProjectileAt(GridCoords coords, Stats targetCombatant, int projectileNumber, bool noDamage)
    {
        if (targetCombatant != null && getDamageFormula() != null &&
            ((!targetMustBeDead() && targetCombatant.isAlive()) || (targetMustBeDead() && !targetCombatant.isAlive())))
        {
            bool crit = DamageCalculator.isACrit(getCritFormula(), getName());
            int finalDamage = findFinalDamage(targetCombatant, crit)[0];

            targetCombatant.modifyCurrentHealth(finalDamage, healsTarget());

            if (noDamage)
            {
                if (!inPreviewMode)
                {
                    if (crit)
                    {
                        Exuberances.addExuberance(MultiStackProcType.YellowThorn, singleExuberanceStack);
                    }

                    if (healsTarget() && actorIsAlly())
                    {
                        Exuberances.addExuberance(MultiStackProcType.GreenLeaf, singleExuberanceStack);
                    }

                    createEffectAnimation(coords, crit, finalDamage);
                }

                return 1;
            }
            else if (finalDamage >= 0)
            {
                if (!inPreviewMode)
                {
                    if (crit && actorIsAlly())
                    {
                        Exuberances.addExuberance(MultiStackProcType.YellowThorn, singleExuberanceStack);
                    }

                    if (healsTarget() && actorIsAlly())
                    {
                        Exuberances.addExuberance(MultiStackProcType.GreenLeaf, singleExuberanceStack);
                    }

                    createEffectAnimation(coords, crit, finalDamage);
                }

                return 1;
            }
        }

        return 0;
    }

    public int sendProjectileAtSpace(GridCoords coords, int projectileNumber)
    {
        if (!inPreviewMode)
        {
            createEffectAnimation(coords);
        }

        return 1;
    }

    public int sendProjectileAt(GridCoords coords, Stats targetCombatant, int projectileNumber, int predeterminedDamage, bool predeterminedCrit)
    {
        if (targetCombatant != null &&
            ((!targetMustBeDead() && targetCombatant.isAlive()) || (targetMustBeDead() && !targetCombatant.isAlive())))
        {
            targetCombatant.modifyCurrentHealth(predeterminedDamage, healsTarget());

            if (predeterminedDamage >= 0 && !inPreviewMode)
            {
                if (predeterminedCrit && actorIsAlly())
                {
                    Exuberances.addExuberance(MultiStackProcType.YellowThorn, singleExuberanceStack);
                }

                if (healsTarget() && actorIsAlly())
                {
                    Exuberances.addExuberance(MultiStackProcType.GreenLeaf, singleExuberanceStack);
                }

                createEffectAnimation(coords, predeterminedCrit, predeterminedDamage);
                return 1;
            }
        }

        return 0;
    }

    #endregion

    #endregion

    #region Sprite and GameObject

    public virtual string getIconName()
    {
        throw new IOException("The base class version of getIconName() was called extraneously");
    }


    public Sprite getIconSprite()
    {
        return Helpers.loadSpriteFromResources(getIconName());
    }

    public virtual void highlightActorSprites()
    {
        Stats actorStats = getActorStats();

        if (actorStats != null && !(actorStats is null))
        {
            actorStats.setOutline();
        }
    }

    public virtual void removeHighlightFromActorSprites()
    {
        Stats actorStats = getActorStats();

        if (actorStats != null && !(actorStats is null))
        {
            actorStats.removeOutline();
        }

        SelectorManager.declareSelectors();
    }

    #endregion

    #region Cooldown

    public virtual int getMaximumCooldown()
    {
        return 1;
    }

    public string getMaximumCooldownForDisplay()
    {
        if (getMaximumCooldown() - 1 <= 0)
        {
            return noCooldownMessage;
        }
        else if (getMaximumCooldown() - 1 == 1)
        {
            return (getMaximumCooldown() - 1) + " Round";
        }
        else
        {
            return (getMaximumCooldown() - 1) + " Rounds";
        }
    }

    public virtual int getCooldownRemaining()
    {
        return cooldownRemaining;
    }

    public virtual void setCooldownToMax()
    {
        setCooldownRemaining(1);

    }

    public void setCooldownRemaining(int cooldownRemaining)
    {
        this.cooldownRemaining = cooldownRemaining;
    }

    public virtual void takeOffCooldown()
    {
        setCooldownRemaining(0);
    }

    public void tickDown()
    {
        if (cooldownRemaining > 0)
        {
            setCooldownRemaining(cooldownRemaining - 1);
        }
    }

    public virtual CombatAnimationType getCombatAnimationType()
    {
        return CombatAnimationType.Effect;
    }

    #endregion

    #region Targeting and Selector

    public virtual Selector getTargetSelector() //used for finding selectors when enemies are targeting
    {
        SelectorManager selectorManager = SelectorManager.getInstance();
        Selector selector = null;
        Stats actor = getActorStats();

        List<Stats> listOfTargets;

        if (actor.shouldTargetEnemy())
        {
            listOfTargets = CombatGrid.getAllAliveEnemyCombatants();
        }
        else
        {
            listOfTargets = CombatGrid.getAllAliveAllyCombatants();
        }

        if (isSelfTargeting())
        {
            selector = selectorManager.selectors[getRangeIndex()].clone();
            selector.setToLocation(getActorCoords());
            return selector;
        }

        return actor.traitContainer.findTargetLocation(selectorManager.selectors[getRangeIndex()].clone(), listOfTargets);
    }

    public void addPreviousTarget(GridCoords coords)
    {
        previousTargets.Add(coords);
    }

    public GridCoords getPreviousTarget()
    {
        GridCoords previousTarget = (GridCoords)previousTargets[previousTargets.Count - 1];

        previousTargets.RemoveAt(previousTargets.Count - 1);

        return previousTarget;
    }

    public virtual Selector getSelector()
    {
        return selector;
    }

    public virtual bool isSelfTargeting()
    {
        return false;
    }

    public virtual GridCoords getSecondaryCoords()
    {
        throw new IOException("Base version of getSecondaryCoords() was called erroneously");
    }

    public virtual void setSecondaryCoords(GridCoords coords)
    {
        throw new IOException("Base version of setSecondaryCoords() was called erroneously");
    }

    public virtual bool secondaryCoordsRequiresEmptySpace()
    {
        return false;
    }

    public virtual bool requiresSecondaryCoords()
    {
        return false;
    }
    public virtual bool requiresTertiaryCoords()
    {
        return false;
    }

    public virtual bool resetCoordsWhenChoosingTertiary() // if true, when the player backs out of choosing the tertiary target, then the previous selector will snap to it's default position 
    {
        return false;
    }

    public virtual bool resetCoordsOnBackOutOfTertiary() // if true, when the player backs out of choosing the tertiary target, then the previous selector will snap to it's default position 
    {
        return false;
    }

    public virtual bool targetsAllySection()
    {
        return false;
    }

    public virtual bool targetsOnlyEmptySpace()
    {
        return false;
    }

    public virtual bool tertiaryCoordsRequiresEmptySpace()
    {
        throw new IOException("The base class version of tertiaryCoordsRequiresEmptySpace() was called extraneously");
    }

    public virtual void setTertiaryCoords(GridCoords coords)
    {
        throw new IOException("The base class version of setTertiaryCoords() was called extraneously");
    }

    public abstract int getRangeIndex();

    public abstract string getRangeTitle();

    public GameObject getSelectorObject()
    {
        if (selector == null)
        {
            return null;
        }

        return selector.getSelectorObject();
    }

    public virtual void setSelector(Selector selector)
    {
        this.selector = selector;
    }

    public virtual GridCoords getActorCoords()
    {
        return actorStats.position.clone();
    }

    public virtual Stats getActorStats()
    {
        if (inPreviewMode)
        {
            return previewActor;
        }
        else if (actorStats != null)
        {
            return actorStats;
        }
        else if (!CombatStateManager.inCombat)
        {
            return OverallUIManager.getCurrentPartyMember();
        }

        return actorStats;
    }

    public virtual GridCoords getTargetCoords()
    {
        if (getSelector() != null)
        {
            return getSelector().getCoords();
        }
        else
        {
            return GridCoords.getDefaultCoords();
        }
    }

    public Vector3 getTargetPosition()
    {
        return CombatGrid.getPositionAt(getTargetCoords());
    }

    public virtual Vector3 getTertiaryPosition()
    {
        throw new IOException("Called base version of getTertiaryPosition");
    }

    public virtual Selector getTertiarySelector()
    {
        Selector tertiarySelector = SelectorManager.getInstance().selectors[Range.singleTargetIndex];

        //if (!tertiaryCoords.Equals(GridCoords.getDefaultCoords()))
        //{
        //    tertiarySelector.setToLocation(tertiaryCoords); 
        //}

        return tertiarySelector;
    }

    public virtual void setActorCoords(GridCoords newPosition)
    {
        if (actorStats == null)
        {
            actorStats = CombatGrid.getCombatantAtCoords(newPosition);
        }
        else
        {
            actorStats.position = newPosition;
        }
    }

    public virtual void setActor(Stats actor)
    {
        this.actorStats = actor;
    }

    #endregion

    #region Traits

    //sometimes you want an applied trait, but you don't want to describe it as in the case of activated passives which
    //apply a trait that is basically a copy of their ability description. In this case it would be redundant to describe
    //both, and in these cases the action should reimplement getAppliedTraitForDescription() to return null or another trait
    public virtual Trait getAppliedTraitForDescription()
    {
        return getAppliedTrait();
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

    public virtual Trait getAppliedTrait()
    {
        return null;
    }

    public virtual void applyTrait(Stats target, Trait traitToApply = null)
    {
        if(traitToApply == null)
        {
            traitToApply = getAppliedTrait();
        }

        if (target == null || traitToApply == null)
        {
            return;
        }

        if(checkForResistance(target, traitToApply))
        {
            DamageNumberPopup.createResistPopUp(target.position, CombatGrid.getPositionAt(target.position), CombatAnimationManager.getInstance().damageNumberCanvas);
            return;
        }

        traitToApply.traitApplier = getActorStats();

        if (!inPreviewMode && actorIsAlly() && traitToApply.isBuff())
        {
            Exuberances.addExuberance(MultiStackProcType.BlueShield, singleExuberanceStack);
        }

        if (!inPreviewMode && actorIsAlly() && traitToApply.isDebuff())
        {
            Exuberances.addExuberance(MultiStackProcType.YellowThorn, singleExuberanceStack);
        }

        target.addTrait(traitToApply);
    }

    private bool checkForResistance(Stats target, Trait traitToApply = null)
    {
        if(traitToApply == null)
        {
            traitToApply = getAppliedTrait();
        }

        if(!traitToApply.isDebuff())
        {
            return false;
        }

        if(traitToApply.traitType == TraitType.Wound && target.rollAgainstPhysicalResistance())
        {
            return true;
        } 
        
        if(traitToApply.traitType == TraitType.Mental && target.rollAgainstMentalResistance())
        {
            return true;
        } 

        return false;
    }
    #endregion

    #region Queueing Actions

    public virtual void queueingAction()
    {
        //empty on purpose
    }

    public virtual void unqueueingAction()
    {
        //empty on purpose
    }

    public virtual void activatingAction()
    {
        //empty on purpose
    }

    #endregion

    #region Action Costs

    public virtual int[] getActionCosts()
    {
        return new int[] { 0 };
    }

    public virtual ActionCostType[] getActionCostTypes()
    {
        return new ActionCostType[] { ActionCostType.None };
    }

    #endregion

    #region Boolean Getters

    public virtual bool usableWithoutItemsInInventory()
    {
        return true;
    }

    public bool actorIsPriorityAttacker()
    {
        if (getActorCoords().Equals(GridCoords.getDefaultCoords()) || getActorStats() == null)
        {
            return false;
        }

        return getActorStats().isPriorityAttacker();
    }

    public bool actorIsLowPriorityAttacker()
    {
        if (getActorCoords().Equals(GridCoords.getDefaultCoords()) || getActorStats() == null)
        {
            return false;
        }

        return getActorStats().isPriorityAttacker();
    }

    public virtual bool hasRepetitionsRemaining()
    {
        return false;
    }
    public virtual bool takesAWeaponSlot()
    {
        return false;
    }

    public bool hasAvailableSlots(IActionArrayStorage actionArrayStorage)
    {
        return hasAvailableSlots(actionArrayStorage.getStoredCombatActionArray());
    }

    public virtual bool hasAvailableSlots(CombatActionArray combatActionArray)
    {
        return combatActionArray.hasAvailableSlots(this);
    }

    public virtual bool requiresAnAction()
    {
        return true;
    }

    public virtual bool unactivatable()
    {
        return false;
    }

    public virtual bool canBePlacedInActionSlot()
    {
        return true;
    }

    public bool actorIsSlowed()
    {
        if (getActorStats() == null)
        {
            return false;
        }
        else
        {
            return getActorStats().isSlowed();
        }
    }

    public virtual bool autoApply()
    {
        return false;
    }

    public bool actorIsAlly()
    {
        return CombatGrid.positionIsOnAlliedSide(getActorStats().position);
    }

    public virtual bool healsTarget()
    {
        return false;
    }

    public virtual bool targetMustBeDead()
    {
        return false;
    }

    public virtual bool repositionsCaster()
    {
        return false;
    }

    public virtual bool killsCaster()
    {
        return false;
    }

    public virtual bool canBePlacedInPassiveSlot()
    {
        return false;
    }

    public virtual bool needsItemQuantityPanel()
    {
        return false;
    }

    public virtual bool movesTarget()
    {
        return false;
    }

    public virtual bool multiActorAction()
    {
        return false;
    }

    #endregion

    #region Passive Benefits

    public virtual int getRedStacksAtStart()
    {
        return 0;
    }

    public virtual int getBlueStacksAtStart()
    {
        return 0;
    }

    public virtual int getYellowStacksAtStart()
    {
        return 0;
    }

    public virtual int getGreenStacksAtStart()
    {
        return 0;
    }

    #endregion

    #region Miscellaneous

    public virtual void playEquipSFX()
    {
        AudioManager.playSFX(AudioClipList.actionEquipSFX);
    }

    public virtual void playUnequipSFX()
    {
        AudioManager.playSFX(AudioClipList.actionUnequipSFX);
    }


    public virtual void applySettings(CombatActionSettings settings)
    {
        //empty on purpose
    }

    public virtual string getUseDescription()
    {
        return "";
    }

    public virtual string getKey()
    {
        throw new IOException("The base class version of getKey() was called extraneously");
    }

    public virtual void onAddToAbilityMenu() //for updating things like checking for source item quantity
    {
        // Empty on purpose
    }

    public void setToPreviewMode()
    {
        previewActor = getActorStats().clone();

        inPreviewMode = true;
    }

    public virtual int getSaveType()
    {
        throw new IOException("The base class version of getSaveType() was called extraneously");
    }

    public virtual Item getSourceItem()
    {
        return null;
    }

    public virtual void setSourceItem(Item sourceItem)
    {
        throw new IOException("The base class version of setSourceItem() was called extraneously");
    }

    public virtual string getDisplayType()
    {
        throw new IOException("The base class version of getDisplayType() was called extraneously");
    }

    public virtual int getMaximumSlots()
    {
        return 1;
    }

    public virtual string getMaximumSlotsForDisplay()
    {
        if (getMaximumSlots() <= 0)
        {
            return noSlotsRequiredMessage;
        }
        else
        {
            return getMaximumSlots() + " Slot(s)";
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

        if(reqLevel <= 1)
        {
            return Constants.emptyString;
        }

        return reqLevel.ToString();
    }

    //convertToJson is for save files, you will never need to save an actions coords so actor/target coords are not saved
    public abstract string convertToJson();

    public static CombatAction extractFromJson(Stats statsSource, string json)
    {
        if (json == null || json.ToLower().Equals("null"))
        {
            return null;
        }

        json = json.Replace("\"CombatActionSaveType\":\"0}", "\"CombatActionSaveType\":\"0\"}");
        json = json.Replace("\"CombatActionSaveType\":\"1}", "\"CombatActionSaveType\":\"1\"}");
        json = json.Replace("\"CombatActionSaveType\":\"2}", "\"CombatActionSaveType\":\"2\"}");

        string[] kvps = json.Replace("{", "").Replace("}", "").Replace("\"", "").Split(",");
        int saveType = int.Parse(kvps[kvps.Length - 1].Split(":")[1]);

        switch (saveType)
        {
            case (int)CombatActionSaveType.Attack:
                return (CombatAction)new Attack(statsSource, (Weapon)SaveBlueprint.convertJsonToItem(json));
            case (int)CombatActionSaveType.Ability:
                return AbilityList.getAbility(statsSource, kvps[0].Split(":")[1]);
            case (int)CombatActionSaveType.ItemCombatAction:
                return (CombatAction)new ItemCombatAction(statsSource, (UsableItem)SaveBlueprint.convertJsonToItem(json));
            default:
                return null;
        }
    }

    public static List<GlossaryEntry> getAllActionTypeGlossaryEntries()
    {
        List<GlossaryEntry> allActionTypesGlossaryEntries = new List<GlossaryEntry>();

        allActionTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Ability", "Action Types", "An Action gained through leveling up. Abilities are the only type of Action that benefits from your Bonus Damage. Every time you level up and raise your Primary Stats, you will gain at least two new Abilities. Remember to add your new Abilities to your Action Wheel, or else you won't be able to use them in Combat."));
        allActionTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Attack", "Action Types", "Attacks are gained by equipping Weapons to the Action Wheel. Attacks never have a cooldown, and the amount of Action Wheel Slots you can use for Attacks is determined by your Wisdom. An Attack acquired from a one handed weapon will add your off hand weapon's damage to it's damage. Using an Attack from a two handed weapon will not add your off hand weapon's damage, and will remove your shield's armor bonus for the rest of the turn."));
        allActionTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Item", "Action Types", "Item Actions are gained by equipping Usable Items to the Action Wheel. Some Item Actions can be performed before the end of a Round, such as Bandages, allowing you to use them and still use another Action that turn. Most Item Actions destroy the Item after use. Item's rarely have a cooldown, but some do."));
        allActionTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Passive", "Action Types", "Passive Abilities are Abilities that provide a constant effect without requiring activation. Unlike all other Action Types, Passives do not need to be equipped to the Action Wheel to be benefited from."));
        allActionTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Equipped Passive", "Action Types", "An Equipped Passive is an Ability that generates a unique Trait at the start of Combat. Unlike normal Passive Abilities, Equipped Passives must be added to your Action Wheel to benefit from them. The Traits Equipped Passives generate are permanent, so long as the Equipped Passive remains on your Action Wheel."));
        allActionTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Skill", "Action Types", "Skills are Actions that are used out of combat. They cannot be added to the Action Wheel, but instead are activated using the 1-4 keys while moving around the overworld. Their ranges are measured in tiles away from the player, and they often have a limited number of uses per area."));


        return allActionTypesGlossaryEntries;
    }

    #endregion

    #region ICloneable

    public object Clone()
    {
        return MemberwiseClone();
    }

    public virtual CombatAction clone()
    {
        CombatAction clone = (CombatAction)Clone();

        clone.setActor(actorStats);

        if (getSelector() != null)
        {
            clone.setSelector(getSelector().clone());
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

        actionClone.setActor(statsSource);

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
        return getActorStats();
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
            case RowType.CompanionAbilities:
                rowTypeName = PrefabNames.companionAbilityRow;
                break;
            case RowType.AbilityEditor:
                rowTypeName = PrefabNames.actionEditorRow;
                break;
            case RowType.CombatActionOrder:
                rowTypeName = PrefabNames.combatCombatActionOrderRow;
                break;
            case RowType.LevelUp:
                rowTypeName = PrefabNames.actionLevelUpDescriptionPanels;
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
            case PanelType.Combat:
            case PanelType.CombatHover:
                panelTypeName = PrefabNames.actionHoverDescriptionPanel;
                break;
            case PanelType.Standard:
            case PanelType.AbilityEditor:
                panelTypeName = PrefabNames.actionDescPanelFull;
                break;
            case PanelType.GlossaryDescription:
                panelTypeName = PrefabNames.perkDescriptionPanelFull;
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

        DescriptionPanel.setText(panel.nameText, getName());

        DescriptionPanel.setText(panel.damageText, getDamageTotalForDisplay());
        DescriptionPanel.setText(panel.critRatingText, getCritTotalForDisplay());
        DescriptionPanel.setText(panel.rangeText, getRangeTitle());
        DescriptionPanel.setText(panel.typeText, getDisplayType());
        DescriptionPanel.setText(panel.amountText, getMaximumSlotsForDisplay());
        DescriptionPanel.setText(panel.timerText, getMaximumCooldownForDisplay());
        DescriptionPanel.setText(panel.useDescriptionText, getUseDescription());


        DescriptionPanel.setImage(panel.iconPanel, Helpers.loadSpriteFromResources(getIconName()));

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

        DescriptionPanel.setText(panel.nameText, getName());

        DescriptionPanel.setImage(panel.iconPanel, Helpers.loadSpriteFromResources(getIconName()));
    }

    public virtual void setUpDecisionPanel(IDecisionPanel descisionPanel)
    {

    }

    public virtual bool ineligible()
    {
        if (CombatStateManager.inCombat && getActorStats().isStunned())
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
        return caster.canPayActionCost(getActionCostTypes(), getActionCosts());
    }

    public void chargeActorActionCost()
    {
        if (!getActionCostTypes().Contains(ActionCostType.None))
        {
            getActorStats().payActionCost(getActionCostTypes(), getActionCosts());
        }
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

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getActionTypeBlock(getType()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDamageBlock(getDamageTotalForDisplay(), getDamageFormulaForDisplay()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getCritBlock(getCritTotalForDisplay(), getCritFormulaForDisplay()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getRangeBlock(getRangeTitle()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getCooldownBlock(getMaximumCooldownForDisplay()));

        // if (getAppliedTrait() != null)
        // {
        //     buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(getAppliedTrait().getMaxRoundsLeftForDisplay()));
        // }

        if (!CombatStateManager.inCombat)
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getSlotsBlock(getMaximumSlotsForDisplay()));
        }

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getUseDescription()));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getIconName()));

        //buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(getCritTotalForDisplay()));

        return buildingBlocks;

    }

    public bool requiresInspectNode()
    {
        return true;
    }

    #endregion


}