using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHalfScript : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {
        bool hasFirstKeyHalf = Inventory.inventoryContainsItem(ItemList.getItem(ItemList.keyItemListIndex, ItemList.directorsOfficeKeyFrontIndex).getKey());
        bool hasSecondKeyHalf = Inventory.inventoryContainsItem(ItemList.getItem(ItemList.keyItemListIndex, ItemList.directorsOfficeKeyBackIndex).getKey());

        if (hasFirstKeyHalf && hasSecondKeyHalf)
        {
            QuestList.activateQuestStep(QuestNameList.thePlanQuestTitle, QuestNameList.thePlanStepTitleSeventeen);
        } else if (hasFirstKeyHalf || hasSecondKeyHalf)
        {
            QuestList.activateQuestStep(QuestNameList.thePlanQuestTitle, QuestNameList.thePlanStepTitleSixteen);
        } else
        {
            Debug.LogError("KeyHalfScript ran but no key halves detected");
        }
    }

}
