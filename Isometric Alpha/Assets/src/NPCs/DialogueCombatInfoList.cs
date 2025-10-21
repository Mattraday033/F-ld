using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogueCombatInfoList
{

    public readonly static NPCCombatInfo vazulCombatInfo = new NPCCombatInfo(new EnemyPackInfo[] { EnemyPackInfoList.guardVazulFight },
                                                                            new DeadNameList[] { new DeadNameList(new string[] { NPCNameList.guardVazul }) });

// new NPCCombatInfo(new EnemyPackInfo[]{     EnemyPackInfoList.halfSlavesNoGuardFight,
//                                                                                                                                                                                                                                         EnemyPackInfoList.halfSlavesFight,
//                                                                                                                                                                                                                                         EnemyPackInfoList.fullSlavesNoGuardFight,
//         /*Dont delete this white space*/																																																EnemyPackInfoList.fullSlavesFight},
//                                                                                                                                                                                                                      new DeadNameList[]{new DeadNameList(new string[]{NPCNameList.kende, NPCNameList.pan, NPCNameList.imre}),
//                                                                                                                                                                                                                                         new DeadNameList(new string[]{NPCNameList.kende, NPCNameList.pan, NPCNameList.imre}),
//                                                                                                                                                                                                                                         new DeadNameList(new string[]{NPCNameList.kende, NPCNameList.pan, NPCNameList.imre}),
//                                                                                                                                                                                                                                         new DeadNameList(new string[]{NPCNameList.kende, NPCNameList.pan, NPCNameList.imre})})));



}
