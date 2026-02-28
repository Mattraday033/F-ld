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

    private string characterName;

    public GridCoords position;

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

    public void setPreviousColor(Color newColor)
    {
        if (previousColor.Equals(Color.clear) && !newColor.Equals(Color.clear))
        {
            previousColor = Helpers.cloneColor(newColor);
        }
    }

    public virtual void setToDeadSprite()
    {
        if (CombatStateManager.whoseTurn != WhoseTurn.Resolving && CombatStateManager.whoseTurn != WhoseTurn.Start )
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
        return position.clone();
    }

    public virtual string getCombatSpriteName()
    {
        return combatSpriteName;
    }

    public virtual GameObject instantiateCombatSprite(GridCoords initialPosition)
    {
        combatSprite = Instantiate(Resources.Load<GameObject>(getCombatSpriteName()), CombatStateManager.getCreatureParent());

        position = initialPosition.clone();

        setUpComponents(combatSprite.GetComponent<ComponentList>());

        moveTo(position);

        return combatSprite;
    }

    public virtual void setUpComponents(ComponentList list)
    {
        healthBarManager = list.healthBarManager;
        updateHealthBar();

        list.combatantHover.linkedStats = this;

        spriteRenderer = list.spriteRenderer;

        outline = new SpriteOutline();
        outline.setSpriteRenderer(spriteRenderer);

        animationManager = list.animationManager;
        animationManager.healthBarManager = healthBarManager;
        animationManager.setAnimations(getName() + getGenderMarker() + getAnimationSuffixes());

        tutorialTarget = list.tutorialTarget;
        tutorialTarget.tutorialHash = getTutorialTargetHash();

        foreach(Trait trait in traitContainer)
        {
            trait.setIdleAnimationOnApplication(animationManager);
        }
    }

    public virtual AbilityMenuManager getAbilityMenuManager()
    {
        return null;
    }

    public virtual void setOutline()
    {
        outline.createOutline(getOutlineColor());
    }

    public virtual void removeOutline()
    {
        outline.removeOutline();
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
        CombatGrid.setCombatantAtCoords(position, null);
    }

    public virtual bool isInsideCoordinates(GridCoords coords)
    {
        return coords.Equals(position);
    }

    public virtual bool isInsideCoordinates(GridCoords[] coords)
    {
        return coords.Contains(position);
    }

    #endregion

    #region Tutorial
    
    public TutorialSequenceStepTargetObject tutorialTarget;

    public string getTutorialTargetHash()
    {
        if(getName().Equals(PartyManager.getPlayerStats().getName()))
        {
            return TutorialSequenceList.playerCombatSpriteTargetHash;
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

    public void modifyCurrentHealth(int changeInHealth)
    {
        modifyCurrentHealth(changeInHealth, false);
    }

    public void modifyCurrentHealth(int changeInHealth, bool healing)
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

    public virtual bool rollAgainstPhysicalResistance()
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
        return StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusArmorPenetrationFormula());
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
        return "" + StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getInvulnerableFormula());
    }

    public virtual string getVulnerability()
    {
        return "" + StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getVulnerableFormula());
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

    public void moveTo(GridCoords newCoords)
    {
        moveTo(newCoords, true);
    }

    public void moveTo(GridCoords newCoords, bool moveSprite)
    {
        GridCoords oldCoords = position.clone();

        //adjustCombatActionsActorCoords(oldCoords, newCoords);

        if (CombatGrid.getCombatantAtCoords(oldCoords) == this)
        {
            CombatGrid.setCombatantAtCoords(oldCoords, null);
        }

        CombatGrid.setCombatantAtCoords(newCoords, this);

        position = newCoords.clone();

        if (moveSprite)
        {
            CombatGrid.updateStatsSpritePosition(newCoords);
        }

        EnvironmentalCombatActionManager.getInstance().updateEnvironmentalCasterPosition(oldCoords, newCoords);
    }

    public abstract int getTotalArmorRating();
    public virtual int getTotalArmorShred()
    {
        return StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getArmorShredFormula());
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

    public virtual bool costsPartyCombatActions()
    {
        return false;
    }

    public void payActionCost(ActionCostType[] costTypes, int[] actionCosts)
    {
        for (int index = 0; index < costTypes.Length && index < actionCosts.Length; index++)
        {
            if (costTypes[index] == ActionCostType.None)
            {
                continue;
            }

            if (costTypes[index] == ActionCostType.RedKnife ||
                costTypes[index] == ActionCostType.BlueShield ||
                costTypes[index] == ActionCostType.YellowThorn ||
                costTypes[index] == ActionCostType.GreenLeaf
               )
            {
                Exuberances.payCost(costTypes[index], actionCosts[index]);
                continue;
            }

            Trait costTrait = Helpers.getObjectWithQuality<Trait>(traitContainer, t => t.hasActionCostType(costTypes[index]));

            if (costTrait != null)
            {
                costTrait.removeStacks(costTypes[index], actionCosts[index]);
            }
        }
    }

    public bool shouldTargetEnemy()
    {
        if (CombatGrid.positionIsOnAlliedSide(position))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual CombatActionArray getActionArray()
    {
        return new CombatActionArray(null);
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
        string bonusCritChance = "" + StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getCritFormula());

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

    public void addTrait(Trait newTrait)
    {
        if (newTrait == null || isDead())
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
                DamageNumberPopup.create(traitApplicationDamage,
                                     CombatGrid.getPositionAt(position),
                                     DamageNumberPopup.getDirectionByTargetCoords(position),
                                     CombatAnimationManager.getInstance().damageNumberCanvas,
                                     isNotACrit,
                                     doesNotHealTarget,
                                     traitApplicationDamageFrameDelay);
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
            traitToRemove.setIdleAnimationOnRemoval(animationManager);
            Trait.OnTraitRemoval.Invoke(traitToRemove);
        }
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
        return Helpers.hasQuality<Trait>(traitContainer, t => t.preventsCombatAction());
    }

    private void removeTraitsRemovedByDamage()
    {
        traitContainer.removeAllTraitsRemovedByDamage();
    }

    public void prepareOnDeathEffects()
    {
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

    public abstract GridCoords findLocationToSpawn();

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

        if(CombatStateManager.inCombat)
        {
            return stats.position.Equals(position) && stats.getName().Equals(getName());
        } else
        {
            return stats.getName().Equals(getName());
        }
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

    public bool isInParty(Stats[][] positionGrid)
    {
        for (int row = 0; row < positionGrid.Length; row++)
        {
            for (int col = 0; col < positionGrid[row].Length; col++)
            {
                if (positionGrid[row][col] == this)
                {
                    return true;
                }
            }
        }

        return false;
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
        clone.position = position.clone();

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
