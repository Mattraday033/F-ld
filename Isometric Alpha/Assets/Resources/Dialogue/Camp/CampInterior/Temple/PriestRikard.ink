VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR askedRikardAboutBeast = false
VAR henrikMentionedPriest = false
VAR sculptingTabooMentioned = false

VAR discoverFlagLovashi_CampSECamp = false

VAR rikardIndex = 1

VAR playerName = ""


->1a

=== 1a ===

Hello. Are you a recent delivery?

    +I'm fresh off the cart. I arrived last night.
        Hmm, I thought as much. I'll memorize your face before long.
        ->1b
    +What does it matter to you?
        It matters little, truly. I merely wanted to know if I had failed to memorize your face, or if you were new.
        ->1b

//I try to memorize each of the branded's faces, but yours seems new. 

=== 1b ===

I am Rikard, the priest of this temple. Know that so long as the sun is up, and you have no other duties, you are welcome in my temple. Even during this lockdown. 

The Director is a devout man, and understands the importance of the work we do here. The guards have orders to leave you be while you are under my care.

    ->1c

=== 1c ===

{
-true:
    +I am unused to being in a <i>Lovashi</i> temple. What rites do you hold here?
        The Lovashi keep many of the same days sacred as your people do. The Day of Mothers, Cycle's Close, Harmondine, and so on.

        In my younger days, I spent some time among the lay-shrines your people keep. The biggest difference you will notice is that we hold Beast in high regard, where the Craft Folk seem to have little time for the God of Creatures. 

        ->1d
    +It's not much of a temple. It looks more to me like a shack in the woods.

        setToTrue(sculptingTabooMentioned)
        I will forgive your tone, because I recognize this place must seem very different than what you are used to.

        It is typical for your people to adorn your temples with baubles and statues that were made by your priests. To our people, that practice appears foreign.

        When we roamed the High Steppes, everything we owned we had to carry with us. To create something whose only purpose was to adorn a temple was a luxury our ancestors thought too expensive.

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

        And in this temple, we use His example to teach the care for horses to the branded. It was your opposition to His decree to hold them as equals that landed you in this camp, your time here will be spend ruminating on how you went so wrong.

        ->1d

}

{
-true:
    +Do the branded have to work on the holidays you mentioned?

        It varies, and of course the lockdown may affect what rites we are permitted to perform. 
        
        Gods willing, the Day of Mothers will be a small affair with extra rations and a ceremony held to commemorate the Great Mother and each of our own. Work will commence after the noonday break.

        Harmondine will grant a full day's exemption from work. I have reminded the Director the purpose of the day is to venerate the ties Harmony bestowed between all folks. In this vein, you will be allowed the typical washing and annointing, and then extra rations for your supper.

        Cycle's Close is many months from now, so the Director and I have not spoken about it yet. I hope to at least convince him to hold some recognition of the holiday. We may not all be burning flour covered effigies, but perhaps a modest increase in rations and some rest would not be remiss.

        ->1d
    +I have other questions.
        Ask, and it is my duty to answer.
        ->1c
}


=== 1e ===

{
-discoverFlagLovashi_CampSECamp:
    +If statues are taboo, why is there one in front of the mess hall?
        Ah, you got me.
        ->1e
-else:
    +Can you explain to me why the Lovashi have made sculptures so taboo?
        No, I don't think I will.
        ->1e
}

{
-true:
    +I would like to talk about something else.
        ->1c
}

=== 1f ===

    +Can you explain some of the history behind all of this? Why are the Lovashi doing this to us?
        
        \*Rikard sighs.* Your wording is typical of how the branded view the conflict between our groups. The question you've asked is a poor one. Instead you should ask what <i>you</i> have done to cause the Lovashi to act this way.

        Long ago, there was 
        //Értékes
        
        ->1f


{
-true:
    +I would like to talk about something else.
        ->1c
}

=== Close ===

close()

->DONE