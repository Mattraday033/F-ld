using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public static class DamagePreviewManager
{

    public readonly static UnityEvent<CombatAction> UpdateDamagePreviews = new UnityEvent<CombatAction>();
	public static Dictionary<Stats, HealthBarManager> damagePreviewHealthBarDict = new Dictionary<Stats, HealthBarManager>();

    [RuntimeInitializeOnLoadMethod]
    private static void initializeDamagePreviewManager()
    {
        damagePreviewHealthBarDict = new Dictionary<Stats, HealthBarManager>();
    }

	public static HealthBarManager applyAllPreviewDamageToHealthBar(Stats stats, HealthBarManager healthBar, CombatAction actionToPreview)
	{
        CombatAction actionClone = actionToPreview.clone();

        actionClone.setToPreviewMode();
        int healthBarOccurances = actionToPreview.getSelector().countHealthBarOccurances(healthBar);

        if(healthBarOccurances <= 0)
        {
            healthBarOccurances = 1;
        }

        List<Stats> cloneTarget = new List<Stats>();
        cloneTarget.Add(stats.getPreviewClone());

        actionClone.performCombatAction(cloneTarget);

        for (int index = 0; index < healthBarOccurances; index++)
        {
            healthBar.addPreviewHealth(stats.currentHealth - cloneTarget[Constants.indexZero].currentHealth);
        }

        return healthBar;
	}

    public static void addDamagePreview(Stats stats, HealthBarManager healthBar, CombatAction combatAction)
    {
        if(damagePreviewHealthBarDict.ContainsKey(stats) || stats == null || combatAction == null ||
			stats.isDead() || improperTargetForAction(stats, combatAction))
        {
            return;
        }

        healthBar = applyAllPreviewDamageToHealthBar(stats, healthBar, combatAction);

        damagePreviewHealthBarDict[stats] = healthBar;
    }

    public static void wipeAllDamagePreviews()
    {
        foreach(KeyValuePair<Stats, HealthBarManager> kvp in damagePreviewHealthBarDict)
        {
            kvp.Key.updateHealthBar();
        }

        damagePreviewHealthBarDict = new Dictionary<Stats, HealthBarManager>();
    }

    public static void removeDamagePreview(Stats stats)
    {
        if(damagePreviewHealthBarDict.ContainsKey(stats))
        {
            damagePreviewHealthBarDict.Remove(stats);
            stats.updateHealthBar();
        }
    }

	private static bool improperTargetForAction(Stats actualTarget, CombatAction actionToPreview)
	{
		if ((actionToPreview.targetsAllySection() && actualTarget.positions.Any(p => CombatGrid.positionIsOnEnemySide(p))) ||
            (!actionToPreview.targetsAllySection() && actualTarget.positions.Any(p => CombatGrid.positionIsOnAlliedSide(p))) ||
                actualTarget.queuedToMove())
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
