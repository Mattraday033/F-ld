VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR mineLvl3ClearedCratesToGuards = false
VAR mineLvl3ToldAboutJelly = false
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


->1a

=== 1a ===

changeCamTarget(1)

You there! Step into the light and identify yourself!

    +\*Comply.* I am {playerName}.
        ->1b
    +\*Ignore him.*
        ->Close

=== 1b ===

A slave? And not one of the other survivors. I don't recognize you. Explain why you are down here, immediately!

{
-mineLvl3ToldAboutJelly:
    +I'm helping Nándor seal the breach that the worms are coming through. We need your help and some blasting jelly.
        ->1k
}
    +I was let out of my hut by Guard László and I was trying to be helpful by looking for survivors. *Show badge*
        ->1i
    *I got lost.
        ->1c

=== 1c ===

You got so lost that you wandered all the way down to the third level of the mine.
    
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

And you were just now wandering towards the deepest part of the mine with the most worms in it when you happened to turn towards this room instead and get spotted by me.

{
-charisma >= 2:
    +You've summed it up pretty well. <Cha 2>
        ->1f
}
    +Well when you put it like that it does sound a little silly.
        ->1g

=== 1f ===

Well good thing you did or you might have gotten hurt! We're down enough slaves without you going off and dying on us. Quick, climb over here and I'll introduce you to the Overseer.
        
    ->2a

=== 1g ===

That may well be the most far-fetched story I've ever heard. I'm going to climb over this barricade now and execute you as a spy. Please hold still. <Combat>
    ->Close

=== 1h ===

keepDialogue()

How about I give you one more chance to explain yourself before I start to get belligerent.
    ->1b

=== 1i ===

You're down here by yourself? I've never heard of a slave who'd risk their neck like that for a guard. Especially some guards they never met.

{
-charisma >= 2:
    +Just because you're a slaver doesn't mean you deserve to die down here in the dark. <Cha 2>
        ->1j
}
    +In truth I was looking for other slaves and just happened to find you instead.
        That sounds more likely, but it's an awful long way to go for anyone. I'm not taking any chances. Let's see if your story changes after a few rounds of flogging. <Combat>
        ->Close
    +Well you've never met me.
        And that makes our first meeting being at the bottom of a worm infested mine all the more suspicious. Come here, let's see if you're still an altruist after I finish flogging you. <Combat>
        ->Close

    
=== 1j ===

Now that's something I can believe. I'm not sure anyone deserves that. Let me show you to the overseer, see what he makes of you.


->2a

=== 1k ===

Nándor is alive? Is anyone else with him?

    +Carter and Márcos made it as well.
        ->1l


=== 1l ===

Incredible. If you were able to make it down here, maybe with your help we actually stand a chance. It would certainly beat being stuck in here forever. Let me show you to the overseer.

~toldPazmanAboutSealingBreach = true

->2a

=== 2a === 

fadeToBlack()

deactivate(1)
activate(2)
moveToLocalPos(-41.5,-1.35)
changeCamTarget(3)
setToTrue(mineLvl3ClearedCratesToGuards)
fadeBackIn(60)

Pázmán, why is there a slave in my storeroom? My orders were clear, we don't know how long we're going to be down here and we can't afford any extra mouths.
    
changeCamTarget(2)

{toldPazmanAboutSealingBreach: ->2b | PH }


->Close
    
=== 2b ===

Sir, this isn't one of the slaves that works on this level. They came down here from up top.

changeCamTarget(3)

\*The overseer squints at you.* Are you sure? All of the branded look the same to me.

changeCamTarget(2)

I'm certain sir. This means the gate is open. We can leave!

    +Actually, we can't leave yet. We need to seal the breach the worms are coming through. 
        ->2c

    +Yeah that's right. No need to thank me. 
        ->Close

=== 2c ===

changeCamTarget(3)

Slave, the only think keeping me from beating you for daring to give me an order is my elation at getting to sleep in my own bed again. You have one chance to explain why we would do that before that feeling leaves my body.

{
-strength >= 3:
    +How about this for a reason: when I kill you in your own mine, your body will come pre-buried. <Str 3>
        ->Close
}

    +I meant no disrespect, I was only pointing out that if we didn't do it now we would have to do it later when there were more worms.
        ->2d
    +I've killed more worms than you can count. You can't cow me.
        ->Close


=== 2d ===

changeCamTarget(4)

Sir, the slave is right. We should strike now while we can still walk around down here, instead of later when we'll have to swim.

changeCamTarget(3)

\*The overseer kneads his eyebrows.* I see your point. Fine, everyone grab your gear and get ready to move. Slave, since you're so eager, you get to lead the way. Don't worry, when you inevitably get swarmed by the buggers we'll step over your corpse and finish the job.

Pázmán, stay here and keep manning the barricade. If we need to retreat and rest we will come back.

changeCamTarget(2)

Yes sir!

setToTrue(mineLvl3GuardsInParty)
fadeToBlack()

deactivate(3)
deactivate(4)
deactivate(5)

fadeBackIn(60)
    ->Close

=== 2e ===

->Close

=== 2f ===

->Close

=== 2h ===

->Close

=== 2i ===

->Close

=== 2j ===

->Close

=== 2k ===

->Close

=== 2l ===

->Close

=== 2m ===

->Close

=== 2n ===

->Close

=== 3a ===

->Close

=== 3b ===

->Close

=== 3c ===

->Close

=== 3d ===

->Close

=== 3e ===

->Close

=== 3f ===

->Close

=== 3g ===

->Close

=== 3h ===

->Close

=== 3i ===

->Close

=== 3j ===

->Close

=== 3k ===

->Close

=== 3l ===

->Close

=== Close ===

close()

->DONE