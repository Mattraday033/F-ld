using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The purpose of a PlayerInteractionScript is to perform an additional behavior when certain player actions are performed.
//For example: Starting a quest or activating a quest step when opening a chest, setting a flag when walking through a door
//Subclasses will handle individual behaviours 
public class PlayerInteractionScript : ScriptableObject
{
    public virtual void runScript()
    {
        //empty on purpose
    }
    
    public virtual bool evaluateScript()
    {
        return false;
    }
    
    public virtual void runScript(GameObject target)
    {
        //empty on purpose
    }

    public static int getIndexOfFirstScriptToEvaluate(PlayerInteractionScript[] scripts)
    {
        if (scripts == null)
        {
            return -1;
        }

        for (int index = 0; index < scripts.Length; index++)
        {
            if (scripts[index] == null)
            {
                continue;
            }

            if (scripts[index].evaluateScript())
            {
                return index;
            }
        }

        return -1;
    }

    public static bool evaluateAnyScript(PlayerInteractionScript[] scripts)
    {
        if (scripts == null)
        {
            return false;
        }

        foreach (PlayerInteractionScript script in scripts)
        {
            if (script == null)
            {
                continue;
            }

            if (script.evaluateScript())
            {
                return true;
            }
        }

        return false;
    }

    public static bool evaluateAllScripts(PlayerInteractionScript[] scripts)
    {
        if (scripts == null)
        {
            return false;
        }

        foreach (PlayerInteractionScript script in scripts)
        {
            if (script == null)
            {
                continue;
            }

            if (!script.evaluateScript())
            {
                return false;
            }
        }

        return true;
    }

    public static void runAllScripts(PlayerInteractionScript[] scripts)
    {
        if(scripts == null)
        {
            return;
        }

        foreach(PlayerInteractionScript script in scripts)
        {
            if(script == null)
            {
                continue;
            }

            script.runScript();
        }
    }

}
