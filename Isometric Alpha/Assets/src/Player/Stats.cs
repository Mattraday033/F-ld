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

    public const string zoiTraitName = "'s Influence";
    public const string zoiTraitDescription = "The benefits of a Zone of Influence are being applied to this creature.";

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

    public GameObject combatSprite;
    public Stats repositionClone;
    //used to track if Reposition Ability
    //is already moving creature

    public string combatSpriteName;

    public int currentHealth;

    public Trait[] traits = new Trait[0];
    public Trait[] hiddenTraits = new Trait[0];

    #endregion

    #region Constructors

    public Stats(string name)
    {
        this.characterName = name;
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

        if (notResurrectable())
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
    }

    public virtual GridCoords getPositionToHit(Selector selector, int skips)
    {
        return position.clone();
    }

    public virtual string getCombatSpriteName()
    {
        return combatSpriteName;
    }

    public virtual GameObject instantiateCombatSprite()
    {
        combatSprite = Instantiate(Resources.Load<GameObject>(getCombatSpriteName()), CombatStateManager.getCreatureParent());

        setUpComponents(combatSprite.GetComponent<ComponentList>());

        return combatSprite;
    }

    public virtual void setUpComponents(ComponentList list)
    {
        healthBarManager = list.healthBarManager;
        updateHealthBar();

        animationManager = list.animationManager;
        animationManager.healthBarManager = healthBarManager;
        animationManager.setAnimations(getName());

        spriteRenderer = list.spriteRenderer;

        outline = new SpriteOutline();
        outline.setSpriteRenderer(spriteRenderer);

        tutorialTarget = list.tutorialTarget;
        tutorialTarget.tutorialHash = getTutorialTargetHash();

        list.combatantHover.linkedStats = this;

        foreach(Trait trait in traits)
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

        if (!inPreviewMode && changeInHealth >= getTotalHealth() && !healing && hasTraitAtIndex(TraitList.master) >= 0)
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

        foreach (Trait trait in traits)
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

    #region Combat and Action Arrays
    
    public bool queuedToMove()
    {
        return repositionClone != null;
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

    public string getTotalArmorRatingForDisplay()
    {
        return getTotalArmorRating() + "";
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
                continue;
            }

            foreach (Trait trait in traits)
            {
                if (trait == null)
                {
                    continue;
                }

                if (trait.getNumberOfStacks(costType) >= actionCosts[index])
                {
                    costsPayable[index] = true;
                }
            }
        }

        return !costsPayable.Contains(false);
    }

    public int modifyOutgoingDamage(int baseDamage)
    {
        int bonusDamage = 0;
        Trait[] traitList = getTraits();

        foreach (Trait trait in traitList)
        {
            if (trait != null)
            {
                bonusDamage += trait.getBonusDamageDealt();
            }
        }

        bonusDamage += getSynergyModifier();

        return (baseDamage + bonusDamage);
    }

    public int modifyIncomingDamage(int baseDamage)
    {
        int modifiedDamage = (int)(((double)baseDamage) * (1.0 - Armor.getDamageReduction(getTotalArmorRating())));

        Trait[] traitList = getTraits();

        foreach (Trait trait in traits)
        {
            if (trait != null)
            {
                modifiedDamage = trait.addBonusDamageTaken(modifiedDamage);
            }
        }

        foreach (Trait trait in traitList)
        {
            if (trait != null &&
              !(trait.getName().Equals(TraitList.repositioningInvulnerability.getName()) && CombatStateManager.whoseTurn != WhoseTurn.Resolving))
            {
                modifiedDamage = trait.reduceDamageByPercentage(modifiedDamage);

                if (modifiedDamage == 0)
                {
                    return 0;
                }
            }
        }

        modifiedDamage -= getSynergyModifier();

        if (modifiedDamage < 1)
        {
            modifiedDamage = 1;
        }

        return modifiedDamage;
    }

    public bool wasSummoned()
    {
        return hasTrait(TraitList.summoned);
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

            Trait costTrait = Helpers.getObjectWithQuality<Trait>(traits, t => t.hasActionCostType(costTypes[index]));

            if (costTrait != null)
            {
                costTrait.removeStacks(costTypes[index], actionCosts[index]);
            }
        }
    }

    public virtual bool isPartOfVolley()
    {
        return false;
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
        return Helpers.hasQuality<Trait>(traits, t => t.isMandatoryTarget());
    }

    public bool isTargetable()
    {
        return !Helpers.hasQuality<Trait>(hiddenTraits, hT => hT.isUntargetable());
    }

    public virtual int getBonusAbilityDamage()
    {
        return 0;
    }

    public string getBonusCritChance()
    {
        string bonusCritChance = DamageCalculator.combineFormulas(getBonusCritChanceFromArmor(), getBonusCritChanceFromTraits());

        if (bonusCritChance == null || bonusCritChance.Length <= 0 || bonusCritChance.Equals("0") || bonusCritChance.Equals("+0"))
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

    #endregion

    #region Traits

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
        
        dealTraitApplicationDamage(newTrait);

        if (hasTraitAtIndex(newTrait) >= 0)
        {
            traits[hasTraitAtIndex(newTrait)].reapply();
        }
        else
        {
            traits = Helpers.appendArray<Trait>(traits, newTrait);
        }

        Trait.OnTraitApplication.Invoke(newTrait);
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
            return Helpers.sum<Trait>(traits, t => t.damageOnDebuffApplication());
        }
        else if (newTrait.isBuff())
        {
            return Helpers.sum<Trait>(traits, t => t.damageOnBuffApplication());
        }
        else
        {
            return 0;
        }
    }

    public void addTraits(Trait[] newTraits)
    {
        foreach (Trait trait in newTraits)
        {
            addTrait(trait);
        }
    }

    public Trait[] getTraits()
    {
        return traits;
    }

    public void removeTrait(Trait traitToRemove)
    {
        List<Trait> newTraits = new List<Trait>();
        Trait removedTrait = null;

        foreach (Trait trait in traits)
        {
            if (!trait.getName().Equals(traitToRemove.getName()))
            {
                newTraits.Add(trait);
            }
            else
            {
                removedTrait = trait;
            }
        }

        traits = newTraits.ToArray();

        if(removedTrait != null)
        {
            removedTrait.setIdleAnimationOnRemoval(animationManager);
            Trait.OnTraitRemoval.Invoke(removedTrait);
        }
    }

    public void removeAllTraits()
    {
        traits = new Trait[0];
    }

    public void removeAllTraitsOfType(string traitType)
    {
        Trait[] newTraits = new Trait[traits.Length];

        for (int index = 0; index < traits.Length; index++)
        {
            if (traits[index] != null && !traits[index].getType().Equals(traitType))
            {
                newTraits[index] = traits[index];
            }
        }

        traits = newTraits.Where(t => t != null).ToArray();
    }

    public virtual void addEquippedPassiveTraits()
    {
        //empty on purpose
    }

    public string getBonusCritChanceFromTraits()
    {
        return "" + Helpers.sum<Trait>(traits, t => t.getBonusCritChance());
    }

    public bool isSlowed()
    {
        return Helpers.hasQuality<Trait>(traits, t => t.slowsTraitHolder());
    }

    public bool isStunned()
    {
        if (traits.Length < 1)
        {
            return false;
        }

        int index = 0;
        foreach (Trait trait in traits)
        {
            index++;
        }

        int stunnedStatus = traits.Aggregate(0, (status, trait) => status += Convert.ToInt32(trait.preventsCombatAction()));

        if (stunnedStatus > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void removeTraitsRemovedByDamage()
    {
        if (traits == null)
        {
            return;
        }

        Trait[] newTraits = new Trait[0];

        for (int index = 0; index < traits.Length; index++)
        {
            if (!traits[index].isRemovedOnDamage())
            {
                newTraits = Helpers.appendArray<Trait>(newTraits, traits[index]);
            }
        }

        traits = newTraits;
    }

    public void prepareOnDeathEffects()
    {
        foreach (Trait trait in traits)
        {
            if (trait != null)
            {
                trait.onDeathEffect(this);
            }
        }
    }

    public bool hasTrait(Trait trait)
    {
        return Helpers.hasQuality<Trait>(traits, t => t.getName().Equals(trait.getName()));
    }

    public int hasTraitAtIndex(Trait traitToCheck)
    {
        Trait[] traitList = getTraits();

        int traitIndex = 0;
        foreach (Trait trait in traitList)
        {
            if (trait != null && trait.getName().Equals(traitToCheck.getName()))
            {
                return traitIndex;
            }

            traitIndex++;
        }

        return -1;
    }

    public bool hasTraitOfType(string traitTypeToCheck)
    {
        Trait traitOfType = getTraitOfType(traitTypeToCheck);

        return traitOfType != null && !(traitOfType is null);
    }

    public Trait getTraitOfType(string traitTypeToCheck)
    {
        Trait[] traitList = getTraits();

        int traitIndex = 0;
        foreach (Trait trait in traitList)
        {
            if (trait != null && trait.getType().Equals(traitTypeToCheck))
            {
                return trait;
            }

            traitIndex++;
        }

        return null;
    }
    public void addHiddenTrait(Trait trait)
    {
        if (hiddenTraits == null || hiddenTraits is null)
        {
            hiddenTraits = new Trait[0];
        }

        hiddenTraits = Helpers.appendArray<Trait>(hiddenTraits, trait);
    }

    private void harmAllLinkedTargets(int damage)
    {
        foreach (Trait trait in traits)
        {
            if (trait != null)
            {
                trait.harmAllLinkedTargets(damage);
            }
        }
    }

    public virtual bool notResurrectable()
    {
        return Helpers.hasQuality<Trait>(traits, (t => t.preventsResurrection()));
    } 

    public bool isBuffed()
    {
        return Helpers.hasQuality<Trait>(traits, (t => t.isBuff()));
    } 

    public bool isDebuffed()
    {
        return Helpers.hasQuality<Trait>(traits, (t => t.isDebuff()));
    } 

    #region Zone of Influence

    public virtual ZoneOfInfluenceTrait getZoneOfInfluenceTrait()
    {
        return null;
    }

    public void removeAllZoneOfInfluenceTraits()
    {
        if (traits == null)
        {
            traits = new Trait[0];
            return;
        }

        Trait[] nonZOITraits = new Trait[0];

        foreach (Trait trait in traits)
        {
            if (trait != null && !trait.fromZoneOfInfluence())
            {
                nonZOITraits = Helpers.appendArray<Trait>(nonZOITraits, trait);
            }
        }

        traits = nonZOITraits;
    }
    
    #endregion

    #endregion

    #region Equipment

    public double getCurrentTotalArmorPercentage()
    {
        double currentTotalArmorPercentage = 1.0 - Helpers.sum<Trait>(traits, t => t.getPercentageArmorLost());

        if (currentTotalArmorPercentage < 0.0)
        {
            return 0.0;
        }
        else
        {
            return currentTotalArmorPercentage;
        }
    }

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

    public virtual string getBonusCritChanceFromArmor()
    {
        return "";
    }

    #endregion

    #region Miscellaneous

    public abstract IDescribable getHoverPanelDescribable();

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

    public Stats clone()
    {
        Stats clone = (Stats)Clone();

        clone.repositionClone = null;
        clone.position = position.clone();

        if (traits == null)
        {
            traits = new Trait[0];
        }

        if (hiddenTraits == null)
        {
            hiddenTraits = new Trait[0];
        }

        clone.traits = new Trait[traits.Length];
        clone.hiddenTraits = new Trait[hiddenTraits.Length];

        for (int index = 0; index < clone.traits.Length; index++)
        {
            clone.traits[index] = traits[index].clone();
        }

        for (int index = 0; index < clone.hiddenTraits.Length; index++)
        {
            clone.hiddenTraits[index] = hiddenTraits[index].clone();
        }

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

    public string getName()
    {
        return characterName;
    }

    public bool ineligible()
    {
        return false;
    }

    public virtual GameObject getRowType(RowType rowType)
    {
        return null;
    }

    public GameObject getDescriptionPanelFull()
    {
        return getDescriptionPanelFull(PanelType.Standard);
    }

    public GameObject getDescriptionPanelFull(PanelType type)
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

    public GameObject getDecisionPanel()
    {
        return null;
    }

    public bool withinFilter(string[] filterParameters)
    {
        return true;
    }

    public virtual void describeSelfFull(DescriptionPanel panel)
    {
        panel.setObjectBeingDescribed(this);

        DescriptionPanel.setText(panel.nameText, getName().Replace(PartyManager.playerMarker, ""));
        DescriptionPanel.setText(panel.hpText, currentHealth + " / " + getTotalHealth());
        DescriptionPanel.setText(panel.armorRatingText, getTotalArmorRatingForDisplay());

        DescriptionPanel nestedPanel = panel.getNestedDescriptionPanel();

        if (nestedPanel != null)
        {
            getHoverPanelDescribable().describeSelfFull(nestedPanel);
        }
    }

    public void describeSelfRow(DescriptionPanel panel)
    {
        describeSelfFull(panel);
    }

    public void setUpDecisionPanel(IDecisionPanel descisionPanel)
    {

    }

	public List<IDescribable> getRelatedDescribables()
	{
		return new List<IDescribable>();
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


        buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName().Replace(PartyManager.playerMarker, "")));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getHealthBlock(currentHealth.ToString()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getArmorBlock(getTotalArmorRatingForDisplay()));


        return buildingBlocks;
    }

    #endregion


    public override bool Equals(object obj)
    {
        Stats stats = obj as Stats;

        if (stats == null)
        {
            return false;
        }

        return stats.position.Equals(position) && stats.getName().Equals(getName());
    }

}
