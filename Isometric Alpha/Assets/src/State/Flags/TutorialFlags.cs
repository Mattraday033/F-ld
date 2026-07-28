using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class TutorialFlags 
{

	private static Dictionary<string, bool> tutorialFlags = new Dictionary<string, bool>();

	public static bool getFlag(string flagName, bool ignoreTutorialDisabledSetting = false)
	{
        if(!tutorialsEnabled() && !ignoreTutorialDisabledSetting)
        {
            return true;
        }

		if (!tutorialFlags.ContainsKey(flagName))
		{
			tutorialFlags.Add(flagName, false);
		}
            
        return tutorialFlags[flagName];
	}

    private static bool tutorialsEnabled()
    {
        return GameplaySettingsList.tutorialsEnabled.settingOptions[Constants.onSettingIndex].set;
    }

	public static void setFlag(string flagName, bool flagStatus)
	{
		if (!tutorialFlags.ContainsKey(flagName))
		{
			tutorialFlags.Add(flagName, flagStatus);
		}
		else
		{
			tutorialFlags[flagName] = flagStatus;
		}
	}

	public static void overwriteFlags(Dictionary<string, bool> newFlags)
	{
		tutorialFlags = new Dictionary<string, bool>();

		if (newFlags == null)
		{
			return;
		}

		foreach (KeyValuePair<string, bool> flag in newFlags)
		{
			tutorialFlags[flag.Key] = newFlags[flag.Key];
		}
	}

	public static void resetFlags()
	{
		tutorialFlags = new Dictionary<string, bool>();
	}

}
