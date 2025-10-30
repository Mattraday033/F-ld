VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR selfIndex = 1
VAR playerName = ""

VAR didNotExecuteAndras = false
VAR gaveAndrasFiftyLashes = false
VAR executedAndras = false

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

changeCamTarget({selfIndex})

{
- didNotExecuteAndras:
Thank you for freeing András. We both are in your debt.
- executedAndras:
\*Janos sobs* Why? Why execute him? He was never a harm to you, or anyone!
- gaveAndrasFiftyLashes:
Were the lashes really necessary? He helped us! Doesn't that mean anything?
- else:
Please, don't hurt András. I will vouch for him; he's a good person.

    +Will you vouch for any of the prisoners?
        keepDialogue()
        The only one I know well is András. And of course I will vouch for him.
        ->1a
    +I must be going.
        ->Close

}

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