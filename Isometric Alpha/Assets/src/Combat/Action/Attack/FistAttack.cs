using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FistAttack : Attack, IJSONConvertable
{
    public FistAttack()
	{

	}
	
	public override int getSaveType()
	{
		return (int) CombatActionSaveType.Ability;
	}
	
	public override Item getSourceItem()
	{
		return (Item) ItemList.getMainHandFist(getActorStats() as AllyStats);
	}
	
	//convertToJson is for save files, you will never need to save an actions coords so actor/target coords are not saved
	public override string convertToJson()
	{				
		return new Ability(CombatActionSettings.build(getKey())).convertToJson();
	}
}
