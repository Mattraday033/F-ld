using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHalfScript : QuestStepActivationScript
{
    private const string questName = "The Plan";

    private const int haveBothHalvesQuestStepIndex = 17; 
    private const int haveOneHalfQuestStepIndex = 16;

    public override void runScript()
    {
        bool hasFirstKeyHalf = Inventory.inventoryContainsItem(ItemList.getItem(ItemList.keyItemListIndex, ItemList.directorsOfficeKeyFrontIndex, 1).getKey());
        bool hasSecondKeyHalf = Inventory.inventoryContainsItem(ItemList.getItem(ItemList.keyItemListIndex, ItemList.directorsOfficeKeyBackIndex, 1).getKey());

        if (hasFirstKeyHalf && hasSecondKeyHalf)
        {
            QuestList.activateQuestStep(questName, haveBothHalvesQuestStepIndex);
        } else if (hasFirstKeyHalf || hasSecondKeyHalf)
        {
            QuestList.activateQuestStep(questName, haveOneHalfQuestStepIndex);
        } else
        {
            Debug.LogError("KeyHalfScript ran but no key halves detected");
        }

    }

}
