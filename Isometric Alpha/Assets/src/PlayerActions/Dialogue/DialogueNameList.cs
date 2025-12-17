using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DialogueNameList
{

    #region Dialogues Without Area Names
    public const string taborManse2F2BKey = "taborManse-2F-2B";
    public const string directorDefeatedConvoKey = NPCNameList.director + "DefeatedConvo";
    public const string guardPunishmentConvoKey = "guardPunishmentStartConvo";


    public const string chiefTaborPunishmentDialogueKey = "ChiefTabor";
    #endregion

    public const string seperatorChar = "/";
    public const string dialogueResourcesPathName = "Dialogue" + seperatorChar;

    public const string campPathName = dialogueResourcesPathName + ZoneKeyList.lovashiCamp + seperatorChar;
    public const string campInteriorPathName = campPathName + AreaNameList.lovashiCampInterior + seperatorChar;
    public const string campExteriorPathName = campPathName + AreaNameList.lovashiCampExterior + seperatorChar;

    #region Interactables
    public const string interactablesPath = dialogueResourcesPathName + "Interactables" + seperatorChar;

    #region Gates
    public const string gatesPath = interactablesPath + "Gates" + seperatorChar;
    public const string liftableRubblePath = gatesPath + "LiftableRubble";
    public const string fallenBeamPath = gatesPath + "FallenBeam";
    public const string awkwardRubblePath = gatesPath + NPCNameList.awkwardRubble;
    public const string ancientPortcullisPath = gatesPath + NPCNameList.ancientPortcullis;
    public const string liftableGatePath = gatesPath + NPCNameList.liftableGate;
    public const string cellDoorPath = gatesPath + NPCNameList.cellDoor;
    public const string unstablePillarPath = gatesPath + NPCNameList.unstablePillar;
    #endregion

    #region Vaultable Objects
    public const string vaultableObjectPath = interactablesPath + "VaultableObject";
    #endregion
    
    #endregion

    #region PartyMembers

    public const string partyMemberFolderPathName = dialogueResourcesPathName + "PartyMembers/";


    #endregion

    #region Camp
    public const string wallPatchPath = campPathName + "WallPatch";

    #region Slave Shack 1
    public const string sebPath = campInteriorPathName + LocationNameList.slaveShackOne + seperatorChar + NPCNameList.seb;
    public const string balintPath = campInteriorPathName + LocationNameList.slaveShackOne + seperatorChar + "Balint";
    #endregion
    #region Slave Shack 2
    public const string introDialoguePath = campInteriorPathName + LocationNameList.slaveShackTwo + seperatorChar + "IntroDialogue";
    public const string garchaPath = campInteriorPathName + LocationNameList.slaveShackTwo + seperatorChar + NPCNameList.garcha;
    #endregion
    #region Slave Shack 3
    public const string janosPath = campInteriorPathName + LocationNameList.slaveShackThree + seperatorChar + NPCNameList.janos;
    public const string janosAfterKillingAndrasPath = campInteriorPathName + LocationNameList.slaveShackThree + seperatorChar + "JanosAfterKillingAndras";
    public const string janosAfterKillingAndrasKey = "JanosAfterKillingAndras";
    public const string andrasPath = campInteriorPathName + LocationNameList.slaveShackThree + seperatorChar + "Andras";
    #endregion
    #region Slave Shack 4
    public const string kastorPlanPath = campInteriorPathName + LocationNameList.slaveShackFour + seperatorChar + "KastorPlan";
    public const string guardMarcosSS4Path = campInteriorPathName + LocationNameList.slaveShackFour + seperatorChar + "GuardMarcos";
    #endregion
    #region Slave Shack 5
    public const string ervinPath = campInteriorPathName + LocationNameList.slaveShackFive + seperatorChar + NPCNameList.ervin;
    #endregion
    #region Slave Shack 6
    public const string thatchPath = campInteriorPathName + LocationNameList.slaveShackSix + seperatorChar + NPCNameList.thatch;
    public const string slatePath = campInteriorPathName + LocationNameList.slaveShackSix + seperatorChar + NPCNameList.slate;
    public const string vazulPath = campInteriorPathName + LocationNameList.slaveShackSix + seperatorChar + "Vazul";
    public const string immovableRubblePath = campInteriorPathName + LocationNameList.slaveShackSix + seperatorChar + "ImmovableRubble";

    #endregion

    #region Mess Hall
    public const string kendePath = campInteriorPathName + LocationNameList.messHall + seperatorChar + NPCNameList.kende;
    #endregion


    #region Stables
    public const string horsePath = campInteriorPathName + LocationNameList.stables + seperatorChar + NPCNameList.horse;
    public const string beamPath = campInteriorPathName + LocationNameList.stables + seperatorChar + NPCNameList.beam;
    #endregion

    #region Stockhouse
    public const string urosPath = campInteriorPathName + LocationNameList.stockhouse + seperatorChar + NPCNameList.uros;
    public const string emesePath = campInteriorPathName + LocationNameList.stockhouse + seperatorChar + "Emese";
    public const string dudCratePath = campInteriorPathName + LocationNameList.stockhouse + seperatorChar + "DudCrates";
    public const string dudBarrelPath = campInteriorPathName + LocationNameList.stockhouse + seperatorChar + "DudBarrels";
    public const string barrelsWithNuggetPath = campInteriorPathName + LocationNameList.stockhouse + seperatorChar + "BarrelsWithNugget";

    #endregion

    #region Camp North East
    public const string leafPilePath = campExteriorPathName + LocationNameList.campNorthEast + seperatorChar + "LeafPile";
    public const string slavesAfterKillingOverseerCampNEKey = "slavesAfterKillingOverseerCampNE";
    public const string slavesAfterKillingOverseerCampNEPathName = campExteriorPathName + LocationNameList.campNorthEast + seperatorChar + slavesAfterKillingOverseerCampNEKey;
    
    public const string woundedSlaveNECampPathName = campExteriorPathName + LocationNameList.campNorthEast + seperatorChar + NPCNameList.woundedSlave;
    public const string woundedSlaveOneNECampPathName = campExteriorPathName + LocationNameList.campNorthEast + seperatorChar + NPCNameList.woundedSlave+"1";
    public const string woundedSlaveTwoNECampPathName = campExteriorPathName + LocationNameList.campNorthEast + seperatorChar + NPCNameList.woundedSlave+"2";

    public const string slaveFiveNECampPathName = campExteriorPathName + LocationNameList.campNorthEast + seperatorChar + NPCNameList.slave+"5";
    public const string slaveSixNECampPathName = campExteriorPathName + LocationNameList.campNorthEast + seperatorChar + NPCNameList.slave+"6";
    public const string slaveSevenNECampPathName = campExteriorPathName + LocationNameList.campNorthEast + seperatorChar + NPCNameList.slave+"7";
    public const string slaveEightNECampPathName = campExteriorPathName + LocationNameList.campNorthEast + seperatorChar + NPCNameList.slave+"8";
    public const string slaveNineNECampPathName = campExteriorPathName + LocationNameList.campNorthEast + seperatorChar + NPCNameList.slave+"9";

    public const string duringRevolutionSuffix = "DuringRevolution";
    public const string kastorNECampPathName = campExteriorPathName + LocationNameList.campNorthEast + seperatorChar + NPCNameList.kastor+duringRevolutionSuffix;
    public const string urosNECampPathName = campExteriorPathName + LocationNameList.campNorthEast + seperatorChar + NPCNameList.uros+duringRevolutionSuffix;
    public const string garchaNECampPathName = campExteriorPathName + LocationNameList.campNorthEast + seperatorChar + NPCNameList.garcha+duringRevolutionSuffix;
    public const string guardMarcosNECampPathName = campExteriorPathName + LocationNameList.campNorthEast + seperatorChar + NPCNameList.guardMarcos+duringRevolutionSuffix;
    #endregion

    #region Camp Center
    public const string csalanPath = campExteriorPathName + LocationNameList.campCenter + seperatorChar + NPCNameList.csalan;
    public const string taborPath = campExteriorPathName + LocationNameList.campCenter + seperatorChar + NPCNameList.tabor;
    public const string feherPath = campExteriorPathName + LocationNameList.campCenter + seperatorChar + NPCNameList.feher;
    public const string slavesWatchingTaborPath = campExteriorPathName + LocationNameList.campCenter + seperatorChar + "SlavesWatchingTabor";
    public const string guardWatchingTaborPath = campExteriorPathName + LocationNameList.campCenter + seperatorChar + "GuardWatchingTabor";
    public const string templePath = campExteriorPathName + LocationNameList.campCenter + seperatorChar + NPCNameList.temple;

    public const string campGatePath = campExteriorPathName + LocationNameList.campCenter + seperatorChar + NPCNameList.campGate;

    public const string pageBeforeLeavingPath = campExteriorPathName + LocationNameList.campCenter+ seperatorChar + NPCNameList.page;
    #endregion

    #region Camp Mine Entrance
    public const string muszaPath = campExteriorPathName + LocationNameList.campMineEntrance + seperatorChar + "GuardMuzsa";
    #endregion

    #region Camp Manse
    public const string imrePath = campExteriorPathName + LocationNameList.campManse + seperatorChar + NPCNameList.imre;
    public const string manseFrontDoorPath = campExteriorPathName + LocationNameList.campManse + seperatorChar + "ManseFrontDoor";
    public const string manseServiceEntrancePath = campExteriorPathName + LocationNameList.campManse + seperatorChar + "ManseServiceEntrance";
    #endregion
    #region Camp South East
    public const string directorStatuePath = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + "DirectorStatue";
    public const string brokenDirectorStatuePath = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + "BrokenDirectorStatue";
    public const string crowdSlave1Path = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + "CrowdSlave1";
    public const string crowdSlave2Path = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + "CrowdSlave2";
    public const string crowdSlave3Path = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + "CrowdSlave3";

    public const string kastorGuardPunishmentPath = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + NPCNameList.kastor;
    public const string garchaGuardPunishmentPath = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + NPCNameList.garcha;
    public const string broglinGuardPunishmentPath = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + NPCNameList.broglin;


    public const string guardPunishmentStartConvoPath = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + "GuardPunishmentStartConvo";
    public const string nandorGuardPunishmentPath = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + "Nandor";

    public const string carterGuardPunishmentPath = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + NPCNameList.carter;
    public const string thatchGuardPunishmentPath = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + NPCNameList.thatch;

    public const string ervinGuardPunishmentPath = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + NPCNameList.ervin;
    public const string janosGuardPunishmentPath = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + NPCNameList.janos;

    public const string pazmanGuardPunishmentPath = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + "Pazman";
    public const string rekaGuardPunishmentPath = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + "Reka";
    public const string taborGuardPunishmentPath = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + NPCNameList.chiefTabor;
    public const string marcosGuardPunishmentPath = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + "Marcos";
    public const string andrasGuardPunishmentPath = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + "Andras";

    public const string taborAfterClayFightKey = "TaborAfterClayFight";
    public const string taborAfterClayFightPath = campExteriorPathName + LocationNameList.campSouthEast + seperatorChar + "TaborAfterClayFight";

    #endregion

    #endregion

    #region Mine

    public const string mineFolderPath = dialogueResourcesPathName + "Mine/";

    #region MineLvl_2

    public const string mineLvl2FolderPath = mineFolderPath + "MineLvl_2/";

    public const string controlPanelPath = mineLvl2FolderPath + NPCNameList.controlPanel;
    public const string suspiciousWallPathML2 = mineLvl2FolderPath + NPCNameList.suspiciousWall;
    public const string mineArmoryGatePath = mineLvl2FolderPath + NPCNameList.mineArmoryGate;

    #endregion

    #region MineLvl_3
    public const string mineLvl3FolderPath = mineFolderPath + "MineLvl_3/";

    public const string ml3GuardCampLiftableGatePath = mineLvl3FolderPath + NPCNameList.guard + " " + NPCNameList.liftableGate;
    public const string rekaML3CampPath = mineLvl3FolderPath + "GuardReka";
    public const string ml3GuardBarricadePath = mineLvl3FolderPath + "GuardsCrate";
    public const string pazmanML3CampPath = mineLvl3FolderPath + "GuardPazman";
    public const string viragML3CampPath = mineLvl3FolderPath +  "GuardVirag";

    public const string ml3MinerBarricadePath = mineLvl3FolderPath + "MinersCrate";
    public const string ml3CarterPath = mineLvl3FolderPath + "Carter";
    public const string ml3MarcosPath = mineLvl3FolderPath + "GuardMarcos";
    public const string ml3NandorPath = mineLvl3FolderPath + "Nandor";

    public const string suspiciousWallPathML3 = mineLvl3FolderPath + NPCNameList.suspiciousWall;

    public const string pocketRubblePathML3 = mineLvl3FolderPath + NPCNameList.rubble;

    public const string afterKillingGuardsMineLvl3Path = mineLvl3FolderPath + "AfterKillingGuardsMineLvl3";
    public const string afterKillingGuardsMineLvl3Key = "AfterKillingGuardsMineLvl3";

    #endregion

    #endregion


    #region Manse

    public const string mansePathName = dialogueResourcesPathName + LocationNameList.manse + seperatorChar;
    public const string manseFirstFloorPathName =  mansePathName + ZoneKeyList.manseFirstFloor + seperatorChar;

    #region Manse-1F

    #region Manse-1F-Kitchens

    public const string kendeUponEnteringKitchensPathName = manseFirstFloorPathName + "kendeUponEnteringKitchens";

    public const string loyalImrePathName = manseFirstFloorPathName + "LoyalImre";

    #endregion

    #region Manse-1F-3b

    public const string beamAndCsalanPathName = manseFirstFloorPathName + NPCNameList.beam+"And"+NPCNameList.csalan;

    #endregion


    #endregion

    public const string manseSecondFloorPathName =  mansePathName + ZoneKeyList.manseSecondFloor + seperatorChar;

    #region Manse-2F

    #region Manse-2F-2c

    public const string chiefTaborManseSecondFloorPathName = manseSecondFloorPathName + NPCNameList.chiefTabor;

    #endregion

    #region Manse-2F-Office

    public const string officeDoorPathName = manseSecondFloorPathName + NPCNameList.officeDoor;
    public const string directorPathName = manseSecondFloorPathName + NPCNameList.director;
    public const string directorDefeatedPathName = manseSecondFloorPathName + NPCNameList.director+"DefeatedConvo";

    #endregion

    #endregion

    #endregion

}
