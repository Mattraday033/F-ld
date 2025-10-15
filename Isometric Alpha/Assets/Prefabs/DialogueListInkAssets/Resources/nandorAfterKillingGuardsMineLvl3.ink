VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR nandorIndex = 1
VAR carterIndex = 2
VAR marcosIndex = 3
VAR pazmanIndex = 4
VAR rekaIndex = 5

VAR deathFlagGuardMárcos = false
VAR deathFlagGuardPázmán = false

VAR toldToFindNandor = false

VAR goesWithBroglinsPlan = false
VAR mineLvl3GuardsInParty = false
VAR mineLvl3MarcosAgreedToIgniteJelly = false
VAR mineLvl3MarcosTaughtHowToIgniteJelly = false
VAR mineLvl3CarterAndNandorInParty = false
VAR mineLvl3RefusedToFightGaspar = false
VAR mineLvl3DealtWithGaspar = false
VAR mineLvl3GuardsBackToSurface = false
VAR mineLvl3SlavesBackToSurface = false
VAR mineLvl3ToldPazmanToEatShit = false
VAR mineLvl3ThreatenedGaspar = false

VAR mineLvl3ConvincedRekaAndPazman = false
//VAR mineLvl3ConvincedOnlyReka = false
VAR mineLvl3PromisedToProtectRekaAndPazman = false
VAR mineLvl3ThreatenedRekaAndPazmanAsPrisoners = false

VAR smallCupPlacedOnBarrel = false
VAR largeCupFilledWithWater = false
VAR largeCupPlacedOnBarrel = false
VAR agreedToFightGaspar = false

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

->4a


=== 4a ===

activate({nandorIndex})
activate({carterIndex})

{
-deathFlagGuardMárcos and mineLvl3MarcosAgreedToIgniteJelly:

changeCamTarget({nandorIndex})

Márcos...

    +He gave his life so that we might be free. Remember him that way.
        I will. Have no doubt about that.
        finishQuest(Sealing the Breach, true, 7)
        activateQuestStep(Finding Nándor, 2)
        ->4b
    +I will not weep for a slaver. Even one who turned at the eleventh hour.
        finishQuest(Sealing the Breach, true, 7)
        activateQuestStep(Finding Nándor, 2)
        I can understand why you say that, but to me he seemed different. Alas, it doesn't matter now.
        ->4b

-mineLvl3ConvincedRekaAndPazman:

activate({marcosIndex})
activate({pazmanIndex})
activate({rekaIndex})

changeCamTarget({rekaIndex})

Gáspár, you idiot. A stubborn ox to the last. 

finishQuest(Sealing the Breach, true, 8)
activateQuestStep(Finding Nándor, 2)

changeCamTarget({pazmanIndex})

If he still had breath he would say he was being loyal, not stubborn.

changeCamTarget({rekaIndex})

    ->4aa

-else:

    activate({marcosIndex})

    changeCamTarget({nandorIndex})

    The guards are dead, and the breach is sealed. I have not felt like this in a long time... Relieved. Hopeful. All thanks to your efforts.
    
    {
    -not deathFlagGuardMárcos and mineLvl3MarcosAgreedToIgniteJelly:
        finishQuest(Sealing the Breach, true, 6)
    -else:
        finishQuest(Sealing the Breach, true, 9)
    }
    
    activateQuestStep(Finding Nándor, 2)
    
    changeCamTarget({carterIndex})
    
    keepDialogue()
    
    If Nándor had asked me only a few hours ago what our prospects of seeing the surface again were, I would have told him we would die down here. But thanks to you, we can finally leave this place.
    
    ->4b
}

=== 4aa ===

Fat lot of good it did him in the end.

    +I for one won't miss him. \*Spit.*
    
        keepDialogue()
        
        Neither will I, I suppose. But it still seems like such a waste.
        
        ->4aa
        
    +Enough of this. You are our prisoners now.
        ->4ab

=== 4ab ===

    \*Guard Réka looks like she wants to say something, but thinks better of it and nods instead.*
        
        +I am not so new to this camp that I am not familiar with the cruelty of the guards. Step out of line even once and I will return your 'hospitality' in kind.
            ->4ac
        
        +If you do everything we say and do not resist, you will be treated much better than the guards have treated the branded. You have my word on that.
            ~mineLvl3PromisedToProtectRekaAndPazman = true
            setToTrue(mineLvl3PromisedToProtectRekaAndPazman)
            ->4aca
            
        +Give me any excuse to harm you and I'll take it. Test me at your peril.
            ->4ac

=== 4ac ===

changeCamTarget({pazmanIndex})

\*Pázmán gulps and nods.*

changeCamTarget({rekaIndex})

\*Réka looks to her weapon on the ground with regret, but then returns your gaze and nods.*

    +Good. Nándor, is there a place we can stow these two before we meet with Kastor? We don't want them being discovered before we make our move.
        changeCamTarget({nandorIndex})
        ->4ad

=== 4aca ===

\*Réka nods solemnly.*

    +Nándor, is there a place we can stow these two on our way to meet with Kastor? We don't want them being discovered before we make our move.
        changeCamTarget({nandorIndex})
        ->4ad

=== 4ad ===

There's a room on the first floor the guards would keep slaves in when they didn't have time to supervise them. We can lock them in there and come back for them when we've won and found the key. 

    +What if we don't succeed? Won't they be trapped in there?
        ->4ada
    +It will have to do. Let's get moving then.
        keepDialogue()
        
        Wait a moment. I just wanted to say that, with the breach sealed and the worms no longer a threat, I have not felt like this in a long time... Relieved. Hopeful. All thanks to your efforts.
        ->4b

=== 4ada ===

keepDialogue()

I don't like to plan for that event, but if we fail then the guards should free them when they retake the mine. They won't have any reason to keep the lockdown going with the worms gone and us... no longer in the way. And we can leave them with some of the supplies from the stockroom on this level just in case.

    ->4ad

=== 4b ===

The breach is sealed and the worms are no longer a threat. I have not felt like this in a long time... Relieved. Hopeful. All thanks to your efforts.

    +Don't thank me yet. We still need to rally the other slaves and fight clear of the guards.
        ->4c
    +I have that effect on people. Now, if you're ready, I say we head for the surface.
        ->4c

=== 4c ===

changeCamTarget({nandorIndex})
setToTrue(mineLvl3SlavesBackToSurface)
    
Of course. We will follow your lead.
    ->Close
    

=== Close ===

deactivate({nandorIndex})
deactivate({carterIndex})
deactivate({marcosIndex})
deactivate({pazmanIndex})
deactivate({rekaIndex})

close()

->DONE