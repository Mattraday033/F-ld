using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SummonAbility: Ability
{
	private const string summonIconName = "Egg";

	private SummonCombos creaturesToSpawn;

	public SummonAbility(CombatActionSettings settings, string creatureKey): base(settings)
	{
		this.creaturesToSpawn = new SummonCombos(new string[][]{new string[]{creatureKey}});
	}

    public SummonAbility(CombatActionSettings settings, string[] creatureComboToSpawn) : base(settings)
    {
		this.creaturesToSpawn = new SummonCombos(new string[][]{ creatureComboToSpawn });
    }

    public SummonAbility(CombatActionSettings settings, string[][] creatureCombosToSpawn) : base(settings)
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
		List<EnemyStats> comboToSpawn = creaturesToSpawn.getNextCombo();
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

			if(comboIndex < comboToSpawn.Count)
			{
				enemySpawner.spawnEnemy(comboToSpawn[comboIndex], coords);
			} else
			{
				break;
			}
			
			comboIndex++;
		}

        playActivationAnimation();
	}

    public override void performCombatAction(ArrayList targets)
    {
		performCombatAction();
    }

    private class SummonCombos
	{
		private List<string[]> combos;
        private Random rng = new Random();


		public SummonCombos(string[][] combos)
		{
			this.combos = combos.ToList();
		}

		public List<EnemyStats> getNextCombo()
		{
            combos = combos.OrderBy(_ => rng.Next()).ToList();
            List<EnemyStats> list = new List<EnemyStats>();

            foreach(string creatureKey in combos[0])
            {
                list.Add(EnemyStatsList.getEnemyStats(creatureKey));
            }
			return list;
		}
	}
}
