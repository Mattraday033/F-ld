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
    #region Unity Events
    public readonly static UnityEvent OnMinionSummonDeath = new UnityEvent();
    public readonly static UnityEvent OnEnemyDeath = new UnityEvent();

    #endregion

    #region Global Variables

    [SerializeField]
    private int totalHealth;
    [SerializeField]
    private bool priorityAttacker;
    [SerializeField]
    private bool lowPriorityAttacker;
    public int armor;

    public SpawnDetails spawnDetails;
    private CombatAction combatAction;

    #endregion

    #region Constructors

    public EnemyStats(string key, int armor, int tHP) :
    base(key)
    {
        this.armor = armor;

        this.totalHealth = tHP;
        this.currentHealth = totalHealth;
    }

    public EnemyStats(string key, int armor, int tHP, CombatAction combatAction, Trait[] traits) :
    base(key)
    {
        this.armor = armor;

        this.totalHealth = tHP;
        this.currentHealth = totalHealth;

        this.combatAction = combatAction.clone(this);

        foreach (Trait trait in traits)
        {
            addTrait(trait);
        }
    }

    #endregion

    #region Sprite and GameObject

    public override GameObject instantiateCombatSprite()
    {
        combatSprite = Instantiate(Resources.Load<GameObject>(PrefabNames.enemySprite), CombatStateManager.getCreatureParent());
        setUpComponents(combatSprite.GetComponent<ComponentList>());

        combatSprite.transform.localScale = new Vector3(1f, 1f, 1f);

        Helpers.updateGameObjectPosition(combatSprite);

        return combatSprite;
    }

    public void playSpawnAnimation()
    {
        animationManager.playSpawnAnimation();
    }

    public override Color getOutlineColor()
    {
        return ColorList.attacksOnSight;
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

            if (isLarge())
            {
                destroyAllSpawnPositions();
            }
            else
            {
                CombatGrid.setCombatantAtCoords(position, null);
            }

            OnMinionSummonDeath.Invoke();

        }
        else if (notResurrectable())
        {
            Destroy(combatSprite);
        }
        else
        {
            base.setToDeadSprite();
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

    #endregion

    #region Health

    public override int getTotalHealth()
    {
        return totalHealth;
    }

    #endregion

    #region Combat and Actions

    public override int getTotalArmorRating()
    {
        return (int)((double)armor * getCurrentTotalArmorPercentage());
    }


    public virtual void spawningCombatAction()
    {
        //Empty On Purpose
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

    public override bool isPriorityAttacker()
    {
        return priorityAttacker;
    }

    public override bool isLowPriorityAttacker()
    {
        return lowPriorityAttacker;
    }

    public virtual bool notResurrectable()
    {
        return Helpers.hasQuality<Trait>(traits, (t => t.preventsResurrection()));
    }

    #endregion

    #region Traits

    public bool isMinion()
    {
        return hasTrait(TraitList.minion);
    }

    public bool isLarge()
    {
        return hasTrait(TraitList.large);
    }
    
    #endregion

    #region Miscellaneous

    private void destroyAllSpawnPositions()
    {
        foreach (GridCoords coords in spawnDetails.allSpawnPositions)
        {
            CombatGrid.setCombatantAtCoords(coords, null);
        }
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
            return AbilityList.getAbility(this, AbilityList.harmlessKey);
        }
    }
    #endregion

    #region IDescribable

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


    #endregion

    #region IDescribableInBlocks

    public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getHealthBlock(currentHealth + " / " + getTotalHealth()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getArmorBlock(getTotalArmorRatingForDisplay()));

        return buildingBlocks;
    }

    #endregion



}
