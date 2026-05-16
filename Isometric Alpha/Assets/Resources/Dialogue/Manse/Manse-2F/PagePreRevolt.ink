VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerIndex = 0
VAR pageIndex = 1
VAR directorIndex = 2
VAR taborIndex = 3
VAR weftIndex = 4
VAR adelaIndex = 5
VAR leftGuardIndex = 6
VAR rightGuardIndex = 7
VAR gasparIndex = 8
VAR taborBehindDeskIndex = 9

VAR playerName = ""

VAR hostagesDead = false
VAR metDirectorAfterHostages = false
VAR sentIntoMineByDirector = false

VAR gaveWeftCreditAfterHostages = false
VAR tookBlameForHostageDeath = false
VAR blamedWeftForHostageDeath = false

VAR knowsAboutTheMine = false

VAR toldToAnswerQuestion = false
VAR askedTheDirectorAQuestion = false
VAR knowWhoTheDirectorIs = false
VAR directorMentionedSurvivors = false

VAR taborMentionedRewardForHostages = false

VAR mineLvl3BreachSealed = false
VAR deathFlagOverseerGáspár = false
VAR gasparAddedToParty = false
VAR mineLvl3GuardsInParty = false

VAR returnedFromMine = false

{
-sentIntoMineByDirector and mineLvl3BreachSealed:
    ->sealedBreach_1a
-metDirectorAfterHostages and not sentIntoMineByDirector:
    ->alreadySpokeToDirector_1a
-sentIntoMineByDirector:
    ->alreadySpokeToDirector_1a
-else:
    ->1a
}

=== 1a ===

changeCamTarget({pageIndex})

Hello. Are you here to see the Director?

    +Yes, Chief Tabor said I was to report to his office?
        I see. You're expected, go on in.
        ->enterDirectorsOffice(->1b)

=== enterDirectorsOffice(->divert) ===

fadeToBlack(true, false)

setToTrue(metDirectorAfterHostages)

movePlayer(-2,-1)
setFacing(NE)
setNPCFacing({directorIndex},NW)

changeCamTarget({directorIndex})

activate({weftIndex})
activate({adelaIndex})

{
-returnedFromMine:
activate({taborBehindDeskIndex})
-else:
activate({taborIndex})
}

{
-returnedFromMine and (gasparAddedToParty or mineLvl3GuardsInParty) and not deathFlagOverseerGáspár:
activate({leftGuardIndex})
activate({rightGuardIndex})
activate({gasparIndex})
}

fadeBackIn(60)

->divert

=== 1b ===

changeCamTarget({taborIndex})

Director, sir, these are the two branded that Captain Adéla and I spoke of. The ones we used to negotiate for the hostages.

{
-hostagesDead:
    ->hostagesDead_2a
-else:
    ->hostagesSaved_2a
}

=== hostagesDead_2a ===

changeCamTarget({adelaIndex})

And the ones responsible for their deaths.

changeCamTarget({directorIndex})

\*A man, his hair grey, his armor made for someone larger, sits behind a desk. He stares past Adéla at the steppe green and gold of the Lovashi banner that adorns the office wall. After a moment, he speaks.*

I have already heard your account of what happened, captain. I wish to listen to what they have to say.

setNPCFacing({directorIndex},SW)

Captain Adéla wants you dead for your part in the deaths of her guards, but that choice is not hers to make. Chief Tabor lobbies for a pardon, but I am unsure of the affect that will have on this camp's morale. 

->2a

=== hostagesSaved_2a ===

changeCamTarget({directorIndex})

\*A man, his hair grey, his armor made for someone larger, sits behind a desk. He stares past Adéla at the steppe green and gold of the Lovashi banner that adorns the office wall. After a moment, he speaks.*

They have done what you could not it seems, Captain Adéla.

changeCamTarget({adelaIndex})

Yes, sir.

changeCamTarget({directorIndex})
setNPCFacing({directorIndex},SW)

You two have performed admirably. It has made me realize you may be resourceful enough to solve an even greater issue we are plagued with. I have been mulling it over whether to present it to you.

->2a

=== 2a ===

changeCamTarget({directorIndex})

Before I make my decision, I would know you. You are Weft, and you are {playerName}, are you not?

changeCamTarget({weftIndex})

Yes, sir. Weft is my name.

    +And I am {playerName}, sir.
        ->2b
    +That is my name, yes.
        ->2b
    +\*Say nothing.*
        ->taborSaysToAnswerQuestion(->2b)
    +You already know that. Why are you asking me?
        ->taborSaysToAnswerQuestion(->2b)

=== taborSaysToAnswerQuestion(->divert) ===

    ~toldToAnswerQuestion = true

    changeCamTarget({taborIndex})
    setNPCFacing({taborIndex},NW)

    Answer the Director's question, branded.

    changeCamTarget({directorIndex})

    \*The Director holds up a hand.* No, let them answer how they like. I want to understand those I am assigning this duty to.
    setNPCFacing({taborIndex},NE)

    ->divert

=== 2b ===

changeCamTarget({directorIndex})

Your names were but scratches in a ledger to me before. Now, they have faces to them. You've made yourselves real, in a way.

{
-hostagesDead:
->hostagesDead_2b
-else:
->hostagesSaved_2b
}

=== hostagesDead_2b ===

The names of the hostages have been removed from our ledgers, thanks to your actions. Two more names, buried along with their faces. How does that make you feel?

    +Why should I lose sleep over a pair of dead slavers?
        Why would you, indeed.
        ->2c
    +I'm miserable over it, sir.
        Are you? You'd be the first branded that <i>I</i> have met for that to be true.
        ->2c
    +\*Say nothing.*
        {
        -not toldToAnswerQuestion:
            ->taborSaysToAnswerQuestion(->2c)
        -else:
            Silence, perhaps, was the only answer. What words could you say that would satisfy me? What do I really want from you?
            ->2c
        }

=== hostagesSaved_2b ===

setToTrue(knowWhoTheDirectorIs)
keepDialogue()
I am Lord Gábor Kálnoky, uncle to Count Béla Kálnoky. Now that we are known to each other, allow me to cut to the reason I have summoned you here. The camp is stuck in lockdown while the mine is closed. Are you aware of why?

->2c

=== 2c ===

I'll cut to the reason why I've called for you. The camp is stuck in lockdown while the mine is closed. Are you aware of why?

{
-knowsAboutTheMine:
    +There are creatures inside that you haven't been able to remove.
        setNPCFacing({directorIndex},NW)
        \*The Director lifts an eye-brow, then looks to Captain Adéla.* Your lockdown is not as air-tight as we had believed, captain.

        changeCamTarget({adelaIndex})

        \*Adéla glares at you wordlessly.*

        setNPCFacing({directorIndex},SW)
        changeCamTarget({directorIndex})

        ->2d
}
    +I am not.
        Good, I had suspected not.
        ->2da
    +\*Say nothing.*
        {
        -not toldToAnswerQuestion:
            ->taborSaysToAnswerQuestion(->2da)
        -else:
            ->2da
        }

=== 2da ===

setToTrue(knowsAboutTheMine)

A few days ago, the mine's lowest level was invaded by a swarm of creatures my guards are calling 'worms'. They came out of one of the shafts the dig teams were excavating. We believe they were living in a cavern, or 'pocket', deep below the surface. 

When the worms had overwhelmed the guards on the bottom floor, the rest of them evacuated the mine and closed the gate to that floor to keep the worms from making any more progress. And that's when the lockdown began, so that we have order while the dig teams are unable to return to work.

->2d

=== 2d ===

Work cannot continue until the worms are dealt with, and that cannot happen until the pocket they came from has been stoppered. That is the task I have for you: enter the mine, plug the tunnel, and return. 

changeCamTarget({weftIndex})

Yes, Director. On your orders we wi-

changeCamTarget({directorIndex})

\*The Director gestures for Weft to be silent.* You are not being commanded to do this. Rather, you are being given a choice.

changeCamTarget({adelaIndex})
setNPCFacing({adelaIndex},SE)

Sir?

changeCamTarget({directorIndex})

I am a veteran of the Emancipation Conflict. Many battles have I fought against the Craft Folk where they ordered soldiers to die so that others may gain some advantage. 

We are not like them. To execute a person for a crime is one thing. To expect those below you to march cheerily to their death is quite another. 

Your choice is this, branded: brave the gauntlet of the mine to seal the tunnel that delivered these worm-things to us, and I shall grant you freedom and safe passage to Mason lands. 

{
-hostagesDead:
Or live as a slave to the Confederation, with Tabor's pardon for your failures. You won't be harmed for any previous crimes, but without another offer of freedom no doubt the captain will find her cause to punish you eventually.
-else:
Or you may return to your hut with a belly full of food and the rest of the day off... but still a slave, never again to have a chance at a life not filled with toil.

}

Before you make this choice, surely you have questions about what I have said. Ask them. 

    ->2e(->3a)

=== 2e(->divert) ===

{
-knowWhoTheDirectorIs and wisdom > 2:
    +If you are a lord, why are you running a mine? That seems beneath you. <Wis {wisdom}/2>
        An astute observation. Normally, it would be. Perform this task for me and I will provide you with your answer.
        ->2e(divert)
}

{
-not hostagesDead:
    +I save two of your guards, and I am rewarded with a suicide mission? Is this some shallow Lovashi jest?
        ~askedTheDirectorAQuestion = true
        You have been rewarded with a chance no other branded of this camp will ever be given, and one which you are free to turn down. Do not mistake this kindness for mockery.
        ->2e(divert)
-else:
    +I thought the brand was a death sentence. Who are you that you have the power to commute it?
        ~askedTheDirectorAQuestion = true
        setToTrue(knowWhoTheDirectorIs)
        I am Lord Gábor Kálnoky, uncle to Count Béla Kálnoky, who rules from Pharos. A letter to my nephew would be all it would take to absolve you of your crimes.
        ->2e(divert)
}

    +Clearly, this will be dangerous or you would have done it already.
        ~askedTheDirectorAQuestion = true
{
-not hostagesDead:
Unbelievably so. You will almost certainly perish in the attempt. Otherwise, I would not have made the reward so dear. 
-else:
Unbelievably so. You will almost certainly perish in the attempt. Otherwise, I would not have made the reward so dear. Should you decide to do this, Captain Adéla will likely have been granted her request for execution.
}
         ->2e(divert)
    +What can I expect when I enter the mine?
        ~askedTheDirectorAQuestion = true
        setToTrue(directorMentionedSurvivors)
        The worms are reported to be large. Some are the size of men, those that fought the creatures have said, although I am uncertain of whether to believe them. Some can spit acid, bite so hard they can crush stone, or split when cut down and rise again attacking from multiple directions.

        There may also be survivors inside the mine, although I doubt it. Many of the bodies of our dead went unclaimed in the evacuation, so we cannot be certain of their fate, but I believe those left behind have become food for their assailants.
        ->2e(divert)
    +How would I close the breach?
        ~askedTheDirectorAQuestion = true
        The overseers of my mine employ blasting jelly to remove rubble. A barrel of such material would be placed at your disposal, and you would be trained in it's use.
        ->2e(divert)
    +Can I choose other slaves to come with us?
        ~askedTheDirectorAQuestion = true
        Any slaves assigned to your work party will be permitted to enter the mine with you, for the same reward.
        ->2e(divert)
    +I have no {askedTheDirectorAQuestion:more }questions.
        ->divert

=== 3a ===

Then what is your decision?

    +You Lovashi love your carrots and your sticks. You can shove both up your ass.
        ->3b
    +Can I have time to think on this?
        ->3c
    +I shall perform this task. How should I start?
        ->5a

=== 3b ===

changeCamTarget({taborIndex})
setNPCFacing({taborIndex},NW)
playAnimation({taborIndex},Idle_Back)

Such disrespect shall not be borne! 

disableDialogueUI()

playAnimation({taborIndex},Attack_Normal_Back)

wait(.625)
playDelayedSFX(Whip, 10)

playAnimation({playerIndex},OOC_Wounded_Back)
dealDamageSafe({playerName},25)

wait(2)

enableDialogueUI()

changeCamTarget({directorIndex})

That will do, Tabor. I am not yet so fragile that I can be harmed by the words of an uppity slave.

setNPCFacing({taborIndex},NE)
playAnimation({taborIndex},OOC_Idle_Back)

changeCamTarget({taborIndex})

Yes sir.

changeCamTarget({directorIndex})

->3c

=== 3c ===

activateQuestStep(No Good Deed,Take time to think.)

The two of you may take the rest of the day off to consider. If you should be brave enough to take my offer, I can have you sent in to the mine immediately. 

->deactivateExtras

=== 5a ===

activateQuestStep(No Good Deed,Make for the stockhouse.)

setToTrue(sentIntoMineByDirector)

You are brave, branded. I've met few who would have made that choice. And what of you, Weft? What choice do you make?

changeCamTarget({weftIndex})

I will also enter the mines, sir. 

changeCamTarget({directorIndex})

Really? I had expected you to ask permission to cower back in your hut. Very well, you may both go together.

prepItem()

Take this. It will prove you are on an important task to my guards. Go to the camp's stockhouse, just north of the mine in the camp's southwest. Tell Quartermaster Emese that you are to be taught to use blasting jelly. You'll need it to close the pocket.

giveItem(3,9,1)

->deactivateExtras

=== alreadySpokeToDirector_1a ===

changeCamTarget({pageIndex})

Yes? What is it?

    {
    -not sentIntoMineByDirector:
    +The Director told me I should come back when I had an answer for him.
        ->alreadySpokeToDirector_1b
    }
    +Nothing, I must be going.
        ->Close

=== alreadySpokeToDirector_1b ===

I'll ask him if he is available to receive you.

fadeToBlack(true, false)

setToTrue(metDirectorAfterHostages)

movePlayer(-2,-1)
setFacing(NE)
setNPCFacing({directorIndex},SW)

activate({weftIndex})

changeCamTarget({directorIndex})

fadeBackIn(60)

keepDialogue()

Have you made your decision?

->alreadySpokeToDirector_1c

=== alreadySpokeToDirector_1c ===

Then what is your decision?

    +I had more questions to ask you.
        Ask them.
        ->2e(->alreadySpokeToDirector_1c)
    +I need more time to think about this.
        ->deactivateExtras
    +I shall perform this task. How should I start?
        ->5a

=== sealedBreach_1a ===

changeCamTarget({pageIndex})

\*Page looks at you in astonishment.* You've just returned from the mines, if the dust you're caked in is any indication. Did you actually manage to stop those beasts?

    +I've been to the deepest tunnel and back. They are no more.
        ->sealedBreach_1aa
    +An easier task I've never been given. The Director should have given me something challenging if he wanted a fair exchange.
        ->sealedBreach_1aa
    +I'm here to see the Director, not answer your questions. 
        ->sealedBreach_1ab

=== sealedBreach_1aa ===

Then you are quite the warrior, to have accomplished such a feat. I will inform the Director you are here. He will certainly wish to speak with you immediately.

setToTrue(returnedFromMine)

    ->enterDirectorsOffice(->sealedBreach_2a)

=== sealedBreach_1ab ===

Of course. I shall inform him that you are here to see him right away.

setToTrue(returnedFromMine)

->enterDirectorsOffice(->sealedBreach_2a)

=== sealedBreach_2a ===

setNPCFacing({directorIndex},SW)
changeCamTarget({directorIndex})

Page informs me that you were successful, but I would hear it from you. Were you able to secure the mines against these worm intruders?

{
-gasparAddedToParty or mineLvl3GuardsInParty:
    ->dealingWithGaspar_1a
}

    +Sup
        ->Close

=== dealingWithGaspar_1a === 

+I was, but these extra guards of yours bode ill for your willingness to keep your side of our bargain.
    Excellent! And I understand why you would feel that way. Worry not. If you will allow me to demonstrate their purpose, they will not feed your apprehension for longer than they must.
    ->dealingWithGaspar_1b
+The fighting was fierce, but I managed to deal with the worms after a fashion. Any that remain are trapped, and you won't see new ones until you start digging again.
    Masterfully done. If only my guards had shown similar tenacity. Then perhaps we would have never lost the mine to begin with. Now, before I bestow upon you your reward, I must quickly deal with another matter.
    ->dealingWithGaspar_1b


=== dealingWithGaspar_1b === 

Overseer Gáspár, give your report on the state my mine. When will the camp be ready to resume our work?

changeCamTarget({gasparIndex})

Yes, Director. It is my opinion that once we've been sent replacements for the slain slaves, we will be able to resume work at our previous capacity rather quickly.

changeCamTarget({directorIndex})

I see. And if such a shipment should not be forthcoming?

changeCamTarget({gasparIndex})

Sir?

changeCamTarget({directorIndex})

I'm asking what of the branded that were placed under your supervision? Should work resume tomorrow, at what speed will your teams be working?

changeCamTarget({gasparIndex})

\*Gáspár pauses uncomfortably for a moment.* No branded escaped the worms, sir. The work cannot continue until they are replaced.

changeCamTarget({directorIndex})

And to conclude your report on our current situation, who ultimately is responsible for abandoning these branded to their fates once the worms appeared? Before you answer, bear in mind I've already heard the reports of your subordinates.

changeCamTarget({gasparIndex})

But sir, once the gate was closed to the bottom level, my first concern was the safety of the guards under my care. It was unclear how long we would be trapp-

changeCamTarget({adelaIndex})

The Director asked you a question, overseer. You will keep your tone level while you answer it.

changeCamTarget({gasparIndex})

Yes ma'am. *Gáspár inhales deeply, steadying himself before answering.* I am responsible for the deaths of the branded. They were given to me to protect, and I failed in that duty. 

changeCamTarget({directorIndex})

Yes, you have. Had even a fraction of their number survived, we may have been able to continue with some level of progression before the arrival of the next caravan. Now, we shall be force to wait for what could be weeks while the required hands are made ready.

playAnimation({leftGuardIndex},Idle_Back)
playAnimation({rightGuardIndex},Idle_Back)
playAnimation({taborBehindDeskIndex},Idle_Front)
playAnimation({adelaIndex},Idle_Front)

Overseer Gáspár, for severe dereliction of duty and cowardice in the face of the enemy, I am placing you under arrest. Considering the importance of our mission here, your punishment shall be execution, to be carried out within the hour.

changeCamTarget({gasparIndex})

\*Gáspár says nothing. His face is sullen as he allows himself to be disarmed. The guards take him away without any resistance.*

fadeToBlack(true,false)
removeFromParty({gasparIndex},true)

deactivate({gasparIndex})
deactivate({leftGuardIndex})
deactivate({rightGuardIndex})
playAnimation({adelaIndex},OOC_Idle_Front)

playAnimation({taborBehindDeskIndex},OOC_Idle_Front)
playAnimation({adelaIndex},OOC_Idle_Front)

fadeBackIn(60)

I apologize for making you wait through that. This meeting should not be about admonishing the failures of cowards, but about rewarding you for your display of prowess.

->dealingWithGaspar_1c

=== dealingWithGaspar_1c === 

{// Asterisk on purpose
-wisdom >= 2: 
    *Why make me watch all of that? You could have have handled that at any time, but instead you waited until I was present. <Wis {wisdom}/2>
        ->dealingWithGaspar_1c
}

    +No apologies necessary. It was a treat to witness Gáspár get his comeuppance.
        ->Close
    +Let's just get on with it. I'm eager to be rid of this camp.
        ->Close

=== deactivateExtras ===

{
-not hostagesDead and not taborMentionedRewardForHostages:
setNPCFacing({taborIndex},NW)
changeCamTarget({taborIndex})
setToTrue(taborMentionedRewardForHostages)
setToTrue(haveQuestForRewardFromTabor)

activateQuestStep(Tabor's Reward,Meet with Quartermaster Emese)

I have informed Quartermaster Emese that you are to be given the reward I promised you. You can find her in the stockhouse next to the mine entrance, in the southwest corner of the camp.

}

fadeToBlack()

deactivate({weftIndex})
deactivate({taborIndex})
deactivate({adelaIndex})

deactivate({leftGuardIndex})
deactivate({rightGuardIndex})
deactivate({gasparIndex})

movePlayerPos(-6,-2)
setFacing(SW)

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE