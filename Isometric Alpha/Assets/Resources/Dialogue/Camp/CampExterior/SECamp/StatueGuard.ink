VAR wisdom = 1
VAR strength = 1
VAR dexterity = 1
VAR charisma = 1

VAR playerName = ""

VAR playerIndex = 0
VAR henrikIndex = 1
VAR spokeWithHenrik = false
VAR saidHenrikReveresDirector = false

{
-spokeWithHenrik:
    ->3a
-else:
    ->1a
}

=== 1a ===

setNPCFacing({henrikIndex},NW)
fadeToBlack()

setToTrue(spokeWithHenrik)

movePlayerPos(7,2)
setFacing({playerIndex},NW)

fadeBackIn(60)

\*Henrik mutters under his breath.* ... but I'm serving with the Hero of the West. Who amounted to nothing now, pa?

    +Whose statue is this?
        ->1aa(->1b)
    +Yeah, my father was right bastard too.
        ->1aa(->1b)


=== 1aa(->divert)

setFacing(NE)
setNPCFacing({henrikIndex},SW)

->divert

=== 1ba ===

Bah! You startled me, branded. Sneak up on enough guards and you're bound to get cuffed arcoss the cheek, or worse.

    +I was merely wondering whose statue this was.
        ->1b

=== 1b ===

This is a statue of the Director. His full name is Lord Gábor Kálnoky, but you will refer to him by his title. You'd better familiarize yourself with his likeness, so you can be on your best behavior when he's around.

{
-wisdom >= 2:
    +That can't be right. You're telling me the Commander of the Western Lance, one of the greatest heros of the last war and uncle to the current Count Kálnoky, is the Director of this camp? <Wis {wisdom}/2>
        ->1d
-else:
    +Is he someone special? I've never heard of him. <Wis {wisdom}/2>
        Never heard of him? I'm surprised even a slave could be so ignorant. 
        ->1c(->1e)
}


=== 1c(->divert) ===

When I was but a boy, my father's and my father's father's generations went to war with the Kingdom of Masons, to the south. The Counts appointed three lords to lead their armies, the Commanders of the Western, Southern, and Eastern lances. 

The Director was the Commander of the Western Lance, and where he rode the fighting was the fiercest. No other lord struck as hard or as far as he did. He's a great man and a strategic genius.

->divert

=== 1d ===

Don't forget 'Taker of Slaves', 'Freer of Horses', and, if the rumors are true, decent poet and singer. I truly could speak of him all day.

->1e

=== 1e ===

{
-true:
    +I have questions about the Director if you'd be willing to answer them.
        ->2aa
}
{
-wisdom < 2:
    +If he's so brave and smart, why is there still a Kingdom of Masons? Seems like they were too much for him.
        Because the Craft Folk cheated, thats why! Enough with your back-talk. Get back to work!
        ->Close
}
{
-true:
    +I've got too much work to do to listen to a history lesson. I must be going.
        ->Close
}

=== 2aa ===

I'd be happy educate you. Ask away.

->2a

=== 2a ===

{
    -wisdom < 2:
    +If the Director is a Kálnoky, does that make him a count?
        He's not the count, his nephew is. Count Béla Kálnoky rules over County Kálnoky. Only the current ruler of a county is the count. You would address the other members of his family as lord or lady.
        ->2a
    -wisdom > 2:
    +Tell me what you know of the Director's deeds.
        ->1c(->2a)
}

{
-saidHenrikReveresDirector:
    +The Lovashi have a lot of history it seems. Can you tell me more stories from your past?
        I don't have the time for that. If you're allowed a break you can ask Priest Rikard, in the temple at the center of camp. He would gladly answer your questions.
        ->2a
}

    +You seem to really look up to the Director.
        setToTrue(saidHenrikReveresDirector)
        I believe we all do. He is old enough to remember a time before the Confederation. Before counts and the Emancipation Conflict. Before our people were settled and still.

        When he was young, the Riding Folk were too. The priests tell us back then we rode the High Steppes and hunted deer and kept sheep. Back then we were one people, under one sky and one king.

        But he doesn't need to listen to priests to know this. He isn't some actor playing a role like Chief Tabor does. He lived it. He can remember it, derive wisdom from it. And every action he takes reflects that wisdom.

        ->2a

    +If the Director's such a hero, what's he doing overseering a mining camp? Isn't that beneath him?
        It's not for me to question his motives. A soldier must trust his commander, and I trust the Director with my life.
        ->2a

    +I must be going.
        ->Close

=== 3a ===

    You've returned. Is there no where you're supposed to be?

    +I have questions about the Director if you would be willing to answer them.
        ->2a

    +I have to get back to work. *Leave.*
        ->Close

=== Close ===

close()

->DONE