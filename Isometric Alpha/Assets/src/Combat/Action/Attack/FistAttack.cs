using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FistAttack : Attack, IJSONConvertable
{
    //always the actor's current fist, which changes as their stats do; sets still go to Attack
    public override Item sourceItem => ItemList.getMainHandFist(actorStats as AllyStats);

    public FistAttack(Stats statSource, Catalog<IQueuingUnit> queuingUnits = null, List<TargetingLogic> targetingLogics = null):
    base(statSource, ItemList.getMainHandFist(statSource as AllyStats), queuingUnits, targetingLogics)
	{
        
        
	}
	
	//convertToJson is for save files, you will never need to save an actions coords so actor/target coords are not saved
	public override string convertToJson()
	{				
		return new Ability(CombatActionSettings.build(key)).convertToJson();
	}
}
