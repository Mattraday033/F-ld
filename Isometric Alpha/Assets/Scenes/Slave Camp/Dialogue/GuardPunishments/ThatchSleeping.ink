VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR givenFullExplanation = false

VAR givenTutorialQuest = false
VAR toldKastorOfThatchsFate = false

VAR metThatch = false
VAR slateFound = false

VAR thatchIndex = 1
VAR removableRubbleIndex = 2

VAR playerName = ""

VAR formationScreenTutorialKey = "Formation Tutorial"

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

\*Thatch lays with his head slumped against the mess hall's wall. His eyes are closed, and soft snoring can be heard.*

    +\*Leave.*
        ->Close


=== Close ===

close()

->DONE  