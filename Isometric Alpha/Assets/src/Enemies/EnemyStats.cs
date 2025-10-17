using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct SpawnDetails
{
	public bool hasSpawnDetails;
	public bool dontSpawnWhenSurprised;
	
	public GridCoords[] allSpawnPositions; //every coords that has a reference to the enemy's stats
	public GridCoords baseStatsPosition;   //the coords put into the "position" of the base class
	public GridCoords spritePosition;      //the coords that the sprite is placed at on the grid
	
	public SpawnDetails(GridCoords[] allSpawnPositions, GridCoords baseStatsPosition, GridCoords spritePosition, bool dontSpawnWhenSurprised)
	{
		this.allSpawnPositions = allSpawnPositions;
		this.baseStatsPosition = baseStatsPosition;
		this.spritePosition = spritePosition;
		this.dontSpawnWhenSurprised = dontSpawnWhenSurprised;
		
		this.hasSpawnDetails = false;
	}
}

public class EnemyStats : Stats
{
	[SerializeField]
	private bool priorityAttacker;
	[SerializeField]
	private bool lowPriorityAttacker;

	public int armor;

	public SpawnDetails spawnDetails;

	public static UnityEvent OnMinionSummonDeath = new UnityEvent();
	public static UnityEvent OnEnemyDeath = new UnityEvent();

	[SerializeField]
	private int totalHealth;

	private CombatAction combatAction;

    public EnemyStats(string key, int armor, int tHP):
    base(key)
    {
        this.armor = armor;

		this.totalHealth = tHP;
    }

    public EnemyStats(string key, int armor, int tHP, CombatAction combatAction, Trait[] traits) :
    base(key)
    {
        this.armor = armor;

        this.totalHealth = tHP;

        this.combatAction = combatAction.clone();

        foreach (Trait trait in traits)
        {
            addTrait(trait);
        }
    }

    public override GameObject instantiateCombatSprite()
    {
        combatSprite = Instantiate(Resources.Load<GameObject>(PrefabNames.enemySprite));

        combatSprite.transform.localScale = new Vector3(1f, 1f, 1f);

        Helpers.updateGameObjectPosition(combatSprite);

        // EnemyStatsHover statsHover = combatSprite.AddComponent<EnemyStatsHover>();

        // statsHover.stats = this;

        return combatSprite;
    }

	public override int getTotalArmorRating()
	{
		return (int)((double)armor * getCurrentTotalArmorPercentage());
	}

	public override int getTotalHealth()
	{
		return totalHealth;
	}

	public override Color getOutlineColor()
	{
		return RevealManager.attacksOnSight;
	}

	public virtual void spawningCombatAction()
	{
		traits = TraitList.getListOfTraits(traitNames);
	}

	public void instateEnvironmentalCombatAction()
	{
		// EnvironmentalCombatActionManager.getInstance().instateEnvironmentalCombatAction(environmentalCombatActionKey, environmentalTargetingTraitKey, CombatGrid.getCombatantAtCoords(position));
	}

	public CombatAction getCombatAction()
	{
		if (combatAction == null || combatAction is null)
		{
			return null;
		}

		CombatAction combatActionClone = combatAction.clone();
		combatActionClone.setActorCoords(position);

		return combatActionClone;
	}

	public EnemyStats getSpawnType()
    {
        Debug.LogError("getSpawnType Not Implemented");
		return null;
	}

	public override bool isPriorityAttacker()
	{
		return priorityAttacker;
	}

	public override bool isLowPriorityAttacker()
	{
		return lowPriorityAttacker;
	}

	private void checkForMissingTraitList()
	{
		if (traits == null || traits.Length == 0)
		{
			traits = TraitList.getListOfTraits(traitNames);
		}
	}

	public override GridCoords getPositionToHit(Selector selector, int skips)
	{

		if (spawnDetails.allSpawnPositions == null || spawnDetails.allSpawnPositions.Length <= 1)
		{
			return position.clone();
		}

		GridCoords[] allSelectorCoords = selector.getAllSelectorCoords();
		List<GridCoords> allCompatabilePositions = new List<GridCoords>();


		foreach (GridCoords coords in allSelectorCoords)
		{
			if (spawnDetails.allSpawnPositions.Contains(coords))
			{
				allCompatabilePositions.Add(coords);
			}
		}

		if (allCompatabilePositions.Count == 0 || skips >= allCompatabilePositions.Count)
		{
			return position.clone();
		}
		else
		{
			return allCompatabilePositions[skips];
		}
	}

	public bool isMinion()
	{
		return traits.Contains(TraitList.minion);
	}

	public bool isLarge()
	{
		return traits.Contains(TraitList.large);
	}

	public virtual bool cantBeResurrected()
	{
		return Helpers.hasQuality<Trait>(traits, (t => t.preventsResurrection()));
	}

	public override void setToDeadSprite()
	{
		if (CombatStateManager.whoseTurn != WhoseTurn.Resolving)
		{
			return;
		}

		CombatStateManager.deadMonsterCount++;

		if (isMinion() || wasSummoned())
		{
			Destroy(combatSprite);
			Destroy(healthBar);

			if (isLarge())
			{
				destroyAllSpawnPositions();
			}
			else
			{
				CombatGrid.setCombatantAtCoords(position, null);
			}

			isDead = true;
			OnMinionSummonDeath.Invoke();

		}
		else if (cantBeResurrected())
		{
			Destroy(combatSprite);
			Destroy(healthBar);
			isDead = true;
		}
		else
		{
			base.setToDeadSprite();
			isDead = true;
		}

		OnEnemyDeath.Invoke();

		prepareOnDeathEffects();
	}

	public override void bringBackFromDeath()
	{
		if (Helpers.hasQuality<Trait>(traits, t => t.preventsResurrection()))
		{
			return;
		}

		CombatStateManager.deadMonsterCount--;

		base.bringBackFromDeath();
	}

	private void destroyAllSpawnPositions()
	{
		foreach (GridCoords coords in spawnDetails.allSpawnPositions)
		{
			CombatGrid.setCombatantAtCoords(coords, null);
		}
	}

	public void setCombatAction(CombatAction combatAction)
	{
		this.combatAction = combatAction;
	}

	public override IDescribable getHoverPanelDescribable()
	{
		IDescribable hoverPanelDescribable = getCombatAction();

		if (hoverPanelDescribable != null)
		{
			return hoverPanelDescribable;
		}
		else if (hoverPanelDescribable == null && isPartOfVolley())
		{
			return new VolleyAbility(true);
		}
		else
		{
			return AbilityList.getAbility(AbilityList.harmlessKey);
		}
	}

	//IDescribable methods

	public override void describeSelfFull(DescriptionPanel panel)
	{
		base.describeSelfFull(panel);

		if (isMinion())
		{
			DescriptionPanel.setText(panel.typeText, TraitList.minion.getName());
		}
		else if (wasSummoned())
		{
			DescriptionPanel.setText(panel.typeText, TraitList.summoned.getName());
		}
		else
		{
			DescriptionPanel.setText(panel.typeText, TraitList.master.getName());
		}
	} 

	public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{
		List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getHealthBlock(currentHealth + " / " + getTotalHealth()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getArmorBlock(getTotalArmorRatingForDisplay()));

		return buildingBlocks;
	}

	/*
	
	public override void describeStats(HoverPanel hoverPanel)
	{
		DescriptionPanel descriptionPanel = hoverPanel.descriptionPanels[descriptionPanelIndex];
		
		descriptionPanel.gameObject.SetActive(true);
		
		descriptionPanel.nameText.text = name;
		descriptionPanel.hpText.text = currentHealth + " / " + getTotalHealth();
		descriptionPanel.armorRatingText.text = "" + getTotalArmorRating();
		descriptionPanel.typeText.text = traitNames[0];
	}
	
	*/
}
