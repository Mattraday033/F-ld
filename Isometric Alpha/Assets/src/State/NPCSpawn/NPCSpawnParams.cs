using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawnParams
{
    public const bool doNotSpawn = false;
    public const bool doSpawn = true;

    public StartSpawningFlagList startSpawningFlagList;
    public StopSpawningFlagList stopSpawningFlagList;

    public bool spawnWhileHostile;
    public bool onlySpawnWhileHostile;

    public NPCSpawnParams()
    {
        this.startSpawningFlagList = new StartSpawningFlagList();
        this.stopSpawningFlagList = new StopSpawningFlagList();
    }

    public NPCSpawnParams(bool spawnWhileHostile)
    {
        this.startSpawningFlagList = new StartSpawningFlagList();
        this.stopSpawningFlagList = new StopSpawningFlagList();
        this.spawnWhileHostile = spawnWhileHostile;
        this.onlySpawnWhileHostile = false;
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

    public NPCSpawnParams(StartSpawningFlagList startSpawningFlagList, bool spawnWhileHostile)
    {
        this.startSpawningFlagList = startSpawningFlagList;
        this.stopSpawningFlagList = new StopSpawningFlagList();
        this.spawnWhileHostile = spawnWhileHostile;
        this.onlySpawnWhileHostile = false;
    }

    public NPCSpawnParams(StopSpawningFlagList stopSpawningFlagList, bool spawnWhileHostile)
    {
        this.startSpawningFlagList = new StartSpawningFlagList();
        this.stopSpawningFlagList = stopSpawningFlagList;
        this.spawnWhileHostile = spawnWhileHostile;
        this.onlySpawnWhileHostile = false;
    }

    public NPCSpawnParams(StartSpawningFlagList startSpawningFlagList, StopSpawningFlagList stopSpawningFlagList)
    {
        this.startSpawningFlagList = startSpawningFlagList;
        this.stopSpawningFlagList = stopSpawningFlagList;
    }

    public NPCSpawnParams(StartSpawningFlagList startSpawningFlagList, StopSpawningFlagList stopSpawningFlagList, bool spawnWhileHostile)
    {
        this.startSpawningFlagList = startSpawningFlagList;
        this.stopSpawningFlagList = stopSpawningFlagList;
        this.spawnWhileHostile = spawnWhileHostile;
        this.onlySpawnWhileHostile = false;
    }

    public NPCSpawnParams(StartSpawningFlagList startSpawningFlagList, StopSpawningFlagList stopSpawningFlagList, bool spawnWhileHostile, bool onlySpawnWhileHostile)
    {
        this.startSpawningFlagList = startSpawningFlagList;
        this.stopSpawningFlagList = stopSpawningFlagList;
        this.spawnWhileHostile = spawnWhileHostile;
        this.onlySpawnWhileHostile = onlySpawnWhileHostile;
    }

    public virtual bool canSpawn(string npcName)
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

public class MonsterSpawnParams : NPCSpawnParams
{
    public MonsterSpawnParams():
    base()
    {
        spawnWhileHostile = true;
        onlySpawnWhileHostile = true;
    }

    public MonsterSpawnParams(StartSpawningFlagList startSpawningFlagList):
    base(startSpawningFlagList)
    {
        spawnWhileHostile = true;
        onlySpawnWhileHostile = true;
    }

    public MonsterSpawnParams(StopSpawningFlagList stopSpawningFlagList):
    base(stopSpawningFlagList)
    {
        spawnWhileHostile = true;
        onlySpawnWhileHostile = true;
    }

    public MonsterSpawnParams(StartSpawningFlagList startSpawningFlagList, StopSpawningFlagList stopSpawningFlagList):
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

public class PartyMemberNPCSpawnParams : NPCSpawnParams
{

    private bool ignoreInParty;

    public PartyMemberNPCSpawnParams(bool ignoreInParty, StartSpawningFlagList startSpawningFlagList) :
    base(startSpawningFlagList)
    {
        this.ignoreInParty = ignoreInParty;
    }

    public PartyMemberNPCSpawnParams(StartSpawningFlagList startSpawningFlagList, bool spawnWhileHostile) :
    base(startSpawningFlagList, spawnWhileHostile)
    {
        this.ignoreInParty = false;
    }

    public PartyMemberNPCSpawnParams(StopSpawningFlagList stopSpawningFlagList, bool spawnWhileHostile) :
    base(stopSpawningFlagList, spawnWhileHostile)
    {
        this.ignoreInParty = false;
    }


    public PartyMemberNPCSpawnParams(bool ignoreInParty, StopSpawningFlagList stopSpawningFlagList, bool spawnWhileHostile) :
    base(stopSpawningFlagList, spawnWhileHostile)
    {
        this.ignoreInParty = ignoreInParty;
    }

    public PartyMemberNPCSpawnParams(bool ignoreInParty, StartSpawningFlagList startSpawningFlagList, StopSpawningFlagList stopSpawningFlagList) :
    base(startSpawningFlagList, stopSpawningFlagList)
    {
        this.ignoreInParty = ignoreInParty;
    }

    public PartyMemberNPCSpawnParams(bool ignoreInParty, StartSpawningFlagList startSpawningFlagList, StopSpawningFlagList stopSpawningFlagList, bool spawnWhileHostile, bool onlySpawnWhileHostile) :
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

        return base.evaluateFlags();
    }

}

public class StartSpawningAllTrueFlagList : FlagList
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