VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR metClay = false
VAR spokeToSeb = false
VAR clayExplainedCrime = false
VAR clayExplainedReward = false
VAR clayExplainedJob = false
VAR acceptedClaysFirstJob = false
VAR acceptedClaysSecondJob = false
VAR hasThatchsNecklace = false
VAR threatenedThatch = false
VAR knowsAboutKendesShop = false
VAR gotKnifeFromClay = false
VAR toldClaySpokeToSeb = false
VAR gaveNoteToSeb = false

VAR deathFlagThatch = false

VAR clayRemorseKey = "A Weary Heart"
VAR clayFrontalAssaultKey = "The Frontal Assault"
VAR clayStealthKey = "Stay Unseen"
VAR clayKeptNecklaceKey = "Keepsake Kept"
VAR clayPacifistKey = "Slipping Upward"
VAR clayHeroKey = "A Hero, Actually"


VAR weaponListIndex = 1
VAR bronzeDirkIndex = 8
VAR questItemListIndex = 3
VAR claysNoteIndex = 6
VAR thatchsNecklaceKey = "Thatch's Silver Necklace"

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

->1a

=== 1a ===

Don't worry, they won't get past us. And thanks. For everything.
        ->Close

=== Close ===

close()

->DONE