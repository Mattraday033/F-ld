using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectCurrentActor : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        Selector currentSelector = SelectorManager.currentSelector;

        Collider2D collider = Helpers.getCollision(currentSelector.getCollider());

        if(collider == null)
        {
            return;
        }

        if (collider.gameObject.tag.Equals(LayerAndTagManager.playerTag) || collider.gameObject.tag.Equals(LayerAndTagManager.npcTag))
        {
            AbilityMenuManager currentAbilityManager = collider.gameObject.GetComponent<AbilityMenuManager>();

            currentAbilityManager.enableAbilityButtonCanvas();
        }
    }
}
