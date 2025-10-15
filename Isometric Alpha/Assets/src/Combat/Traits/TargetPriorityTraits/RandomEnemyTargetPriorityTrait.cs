using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemyTargetPriorityTrait : TargetPriorityTrait
{

    public RandomEnemyTargetPriorityTrait() : 
    base("","","",  Color.black) //used when name/description/icon/background color are overridden, like with BufferTargetPriorityTrait
    {

    }

    public RandomEnemyTargetPriorityTrait(string traitName, string traitDescription, string traitIconName, Color traitIconBackgroundColor) :
    base(traitName, traitDescription, traitIconName, traitIconBackgroundColor)
    {

    }
    
    public override Stats getMandatoryTarget(ArrayList listOfTargets)
    {
        listOfTargets = CombatGrid.getAllAliveEnemyCombatants();

        if(listOfTargets.Count == 0)
        {
            return null;
        }

        int index = UnityEngine.Random.Range(0, listOfTargets.Count);
        
        return (Stats) listOfTargets[index];
    }

}
