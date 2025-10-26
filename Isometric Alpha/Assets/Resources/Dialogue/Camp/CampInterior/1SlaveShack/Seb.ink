VAR acceptedClaysSecondJob = false
VAR gaveNoteToSeb = false

VAR playerIndex = 0
VAR sebsIndex = 1
VAR balintsIndex = 2

->1a

=== 1a ===

setToTrue(spokeToSeb)

\*The man lies on the ground, prone. He wears only the tattered trousers of a slave, soaked some with blood. His back is choked with red brown lash wounds that look recently scarred over. His left side has a series of purple bruises, in the shape of a boot's toe. It looks like someone has taken care to place him in soft straw, and position him facing the wall. He mutters to himself occasionally, and gives no signs of noticing you.*

    {
    -acceptedClaysSecondJob:
        +Excuse me, is your name Seb? 
            changeCamTarget({balintsIndex})
            ->2a
    -else:
        +Hello? Are you alright?
            ->1b
    }
    +\*Leave.*
        ->Close

->Close

=== 1b ===

\*Seb shudders, but does not reply*

    +\*Leave.*
        ->Close

->Close

=== 2a ===

Yeah, that's him. He hasn't responded to anything I've asked him or said anything intelligable since the guards threw him in here a few days ago. 

    +What did they do to him?
        ->2b
    +I'm supposed to deliver him a note.
        ->2c

=== 2b ===

keepDialogue()

Tabor worked him longer than most I've seen. Made a bunch of us skip a meal to come watch it too. I don't know what he did but Tabor took it personally. He kept going after the ropes holding him to the post slipped and Seb fell to the ground. 

->2a

=== 2c ===

A note? Who's it from?

    +I'd rather not say, they might not want you to know.
        I won't pressure you to tell me. It's none of my business.
        ->2d
    +Clay. He used to be Seb's hutmate.
        I know him. A hard fellow, watch yourself with that one.
        ->2d

=== 2d ===

Whatever the case, he's not going to be able to read it. I've seen this happen to slaves after the guards are through with them. Sometimes they come back to us, and sometimes they don't.

    +\*Read the note aloud.*
        ->2e
    +\*Keep Seb company for a while.*
        ->2f
    +\*Place the note in front of Seb and leave.*
    
        ~gaveNoteToSeb = true
        setToTrue(gaveNoteToSeb)
        activateQuestStep(Note Worthy,2)
        
        prepForItem()
        
        \*Seb shudders but otherwise does not notice your presence.*
        
        takeAllOfItem(Clay's Note)
        ->Close

=== 2e ===

activateQuestStep(Note Worthy,3)
~gaveNoteToSeb = true
setToTrue(gaveNoteToSeb)

changeCamTarget({playerIndex})

prepForItem()

"Breaking eggs was how I lost my old crew. Should have known it would lose me my new one. Your wit will be missed my friend, but I'll paint the town red in your absence. Apologies are easier to write than say, coward that I am. If you don't get this note, I swear I'll say it at my hearth." *You leave the note in front of Seb.*

takeAllOfItem(Clay's Note)

changeCamTarget({sebsIndex})

\*Seb shows no sign he has noticed you.*

->Close

=== 2f ===
activateQuestStep(Note Worthy,2)
~gaveNoteToSeb = true
setToTrue(gaveNoteToSeb)


changeCamTarget({playerIndex})

prepForItem()

\*You wait. Seb does not stir. Finally, having shared Seb's sobering company for a time, you leave the note in front of him and leave.*

takeAllOfItem(Clay's Note)

changeCamTarget({sebsIndex})

\*Seb shows no sign he has noticed you.*
->Close

=== 2g ===

->Close

=== 2h ===

->Close

=== 2i ===

->Close
=== Close ===

close()

->DONE