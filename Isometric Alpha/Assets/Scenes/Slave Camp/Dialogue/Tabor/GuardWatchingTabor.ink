VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerName = ""
VAR heardTaborsLesson = false

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

->1a

=== 1a ===

{
-heardTaborsLesson:
You'd better get back where you're supposed to be, 'fore I reach for my whip.
-else:
Go bother someone else, I'm trying to watch ol' Tabor at work.
}
    ->Close

=== Close ===

close()

->DONE