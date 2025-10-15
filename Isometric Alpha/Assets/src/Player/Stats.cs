using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


[CreateAssetMenu]
public class Stats : ScriptableObject, ICloneable, IDescribable, IDescribableInBlocks
{
    public static UnityEvent OnHealthChange = new UnityEvent();
    public static UnityEvent OnStatsChange = new UnityEvent();

	public const string zoiTraitName = "'s Influence";
	public const string zoiTraitDescription = "The benefits of a Zone of Influence are being applied to this creature.";

	private const int traitApplicationDamageFrameDelay = 2;
	private const bool doesNotHealTarget = false;
	private const bool isNotACrit = false;

	public static UnityEvent PredationProc = new UnityEvent();

	public GameObject combatSprite;
	public string combatSpriteName;
	public Vector3 adjustment;

	public static Vector3 healthBarAdjustment = new Vector3(0, CombatGrid.gridSpaceIncrementY * 3.5f, 0);
	public GameObject healthBar;
	public HealthBarManager healthBarManager;

	public GridCoords position;

	public static Color32 green = new Color32(0, 175, 55, 255); //standard health bar Color's in hex
	public static Color32 yellow = new Color32(240, 240, 0, 255);
	public static Color32 red = new Color32(200, 10, 0, 255);

	public Color previousColor = Color.clear;

	public string name;

	public bool isDead;

	public Trait[] traits = new Trait[0];
	public Trait[] hiddenTraits = new Trait[0];

	public string[] traitNames;

	public int currentHealth;

	public Stats(string name)
	{
		this.name = name;
	}

	public Stats(string name, int cHP)
	{
		this.name = name;

		this.currentHealth = cHP;
	}

	public Stats(GameObject combatSprite, string combatSpriteName, string name, int cHP)
	{

		this.combatSprite = combatSprite;
		this.combatSpriteName = combatSpriteName;

		this.name = name;

		this.currentHealth = cHP;
	}

	public virtual int getLevel()
	{
		return -1;
	}

	public virtual void setLevel(int newLevel)
	{

	}

	public virtual void incrementLevel()
	{

	}

	public virtual int getTotalHealth()
	{
		throw new Exception("getTotalHealth() was called via the base class instead of using the proper overriding method.");
	}

	public virtual int getTotalArmorRating()
	{
		throw new Exception("getTotalArmorRating() was called via the base class instead of using the proper overriding method.");
	}

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

	public virtual AbilityMenuManager getAbilityMenuManager()
	{
		return null;
	}

	public virtual bool isPriorityAttacker()
	{
		return false;
	}

	public virtual bool isLowPriorityAttacker()
	{
		return false;
	}

    public virtual string getCombatSpriteName()
    {
        return combatSpriteName;
    }
    
    public virtual GameObject instantiateCombatSprite()
    {
        combatSprite = Instantiate(Resources.Load<GameObject>(getCombatSpriteName()));

        return combatSprite;
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

	public bool canPayActionCost(ActionCostType[] costTypes, int[] actionCosts)
	{
		bool[] costsPayable = new bool[costTypes.Length];

		int index = 0;
		foreach (ActionCostType costType in costTypes)
		{
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

			index++;
		}

		return !costsPayable.Contains(false);
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

	public void adjustCombatActionsSecondaryCoords(GridCoords oldCoords, GridCoords newCoords)
	{
		ArrayList listOfCombatActionQueues = new ArrayList();

		listOfCombatActionQueues.Add(EnemyCombatActionManager.enemyCombatActionQueue);
		listOfCombatActionQueues.Add(EnemyCombatActionManager.slowedEnemyCombatActionQueue);
		listOfCombatActionQueues.Add(PlayerCombatActionManager.playerCombatActionQueue);
		listOfCombatActionQueues.Add(SummonsCombatActionManager.alliedSummonsCombatActionQueue);
		listOfCombatActionQueues.Add(SummonsCombatActionManager.enemySummonsCombatActionQueue);
		listOfCombatActionQueues.Add(CombatActionManager.lockedInCombatActionQueue);
		listOfCombatActionQueues.Add(CombatActionManager.onDeathCombatActionQueue);

		foreach (ArrayList actionQueue in listOfCombatActionQueues)
		{
			foreach (CombatAction action in actionQueue)
			{
				if (action.getActorCoords().Equals(oldCoords))
				{
					action.setActorCoords(newCoords.clone());
				}
			}
		}
	}

	public virtual Color getOutlineColor()
	{
		return RevealManager.canBeInteractedWith;
	}

	public virtual bool removableFromFormation()
	{
		return true;
	}

	public virtual bool costsPartyCombatActions()
	{
		return false;
	}

	public bool isAlive()
	{
		return currentHealth > 0;
	}

	public virtual bool isPartOfVolley()
	{
		return false;
	}

	public virtual bool wasSummoned()
	{
		return false;
	}

	public void addTrait(Trait newTrait)
	{
		if (newTrait == null || isDead)
		{
			return;
		}

		newTrait = newTrait.clone();
		newTrait.onApplication();
		newTrait.setTraitHolder(this);


		if (traits == null || traits is null)
		{
			traits = TraitList.getListOfTraits(traitNames);
		}

		dealTraitApplicationDamage(newTrait);

		if (hasTrait(newTrait) >= 0)
		{
			traits[hasTrait(newTrait)].reapply();
		}
		else
		{
			traits = Helpers.appendArray<Trait>(traits, newTrait);
		}
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
		if (traits == null || traits is null)
		{
			traits = TraitList.getListOfTraits(traitNames);
		}

		return traits;
	}

	public void removeTrait(Trait traitToRemove)
	{
		Trait[] newTraits = new Trait[0];

		foreach (Trait trait in traits)
		{
			if (!trait.getName().Equals(traitToRemove.getName()))
			{
				newTraits = Helpers.appendArray<Trait>(newTraits, trait);
			}
		}

		traits = newTraits;
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

	public void setUpHealthBar(GameObject healthBar)
	{
		this.healthBar = healthBar;
		this.healthBarManager = healthBar.GetComponent<HealthBarManager>();

		updateHealthBar();
	}

	public void updateHealthBar(int missingHealth)
	{
		updateHealthBar(missingHealth, 0);
	}

	public void updateHealthBar(int missingHealth, int incomingDamage)
	{
		if (healthBar == null || healthBar is null ||
		   healthBarManager == null || healthBarManager is null)
		{
			return;
		}

		healthBarManager.setTotalHealth(getTotalHealth());
		healthBarManager.setMissingHealth(missingHealth);
		healthBarManager.addPreviewHealth(incomingDamage);
	}

	public void updateHealthBar()
	{
		if (healthBar == null || healthBar is null ||
		   healthBarManager == null || healthBarManager is null)
		{
			return;
		}

		healthBarManager.setTotalHealth(getTotalHealth());
		healthBarManager.setMissingHealth(getMissingHealth());
		healthBarManager.resetPreviewHealth();
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

        if (changeInHealth >= getTotalHealth() && !healing && hasTrait(TraitList.master) >= 0)
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

        if (CombatStateManager.inCombat && CombatStateManager.whoseTurn == WhoseTurn.Resolving)
        {
            updateHealthBar();

            if (!healing && PartyManager.getPlayerStats().currentHealth > 0)
            {
                harmAllLinkedTargets(changeInHealth);
            }
        }

        OnHealthChange.Invoke();
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

	public virtual int getExtraArmorFromDexterity()
	{
		return 0;
	}

    public int getMissingHealth()
    {
        return getTotalHealth() - currentHealth;
    }

	//if they are currently in your lineup
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

	//returns index of trait if it has the trait, or -1 if it doesn't
	public int hasTrait(Trait traitToCheck)
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
			Debug.Log("Trait is " + trait.getName() + " and it's type is " + trait.getType());

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

    public virtual int getWeaponSlots()
    {
        return 0;
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

	public virtual GridCoords getPositionToHit(Selector selector, int skips)
	{
		return position.clone();
	}

	public virtual void addEquippedPassiveTraits()
	{
		//empty on purpose
	}

	public virtual void removeActivatedPassiveTraits()
	{
		//empty on purpose
	}

	public virtual Trait getZoneOfInfluenceTrait()
	{
		throw new IOException("getZoneOfInfluenceTrait() was called from it's base class version");
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

	public static string[] getAllZoneOfInfluenceBoostKeys(string[] allBoostKeysAffectingTarget)
	{
		string[] boostKeys = new string[0];

		foreach (string boostKey in allBoostKeysAffectingTarget)
		{
			SecondaryStatBoost statBoost = StatBoostList.getStatBoost(boostKey);

			if (statBoost != null && statBoost.affectsZone)
			{
				boostKeys = Helpers.appendArray<string>(boostKeys, boostKey);
			}
		}

		return boostKeys;
	}

    public virtual int getBonusExuberances()
    {
        return 0;
    }

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

	public void setPreviousColor(Color newColor)
	{
		if (previousColor.Equals(Color.clear) && !newColor.Equals(Color.clear))
		{
			previousColor = Helpers.cloneColor(newColor);
		}
	}

	public virtual void setToDeadSprite()
	{
		setPreviousColor(combatSprite.GetComponent<SpriteRenderer>().color);
		combatSprite.GetComponent<SpriteRenderer>().color = Color.black;

		if (combatSprite.GetComponent<Collider2D>() != null)
		{
			combatSprite.GetComponent<Collider2D>().enabled = false;
		}
		else
		{
			combatSprite.transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
		}

		healthBar.SetActive(false);
		isDead = true;
	}

	public virtual void bringBackFromDeath()
	{
		Debug.LogError("bringBackFromDeath()");

		combatSprite.GetComponent<SpriteRenderer>().color = Helpers.cloneColor(previousColor);

		if (combatSprite.GetComponent<Collider2D>() != null)
		{
			combatSprite.GetComponent<Collider2D>().enabled = true;
		}
		else
		{
			combatSprite.transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
		}

		healthBar.SetActive(true);
		isDead = false;
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

	public bool isSlowed()
	{
		return Helpers.hasQuality<Trait>(traits, t => t.slowsTraitHolder());
	}

	public bool isTargetable()
	{
		return !Helpers.hasQuality<Trait>(hiddenTraits, hT => hT.isUntargetable());
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

	public virtual Color getSpriteColor()
	{
		return Color.white;
	}

	public bool isMandatoryTarget()
	{
		return Helpers.hasQuality<Trait>(traits, t => t.isMandatoryTarget());
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

	public virtual string getBonusCritChanceFromArmor()
	{
		return "";
	}

	public string getBonusCritChanceFromTraits()
	{
		return "" + Helpers.sum<Trait>(traits, t => t.getBonusCritChance());
	}

	public virtual float getDevastatingCriticalPercentage()
	{
		return 0f;
	}

	public virtual void evolve()
	{
		//left intentionally blank
	}

	public virtual void devolve()
	{
		//left intentionally blank
	}

	public virtual int getPassiveSlotsUnlocked()
	{
		return 0; 
	}

    public virtual bool hasSynergy()
    {
        return false;
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



    public virtual int getBonusAbilityDamage()
    {
        return 0;
    }

	public virtual IDescribable getHoverPanelDescribable()
	{
		throw new IOException("Base version of getHoverPanelDescribable() was called erroneously");
	}

	public static AllyStats convertIDescribableToStats(IDescribable describable)
	{
		if (describable as PartyMember != null)
		{
			return (describable as PartyMember).stats;
		}
		else
		{
			return (describable as AllyStats);
		}
	}

	public virtual bool hasAvailableWeaponSlots()
	{
		return false;
	}

    public object Clone()
    {
        return this.MemberwiseClone();
    }

	public Stats clone()
	{
		Stats clone = (Stats)Clone();

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

    public virtual EquippedItems getEquippedItems()
    {
        return null;
    }

    //IDescribable methods
    public string getName()
    {
        return name;
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

		DescriptionPanel.setText(panel.nameText, getName());
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

	public ArrayList getRelatedDescribables()
	{
		return new ArrayList();
	}

	public bool buildableWithBlocks()
	{
		return true;
	}

	public bool buildableWithBlocksRows()
	{
		return true;
	}

	//IDescribableInBlocks methods	
	public virtual List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{

		List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();


		buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getHealthBlock(currentHealth + " / " + getTotalHealth()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getArmorBlock(getTotalArmorRatingForDisplay()));


		return buildingBlocks;
	}

	public virtual int getStrength()
	{
		return 1;
	}

	public virtual int getDexterity()
	{
		return 1; 
	}

	public virtual int getWisdom()
	{
		return 1; 
	}

	public virtual int getCharisma()
	{
		return 1; 
	}

}
