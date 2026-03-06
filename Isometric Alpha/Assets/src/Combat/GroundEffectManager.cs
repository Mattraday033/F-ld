using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GroundEffect
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
		
		indicator.transform.position = CombatGrid.getPositionAt(position);
		
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
	public Dictionary<GridCoords, GroundEffect> allGroundEffects = new Dictionary<GridCoords, GroundEffect>();

	public static void createNewGroundEffect(GroundEffect template, GridCoords coords)
	{
		GroundEffect newGroundEffect = template.clone();
		
		newGroundEffect.position = coords.clone();
		
		removeGroundEffect(coords);
		
		newGroundEffect.instantiate();
		
		getInstance().allGroundEffects.Add(coords, newGroundEffect);
	}
	
	public static void removeGroundEffect(GridCoords positionToRemoveAt)
	{
        if(getAllGroundEffects().ContainsKey(positionToRemoveAt))
        {
            getAllGroundEffects()[positionToRemoveAt].destroy();

            getAllGroundEffects().Remove(positionToRemoveAt);
        }
	}
	
	public static Dictionary<GridCoords, GroundEffect> getAllGroundEffects()
	{
		return getInstance().allGroundEffects;
	}
	
	public static void applyAllGroundEffectDamage()
	{
		foreach(GroundEffect groundEffect in getInstance().allGroundEffects.Values)
		{
			Stats target = CombatGrid.getCombatantAtCoords(groundEffect.position);
			
			if(target == null || target is null)
			{
				continue;
			} else
			{
				int damageDealt = DamageCalculator.calculateFormula(groundEffect.damageFormula, DamageCalculator.noStatsSource);
				
				DamageNumberPopup.create(groundEffect.position, damageDealt, CombatGrid.getPositionAt(groundEffect.position), DamageNumberPopup.getDirectionByTargetCoords(groundEffect.position),
                                        getInstance().damageNumberCanvas, isNotACrit, doesNotHealTarget);
				
				target.modifyCurrentHealth(DamageCalculator.calculateFormula(groundEffect.damageFormula, DamageCalculator.noStatsSource));
                target.playAnimationOnDamage();
			}
			
			groundEffect.tickDown();
		}
	}
	
	public static void removeAllFinishedGroundEffects()
	{
        List<GroundEffect> finishedGroundEffects = new List<GroundEffect>();
        
        foreach(GroundEffect groundEffect in getAllGroundEffects().Values)
        {
            if(groundEffect.turnsRemaining <= 0)
            {
                finishedGroundEffects.Add(groundEffect);
            }
        }

        foreach(GroundEffect groundEffect in finishedGroundEffects)
        {
            if(groundEffect.turnsRemaining <= 0)
            {
                removeGroundEffect(groundEffect.position);
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
