VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
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

These doors lead out into the camp.

    +Open the doors.
        ->1b
    +Leave the doors alone.
        ->Close

=== 1b ===

setToTrue(manseDoorsOpenedRevolt)

deactivate(1)

openGate()

You open the doors.

    ->Close

=== Close ===

close()

->DONE