VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR mineLvl3ClearedCratesToGuards = false
VAR mineLvl3ToldAboutJelly = false
VAR toldPazmanAboutSealingBreach = false


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

changeCamTarget(1)

You're back. {playerName}, was it? Do you need rest?

    +Yes.
        restParty()
        ->Close
    +Not right now.
        ->Close

=== 1b ===

->Close

=== 1c ===

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

=== 3c ===

->Close

=== 3d ===

->Close

=== 3e ===

->Close

=== 3f ===

->Close

=== 3g ===

->Close

=== 3h ===

->Close

=== 3i ===

->Close

=== 3j ===

->Close

=== 3k ===

->Close

=== 3l ===

->Close

=== Close ===

close()

->DONE