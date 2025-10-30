VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR metMuzsa = false
VAR givenTaskByLaszlo = false
VAR knowsAboutTheMine = false
VAR turnedDownMuzsasTask = false
VAR givenTaskByMuzsa = false
VAR hasCandy = false
VAR gaveCandyToMuzsa = false
VAR mineCratesCleared = false
VAR mentionedGoodReasonForGoingInsideMine = false
VAR mentionedBadReasonForGoingInsideMine = false
VAR gaveSnipeHuntExcuseToMuzsa = false

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
-gaveSnipeHuntExcuseToMuzsa:
    ->4b
-mineCratesCleared:
    ->3a
-not metMuzsa:
    ->1a
-mentionedBadReasonForGoingInsideMine:
    ->2d
-not turnedDownMuzsasTask and not givenTaskByMuzsa and mentionedBadReasonForGoingInsideMine:
    ->3a
-not turnedDownMuzsasTask and not givenTaskByMuzsa:
    ->1a
-turnedDownMuzsasTask and not givenTaskByMuzsa:
    ->1g
-givenTaskByMuzsa and not gaveCandyToMuzsa:
    ->1i
}

=== 1a ===

Whoa there slave! The mine's off limits right now. Where do you think you're going?

{
-charisma >= 2:
    +One of the guards told me the mine was full of snipes. He sent me to clear them out. *Show badge* <Cha {charisma}/2>
        ->4a
}
    +Right back to work, ma'am!
        That's where I thought you were going.
        ->Close
    +I was just curious about the barricade. It seems strange for the mine to be sealed up like this.
        ->1b
    +I wanted to take a look inside the mine.
        ->2a

=== 1b ===

setToTrue(metMuzsa)
setToTrue(knowsAboutTheMine)

~knowsAboutTheMine = true

You're new, so I'll give you the quick version of what happened. Some critters dug their way into a tunnel on the lowest floor and the whole mine is lousy with them. The bats were already bad enough but now we're shutting the whole place down until we can flush them out.

    +Critters?
        ->1c
    +That's too bad. I was hoping to take a look inside.
        ->2a
    +I see why you built the barricade. I'll be getting back to work now.
        ->Close

=== 1c ===

Yeah, some kind of worms or whatever. We'll get rid of them soon enough but until then we all get to sit around with our thumbs up our asses in this lockdown.

Which reminds me: what are you doing out of quarters?

{
-givenTaskByLaszlo:
    +Guard László told me to run an errand for him. *Show badge*
    
        If László trusts you to help him with the odd job, then maybe you can help me out too.
        ->1d
-else:
Placeholder: givenTaskByLaszlo = false
->Close
}


    
=== 1d ===

I can't leave my post because I have to watch the mine, but I'm dying for something sweet. Kende, the cook in the mess hall, runs a little side business pushing hard to get items he has brought in with his supply orders.

    ->1da

=== 1da ===

If you run over and buy some candy off him, I won't tell anyone I caught you goofing off near the barricade. Plus, I'll let you keep the change. What do you say?

    +I don't know. It's suspicious that you're offering to pay me instead of just ordering me to do it.
        ->1db
    +I want to look around the mine a little. If I do this for you, will you let me by?
        keepDialogue()
        Hmmm, I really shouldn't... Fine. Help me out, and I'll let you take a quick look around in there.
        ->1da
    +Yes ma'am. Right away, ma'am.
        ->1e
    +That's mighty generous of you. Sure, I'm game.
        ->1e
    +No disrespect, ma'am, but I'm already late getting back to László.
        ->1f

=== 1db ===

keepDialogue()

Maybe I've been a guard here long enough to know the best way to get a slave to do what you want to the letter is to offer a carrot rather than a stick. Besides, if Kende's side business gets compromised because of a mouthy slave like you I'll get to take a stick to your hide anyways. So how about it, do we have a deal?

    ->1da

=== 1e ===

setToTrue(givenTaskByMuzsa)

{
-mentionedGoodReasonForGoingInsideMine:
activateQuestStep(Múzsa's Sweet Tooth, 1)
-else:
activateQuestStep(Múzsa's Sweet Tooth, 0)
}

prepForItem()

The mess hall is the large building with the fenced-in yard in the south eastern part of the camp. If you just continue back down the path behind you until you hit the eastern wall you can't miss it. When you see Kende, tell him Múzsa sent you. He should be a little more talkative about his wares that way.
        
giveCoins(50)
        ->Close

=== 1f ===

setToTrue(turnedDownMuzsasTask)

Whatever. Now get out of here before I report you for laziness.

->Close

=== 1g ===

You're back. Are you finished helping László yet?

    +I'm able to run your errand now.
        ->1e
    +Sorry ma'am, not yet.
        ->1h

=== 1h ===

Fine, then stop wasting my time.
    ->Close

=== 1i ===

searchInventoryFor(hasCandy,Candy)

Any luck with Kende? He can be a bit of a hard ass sometimes.

{
-hasCandy:
    +Yeah I got some. *Hand over the candy.*
        ->1j
}
    +No, not yet. I'm working on it right now, ma'am.
        ->1h

=== 1j ===

~gaveCandyToMuzsa = true
setToTrue(gaveCandyToMuzsa)


{
-mentionedGoodReasonForGoingInsideMine:

finishQuest(Múzsa's Sweet Tooth, true, 6)

prepForItem()

Yes! Finally! Ok, I'll keep my end of the bargain. You can go on in but be careful in there. If they come looking for you I never saw you go in, but if they find your corpse in there later it's probably gonna be my ass on the chopping block.

takeAllOfItem(Candy)

    +I understand.
        
        fadeToBlack()
        
        deactivate(1)
        activate(2)
        
        fadeBackIn(60)
    ->Close
-else:

finishQuest(Múzsa's Sweet Tooth, true, 6)

prepForItem()

Yes! Finally! Ok, I'll keep my end of the bargain. Keep the change, I consider it money well spent. Now get out of here before you get both of us in trouble.

takeAllOfItem(Candy)
   
    +Can I go inside the mine?
        ->3b
    +Alright, I'll go.
        ->Close
}


=== 1k ===

->Close

=== 1l ===

->Close

=== 1m ===

->Close

=== 1n ===

->Close

=== 2a === 

setToTrue(metMuzsa)

You're the first slave I've ever met who wanted to go inside the mine. Especially now that it's crawling with bats and worms and who knows what else. Why the blazes do you want to look in there?

{
-knowsAboutTheMine:
    +I hoped that if I kill some worms the guards might give me some extra rations.
        ~mentionedGoodReasonForGoingInsideMine = true
        ->2b
}
    +I was just curious what all the fuss was over the lockdown.
        ->2c
    +I thought people might have dropped things when they ran out that I could grab.
        ~mentionedGoodReasonForGoingInsideMine = true
        ->2b
    +One of my hutmates left something in there that I wanted to retrieve. <Lie>
        ->2c
    
=== 2b ===

You're quite the little opportunist, aren't you? A sharp, enterprising slave like yourself can go far in this camp with the right friends. 

How about this. Prove to me you can be trusted with a quick task and I'll enable your little venture inside the mine.

    +What's the job?
        ->1d

=== 2c ===

~mentionedBadReasonForGoingInsideMine = true
setToTrue(mentionedBadReasonForGoingInsideMine)

activateQuestStep(Múzsa's Sweet Tooth, 2)

Well that's too bad, because I'm not getting in trouble just so you can take a look around in there. On your way, slave.

->Close

=== 2d ===

searchInventoryFor(hasCandy,Candy)

You're back? What part of 'on your way' did you not understand?

{
-hasCandy:
    +I was only wondering if you were interested in a simple deal? Something sweet, and I get to take a look inside the mine?
        ->2e
}
    +My apologies, ma'am. I was just leaving.
        ->Close

=== 2e ===

\*Múzsa is clearly thinking it over.* Something sweet? Show me.

    +\*Show her the candy.*
        ->2f

=== 2f ===

Those look like Kende's sweets. Fine, if he trusts you, I suppose I can too. Hand them over.

    +\*Pass Múzsa the candy.*
        ->3e

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

searchInventoryFor(hasCandy,Candy)

Scram slave, I'm busy.

{
-gaveCandyToMuzsa and not mineCratesCleared:
    +Can I go inside the mine?
        ->3b
-hasCandy and mentionedBadReasonForGoingInsideMine:
    +I heard around the camp that you have a bit of a sweet tooth.
        ->3c
    +Alright, I'll go.
        ->Close
-else:
    +Alright, I'll go.
        ->Close
}


=== 3b ===

setToTrue(mineCratesCleared)

I really shouldn't let you in there... but they don't pay me to keep slaves out of the mine, just to keep the worms in. Sure you can go inside, but don't get yourself killed in there or someone might make it my problem.

fadeToBlack()

deactivate(1)
activate(2)

fadeBackIn(60)

->Close

=== 3c ===

\*Múzsa looks you over.* I wouldn't say no to some candy, if you have any.

+That I do. What would you say to a little trade? I give you the Candy and you let me past the barricade?
    ->3d
    
=== 3d ===

Past the barricade? Eh, fine. If you get yourself killed in there no one's gonna notice one more slave's corpse. You have a deal.

    +\*Hand over the Candy.*
        ->3e
    
=== 3e ===

finishQuest(Múzsa's Sweet Tooth, true, 6)

prepForItem()

Alright, a deals a deal. Now get on in there before anyone notices.

takeAllOfItem(Candy)

fadeToBlack()

deactivate(1)
activate(2)

fadeBackIn(60)

->Close

=== 4a ===

setToTrue(gaveSnipeHuntExcuseToMuzsa)
setToTrue(mineCratesCleared)

\*Múzsa gives you a pitying look.* I'm not gonna get in the way of such an <i>important</i> task, but don't go past the first floor, and watch out for the bats. And if any of the guards say they forgot their 'left-handed smoke shifter' somewhere, come get me. I'll help you find it.

fadeToBlack()

deactivate(1)
activate(2)

fadeBackIn(60)

    ->Close

=== 4b ===

How's the snipe hunt going? *Múzsa chuckles and shakes her head.*

->Close

=== Close ===

close()

->DONE