using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DamagePreviewManager : MonoBehaviour
{
	private static DamagePreviewManager instance;

	public static Dictionary<GridCoords, HealthBarManager> damagePreviewHealthBarContainer = new Dictionary<GridCoords, HealthBarManager>();
	public static Dictionary<GridCoords, HealthBarManager> hoverDamagePreviewHealthBarContainer = new Dictionary<GridCoords, HealthBarManager>();
	public static CombatAction actionToPreview;

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

			ArrayList actualTargets = SelectorManager.currentSelector.getAllTargets();
			ArrayList cloneTargets = SelectorManager.currentSelector.getAllTargetClones();

			actionClone.performCombatAction(cloneTargets);

			for (int index = 0; index < actualTargets.Count && index < cloneTargets.Count; index++)
			{
				Stats currentActualTarget = (Stats)actualTargets[index];
				Stats currentCloneTarget = (Stats)cloneTargets[index];

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

			ArrayList actualTargets = new ArrayList();
			actualTargets.Add(stats);

			ArrayList cloneTargets = new ArrayList();
			cloneTargets.Add(stats.clone());

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
			actualTarget.isDead || cloneTarget.isDead)
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

		HealthBarManager healthBarManager = actualTarget.healthBarManager;

		if ((healthBarAlreadyHasHoverPreview(healthBarManager) && isHoverPreview) || (healthBarAlreadyHasPreview(healthBarManager) && !isHoverPreview))
		{
			if (!actualTarget.hasHealthBarWithPreview())
			{
				healthBarManager.addPreviewHealth(actualTarget.currentHealth - cloneTarget.currentHealth);
			} 
			return;
		}

		if (healthBarAlreadyHasHoverPreview(healthBarManager) && !isHoverPreview)
		{
			damagePreviewHealthBarContainer[actualTarget.position] = healthBarManager;
			return;
		}

		if (healthBarAlreadyHasPreview(healthBarManager) && isHoverPreview)
		{
			return;
		}

		healthBarManager.addPreviewHealth(actualTarget.currentHealth - cloneTarget.currentHealth);

		if (isHoverPreview)
		{
			hoverDamagePreviewHealthBarContainer[actualTarget.position] = healthBarManager;
		}
		else
		{
			damagePreviewHealthBarContainer[actualTarget.position] = healthBarManager;
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

			if (kvp.Key.Equals(CombatTileHover.previousGridCoords) ||
				(hoverTarget != null && kvp.Value == hoverTarget.healthBarManager))
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
		ArrayList allCombatants = CombatGrid.getAllAliveCombatants();

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
			(!actionToPreview.targetsAllySection() && CombatGrid.positionIsOnAlliedSide(actualTarget.position)))
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
	public static void removeAllUpdatedPreviews()
	{
		for (int row = CombatGrid.rowUpperBounds; row <= CombatGrid.rowLowerBounds; row++)
		{
			for (int col = CombatGrid.colLeftBounds; col <= CombatGrid.colRightBounds; col++)
			{
				GridCoords currentCoords = new GridCoords(row, col);
				Stats stats = CombatGrid.getCombatantAtCoords(currentCoords);

				if (hoverDamagePreviewHealthBarContainer.ContainsKey(currentCoords) && stats != null && !stats.hasHealthBarWithPreview())
				{
					hoverDamagePreviewHealthBarContainer.Remove(currentCoords);
				}

				if (damagePreviewHealthBarContainer.ContainsKey(currentCoords) && stats != null && !stats.hasHealthBarWithPreview())
				{
					damagePreviewHealthBarContainer.Remove(currentCoords);
				}
			}
		}
	}
*/