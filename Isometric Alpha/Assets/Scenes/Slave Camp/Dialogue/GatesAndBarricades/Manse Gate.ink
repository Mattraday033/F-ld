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

These doors lead into the Manse. They are closed and barred.

{
-strength >= 4:
    +\*Break down the doors.*
        ->1b
-kastorStartedRevolt:
    +Alright everyone! Break down these doors!
        ->1c
}
    +Leave the doors alone.
        ->Close

=== 1b ===

setToTrue(manseDoorsOpenedRevolt)

deactivate(1)

openGate()

You break down the doors.

    ->Close

=== 1c ===

setToTrue(manseDoorsOpenedRevolt)

deactivate(1)

openGate()

The slaves surge forward and clear the door with pick and mattock.

    ->Close

=== Close ===

close()

->DONE