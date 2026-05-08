using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllyAmountList
{

    #region Named NPCs

    #region Lovashi Guards
    public readonly static CreatureAmount guardReka = new CreatureAmount(Constants.oneCreature, AlliedSummonStatsList.getSummonStats(NPCNameList.guardReka));
    public readonly static CreatureAmount guardPazman = new CreatureAmount(Constants.oneCreature, AlliedSummonStatsList.getSummonStats(NPCNameList.guardPazman));
    public readonly static CreatureAmount guardVirag = new CreatureAmount(Constants.oneCreature, AlliedSummonStatsList.getSummonStats(NPCNameList.guardVirag));
    public readonly static CreatureAmount overseerGaspar = new CreatureAmount(Constants.oneCreature, AlliedSummonStatsList.getSummonStats(NPCNameList.overseerGaspar));

    public readonly static CreatureAmount chiefTabor = new CreatureAmount(Constants.oneCreature, AlliedSummonStatsList.getSummonStats(NPCNameList.chiefTabor));
    #endregion

    #endregion

    #region Slaves
    public readonly static CreatureAmount southEastSlaves = new CreatureAmount(Constants.fourCreatures, AlliedSummonStatsList.getSummonStats(MonsterNameList.brandedRioter));
    public readonly static CreatureAmount northEastSlaves = new CreatureAmount(Constants.sixCreatures, AlliedSummonStatsList.getSummonStats(MonsterNameList.brandedRioter));
    public readonly static CreatureAmount manseSlaves = new CreatureAmount(Constants.fiveCreatures, AlliedSummonStatsList.getSummonStats(MonsterNameList.noBrandRioter));
    #endregion

}
