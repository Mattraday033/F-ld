using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.Events; 


public abstract class Stats : ScriptableObject, ICloneable, IDescribable, IDescribableInBlocks
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

    public string characterName;

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

    public Color previousColor = Color.clear;

    public bool inPreviewMode = false;
    public bool inOnDeathEffect = false;

    public GameObject combatSprite;
    public Stats repositionClone;
    //used to track if Reposition Ability
    //is already moving creature

    public string combatSpriteName;

    public int currentHealth;

    // public Trait[] traits = new Trait[0];
    // public Trait[] hiddenTraits = new Trait[0];
    public TraitContainer traitContainer;

    protected Dictionary<CharacterAnimationType, SFXType> animationAudioClipDictionary;

    #endregion

    #region Constructors

    public Stats(string name)
    {
        this.characterName = name;
        traitContainer = new TraitContainer(this);
    }

    #endregion

    #region Sprite and GameObject

    public SpriteRenderer spriteRenderer;
    public SpriteOutline outline;

    public virtual Color getOutlineColor()
    {
        return ColorList.canBeInteractedWith;
    }

    public virtual SpriteOutline[] getOutlines()
    {
        return new SpriteOutline[]{outline};
    }

    public void setPreviousColor(Color newColor)
    {
        if (previousColor.Equals(Color.clear) && !newColor.Equals(Color.clear))
        {
            previousColor = Helpers.cloneColor(newColor);
        }
    }

    public virtual void setToDeadSprite()
    {
        if (CombatStateManager.whoseTurn != WhoseTurn.TickDown && 
            CombatStateManager.whoseTurn != WhoseTurn.Resolving && 
            CombatStateManager.whoseTurn != WhoseTurn.Start )
        {
            return;
        }

        if (notResurrectable() && !hasUnusedDeathEffect())
        {
            destroyCombatSprite();
            removeFromGrid();
        } else
        {
            healthBarManager.hide();
        }
        
        if(CombatStateManager.whoseTurn == WhoseTurn.Start)
        {
            animationManager.setCurrentIdle(CharacterAnimationType.Death);
        }
        
        if(CombatStateManager.whoseTurn != WhoseTurn.Start)
        {
            prepareOnDeathEffects();
        }
    }

    public virtual void bringBackFromDeath()
    {
        healthBarManager.show();
        animationManager.setToDefaultIdle();
        animationManager.playSpawnAnimation();
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

    public virtual string getCombatSpriteName()
    {
        return combatSpriteName;
    }

    public virtual GameObject instantiateCombatSprite(List<GridCoords> initialPositions)
    {
        combatSprite = Instantiate(Resources.Load<GameObject>(getCombatSpriteName()), CombatStateManager.getCreatureParent());

        positions = initialPositions.Select(p => p.clone()).ToList();

        setUpComponents(combatSprite.GetComponent<ComponentList>());

        moveTo(positions);

        return combatSprite;
    }

    protected SpawnDetails obtainSpawnDetails()
    {
        SpawnDetails spawnDetails = null;

        if(!obtainedSpawnDetails)
        {
            spawnDetails = State.enemyPackInfo.getNextSpawnDetails();
        }

        obtainedSpawnDetails = true;

        return spawnDetails;
    }

    protected bool obtainedSpawnDetails = false;

    public virtual void setUpComponents(ComponentList list)
    {
        healthBarManager = list.healthBarManager;
        updateHealthBar();

        list.combatantHover.linkedStats = this;

        spriteRenderer = list.spriteRenderer;

        outline = new SpriteOutline();
        outline.setSpriteRenderer(spriteRenderer);

        animationManager = list.animationManager;
        animationManager.linkedStats = this;
        animationManager.healthBarManager = healthBarManager;
        animationManager.setAnimations(getName() + getGenderMarker() + getAnimationSuffixes());

        tutorialTarget = list.tutorialTarget;
        tutorialTarget.tutorialHash = getTutorialTargetHash();

        foreach(Trait trait in traitContainer)
        {
            trait.setIdleAnimationOnApplication(animationManager);
        }
    }

    public virtual void spawningActions()
    {
        //Empty on Purpose
    }

    public virtual AbilityMenuManager getAbilityMenuManager()
    {
        return null;
    }

    public virtual void setOutline()
    {
        outline.createOutline(getOutlineColor());
    }

    public virtual void setOutline(byte alpha)
    {
        Color32 color = getOutlineColor();

        color.a = alpha;

        outline.createOutline(color);
    }

    public virtual void removeOutline()
    {
        outline.removeOutline();
    }

    public virtual bool multiSpaceEnemy()
    {
        return false;
    }

    public virtual void destroyCombatSprite()
    {
        Destroy(combatSprite);
    }

    public void playSpawnAnimation()
    {
        animationManager.playSpawnAnimation();
    }

    public void instateEnvironmentalCombatAction()
    {
        EnvironmentalCombatActionManager.instateEnvironmentalCombatAction(this);
    }

    public virtual void removeFromGrid()
    {
        foreach (GridCoords coords in positions)
        {
            if (CombatGrid.getCombatantAtCoords(coords) == this)
            {
                CombatGrid.setCombatantAtCoords(coords, null);
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

    #region Tutorial
    
    public TutorialSequenceStepTargetObject  tutorialTarget;

    public string getTutorialTargetHash()
    {
        if(getName().Equals(PartyManager.getPlayerStats().getName()))
        {
            return TutorialSequenceList.playerCombatSpriteTargetHash;
        }

        if(isMandatoryTarget())
        {
            return TutorialSequenceList.mandatoryTargetMonsterTargetHash;
        }

        switch(getName())
        {
            case MonsterNameList.armoredBat:
                return TutorialSequenceList.traitMonsterTargetHash;
            default:
                return getName();
        }
    }

    #endregion

    #region HealthBarManager

    public HealthBarManager healthBarManager;

    public void updateHealthBar()
    {
        if (healthBarManager == null || healthBarManager is null)
        {
            return;
        }

        healthBarManager.setLinkedStats(this);
        healthBarManager.setTotalHealth(getTotalHealth());
        healthBarManager.setMissingHealth(getMissingHealth());
        healthBarManager.resetPreviewHealth();
    }

    #endregion

    #region AnimationManager
    public AnimationManager animationManager;

    public virtual void playAnimationOnDamage()
    {
        if (isDead())
        {
            animationManager.playDeathAnimation();
            healthBarManager.hide();
        } else
        {
            animationManager.playWoundedAnimation();
        }
    }

    public void playAttackAnimation()
    {
        if(animationManager == null)
        {
            return;
        }

        animationManager.playAttackAnimation();
    }

    public void playAttackIntoFrontIdleAnimation()
    {
        animationManager.playAttackIntoFrontIdleAnimation();
    }

    public void playAttackIntoSecondaryIdleAnimation()
    {
        animationManager.playAttackIntoSecondaryIdleAnimation();
    }

    public void playSpecialAttackAnimation()
    {
        animationManager.playSpecialAttackAnimation();
    }

    #endregion

    #region Health

    public abstract int getTotalHealth();

    public bool isAlive()
    {
        return currentHealth > 0;
    }

    public bool isDead()
    {
        return currentHealth <= 0;
    }

    public int getMissingHealth()
    {
        return getTotalHealth() - currentHealth;
    }

    public bool hasHealthBarWithPreview()
    {
        if (healthBarManager == null)
        {
            return false;
        }

        return healthBarManager.getMissingHealth() == getMissingHealth() &&
                healthBarManager.getTotalHealth() == getTotalHealth();
    }

    public void fullHeal()
    {
        modifyCurrentHealth(getTotalHealth(), true);
    }

    // public void modifyCurrentHealth(int changeInHealth)
    // {
    //     modifyCurrentHealth(changeInHealth, false);
    // }

    public void modifyCurrentHealth(int changeInHealth, bool healing = false, bool playAnimation = true)
    {
        int totalHealth = getTotalHealth();

        if (!inPreviewMode && changeInHealth >= getTotalHealth() && !healing && isMaster())
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

                if ((changeInHealth + currentHealth) >= totalHealth)
                {
                    currentHealth = totalHealth; //healed actor to full, set to total to not go over;
                }
                else
                {
                    currentHealth += changeInHealth; //healed enemy, increment by changeInHealth
                }
            }

            if(CombatStateManager.inCombat && playAnimation && !inPreviewMode)
            {
                playAnimationOnDamage();
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

    public virtual int getStrength()
    {
        return 1;
    }

    public virtual bool rollAgainstWoundResistance()
    {
        return failedToResist;
    }

    #endregion

    #region Dexterity + Secondaries

    public virtual int getDexterity()
    {
        return 1;
    }

    public virtual int getExtraArmorFromDexterity()
    {
        return 0;
    }

    #endregion

    #region Wisdom + Secondaries

    public virtual int getWisdom()
    {
        return 1;
    }

    public virtual int getPassiveSlotsUnlocked()
    {
        return 0;
    }

    public virtual int getArmorPenetration()
    {
        return StatBoostSource.calculateAllStatFormulas(getAllStatBoosts(), b => b.getBonusArmorPenetrationFormula());
    }

    public virtual bool rollAgainstMentalResistance()
    {
        return failedToResist;
    }

    #endregion

    #region Charisma + Secondaries

    public virtual int getCharisma()
    {
        return 1;
    }

    public virtual int getZOIStat()
    {
        return getCharisma();
    }

    public virtual int getSynergyCoefficient()
    {
        return 0;
    }

    public int getSynergyModifier()
    {
        int sum = 0;
        int synergyBonus = getSynergyCoefficient();

        foreach (Trait trait in traitContainer)
        {
            if (trait.fromZoneOfInfluence())
            {
                sum += synergyBonus;
            }
        }

        return sum;
    }

    public virtual int getBonusExuberances()
    {
        return 0;
    }

    #endregion

    #endregion

    #region Tertiary Stats

    public virtual string getInvulnerability()
    {
        return "" + StatBoostSource.calculateAllStatFormulas(getAllStatBoosts(), b => b.getInvulnerableFormula());
    }

    public virtual string getVulnerability()
    {
        return "" + StatBoostSource.calculateAllStatFormulas(getAllStatBoosts(), b => b.getVulnerableFormula());
    }

    public virtual string getHealingBoost()
    {
        return "" + StatBoostSource.calculateAllStatFormulas(getAllStatBoosts(), b => b.getHealingBoostFormula());
    }

    #endregion

    #region Combat and Action Arrays
    
    public bool queuedToMove()
    {
        return repositionClone != null;
    }

    public bool isRepositionClone()
    {
        return Helpers.hasQuality<Trait>(traitContainer, t => t.Equals(TraitList.repositioningInvulnerability));
    }

    public virtual bool isPriorityAttacker()
    {
        return false;
    }

    public virtual bool isLowPriorityAttacker()
    {
        return false;
    }

    public void moveTo(List<GridCoords> newCoords, bool moveSprite = true)
    {
        List<GridCoords> oldCoords = positions.Select(p => p.clone()).ToList();

        CombatGrid.removeCombatantFromGrid(this);

        positions = newCoords.Select(p => p.clone()).ToList();

        CombatGrid.addCombatantToGrid(this);

        if (moveSprite && positions.Count > 0)
        {
            updateSpritePosition();
        }
    }

	public virtual void updateSpritePosition()
	{
        int coordsSum = 0;
        GridCoords lowestYPosCoords = positions[0];

        foreach(GridCoords position in positions)
        {
            if(position.sum() > coordsSum)
            {
                coordsSum = position.sum();
                lowestYPosCoords = position;
            }
        }

		combatSprite.transform.position = CombatGrid.getPositionAt(lowestYPosCoords);
	}

    public abstract int getTotalArmorRating();
    public virtual int getTotalArmorShred()
    {
        return StatBoostSource.calculateAllStatFormulas(getAllStatBoosts(), b => b.getArmorShredFormula());
    }

    public string getTotalArmorRatingForDisplay()
    {
        if(getTotalArmorRating() - getTotalArmorShred() < 0)
        {
            return Constants.zeroRating + "%";
        } else
        {
            return (getTotalArmorRating() - getTotalArmorShred()).ToString() + "%";
        }
    }

    public virtual double getCritDamageMultiplier()
    {
        return 1.5;
    }

    public virtual float getSurpriseDamageMultiplier()
    {
        return 1f;
    }

    public bool canPayActionCost(ActionCostType[] costTypes, int[] actionCosts)
    {
        bool[] costsPayable = new bool[costTypes.Length];
        List<ActionCostType> actionCostTypesUnpayable = new List<ActionCostType>();

        int index = -1;
        foreach (ActionCostType costType in costTypes)
        {
            index++;
            if (costType == ActionCostType.None)
            {
                costsPayable[index] = true;
                continue;
            }

            if (costType == ActionCostType.RedKnife ||
                costType == ActionCostType.BlueShield ||
                costType == ActionCostType.YellowThorn ||
                costType == ActionCostType.GreenLeaf
                )
            {
                costsPayable[index] = Exuberances.canPayCost(costType, actionCosts[index]);

                if(!costsPayable[index])
                {
                    actionCostTypesUnpayable.Add(costType);
                }

                continue;
            }

            foreach (Trait trait in traitContainer)
            {
                if (trait == null)
                {
                    continue;
                }

                if (trait.getNumberOfStacks(costType) >= actionCosts[index])
                {
                    costsPayable[index] = true;
                    
                    if(!costsPayable[index])
                    {
                        actionCostTypesUnpayable.Add(costType);
                    }
                }
            }
        }

        ExuberanceTracker.ActionCostCannotBePaid.Invoke(actionCostTypesUnpayable);

        return !costsPayable.Contains(false);
    }

    public int modifyIncomingDamage(int baseDamage, int armorPen)
    {
        // int modifiedDamage = baseDamage;
        int modifiedDamage = (int)(((double)baseDamage) * (1.0 - Armor.getDamageReduction(getTotalArmorRating() - (getTotalArmorShred() + armorPen))));

        int vulnInvulnMod = int.Parse(getVulnerability()) - int.Parse(getInvulnerability());

        modifiedDamage += vulnInvulnMod;

        modifiedDamage -= getSynergyModifier();

        if (modifiedDamage < 1)
        {
            modifiedDamage = 1;
        }

        return modifiedDamage;
    }

    public int modifyIncomingHealing(int baseDamage)
    {
        string healingBoost = getHealingBoost();

        if(healingBoost.Length <= 0)
        {
            return baseDamage;
        }

        return baseDamage + int.Parse(healingBoost);
    }

    public virtual bool costsPartyCombatActions()
    {
        return false;
    }

    public void payActionCost(ActionCostType[] costTypes, int[] actionCosts)
    {
        for (int index = 0; index < costTypes.Length && index < actionCosts.Length; index++)
        {
            switch(costTypes[index])
            {
                case ActionCostType.None:
                    continue;
                case ActionCostType.RedKnife:
                case ActionCostType.BlueShield:
                case ActionCostType.YellowThorn:
                case ActionCostType.GreenLeaf:
                    Exuberances.payCost(costTypes[index], actionCosts[index]);
                    continue;
                default:
                    Trait costTrait = Helpers.getObjectWithQuality<Trait>(traitContainer, t => t.hasActionCostType(costTypes[index]));

                    if (costTrait != null)
                    {
                        costTrait.removeStacks(costTypes[index], actionCosts[index]);
                    }
                    break;
            }
        }
    }

    public void refundActionCost(ActionCostType[] costTypes, int[] actionCosts)
    {
        for (int index = 0; index < costTypes.Length && index < actionCosts.Length; index++)
        {
            switch(costTypes[index])
            {
                case ActionCostType.None:
                    continue;
                case ActionCostType.RedKnife:
                case ActionCostType.BlueShield:
                case ActionCostType.YellowThorn:
                case ActionCostType.GreenLeaf:
                    Exuberances.addExuberance(costTypes[index], actionCosts[index]);
                    continue;
                default:
                    Trait costTrait = Helpers.getObjectWithQuality<Trait>(traitContainer, t => t.hasActionCostType(costTypes[index]));

                    if (costTrait != null)
                    {
                        costTrait.removeStacks(costTypes[index], actionCosts[index]);
                    }
                    break;
            }
        }
    }

    public bool shouldTargetEnemy()
    {
        return positions.Any(p => CombatGrid.positionIsOnAlliedSide(p));
    }

    public virtual CombatActionArray getActionArray()
    {
        return new CombatActionArray(null);
    }

    public bool mandatoryTargetForTargetingType(Trait targetTrait)
    {
        if(targetTrait == null)
        {
            return false;
        }

        switch(targetTrait.getName())
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

    public bool isMandatoryTarget()
    {
        return Helpers.hasQuality<Trait>(traitContainer, t => t.isMandatoryTarget());
    }

    public bool isTargetable()
    {
        return !Helpers.hasQuality<Trait>(traitContainer, t => t.isUntargetable());
    }

    public virtual int getBonusAbilityDamage()
    {
        return 0;
    }

    public virtual string getBonusCritChance()
    {
        string bonusCritChance = "" + StatBoostSource.calculateAllStatFormulas(getAllStatBoosts(), b => b.getCritFormula());

        if (bonusCritChance == null || 
            bonusCritChance.Length <= 0 || 
            bonusCritChance.Equals(Constants.zeroRating) || 
            bonusCritChance.Equals(Constants.zeroBonus))
        {
            return "";
        }

        return "+" + bonusCritChance;
    }

    public virtual void evolve()
    {
        //left intentionally blank
    }

    public virtual void devolve()
    {
        //left intentionally blank
    }

    public virtual float getDevastatingCriticalPercentage()
    {
        return 0f;
    }

    #region Volley
    public virtual bool isPartOfVolley()
    {
        return false;
    }

    public virtual int getVolleyAccuracy()
    {
        return Constants.perfectVolleyAccuracy;
    }

    public virtual int getBonusVolleyAccuracy()
    {
        return Constants.sizeZero;
    }


    public abstract string getVolleyAnimationType();
    #endregion
    #endregion

    #region Traits

    public bool isMaster()
    {
        return hasTrait(TraitList.master);
    }

    public bool isMinion()
    {
        return hasTrait(TraitList.minion);
    }

    public bool isSummon()
    {
        return hasTrait(TraitList.summoned);
    }

    public virtual bool isLarge()
    {
        return hasTrait(TraitList.large);
    }

    public void addTrait(Trait newTrait)
    {
        if (newTrait == null || (isDead() && CombatStateManager.whoseTurn != WhoseTurn.Start))
        {
            return;
        }

        newTrait = newTrait.clone();
        newTrait.onApplication();
        newTrait.setTraitHolder(this);

        if (CombatStateManager.inCombat)
        {
            newTrait.setIdleAnimationOnApplication(animationManager);
        }

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
                                         CombatAnimationManager.getInstance().damageNumberCanvas,
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
            if(!isDead())
            {
                traitToRemove.setIdleAnimationOnRemoval(animationManager);
            }
            
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

    public bool isSlowed()
    {
        return Helpers.hasQuality<Trait>(traitContainer, t => t.slowsTraitHolder());
    }

    public bool isStunned()
    {
        if(Helpers.hasQuality<Trait>(traitContainer, t => t.immuneToStun()))
        {
            return false;
        }

        return Helpers.hasQuality<Trait>(traitContainer, t => t.preventsCombatAction());
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

    public bool hasUnusedDeathEffect()
    {
        return Helpers.hasQuality<Trait>(traitContainer, t => t.hasUnusedOnDeathEffect()) || inOnDeathEffect;
    }

    public virtual bool notResurrectable()
    {
        return Helpers.hasQuality<Trait>(traitContainer, t => t.preventsResurrection());
    } 

    public bool isBuffed()
    {
        return Helpers.hasQuality<Trait>(traitContainer, t => t.isBuff());
    } 

    public bool isDebuffed()
    {
        return Helpers.hasQuality<Trait>(traitContainer, t => t.isDebuff());
    } 

    public bool isFrontline()
    {
        return hasTrait(TraitList.frontLine);
    } 

    public bool isBackline()
    {
        return hasTrait(TraitList.backLine);
    } 

    #region Zone of Influence

    public virtual ZoneOfInfluenceTrait getZoneOfInfluenceTrait()
    {
        return null;
    }

    public void removeAllZoneOfInfluenceTraits()
    {
        traitContainer.removeAllTraitsOfType(TraitType.Influence);
    }
    
    #endregion

    #endregion

    #region Equipment

    public virtual EquippedItems getEquippedItems()
    {
        return null;
    }

    public virtual bool hasAvailableWeaponSlots()
    {
        return false;
    }

    public virtual int getWeaponSlots()
    {
        return 0;
    }

    #endregion

    #region Miscellaneous

    public abstract List<GridCoords> findLocationToSpawn();

    public bool isMultiTile()
    {
        return positions.Count > Constants.sizeOne;
    }

    public void disablePolygonCollider()
    {
        if(animationManager != null && animationManager.polygonCollider2D != null)
        {
            animationManager.polygonCollider2D.enabled = false;
        }
    }

    public virtual string getGenderMarker()
    {
        return "";
    }

    public virtual string getAnimationSuffixes()
    {
        return "";
    }

    public virtual List<StatBoostSource> getAllStatBoosts()
    {
        return StatBoostSource.getAllStatBoosts(traitContainer);
    }

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

            return stats.positions.Any(p => positions.Contains(p)) && stats.getName().Equals(getName());
        } else
        {
            return stats.getName().Equals(getName());
        }
    }

    public override int GetHashCode()
    {
        return getName().GetHashCode();
    }

    public virtual bool removableFromFormation()
    {
        return true;
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

    private Sprite getHeadSprite()
    {
        return EnemyTypeFolderPathList.getHeadSprite(getName());
    }

    public void setHeadSprite(DescriptionPanel panel)
    {
        DescriptionPanel.setImage(panel.typeIconPanel, getHeadSprite());
    }

    public void setHeadSprite(Image image)
    {
        if(image != null)
        {
            image.sprite = getHeadSprite();
        }
    }

    public void setHeadSprite(SpriteRenderer spriteRenderer)
    {
        if(spriteRenderer != null)
        {
            spriteRenderer.sprite = getHeadSprite();
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

        clone.traitContainer = traitContainer.clone(clone);

        return clone;
    }

    public Stats getPreviewClone()
    {
        Stats previewClone = clone();

        previewClone.inPreviewMode = true;

        return previewClone;
    }

    #endregion

    #region IDescribable

    public virtual string getName()
    {
        return characterName;
    }

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
        string panelName = "";

        switch (type)
        {
            case PanelType.PartyScreenStats:
                panelName = PrefabNames.partyMemberStatsScreenDescPanel;
                break;
            default:
                panelName = PrefabNames.statsDescriptionPanel;
                break;
        }
        return Resources.Load<GameObject>(panelName);
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

        DescriptionPanel.setText(panel.nameText, getName().Replace(PartyManager.playerMarker, ""));
        DescriptionPanel.setText(panel.hpText, currentHealth + " / " + getTotalHealth());
        DescriptionPanel.setText(panel.armorRatingText, getTotalArmorRatingForDisplay());
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

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName().Replace(PartyManager.playerMarker, "")));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getHealthBlock(currentHealth, getTotalHealth()));

        if(getTotalArmorShred() > 0)
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getArmorBlock(getTotalArmorRatingForDisplay(), getTotalArmorRating() + " - " + getTotalArmorShred()));
        } else
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getArmorBlock(getTotalArmorRatingForDisplay()));
        }

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getInvulnerableBlock(getInvulnerability()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getVulnerableBlock(getVulnerability()));

        return buildingBlocks;
    }

    public bool requiresInspectNode()
    {
        return false;
    }

    #endregion

}
