VAR strength = 0 
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR directorIndex = 1
VAR pageIndex = 2
VAR carterIndex = 3
VAR nandorIndex = 4

VAR mineLvl3CarterAndNandorInParty = false
VAR acceptingGuardPrisoners = false
VAR notAcceptingGuardPrisoners = false
VAR mineLvl3ConvincedRekaAndPazman = false
VAR convincedImre = false
VAR terrifiedImre = false
VAR foughtKendeInManseKitchen = false
VAR letTaborLive = false

VAR directorDefeated = false
VAR directorConvoFinished = false

VAR deathFlagCarter = false
VAR deathFlagNándor = false

VAR thePlanQuestName = "The Plan"
VAR questSucceeded = true
VAR killedDirectorQuestStepIndex = 18
VAR tookDirectorPrisonerQuestStepIndex = 19

VAR dealWiththePrisonersQuestName = "Deal With the Prisoners"

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

setToFalse(directorDefeatedConvo)

activate({pageIndex})
activate({carterIndex})
activate({nandorIndex})

changeCamTarget({directorIndex})

\*The Director wobbles, and then falls to his knees. His swordarm falls slack, and his weapon tumbles to the ground. Blood and sweat cake his face.*

In my years of service to the Confederation, I've been on both sides of defeat. I know what it looks like. \*A heavy sigh escapes his labored chest.* Get on with it. Kill me.

    +Spoken almost like a man who wants me to kill him.
        ->1b

=== 1b ===

changeCamTarget({pageIndex})

He does. If he does not die in the revolt, his superiors will surely hunt him down and do far worse to him.
    
{
-not deathFlagCarter and mineLvl3CarterAndNandorInParty:

changeCamTarget({carterIndex})

Page! It's good to see you still alive!

changeCamTarget({pageIndex})

Likewise, Carter. I had wondered if you had a part in the riots. Command may not understand, but you will hear no admonishments from me.

changeCamTarget({directorIndex})

Page? What is going on? Who is this slave and what are you talking about?

changeCamTarget({pageIndex})

Neither of us were ever yours, Director. I was inserted into your household by the Kingdom of Masons to monitor you. Carter was assigned to assist me when you founded this camp.  

\*Page turns back to address you.* If you kill him, you free him from a life hunted by the Lovashi. If you keep him alive, he could be a valuable asset to the Kingdom in our opposition to the Confederation.

-else:

changeCamTarget({directorIndex})

Page, do not speak. This is betw-

changeCamTarget({pageIndex})

No, my time taking your orders is at an end. Stay silent now.

You there. I am an agent of the Kingdom of Masons, supplanted into the Director's household years ago to monitor his and other Lovashi officer's movement's. We fight for the freeing of all slaves from bondage, and you can trust my intentions.

If you kill him, you free him from a life hunted by the Lovashi. If you keep him alive, he could be a valuable asset to the Kingdom in our opposition to the Confederation.
}



->1c

=== 1c ===

{
-not deathFlagCarter and mineLvl3CarterAndNandorInParty:

    +Carter, you know her?
    changeCamTarget({carterIndex})
    
  She's that other agent I mentioned, the one set up in the Manse. Everything she's saying is true; if she says the Kingdom can squeeze some use from this bastard then I say you should listen to her.

        ->1c
}

    +Keeping him alive would rob every slave here of their revenge.
        changeCamTarget({pageIndex})
    
        It would, but his death may spell the demise of many slaves in the future. Surely postponing it for a time is worth preventing that?
        ->1c
    +I've waited a long time to see him dead. I won't be thwarted at the last moment by you.
        ->3a
    +I need no excuse to prevent an execution. You can have him.
        setToTrue(keptDirectorAlive)
        finishQuest({thePlanQuestName}, {questSucceeded}, {tookDirectorPrisonerQuestStepIndex})
        changeCamTarget({pageIndex})
        ->2a
    +You may take him with my blessing, so long as you remember who allowed it.
    
        changeCamTarget({pageIndex})
        setToTrue(keptDirectorAlive)
        finishQuest({thePlanQuestName}, {questSucceeded}, {tookDirectorPrisonerQuestStepIndex})
        I certainly won't forget what you've done here today. And you've made a wise decision. 
       
        keepDialogue()     
        I'm not in any place to give orders to those you lead. Would you please place the sla-, sorry, former slaves you trust the most to guard him? I would like to prevent any of the others from taking their revenge before he can be moved.
        ->2a


=== 2a ===

You've made a wise decision. I'm not in any place to give orders to those you lead. Would you please place the sla-, sorry, former slaves you trust the most to guard him? I would like to prevent any of the others from taking their revenge before he can be moved.

    +Certainly.
        ->4a

=== 3a ===

changeCamTarget({pageIndex})

That would be your right, and it would be a lie to say I haven't wanted to put him down myself many times over the years. But I have warmed to his children whom I tutored. 

\*Page turns towards the Director and looks down at him on his knees.* Director, should I ever see them again, I will tell them their father died with at least some dignity.

changeCamTarget({directorIndex})

\*The Director does not return Page's gaze. Instead, he takes off his helmet and throws it to the ground, leaving his hair to hang, sweaty and undignified, against his head. He then looks up at you.* Get on with it.

    +\*Kill the Director.*
        kill({directorIndex})
        finishQuest({thePlanQuestName}, {questSucceeded}, {killedDirectorQuestStepIndex})
        ->3c

=== 3b ===
/*
{
-not deathFlagNándor:
changeCamTarget({nandorIndex})

\*Nándor looks down at the Director's headless corpse.* Months of labor, days of waiting in a cave deep below the ground, hours of bloody fighting... for this. It feels unreal.

{
-not deathFlagCarter:

changeCamTarget({carterIndex})

I know what you mean. But right now we need to tell the others that it's over.
} 

}*/

->3c

=== 3c ===

changeCamTarget({pageIndex})

And with that, you all are free. How does revenge feel?

    +It feels fantastic. The Director is dead and I am triumphant. How else could it feel?
        setToTrue(toldPageRevengeFeltFantastic)
        keepDialogue()
        Revel in it as you like, but I expect that feeling will be fleeting. I mourn what could have been gleaned had we kept him alive, but I have spent the last few years gathering information from him. That will have to suffice.
        
        ->3d
    +Now that it's over, it feels like I get to get on with my life.
        ->3d
    +Justice dictated that I go through with it. Nothing more.
        ->3d
    +It was a quick death, more than could have been said if I had left it to the others, or even, I suspect, the Confederation. It was a mercy.
    
    keepDialogue()
    
    \*Page raises an eyebrow at that.* I would not have expected mercy to be on the mind of a branded. A unique perspective.
        ->3d

=== 3d ===

I understand. I mourn what could have been gleaned had we kept him alive, but I have spent the last few years gathering information from him. That will have to suffice.

->4a

=== 4a ===

changeCamTarget({pageIndex})

No doubt you will want to take this opportunity to celebrate. However, my mission here meant I needed to suck up to the Director which has made me... unpopular with the others. I will stay out of their way so as not to cause a scene. Come find me by the camp's exit before you leave. We have some things we need to discuss.

->4aa

=== 4aa ===

{
-mineLvl3CarterAndNandorInParty:

changeCamTarget({nandorIndex})

\*Nándor shivers, then shakes his head.* Months of labor, days of waiting in the dark, hours of bloody fighting... for this. It feels unreal.


changeCamTarget({carterIndex})

I know what you mean. But right now we need to tell the others that it's over.

changeCamTarget({nandorIndex})

I will gather them in the southeastern part of the camp, near the mess hall. We'll open the Manse's stores and prepare a feast to celebrate our freedom. It will also give us a chance to address everyone.

    {
    -acceptingGuardPrisoners:
    
        We will need to decide the fate of any prisoners that may have been taken during the riots, as well as the guards that assisted our escape plans.
    
    -notAcceptingGuardPrisoners:
        Doubtless most of the prisoners heeded your orders to accept no surrender, but we still need to decide the fate of the guards that assisted our escape plans. The former workers will want to know what will befall them, if anything.
    }
    
    {
    -mineLvl3ConvincedRekaAndPazman:
    
        changeCamTarget({carterIndex})
    
        We should also send someone to collect the guards we took prisoner from Gáspár's team in the mines. 
    }
    
    +Very well. I'll meet you at the mess hall.
    
    activateQuestStep({dealWiththePrisonersQuestName}, 0)
    
    {
    - letTaborLive:
        activateQuestStep(An Uneasy Truce, 2)
    }
    
    setToTrue(directorDefeated)
    
    {
    - (convincedImre or terrifiedImre) and not foughtKendeInManseKitchen:
    
    addDeathFlag(Imre)
    finishedQuest(Assist the Nonbranded, false, 6)
    }
    
    setAreaToPassive(NECamp)
	setAreaToPassive(CenterCamp)
	setAreaToPassive(ManseCamp)
	setAreaToPassive(SECamp)
	setAreaToPassive(MineEntranceCamp)
	setAreaToPassive(MessHall)
        ->deactivateExtras
-else:
        ->deactivateExtras
}

=== 4b ===

{
- mineLvl3CarterAndNandorInParty:

changeCamTarget({nandorIndex})




    {
    -acceptingGuardPrisoners:
    }

}


->deactivateExtras

=== 4ba ===

->Close

=== deactivateExtras ===

setToTrue(directorConvoFinished)

fadeToBlack()

deactivate({directorIndex})
deactivate({pageIndex})
deactivate({carterIndex})
deactivate({nandorIndex})

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE