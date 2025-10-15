VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR speakerIndex = 1
VAR barricadeParentIndex = 2

VAR barricadeGuardDeathFlagNameIndex = 2

VAR barricadeEnemyInfoIndex = 0

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

Damn, they got around us somehow! To arms! Beat them back!

    //deactivate({barricadeParentIndex})
    //kill({barricadeGuardDeathFlagNameIndex})
    enterCombat({barricadeEnemyInfoIndex})
        ->Close

    
=== Close ===

close()

->DONE