VAR playerName = ""

VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR coins = 0

VAR nandorIndex = 1
VAR carterIndex = 2
VAR kastorIndex = 3 
VAR janosIndex = 4
VAR broglinIndex  =5
VAR garchaIndex = 6
VAR slaveOneIndex = 7
VAR slaveTwoIndex = 8
VAR slaveThreeIndex = 9
VAR crowdIndex = 10
VAR taborIndex = 11
VAR marcosIndex = 12
VAR andrasIndex = 13
VAR rekaIndex = 14
VAR pazmanIndex = 15
VAR ervinIndex = 16
VAR clayIndex = 17

VAR letTaborLive = false
VAR acceptedTaborsSurrenderAfterDirectorFight = false
VAR acceptingGuardPrisoners = false
VAR mineLvl3MarcosDiedSealingBreach = false
VAR agreedToBeLeader = false
VAR broglinFreed = false
VAR directorDefeated = false
VAR gotKeyFromJanos = false
VAR andrasIsDead = false
VAR afterNandorDecidesGuardPunishments = false
VAR letNandorDecideGuardPunishments = false
VAR mineLvl3ConvincedRekaAndPazman = false

VAR deathFlagCarter = false
VAR deathFlagNándor = false
VAR deathFlagGuardMárcos = false
VAR deathFlagGuardAndrás = false
VAR deathFlagChiefTabor = false

VAR noPrisoners = false
VAR marcosIsAtTrial = false
VAR andrasIsAtTrial = false
VAR taborIsAtTrial = false
VAR janosIsAtTrial = false
VAR guardPazmanAndRekaAtTrial = false

VAR nandorDialogueFileIndex = 1

{
-not taborIsAtTrial:
    deactivate({taborIndex})
}

{
-not marcosIsAtTrial:
    deactivate({marcosIndex})
}

{
-not andrasIsAtTrial:
    deactivate({andrasIndex})
}

{
-not janosIsAtTrial:
    deactivate({janosIndex})
}

{
-not guardPazmanAndRekaAtTrial:
    deactivate({rekaIndex}) 
    deactivate({pazmanIndex})
}

moveLocalPosition(-29.5,-21.35)
setfacing(NW)

->1a

=== 1a ===

healParty()

changeCamTarget({nandorIndex})

My fellow freed men and women, hear me! The Director has been defeated! The guards are broken! Never again will you wear their shackles and perform their labors! Rejoice, for it was the blood we all shed that made this possible!

changeCamTarget({crowdIndex})

\*The gathered former slaves erupt in cheers. The air is electric with their joy.*

setfacing(SW)
changeCamTarget({nandorIndex})

{
-noPrisoners:
    ->6a
}

\*Nándor turns to you and speaks so only you can hear him beneath the noise of the crowd.* I have gathered all of the guards that were taken as prisoners that I could find, so that we may pass judgement on them. This way, the other branded can see who has helped us and who has hindered us, and will know who to treat as a friend once we exit the camp.
{
-agreedToBeLeader:
    ->2a
-else:
    ->2d
}

=== 2a ===

As our leader, it should fall to you to pass judgement on them. The others may wish to take punishment into their own hands, but we must not behave as animals. I want to prove beyond any doubt that we are better than the Lovashi think we are.

    +I understand. I will address the crowd now.
        ->agreedToBeJudge
    +They're slavers. Why wouldn't we just execute them all and have done with it?   
        ->2b(->2ba)
    +I'm not one to pass judgement over others. Perhaps you should be the one to do this.
        ->2da
    +I shall lead the branded in our revenge. These guards are powerless now; we can finally have a little fun with them.
        ->5a


=== 2b(->divert) === //why not kill them all

{
-not deathFlagGuardMárcos and gotKeyFromJanos and not (deathFlagGuardAndrás or andrasIsDead):

I understand the sentiment. But some of those arrayed here were instrumental to the success of the plan. Such as Márcos saving Carter and I. Or Guard András surrendering the key to the mine's armory. Surely these acts deserve some consideration?

-not deathFlagGuardMárcos:

I understand the sentiment. But some of those arrayed here were instrumental to the success of the plan. Such as Márcos saving Carter and I. Shouldn't the magnitude of his heroism affect his sentence some? Or even perhaps the sentences of his former compatriots? 
    
-gotKeyFromJanos and not (deathFlagGuardAndrás or andrasIsDead):

I understand the sentiment. But some of those arrayed here were instrumental to the success of the plan. Such as Guard András surrendering the key to the mine's armory. Surely an act like that should give us pause? Even if it is only for a moment?

-else:

I understand the sentiment. But many of the workers of this camp fell victim to that same warped logic when they were given their brands. I would not have us stoop to the level of our captors, even when given the chance to deal them back in kind.
}

{
-letTaborLive and not acceptedTaborsSurrenderAfterDirectorFight:

There is also the issue of Chief Tabor. I pulled him from a group of rioters as we left the Manse. They had disarmed him and were taking it in turns to stomp his guts out. Had I not dispersed the mob and dragged him back to Kastor, he would certainly be dead. 

}

{
-letTaborLive:

{acceptedTaborsSurrenderAfterDirectorFight:There is also the issue of Chief Tabor. }I care not what punishment he receives: the less I have to think about him, the better I'll feel. But of all the prisoners, he is the most likely to be attacked if given his freedom. And I don't wish to put him into the arms of the crowd. They'll certainly kill him horribly. When you decide his fate, all I ask is you consider that executing him may be the most merciful option.

}


->2ba

=== 2ba ===

{
-not deathFlagGuardMárcos and gotKeyFromJanos and not (deathFlagGuardAndrás or andrasIsDead):

    +I had not realized Márcos and András were even being considered for punishment.
        
        I wanted to preempt any of the other freed slaves from taking justice into their own hands, so keeping them guarded with the other prisoners was pragmatic. I only lined them up here to give me a chance to tell their stories to the crowd. 
        
        ->2ba

-not deathFlagGuardMárcos:

    +I had not realized Márcos was even being considered for punishment.
        
        I wanted to preempt any of the other freed slaves from taking justice into their own hands, so keeping him guarded with the other prisoners was pragmatic. I only lined him up here to give me a chance to tell his stories to the crowd. 
        
        ->2ba
    
-gotKeyFromJanos and not (deathFlagGuardAndrás or andrasIsDead):

    +I had not realized András was even being considered for punishment.
        
        I wanted to preempt any of the other freed slaves from taking justice into their own hands, so keeping him guarded with the other prisoners was pragmatic. I only lined him up here to give me a chance to tell his stories to the crowd. 
        ->2ba

}

    {
    -agreedToBeLeader:
    +I see. I will give our prisoner's punishment some thought then.
        ->2c
    +I am not the person who should be making that decision. The privilege of a leader is to delegate, and my choice is you.
        ->2da
    -else:
    +I see. I will give our prisoner's punishment some thought then.
        ->2c
    +We both agreed that you would be the leader. You should be making these decisions.
        ->2da
    }

    +An eleventh hour change of temperament won't sway me. Execute the lot of them and be done with it.
        ->4a


=== 2c === 

I'm glad we agree. Speak with each prisoner in turn and listen to what they have to say. Once you have heard their piece, address the crowd and announce your judgement. I will see to it that each sentence is carried out, whatever you decide. 

->2ca

=== 2ca ===

    +Even should you oppose that sentence?
        
        Yes. I don't need an excuse to set most of the prisoners free. They are powerless now; my fight with them is over. And should you decide to execute any of them, I will ensure that execution is swift and painless. It would be a more merciful end than giving them to the crowd.
    
        {
        -letTaborLive:

        There is also the issue of Chief Tabor. I care not what punishment he receives: the less I have to think about him, the better I'll feel. But of all the prisoners, he is the most likely to be attacked if given his freedom. And I don't wish to put him into the arms of the crowd. They'll certainly kill him horribly. When you decide his fate, all I ask is you consider that executing him may be the most merciful option.

        }

        ->2ca
    +Very good. Excuse me.
        ->agreedToBeJudge

=== askedForAdviceOnJudgements ===


{
-letTaborLive:

{acceptedTaborsSurrenderAfterDirectorFight:There is also the issue of Chief Tabor. }I care not what punishment he receives: the less I have to think about him, the better I'll feel. But of all the prisoners, he is the most likely to be attacked if given his freedom. And I don't wish to put him into the arms of the crowd. They'll certainly kill him horribly. When you decide his fate, all I ask is you consider that executing him may be the most merciful option.

}

->2c

=== 2d ===
combineDialogue()

We spoke before of who would be leader when we addressed the others, and you declined the role. However, just for this moment, I must ask you to reconsider. I can play the part of leader when it is necessary, but

->2da

=== 2da ===

I will admit that there is also a part of me that finds myself unworthy to pass judgement on others. That is why I had hoped I could pass the role of judge on to you.

    ->2db

=== 2db ===

    +You think I would do any better?
        ->2f
    +How will I know what to decide?
        ->2fa
    +You were the one who organized this whole affair. I think you should lead it.
        ->2e
    +They're slavers. We should simply execute the lot of them.   
        ->2b(->2db)
    +I shall lead the branded in our revenge. These guards are powerless now; we can finally have a little fun with them.
        ->5a
    +Fine, I'll do it.
        ->thankYouBeforeAgreed


=== 2e ===

I considered it, but my principles are conflicted here. I wish to use this trial to prevent the others from implementing their own, harsher justice, and at the same time I can't bring myself to trust any court with myself at it's head. 

    +For what it's worth, I trust your perspective, and so do the others.
        ->2ea
    +I can understand that. One's self doubts can be powerful deterrents.
        ->2eb
    +Shirk your duties if you wish, but I'm not doing this.
        Fine, if there is not other option then I shall speak with the crowd. But so far as I am concerned, you have just forfeited your right to complain about any of my decisions.
        ->3ba
    +I wouldn't trust you with something like this either. Once again, it falls to me to lead.
        If you meant to wound me, you've succeeded. If you are to do this, perhaps approach it with a bit more tact?
        ->agreedToBeJudge
    +I did not realize the depths of your conflict. I will do this for you.
        Thank you. You've relieved me of a great burden.
        ->agreedToBeJudge

=== 2ea ===

You are kind to say that. But I still wish for you to act as judge.

    +I will do it.
        ->thankYouBeforeAgreed
    +I cannot do this for you, Nándor. It must be you. If you cannot trust in yourself, then trust in my trust in you.
        ->3bb
    +Whatever your doubts are, I've got them worse.
        ->3a

=== 2eb ===

keepDialogue()

Sometimes, I think they are the most powerful. Please, would you do this for me?
    ->2ea

=== 2f ===

I've seen the way you fight. The way you speak. The others know how much you've done for us: I believe they'll listen to you, and respect whatever you decide.

    ->2db

=== 2fa ===

Speak with each of the prisoners. Hear what they have to say. Use their testimonies and your own experiences with each of them to make this decision. Put aside your worries, I trust you to decide what is best.

->2db

=== 3a ===

You lead us in combat. You know the sacrifices the branded have made to free ourselves more than any other. Your voice will hold the most weight when passing judgement on our former enemies.

    +I will not sit as judge. It must be you.
        ->3b
    +Fine. I shall consider their sentences.
        ->agreedToBeJudge

=== 3b ===

As you wish. I shall speak with the crowd. 

    ->nandorIsJudge

=== 3ba ===

    +Of course.
        ->nandorIsJudge
    +Yeah, whatever.
        ->nandorIsJudge
    +I'll complain no matter what you do.
        ->nandorIsJudge

=== 3bb ===

    I think I can do that. Very well. I shall decide what will become of the prisoners. Now, stand back, so I may address the others.

    +Of course.
        ->nandorIsJudge

=== nandorIsJudge ===
setToTrue(letNandorDecideGuardPunishments)

{
-guardPazmanAndRekaAtTrial:

fadeToBlack(true, false)

activate({rekaIndex})
activate({pazmanIndex})

fadeBackIn(60)

}

->3c

=== 3c ===

\*Nándor addresses the crowd.* I shall now pass sentencing. Listen well, and heed my verdicts, for those that contradict them shall be seen as enemies of this court!

changeCamTarget({crowdIndex})

\*The crowd begins to settle down, awaiting Nándor's words.*

{
-marcosIsAtTrial:

changeCamTarget({nandorIndex})

\*Nándor points at Márcos.* This is Guard Márcos! Down in the mine, I came the closest to death I have ever been. Had he not stood between me and my fate, I would not be speaking these words to you now. I now bid him rise to his feet and hold his head high, just as I bid you to treat him as a true comrade. 

changeCamTarget({crowdIndex})

\*The crowd voices it's elation as Márcos stands up.*

changeCamTarget({marcosIndex})

\*Nándor cuts the bonds on Márcos's wrists, and Márcos speaks to the crowd.* Your mercy does you all credit. There were many lies spoken about the conduct of the Craft Folk, some of which I am ashamed to admit I believed. Now, the truth of the goodness of your character has never been more plain to me. Thank you. 
}

{
-andrasIsAtTrial:

changeCamTarget({nandorIndex})

\*Nándor points at András.* This is Guard András! After the Director was defeated, I took the time to discuss his actions with Kastor and Janos. Each of them has spoken at length of both András's sympathy for our plight, and his aid to our cause. When approached to aid the revolution, András not only assisted our plans, but hid them when approached by other guards. For the assistance he rendered, as well as the danger he braved on our behalf, I bid him stand among greatful friends.

changeCamTarget({crowdIndex})

\*The crowd shouts joyously as András gets to his feet.*

changeCamTarget({andrasIndex})

\*Nándor cuts András's bond, and András turns to face the crowd. Meeting their eyes only briefly, András bows a very deep, rigid bow.* For my part in what you have been through, words can not express the depths of my apology. Your kindness in letting me live is testament to how wrong it was that you suffered so.
}

{
-guardPazmanAndRekaAtTrial:

changeCamTarget({nandorIndex})

\*Nándor points at Réka.* This is Guard Réka! My companions and I accepted her surrender after her squad helped us prevent more worm creatures from entering the mines. She surrendered rather than fight alongside her fellows, and avidly attempted to persuade them to give up their arms as well. While she did not side with the revolution until pressed, her sympathies and lack of resistance do her some credit. My verdict for her shall be fifty lashes, after which she shall have paid her debts to the branded.

changeCamTarget({crowdIndex})

\*The crowd's reaction is mixed, some cheering while others shouting in anger. It's hard to tell whether either reaction is more aimed at Réka, or Nándor*

changeCamTarget({rekaIndex})

\*Nándor assists Réka to her feet, and motions for two branded to take her away from the crowd. Réka's face is sullen, and she has trouble keeping her eyes off the jeering crowd.*

fadeToBlack(true, false)

deactivate({rekaIndex})

fadeBackIn(60)

changeCamTarget({nandorIndex})

\*Nándor points at Pázmán.* This is Guard Pázmán! We accepted his surrender at the same time as Réka. While he took more convincing than Réka did, he too gave up his arms rather than fight to keep our freedom from us. For his part, I also sentence him to fifty-

changeCamTarget({ervinIndex})

\*Ervin waves his arms to get Nándor's attention, his croaking voice straining to be heard.* Wait! I would speak! 

changeCamTarget({nandorIndex})

Quiet everyone! Ervin's condition affects his voice, allow him to speak his piece.

changeCamTarget({ervinIndex})

Pázmán was among the guards who branded me! I witnessed his mistreatment of another slave, and he sought to silence me. Now everytime I speak, until my last day, I will be reminded of him from the pain in my throat. This monster laughed while he maimed me: I would see him die for it!

changeCamTarget({crowdIndex})

\*The crowd bursts into a wave of angry shouts. Their message is clear: they call for Pázmán's blood.*

changeCamTarget({nandorIndex})

This is alarming news. In light of your words, Ervin, and Pázmán's horrific deeds, the course forward is clear. Pázmán's sentence is death, which I will personally carry out.

changeCamTarget({crowdIndex})

\*The crowd's anger turns to rapture as Nándor raises his sword high.*

changeCamTarget({nandorIndex})

\*Nándor brings his sword down on Pázmán's neck with a swift, two-handed strike. Pázmán's head separates from his body, and tumbles into the dirt.*

changeCamTarget({crowdIndex})

\*The crowd continues it's cheers as Nándor has Pázmán's body removed from the yard.*

fadeToBlack(true, false)

kill({pazmanIndex})

fadeBackIn(60)
}

{
-taborIsAtTrial:

finishQuest(An Uneasy Truce, true, 10)

changeCamTarget({nandorIndex})

\*Nándor points at Tabor.* You all know this man. This is Chief Tabor. The man responsible for how the guards conducted their campaign of torture and abuse. Despite his high station, and a number of guards under his command, Tabor personally took part in many of the evil acts that were perpetrated against us. Save for the Director, he is the guard most responsible for our agonies.

changeCamTarget({crowdIndex})

\*The crowd's reaction is visceral. Over the general din of anger, some slaves shout suggestions for his punishment.* Flog him! Brand him! Set him alight!

changeCamTarget({nandorIndex})

I personally have been the recipient of Chief Tabor's 'lessons' on many occasions. I can remember on one instance, part way through a shift in the mine, I found my palms too blistered to swing a pick. The ratty, cloth gloves I had been given had long worn through, and the rough wood of the pick's handle dug deep into my hands. The blood and pus from my wounds, along with my weak grip, kept the pick from sitting in my hands.

As I struggled to work, my overseer witnessed my predicament and, mistaking it for insubordination, sent me topside to be disciplined. My relief at leaving the mines was short lived, of course, for soon I found myself before Tabor. Tabor had me tied to a post outside the mine's entrance, and gave me a long speech meant to teach me... something. Now that I am free, I can safely admit I was too tired at the time to absorb much of it.

I think I may have even fell asleep while he spoke, but I was awakened when the other miners ended their shift. As the rest of you left the mine entrance, the flogging began. As Tabor was fond of saying, 'a private flogging is a lesson for one. A public flogging is a lesson for all.' I was flogged until the rest of you had been brought back to your huts, then I was brought to mine.

The pain he inflicted that evening left me unable to move for days. I was seen to by the guards, who washed both the wounds I received upon my back, as well as those on my hands, to prevent infection. Once Tabor deemed me fit for duty again, he gave me a new pair of work gloves, this time made of leather, and sent me back to work. It would not be the last time I would be brought before him.

changeCamTarget({crowdIndex})

\*The crowd, at the outset of Nándor's speech, was loud and boisterous. Now, it listens to his words, nodding along to his recollections of their shared experience.*

changeCamTarget({nandorIndex})

During the riot, I would not have given much thought to slaying a guard who stood between me and freedom. My heart was resolved to be free, and it produced the rage necessary to see that fighting through. But now my heart is tired. Tired of the fighting, tired of the pain. And while my hands have the opportunity to pay Tabor's 'lessons' back in kind, my heart is too tired to see it done.

I ask those among you who wish him harm, what good would it do you? How does it benefit us, to see this man die in pain? I would rather have it be done with, and leave this camp all the quicker. Just existing within this camp plagues me with painful memories, and every second I would prolong his life in suffering, so too would I prolong my own by staying here. For that reason, so as to save myself and those like me that suffering, I sentence him to death, to be carried out as quickly as it can be.

changeCamTarget({crowdIndex})

\*The crowd seems conflicted. They murmur among themselves, unable to decide on whether to agree or disagree with Nándor's verdict.*

changeCamTarget({nandorIndex})

\*Without waiting for the crowd's opinion, Nándor raises his sword and beheads Tabor with a powerful chop to the neck.*

changeCamTarget({crowdIndex})

\*With the deed done, some of the crowd applaud weakly, while others begin to leave. After a time, it disperses.*

fadeToBlack(true, false)

kill({taborIndex})
deactivate({clayIndex})
deactivate({crowdIndex})

fadeBackIn(60, false)

-else:
changeCamTarget({crowdIndex})

\*With the verdicts over, the crowd begins to disperse.*

fadeToBlack()

deactivate({clayIndex})
deactivate({crowdIndex})

fadeBackIn(60)
}

finishQuest(Deal With the Prisoners, true, 22)
setToTrue(afterNandorDecidesGuardPunishments)

->Close

=== 4a ===

Are you certain? Among those gathered, you cannot see any reason to let any of them live?

    +Let us stop wasting time on this and have it done.
        ->executeEveryone
    +If you wish to pass judgement, I won't stop you. I was only giving my opinion because you asked it of me.
        If that is the level of thought you're going to put into this, then I am left with no choice but to intervene. Gods grant me the wisdom to judge fairly.
        ->nandorIsJudge
    +Perhaps I was a bit hasty. I will speak with the prisoners, and then announce their sentences.
        ->thankYouBeforeAgreed

=== 5a ===

The point of this court is to keep the branded from taking justice into their own hands, not to give into our baser instincts. Please, don't allow this to devolve into petty revenge.

    +Why are you so concerned for the wellbeing of your captors? I would have expected you to want to join in.
        keepDialogue()
        I have been in this camp long enough. I have seen enough pain. While I endured the whips of the Lovashi, I wished to be free, not to be the one holding the whip. I don't want that for my comrades either.
        ->5a
    +My people have suffered greatly. It is only right that they get to return that suffering in kind.
        ->5b
    +I see your point. I'll proceed with some caution.
        ->thankYouBeforeAgreed

=== 5b ===

Right? Is there a right way to inflict suffering?

    +When it's been done to you, yes.
        ->5c
    +The punishment should fit the crime, and the worst crimes demand the worst punishments.
        ->5e
    +When I do it, it's right.
        ->5ba
    +I won't be lectured into forbidding violence against slavers. Don't even try.
        ->wontFightYou

=== 5ba ===

No words could be more hypocritical. And who chose you for this privilege?

{
-agreedToBeLeader:
    +You did, when you asked me to lead.
        I don't have the authority to let you hurt these prisoners without limit! No one does! That's the point of this entire affair!
        ->5bb
-else:
    +You did, when you asked me to act as judge.
        I don't have the authority to let you hurt these prisoners without limit! No one does! That's the point of this entire affair!
        ->5bb
}
    +<i>I</i> fought my way through the mines. <i>I</i> conducted the branded in combat. <i>I</i> defeated the Director. My deeds have earned me this privilege.
        No one can deny that your deeds are great. But that doesn't give you the right to hurt these prisoners without limit! No one has that right! That's the point of this entire affair!
        ->5bb
    +The branded chose me when they answered my call to riot. They will follow my lead as I punish their tormentors.
        The branded need little excuse to hurt these prisoners. That's the point of this entire affair! To put limits on how far we debase ourselves!
        ->5bb

=== 5bb ===

    +You're too weak to make these decisions, that's why the duty falls to me.
        The more we speak on this matter, the more your true colors become apparent to me. Just know this: if you let the crowd involve themselves in carrying out your sentences, you should no longer consider us comrades.
        ->agreedToBeJudge
    +I won't hurt them without limit. Only to the amount that is fair for their crimes.
        I suspect our definitions of 'fair' are completely different. Just know this: if you let the crowd involve themselves in carrying out your sentences, you should no longer consider us comrades.
        ->agreedToBeJudge
    +I won't be the one hurting them. I'll leave that to the crowd you've gathered.
        If you let the crowd involve themselves in carrying out your sentences, you should no longer consider us comrades. I pray that you see reason before you allow the branded to become what the Lovashi fear us to be.
        ->agreedToBeJudge
    +If you don't like the way I'll handle this, the perhaps you should be judge.
        ->youLeaveMeNoChoice
    +You're right, Nándor. You've shown me the error of my words. I will do my best to judge the prisoners fairly.
        Thank you. Your ability to listen to those who disagree is a virtue. And by agreeing to stand as judge in my stead, you have lifted a great burden from my shoulders.
        ->agreedToBeJudge

=== 5c === //When it's been done to you, yes.

When you inflict that suffering on others, would that make their reprisal justified in turn? Has all suffering been justified except the first act?

    +When we leave this camp, no one will be left to come after us. We need not fear their reprisal.
        ->5cc
    +Yes.
        ->5ca
    +I don't care about that. I care about the now, and my revenge.
        ->5d
    +I get your point. I shall consider their sentences a bit more carefully.
        ->thankYouBeforeAgreed

=== 5ca ===

\*Nándor blinks.* That's ludicrous. You can't possibly believe that.

    +Suffering is the way of the world. If it weren't, the Gods would not have filled their world with so much of it.
        ->5cb
    +I won't sit here and allow you to call me a liar. I have prisoners to judge.
        ->gettingWorseAllTheTime

=== 5cb ===

combineDialogue()

Usually when someone expresses that sentiment it's as a lament, not as a call to add to the suffering. 

->nandorRegretsGivingPlayerResponsibility

=== 5cc ===

That wasn't the point of my question. And if you're planning to kill them all anyways, why not execute them and have it done with?

    +Because that wouldn't be as fun.
        ->5cca
    +You're right, that would save us some time. Execute the lot of them.
        ->executeEveryone
    +If you're so against this, then you should be judge in my place.
        ->nandorIsJudge

=== 5cca ===

    combineDialogue()

    You disgust me. 

        ->nandorRegretsGivingPlayerResponsibility

=== 5d ===

Your revenge? You are new to this camp, are you not? It sounds as if you have the least claim to vengeance of anyone here.

    +I was still taken from my home, same as all of you. The brand about my neck prevents me from ever returning.
        I can understand that loss; it will garner nothing but empathy from me. But hurting these prisoners will not bring you any closer to those you love.
        ->5da
    +I was a branded slave for a time before I was sent here. I have no less claim to revenge as any of you.
        Then our shared experience makes us as siblings. And as one sibling to another, I would not see you go down this road. Please, don't do this.
        ->5da
    +I have been put in more danger in the last day than I have experienced in my entire life. That demands satisfaction.
        You have done more for us than could be asked of anyone. And it was not right for us to demand so much of you. So I cannot force your choice, but simply beg you: please, do not do this.
        ->5da
    +Many slaves died in the riot. I have seen others or tortured, or even maimed. The revenge I speak of is communal. 
        I have witnessed what you speak of, and those same hardships are why I must ask you not to reciprocate. To let our friends descend to this level would be the final debasement; one last degradation they would have to suffer in this camp. 
        ->5da

=== 5da ===

    +Save your speech, Nándor. My revenge is fated: it won't be stopped by you.
        ->wontFightYou
    +You're... you're right. I will consider their sentences carefully.
        ->thankYouBeforeAgreed
    +Maybe your empathy makes you a better candidate. You should be judge.
        I had wished not to be forced into this position, but I can see your internal struggle rages worse than mine. I shall be judge in your place.
        ->nandorIsJudge

=== 5e === //The punishment should fit the crime, and the worst crimes demand the worst punishments.

The Lovashi thought as you do. That's what lead them to create this camp, the brand, their entire system! Do not allow us to act as they would.

    +The Lovashi do not care if we are innocent of our crimes. The difference is that I know that these guards are guilty.
        ->5ea
    +The Lovashi started this conflict. Now, I will end it.
        keepDialogue()
        Ignoring for a moment that there is no doubt in my mind the Lovashi would claim we started it, if you're planning to kill them all anyways why not execute them and have it done with?
        ->5cc

=== 5ea ===

That's the only difference between us to you? You have witnessed the horrors of this camp and can still think of a crime worth inflicting them for?

    +Yes. The crime of slavery.
        ->5ed
    +It's not the only difference. We smell better, for one.
        ->5ec
    +No, you're right. Nothing warrants all of this.
        ->5eb

=== 5eb ===

I'm glad we can agree on that. If you act as judge, will you consider each prisoner's punishment, and promise not to allow the crowd to carry out their punishments?

    +I'll play the role of judge, but I promise nothing.
        ->wontFightYou
    +I promise.
        ->thankYouBeforeAgreed
    +You should be judge. I'm not cut out for this.
        ->youLeaveMeNoChoice 
    +Just execute them all. It's not worth sorting them out.
        ->2b(->5eba)

=== 5eba ===

    +An eleventh hour change of temperament won't sway me. Execute the lot of them and be done with it.
        ->4a
    +I'll play the role of judge, but I promise nothing.
        ->wontFightYou
    +I promise.
        ->thankYouBeforeAgreed
    +You should be judge. I'm not cut out for this.
        ->youLeaveMeNoChoice 

=== 5ec ===

Would you please take this seriously? We're deciding the fate of real people here.

    +Don't think of them as people. They don't think of us that way.
        ->5ecb
    +I was only joking. You need to lighten up.
        ->5eca
    +I was being serious.
        combineDialogue()
        \*Nándor kneeds his eyebrows in annoyance.* 
        ->nandorRegretsGivingPlayerResponsibility


=== 5eca ===

I'm trying to impart the gravity of this situation to you. As much as it pains me to consider it, perhaps I should act as judge instead of you.

    +It's too late for that now. I am ready to begin.
        ->wontFightYou
    +Perhaps you should.
        ->youLeaveMeNoChoice

=== 5ecb ===

Many of the camp guards' held no empathy for us, it's true. But that lack of empathy was their worst quality. No part of me wants to emulate it.

    +Then I will make the hard decisions for you. I am ready to play the role of judge.
        ->wontFightYou
    +Maybe your empathy makes you a better candidate. You should be judge.
        ->youLeaveMeNoChoice

=== 5ed ===

\*Nándor shakes his head.* There is no worse crime, to be sure. But if a crime's punishment requires me to act as a slaver would, then I am not fit to act as punisher.

    +Then I will take your place. I am ready to begin.
        ->wontFightYou
    +But I would still trust you to be judge. You may find an outcome that I have missed.
        ->youLeaveMeNoChoice

=== nandorRegretsGivingPlayerResponsibility ===

I admit my regrets in asking you to be {agreedToBeLeader:leader|judge}.
    
    +It's too late for that now. I am ready to begin.
        ->wontFightYou
    +Then perhaps you should be judge in my place.
        ->youLeaveMeNoChoice

=== youLeaveMeNoChoice ===

You leave me no choice. Gods grant me the wisdom to judge fairly.
    ->nandorIsJudge

=== gettingWorseAllTheTime ===

The more we speak on this matter, the more your true colors become apparent to me. Just know this: if you let the crowd involve themselves in carrying out your sentences, you should no longer consider us comrades.

    ->agreedToBeJudge

=== wontFightYou ===

Fine, I won't fight you. Just know this: if you go through with this and let the crowd get involved, you should no longer consider us comrades.
    ->agreedToBeJudge

=== thankYouBeforeAgreed ===
Thank you. You have lifted a great burden from me.
    ->agreedToBeJudge

=== agreedToBeJudge ===

activateQuestStep(Deal With the Prisoners, 1)

{
-letTaborLive and acceptedTaborsSurrenderAfterDirectorFight:
activateQuestStep(An Uneasy Truce, 5)
-letTaborLive:
activateQuestStep(An Uneasy Truce, 6)
}

    ->Close

=== 6a === 

swapInkFile(0,noPrisoners,true)

->Close

=== executeEveryone ===

Very well. I will see that your judgement is carried out. Speak with me again once the deed is done.

fadeToBlack()

finishQuest(Deal With the Prisoners, true, 2)

{
-marcosIsAtTrial:
kill({marcosIndex})
setToTrue(executedMarcos)
}

{
-taborIsAtTrial:
kill({taborIndex})
setToTrue(executedTabor)
finishQuest(An Uneasy Truce, true, 8)
}

{
-andrasIsAtTrial:
kill({andrasIndex})
setToTrue(executedAndras)
}

{
-guardPazmanAndRekaAtTrial:
kill({pazmanIndex})
setToTrue(executedPazman)

kill({rekaIndex})
setToTrue(executedReka)
}

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE