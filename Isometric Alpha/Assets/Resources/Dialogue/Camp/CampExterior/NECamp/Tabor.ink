VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerName = ""

VAR spokeToTaborAtBeginningOfSituation = false

VAR taborIndex = 1
VAR adelaIndex = 2
VAR weftIndex = 3

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

fadeToBlack(true, false)

setToTrue(spokeToTaborAtBeginningOfSituation)

activate({weftIndex})
movePlayer(-9,-11)
setFacing(NW)

setNPCFacing({taborIndex},SE)
setNPCFacing({adelaIndex},SE)

fadeBackIn(60)

The situation is this: inside the hut to my left is a group of branded who have been attempting to tunnel underneath their hut, below the wall and ditch, and out into the forest. They haven't completed it yet, or else they'd have already made their escape.

changeCamTarget({adelaIndex})

We discovered this plot when a squad of my men didn't come back from their morning inspection. The scum disarmed them somehow and have killed at least one of them. The rest are their prisoners, and what little dialogue we have managed to coax from the group's leader has revealed they want to use them to barter for their freedom.

changeCamTarget({weftIndex})

Those leeches! They'll regret that soon, I'm sure.

    +Where do we come into this?
        ->1b
    +Shut it, Weft.

    \*Weft acts like you didn't say anything, but he doesn't say anything else either.*

        ->1b

=== 1b === 

changeCamTarget({taborIndex})

We don't think we can force the entrance of the hut without the branded killing the prisoners, and we haven't had much luck negotiating with them from out here.

changeCamTarget({adelaIndex})

They're stalling. They must think they can complete their tunnel before we decide to storm their hut.

changeCamTarget({taborIndex})

You two are going to negotiate for us. I doubt they'd let a guard, even one who is unarmed, enter the hut without taking them hostage.

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

        I am the Chief Correction Officer. I am the one who determines how we reprimand the slaves and for what reasons. You may be my superior, but the Director has given me this role and he will hear about anyone who encroaches on it.
        
        changeCamTarget({adelaIndex})
    
        \*Adéla holds Tabor's gaze, but says nothing.*

        setNPCFacing({adelaIndex},SE)
        setNPCFacing({taborIndex},SE)
        changeCamTarget({taborIndex})

        \*Tabor turns back to you.* You two should act as you believe is best. You can go where we cannot, and that utility is what we are forced to rely upon.

        ->1c
    +What do we have to offer them? Will you kill them if they surrender?
        changeCamTarget({taborIndex})

        I am prepared to offer them only a dozen lashes each, and they will be moved to separate huts, but they will get to live.


        
        ->1c
    +If they attack us, are you going to come charging in to save us?
        ->Close

=== alreadySpokeToTabor ===

Be careful in there. They're backed into a corner, so they'll lash out if provoked.

->Close

=== Close ===

close()

->DONE