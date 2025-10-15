VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR fullGuardFightIndex = 0
VAR guardsWithBarricadeFightIndex = 1

VAR playerIndex = 0
VAR pazmanIndex = 2
VAR gasparIndex = 3
VAR rekaIndex = 4
VAR viragIndex = 5
VAR pazmanCratesIndex = 6

VAR introducerIndex = 2

VAR questItemListIndex = 3
VAR blastJellyItemIndex = 5
VAR blastJellyItemQuantity = 1

VAR askedForReward = false

VAR mineLvl2GuardsMovedToSecondLevelGate = false
VAR mineLvl2GuardsFinishedMove = false

VAR mineLvl3ToldToFindMarcos = false
VAR mineLvl3ClearedCratesToGuards = false
VAR mineLvl3ToldAboutJelly = false
VAR mineLvl3ToldPazmanToEatShit = false
VAR mineLvl3ThreatenedGaspar = false
VAR mineLvl3GuardsInParty = false


VAR mineLvl3SpeakingFromGuardCrates = false
VAR mineLvl3SpeakingFromBrokenGate = false

VAR toldPazmanAboutSealingBreach = false

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
-mineLvl3SpeakingFromBrokenGate:
    setToFalse(mineLvl3SpeakingFromGuardCrates)
    ~mineLvl3SpeakingFromGuardCrates = false
    ~introducerIndex = viragIndex
-else:
    ~mineLvl3SpeakingFromGuardCrates = true
    setToTrue(mineLvl3SpeakingFromGuardCrates)
    ~introducerIndex = pazmanIndex
}

{
-mineLvl3ToldToFindMarcos:
->4a
-else:
->1a
}

=== 1a ===

changeCamTarget({introducerIndex})

{
-mineLvl3SpeakingFromBrokenGate:

    \*A guard turns at the sound of the gate's lifting.* You there! Step into the light and identify yourself!
-else:

    \*A man wearing a guard uniform and holding a torch shouts at you from behind his barricade.* Halt! Stand and let me get a good look at you.
}

    {
    -mineLvl3SpeakingFromGuardCrates:
        +\*Ignore the guard and leave.*
            ->Close
    }

    +\*Comply.*

    {
    -mineLvl3SpeakingFromBrokenGate:
        changeCamTarget({playerIndex})
        fadeToBlack()
        moveToLocalPos(-40.5,-1.35)
        setFacing(SW)
        fadeBackIn(60)
        changeCamTarget({introducerIndex})
    }
        ->1b
    +This ain't the surface. I'm gonna make you eat those orders. <Combat>
        ->combatPrep

=== 1b ===

A slave? And not one of the other survivors. I don't recognize you. Explain why you are down here, immediately!

{
-mineLvl3ToldAboutJelly:
    +I'm helping Guard Márcos seal the breach that the worms are coming through. We need your help and some blasting jelly.
        ->1k
}
    +I was let out of my hut by Guard László and I was trying to be helpful by looking for survivors. *Show badge*
        ->1i
    *I got lost.
        ->1c

=== 1c ===

You got so lost that you wandered all the way down to the lowest level.
    
    +Uh... yeah.
        ->1d
    +No...
        ->1h
    
=== 1d ===

Past the flocks of bats, the packs of hungry worms and the locked gate.

    +Ayup.
        ->1e
    +Well actually...
        ->1h

=== 1e ===

{
-mineLvl3SpeakingFromBrokenGate:
And you were just now wandering towards the deepest part of the mine, with the most worms in it, when you happened to turn towards this room instead, lift two gates to get inside it, and then get spotted by me.
-else:
And you were just now wandering towards the deepest part of the mine, with the most worms in it, when you happened to turn towards this room instead and get spotted by me.
}

{
-charisma >= 2:
    +You've summed it up pretty well. <Cha 2>
        ->1f
}
    +Well when you put it like that it does sound a little silly.
        ->1g

=== 1f ===

{
-mineLvl3SpeakingFromBrokenGate:

Well good thing you did or you might have gotten hurt! We're down enough slaves without you going off and dying on us. Let me introduce you to the Overseer.

-else:

Well good thing you did or you might have gotten hurt! We're down enough slaves without you going off and dying on us. Quick, climb over here and I'll introduce you to the Overseer.

}
    ->2a

=== 1g ===

That may well be the most far-fetched story I've ever heard. I'm not taking any chances with the likes of you.
    ->1m

=== 1h ===

keepDialogue()

How about I give you one more chance to explain yourself before I start to get belligerent.
    ->1b

=== 1i ===

You're down here by yourself? I've never heard of a slave who'd risk their neck like that for a guard. Especially some guards they never met.

{
-charisma >= 2:
    +Just because you're a slaver doesn't mean you deserve to die down here in the dark. <Lie?> <Cha {charisma}/2>
        ->1j
}
    +In truth I was looking for other slaves and just happened to find you instead.
        That sounds more likely, but it's an awful long way to go for anyone. I'm not taking any chances.
        ->1m
    +Well you've never met me.
        And that makes our first meeting being at the bottom of a worm infested mine all the more suspicious.
        ->1m

    
=== 1j ===

Now that's something I can believe. I'm not sure anyone deserves that. Let me show you to the overseer, see what he makes of you.

->2a

=== 1k ===

Guard Márcos is alive? Is anyone else with him?

    +Carter and Nándor made it as well.
        ->1l


=== 1l ===

Incredible. If you were able to make it down here, maybe with your help we actually stand a chance. It would certainly beat being stuck in here forever. Let me show you to the overseer.

~toldPazmanAboutSealingBreach = true

->2a

=== 1m ===

Slave, you will submit to my authority. Drop any tools you may be carrying and keep your hands where I can see them while I climb over this barricade and apprehend you.

{
-mineLvl3SpeakingFromGuardCrates:
    +Eat shit, slaver. I'm not going anywhere with you. *Leave.*
        setToTrue(mineLvl3ToldPazmanToEatShit)
        ->Close
    +Yes sir. I need to talk to someone about all the worms around here anyways.
        ->2a
    +Yes, come to this side of the barricade. It'll be easier to bleed you that way. <Combat>
        ->combatPrep
-else:
    +Yes ma'am. I need to talk to someone about all the worms around here anyways.
        ->2a
    +Command me at your peril, slaver. <Combat>
        ->combatPrep
}

=== 2a === 

fadeToBlack(true, false)

{
-mineLvl3SpeakingFromGuardCrates:
    deactivate({pazmanCratesIndex})
    setToTrue(mineLvl3ClearedCratesToGuards)
    activate({pazmanIndex})
}

moveToLocalPos(-41.5,-1.35)

setFacing(NE)

setToTrue(mineLvl3MetGaspar)
changeCamTarget({gasparIndex})

fadeBackIn(60)

{mineLvl3SpeakingFromGuardCrates:Pázmán|Virág}, why is there a slave in my stockroom? My orders were clear: we don't know how long we're going to be down here and we can't afford any extra mouths.
    
changeCamTarget({introducerIndex})

->2b

=== 2b ===

Sir, this isn't one of the slaves that works on this level. They came down here from up top.

changeCamTarget({gasparIndex})

\*The overseer squints at you.* Are you sure? All of the branded look the same to me.

changeCamTarget({introducerIndex})

I'm certain sir. This means the gate is open. We can leave!
    
{
-mineLvl3ToldAboutJelly || mineLvl3ToldAboutJelly:
    +Actually, we can't leave yet. I have gathered some of the other survivors. We need to seal the breach the worms are coming through. 
        changeCamTarget({gasparIndex})
        ->2c
}

    +And I am the one who opened the gate to the upper levels. I suppose a reward is in order?
        ~askedForReward = true
        -> 3a

    +Yeah that's right. No need to thank me. 
        ->3a

=== 2c ===

You apparently do not understand the predicament you are in. Protocal dictates that should a level of the mine be overrun, the survivors are to quarantine that level until they have the force to take it back. By opening that gate, you have violated the Director's orders that it should remain closed! And now you dare to give us orders?

changeCamTarget({rekaIndex})

Overseer, with respect, the slave has risked their life considerably to get down here. Shouldn't we at least hear the reason?

->2caa

=== 2caa ===

{

-strength >= 3 and wisdom >= 3:
    +How about this for a reason: when I kill you in your own mine, your body will come pre-buried. <Str {strength}/3>
        ->2ca
        
    +\*Look around at the other guards.* It seems to me that you have three witnesses to you choosing not to seal the breach when you could have. I wonder what the Director will think of your cowardice should he learn of it. <Wis {wisdom}/3>
        ->2cb
-strength >= 3:
    +How about this for a reason: when I kill you in your own mine, your body will come pre-buried. <Str {strength}/3>
        ->2ca
-wisdom >= 3:
    +\*Look around at the other guards.* It seems to me that you have three witnesses to you choosing not to seal the breach when you could have. I wonder what the Director will think of your cowardice should he learn of it. <Wis {wisdom}/3>
        ->2ca
}
    +I meant no disrespect, but if we don't do it now we will have to do it later when there are more worms.
        ->2d
    +I've killed more worms than you can count. You can't cow me.
        ->2f

=== 2ca === //str

~mineLvl3ThreatenedGaspar = true
setToTrue(mineLvl3ThreatenedGaspar)
changeCamTarget({gasparIndex})

\*Gáspár's eyes dart to the guards surrounding you, then back to your confident expression. He chuckles nervously.* I think we would have better odds at succeeding if we didn't fight each other now. We can, uh, discuss your insubordination when we get back to the surface.

    ->2e

=== 2cb === //str

changeCamTarget({gasparIndex})

\*Gáspár's eyes dart to the other guards. He reaches for the whip on his belt, but  his hand lingers on the hilt as he thinks better of it. Finally, he shakes his head.* Fine. But since you're so eager, you get to lead the way. Don't worry, when you inevitably get swarmed by the buggers we'll step over your corpse and finish the job.
    ->2e

=== 2d ===

changeCamTarget({rekaIndex})

Sir, the slave is right. We should strike now while we can still walk around down here, instead of later when we'll have to swim.

changeCamTarget({gasparIndex})

\*The overseer kneads his eyebrows.* Fine, everyone grab your gear and get ready to move. Slave, since you're so eager, you get to lead the way. Don't worry, when you inevitably get swarmed by the buggers we'll step over your corpse and finish the job.

->2e

=== 2e ===

Pázmán, stay here and keep manning the {mineLvl2GuardsFinishedMove:gate|barricade}. If we need to retreat and rest we will come back. Come find us once we've cleared the way.

changeCamTarget({pazmanIndex})

Yes sir!

setToTrue(mineLvl3GuardsInParty)
activateQuestStep(Sealing the Breach, 1)

->deactivateExtras

=== 2f ===

changeCamTarget({gasparIndex})

Oh, I doubt it: I can count to twelve on a good day and there's no way you've killed more than that. Stand still, slave. I'll make this quick for you. *The guards reach for their weapons.*

->combatPrep

=== 3a ===

changeCamTarget({gasparIndex})

{askedForReward:A reward?|Thank you?} That gate was closed to prevent any of these worms from getting through to the higher levels! You opening it is in direction violation of standing camp orders! If you're looking for a reward for your transgressions, the only thing you'll receive is a trip to the pit, scum!

    +I've saved you and your guards from this death trap! A 'thank you' isn't too much to ask!
        ->3b
    +Look, lets calm down. We can put the gate back the way it was, but with us on the right side of it, and we all get to see the sun again. No harm done!
        ->3c
    +Are you saying I should have left you to rot?
        ->3d

=== 3b ===

Oh I'm thankful for what you've done, but I don't owe you anything. A branded who thinks a guard can be in their debt is a branded who doesn't understand their station! Your only "reward" is you get to keep breathing, and if you work hard, you'll get more of the same. When we get back to the surface I'll teach you how we treat those who don't follow orders, and I'll make sure the whole camp is present to learn the same lesson too.

->3e

=== 3c ===

The harm is that you took your own initiative to come down here, and broke camp protocol to do it. A slave with initiative is a slave that needs to be broken in a little more. When we get back to the surface I'll teach you how we treat those who break the rules, and I'll make sure the whole camp is present to learn the same lesson too.

->3e

=== 3d ===

Yes! Those were your orders, and you disregarded them completely. When we get back to the surface I'll teach you how we treat those who don't follow orders, and I'll make sure the whole camp is present to learn the same lesson too.

->3e

=== 3e ===

changeCamTarget({viragIndex})

Sir, what about Guard Márcos? He and the other slaves could still be out there somewhere.

changeCamTarget({gasparIndex})

You make a good point. And I think I have the right slave for the job. 
    ->3ea

=== 3ea ===

Since you're so fond of wandering around down here, I now task you with finding Guard Márcos... or his corpse. We're going to make our way to the gate you opened, to prevent any worms from getting through to the camp. If you return empty handed or take too long, we'll leave you down here and seal the gate behind us, so don't drag your feet and don't even think about coming back until the job is done.

{
-mineLvl3ToldAboutJelly:
    +Wait, I know where Guard Márcos is. He sent me here to speak with you.
        ->3fa
    +If you're just going to whip me when we get back to the surface, I think I'll take my chances with killing you now. Less witnesses that way. <Combat>
        ->combatPrep
-else:
    +I need some information first.
        ->5a
    +\*Keep quite and leave.*
        ->3eb

    +Fine. I'll look for your missing Guard Márcos. I prefer the worms to you lot anyways; they're friendlier *and* better looking.
    
        That's a dozen more lashes when we get back to camp. Now begone!

        ->3eb
        
    +If you're just going to whip me when we get back to the surface, I think I'll take my chances with killing you now. Less witnesses that way. <Combat>
        ->combatPrep

}

=== 3eb ===

setToTrue(mineLvl3ToldToFindMarcos)
activateQuestStep(Find Guard Márcos, 0)
setToTrue(mineLvl2GuardsMovedToSecondLevelGate)

->deactivateExtras

=== 3fa ===

So, that stubborn fool made it after all. What was his condition?

    ->3fc

=== 3fb ===

finishQuest(Find Guard Márcos, true, 2)

prepForItem()

So, that stubborn fool made it after all. What was his condition?

addXP(50)

    ->3fc


=== 3fc ===

    +Not good, he was bleeding all over the last I saw him. He wanted me to come get you and help you seal the breach the worms are coming out of.
        Seal the breach? As far as I'm concerned, Márcos is a traitor for disobeying my orders to leave the workers to their fate, and you're just a slave. Give me any reason, any reason at all, I shouldn't just leave the lot of you here to drown in worms and bats.
            ->2caa

=== 4a ===

changeCamTarget({gasparIndex})

I don't see Guard Márcos with you. Did you find his corpse or are you just trying to waste my time?

    {
    -mineLvl3ToldAboutJelly:
    +I found Márcos, alive.
        ->3fb
    }
    //+I'm sick of taking orders from you. <Combat>
     //   ->combatPrep

    +I'm still looking. *Leave.*
        ->deactivateExtras

=== 5a ===

I'll allow you to ask your questions, but make them quick. I am loathe to stay here any longer than I must.

    +Who is Guard Márcos?
        keepDialogue()

        He's one of my men. I ordered him to leave the branded that survived our first encounters with the worms, but he refused. He's either still out there somewhere, or he's worm food, but my superiors will want to know what happened to him. It's your job to bring me the answer.
        ->5a
    +Any idea where I should start looking?
        keepDialogue()

        Leave this room by the western tunnel, then turn north. That's the last we saw of him.
        ->5a
    +How did you get stuck down here?       
        Bad luck, mostly. It was our shift to watch the miners in the newest tunnel we were digging, and one of them struck a pocket. These worm things swarmed out of a gap in the wall, making quick work of the team closest to the breach.
        
        keepDialogue()
        In the confusion, some must have gotten around us out into the rest of this level because once we were able to extract ourselves from the first wave, the alarm had been sounded and the others had pulled out to the higher levels. Of course, protocol is to close the gate behind you during an evacuation, so we ended up trapped by our own comrades. 
        ->5a

    +I'm finished with my questions. 
        keepDialogue()

        If you're finished pestering me, then get going.
        ->3ea

=== combatPrep ===

{
    -mineLvl3ToldAboutJelly:
    activateQuestStep(Sealing the Breach, 2, true)
}

//deactivate({pazmanCratesIndex})
setToTrue(mineLvl3ClearedCratesToGuards)
setToTrue(mineLvl3KilledGuards)
//kill({pazmanIndex})
//kill({viragIndex})
//kill({rekaIndex})
//kill({gasparIndex})

{
-mineLvl3ClearedCratesToGuards or mineLvl3SpeakingFromBrokenGate:
    enterCombat({fullGuardFightIndex})
-else:
    enterCombat({guardsWithBarricadeFightIndex})
}


->Close

=== deactivateExtras ===
{
-(not mineLvl2GuardsFinishedMove) or mineLvl3GuardsInParty or (mineLvl2GuardsMovedToSecondLevelGate and not mineLvl2GuardsFinishedMove):
fadeToBlack()
}


{
-not mineLvl2GuardsFinishedMove:
    setToTrue(mineLvl3ClearedCratesToGuards)
    deactivate({pazmanCratesIndex})
}

{

-mineLvl3GuardsInParty:
    activate({pazmanIndex})
    deactivate({viragIndex})
    deactivate({rekaIndex})
    deactivate({gasparIndex})

-mineLvl2GuardsMovedToSecondLevelGate and not mineLvl2GuardsFinishedMove:
    deactivate({pazmanIndex})
    deactivate({viragIndex})
    deactivate({rekaIndex})
    deactivate({gasparIndex})
    setToTrue(mineLvl2GuardsFinishedMove)
}

fadeBackIn()

->Close

=== Close ===

close()

->DONE

/*
=== 1aa ===

changeCamTarget({pazmanCratesIndex})

Hey, you're that slave from earlier! Stand still this time, I'm coming over.
    
    {
    -mineLvl3ToldAboutJelly:
        +By the Mother, this is awkward. Look, Guard Márcos is still alive and he wants to use blasting jelly to seal the breach so more worms can't come through. I'll submit to your custody if you bring me to whoever's in charge.
            ~toldPazmanAboutSealingBreach = true
            ->1ab
    }
    
    +Fine, I submit.
        ->2a
    +\*Ignore him and leave.*
        ->Close
    +This ain't the surface. I'm gonna make you eat those orders. <Combat>
        ->combatPrep

=== 1ab ===

You're in no position to be making demands. But if what you say is true, Márcos has a good head on his shoulders. Fine. I'll bring you to the Overseer if you come quietly.
    
    +Deal. *Go with the guard.*
        ->2a
    +Too risky. I'm leaving.
        ->Close


*/