using System;
using System.Collections.Generic;

public interface IActionUnit
{
    public Combatant target
    {
        get;
    }

    public List<IAnimationUnit> perform();

    public void repeal();
}


public enum TraitApplicationState { NotApplied, Applied, Resisted }
public class ApplyTrait : IActionUnit
{
    public Combatant target { get; }
    private Trait trait;

    private int healthBeforeTrait = 0;
    private TraitContainer traitContainerSnapshot;
    private TraitApplicationState traitApplicationState = TraitApplicationState.NotApplied;

    public ApplyTrait(Combatant target, Trait trait)
    {
        this.target = target;
        this.trait = trait.clone();
    }

    public List<IAnimationUnit> perform()
    {
        traitContainerSnapshot = target.stats.traitContainer.clone(target.stats);
        
        healthBeforeTrait = target.stats.currentHealth;

        if(checkForResistance())
        {
            traitApplicationState = TraitApplicationState.Resisted;

            return new List<IAnimationUnit>() { new CreateMessage(target, Constants.resist) };
        } else
        {
            target.stats.addTrait(trait);
            
            traitApplicationState = TraitApplicationState.Applied;

            return new List<IAnimationUnit>();
        }
    }

    //true means resisted, false means failed to resist
    private bool checkForResistance()
    {
        if(!trait.isDebuff())
        {
            return false;
        }

        switch(trait.traitType)
        {
            case TraitType.Wound:
                return target.stats.rollAgainstWoundResistance();
            case TraitType.Mental:
                return target.stats.rollAgainstMentalResistance();
            default:
                return false;
        }
    }

    public void repeal()
    {
        switch(traitApplicationState)
        {
            case TraitApplicationState.Applied:
                target.stats.currentHealth = healthBeforeTrait;
                target.stats.traitContainer = traitContainerSnapshot;
                return;
            default:
                return;
        }
    }
}

public class ChangeHealth : IActionUnit
{

    public Combatant target { get; }
    private bool healing;
    private int damage;

    private int originalHealth = -1;

    public ChangeHealth(Combatant target, int damage, bool healing)
    {
        this.target = target;
        this.damage = damage;
        this.healing = healing;
    }

    public List<IAnimationUnit> perform()
    {
        target.stats.modifyCurrentHealth(damage, healing, false);

        List<IAnimationUnit> output = new List<IAnimationUnit>()   { 
                                                    new CreateMessage(target, damage.ToString()) 
                                                };

        if(target.stats.isDead)
        {
            output.Add(new StartAnimation(target, CharacterAnimationType.Death));
        } else
        {
            output.Add(new StartAnimation(target, CharacterAnimationType.Wounded));
        }

        return output;
    }

    public void repeal()
    {
        if(originalHealth >= 0)
        {
            target.stats.currentHealth = originalHealth;
        }
    }
}

public class MoveCombatant : IActionUnit
{
    public Combatant target { get; }
    private List<GridCoords> newPositions;
    private List<GridCoords> originalPositions;

    public int rowDifference => Math.Abs(newPositions[0].row - originalPositions[0].row);
    public int colDifference => Math.Abs(newPositions[0].col - originalPositions[0].col);

    public MoveCombatant(Combatant target, List<GridCoords> newPositions)
    {
        this.target = target;
        this.newPositions = newPositions;
        this.originalPositions = target.stats.positions;
    }

    public List<IAnimationUnit> perform()
    {
        if(!newPositions[0].Equals(originalPositions[0]))
        {
            target.stats.moveTo(newPositions);
        }

        return new List<IAnimationUnit>();
    }

    public void repeal()
    {
        target.stats.moveTo(originalPositions);
    }
}