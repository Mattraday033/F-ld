using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropCombatAction : DragAndDropUIObject
{
    public AbilityMenuButton displayButton;

    public override void setObjectBeingDragged(IDescribable objectBeingDragged)
    {
        this.objectBeingDragged = objectBeingDragged;

        displayButton.disableButtonComponent();
        displayButton.loadCombatAction(objectBeingDragged as CombatAction);
        displayButton.updateAppearance();
    }

    public override string getTargetTag()
    {
        return LayerAndTagManager.abilityEditorTag;
    }

    public override void handleTargetObject(Collider2D collision)
    {
        EditorAbilityMenuButton menuButton = collision.gameObject.GetComponent<EditorAbilityMenuButton>();

        menuButton.setPlayerCombatActionAtIndex(getObjectBeingDragged() as CombatAction);
    }
}
