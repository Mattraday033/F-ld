VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR playerName = ""

VAR nandorIndex = 1
VAR carterIndex = 2
VAR kastorIndex = 3
VAR marcosIndex = 4
VAR andrasIndex = 5
VAR taborIndex = 6
VAR crowdIndex = 7
VAR clayIndex = 8
VAR garchaIndex = 9
VAR thatchIndex = 10

VAR gaveAGuardToTheCrowd = false
VAR executedAnyGuard = false

VAR didNotExecuteMarcos = false
VAR gaveMarcosToTheCrowd = false
VAR gaveMarcosFiftyLashes = false
VAR executedMarcos = false

VAR didNotExecuteAndras = false
VAR gaveAndrasToTheCrowd = false
VAR gaveAndrasFiftyLashes = false
VAR executedAndras = false

VAR letTaborLive = false

VAR executedTabor = false
VAR didNotExecuteTabor = false
VAR gaveTaborToTheCrowd = false
VAR foughtCrowdForTabor = false
VAR crowdForcedTaborExecution = false

VAR nandorLeftParty = false

VAR letNandorDecideGuardPunishments = false
VAR afterNandorDecidesGuardPunishments = false

VAR canSpeakToKastorAboutFoodShortage = false
VAR spokeToKastorAboutFoodShortage = false
VAR kastorDiscussingFoodShortage = false

VAR sworeToBurnCsalansBody = false
VAR foughtHorsesInManse = false

VAR mineLvl3MarcosDiedSealingBreach = false
VAR andrasIsDead = false
VAR deathFlagGuardMárcos = false
VAR deathFlagGuardAndrás = false
VAR deathFlagChiefTabor = false

VAR mineLvl3ConvincedRekaAndPazman = false

VAR acceptingGuardPrisoners = false
VAR gotKeyFromJanos = false

VAR refusedToHearNandorsTale = false
VAR spokeWithPageBeforePrisoners = false

VAR crowdDispersed = false

VAR marcosNeedsHandling = false
VAR andrasNeedsHandling = false
VAR rekaNeedsHandling = false
VAR pazmanNeedsHandling = false
VAR taborNeedsHandling = false

VAR noPrisoners = false
VAR marcosIsAtTrial = false
VAR andrasIsAtTrial = false
VAR taborIsAtTrial = false
VAR janosIsAtTrial = false
VAR guardPazmanAndRekaAtTrial = false

VAR nandorReadyToSpeakAfterTrial = false

//changeCamTarget(int targetIndex)
//keepDialogue()
//setToTrue(string flagName)
//setToFalse(string flagName)
//activate(int index of gameobject you're activating)
//deactivate(int index of gameobject you're deactivating)
//activateQuestStep(string questTitle, int questStepIndex)
//prepForItem()
//giveItem(int listIndex, int itemIndex, int quantity)
//giveItems(int listIndex1, int itemIndex1, int quantity1 |
//          int listIndex2, int itemIndex2, int quantity2 |
//          ... etc)
//takeAllOfItem(string itemName)

{
-noPrisoners:
    changeCamTarget({nandorIndex})
    ->3ab
-afterNandorDecidesGuardPunishments:
    changeCamTarget({nandorIndex})
    ->4a
-kastorDiscussingFoodShortage:
    changeCamTarget({kastorIndex})
    ->3aa
-nandorLeftParty:
    changeCamTarget({nandorIndex})
    ->10a
-not nandorReadyToSpeakAfterTrial:
    changeCamTarget({nandorIndex})
    ->1a
-else:
    changeCamTarget({nandorIndex})
    ->1b
}

=== 1a ===

We'll discuss everything that has happened once all of the sentences have been handed down.

{
-marcosIsAtTrial and marcosNeedsHandling:

    +Will you vouch for any of the prisoners?

        {
        -taborIsAtTrial:
            Márcos is the only one I would say with certainty deserves no punishment. The others I need no excuse to let live, except for maybe Tabor. In his case, a swift death would be a mercy compared to what the crowd might do to him if given the chance.
        -else:
            Márcos is the only one I would say with certainty deserves no punishment. I leave everything else to your discretion.
        }
        
        ->Close

    +I must be going.

        ->Close
}

    ->Close

=== 1b === 

setToTrue(spokeWithNandorAfterPrisoners)

{
-foughtCrowdForTabor and gaveAGuardToTheCrowd:
    ->2aaa
-gaveAGuardToTheCrowd:
    ->2a
-foughtCrowdForTabor:
    ->foughtClayForTabor1a
-executedMarcos:
    ->2b
-gaveMarcosFiftyLashes:
    ->2c
-didNotExecuteMarcos:
    ->2cc
-else:
    ->1a
}

=== 2aaa ===

keepDialogue()

You allowed the crowd to paricipate, but then fight them to save Tabor? What madness do you suffer from that makes you act this way?

{
-gaveMarcosToTheCrowd:
And Márcos... why? Of all the guards, he was the one I most trusted to have understood how they had tormented us. He saved my life! And you had him torn apart!

    +Don't weep for a dead slaver, Nándor. He wasn't worth your thoughts, much less your tears.
        ->2aa
}
    +I don't understand. What are you more angry about, letting the crowd participate or protecting Tabor?
        combineDialogue()
        I'm saddened by those that died trying to hurt Tabor, but I am most horrified by how you encouraged the other slaves to conduct themselves.
            ->2aa

=== 2a ===

You allowed the crowd to decide? That was the entire reason for gathering all of the prisoners here in the first place! So we wouldn't give in to such depravity!

{
-gaveMarcosToTheCrowd:
And Márcos... why? Of all the guards, he was the one I most trusted to have understood how they had tormented us. He saved my life! And you had him torn apart!

    +Don't weep for a dead slaver, Nándor. He wasn't worth your thoughts, much less your tears.
        ->2aa
}
    +The slaves have suffered too much at the guards hands to be denied their revenge. All I did was give it to them.
        ->2aa
    +Why direct your anger at me? I never told the crowd to kill, only to decide the punishment.
        ->2ab

=== 2aa ===

You disgust me. I followed you. I thought you were someone who could be trusted to lead the branded. I can't believe how mistaken I was. When we leave this camp, I hope you never disgrace me with your presence again.

    +You said you would accept my decision, no matter the outcome.
        keepDialogue()
        I did, but I thought you understood the purpose of this gathering was to prevent exactly what you just did.
        ->2aa
    +You would chose a dead guard over the fellow branded who freed you from servitude?
        No. I chose to live my life apart from malicious pondscum like you.  
        ->nandorLeavesParty
    +Do me the same favor then. I'll give you a wide birth if you do so too.
        Gladly.
        ->nandorLeavesParty
    +Very well. Your moralizing sickens me anyways.
        ->nandorLeavesParty

=== 2ab ===

Were you not listening to anything I said before? You can't seriously expect me to believe you are that naive.

{
-wisdom >= 2:

+I listened, but I decided to do it anyways. <Wis {wisdom}/2>
    ->2aa

-wisdom < 2:

+Was it naive to hope that the crowd would have sated it's bloodlust already? <Wis {wisdom}/2>
    ->2ac

}

=== 2ac ===

Yes. Extremely. I hate what you've done, but I can see you didn't do it out of malice. I will just have to keep a closer eye on you in the future. And pray that Márcos didn't suffer too much before the end.

    ->2ca

=== 2b ===

Márcos is dead... He asked me, while we were down in the mine, if I thought that the other branded would find a way to forgive him. I told him that it might be a harsh road, but I thought they could, in time. While I may never know if that was true, I still like to think it was possible.

    +If you had qualms about executing him, why did you carry out the sentence?
        
        If there was any confusion among the rebellion's leadership to the motives and integrity of any of the guards that collaborated with us, it would be even harder for the others to accept letting them live. 
        
        You may not realize it, but the branded follow you for your contributions to the rebellion. You're the one they saw leading the fighting, and you did the lion's share of work to free them in the first place. Had anyone else decreed a sentence for Márcos, it is doubtful the others would have followed it. And if we had stood against you? We may have seen ourselves become their victims as well.
        
        keepDialogue()
        
        Rather than risk inciting the crowd to attacking us both, I chose to carry out Márcos's sentence as quickly and painlessly as possible. At least in that way, I could save my friend from any further suffering.
            ->2b

    +The Gods will judge him at his hearth. If they can find a way to forgive him, then he will be better off than he would have been here. 
        
        It may be as you say. I will pray for him, and perhaps my testimony will make some small difference.
            ->2ca

    +Don't mourn his death, Nándor. He was a slaver. A quick death is more than he deserved.
        
        I will mourn whomever I please. And I suggest you keep your opinions to yourself when speaking of them.
            ->2ca

=== 2c ===

You spared Márcos. Thank you. I wish he hadn't been given the punishment you decreed, but I understand the necessity of it. The slaves will have an easier time accepting him now that he has suffered for his crimes, and in a similar way to how they suffered as well. I think they'll appreciate the symmetry of it.
    ->2ca

=== 2ca ===

{
-foughtCrowdForTabor:
    ->foughtClayForTabor1a
-didNotExecuteTabor:
    ->2cb
-crowdForcedTaborExecution:
    ->2cba
-else:
    ->2d
}

=== 2cb ===

It was interesting to see you stand up for Tabor. I, like many of the branded, had been his victim before. I detest the man and what he's done, but there is a part of me that is happy to see another former slave so loudly call for a peaceful resolution. Few would see it as a virtue in the way you appear to, and even fewer in a place like this.
    ->2d

=== 2cba ===

I'm sorry about what happened to Tabor. I, like many of the branded, had been his victim before. I detest the man and what he's done, but a part of me was happy to see another former slave so loudly call for a peaceful resolution. Sadly, too few can see it as the virtue it is.
    
    +If I could have kept him alive without shedding the blood of the branded, I would have. Alas, it proved impossible.
        ->2daa
    +It is what it is. Saving Tabor's life was always a long shot.
        ->2daa

=== foughtClayForTabor1a ===

It took a strength of character few possess to stand up to Clay as you did. I'm glad we had you with us to keep the peace.

    +I'm surprised. I expected you would have been more upset over this.
        ->foughtClayForTabor1b
    +Thank you. Your words make me feel better over what just happened.
        ->foughtClayForTabor1c
    +Did I make the right decision, Nándor? I killed slaves to keep a slaver alive. This does not sit well with me.
        ->foughtClayForTabor1d
    +I have killed those I fought to protect. I have failed in my duty to my comrades. How can you speak so evenly about this?
        ->foughtClayForTabor1e

=== foughtClayForTabor1b ===

I am upset, but not with you. I knew the crowd would be bloodthirsty but I had hoped, perhaps beyond reason, that we would not fight among ourselves. What Clay would have had us do... we would have been no better than the slavers. The point of regimenting the guards sentences like this was to keep us from mob rule. And in that, at least, you have succeeded.

    ->2dab

=== foughtClayForTabor1c ===

You are welcome. The trials of leadership are many: it would be hypocritical of me to ask you to carry this responsibility, then to criticize how you wield it.

    ->2dab

=== foughtClayForTabor1d ===

Had I the courage to stand as judge, perhaps I would have decided Tabor's fate differently. That version of events is unknown to us. What I do know is had I chosen Tabor's fate, and Clay threatened me in the way he did you, I would have defended Tabor and myself in the same way you did. Disperse your worries, it is over and you have the rest of your life to live.

    ->2dab

=== foughtClayForTabor1e ===

Your duties as leader are many. You have a duty to uphold the peace: Clay threatened it, and you stood him down. You have a duty to decency: Clay would have soiled it with his actions, and you prevented that. The others made their choices, just as you did. Do not berate yourself for holding to higher morals than theirs.

    ->2dab

=== 2cc ===

You spared Márcos. Thank you. You may not realize it, but the branded look up to you because of what you've done for them. Your words will go a long way towards helping him integrate with the other prisoners.

    ->2ca

=== 2daa ===

Even so, your efforts have earned my respect. 
    ->2d

=== 2dab ===

    +You are slow to form grudges, and quick to discard them. Why is that?
        ->2da

=== 2d ===

    +You have an easy time letting go of grudges. Why is that?
        ->2da

=== 2da ===

Perhaps that is true. As for why? It's a long story, but I will tell it if you wish to hear it in it's entirety.
    
    +I would hear it 'til it's end.
        ->2db
    +This doesn't seem the time for a saga. Maybe later.
        ~refusedToHearNandorsTale = true
        ->2e

=== 2db ===

As you know, the branded are, in theory at least, given their brand for committing the worst of crimes. While in practice this is often not the case, in mine it is. I received my branding for murder. And not for a single killing, but two.

The first was arguably the more justified, if such a thing is possible. Before my branding, I was a serf to a Lovashi lord in the northeastern county of Eszter. We had a poor harvest, and the makings of a famine were spreading throughout the county. Our lord, whether through selfishness or cruelty, refused our petition to lower our taxes during the famine. 

Because we were poor, we paid the tax mostly with our harvests. But as we had little to show for the year's work, we were forced to pull from our stores instead. With how little we had left, it was obvious we would starve if things did not change. I was one of the few people in my village who had experience traveling, so I was instructed to journey to the villages we believed still had some food and ask them for whatever they could spare. I took my cousin, Vidor, with me for safety.

We traveled to a few villages, but no one had much. One night, as we went to beg for space to stay at a local inn, we saw the tax collector who had taken our levy. I wish I could say I was drunk, or overcome with emotion, but my blood was cold as icewater as I walked up to him. I struck him across the back of the head with my walking staff, and I kept hitting him, even after he had stopped moving. I think it was the sight of him buying an ale that set me off. In my mind, the coin he spent had come from my own village.

After I had realized the gravity of what I had done, I fled. I took Vidor with me into the woods; my cousin and I hid there for days. We lived off grubs and moss, having little luck with snaring rabbits or fishing. Eventually, Vidor wished to return to the village, but I was too paralyzed with fear to go back. We argued. Finally, he said he would leave with or without me. That's when my fear got the better of me. I shoved Vidor to the ground, then sat atop him while I strangled him. His bulging eyes cursed me the entire time, but in the end he relented, and so did I.

I had killed my cousin to keep him from revealing where I hid, but it didn't matter in the end. Someone from the inn had summoned the count's men, who were combing the woods looking for me. Only a few hours after I had killed Vidor, they found me drinking at a stream. They chased me with hounds and horses, and I didn't get far. After they caught me, I was quickly branded and sent to this camp. 

During my time here, I constantly thought about what I did. I hated myself for my weakness in allowing my hatred to get the better of me. I loathed how I let my family down by failing to gather the food they were counting on me for. And worst of all, I had slain my own kin at the mere worry he would betray me. These feelings festered, until I couldn't take it anymore. In those moments, I wanted to die.

Now that it's over, I don't mind telling you this. The plan to escape? When I created it, I didn't think it would succeed. I thought that my friends and I would die fighting the guards. And I was ready for that. Wanted it. It wasn't until Márcos saved Carter and I in the tunnel that I even considered wanting to keep on living. Watching Márcos almost die for us, watching him disobey his Overseer, seeing that he considered us worth saving despite our crimes, made me think that maybe my life was worth saving too.

When we were stuck in the mine together, Márcos asked me if I could forgive him for what he had done. Before I answered, I told him my tale and asked him if there was forgiveness enough in the world for me too. He said if there could be for him, then there certainly was for me. And that thought got me through everything that came after. I want to live long enough to find out if that kind of forgiveness exists.

{
-letNandorDecideGuardPunishments:
    Why can't I bring myself to judge another? I have killed my own kin, a crime for which I can muster no forgiveness. I forgive the crimes of others so readily because so much of my energies are tied up in loathing for myself that I cannot bring myself to loath another. And until I can find a way to unclench my spirit, that way I will remain.

-else:
    You ask me of grudges? What are they now to me. No grudge can ever match the one I hold against myself. I forgive and forget so easily because so much of my energies are tied up in those feelings that I can hold no space in my mind for any others. And until I can find a way to unclench my spirit, that way I will remain.
}

  
    +Your history of murdering your traveling companions unnerves me. Don't think I will be so easy a victim as your cousin.
    
        I understand why it would. You have never given me any reason to wish harm upon you, and I strive to master myself every day. You won't need to consider me a threat, I promise you.
            ->2e
            
    +You should have brought this up sooner. I can respect anyone who slays a tax collector.
    
        \*Nándor eyes you quizzically* That is certainly a new perspective on my tale. I'm not sure you were listening as aptly as I would have liked.
    
        ->2e
    +That is quite the tale. For what it's worth, I prize the Nándor who fought with me against the guards over the man who killed his cousin. They are two different men to me.
    
        It means a great deal to me that you would say that. There are parts of me that wish to believe that we are different people, and there are parts that won't let me. For you to think of them apart gives me some hope that one day I can too.
        ->2e



=== 2e ===

{
-letNandorDecideGuardPunishments:
    //empty on purpose
-gaveAGuardToTheCrowd:
    finishQuest(Deal With the Prisoners, true, 21)
-else:
    finishQuest(Deal With the Prisoners, true, 19)
}

{
-spokeWithPageBeforePrisoners:
    activateQuestStep(Leave the Camp, 3)
-else:
    activateQuestStep(Leave the Camp, 1)
}

{refusedToHearNandorsTale:Very well. }On to other matters. With the prisoners no longer a concern, we can start thinking about leaving this wretched camp. I've placed Kastor in charge of getting the others prepared to leave, and he has informed me that the stores we found within the Manse were less than we expected. 

->3a
=== 3aa ===

I've been placed in charge of getting the others prepared to leave. While I was tallying the food supplies we liberated from the Manse, I discovered that they were far less ample than we expected. 

->3a

=== 3ab === //skipped guard punishments because no prisoners

\*Nándor turns to you and speaks so only you can hear him beneath the noise of the crowd.* I, like any other slave, have dreamt long dreams of what this moment would be like. The truth of what has happened has yet to sink in fully: part of me fears this may still <i>be</i> a dream. 

Kastor and I have been taking a count of everyone who survived the riots, to better understand our food situation. As we tallied the survivors, I noted somberly many of those I've worked beside did not make it. Whether slain in the mine's evacuation, or the rioting, the loss of life has not been small.

I am forced to wonder how much our actions played a roll in that. Had we taken more prisoners, would the butcher's toll have been as high as it was? Had we allowed the guards to surrender, perhaps they would not have fought so hard. And of course, I wish Márcos had lived to see this moment. His sacrifice will not go without remembering.

    +You're bringing down the mood, Nándor. Now is a time for merriment, not sober reflection.
        I'm sorry, merriment does not come easily to me. I am consumed by thoughts of what could have been. While we rioted I allowed my anger to carry me through, but now that its all over, my thoughts remain fixated on the dead. Even the guards I slew: were they fated to die by my hand? Was there no possible future where we could have both lived apart from one another?
        ->3aba
    +We are only human. We made the choices we made; now we must live with them.
        You are right, of course. Nothing can be done about it now. But even as my mind knows this to be true, my heart bleeds to learn if things could have been different. With the fighting over, my thoughts remain fixated on the dead. Even the guards I slew: were they fated to die by my hand? Was there no possible future where we could have both lived apart from one another?
        ->3aba
    +Taking prisoners would have been a waste of time. We'd just end up executing them all anyways.
        What of Márcos then? Would you have executed him as well, despite all he has done for Carter and I? I don't know how you can dismiss these worries so easily. Part of me knows I should be celebrating, and yet my thoughts remain fixated on the dead. Even the guards I slew: were they fated to die by my hand? Was there no possible future where we could have both lived apart from one another?
        ->3aba

=== 3aba ===

    +Few of the slaves will lament the guards they killed. You have an easy time letting go of grudges. Why is that?
        ->2da

=== 3a ===

It seems that the guards, in one final act of malice, destroyed some of their stocks to prevent us from being able to capture them. Despite our well-earned freedom, we will need to hold off on any feasting to prevent the wasting of food. While we have enough to keep the freed slaves sustained for the time being, if we implement rationing, it may not be enough to keep them fed for the entire journey through the forest. 

{
-kastorDiscussingFoodShortage and sworeToBurnCsalansBody:

    I have heard that you were present when the Director's horses were slain, and that you swore an oath to prevent us from eating them. I would not ask you to consider breaking it if things were not dire, but the meat from the horse's bodies would go a long way towards bolstering our food supply. The slaves were already malnourished while the guards still ruled the camp; I'm afraid that stretching our meager stocks too thin could cause some of our less sturdy comrades to starve.
        ->3b
-kastorDiscussingFoodShortage:

    I was not there for these events, but the Director's horses were slain during the fighting that occured in the Manse. Those horses were well cared for, and had a decent amount of meat on their bones. Nándor and I took it upon ourselves to order the bodies kept from the others so that they could be properly butchered and preserved, but I wanted to inform you about this so you could have the final word on how the meat is divvied up.
        ->3b
-not foughtHorsesInManse:

    Some of prisoners that fought their way into the Manse reported that they slew the Director's horses. Those horses were well cared for, and had a decent amount of meat on their bones. Kastor and I took it upon ourselves to order the bodies kept from the others so that they could be properly butchered and preserved, but I wanted to inform you about this so you could have the final word on how the meat is divvied up.

        ->3b
-sworeToBurnCsalansBody:

    I witnessed your oath to Csalan. And I would not ask you to consider breaking it if things were not dire. But the meat from the horse's bodies would go a long way towards fighting our current food insecurity. The slaves were already malnourished while the guards still ruled the camp; I'm afraid that stretching our meager stocks too thin could cause some of our less sturdy comrades to starve.
        ->3b
-foughtHorsesInManse:

    I was there when we fought the Director's horses. After the battle, I took it upon myself to order the horses' bodies kept from the others so that they could be properly butchered and preserved, but I wanted to inform you about this so you could have the final word on how the meat is divvied up.

        ->3b
}

=== 3b ===

    {
    -sworeToBurnCsalansBody:
        +I take my oaths seriously. I won't break my word, no matter how bad things are.
            ->orderedTheHorsesBurned(->3d)
    }
    {
    -foughtHorsesInManse:
        +Exactly how dire is our food situation?
            ->3baa
        +They were able to converse as humans do. How can you even consider eating from their corpses? 
            ->3ba
        +I have come to a decision.
            ->3c
    -else:
        +Exactly how dire is our food situation?
            ->3baa
        +I have come to a decision.
            ->3c
    }

=== 3baa ===

Carter has told me the journey through the forest took his team five days. A group as large as ours only moves at the pace of it's slowest members, and the ground we must cover is anything but friendly. All told? The journey could take a week, or more. 

If we were to cut our rations to a half of what they were before the revolution, we may be able to stretch the food we would bring with us for the entire journey. It might also be possible to bolster our stocks with some hunting or foraging, but that in turn would lengthen the journey due to the slower pace. 

Half of our current rations would be hard pressed to feed a small child, let alone an already malnourished person of working age. The horse meat would do much to preserve our health during the journey to civilization.

    ->3b

=== 3ba ===
{
-kastorDiscussingFoodShortage:
    It is not a decision I make lightly, believe me. But the grim reality of the situation is that they are dead, and we are not. Eating them hurts no one; letting their bodies go to waste puts us at serious risk of starvation.
-foughtHorsesInManse:
    It is not a decision I make lightly, believe me. When we fought the horses, I assisted with their dispatching because they stood between my comrades and freedom, not because I wished to eat them. But the grim reality of the situation is that they are dead, and we are not. Eating them hurts no one; letting their bodies go to waste puts us at serious risk of starvation.
-else:
    It is not a decision I make lightly, believe me. I did not kill the horses, but if I had it would have been because they stood between my comrades and freedom, not because I wished to eat them. With that said, the grim reality of the situation is that they are dead, and we are not. Eating them hurts no one; letting their bodies go to waste puts us at serious risk of starvation.
}

    {
    -sworeToBurnCsalansBody:
        +It may not hurt any of you, but the Gods do not look kindly on oath-breakers. I would be the one to suffer for this decision.
            keepDialogue()
            I know, and whatever you decide, I will defend it before the others. 
            ->3c
    }
    {
    -not noPrisoners:
    +I was told that the trials were meant to prove that we are not the brutes the Lovashi think we are. I expect the Lovashi would rather starve before they resort to what they saw as cannibalism. Surely we are no less moral than they.
        {
        -kastorDiscussingFoodShortage:
            keepDialogue()
            Perhaps those morals would have been better served by not having the crowd rip the prisoners apart. I see little point in sticking to them now, after having discarded them so recently.
        -else:
            keepDialogue()
            \*Your words are met with a sheepish frown.* Your point is fair. It does not sit well in my heart that this decision must be made. And I feel even worse that I have forced it upon you instead of making it myself. For that, I am sorry.
        }

       ->3c
    }

    +I've come to a decision.
        ->3c
=== 3c ===

    I will support whatever you decide.

{
-charisma >= 2:
    +Have the horses butchered, but have it done in secret. Only my personal companions deserve to share in their bounty. <Cha {charisma}/2>
        ->3h
}
{
-sworeToBurnCsalansBody:
    +My oath stands. Burn their bodies.
        ->orderedTheHorsesBurned(->3d)
-else:
    +I won't allow the horses to be eaten. Their bodies must be burned instead.
        ->orderedTheHorsesBurned(->3d)
}

    +Butcher the horses and add their meat to the rations. Make sure everyone gets the same amount.
        setToTrue(sharedTheHorseMeat)
        ->orderedTheHorsesEaten(->3ea)
    +Keep the largest carcass for the leaders of the revolution. Add the rest to the rations.
        setToTrue(sharedTheHorseMeat)
        ->orderedTheHorsesEaten(->3f)
    +It would be a blow to morale to keep the freed prisoners from celebrating. Prepare a feast from the horsemeat.
        setToTrue(sharedTheHorseMeat)
        setToTrue(celebratedWithTheHorseMeat)
        ->orderedTheHorsesEaten(->3g)



=== 3d ===

I will personally see that your orders are carried out. The others may not understand, but should they voice their qualms around me, I will defend your decision.

->3j

=== 3ea ===

finishQuest(Food Shortage, true, 3)

->3e(->3j)

=== 3e(->divert) ===

{
-sworeToBurnCsalansBody:
{kastorDiscussingFoodShortage:*Kastor|*Nándor} nods.* I know it could not have been an easy decision to make, but it had to be made. Just know, I believe that you have saved lives by making it. May the Gods be merciful in their punishment.
-else:
{kastorDiscussingFoodShortage:*Kastor|*Nándor} nods.* I know it could not have been an easy decision to make, but it had to be made. Just know, I believe that you have saved lives by making it.
}

->divert

=== 3f ===

finishQuest(Food Shortage, true, 4)

prepItem()

->3e(->3fa)

=== 3fa ===

giveItem(0,0,7)

->3j

=== 3g ===

finishQuest(Food Shortage, true, 5)

I question whether this is the best use of our food, but the others will be overjoyed. And perhaps that is all that matters at this time.

->3j

=== 3h ===

The others will certainly react poorly should they ever learn of this. I will do my best to keep it a secret... but I'm not sure where in the camp the smell of cooked meat will go unnoticed. 

    +I don't care how you do it, just get it done.
        setToTrue(hoardedTheHorseMeat)
        ->orderedTheHorsesEaten(->3i)
    +I see. Maybe I should reconsider.
        ->3c

=== 3i ===

finishQuest(Food Shortage, true, 6)

prepItem()

I will see to it personally.

giveItem(0,0,20)

->3j

=== 3j ===

With that taken care of, {kastorDiscussingFoodShortage:Nándor|Kastor} and I will see to the rest of the preparations, so you don't need to worry about the specifics. But we need to be certain of the route through the forest. Carter and Page want to discuss that with you. They said you should meet them by the camp's exit when you're ready. 

->FinalClose

=== 4a ===
setToFalse(afterNandorDecidesGuardPunishments)

I'm glad that is over. It makes me... uncomfortable to pass judgement on others.


    +Why? Your judgements seemed more than fair.
        keepDialogue()
        You are kind to say that. As for why? It's a long story, but I will tell it if you wish to hear it in it's entirety.
        ->2da
    +I expect that is an uncommon trait among the branded. What has made you this way?
        keepDialogue()
        And <i>I</i> expect you are correct. As for why? It's a long story, but I will tell it if you wish to hear it in it's entirety.
        ->2da

=== orderedTheHorsesBurned(->divert) ===

{
-sworeToBurnCsalansBody:
    finishQuest(Food Shortage, true, 1)
}
setToTrue(orderedTheHorsesBurned)

->divert

=== orderedTheHorsesEaten(->divert) ===

{
-sworeToBurnCsalansBody:
    activateQuestStep(Food Shortage, 2)
}

setToTrue(orderedTheHorsesEaten)

->divert

=== 10a ===

Leave me be. I no longer wish to speak with you after what you've done.

->Close

=== nandorLeavesParty ===

I had more I had wished to discuss, but I can no longer stand to speak with you. Kastor knows the details as well as I do, you should discuss with him what needs to be done before the other freed slaves are ready to leave the camp.

setToTrue(canSpeakToKastorAboutFoodShortage)
setToTrue(nandorLeftParty)
setToTrue(nandorLeftPartyOverPrisonerPunishment)

removeFromParty({nandorIndex})

activateQuestStep(Food Shortage, 0)

{
-spokeWithPageBeforePrisoners:
    activateQuestStep(Leave the Camp, 3)
-else:
    activateQuestStep(Leave the Camp, 2)
}

finishQuest(Deal With the Prisoners, true, 20)

->FinalClose

=== FinalClose ===

{
-kastorDiscussingFoodShortage:
setToFalse(kastorDiscussingFoodShortage)
}

fadeToBlack()

deactivate({nandorIndex})
deactivate({carterIndex})
//deactivate({kastorIndex})
deactivate({marcosIndex})
deactivate({andrasIndex})
deactivate({taborIndex})
deactivate({crowdIndex})
deactivate({clayIndex})
deactivate({garchaIndex})
deactivate({thatchIndex})

setToTrue(crowdDispersed)

fadeBackIn(60)

close()

->DONE

=== Close ===

close()

->DONE