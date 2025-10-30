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

Halt, slave! Riot control is in effect. Any slave that is found in their huts without a weapon after order is restored to the camp will be pardoned. Anyone who continues to resist will be put to death. What say you?

    +I'm getting through this barricade whether you man it or not. For freedom! <Attack>
        //deactivate({barricadeParentIndex})
        //addDeathFlag({barricadeGuardDeathFlagNameIndex})
        enterCombat({barricadeEnemyInfoIndex})
        ->Close
    +\*Leave without fighting.*
        ->Close

    
=== Close ===

close()

->DONE