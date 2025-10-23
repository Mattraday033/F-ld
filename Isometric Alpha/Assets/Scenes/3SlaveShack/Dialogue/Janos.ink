VAR wisdom = 0
VAR dexterity = 0
VAR strength = 0
VAR charisma = 0

VAR gotThePlanFromKastor = false
VAR spokeToJanos = false
VAR refusedToWorkWithJanos = false
VAR canWaitWithJanos = false
VAR declaredAndrasMustDie = false
VAR intimidatedAndras = false
VAR andrasAttackedPlayer = false
VAR andrasIsDead = false
VAR janosIsCrying = false
VAR obtainedMineArmoryKey = false
VAR gotKeyFromJanos = false
VAR toldToAidJanos = false
VAR janosExplainedHowHeMetAndras = false

VAR janosIndex = 1
VAR andrasIndex = 2
VAR andrasAfterConvoIndex = 3

VAR dropKeyFightIndex = 0
VAR doNotDropKeyFightIndex = 1

VAR dialogueKeyForAfterKillingAndras = "JanosAfterKillingAndras"

VAR playerName = ""

//changeCamTarget(int targetIndex)
//keepDialogue()
//setToTrue(string flagName)
//setToFalse(string flagName)

searchInventoryFor(obtainedMineArmoryKey,Key,0)

{
-janosIsCrying:
    ->7a
-refusedToWorkWithJanos and not andrasIsDead and obtainedMineArmoryKey:
    ->7bb
-not andrasIsDead and gotKeyFromJanos:
    ->7b
-andrasIsDead and obtainedMineArmoryKey:
    ->7c
-refusedToWorkWithJanos:
    ->4a
-canWaitWithJanos:
    ->4b
-else:
    ->1a
}
=== 1a ===

setToTrue(spokeToJanos)

Hello. Am I needed for something?
//added not obtainedMineArmoryKey part of if to fix save file compatability. Eventually can remove, added 07/13/2025
{
-gotThePlanFromKastor and (toldToAidJanos or not obtainedMineArmoryKey): 
    +Which way does the wind blow?
        ->1b
    +Nothing right now. I was just looking around.
        ->Close
-else:
    +Nothing right now. I was just looking around.
        ->Close
}

=== 1b ===

To the East. Thank the Sun you're here. The timing worked out perfectly.

     +What do you mean?
        ->1c

=== 1c ===

Did Kastor explain what I'm doing?
    
    +Yes, we're stealing a key.
        ->1e
    +No, can you fill me in?
        ->1d
    
=== 1d ===

My part of the plan is to obtain the key to the armory. And I know just how to get it.

    +Right, I remember. But Kastor wants the key to the mine's armory now instead.
        ->1f

=== 1e ===

Correct, the key to the armory in the guardhouse.

    +Kastor actually now wants us to steal the key to the armory in the mine.
        ->1f

=== 1f ===

I see. With the mine locked down, that would be less likely to raise the alarm. And it shouldn't change anything I have planned out already.

    +What is the plan then?
        ->1g

=== 1g ===

I have set up a meeting with one of the guards who holds the key. He knows nothing of what I'm actually after, and he won't be expecting you to be there as well.

    +I see, and you want me to get the key while he is isolated. Where is the meeting?
        keepDialogue()
        It will take place here in this hut. I can't leave with the lockdown going on so we had no other choice.
        ->1g
    +How can we be sure he'll have the keys on him?
        ->1h
    +How did you set up the meeting during a lockdown?
        ->1i
    +How can you be sure this guard is going to show up alone?
        ->1i


=== 1h===

keepDialogue()

He always has the keys on him. If he lost them he would be punished severely, so he keeps them on him at all times. 
    ->1g

=== 1i ===

We've been meeting like this for a while. It's the best way to get some... privacy. He won't miss it for anything. When he arrives I'll explain the situation, then I'll get him to hand the key to you and you can take it back to Kastor.

    +Just like that? He'll hand it over without a fight?
        ->1l
    +Your relations with this guard are not filling me with trust.
        ->2d
    +You're literally sleeping with the enemy? How do I know you aren't compromised?
        ->3a

=== 1l ===

He's confessed to me many times that he wishes us all to be free. Over all the time I've known him I've found him to be a good and caring person. I know he will help us.

    +I find your empathy for this guard disturbing
        ->2a
    +I do not think that is wise. Letting him live is a risk we shouldn't take.
        ->2b
    +I will avoid bloodshed if possible, but I will not hesitate to defend myself.
        ->1m
    +If I get a chance to hurt a guard, I'm taking it.
        ->2a


=== 1m ===

Thank you. Now, will you wait here for him or do you have other things to see to first?

    +I will wait with you.
            ->1ma
    +I need to go, but I will return shortly
        setToTrue(canWaitWithJanos)
        activateQuestStep(Aiding Janos,2)
        Don't take too long. He will arrive soon and we will waste our chance.
            ->Close


=== 1ma ===

    Good. He should be here shortly.
{
-not declaredAndrasMustDie:
    +While we wait, I am curious. What led you to seek solace in the company of one of the guards?
        ->8a
    +\*Wait in silence.*
        ->4c //break point for testing.
-else:
    ->4c
}


=== 2a === //If I get a chance to hurt a guard, I'm taking it. / I find your empathy for this guard disturbing.

I understand why you would say that. The rest of the guards, down to the last, are scum. I've been here for months, and I've seen them do all manner of horrific things. But this one is different! He is a kind soul, I've seen it!
    
    
    +Do you hear yourself right now? You're defending someone who has enslaved you!
        ->2f
    +I believe you believe that, but I cannot. His crimes prevent it.
        keepDialogue()
        Fine, don't believe it. But letting him live helps the plan: if he goes missing, it will be noticed. And he has a set of keys. If someone comes looking for him or his keyring, they will begin to ask questions. And I cannot be certain no one else knows he visits me. By killing him, you risk the entire plan.
        ->2b
    +You would know him better than I. I just hope you are right.
        ->1m

    
=== 2b === //I believe you believe that, but I cannot. His crimes prevent it.
           //I do not think that is wise. Letting him live is a risk we shouldn't take.
The greater risk is if he is killed. If he goes missing, it will be noticed. And he has a set of keys. If someone comes looking for him or his keyring, they will begin to ask questions. And I cannot be certain no one else knows he visits me. By killing him, you risk the entire plan.

    +I see. Be that as it may, I cannot trust him to keep our secret. He must die.
        ->2c
    +I will attempt to persuade him. But I make no promises.
        keepDialogue()
        Thank you, that is all I can ask of you. Now, will you wait here for him or do you have other things to see to first?
        ->1m

=== 2c ===

~declaredAndrasMustDie = true
setToTrue(declaredAndrasMustDie)

I hate this place. With every part of me, I hate it. The labour, the pain, the stress. And even when you fight it and steal something good from all of it, it finds a way to steal it right back. 

    +Do as I do. Take comfort in the fact that one day you will leave it behind you.
        keepDialogue()
        A far off comfort it is. But I will try. Now, are you ready to wait with me for him to arrive, or do you have something you need to do first?
        ->1m
    +You tricked yourself by thinking anything good could come from a place like this.
        keepDialogue()
        Maybe you're right. It doesn't make me feel any better though. Now, are you ready to wait with me for him to arrive, or do you have something you need to do first?
        ->1m
    +You are a fool, paying the fool's price. I pity you.
        keepDialogue()
        We must now wait for him to arrive. Please spend the time speaking to me as little as possible.
        ->1m

=== 2d === //Your relations with this guard are not filling me with trust.

I understand how this looks, believe me. But you're new. You've never worked the mine like I have. Worked until your fingers bled and your back gives out and you choke on dust and stale air. When you do that, for weeks on end, you take comfort where you can find it. I'm just as committed to the cause as you or Kastor. I'm merely a little luckier.

    +I guess I can understand that. We all have needs after all.
        ->1m
    +No, don't rationalize it to me. I don't need to have walked a mile in your boots to hear how ridiculous you sound.
        ->2e

=== 2e ===

\*Janos sighs.* Why did I think this would work? Where does this leave us? Are you going to help me get the key or not?

    +I don't trust you. In fact, you're probably setting this whole thing up as a trap.
        ->3a
    +I'll help you get the key. But if this goes south, I won't hesitate to defend myself.
    keepDialogue()
    Alright, that's fair enough. Can you wait here with me now, or do you have something else you need to finish first?
        ->1m

=== 2f === //Do you hear yourself right now? You're defending someone who has enslaved you!

He didn't enslave me, it was Count Károly Erba who did that! He's just doing a job, one that he has confessed he regrets! Can you not see there is a difference?

    +These are semantics. Admit it, this is indefensible.
        ->2e
    +I think I see your point. And a confession of remorse is the first step to righting one's wrongs.
        ->1m

=== 3a ===

You have every right to be wary, but think about this for a moment. What do I get out of setting a trap like this? I already know Kastor is in on the plot, if I wanted to betray you I could have done so without going to all this trouble.

    +That's a good point. I guess I was a little too hasty.
        ->1m
    +I haven't figured that part out yet, but I'm not about to walk into this blind. I'll find another way to get the key.
        ~refusedToWorkWithJanos = true
        setToTrue(refusedToWorkWithJanos)
        activateQuestStep(Aiding Janos,1)
        Oh you stubborn fool! Fine, come back when you're ready to take this seriously!
            ->Close

=== 4a ===


You're back. Have you decided to trust in my plan?
    +After considering it some more, I think your methods are the least risky option.
        keepDialogue()
        Good, I'm glad you came to your senses. He should be here shortly.
            ->1ma //Go to waiting
    +I still don't trust you. I'm not having any part of this.
        Then why did you come back at all? Leave before you jeopardize everything.
            ->Close

=== 4b ===

{
-declaredAndrasMustDie:
You're back. I had hoped you might never come back.
    +I'm finished with my business. Let's get this over with.
        He should be here shortly.
        ->4c //Go to Andras arriving
    +It's not the last you'll see of me. I need a little bit longer.
        Return or don't, it makes little difference to me. 
            ->Close
-else:
You're back. Are you going to stay until the meeting?
    +Yes, I'll stay. Let's get this over with.
        keepDialogue()
        Good. He should be here shortly.
            ->1ma //Go to waiting
    +Not yet. I need a little more time.
        Alright, but don't take too long. If you miss the meeting the plan may be in jeopardy.
        ->Close
}

=== 4c ===

fadeToBlack()

activate({andrasIndex})

changeCamTarget({andrasIndex})

setFacing(SE)

fadeBackIn(60)

Jan, I'm he-. Who are you?
    
    +I'm {playerName}. We need to talk.
        ->4d
    +I'm one of the new slaves. We have business to discuss.
        ->4d
    +Don't let your last thoughts be of something so trivial. <Combat>
        ->fightingAndras

=== 4d ===

\*András looks at Janos.* What is this?

changeCamTarget({janosIndex})
    
Hear them out, my love. It's important.

changeCamTarget({andrasIndex})

Fine. Make it quick.

    +Janos says you can be trusted. We're planning an escape.
        ->4da
    +I don't trust you, so know if you try to run I'll kill you.
        ->4e

=== 4da ===

    Escape? Do you have any idea what that's going to take? The three of us aren't going to cut it. This isn't something to joke about even if we had all of the other slaves on our side.

    +This plan has been in motion since before the mine closed. We have others helping us.
        ->5a


=== 4e ===

You don't scare me. You're unarmed, and I'm no slouch with a blade.

{
-wisdom >= 2 and strength >= 2:
    +They who know themselves are never unarmed. <Wis {wisdom}/2>
        ->4f
    +I'm more dangerous without a weapon than you are with one. <Str {strength}/2>
        ->4g
    +Be that as it may, I can still take you.
        ->4h
    +Uh... damn. I wasn't expecting you to fight back.
        ->4i

-wisdom >= 2:
    +They who know themselves are never unarmed. <Wis {wisdom}/2>
        ->4f
    +Be that as it may, I can still take you.
        ->4h
    +Uh... damn. I wasn't expecting you to fight back.
        ->4i
        
-strength >= 2:
    +I'm more dangerous without a weapon than you are with one. <Str {strength}/2>
        ->4g
    +Be that as it may, I can still take you.
        ->4h
    +Uh... damn. I wasn't expecting you to fight back.
        ->4i
        
-else:
    +Be that as it may, I can still take you.
        ->4h
    +Uh... damn. I wasn't expecting you to fight back.
        ->4i
}


=== 4f ===

~intimidatedAndras = true
setToTrue(intimidatedAndras)

I-I'd rather not test you on that. Let's just calm down and keep this civil.
    +Good, now that I have your attention we need your keys for an escape attempt.
        ->4da

=== 4g ===

~intimidatedAndras = true
setToTrue(intimidatedAndras)

Hey now, I-I'm not looking for a fight. How about I keep still, and you don't hurt me.
    +Good, now that I have your attention we need one of your keys for an escape attempt.
        ->4da

=== 4h ===

I'd rather not come to blows. Just tell me what you want.

    +Fine. We're planning to escape, and we need your keys.
        ->4da
    +What I want is for you to know who's in charge here. <Combat>
        ->fightingAndras

=== 4i ===

Jan said to listen, so I will. Just stop wasting my time and get on with it.
    +Fine. We're planning to escape, and we need your keys.
        ->4da
    +What I want is for you to know who's in charge here. <Combat>
        ->fightingAndras

=== 5a ===

Then you're serious about this. But I'm not convinced you understand what this will take. Even if you do succeed, many will die in the attempt.

    +Better to die than to continue to live as a slave.
        ->5b
    +Some may die, but the strong will be free.
        ->5b
    +If you help us, you can save some of those lives.
        ->5d

=== 5b ===

I won't take a course of action that will endanger Jan. I don't know if I can trust you to keep him safe.
    
    +You would rather him remain a slave?
        ->5c
    +You would rather him remain a slave!
        ->5i
    +He has already pledged to help. You should respect his decision.
        ->5d

=== 5c ===

No! But I don't wish to gamble with his life!
    
    
{
-charisma >= 2:
    +It's not suicide. With your help we can save as many as possible. <Cha {charisma}/2>
        ->5d
}

    +You have my word that I will do everything in my power to keep Janos safe.
        {intimidatedAndras:->5d|->5g}

    +This is going nowhere. I tried being reasonable. <Combat>
        ->fightingAndras

=== 5d ===

\*András looks to Janos and sighs.* Alright. If you're determined to go through with this, I think the best thing to do is help. Which keys do you need?
    
    +I need the mine armory key.
        ->5e
    +Just like that?
        keepDialogue()
        Yes, just like that. Tell me which key you need before I come to my senses.
        ->5d

=== 5e ===

setToTrue(obtainedMineArmoryKey)
~gotKeyFromJanos = true
setToTrue(gotKeyFromJanos)
activateQuestStep(Aiding Janos,7)
prepForItem()

The mine armory? That's going to be tricky. I hope you know what you're doing.

giveItem(4,0,1)

    +Now that I have the key, how do I know you wont tell the guards once I leave?
        ->5h
    +Your part in this is done. Time to die. <Combat>
        ->fightingAndras

=== 5f ===

changeCamTarget({janosIndex})

Thank you. For trusting me, and for trusting him.

    +When the fighting starts, tell András to hide. The others may not be as understanding.
        I will. Be careful out there.
        ->leavingWithAndrasAlive
            
    +I hope you know what you're doing. Take care of yourself.

        ->leavingWithAndrasAlive
        
=== 5g ===

changeCamTarget({andrasIndex})

How can I trust that? I've just met you, and you ask me to put both of our lives in your hands?

    +All members of our plan must trust each other for it to succeed. Asking this of you is no different.
        ->5ga
    +Janos has pledged himself to our cause. His life is already at risk. Your choice now is between protecting him by helping us, or jeopardizing our plan and risking exposing his part in it.
        ->5d
    +You're right, you don't know me. But I'm the one the other slaves have placed their trust in to see them freed and safe. I can't do that unless you hand me the key.
        ->5ga
    +I'm not asking you to trust me. I'm asking you to trust Janos. He suggested we approach you for help, do not betray his faith in you by attacking me.
        ->5d

=== 5ga ===

I'm sorry, but the risk is too great. You will be coming with me. *András draws his sword and approaches*.
        ->fightingAndras


=== 5h ===

My shifts are set up so that I can sneak away for hours. I'll stay here with Jan. He'll keep me honest.
    
    +I actually want to discuss your relationship with Janos. *Pull András aside.*
        ->9a

    +I guess that will have to be enough. I must be going.
        ->leavingWithAndrasAlive

    +No, I can't trust that. Time to die András. <Combat>
        ->fightingAndras

=== 5i ===

   How dare you! You bring this danger on us and then you accuse me? I'll make you pay for this! *András draws his sword and attacks*
        ->fightingAndras

=== 7a === //Janos is crying

\*Janos sobs quietly and does not respond to your presence*
    
    +\*Leave.*
        ->Close

=== 7b === //Andras is alive 

I am happy to see you again, but please do not dawdle here. We're counting on you.

    +You're right, I must be going.
        ->Close

=== 7bb === //Andras is alive but got key elsewhere

Please do not dawdle here. We're counting on you.

    +You're right, I must be going.
        ->Close


=== 7c === //Andras is dead

Please, leave me. I would like to be alone.

    +Of course.
        ->Close


=== 8a === //    +While we wait, I am curious. What led you to seek solace in the company of one of the guards?

    setToTrue(janosExplainedHowHeMetAndras)

    A week or so after my arrival at this camp, myself and a few other branded snuck into the mess hall and stole some extra rations for ourselves. It was a foolish thing to do, but my life before my branding had ill-prepared me for the meager portions we are provided. The hunger in those first days was the hardest.

    Once the theft was discovered, all of the thieves were quickly apprehended. One of our number, Sampson, chose to give the rest of us up in exchange for special treatment. When the punishments were being handed down, András spoke up on my behalf. I was given half the lashes my compatriots received. 

    While I was recovering, he visited me. The care he showed me then, and since, has revealed to me he has a sensitive soul. That is how I know he will help us.

    ->8aa

=== 8aa ===

    +He only spoke on your behalf? Did he not speak up for the other branded?
        He lied before Chief Tabor and the other guards and claimed he knew me well. He spoke of my character and how I must have been coerced into helping the others. His lie could not have protected everyone in the group. 
        ->8aa
    +Has András ever threatened you while the two of you were alone?
        No, never. Although in the beginning, because of my experiences with other guards, I admit I was fearful of what would happen if I angered him. In the time I have known him, I have learned that fear was unfounded.
        ->8aa
    +How can you be certain? How can you trust the veracity of his feelings, or even your own for that matter, when you are his prisoner?
        combineDialogue()
        Certain? I am certain of nothing, except that he is the one person to show me concern in all my time here. 
        ->8ab
    +Such kindness is a rare thing in a place like this. I can see the attraction in it.
        ->8bc
    +I do not believe a guard capable of the good you claim he has done. I suspect he has some other motive.
        combineDialogue()
        Must all kindness obscure some hidden treachery? 
        ->8ab

=== 8ab === 

Must I cut myself off from all support simply because it comes from a strange place? Can I afford to do so in my predicament? Could anyone?

    +There is no need to raise your voice. Your defensiveness speaks volumes enough.
        ->janosWantsSilence
    +It is no surprise to me that you accepted support when you were low. But consider that he knew how low you were, and still sought this relationship.
        ->8b

=== 8b === 

You think my feelings are born from my desperation? *Janos raises his voice in anger, but then hesitates.* I see. How could you not ask me that? And if I were in your place, I should hope I would ask the same thing. The thought unsettles me, not because I couldn't handle the discovery that my love for him was built on such a foundation, but because I am afraid to scrutinize <i>his</i> feelings too closely. I don't think I could handle it all being some impish game.

{
-wisdom >= 2:
    +That is because if your own feelings are hollow, you could still look to him for support. To find him lacking would mean you never truly could. <Wis {wisdom}/2>
        ->8ba
}
    +I have robbed you of your faith. I am sorry. 
        keepDialogue()
        If your concern is sincere, then do not apologize. But if you simply wish to see my new suspicions send me spiralling without a tether, then I wish you ill.
        ->8b
    +Let our request for his help be a test. If he balks at your freedom, you have your answer.
        ->8bb

=== 8ba ===

Your logic is sound. Perhaps I did not know the depths of my own desperation. I would discuss this with András, but the thought of provoking him to leave me and being alone again, after finding surety in his embrace, has me terrified.

    +Should you find his love lacking, you are not alone. You have Kastor, and you have myself. Do not look to the guards for support, but to your fellow prisoners.

        Thank you. Your advice is sound. *Janos chuckles.* I feel ridiculous. Here we are, conspiring to overthrow the guards, and yet I'm more worried for my heart than my health.

        ->gaveJanosCourageToAsk

    +Don't ask him anything difficult. Should that discussion turn sour, it may affect his willingness to cooperate with us.

        combineDialogue()
        I see. Then for the good of the conspiracy I will hold my tongue until we have succeeded.
        ->janosWontAsk

    +Ask him or don't. I was more curious to learn how one could justify your relationship than to learn if it will survive.

        You have only been here for a short time. Should you work the mines for longer than a day, you won't need to ask that question.

        combineDialogue() 

        But I'll admit that I hope I never have to press him on whether his love is true.
        ->janosWontAsk

    +If you discuss your fears with him, and dislike his answers, worry not. Once the plan succeeds, you will be free. Beyond this camp there will be others that you will meet under more trusting circumstances. 
        
        combineDialogue()

        It is difficult to admit, but that is the truth.

        ->gaveJanosCourageToAsk

=== 8bb ===

\*Janos slowly nods.* I can see the merit in that. May his sincerity ring in his words.

->4c

=== 8bc ===

Your support is appreciated. You have most understanding, and I thank you for it.

->4c

=== janosWantsSilence ===

If my voice offends you so, then perhaps we should continue the rest of our wait in silence.

->4c

=== gaveJanosCourageToAsk ===

I believe the plan to persuade András to give us the key is still sound. But I now know what to discuss with him afterwards. I think I now have the courage to ask the questions I should have already asked.

->4c

=== janosWontAsk ===

Maybe I'll put off that discussion forever. What will it matter when we're free?

->4c

=== 9a ===

fadeToBlack(true, false)
setToTrue(andrasLeftInHut)
deactivate({andrasIndex})
activate({andrasAfterConvoIndex})
changeCamTarget({andrasAfterConvoIndex})
movePlayer(-0.475f,1.645f)
setFacing(NE)
fadeBackIn(60)

What did you wish to discuss?

    +As a friend to Janos, I must know. What are your intentions with him?
        ->9b
    +Stay away from Janos. I don't trust you.
        ->9c

=== 9b ===

    Intentions? What do you mean?

        +A relationship between one of the guards and a prisoner is the definition of untoward. I want to know your motives behind initiating it.
            ->9ba

=== 9ba ===

Of course. I understand. I initiated it because of a great loneliness I have felt since my arrival at this camp. We are far from home here, and have orders to never leave the camp. Before I met Janos, I was restless and without a true companion.

Then Janos was brought before Chief Tabor for stealing rations. I lied on his behalf and had his sentence lightened. And while he recovered, I met with him and admitted the truth: that when I first saw him, I knew he stole not because he was wicked, but because he was hungry, and he had my sympathy.

In the time since, I have grown to respect him and his morals. He is the reason why I agreed to fight with you. Had he not been the kind of man he is, I would not have learned the kind of man I had become serving as a guard.


//I fancied him. And I have since I noticed him when he first arrived here, but I did not know how to approach him. Then he was brought before Chief Tabor for stealing rations. I lied on his behalf and had his sentence lightened: not because it was the right thing to do, but because my mind's abacus told me it would ingratiate myself to him.

//I then used a favor I had with one of the Overseers and had Jan given extra recovery time so that I could meet with him in private. While he recovered, we got to talking, and I began to find not just his looks, but his wit to be quite charming as well. 

//In the beginning, I expected this to be no more than a quick fling. The branded do not live long; I had hardened my heart to that a long time ago. But as I came to enjoy my time with Jan more and more, I began to lament that fact. I have said in his presence many times that I wished he could be free. That you all could be. 

//Think me a monster if you wish; I am sickened by the man I was but a few months earlier. But know that my feelings for Jan are real, and that I would no more harm him now than anyone who cares for another. And to lend my words weight, I will lie to my superiors again to keep you all safe, and raise my sword for your cause when the time comes.

->9bb

=== 9bb ===

    +Love can be a teacher, but it can also be ruinous. Should Janos fall, or renounce his affection, will you still support our cause?
        Janos has not taught me that he alone should be free, but that all branded should be. My support for your cause is unwavering.
        ->9bb
    +Your status as a guard puts you in a dominant position. Will your feelings survive becoming equals?
        ->9bca
    +In as compromising a place as Janos's, how can I trust his safety is assured? Or your intentions are as you say?
        ->9bcb
    +I would be a fool to believe you. I can't trust the word of someone so heinous. <Combat>
        ->fightingAndras

=== 9bca ===

Of course. Our feelings are eternal.
    
    +You agree too hastily. Have you thought about what comes after? A life of running from the Lovashi, removed from your kin and friends?
        ->9bcc(->9bda)

=== 9bcb ===

My intentions are pure. I have not harmed him and I never will.

    +You protest too hastily. Just because you agree with us in principle does not mean you will keep to your morals when angry, or frightened. Have you thought about what it would take unlearn your training as a guard?
        ->9bcc(->9bdb)

=== 9bcc(->divert) ===

I have thought about it some...

    ->divert

=== 9bda ===

    +And even should you make it to somewhere safe, Janos will never be able to remove his brand, while you could simply melt back into the Confederation. 
        ->9bdb

=== 9bdb ===

{
-wisdom >= 2:
    +That is not enough. You cannot just be dedicated to him. You have to be dedicated to bettering yourself. <Wis {wisdom}>/2>
        ->9bdd
-else:
    +Some? Can you not see yourself as the threat one of the branded would see you as? Are you going to prioritize his safety?
        ->9bdc
}

=== 9bdc ===

Yes, but-

    +Do not lie to me. I must be certain of your certainty.
        ->9bdc

=== 9bdd ===

How can I prove my dedication to you?

    +Vow to me that once we all are free, you will take the brand. Prove that you can live as Janos must. 
        ->9c
    +Vow to me that after we leave this camp, you will stay away from Janos for a month's time. If your relationship can survive the separation and his freedom, I will trust in its permanence.
        ->9d
    +Make an oath now, before the Gods, that stakes your health against his. If you harm him, the Gods will see you suffer for it.
        ->9e
    +Simply give me your word that you will not harm Janos. That will suffice.
        ->Close
    +I don't believe you can. <Combat>
        ->fightingAndras

=== 9c ===
setToTrue(andrasAgreedToBeBranded)

\*András looks back at Janos. He thinks for moment, then turns back to you.* I will do it. My people created the brand to serve as a symbol of oppression; to mark a person without a future. Let ours instead symbolize our devotion to one another, and the future of our love.

    +Well said, András. With that, I leave you. Spend what little time you have together well. Before long, we will find ourselves fighting for our freedom.
        ->leavingWithAndrasAlive
    +That is easy to say before the brand is hot. I will leave you with time to think. Pray that when the time comes, I will not find you wanting.
        ->leavingWithAndrasAlive

=== 9d ===

setToTrue(andrasAgreedToMonthSeparation)

Easily done. Our affections will not wither over such a short time. In fact, I expect the distance will only strengthen it.

    +Your confidence does you credit. I must leave now: talk it over with Janos. Should you agree, I will hold you both to it.
        ->leavingWithAndrasAlive
    +Once you have both left this camp, do not be surprised if life provides each of you with new perspectives.
        ->leavingWithAndrasAlive

=== 9e ===

setToTrue(andrasSworeGodOath)

Vows sworn before the Gods are a serious matter but I know the words I will speak.

\*András raises his voice, speaking with clear purpose.* I swear before Beast that I will always act to preserve Janos's health and dignity. Should I fail in this, I accept that my own health is forfeit.

    +I will tell the others that I bore witness to this oath. I must leave you now. Spend this time with Janos wisely, soon the plan will commence and your aid will be needed again.
        ->leavingWithAndrasAlive
    +I shall hold you to your oath. If you break it, pray the Gods find you before I do.
        ->leavingWithAndrasAlive

/*
=== 9bc ===

    I am only asking you to trust that I will keep doing what I just did: admit to my wrongs, and strive to better myself. I have answered your question with honesty; what good would it do me to lie about doing such distasteful things? Your dislike for my answers does not mean I have given you reason not to trust them.

    +I worry what will happen afterwards, when the fighting is over. Can someone like you handle treating a branded as an equal? I would not wish Janos to stay a prisoner after we are all free.
        ->9ca
    +I see your point. Your past fills me with disgust, but there is a chance your future doesn't have to. Do not make me regret leaving you in peace.
        ->9z
    +If not for how it would affect Janos, I would strike you down. Give me reason, and it may yet happen.
        ->9z
    +Perhaps you can be trusted, but that does not mean you can be relied upon. I'm not taking any chances. <Combat>
        ->fightingAndras

=== 9c ===

You have every right not to trust me, but no right to keep me from Jan. If you accomplish what you spoke of, then the fighting will be fierce. I won't leave him vulnerable during the rioting.

    +A noble sentiment. But I worry what will happen afterwards, when the fighting is over. Can someone like you handle treating a branded as an equal? I would not wish Janos to stay a prisoner after we are all free.
        ->9ca
    +Your answer surprises me. What do you want with Janos? Why initiate your relationship with him?
        combineDialogue()
        The answer is quite shameful on my part: 
        ->9ba

=== 9ca ===

Your concern is understandable. I do not know the answer myself. But I know that I would rather us be equals than him remain a slave. I will cope, or I will suffer, but I will not keep him beneath me.

    +I will make sure that you do not.
        ->leavingWithAndrasAlive
    +I cannot bring myself to trust you. Janos may weep, but he will endure a free man. <Combat>
        ->fightingAndras

=== 9z ===

I won't. I promise you that.

->leavingWithAndrasAlive
 */

=== leavingWithAndrasAlive ===

    fadeToBlack()

    setToTrue(andrasLeftInHut)
    deactivate({andrasIndex})
    activate({andrasAfterConvoIndex})
    
    fadeBackIn(60)

->Close

=== fightingAndras ===

setToFalse(andrasLeftInHut)

{
-gotKeyFromJanos:
enterCombat({doNotDropKeyFightIndex},{dialogueKeyForAfterKillingAndras})
-else:
enterCombat({dropKeyFightIndex},{dialogueKeyForAfterKillingAndras})
}

->Close

=== Close ===

close()

->DONE