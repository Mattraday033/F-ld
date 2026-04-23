VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR weftIndex = 1

VAR heardTaborsLesson = false

VAR playerName = ""


->1a

=== 1a ===

\*The man before you has the same brand on his neck as the others you've seen, but wears much nicer clothes. He turns to look at you and his expression sours.* Yes? Am I needed for something?

    +My name is {playerName}. I've been told we are to share a hut and to deliver you your rations. 
        ->1b
    +\*Leave.*
        ->Close

=== 1b ===

finishQuest(My New Hutmate,true,Rations delivered.)

prepItem()

\*The sourness fades from his face.* Ah, wonderful! I thought you were just another of the branded. I'm Weft. I take it you're new here?

exchangeItemForXP(Weft's Rations,1,50)

    +That's right. Fresh off the cart this morning.
        You move fast then, if you've already been sent here. And it's well you have been: this hut is where the branded that have a future are sent. All the others aren't worth our time.
        ->1c

=== 1c ===

    +Why is that?
        \*Weft shrugs as he eats his rations.* They're unlikely to outlive it.
        ->1c
    +You have a mightly high opinion of yourself.
        I've been set aside for greater things. The Lovashi wouldn't waste clothes and a bed on a slave they didn't mean to keep around.
        ->1d
    +You've got that right.
        I'm glad we see things the same way. I can be a lonely experience to be recognized like this. Even the Manse servants look down their nose at us. That's why we've got to stick together.
        ->1d

=== 1d ===

I've been at this camp for a long time, so I'll give you some advice. There are two types of guards: the sadists, and the fanatics. You deal with them each a little differently.

The sadists are here for the cheap thrills, but they have plenty of easier targets than you or I. Don't look them in the eye, say 'yessir' or 'yes ma'am', and they'll move on to easier prey. 

The fanatics on the other hand are here to make sure you learn something. Give too quick of an answer to their questions and they'll think you aren't paying attention. That's when the 'lessons' start.

{
-heardTaborsLesson:
    +Chief Tabor has already given me the whole speech. 
        ->1e
}

    +What could I possibly learn from these monsters?
        ->1e

=== 1e ===

Oh, it's not real learning. It's more like indoctrination. The Lovashi are a culture of horsemen, and they treasure their horses like people.



->Close

=== Close ===

close()

->DONE  