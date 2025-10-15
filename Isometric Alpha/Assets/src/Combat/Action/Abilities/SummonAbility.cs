using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SummonAbility: Ability
{
	private const string summonIconName = "Egg";

	private SummonCombos creaturesToSpawn;

	public SummonAbility(CombatActionSettings settings, EnemyStats creatureToSpawn): base(settings)
	{
		this.creaturesToSpawn = new SummonCombos(new EnemyStats[][]{new EnemyStats[]{creatureToSpawn}});
	}

    public SummonAbility(CombatActionSettings settings, EnemyStats[] creatureComboToSpawn) : base(settings)
    {
        this.creaturesToSpawn = new SummonCombos(new EnemyStats[][] { creatureComboToSpawn });
    }

    public SummonAbility(CombatActionSettings settings, EnemyStats[][] creatureCombosToSpawn) : base(settings)
    {
        this.creaturesToSpawn = new SummonCombos(creatureCombosToSpawn);
    }

    public override void queueingAction()
    {
		base.queueingAction();

		GridCoords[] allTileCoords = getSelector().getAllSelectorCoords();

        foreach (GridCoords coords in allTileCoords)
		{
			CombatStateManager.allQueuedSummonLocations.Add(coords);
		}
    }

    public override void unqueueingAction()
    {
        base.unqueueingAction();

        GridCoords[] allTileCoords = getSelector().getAllSelectorCoords();

        foreach (GridCoords coords in allTileCoords)
        {
            CombatStateManager.allQueuedSummonLocations.Remove(coords);
        }
    }

    public override void applySettings(CombatActionSettings settings)
    {
		settings.descriptionParams.iconName = summonIconName;

		base.applySettings(settings);
    }

    public override void performCombatAction()
	{
		EnemySpawner enemySpawner = EnemySpawner.getInstance();
		EnemyStats[] comboToSpawn = creaturesToSpawn.getNextCombo();
		Selector selector = getSelector();
		GridCoords[] targetCoords = selector.getAllSelectorCoords();
		
		int comboIndex = 0;

		foreach(GridCoords coords in targetCoords)
		{
			if(CombatGrid.getCombatantAtCoords(coords) != null)
            {
                comboIndex++;
                continue;
			}

			if(comboIndex < comboToSpawn.Length)
			{
				enemySpawner.spawnEnemy(comboToSpawn[comboIndex], coords);
			} else
			{
				break;
			}
			
			comboIndex++;
		}
	}

    public override void performCombatAction(ArrayList targets)
    {
		performCombatAction();
    }

    private struct SummonCombos
	{
		private EnemyStats[][] combos;
		private bool[] used;

		public SummonCombos(EnemyStats[][] combos)
		{
			this.combos = combos;
			this.used = new bool[combos.Length];
		}

		public EnemyStats[] getNextCombo()
		{
			if(getFirstUnusedComboInOrder() < 0)
			{
				resetUsed();
			}
			
			int comboIndex = 0;
			int attempts = 0;
			
			do{
				comboIndex = UnityEngine.Random.Range(0, used.Length);
				attempts++;
			} while(used[comboIndex] && attempts < (Int32.MaxValue/10000));
			
			if(attempts > 1000)
			{
				Debug.LogError("Attempts was high: attempts = " + attempts);
			}
			
			used[comboIndex] = true;
			return combos[comboIndex];
		}

		private int getFirstUnusedComboInOrder()
		{
			for(int comboIndex = 0; comboIndex < combos.Length; comboIndex++)
			{
				if(!used[comboIndex])
				{
					return comboIndex;
				}
			}
			
			return -1;
		}

		private void resetUsed()
		{
			this.used = new bool[combos.Length];
		}
	}
}
