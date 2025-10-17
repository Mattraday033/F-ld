using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionaryEnemyStats : EnemyStats
{
	private const bool willEvolveOrDevolve = true;
	private const bool wontEvolveOrDevolve = false;
	
	//[SerializeField]
	private EnemyStats evolutionEnemyType;
	//[SerializeField]
	private EnemyStats devolutionEnemyType;

	public EvolutionaryEnemyStats(string name, int armor, int tHP): 
	base(name, armor, tHP)
	{
		
	}

	public EvolutionaryEnemyStats(string name, int armor, int tHP, string evolutionEnemyType): 
	base(name, armor, tHP)
	{
		this.evolutionEnemyType = Resources.Load<EnemyStats>(evolutionEnemyType);
		this.devolutionEnemyType = null;
	}
	
	public EvolutionaryEnemyStats(string name, int armor, int tHP, string evolutionEnemyType, string devolutionEnemyType): 
	base(name, armor, tHP)
	{
		this.evolutionEnemyType = Resources.Load<EnemyStats>(evolutionEnemyType);
		this.devolutionEnemyType = Resources.Load<EnemyStats>(devolutionEnemyType);
	}

	public override void setToDeadSprite()
	{
		setToDeadSprite(canDevolve());
	}

	public void setToDeadSprite(bool willEvolveAfterDeath)
	{
		if(canDevolve() && !willEvolveAfterDeath)
		{
			base.setToDeadSprite();
			devolve();
		} else
		{
			base.setToDeadSprite();
		}
	}

	private bool canEvolve()
	{
		return evolutionEnemyType != null;
	}
	
	private bool canDevolve()
	{
		return devolutionEnemyType != null;
	}

	public override void evolve()
	{
		if(!canEvolve())
		{
			return;
		}
		
		setToDeadSprite(willEvolveOrDevolve);
		
		EnemySpawner.getInstance().spawnEnemy(evolutionEnemyType, position);
	}

	public override void devolve()
	{
		if(!canDevolve())
		{
			return;
		}
		
		setToDeadSprite(willEvolveOrDevolve);
		
		EnemySpawner.getInstance().spawnEnemy(devolutionEnemyType, position);
	}

	public override bool cantBeResurrected()
	{
		return true;
	}

}
