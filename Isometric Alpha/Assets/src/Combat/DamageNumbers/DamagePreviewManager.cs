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
		if ((actionToPreview.targetsAllySection() && CombatGrid.positionIsOnEnemySide(actualTarget.position)) ||
            (!actionToPreview.targetsAllySection() && CombatGrid.positionIsOnAlliedSide(actualTarget.position)) ||
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

/*
	private static DamagePreviewManager instance;

	public static Dictionary<GridCoords, HealthBarManager> damagePreviewHealthBarContainer = new Dictionary<GridCoords, HealthBarManager>();
	public static Dictionary<GridCoords, HealthBarManager> hoverDamagePreviewHealthBarContainer = new Dictionary<GridCoords, HealthBarManager>();
	public static CombatAction actionToPreview;

    [RuntimeInitializeOnLoadMethod]
    private static void initializeDamagePreviewManager()
    {
        damagePreviewHealthBarContainer = new Dictionary<GridCoords, HealthBarManager>();
        hoverDamagePreviewHealthBarContainer = new Dictionary<GridCoords, HealthBarManager>();
        instance = null;
        actionToPreview = null;
    }

    public static DamagePreviewManager getInstance()
    {
        return instance;
    }

	private void Awake()
	{
		if (instance != null)
		{
			throw new IOException("Another instance of DamagePreviewManager already exists");
		}

		instance = this;
	}

	public void removeCombatActionToPreview()
	{
		actionToPreview = null;
	}

	public void setupDamagePreviews(CombatAction action)
	{
		actionToPreview = action;

		setUpDamagePreviews();
	}

	public static void setUpDamagePreviews()
	{
		resetAllDamagePreviews();

		if (actionToPreview == null)
		{
			return;
		}
		else
		{
			CombatAction actionClone = actionToPreview.clone();

			actionClone.setToPreviewMode();

			List<Stats> actualTargets = SelectorManager.currentSelector.getAllTargets();
			List<Stats> cloneTargets = SelectorManager.currentSelector.getAllPreviewTargetClones();

			actionClone.performCombatAction(cloneTargets);

			for (int index = 0; index < actualTargets.Count && index < cloneTargets.Count; index++)
			{
				Stats currentActualTarget = (Stats)actualTargets[index];
				Stats currentCloneTarget = (Stats)cloneTargets[index];

                currentCloneTarget.healthBar = null;

				addDamagePreviewToHealthBar(currentActualTarget, currentCloneTarget);
			}

			addDamagePreviewToHealthBar(actionToPreview.getActorStats(), actionClone.getActorStats());
		}
	}

	public static void setUpHoverDamagePreview(Stats stats)
	{
		if (actionToPreview == null || stats == null)
		{
			return;
		}
		else
		{
			CombatAction actionClone = actionToPreview.clone();

			actionClone.setToPreviewMode();

			List<Stats> actualTargets = new List<Stats>();
			actualTargets.Add(stats);

			List<Stats> cloneTargets = new List<Stats>();
			cloneTargets.Add(stats.getPreviewClone());

			actionClone.performCombatAction(cloneTargets);

			for (int index = 0; index < actualTargets.Count && index < cloneTargets.Count; index++)
			{
				Stats currentActualTarget = (Stats)actualTargets[index];
				Stats currentCloneTarget = (Stats)cloneTargets[index];

				addDamagePreviewToHealthBar(currentActualTarget, currentCloneTarget, true);
			}
		}
	}
	private static void addDamagePreviewToHealthBar(Stats actualTarget, Stats cloneTarget)
	{
		addDamagePreviewToHealthBar(actualTarget, cloneTarget, false);
	}

	private static void addDamagePreviewToHealthBar(Stats actualTarget, Stats cloneTarget, bool isHoverPreview)
	{
		if (actualTarget == null || cloneTarget == null ||
			actualTarget.isDead())
		{
			return;
		}

		if (improperTargetForAction(actualTarget))
		{
			return;
		}

		if ((hasHoverPreviewAtCoords(actualTarget.position) && isHoverPreview) ||
			(hasPreviewAtCoords(actualTarget.position) && !isHoverPreview))
		{
			return;
		}

		if (hasHoverPreviewAtCoords(actualTarget.position) && !isHoverPreview)
		{
			damagePreviewHealthBarContainer[actualTarget.position] = hoverDamagePreviewHealthBarContainer[actualTarget.position];
			return;
		}

		HealthBarManager healthBar = actualTarget.healthBar;

		if ((healthBarAlreadyHasHoverPreview(healthBar) && isHoverPreview) || (healthBarAlreadyHasPreview(healthBar) && !isHoverPreview))
		{
			if (!actualTarget.hasHealthBarWithPreview())
			{
				healthBar.addPreviewHealth(actualTarget.currentHealth - cloneTarget.currentHealth);
			} 
			return;
		}

		if (healthBarAlreadyHasHoverPreview(healthBar) && !isHoverPreview)
		{
			damagePreviewHealthBarContainer[actualTarget.position] = healthBar;
			return;
		}

		if (healthBarAlreadyHasPreview(healthBar) && isHoverPreview)
		{
			return;
		}

		healthBar.addPreviewHealth(actualTarget.currentHealth - cloneTarget.currentHealth);

		if (isHoverPreview)
		{
			hoverDamagePreviewHealthBarContainer[actualTarget.position] = healthBar;
		}
		else
		{
			damagePreviewHealthBarContainer[actualTarget.position] = healthBar;
		}
	}

	public static void removeAllHoverPreviews()
	{
		foreach (KeyValuePair<GridCoords, HealthBarManager> kvp in hoverDamagePreviewHealthBarContainer)
        {
            if (SelectorManager.currentSelector.getAllSelectorCoords().Contains(kvp.Key))
            {
                damagePreviewHealthBarContainer[kvp.Key] = hoverDamagePreviewHealthBarContainer[kvp.Key];
            }
            else if (!hasPreviewAtCoords(kvp.Key))
            {
                CombatGrid.getCombatantAtCoords(kvp.Key).updateHealthBar();
            }
		}

        hoverDamagePreviewHealthBarContainer = new Dictionary<GridCoords, HealthBarManager>();
	}

	public static void resetAllDamagePreviews()
	{
		foreach (KeyValuePair<GridCoords, HealthBarManager> kvp in damagePreviewHealthBarContainer)
		{
			Stats hoverTarget = CombatGrid.getCombatantAtCoords(CombatTileHover.previousGridCoords);

            if (((hoverTarget != null && kvp.Value == hoverTarget.healthBar) || 
                kvp.Key.Equals(CombatTileHover.previousGridCoords)) && !improperTargetForAction(hoverTarget))
            {
                hoverDamagePreviewHealthBarContainer[kvp.Key] = damagePreviewHealthBarContainer[kvp.Key];
            }
            else
            {
                CombatGrid.getCombatantAtCoords(kvp.Key).updateHealthBar();
            }
		}

        damagePreviewHealthBarContainer = new Dictionary<GridCoords, HealthBarManager>();
	}

	public static void resetAllDamagePreviewsOnStateChange()
	{
		List<Stats> allCombatants = CombatGrid.getAllAliveCombatants();

		foreach (Stats stats in allCombatants)
		{
			stats.updateHealthBar();
		}

		hoverDamagePreviewHealthBarContainer = new Dictionary<GridCoords, HealthBarManager>();
		damagePreviewHealthBarContainer = new Dictionary<GridCoords, HealthBarManager>();
	}

	public static bool hasPreviewAtCoords(GridCoords coords)
	{
		return damagePreviewHealthBarContainer.ContainsKey(coords);
	}

	public static bool healthBarAlreadyHasPreview(HealthBarManager healthBar)
    {
        

		return damagePreviewHealthBarContainer.ContainsValue(healthBar);
	}

	public static bool hasHoverPreviewAtCoords(GridCoords coords)
	{
		return hoverDamagePreviewHealthBarContainer.ContainsKey(coords);
	}

	public static bool healthBarAlreadyHasHoverPreview(HealthBarManager healthBar)
	{
		return hoverDamagePreviewHealthBarContainer.ContainsValue(healthBar);
	}
	private static bool improperTargetForAction(Stats actualTarget)
	{
		if ((actionToPreview.targetsAllySection() && CombatGrid.positionIsOnEnemySide(actualTarget.position)) ||
            (!actionToPreview.targetsAllySection() && CombatGrid.positionIsOnAlliedSide(actualTarget.position)) ||
                actualTarget.queuedToMove())
		{
			return true;
		}
		else
		{
			return false;
		}
	}

*/