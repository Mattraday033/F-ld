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
    private int _TotalHealth;
    [SerializeField]
    private bool priorityAttacker;
    [SerializeField]
    private bool lowPriorityAttacker;
    public int armor;

    public bool gendered;
    protected string _GenderMarker;

    //an enemy takes at most one layout from its pack's spawn layouts, see CombatantSpawnDetails.forSpawn
    public bool obtainedSpawnLayout = false;

    private CombatAction _CombatAction;

    private const int noCloneKey = -1;

    //set when a template is cloned into an enemy; that enemy's own clones (previews, snapshots) keep it, so they still Equal it
    private int cloneKey = noCloneKey;

    #endregion

    #region Names

    //the key follows the name with no separator, so DialogueList.scrubNameOfEndNumbers still finds the enemy type.
    //characterName is left as is, so it still names the enemy type and displayName never shows the key
    public override string uniqueName
    {
        get
        {
            return cloneKey == noCloneKey ? base.uniqueName : base.uniqueName + cloneKey;
        }
    }

    #endregion

    #region Boolean Properties

    public override bool isPriorityAttacker => priorityAttacker;
    public override bool isLowPriorityAttacker => lowPriorityAttacker;
    public override bool notResurrectable => isMinion || isSummon || base.notResurrectable;

    #endregion

    #region Properties

    #region Sprite and GameObject

    public override Color outlineColor => ColorList.attacksOnSight;

    #endregion

    #region Health

    public override int totalHealth => _TotalHealth;

    #endregion

    #region Combat and Actions

    public override int totalArmorRating => armor + StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getArmorFormula());

    //a fresh clone on every read, so callers can't change the enemy's own action
    public CombatAction combatAction
    {
        get
        {
            if (_CombatAction == null || _CombatAction is null)
            {
                return null;
            }

            CombatAction combatActionClone = _CombatAction.clone();

            if(_CombatAction.hasAssignedActor(out Stats actor))
            {
                combatActionClone.actorStats = actor;
            }

            return combatActionClone;
        }
    }

    public override EffectAnimationType volleyAnimationType
    {
        get
        {
            if(combatAction == null)
            {
                return EffectAnimationType.Pierce;
            }

            return combatAction.effectAnimationType;
        }
    }

    #endregion

    #region Traits

    public double linkedPercentage
    {
        get
        {
            Trait linkedTrait = Helpers.getObjectWithQuality<Trait>(traitContainer, t => t.getLinkedPercentage() > 0.0);

            if(linkedTrait == null)
            {
                return 0.0;
            }

            return linkedTrait.getLinkedPercentage();
        }
    }

    #endregion

    #region Miscellanious

    public override string genderMarker
    {
        get
        {
            if(!gendered)
            {
                return base.genderMarker;
            }

            if(_GenderMarker == null || _GenderMarker.Length <= 0)
            {
                setGenderMarker();
            }

            return _GenderMarker;
        }
    }

    #endregion

    #endregion

    #region Constructors

    public EnemyStats(  string key,
                        int armor,
                        int tHP,
                        CombatAction combatAction = null,
                        Trait[] traits = null,
                        bool gendered = false,
                        Dictionary<CharacterAnimationType, SFXType> animationAudioClipDictionary = null) :
    base(key)
    {
        this.armor = armor;

        // this._TotalHealth = tHP;
        this._TotalHealth = 1;
        this.currentHealth = _TotalHealth;

        this.gendered = gendered;

        if(combatAction != null)
        {
            this._CombatAction = combatAction.clone(this);
        }

        if(traits != null)
        {
            foreach (Trait trait in traits)
            {
                trait.traitApplier = this;
                addTrait(trait);
            }
        }

        this.animationAudioClipDictionary = animationAudioClipDictionary;
    }

    #endregion

    #region ICloneable

    public override Stats clone()
    {
        EnemyStats clone = (EnemyStats)base.clone();

        if(clone.cloneKey == noCloneKey)
        {
            clone.cloneKey = INameSource.getCurrentKey();
        }

        return clone;
    }

    #endregion

    #region Sprite and GameObject

    public override IAppearance appearance
    {
        get
        {
            return Costume.getDefaultCostume();
        }
    }

    public override void setToDeadSprite()
    {
        base.setToDeadSprite();

        if (isMinion || isSummon)
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

    #region Combat and Actions

    public virtual void spawningCombatAction()
    {
        //Empty On Purpose
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
            switch(genderMarker)
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

    public void setGenderMarker()
    {
        int gender = UnityEngine.Random.Range(0, 2);

        if(gender == Constants.indexZero)
        {
            _GenderMarker = Constants.maleMarker;
        } else
        {
            _GenderMarker = Constants.femaleMarker;
        }
    }

    public override List<GridCoords> findLocationToSpawn()
    {
        if(isFrontline)
        {
            return new List<GridCoords>() { CreatureSpawner.getNextFreeEnemyFrontLineSpace()};
        }

        if(isBackline)
        {
            return new List<GridCoords>() { CreatureSpawner.getNextFreeEnemyBackLineSpace()};
        }

        return new List<GridCoords>() { CombatGrid.findRandomOpenSpaceInEnemyZone() };
    }

    #endregion



    #region IDescribable

    public override void describeSelfFull(DescriptionPanel panel)
    {
        base.describeSelfFull(panel);

        if (isMinion)
        {
            DescriptionPanel.setText(panel.typeText, TraitList.minion.displayName);
        }
        else if (isSummon)
        {
            DescriptionPanel.setText(panel.typeText, TraitList.summoned.displayName);
        }
        else
        {
            DescriptionPanel.setText(panel.typeText, TraitList.master.displayName);
        }
    }


    #endregion

    #region IDescribableInBlocks

    // public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    // {
    //     List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

    //     buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(displayName));

    //     buildingBlocks.Add(DescriptionPanelBuildingBlock.getHealthBlock(currentHealth, getTotalHealth()));

    //     buildingBlocks.Add(DescriptionPanelBuildingBlock.getArmorBlock(getTotalArmorRatingForDisplay()));

    //     buildingBlocks.Add(DescriptionPanelBuildingBlock.getInvulnerableBlock(getInvulnerability()));

    //     buildingBlocks.Add(DescriptionPanelBuildingBlock.getVulnerableBlock(getVulnerability()));

    //     return buildingBlocks;
    // }

    #endregion



}
