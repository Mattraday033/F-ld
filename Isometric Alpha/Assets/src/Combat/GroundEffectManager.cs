using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public struct GroundEffect
{
	public string damageFormula;
	public int turnsRemaining;
	public GridCoords position;
	public GameObject indicator;
	
	public GroundEffect(string damageFormula, int turnsRemaining, GridCoords position, GameObject indicator)
	{
		this.damageFormula = damageFormula;
		this.turnsRemaining = turnsRemaining;
		this.position = position;
		this.indicator = indicator;
	}
	
	public void tickDown()
	{
		if(turnsRemaining > 0)
		{
			turnsRemaining--;
		}
	}
	
	public void instantiate()
	{
		indicator = GameObject.Instantiate(indicator, GroundEffectManager.getInstance().indicatorParent);
		
		indicator.transform.position = CombatGrid.fullCombatGrid[position.row][position.col];
		
		Helpers.updateGameObjectPosition(indicator);
	}
	
	public void destroy()
	{
		GameObject.Destroy(indicator);
	}
	
	public GroundEffect clone()
	{
		return new GroundEffect(damageFormula, turnsRemaining, position.clone(), indicator);
	}
}

public class GroundEffectManager : MonoBehaviour
{
	private static GroundEffectManager instance;
	private const bool isNotACrit = false;
	private const bool doesNotHealTarget = false;
	
	public Transform indicatorParent;
	public Transform damageNumberCanvas;
	public ArrayList allGroundEffects = new ArrayList();

	public static void createNewGroundEffect(GroundEffect template, GridCoords coords)
	{
		GroundEffect newGroundEffect = template.clone();
		
		newGroundEffect.position = coords.clone();
		
		removeGroundEffect(coords);
		
		newGroundEffect.instantiate();
		
		getInstance().allGroundEffects.Add(newGroundEffect);
	}
	
	public static void removeGroundEffect(GridCoords positionToRemoveAt)
	{
		for(int effectIndex = 0; effectIndex < getAllGroundEffects().Count; effectIndex++)
		{
			if(((GroundEffect) getAllGroundEffects()[effectIndex]).position.Equals(positionToRemoveAt))
			{
				GroundEffect removedGroundEffect = (GroundEffect) getAllGroundEffects()[effectIndex];
				
				getAllGroundEffects().RemoveAt(effectIndex);
				
				removedGroundEffect.destroy();
				
				return;
			}
		}
	}
	
	public static ArrayList getAllGroundEffects()
	{
		return getInstance().allGroundEffects;
	}
	
	public static void applyAllGroundEffectDamage()
	{
		foreach(GroundEffect groundEffect in getInstance().allGroundEffects)
		{
			Stats target = CombatGrid.getCombatantAtCoords(groundEffect.position);
			
			if(target == null || target is null)
			{
				continue;
			} else
			{
				int damageDealt = DamageCalculator.calculateFormula(groundEffect.damageFormula, DamageCalculator.noStatsSource);
				
				DamageNumberPopup.create(damageDealt, CombatGrid.getPositionAt(groundEffect.position), getInstance().damageNumberCanvas, isNotACrit, doesNotHealTarget);
				target.modifyCurrentHealth(DamageCalculator.calculateFormula(groundEffect.damageFormula, DamageCalculator.noStatsSource));
			}
			
			groundEffect.tickDown();
		}
	}
	
	public static void removeAllFinishedGroundEffects()
	{
		for(int effectIndex = 0; effectIndex < getAllGroundEffects().Count; effectIndex++)
		{
			GroundEffect currentGroundEffect = (GroundEffect) getAllGroundEffects()[effectIndex];
			
			if(currentGroundEffect.turnsRemaining <= 0)
			{
				getAllGroundEffects().RemoveAt(effectIndex);
				effectIndex--;
			}
		}
	}
	
	public static GroundEffectManager getInstance()
	{
		return instance;
	}
	
	private void Awake()
	{
		if(instance != null)
		{
			throw new IOException("there is already an instance of GroundEffectManager");
		}
		
		instance = this;
	}
}
