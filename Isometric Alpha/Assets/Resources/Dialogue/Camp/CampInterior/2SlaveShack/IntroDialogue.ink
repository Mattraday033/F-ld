VAR askedWhyMe = false
VAR askedDoYouWantMeToDoThis = false
VAR impressedGuard = false
VAR needsABut = false
VAR killedBAndG = false
VAR peacefullyRefusedBrush = false
VAR tellTheGuardTheTruth = false
VAR takeGézaAway = false
VAR gotBrushKilledByGuard = false
VAR spokeToGézaAboutPlan = false
VAR skipFirstDialogue = false
VAR charisma = 0
VAR wisdom = 0

VAR rudeness = 0
VAR rudenessThreshold = 3

VAR playerIndex = 0
VAR brushIndex = 1
VAR gézaIndex = 2
VAR laszloActualIndex = 3
VAR laszloVoiceIndex = 4
VAR gézaSecondLocationIndex = 5
VAR gézaPlayingCrazyIndex = 6

duckMusic()
playAnimation({playerIndex},death_back_weaponless)
playDelayedSFX(snoring,100)
setNPCFacing({brushIndex},SE)
stopAllFades()
disableDialogueUI()
changeCamTarget({playerIndex})
slowFadeBackIn(7)
wait(2)

{
-skipFirstDialogue:
    ->Close
-else:
    ->1a
}

=== 1a === //Brush starts

setFacing(sw)
setNPCFacing({brushIndex},SE)

setToTrue(finishedFirstDialogue)

changeCamTarget({brushIndex}) 

enableDialogueUI()

changeCamTarget({brushIndex})

... and you can still find ways to hurt my pride. It's a gift. 

changeCamTarget({gézaIndex})

Discussing the plan is getting us nowhere. Let's just get on with it.

changeCamTarget({brushIndex})

What time do you think it is?

changeCamTarget({gézaIndex})

I hear birds, but there's no sun yet.

changeCamTarget({brushIndex})

How long does that give us until inspection?

changeCamTarget({gézaIndex})

An hour at most. Probably less.

changeCamTarget({brushIndex})

Ok, you're right. We'd better wake the princess.

setNPCFacing({brushIndex},NE)
setNPCFacing({gézaIndex},NE)

Hey, you awake?

    +Yeah, I'm awake.
        playAnimation({playerIndex},ooc_idle_front)
     -> 1c
    
    +No one could sleep through all this talk.
        playAnimation({playerIndex},ooc_idle_front)
     -> 1b
    
=== 1b ===

My apologies. I've had my brand long enough to know how dear sleep can be.

-> 1d

=== 1c ===

Good, this conversation would be pretty one-sided if you weren't. \*Brush smiles.*

-> 1d

=== 1d ===

I'm Brush, and this is Géza. We're your new hutmates. 

changeCamTarget({gézaIndex})

Pleasure to meet you.

+The same to you.

changeCamTarget({brushIndex})
It's good that you're the amicable sort. Géza and I have been stuck in here for a few days now. We're just about sick of one another.
    ->1e

+We're not friends. Keep your distance. 

~rudeness++
changeCamTarget({brushIndex})
We may not be friends, but that doesn't mean we can't be friendly. We're going to be stuck in here for a while, so we should at least learn to tolerate each other.
    ->1e


=== 1e ===

+'Stuck'? Why is that?

    ->1f

=== 1f ===

This camp was set up a few months ago to pull rocks out of a new mine. But the whole camp's on lockdown right now, 'cause the mine's been put out of commission. 

The guards aren't letting anyone out of their huts. Easier to deal with us that way. But sometimes they make exceptions for the slaves that suck up enough.

changeCamTarget({gézaIndex})

Like Weft, that prig.

changeCamTarget({brushIndex})

May the fleas of a thousand goats nest in his armpits. *Brush spits.*

changeCamTarget({gézaIndex})

Gods willing. So, newling, what did you do to get your brand?

changeCamTarget({brushIndex})

\*Brush shoots Géza a glare.* We've all got our reasons for being here, so whatever the cause you'll get no judgement from us. Géza just wants to know if you're the violent sort is all.

->1fa

=== 1fa ===

+Who is this Weft?
    changeCamTarget({gézaIndex})

    Every so often you get a slave that thinks they can carve out a little bit of power by dragging on his fellows. Weft is one of them. He got his start by singing to the guards when a group of us stole extra rations from the mess. And he's only gotten slimier from there.

    ->1fa

+\*Shrug.* I only pick fights I can win. That just happens to be most of them.
    changeCamTarget({gézaIndex})

    That is good. Hutmates need to look out for one another. It's reassuring that you seem like the discerning type.
    ->2a
+I'll be as violent as I'm called to be. And don't either of you forget it.
    ~rudeness++
    changeCamTarget({gézaIndex})

    We're not likely to, and neither are the guards. A fight will earn all involved a flogging, so think before you throw a punch in anger.
    ->2a
+Violence isn't my way. Never has been.
    changeCamTarget({brushIndex})

    Never has a way of arriving early in this camp. Don't expect the other branded to hold to the same principles.
    ->2a

=== 2a ===

+What's this plan I heard you mention before? 
    ->2ba
+This is a waste of my time. I'm going back to sleep.

    changeCamTarget({brushIndex})
    
    combineDialogue()

    Wait a moment. Your time is precious, I understand that. I'll cut to the chase. 
    ->2bb

=== 2ba === 

setNPCFacing({gézaIndex},NW)

changeCamTarget({gézaIndex})

I told you you were too loud.

changeCamTarget({brushIndex})
setNPCFacing({brushIndex},SE)

No, you said "I've seen sheep in heat bleat quieter than you", which I took offense to.

setNPCFacing({brushIndex},NE)
setNPCFacing({gézaIndex},NE)

combineDialogue()

Fine, so we weren't making small talk because we enjoy your company. I'll get to the point. 
    ->2bb

=== 2bb ===

The brand is a death sentence, as I'm sure you know. And some of the other branded and I aren't too keen on being worked to death. So we've come up with a plan to get us all out of here. But we need some help from you to pull it off.

I'll level with you: we wouldn't be asking something like this of a complete stranger if we weren't desperate. Very desperate. That's why we were sizing you up before.

changeCamTarget({gézaIndex})

We don't know your quality, but neither do the guards. If you were to make a good first impression, they may give you some tasks outside this hut. That would mean you could move around during the lockdown, and maybe get some more of the plan done while we're trapped in here.
    ->2b

=== 2b ===  

//So if you don't want to be stuck in this hut like we are, we need the guards to think you're trustworthy. And for that to happen, we need them to think you'll sell out your pals.

    +How desperate are you?
        changeCamTarget({brushIndex})

        We've seen a lot of miners die. After a while, they just drop from exhaustion. And all of the guards act real cagey. They never talk about where this camp is, or why we're here.
        changeCamTarget({gézaIndex})

        We pull a lot of stone out of the mine, but never ore. And a few weeks back we found some ruins on the lower levels. Old rooms, buildings built directly into the stone. The guards are looking for something.
        changeCamTarget({brushIndex})

        We don't know what, but when they find it, it's a sure bet they'll kill the lot of us to keep their secret. So like I said, we're desperate to get out of here.
        -> 2b
    +I don't want the other slaves to think I'm a rat.
        changeCamTarget({brushIndex})

        Don't worry, we'll put out the good word once the lockdown's up that you aren't a bootlicker. Géza and I have been around since the camp started: the others will listen to us.
        -> 2b
    +Why me? Why not either of you?
        changeCamTarget({brushIndex})

        The guards aren't about to hand out any special privileges to Géza and me, we've been around too long. They know we're not the groveling types. But you just got here. If we can give them a good first impression, they'll find a use for you. 

        changeCamTarget({gézaIndex})

        The guards are always antsy about the slaves plotting, so they try to find all the brown-nosers early and keep them on side. That way the slaves are too busy fighting among themselves and the guards don't feel so badly outnumbered.
        -> 2b
    +<I>If</I> I did this, what exactly would I need to do?
         -> 3a

=== 3a ===

changeCamTarget({brushIndex})

There's an inspection every morning and night. The guards will check us for contraband, signs of disease, that sort of thing. If we pass, they'll give us our morning rations.

While they're doing that, you need to accuse me of trying to get you in on an escape plan.

    +Oh, so like you're doing now?
        ->3b

=== 3b ===

\*Brush chuckles.* Yeah, something like that. Once you do, the guards will know you're a suck up. After they cart me off, you'll be sure to receive a nice reward: more work somewhere outside.

    +Won't they kill you for that?
    ->3c

=== 3c === 

Aye, they might. But they'll have to interrogate me first. And I only know one other person who's in on it besides Géza and yourself. He told me that the rest are scattered all over the camp; each hut has to work on one part of the plan. It's separated that way so we can't ruin the whole plan if we're caught.

And if they end up killing me, at least I won't ever have to enter that blasted mine again. Just don't leave me to their hospitality too long. So are you in?

    +I'm in. I'd do anything to get out of here.
        ->4a
    +<Not Implemented> No way I'm getting involved in a plan like this with someone I just met.
        ->3d

=== 3d ===

changeCamTarget({gézaIndex})

Told you they wouldn't go for it.

changeCamTarget({brushIndex})

Give them a second, they haven't let me sweeten the pot yet.

    +I'm listening.
        ->3e

=== 3e ===

We can't have you telling the guards about the plan. If you don't go along with us, when the guards get here I'm gonna tell him <i>you</i> were the one who asked <i>me</i> for help escaping. After that, they won't believe anything you say.

    +Fine, you bastard. I'll help you.
        ->4a
    +Oh yeah? It'll be my word against yours.
        ->8a
    +Threatening me isn't wise, friend. <Combat>
        fadeToBlack()
        changeCamTarget({laszloVoiceIndex})
        deactivate({brushIndex})
        deactivate({gézaIndex})
        setToTrue(killedBrushAndGéza)
        ~killedBAndG = true
        fadeBackIn(60)
        ->7a

->4a
//Agreed to go along with Brush's plan.
=== 4a ===

changeCamTarget({laszloVoiceIndex})

\*A voice barks orders from outside the hut.* Up against the wall! It's inspection time!

{peacefullyRefusedBrush: ->4b}

activateQuestStep(The Plan, I just got here!)

setToTrue(goesWithBrushsPlan)

changeCamTarget({brushIndex})

Okay, this is it. Remember, you're ratting me out to the guard. Lay it on thick and he'll believe you. I'll follow your lead.

    +Right. Got it.
        ->4b
    
    +What's Géza's part?
        ->4c

=== 4b ===

fadeToBlack()

activate({laszloActualIndex})

deactivate({gézaIndex})
activate({gézaPlayingCrazyIndex})

movePlayer(4,0)
setFacing(NW)
setNPCFacing({brushIndex},SE)

changeCamTarget({laszloActualIndex})

fadeBackIn(60)

I said back up against the walls, slaves, or I'm keeping your rations for myself.
  
  {peacefullyRefusedBrush: ->8b}
  
    +Sir! Sir! Brush here's been talking of escape.
    
        setFacing(SW)
        ->4d
    
    +<Not Implemented> *Say a prayer and attack the fully armed guard with nothing but your fists* 
        setToTrue(killedGuardLaszlo)
        ->6a

=== 4c ===

He'll play crazy. Happens to some slaves, the guards won't bat an eye. Easy for him to play the part too, he's halfway there anyway. It's the only way he can seem uninvolved. Now get ready, they're coming in.

->4b

=== 4d ===

What? Brush, you treacherous dog!

    +It's true sir! I told him where he could shove that treasonous speech of his, but he kept moaning about it!
        ->4e

=== 4e ===

changeCamTarget({brushIndex})

Traitor! I should've known you were a rat!

changeCamTarget({laszloActualIndex})

Oh ho! I've been waiting for an excuse to lay into you, Brush. Couldn't help but give it to me, eh?

    +Oh, please show some mercy. He's too simple to know what's good for him!
        ->4j
    //+He also had some very choice words to say about how the guards all smell like horse dung.
        //->4g
    +Yeah, tan his hide good!
        ->4f
    +\*Say nothing*
        ->4j

=== 4f ===

changeCamTarget({brushIndex})

\*Under his breath* Wow, they're really selling it.

->4i

=== 4g ===

How dare he!

    +He also said they like to chew rocks.
        ->4h
    +<Say nothing>
        ->4i

=== 4h ===

\*His eye twitches*

changeCamTarget({brushIndex})

\*Whispers* Hey, quit it! This guy's gonna mess me up good!

    +And he went on and on about how inbred they are.
        ->4k
        
    +\*Say nothing*
        ->4i

//More Impressed Guard Outcome
=== 4i ===

changeCamTarget({laszloActualIndex})

Hope you don't like the sun, Brush, cause it doesn't shine where you're going. Boys! Get in here. We've got {takeGézaAway:more traitors|another traitor} for the pit!

fadeToBlack()

deactivate({brushIndex})

{takeGézaAway:deactivate({gézaIndex})}

~impressedGuard = true

activateQuestStep(The Plan, Making an impression.)

movePlayer(4,1)

setToTrue(impressedGuardLaszlo)

->5a

//Less Impressed Guard Outcome (switched from paper)
=== 4j ===

changeCamTarget({laszloActualIndex})

Boys! Come on in, we got ourselves another one for the pit!

fadeToBlack()

movePlayer(4,1)
deactivate({brushIndex})

-> 5a

//Guard Kills Brush Outcome
=== 4k ===

activateQuestStep(The Plan, The death of Brush.)

setToTrue(gotBrushKilledByGuard)
~gotBrushKilledByGuard = true

changeCamTarget({laszloActualIndex})

Oh that's it! You quipped your last, slave!

changeCamTarget({brushIndex})

No please!

fadeToBlack()

deactivate({brushIndex})

~impressedGuard = true

activateQuestStep(The Plan,Making an impression.)

setToTrue(impressedGuardLaszlo)

->5a


//Guard interactions after completing Brush's plan.!
//Brush's alive, Guard is impressed
=== 5a ===

setNPCFacing({laszloActualIndex}, NE)

fadeBackIn(60)

changeCamTarget({laszloActualIndex})

You did good to tell me what he was saying. This lockdown has everyone's blood up, and when it ends I don't want him spreading that bile to the rest of the workers.

    +Thank you, sir.
    ->5b
    +Just doing my duty, sir.
    ->5b

=== 5b ===

changeCamTarget({laszloActualIndex})

I can't be everywhere at once, and what happened with the mine has us short staffed. How would you like to step up and help out?

    +Of course, sir!
    {impressedGuard: ->5d | -> 5e}
    +I don't know, what's in it for me?
    ->5c

=== 5c ===

\*The guard chuckles.* You slaves are all the same. Always looking out for yourselves.

{impressedGuard: ->5d | -> 5e}

=== 5d ===

prepForItem()

Well you earned it, so here you go. Here's Brush's {takeGézaAway + killedBAndG: and Géza's }rations, and your own. It's not like {takeGézaAway + killedBAndG:they need |he needs }it now. But seriously, back to business.
    
{takeGézaAway + killedBAndG:giveItem(0,0,3)|giveItem(0,0,2)}

->5f

=== 5e ===

activateQuestStep(The Plan,Deemed trustworthy.)

prepForItem()

Here's your rations. I like the cut of you, but you'll need to work hard to get anywhere in this camp. But I know a way you can get started.

giveItem(0,0,1)

-> 5f

=== 5f ===

setToTrue(givenTaskByLaszlo)
activateQuestStep(My New Hutmate,Deliver Rations to Weft.)

prepItem()
I need you to take this bit of rations to Weft. He's another slave like you: he knows what's good for him. {killedBAndG:You'll be living in his hut from now on; can't have you staying in this abattoir.|He's also your new hutmate, so stay on his good side.} Just follow the road south from this hut then turn west past the stables. Weft lives in the big shack in front of the Manse. 

giveItem(3,8,1)

prepItem()

And take this badge, as well. If anyone stops you and orders you around, show them the badge. It'll prove that you're on a job for me. Now get to it!

giveItem(3,7,1)

fadeToBlack()

deactivate({gézaIndex})
deactivate({gézaPlayingCrazyIndex})
deactivate({laszloActualIndex})
activate({gézaSecondLocationIndex})

fadeBackIn(60)
->10aa

=== 6a ===
fadeToBlack()

deactivate({laszloActualIndex})

changeCamTarget({gézaIndex})

fadeBackIn(60)

Mother preserve us! You killed him!

changeCamTarget({brushIndex})

You fool! Now they'll never trust you!

    +You lot may be slaves, but I'll never let someone own me.
        ->6b
    +Don't worry about that now, you'll be joining him soon. <Combat>
        fadeToBlack()
    
        deactivate({brushIndex})
        deactivate({gézaIndex})
        setToTrue(killedBrushAndGéza)
        
        fadeBackIn(60)
        ->Close


=== 6b ===

changeCamTarget({brushIndex})

Oh, is that so. And whats your plan for the dozens of other guards in the camp. You can't fight them all.

    +I'm new, and they haven't memorized my face yet. I'll take the guard's armor as a disguise.
        ->6c
    +That's none of your concern.
        ->6d

=== 6c ===

changeCamTarget({gézaIndex})

Well, don't think it's gonna be as easy as walking out the front gate. It will look plenty suspicious if you left during a lockdown.
    
->6d

=== 6d ===

changeCamTarget({brushIndex})

I know the people here might not matter to you, but without someone to walk between the huts the escape plan we got cooking won't work. 

If you can find it in your heart to help us, please, go find Kastor. He will be in one of the larger huts, near the wall to the south. He's the one who told me about the escape plan; he should know more about how you can help. In exchange, we'll hide the guard's body for as long as we can. 

    +No promises.
        ->6e
    +I'll consider it.
        ->6f
    +You lot are pathetic.
        ->Close



=== 6e ===

Just think it over.

-> Close
    
=== 6f ===

That's all I can ask, I suppose.
    
->Close

//Killed Brush and Géza
=== 7a ===

\*You hear hurried footsteps outside the hut*

fadeToBlack()

activate({laszloActualIndex})

changeCamTarget({laszloActualIndex})

fadeBackIn(60)

What was that noise? These bodies... You killed them!

{
-charisma >= 2: 
    +They spoke of escape, and wanted me to join them! When I refused they jumped me! <Cha {charisma}/2>
        ->7b
}

    +This isn't what it looks like!
        ->7c
    +Can't have you raising the alarm <Combat>
        fadeToBlack()
        
        deactivate({laszloActualIndex})
        setToTrue(killedGuardLaszlo)
        
        fadeBackIn(60)
        ->Close

=== 7b ===

Damned lockdown's got everyone on edge. Hell of a first day for you, eh?

    +Yeah, it's been a ride.
        -> 7e
    + I've had worse.
        -> 7d

=== 7c ===

A murderer *and* a liar? Die scum! [Combat]

fadeToBlack()

deactivate({laszloActualIndex})

fadeBackIn(60)

->Close

=== 7d ===
    
I can't imagine what's worse than this. You're covered in blood!

~needsABut = true
->7e

=== 7e ===

{needsABut: But, f|F}or killing those traitors and saving me the trouble, I suppose I should take pity on you. How would you like to run an errand for me? It'll get you out of this shack for a time.

    +Of course, sir!
        ->5f
    +Anything to stretch these legs.
        ->5f
    +I'm not your dog <Combat>
        fadeToBlack()

        deactivate({laszloActualIndex})
        setToTrue(killedGuardLaszlo)
        
        fadeBackIn(60)
        ->Close

//Peacefully refuse to help Brush
=== 8a ===

~peacefullyRefusedBrush = true

changeCamTarget({brushIndex})

Damn it, you stubborn ass. You're ruining-

->4a

=== 8b ===

changeCamTarget({brushIndex})

Sir, thank the Gods you're here. This slave is talking of escape!

    +He's lying. These two asked *me* to help *them* escape!
    ~tellTheGuardTheTruth = true
        ->8c
    +I have no idea what he's talking about.
        ->8c
    +\*Say a prayer and attack the fully armed guard with nothing but your fists*
        
        fadeToBlack()

        deactivate({brushIndex})
        deactivate({gézaIndex})
        deactivate({laszloActualIndex})
        setToTrue(killedBrushAndGéza)
        setToTrue(killedGuardLaszlo)
        
        fadeBackIn(60)
        ->Close //nobody left alive to talk to

=== 8c ===

changeCamTarget({laszloActualIndex})

{tellTheGuardTheTruth:Hmmm, I never did like you Brush, but these new slaves are always the ones to cause trouble. I guess the only way to be sure I get the troublemaker is to send you both to the pit. |Oh is that so? Brush may be a pain in my ass, but I know he doesn't have the spine to try to escape. New slaves, however... Sometimes they require breaking in.}

{tellTheGuardTheTruth: ->8d | ->8e}

=== 8d ===

{
-charisma >= 2:

+Wait! Please!
    setToTrue(sentToThePit)
    ->Close //Taken to the pit, in another scene
    
+Brush mentioned others: I can show you! <Cha {charisma}/2>
    ->8f

-else:

+Wait! Please!
    setToTrue(sentToThePit)
    ->Close //Taken to the pit, in another scene
    
}
=== 8e ===

{
-charisma >= 2:

+Wait! Please!
    setToTrue(sentToThePit)
    ->Close //Taken to the pit, in another scene
    
+Brush mentioned others: I can show you! <Cha {charisma}/2>
    ->8f

-else:

+Wait! Please!
    setToTrue(sentToThePit)
    ->Close //Taken to the pit, in another scene
    
}
=== 8f ===

changeCamTarget({laszloActualIndex})

Others you say?

    +There are other huts working separately on different parts of the plan. That way if you captured one of {tellTheGuardTheTruth:them|us} the rest could continue without your knowledge.
        ->8g
        
=== 8g ===

Interesting... which other huts?

    +Er... I'm not sure. I only know about the plan because Brush told me. You'd need to get it out of him.
        ->8h
    
        
=== 8h ===

setToTrue(rattedOutBrush)

changeCamTarget({brushIndex})

Damn your eyes, you've doomed us!

    +You have no one to blame for that but yourself.  
        ->8i
    +"Us" doesn't mean me.
        ->8i
        
=== 8i ===

changeCamTarget({laszloActualIndex})

This was a very clever plan for a bunch of slaves. Makes me wonder if you even came up with it yourselves. 

~takeGézaAway = true

    ->4i

/////////////////////////////////////////////////////////////////////////////////////
// Speaking to Géza \/\/\/\/\/\/
/////////////////////////////////////////////////////////////////////////////////////
=== 10aa ===

changeCamTarget({gézaSecondLocationIndex}) //change cam to Géza at door

{gotBrushKilledByGuard: Brush... no matter how many times I see an execution, I never get used to it. Couldn't just shut up, could ya. You had to keep talking, and now he's dead.| ->10a}


{
-charisma >= 2:
    *You can't blame me for the guard's thin skin
        ->10ab
    *I'm sorry, I thought making that stuff up would help the guards warm up to me.
        ->10ac
    *Brush knew the risks of his plan. The best way to honor him is to finish what he started. <Cha {charisma}/2>
        ->10ad
    
-else:
    *You can't blame me for the guard's thin skin
        ->10ab
    *I'm sorry, I thought making that stuff up would help the guards warm up to me.
        ->10ac
}
    
=== 10ab ===

I can and I will. Your actions have lost me the only friend I have left in this world, and the one thing keeping me from striking you is that I know I need you for the rest of the plan.

    *That's right, you do need me. So how about you show some respect before I change my mind.
        ->10ae
    *I'll take more care with my actions in the future.
        ->10ac
    *After we get out of here, we never have to speak to each other again. Until then, lets just resolve to get along.
        ->10af

=== 10ac ===

keepDialogue()

Lets just get back to business. I can mourn when you're gone.

    ->10a
    
=== 10ad ===

keepDialogue()

I suppose you're right. Lets get everyone out of here, and we can mourn those we lost when it's over.
    
->10a

=== 10ae ===

keepDialogue()

You... *Géza grits his teeth.* I'm sorry. Lets work together for the good of the plan.
    
->10a

=== 10af ===

keepDialogue()

I look forward to seeing the back of you. But you're right, we have more important matters.
    
->10a

=== 10a === //Talking to Géza

That went better than I expected. But I suppose we shouldn't celebrate Brush being sent to the pit. 

    {
    -not gotBrushKilledByGuard:
    *So what now? 
        ->20a
    *Is Brush going to be ok?
        ->10g
    -else:
    *So what now?
        ->20a
    }


=== 10g ===

It's hard to say. I hope they let him sweat in there a little before they start in on extracting information. It would give us more time to maneuver. But they could also start the torture right away, and it would be a race against time before he breaks. I don't care who you are. When someone wants what you know badly enough, they'll get it out of you eventually. 

    *You sound like you're familiar with the process.
->10h

=== 10h ===

Most slaves are.

    
{
-wisdom >= 2:   
    *But I don't see many scars on you. <Wis {wisdom}/2>
        ->10i
}

    *Ain't that the truth.
        ->10j



=== 10i ===

keepDialogue()

I've done what I had to do in my life. Made some unseemly choices, which led me here. Let's just leave it at that. //Journal Entry (possible to recruit Géza?)
        
->10a

=== 10j ===

But enough about Brush. I'd prefer not to waste time thinking about his fate that I could be using to rescue him.

    *So what now?
        ->20a

//So what now?
=== 20a ===

activateQuestStep(The Plan,Make contact with Kastor.)

Now, you run as quick as you can to Kastor. He's the one that told Brush and I about the escape plan; he should know others who are in on it, too. He's in a large hut in the southeastern part of the camp, along the southern wall. Just follow the road past the gate and you can't miss it.

setToTrue(knowRevolutionPassword)
    
To identify yourself as an ally, ask him which way the wind is blowing.

    +And which way <i>is</i> it blowing?
        ->20b


=== 20b ===

East. The way the road leads out of this hellhole.

    +Understood. Anything else?
        ->20c

=== 20c ===

setToTrue(spokeToGézaAboutPlan)

If for any reason you find yourself hurt or in need of respite, come back to me. I'll hide you for a time so you can rest. And please be careful. For all of our sakes.

fadeToBlack()

deactivate({gézaSecondLocationIndex})
activate({gézaIndex})
setNPCFacing({gézaIndex},NW)

fadeBackIn(60)
    ->Close

=== Close ===

setPromptMessage(WASD: Move)

close()

->DONE