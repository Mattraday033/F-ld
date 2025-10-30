VAR imreWontSpeakToPlayer = false
VAR finishedBalintsTask = false
VAR obtainedMineArmoryKey = false
VAR convincedImre = false
VAR terrifiedImre = false
VAR givenTaskByErvin = false
VAR imreReadyToHelpPlayer = false
VAR kastorStartedRevolt = false
VAR toldImreNeededToRest = false
VAR hasToolBundle = false
VAR gaveKastorToolBundle = false
VAR learnedCampLocationFromCarter = false
VAR attitude = 0
VAR charisma = 0
VAR strength = 0

VAR imreIndex = 1

VAR imreCombatIndex = 0

VAR playerName = ""

//changeCamTarget(int targetIndex)
//keepDialogue()
//setToTrue(string flagName)
//setToFalse(string flagName)

searchInventoryFor(obtainedMineArmoryKey,Key,0)
searchInventoryFor(hasToolBundle,Tool Bundle)

{
-toldImreNeededToRest:
->8b
-imreReadyToHelpPlayer && kastorStartedRevolt:
->8a
-terrifiedImre:
 ->1ac
-convincedImre:
 ->1ab
-imreWontSpeakToPlayer:
 ->1aa
-givenTaskByErvin:
 ->1a
-else:
 ->1ad
}

=== 1ad ===

I'm sorry friend, but I'm trying to use the rest of my break time to relax. Please leave me be.
    ->Close

=== 1ac ===

Oh Gods you're back. Don't worry, I'm working on my pitch right now!
    ->Close


=== 1ab ===

Don't worry about me. I'll do my part.
    ->Close


=== 1aa ===

Get away from me!
    ->Close

=== 1a ===

Hello. Are you new? I don't recognize you, but then again I don't know many of the branded.

    +Yeah, I'm new. Name's {playerName}. Who are you?
        ->1b
    +I'm one of the new slaves. I don't believe we've met.
        ->1b

=== 1b ===

I'm Imre. I work in the Manse kitchens, but I'm on break right now. We're under lockdown, are you supposed to be walking around outside?
    
    +Yes, Guard László told me to run some errands for him. *Show badge*
        ->1d
        
=== 1d ===

Oh I see. Need something?

    +Why are you allowed to take a break alone, outside, while the camp is under lockdown?
        ->1da
    +I don't have anywhere to be until Laszlo comes back. I'm just looking to chat.
        ->1e
    +I'm going to level with you. Some friends and I are planning to escape. Are you in?
        ->1f
    +Actually, yes. Listen carefully while I explain. Don't wander off or I'll have to break your legs.
        ->4a

=== 1da ===

keepDialogue()

Oh, the lockdown is really just for the branded like you. The guards allow us outside because some of our duties involve running errands between the Manse and the guardhouse and it would be too much trouble to keep us inside.
    ->1d


=== 1e ===

Well I've got a few more minutes of my break left. Sit a bit and keep me company.
    
    +Don't mind if I do. So, have you been here a while?
        ->2a
    +I'll take you up on that. Is it hard working in the kitchens?
        ->3a

=== 1f ===

Woah! Keep your voice down! *Imre looks around to see if anyone heard.* Be careful what you say, you could get us in a lot of trouble!

{
-charisma >= 2:
    +I am being careful. Calm down, I already checked and no one is near us. <Cha {charisma}/2>
        ->1fa
}
    +Would you relax? We're alone and everything is fine.
        ->addAttitude(-1, ->1fa)
    +Don't worry, no one is close enough to hear.
        ->addAttitude(-1, ->1fa)

=== 1fa ===

keepDialogue()
Ok, ok, I'm calm. Were you serious?
    ->7c

=== 2a ===

That depends on what you mean. I've been "here" since the camp started, but if you're asking how long the Director has been my master, I've been his for about four years now.
    
    +Four years. That's a long time.
        ->2b
    +So you know him well? What's he like?
        ->2d
    
=== 2b ===

Ayup, and I've got another six before my time is up. I don't like to think about it. But I guess it's nothing compared to what you've got going.

    +Six more years would put anyone on edge.
        //combineDialogue()
        I suppose it would. *Imre sighs.* But like I said, I don't like to think about it.
            ->addAttitude(1,->7a)
    +That's right it's not, so I don't want to hear any whining from you.
            My apologies, that was a little insensitive of me.
            ->addAttitude(-1,->7a)

=== 2d ===

The best thing I can say about him is that he could be worse. I spoke to some other slaves before I moved here with the Director and from what I've heard we don't have it so bad. He's a stern and demanding master, but not overly cruel.

    +In my experience, things can always get worse.

        Isn't that the truth. But enough about my hardships.
            ->addAttitude(1,->7a)

    +Speak for yourself. From what I've seen around camp he's plenty cruel.

        Yes, of course. Forgive me.
            ->addAttitude(-1,->7a)

=== 3a ===

It's no easy task. We have to cook meals for the whole camp. That includes the Director, his household, the guards, and the branded and nonbranded slaves. If we're not cooking, we're cleaning up after the previous meal or prepping for the next one. It's gruelling.

    +Wow, that sounds really tough.
        ->addAttitude(1,->3aa)
    +What, really? That's nothing compared to working the mines.
        ->addAttitude(-1,->3b)
    +Well at least you get breaks. The branded don't get those.
        ->addAttitude(-1,->3c)

=== 3aa ===

combineDialogue()
And that's why they assigned it to slaves. But enough about my hardships.
    ->7a

=== 3b ===

I guess I am complaining to one of the only people who is worse off than I am. Sorry.

    +Don't worry about it.
            ->7a
    +Just don't let me catch you saying that stuff again.
        Of course.
            ->addAttitude(-1,->7a)

=== 3c ===

True, but the branded do not have to work during the lockdown. The camp doesn't stop eating just because the mine is shutdown.
    
    +A fair point.
            ->7a
    +That's not the same! The guards literally are working us to death!
        Right, of course. My apologies.
            ->addAttitude(-1,->7a)

=== 4a ===

U-uh, ok. I'll stay, just please don't touch me.

    +Good. Now that I have your attention, I need you to do something for me.
        ->4b


=== 4b ===

Absolutely! You need anything, I'm your g-guy.

    +I need you to convince the other Manse slaves to help us out. We're planning a breakout.
        -> 4d

/*
=== 4c ===

A breakout? Oh, I don't know about this...

    +\*Lean in close and whisper something horrible in Imre's ear* <Pick a Background>
       ->4d
            
    +You have two choices. Do what I ask, or find out how many ways I know how to break a bone. <Skip Background>
        ->4d
*/          
            
=== 4d ===

A breakout? Oh, I don't know about this... uh, I mean, of course I'll do it. But they won't be easy to convince. What should I tell them?

{
-strength >= 2:
    +Think of something. Find inspiration in how much you enjoy breathing. <Str {strength}/2>
        ->setAttitude(10,->4e)
}

{
-(hasToolBundle or gaveKastorToolBundle):
    *We already have a stash of weapons ready to be passed out. We just need your support.
        ->addAttitude(2,->4da)

-obtainedMineArmoryKey:
    *We already have the key to the armoury. We'll be taking on all the risk.
        ->addAttitude(1,->4da)
}

{
-(finishedBalintsTask or learnedCampLocationFromCarter):
    *We figured out where the camp is. Once we get rid of the guards we're home free.
        ->addAttitude(1,->4db)
}

    +Don't think of the risks, think of the reward: freedom. What say you to that?
        ->4e

=== 4da ===

->majorStep(->4d)

=== 4db ===

keepDialogue()
I always wondered where we are. You've done some legwork, it seems.
    ->4d

=== 4e ===

{
-attitude == 10:

\*Imre gulps* Aha, I w-will do j-j-just that.

    +Good. Now leave. I'm sick of looking at you.
        activateQuestStep(Aiding Ervin,3)
        setToTrue(convincedImre)
        setToTrue(terrifiedImre)
        activateQuestStep(Assist the Nonbranded,0)
            ->Close

-attitude >= 2:
I-I'll tell them exactly what you told me.

    +Good. Now leave. I'm sick of looking at you.
        activateQuestStep(Aiding Ervin,3)
        setToTrue(convincedImre)
        setToTrue(terrifiedImre)
        activateQuestStep(Assist the Nonbranded,0)
            ->Close
    
-else:
Er... Ok, I'll try to make that work. P-Please don't hurt me if I don't sell it as well as you could.

    +Good. Now leave. I'm sick of looking at you.
        activateQuestStep(Aiding Ervin,3)
        setToTrue(terrifiedImre)
        activateQuestStep(Assist the Nonbranded,1)
            ->Close
}

/*
=== 6a ===

Tell me about yourself. Before you found yourself a slave, did you ever have to work hard like this?

    +Oh yes, very often. <Pick a background>
        ->6b
    +No never. My life was quite uneventful before this. <Skip background>
        ->6c
*/

=== 6b ===

That must have been very hard. But then again, I can't imagine it was worse than where you are now.

combineDialogue()

You'll have to forgive me; I think my break is over.
    ->7a

=== 6c ===

Oh? Well, this must be quite the ugly awakening for you.

combineDialogue()

You'll have to forgive me; I think my break is over.
    ->7a

=== 7a ===

It's been fun chatting, but I've been out here a while and I really need to get back to work now.
    
    +Before you go, there's one last thing. I actually do need your help with something.
        ->7b

=== 7b ===

Name it but make it quick. I cannot be late.

    +Some friends and I are planning on adjusting our sentences. Are you interested?
        ->7c

=== 7c ===

\*Imre looks around to make sure no one is watching, then he leans in close.* Are you saying what I think you are?

    +Yep, I've got a plan to bust out of here. It's already in motion too. Interested?
        ->7d

=== 7d ===

If I am, what would you need from me?
    
    +We need you to get the other brandless on board.
        ->7e

=== 7e ===

On board for what?

    +We're planning a breakout. We're going to fight our way out of here. The more hands the better.
            ->7f
            
=== 7f ===

This is sounding risky. How do I know this will even work?

{
-charisma >= 2:
    +We're keeping everything need-to-know. Just do your part. I'll handle the rest. <Cha {charisma}/2>
        ->setAttitude(10,->7fa)
}

{
-(hasToolBundle or gaveKastorToolBundle):
    *We already have a stash of weapons ready to be passed out. We just need your support.
        ->addAttitude(2,->7fb)
-obtainedMineArmoryKey:
    *We already have the key to the armoury. We'll be taking on all the risk.
        ->addAttitude(1,->7fb)
}

{
-(finishedBalintsTask or learnedCampLocationFromCarter):
    *We figured out where the camp is. Once we get rid of the guards we're home free.
        ->addAttitude(1,->7fc)
}

    +Don't think of the risks, think of the reward: freedom. What say you to that?
        ->7g

=== 7fa ===

keepDialogue()
I can do that. I should be able to get the Manse slaves on our side by the time you make your move.
    ->7g

=== 7fb ===

->majorStep(->7f)

=== 7fc ===

->homeFree(->7f)

=== 7g ===

{
-attitude >= 1:
I see you've put in the work. This should be an easy sell to get the rest of the Manse staff on our side.

    +"Our" side? So you're in?
        setToTrue(convincedImre)
        activateQuestStep(Aiding Ervin,2)
        activateQuestStep(Assist the Nonbranded,0)
        ->7h
    
-else:
I'm not going to throw my lot in with someone I just met. I'm late coming back from break, so if you'll excuse me...

    +You're making a huge mistake, but it's yours to make. I'll be going as well.
        setToTrue(imreWontSpeakToPlayer)
        activateQuestStep(Aiding Ervin,4)
        ->Close
        
    +I'm sorry, but if you wont join us I can't let you live. <Combat>
        activateQuestStep(Aiding Ervin,5)
        enterCombat({imreCombatIndex})
        ->Close
}


=== 7h ===

We're in this together now. I'm overdue to come back from break. You tell the others we'll be ready when you need us. Just don't leave us to the guards.

    +Of course. Next time I see you will be when the fighting starts.
        ->Close

=== 8a ===

You've kept to your word and I've kept mine. The other Manse slaves stand ready to assist you in any way they can. However, we have a small problem.

activateQuestStep(Assist the Nonbranded, 3)

The head chef has kept us penned up in the kitchen while we wait out the riots. I only managed to sneak away because I had Pan catch a beating to provide a distraction. I've unbarred the doors, but we have no time to waste. Are you ready to enter the Manse? Once you enter the kitchens, the guards will rush to block your passage.

    +Yes, I'm ready. Lead the way.
        setToTrue(askedImreToLeadTheWay)
        deactivate(1)
        ->Close
    +No, I need a moment.
        setToTrue(toldImreNeededToRest)
        I'll wait here until you're back.
        ->Close

=== 8b ===

Are you ready to enter the kitchens?

    +Yes, I'm ready. Lead the way.
        setToTrue(askedImreToLeadTheWay)
        deactivate(1)
        ->Close
    +No, I need a moment to rest.
        setToTrue(toldImreNeededToRest)
        I'll wait here until you're back.
        ->Close

=== homeFree(->divert) ===

keepDialogue()
Home free. That does sound good.
    ->divert

=== majorStep(->divert) ===

keepDialogue()
That's a major step.
    ->divert

=== addAttitude(amount, ->divert) ===

~attitude += amount
/*
{
-amount > 0:
    Incremented attitude by {amount}. Attitude is now {attitude}.

-amount < 0:
    Decremented attitude by {amount}. Attitude is now {attitude}.

-else:
    No change in attitude. Attitude is still {attitude}.
}
*/
    -> divert

=== setAttitude(newAttitude, ->divert) ===

~attitude = newAttitude

//Attitude set to {attitude}.

    -> divert

=== Close ===

close()

->DONE

/* 4d intimidate if statements
{
-(finishedBalintsTask or learnedCampLocationFromCarter) and (hasToolBundle or gaveKastorToolBundle):
    {
    -strength >= 2:
        +Think of something. Find inspiration in how much you enjoy breathing. <Str {strength}/2>
            ~attitude = 10
               ->4e
    }
        *We already have a stash of weapons ready to be passed out. We just need your support.
            ~attitude++
            ~attitude++
            keepDialogue()
            That's a major step. That should be very convincing.
                ->4d
        *We figured out where the camp is. Once we get rid of the guards we're home free.
            ~attitude++
            keepDialogue()
            Home free. I think they'll like the sound of that.
                ->4d
        +Tell them not to think of the risks, but to think of the reward: freedom. What say you to that?
            ->4e
-(finishedBalintsTask or learnedCampLocationFromCarter) and obtainedMineArmoryKey:
    {
    -strength >= 2:
        +Think of something. Find inspiration in how much you enjoy breathing. <Str {strength}/2>
            ~attitude = 10
               ->4e
    }
        *We already have the key to the armoury. We'll be taking on all the risk.
            ~attitude++
            keepDialogue()
            That's a major step. That should be very convincing.
                ->4d
        *We figured out where the camp is. Once we get rid of the guards we're home free.
            ~attitude++
            keepDialogue()
            Home free. I think they'll like the sound of that.
                ->4d
        +Tell them not to think of the risks, but to think of the reward: freedom. What say you to that?
            ->4e
-(finishedBalintsTask or learnedCampLocationFromCarter):
    {
    -strength >= 2:
        +Think of something. Find inspiration in how much you enjoy breathing. <Str {strength}/2>
                ->4e
    }
        *We figured out where the camp is. Once we get rid of the guards we're home free.
            ~attitude++
            keepDialogue()
            Home free. I like the sound of that.
                ->4d
        +Tell them not to think of the risks, but to think of the reward: freedom. What say you to that?
                ->4e
-(hasToolBundle or gaveKastorToolBundle):
    {
    -strength >= 2:
        +Think of something. Find inspiration in how much you enjoy breathing. <Str {strength}/2>
            ~attitude = 10
               ->4e
    }
        *We already have a stash of weapons ready to be passed out. We just need your support.
            ~attitude++
            ~attitude++
            keepDialogue()
            That's a major step. That should be very convincing.
                ->7f
        +Tell them not to think of the risks, but to think of the reward: freedom. What say you to that?
            ->4e
-obtainedMineArmoryKey:
    {
    -strength >= 2:
        +Think of something. Find inspiration in how much you enjoy breathing. <Str {strength}/2>
        ~attitude = 10
            ->4e
    }
        *We already have the key to the armoury. We'll be taking on all the risk.
            ~attitude++
            keepDialogue()
            That's a major step. You at least know what you're doing.
                ->4d
        +Tell them not to think of the risks, but to think of the reward: freedom. What say you to that?
                ->4e

-else:
    {
    -strength >= 2:
        ++Think of something. Find inspiration in how much you enjoy breathing. <Str {strength}/2>
            ~attitude = 10
            ->4e
    }
        +Tell them not to think of the risks, but to think of the reward: freedom. What say you to that?
            ->4e
}
*/

/////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////

/* 7f persuasion if statements
{
-(finishedBalintsTask or learnedCampLocationFromCarter) and (hasToolBundle or gaveKastorToolBundle):
    {
    -charisma >= 2:
        +We're keeping everything need-to-know. Just do your part. I'll handle the rest. <Cha {charisma}/2>
            ~attitude = 10
            keepDialogue()
            I can do that. I should be able to get the Manse slaves on our side by the time you make your move.
                ->7g
    }
        *We already have a stash of weapons ready to be passed out. We just need your support.
            ~attitude++
            ~attitude++
            keepDialogue()
            That's a major step. You at least know what you're doing.
                ->7f
        *We figured out where the camp is. Once we get rid of the guards we're home free.
            ~attitude++
            keepDialogue()
            Home free. I like the sound of that.
                ->7f
        +Don't think of the risks, think of the reward: freedom. What say you to that?
                ->7g
-(finishedBalintsTask or learnedCampLocationFromCarter) and obtainedMineArmoryKey:
    {
    -charisma >= 2:
        +We're keeping everything need-to-know. Just do your part. I'll handle the rest. <Cha {charisma}/2>
            ~attitude = 10
            keepDialogue()
            I can do that. I should be able to get the Manse slaves on our side by the time you make your move.
                ->7g
    }
        *We already have the key to the armoury. We'll be taking on all the risk.
            ~attitude++
            keepDialogue()
            That's a major step. You at least know what you're doing.
                ->7f
        *We figured out where the camp is. Once we get rid of the guards we're home free.
            ~attitude++
            keepDialogue()
            Home free. I like the sound of that.
                ->7f
        +Don't think of the risks, think of the reward: freedom. What say you to that?
                ->7g
-(finishedBalintsTask or learnedCampLocationFromCarter):
    {
    -charisma >= 2:
        +We're keeping everything need-to-know. Just do your part. I'll handle the rest. <Cha {charisma}/2>
            ~attitude = 10
            keepDialogue()
            I can do that. I should be able to get the Manse slaves on our side by the time you make your move.
                ->7g
    }
        *We figured out where the camp is. Once we get rid of the guards we're home free.
            ~attitude++
            keepDialogue()
            Home free. I like the sound of that.
                ->7f
        +Don't think of the risks, think of the reward: freedom. What say you to that?
                ->7g
-hasToolBundle or gaveKastorToolBundle:
    {
    -charisma >= 2:
        +We're keeping everything need-to-know. Just do your part. I'll handle the rest. <Cha {charisma}/2>
            ~attitude = 10
            keepDialogue()
            I can do that. I should be able to get the Manse slaves on our side by the time you make your move.
                ->7g
    }
        *We already have a stash of weapons ready to be passed out. We just need your support.
            ~attitude++
            ~attitude++
            keepDialogue()
            That's a major step. You at least know what you're doing.
                ->7f
        +Don't think of the risks, think of the reward: freedom. What say you to that?
                ->7g
-obtainedMineArmoryKey:
    {
    -charisma >= 2:
        +We're keeping everything need-to-know. Just do your part. I'll handle the rest. <Cha {charisma}/2>
            ~attitude = 10
            keepDialogue()
            I can do that. I should be able to get the Manse slaves on our side by the time you make your move.
                ->7g
    }
        *We already have the key to the armoury. We'll be taking on all the risk.
            ~attitude++
            keepDialogue()
            That's a major step. You at least know what you're doing.
                ->7f
        +Don't think of the risks, think of the reward: freedom. What say you to that?
                ->7g
-else:
    {
    -charisma >= 2:
        +We're keeping everything need-to-know. Just do your part. I'll handle the rest. <Cha {charisma}/2>
            ~attitude = 10
            keepDialogue()
            I can do that. I should be able to get the Manse slaves on our side by the time you make your move.
                ->7g
    }
        +Don't think of the risks, think of the reward: freedom. What say you to that?
                ->7g
}

*/