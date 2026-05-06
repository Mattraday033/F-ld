VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerName = ""

VAR dezsoIndex = 1
VAR loamIndex = 2
VAR hostageOneIndex = 3
VAR hostageTwoIndex = 4
VAR deadGuardIndex = 5
VAR weftIndex = 6
VAR outsideGuardsIndex = 7

VAR dezsoAndSlavesFightIndex = 0
VAR dezsoOnlyFightIndex = 1

VAR declaredHostagesDead = false
VAR savedHostages = false
VAR hostagesDead = false
VAR foughtDezsoAndLoam = false

VAR toldNotAllowedToLeave = false

VAR concludedHostageNegotiations = false
VAR spokeToTaborAtBeginningOfSituation = false

VAR mentionedStoneMan = false
VAR failedRushDezso = false

VAR hostageTakersStandardPunishment = false
VAR hostageTakersNoPunishment = false
VAR hostageTakersLeaderPunished = false
VAR hostageTakersLaborPunishment = false

{
-spokeToTaborAtBeginningOfSituation:

    movePlayer(3,5)

    activate({weftIndex})

    ->1b
-else:
    ->1a
}

=== 1a ===

You aren't supposed to be here. Leave before the guards get the wrong impression.

        ->Close

=== 1b === 

playAnimation({dezsoIndex},Secondary_Idle_Front)

The Lovashi's lapdogs have finally arrived. I'm not surprised to see Weft here, but I do not know you. 

    +I'm {playerName}. I'm here to negotiate your surrender.
        ->1c
    +Both you and I have made many decisions that lead us here. Few of them were savory.
        ->1ca

=== 1ca ===

Ah, but my decisions will set my friends and I free, while yours will keep you in chains.

    +There is no need to be adversarial. Let us come to an agreement with level heads.
        \*Dezso nods.* You are right. Let us exhaust all possibilities. 
        ->1d
    +That has yet to be seen. We are here seeking the release of the prisoners, and your surrender.
        ->1c
    +<Not Implemented> On the latter, I hope to prove you wrong: the Lovashi believe me here to negotiate, but I wish to join you.
        ->Close


=== 1c ===

We will not surrender to the guards. We're through with the labors of slaves.

    +You cannot seriously believe that you will escape here.
        Oh, we very much believe it. 
        ->1d

=== 1d ===

    +If you are so adamant that you have a chance at escape, why have you not made good on it already?
        ->1g
    +The Lovashi wait outside this hut in force. There is no escape.
        ->1e
    +Chief Tabor is keen to avoid bloodshed. He has given us an offer to bring to you if you are willing to hear it.
        ->2a(false)

=== 1e ===

We are aware they surround us, but they would not dare to harm us while we hold these hostages. We are content to wait as long as it takes for them to see reason.

{
-wisdom >= 2:
    +Do I hear... digging? <Wis {wisdom}/2>
        ->1h
}

    +I am branded, what do I care for your hostages? One word from me, and the Lovashi will kill the lot of you believing your prisoners already dead.
        ->1f
    +This is foolish. Waiting only benefits the seiger here, not the beseiged.
        Foolish to the blind, perhaps. We see no need to enlighten you to our motives.
        ->1d
    +Fine, then while you wait allow me to recite the offer Chief Tabor empowered me with.
        ->2a(false)

=== 1f ===

You're bluffing. They would surely learn of your actions.

    +How? Everyone in this room will be too dead to tell them. <Str {strength}/3>
    {
    -strength >= 3:
        ->1fa
    -else:
        ->1fb
    }

    +Believe what you will. I simply wish to set the tone of these proceedings.

        keepDialogue()

        Such threats are unnecessary. We are ready to hear your position in full.

        ->2a(false)

=== 1fa ===

\*Dezso blanches and drops his weapon.* There is no need for us to threaten each other. Y-you would prefer a different solution, yes? Or else you'd have already called the guards?

    +I'm glad you've decided to see things my way. Chief Tabor has an offer he wants me to give you.
        ->2a(true)

=== 1fb ===

Do it then. I am no fool: call the guards. 

+\*Shout as loud as you can.* Oh Gods! They've killed the hostages! <Combat>
    ->3bb
+Eh, you got me. It was just a bluff.
    Amusing. If there are more acts to this farce, we would happily watch you degrade yourself a while longer.
    ->1d

=== 1g ===

We are looking to make a trade: the hostages for our passage out of the camp.

    +But the Lovashi said all of their previous attempts to negotiate were rebuffed. Why stall like this?
        We are not stalling. We are simply trying to make sure that the Lovashi are negotiating in good faith. Their appetite for discussion is a good sign.
        ->1d

=== 1h === 

\*Dezso looks surprised, and worried.* Why would you say that?

    +I'm certain now. I hear shovels moving dirt. Are you trying to dig under the wall?
        ->1ha

=== 1ha ===

I don't know what you mean.

changeCamTarget({loamIndex})

The charade is obvious now. Just go ahead and tell them.

changeCamTarget({dezsoIndex})

\*Dezso shakes his head.* You've found us out. There are others of us tunneling towards the forest.

    +And that's why you're trying to stall for time. To let your friends complete it.
        You're right, of course. But now that you've discovered our ruse, we can't let you leave here to tell the Lovashi.
        ->1hb

=== 1hb === 

    +If time is what you need, then you won't mind telling me your plan before you attack.
        \*Dezso considers your proposal for a moment.* I can see the logic in that. Very well.
        ->2cba(false)
    +Then nothing has changed. Allow me to fill the time reciting the Lovashi's offer, and perhaps we can come to an agreement before we come to blows.
        ->2a(false)


=== 2a(succeededStrengthCheck) ===

We will hear it.

{
-hostageTakersLeaderPunished:
    +The Lovashi have declared that only the leader of this conspiracy will receive any punishment: a dozen lashes for each branded led astray. The rest are to be spread across the other huts of the camp, but will otherwise be left unhurt.
        {succeededStrengthCheck:->4a|->2d}
-hostageTakersNoPunishment or hostageTakersLaborPunishment:
    +I have persuaded Chief Tabor to spare you the whip and the axe, so long as the rest of the hostages are allowed to leave this hut intact.
        {succeededStrengthCheck:->4a|->2c}
-else:
    +Tabor has said that he will allow you to keep your lives, but you will suffer a dozen lashes each and be spread across the other huts of the camp.
        {succeededStrengthCheck:->4a|->2b}
}

=== 2b ===

How generous of him! Give up our dreams of freedom and submit to the beater's post? What a heap of dung.

    +It is a fair deal, considering you have already slain a guard. They are hardly likely to offer it a second time.
        changeCamTarget({loamIndex})

        What the newling says is true. We aren't making much progress here anyways.
        ->2ba
    +You can think what you like, but that's the only offer I am permitted to give.
        changeCamTarget({loamIndex})

        Maybe we should consider this. We aren't making much progress here anyways.
        ->2ba

=== 2ba ===

changeCamTarget({dezsoIndex})
setNPCFacing({dezsoIndex},NW)

Has your courage broken so quickly? If we stay the course, we may see our families again!

changeCamTarget({loamIndex})

\*Loam doesn't meet Dezso's gaze.* We could also end up dead...

changeCamTarget({dezsoIndex})

Then we'd be just as dead as these poor smears here. The Lovashi have proved they're too soft to risk that: they've resorted to sending these grovelers in their place!

changeCamTarget({loamIndex})

...

changeCamTarget({dezsoIndex})
setNPCFacing({dezsoIndex},SW)

We want no part of such a deal. Not when we're this close to the outside.

    ->3a

=== 2c ===

changeCamTarget({dezsoIndex})

Like we would believe that. The Lovashi aren't so forgiving.

changeCamTarget({loamIndex})

No, but this Tabor has done this before. Did you hear what happened to Awl?

setNPCFacing({dezsoIndex},NW)
changeCamTarget({dezsoIndex})

The tanner's son? Got here a few weeks ago, about yea high?

changeCamTarget({loamIndex})

That's the one. After his first shift, he refused to work. They found him during inspection up in the rafters of his hut, and couldn't get him down.

Eventually they had to call Tabor in to bring him to the ground. Tabor talked with him for a long while, then said he'd give Awl a full pardon if he went back to work.

changeCamTarget({dezsoIndex})

So... what? You think this is genuine?

changeCamTarget({loamIndex})

Could be. Awl doesn't have any whip marks far as I know.

changeCamTarget({dezsoIndex})
setNPCFacing({dezsoIndex},SW)

\*Dezso considers this for a moment.*

    +Tabor will keep to his word, but only if you submit now. If you throw this back in their faces, the Lovashi won't extend this offer a second time.
        ->2ca

=== 2ca ===

changeCamTarget({dezsoIndex})

I loath the logic of your words. *Dezso shakes his head.* Fire take this entire poxy camp, we were so close! So very close!

changeCamTarget({loamIndex})

Until that stone man came out of the ground, my entire form thought we had made it.

    +What stone man?
        ->2cb

=== 2cb ===

changeCamTarget({loamIndex})

\*Loam looks sheepishly to Dezso.*

changeCamTarget({dezsoIndex})

It hardly matters now: we'll be taking the pardon either way. Tell them.

->2cba(true)

=== 2cba(givenPardon) ===

setToTrue(mentionedStoneMan)

changeCamTarget({loamIndex})

Our plan was to tunnel under the wall and out into the forest. We had been hiding and repairing bits of broken tools we smuggled from the mine for days.

changeCamTarget({dezsoIndex})

We got to somewhere under the ditch, we think. That's when this large rock moved up out of the dirt like a swimmer in a pond, and beat a tune into Loam.

changeCamTarget({loamIndex})

This stone creature was the size and vague shape of a large, muscular man. I was lucky that a small part of the tunnel collapsed when it attacked, or I'd have died down there. Dezso and the others pulled me out while it was getting free. Fortunately, it didn't follow us back into the hut.

changeCamTarget({dezsoIndex})

We were discussing what to do next when the guards came in for inspection. We got the better of them, but one of them died in the fighting. We kept the others as hostages to stall for time while we took turns digging another tunnel.

    ->2cc(givenPardon)

=== 2cc(givenPardon) ===


    +How close did you get with the second tunnel?
        Not very far. The first tunnel took us all night.
        ->2cc(givenPardon)
    +This stone thing you saw: do you think you could kill it?
        Maybe. One of us hit it with a pick during the confusion and it didn't like that very much. But it's profanely strong. And the way it moves through earth... we'd be in it's element down there.
        ->2cc(givenPardon)
    +You seriously thought a tunnel would work? 
        I have no doubt it would have. If the stone creature had never appeared, we could have completed the last stretch and made it to the surface outside the camp wall. It would have been a mad dash to the treeline, but after that we'd have been clear. 
        ->2cc(givenPardon)
    +Your luck changed at the worst moment.
        Aye, it certainly did.
        {givenPardon:->2cd|->1hb}

=== 2cd ===


    +But that's all over now. Untie the hostages and then lie on the ground with your hands behind your heads. After they are clear, I'll call the guards in.

        ->savedHostagesFinish

    +<Not Implemented> What if I could remove the stone creature for you?
        ->Close

=== 2d ===

changeCamTarget({loamIndex})

\*Loam blinks.*

changeCamTarget({dezsoIndex})

Like we would believe that. The Lovashi aren't likely to forgive any of us.

changeCamTarget({loamIndex})

Which guard did you say was leading the negotiations?

    +Chief Tabor gave me this offer.
        ->2da

=== 2da ===

setNPCFacing({dezsoIndex},NW)
changeCamTarget({dezsoIndex})

What does it matter?

changeCamTarget({loamIndex})

I've seen Tabor pardon others before. Maybe they mean what they say.

changeCamTarget({dezsoIndex})

You're speaking drivel. We killed a guard! There's no coming back from that!

changeCamTarget({loamIndex})

You were the one who started the brawl, I just followed what you said! Now we're stuck in this shack with two hostages and no way out, and you led us here!

changeCamTarget({dezsoIndex})


My plan would have worked, had the very earth not conspired to thwart us! That stone man knocked your sense from you and now you're grasping in the dark like a fool!

{
-mentionedStoneMan:
    ->2dba
-else:    
    +Stone man? What stone man?
        ->2db
}


=== 2db ===

setNPCFacing({dezsoIndex},SW)
setToTrue(mentionedStoneMan)

Before we were forced to take the guards as hostages, the plan was to dig our way out of here. Just as we were close to finishing the tunnel we were attacked by a massive man made of stone, and forced to come back up to the surface.

We took the guards hostage to buy more time to create a second tunnel, but now you've ruined any chance of that happening.

    ->2dba

=== 2dba ===

    +Do the rest of you submit to the deal?
        ->2dc

=== 2dc ===

changeCamTarget({loamIndex})

\*Loam and the others nod their heads.*

setNPCFacing({dezsoIndex},SW)
changeCamTarget({dezsoIndex})
playAnimation({dezsoIndex},Idle_Front)

The lot of you are traitors, and incompetents to boot. You're forgetting I have the hostages!

    +\*Rush Dezso while his attention is on the others.* <Dex {dexterity}/2>
        {
        -dexterity >= 2:
            ->2de
        -else:
            setToTrue(failedRushDezso)    
            ->hostagesKilled(->combat)
        }
    +Don't do this. If the hostages die, the Lovashi will kill you and may not honor their deal with the others.
        ->2dca
    +Clinging to a failed plan is lunacy. The others can see that, why can't you?
        ->2dd
    +The lot of you can do what you like. I've delivered the message. My work here is done.
        setNPCFacing({dezsoIndex},SW)
        ->2dda

=== 2dca ===

setNPCFacing({dezsoIndex},SW)

What do I care what happens to them? They've sold me out, after everything I've done for them.

    +That's right, you've gambled much to save them. But do not blame them for knowing the fear that all slaves know.
        But now you say I am to shoulder their sentence? When I always knew that same fear? If our suffering was equal but I was the only one who acted, why shouldn't I now equalize our punishment?
        ->2dcba
    +Those in need follow heroes, the desperate abandon them. They are now desperate, but you can still play the role of hero.
        But do they deserve that? I suffered as they did, am I the only one expected to be brave?
        ->2dcba

=== 2dcba ===

    +Forget what they deserve. Look now to yourself: you play with your own life as much as theirs. Do you want to die a coward, or live as a savior?
        ->2dcbb

=== 2dcbb ===

setNPCFacing({dezsoIndex},NW)

\*Dezso looks from you to his hostages, then to Loam. Loam holds his gaze for long moment.*

setNPCFacing({dezsoIndex},SW)
playAnimation({dezsoIndex},OOC_Idle_Front)

\*Finally, Dezso tears his eyes to you and drops his weapon.* I am no savior. But I can do one last right by these ungrateful louts.

    +Maybe you aren't, but I'm certain they will never forget this.

        \*Dezso laughs without mirth.* Are your kidding? They already have.

        ->savedHostagesFinish

=== 2dd === 

setNPCFacing({dezsoIndex},SW)

\*Dezso raises his weapon high.* I'm no lunatic, I'm simply too daft to tell friend from foe. A smarter man wouldn't have relied on these snakes!

->2dda

=== 2dda ===

executeLeaveBodies({hostageOneIndex},{hostageTwoIndex})

I hope the lot of you are pleased. I'll see you at the executioner's block.
    
    +I doubt the Lovashi will let you live that long.
        
        changeCamTarget({weftIndex})

        Guards! Guards! The hostages have been killed!
        ->hostagesKilled(->combat)

=== 2de ===

fadeToBlack()

movePlayer(5,6)

executeLeaveBodies({dezsoIndex})

changeCamTarget({loamIndex})

Thank the Sun. I think he actually meant to do it.

    +You should be thanking <i>me</i>. 
        You're right, of course. Thank you.
        ->2dea
    +Finally, it's over.
        What should we do now?
        ->2dea

=== 2dea ===

    +Untie the hostages and then lie on the ground with your hands behind your heads. After they are clear, I'll call the guards in.
        ->savedHostagesFinish

=== 3a ===

{
-toldNotAllowedToLeave:
+If that is your answer, where does that leave us? 
    Our freedom relies on the guards taking as long as possible to start their attack, and the ambiguity of our position will give us more time. We cannot allow you to go back to your masters.
    ->3b
-else:
+If that is your answer, I will relay it to the guards.
    No, you will not. Our freedom relies on the guards taking as long as possible to start their attack, and the ambiguity of our position will give us more time. You cannot leave.
    ->3b
}

=== 3b ===


    +\*Shout as loud as you can.* Oh Gods! They've killed the hostages! <Combat>
        ->3bb
    +Fine. I won't fight you. Take me as a hostage instead.
        ->3ba
    +That is unfortunate. I had hoped to avoid violence. <Combat>    
        ->hostagesKilled(->combat)
    +If you're threatening me, then you've brought this on yourself. <Combat>    
        ->hostagesKilled(->combat)

=== 3ba ===

changeCamTarget({weftIndex})
setNPCFacing({weftIndex},SE)

\*Weft gives you a worried expression.* Wait a moment. If we submit, then won't the Lovashi be out of options? They'll attack the hut when they realize we failed.

    +I don't care. I'm not killing a fellow branded.
        setToTrue(allowedYourselfToBeTakenHostage)    
        ->hostagesKilled(->deactivateExtras)
    +If that is true, then my hand is forced. <Combat>    
        ->hostagesKilled(->combat)

=== 3bb ===

setToTrue(declaredHostagesDead)
changeCamTarget({dezsoIndex})

\*Dezso's eyes go wide.*

changeCamTarget({outsideGuardsIndex})

The negotiations failed! Get in there!

    ->hostagesKilled(->combat)

=== 4a ===
{
-hostageTakersLeaderPunished:
\*Dezso hangs his head.* To have given up when we were so close... I led the others to this. This is a fitting punishment, in a way. 
    ->2dea
-else:
\*Dezso eyes you like a dog he is uncertain won't bite.* That is very generous... we have no choice but to accept.
    ->2dea
}

=== combat ===

setToTrue(foughtDezsoAndLoam)

{
-failedRushDezso:
    enterCombat({dezsoOnlyFightIndex})
-else:
    enterCombat({dezsoAndSlavesFightIndex})
}

->Close

=== savedHostagesFinish ===

setToTrue(savedHostages)

->deactivateExtras

=== hostagesKilled(->divert) ===

setToTrue(hostagesDead)

    ->divert

=== deactivateExtras === 

fadeToBlack()

deactivate({dezsoIndex})
deactivate({loamIndex})
deactivate({hostageOneIndex})
deactivate({hostageTwoIndex})
deactivate({weftIndex})

updateNPCVisibility()

fadeBackIn(60)
getNewDialogueFromList(7SlaveShackWeft)

->Close

=== Close ===

{
-spokeToTaborAtBeginningOfSituation:
setToTrue(concludedHostageNegotiations)
}

close()

->DONE