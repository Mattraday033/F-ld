VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR hasOfficeKeyFrontHalf = false
VAR hasOfficeKeyBackHalf = false

VAR playerName = ""

//changeCamTarget(int targetIndex)
//keepDialogue()
//setToTrue(string flagName)
//setToFalse(string flagName)
//activate(int index of gameobject you're activating)
//deactivate(int index of gameobject you're deactivating)
//activateQuestStep(string questTitle, int questStepIndex)
//giveItem(int listIndex, int itemIndex, int quantity)
//giveItems(int listIndex1, int itemIndex1, int quantity1 |
//          int listIndex2, int itemIndex2, int quantity2 |
//          ... etc)
//takeAllOfItem(string itemName)

->1a

=== 1a ===

searchInventoryFor(hasOfficeKeyFrontHalf,Key,2)
searchInventoryFor(hasOfficeKeyBackHalf,Key,3)

The wall looks formidable, but you can feel a slight draft. 

    +Search around for anything suspicious. <Wis {wisdom}/2>
        ->1b
    +Leave the wall alone.
        ->Close

=== 1b ===

You search around for anything that catches your eye, and notice that between two of the bricks there is a section of mortar missing in the shape of a keyhole.

{
-hasOfficeKeyFrontHalf and hasOfficeKeyBackHalf:
    +Combine both key halfs and open the door.
        ->1c
}
    +Don't touch anything.
        ->Close

=== 1c ===

deactivate(1)
deactivate(2)
activate(3)
setToTrue(manseOfficeSecretDoorsOpened)
setToTrue(manseHiddenStairsFound)

You insert the key into the lock and turn it. The wall is pulled to the side by a hidden mechanism.
    ->Close
    
=== 1d ===

->Close

=== 1e ===

->Close

=== 1f ===

->Close

=== 1g ===

->Close

=== 1h ===

->Close

=== 1i ===

->Close

=== 1j ===

->Close

=== 1k ===

->Close

=== 1l ===

->Close

=== 1m ===

->Close

=== 1n ===

->Close

=== 2a === 

->Close
    
=== 2b ===

->Close

=== 2c ===

->Close

=== 2d ===

->Close

=== 2e ===

->Close

=== 2f ===

->Close

=== 2h ===

->Close

=== 2i ===

->Close

=== 2j ===

->Close

=== 2k ===

->Close

=== 2l ===

->Close

=== 2m ===

->Close

=== 2n ===

->Close

=== 3a ===

->Close

=== 3b ===

->Close

=== Close ===

close()

->DONE