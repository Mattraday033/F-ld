VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerIndex = 0
VAR nandorIndex = 1
VAR carterIndex = 2
VAR marcosIndex = 3
VAR pazmanIndex = 4
VAR rekaIndex = 5
VAR viragIndex = 6
VAR gasparIndex = 7
VAR weftIndex = 8
VAR thatchIndex = 9
VAR thatchWithNandorIndex = 10

VAR deathFlagGuardMárcos = false
VAR deathFlagGuardPázmán = false

VAR toldToFindNandor = false

VAR goesWithBrushsPlan = false
VAR knowRevolutionPassword = false
VAR toldCarterPassword = false
VAR mineLvl3GuardsInParty = false
VAR mineLvl3MarcosAgreedToIgniteJelly = false
VAR mineLvl3MarcosTaughtHowToIgniteJelly = false
VAR mineLvl3CarterAndNandorInParty = false
VAR mineLvl3RefusedToFightGaspar = false
VAR mineLvl3DealtWithGaspar = false
VAR mineLvl3AgreedToFightGaspar = false
VAR mineLvl3GuardsBackToSurface = false
VAR mineLvl3SlavesBackToSurface = false
VAR mineLvl3ToldPazmanToEatShit = false
VAR mineLvl3ThreatenedGaspar = false

VAR partyFlagThatch = false

VAR deathFlagGuardVazul = false

VAR mineLvl3ConvincedRekaAndPazman = false
//VAR mineLvl3ConvincedOnlyReka = false
VAR mineLvl3PromisedToProtectRekaAndPazman = false
VAR mineLvl3ThreatenedRekaAndPazmanAsPrisoners = false

VAR kastorExecutedWeft = false
VAR weftAddedToParty = false

VAR smallCupPlacedOnBarrel = false
VAR largeCupFilledWithWater = false
VAR largeCupPlacedOnBarrel = false

VAR playerName = ""

{
-mineLvl3RefusedToFightGaspar:
    ->5a
-else:
    ->4a
}

=== 4a ===

activate({nandorIndex})
activate({carterIndex})

{
-partyFlagThatch:
activate({thatchWithNandorIndex})
}

{
-weftAddedToParty:
activate({weftIndex})
setNPCFacing({weftIndex},SE)
}

{
-deathFlagGuardMárcos and mineLvl3MarcosAgreedToIgniteJelly:

setFacing(SE)
setNPCFacing({nandorIndex},NW)
setNPCFacing({carterIndex},SW)

changeCamTarget({nandorIndex})

Márcos...

    +He gave his life so that we might be free. Remember him that way.
        I will. Have no doubt about that.
        ->4ba
    +I will not weep for a slaver. Even one who turned at the eleventh hour.
        I can understand why you say that, but to me he seemed different. Alas, bleak as it may be, it doesn't matter now.
        ->4ba

-mineLvl3ConvincedRekaAndPazman:

activate({marcosIndex})
activate({pazmanIndex})
activate({rekaIndex})

changeCamTarget({rekaIndex})

Gáspár, you idiot. A stubborn ox to the last.

changeCamTarget({pazmanIndex})

If he still had breath he would say he was being loyal, not stubborn.

changeCamTarget({rekaIndex})

    ->4aa

-else:

    activate({marcosIndex})

    setFacing(SE)
    setNPCFacing({nandorIndex},NW)
    setNPCFacing({marcosIndex},NE)
    setNPCFacing({carterIndex},SW)

    changeCamTarget({nandorIndex})

    The guards are dead, and the breach is sealed. I have not felt like this in a long time... relieved. Hopeful. All thanks to your efforts.

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
            
        +Give me any excuse to harm you and I'll take it. Do not risk testing me.
            ->4ac

=== 4ac ===

changeCamTarget({pazmanIndex})

\*Pázmán gulps and nods.*

changeCamTarget({rekaIndex})

\*Réka looks to her weapon on the ground with regret, but then returns your gaze and nods.*

{
-not toldCarterPassword:
    ->4ada
}

    +Good. Nándor, is there a place we can stow these two before we meet with Kastor? We don't want them being discovered before we make our move.
        setFacing(SE)
        changeCamTarget({nandorIndex})
        ->4ad

=== 4aca ===

\*Réka nods solemnly.*

{
-not toldCarterPassword:
    ->4ada
}

    +Nándor, is there a place we can stow these two on our way to meet with Kastor? We don't want them being discovered before we make our move.
        setFacing(SE)
        changeCamTarget({nandorIndex})
        There's a room on the first floor the guards would keep slaves in when they didn't have time to supervise them. We can lock them in there and come back for them when we've won and found the key. 
        ->4ad

=== 4ad ===


    +What if we don't succeed? Won't they be trapped in there?
        I don't like to plan for that event, but if we fail then the guards should free them when they retake the mine. They won't have any reason to keep the lockdown going with the worms gone and us... no longer in the way. And we can leave them with some of the supplies from the stockroom on this level just in case.
        ->4ad
    +It will have to do. Let's get moving then.
        ->4ada

=== 4ada ===

setFacing(SE)

setNPCFacing({nandorIndex},NW)
setNPCFacing({carterIndex},SW)
setNPCFacing({marcosIndex},NE)

changeCamTarget({nandorIndex})

keepDialogue()

Wait a moment. I just wanted to say that, with the breach sealed and the worms no longer a threat, I have not felt like this in a long time... relieved. Hopeful. All thanks to your efforts.
    ->4b

=== 4ba ===

finishQuest(Sealing the Breach, true, Márcos Sealed the Breach.)

setFacing(SE)

setNPCFacing({nandorIndex},NW)
setNPCFacing({carterIndex},SW)

changeCamTarget({nandorIndex})

->4b

=== 4b ===

The breach is sealed and the worms are no longer a threat. I have not felt like this in a long time... relieved. Hopeful. All thanks to your efforts.

{
-toldCarterPassword:
    +Don't thank me yet. We still need to rally the other slaves and fight clear of the guards.
        ->4c
-else:
    +Our cooperation was what made this possible. But let us not waste time, I am eager to return to camp.
        ->4c
}
    +I have that effect on people. Now, if you're ready, I say we head for the surface.
        ->4c

=== 4c ===

changeCamTarget({nandorIndex})

{
-not toldCarterPassword:
    ->4d
}

activateQuestStep(Finding Nándor, Return to the Surface.)
setToTrue(mineLvl3SlavesBackToSurface)
    
    {
        -weftAddedToParty:
            ->weftHesitates_1a
        -else:
            ->Close
    }
    
=== weftHesitates_1a ===

fadeToBlack(true,false)

deactivate({thatchWithNandorIndex})
deactivate({marcosIndex})
deactivate({carterIndex})
deactivate({nandorIndex})
changeCamTarget({weftIndex})

movePlayerPos(-5,5)
setFacing(SW)

setNPCFacing({weftIndex}, NE)

fadeBackIn(60)

\*As the others move to leave, Weft hesitates.*

    +Weft? Is something the matter?
        ->weftHesitates_1b

=== weftHesitates_1b ===

It's nothing. I'm just weary from battle.

    +You're normally better at lying than that. What is it?
        ->weftHesitates_1c
    +If you don't wish to discuss it, then we should keep moving.
        ->Close

=== weftHesitates_1c ===

I have no love for the guards, but after having slain some... I am forced to wonder if the same fate awaits me at the end of this revolution.

The other branded may hate me even more than they hate the guards. When this road ends, what awaits one such as I?

    +They may hate you, but you haven't done anything to offend me yet. I can keep you safe from the others, if that remains so.
        ->weftHesitates_1ea
    +That will be resolved at a later time. There is no use worrying about it now.
        ->weftHesitates_1eb
    +You chose your fate when you turned against your fellow branded. 
        ->weftHesitates_1d

=== weftHesitates_1d ===

And what fate is that? Beheading? Hanging from the neck? Worse?

    +Nothing so dramatic. You'll be punished, but not with your ending. And after, you'll be free like the rest of us.
        ->weftHesitates_1ea
    +A quick death, without humiliation. That will be preferable to the fates of many of the guards, believe me.
        \*Weft hangs his head.* If that is the case, I will begin making my peace with it.
        ->Close
    +I'm unable to say, but it won't be pleasant. Now keep up, we must be going.
        \*Weft keeps his face a mask as he follows your lead.*
        ->Close

=== weftHesitates_1ea ===

When the stakes are so high, the only way I can trust what you say is if I am given collateral. And the most binding form of such is with an oath before the Gods.

->weftHesitates_1e

=== weftHesitates_1eb ===

No! This cannot wait. No question could be more pressing; I need it answered!

    +If you showed such courage before the guards, you'd gain more respect. But if it will calm you, I don't plan to see you executed when this is over.
        ->weftHesitates_1ea
    +You are not in a place to make demands. Now, we must move before the others begin to worry.
        \*Weft keeps his face a mask as he follows your lead.*
        ->weftHesitates_1ea


=== weftHesitates_1e ===

Swear it. Swear before the Gods that when this is all over I will leave this revolution with my life, lest They take my revenge for me after my passing.

    +I'm not going to do that. It isn't right to bother the Gods with a matter so trivial.
        setToTrue(refusedToGiveOathForWeftsLife)
        That response does not fill me with hope. Let's just get moving before the others start to worry.
            ->Close
    +Very well. I swear that your death will not come from punishment received from your fellow revolutionaries, or may the Gods conjure a punishment for me worthy of my failure.
        setToTrue(gaveOathForWeftsLife)
        \*The worry fades from Weft's features.* Thank you. Such an oath does much credit to your intentions. 
            ->Close

=== 4d ===

Before we do, I wish to include you in something. That is, unless you object, Carter?

changeCamTarget({carterIndex})

No, say your piece. I think they've more than earned the right to know.

changeCamTarget({nandorIndex})

Carter and I are a part of a conspiracy to free the branded. Before being trapped on this floor, I had derived a plan in which we would eventually find weapons, gather the other branded, and fight the guards in open revolt.

{
-toldToFindNandor:
    +I am aware of your part in the plan. Kastor asked me to find you and ask you which way the wind blows.
        ->agreedToJoinNandor(->4ga)
-knowRevolutionPassword:
    +Then perhaps you know which way the wind is blowing?
        ->agreedToJoinNandor(->4gb)
-else:
    +This is very interesting. And I suppose this is the part where you ask me to help you?
        You've divined my motive. What say you?
        ->4ea
    +That sounds like the plan of a fool. The guards would break us like twigs.
        ->4e
}

=== 4e ===

Does it? Consider their casualties. Many guards died during the evacuation of this floor. There will never again come an opportunity like the present, for eventually they will replenish those numbers.

{
-mineLvl3AgreedToFightGaspar or mineLvl3DealtWithGaspar:
And we, just now, put to rest the myth of their invincibility. We should capitalize on their weakness now while we can, or else we will never again be able to!
}

changeCamTarget({carterIndex})

Nándor speaks true. His plan is sound, and the time is right. We should strike while the guards are weak and diminished.

    ->4ea

=== 4ea ===

    +If you have a plan, then I would hear it all before I commit myself to it.
        changeCamTarget({nandorIndex})
        setToTrue(learnedCampLocationFromCarter)

        The first part of the plan was to learn where this camp lies on a map. The guards have kept the camp's location a secret since it was founded, but Carter has informed me we are currently within the Kingdom of Masons. If we can fight clear of the guards, we will not have to worry about more Lovashi taking back the camp for a long time.

        After that, we must gather weapons. Originally, I thought to find a way into the armory the guards keep in the barracks on the surface, but the mine itself has another cache of tools that we could plunder. The lockdown has made this part of the plan much easier.

        Lastly, we will need to make contact with the nonbranded slaves inside the Manse. They are not as numerous as the branded, but any assistance they could provide us may prove pivotal. This may be the trickiest part.

        After that, we would be ready to begin a riot which would allow us to rally the other branded. After that, we would begin to free the other parts of the camp and eventually, take the Manse itself.
        ->4ea
    +You are beginning to sway me. I will see where this goes.        
        ->agreedToJoinNandor(->4g)
    +I won't be your lemming, no matter how appealing you try to make this cliff.
        ->4eb

=== 4eb ===
changeCamTarget({nandorIndex})

\*Nándor gives an expression of genuine surprise.* Please, friend. Reconsider. I've seen how you fight: your help would be a considerable boon to our cause.

    +I will not be partner to your lunacy. I won't die fighting the guards.
        ->4f
    +Fine. I will help you with your plan, despite my misgivings.
        ->agreedToJoinNandor(->4g)

=== 4f ===

changeCamTarget({nandorIndex})
setNPCFacing({nandorIndex},NE)

What do we do?

changeCamTarget({carterIndex})

They've said no. Their knowledge of our plans puts those plans in jeopardy.

removeFromParty({nandorIndex})
removeFromParty({carterIndex})

changeCamTarget({nandorIndex})
setNPCFacing({nandorIndex},NW)

playAnimation({nandorIndex},Idle_Back)
playAnimation({carterIndex},Idle_Front)

\*Nándor shakes his head, then draws his weapon.* This is such a waste, but Carter is right. Your silence is a chance we cannot take.

->Close

=== agreedToJoinNandor(->divert) ===
activateQuestStep(Finding Nándor,Return to the Surface.)
changeCamTarget({nandorIndex})
->divert

=== 4g ===

Glorious. Now, let us return to Kastor, the final member of this conspiracy's leadership. His hut is in the southwest portion of the camp, along the southern wall, up on the surface. He will inform us how the plan has faired while we've been trapped down here.

->Close

=== 4ga ===

\*Nándor lets out a laugh.* Glorious! I did not realize you were already a friend of the revolution. Let us return to Kastor then. He will inform us how the plan has faired while we've been trapped down here.

->Close

=== 4gb ===

\*Nándor lets out a laugh.* East! This is glorious, I did not realize you were already a friend of the revolution. Let us return to Kastor then. He will inform us how the plan has faired while we've been trapped down here.

->Close

=== 5a ===

activate({pazmanIndex})
activate({rekaIndex})
activate({viragIndex})
activate({gasparIndex})

playAnimation({playerIndex},OOC_Idle_Back)

{
-partyFlagThatch:
activate({thatchIndex})
}

{
-weftAddedToParty and not kastorExecutedWeft:
activate({weftIndex})
}

changeCamTarget({gasparIndex})

Damn. More dead branded.

setNPCFacing({pazmanIndex},NW)
changeCamTarget({pazmanIndex})

It wasn't like we were going to make our quota before, anyways. What's two more? Shame about Márcos, though.

changeCamTarget({gasparIndex})

\*Gáspár seems lost in thought.*

changeCamTarget({pazmanIndex})

Overseer? Your orders?

changeCamTarget({gasparIndex})

My orders are to return to the surface. We'll need to inform the Director of what happened here. After that, hot food and bed rest for each of us.

    +You look concerned, overseer. Your face wears empathy like the two are strangers.
        setNPCFacing({pazmanIndex},SE)
        ->5b
    +That had better include me. I'm starving.
        Rewards are not your place to decide. However, I'm certain Chief Tabor would like to reward you once he hears of the loyalty you displayed just now.

        But let us be gone from here. I wish to finally see the sun again.
        ->Close

=== 5b ===

This is not empathy. Traitors deserve none. I'm merely concerned about the Director will think ab- *Gáspár cuts himself off in frustration* I need not explain myself to you, scum!

    +Then forget I asked. Let us be gone from here.
        ->Close

=== Close ===

fadeToBlack()

deactivate({nandorIndex})
deactivate({carterIndex})
deactivate({marcosIndex})
deactivate({pazmanIndex})
deactivate({rekaIndex})
deactivate({viragIndex})
deactivate({gasparIndex})
deactivate({weftIndex})
deactivate({thatchIndex})
deactivate({thatchWithNandorIndex})

fadeBackIn(60)

close()

->DONE