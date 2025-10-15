using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public static class Exuberances
{
    public static UnityEvent OnExuberanceChance = new UnityEvent();
    private static Dictionary<MultiStackProcType, int> exuberanceDictionary;

    static Exuberances()
    {
        resetExuberanceDictionary();
    }

    public static int getRedKnife()
    {
        return exuberanceDictionary[MultiStackProcType.RedKnife];
    }

    public static int getBlueShield()
    {
        return exuberanceDictionary[MultiStackProcType.BlueShield];
    }

    public static int getYellowThorn()
    {
        return exuberanceDictionary[MultiStackProcType.YellowThorn];
    }

    public static int getGreenLeaf()
    {
        return exuberanceDictionary[MultiStackProcType.GreenLeaf];
    }

    public static bool canPayCost(ActionCostType type, int cost)
    {
        return canPayCost(convertActionCostToProcType(type), cost);
    }

    public static bool canPayCost(MultiStackProcType type, int cost)
    {
        return exuberanceDictionary[type] >= cost;
    }

    public static void payCost(ActionCostType type, int cost)
    {
        payCost(convertActionCostToProcType(type), cost);
    }

    public static void payCost(MultiStackProcType type, int cost)
    {
        exuberanceDictionary[type] -= cost;
        OnExuberanceChance.Invoke();
    }

    public static void addExuberance(ActionCostType type, int amount)
    {
        addExuberance(convertActionCostToProcType(type), amount);
    }

    public static void addExuberance(MultiStackProcType type, int amount)
    {
        exuberanceDictionary[type] += amount;
        OnExuberanceChance.Invoke();
    }

    private static MultiStackProcType convertActionCostToProcType(ActionCostType type)
    {
        switch (type)
        {
            case ActionCostType.RedKnife:
                return MultiStackProcType.RedKnife;
            case ActionCostType.BlueShield:
                return MultiStackProcType.BlueShield;
            case ActionCostType.YellowThorn:
                return MultiStackProcType.YellowThorn;
            case ActionCostType.GreenLeaf:
                return MultiStackProcType.GreenLeaf;
            default:
                throw new IOException("Cannot convert ActionCostType("+type.ToString()+") to MultiStackProcType");
        }
    }

    public static void setExuberancesToStartingAmount()
    {
        resetExuberanceDictionary();

        addExuberance(MultiStackProcType.RedKnife, PartyStats.getStartingRedKnife());
        addExuberance(MultiStackProcType.BlueShield, PartyStats.getStartingBlueShield());
        addExuberance(MultiStackProcType.YellowThorn, PartyStats.getStartingYellowThorn());
        addExuberance(MultiStackProcType.GreenLeaf, PartyStats.getStartingGreenLeaf());
    }

    private static void resetExuberanceDictionary()
    {

        exuberanceDictionary = new Dictionary<MultiStackProcType, int>();

        exuberanceDictionary.Add(MultiStackProcType.RedKnife, 0);
        exuberanceDictionary.Add(MultiStackProcType.BlueShield, 0);
        exuberanceDictionary.Add(MultiStackProcType.YellowThorn, 0);
        exuberanceDictionary.Add(MultiStackProcType.GreenLeaf, 0);
    }

}
