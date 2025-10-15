VAR spokeToBalint = false
VAR spokeToSeb = false
VAR gotThePlanFromKastor = false
VAR knowBalintIsFromCarnassus = false
VAR gaveBalintThePassword = false
VAR givenTaskByBalint = false
VAR gotLeavesForBalint = false
VAR finishedBalintsTask = false
VAR templeMentionedBackground = false
VAR interruptedBalint = false
VAR guessedCraftFolkCruelty = false
VAR givenAdviceFromBalint = false
VAR charisma = 0
VAR wisdom = 0


VAR playerName = ""

//changeCamTarget(int targetIndex)
//keepDialogue()
//setToTrue(string flagName)
//setToFalse(string flagName)

{
-spokeToBalint:
    ->1b
-else:
    ->1a
}

=== 1a ===

setToTrue(spokeToBalint)
~spokeToBalint = true

Hello there. Has the lockdown been lifted?

    *No, not yet. I have permission to move from the guards.
        
        keepDialogue()
        
        Ah. Well if you don't mind, I am quite tired. I'm trying to take advantage of this time off from the mines to rest.
    
        ->1b

->Close

=== 1b ===

What can I help you with?

{
-givenTaskByBalint and not finishedBalintsTask:
    +I want to discuss these plant samples.
        ->5e
}
{       
-gaveBalintThePassword and not givenTaskByBalint:
    +Kastor asked me to help you in any way I can.
        ->5b
}

{
-gotThePlanFromKastor and not gaveBalintThePassword:
    +Which way is the wind blowing?
        ~gaveBalintThePassword = true
        setToTrue(gaveBalintThePassword)
        keepDialogue()
        East, friend.
        ->1b
}

{
-spokeToSeb:
    +Is that guy alright?
        ->1c
}

{
-true:
    +I'd like to learn a little about you.
        ->2a
}

{
-templeMentionedBackground:
    +Temple said that you could tell me more about the Craft Folk's history?
        ->6a
-else:
    +You seem rather learned. Do you know anything of history?
        ->6a
}

    +I won't disturb you.
        ->Close

=== 1c ===

Seb? No, far from it. Seb had a bad run in with the guards. I fear he may never recover from the lashing he took.

    +Is there anything we can do for him?
        ->1d
    +What will become of him.
        ->1e
    +How did he end up as your roommate?
        ->1f
    +Lets talk about something else.
        keepDialogue()
        As you wish.
           ->1b
        

->Close

=== 1d ===

keepDialogue()

Nothing can be done now except forcing him to eat and drink. And pray. His battle is in his and the Gods' hands now.
    ->1c


=== 1e ===

I know little of his mental condition, but I've seen a few other examples of it during my stay here. When this happens, the guards give the slave a few days of rest to see if they snap out of it.

keepDialogue()

It's been days since the lockdown began, and Seb has yet to recover. The guards will end his misery if he doesn't come back to us before the lockdown ends.
                
->1c

=== 1f ===

keepDialogue()

The guards put him and I together because we're the most feeble. This hut is closest to the gate, and we're the least likely to make a break for it. 
        ->1c

=== 2a ===

What did you want to know?


{
-knowBalintIsFromCarnassus:

    +Why are we both here if we're from such different places?
        ->3a
        
}

    +How did you become a slave?
        ->2b
    +I'm new. Is there anything I should know about this place?
        ->3b
    +You look well traveled. Any interesting stories?
        ->3c
    +I think that's it for now.
        keepDialogue()
        Alright.
        ->1b


=== 2b === //how did you become a slave

That is quite a long story; about as long as I've been alive. But I'll abridge the tale to save you some years and grey hair.

    +I would hear it.
        ->2c
    +I just remembered somewhere I need to be...
        ->Close

=== 2c ===

I think the best place to start is with a little context...

    +\*Continue listening*
      ->2e  
    +I thought you said you'd keep this brief
      ->2d
    +I just remembered somewhere I need to be...
      ->Close

=== 2d ===

You're the one who approached me and asked questions. If you're not willing to listen, you can leave.

    +I'm sorry. Please continue.
        ->2e
    +You're right. I'm leaving.
        ->Close

=== 2e ===

~knowBalintIsFromCarnassus = true

setToTrue(knowBalintIsFromCarnassus)

I was a scholar of some reknown back in Carnassus. I lived comfortably, and spent my days writing treatise on a variety of subjects. The Riding Folk conquests; new irrigation methods; proper quillwork technique. I even dabbled as a playwrite, if you'd believe it.

    +I already have questions.
        ->2ea
    +\*Continue listening*
        ->2f
    +I just remembered somewhere I need to be...
        ->Close


=== 2ea ===

As much as I appreciate an inquistive student, this story will never end if you ask too many.

    +Who are the Riding Folk?
        The Riding Folk were the predecessors to the Lovashi. A nomadic people, originating from the western steppes, it is from them who many of the Lovashi are descended. 
        
        keepDialogue()
        
        The Riding Folk and their horsemen conquered much of the land that would become the Lovashi Confederation, and each of the Confederation's counts can trace their lineage back to these conquests. It is from the Riding Folk word for "Rider" that the name "Lovashi" is derived.
        ->2ea
    //*You're from Carnassus? That's interesting. I'm from ________ <Pick Background>
      //  ->Close*/
      
    +I've never heard of Carnassus. What's it like?
        You've never-... No, I suppose chastising the ignorant is wasted breath. Carnassus is known as the City of Statues. Long ago, before the Lovashi conquered it, it was the capital of it's own kingdown. Now, it is one of the Stolen Cities and a true jewel of the Confederation.
        
        But I'm sorry, you were probably asking about what it's like to live there. You're not here to listen to an old man wax on about history. I lived there for most of my adult life, and it truly is a place without equal. The entire city is made from white stone, with statues of great figures dominating every street. Some many times as tall as a human. 
        
        keepDialogue()
        
        It has great squares thronged with people buying and selling goods from across the Confederation. Massive temples dot the city, lit with bonfires at all hours of the day and night. Buildings that scrape the sky, taller than any tree you have ever seen. It is a place that makes you feel like you're a part of something wondrous.
        
        ->2ea
    +I have no more questions.
        keepDialogue()
        I will continue then.
        ->2f

=== 2f ===

When I wasn't writing, I would take part in healthy debate with other sages. I wasn't the best orator, I prefer to perform my rhetoric with a pen and parchment, but I could get my point across when I needed.

    +\*Continue listening*
        ->2g
    +I just remembered somewhere I need to be...
        ->Close

=== 2g ===

Over the course of my life I gained a few rivals. It's inevitable really, in my chosen pursuit. One of these was a brigand by the name of Antiphoni. Mutual loathing was our only common trait. He saw me as someone to crush on the way to political power, and I never thought much of him beyond an upstart loudmouth. 

    +\*Continue listening*
        ->2h
    +I just remembered somewhere I need to be...
        ->Close
        
=== 2h ===

That was until a few months ago... Gods it feels like years since then. A plot was discovered to usurp the ruling council of Carnassus. I wasn't involved; the first I heard of the affair was when the count's men broke down my door and arrested me for treason.

    +\*Continue listening*
        ->2i
    +I just remembered somewhere I need to be...
        ->Close

=== 2i ===

The plot had been uncovered by Antiphoni, who brought forward letters of correspondence between myself and the plot's leaders that implicated me. Forgeries, but convincing ones. They declared I would be branded for my "actions".

    +\*Continue listening*
        ->2j
    +I just remembered somewhere I need to be...
        ->Close
        
=== 2j ===

I had hoped the count would see reason, but he declared I would have the harshest punishment imaginable. A few days later I was on the cart that brought me here. I have no doubt my sentencing was affected by Antiphoni as well.

    +That is quite the tale. I'm sorry for what happened to you.
        keepDialogue()
        Thank you. No matter what is taken from me, I take comfort that I still can entertain with my stories.
        ->2a

=== 3a === //Why are we both here if we're from different places?

An excellent question. Over my time here, I've noticed many of the slaves are from different origins.

The Lovashi Confederation is made of fourteen counties, each ruled over by a count. These count's have the power to sell criminals in their lands into slavery to private slave owners.

keepDialogue()

Whoever our owner is, and the owner of this camp for that matter, must be wealthy enough to have connections in slave markets that span multiple counties. Or perhaps this camp is jointly owned, and multiple investors are providing slaves. Who is to say?

->2a

=== 3b === //I'm new. Is there anything I should know about this place?

setToTrue(givenAdviceFromBalint)

There is only one thing you must know here: the guards cannot break you. They can only trick you into breaking yourself.

They can hurt you, yes, but most will only do so if you act out of line. In this way, compliance in action but not mind is conducive to keeping hope alive. Patience is necessary for survival. You may only ever be able to pick one battle, make sure it is one you can win.

I have seen many slaves who have lost hope, and then their minds. They tell themselves they will be free in a week, in a month. Then the day passes and that little failure crushes them again and again. In this way they are ground down and defeat themselves.

    +You advice is sound. Thank you.
        keepDialogue()
        You are welcome.
        ->2a
    +I will see us all free. Just keep hope alive until then.
        keepDialogue()
        You will not free yourself without help. Do not do anything you may regret.
        ->2a
    +I will be free, and then it will be the guards who lack hope.
        keepDialogue()
        Heh. I've seen many like you. Perhaps if enough of you try, you'll eventually succeed.
        ->2a

=== 3c === //You look well traveled. Any interesting stories?

"Well traveled", eh? How come the young never just come out and say "old"?

    
{
-charisma >= 2 and wisdom >= 2: 
    +There is no shame in age, if it comes with wisdom. <Wis {wisdom}/2>
        ->3d
    +I only meant that I am interested in what you would know. <Cha {charisma}/2>
        ->4a
    +I meant no offense by it.
        keepDialogue()
        None taken, but I am very tired. Maybe I can tell you a story another day.
        ->2a
    +If you're going to be like that, then nevermind.
        ->Close
-charisma >= 2: 

    +I only meant that I am interested in what you would know. <Cha {charisma}/2>
        ->4a
    +I meant no offense by it.
        keepDialogue()
        None taken, but I am very tired. Maybe I can tell you a story another day.
        ->2a
    +If you're going to be like that, then nevermind.
        ->Close

-wisdom >= 2: 

    +There is no shame in age, if it comes with wisdom. <Wis {wisdom}/2>
        ->3d
    +I meant no offense by it.
        keepDialogue()
        None taken, but I am very tired. Maybe I can tell you a story another day.
        ->2a
    +If you're going to be like that, then nevermind.
        ->Close

-else:
    +I meant no offense by it.
        keepDialogue()
        None taken, but I am very tired. Maybe I can tell you a story another day.
        ->2a
    +If you're going to be like that, then nevermind.
        ->Close
}
=== 3d ===
Implying that age without wisdom is shameful? *Bálint smiles.* I can't say I disagree with that statement. How about this: I shall tell you a tale and you can decide if I am wise.

    +Please. I am listening.
        ->3e
    +I actually am short on time.
    How is it that those with the most time never have enough of it? Bah.
        ->Close

=== 3e ===

Over the years, I have spent much of my time in the pursuit of knowledge. Studying, researching, writing, arguing. Even when I was young, I tended towards the sage's lifestyle, and as time passed I only became more set in my ways.

Knowledge and oration come with their own merits. A well spoken argument has a charisma of it's own. I would from time to time attract others who were... swayed towards me for my wit.

One of these was a brilliant woman who I met well into my fourth decade. Younger than me and eager to learn what I had accumulated in my time, we formed a bond of sorts. A bond which I, somewhat naively, thought was purely one of a master and student.

Eventually it became clear to me that she was hoping we might move beyond that stage. I did not find this an altogether unpleasant notion; I gave my approval and all was well for a time.

But as time wore on, more and more of my time was spent removed from my studies. My works became secondary for the first time in my life, and I became uncomfortable with that. So much so, that I eventually lashed out.

We rowed often, culminating in me banishing her from my presence. I heard nothing from her for weeks, until eventually I recieved a letter from her saying she was leaving the city in which we lived. I never heard from her again.

    +A sad story.
        ->3f
    +These things happen to all of us eventually.
        ->3f


=== 3f ===

What would you have had me do? I go back and forth on the matter myself.

    +She loved and trusted you, and you shunned her. How could you?
        That she did and that I did. I will not hide from that fact. Wherever she is now, I can only hope she has found someone who can give her what I could not.
        ->3g
    +Your work was important to you, and she should have understood that.
        Maybe. I can only take solace in everything I have accomplished since last we spoke.
        ->3g
    +A relationship like that is only a distraction. Your work of course comes first.
        That's what I thought as well, but hearing you say it is still as unconvincing as ever. I guess I must live with the fact that it always will be.
        ->3g
    +She was young; her priorities were different. Don't blame yourself.
        Easier said than done. But that won't stop me from trying.
        ->3g
    +I simply don't know enough about it. I can say nothing for certain.
        An uncommital answer, but only a fool commits themself without understanding. I can respect that.
        ->3g

=== 3g ===

Anyways, thank you for listening. Trapped as I am, I've had a lot of time to think about what I have done with my life. Discussing it with you has given me some perspective at least.

keepDialogue()

Was there anything else you wanted to know?

    ->2a

=== 4a === //I only meant that I am interested in what you would know. <2 cha>

Then I was too hasty. Forgive my assumption, it's a filthy habit in a sage. A story you want? I have a few knocking about.

Have you heard the tale of the dog and the bridge? It's one of my favorites.

    +Please, tell it to me.
        ->4b
    +Only a thousand times.
        ->4f
    +I'm actually short on time and must be going.
        keepDialogue()
        Why is it that those with the most time always seem to have the least? Bah.
        ->Close

=== 4b ===

I'm told my telling is long and indulgent, so I will trim it some for time's sake.

Long ago and far away, past seven forests and across seven streams, there lay a village. In this village, there lived an Owl, a Bear, and a Dog, and many happy villagers.

This village was built along both sides of a great river, and it had one massive bridge that connected either shore. The villagers used the bridge each day to cross the river, and life was good.

But one day an evil King, with a vast army, heard about the bridge and wanted to use it to cross the river. The Owl, the Bear, the Dog, and all of the villagers were scared of the King and his army, and didn't know what to do.

The villagers went to the Owl and said "Owl, we are scared of what the evil King will do if we let his army into our village. Please, you are wise, fly to him and convince him not to cross our bridge."

So the Owl went. They flew all day and all night to talk to the King. But the King was stubborn and would not listen, so the Owl flew home and said they had failed.

The villagers then went to the Bear and said "Bear! We are scared of the evil King, but you are mighty. Please defeat his army so they cannot cross the bridge!"

So the Bear went. They ran all day and all night and when they found the army the bear attacked them with all their might. But the army was too big and too strong, and the bear ran home and said they had failed.

By now, the army was getting close and the villagers didn't know what to do. But then the Dog came to them and said they had a plan. They told the villagers to leave the village and not to return for seven days and seven nights. When they returned, the village would be safe.

The villagers did, and the Dog went about their plan. They opened all of the gates to the village and then opened all of the doors and windows of every house. He then sat atop the tallest gate in the village and waited.

The King and his army arrived, and the Dog hopped up and down with joy atop his perch, and wagged his little Dog tail. Then they said to the King "Welcome King! Welcome to my village!"

The King said "Hello Dog. You cannot stop me from entering your village, or from crossing your bridge. The Owl could not convince me, and the Bear could not defeat me, and you will not stop me."

The Dog replied "I do not want to stop you. I want to invite you in! I'm so sorry my friends were so rude to you before!" The King was confused. He did not understand why the Dog was so happy. He suspected a trick. He thought the villagers were waiting in the village, ready to jump out and attack. 

But he looked through the villages gate. He looked past all of the open doors, and the open windows. The King could not find the villagers. He couldn't find the trick. So he said "Dog, I know you are tricking me. But I cannot see your trick. I do not want to fall for it, so I will go home."

    ->4c

=== 4c ===

The King turned around and led his army away. When the villagers returned, they saw that the King had left and hadn't entered the village. They asked the Dog about their trick, but the Dog replied "I did not trick the King. The King tricked himself."

    +A great story. Thank you.
        keepDialogue()
        I enjoyed the telling it. It was a worthy distraction for a time.
            ->2a
    +I don't get it.
        ->4e
    +Why didn't the villagers just let the King cross the bridge?
        ->4d
    +That was a waste of my time.
        You ask me for a story, and then complain when I give it to you? Begone!
            ->Close

=== 4d ===

keepDialogue()

Well my guess would be that they were afraid the King's army would rob them or hurt them. "Cross their bridge" is almost certainly a euphimism for all the terrible things an army can do to a conquered people. 

->4c

=== 4e ===

keepDialogue()

"Get"? If you're looking for a moral, it could be that it is sometimes better to be underhanded than strong or smart. Or maybe the story exists to introduce the idea of a double bluff to the world. Or maybe it's all bupkis. It's up to your interpretation.

->4c

=== 4f === //Only a thousand times.

Oh really. Please then, if it's such a common tale, tell it to me.

    +Well there's this dog and a bridge...
        ->4h
    +Actually, I think you would tell it better. 
        ->4b 
    +Nevermind this. I'm leaving.
        ->Close

=== 4g ===

As interesting a story as that might have been, that is not any version of the tale I have ever heard. Would you like to stop wasting my time now?

->Close

=== 4h ===

Uh huh. Go on.

    +And theres an Owl and a Bear...
        ->4i
    +And this donkey and a cart...
        ->4g
    +And there is an eagle and a mouse...
        ->4g

=== 4i ===

Yes, and what else?

    +And a sundial with no shadow.
        ->4g
    +And a big door on the side of a tree.
        ->4g
    +And a village full of villagers.
        ->4j

=== 4j ===

Right. Anyone else?

    +A bunch of bandits.
        ->4g
    +A king and his army.
        ->4k
    +A pack of wolves.
        ->4g

=== 4k ===

And what happens in the story?
    
    
    +The Owl and Bear mess up, but the Dog stops the King.
        ->4l
    +The Owl, Bear, and Dog team up to stop the King.
        ->4g
    +The King takes over the village, but the animals rescue the villagers.
        ->4g

=== 4l ===

Hmmph. That was correct but your telling lacked panache.

    +Do you want to tell it?
        ->4b
    +Maybe if you let me tell it instead of quizing me.
        keepDialogue()
        Ah, well my apologies. Maybe another time.
        ->2b
    +I'm done with this.
        ->Close

=== 5a ===

    ->Close

=== 5b ===

Did Kastor fill you in on what I've been tasked with?

    +Yes, we're trying to discern our location.
        ->5d
    +Can you remind me?
        ->5c

=== 5c ===

Kastor tasked me with obtaining our location. I have studied many maps of the Lovashi Confederation and I should be able to determine where we are with some help.

    ->5d

=== 5d ===

What I need is for you to follow up on a lead for me. We have a sheer wall of rock to our west, and a forest that runs along it. There are only a few places we could be that fit that description.

To narrow it down, I need you to find samples of local leaves that could help winnow those possibilities. It's been some time since I studied the various types of plants that live across Föld, but with luck, I should be able to find a clue to where we are.

Remember: the more life the leaves have in them, and the more complete they are, the more useful they are to me. Torn up or fully brown leaves are next to useless.

    +I understand.
        setToTrue(givenTaskByBalint)
        activateQuestStep(Aiding Bálint,1)
        The wind sometimes carries leaves over the northern wall. I would look there. Return to me when you find something.
            ->Close

=== 5e ===

Have you found anything that I can use?
    
{
-gotLeavesForBalint:
    +Are these leaves what you needed?
        ->5f
-else:
    +Not yet.
        Return to me when you do. Time is of the essence.
            ->Close
}

=== 5f ===

activateQuestStep(Aiding Bálint, 3)

prepForItem()

Let me take a look. *Bálint studies the leaves intently.* This is very interesting.
    
takeAllOfItem(Leaf Samples)    
    
    +How so?
        ->5g
    +Are they what we need or not? We're short on time.
        ->5g

=== 5g ===

Do you see how this leaf fans out like this, but tapers again farther out? And the edge of each side is, for lack of a better word, serrated with small spikes along the side?
    
    +Yes, what about it?
        ->5h

=== 5h ===

By noting these features, we can determine the type of tree this leaf came from. In this case its tree of origin, peculiar as it is, doesn't grow within the Confederation. At least, not anywhere I know of.
            
    +So where are we?
        ->5i

=== 5i ===

The closest place I know with trees like this is the Kingdom of Masons, on the other side of the Waking Mountains. This mountain range forms the Confederation's southern border, and we must be beyond it.

    +What does this mean?
        ->5j
    +Why would they build a mining camp way out here?
        ->5k
    +Kastor will want to know this immediately.
        ->5l

=== 5j ===

keepDialogue()

I would imagine it means the Lovashi aren't about to swoop in to put down a slave revolt outside their borders. And even if they wanted to, it could take weeks for word to reach them and then to muster themselves for an assault.
        ->5i

=== 5k ===

keepDialogue()

I have no clue, but it explains why the guards never speak of our location and never leave the camp. But these answers only lead to more questions... I'll mull it over while you deliver this news.
        ->5i

=== 5l ===

~finishedBalintsTask = true
setToTrue(finishedBalintsTask)

Yes, tell him my assessment. It should brighten his mood some.

->Close

=== 6a ===

You're interested in history? If you have the humility to ask, then I will muster the energy to answer.

    +How did the feud between the Craft Folk and Lovashi start?
        ->6b
    +That's enough for now.
        ->1b

=== 6b ===

There are many versions of the tale, and I suspect each of them only tells a partial story. I will give you what I believe is the most reasonable account, but even it may not be entirely accurate.

As you well know, the Gods made many groups of people and seeded them across Föld. These tribes, or Folks, were each tasked with what the Gods made them to do. The Farming Folk were taught the miracle of agriculture, the Delving Folk were taught the secrets of the deep places, the Folk of Song were taught their melodies and so on.

    +And two of these Folks were the Lovashi and the Craft Folk?
        ->6c

=== 6c ===

Precisely. The Craft Folk were one of the largest and most wide spread tribes the Gods created as their ancestors were subdivided into many kingdoms, each tasked with mastering a different trade. Pottery, masonry, the smithing arts, and weaving, to name just a few. Take my own home of Carnassus for example. It is known as the City of Statues, and once served as the seat of the Kingdom of Sculptures in ancient times.

The Lovashi, however, trace their heritage from the Riding Folk, or the Folk of the Saddle. Their ancestors lived on the High Steppes, which borders the Confederation to its west, and would roam the plains on horseback. Their purpose was to commune with the beasts of the world and learn how to integrate much of the God's natural bounty into human civilization. For this purpose the Gods gifted the Lovashi many of the domesticated animals, such as dogs, sheep, and of course, horses.

    +What does this have to do with the feud?
        ~interruptedBalint = true
        ->6d
    +\*Continue listening.*
        ~interruptedBalint = false
        ->6e
    +I didn't realize the answer was going to be this long. I'll return when I have time for it.
        ->Close

=== 6d ===

Patience, I'm getting to that. *Bálint pauses for a moment.* Blast, I lost my place. Where was I?

    +The Lovashi and Craft Folk and what the Gods tasked them with.
        ->6e

=== 6e ===

{interruptedBalint:Yes, exactly. }As part of their purpose to domesticate animals, the Lovashi were gifted horses by the God of nature, Beast, and taught their language by Him. They call it the 'old horse tongue', and fluency in it allows them to communicate with their horses. Horses taught the language can speak back and comprehend complex thoughts much the same way humans do. In Lovashi society, there is very little difference in how a human and horse are treated.

    +There is no way that's true. They treat us much worse than their horses.
        ->6f
    +What do you mean 'horses taught the language'? It's their language.
        ->6g
        
=== 6f ===

You jest, but you have inadvertantly landed on the issue at hand. 

->6h

=== 6g ===

Well spied. Horses aren't born knowing the language, in the same way humans aren't born knowing how to speak. They have to be taught it by their parents and community. But back to the story.

->6h

=== 6h ===

There came a time when the Lovashi allowed one of the Craft Folk kings to learn how to ride horses. But when they did so, according to Lovashi accounts, the Craft King abused the horse, which lead to it's death. To make matters worse, the horse was the child of the Lovashi king's mount, making it royalty in Riding Folk culture. The Lovashi went to war over the incident.

The conflict dragged many of the surrounding Craft kindoms into the fighting, and even then the Craft Folk were losing. The advantage of horses on the battlefield was simply too great. That is, until one of them had the idea to steal horses for themselves. They crept into Lovashi camps at night, and stole away with many foals. Soon the Craft Folk had cavalry of their own to match the Lovashi with.

{
-wisdom >= 2:
    +I'm guessing that the Craft Folk didn't treat those horses much better than they did the first one. <Wis {wisdom}/2>
        ~guessedCraftFolkCruelty = true
        ->6i
-else:
    +And that made the Lovashi even madder.
        ~guessedCraftFolkCruelty = false
        ->6i
}

=== 6i ===

{guessedCraftFolkCruelty:To hear the Lovashi tell it, no they did not. |Exactly. This is the crux of the feud. }The horses they stole never learned to speak; their Craft Folk riders lacked knowledge of the horse tongue to teach them. They were like any human child robbed of language: they became diminished. The Lovashi call them 'feral', although of course the Craft Folk consider their horses domesticated.

->6j

=== 6j ===

When the Lovashi realized what the Craft Folk had done, they became incensed and in retaliation began to enslave those Craft Folk who took up arms against them. This is the origin of slavery on Föld, as none of the tribes of humanity had ever commited that crime before. But in the Lovashi view, they say that slavery was invented by the Craft Folk when they stole the first horses. This is how they justify their deeds to this day, recompense for all the horses that the Craft Folk keep in bondage.

    +So all these people are being worked to death over something our ancestors may or may not have done ages ago?
        ->6k

=== 6k ===

That is correct. To the Lovashi, conquering all but a handful of the Craft Kingdoms, enslaving many of the Craft Folk and ruling over the rest, was justified. In their view, they think themselves righteous for continuing the practice.

    +I'd like to talk about something else now.
        ->1b
    +I need time to think on this.
        ->Close

=== Close ===

close()

->DONE