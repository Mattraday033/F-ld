using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class VolleyParticipantStats : EnemyStats
{
    public override bool isPartOfVolley => true;

    public VolleyParticipantStats(string key, int armor, int tHP, CombatAction combatAction, Trait[] traits = null, bool gendered = false, Dictionary<CharacterAnimationType, SFXType> animationAudioClipDictionary = null) :
    base(key, armor, tHP, combatAction, traits, gendered: gendered, animationAudioClipDictionary: animationAudioClipDictionary)
    {

    }
}