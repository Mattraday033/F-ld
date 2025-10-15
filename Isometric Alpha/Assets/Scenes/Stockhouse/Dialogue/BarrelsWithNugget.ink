VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR startledUros = false
VAR gotIronNuggetFromBarrels = false
VAR convincedUros = false
VAR intimidatedUros = false
VAR threatenedToSnitchOnUros = false
VAR snitchedOnUros = false

VAR treasureItemListIndex = 5
VAR lostIronNuggetIndex = 3

VAR playerName = ""

//changeCamTarget(int targetIndex)
//keepDialogue()
//setToTrue(string flagName)
//setToFalse(string flagName)
//activate(int index of gameobject you're activating)
//deactivate(int index of gameobject you're deactivating)
//activateQuestStep(string questTitle, int questStepIndex)
//prepForItem() //used before giveItem/giveItems/takeAllOfItem to add obtained/removed text after next line
//giveItem(int listIndex, int itemIndex, int quantity)
//giveItems(int listIndex1, int itemIndex1, int quantity1 |
//          int listIndex2, int itemIndex2, int quantity2 |
//          ... etc)
//takeAllOfItem(string itemName)
//activateQuestStep(string fullTitleOfQuestFoundInQuestJsonFile,int questStepIndex)
//searchInventoryFor(string nameOfVarSetToTrueInsideInkFile,string itemNameToSearchFor)
//fadeToBlack()
//fadeBackIn(int numberOfFramesToWaitBeforeFadingBackIn)
//moveToLocalPos(float xCoord,float yCoord)

//prepForitem()

//Here you go!

//giveItem(5,0,1)

->1a

=== 1a ===

{
-not gotIronNuggetFromBarrels:

setToTrue(gotIronNuggetFromBarrels)

prepForItem()

Searching behind some barrels, you find a small scrap of iron.

{
-startledUros:
    {
    -convincedUros or intimidatedUros or threatenedToSnitchOnUros:
    activateQuestStep(Stockhouse Stash,7)
    -snitchedOnUros:
    activateQuestStep(Stockhouse Stash,8)
    -else:
    activateQuestStep(Stockhouse Stash,6)
    }
-else:
    activateQuestStep(Stockhouse Stash,0)
}

giveItem({treasureItemListIndex},{lostIronNuggetIndex},1)

    ->Close
    
-else:
Searching behind some barrels, you find nothing of interest.

->Close
}



=== Close ===

close()

->DONE