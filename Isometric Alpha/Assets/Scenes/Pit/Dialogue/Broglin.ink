VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR hasPitCellKey = false
VAR toldAboutCellKey = false

VAR broglinIndex = 1
VAR gateIndex = 2

VAR pitCellKeyIndex = 1
VAR pitGateKey = "Pit-Gate"

VAR gateFlagPitGate = false

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

searchInventoryFor(hasPitCellKey,Key,{pitCellKeyIndex})

{
-gateFlagPitGate:
    ->2b
-toldAboutCellKey:
    ->2a
-else:
    ->1a
}


=== 1a ===

\*Broglin stands up from where he was leaning against the wall of the cavern. Even in the dim light you can see that the entire right side of his face is bruised and swollen. His injuries continue down beneath his rags; it is as if he was beaten by someone with only one arm. He smiles through a grin that has lost many teeth since the last time you saw him.* Oh thank the Mother, you're here. After the guards left I thought I'd starve in here!

{
-hasPitCellKey:
    +We have little time, Broglin. I have the key to your cell. Can you move on your own?
        ->3a
-else:
    +What happened to your face?
        ->1b
    +I'm glad to see you are still breathing, Broglin.
        ->1c
    +We have little time. Where is the key to your cell?
        ->1d
}


=== 1b ===

keepDialogue()

After you accused me of trying to recruit you to the escape plan, they worked me over trying to get me to squeal on the people I was working with. I took their beatings for a while, but eventually I started to name some of the guards that I held a grudge against. I spun a good yarn, so they gave me a little time off from the beatings to investigate what I was telling them. That is until they started talking about a revolt upstairs and left me down here.

->1a

=== 1c ===

keepDialogue()

And I you, friend. I would say you took your sweet time but you were very quick, considering what you had to get done. 

->1a
    
=== 1d ===

setToTrue(toldAboutCellKey)

activateQuestStep(Rescue Broglin, 5)

The guards threw the key into that hole behind you, where they were chucking their garbage. When everything's quiet you can hear something moving around down there, so be careful.

    +Got it, I'll be right back.
        ->Close
    +The guards must have thought they weren't coming back here. Or they really just don't like you.
        ->1e
        
=== 1e ===

A little of both. I heard them talking: they're scared of you. Good luck.

->Close

=== 2a === 

Any luck finding the key?

{
-hasPitCellKey:
    +I have the key to your cell. Can you move on your own?
        ->3a
-else:
    +Where is the key to your cell, again?
        ->1d

    +Not yet. I'll be right back.
        ->Close
}

=== 2b ===

Thank you for freeing me. Forgive me if I don't jump for joy, however, my leg won't permit it.

    +Can you move on your own?
        ->3a

->Close

=== 3a ===

finishQuest(Rescue Broglin, true, 6)

prepForItem()

Aye, but I won't be much use in a fight. I would have liked to get a few licks in on the Director before you put him out of our misery, but this eye is too swollen to see and my right arm is probably broken. I'll make my way back to the others on my own so I don't get in your way. And thanks again!

addXP(250)

fadeToBlack()

setToTrue(freedBroglin)

openGate({pitGateKey})

deactivate({broglinIndex})
deactivate({gateIndex})

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE