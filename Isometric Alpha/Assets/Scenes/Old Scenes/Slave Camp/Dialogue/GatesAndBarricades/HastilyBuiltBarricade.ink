VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR playerName = ""

VAR barricadeIndex = 1 

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

This barricade is poorly put together, and could be destroyed with enough effort. Or they could be climbed over.

{
-strength >= 2:
    +Destroy the barricade. <Str {strength}/2>
        ->1b
}
{
-dexterity >= 2:
    +Climb over the barricade. <Dex {dexterity}/2>
        ->1c
}

    +Leave the barricade alone.
        ->Close

=== 1b ===

deactivate({barricadeIndex})

openGate()

You tear apart the barricade with your bare hands.
    
    ->Close


=== 1c ===

fadeToBlack()

adjustGridSquare()

fadeBackIn(60)

deactivate({barricadeIndex})

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