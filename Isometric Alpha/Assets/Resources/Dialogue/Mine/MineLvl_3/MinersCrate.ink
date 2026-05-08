VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR knowRevolutionPassword = false
VAR knowsAboutTheMine = false
VAR toldToFindNandor = false
VAR toldCarterPassword = false
VAR toldCarterWrongPassword = false

VAR mineLvl3ClearedCratesToMiners = false
VAR mineLvl3MetGaspar = false
VAR mineLvl3KilledGuards = false
VAR mineLvl3MarcosAgreedToIgniteJelly = false
VAR mineLvl3ToldToFindMarcos = false

VAR sentIntoMineByDirector = false
VAR trainedByEmeseToUseBlasingJelly = false
VAR mineLvl3MetMinersButDidNotAcceptHelp = false

VAR takingCarterNandorWithYou = false
VAR saidYouMustBeJoking = false

VAR mentionedKastorMinersCrates = false
VAR mentionedDirectorMinersCrates = false
VAR askedWhoMarcosIsMinersCrates = false

VAR nandorSomethingToDiscuss = false

VAR hostagesDead = false

VAR formationScreenTutorialKey = "Formation Tutorial"

//camera Target index's
VAR barricadeIndex = 1
VAR carterIndex = 2
VAR nandorIndex = 3
VAR marcosIndex = 4

VAR marcosDialogueIndex = 0

VAR backFromMarcosDialogue = false

VAR playerName = ""

{
-mineLvl3MetMinersButDidNotAcceptHelp:
    ->5a
-backFromMarcosDialogue:
    ->2c
-else:
    ->1a
}

=== 1a ===

changeCamTarget({carterIndex})

You there! Step into the light, with your hands where I can see them. *A branded slave, wielding a large mining pick in both hands, glares at you from behind a makeshift barricade.*
    
    +\*Comply.* I am {playerName}.
        ->1b
    +\*Leave.*
        ->Close

=== 1b ===

I have no idea who you are. You have the brand but you're not one of the slaves that normally works on this level. Did Gáspár send you?
/*
{
-mineLvl3MetGaspar:
    +I have never met this Gáspár before. I'm a new arrival. <Lie>
        ->5a //5a is a place holder
-else:
    +I have never met this Gáspár before in my life. I'm a new arrival.
        ->5a //5a is a place holder
}*/

    {
    -sentIntoMineByDirector:
    +The Director sent me into the mines to close the tunnel that the worms are coming out of. *Show Director's Seal.*
        setToTrue(mentionedDirectorMinersCrates)
        ->1bb
    }

    {
    -toldToFindNandor:
    +No, Kastor did. I'm looking for one of the branded named Nándor. Do you know where he is?
    setToTrue(mentionedKastorMinersCrates)
        ->1ba

    -knowRevolutionPassword:
    +I'm down here looking for comrades. Do you know which way the wind is blowing?
        ->4a
    }

    +I'm a new slave. I came down here looking for any survivors.
        ->3a

=== 1ba ===

Kastor? From the surface? Perhaps you know which way the wind is blowing, then.

    +East, of course.
    
        setToTrue(mentionedKastorMinersCrates)
        ~toldCarterPassword = true
        activateQuestStep(Finding Nándor, Nándor found.)
        \*Carter breaks out into laughter.* Forgive me, you're the first friendly face we've seen in days; I am simply overcome with relief. Nándor still breaths. Let me show you to him.

        ->clearCrates(->1c)

=== 1bb === 

\*Carter looks at you perplexed.* The Director is sending branded into the mines? The guards must be stretched thinner than we realized.

You are a capable fighter if you've made it this far. Let me show you to the others. Nándor will know what to make of you.

    {
    -toldToFindNandor:
    +Nándor you say? Kastor said I was to look for him while I was descending through the mine. 
        keepDialogue()
        You are a friend of Kastor's? Perhaps you know which way the wind is blowing, then.
        ->1ba
    }

    +Alright, I'll meet with this 'Nándor'.
        ->clearCrates(->1c)
    +I don't have time for this. Just point me in the direction of the worm's tunnel and I'll be on my way.
        Very well. The worms came from the southern most shaft on this floor. Just take every opportunity to turn south and you can't miss it.
        ->Close

=== clearCrates(->divert) ===

fadeToBlack()

setToTrue(mineLvl3ClearedCratesToMiners)

changeCamTarget({nandorIndex})
deactivate({barricadeIndex})
activate({carterIndex})
moveToPos(2,1)
setFacing(NE)
fadeBackIn(60)

->divert

=== 1c ===
    
{
    -toldCarterPassword:
        Carter tells me you are an ally. You have the gratitude of everyone here, but we're not safe yet.
    -else:
        Carter tells me you came from the surface. It's remarkable that you made it this far.
}

{
-mentionedKastorMinersCrates or toldCarterPassword:
    +I have opened the gate to the second level. We can leave as soon as you are ready.
        ->1d
-else:
    +What did you wish to discuss with me?
        {
        -mentionedDirectorMinersCrates:
        If your goal is to seal the pocket that the worms are coming from, then we would like to come with you. We know where it is and have already fought them many times. We would be a great asset.
        ->1da
        -else:        
        My group was looking for some opportunity to make a push towards the tunnel the worms are coming from, and seal it. With you, we may just have a chance.
        ->1da
        }
}

=== 1d ===

You have done well, but as much as I would love to exit the mines there is something we must do first.  
    
    {
    -sentIntoMineByDirector:
    +This other thing wouldn't happen to be collapsing the tunnel the worms are coming out of, would it?
        ->1daa
    }
    +What do you mean?
        ->1e
    +More? You must be joking.
        ~saidYouMustBeJoking = true
        ->1e

=== 1daa ===

Yes, actually. How did you know that?
    
    +I didn't, but the Director has assigned me to do exactly that as well. *Show Director's Seal.*
        ->1fb
    +I just assumed you'd want to be rid of these worms as much as anyone.
        You assumed correctly. If they are not stopped, then we will all be in grave danger.
        ->1f

=== 1da ===

    +The way back to the surface is no longer blocked. You do not need to do this to return to camp.
        We could, but the worms are a threat to everyone. The loss of life would be far less if we finish this now, rather than waiting for the worms to escape the mines.
        ->1db

=== 1db ===


    +Not many people would want to stay in this mine after having been trapped in it for so long. What are you really after?
        As a branded, I know how easy it is to be suspicious of a helping hand. Do not let our condition as slaves get in the way of honest collaboration.
        ->1db

    +How did you come to be trapped down here?
        Carter and I were miners that worked this level. We were excavating a new tunnel when one of our team struck his pick through the wall in front of him, revealing a new cavern.

        The cavern was filled with these worm creatures. In moments, they had escaped the cavern and were ripping into the other slaves of our team. Some of the guards rushed to combat them, but it was quickly deemed a hopeless fight.

        Overseer Gáspár, the guard who was in charge of work on this floor, led the guards in a retreat back to the surface, but when we reached the exit to this floor we found it already blocked. He then took the guards from our group and declared he would fight back to the stockroom which contained the food and supplies the guards keep for this level.
        
        Gáspár forbade the branded from following him, because he didn't want to share the stockrooms food with us for however long we'd be trapped for. Our group then split off from theirs and fought our way here. We three are the only ones that made it this far.
        ->1db
    +Where is the tunnel the worms are coming from? 
        The worms first entered the mine from the southern most shaft. At every opportunity turn south and you will find it eventually.
        ->1db
    +This guard you are with. Who is he?
        setToTrue(askedWhoMarcosIsMinersCrates)
        That is Guard Márcos. He was the only guard to side with the branded when Overseer Gáspár barred us from seeking protection in his stockroom.
        ->1h(->1dba)
    +You are brave men. I would welcome any help you could offer me.
        Excellent. We will be much more effective if we work together.
        ->1f
    +I'm sorry, but I've only just met you. Let me scout ahead before commiting to fighting with you.
        ->1dc

=== 1dba ===

Carter and I would have surely perished if he had not made that choice.

->1db

=== 1dc ===

I understand. We shall stay here and keep this cavern clear while you go forward on your own. If you get hurt, return to us and we will protect you while you rest.

->unclearCrates(->Close)

=== unclearCrates(->divert) ===

fadeToBlack()

setToFalse(mineLvl3ClearedCratesToMiners)

setToTrue(mineLvl3MetMinersButDidNotAcceptHelp)

changeCamTarget({nandorIndex})
activate({barricadeIndex})
deactivate({carterIndex})
moveToPos(3,4)
setFacing(NW)
fadeBackIn(60)

    ->divert

=== 1e ===

{
-saidYouMustBeJoking:
I wish I was. But I'm deathly serious: these worm-things are growing in number. They are coming through a breach in the latest shaft we were digging.
-else:
To keep it brief, these worm-things are growing in number. They are coming through a breach in the latest shaft we were digging.
}

    ->1ea

=== 1ea ===

{
-knowsAboutTheMine && toldToFindNandor:
    +From a pocket. Kastor told me.
        He was correct. If we don't seal that pocket, we will be neck deep in these things before long. It will be difficult to fight the guards and the worms simultaneously.
        ->1f
-else:
    +What does this have to do with us?

        If we don't seal the breach, we will be neck deep in these things before long. When the worms first appeared, the guards evacuated every slave they could from the upper levels, but the guards and slaves that were left behind on the bottom floor were trapped.
        
        Quickly realizing that we may be here for weeks, the guards fortified the stockroom on this level and decided to rid themselves of any extra mouths. The slaves trapped with them were forced to find their own shelter. Of the more than a dozen slaves that tried to make the journey, we are all that remains.
    {
    -toldCarterPassword:
        Should the worms make it to the surface and attack the camp, they will be a formidable foe indeed. It will severely complicate things for our cause. It is best we remove that complication before revolting against the guards.
            ->1f
            
    -else:
        Should the worms make it to the surface and attack the camp, I expect the guards there will have a similar attitude towards the slaves under their command. We will be used as fodder to keep the worms at bay until help can arrive.
            ->1f
    }
}


=== 1f ===


{
-not mentionedDirectorMinersCrates and sentIntoMineByDirector:
    +The Director also wants us to end the worm threat. He sent me down here to do exactly that. *Show Director's Seal.*
        setToTrue(mentionedDirectorMinersCrates)
        ->1fb
-not mentionedDirectorMinersCrates:
    +Why don't we simply fight our way back up to the second floor gate and seal it again?
        ->1fa
}

{
-toldCarterPassword:
    +Maybe the worms could be used to fight the guards.
        ->1j
}

{
-not askedWhoMarcosIsMinersCrates:
    +Who is this guard with you?
        ->1g
}
{
-sentIntoMineByDirector:
    +I must make for the stockroom. I can use the blasting jelly stored there to collapse the tunnel.
        The stockroom is southeast of here. It's the last tunnel you can take before you reach the bridge over the underground stream.
        ->1lb
-else:
    +Do we even have a way of sealing the tunnel?
        ->1k
}

=== 1fa ===

Having fought the worms many times while we've been stuck down here, I'm all too familiar with their ability to spit acid, and what it can do to metal tools. I don't think the relatively thin metal of the gate will hold up long if they decide to chew through it.

The acid that hits the ground doesn't eat through the rock so easily, however. If we can find a way to seal the breach that they're coming through, we should be able to prevent the worms from flowing out of it. At least for longer than the gate can.
        ->1f

=== 1fb ===
    setToTrue(mentionedDirectorMinersCrates)

    {
    -toldCarterPassword:
        You've met with the Director? Your time in this camp has been short but eventful it seems.
    -else:
        \*Nándor frowns.* The Director has given you his seal? He must have a rather high opinion of you.
    }

    {
    -hostagesDead:
        +Not exactly. The guards used me to negotiate with some branded but the talks soured. The hostages died, and the Director sent me down here to be rid of me, more than likely.
            I see. There seems to be little love lost between you then.
            ->1f
    -else:
        +I've performed a few tasks for him. The more I help the guards, the more dangerous the situation they throw me in to, it seems.
            I see. There seems to be little love lost between you then.
            ->1f
    }

    {
    -knowRevolutionPassword and (mentionedKastorMinersCrates or toldCarterPassword):
        +All a ruse, of course. The guards grow to depend on me so that I can more easily come and go about camp.
            Ah, well done. You seem to have proven quite the asset to Kastor.
            ->1f
    -else:
        +That is no concern of yours.
            Yes, of course. No concern of mine.
            ->1f
    }

=== 1g ===

That is Guard Márcos. He helped save Carter and myself and escorted us here when the other guards wouldn't let us inside their barricade. Without him, I would be dead.

    {
    -toldToFindNandor:
        +I see. Guard Márcos, Kastor also spoke of your heroics.
            ->1h(->1i)
    }

    {
    -toldCarterPassword:
        +Do you trust him enough to discuss our plans around him?
        ->1i
    }

    +I'm glad he proved useful, then.
        That and more so.
            ->1f
    
    +Strange that a guard would go to such lengths for some slaves. Especially for the branded.
        ->1ha

=== 1h(->divert) ===

changeCamTarget({marcosIndex})
\*Guard Márcos's torso and legs are covered in bite marks, and dried lines of blood trail from many punctures across his body.* I merely did what anyone with the power should.
changeCamTarget({nandorIndex})
    ->divert

=== 1ha ===

changeCamTarget({marcosIndex})

\*Guard Márcos's torso and legs are covered in bite marks, and dried lines of blood trail from many punctures across his body.* No one deserves to be eaten by those things. I can't think of any crime that would warrant that.


{
    -toldToFindNandor:
        ->1i
    -else:
    changeCamTarget({nandorIndex})
    
    If his presence bothers you, do not let it. I have complete trust in his intentions.
        ->1f
}

=== 1i ===

changeCamTarget({nandorIndex})

He has bled for us many times, and we've spoken at length while we've been trapped down here. He will fight with us again when the time comes.
    ->1f

=== 1j ===

changeCamTarget({marcosIndex})

I would not count on it. If pressed, the guards will plan to retreat towards the Manse, the guard barracks, and the camp entrance. It's the easiest part of the camp to fortify, and they will want to both prevent you from escaping and allow help to arrive unimpeded.

When they do this, you will be caught between the worms escaping through the mine entrance, and the guards' barricades. It will be a slaughter.

changeCamTarget({nandorIndex})

If we plan on getting out of this alive, the breach must be sealed.
    ->1f

=== 1k ===

{
-mentionedDirectorMinersCrates:
changeCamTarget({marcosIndex})
We do, but it will be tricky. Some of the guards are trained in the use of blasting jelly; we use it to clear rubble sometimes. I was not the only guard to be trapped on this level when the evacuation was sounded. Those guards that remain on this level are barricaded within the chamber we use to store the jelly and other provisions, to the southeast of here. We will need to convince them to help us.

-else:
changeCamTarget({marcosIndex})
We do, but it will be tricky. Some of the guards are trained in the use of blasting jelly; we use it to clear rubble sometimes. I was not the only guard to be trapped on this level when the evacuation was sounded. Those guards that remain on this level are barricaded within the chamber we use to store the jelly and other provisions, to the southeast of here. We will need to convince them to help us, or failing that, kill them and take the jelly for ourselves.

}

{
-mineLvl3KilledGuards:
+I have actually already taken care of the guards. 
    ->2a
-mentionedDirectorMinersCrates:
+If they have any loyalty to the Director, they will help us once I show them his seal.
    changeCamTarget({carterIndex})
    Then it is fortunate for us that you have it.
    ->1lb
-else:
+If we kill them, how will we use the jelly?
    ->1l
}

=== 1l ===

changeCamTarget({marcosIndex})

I can use the jelly if we truly need, but with my injuries I may make a mistake. It would be best if we had another guard detonate the breach instead.

    +And killing your fellow guards doesn't bother you?
        ->1la
    
=== 1la === 
    I have had much time to think, down here in the dark. Coming so close to death has made me wonder what kind of a man I will be when I go to my hearth. 
    
    I would prefer not to kill the men and women I have worked with for months. And leaving them alive is pragmatic: I cannot be sure I can handle the blasting jelly as effectively as they can.
    
    But if they should stand between us and the safety of the camp simply because the plan came from the mouth of a slave? Then the needs of the many demand we act. I will mourn their deaths in my own time. Does that satisfy your question?
    
    +For now. But I will be watching.
        I understand.
        ->1lb
    +No. I don't buy it. Get in the way of the plan and I will remove you from our path. Believe me.
        I understand. I won't give you cause.
        ->1lb

=== 1lb ===


changeCamTarget({nandorIndex})
setToTrue(mineLvl3ToldAboutJelly)
Are you ready to set out?

    +Yes. Let's go.
    ~takingCarterNandorWithYou = true
        ->1m
    +No, let me scout ahead first.
        Alright, but hurry back. We need to move before the number of worms grows out of control.
        ->1m

=== 1m ===
/*
{
-takingCarterNandorWithYou:
    fadeToBlack(true, false)

    deactivate({carterIndex})
    addToParty({carterIndex})
    deactivate({nandorIndex})
    addToParty({nandorIndex})

    fadeBackIn(60)
-else:
    addToPartybutNotFormation({nandorIndex})
    addToPartybutNotFormation({carterIndex})
}*/
    addToPartybutNotFormation({nandorIndex})
    addToPartybutNotFormation({carterIndex})
    startTutorial({formationScreenTutorialKey})
    setFacing(SE)
    setToTrue(mineLvl3CarterAndNandorInParty)

{
-not mineLvl3MarcosAgreedToIgniteJelly:

    {
    -not sentIntoMineByDirector:
    activateQuestStep(Sealing the Breach,Convince the Guards.)
    -else:
    activateQuestStep(Sealing the Breach,Find blasting jelly.)
    }

    {
    -mineLvl3ToldToFindMarcos:
        activateQuestStep(Find Guard Márcos,Márcos has been found.)
    }
    
    
    changeCamTarget({marcosIndex})
    While you are out there, remember: you can come back to me if you are hurt. I will stand watch while you rest.
        healParty()
        ->Close
-not trainedByEmeseToUseBlasingJelly:

    activateQuestStep(Sealing the Breach,Márcos Will Help.)
        ->Close
}


    ->Close

=== 2a === 

changeCamTarget({marcosIndex})

That is... unfortunate. There is no love lost between myself and Gáspár, but without him or Guard Virág there is no one else trained in the blasting jelly's use on this level. I will have to detonate the jelly in their stead.

changeCamTarget({carterIndex})

Márcos, you can't help but shiver from your wounds! There is no way you will be able to close the pocket without detonating the jelly prematurely.

changeCamTarget({marcosIndex})

I am not as wounded as I appear. I can perform the detonation as I have a dozen times before.


    +Your bravado is fooling no one. I am a quick learner, teach me how to perform the detonation and I will carry it out. <Wis {wisdom}/2>
        ->2b

    +There is little time to argue. Márcos will do what needs to be done. Let us move before the worm situation gets worse.
        setToTrue(mineLvl3MarcosAgreedToIgniteJelly)
        ~mineLvl3MarcosAgreedToIgniteJelly = true
        ~takingCarterNandorWithYou = true
        ->1m
    
=== 2b ===

changeCamTarget({marcosIndex})

swapInkFiles({marcosDialogueIndex},startingFromMinersDialogue)

->Close

=== 2c ===

fadeToBlack()

deactivate({carterIndex})
deactivate({nandorIndex})
deactivate({marcosIndex})
addToParty({carterIndex})
addToParty({nandorIndex})
//startUITutorial({formationScreenTutorialKey})
setToTrue(mineLvl3CarterAndNandorInParty)

fadeBackIn(60)
->Close

=== 3a ===

You're looking for survivors? You'll forgive me, but I find it a little odd you would brave three levels of the mine by yourself just to help us. Unless... do you know which way the wind is blowing?


{
    -knowRevolutionPassword:
    +How should I know which way the wind is blowing? I've been in this cave for hours.
        ->3b
    +North?
        ->3ba
    +East, friend.
        ~toldCarterPassword = true
        setToTrue(toldCarterPassword)
        keepDialogue()
        Incredible, you're the first friendly face we've seen in days, *and* you're one of us. You fought your way here from the surface?
        ->4a
    +South?
        ->3ba
    +West?
        ->3ba

    -else:
    +How should I know which way the wind is blowing? I've been in this cave for hours.
        ->3b
    +North?
        ->3ba
    +East?
        ->3d
    +South?
        ->3ba
    +West?
        ->3ba
}   


=== 3b ===

setToTrue(toldCarterWrongPassword)

Right, of course. It was a stupid question. 

    ->3c

=== 3ba ===

setToTrue(toldCarterWrongPassword)

Blast, nevermind it then. 

->3c

=== 3c ===

In any case, you being here means that the way back up is no longer blocked. But that also means the worms can escape to the upper levels and endanger the rest of the camp. I'm not sure I can trust you yet, but we're not in a position to turn away help down here: Nándor will have something to discuss with you. Are you willing to stay with us for a time?

    +Yes, I'll hear what you have to say. Which one of you is Nándor?

    \*The man lowers his weapon and extends a hand over the barricade.* I'll show you to him. I'm Carter, by the way.
        ->clearCrates(->1c)
        
    +I'm sorry but I must be going. I'll return when I am able.
        ->Close

=== 3d ===

Hmm, you don't sound very confident about that. Either way, I'll bring you to Nándor if you'd like. He'll know what to make of you.

    +Fine, I will speak to Nándor.

    \*The man lowers his weapon and extends a hand over the barricade.* Come with me. I'm Carter, by the way.
        ->clearCrates(->1c)
        
    +I'm sorry but I must be going. I'll return when I am able.
        ->Close

=== 4a ===

East! Incredible, you're the first friendly face we've seen in days, *and* you're one of us. You fought your way here from the surface?

{
-toldToFindNandor:
    +Yes, Kastor sent me. He told me to look for a slave named Nándor. Are you him?
        
        No, but he still lives! Quickly, come over the barrier and I'll introduce you.
        activateQuestStep(Finding Nándor, Nándor found.)
        setToTrue(toldCarterPassword)
        ~toldCarterPassword = true
        ->clearCrates(->1c)
-else:
    +I'm here looking for survivors to help us in our cause. Are there more with you?
        
        Yes, a few of us are still alive. Quickly, come over the barrier and I'll introduce you.
        setToTrue(toldCarterPassword)
        ~toldCarterPassword = true
        ->clearCrates(->1c)
}

=== 5a ===

You have returned. How did you fair against the worms?

    +I've decided I would like your help after all.
        ->clearCrates(->5b)
    +I'm in need of rest.
        restParty()
        ->Close
    +I just got turned around. I'll be going.
        ->Close

=== 5b ===

I'm glad you've decided to accept our help. This will make things much easier.

->1f

=== Close ===

close()

->DONE