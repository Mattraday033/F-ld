using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SpawnParams
{
    public abstract bool canSpawn(string npcName);
}

public class InteractableSpawnParams : SpawnParams
{
    public const bool doNotSpawn = false;
    public const bool doSpawn = true;

    public StartSpawningFlagList startSpawningFlagList;
    public StopSpawningFlagList stopSpawningFlagList;

    public bool spawnWhileHostile;
    public bool onlySpawnWhileHostile;

    public InteractableSpawnParams()
    {
        this.startSpawningFlagList = new StartSpawningFlagList();
        this.stopSpawningFlagList = new StopSpawningFlagList();
        spawnWhileHostile = true;
    }

    public InteractableSpawnParams(bool spawnWhileHostile)
    {
        this.startSpawningFlagList = new StartSpawningFlagList();
        this.stopSpawningFlagList = new StopSpawningFlagList();
        this.spawnWhileHostile = spawnWhileHostile;
        this.onlySpawnWhileHostile = false;
    }

    public InteractableSpawnParams(bool spawnWhileHostile, bool spawnOnlyWhileHostile)
    {
        this.startSpawningFlagList = new StartSpawningFlagList();
        this.stopSpawningFlagList = new StopSpawningFlagList();
        this.spawnWhileHostile = spawnWhileHostile;
        this.onlySpawnWhileHostile = spawnOnlyWhileHostile;
    }


    public InteractableSpawnParams(StartSpawningFlagList startSpawningFlagList)
    {
        this.startSpawningFlagList = startSpawningFlagList;
        this.stopSpawningFlagList = new StopSpawningFlagList();
    }

    public InteractableSpawnParams(StopSpawningFlagList stopSpawningFlagList)
    {
        this.startSpawningFlagList = new StartSpawningFlagList();
        this.stopSpawningFlagList = stopSpawningFlagList;
    }

    public InteractableSpawnParams(StartSpawningFlagList startSpawningFlagList, bool spawnWhileHostile)
    {
        this.startSpawningFlagList = startSpawningFlagList;
        this.stopSpawningFlagList = new StopSpawningFlagList();
        this.spawnWhileHostile = spawnWhileHostile;
        this.onlySpawnWhileHostile = false;
    }

    public InteractableSpawnParams(StopSpawningFlagList stopSpawningFlagList, bool spawnWhileHostile)
    {
        this.startSpawningFlagList = new StartSpawningFlagList();
        this.stopSpawningFlagList = stopSpawningFlagList;
        this.spawnWhileHostile = spawnWhileHostile;
        this.onlySpawnWhileHostile = false;
    }

    public InteractableSpawnParams(StartSpawningFlagList startSpawningFlagList, StopSpawningFlagList stopSpawningFlagList)
    {
        this.startSpawningFlagList = startSpawningFlagList;
        this.stopSpawningFlagList = stopSpawningFlagList;
    }

    public InteractableSpawnParams(StartSpawningFlagList startSpawningFlagList, StopSpawningFlagList stopSpawningFlagList, bool spawnWhileHostile)
    {
        this.startSpawningFlagList = startSpawningFlagList;
        this.stopSpawningFlagList = stopSpawningFlagList;
        this.spawnWhileHostile = spawnWhileHostile;
        this.onlySpawnWhileHostile = false;
    }

    public InteractableSpawnParams(StartSpawningFlagList startSpawningFlagList, StopSpawningFlagList stopSpawningFlagList, bool spawnWhileHostile, bool onlySpawnWhileHostile)
    {
        this.startSpawningFlagList = startSpawningFlagList;
        this.stopSpawningFlagList = stopSpawningFlagList;
        this.spawnWhileHostile = spawnWhileHostile;
        this.onlySpawnWhileHostile = onlySpawnWhileHostile;
    }

    public override bool canSpawn(string npcName)
    {
        if (DeathFlagManager.isDead(npcName))
        {
            return doNotSpawn;
        }

        if (!spawnWhileHostile && AreaList.currentSceneIsHostile())
        {
            return doNotSpawn;
        }

        if (onlySpawnWhileHostile && !AreaList.currentSceneIsHostile())
        {
            return doNotSpawn;
        }

        if (!ignoreInPartyForSpawning() && State.formation.contains(npcName))
        {
            return doNotSpawn;
        }

        if (!startSpawningFlagList.evaluateFlags())
        {
            return doNotSpawn;
        }

        if (!startSpawningFlagList.evaluateFlags())
        {
            return doNotSpawn;
        }

        if (stopSpawningFlagList.evaluateFlags())
        {
            return doNotSpawn;
        }

        return doSpawn;
    }

    public virtual bool ignoreInPartyForSpawning()
    {
        return true;
    }
}

public class NeverSpawnParams : InteractableSpawnParams
{
    public override bool canSpawn(string npcName)
    {
        return false;       
    }
}

public class StatBasedSpawnParams : InteractableSpawnParams
{
    private PrimaryStat primaryStat;
    private int statLevelRequirement;

    public StatBasedSpawnParams(PrimaryStat primaryStat, int statLevelRequirement, StopSpawningFlagList stopSpawningFlagList, bool spawnWhileHostile) :
    base(stopSpawningFlagList, spawnWhileHostile)
    {
        this.primaryStat = primaryStat;
        this.statLevelRequirement = statLevelRequirement;
    }
    
    public override bool canSpawn(string npcName)
    {
        if(!base.canSpawn(npcName))
        {
            return doNotSpawn;
        }

        switch(primaryStat)
        {
            case PrimaryStat.Strength:
                if(PartyStats.getHighestStrength() >= statLevelRequirement)
                {
                    return doSpawn;
                }
                break;
            case PrimaryStat.Dexterity:
                if(PartyStats.getHighestDexterity() >= statLevelRequirement)
                {
                    return doSpawn;
                }
                break;
            case PrimaryStat.Wisdom:
                if(PartyStats.getHighestWisdom() >= statLevelRequirement)
                {
                    return doSpawn;
                }
                break;
            case PrimaryStat.Charisma:
                if(PartyStats.getHighestCharisma() >= statLevelRequirement)
                {
                    return doSpawn;
                }
                break;
        }

        return doNotSpawn;
    }
}

public class MonsterSpawnParams : InteractableSpawnParams
{
    public MonsterSpawnParams() :
    base()
    {
        spawnWhileHostile = true;
        onlySpawnWhileHostile = true;
    }

    public MonsterSpawnParams(StartSpawningFlagList startSpawningFlagList) :
    base(startSpawningFlagList)
    {
        spawnWhileHostile = true;
        onlySpawnWhileHostile = true;
    }

    public MonsterSpawnParams(StopSpawningFlagList stopSpawningFlagList) :
    base(stopSpawningFlagList)
    {
        spawnWhileHostile = true;
        onlySpawnWhileHostile = true;
    }

    public MonsterSpawnParams(StartSpawningFlagList startSpawningFlagList, StopSpawningFlagList stopSpawningFlagList) :
    base(startSpawningFlagList, stopSpawningFlagList)
    {
        spawnWhileHostile = true;
        onlySpawnWhileHostile = true;
    }

    public override bool canSpawn(string monsterDefeatKey)
    {
        if (MonsterDefeatKeysList.monsterIsDefeated(monsterDefeatKey))
        {
            return doNotSpawn;
        }

        if (!startSpawningFlagList.evaluateFlags())
        {
            return doNotSpawn;
        }

        if (!startSpawningFlagList.evaluateFlags())
        {
            return doNotSpawn;
        }

        return doSpawn;
    }
}

public class PartyMemberSpawnParams : InteractableSpawnParams
{

    private bool ignoreInParty;

    public PartyMemberSpawnParams(bool ignoreInParty, StartSpawningFlagList startSpawningFlagList) :
    base(startSpawningFlagList)
    {
        this.ignoreInParty = ignoreInParty;
    }

    public PartyMemberSpawnParams(StartSpawningFlagList startSpawningFlagList, bool spawnWhileHostile) :
    base(startSpawningFlagList, spawnWhileHostile)
    {
        this.ignoreInParty = false;
    }

    public PartyMemberSpawnParams(StopSpawningFlagList stopSpawningFlagList, bool spawnWhileHostile) :
    base(stopSpawningFlagList, spawnWhileHostile)
    {
        this.ignoreInParty = false;
    }


    public PartyMemberSpawnParams(bool ignoreInParty, StopSpawningFlagList stopSpawningFlagList, bool spawnWhileHostile) :
    base(stopSpawningFlagList, spawnWhileHostile)
    {
        this.ignoreInParty = ignoreInParty;
    }

    public PartyMemberSpawnParams(bool ignoreInParty, StartSpawningFlagList startSpawningFlagList, StopSpawningFlagList stopSpawningFlagList) :
    base(startSpawningFlagList, stopSpawningFlagList)
    {
        this.ignoreInParty = ignoreInParty;
    }

    public PartyMemberSpawnParams(StartSpawningFlagList startSpawningFlagList, StopSpawningFlagList stopSpawningFlagList, bool spawnWhileHostile) :
    base(startSpawningFlagList, stopSpawningFlagList,spawnWhileHostile)
    {
        this.ignoreInParty = false;
    }

    public PartyMemberSpawnParams(bool ignoreInParty, StartSpawningFlagList startSpawningFlagList, StopSpawningFlagList stopSpawningFlagList, bool spawnWhileHostile, bool onlySpawnWhileHostile) :
    base(startSpawningFlagList, stopSpawningFlagList, spawnWhileHostile, onlySpawnWhileHostile)
    {
        this.ignoreInParty = ignoreInParty;
    }

    public override bool ignoreInPartyForSpawning()
    {
        return ignoreInParty;
    }
}

public class HiddenTerrainSpawnParams : SpawnParams
{
    private string secretDoorFlag;

    public HiddenTerrainSpawnParams(string secretDoorFlag)
    {
        this.secretDoorFlag = secretDoorFlag;
    }

    public override bool canSpawn(string npcName)
    {
        return SecretDoorFlags.secretDoorHasBeenDiscovered(secretDoorFlag);
    }
}

public class HostilitySpawnParams : SpawnParams
{
    private string locationName;

    public HostilitySpawnParams(string locationName)
    {
        this.locationName = locationName;
    }

    public override bool canSpawn(string npcName)
    {
        return AreaList.getArea(locationName).isHostile();
    }
}

public class SecretDoorObstacleSpawnParams : HiddenTerrainSpawnParams
{
    public SecretDoorObstacleSpawnParams(string secretDoorFlag):
    base(secretDoorFlag)
    {
        
    }

    public override bool canSpawn(string npcName)
    {
        return !base.canSpawn(npcName);
    }
}

public abstract class FlagList : IEnumerable
{
    public string[] flags;

    public FlagList()
    {
        this.flags = new string[0];
    }

    public FlagList(string[] flags)
    {
        this.flags = flags;
    }

    public abstract bool evaluateFlags();

    public IEnumerator GetEnumerator()
    {
        foreach (string flag in flags)
        {
            yield return flag;
        }
    }
}

public class StopSpawningFlagList : FlagList
{
    public StopSpawningFlagList():
    base()
    {
        
    }

    public StopSpawningFlagList(string[] flags) :
    base(flags)
    {
    }

    public override bool evaluateFlags()
    {
        foreach (string flag in this)
        {
            if (Flags.getFlag(flag))
            {
                return true;
            }
        }

        return false;
    }
}

public class StopSpawningMetaFlagList : StopSpawningFlagList
{
    private StopSpawningFlagList stopSpawningFlagList;

    public StopSpawningMetaFlagList(string[] metaFlags):
    base(metaFlags)
    {
    }

    public StopSpawningMetaFlagList(string[] metaFlags, StopSpawningFlagList stopSpawningFlagList):
    base(metaFlags)
    {
        this.stopSpawningFlagList = stopSpawningFlagList;
    }

    public override bool evaluateFlags()
    {
        if (stopSpawningFlagList != null && stopSpawningFlagList.evaluateFlags())
        {
            return true;
        }

        foreach (string metaFlag in this)
        {
            if (MetaFlags.getMetaFlag(metaFlag))
            {
                return true;
            }
        }

        return false;
    }

}

public class StartSpawningFlagList : FlagList
{
    public StartSpawningFlagList() :
    base()
    {

    }

    public StartSpawningFlagList(string[] flags) :
    base(flags)
    {
    }

    public override bool evaluateFlags()
    {
        if (flags.Length <= 0)
        {
            return true;
        }

        foreach (string flag in this)
        {
            if (Flags.getFlag(flag))
            {
                return true;
            }
        }

        return false;
    }

}

public class StartSpawningAllTrueFlagList : StartSpawningFlagList
{
    public StartSpawningAllTrueFlagList():
    base()
    {
        
    }

    public StartSpawningAllTrueFlagList(string[] flags) :
    base(flags)
    {
    }

    public override bool evaluateFlags()
    {
        if(flags.Length <= 0)
        {
            return true;
        }
        
        foreach (string flag in this)
        {
            if (!Flags.getFlag(flag))
            {
                return false;
            }
        }
        
        return true;
    }
    
}

public class StartSpawningAllTrueMetaFlagList : StartSpawningAllTrueFlagList
{
    private StartSpawningAllTrueFlagList startSpawningAllTrueFlagList;

    public StartSpawningAllTrueMetaFlagList(string[] metaFlags, StartSpawningAllTrueFlagList startSpawningAllTrueFlagList):
    base(metaFlags)
    {
        this.startSpawningAllTrueFlagList = startSpawningAllTrueFlagList;
    }

    public override bool evaluateFlags()
    {
        if(startSpawningAllTrueFlagList != null && !startSpawningAllTrueFlagList.evaluateFlags())
        {
            return false;
        }
        
        foreach (string metaFlag in this)
        {
            if (!MetaFlags.getMetaFlag(metaFlag))
            {
                return false;
            }
        }
        
        return true;
    }
    
}