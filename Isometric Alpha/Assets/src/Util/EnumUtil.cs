using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumUtil
{
    public static string ToFriendlyString(this Enum template)
    {
        string name = template.ToString();
        string newName = "";

        int index = 0;
        foreach(char c in name)
        {
            if((Char.IsUpper(c) || Char.IsDigit(c)) && 
                index != 0)
            {
                newName += " " + c;
            } else
            {
                newName += c;
            }

            index++;
        }

        return newName;
    }


    public static Trait getCostTrait(this ActionCostType costType)
    {
        switch(costType)
        {
            case ActionCostType.Bloodlust:
                return TraitList.bloodlust.clone();
            case ActionCostType.Predation:
                return TraitList.predation.clone();
            default:
                return null;
        }
    }

    public static string getCostDescription(this ActionCostType costType, int amount)
    {
        switch(costType)
        {
            case ActionCostType.RedKnife:
            case ActionCostType.BlueShield:
            case ActionCostType.YellowThorn:
            case ActionCostType.GreenLeaf:
                return "Costs " + amount + " Stacks of the " + costType.ToFriendlyString() + " Exuberance.";
            case ActionCostType.Stance:
                return "Costs " + amount + " Stacks of any Stance.";
            default:
                return "Costs " + amount + " Stacks of the " + costType.ToFriendlyString() + " Trait.";
        }
    }

    public static string getPrimaryStatCharGenCombatDescription(this PrimaryStat stat)
    {
        switch(stat)
        {
            case PrimaryStat.Strength:
                return "Characters who train their Strength gain access to Abilities that affect large areas with big bursts of damage. Their Critical Hits deal more damage as well, and they have higher Health pools than other Characters.\n\nCertain Strength Abilities can push enemies around, or prevent them from attacking vulnerable allies.";
            case PrimaryStat.Dexterity:
                return "Training Dexterity unlocks Abilities that focus on debilitation, damage over time, and the element of surprise. Dexterity also increases a Character's own Armor Score, their Armor Penetration, and provides a damage boost during a surprise round.\n\nMost Actions have their Critical Hit chance determined by a Character's Dexterity.";
            case PrimaryStat.Wisdom:
                return "Raising Wisdom teaches Abilities that are more tactical. A Wise Character can reposition their opponents to better deal with them, or interrupt a foe's plans with a well placed strike.\n\nWisdom governs the number of Weapons that can be held at once, and the number of Passive Abilities that can be equipped.";
            default:
                return "In Combat, Charisma measures a Character's ability to lead and coordinate with their Party Members. Charisma has Abilities that bolster one's allies, and expose the weaknesses of one's enemies.\n\nCharisma also provides access to Exuberances, which are a resource that is used to activate certain powerful Abilities.";
        }
    }

    public static string getPrimaryStatCharGenDialogueDescription(this PrimaryStat stat)
    {
        switch(stat)
        {
            case PrimaryStat.Strength:
                return "Strength speech checks often involve coercing others into providing aid, or gaining another's confidence through displays of physical prowess.";
            case PrimaryStat.Dexterity:
                return "In Dialogue, Dexterity measures a Character's ability to outwit and outmaneuver. A Dexterity speech check might involve using double-talk to trick someone into giving up information, or catching their arm before they can draw a weapon in anger.";
            case PrimaryStat.Wisdom:
                return "Wisdom is the Primary Stat of perception, knowledge, and reason. Wisdom provides speech checks that allow a Character to make logical arguments, bestow sagely advice, or detect what has been obscured.";
            default:
                return "Charisma determines a Character's powers of communication and persuasion. A Charismatic Character could with one speech check gain someone's confidence, and with the next destroy their ego with mockery.";
        }
    }

    public static string getPrimaryStatCharGenMobilityDescription(this PrimaryStat stat)
    {
        switch(stat)
        {
            case PrimaryStat.Strength:
                return "Characters with high Strength can use their powerful muscles to lift boulders, break down gates, and push aside rubble. They may also use the Intimidate Skill to challenge enemies to open combat, and compel shopkeepers to lower their prices.";
            case PrimaryStat.Dexterity:
                return "Increasing Dexterity allows a Character to clamber over obstructions, squeeze into tight spaces, and operate mechanisms. Dexterous Characters also can use the Cunning Skill to activate traps and deceive enemies; either to slip past them, or assault them from behind.";
            case PrimaryStat.Wisdom:
                return "A high Wisdom unlocks the Observation Skill. This Skill can be used to find secret passages, uncover hidden objects, and even find ambushes before they are sprung.";
            default:
                return "Charisma allows a Character to use the Leadership skill to coordinate multiple Party Members at once. This allows Party Members to block the movement of enemies, or operate mechanisms as a team.";
        }
    }

    public static SFXType convertEffectTypeToSFXType(this EffectAnimationType effect)
    {
        string effectName = effect.ToString();

        if(Enum.TryParse(effectName, ignoreCase: true, out SFXType sfxType))
        {
            return sfxType;
        } else
        {
            return SFXType.NoSFX;
        }
    }

    public static string getMaterialVARName(this ColorReplaceSlot slot)
    {
        return "_"+slot.ToString()+"_Replace";
    }

    public static IEnumerable<T> getValues<T>() {
        return (T[]) Enum.GetValues(typeof(T));
    }
}
