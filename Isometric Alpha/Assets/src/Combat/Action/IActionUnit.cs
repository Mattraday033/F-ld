using System;
using System.Collections.Generic;
using System.Linq;

public interface IActionUnit
{

    public readonly static Type[] executionOrder =  {
                                                        typeof(MoveCombatant),
                                                        typeof(ChangeHealth),
                                                        typeof(ApplyTrait)
                                                    };

    //Stats rather than Combatant, so units also work on preview clones, which have no Combatant
    public Stats target
    {
        get;
    }

    public List<IAnimationUnit> perform();

    public void repeal();
}

public static class ActionUnitUtil
{
    //puts a see-through copy of the target on the grid where the move will take it.
    //A move that leaves the target where it is gets none, since the copy would sit on top of the target
    public static void generatePlaceholder(this MoveCombatant move)
    {
        if(move.newPositions[0].Equals(move.originalPositions[0]))
        {
            return;
        }

        //only empty cells, so a placeholder never takes a real combatant's place on the grid
        List<GridCoords> emptyCells = move.newPositions.Where(cell => !CombatGrid.combatantExistsAtCoords(cell)).ToList();

        //with no cell to stand on, the placeholder couldn't be found on the grid to be removed again, so it isn't made at all
        if(emptyCells.Count == 0)
        {
            return;
        }

        Stats placeholder = move.target.clone();

        placeholder.positions = move.newPositions.Select(p => p.clone()).ToList();
        placeholder.addTrait(TraitList.repositioningInvulnerability);
        placeholder.addTrait(TraitList.untargetable);

        RepositionPlaceholderGenerator.generatePlaceholderObject(placeholder, placeholder.positions[0]);

        foreach(GridCoords cell in emptyCells)
        {
            CombatGrid.setCombatantAtCoords(cell, placeholder);
        }

        //the only link from a placeholder back to its original: hovers, selectors and previews find the original through it
        move.target.repositionClone = placeholder;
    }

    //takes the placeholder generatePlaceholder made for this move off the grid and destroys it.
    //It's found on the move's destination cells by being a placeholder of the move's target, so nothing happens if there isn't one
    public static void removePlaceholder(this MoveCombatant move)
    {
        Stats placeholder = null;

        foreach(GridCoords cell in move.newPositions)
        {
            if(CombatGrid.tryGetCombatantAt(cell, out Combatant combatant) &&
                combatant.isPlaceholder &&
                combatant.stats.uniqueName.Equals(move.target.uniqueName))
            {
                placeholder = combatant.stats;
                CombatGrid.removeCombatantAtCoords(cell);
            }
        }

        if(placeholder == null)
        {
            return;
        }

        //only this move's placeholder, so a placeholder another queued action made for the same target stays linked
        if(ReferenceEquals(move.target.repositionClone, placeholder))
        {
            move.target.repositionClone = null;
        }

        //copied, since each Combatant takes itself out of the list as it's destroyed
        foreach(Combatant combatant in placeholder.combatants.ToList())
        {
            if(combatant != null)
            {
                UnityEngine.Object.Destroy(combatant.gameObject);
            }
        }
    }
}

public enum TraitApplicationState { NotApplied, Applied, Resisted }
public class ApplyTrait : IActionUnit
{
    public Stats target { get; }
    private Trait trait;

    private int healthBeforeTrait = 0;
    private TraitContainer traitContainerSnapshot;
    private TraitApplicationState traitApplicationState = TraitApplicationState.NotApplied;

    public ApplyTrait(Stats target, Trait trait)
    {
        this.target = target;
        this.trait = trait?.clone();
    }

    public List<IAnimationUnit> perform()
    {
        if(trait == null)
        {
            return new List<IAnimationUnit>();
        }

        traitContainerSnapshot = target.traitContainer.clone(target);

        healthBeforeTrait = target.currentHealth;

        if(checkForResistance())
        {
            traitApplicationState = TraitApplicationState.Resisted;

            return new List<IAnimationUnit>() { new CreateMessage(target.getCombatant(), Constants.resist) };
        } else
        {
            target.addTrait(trait);

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
                return target.rollAgainstWoundResistance();
            case TraitType.Mental:
                return target.rollAgainstMentalResistance();
            default:
                return false;
        }
    }

    public void repeal()
    {
        switch(traitApplicationState)
        {
            case TraitApplicationState.Applied:
                target.currentHealth = healthBeforeTrait;
                target.traitContainer = traitContainerSnapshot;
                return;
            default:
                return;
        }
    }
}

public class ChangeHealth : IActionUnit
{

    public Stats target { get; }
    private bool healing;
    private int damage;

    private int originalHealth = -1;

    public ChangeHealth(Stats target, int damage, bool healing)
    {
        this.target = target;
        this.damage = damage;
        this.healing = healing;
    }

    public List<IAnimationUnit> perform()
    {
        originalHealth = target.currentHealth;

        target.modifyCurrentHealth(damage, healing, false);

        List<IAnimationUnit> output = new List<IAnimationUnit>()   {
                                                    new CreateMessage(target.getCombatant(), damage.ToString())
                                                };

        if(target.isDead)
        {
            output.Add(new StartAnimation(target.getCombatant(), CharacterAnimationType.Death));
        } else
        {
            output.Add(new StartAnimation(target.getCombatant(), CharacterAnimationType.Wounded));
        }

        return output;
    }

    public void repeal()
    {
        if(originalHealth >= 0)
        {
            target.currentHealth = originalHealth;
        }
    }
}

public class MoveCombatant : IActionUnit
{
    public Stats target { get; }
    public List<GridCoords> newPositions { get; }
    public List<GridCoords> originalPositions { get; }

    public int rowDifference => Math.Abs(newPositions[0].row - originalPositions[0].row);
    public int colDifference => Math.Abs(newPositions[0].col - originalPositions[0].col);

    public MoveCombatant(Stats target, List<GridCoords> newPositions)
    {
        this.target = target;
        this.newPositions = newPositions;
        this.originalPositions = target.positions;
    }

    public List<IAnimationUnit> perform()
    {
        //moveTo edits the real CombatGrid, so a preview clone must never move
        if(!target.inPreviewMode && !newPositions[0].Equals(originalPositions[0]))
        {
            //the placeholder stands where the target is going, so it's cleared off first
            this.removePlaceholder();

            target.moveTo(newPositions);
        }

        return new List<IAnimationUnit>();
    }

    public void repeal()
    {
        if(!target.inPreviewMode)
        {
            target.moveTo(originalPositions);
        }
    }
}
