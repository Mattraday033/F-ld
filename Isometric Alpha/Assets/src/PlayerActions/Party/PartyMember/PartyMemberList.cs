using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class PartyMemberList
{



    private const int higherStrength = 2;
    private const int higherDexterity = 2;
    private const int higherWisdom = 2;
    private const int higherCharisma = 2;
    private const int normalStat = 1;

    public static PartyMember getResetPartyMember(string allyName)
    {
        AbilityList.initialize();

        switch (allyName)
        {
            case NPCNameList.thatch:

                PartyMember thatch = new PartyMember(new AllyStats(NPCNameList.thatch, higherStrength, normalStat, normalStat, normalStat));

                thatch.stats.combatActionArray = new CombatActionArray(thatch.stats, Strength.getStartingActions());
                thatch.stats.combatActionArray.equipCombatAction(new Attack(ItemList.getItem(ItemList.weaponsListIndex, ItemList.cudgelIndex) as Weapon), 0);
                thatch.stats.equippedItems = new EquippedItems(thatch.stats);

                return thatch;
            case NPCNameList.nandor:

                PartyMember nandor = new PartyMember(new AllyStats(NPCNameList.nandor, normalStat, normalStat, higherWisdom, normalStat));

                nandor.stats.combatActionArray = new CombatActionArray(nandor.stats, Wisdom.getStartingActions());
                nandor.stats.equippedItems = new EquippedItems(nandor.stats);

                return nandor;
            case NPCNameList.carter:

                PartyMember carter =  new PartyMember(new AllyStats(NPCNameList.carter, normalStat, higherDexterity, normalStat, normalStat));

                carter.stats.combatActionArray = new CombatActionArray(carter.stats, Dexterity.getStartingActions());
                carter.stats.combatActionArray.equipCombatAction(new Attack(ItemList.getItem(ItemList.weaponsListIndex, ItemList.lightPickIndex) as Weapon), 0);
                carter.stats.equippedItems = new EquippedItems(carter.stats);

                return carter;
        }

        throw new IOException("No PartyMember by the name " + allyName + " exists");

    }

    public static bool isPartyMemberName(string name)
    {
        switch (name)
        {
            case NPCNameList.thatch:
            case NPCNameList.carter:
            case NPCNameList.nandor:
                return true;
            default:
                return false;
        }
    }

}
