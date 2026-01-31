using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitContainer : StatBoostSourceCombiner, ICloneable
{

    private Stats owner;

    private List<Trait> traits = new List<Trait>();

    public TraitContainer(Stats owner)
    {
        this.owner = owner;
    }

    public override IEnumerator GetEnumerator()
    {
        return traits.GetEnumerator();
    }

    public List<Trait> getVisibleTraits()
    {
        return traits;
    }

    public override string getName()
    {
        return owner.getName() + "'s Trait Container";
    }

    public override Stats getStatSource()
    {
        return owner;
    }

    public void addTrait(Trait newTrait)
    {
        foreach(Trait trait in traits)
        {
            if(trait.Equals(newTrait))
            {
                trait.reapply();
                return;
            }
        }

        switch(newTrait.traitType)
        {
            case TraitType.FoeType:

                List<Trait> newTraits = new List<Trait>();

                newTraits.Add(newTrait);
                newTraits.AddRange(traits);

                traits = newTraits;
                return;
            default:
                traits.Add(newTrait);
                return;
        }
    }

    public bool removeTrait(Trait traitToRemove)
    {
        return traits.Remove(traitToRemove);
    }

    public void removeAllTraitsOfType(TraitType traitType)
    {
        List<Trait> traitsToRemove = Helpers.getAllObjectsWithQuality<Trait>(traits, t => t.traitType == traitType);

        foreach(Trait trait in traitsToRemove)
        {
            traits.Remove(trait);
        }
    }

    public void removeAllTraitsRemovedByDamage()
    {
        List<Trait> traitsToRemove = Helpers.getAllObjectsWithQuality<Trait>(traits, t => t.isRemovedOnDamage());

        foreach(Trait trait in traitsToRemove)
        {
            traits.Remove(trait);
        }
    }

    public Selector findTargetLocation(Selector selector, List<Stats> listOfTargets)
    {
        Selector output = null;

        foreach (Trait trait in this)
        {
            if (trait == null)
            {
                continue;
            }

            output = trait.findTargetLocation(selector, listOfTargets);

            if (output != null)
            {
                break;
            }
        }

        return output;
    }

    #region ICloneable

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public TraitContainer clone(Stats statsSource)
    {
        TraitContainer traitContainer = new TraitContainer(statsSource);

        foreach(Trait trait in this)
        {
            Trait traitClone = trait.clone();

            traitClone.setTraitHolder(statsSource);

            traitContainer.addTrait(traitClone);
        }

        return traitContainer;
    }

    #endregion

    public List<Trait> toListForDisplay()
    {
        List<Trait> listForDisplay = new List<Trait>();

        foreach(Trait trait in this)
        {
            if(!trait.isHiddenTrait())
            {
                listForDisplay.Add(trait);
            }
        }

        return listForDisplay;
    }

}
