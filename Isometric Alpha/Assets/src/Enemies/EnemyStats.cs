using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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

    private CombatAction combatAction;

    #endregion

    #region Constructors

    public EnemyStats(string key, int armor, int tHP, CombatAction combatAction = null, Trait[] traits = null) :
    base(key)
    {
        this.armor = armor;

        this.totalHealth = tHP;
        this.currentHealth = totalHealth;

        if(combatAction != null)
        {
            this.combatAction = combatAction.clone(this);
        }

        if(traits != null)
        {
            foreach (Trait trait in traits)
            {
                addTrait(trait);
            }
        }
    }

    #endregion

    #region Sprite and GameObject

    public override GameObject instantiateCombatSprite(GridCoords coords)
    {
        combatSprite = base.instantiateCombatSprite(coords);

        combatSprite.transform.localScale = new Vector3(1f, 1f, 1f);

        Helpers.updateGameObjectPosition(combatSprite);

        return combatSprite;
    }
    public override string getCombatSpriteName()
    {
        return PrefabNames.enemySprite;
    }
    public override Color getOutlineColor()
    {
        return ColorList.attacksOnSight;
    }

    public override GridCoords getPositionToHit(Selector selector, int skips)
    {
        return position.clone();
    }

    public override void setToDeadSprite()
    {
        base.setToDeadSprite();

        if (isMinion() || wasSummoned())
        {
            OnMinionSummonDeath.Invoke();
        }

        CombatStateManager.deadMonsterCount++;
        OnEnemyDeath.Invoke();
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

    public override string getVolleyAnimationType()
    {
        if(getCombatAction() == null)
        {
            return EffectAnimationType.Pierce.ToString();
        }

        return getCombatAction().getEffectAnimationType();
    }

    #endregion

    #region Traits

    public bool isMinion()
    {
        return hasTrait(TraitList.minion);
    }

    public virtual bool isLarge()
    {
        return false;
    }
    
    public override bool notResurrectable()
    {
        return isMinion() || wasSummoned() || base.notResurrectable();
    } 

    #endregion

    #region Miscellanious
    
    public override GridCoords findLocationToSpawn()
    {
        if(isFrontline())
        {
            return CreatureSpawner.getNextFreeEnemyFrontLineSpace();
        }

        if(isBackline())
        {
            return CreatureSpawner.getNextFreeEnemyBackLineSpace();
        }

        return CombatGrid.findRandomOpenSpaceInEnemyZone();
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
