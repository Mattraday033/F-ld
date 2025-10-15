using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Ink.Runtime;

public static class MetaFlags
{


    public static Dictionary<string, bool> metaFlags = new Dictionary<string, bool>();

    private static void buildMetaFlags()
    {
        metaFlags = new Dictionary<string, bool>();

        //who is at Guard Punishment scene
        metaFlags[MetaFlagNameList.marcosIsAtTrial] = !DeathFlagManager.isDead(NPCNameList.guardMarcos);
        metaFlags[MetaFlagNameList.taborIsAtTrial] = !DeathFlagManager.isDead(NPCNameList.chiefTabor);
        metaFlags[MetaFlagNameList.andrasIsAtTrial] = !DeathFlagManager.isDead(NPCNameList.guardAndras) && Flags.getFlag(FlagNameList.acceptingGuardPrisoners) && Flags.getFlag(FlagNameList.gotKeyFromJanos);
        metaFlags[MetaFlagNameList.janosIsAtTrial] = Flags.getFlag(FlagNameList.gotKeyFromJanos);
        metaFlags[MetaFlagNameList.guardPazmanAndRekaAtTrial] = Flags.getFlag(FlagNameList.mineLvl3ConvincedRekaAndPazman);
        metaFlags[MetaFlagNameList.noPrisoners] = !metaFlags[MetaFlagNameList.marcosIsAtTrial] && !metaFlags[MetaFlagNameList.taborIsAtTrial] && !metaFlags[MetaFlagNameList.andrasIsAtTrial] && !metaFlags[MetaFlagNameList.guardPazmanAndRekaAtTrial];

        //who has been handled in Guard Punishment scene
        metaFlags[MetaFlagNameList.marcosNeedsHandling] = metaFlags[MetaFlagNameList.marcosIsAtTrial] && !(Flags.getFlag(FlagNameList.didNotExecuteMarcos) || Flags.getFlag(FlagNameList.gaveMarcosToTheCrowd) || Flags.getFlag(FlagNameList.gaveMarcosFiftyLashes) || Flags.getFlag(FlagNameList.executedMarcos));
        metaFlags[MetaFlagNameList.andrasNeedsHandling] = metaFlags[MetaFlagNameList.andrasIsAtTrial] && !(Flags.getFlag(FlagNameList.didNotExecuteAndras) || Flags.getFlag(FlagNameList.gaveAndrasToTheCrowd) || Flags.getFlag(FlagNameList.gaveAndrasFiftyLashes) || Flags.getFlag(FlagNameList.executedAndras));
        metaFlags[MetaFlagNameList.rekaNeedsHandling] = metaFlags[MetaFlagNameList.guardPazmanAndRekaAtTrial] && !(Flags.getFlag(FlagNameList.didNotExecuteReka) || Flags.getFlag(FlagNameList.gaveRekaToTheCrowd) || Flags.getFlag(FlagNameList.gaveRekaFiftyLashes) || Flags.getFlag(FlagNameList.executedReka));
        metaFlags[MetaFlagNameList.pazmanNeedsHandling] = metaFlags[MetaFlagNameList.guardPazmanAndRekaAtTrial] && !(Flags.getFlag(FlagNameList.didNotExecutePazman) || Flags.getFlag(FlagNameList.gavePazmanToTheCrowd) || Flags.getFlag(FlagNameList.gavePazmanFiftyLashes) || Flags.getFlag(FlagNameList.executedPazman));
        metaFlags[MetaFlagNameList.taborNeedsHandling] = metaFlags[MetaFlagNameList.taborIsAtTrial] && !(Flags.getFlag(FlagNameList.didNotExecuteTabor) || Flags.getFlag(FlagNameList.gaveTaborToTheCrowd) || Flags.getFlag(FlagNameList.executedTabor));

        //Aggregate guard punishment flags
        metaFlags[MetaFlagNameList.gaveAGuardToTheCrowd] = Flags.getFlag(FlagNameList.gaveMarcosToTheCrowd) || Flags.getFlag(FlagNameList.gaveAndrasToTheCrowd) || Flags.getFlag(FlagNameList.gaveRekaToTheCrowd) || Flags.getFlag(FlagNameList.gavePazmanToTheCrowd) || Flags.getFlag(FlagNameList.gaveTaborToTheCrowd);
		metaFlags[MetaFlagNameList.executedAnyGuard] = Flags.getFlag(FlagNameList.executedMarcos) || Flags.getFlag(FlagNameList.executedAndras) || Flags.getFlag(FlagNameList.executedReka) || Flags.getFlag(FlagNameList.executedPazman) || Flags.getFlag(FlagNameList.executedTabor);

        metaFlags[MetaFlagNameList.nandorReadyToSpeakAfterTrial] = !(metaFlags[MetaFlagNameList.marcosNeedsHandling] || metaFlags[MetaFlagNameList.andrasNeedsHandling] || metaFlags[MetaFlagNameList.pazmanNeedsHandling] || metaFlags[MetaFlagNameList.rekaNeedsHandling] || metaFlags[MetaFlagNameList.taborNeedsHandling]);
    }


    public static Story addAllVariables(Story story)
    {
        buildMetaFlags();

        foreach (KeyValuePair<string, bool> kvp in metaFlags)
        {
            if (story.variablesState[kvp.Key] != null)
            {
                story.variablesState[kvp.Key] = kvp.Value;
            }
        }

        return story;
    }
}
