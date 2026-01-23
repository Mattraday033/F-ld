using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllyAmountList
{

    #region Named NPCs
    public readonly static CreatureAmount guardReka = new CreatureAmount(Constants.oneCreature, AlliedSummonStatsList.getSummonStats(NPCNameList.guardReka));
    public readonly static CreatureAmount guardPazman = new CreatureAmount(Constants.oneCreature, AlliedSummonStatsList.getSummonStats(NPCNameList.guardPazman));
    public readonly static CreatureAmount guardVirag = new CreatureAmount(Constants.oneCreature, AlliedSummonStatsList.getSummonStats(NPCNameList.guardVirag));
    public readonly static CreatureAmount overseerGaspar = new CreatureAmount(Constants.oneCreature, AlliedSummonStatsList.getSummonStats(NPCNameList.overseerGaspar));

    #endregion

	

}
