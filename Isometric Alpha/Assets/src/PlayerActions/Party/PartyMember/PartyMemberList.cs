using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class PartyMemberList
{

    private const string biffName = "Biff the Understudy";

    private const int higherStrength = 2;
    private const int higherDexterity = 2;
    private const int higherWisdom = 2;
    private const int higherCharisma = 2;
    private const int normalStat = 1;


    private readonly static EquippableItem[] thatchStartingArmor = new EquippableItem[] {
                                                                                    null, 
                                                                                    null, 
                                                                                    ItemList.getItem(new ArmorListID(ItemList.slaveRagsIndex)) as EquippableItem,
                                                                                    null, 
                                                                                    null, 
                                                                                    null
                                                                                    };

    private readonly static EquippableItem[] carterStartingArmor = new EquippableItem[] {
                                                                                    null, 
                                                                                    ItemList.getItem(new ArmorListID(ItemList.minersHelmetIndex)) as EquippableItem,
                                                                                    ItemList.getItem(new ArmorListID(ItemList.slaveRagsIndex)) as EquippableItem,
                                                                                    ItemList.getItem(new ArmorListID(ItemList.clothGlovesIndex)) as EquippableItem,
                                                                                    ItemList.getItem(new ArmorListID(ItemList.rottenSandalsIndex)) as EquippableItem,
                                                                                    null
                                                                                    };

    private readonly static EquippableItem[] nandorStartingArmor = new EquippableItem[] {
                                                                                    null, 
                                                                                    ItemList.getItem(new ArmorListID(ItemList.minersHelmetIndex)) as EquippableItem,
                                                                                    ItemList.getItem(new ArmorListID(ItemList.slaveRagsIndex)) as EquippableItem,
                                                                                    ItemList.getItem(new ArmorListID(ItemList.leatherGlovesIndex)) as EquippableItem,
                                                                                    ItemList.getItem(new ArmorListID(ItemList.rottenSandalsIndex)) as EquippableItem,
                                                                                    null
                                                                                    };

    private readonly static EquippableItem[] weftStartingArmor = new EquippableItem[] {
                                                                                    null, 
                                                                                    null,
                                                                                    ItemList.getItem(new ArmorListID(ItemList.servantsClothesIndex)) as EquippableItem,
                                                                                    null,
                                                                                    null,
                                                                                    null
                                                                                    };

    public static PartyMember getResetPartyMember(string allyName)
    {
        AbilityList.initialize();

        switch (allyName)
        {
            case NPCNameList.carter:

                PartyMember carter = new PartyMember(new AllyStats(NPCNameList.carter, normalStat, higherDexterity, normalStat, normalStat));

                carter.stats.combatActionArray = new CombatActionArray(carter.stats, Dexterity.getStartingActions(carter.stats));
                carter.stats.combatActionArray.equipCombatAction(new Attack(carter.stats, ItemList.getItem(ItemList.weaponsListIndex, ItemList.lightPickIndex) as Weapon), 0);
                carter.stats.equippedItems = new EquippedItems(carter.stats, carterStartingArmor);

                return carter;
            case NPCNameList.nandor:

                PartyMember nandor = new PartyMember(new AllyStats(NPCNameList.nandor, normalStat, normalStat, higherWisdom, normalStat));

                nandor.stats.combatActionArray = new CombatActionArray(nandor.stats, Wisdom.getStartingActions(nandor.stats));
                nandor.stats.equippedItems = new EquippedItems(nandor.stats, nandorStartingArmor);

                return nandor;
            case NPCNameList.thatch:

                PartyMember thatch = new PartyMember(new AllyStats(NPCNameList.thatch, higherStrength, normalStat, normalStat, normalStat));

                thatch.stats.combatActionArray = new CombatActionArray(thatch.stats, Strength.getStartingActions(thatch.stats));
                thatch.stats.combatActionArray.equipCombatAction(new Attack(thatch.stats, ItemList.getItem(ItemList.weaponsListIndex, ItemList.cudgelIndex) as Weapon), 0);
                thatch.stats.equippedItems = new EquippedItems(thatch.stats, thatchStartingArmor);

                return thatch;

            case NPCNameList.weft:

                PartyMember weft = new PartyMember(new AllyStats(NPCNameList.weft, normalStat, normalStat, normalStat, higherCharisma));

                weft.stats.combatActionArray = new CombatActionArray(weft.stats, Dexterity.getStartingActions(weft.stats));
                weft.stats.combatActionArray.equipCombatAction(new Attack(weft.stats, ItemList.getItem(ItemList.weaponsListIndex, ItemList.sharpRockIndex) as Weapon), 0);
                weft.stats.equippedItems = new EquippedItems(weft.stats, weftStartingArmor);

                return weft;
        }

        PartyMember defaultPartyMember = getResetPartyMember(NPCNameList.carter);

        defaultPartyMember.stats.name = biffName;

        return defaultPartyMember;
    }

    public static bool characterIsPartyMember(string name)
    {
        return !getResetPartyMember(name).getName().Equals(biffName);
    }
}
