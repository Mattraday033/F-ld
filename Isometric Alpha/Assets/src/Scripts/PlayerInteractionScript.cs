using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The purpose of a PlayerInteractionScript is to perform an additional behavior when certain player actions are performed.
//For example: Starting a quest or activating a quest step when opening a chest, setting a flag when walking through a door
//Subclasses will handle individual behaviours 
public class PlayerInteractionScript : ScriptableObject
{
    public virtual bool evaluateScript()
    {
        return false;
    }
    
    public virtual void runScript(GameObject target = null)
    {
        //empty on purpose
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