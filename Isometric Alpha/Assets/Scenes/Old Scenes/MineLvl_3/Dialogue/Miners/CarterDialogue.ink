VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR toldToFindNandor = false
VAR mineLvl3ClearedCratesToMiners = false
VAR mineLvl3MetGaspar = false
VAR takingCarterNandorWithYou = false

VAR carterIndex = 1
VAR nandorIndex = 2

VAR formationScreenTutorialKey = "Formation Tutorial"

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

changeCamTarget({carterIndex})

Do you need my assistance?
    
    +Yes, lets go.
        fadeToBlack()
        setToTrue(mineLvl3CarterAndNandorInParty)
        deactivate({nandorIndex})
        deactivate({carterIndex})
        addToParty({nandorIndex})
        addToParty({carterIndex})
        startUITutorial({formationScreenTutorialKey})
        fadeBackIn(60)
        ->Close
    +I need a little more time on my own.
        Of course.
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