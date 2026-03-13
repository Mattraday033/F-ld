using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinionStats : VolleyParticipantStats
{
    public MinionStats(string key, int armor, int tHP, CombatAction combatAction, Trait[] traits, bool gendered = false, string[] animationSuffixes = null, Dictionary<CharacterAnimationType, string> animationAudioClipDictionary = null) :
    base(key, armor, tHP, combatAction, traits, gendered: gendered, animationSuffixes: animationSuffixes, animationAudioClipDictionary: animationAudioClipDictionary)
    {
        addTraits(traits);
        setFoeTypeToMinion();
    }

    private void setFoeTypeToMinion()
    {
        traitContainer.removeAllTraitsOfType(TraitType.FoeType);

        traitContainer.addTrait(TraitList.minion);
    }


}