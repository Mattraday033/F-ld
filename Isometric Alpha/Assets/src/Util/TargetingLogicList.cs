using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate Catalog<IActionUnit> TargetingLogic(CombatAction action, Catalog<IActionUnit> catalog);

public static class TargetingLogicList
{
    public readonly static TargetingLogic pushBack = (action, catalog) =>
    {
        List<Stats> targets = action.selector.getAllTargets();

        foreach(Stats target in targets)
        {
            if(target.isImmobile)
            {
                catalog.Add(new MoveCombatant(target.combatant, target.positions));
            } else
            {
                catalog.Add(TargetingUtil.push(target));
            }
        }

        return catalog;
    };

    public readonly static TargetingLogic modifyHealth = (action, catalog) =>
    {
        List<Stats> targets = action.selector.getAllTargets();

        catalog.Add(targets.Select(t => new ChangeHealth(t.combatant, 
                                                        CombatActionUtil.calculateDamage(action, t, DamageCalculator.rollCritChance(action)),
                                                        action.healsTarget)).Cast<IActionUnit>().ToList());  
        return catalog;
    };

    public readonly static TargetingLogic damageFromPushback = (action, catalog) =>
    {
      
        catalog.Add(catalog.GetValue(typeof(MoveCombatant)).Cast<MoveCombatant>().Select(m => new ChangeHealth(   
                                                                                                                m.target, 
                                                                                                                CombatActionUtil.calculateDamage(action, m.target.stats, DamageCalculator.rollCritChance(action)) * (int) (1.0f + (m.rowDifference * Constants.thirtyPercentPerSquareMoved)),
                                                                                                                action.healsTarget)).Cast<IActionUnit>().ToList());  

        return catalog;
    };

    public readonly static TargetingLogic applyTraitToAllDamaged = (action, catalog) =>
    {
        catalog.Add(catalog.GetValue(typeof(ChangeHealth)).Cast<ChangeHealth>().Select(m => new ApplyTrait( m.target, 
                                                                                                            action.appliedTrait)).Cast<IActionUnit>().ToList());  

        return catalog;
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
                if(CombatGrid.combatantExistsAtCoords(row, cell.col, out Stats occupant) && !occupant.Equals(stats))
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

        List<IActionUnit> collisionTargets = new List<IActionUnit>() { new MoveCombatant(stats.combatant, newPositions) };

        //the face farthest from the dividing line is the shape's row closest to the back row
        int farthestRow = alliedSide ? newPositions.Max(p => p.row) : newPositions.Min(p => p.row);

        //combatants touching that face stay where they are, but are still added so they take collision damage
        foreach(GridCoords cell in newPositions.Where(p => p.row == farthestRow))
        {
            if(CombatGrid.combatantExistsAtCoords(cell.row + direction, cell.col, out Stats occupant) &&
               !occupant.Equals(stats) &&
               !collisionTargets.Any(m => m.target.stats.Equals(occupant)))
            {
                collisionTargets.Add(new MoveCombatant(occupant.combatant, occupant.positions));
            }
        }

        return collisionTargets;
    }
}

public class Catalog<T>
{
    private Dictionary<Type, List<T>> catalog = new Dictionary<Type, List<T>>();

    public void Add(T t)
    {
        Add(new List<T>(){t});
    }

    public void Add(List<T> t)
    {
        Type type = t.GetType();

        if(!catalog.ContainsKey(type))
        {
            catalog[type] = new List<T>();
        }

        catalog[type].AddRange(t);
    }

    public List<T> GetValue(Type type)
    {
        if(!catalog.ContainsKey(type))
        {
            catalog[type] = new List<T>();
        } 
        
        return catalog[type]; 
    }
}