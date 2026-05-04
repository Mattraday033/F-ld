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

VAR playerName = ""

VAR hostagesDead = false
VAR sentIntoMineByDirector = false

VAR gaveWeftCreditAfterHostages = false
VAR tookBlameForHostageDeath = false
VAR blamedWeftForHostageDeath = false


{
-sentIntoMineByDirector:
    ->alreadySpokeToDirector
-else:
    ->1a
}

=== 1a ===

changeCamTarget({pageIndex})

Hello. Are you here to see the Director?

    +Yes, Chief Tabor said I was to report to his office?
        ->1b

=== 1b ===

I see. You're expected, go on in.

fadeToBlack()

setToTrue(sentIntoMineByDirector)

movePlayer(-2,-1)
setFacing(NE)
setNPCFacing({directorIndex},NW)

activate({taborIndex})
activate({weftIndex})

{
-hostagesDead:
activate({adelaIndex})
}
changeCamTarget({taborIndex})

fadeBackIn(60)

Director, sir, these are the two branded that Captain Adéla and I spoke of. The ones we used to negotiate for the hostages.

{
-hostagesDead:
    ->2a
-else:
    ->Close
}

=== 2a ===

changeCamTarget({adelaIndex})

And they are the ones responsible for the deaths of those same hostages.

changeCamTarget({directorIndex})

\*A man, his hair grey, his armor made for someone larger, sits behind a desk. He stares at the steppe green and gold of the Lovashi banner that adorns the office wall. After a moment, he turns to you.*

setNPCFacing({directorIndex},SW)

I have already listened to your account of what happened, captain. I wish to listen to what they have to say.

{
-tookBlameForHostageDeath:
-blamedWeftForHostageDeath:
-else:

}

You are the branded known as {playerName}, are you not?

    +I am, sir.
        ->Close
    +That is my name, yes.
        ->Close
    +\*Say nothing.*
        changeCamTarget({taborIndex})

        Answer the Director's question, branded.

        changeCamTarget({directorIndex})

        \*The Director holds up a hand.* No, let them answer how they like. I want to understand the branded I am assigning this duty to.

        ->2b
    +You already know that. Why are you asking me?
        ->2b

=== 2b ===

Your name was but a scratch in a ledger to me before. Now, it has a face to it. You've made yourself real, in a way.

The names of the hostages have been removed from our ledgers, thanks to your actions. Two more names, buried along with their faces. How does that make you feel?

    +I'm miserable over it, sir.
        ->Close
    +Why should I lose sleep over a pair of dead slavers?
        ->Close
    +\*Say nothing.*
        ->Close



=== alreadySpokeToDirector ===

PH

->Close

=== Close ===

close()

->DONE