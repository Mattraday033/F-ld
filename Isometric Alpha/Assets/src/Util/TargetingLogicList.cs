using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate Catalog<IActionUnit> TargetingLogic(CombatAction combatAction);

public static class TargetingLogicList
{

    public readonly static TargetingLogic pushBackWithCollision = (a) =>
    {
        List<Stats> targets = a.selector.getAllTargets();
        Catalog<IActionUnit> output = new Catalog<IActionUnit>();

        foreach(Stats target in targets)
        {
            if(target.isImmobile)
            {
                output.Add(new MoveCombatant(target.combatant, target.positions));
            } else
            {
                output.Add(TargetingUtil.push(target));
            }
        }

        output.Add(output.GetValue(typeof(MoveCombatant)).Cast<MoveCombatant>().Select(m => new ChangeHealth(   
                                                                                                                m.target, 
                                                                                                                CombatActionUtil.calculateDamage(a, m.target.stats, DamageCalculator.rollCritChance(a)) * (int) (1.0f + (m.rowDifference * Constants.thirtyPercentPerSquareMoved)),
                                                                                                                a.healsTarget)).Cast<IActionUnit>().ToList());

        return output;
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