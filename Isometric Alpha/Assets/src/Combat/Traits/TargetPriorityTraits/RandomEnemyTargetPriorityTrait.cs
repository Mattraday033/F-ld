using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemyTargetPriorityTrait : TargetPriorityTrait
{

    public RandomEnemyTargetPriorityTrait() : 
    base("","","") //used when name/description/icon/background color are overridden, like with BufferTargetPriorityTrait
    {

    }
    
    public override Stats getMandatoryTarget(List<Stats> listOfTargets)
    {
        listOfTargets = CombatGrid.getAllAliveEnemyCombatants();

        if(listOfTargets.Count == 0)
        {
            return null;
        }

        int index = Random.Range(0, listOfTargets.Count);
        
        return listOfTargets[index];
    }

}

public class RandomEnemyBesidesSelfTargetPriorityTrait : TargetPriorityTrait
{

    public RandomEnemyBesidesSelfTargetPriorityTrait() : 
    base("","","") //used when name/description/icon/background color are overridden, like with BufferTargetPriorityTrait
    {

    }
    
    public override Stats getMandatoryTarget(List<Stats> listOfTargets)
    {
        listOfTargets = CombatGrid.getAllAliveEnemyCombatants();

        listOfTargets.Remove(traitHolder);

        if(listOfTargets.Count == 0)
        {
            return null;
        }

        int index = Random.Range(0, listOfTargets.Count);
        
        return listOfTargets[index];
    }

}
