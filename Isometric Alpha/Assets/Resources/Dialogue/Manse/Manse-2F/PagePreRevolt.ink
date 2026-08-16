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
VAR nandorIndex = 10
VAR carterIndex = 11
VAR weftOutsideIndex = 12

VAR playerName = ""

VAR hostagesDead = false
VAR metDirectorAfterHostages = false
VAR sentIntoMineByDirector = false 

VAR knowsAboutTheMine = false

VAR toldToAnswerQuestion = false
VAR askedTheDirectorAQuestion = false
VAR knowWhoTheDirectorIs = false
VAR directorMentionedSurvivors = false
VAR beamToldAboutWudra = false

VAR taborMentionedRewardForHostages = false
VAR gasparBroughtToExecution = false

VAR mineLvl3BreachSealed = false
VAR deathFlagOverseerGáspár = false
VAR gasparAddedToParty = false
VAR mineLvl3GuardsInParty = false
VAR mineLvl3CarterAndNandorInParty = false
VAR learnedCampLocationFromCarter = false
VAR nandorMentionedCampLocation = false
VAR finishedBalintsTask = false
VAR nandorSpokeToPlayerAboutDirectorBetrayal = false
VAR acceptedDirectorVidraLetterJob = false
VAR directorMentionedAnnouncement = false

VAR kastorExecutedWeft = false

VAR metBrandedSurvivors = false

VAR toldToFindNandor = false
VAR pageGaveKnife = false
VAR toldToFindCarterByPage = false

VAR knowsCampLocation = false

VAR discussedWithWeftAfterTookMineJob = false

VAR askedAboutDirectorStuckInOffice = false

VAR receivedDirectorsPardon = false
VAR askedDirectorAboutCampLocationAndPardon = false

VAR partyFlagNándor = false
VAR deathFlagNándor = false
VAR deathFlagCarter = false

{
-receivedDirectorsPardon and not acceptedDirectorVidraLetterJob:
    ->receivedDirectorsPardon_1b
-receivedDirectorsPardon:
    ->receivedDirectorsPardon_1a
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

        {
            -kastorExecutedWeft:
            I was told to expect two branded. Wasn't Weft supposed to arrive with you? The Director wants to see the both of you, so go find Weft before he gets angry.
            ->Close
        }

        I see. You're expected, go on in.
        ->enterDirectorsOffice(->1b)

=== enterDirectorsOfficeReturnedFromMine(->divert) ===

setToTrue(receivedDirectorsPardon)

->enterDirectorsOffice(divert)

=== enterDirectorsOffice(->divert) ===

fadeToBlack(true, false)

setToTrue(metDirectorAfterHostages)

movePlayer(-2,-1)
setFacing(NE)
changeCamTarget({directorIndex})

{
-not kastorExecutedWeft:
activate({weftIndex})
}

{
-not receivedDirectorsPardon:
setNPCFacing({directorIndex},NW)
activate({taborIndex})
activate({adelaIndex})
}

{
-receivedDirectorsPardon and (gasparAddedToParty or mineLvl3GuardsInParty) and not deathFlagOverseerGáspár and not directorMentionedAnnouncement:
activate({leftGuardIndex})
activate({rightGuardIndex})
activate({gasparIndex})
activate({adelaIndex})
activate({taborBehindDeskIndex})
setNPCFacing({directorIndex},SW)
}

{
-directorMentionedAnnouncement and not acceptedDirectorVidraLetterJob:
activate({directorIndex})
setNPCFacing({directorIndex},SW)
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

activateQuestStep(The Director,Take time to think.)

The two of you may take the rest of the day off to consider. If you should be brave enough to take my offer, I can have you sent in to the mine immediately. 

->deactivateExtras

=== 5a ===

finishQuest(The Director,true,Job accepted.)
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
        {
            -kastorExecutedWeft:
            The Director is a busy man. He isn't interested in repeating himself. Go find Weft and bring him here before I interrupt the Director's work for you.
            ->Close
        }
        ->alreadySpokeToDirector_1b
    }
    +Nothing, I must be going.
        ->Close

=== alreadySpokeToDirector_1b ===

I will ask him if he is available to receive you.

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

VAR thenYouAreQuiteTheWarrior = "Then you are quite the warrior. I will inform the Director you are here."
VAR returnedFromTheMines = "*Page looks at you in astonishment.* You must have just returned from the mines. You're filthy!"

{
-toldToFindCarterByPage and not nandorSpokeToPlayerAboutDirectorBetrayal:
    {returnedFromTheMines}  Did you find Carter? Is he with you?
        ->pageAskedIfFoundCarter_1a
-nandorSpokeToPlayerAboutDirectorBetrayal:
    ->sealedBreach_1ac
-else:
    {returnedFromTheMines} Did you actually manage to stop those beasts?
        ->sealedBreach_1aa
}

=== sealedBreach_1aa ===

    +I've been to the deepest tunnel and back. They are no longer a problem.
        {thenYouAreQuiteTheWarrior}
        ->sealedBreach_1ab
    +An easier task I've never been given. The Director should have handed me something challenging if he wanted a fair exchange.
        {thenYouAreQuiteTheWarrior}
        ->sealedBreach_1ab
    +I'm here to see the Director, not answer your questions. 
        Of course. I shall inform him that you are here to see him right away.
        ->sealedBreach_1ab

=== pageAskedIfFoundCarter_1a ===

{
    -mineLvl3CarterAndNandorInParty and deathFlagCarter:
        +I did, but he did not endure long enough to escape the mine.
            \*Page stays silent for a moment. She nods slowly.* Your mission was a dangerous one. I just hope it was a painless end.
            ->pageAskedIfFoundCarter_1b
        +I did, but he was a traitor. He forced me to put him down.
            Carter? A traitor? *Shock, followed by resignation colors Page's face.* If what you say is true, then you did what you had to.

            I will inform the Director of this; he awaits your return. Were you able to stop those beasts in the mine?
            ->sealedBreach_1aa
        +I wasn't able to find him. I'm sorry. <Lie>
            setToTrue(liedToPageAboutFindingCarter)

            ->pageAskedIfFoundCarter_1aa
    -mineLvl3CarterAndNandorInParty and not deathFlagCarter:
        +I did, and he is here with me. Would you like for me to introduce you?
            \*Page's face reddens for a moment.* No! That's alright. He just survived such an ordeal, he wouldn't want to be bothered right now. There will be another time, I'm sure. Besides, you and I have other things we should be discussing.
            ->pageAskedIfFoundCarter_1b
    -metBrandedSurvivors:
        +He is not with me now, but worry not. He is still very much alive.
            What? You speak truly? You're a hero, branded. A champion! I am sorry that I have no more to give you than my gratitude, but thank you. Thank you!
            
            But I won't keep you any longer. Surely you are eager to be given your true reward by our master.
            ->pageAskedIfFoundCarter_1b
    -else:
        +I found no trace of him.
            ->pageAskedIfFoundCarter_1aa
}

=== pageAskedIfFoundCarter_1aa ===

\*Page stays silent for a moment. She nods slowly.* It was stupid of me to think he could have survived. Thank you for looking, despite the danger.

->pageAskedIfFoundCarter_1b

=== pageAskedIfFoundCarter_1b ===

The Director is waiting for your report. Were you able to stop those beasts in the mine?

->sealedBreach_1aa

=== sealedBreach_1ab ===

{
-mineLvl3CarterAndNandorInParty and not deathFlagNándor and not deathFlagCarter:
    ->nandorSpeaksUp_1a
}

->enterDirectorsOfficeReturnedFromMine(->sealedBreach_2a)

=== sealedBreach_1ac ===

The Director will see you now. Are you ready?

    +Yes I am.
        ->enterDirectorsOfficeReturnedFromMine(->sealedBreach_2a)
    +Not yet.
        ->Close

=== nandorSpeaksUp_1a ===

fadeToBlack(true,false)

setToTrue(nandorSpokeToPlayerAboutDirectorBetrayal)

activate({nandorIndex})
changeCamTarget({nandorIndex})

fadeBackIn(60)

Wait, I would speak with you before the Director does.

    +Yes? What is it?
        ->nandorSpeaksUp_2a
    +I don't have time for this.
        setFacing(SE)
        ->nandorSpeaksUp_1b

=== nandorSpeaksUp_1b ===

This is important. If you wish to keep Carter and I as your companions you won't continue on to meet the Director.

    +Fine, I will hear you out.
        ->nandorSpeaksUp_2a
    +Like I care. Begone with you.

        fadeToBlack(true,false)

        deactivate({nandorIndex})
        changeCamTarget({pageIndex})
        movePlayerPos(-7,1)
        setNPCFacing({pageIndex},SW)
        setFacing(NE)

        fadeBackIn(60)

        ->sealedBreach_1a

=== nandorSpeaksUp_2a ===

fadeToBlack(true,false)

setNPCFacing({nandorIndex},SE)
movePlayerPos(-6,-4)
setFacing(NW)

fadeBackIn(60)

I don't know the full extent of your business with the Director, and that ignorance makes me nervous. I wish to know about everything that is going on between you.

    +I have a deal for my freedom. He will give me a pardon, in exchange for having secured his mine for him.
        ->nandorSpeaksUp_2b
    +You aren't entitled to that information.
        ->nandorSpeaksUp_2aa

=== nandorSpeaksUp_2aa ===

If you should speak with the Director, then I am going to assume the worst and Carter and I will leave your company. Tell me, so I know you aren't betraying our cause. 

    +I have a deal for my freedom. He will give me a pardon, in exchange for having secured his mine for him.
        ->nandorSpeaksUp_2b
    +I am not going to do that.
        The we've reached an impasse. My conditions are clear: follow through with this and we will consider you an enemy of the revolution.
        ->deactivateExtras

=== nandorSpeaksUp_2b ===

That is ridiculous. The brand is a permanent scar. You can't pardon that, and even if you could, the Lovashi would never allow it. This is clearly a trap of some kind.

    +The Director has been fair to me so far. I don't see any reason not to trust him.
        ->nandorSpeaksUp_2ba
    +You're right. I knew it was too good to be true. 
        ->nandorSpeaksUp_3b

=== nandorSpeaksUp_2ba ===

~nandorMentionedCampLocation = true
Even if he wanted to pardon you, <i>and</i> had some fantastical means of doing so, he still couldn't. This camp does not lie within the Confederation's borders. It is actually a secret, illegal colony erected within the Kingdom of Masons.

So putting aside all of the other reasons he would not pardon you, he can't release you because you would put this camp at risk by simply knowing of its existence. There's just too much at stake. 

    ->nandorSpeaksUp_2baa

=== nandorSpeaksUp_2baa ===

{
-not finishedBalintsTask and not learnedCampLocationFromCarter:
    +How do you know this?
        It is not for me to say. To tell you how I have learned where this camp lies would put a friend of mine in jeopardy. For now, you must ask yourself who you trust more: your fellow slave, or your owner.
        ->nandorSpeaksUp_2baa
}
    +You make a good point. Perhaps I shouldn't meet with him.
        ->nandorSpeaksUp_3b
    +I don't care. I'm going through with this.
        ->nandorSpeaksUp_3a

=== nandorSpeaksUp_3a ===

You're making a foolish mistake, and I won't be a part of it. I hope beyond hope I am wrong, but I suspect this will be the last time we speak to each other in this life. Good luck, and farewell.

->deactivateExtras

=== nandorSpeaksUp_3b ===

Thank you for hearing me out. Let's leave the Manse before the Director suspects something.
->deactivateExtras

=== sealedBreach_2a ===

setNPCFacing({directorIndex},SW)
changeCamTarget({directorIndex})

removeFromParty({carterIndex})
removeFromParty({nandorIndex})

{
-kastorExecutedWeft:
Page has told me that you were successful, but I do not see Weft with you. What became of him?
    +He did not make it. He was killed by the worms. <Lie>
        That is unfortunate. He died a braver death than I'd have expected. But was what Page said true? Were you able to secure the mines against these worm intruders?
        ->sealedBreach_2aa
-else:
Page has told me that you were successful, but I would hear it from you. Were you able to secure the mines against these worm intruders?
    ->sealedBreach_2aa
}

=== sealedBreach_2aa ===

{
-gasparAddedToParty or mineLvl3GuardsInParty:
    ->dealingWithGaspar_1a
}

    ->sealedBreach_2b

=== dealingWithGaspar_1a === 

+I was, but these extra guards of yours bode ill for your willingness to keep your side of our bargain.
    Excellent! And I understand why you would feel that way. Worry not. If you will allow me to demonstrate their purpose, they will not feed your apprehension for longer than they must.
    ->dealingWithGaspar_1b
+The fighting was fierce, but I managed to deal with the worms after a fashion. Any that remain are trapped, and you won't see new ones until you start digging again.
    Masterfully done. If only my guards had shown similar tenacity. Then we may have never lost the mine to begin with. Now, before I bestow upon you your reward, I must quickly deal with another matter.
    ->dealingWithGaspar_1b


=== dealingWithGaspar_1b === 

setToTrue(gasparBroughtToExecution)

Overseer Gáspár, give your report on the state my mine. When will the camp be ready to resume our work?

changeCamTarget({gasparIndex})

Yes, Director. It is my opinion that once we've been sent replacements for the slain slaves, we will be able to resume work at our previous capacity rather quickly.

changeCamTarget({directorIndex})

I see. And if such a shipment should not be forthcoming?

changeCamTarget({gasparIndex})

Sir?

changeCamTarget({directorIndex})

I'm asking what of the branded that were placed under your supervision. If work resumed tomorrow, at what speed would your teams be operating?

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

Yes, you have. Had even a fraction of their number survived, we may have been able to continue with some level of progression before the arrival of the next caravan. Now, we shall be forced to wait for what could be weeks while the required hands are made ready.

playAnimation({leftGuardIndex},Idle_Back)
playAnimation({rightGuardIndex},Idle_Back)
playAnimation({taborBehindDeskIndex},Idle_Front)
playAnimation({adelaIndex},Secondary_Idle_Front)

Overseer Gáspár, for severe dereliction of duty and cowardice in the face of the enemy, I am placing you under arrest. Considering the importance of our mission here, your punishment shall be execution, to be carried out within the hour.

changeCamTarget({gasparIndex})

\*Gáspár says nothing. His face is sullen as he allows himself to be disarmed. The guards take him away without any resistance.*

fadeToBlack(true,false)
removeFromParty({gasparIndex},true)

deactivate({gasparIndex})
deactivate({leftGuardIndex})
deactivate({rightGuardIndex})
deactivate({adelaIndex})
deactivate({taborBehindDeskIndex})

// playAnimation({taborBehindDeskIndex},OOC_Idle_Front)
playAnimation({adelaIndex},OOC_Idle_Front)
changeCamTarget({directorIndex})

fadeBackIn(60)

I apologize for making you wait through that. This meeting should not be about punishing the failures of cowards, but about rewarding you for your display of prowess.

->dealingWithGaspar_1c

=== dealingWithGaspar_1c === 

{// Asterisk on purpose
-wisdom >= 2: 
    *Why make me watch all of that? You could have have handled that at any time, but instead you waited until I was present. <Wis {wisdom}/2>
        Because, I have no doubt the conduct of the soldiers under my command has given you cause to distrust us. It is important to me that you are aware that some Lovashi still hold bravery, competence, and honor in high esteem.
        ->dealingWithGaspar_1c
}

    +No apologies necessary. It was a treat to witness Gáspár get his comeuppance.
        I expected you would enjoy that. Gáspár had some traits that make for a fine soldier, but lacked many others required of a man of quality.
        ->sealedBreach_2bb
    +Let's just get on with it. I'm eager to be rid of this camp.
        I understand. Let us not waste more of your time, then.
        ->sealedBreach_2bb

=== sealedBreach_2b === 

+The fighting was fierce, but I managed to deal with the worms after a fashion. Any that remain are trapped, and you won't see new ones until you start digging again.
    ->sealedBreach_2ba

+You sent me down there to die, but instead I persevered. Now, hold to your end of the bargain and release me. 
    ->sealedBreach_2ba

=== sealedBreach_2ba === 

Masterfully done. If only my guards had shown similar tenacity. Then we may have never lost the mine to begin with. Now for the logistics of your reward.

    ->sealedBreach_2bb

=== sealedBreach_2bb ===

prepItem()

You are the first branded I have ever heard of to be granted a stay of execution. Do not fret, simply because something is unprecedented doesn't mean it is impossible. It will just take a little bit of doing, even for me.

addXP(500)

finishQuest(No Good Deed, true, Pardon me?)

While it was never expected to be necessary, a pardon for a branded was conceived of in the case of exonerating evidence coming to light after a brand was applied. A special seal was devised with the count's mark placed upon it, which when shown would prove a branded innocent of the crime they were accused of.

It's not much, but outside of a miracle there really is no way to heal the brand. I can have some of these seals sent for, and they would arrive within the next few weeks. You would be exempt from labor during that time, of course, and when it arrives you would be transported to the Kingdom of Masons, where you would be released. 

    +Weeks? That is outrageous!
        Your frustration is warranted, but not even I can make this move any faster. I promised you a pardon, branded, not a speedy one.
        ->sealedBreach_2c
    +I have no choice but to accept. Thank you.
        No, thank you. Your freedom may be valuable to you, but I am certainly the one who has come out of this deal the wealthier.
        ->sealedBreach_2c

=== sealedBreach_2c === 

If you consider this deal to have been favorably concluded, then could I interest you in another? You have a new life ahead of you in the Kingdom of Masons, but likely one as a pauper unless you can find some means of employment. Why not start now, and make yourself a bit of coin to enter your new life with?

    ->sealedBreach_2ca

=== sealedBreach_2ca === 
{
    -true:
    +Before I answer, I would know more about my potential employer.
        ->sealedBreach_2cba(->sealedBreach_2ca)
}
{
    -knowsCampLocation:
    +Something has been bothering me about our deal. This camp lies on Mason land, and its existence is a secret I expect you dearly wish kept. Why release me if I have a chance of revealing it to your enemies?
        ->answerAboutCampLocationAndPardon_1a
}
    +I'm through working for you. I will be going now.
        ->sealedBreach_3a
    +I'll hear the job, but I make no promises towards accepting it.
        ->sealedBreach_3b

=== answerAboutCampLocationAndPardon_1a ===

setToTrue(askedDirectorAboutCampLocationAndPardon)
setNPCFacing({directorIndex},NW)

I was not aware of your knowledge of this. *The Director thinks for a moment before speaking.* I shall do you the favor of not inquiring how you learned this. I suspect it would put you in an awkward position.

My position is no less in jeopardy, I'm afraid. You are correct, this camp is far outside the boundaries of the Confederation. Should the Masons learn of its construction, they would surely destroy it. That is something my superiors cannot abide.

But my superiors are not what they were. <i>We</i>, the Lovashi, are not what we were. This conflict has eroded us, and steeped us in a culture not our own. The proud riders of yester-age are now landed lords, ruling over serfs and slaves.

setNPCFacing({directorIndex},SW)

The last war between our peoples left the Masons burnt and broken; starving and scattered. But it did not leave us unhurt either. Surviving that fight has robbed me of any thirst for the next. Each contest between us leaves us more and more alike, learning the ways of the other to better defeat them at arms.

As the cycle grinds on, I am worried that even were victory inevitable, we would be rendered unrecognizable from the steppe-folk we were, once. 

{
-wisdom >= 2:
    +It sounds like you almost want me to give away your secret. <Wis {wisdom}/2>
        Perhaps I do. The thought is not new to me. Should we be pushed from this camp, the lost progress the Lovashi would suffer would be considerable. And I have profited from gambling on your actions in the past. What is another wager? One which I bet little and could gain much.
        ->answerAboutCampLocationAndPardon_1b
-else:
    +What does this have to do with my pardon? 
        You ask why I would pardon you if it put my camp at risk? Perhaps I wish it so. Should we be pushed from this camp, the lost progress the Lovashi would suffer would be considerable. And it would slow our long decline to the mockery of our heritage that we are becoming.
        ->answerAboutCampLocationAndPardon_1b
}

=== answerAboutCampLocationAndPardon_1b ===

VAR askedWhyRiskPardon = false

{
-true:
    +Why are you trusting me with this? You don't think I would betray you?
        \*The Director looks at you with mild confusion.* To who would you betray me? Who would believe you, one of the branded, over me, a hero of the Confederation many times over? No. The only thing I trust is that you are not stupid enough to attempt it.
        ->answerAboutCampLocationAndPardon_1b     
    +You were the last Lovashi I expected to have turned to treason.
        You are not alone in that. It would shame my younger self to no end. But it is only treason to a regime that has already betrayed us in turn. I do what I must to keep my people protected, even from those who wish the same in their hearts but not their actions.
        ->answerAboutCampLocationAndPardon_1b
    +Then why risk revealing yourself by writing your superiors for my pardon papers?
        ~askedWhyRiskPardon = true
        In truth, the pardon was but a facade, meant to explain your freedom to my soldiers and buy me time to approach you about this very topic. You will never be free, not truly, in the eyes of my nephew, Count Kálnoky. The less he knows of this entire affair, the better off the both of us will be.
        ->answerAboutCampLocationAndPardon_1b
}
{
-askedWhyRiskPardon:
    +If the pardon is fake, how will you fulfill your promise that I be escorted to Mason territory?
        I think you are confused. You are already on Mason ground. I had already brought you here before I even made the promise. Your next question should be how we will get you out of the camp without the guards noticing your absence.
        
        But that is a problem we will overcome in due course. I have soldiers I trust to smuggle you beyond the forest, but these things will take time to arrange. But my promise to you still stands: when the time is right, you will be set free.
        ->answerAboutCampLocationAndPardon_1b   
}
    +This job you mentioned. Would its completion hurt the Confederation somehow?
        Not immediately, but it runs counter to their interests. Why?
        ->answerAboutCampLocationAndPardon_1c

=== answerAboutCampLocationAndPardon_1c ===

    +I am interested in any opportunity to stick my thumb in their eye.
        These are still my countryman, branded. Do not expect me to revel with you in harming them.
        ->sealedBreach_3ca
    +I want to know the risks before I accept. Explain what you want me to do and I'll think on it.
        keepDialogue()
        Good. I'm glad to see you will at least consider it.
        ->sealedBreach_3b

=== sealedBreach_2cba(->divert) === 

If you have questions about me, ask them, but know I guard some pieces of information more closely than others.
    ->sealedBreach_2cb(divert)

=== sealedBreach_2cb(->divert) === 

    {
    -true:
    +You're a lord, but you administer a mining camp? That seems somewhat beneath your station.
        That is true. Or it normally would be. Everyone in this world has a master, branded. In having lost yours, I expect you will soon find another. But to give you an answer, my master needed a job done right, so he sent the man he most trusted to see it through.
        ->sealedBreach_2cb(divert)
    +Your guards treat you with a lot of respect.
        They are good soldiers, and I have been leading warriors for the Confederation for a long time. I was the Commander of the Western Lance during our invasion of the Kingdom of Masons; during that time, I had entire hordes answer my commands. 
        
        Such a position makes me the highest ranked officer they are ever likely to meet. Their reverence for me is born of an acknowledgment of the battles I have won, lost, and survived.
        ->sealedBreach_2cb(divert)
    +You mentioned the 'Emancipation Conflict' before. What is that?
        The Emancipation Conflict is the name my people gave to the struggle between yours and mine: the Craft Folk, and the Lovashi. It was born of a grudge from the time of my father's generation, when the king of the Lovashi gave the child of his mount to a Craft Folk sovereign to take for a steed.

        This horse, a prince to my tribe, was mistreated, and died soon after. For this insult, the Lovashi descended on the Craft Folk seeking revenge. In their efforts to keep us at bay, your ancestors stole foals from our camps and raised them as slave mounts. My people share a language with horses; we are kin, in a way. In your ignorance, you could not teach them this language, and in so doing raised them to be simple beasts of burden.

        The horses that the Craft Folk rear are now but feral children, mute animals without sentience or culture. The Conflict is our attempt to right this wrong. The brand is our tool to teach you the weight of your folly... or so my nephew, the count, would claim. He is young, and has seen too little of history to know its cycles.
        ->sealedBreach_2cb(divert)
    +I've never seen you about camp. Why stay cloistered within this office?
        ~askedAboutDirectorStuckInOffice = true
        I grow tired easily in my old age. But even were I still young I would likely not wander idly. Many years ago I suffered a lance through my left leg during the seige of Wudra. It remains set on reminding me of the closest I've come to passing from this life.
        ->sealedBreach_2cb(divert)
    } 
    {
    -beamToldAboutWudra:
    +A servant of yours said you fought the Masons at a place called Wudra.
        Ah, yes, I was there. I lead a Lovashi horde that lay seige to that city.
        ->WudraAnswer_1a(divert)
    -askedAboutDirectorStuckInOffice:
    +Wudra must have been some battle, then. I'd hear you speak of it, if you're willing.
        I like to fixate on it less than my countrymen do, but very well.
        ->WudraAnswer_1b(divert)
    }

    +Your disposition is less violent than some of your subordinates.
        Violence is a tool, branded. The soldiers you speak of have been taught to use that tool. I am old enough to have taught them why they should use it, and forgotten why myself.
        ->sealedBreach_2cb(divert)

    +I'm finished with my questions.
        Then have you come to an answer?
        ->divert

=== WudraAnswer_1a(->divert) ===

    +I've heard that many tales are told of your exploits there. Among the Lovashi, at least. Would you tell me what happened?
        combineDialogue()

        I can give my account, if you'd like. 
        ->WudraAnswer_1b(divert)

=== WudraAnswer_1b(->divert) === 
        Wudra is a city in the Kingdom of Masons, in the kingdom's western riverlands. There is a great river, the Wandering Roil, that meanders through Mason land, and Wudra sits at it's mouth.

        The Confederation, over a decade ago, struck through the Masonic Gap with three great hordes. I was selected from my peers to lead the warriors meant to pacify the kingdom's western half. I believe Wudra captures the imagination of the younger generations because it was the deepest our people have ever cut into Craft Folk territory. To them, what was in truth a bitter defeat has instead become a tale that serves as a metric of our achievements, and a call to surpass them.
        
        It was at Wudra that we were turned back, just barely kept from breaking its walls by the river it straddled. We were a mane's hair from victory, until a Mason host releaved the city from seige at the worst moment. What was to be my opus became a trap I only escaped by the grace of the ficklest of Gods. But They didn't let me leave without dealing me a wound for my hubris that still haunts my left leg.
        ->sealedBreach_2cb(divert)

=== sealedBreach_3a ===

You have earned that right, but should you change your mind the offer stands until you leave this camp.

->sealedBreach_Finished

=== sealedBreach_3b ===

Good. The work you've done so far has rendered me much profit. I'm glad to see you will at least consider continuing it.

    +We are not friends, Director. You repulse me to my core; I am simply in no place to turn down the coin.
        ->sealedBreach_3ba
    +And may it continue to be so, for the both of us.
        ->sealedBreach_3ca
    +The friendlier you act, the more likely I am to reject the job.
        ->sealedBreach_3ba

=== sealedBreach_3ba ===

Yes, of course. I'll skip the pleasantries then.

->sealedBreach_3ca

=== sealedBreach_3caa ===

You've returned. Have you come back to accept my proposal?
    ->sealedBreach_3cab

=== sealedBreach_3cab ===

    +Can you remind me what the job was again?
        ->sealedBreach_3ca
    +Before I answer, I would know more about my potential employer.
        ->sealedBreach_2cba(->sealedBreach_3cab)
    +Yes. Hand over the letter; I will see it to your friend.
        ->sealedBreach_3d
    +There's more going on here than I'm comfortable with. I'll pass.
        ->sealedBreach_3da

=== sealedBreach_3ca ===

I have written a letter to a comrade of mine. He has taken up residence in the town of Rice Hill, in the Kingdom of Mason's northern frontier. I thought that, since you will be going that way anyways, you could deliver it to him for me? For this task, I would give you a small commission up front, and then my friend would gladly pay you the amount of three hundred pieces of gold upon delivery. 

    +What is your friend's name?

        He has changed names many times while I have known him. He is unlikely to have kept what I used for him last. But he will know you as an associate if you call him by his title: 'Vidra'.

        A new branded in town will be quite the novelty to him. Pay for a room at one of the local inns, spend some time out in the open. He certainly will find you.
        ->sealedBreach_3c

=== sealedBreach_3c ===

{
-not askedDirectorAboutCampLocationAndPardon:
    +Why all this secrecy? This hardly seems like your average messenger job.
        My friend is not well liked by the kingdom. He holds no love for the Masons, but neither is he their enemy. Things are simply more convenient for him if discretion is used while attempting contact.

        I can't explain more than that, it isn't my place to betray the secrets of a friend. But should you keep them as well, Vidra will be more inclined to give you answers. Think of it as a test; one on which further employment and rewards are contingent. 
        ->sealedBreach_3c
    +I suspect the Masons would be interested in the correspondences of a Lovashi lord. What is stopping me from bringing this letter to the nearest sheriff once I'm on their land, or reading it myself for that matter?
        Do so if you wish, but know that the letter is written in the horsetongue, and contrived via innuendo. I doubt it would mean much to anyone but my friend.

        And think of the consequences of such a choice: a branded, recently exiled and trusted with a letter from me, giving it up for personal gain? That would be highly suspect. They would see you as a traitor at best and a saboteur at worst. Hardly how one would want to ingratiate themself with their new hosts.
        ->sealedBreach_3c
    +Why not send one of your guards? They would arrive faster, seeing as I cannot even set out for a few more weeks.
        My guards lack the subtlety necessary for such a task, even if they could make the journey quicker. The brand gives you a reason for coming and going that they would lack. Certainty of delivery outweighs haste in this matter.
        ->sealedBreach_3c
}

    +What's the letter say?
    {
        -askedDirectorAboutCampLocationAndPardon:
            ->sealedBreach_3c   
        -else:
            ->sealedBreach_3c
    }

{
-directorMentionedAnnouncement:
    +Before I answer, I would know more about my potential employer.
        ->sealedBreach_2cba(->sealedBreach_3c)
}
    +The job seems simple. I'll do it.
        ->sealedBreach_3d
    +There's more going on here than I'm comfortable with. I'll pass.
        ->sealedBreach_3da

=== sealedBreach_3d ===

setToTrue(acceptedDirectorVidraLetterJob)

prepItem()

It is well you are so amenable. Here is what I promised up front. I expect it will cover food and board on the road and then some.

giveCoins(100)&
giveItem(3,11,1)

->sealedBreach_Finished

=== sealedBreach_3da ===

Unfortunate, but I understand. I will keep the letter ready, should you change your mind{askedDirectorAboutCampLocationAndPardon:.| before you set out.}
    ->sealedBreach_Finished

=== sealedBreach_Finished ===

{
-directorMentionedAnnouncement:
    ->deactivateExtras
}

setToTrue(directorMentionedAnnouncement)
activateQuestStep(Stay Of Execution, Meet the Director.)

VAR unorthodox = "Because of how unorthodox a pardon is for one of the branded, I will need to introduce you to the guards of the camp so they understand you are not to be given work duties, or harrassed."
VAR ready = "When you are ready, make your way there. Until then, farewell."

{
-gasparBroughtToExecution:

{unorthodox} It would be best for that to happen during Gáspár's execution, as much of the camp will be gathered for it. The execution will take place in the unfinished section in the camp's northwest. {ready}

-else:

{unorthodox} I will have them gather in the camp's northwest so they can meet you. {ready}

}

->deactivateExtras

=== receivedDirectorsPardon_1a ===

changeCamTarget({pageIndex})

I've been told the Director is no longer available to meet. You will need to come back another time.

->Close

=== receivedDirectorsPardon_1b ===

changeCamTarget({pageIndex})

The Director is busy, but told me to notify him if you wished to meet.

+Tell him I want to discuss the job we spoke about.
    ->enterDirectorsOfficeReturnedFromMine(->sealedBreach_3caa)
+I must be going.
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
{
-metDirectorAfterHostages and not discussedWithWeftAfterTookMineJob and not mineLvl3BreachSealed:
    fadeToBlack(true, false)
-else:
    fadeToBlack()
}

deactivate({weftIndex})
deactivate({taborIndex})
deactivate({adelaIndex})

deactivate({leftGuardIndex})
deactivate({rightGuardIndex})
deactivate({gasparIndex})

deactivate({carterIndex})
deactivate({nandorIndex})

{
-receivedDirectorsPardon:
deactivate({directorIndex})
}

{
-metDirectorAfterHostages and not discussedWithWeftAfterTookMineJob and not mineLvl3BreachSealed:
setToTrue(discussedWithWeftAfterTookMineJob)

movePlayerPos(-5,-1)
setFacing(SE)
changeCamTarget({weftOutsideIndex})
activate({weftOutsideIndex})

->discussWithWeft_1a

-else:
movePlayerPos(-6,-2)
setFacing(SW)
}

fadeBackIn(60)

->Close


=== discussWithWeft_1a ===

VAR beDamned = "We're likely to die down there, or up here when we return so the Lovashi can save face, promises of a pardon be damned."

fadeBackIn(60)

I didn't think such a thing was possible. A branded, receiving a pardon? I've certainly never heard of it happening before now.

    +I wouldn't get your hopes up. We have to survive the mine first.
        Too true. You're handy in a fight, but I don't expect we will get very far.
        ->discussWithWeft_1b
    +They're just trying to motivate us to solve their problem without risking any of their guards. We're not getting out of here. Not alive.
        I expect you're right. {beDamned}
        ->discussWithWeft_1b
    +I'm just glad to take any lifeline I'm given.
        I'm not certain this <i>is</i> a lifeline. {beDamned}
        ->discussWithWeft_1b
    +If you found it so strange, why did you accept? Even the Director was surprised.
        ->discussWithWeft_1ba

=== discussWithWeft_1b ===

    +Then why did you accept? Even the Director was surprised.
        ->discussWithWeft_1ba

=== discussWithWeft_1ba ===

    {
    -calculateWeftMood() > 0:
        ->discussWithWeft_1c
    -else:
        ->discussWithWeft_1d
    }

=== discussWithWeft_1c ===

\*Weft hesitates before speaking, keeping his voice low.* Because, either way our mission ends, it is an escape from this torturous betweenness I've woven for myself. 

The guards use me as an example of the perfect branded, but my usefulness only earns me a calculating hesitation before their abuses begin. Even Tabor would see me dead after I've learned all I can. And the other branded sharpen knives in their minds when they see me approach.

I did not realize the decisions that earned my bed would also reward me with too much worry to find any rest in it. Even as I reveal this to you I think 'does it matter?' Do they think this is some scheme to drop their guard so I can leave them when the worms are close?

->discussWithWeft_1ca

=== discussWithWeft_1ca ===

    +And all that talk about taking pride in being chosen by the Lovashi? Was that just rubbish?
        I had just met you. I knew not if you were friend, foe, or some test of the masters to keep me on my toes. I am still not entirely sure, but if we are to risk death in the mine together, I believe a revelation like this is warranted.
        ->discussWithWeft_1ca
    +I admit, the thought of a betrayal had crossed my mind.
        I understand. I am not a fighter, but I will do what I can to earn my keep. By the end of this, you will know you can trust me at with your back.
        ->discussWithWeft_DeactivateExtras
    +I can sense the realness of your words. Stick close. We'll get through this together.
        Truly? I am unsure whether you jest, but I will show you through my actions that I am worthy of such trust.
        ->discussWithWeft_DeactivateExtras
    +You sold out your friends, strut around like the lord of the camp, and then weep when you're hated for it? You'll not gain sympathy from me.
        \*Weft shakes his head.* I should have expected such. I won't bother you with these thoughts again.
        ->discussWithWeft_DeactivateExtras

=== discussWithWeft_1d ===

Because you are new, you have not yet learned the importance of maintaining your value to the Lovashi. Let us get on with it.

->discussWithWeft_DeactivateExtras

=== discussWithWeft_DeactivateExtras ===

fadeToBlack()

deactivate({weftOutsideIndex})

{
-not toldToFindNandor:
->pageAsksToFindCarter_1a 
-else:
setFacing(SW)

fadeBackIn(60)

->Close
}

=== pageAsksToFindCarter_1a ===

setToTrue(toldToFindCarterByPage)

VAR concerned = "I've been very worried about a friend who I have not seen return from the mine. He's a branded like you. His name is Carter."

changeCamTarget({pageIndex})
setNPCFacing({pageIndex},SE)
setFacing(NW)

fadeBackIn(60)

Excuse me, but could I speak to you for a moment? The Director informed me before you arrived that he would be asking you to enter the mine for him. I was wondering if you accepted?

    +I have. Why does that interest you?
        You must be very brave to accept going down there. {concerned}
        ->pageAsksToFindCarter_1b
    +That's no concern of yours.
        If you'll forgive me, I know it isn't, but I have to ask. {concerned}
        ->pageAsksToFindCarter_1b

=== pageAsksToFindCarter_1b ===

{
-mineLvl3CarterAndNandorInParty:
    ->pageAsksToFindCarter_1d
}

{
-wisdom >= 2:
    *I would expect the guards would frown on such a friendship. <Wis {wisdom}/2>
        Ok, so we aren't exactly 'friends'. I've seen him work before, from a distance, and I've kept my... appreciation to myself. He probably isn't even aware I exist. But I would just feel terrible if I knew I could have helped him and I said nothing.
        ->pageAsksToFindCarter_1b
}
    +And you want me to see if he's still alive?
        Yes, exactly! If you'll be going down there anyways, I thought maybe you could look into what happened to him for me? It may be naive to hope he's still alive, but with no body we don't know his fate for certain.
        ->pageAsksToFindCarter_1c

=== pageAsksToFindCarter_1c ===

    +I'm not about to do this for nothing.
        setToTrue(pageGaveKnife)
        I understand. I don't have much to give you, but I have a small knife I use to open documents with. I'm sure the Director wouldn't notice if I gave it to you. But only if you promise to bring back Carter! Or proof of what happened to him...
        ->pageAsksToFindCarter_1c
    +If I find his body, I will let you know.
        ->pageAsksToFindCarter_1ca
    +I am already risking my life, I'm not going to risk it further by wandering around in the dark.
        ->pageAsksToFindCarter_DeactivateExtras
    +I will see what became of him for you. 
        ->pageAsksToFindCarter_1ca

=== pageAsksToFindCarter_1ca ===

activateQuestStep(Find Carter, Search the mine.)
setToTrue(acceptedFindCarterQuest)

{
-pageGaveKnife:
prepItem()
}

Thank you, branded. I'm Page, by the way. I will pray for the Gods to bring you and Carter back safe.

{
-pageGaveKnife:
giveItem(2,31,1)
}

->pageAsksToFindCarter_DeactivateExtras

=== pageAsksToFindCarter_1d ===

    +Actually, Carter is alive. He is traveling with me now.
        ->pageAsksToFindCarter_1da

=== pageAsksToFindCarter_1da ===

prepItem()

\*Page looks at you in surprise, and then blushes shyly.* I did not realize. Then my concerns were over nothing. There is no need for us to speak, the guards would begin to get suspicious if we fraternized. I'll just have to introduce myself later.

addXP(200)

->pageAsksToFindCarter_DeactivateExtras

=== pageAsksToFindCarter_DeactivateExtras ===

fadeToBlack()

setFacing(SW)
setNPCFacing({pageIndex},SW)

fadeBackIn(60)

->Close

VAR liedToWeftAboutHearingExtortion = false
VAR weftKnowsYouLiedAboutHearingExtortion = false
VAR insultedWeftAfterHostages = false
VAR blamedWeftForHostageDeath = false
VAR gaveWeftCreditAfterHostages = false
VAR tookBlameForHostageDeath = false

=== function calculateWeftMood() === 

VAR mood = 0

{
-weftKnowsYouLiedAboutHearingExtortion: 
    ~mood--
-liedToWeftAboutHearingExtortion:
    ~mood++
}

{
-insultedWeftAfterHostages: 
    ~mood--
}

{
-blamedWeftForHostageDeath:
    ~mood--
}

{
-gaveWeftCreditAfterHostages or tookBlameForHostageDeath:
    ~mood++
}

~return mood

=== Close ===

{
-nandorMentionedCampLocation:
setToTrue(learnedCampLocationFromCarter)
}

close()

->DONE