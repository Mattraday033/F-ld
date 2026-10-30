using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//targets are passed in rather than read from action.selector, so damage previews can supply clones
public delegate Catalog<IActionUnit> TargetingLogic(CombatAction action, List<Stats> targets);

public static class TargetingLogicList
{
    public readonly static TargetingLogic modifyHealth = (action, targets) =>
    {
        action.actionCatalog.Add(targets.Select(t => new ChangeHealth(t,
                                                        CombatActionUtil.calculateDamage(action, t, DamageCalculator.rollCritChance(action)),
                                                        action.healsTarget)).Cast<IActionUnit>().ToList());
        return action.actionCatalog;
    };

    public readonly static TargetingLogic damageFromPushback = (action, targets) =>
    {

        action.actionCatalog.Add(action.actionCatalog[typeof(MoveCombatant)].Cast<MoveCombatant>().Select(m => new ChangeHealth(
                                                                                                                m.target,
                                                                                                                CombatActionUtil.calculateDamage(action, m.target, DamageCalculator.rollCritChance(action)) * (int) (1.0f + (m.rowDifference * Constants.thirtyPercentPerSquareMoved)),
                                                                                                                action.healsTarget)).Cast<IActionUnit>().ToList());

        return action.actionCatalog;
    };

    public readonly static TargetingLogic applyTraitToAllDamaged = (action, targets) =>
    {
        action.actionCatalog.Add(action.actionCatalog[typeof(ChangeHealth)].Cast<ChangeHealth>().Select(m => new ApplyTrait( m.target,
                                                                                                            action.appliedTrait)).Cast<IActionUnit>().ToList());

        return action.actionCatalog;
    };

}


public static class TargetingUtil
{
    public static List<IActionUnit> push(Stats stats)
    {
        bool alliedSide = CombatGrid.positionIsOnAlliedSide(stats.positions[0]);
        int direction = alliedSide ? 1 : -1;
        int backRow = alliedSide ? CombatGrid.allyRowLowerBounds : CombatGrid.enemyRowUpperBounds;

        bool notPastBackRow(int row) => alliedSide ? row <= backRow : row >= backRow;

        int freeRowsBehind(GridCoords cell)
        {
            int freeRows = 0;

            for(int row = cell.row + direction; notPastBackRow(row); row += direction)
            {
                if(CombatGrid.tryGetCombatantAt(row, cell.col, out Combatant occupant) && !occupant.stats.Equals(stats))
                {
                    break;
                }

                freeRows++;
            }

            return freeRows;
        }

        //a side is four rows deep, so nothing can be pushed further than three
        int maxDistance = CombatGrid.allyRowLowerBounds - CombatGrid.allyRowUpperBounds;
        int distance = Math.Min(maxDistance, stats.positions.Min(freeRowsBehind));

        List<GridCoords> newPositions = stats.positions.Select(p => new GridCoords(p.row + direction * distance, p.col)).ToList();

        List<IActionUnit> collisionTargets = new List<IActionUnit>() { new MoveCombatant(stats, newPositions) };

        //the face farthest from the dividing line is the shape's row closest to the back row
        int farthestRow = alliedSide ? newPositions.Max(p => p.row) : newPositions.Min(p => p.row);

        //combatants touching that face stay where they are, but are still added so they take collision damage
        foreach(GridCoords cell in newPositions.Where(p => p.row == farthestRow))
        {
            if(CombatGrid.tryGetCombatantAt(cell.row + direction, cell.col, out Combatant occupant) &&
               !occupant.stats.Equals(stats) &&
               !collisionTargets.Any(m => m.target.Equals(occupant.stats)))
            {
                collisionTargets.Add(new MoveCombatant(occupant.stats, occupant.stats.positions));
            }
        }

        return collisionTargets;
    }
}

public class Catalog<T>
{
    private Dictionary<Type, List<T>> catalog = new Dictionary<Type, List<T>>();

    public Catalog()
    {
    }

    //starts filled with the given items, which also copies another catalog when given its Values
    public Catalog(IEnumerable<T> items)
    {
        Add(items.ToList());
    }

    //every item, a type's items together, in the order each type was first added
    public IEnumerable<T> Values => catalog.Values.SelectMany(list => list);

    public void Add(T t)
    {
        Add(new List<T>(){t});
    }

    public void Add(List<T> list)
    {
        foreach(T t in list)
        {
            Type type = t.GetType();

            if(!catalog.ContainsKey(type))
            {
                catalog[type] = new List<T>();
            }

            catalog[type].Add(t);
        }
    }

    //swaps in new lists rather than emptying the old ones, so a list already handed out keeps what it held
    public void clear()
    {
        foreach(Type type in catalog.Keys.ToList())
        {
            catalog[type] = new List<T>();
        }
    }

    public List<T> GetValuesInOrder(Type[] typeOrder)
    {
        List<T> values = new List<T>();

        foreach(Type type in typeOrder)
        {
            if(catalog.TryGetValue(type, out List<T> list))
            {
                values.AddRange(list);
            }
        }

        return values;
    }

    public List<T> this[Type type]
    {
        get
        {
            if(!catalog.ContainsKey(type))
            {
                catalog[type] = new List<T>();
            }

            return catalog[type];
        }
    }
}