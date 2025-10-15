VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR knowRevolutionPassword = false
VAR knowsAboutTheMine = false
VAR toldToFindNandor = false
VAR toldCarterPassword = false
VAR toldCarterWrongPassword = false

VAR mineLvl3ClearedCratesToMiners = false
VAR mineLvl3MetGaspar = false
VAR mineLvl3KilledGuards = false
VAR mineLvl3MarcosAgreedToIgniteJelly = false
VAR mineLvl3ToldToFindMarcos = false

VAR takingCarterNandorWithYou = false
VAR saidYouMustBeJoking = false

VAR formationScreenTutorialKey = "Formation Tutorial"

//camera Target index's
VAR cratesIndex = 1
VAR carterCratesIndex = 2
VAR carterIndex = 3
VAR nandorIndex = 4
VAR marcosIndex = 5

VAR marcosDialogueIndex = 0

VAR backFromMarcosDialogue = false

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


{
-backFromMarcosDialogue:
    ->2c
-else:
    ->1a
}

=== 1a ===

changeCamTarget({carterCratesIndex})

You there! Step into the light, with your hands where I can see them. *A branded slave, wielding a large mining pick in both hands, glares at you from behind a makeshift barricade.*
    
    +\*Comply.* I am {playerName}.
        ->1b
    +\*Leave.*
        ->Close

=== 1b ===

I have no idea who you are. You have the brand but you're not one of the slaves that normally works on this level. Did Gáspár send you?
/*
{
-mineLvl3MetGaspar:
    +I have never met this Gáspár before. I'm a new arrival. <Lie>
        ->5a //5a is a place holder
-else:
    +I have never met this Gáspár before in my life. I'm a new arrival.
        ->5a //5a is a place holder
}*/

    {
    -toldToFindNandor:
    +No, Kastor did. I'm looking for one of the branded named Nándor. Do you know where he is?
        ->1ba

    -knowRevolutionPassword:
    +I'm down here looking for friends. Do you know which way the wind is blowing?
        ->4a
    }

    +I'm a new slave. I came down here looking for any survivors.
        ->3a

=== 1ba ===

Kastor? From the surface? Perhaps you know which way the wind is blowing, then.

    +East, of course.
        activateQuestStep(Finding Nándor, 1)
        \*Carter breaks out into laughter.* Forgive me, you're the first friendly face we've seen in days; I am simply overcome with relief. Nándor still breaths. Let me show you to him.

        ->1c

=== 1c ===
    
fadeToBlack()

setToTrue(mineLvl3ClearedCratesToMiners)

changeCamTarget({nandorIndex})
deactivate({cratesIndex})
activate({carterIndex})
moveToLocalPos(-44.5,14.15)
fadeBackIn(60)

{
    -toldCarterPassword:
        Carter tells me you are an ally. You have the gratitude of everyone here, but we're not safe yet.
    -else:
        Carter tells me you came from the surface. It's remarkable that you made it this far.
}


    +I have opened the gate to the second level. We can leave as soon as you are ready.
        ->1d
    
=== 1d ===

You have done well, but as much as I would love to exit the mines there is something we must do first.  
    
    +What do you mean?
        ->1e
    +More? You must be joking.
        ~saidYouMustBeJoking = true
        ->1e

=== 1e ===

{
-saidYouMustBeJoking:
    
I wish I was. But I'm deathly serious: these worm-things are growing in number. They are coming through a breach in the latest shaft we were digging.
-else:
To keep it brief, these worm-things are growing in number. They are coming through a breach in the latest shaft we were digging.
}

{
-knowsAboutTheMine && toldToFindNandor:
    +From a pocket. Kastor told me.
        ->1f
-else:
    +What does this have to do with us?

        If we don't seal the breach, we will be neck deep in these things before long. When the worms first appeared, the guards evacuated every slave they could from the upper levels, but the guards and slaves that were left behind on the bottom floor were trapped.
        
        Quickly realizing that we may be here for weeks, the guards fortified the stockroom on this level and decided to rid themselves of any extra mouths. The slaves trapped with them were forced to find their own shelter. Of the more than a dozen slaves that tried to make the journey, we are all that remains.
    {
    -toldCarterPassword:
        keepDialogue()
        
        Should the worms make it to the surface and attack the camp, they will be a formidable foe indeed. It will severely complicate things for our cause. It is best we remove that complication before revolting against the guards.
            ->1f
            
        -else:
        keepDialogue()
        
        Should the worms make it to the surface and attack the camp, I expect the guards there will have a similar attitude towards the slaves under their command. We will be used as fodder to keep the worms at bay until help can arrive.
            ->1f
    }
}


=== 1f ===

He was correct. If we don't seal that pocket, we will be neck deep in these things before long. It will be difficult to fight the guards and the worms simultaneously.
{
-toldCarterPassword:
    +Maybe the worms could be used to fight the guards.
        ->1j
}
    +Speaking of the guards, who is this one with you?
        ->1g
    +Why don't we simply fight our way back up to the second floor gate and seal it again?
        ->1fa
    +Do we even have a way of sealing the tunnel?
        ->1k

=== 1fa ===

Having fought the worms many times while we've been stuck down here, I'm all too familiar with their ability to spit acid, and what it can do to metal tools. I don't think the relatively thin metal of the gate will hold up long if they decide to chew through it.

keepDialogue()

The acid that hits the ground doesn't eat through the rock so easily, however. If we can find a way to seal the breach that they're coming through, we should be able to block the flow of worms coming through it. At least for longer than the gate can.
        ->1f

=== 1g ===

That is Guard Márcos. He helped save Carter and myself and escorted us here when the other guards wouldn't let us inside their barricade. Without him, I would be dead.

    {
    -toldToFindNandor:
        +I see. Guard Márcos, Kastor also spoke of your heroics.
            ->1h
    }

    {
    -toldCarterPassword:
        +Do you trust him enough to discuss our plans around him?
        ->1i
    }

    +I'm glad he proved useful, then.
        keepDialogue()
        That and more so.
            ->1f
    
    +Strange that a guard would go to such lengths for some slaves. Especially for the branded.
        ->1ha


=== 1h ===

changeCamTarget({marcosIndex})
\*Guard Márcos's torso and legs are covered in bite marks, and dried lines of blood trail from many punctures across his body.* I merely did what anyone with the power should.
changeCamTarget({nandorIndex})
    ->1i

=== 1ha ===

changeCamTarget({marcosIndex})

\*Guard Márcos's torso and legs are covered in bite marks, and dried lines of blood trail from many punctures across his body.* No one deserves to be eaten by those things. I can't think of any crime that would warrant that.


{
    -toldToFindNandor:
        ->1i
    -else:
    changeCamTarget({nandorIndex})
    
    keepDialogue()

    If his presence bothers you, do not let it. I have complete trust in his intentions.
        ->1f
}

=== 1i ===

changeCamTarget({nandorIndex})

keepDialogue()

He has bled for us many times, and we've discussed it at length while we've been trapped down here. He will fight with us again when the time comes.
    ->1f

=== 1j ===

changeCamTarget({marcosIndex})

I would not count on it. If pressed, the guards will plan to retreat towards the Manse, the guard barracks, and the camp entrance. It's the easiest part of the camp to fortify, and they will want to both prevent you from escaping and allow help to arrive unimpeded.

When they do this, you will be caught between the worms escaping through the mine entrance, and the guards' barricades. It will be a slaughter.

changeCamTarget({nandorIndex})

keepDialogue()

If we plan on getting out of this alive, the breach must be sealed.
    ->1f

=== 1k ===

We do, but it will be tricky. Some of the guards are trained in the use of blasting jelly; we use it to clear rubble sometimes. The guards that remain on this level are holed up within the chamber we use to store the jelly and other provisions. We will need to convince them to help us, or failing that, kill them and take the jelly for ourselves.
{
-mineLvl3KilledGuards:
+I have actually already taken care of the guards. 
    ->2a
-else:
+If we kill them, how will we use the jelly?
    ->1l
}
=== 1l ===

changeCamTarget({marcosIndex})

I can use the jelly if we truly need, but with my wounds I may make a mistake. It would be best if we had another guard detonate the breach instead.

    +And killing your fellow guards doesn't bother you?
        ->1la
    
=== 1la === 
        I have had much time to think, down here in the dark. Coming so close to death has made me wonder what kind of a man I will be when I go to my hearth. 
        
        I would prefer not to kill the men and women I have worked with for months. And leaving them alive is pragmatic: I cannot be sure I can handle the blasting jelly as effectively as they can.
        
        But if they should stand between us and the safety of the camp simply because the plan came from the mouth of a slave? Then the needs of the many demand we act. I will mourn their deaths in my own time. Does that satisfy your question?
        
        +For now. But I will be watching.
            As would I.
            ->1lb
        +No. I don't buy it. Get in the way of the plan and I will act. Believe me.
            I understand. I won't give you cause.
            ->1lb

=== 1lb ===


changeCamTarget({nandorIndex})
setToTrue(mineLvl3ToldAboutJelly)
Are you ready to set out?

    +Yes. Let's go.
    ~takingCarterNandorWithYou = true
        ->1m
    +No, let me scout ahead first.
        Alright, but hurry back. We need to move before the number of worms grows out of control.
        ->1m

=== 1m ===

{
-takingCarterNandorWithYou:
    fadeToBlack()
        
    deactivate({carterIndex})
    addToParty({carterIndex})
    deactivate({nandorIndex})
    addToParty({nandorIndex})
    //startUITutorial({formationScreenTutorialKey})
    setToTrue(mineLvl3CarterAndNandorInParty)
    changeCamTarget({marcosIndex})
    fadeBackIn(60)
    -else:
        changeCamTarget({marcosIndex})
}
    
{
-not mineLvl3MarcosAgreedToIgniteJelly:
    activateQuestStep(Sealing the Breach, 0)
    
    {
    -mineLvl3ToldToFindMarcos:
        activateQuestStep(Find Guard Márcos, 1)
    }
    
    
    While you are out there, remember: you can come back to me if you are hurt. I will stand watch while you rest.
        ->Close
-else:
    //deactivate({marcosIndex})
    activateQuestStep(Sealing the Breach, 4)
        ->Close
}

=== 2a === 

changeCamTarget({marcosIndex})

That is... unfortunate. There is no love lost between myself and Gáspár, but without him or Guard Virág there is no one else trained in the blasting jelly's use on this level. I will have to detonate the jelly in their stead.

changeCamTarget({carterIndex})

Márcos, you can't help but shiver from your wounds! There is no way you will be able to close the pocket without detonating the jelly prematurely.

changeCamTarget({marcosIndex})

I am not as wounded as I appear. I can perform the detonation as I have a dozen times before.

{
-wisdom >= 2:
    +Your bravado is fooling no one. I am a quick learner, teach me how to perform the detonation and I will carry it out. <Wis {wisdom}/2>
        ->2b
}

    +There is little time to argue. Márcos will do what needs to be done. Let us move before the worm situation gets worse.
        setToTrue(mineLvl3MarcosAgreedToIgniteJelly)
        ~mineLvl3MarcosAgreedToIgniteJelly = true
        ~takingCarterNandorWithYou = true
        ->1m
    
=== 2b ===

changeCamTarget({marcosIndex})

swapInkFiles({marcosDialogueIndex},startingFromMinersDialogue)

->Close

=== 2c ===

fadeToBlack()

deactivate({carterIndex})
deactivate({nandorIndex})
deactivate({marcosIndex})
addToParty({carterIndex})
addToParty({nandorIndex})
startUITutorial({formationScreenTutorialKey})
setToTrue(mineLvl3CarterAndNandorInParty)

fadeBackIn(60)
->Close

=== 3a ===

You're looking for survivors? You'll forgive me, but I find it a little odd you would brave three levels of the mine by yourself just to help us. Unless... do you know which way the wind is blowing?


{
    -knowRevolutionPassword:
    +How should I know which way the wind is blowing? I've been in this cave for hours.
        ->3b
    +North?
        ->3ba
    +East, friend.
        ~toldCarterPassword = true
        setToTrue(toldCarterPassword)
        keepDialogue()
        Incredible, you're the first friendly face we've seen in days, *and* you're one of us. You fought your way here from the surface?
        ->4a
    +South?
        ->3ba
    +West?
        ->3ba

    -else:
    +How should I know which way the wind is blowing? I've been in this cave for hours.
        ->3b
    +North?
        ->3ba
    +East?
        ->3d
    +South?
        ->3ba
    +West?
        ->3ba
}   


=== 3b ===

setToTrue(toldCarterWrongPassword)

Right, of course. It was a stupid question. 

    ->3c

=== 3ba ===

setToTrue(toldCarterWrongPassword)

Blast, nevermind it then. 

->3c

=== 3c ===
In any case, you being here means that the way back up is no longer blocked. But that also means the worms can escape to the upper levels and endanger the rest of the camp. I'm not sure I can trust you yet, but we're not in a position to turn away help down here: Nándor will have something to discuss with you. Are you willing to stay with us for a time?

    +Yes, I'll hear what you have to say. Which one of you is Nándor?

    \*The man lowers his weapon and extends a hand over the barricade.* I'll show you to him. I'm Carter, by the way.
        ->1c
        
    +I'm sorry but I must be going. I'll return when I am able.
        ->Close

=== 3d ===

Hmm, you don't sound very confident about that. Either way, I'll bring you to Nándor if you'd like. He'll know what to make of you.

    +Fine, I will speak to Nándor.

    \*The man lowers his weapon and extends a hand over the barricade.* Come with me. I'm Carter, by the way.
        ->1c
        
    +I'm sorry but I must be going. I'll return when I am able.
        ->Close

=== 4a ===

East! Incredible, you're the first friendly face we've seen in days, *and* you're one of us. You fought your way here from the surface?

{
-toldToFindNandor:
    +Yes, Kastor sent me. He told me to look for a slave named Nándor. Are you him?
        
        No, but he still lives! Quickly, come over the barrier and I'll introduce you.
        activateQuestStep(Finding Nándor, 1)
        setToTrue(toldCarterPassword)
        ~toldCarterPassword = true
        ->1c
-else:
    +I'm here looking for survivors to help us in our cause. Are there more with you?
        
        Yes, a few of us are still alive. Quickly, come over the barrier and I'll introduce you.
        setToTrue(toldCarterPassword)
        ~toldCarterPassword = true
        ->1c
}

=== 4b ===

->Close

=== 4c ===

->Close

=== Close ===

close()

->DONE