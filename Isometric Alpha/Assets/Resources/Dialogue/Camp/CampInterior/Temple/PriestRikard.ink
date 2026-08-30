VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR askedRikardAboutBeast = false
VAR rikardExplainedBrandOrigins = false
VAR henrikMentionedPriest = false
VAR sculptingTabooMentioned = false
VAR heardTaborsLesson = false

VAR secretDoorFlagCenterCampWall_Patch2 = false
VAR metFlagPriest_Rikard = false

VAR discoverFlagCampSECamp = false

VAR rikardIndex = 1

VAR playerName = ""

{
-metFlagPriest_Rikard:
    {
        -secretDoorFlagCenterCampWall_Patch2:
            If you were a part of the work detail that tore down my wall, I'd like to speak with your overseer. That should not have happened so near to services!
        -else:
            I appreciate your appetite for learning. A curious branded is one who can be reached.
    }

    ->1c
-else:
    ->1a
}

=== 1a ===

setToTrue(metFlagPriest_Rikard)

Hello. Are you a recent delivery?

    +I'm fresh off the cart. I arrived last night.
        Hmm, I thought as much. I'll memorize your face before long.
        ->1b
    +What does it matter to you?
        It matters little, truly. I merely wanted to know if I had failed to memorize your face, or if you were new.
        ->1b

//I try to memorize each of the branded's faces, but yours seems new. 

=== 1b ===

I am Rikard, the priest. Know that so long as Sun is up, and you have no other duties, you are welcome in my temple. Even during this lockdown. 

The Director is a devout man, and understands the importance of the work we do here. The guards have orders to leave you be while you are under my care.

    ->1c

=== 1c ===

{
-true:
    +I am unused to being in a <i>Lovashi</i> temple. What rites do you hold here?
        The Lovashi keep many of the same days sacred as your people do. The Day of Mothers, Cycle's Close, Harmondine, and so on.

        In my younger days, I spent some time among the lay-shrines your people keep. The biggest difference you will notice is that we hold Beast in high regard, where the Craft Folk seem to have little time for the God of Creatures. 

        ->1d
    +Is this temple dedicated to a specific God?

        No, it venerates many, as is required by the needs of the camp. You will find that temples with priesthoods who honor only a signal god are a luxury of larger cities, where wealth and followings can be shared among many prestigious holyhalls.

        In places like this, a single priest will be called on to help their charges with the rites to all the Gods, as each of them affects our lives in many ways daily.

        ->1c
    +It's not much of a temple. It looks more to me like a shack in the woods.

        setToTrue(sculptingTabooMentioned)
        I will forgive your tone, because I recognize this place must seem very different than what you are used to.

        It is typical for your people to adorn your temples with baubles and statues that were made by your priests. To our people, that practice appears foreign.

        When we roamed the High Steppes, everything we owned we had to carry with us. To create something whose only purpose was to adorn a temple was a luxury our ancestors thought too expensive in labor.

        If a simple tent was enough for our elders, then a hut is more than enough for us. And all of that doesn't even touch on the ban on sculpting. You will see few statues in Lovashi temples.
        ->1e

}

{
-heardTaborsLesson:
    +Chief Tabor told me I could speak to you to learn more about horses and the brand?
        
        Absolutely! I'm glad to have a branded ask about their place in all of this. Most just use the temple as a place to catch their breath until I am forced to chase them out. What would you like to know?
        ->1f
}

{
-true:
    +What's with the boot in the corner?
        
        That is not just some boot, it is this camp's 'showtouch'. Please, speak of it respectfully.

        It is common in victory for a Lovashi horde to take trophies or looted items from their enemies. But the Craft Folk have a disgusting practice of making their equipment out of tanned horseflesh.

        It is law that when a object made from horse leather is found, it must be burned. To teach a horde what these items look like, a commander who has seen victory is entitled to take a grotesque of horseflesh and have it consecrated as his horde's showtouch.

        That specific totem was liberated by the Director a generation hence during his contributions to our confederation's war with the Kingdom of Masons. It has traveled with him ever since, to serve to remind to his soldiers of the depths the Craft Folk will sink to in their depravity.
        ->1c

    +I must be going.
        ->Close
}

=== 1d ===

{
-true:
    +Why do the Lovashi hold so much esteem for Beast?
        setToTrue(askedRikardAboutBeast)
        \*Rikard chuckles.* I do not laugh at you, branded, it is merely a question I am asked all too often. And one that would seem strange to one of my kin.

        Each of the folks of Föld were granted patronage by one God or another when they were made. As you know, yours were commissioned by the Great Mother, Angyel, and so your people hold her highest, along with her two daughters: Sun, and Harmony.

        My people were commissioned by Beast, and are His favorites. He gave us horses, and the horsetongue to commune with them. He then mandated us to learn all we can of the natural world, and find uses for all animals in human civilization.

        I think a more appropriate question would be how could we not hold so much esteem for Him.
        ->1d
}

{
-askedRikardAboutBeast:
    +When my people invoke the name of the Father of Animals, it is usually to placate, not to beseech Him. What benefits do His blessings bring?
        
        I have little doubt that is true. Beast is not merely some predator to be appeased while He slinks about beyond the light of your village.

        First, He was the Great Mother's first mate. It was with Her He sired Their eldest daughter, Harmony. It is to Him we give thanks on the Day of Fathers, and men seek his wisdom while raising a family.

        If you have experience in the field, I would expect you've spoken some rendition of the Grazier's prayer, to fatten sheep and pigs, and to fend off wolves and worse. It is to Beast this prayer calls for aid.

        And in this temple, we use His example to teach the care for horses to the branded. It was your opposition to His decree to hold them as equals that landed you in this camp. Your time here will be spend ruminating on how you went so wrong.

        ->1d

}

{
-true:
    +Do the branded have to work on the holidays you mentioned?

        It varies, and of course the lockdown may affect what rites we are permitted to perform. 
        
        Gods willing, the Day of Mothers will be a small affair with extra rations and a ceremony held to commemorate the Great Mother and each of our own. Work will commence after the noonday break.

        Harmondine will grant a full day's exemption from work. I have reminded the Director the purpose of the day is to venerate the ties Harmony bestowed between all folks. In this vein, you will be allowed the typical washing and anointing, and then extra rations for your supper.

        Cycle's Close is many months from now, so the Director and I have not spoken about it yet. I hope to at least convince him to hold some recognition of the holiday. We may not all be burning flour covered effigies, but perhaps a modest increase in rations and some rest would not be remiss.

        ->1d
    +I have other questions.
        ->askReply(->1c)
}

=== 1e ===
{
-true:
    +Can you explain to me why the Lovashi have made sculptures so taboo?
        Sculpting is an art steeped in hubris. To capture some thing's essence so exactly is to liken one's self to the Gods. To adorn our temples with them is to flip the natural order completely backwards.

        The Gods created us by molding our likeness from soil. To cleave the Gods' likeness from rock or metal is as if to put ourselves in their place. This is the height of folly, and it is well it has been outlawed.
        ->1e
}
{
-discoverFlagCampSECamp:
    +If statues are taboo, why is there one in front of the mess hall?
        That is a... sore subject between myself and the Director. One I don't wish to discuss with one of the branded.

        All you need to know is that confederation law allows for a count or members of one's family to have statues commissioned of their likeness. So it is not technically illegal, but whether it is in poor taste is, at least in the eyes of <i>secular</i> law, a matter of opinion.
        ->1e
}

{
-true:
    +I have other questions.
        ->askReply(->1c)
}

=== 1f ===

{
-true:
    +Can you explain some of the history behind the brand? Why are the Lovashi doing this to us?
        ->1fa
}
{
-true:
    +Chief Tabor said that he's teaching us to be better even though we're branded for life. What's the point if we die at the end?

        Chief Tabor is perhaps the most dedicated of the guards toward our purpose. It's good for you that you're listening to his lessons.

        To answer your question, you must not think merely of this life, but of the next. Each of us, as one of the Great Mother's children, is granted a hearth to rest at after our souls leave our bodies.

        Before our souls can pass through the hearth and be reborn into this world, we must make amends with everyone we have had conflict with in our previous life. So it is imperative that we teach the branded the fault in their ways so they hold no grudges in the afterlife.

        You can see why this would feel important to soldiers such as the guards of the camp: should they fall in battle or even die of old age, their afterlife would be arduous indeed if they had to seek forgiveness from every foe they've slain.

        The lessons and the punishments serve to decrease the time spent cleansing our souls of conflict before being granted reincarnation, both for branded and master alike.

        ->1fc
    +Chief Tabor seems to be the only guard I've met who cares about your 'mission' to better the branded. The others seem like oppressive, sadistic louts. Not that Chief Tabor is much better, mind you.

        My own irritation at the guards' laxness with their duties is what keeps me from calling one of them to rectify your tone, branded. You speak some truth, but say it before another Lovashi and you'll find yourself flogged.

        The Lovashi fight what we call the 'Emancipation Conflict'; this is our name for our crusade to free all horses from human captivity. It started with my grandfather's generation, some sixty or so years prior.

        The records hold that in older days, our course was clear and set. Each Lovashi knew their part in the conflict. But in current times, sons of sons and daughters of daughters keep the fight going. There is no clear end in sight, and what was once important seems so very far to them.

        Chief Tabor does what he can, but it falls to the priests to instill in others the urgency of what we fight for. The attitude you observe is a symptom of my own failings as much as it is this generation's. One I fight every day to overturn.

        ->1f
}

{
-rikardExplainedBrandOrigins:
    +What happened to the Lovashi king and queen? I've only heard of the Lovashi counts.
        The tale of the Lovashi sovereign line is one full of loss and mourning. However, I suspect it's one the branded never grow tired of hearing, so I will give you its briefest telling.

        The royal line of the Lovashi perished in the wars that followed the death of Értékes, save for a handful who returned to the High Steppes rather than continue the conflict. The counts are the Lovashi lords who rule the conquered Craft Folk territories in their absence.
        ->1f
}

{
-true:
    +I have other questions.
        ->askReply(->1c)
}

=== 1fa ===

\*Rikard sighs.* The way you've worded that is so typical of how the branded view the conflict between our peoples. The question you've asked is a poor one. Instead you should ask what <i>you</i> have done to cause the Lovashi to act this way.

This is also not a story quickly told. I would be happy to tell it, so long as your duties permit you the time to hear it.

    +I have time to listen to it.
        ->1fb
    +I should be getting back to work.
        Very well.
        ->Close

=== 1fb ===

setToTrue(rikardExplainedBrandOrigins)

Long ago, there was a great prince of the Riding Folk, the people from whom the Lovashi descend. His name was Értékes, born to the mounts of the Riding Folk's king and queen. 

Értékes was a blessed horse. Sun took a special interest in his birth, and granted him a portion of Her brilliance. He had hooves like stones of great worth, and hair and mane that radiated golden light while Sun was rizen.

To abridge our tale some, eventually there came a time of contention between the Riding Folk and the Craft Folk. An exchanging of gifts was arranged to appease both sides. In this exchange, the sovereigns of the Riding Folk allowed Értékes to be taken as a mount by one of the Craft Folk kings.

It is important that you understand what this meant to the Riding Folk. The Gods had declared they teach all of humanity how to integrate animals into their kingdoms, but not yet had the Riding Folk allowed the secrets of horsemanship to be taught to outsiders. Értékes would be the first horse to take a foreign rider.

It was hoped this bonding would help to avoid war, but peace was not to be. The Craft Folk mistreated Értékes, and he died from their abuse. Such was the anger of the Riding Folk that the once nomadic and disparate people banded as one horde, and invaded. 

The war between our cultures may have ended with the death of the offending king, had not your people in their fear and greed stolen horse foals from our camps during the fighting. They used these abducted children to breed horses of their own, and pressed them into slavery to be used as weapons to fight the Riding Folk with.

We brand your people to both show you the horror you have inflicted on horsekind, and to use your labor against those Craft Folk who still resist. The brand is a tool to show you the error of your ways.

    ->1f

=== 1fc ===

    +Do you really expect my people will leave this life grudgeless after all you've done to them?
        The Craft Folk are an obstinate lot but, even if progress is slow, our cause is worthy. For both our sakes.
        ->1f
    +I don't get it.
        Don't worry about that for now. There will be many lessons before your tenure here is finished. There is time yet for you to learn.
        ->1f
    +I guess that makes <i>some</i> kind of twisted sense.
        Despite your tone, that is a much more enlightened way of thinking than your compatriots have shown. I will take what I can get.
        ->1f

=== 1g ===

->Close

=== askReply(->divert) ===

Ask, and it is my duty to answer.
->divert

=== Close ===

close()

->DONE