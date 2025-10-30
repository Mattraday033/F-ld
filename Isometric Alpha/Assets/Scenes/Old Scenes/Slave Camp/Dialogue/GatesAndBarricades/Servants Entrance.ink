VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR askedImreToLeadTheWay = false
VAR kastorStartedRevolt = false

VAR playerName = ""

//changeCamTarget(int targetIndex)
//keepDialogue()
//setToTrue(string flagName)
//setToFalse(string flagName)
//activate(int index of gameobject you're activating)
//deactivate(int index of gameobject you're deactivating)
//activateQuestStep(string questTitle, int questStepIndex)
//prepForItem()
//giveItem(int listIndex, int itemIndex, int quantity)
//giveItems(int listIndex1, int itemIndex1, int quantity1 |
//          int listIndex2, int itemIndex2, int quantity2 |
//          ... etc)
//takeAllOfItem(string itemName)

    ->1a

=== 1a ===

{
-askedImreToLeadTheWay:

These doors provide access to the Manse's kitchens. They are unbarred, and stand slightly ajar.

-else:

These doors provide access to the Manse's kitchens. They are closed and barred.

}


{
-askedImreToLeadTheWay:
    +Proceed through the door.
        deactivate(1)

        openGate()
        ->Close
}
    +Leave the doors alone.
        ->Close

=== Close ===

close()

->DONE