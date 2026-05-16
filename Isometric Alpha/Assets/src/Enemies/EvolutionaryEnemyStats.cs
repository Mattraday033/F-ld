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

	public EvolutionaryEnemyStats(string name, int armor, int tHP, string evolutionEnemyType, Trait[] traits, Dictionary<CharacterAnimationType, string> animationAudioClipDictionary = null): 
	base(name, armor, tHP, traits: traits, animationAudioClipDictionary: animationAudioClipDictionary)
	{
		this.evolutionEnemyType = EnemyStatsList.getEnemyStats(evolutionEnemyType);
		this.devolutionEnemyType = null;
	}
	
	public EvolutionaryEnemyStats(string name, int armor, int tHP, string evolutionEnemyType, string devolutionEnemyType, Trait[] traits, Dictionary<CharacterAnimationType, string> animationAudioClipDictionary = null): 
	base(name, armor, tHP, traits: traits, animationAudioClipDictionary: animationAudioClipDictionary)
	{
		this.evolutionEnemyType = EnemyStatsList.getEnemyStats(evolutionEnemyType);
		this.devolutionEnemyType = EnemyStatsList.getEnemyStats(devolutionEnemyType);
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
		
		CreatureSpawner.spawn(evolutionEnemyType.clone(), positions);
	}

	public override void devolve()
	{
		if(!canDevolve())
		{
			return;
		}
		
		setToDeadSprite(willEvolveOrDevolve);
		
		CreatureSpawner.spawn(evolutionEnemyType.clone(), positions);
	}

	public override bool notResurrectable()
	{
		return true;
	}

}
