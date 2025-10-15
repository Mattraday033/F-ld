VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR initialRubbleIndex = 1
VAR blastRubbleIndex = 2
VAR pazmanIndex = 3
VAR rekaIndex = 4
VAR viragIndex = 5
VAR gasparIndex = 6
VAR carterIndex = 7
VAR nandorIndex = 8
VAR marcosIndex = 9
VAR rubbleMarcosIndex = 10

VAR fullGuardFightIndex = 0
VAR halfGuardFightIndex = 1
VAR dialogueKeyForAfterKillingGuards = "nandorAfterKillingGuardsMineLvl3"

VAR deathFlagGuardMárcos = false
VAR deathFlagGuardPázmán = false

VAR toldToFindNandor = false

VAR goesWithBroglinsPlan = false
VAR mineLvl3GuardsInParty = false
VAR mineLvl3MarcosAgreedToIgniteJelly = false
VAR mineLvl3MarcosTaughtHowToIgniteJelly = false
VAR mineLvl3MarcosDiedSealingBreach = false
VAR mineLvl3CarterAndNandorInParty = false
VAR mineLvl3RefusedToFightGaspar = false
VAR mineLvl3DealtWithGaspar = false
VAR mineLvl3GuardsBackToSurface = false
VAR mineLvl3SlavesBackToSurface = false
VAR mineLvl3ToldPazmanToEatShit = false
VAR mineLvl3ThreatenedGaspar = false

VAR mineLvl3ConvincedRekaAndPazman = false
//VAR mineLvl3ConvincedOnlyReka = false
VAR mineLvl3PromisedToProtectRekaAndPazman = false
VAR mineLvl3ThreatenedRekaAndPazmanAsPrisoners = false


VAR smallCupPlacedOnBarrel = false
VAR largeCupFilledWithWater = false
VAR largeCupPlacedOnBarrel = false
VAR agreedToFightGaspar = false

VAR playerName = ""

//changeCamTarget(int targetIndex)
//keepDialogue()
//setToTrue(string flagName)
//setToFalse(string flagName)
//activate(int index of gameobject you're activating)
//deactivate(int index of gameobject you're deactivating)
//activateQuestStep(string questTitle, int questStepIndex)
//giveItem(int listIndex, int itemIndex, int quantity)
//giveItems(int listIndex1, int itemIndex1, int quantity1 |
//          int listIndex2, int itemIndex2, int quantity2 |
//          ... etc)
//takeAllOfItem(string itemName)

->1a

=== 1a ===

\*A large cave entrance looms before you. The loud squelching of many worms can be heard coming from the darkness within.*

{
-mineLvl3MarcosTaughtHowToIgniteJelly:
    +\*Make sure the tunnel is clear, then seal the breach yourself*.
    finishQuest(Sealing the Breach, true, 6)
        ->1d
}

{
-not deathFlagGuardMárcos and mineLvl3MarcosAgreedToIgniteJelly:
    +Alright Márcos, you're up. Seal the breach.
    finishQuest(Sealing the Breach, true, 7)
        ->6a
}

{
-mineLvl3GuardsInParty:
    +Overseer, the way is clear and the breach can be sealed.
    setToTrue(mineLvl3BreachSealed)
        ->2a
}

    +\*Leave the entrance alone.*
        ->Close

=== 1d ===

\*You place the barrel of mixed blasting jelly in front of the breach, then begin to work on the timer. The timer has three parts: a large cup with a spout, a small cup, and a water skin.* 

{
-largeCupPlacedOnBarrel && largeCupFilledWithWater:
    *\*Tip the large cup so that water drips out of the spout.*
        {
        -not smallCupPlacedOnBarrel:
            ->7a
        -else:
            ->1e
        }
}
    *\*Place the small cup on top of the barrel.*
        ~smallCupPlacedOnBarrel = true
        keepDialogue()
        The small cup now rests on the barrels lid.
        ->1d
    
    *\*Pour the water into the large cup, careful to keep the water from dripping out of the spout.*
        ~largeCupFilledWithWater = true
        keepDialogue()
        The large cup is now filled with water.
        ->1d
        
    *\*Place the large cup on top of the barrel.*
        ~largeCupPlacedOnBarrel = true
        keepDialogue()
        You place the large cup on top of the barrel.
        ->1d

=== 1e ===

The water begins to drip into the small cup. In a minute, the small cup will overflow.

    +*\Seek cover farther down the tunnel.
        setToTrue(mineLvl3BreachSealed)
        ->2a

=== 2a === 

fadeToBlack(true, false)

activate({blastRubbleIndex})

moveToLocalPos(-72,-9.65)

{
-mineLvl3GuardsInParty:
    activate({gasparIndex})
    activate({rekaIndex})
    activate({viragIndex})

    {
    -not deathFlagGuardPázmán:
        activate({pazmanIndex})
    }
}

{
-mineLvl3CarterAndNandorInParty:
    activate({nandorIndex})
    activate({carterIndex})
}

{
-not mineLvl3MarcosAgreedToIgniteJelly:
    activate({marcosIndex})
}

fadeBackIn(60)


{
-mineLvl3GuardsInParty and mineLvl3CarterAndNandorInParty:
    ->3a
-mineLvl3CarterAndNandorInParty:
    ->4a
-mineLvl3GuardsInParty:
    ->5a
-else:
    ->Close
}

=== 3a ===

changeCamTarget({viragIndex})

Good riddance. May I never see one of those things again for as long as I live. 

changeCamTarget({gasparIndex})

Should we all be so lucky. When work resumes, we'll be certain to see another pocket. But at least this time we'll be ready. Fun's over slaves, hand over any tools you may be holding and place your hands on each others shoulders. We're heading back to the surface.

{
-goesWithBroglinsPlan:
    +Things are afoot on the surface. There's only one way you make it out of this mine alive: if *you* surrender your weapons to *us* and come peacefully.
        ~mineLvl3DealtWithGaspar = true
        setToTrue(mineLvl3DealtWithGaspar)
        ->3c
}


    +There isn't going to be any more work, Gáspár. You're not going to make it back to the camp.
        ~mineLvl3DealtWithGaspar = true
        setToTrue(mineLvl3DealtWithGaspar)
        ->3b
    //+\*Hand over your weapon.*
        //->3d



=== 3b ===

What's this? Insurrection? I'll die before any of you see the light of day.

->combatPrep

=== 3c ===

{
-agreedToFightGaspar:
changeCamTarget({nandorIndex})

Gáspár, we refuse! Lay down your own weapons, and we vow no harm will come to you once the slaves take up arms.
}

changeCamTarget({gasparIndex})

What's this? Insurrection? I'll die before any of you see the light of day.

    +Don't be stupid, Gáspár. If I'm more than a match for this mine then I can handle you and your guards.
        
        Formidable you may be, but our duty is to stop you from escaping. We cannot let you leave this mine without being in our custody.
        ->3ca
    +You may be ready to die, but I sense your subordinates are not so eager.
        ->3ca

=== 3ca ===

    changeCamTarget({rekaIndex})
    
    Overseer, please, maybe we should listen to them. My muscles are sore from sleeping on rocks for days. We're all tired from fighting the worms. I don't like our chances.

    changeCamTarget({gasparIndex})
    
    Guard Réka, you will correct your tone and recognize my authority. We all swore an oath to the Director to lay down our lives for this camp. If now is that time, then so be it. We should all make ready for it.

    +Listen to her, Gáspár. Is it worth your life to keep us in bondage?
        ->3cb
    +That same Director who gave the order to leave you down here? It doesn't seem like he deserves your loyalty.
        ->3cc
    +If you're so ready to die then I'm sick of trying to talk you out of it. <Combat>
        ->combatPrep
    
=== 3cb ===

    changeCamTarget({gasparIndex})
    
    I will not take lectures from a criminal on where my priorities lie. If I give my life attempting to prevent you from hurting anyone else, then I will have spent it well.

    +Fine. I can see you are a lost cause. But my offer still stands for the rest of you. Each of your lives can still be saved.
        ->3cba

=== 3cba ===

    changeCamTarget({rekaIndex})
    
    I believe they are telling the truth. If their intention was not to save lives, then why fight with us to defeat the worms? The way to the surface was cleared. If they wanted us dead, why did they not leave us down here?

    {
        -mineLvl3ThreatenedGaspar:
        changeCamTarget({gasparIndex})

        Bah! Are you forgetting how they threatened us earlier? You can't trust the benevolence of these slaves! Our best time to strike is now, when they've been weakened by the worms!
        
        changeCamTarget({viragIndex})
        
        The Overseer is right. You can never trust a slave's intentions. They will only ever have one thing on their mind: revenge.
        
        changeCamTarget({pazmanIndex})

        I'm not becoming the prisoner of someone like that. I'd rather die on my feet.
        
            ->3cbb //sends to this knot because the contents of the knot used to be here and the dialogue would skip down to "your remarks are bordering on traitorous..." instead of going to combat
    }
    
    changeCamTarget({gasparIndex})
    
    Your remarks are bordering on traitorous. Perhaps they were simply using us to seal the breach. Or were trying to get us killed by taking us out of our barricades. It does not matter their exact intentions because the end result will be the same: our deaths.
    
    changeCamTarget({marcosIndex})

    For what it's worth, they have been nothing but kind to me. I may be wounded, but I can vouch that all of my wounds were inflicted by the worms.
    
    changeCamTarget({pazmanIndex})
    
    I trust Márcos. I'll submit, I could use a rest.
    
    changeCamTarget({rekaIndex})

    I'll submit as well. Both their words and their actions tell me they will not harm us.
    
    changeCamTarget({gasparIndex})
    
    ~mineLvl3ConvincedRekaAndPazman = true
    setToTrue(mineLvl3ConvincedRekaAndPazman)
    
    Traitors! I'll see you dead for this, but don't worry. I'll be much kinder than these slaves would have been.

    +So be it. <Combat>.
        ->combatPrep

=== 3cbb === //This knot was made because if statement was skipping choices for some reason

    changeCamTarget({rekaIndex})
    
    Fine. If you're all of the same mind, then I'm with you.
    
    +So be it. <Combat>
        ->combatPrep

=== 3cc ===

    changeCamTarget({gasparIndex})
    
    The Director made a decision to protect the rest of the camp from the worms. I have no doubt it was a difficult one.
    
    changeCamTarget({pazmanIndex})
    
    Oh yeah, I'm sure that he got real teary-eyed as he signed our lives away. Come off it, Overseer. To him, we're barely better than the slaves.
    
    changeCamTarget({gasparIndex})
    
    Pázmán, your insubordinate remarks have earned you a lashing should we make it back to the surface.

    +Seems like that's just one more reason he should give in now. Your authority is waning, Gáspár.
        ->3cca

=== 3cca ===

    changeCamTarget({pazmanIndex})

    If it means I can save myself a beating, then I suppose my decision is made for me.
    {
        -mineLvl3ThreatenedGaspar:
        changeCamTarget({gasparIndex})

        Bah! Are you forgetting how they threatened us earlier? You can't trust the benevolence of these slaves! Our best time to strike is now, when they've been weakened by the worms!
        
        changeCamTarget({rekaIndex})

        I hate to admit it, but the Overseer has convinced me. I'm not becoming the prisoner of someone like that.
        
        changeCamTarget({gasparIndex})

        Take your punishment like a man and all will be forgiven, Pázmán. Become prisoner to this branded and you won't live to regret it.
        
        changeCamTarget({pazmanIndex})

        \*Pázmán grimaces and unsheaths his blade.* Yes sir.
        
            ->combatPrep
    }
        
    changeCamTarget({gasparIndex})

    Fool! You'll suffer much worse at their hands if you submit!
    
    changeCamTarget({rekaIndex})
    
    These slaves have given us no reason to suspect they would harm us. They fought with us to defeat the worms, despite the fact that the way to the surface was cleared. If they wanted us dead, why did they not leave us down here?

    changeCamTarget({gasparIndex})
    
    I won't hear any more of your drivel. Turn traitor at your own peril.

    +It looks like the time for words has passed. Each of you must choose whether you want to surrender or die.
        ->3ccb

=== 3ccb ===
    
    ~mineLvl3ConvincedRekaAndPazman = true
    setToTrue(mineLvl3ConvincedRekaAndPazman)
    
    changeCamTarget({pazmanIndex})

    I surrender.
    
    changeCamTarget({rekaIndex})
    
    I surrender as well.
    
    changeCamTarget({viragIndex})
    
    I won't chance becoming your prisoner.
    
    changeCamTarget({gasparIndex})
    
    I would rather die than betray this camp. 
    
    +So be it. <Combat>
        ->combatPrep

=== PH0 ===
changeCamTarget({marcosIndex})

Don't be fools. No one needs to die here. We can all walk out of this mine without further conflict. If you feel you cannot trust the word of one of the branded, then listen to me: do not resist and you will be treated well.

changeCamTarget({gasparIndex})

How can we trust the words of a traitor? 

changeCamTarget({marcosIndex})

Forget words then, trust your eyes. Look at my body. I am wounded, yes, but the wounds are clearly the bites of the worms, which we all are familiar with. They are not torturers. They will be similarly passive hosts if you should surrender.

changeCamTarget({gasparIndex})

I don't believe it. You just want us to drop our weapons so you can butcher us later!

    +When I arrived, I could have rescued Nándor's party and left you behind. Instead I extended a hand to solve our mutual problem together. Surely that proves our benevolence.
{
-mineLvl3ThreatenedGaspar:

    changeCamTarget({rekaIndex})
    
    {
    -agreedToFightGaspar:
        We owe your friend for opening the gate, but I don't trust you enough to become your prisoner. Not after their threats in the stockroom earlier.
    -else:
        We owe you for opening the gate, but I don't trust you enough to become your prisoner. Not after your threats in the stockroom earlier.
    }
    
    changeCamTarget({pazmanIndex})
    
    \*Pázmán looks to Guard Márcos.* I've sorry Márcos, but if you've thrown in with this lot then you're farther gone than I thought.
    
    ->combatPrep
-else:
    changeCamTarget({rekaIndex})
    
    Overseer, please, maybe we should listen? If not for them, we would still be languishing in the stockroom.
    
    changeCamTarget({gasparIndex})

    Guard Réka, you will correct your tone and recognize my authority. This slave broke protocol by opening the gate to this level, and could have unleashed the worms upon the rest of the camp. Whatever we personally gained from their behavior is irrelevant!
    
    {
    -mineLvl3ToldPazmanToEatShit:
    
        changeCamTarget({pazmanIndex})

        Besides, I doubt they did it out of any goodwill toward's their captors. They don't seem like the type to think that far ahead. Who's about to eat shit now, scum?
        
        ->combatPrep
            
    -else:
        changeCamTarget({pazmanIndex})
    
        But sir, the slave's actions may have saved our lives! How can you so easily disregard that?
        
        changeCamTarget({gasparIndex})

        Because their duty is to die in service to us! To the Confederation! Just as it is all of our duties to ensure that they do! Would you have me bow to a slave? Thank them for carrying rubble? For swinging a pick?
        
        changeCamTarget({rekaIndex})
    
        My life doesn't mean so little to me that I wouldn't respect the one that saved it. Even one of the branded. 
        
        +Save your thanks, you beast. Even your subordinates can see that your actions are wrong.
//            ~mineLvl3ConvincedOnlyReka = true
//            setToTrue(mineLvl3ConvincedOnlyReka)
            ->Close
        +Obviously those that locked you in here do not care whether you live. Do they deserve such loyalty?
            ->Close
        +Anyone who lays down their weapons gets to live. Anyone who resists, dies. Those are my terms.
//            ~mineLvl3ConvincedOnlyReka = true
//            setToTrue(mineLvl3ConvincedOnlyReka)
            ->Close
    }
}

=== PH1 ===

I will not be lectured by a criminal on right and wrong! *The Overseer turns to the other guards.* You lot! Your choice is plain: kill them and stay free, or join them in their branding.

changeCamTarget({pazmanIndex})
    
\*Pázmán grimaces and unsheaths his blade.* Yes sir. 

changeCamTarget({rekaIndex})
    
Pázmán, you wretch. The Gods will roast you over your hearth for this. \*Réka draws her weapon as well, but brandishes it at the other guards.*

    +So be it. <Combat>
        ->combatPrep

=== PH2 ===

changeCamTarget({gasparIndex})

    Oh, and I suppose you do? You just want us to drop our weapons so you can butcher us later.

    +Do you really think if I can walk freely through this mine I'd be afraid to face as ragged a group as you? I'm offering to take you prisoner because I don't want to kill you, not because I can't.
    
        ~mineLvl3ConvincedRekaAndPazman = true
        setToTrue(mineLvl3ConvincedRekaAndPazman)
        ->Close

    +As far-fetched as it may seem, my actions prove it: I opened the gate, I brought the miners and guards together, I helped destroy the worm nest. Any other slave would have turned on you already, or left you to rot.
    
        ~mineLvl3ConvincedRekaAndPazman = true
        setToTrue(mineLvl3ConvincedRekaAndPazman)
        ->Close

    +Ah, you got me. <Combat>
        ->combatPrep

=== PH3 ===

changeCamTarget({rekaIndex})
    
Very true. \*Réka draws her weapon and points it at the Overseer.* I won't slay those who fought to free me. Not for you or the Director.

changeCamTarget({pazmanIndex})
    
\*Pázmán draws his weapon as well and turns his back to you.* Yeah, screw the Director. If protocol says I should die in the dirt, then I say protocol can bite me.

changeCamTarget({gasparIndex})

You'll regret this. All of you! You may have deluded these fools, but I won't surrender so you can shiv me with my hands bound. 

    +So be it. <Combat>
        ->combatPrep

=== PH4 ===

changeCamTarget({rekaIndex})
    
\*Réka draws her weapon and points it at the Overseer.* I'm taking my chances with them. I didn't wait this long in the dark to die here.

changeCamTarget({pazmanIndex})
    
\*Pázmán draws his weapon as well and turns his back to you.* That goes for me too. I-I want to see the sun again. 

changeCamTarget({gasparIndex})

You'll regret this. All of you! You may have deluded these fools, but I won't surrender so you can shiv me with my hands bound. 

    +So be it. <Combat>
        ->combatPrep

=== PH5 ===

changeCamTarget({rekaIndex})
    
\*Réka draws her weapon and points it at the Overseer.* I'm taking my chances with them. I didn't wait this long in the dark to die here.

changeCamTarget({gasparIndex})
Accepting terms from a slave now Réka? How low you've fallen. Kill them, one last bit of bloodshed before we can go home.

changeCamTarget({pazmanIndex})
    
\*Pázmán grimaces and unsheaths his blade.* Yes sir. 

->combatPrep

=== 3d ===
changeCamTarget({nandorIndex})

{
-toldToFindNandor:
\*Nandor whispers in your ear.* What are you doing? They're isolated. Now is our best time to remove them without any witnesses.

    +I'm not fighting some armed guards, even with you at my back.
        setToTrue(mineLvl3RefusedToFightGaspar)
        ->3da
    +If you're with me, I'll fight.
    ~agreedToFightGaspar = true
        ->3c
-else:
\*Nandor whispers in your ear.* I don't have time to explain, but I have no intention of going peacably to the surface. If we fight, will you join us?

    +I'm not fighting some armed guards, even with you at my back.
        setToTrue(mineLvl3RefusedToFightGaspar)
        ->3da
    +If you're with me, I'll fight.
        ->3c
}

=== 3da ===

Gáspár, we refuse! I don't care if I die here, I will no longer be a slave!

    +I'm not with them!
        kill({marcosIndex})
        kill({nandorIndex})
        kill({carterIndex})
        finishQuest(Sealing the Breach, true, 5)
        ->4d

=== 4a ===

{
-deathFlagGuardMárcos and mineLvl3MarcosAgreedToIgniteJelly:

changeCamTarget({nandorIndex})

Márcos...

    +He gave his life so that we might be free. Remember him that way.
        I will. Have no doubt about that.
        ->4b
    +I will not weep for a slaver. Even one who turned at the eleventh hour.
        I can understand why you say that, but to me he seemed different. Alas, it doesn't matter now.
        ->4b

-else:

changeCamTarget({nandorIndex})

    keepDialogue()
    The guards are dead, and the breach is sealed. I have not felt like this in a long time... Relieved. Hopeful. All thanks to your efforts.
    ->4b
}


=== 4b ===

The breach is sealed and the worms are no longer a threat. I have not felt like this in a long time... Relieved. Hopeful. All thanks to your efforts.

    +Don't thank me yet. We still need to rally the other slaves and fight clear of the guards.
        ->4c
    +I have that effect on people. Now, if you're ready, I say we head for the surface.
        ->4c

=== 4c ===

    setToTrue(mineLvl3SlavesBackToSurface)
    
    Of course. We will follow your lead.
        ->Close
    
=== 4d ===

changeCamTarget({gasparIndex})

I would have thought Márcos smarter than to die beside a bunch of slaves. But best not to dwell on dead traitors. Better to dwell on loyal workers, aye? Good work back there.

    +Thank you, sir.
        ->4e
    +\*Say nothing.*
        ->4e

=== 4e ===

setToTrue(mineLvl3GuardsBackToSurface)

The rest of us are heading back up to the surface. Now that I know you can be trusted, and since you seem to be able to make your way around these mines without too much difficulty, you can either come back up with us now or take a rest down here first. You've definitely earned it.

    +I'll return with you, Overseer.
        Alright everyone, we're heading back to the surface. Check your packs and then get moving!
        ->Close
    +I'll take you up on that rest, sir.
        Good, but don't dawdle. I expect you back up on the surface within the hour.
        ->Close

=== 4f ===

->Close

=== 4h ===

->Close

=== 4i ===

->Close

=== 4j ===

->Close

=== 4k ===

->Close

=== 4l ===

->Close

=== 4m ===

->Close

=== 4n ===

->Close

=== 5a ===

->Close
    
=== 2b ===

->Close

=== 2c ===

->Close

=== 2d ===

->Close

=== 2e ===

->Close

=== 2f ===

->Close

=== 2h ===

->Close

=== 2i ===

->Close

=== 2j ===

->Close

=== 2k ===

->Close

=== 2l ===

->Close

=== 2m ===

->Close

=== 2n ===

->Close

=== 6a === 

activate({rubbleMarcosIndex})
changeCamTarget({rubbleMarcosIndex})
setToTrue(mineLvl3BreachSealed)
    
I'll do my best, but it's getting colder. And I can't keep myself from shivering. It may be that I slip and spill the timer. Should this happen, I must know something.
    
    +Of course. Ask.
        ->6b
    +We don't have time for this. Get a move on.
        ->blowUpMarcos

=== 6b === 

I've been a guard at this camp since it was founded months ago. In my time here, I've seen all manner of things. I've... committed many of them. I need to know. Can you forgive me?
    +\*Place your hand on his shoulder.* Through your actions you've saved lives, Márcos. Push what you did before from your mind. To those you kept safe, that matters. 
        That means more to me than you can know. Thank you. I am ready.
        ->blowUpMarcos
    +I've only just gotten to this camp. My opinion can't give you the solace you're asking for.
        Then I hope the Gods are more forgiving than I am. I go to my hearth with many regrets.
        ->blowUpMarcos
    +I'm not a liar, Márcos. Perhaps it is well that your hands tremble. The Gods are the only ones who can give you what you're asking for.
        Then I hope the Gods are more forgiving than I am. I go to my hearth with many regrets.
        ->blowUpMarcos

=== blowUpMarcos ===

~deathFlagGuardMárcos = true
~mineLvl3MarcosDiedSealingBreach = true
setToTrue(mineLvl3MarcosDiedSealingBreach)
kill({marcosIndex})

    ->2a

=== 7a ===

\*The water begins dripping out of the spout and seeps into the barrel. A loud hissing sound begins to be emitted from it.*

    +Uh oh...
        ->Close

=== combatPrep ===

{
-mineLvl3ConvincedRekaAndPazman:

    deactivate({rekaIndex})
    deactivate({pazmanIndex})
    kill({viragIndex})
    kill({gasparIndex})
    enterCombat({halfGuardFightIndex},{dialogueKeyForAfterKillingGuards})
        ->Close
        
-else:

    kill({rekaIndex})
    kill({pazmanIndex})
    kill({viragIndex})
    kill({gasparIndex})
    enterCombat({fullGuardFightIndex},{dialogueKeyForAfterKillingGuards})
        ->Close
}


=== Close ===

deactivate({gasparIndex})
deactivate({rekaIndex})
deactivate({viragIndex})
deactivate({pazmanIndex})
deactivate({nandorIndex})
deactivate({carterIndex})
deactivate({marcosIndex})

close()

->DONE