VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR size = 0
VAR plural = false
VAR objectName = ""

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
-plural:
These {objectName} look climbable. A sufficiently dexterous person could clamber over them.
-else:
This {objectName} looks climbable. A sufficiently dexterous person could clamber over it.
}


{
-dexterity >= 2:
    *Climb over the {objectName}. <Dex {dexterity}/2>
        ->1b
    *Leave the {objectName} alone.
        ->Close
-else:
    *Leave the {objectName} alone. <Dex {dexterity}/2>
        ->Close
}


=== 1b ===

fadeToBlack()

adjustGridSquare({size})

fadeBackIn(60)

->Close

=== 1c ===

->Close

=== 1d ===

->Close

=== Close ===

close()

->DONE