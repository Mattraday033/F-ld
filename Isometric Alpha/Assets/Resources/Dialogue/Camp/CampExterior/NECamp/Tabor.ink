VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerName = ""

VAR spokeToTaborAtBeginningOfSituation = false

VAR taborIndex = 1
VAR adelaIndex = 2
VAR weftIndex = 3
VAR guardIndex = 4

VAR hostageTakersStandardPunishment = false
VAR hostageTakersNoPunishment = false
VAR hostageTakersLeaderPunished = false
VAR hostageTakersLaborPunishment = false

VAR gaveWeftCreditAfterHostages = false
VAR tookBlameForHostageDeath = false
VAR blamedWeftForHostageDeath = false

VAR mentionedStoneMan = false
VAR hostageSituationGuardsLeft = false
VAR insultedWeftAfterHostages = false

VAR hostagesDead = false
VAR declaredHostagesDead = false
VAR concludedHostageNegotiations = false


{
-concludedHostageNegotiations:

    activateQuestStep(No Good Deed,Meet Tabor.)

    {
    -hostagesDead:
        finishQuest(A Situation Brews,true,The hostages were saved.)
        ->setUpSpeakers(->4a)
    -else:
        finishQuest(A Situation Brews,true,The hostages were killed.)
        ->setUpSpeakers(->2a)
    }
-spokeToTaborAtBeginningOfSituation:
    ->alreadySpokeToTabor
-else:
    ->setUpSpeakers(->1a)
}

=== setUpSpeakers(->divert) ===

changeCamTarget({taborIndex})
setNPCFacing({taborIndex},SW)
setNPCFacing({adelaIndex},NE)

disableDialogueUI()

fadeToBlack(true, false)

setToTrue(spokeToTaborAtBeginningOfSituation)

activate({weftIndex})
movePlayer(-9,-11)
setFacing(NW)

setNPCFacing({taborIndex},SE)
setNPCFacing({adelaIndex},SE)

fadeBackIn(60)
enableDialogueUI()

->divert

=== 1a ===

The situation is this: inside the hut to my left is a group of branded who have taken a squad of guards hostage. We have confirmed they have killed at least one of them already.

changeCamTarget({adelaIndex})

The strange part is that the branded don't seem keen to negotiate. We only discovered this plot when my men didn't come back from their morning inspection, and any attempt to create a dialogue has ended in stalemate. 

What use are hostages if they aren't used for leverage? Something is wrong here. They're stalling, but we aren't certain for what reason.

changeCamTarget({weftIndex})

Those leeches! They'll regret this soon, I'm sure.

    +Where do we come into this?
        ->1b
    +Shut it, Weft.

    \*Weft acts like you didn't say anything, but he doesn't say anything else either.*

        ->1b

=== 1b === 

changeCamTarget({taborIndex})

We don't think we can carry the entrance of the hut without the branded killing the prisoners, and we haven't had much luck negotiating with them from out here.

changeCamTarget({adelaIndex})

So we've decided to force the issue. You two are going to go in there and negotiate for us. I doubt they'd let a guard, even one who is unarmed, enter the hut without taking them hostage as well.

    ->1c

=== 1c ===

{
-wisdom >= 2:
    +You're using us because we can be risked without giving them another bargaining chip. <Wis {wisdom}/2>
        
        changeCamTarget({adelaIndex})

        \*Adéla smirks.* Well spied. You're expendable. Don't you ever forget that.
        ->1c
}
    +How many of them are there?
        changeCamTarget({adelaIndex})

        About a half a dozen or so. You're to avoid combat, of course, and not just because they're likely to tear you to pieces if you attempt it.
        ->1c
    +What happens if the hostages die. Are you going to blame us?
        changeCamTarget({taborIndex})

        No, I was the one who suggested using the two of you to negotiate on our behalf. If the hostages die, I will take the blame from the Director.

        changeCamTarget({adelaIndex})

        I, on the other hand, will very much blame you if any more of my men die. Their lives mean much more to me than either of yours do.

        setNPCFacing({taborIndex},SW)
        changeCamTarget({taborIndex})

        Captain Adéla, I forbid you from performing any retribution against these slaves, regardless of the outcome. Their punishments fall to me, and me alone.

        changeCamTarget({adelaIndex})
        setNPCFacing({adelaIndex},NE)

        Tabor, I advise you to reserve your orders for those you outrank. 
        
        changeCamTarget({taborIndex})

        I am the Chief Correctional Officer. I am the one who determines how we reprimand the slaves and for what reasons. You may be my superior, but the Director has given me this role and he will hear about anyone who encroaches on it.
        
        changeCamTarget({adelaIndex})
    
        \*Adéla holds Tabor's gaze, but says nothing.*

        setNPCFacing({adelaIndex},SE)
        setNPCFacing({taborIndex},SE)
        changeCamTarget({taborIndex})

        \*Tabor turns back to you.* You two should act as you believe is best. You can go where we cannot, and that utility is what we are forced to rely upon.

        ->1c
    +If they attack us, are you going to come charging in to save us?

        changeCamTarget({taborIndex})

        If we hear combat, we won't act until we are certain the hostages have been killed. So when you go in there, you will be on your own.
        ->1c
    +What do we have to offer them? Will you kill them if they surrender?

        changeCamTarget({taborIndex})

        I am prepared to offer them only a dozen lashes each, and they will be moved to separate huts, but they will get to live.
            ->1d

=== 1d ===

    +What guarantee would they have that you would keep your word?
        changeCamTarget({taborIndex})
        These branded have been prisoners here for many weeks. They will be used to my conduct. I expect they will believe whatever offer you give them is genuine, if they know it comes from me.

        setNPCFacing({adelaIndex},NE)
        changeCamTarget({adelaIndex})

        That is also why they feel free to act out. They know the ways you are soft, and how to work you.
        ->1d
    +If you give them the choice between punishment and freedom, they will always choose the latter.
        ->1da
    +That seems reasonable, given what they've done.
        setToTrue(hostageTakersStandardPunishment)
        ->afterDecidedPunishment

=== 1da ===

setNPCFacing({adelaIndex},SE)
changeCamTarget({taborIndex})

You are meant to be negotiating with the hostage-takers, not with me.

changeCamTarget({adelaIndex})

Ha! This branded seems like a handful, even for you, Tabor.

changeCamTarget({taborIndex})

What do you propose instead?

    +If you prize the lives of your comrades over making examples of these branded, then promise them no punishment. That will maximize your chances of saving the lives of your men. <nobr><Cha {charisma}/2></nobr>
        {
        -charisma >= 2:
            ->1da_CharismaSuccess
        -else:
            ->1da_CharismaFailure
        }

    +Offer clemency to all but their leader. The others will sell them out and you will have removed the one who caused all of this.

    changeCamTarget({taborIndex})

    An interesting proposal. Would that satisfy you, captain?

    changeCamTarget({adelaIndex})

    So long as you promise to use their leader for one of your 'teachable moments'. I want the camp to know what happens if you foment revolt. 

    changeCamTarget({taborIndex})

    Agreed. I am content with this; the leader will publically bear the others' punishment, and the rest will be split up among the other huts.
    setToTrue(hostageTakersLeaderPunished)
        ->afterDecidedPunishment
    +Quote them no lashes but use them for labor after they have submitted. The reason they were able to rebel is because they had the necessary time and energy for it during this lockdown. 

    changeCamTarget({weftIndex})
    changeNPCFacing({weftIndex},SW)

    \*Weft keeps his face inscrutable.* Your mind is... certainly inclined to thinking like a slaver must.

    changeCamTarget({taborIndex})

    I dislike this solution. What is the difference between this and saying we shall not hurt them and then marching them to the flogging post anyways?

    setNPCFacing({adelaIndex},NE)
    changeCamTarget({adelaIndex})

    Are we against using the branded for labor now? If they expect not to work then they are deluded fools.

    changeCamTarget({taborIndex})

    Fine, we shall not punish them, but move them higher in the labor queue.

    changeCamTarget({adelaIndex})

    To the front.

    changeCamTarget({taborIndex})

    ... to the front of the labor queue.

    setToTrue(hostageTakersLaborPunishment)

        ->afterDecidedPunishment
    +Perhaps you were right. I rescind my statement.

        setToTrue(hostageTakersStandardPunishment)

        ->afterDecidedPunishment

=== 1da_CharismaSuccess ===

setToTrue(hostageTakersNoPunishment)

changeCamTarget({adelaIndex})

The lives of my men are paramount. I regret that circumstance lends advantage to leniency, but my duty to them is to not risk their safety any more than it already has been.

setNPCFacing({adelaIndex},NE)

I will agree to this, but I want it known that I believe this approach will encourage the branded to rebel again in the future.

changeCamTarget({taborIndex})
setNPCFacing({taborIndex},SW)

I shall agree to this as well. But should what you say prove true, we will just have to be ready for it.

changeCamTarget({adelaIndex})

Aye. We will be vigilant.

->afterDecidedPunishment

=== 1da_CharismaFailure ===

changeCamTarget({adelaIndex})

I see such an offer as total capitulation on our part. We will not be made to seem weak in the eyes of our slaves. 

->1d

=== alreadySpokeToTabor ===

Be careful in there. They're backed into a corner, so they'll lash out if provoked.

->Close

=== afterDecidedPunishment ===

activateQuestStep(A Situation Brews,Negotiate.)

setNPCFacing({taborIndex},SE)
setNPCFacing({adelaIndex},SE)

changeCamTarget({taborIndex})

With our position clear, it's time for you to enter the lion's den. We have already informed the branded inside that you were sent for, so they will be expecting you.

When you are inside, assess the condition of the hostages. We believe that two of them should still be alive, but if they have already been killed, you are free to leave and we will storm the hut after you are clear. 

May the Gods be with you, and the hostages.

fadeToBlack()

deactivate({weftIndex})
deactivate({guardIndex})

setNPCFacing({taborIndex},SW)
setNPCFacing({adelaIndex},NE)

fadeBackIn(60)

->Close


=== 2a ===

changeCamTarget({taborIndex})

prepItem()

The hostages are safe, as are the both of you. Exceedingly well done.

addXP(550,1)

changeCamTarget({adelaIndex})

Even I am impressed. No plan of mine accounted for a way forward without bloodshed.

    +None of us wanted unnecessary killing. I just proved to each party it was true.
        ->2c
    +They were at a cliff's edge and balked at the height. The cowards were glad for a way down.
        changeCamTarget({adelaIndex})

        That sounds right to me. Branded are always underestimating the hardness of my guards, to their own hazard.
        ->3a
    +This success couldn't have happened without Weft. His words swayed them to peace.
        ->2b
    +We were fortunate they believed your offer. Enough doubt and the dealings would have floundered.
        ->2c

=== 2b ===

setToTrue(gaveWeftCreditAfterHostages)
setNPCFacing({weftIndex},SW)
changeCamTarget({weftIndex})

\*Weft looks at you questioningly.*

changeCamTarget({taborIndex})

That is good to hear. He has served us well before, and I have no doubt will continue to for a long while.

setNPCFacing({weftIndex},NW)

->3a

=== 2c ===

    changeCamTarget({taborIndex})

    This is why rapport with the branded is so important. They're a calculating bunch; you need to get through to them that working with us is how they survive, not against us. 
    ->3a

=== 3a ===

changeCamTarget({taborIndex})

The branded you negotiated with were attempting to dig a tunnel out of the camp: hence they needed the hostages to prevent us from attacking them before they could finish.

The fools thought it would actually succeed. They must have underestimated the distance they would have needed to dig. Even if they had gotten past the walls, the archers in the arrow towers would have picked them off with ease.

{
-mentionedStoneMan:
    +One of them mentioned a 'man made of stone' that attacked them while they were in the tunnel. It kept them from completing it.
        ->stoneManExplanation(->3b)
    +\*Say nothing.*
        ->3b
-else:
    ->3b
}

=== stoneManExplanation(->divert) ===

changeCamTarget({adelaIndex})

A stone saint, surely. They're spirits of the rock that sometimes appear in mines or canyons. We've been lucky not to run into any yet while we dig, but I guess we'll need to be ready if there are more around.

changeCamTarget({taborIndex})

Stone saints are nasty business. It probably gave the branded a good scare when it came out of the dirt, and was more than the lot of them could handle.

    ->divert

=== 3b ===

prepItem()

Whatever the case, you two have earned a reward for a job well done. I can give you the rest of the rations I keep on me right now, and once I inform the Director of what has happened I will ask him to approve something extra.

giveItem(0,0,5)

changeCamTarget({adelaIndex})
setNPCFacing({adelaIndex},NE)

You spoil them, Tabor. The other branded will think these two your children if you give them much more.

changeCamTarget({taborIndex})
setNPCFacing({taborIndex},SW)

Good behaviour should be as visibily rewarded as bad behaviour is punished, Captain. You know how the branded are: the baser comforts can be a better motivator to them than a hundred lashes.

->3c

=== 3c ===

setNPCFacing({taborIndex},SE)
setNPCFacing({adelaIndex},SE)

changeCamTarget({taborIndex})

We must give our reports to the Director. Once that is done, I will be back to give you two your next task. Take your midday break while I am up in the Manse, then come find me in front of Weft's hut when it is over.

->deactivateGuards

=== 4a ===

changeCamTarget({taborIndex})

prepItem()

The hostages are dead, and so are the plotters. A worse outcome is hard to imagine.

addXP(450,1)

changeCamTarget({adelaIndex})
setNPCFacing({adelaIndex},NE)

I was against bringing the branded in from the start. I can't believe you thought they were equipped to handle this.

changeCamTarget({taborIndex})
setNPCFacing({taborIndex},SW)

Captain, with respect, no other way forward either of us proposed gave a better chance to the hostages for survival. They all involved some means of attack, which would have guaranteed their demise.

changeCamTarget({adelaIndex})

But now we've involved these branded here in our plans, and they've seen how we conduct ourselves: poorly. A branded who believes us incompetent...

setNPCFacing({adelaIndex},SE)

... is one prone to rebellion.

changeCamTarget({weftIndex})

\*Weft shivers.*

changeCamTarget({adelaIndex})
setNPCFacing({adelaIndex},NE)

Chief Tabor, you and I will both brief the Director on what has happened here. And I will be certain to give him my recommendations for how these two branded will be punished for their failure.

    +Captain Adéla, may I humbly request to speak?
        ->4b
    +\*Say nothing.*
        ->3c

=== 4b ===

setNPCFacing({adelaIndex},SE)
changeCamTarget({adelaIndex})

\*Adéla's eyes glint maliciously.* Yes, please do. Make this worse for yourself.

{
-mentionedStoneMan:
    +I feel it is my duty to inform you that during the negotiations, one of the branded mentioned being attacked by a stone man.        
        ->stoneManExplanation(->3c)
}

    +Weft took charge of the negotiations. I tried to prevent him from ruining everything, but he couldn't help himself.
        changeCamTarget({weftIndex})
        setNPCFacing({weftIndex},SW)

        setToTrue(blamedWeftForHostageDeath)

        \*Weft looks at you with horror.* That's not true! I barely said a word!
        
        changeCamTarget({adelaIndex})

        Pathetic. Just what I'd expect from the both of you: tripping over yourselves to blame the other.
            ->3c

    +I was the one whose words led to the death of the hostages. Weft should not be considered at fault for what were the results of my actions.
        setToTrue(tookBlameForHostageDeath)
        I see. Valiantly put, slave. I will add that to my report to the Director.
            ->3c
    +\*Say nothing.*
        That's what I thought.
            ->3c

=== gaveWeftCreditConvo_1a ===

I don't understand. Why did you praise me to Adéla and Tabor?

{
-insultedWeftAfterHostages:
    +This feuding between us is getting us nowhere. Think of it as an olive branch.
        \*Weft studies you while he considers your words.* I can understand that. Hutmates should stick together, after all. I'll calm my aggression as well.
        ->deactivateWeft
}
    +You suck up to them for protection, but I can handle myself. I thought it would benefit you more than me.
{
-insultedWeftAfterHostages:
        Maybe you need their protection, maybe you don't. This doesn't make us friends, but... thanks, I guess.
        ->deactivateWeft
-else:
        Maybe you need their protection, maybe you don't. But I'm not about to seem ungrateful. Thank you.
        ->deactivateWeft
}
    +The only real 'reward' they're going to give us is more work. I was trying to get you to take the brunt of it.
        More work just means more opportunities to prove your usefulness. Your loss, my gain.
        ->deactivateWeft

=== tookBlameForHostages_1a ===

I don't understand. Why tell Adéla and Tabor you were the one at fault?

{
-insultedWeftAfterHostages:
    +This feuding between us is getting us nowhere. Think of it as an olive branch.
        \*Weft studies you while he considers your words.* I can understand that. Hutmates should stick together, after all. I'll calm my aggression as well.
        ->deactivateWeft
}
    +You suck up to them for protection, but I can handle myself. I thought it would benefit you more than me.
{
-insultedWeftAfterHostages:
        Maybe you need their protection, maybe you don't. This doesn't make us friends, but... thanks, I guess.
        ->deactivateWeft
-else:
        Maybe you need their protection, maybe you don't. But I'm not about to seem ungrateful. Thank you.
        ->deactivateWeft
}
    +They will surely give the one they trust more work. I was just trying to avoid future labors.
        More work just means more opportunities to prove your usefulness. Your loss, my gain.
        ->deactivateWeft

=== blamedWeftForHostages_1a ===

You disgust me. Who could trust you now that they've seen you disgrace yourself like this?

    +I never asked for your trust. The Lovashi can only favor one of us the most, and it's going to be me.
        And it shall never be given. You've ruined any chance of cooperation between us.
        ->deactivateWeft
    +\*Smile and spread your hands.* Weft, my friend, you must understand it was a momentary slip of judgement. Nothing more, I swear it.
        Like I'd believe that now, you rat. *Weft spits at your feet.*
        ->deactivateWeft
    +What would you have me say? We both play the same game, and I'm playing to win.
        Think that if you like, but all you've done is reveal to everyone your vile nature. 
        ->deactivateWeft
    +How you're looking at me now? That's how every other branded looks at you.
        \*Weft shudders.* Maybe you're right. But that doesn't mean we can't share the same revulsion.
        ->deactivateWeft

=== setUpPostScriptSpeakers(->divert) ===

activate({weftIndex})

changeCamTarget({weftIndex})
setNPCFacing({weftIndex},SW)
setFacing(NE)

fadeBackIn(60)

->divert

=== deactivateWeft ===

fadeToBlack()

deactivate({weftIndex})

fadeBackIn(60)

->Close

=== deactivateGuards ===

setToTrue(hostageSituationGuardsLeft)

fadeToBlack()

deactivate({adelaIndex})
deactivate({taborIndex})
updateNPCVisibility()

{
-gaveWeftCreditAfterHostages:
    ->setUpPostScriptSpeakers(->gaveWeftCreditConvo_1a) 
-tookBlameForHostageDeath:
    ->setUpPostScriptSpeakers(->tookBlameForHostages_1a)
-blamedWeftForHostageDeath:
    ->setUpPostScriptSpeakers(->blamedWeftForHostages_1a)
}

deactivate({weftIndex})

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE