VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR objectName = ""
VAR size = 0

VAR explanation = ""

VAR strDifficulty = 2
VAR dexDifficulty = 2

VAR gateKey = ""

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

{explanation} A sufficiently dexterous person could vault over it, or it could be torn down with enough strength and determination.


{
-strength >= strDifficulty:
    +Tear down the {objectName}. <Str {strength}/{strDifficulty}>
        ->1b
}
{
-dexterity >= dexDifficulty:
    +Vault over the {objectName}. <Dex {dexterity}/{dexDifficulty}>
        ->1c
}

{
-dexterity >= dexDifficulty or strength >= strDifficulty:
    +\*Leave*
        ->Close
-else:
    +\*Leave* <Str {strength}/{strDifficulty}> / <Dex {dexterity}/{dexDifficulty}>
        ->Close
}


=== 1b ===

fadeToBlack(true, false)

openGateWithKey({gateKey})

fadeBackIn(60)

\*The way is clear.*
    
    ->Close


=== 1c ===

fadeToBlack()

adjustGridSquare({size})

fadeBackIn(60)

        ->Close

=== Close ===

close()

->DONE