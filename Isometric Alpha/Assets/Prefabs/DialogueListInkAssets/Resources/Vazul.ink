VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR givenAdviceFromBalint = false

VAR foundSlate = false
VAR saidKilledForLessThanChew = false

VAR givenFullExplanation = false

VAR givenTutorialQuest = false
VAR toldKastorOfThatchsFate = false

VAR metThatch = false
VAR thatchRemovedTutorialRubble = false
VAR thatchBeginsStranglingVazul = false

VAR vazulMentionedSlatesFate = false

VAR deathFlagGuardVazul = false
VAR vazulFightIndex = 0
VAR vazulAfterFightConvoKey = "Vazul"


VAR vazulIndex = 1
VAR slateIndex = 2
VAR thatchIndex = 3

VAR playerName = ""

setToTrue(foundSlate)

{
-deathFlagGuardVazul:
    ->8a
-thatchRemovedTutorialRubble:
    ->1a
-else:
    ->7a
}

=== 1a ===

fadeToBlack()

activate({thatchIndex})

fadeBackIn(60)

\*The guard turns to regard you as you approach. He chews something, but spits before speaking.* I know Thatch should be here, but I don't know who you are. Why are you out of your hut during lockdown?

{
-strength >= 2:
    +Keep asking stupid questions and you'll wind up dead like your friends. <Str {strength}/2>
        ->4a
}

{
-wisdom >= 2:
    +\*Yawn*. I'm a new slave. Arrived last night, but they've had me working with Thatch until now. <Wis {wisdom}/2>
        Your luck must be awful if you've been selected to be this lout's hutmate. Don't expect to get much rest in the future. *Vazul chuckles.*
        ->2a
}

{
-charisma >= 2:
    +I was sent by Guard László. He wants me to collect Thatch for yet another work detail. *Show badge* <Cha {charisma}/2>
        \*Vazul looks Thatch over and smirks.* And you just got off work, too. They're really running you ragged.
        ->3a
}

    +Where's Slate? What have you done to him?
        ->1b
    +You got any more of that chew on you?
        ->1aa
    +You've misread the situation. Thatch, get his legs. <Combat>
        ->Combat

=== 1aa ===

What's it to you, slave? And you haven't answered my question.

    +You're not getting one. What have you done with Slate?
        ->1b
    +\*Shrug* Eh, what the heck. I've killed for less than a bit of chew before. <Combat>
        setToTrue(saidKilledForLessThanChew)
        ->Combat

=== 1b ===

setToTrue(vazulMentionedSlatesFate)

\*Vazul spits.* He got a little mouthy. Mouthy workers are unproductive workers. There's no point in keeping a branded around who can't pull their weight.

    +That's all I need to hear. <Combat>
        ->Combat
    +Lesson heard, sir. You won't get any more lip from Thatch and I.
        ->1c

=== 1c ===

Good. Now, why are you here?

{
-wisdom >= 2:
    +\*Yawn*. I'm a new slave. Arrived last night, but they've had me working with Thatch until now. <Wis {wisdom}/2>
        Your luck must be awful if you've been selected to be this lout's hutmate. Don't expect to get much rest in the future. *Vazul chuckles.*
        ->2a
}

{
-charisma >= 2:
    +I was sent by Guard László. He wants me to collect Thatch for yet another work detail. *Show badge* <Cha {charisma}/2>
        \*Vazul looks Thatch over and smirks.* And you just got off work, too. They're really running you ragged.

        ->3a
-else:

    +Guard László has me running errands. I'm new and I forgot where to find him. *Show badge* <Cha {charisma}/2>
        ->1d
}

=== 1d ===

Whatever Guard László has you doing, he wouldn't want you wandering around. Maybe a lashing will teach you to remember your orders.

    ->Combat

=== 2a === //Wisdom

{
-not vazulMentionedSlatesFate:

    +If I was lucky, I wouldn't have a brand.
        Watch that tongue of yours. The last slave to talk like that was ol' Slate over there.

        ->showSlatesBody(->2b, vazulIndex)
    +No, sir. Not lucky, sir.
        I'm glad you see it that way. But you're still luckier than ol' Slate over there.

        ->showSlatesBody(->2b, vazulIndex)
    +I was told I would have another hutmate, someone named Slate. Is he here?
        Slate? Oh, he's around here somewhere. Ah, there he is.

        ->showSlatesBody(->2b, vazulIndex)
-vazulMentionedSlatesFate:
    +If I was lucky, I wouldn't have a brand.
        Watch that tongue of yours. The last slave to talk like that was ol' Slate over there.

        ->vazulTauntsThatchAndLeaves
    +No, sir. Not lucky, sir.
        I'm glad you see it that way. But you're still luckier than ol' Slate over there.

        ->vazulTauntsThatchAndLeaves
}

=== 2b ===

He's not so talkative now, is he?

I'll see you around, Thatch. Don't sleep too soundly. I'm sure you'll be put back to work real soon. *Vazul laughs loudly as he moves to leave.*

->vazulTauntsThatchAndLeaves

=== 3a ===  // I was sent by Guard László. He wants me to collect Thatch for yet another work detail. <Cha {charisma}/2>
            // \*Vazul looks Thatch over and smirks.* And you just got off work, too. They're really running you ragged.

{
-not vazulMentionedSlatesFate:

    +They built him sturdy. He'll be fine.
        I'd be careful about making friends with him. The last friend he had didn't fair too well.

        ->showSlatesBody(->3b, vazulIndex)
    +And here I thought he always looked like that.
        As bad as he looks, he looks like a prince next to ol' Slate over there.

        ->showSlatesBody(->3b, vazulIndex)
    +I was told Thatch had a hutmate, someone named Slate. Is he here?
        Slate? Oh, he's around here somewhere. Ah, there he is. 

        ->showSlatesBody(->3b, vazulIndex)
-vazulMentionedSlatesFate:
    +They built him sturdy. He'll be fine.

        keepDialogue()
        I'd be careful about making friends with him. The last friend he had didn't fair too well.

        ->3b
    +And here I thought he always looked like that.
        keepDialogue()
        As bad he looks, he looks like a prince next to ol' Slate over there.
        ->3b
}

=== 3b ===

He won't be doing much work anymore. Poor sod.

I'll see you around, Thatch. Hopefully you can get a little sleep after this. *Vazul laughs loudly as he moves to leave.*

->vazulTauntsThatchAndLeaves

=== 4a ===

Wait, lets talk about this. Surely there's some way I can make it worth your while to let me go.

    +Shut it. I ask the questions. Where is Slate?
        \*Vazul begins to sweat.* He's in the corner.

        ->showSlatesBody(->4b, thatchIndex)

=== 4b ===

setToTrue(thatchBeginsStranglingVazul)
I should have been here. I should have... *Thatch turns to Vazul and wraps his powerful hands about the guard's throat.*

    +\*Remove the guard's weapon.*
        ->killVazul(->4c)
    +Thatch, put him down.
        ->5a

=== 4c ===

activateQuestStep(Look for Thatch, 3)
changeCamTarget({vazulIndex})

prepForItem()

\*The guard's eyes begin to bulge. Thatch doesn't let up, and soon the guard's fumbling attempts to escape the big man's grasp cease. Finally, Thatch drops the guard's corpse in a clump at his feet.*

giveItem(1,8,1)

    ->4caa

=== 4caa ===

    +One less guard to deal with. 
        ->4cd
    +Feel better now?
        ->4d
    +I'm sorry about Slate.
        ->4ea
    +Score! A knife.
        ->4ca

=== 4ca ===
    changeCamTarget({thatchIndex})

    \*Thatch considers you with a weary scowl.* You don't let much get to you, do you.

{
-thatchBeginsStranglingVazul:
    +You see one man strangled, you've seen them all, frankly. 
        ->4cc
}

    +If we're going to survive, we have to keep at least a little cheer. Otherwise, we might as well lay down and die.
        ->4za
    +What's with the look? He didn't kill *my* friend.
        ->4cb

=== 4cb ===

No. He didn't. Whatever, lets get going. I don't want to stay here any longer than I have to.

    +\*Leave.*
        ->Close

=== 4cc ===

You've been through it, haven't you. I would hear your story, when we have a moment. But for now, let us leave. It would not do to linger here.

    +Let's return to Kastor then.
        ->Close

=== 4cd ===
changeCamTarget({thatchIndex})

And a whole camp's worth to go. Though, I expect none will be more vile than he was.
    
    +Who was Slate to you?
        ->4da
    +Time is short. Let's return to Kastor.
        ->Close

=== 4d === //No. My head and heart both ache.

    changeCamTarget({thatchIndex})

    No. My head and heart both ache.

    +Who was Slate to you?
        ->4da
    +Time is short. Let's return to Kastor.
        ->Close

=== 4da === //No. My head and heart both ache.

I was a silversmith's apprentice before I was branded. He was a tailor, before his. He had a workshop of his own, and the way he would talk about it reminded me of the woman I apprenticed under.

I did not know him well. But in this camp, friends are rare. If you find one, cling to them hard, lest they be ripped from your grasp.

    +I'm sorry for your loss.
        ->4ea
    +Time is short. Let's return to Kastor.
        ->Close

=== 4ea === 

changeCamTarget({thatchIndex})

\*Thatch sighs.* Don't be. His death is no one's fault but my own.

    +Stop being bullheaded. What were you to do, fight all the guards by yourself?
        ->4eb
    +I just hope you will watch my back better than you watched his.
        You're a real piece of work, you know that? Whatever, let's stop wasting time here and get moving.
        ->Close
    +You're angry at yourself, but that anger serves no purpose. Be angry at the guards instead. That at least will motivate you to escape.
        ->4ec

=== 4eb === 

combineDialogue()

Bullheaded? A trait that has kept me well, over the years. 
    ->4f

=== 4ec === 

combineDialogue()

You underestimate me. I can be angry at both. 
    ->4f

=== 4f === 

My body wants to fight. I want to avenge every slight the guards have committed against me. But it's hard to muscle on in this camp, where you can fight as hard as you wish but your friends die around you all the same. Where is the hope in that?
    
    +Slate was dead long before you chose to fight with me. You've fought the guards, and walked away unhurt. There is hope yet, in that.
        ->4fa
    +Would Slate wish for his death to rob you of hope? The way you speak of him, I doubt he would.
        ->4fb
    +If you require hope to kill guards, then you're in for disappointment. The sooner you accept there is no hope, the quicker you'll be able to contribute to the cause.
        ->4fc

=== 4fa ===

A meager hope. The guards are many. We are but two slaves, rags and callused hands. Nothing more.

    +You forget, we are many too. Kastor is with us, and all the others he has recruited. Kindle that meager hope for now. I promise it will spark something, before long.
        I will follow you, for now. I have revenge on my mind. But your words do make me wonder how far you'll get. I want to be there to see it.
        ->Close
    +The day is not yet over. Nothing is certain, especially our defeat. Two slaves can accomplish much, with their rags and hands.
        Hmmph. When I describe us, I see only the odds. But when you speak, you evoke how easily you will defy them. I will follow you, for now. I wish to see how far you get.
        ->Close

=== 4fb ===

No... you're right. He would not. But it is a sickly hope indeed if it is the thoughts of the dead that keeps it alive.

    +I'll take a frail hope over none at all, any day. The dead will just have to keep it alive until the living can be bothered to nurse it themselves.
        \*Thatch chuckles.* Surely the living are not so lazy. Fine then, despite my efforts, you've coaxed a smile from me. I'll follow you, if only to see how far we get. Lead on.
        ->Close
    +I can keep enough hope for the both of us. Stay close to me. You'll catch it yourself, after a time. I'm sure of it.
        Do not hold your breath. But I will follow you, for now. I have revenge on my mind, and I wish to see how far we get. Lead on.
        ->Close

=== 4fc ===

The cause is hopeless, but join up all the same? That's your pitch? Do you get many recruits with that sermon?

    +You joke, but I'm deathly serious. The pot is revenge and freedom, the ante is your blood and sweat. If you are not keen on the action I offer, stay here and be worked to death. 
        No, you are right. Anything is better than that. And I was foolish to expect there to be a silver lining, after all I've been through. Lead on, to whatever comes. I will follow.
        ->Close
    +That pitch must sound poor against all the offers you've been getting lately. What's that? I'm your only option? It sounds like your choice is made for you.
        I hate it, but I will begrudingly admit you are right. And I was foolish to expect there to be a silver lining, after all I've been through. Lead on, to whatever comes. I will follow.
        ->Close
=== 4z ===

    +Good. Let's return to Kastor then.
        ->Close

=== 4za ===

There's some wisdom in that. I just hope you take the plan more seriously than you're taking this.

    +Of course. Let's return to Kastor and tell him what has happened here.
        ->Close

=== 5a === 
//    +Thatch, put him down.
//        ->Close

changeCamTarget({thatchIndex})

\*Thatch acts like he doesn't hear you. Vazul's eyes begin to bulge under the pressure of Thatch's effort.*

    +Thatch, I said enough!
        ->5b

=== 5b === 

\*Brought back to the world by your words, Thatch lets go of Vazul. Vazul falls to the floor, gasping for air.*

    +I'm not against killing guards, but there's no reason to draw it out like that. *Finish off Vazul.*
        ->killVazul(->5ba)
    +Thank you. He's more use to us alive, anyways.
        ->5c

=== 5ba === 

activateQuestStep(Look for Thatch, 3)

I guess I can respect that. Even if I wish you had let me end it my way.

prepForItem()

\*Thatch reaches down and takes the guard's weapon from his body.* Here. I don't have much use for it, but maybe it will prove useful to you.

giveItem(1,8,1)

    ->4caa

=== 5c ===

activateQuestStep(Look for Thatch, 5)

Alive? How are we going to keep him prisoner without anyone noticing him?

    +We can bind his hands and feet with some spare rags. Then we gag him, and bury him in the rubble.
        ->5d

=== 5d === 

Hmmph. Seems like a lot of effort to go to to keep him alive. But I said I'd follow your lead...

    +We'll come back for him when we're able. *Bury Vazul.*
        ->5d

=== 5e === 

    fadeToBlack()

    deactivate({vazulIndex})

    fadeBackIn()

    prepForItem()

    Here. He had a knife on him. Maybe you can get some use out of it.

    giveItem(1,8,1)

    +Feel better now?
        ->4d
    +I'm sorry about Slate.
        ->4ea
    +Score! A knife.
        ->5ea

=== 5ea ===

changeCamTarget({thatchIndex})

\*Thatch considers you with a weary scowl.* You don't let much get to you, do you.

    +If we're going to survive, we have to keep at least a little cheer. Otherwise, we might as well lay down and die.
        ->4za
    +What's with the look? He didn't kill *my* friend.
        ->4cb
    +He's not the first man I've buried alive, and I expect the Gods won't let him be the last.
        ->4cc

=== 6a ===

fadeToBlack()

deactivate({vazulIndex})

fadeBackIn()

changeCamTarget({thatchIndex})

Why? Of all the guards, I can think of none more vile than that one.

{
-givenAdviceFromBalint:
    +Bálint told me "you may only ever be able to pick one battle, make sure it is one you can win." I'm saving our energies for that battle.
        Bálint is wise. I trust his judgement. I will have to take my revenge on Slate's killer later.
            ->6b
-else:
    +The less fights we start, the more energy we'll have for when it matters.
        Even tired as I am, I can hear the wisdom in your words. I must focus on the plan now. I will have to take my revenge on Slate's killer later.
            ->6b
}

    +If we let him go, he may tell others we've been seen working together. It'll make it less suspicious if we're seen outside.
        I think it could just as easily draw more attention to us. But it does not matter now. Slate is dead. I will have to take my revenge on his killer later.
            ->6b

    +The fewer guards we kill, the less questions will be asked about their whereabouts.
        Right. I must focus on the plan now. I will have to take my revenge on Slate's killer later.
            ->6b

=== 6b ===

    +Who was Slate to you?
        ->4da
    +Time is short. Let's return to Kastor.
        ->Close

=== 7a ===

setToTrue(foundSlate)

\*The guard turns to regard you as you approach. He chews something, but spits before speaking.* Who are you and why are you ripping holes in the damn walls.
    
    +I could ask you the same thing. There's rubble everywhere in here.
        ->7b
    +You got any more of that chew?
        ->7aa
    +Who's that dead guy behind you?
        ->7c

=== 7aa ===

What's it to you, slave? And you haven't answered my question.

    +\*Shrug* Eh, what the heck. I've killed for less than a bit of chew before. <Combat>
        setToTrue(saidKilledForLessThanChew)
            ->CombatWithoutDialogue
    +You're not getting one. I'm leaving.
        Oh no you're not. You're going to get a lashing for speaking to me like that.
            ->CombatWithoutDialogue

=== 7b ===

One of the branded was giving us too much back talk. Some of the other guards and I showed him what we do to flippant prisoners, and it got a little out of hand. Here, let me show you.

    ->CombatWithoutDialogue

=== 7c ===

Oh, Slate? He talked when he should have worked a few too many times. Now he does neither. *Vazul chuckles to himself.*

    +I don't know who this Slate guy was, but if you didn't like him he must have been a saint. <Combat>
            ->CombatWithoutDialogue
    +Sorry, I think I hear someone calling for me. *Leave.*
        Not so fast. You've made more work for someone by tearing that hole in the wall. And I can't abide an unproductive slave.
            ->CombatWithoutDialogue

=== 8a ===

    changeCamTarget({thatchIndex})
    I can see Slate's body. Vazul deserved worse than he got. Far worse. 

{
-saidKilledForLessThanChew:
    +One less guard to deal with. 
        ->4cd
    +Do you feel any better with Vazul gone?
        ->4d
    +I'm sorry about Slate.
        ->4ea
    +Score! Some chew *and* a knife.
        ->8b
-else:
    +One less guard to deal with. 
        ->4cd
    +Do you feel any better with Vazul gone?
        ->4d
    +I'm sorry about Slate.
        ->4ea
    +Score! A knife.
        ->4ca
}


=== 8b ===

    \*Thatch considers you with a weary scowl.* You don't let much get to you, do you. Was that true, what you said about killing a man for less than some chew?

    +We all got our brand somehow.
        ->8c
    +No, I just said that to keep him off balance.
        ->8d

=== 8c ===

    You're not filling me with confidence. But I said I would help you and your group with your escape attempt if you helped me find Slate, and you held up your end of the bargain. I'll see this through to the end.

        +Who was Slate to you?
            ->4da
        +Good. We should get back to Kastor.
            ->Close

=== 8d ===

    Right. Well, I said I would help you and your group with your escape attempt if you helped me find Slate, and you held up your end of the bargain. I'll see this through to the end.
        
        +Who was Slate to you?
            ->4da
        +Good. We should get back to Kastor.
            ->Close

=== showSlatesBody(->divert, targetIndex) ===

changeCamTarget({slateIndex})

\*Slate lies face down on the dirt floor of the shack. He doesn't move.*

changeCamTarget({targetIndex})
    ->divert

=== killVazul(->divert) ===

fadeToBlack()

kill({vazulIndex})

fadeBackIn(60)

->divert

=== vazulTauntsThatchAndLeaves ===

changeCamTarget({thatchIndex})

\*As soon as Vazul's back is turned, Thatch moves to grab him with two massive hands.*

    +\*Wave off Thatch, and let Vazul go.*
        activateQuestStep(Look for Thatch, 4)
        ->6a

    +\*Back Thatch up.* <Combat>
        ->Combat

=== Combat === 

activateQuestStep(Look for Thatch, 3, true)
enterCombat({vazulFightIndex}, {vazulAfterFightConvoKey})

->Close

=== CombatWithoutDialogue === 

enterCombat({vazulFightIndex})

->Close

=== Close ===

{
-thatchRemovedTutorialRubble:
fadeToBlack()

deactivate({thatchIndex})

fadeBackIn(60)
}

close()

->DONE  