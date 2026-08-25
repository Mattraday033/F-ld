/// <summary>
/// Keys into DialogueList.dialogueList for the dialogues that aren't found by the usual
/// areaName + npcName combination.
///
/// The Resources paths that used to live here are gone: ink stories are now addressed by
/// DialogueKey, which DialogueKeyGenerator derives from what is actually on disk. Use
/// InkAssetList.getInkJSON(DialogueKey.X) rather than reintroducing a path constant.
/// </summary>
public static class DialogueNameList
{
    public const string taborManse2F2BKey = "taborManse-2F-2B";
    public const string directorDefeatedConvoKey = NPCNameList.director + "DefeatedConvo";
    public const string guardPunishmentConvoKey = "guardPunishmentStartConvo";
    public const string chiefTaborPunishmentDialogueKey = "ChiefTabor";

    public const string janosAfterKillingAndrasKey = "JanosAfterKillingAndras";
    public const string afterTakacsFightKey = "afterTakacsFight";
    public const string slavesAfterKillingOverseerCampNEKey = "slavesAfterKillingOverseerCampNE";
    public const string taborAfterClayFightKey = "TaborAfterClayFight";
    public const string afterKillingGuardsMineLvl3Key = "AfterKillingGuardsMineLvl3";
}
