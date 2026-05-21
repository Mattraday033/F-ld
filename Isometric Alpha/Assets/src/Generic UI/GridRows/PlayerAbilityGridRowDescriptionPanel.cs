using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerAbilityGridRowDescriptionPanel : DescriptionPanelWithFormula
{
    public readonly static UnityEvent<Ability> AbilityNoLongerNew = new UnityEvent<Ability>();

    public GameObject newAbilityText;

    private void OnDestroy()
    {
        AbilityNoLongerNew.RemoveListener(markAbilityAsNoLongerNew);
    }

    public override void setObjectBeingDescribed(IDescribable describable)
    {
        AbilityNoLongerNew.RemoveListener(markAbilityAsNoLongerNew);
        base.setObjectBeingDescribed(describable);
    
        if(NewAbilityManager.abilityIsNew(OverallUIManager.getCurrentPartyMember(), describable as Ability))
        {
            newAbilityText.SetActive(true);

            AbilityNoLongerNew.AddListener(markAbilityAsNoLongerNew);
        }
    }

    private void markAbilityAsNoLongerNew(Ability ability)
    {
        if(getObjectBeingDescribed() as Ability == ability)
        {
            AbilityNoLongerNew.RemoveListener(markAbilityAsNoLongerNew);
            NewAbilityManager.removeAbility(OverallUIManager.getCurrentPartyMember(), ability);
            newAbilityText.SetActive(false);
        }
    }
}