VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerIndex = 0
VAR weftIndex = 1
VAR overseerIndex = 2

VAR heardTaborsLesson = false
VAR weftAddedToParty = false
VAR weftKnowsYouKnowHisSecret = false
VAR liedToWeftAboutHearingExtortion = false

VAR succeededCharismaCheck = false
VAR failedCharismaCheck = false

VAR playerName = ""

setToTrue(metWeft)

{
-weftAddedToParty:
    ->2a
-else:
    ->1a
}

=== 1a ===

deactivate({playerIndex})
setNPCFacing({weftIndex},SE)
activate({overseerIndex})
changeCamTarget({overseerIndex})

Did he give it to you?

changeCamTarget({weftIndex})

Yeah, two packs of chew. Just like you asked.

changeCamTarget({overseerIndex})

Excellent. Hand them over.

changeCamTarget({weftIndex})

This is getting out of hand. I'm sick of running errands for you. One of these days I'm going to get caught, and then we'll both be in trouble.

changeCamTarget({overseerIndex})

Don't threaten me, scum. If it weren't for me, you'd have been done in for organizing that stunt you pulled with the mess hall. And you wouldn't have just gotten a hiding like the others neither. The ringleader gets the executioner's axe. That's always been the rule.

changeCamTarget({weftIndex})

...

changeCamTarget({overseerIndex})

That's what I like: a silent worker. And should you ever get uppity again, the Director's gonna find some new evidence on his desk before you can say your first apology.

disableDialogueUI()
manualFadeToBlack()
stopAllFades()

activate({playerIndex})
changeCamTarget({playerIndex})

slowFadeBackIn(2)
wait(1)
enableDialogueUI()
setNPCFacing({weftIndex},SW)
setNPCFacing({overseerIndex},SW)
changeCamTarget({overseerIndex})

Seems like you've got company. I'll give you your next assignment later. Out of my way, slave.

fadeToBlack()
deactivate({overseerIndex})
fadeBackIn(60)

changeCamTarget({weftIndex})

\*The man before you has the same brand on his neck as the others you've seen, but wears much nicer clothing. He looks at you with a sour expression.* You didn't hear any of that before, did you?


    +I did, but your secret is safe with me. <Cha {charisma}/2>
        setToTrue(weftKnowsYouKnowHisSecret)
        {
        -charisma >= 2:
            ->1aa
        -else:
            ->1ab
        }
    +Hear what?
        setToTrue(liedToWeftAboutHearingExtortion)
        ->1ac
    +It sounds like you could get in a lot of trouble if the guards found out about your little secret.
        setToTrue(weftKnowsYouKnowHisSecret)
        ->1ad

=== 1aa ===

~succeededCharismaCheck = true

This is a lot of trust for me to put in the hands of stranger, but I suppose I have no other choice but to hope you mean that. 

    +It's been that kind of day. I'm {playerName}, by the way. Guard László said we are to share a hut and to deliver you your rations. 
        ->1b

=== 1ab ===

~failedCharismaCheck = true

I doubt that. If you try to extort me like the overseer did, I'll make you pay dearly for it.

    +There's no need for threats. Let's start over. I'm {playerName}. Guard László said we are to share a hut and to deliver you your rations. 
        ->1b

=== 1ac ===

Nevermind then. I'm Weft. Did someone send you looking for me?

    +My name is {playerName}. Guard László said we are to share a hut and to deliver you your rations. 
        ->1b

=== 1ad ===

Perhaps I could. What's it to you?

    +I think you'll owe me a favor for keeping it. When I figure out what that will be, I'll let you know.
        ->1ada

    +Nothing. Let's drop it, yeah?
        ->1ab

=== 1ada ===

\*Weft nods slowly.* Fine then. One favor, for your silence. But make it too big and we'll have problems. Now, why are you here? Did someone send you to get me?
    +Guard László said we are to share a hut and to deliver you your rations. 
        ->1b

=== 1b ===

finishQuest(My New Hutmate,true,Rations delivered.)

prepItem()

Ah, I thought you were just another of the branded. I'm Weft. I take it you're new?

exchangeItemForXP(Weft's Rations,1,50)

    +That's right. Fresh off the cart this morning.
        You move fast then, if you've already been sent <i>here</i>. And it's well you that you have been: this hut is for the branded that have a future. All the others aren't worth your time.
        ->1c

=== 1c ===

    +Why is that?
        \*Weft shrugs as he eats his rations.* This camp is a dangerous place for them. They're unlikely to outlive it.
        ->1c
    +You have a mighty high opinion of yourself.
        I've been set aside for greater things. The Lovashi wouldn't waste clothes and a bed on a slave they didn't mean to keep around.
        ->1ca
    +You've got that right.
        I'm glad we see things the same way. It can be a lonely experience to be recognized like this. Even the Manse servants look down their nose at us. That's why we've got to stick together.
        ->1ca

=== 1ca ===

    +What did you do to get recognized?
        ->1cb

=== 1cb ===

Months ago, when the camp was first founded, a few of us snuck out of our huts to grab some food from the Mess Hall. I didn't want to go, mind you, but my hutmate at the time said he'd kill me if I didn't come along.

Of course, we were found out after the food was reported missing. The writing was on the wall for the other conspirators, so I told the Lovashi who was involved. Don't forget that the guards are sure to reward such behavior, if you ever find yourself in the same predicament.

After the others were punished, I got set up here with a larger portion for my rations, and a nice warm bed instead of grimy straw. Completely worth it, if you ask me.

{
-wisdom >= 2:
    +Ah, and the guards don't know you were really the ringleader of the group. That's how that overseer is blackmailing you.
    setToTrue(weftKnowsYouKnowHisSecret)
    {
    -liedToWeftAboutHearingExtortion:
        ->1cba
    -else:
        ->1cbb
    }
    +Seems to me that the guards are using you. You're basically an advertisement for sedition. <Wis {wisdom}/2>
        The more useful I am, the less they can afford to get rid of me. That's how a branded clings to life. What did you do to make the Lovashi think you were useful?
        ->1cc
-else:
    +Ah, and the guards don't know you were really the ringleader of the group. That's how that overseer is blackmailing you.
    setToTrue(weftKnowsYouKnowHisSecret)
    {
    -liedToWeftAboutHearingExtortion:
        ->1cba
    -else:
        ->1cbb
    }
}

    +And you're proud of this? You disgust me.
        Oh, do I? We're in the same boat now, friend. What did Your Highness do to get placed above the rest of our kin if not something equally repulsive?
        ->1cc
    +You're a survivor. That's what's important, at the end of the day.
        And like respects like. What did you do to get sent here?
        ->1cc
    +That's not much different than what I did, really.
        I expected so. What exactly <i>did</i> you have to do to gain the recognition of the guards?
        ->1cc

=== 1cba ===

So you did hear what we were talking about! As the Mother is my witness, if you tell anyone about that I will make you regret it.

    +Your secret is safe with me.
        combineDialogue()
        I'm not confident that's true, but I'm through talking about this. 

        ->1e
    +Whatever. Let's talk about something else then.
    
        combineDialogue()
        I think the time for talk is over. 
        ->1e

=== 1cbb ===

\*Weft grimaces.* I'd rather not discuss that. 

    +Fine, let's talk about something else then.

        combineDialogue()
        I think the time for talk is over. 
        ->1e

    +I should probably be getting back to Guard László anyways.

        combineDialogue()
        No need, //intentionally cut off
        ->1e

=== 1cc ===

    +I'm not willing to say.
        \*Weft smirks.* That bad, eh? Well, it doesn't matter to me. I'm sure I'll hear about it eventually.
        ->1d
    +Someone tried to recruit me to a plan to escape. I got him sent to the Pit instead.
        The Lovashi are always suspecting the branded of working out an escape plan. Knowing you're the type to reject such a plan would certainly put them at ease around you. Well done.
        ->1d

=== 1d ===

I was one of the first branded here, so I'll give you some advice. There are two types of guards: the sadists, and the fanatics. You deal with them each a little differently.

The sadists are here for the power and cheap thrills, but they have plenty of better targets than you or I. Don't look them in the eye, say 'yessir' or 'yes ma'am', and they'll move on to easier prey. 

The fanatics on the other hand are here to make sure you learn something. Give too quick of an answer to their questions and they'll think you aren't paying attention. That's when the 'lessons' start.

{
-heardTaborsLesson:
    +Chief Tabor has already given me the whole speech. 

        The head fanatic himself, lucky you. He's a tricky one; he's both the most fair of them, and the most ready to order a flogging if you slip up. Don't let your guard down around him.

        ->1e
}

    +What could I possibly learn from these monsters?

        Oh, they aren't real lessons. It's more like indoctrination. The Lovashi are a culture of horsemen, and they want you to learn to treasure horses like they do. They've got their own language they can talk to them with, so it's more than some superstition.

        Don't worry if it's all confusing at first, the guards will beat it into you before long. Make it obvious you're listening, or that may become more literal than I meant it.

        ->1e
    

=== 1e ===

Chief Tabor has me working on something and hutmates always work together in teams, so that means you've gotta come along too. We'd best report to him before he comes looking for us.

We can probably find Chief Tabor in the center of the camp, working someone over at the whipping posts. I'll follow you, that way you'll learn the lay of the camp better. Just be sure to set a good pace, or else you'll be accosted for dawdling. 

fadeToBlack()

activateQuestStep(Chief Tabor,Find Chief Tabor.)
setToTrue(weftAddedToParty)
addToParty({weftIndex})
deactivate({weftIndex})

fadeBackIn(60)

->Close

=== 2a ===

If I'm needed for something, say the word. I'll be right behind you.

->Close

=== Close ===

close()

->DONE  