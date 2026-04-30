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

{
-spokeToTaborAtBeginningOfSituation:
    ->alreadySpokeToTabor
-else:
    ->1a
}

=== 1a ===

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

With the situation understood and our position clear, it's time for you to enter the lion's den. We have already informed the branded inside that you were sent for, so they will be expecting you.

When you are inside, assess the condition of the hostages. We believe that two of them should still be alive, but if they have already been killed, you are free to leave and we will storm the hut after you are clear. 

May the Gods be with you, and the hostages.

fadeToBlack()

deactivate({weftIndex})
deactivate({guardIndex})

setNPCFacing({taborIndex},SW)
setNPCFacing({adelaIndex},NE)

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE