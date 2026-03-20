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

        // if(displ)
    }

    public override string[] getTargetTags()
    {
        if(objectBeingDragged as ItemCombatAction != null)
        {
            return new string[] { LayerAndTagManager.abilityEditorTag, LayerAndTagManager.itemUseTargetTag };
        } else
        {
            return new string[] { LayerAndTagManager.abilityEditorTag };
        }
    }

    public override bool handleTargetObject(Collider2D collision, string tag)
    {
        switch(tag)
        {
            case LayerAndTagManager.abilityEditorTag:
                EditorAbilityMenuButton menuButton = collision.gameObject.GetComponent<EditorAbilityMenuButton>();

                return menuButton.setPlayerCombatActionAtIndex(getObjectBeingDragged() as CombatAction);
            case LayerAndTagManager.itemUseTargetTag:
                return handleUsableItemDrop(collision.gameObject);
            default:
                return false;
        }
    }
}
