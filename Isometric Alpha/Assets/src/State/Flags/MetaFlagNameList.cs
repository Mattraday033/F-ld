using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MetaFlagNameList
{
    #region Generic Flags

    public const string inHostileArea = "inHostileArea";

    #endregion

    #region Guard Trial Flags

    #region Slave Crowd Spawn Group Flags

    //1 = didnt attack a barricade from the front that could be avoided by conversation (all but first)
    public const string attackedBarricadeSlaveSpawnGroup1 = "attackedBarricadeSlaveSpawnGroup1";

    //2 = got Tabor to surrender (whether you killed him or didn't)
    public const string taborSurrenderedSlaveSpawnGroup2 = "taborSurrenderedSlaveSpawnGroup2";

    //3 = used secret passage way to get into director's office
    public const string secretPassageIntoOfficeSlaveSpawnGroup3 = "secretPassageIntoOfficeSlaveSpawnGroup3";

    //4 = said that you were accepting prisoners in conversation with Janos
    public const string janosAcceptingPrisonersSlaveSpawnGroup4 = "janosAcceptingPrisonersSlaveSpawnGroup4";

    //5 = got tool bundle
    public const string toolBundleSlaveSpawnGroup5 = "toolBundleSlaveSpawnGroup5";

    //6 = convinced Imre to help (Done)
    public const string convincedImreSlaveSpawnGroup6 = "convincedImreSlaveSpawnGroup6";

    #endregion

    public const string marcosIsAtTrial = "marcosIsAtTrial";
    public const string taborIsAtTrial = "taborIsAtTrial";
    public const string andrasIsAtTrial = "andrasIsAtTrial"; 
    public const string janosIsAtTrial = "janosIsAtTrial"; 
    public const string guardPazmanAndRekaAtTrial = "guardPazmanAndRekaAtTrial"; 
    public const string noPrisoners = "noPrisoners";
    public const string marcosNeedsHandling = "marcosNeedsHandling";
    public const string andrasNeedsHandling = "andrasNeedsHandling";
    public const string rekaNeedsHandling = "rekaNeedsHandling";
    public const string pazmanNeedsHandling = "pazmanNeedsHandling";
    public const string taborNeedsHandling = "taborNeedsHandling";
    public const string gaveAGuardToTheCrowd = "gaveAGuardToTheCrowd";
    public const string executedAnyGuard = "executedAnyGuard";
    public const string nandorReadyToSpeakAfterTrial = "nandorReadyToSpeakAfterTrial";
    #endregion

    #region The Plan relevant flags

    public const string failedToConvinceSlavesToHelpYou = "failedToConvinceSlavesToHelpYou"; // failed slavesAfterKillingOverseerCampNE conversation

    #endregion
}
