VAR strength = 0 
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR directorIndex = 1
VAR pageIndex = 2
VAR carterIndex = 3
VAR nandorIndex = 4
VAR thatchIndex = 5

VAR mineLvl3CarterAndNandorInParty = false
VAR acceptingGuardPrisoners = false
VAR notAcceptingGuardPrisoners = false
VAR mineLvl3ConvincedRekaAndPazman = false
VAR convincedImre = false
VAR terrifiedImre = false
VAR foughtKendeInManseKitchen = false
VAR letTaborLive = false

VAR angeredThatchInHisHut = false

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



->1a

=== 1a ===

setToFalse(directorDefeatedConvo)

activate({pageIndex})
activate({carterIndex})
activate({nandorIndex})
activate({thatchIndex})

changeCamTarget({directorIndex})

\*The Director wobbles, and then falls to his knees. His swordarm goes slack, and his weapon tumbles to the ground. Blood and sweat cake his face.*

In my years of service to the Confederation, I've been on both sides of defeat. I know what it looks like. \*A heavy sigh escapes his labored chest.* Get on with it. Kill me.

    +Spoken almost like a man who wants me to kill him.
        ->1b

=== 1b ===

changeCamTarget({pageIndex})

He does. If he does not die in the revolt, his superiors will surely hunt him down and do far worse to him.

changeCamTarget({directorIndex})

Page, do not speak. This is betw-

changeCamTarget({pageIndex})

No, my time taking your orders is at an end. Stay silent now.

You there. I am an agent of the Kingdom of Masons, supplanted into the Director's household years ago to monitor his movements. We fight for the freeing of all slaves from bondage, and you can trust my intentions.

If you kill him, you free him from a life hunted by the Lovashi. If you keep him alive, he could be a valuable asset to the Kingdom in our opposition to the Confederation.

changeCamTarget({thatchIndex})

Spare him? And give up the revenge we've all bled for? I won't see our prize stolen at the final moment on the whim of a stranger.

{
-not deathFlagCarter and mineLvl3CarterAndNandorInParty:

changeCamTarget({carterIndex})

She is no stranger. She is the other agent I spoke of before. Comrades, allow me to introduce Page, fellow agent of the Kingdom of Masons. It's good to see you again. 

changeCamTarget({pageIndex})

Likewise, Carter. I had wondered if you had a part in the riots. Command may not understand, but you will hear nothing ill from me.

changeCamTarget({thatchIndex})

Whomever she is, she has no right to ask us to spare him. The Director founded this camp. Every atrocity committed here was committed in his name. Our revolution can only end in his death. 

-else:

changeCamTarget({pageIndex})

A stranger I may be, but I have also suffered at his hands. Were it my choice to make I would see him dead and done. But I speak now to save the lives of thousands of my countrymen who will one day wage war against the Lovashi. Perhaps, even one day soon.

changeCamTarget({thatchIndex})

Whomever she is, she has no right to ask us to spare him. The Director founded this camp. Every atrocity committed here was committed in his name. Our revolution can only end in his death. 
}

->1c

=== 1c ===

{
-not deathFlagCarter and mineLvl3CarterAndNandorInParty:

    +Carter, you'd rather let him live? After everything he's put us through?
        changeCamTarget({carterIndex})
    
        It's not about what I want. It's Page's operation, and I am dutybound to assist her however she wants to play things. And... I trust her. If Page says the Kingdom can squeeze some use from this bastard then I say you should listen to her.
        ->1c
}
    +Thatch, you want to kill the Director? What if what she says is true?
        changeCamTarget({thatchIndex})
        I have seen too many people die to let him slip through our fingers. To do the things he's done, and to go free... If the world the Gods made has any justice in it, he will die in the same pain that Slate did. 
        ->1c
    +Nándor, you've been very quite. What is your opinion on all of this?
        changeCamTarget({nandorIndex})
        \*Nándor continues his silence for a small while longer.* If I had my way, I'd let him live. It is enough that he is impotent, and giving him over to the Masons will keep him that way. However, I fear that we may not be able to keep his fate a secret from the other branded. If they learn he still breathes then it would have been a kinder fate to kill him now rather than allow them to take their vengeance on him en masse.
        ->1c
    +I've waited a long time to see him dead. I won't be thwarted at the last moment by you.
        ->3a
    +I need no excuse to prevent an execution. You can have him.
        setToTrue(keptDirectorAlive)
        finishQuest(The Plan, true, I took the Director prisoner.)
        ->2a
    +You may take him with my blessing, so long as you remember who allowed it.

        setToTrue(keptDirectorAlive)
        finishQuest(The Plan, true, I took the Director prisoner.)
        ->2a

=== 2a ===

changeCamTarget({thatchIndex})

{
-angeredThatchInHisHut:

\*Thatch gives you a look that could boil rivers.* This is a betrayal of everyone who fought for you. But then again, you made it clear when we met you thought nothing of the branded and their sacrifices.

-else:

\*Thatch gives you a look that could boil rivers.* I feel as if I am seeing you in a new light. Your lies motivated us to free ourselves, but you never cared for our plight. What are the branded to you, slingstones to be spent against your enemies?

}
changeCamTarget({pageIndex})

You've made a wise decision. I have discovered some of the secret rooms this Manse's builders hid within it's walls. I will keep him there until you are ready to move him.

    +Certainly.
        ->4a

=== 3a ===

changeCamTarget({thatchIndex})
Yes! Finally! Let him end!

changeCamTarget({pageIndex})

That would be your right, and it would be a lie to say I haven't wanted to put him down myself many times over the years. But I have warmed to his children whom I tutored. 

\*Page turns towards the Director and looks down at him on his knees.* Director, should I ever see them again, I will tell them their father died with at least some dignity.

changeCamTarget({directorIndex})

\*The Director does not return Page's gaze. Instead, he takes off his helmet and throws it to the ground, leaving his hair to hang, sweaty and undignified, against his head. He then looks up at you.* Get on with it.

    +\*Kill the Director.*
        kill({directorIndex})
        finishQuest(The Plan, true, I killed the Director.)
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
        Revel in it as you like, but I expect that feeling will be fleeting. I mourn what more could have been gleaned had we kept him alive. 
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

    activateQuestStep(Deal With the Prisoners, To the Mess Hall.)

    {
    - letTaborLive:
        activateQuestStep(An Uneasy Truce, Back to Tabor.)
    }

    setToTrue(directorDefeated)

    {
    - (convincedImre or terrifiedImre) and not foughtKendeInManseKitchen:

    addDeathFlag(Imre)
    finishQuest(Assist the Nonbranded, false, Left to their fate.)
    }
    
    setAreaToPassive(NECamp)
	setAreaToPassive(CenterCamp)
	setAreaToPassive(ManseCamp)
	setAreaToPassive(SECamp)
	setAreaToPassive(MineEntranceCamp)
	setAreaToPassive(Mess Hall)
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
deactivate({thatchIndex})

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE