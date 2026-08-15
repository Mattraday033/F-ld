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

    public bool gendered;
    protected string genderMarker;

    public string[] animationSuffixes;

    private CombatAction combatAction;

    #endregion

    #region Constructors

    public EnemyStats(  string key,
                        int armor, 
                        int tHP, 
                        CombatAction combatAction = null, 
                        Trait[] traits = null, 
                        bool gendered = false, 
                        string[] animationSuffixes = null, 
                        Dictionary<CharacterAnimationType, SFXType> animationAudioClipDictionary = null) :
    base(key)
    {
        this.armor = armor;

        this.totalHealth = tHP;
        this.currentHealth = totalHealth;

        this.gendered = gendered;

        if(combatAction != null)
        {
            this.combatAction = combatAction.clone(this);
        }

        if(traits != null)
        {
            foreach (Trait trait in traits)
            {
                trait.traitApplier = this;
                addTrait(trait);
            }
        }

        if(animationSuffixes != null)
        {
            this.animationSuffixes = animationSuffixes;
        }

        this.animationAudioClipDictionary = animationAudioClipDictionary;
    }

    #endregion

    #region Sprite and GameObject

    public override GameObject instantiateCombatSprite(List<GridCoords> coords)
    {
        SpawnDetails spawnDetails = obtainSpawnDetails();

        if(spawnDetails != null)
        {
            coords = new List<GridCoords>(spawnDetails.allSpawnPositions);
        }

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

    public override void setToDeadSprite()
    {
        base.setToDeadSprite();

        if (isMinion() || isSummon())
        {
            OnMinionSummonDeath.Invoke();
        }

        CombatStateManager.deadMonsterCount++;
        OnEnemyDeath.Invoke();
    }

    public override void bringBackFromDeath()
    {
        if (Helpers.hasQuality<Trait>(traitContainer, t => t.preventsResurrection()))
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
        return armor + StatBoostSource.calculateAllStatFormulas(getAllStatBoosts(), b => b.getArmorFormula());
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
        combatActionClone.setActor(combatAction.getActorStats());

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

    public override bool notResurrectable()
    {
        return isMinion() || isSummon() || base.notResurrectable();
    } 

    public double getLinkedPercentage()
    {
        Trait linkedTrait = Helpers.getObjectWithQuality<Trait>(traitContainer, t => t.getLinkedPercentage() > 0.0);

        if(linkedTrait == null)
        {
            return 0.0;
        }

        return linkedTrait.getLinkedPercentage();
    }

    #endregion

    #region Audio

    public override void playAnimationSFX(CharacterAnimationType animationType)
    {
        if(animationAudioClipDictionary == null || 
            !animationAudioClipDictionary.ContainsKey(animationType))
        {
            return;
        }

        if(gendered)
        {
            switch(getGenderMarker())
            {
                case Constants.maleMarker:
                    animationAudioClipDictionary = AnimationSFXDictionaryList.maleHumanAudioDictionary;
                    break;
                case Constants.femaleMarker:
                    animationAudioClipDictionary = AnimationSFXDictionaryList.femaleHumanAudioDictionary;
                    break;
            }

            AudioManager.playAudioClipAsSingleton(AudioClipList.getAudioClip(animationAudioClipDictionary[animationType]));
        } else
        {
            base.playAnimationSFX(animationType);
        }
    }

    #endregion

    #region Miscellanious
    
    public override string getGenderMarker()
    {
        if(!gendered)
        {
            return base.getGenderMarker();
        }

        if(genderMarker == null || genderMarker.Length <= 0)
        {
            setGenderMarker();
        }
        
        return genderMarker;
    }

    public void setGenderMarker()
    {
        int gender = UnityEngine.Random.Range(0, 2);

        if(gender == Constants.indexZero)
        {
            genderMarker = Constants.maleMarker;
        } else
        {
            genderMarker = Constants.femaleMarker;
        }
    }

    public override string getAnimationSuffixes()
    {
        if(animationSuffixes == null || animationSuffixes.Length <= 0)
        {
            return base.getAnimationSuffixes();
        }

        return animationSuffixes.OrderBy(a => Guid.NewGuid()).ToList()[0];
    }

    public override List<GridCoords> findLocationToSpawn()
    {
        if(isFrontline())
        {
            return new List<GridCoords>() { CreatureSpawner.getNextFreeEnemyFrontLineSpace()};
        }

        if(isBackline())
        {
            return new List<GridCoords>() { CreatureSpawner.getNextFreeEnemyBackLineSpace()};
        }

        return new List<GridCoords>() { CombatGrid.findRandomOpenSpaceInEnemyZone() };
    }

    #endregion

    #region ICloneable

    public override Stats clone()
    {
        EnemyStats cloneStats = base.clone() as EnemyStats;

        cloneStats.animationSuffixes = animationSuffixes;

        return cloneStats;
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
        else if (isSummon())
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

    // public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    // {
    //     List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

    //     buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName()));

    //     buildingBlocks.Add(DescriptionPanelBuildingBlock.getHealthBlock(currentHealth, getTotalHealth()));

    //     buildingBlocks.Add(DescriptionPanelBuildingBlock.getArmorBlock(getTotalArmorRatingForDisplay()));

    //     buildingBlocks.Add(DescriptionPanelBuildingBlock.getInvulnerableBlock(getInvulnerability()));

    //     buildingBlocks.Add(DescriptionPanelBuildingBlock.getVulnerableBlock(getVulnerability()));

    //     return buildingBlocks;
    // }

    #endregion



}
