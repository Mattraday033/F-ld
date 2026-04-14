using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventStunTrait : Trait
{
    
    public PreventStunTrait(string traitName, 
                            string traitDescription = "", 
                            string iconName = "",
                            int roundsLeft = 1,
                            bool permanent = true) : 
    base(traitName, 
                 TraitType.Interaction, 
                 traitDescription, 
                 iconName,
                 roundsLeft: roundsLeft,
                 permanent: permanent)
    {
        
    }

    public override bool immuneToStun()
    {
        return true;
    }
    
}
