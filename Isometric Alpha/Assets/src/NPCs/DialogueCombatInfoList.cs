using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogueCombatInfoList
{

    public readonly static NPCCombatInfo vazulCombatInfo = new NPCCombatInfo(new EnemyPackInfo[] { EnemyPackInfoList.guardVazulFight },
                                                                            new DeadNameList[] { new DeadNameList(new string[] { NPCNameList.guardVazul }) });

    public readonly static NPCCombatInfo andrasCombatInfo = new NPCCombatInfo(new EnemyPackInfo[] { EnemyPackInfoList.guardAndrasWithKeyFight, EnemyPackInfoList.guardAndrasWithOutKeyFight},
                                                                            new DeadNameList[] { new DeadNameList(new string[] { NPCNameList.guardAndras, NPCNameList.andras }) });

    public readonly static NPCCombatInfo imreCombatInfo = new NPCCombatInfo(new EnemyPackInfo[] { EnemyPackInfoList.imreFight },
                                                                            new DeadNameList[] { new DeadNameList(new string[] { NPCNameList.imre }) });



    // new NPCCombatInfo(new EnemyPackInfo[]{     EnemyPackInfoList.halfSlavesNoGuardFight,
    //                                                                                                                                                                                                                                         EnemyPackInfoList.halfSlavesFight,
    //                                                                                                                                                                                                                                         EnemyPackInfoList.fullSlavesNoGuardFight,
    //         /*Dont delete this white space*/																																																EnemyPackInfoList.fullSlavesFight},
    //                                                                                                                                                                                                                      new DeadNameList[]{new DeadNameList(new string[]{NPCNameList.kende, NPCNameList.pan, NPCNameList.imre}),
    //                                                                                                                                                                                                                                         new DeadNameList(new string[]{NPCNameList.kende, NPCNameList.pan, NPCNameList.imre}),
    //                                                                                                                                                                                                                                         new DeadNameList(new string[]{NPCNameList.kende, NPCNameList.pan, NPCNameList.imre}),
    //                                                                                                                                                                                                                                         new DeadNameList(new string[]{NPCNameList.kende, NPCNameList.pan, NPCNameList.imre})})));



}
