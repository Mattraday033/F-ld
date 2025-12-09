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

    public readonly static DeadNameList allMineGuardsDead = new DeadNameList(new string[] { 
                                                                                            NPCNameList.guardPazman, 
                                                                                            NPCNameList.pazman, 
                                                                                            NPCNameList.guardReka,
                                                                                            NPCNameList.reka,
                                                                                            NPCNameList.guardVirag,
                                                                                            NPCNameList.overseerGaspar
                                                                                          });

    public readonly static NPCCombatInfo mineLvl3GuardsCombatInfo = new NPCCombatInfo(new EnemyPackInfo[] { EnemyPackInfoList.ml3GuardsWithBarricades, EnemyPackInfoList.ml3GuardsWithoutBarricades},
                                                                            new DeadNameList[] { allMineGuardsDead,
                                                                                                 allMineGuardsDead
                                                                                               });

    public readonly static DeadNameList gasparViragDead = new DeadNameList(new string[] { 
                                                                                            NPCNameList.guardVirag,
                                                                                            NPCNameList.overseerGaspar
                                                                                          });

    public readonly static NPCCombatInfo breachRubbleCombatInfo = new NPCCombatInfo(new EnemyPackInfo[] { EnemyPackInfoList.ml3GuardsNoSurrenders, EnemyPackInfoList.ml3GuardsRekaPazmanSurrender},
                                                                            new DeadNameList[] { allMineGuardsDead,
                                                                                                 gasparViragDead
                                                                                               });

    public readonly static DeadNameList kendeKitchenDeadNames = new DeadNameList(new string[]{NPCNameList.kende, NPCNameList.pan, NPCNameList.imre});

    public readonly static NPCCombatInfo kendeInKitchensCombatInfo = new NPCCombatInfo( 
                                                                     new EnemyPackInfo[]   {   
                                                                                                EnemyPackInfoList.kendeKitchensHalfSlavesNoGuard,
                                                                                                EnemyPackInfoList.kendeKitchensHalfSlaves,
                                                                                                EnemyPackInfoList.kendeKitchensFullSlavesNoGuard,
                                                                                                EnemyPackInfoList.kendeKitchensFullSlavesNoGuard
                                                                                            },
                                                                    new DeadNameList[]  {
                                                                                            kendeKitchenDeadNames,
                                                                                            kendeKitchenDeadNames,
                                                                                            kendeKitchenDeadNames,
                                                                                            kendeKitchenDeadNames
                                                                                        });

    public readonly static NPCCombatInfo taborCombatInfo = new NPCCombatInfo( 
                                                                     new EnemyPackInfo[]   {   
                                                                                                EnemyPackInfoList.taborManseSecondFloorFight
                                                                                            },
                                                                    new DeadNameList[]  {
                                                                                            new DeadNameList(new string[]{NPCNameList.tabor, NPCNameList.chiefTabor})
                                                                                        });

    public readonly static NPCCombatInfo directorCombatInfo = new NPCCombatInfo( 
                                                                     new EnemyPackInfo[]   {   
                                                                                                EnemyPackInfoList.directorWithBarricades,
                                                                                                EnemyPackInfoList.directorWithoutBarricades
                                                                                            },
                                                                    new DeadNameList[]  {
                                                                                            new DeadNameList(new string[0]),
                                                                                            new DeadNameList(new string[0])
                                                                                        });

    public readonly static NPCCombatInfo beamAndCsalanCombatInfo = new NPCCombatInfo( 
                                                                     new EnemyPackInfo[]   {   
                                                                                                EnemyPackInfoList.beamAndCsalanFight
                                                                                            },
                                                                    new DeadNameList[]  {
                                                                                            new DeadNameList(new string[]{NPCNameList.beam, NPCNameList.csalan})
                                                                                        });

}
