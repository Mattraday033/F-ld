using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public class LargeEnemyStats : EnemyStats
{

    #region Variables
    //the cells this enemy covers, kept so it lands in the same place if it's spawned again
    public CombatantSpawnDetails spawnLayout;

    public const int noHeartBeatRow = -1;

    #endregion

    #region Unity Events
    public readonly static UnityEvent OnLargeEnemySpawn = new UnityEvent();
    #endregion


    #region Constructors

    public LargeEnemyStats( string key,
                            int armor,
                            int tHP,
                            Trait[] traits,
                            CombatAction combatAction = null,
                            Dictionary<CharacterAnimationType, SFXType> animationAudioClipDictionary = null) :
    base(key, armor, tHP, traits: traits, combatAction: combatAction, animationAudioClipDictionary: animationAudioClipDictionary)
    {
        if(!traits.Contains(TraitList.large) && !traits.Contains(TraitList.immobile))
        {
            traitContainer.addTrait(TraitList.large);
        }
    }

    #endregion

    #region Sprite and GameObject

    public override bool multiSpaceEnemy()
    {
        return true;
    }

    //nests share a heartbeat, and each one's row decides where in it its idle beats
    public int getHeartBeatRow()
    {
        switch(uniqueName)
        {
            case MonsterNameList.hiveHeraldNest:
                return 0;
            case MonsterNameList.martyrWormNest:
                return 1;
            case MonsterNameList.toxicWormNest:
                return 2;
            case MonsterNameList.wormNest:
                return 3;
            default:
                return noHeartBeatRow;
        }
    }

    #endregion

    #region Health

    #endregion

    #region Combat and Actions

    #endregion

    #region Traits

    public override bool isLarge()
    {
        return true;
    }

    #endregion

}
