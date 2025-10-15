using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawnParams
{
    private const bool doNotSpawn = false;
    private const bool doSpawn = true;

    public StartSpawningFlagList startSpawningFlagList;
    public StopSpawningFlagList stopSpawningFlagList;

    public bool spawnWhileHostile;
    public bool onlySpawnWhileHostile;

    public NPCSpawnParams()
    {
        this.startSpawningFlagList = new StartSpawningFlagList();
        this.stopSpawningFlagList = new StopSpawningFlagList();
    }

    public NPCSpawnParams(StartSpawningFlagList startSpawningFlagList)
    {
        this.startSpawningFlagList = startSpawningFlagList;
        this.stopSpawningFlagList = new StopSpawningFlagList();
    }

    public NPCSpawnParams(StopSpawningFlagList stopSpawningFlagList)
    {
        this.startSpawningFlagList = new StartSpawningFlagList();
        this.stopSpawningFlagList = stopSpawningFlagList;
    }

    public NPCSpawnParams(StartSpawningFlagList startSpawningFlagList, StopSpawningFlagList stopSpawningFlagList)
    {
        this.startSpawningFlagList = startSpawningFlagList;
        this.stopSpawningFlagList = stopSpawningFlagList;
    }

    public NPCSpawnParams(StartSpawningFlagList startSpawningFlagList, StopSpawningFlagList stopSpawningFlagList, bool spawnWhileHostile, bool onlySpawnWhileHostile)
    {
        this.startSpawningFlagList = startSpawningFlagList;
        this.stopSpawningFlagList = stopSpawningFlagList;
        this.spawnWhileHostile = spawnWhileHostile;
        this.onlySpawnWhileHostile = onlySpawnWhileHostile;
    }

    public bool canSpawn(string npcName)
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

        if(stopSpawningFlagList.evaluateFlags())
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

public class PartyMemberNPCSpawnParams : NPCSpawnParams
{

    private bool ignoreInParty;
    
    public PartyMemberNPCSpawnParams(StartSpawningFlagList startSpawningFlagList, bool ignoreInParty):
    base(startSpawningFlagList)
    {
        
    }

    public PartyMemberNPCSpawnParams(StopSpawningFlagList stopSpawningFlagList, bool ignoreInParty):
    base(stopSpawningFlagList)
    {
        
    }

    public PartyMemberNPCSpawnParams(StartSpawningFlagList startSpawningFlagList, StopSpawningFlagList stopSpawningFlagList, bool ignoreInParty):
    base(startSpawningFlagList, stopSpawningFlagList)
    {
        this.ignoreInParty = ignoreInParty;
    }

    public PartyMemberNPCSpawnParams(StartSpawningFlagList startSpawningFlagList, StopSpawningFlagList stopSpawningFlagList, bool spawnWhileHostile, bool onlySpawnWhileHostile, bool ignoreInParty):
    base(startSpawningFlagList, stopSpawningFlagList, spawnWhileHostile, onlySpawnWhileHostile)
    {
        this.ignoreInParty = ignoreInParty;
    }
    
    public override bool ignoreInPartyForSpawning()
    {
        return ignoreInParty;
    }
}

public class FlagList : IEnumerable
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
    
    public virtual bool evaluateFlags()
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
}

public class StartSpawningFlagList : FlagList
{
    public StartSpawningFlagList():
    base()
    {
        
    }

    public StartSpawningFlagList(string[] flags) :
    base(flags)
    {
    }

    public override bool evaluateFlags()
    {
        if(flags.Length <= 0)
        {
            return true;
        }
        
        return base.evaluateFlags();
    }
    
}