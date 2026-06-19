VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR fightIndex = 0

VAR playerIndex = 0
VAR guardIndex = 1
VAR taborIndex = 2
VAR taborAtAnnouncementIndex = 3
VAR directorIndex = 4
VAR adelaIndex = 5
VAR crowdIndex = 6
VAR weftIndex = 7
VAR thatchIndex = 8
VAR takacsIndex = 9
VAR hangmanIndex = 10
VAR gasparIndex = 11
VAR gasparFallingIndex = 12

VAR partyFlagWeft = false
VAR partyFlagThatch = false

VAR skipTutorialIndex = 0
VAR intimidateTutorialIndex = 1
VAR cunningTutorialIndexIndex = 2

VAR toldByTaborToBuildHouses = false
VAR acceptedTaborSkillTutorial = false
VAR skippedTutorialInNWCamp = false
VAR gasparBroughtToExecution = false

VAR directorMentionedAnnouncement = false
VAR startedDirectorAnnouncement = false
VAR hadGuardWhoBlockedPathFlogged = false

VAR playerName = ""

{
-directorMentionedAnnouncement:
    ->readyCheck_1a
}

->1a

=== 1a ===

changeCamTarget({guardIndex})

You're not allowed in here. Head back the way you came.

{
-toldByTaborToBuildHouses and not (skippedTutorialInNWCamp or acceptedTaborSkillTutorial):
    +I'm under orders from Chief Tabor to report to this area.
        ->1b
}
    +\*Leave.*
        ->Close

=== 1b ===

In that case, the Chief arrived a short bit ago. Go inside, he should be near the hut under construction directly behind me.

\*This area will provide you with a tutorial on the different Skills your Party can use. If you are already familiar with these Skills, this section can be skipped.*

    +\*Learn about skills.*
        ->1c
    +\*Skip tutorial.* <Not recommended for first time players>
        ->1d

=== 1c ===

fadeToBlack()

resetTutorial(intimidateTutorialSequenceEntered)
resetTutorial(secondCunningTutorialSequenceEntered)
resetTutorial(observationTutorialSequenceEntered)
resetTutorial(leadershipTutorialSequenceEntered)

setToTrue(acceptedTaborSkillTutorial)
setToTrue(canEnterCampNorthWest)
deactivate({guardIndex})
activate({taborIndex})

fadeBackIn(60)

->Close

=== 1d ===

setToTrue(skippedTutorialInNWCamp)
getNewDialogueFromList(NWCampChief Tabor,true,skippedTutorialInNWCamp)

->Close

=== readyCheck_1a ===

\*Entering this area will progress the story. You may not be able to return to this location. Are you certain you wish to proceed?*

    +\*Continue.*
        setToTrue(startedDirectorAnnouncement)
        ->proceedToDirectorSpeech
        // ->readyCheck_1b
    +\*Leave.*
        ->Close

=== readyCheck_1b ===

setToTrue(startedDirectorAnnouncement)

fadeToBlack(true,false)

movePlayerPos(-15,0)
setFacing(NE)
setNPCFacing({guardIndex},SW)

fadeBackIn(60)

Speak yer business, branded.

    +The Director gave me orders to report here. I'm to be pardoned.
{
-gasparBroughtToExecution:
    ->3a
-else:
    ->2a
}

=== 2a ===

Yer the branded who cleared the mine? You don't look near tough enough fer that kinda work. Don't see why you get credit for solvin' somethin' that's yer fault in the first place. If we weren't so busy makin' sure yer lot kept in line, we'd 'ave done it ourselves.

    +Is that a joke? The Director had to send me because he couldn't trust 'yer lot' to know your weapon from your arsehole.
        ->2b
    +I don't have time for this. Are you going to let me by or not?
        ->3b

=== 2b ===

You 'aven't been free a day and yer already talkin' like yer a count. Pardon or no, I'll have yous flogged if yeh don't respect yer betters while I'm 'round.

    +How about we both calm down and start over? There's no need to ruin the moment with violence.
        ->3b
    +I've slain worms five times your size. You'll apologize if you know what's good for you.
        ->3b


=== 3a ===

So I've 'eard. *The guard shakes his head.* Gáspár's to be 'anged and yous get to go free? 'ole world's gone upside down.

    +Are you going to let me past or not?
        ->3b
    +Take it up with the Director. Now let me by. 
        ->3b

=== 3b ===

And what if I don't? 

fadeToBlack(true, false)

activate({taborIndex})
changeCamTarget({taborIndex})
playAnimation({taborIndex},Idle_Front)

fadeBackIn(60)

Then you'll get a lashing for harrassing a free citizen.

changeCamTarget({guardIndex})
setNPCFacing({guardIndex},NE)

Chief! I uh... didn't see you there.

changeCamTarget({taborIndex})

So it would appear. {playerName}, was this man bothering you?

    +He was doing more than that.

        setToTrue(hadGuardWhoBlockedPathFlogged)

        Soldier, you have kept the Director waiting with your rudeness. You will report to the whipping post for ten lashes. You're lucky most of the camp will be listening to the Director's speech, or else I'd have them watch the ordeal.
        ->3c
    +Not particularly. I had it handled.
        Soldier, it is because of this one's mercy that you have escaped punishment. Now gather with the others, I will keep your post while I speak with {playerName};
        ->3c
    
=== 3c ===

changeCamTarget({guardIndex})

Yes, Chief. 

fadeToBlack(true, false)

deactivate({guardIndex})
playAnimation({taborIndex},OOC_Idle_Front)
changeCamTarget({taborIndex})

fadeBackIn(60)

It is a confusing time for all of us. The Director has always been one to think laterally, but a pardon is a bit much for the others to swallow. I'm still trying to get my head around it myself.

Whether I agree with the Director's decision or not, I will admit your service was exemplary. And so, because you are unlikely to hear it from anyone else today... thank you. Your work saved many of the guards' lives, even if they will never acknowledge it.

    +I didn't do it for them. In fact, now that you say that I almost wish I didn't.
        I know you didn't, but that doesn't diminish what you did for them. It's worth recognizing all the same.
        ->3d
    +Er... you're welcome?
        \*Tabor shifts uncomfortably.* This feels awkward for me as well, just so you know. But decorum is a virtue in of itself.
        ->3d
    +You're a strange one, Tabor. 
        As are you. {doneGreatService}
        {puttingAside}
        ->3e

VAR puttingAside = "You are certainly the first branded I've ever corrected that survived the ordeal. I want to understand you better. What did we do that reached you? How can we better teach the other branded?"

=== 3d ===

VAR doneGreatService = "You have done the Confederation a worthy service and for that the Director has pardoned you. My term as your correctional officer is at an end: the way I see it, you've graduated from this camp."
VAR thePain = "The pain I've inflicted was meant to help align the branded with the goals of the Confederation."

    +Your mix of courtesy and sadism is certainly unique. Can't say I'll miss it.
        I don't take pleasure in what I've done. {thePain} {doneGreatService}
        {puttingAside}
        ->3e
    +This charade is getting old. It's hard to believe your sincerity after I've seen how quickly you reach for your whip.
        And yet, I ask that you believe it anyways. {thePain} {doneGreatService}
        {puttingAside}
        ->3e
    +It's bizarre to hear this from you, but I'll take whatever thanks I can get.
        {doneGreatService}
        {puttingAside}
        ->3e

=== 3e ===

    +You're a man adrift in your own little world, and I've stepped in shit that I respect more than you. Get out of my way.
        \*Tabor stands aside without saying another word.*
        ->proceedToDirectorSpeech
    +Do the other Lovashi even care to correct the branded? You're one of the few guards I've heard take that seriously.
        There was a time when it was a much revered nuance of the system. Now, it seems that the never-ending nature of our struggle has pushed many Lovashi towards apathy. They think it's all 'pissing in the wind', as Adéla puts it. But to me, it's what separates us from the Craft Folk.

        I take pride that my people did not start this conflict. When we were forced to punish those who did, it was to teach them the error of their ways. That's why, to me, to teach is a calling. I uplift all Lovashi by keeping this tradition alive.
        ->keepingDirectorWaiting_1a
    +Your methods are barbaric. The Confederation will never reach an understanding with the other branded if it continues to send people like you to degrade and abuse them.
        \*Tabor thinks for a moment, then shakes his head.* I don't think I want to live in a world where people like me do not await people like them. That's what keeps the Confederation just.
        ->keepingDirectorWaiting_1a
    +I'm proof that the brand should not exist. If the branded were not consigned to die, they would more easily reveal to you they are worthy of life.
        Perhaps that is true. I'm not convinced it doesn't have its purposes, but there are so many branded. Your unceasing numbers bode ill for the future of the Confederation.
        ->keepingDirectorWaiting_1a
    +I've been here for less than a day. You're awfully full of yourself to take credit for anything I've done.
        You have a point. The teacher could use his own lesson in humility, then.
        ->keepingDirectorWaiting_1a
    +I'm done with this conversation. Goodbye Tabor.
        ->keepingDirectorWaiting_1b

=== keepingDirectorWaiting_1a ===

VAR farewell1 = "Farewell, "
VAR farewell2 = ". I expect not to see your like again."
VAR farewell = ""
~farewell = farewell1 + playerName + farewell2

But we are keeping the Director waiting. It would be best if we move along. {farewell}

->proceedToDirectorSpeech

=== keepingDirectorWaiting_1b ===

{farewell}

->proceedToDirectorSpeech

=== proceedToDirectorSpeech ===

fadeToBlack(true, false)

deactivate({taborIndex})
movePlayerPos(-9,14)
setFacing(SE)
changeCamTarget({directorIndex})

{
-partyFlagWeft:
activate({weftIndex})
}

{
-partyFlagThatch:
activate({thatchIndex})
}

fadeBackIn(60)

->directorSpeech

\*The Director clears his throat.*

changeCamTarget({adelaIndex})

Listen up! The Director speaks!

changeCamTarget({crowdIndex})

\*The gathered guards quiet down.*

changeCamTarget({directorIndex})

{
-gasparBroughtToExecution:
->gasparExecution
-else:
->directorSpeech
}

=== gasparExecution ===

Before we set out from Pharos four months ago, each of you was told what you were to expect. That we were to venture into hostile lands; that you would be surrounded by enemies, far from home; that you may never again return.

Each of you volunteered for this mission, and took oaths against your lives that you would see it completed. Oaths to me, oaths to each other, and oaths to the Confederation. Those that would prove their words empty are no longer your comrades, but cowards and traitors!

disableDialogueUI()

playAnimation({hangmanIndex},Idle_Back)
changeCamTarget({hangmanIndex})

setNPCFacing({directorIndex},NE)
setNPCFacing({gasparIndex},NW)

wait(1)

playAnimation({hangmanIndex},Attack_Normal_Back)

wait(.25)

setNPCFacing({gasparIndex},NE)
hideExtras({gasparIndex})
playAnimation({gasparIndex},Death)

wait(3)

changeCamTarget({directorIndex})

setNPCFacing({directorIndex},SE)
setNPCFacing({hangmanIndex},SE)

wait(.25)

enableDialogueUI()

So dies such a craven! Gáspár of Gécz was charged with the protection of the mine's depths and instead of carrying out that duty, consigned branded and guard alike to deaths by monsters and starvation rather than rise to their defense! 

May his hanging remind each of us of what fate awaits should we no longer cherish our words and bonds to eachother: a coward's death, at the hands of our betters.

->weftInterjection

=== weftInterjection ===

changeCamTarget({weftIndex})

Better than he deserved. *Weft spits*

    +I hope he hits every rock on the way down.
        \*Weft stifles a smirk.*
        ->directorSpeech
    +May his hearth keep him warm.
        ->directorSpeech
    +\*Say nothing*
        ->directorSpeech

=== directorSpeech ===

// changeCamTarget({directorIndex})

// For over four months, you have shouldered the duty of erecting this camp. You have executed your assigned tasks with a speed and sureness that does our confederation much credit. 

// Where once stood untamed forest and crumbling structures, you have forged a bastion which I am proud to hold in the name of Count Kálnoky!

// Many of you come from disparate backgrounds: the cream of every county stands before me. Spears from the shores of Lake Jawan, axes from the forests of County Thököly, even Kiln-breakers from my home of Pharos, and of course, my own household guard. 

// Each of you came recommended highly, and your patrons will know no disappointment from me! I have met priests of Harmony who could not cooperate on the level you have while in my service.

// changeCamTarget({crowdIndex})

// \*The ranks of the Lovashi let out a cheer.*

// changeCamTarget({directorIndex})

// \*The Director waits for the cheer to fade before continuing.* I near the end of my eigth decade. In that time, I have ridden with names many of you only know from stories as we waged our war against the Craft Folk. 

// I watched as the fires rose over Carnassus, and your fathers toppled the great statues of Saint Lysop. I beheld the death of our King Csaba at the hands of the King of Kilns, and the scouring of Pharos which followed. I lead our western lance against the Masons and layed seige to their jewel, Wudra; the furthest our hordes have ridden against our hated foes.

// Over this long life, I have witnessed our traditions of honor, loyalty, and strength earn us lands and trophies at great cost. Over the many cycles of war and peace, I have had cause to wonder if the cost we paid was not simply in blood and kin, but whether those very values we held in such regard were hidden within the final tally when the bill came due.

// createEffect(SmokeBomb,-7,14)

// \*The Director pauses for a moment in thought.*

// These lost virtues are not unrecoverable, however. Our hated enemies are not so fearsome that they cannot be overcome by our combined might. At the end of the last war the Confederation left their armies broken, and their lands torched and scarred. When the next war begins, our people will find them a pathetic, crippled foe unable to withstand the combined might of our glorious hordes!

// disableDialogueUI()
// createEffect(SmokeBomb,-6,14)
// wait(0.33)
// activate({takacsIndex})
// wait(1.8)
// enableDialogueUI()
// setPlayerFacing(NE)
// changeCamTarget({takacsIndex})

// \*Takács's voice rings clear in every ear present, as if speaking from an inch away.* The Director is quite right to be proud of your progress, if we are kind enough to dismiss recent events. Count Kálnoky is pleased with all of you.

// changeCamTarget({adelaIndex})

// Vada... *Adéla shivers with discomfort.*

// changeCamTarget({takacsIndex})

// There is but one, insignificant detail that has been overlooked. A fly buzzes in this camp. A fat, happy, lazy fly, which you will help me catch for my supper.

// disableDialogueUI()
// createEffect(SmokeBomb,-9,10)
// wait(0.4)
// createEffect(SmokeBomb,-4,8)
// wait(0.4)
// createEffect(SmokeBomb,-7,6)
// wait(0.4)
// createEffect(SmokeBomb,-5,10)
// wait(0.4)
// createEffect(SmokeBomb,-10,6)
// wait(0.4)
// createEffect(SmokeBomb,-8,9)
// wait(0.4)
// createEffect(SmokeBomb,-6,8)
// wait(1.5)
// enableDialogueUI()

// The pardoned branded are a threat to the security of this camp: they cannot be allowed to leave with knowledge of its existance. 

// Soldiers of the Confederation! Eliminate the threat.

// changeCameraTarget({crowdIndex})

// setIdleOfNPCsByName(Guard3,Idle_Back)
// setIdleOfNPCsByName(Guard4,Idle_Front)
// playAnimation({adelaIndex},Idle_Front)

// \*The Lovashi draw their weapons and begin to move towards you with ill intent.*

setFacing(SE)
playAnimation({playerIndex},Idle_Front)
changeCamTarget({weftIndex})

Oh gods... what do we do?

    +We fight. There's no other option. <Combat>
        ->enterCombat
    +We jump. Aim for the pool next to the body pile.
        ->jumpInPit

=== jumpInPit ===

disableDialogueUI()

setFacing(NW)
setNPCFacing({weftIndex},NW)

wait(.35)

setDialogueUponSceneLoadKey(afterTakacsFight)

changeLocation(Body Pile)

->Close

=== enterCombat ===

enterCombat({fightIndex})

->Close

=== Close ===

close()

->DONE

/*
VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerIndex = 0
VAR guardOneIndex = 1
VAR guardTwoIndex = 2
VAR ladderIndex = 3
VAR weftIndex = 4
VAR thatchIndex = 5
VAR gasparHangingIndex = 6
VAR gasparShadowIndex = 7
VAR gasparCutDownIndex = 8

VAR partyFlagThatch = false
VAR hadAfterTakacsFightConvo = false
VAR gasparBroughtToExecution = false
VAR gasparSavedFromNoose = false

VAR gasparSaysDirectorDidNotBetrayPlayer = false

VAR acceptedDirectorVidraLetterJob = false

VAR playerName = ""

->1a

=== 1a ===

addSecretDoorFlag(CampBody PileUnseen Barrier)
duckMusic()
setToTrue(hadAfterTakacsFightConvo)

stopAllFades()
setFacing(NE)
deactivate({playerIndex})
activate({guardOneIndex})
activate({guardTwoIndex})
activate({ladderIndex})

{
-gasparBroughtToExecution:
changeCamTarget({gasparHangingIndex})
activate({gasparHangingIndex})
activate({gasparShadowIndex})
playAnimation({gasparHangingIndex},Secondary_Idle)
hideExtras({gasparHangingIndex})
setNPCFacing({guardOneIndex},SE)
setNPCFacing({guardTwoIndex},SE)
-else:
changeCamTarget({guardOneIndex})
}

disableDialogueUI()
slowFadeBackIn(5)
wait(3.25)

enableDialogueUI()
{
-gasparBroughtToExecution:
changeCamTarget({guardTwoIndex})
    ->gasparHanging
}

->1aa

=== gasparHanging ===

Stubborn bastard. How long do you think he's gonna kick for?

changeCamTarget({guardOneIndex})

Dunno. I'm surprised his neck didn't break when he ran out of rope.

changeCamTarget({guardTwoIndex})

He's always been a stiff-necked mule. Maybe he's too uptight to die.

changeCamTarget({guardOneIndex})

No neck's that thick.

changeCamTarget({guardTwoIndex})

Must have caught a branch on the way down then. Would've been smarter not to fight it. Look where it got him.

changeCamTarget({guardOneIndex})
->1aa

=== 1aa ===

Find anything?

{
-gasparBroughtToExecution:
setNPCFacing({guardTwoIndex},NW)
}

changeCamTarget({guardTwoIndex})

Nah, nothing. 

changeCamTarget({guardOneIndex})

Hard to believe anyone could survive a fall like that.

changeCamTarget({guardTwoIndex})

These bodies are too mangled. Between the impact and the scavengers, I can't tell one from another.

changeCamTarget({guardOneIndex})

\*The guard kicks the body in front of him.* You'd think I'd recognize Weft in any condition. I've had him run jobs for me more times than I got fingers, toes, and gaps between. Not sure about that new one though.

changeCamTarget({guardTwoIndex})

Yeah, I didn't know them from any of the other branded... even before they fell down here.

changeCamTarget({guardOneIndex})

Think we should head back? The smell is making my eyes water.

changeCamTarget({guardTwoIndex})
setNPCFacing({guardTwoIndex},NW)

You want to come back with nothing when that Vada's kicking around? I'd like to see the Confederation again, not get executed for incompetance all the way out here.

disableDialogueUI()
wait(1)
playDelayedSFX(Bat_Howl,150)
wait(.3)
setNPCFacing({guardOneIndex},NW)
setNPCFacing({guardTwoIndex},NE)
wait(3.5)
enableDialogueUI()

Did you hear that?

changeCamTarget({guardOneIndex})

How could I not? 

changeCamTarget({guardTwoIndex})

Pick one of the more mashed bodies and let's get out of here. With luck, the Director won't be able to tell the difference neither.

fadeToBlack(true, false)

deactivate({guardOneIndex})
deactivate({guardTwoIndex})
deactivate({ladderIndex})

activate({playerIndex})
activate({weftIndex})

{
-partyFlagThatch:
activate({thatchIndex})
}

movePlayerPos(-2,0)

playAnimation({playerIndex},OOC_Idle_Back)
changeCamTarget({weftIndex})

wait(.5)

fadeBackIn(60)

\*Weft gasps for breath. His lungs can't inhale greedily enough for several moments.*

    +\*Cough up some water.*
        ->1b

=== 1b ===

\*Weft finally starts to get his breath under control, and speaks between gasps.* I can't believe it. I'm alive! Glorious Gods, we're alive!

    +\*Continue coughing.* Maybe you are. I'm still not sure about myself.
        ->1ba
    +\*Whisper* Keep it down! The Lovashi don't know that yet.
        \*Weft quiets himself, looking up to the cliffs above to see if any Lovashi heard him.*
        ->gasparCheck
    +I knew I was going to make it, but count me just as shocked that you did.
        ->1ba

=== 1ba ===

What? I can't hear you, my heart is pounding too fast in my ears. And what beautiful music it is! *Weft laughs*

->gasparCheck

=== gasparCheck ===

{
-gasparBroughtToExecution:

setNPCFacing({weftIndex},SE)
Wait. Is that Gáspár?
    ->cutDownGaspar_1a
-else:
    ->1c
}

=== cutDownGaspar_1a ===

setFacing(SE)
changeCamTarget({gasparHangingIndex})
disableDialogueUI()

wait(3)

enableDialogueUI()
changeCamTarget({weftIndex})

\*Weft looks up at him with pity.* Forget what I said before, this is ghastly to watch.

    +We should cut him down. Even if he still ends up dead, it would be a mercy. <Cut down Gáspár>
        Ok, come on. I'll boost you up to him.
        ->cutDownGaspar_1b
    +Now you know how it feels, overseer! The brand was just like that! <Leave Gáspár>
        ->gasparDies
    +\*Say nothing, and watch him struggle.* <Leave Gáspár>
        ->gasparDies


=== cutDownGaspar_1b ===

changeCamTarget({gasparHangingIndex})
fadeToBlack(true,false)
disableDialogueUI()

deactivate({gasparHangingIndex})
deactivate({gasparShadowIndex})
activate({gasparCutDownIndex})
resetIdleDictionaryEntry({gasparCutDownIndex})
setFacing(NW)
setNPCFacing({gasparCutDownIndex},SE)
setToTrue(gasparSavedFromNoose)

playAnimation({gasparCutDownIndex},OOC_Idle_Front)
changeCamTarget({gasparCutDownIndex})

wait(1)

fadeBackIn(60)

enableDialogueUI()
VAR gaveGasparNeckAdvice = false
VAR askedIfItWasHorrible = false
VAR andWorseForIt = false

\*Gáspár coughs heavily. His entire body shivers, and he sways from side to side. More than once does he reach for his own neck, just to jerk his hand away at the touch of the clear bruise that has begun to show there.*

    +Some of the other branded showed me how to hold your neck so that it hurts less. See? Like this.
        ~gaveGasparNeckAdvice = true
        ->cutDownGaspar_1c
    +\*Wait patiently for Gáspár to stop coughing.*
        ->cutDownGaspar_1c
    +Was it as horrible to experience as it was to watch?
        ~askedIfItWasHorrible = true
        ->cutDownGaspar_1c
    +You're a sturdy one, to survive something like that.
        ~andWorseForIt = true
        ->Close

=== cutDownGaspar_1c ===

\*Gáspár tries to talk but can't manage to get words through his coughing. He resorts to taking long, pained breaths to force air into his lungs.*

\*Eventually, he manages to get some words out as his breathing finds some pained caricature of normalcy. *{gaveGasparNeckAdvice:That helped a little. }{askedIfItWasHorrible:Your jokes are tasteless, branded. }{askedIfItWasHorrible:And all the worse for it. }Why did you cut me down? They'll kill you for that, pardon be damned.

    +The pardon was a ruse. Or it was canceled. I'm not sure, but either way, the Lovashi want us dead.
        ->cutDownGaspar_1d

=== cutDownGaspar_1d ===

~gasparSaysDirectorDidNotBetrayPlayer = true

\*Gáspár continues with a rasp that sometimes reverts to fits of coughing. He shakes his head.* No ruse. The Director wouldn't lie to you. If he wanted to kill you, he'd look you in the eye while he did it.

    +I'm not sure I believe that. Warriors who live that long do so by being crafty.
        It's a matter of honor, branded. Something I wouldn't expect you to understand.
        ->cutDownGaspar_1f
    +I'm surprised to hear you still defending him. Look what he did to you.
        ->cutDownGaspar_1e

=== cutDownGaspar_1e ===

\*Gáspár shakes his head, but then makes a face as if overcome by nausea.* I did this to myself. I fell funny, instead of straight out into the ravine. Must have hit every rock on the way down.

    +He ordered you executed, and still you defend him?
        Such was his right as my superior. That's called loyalty, branded. Something I expect is a mystery to you.
        ->cutDownGaspar_1f

=== cutDownGaspar_1f ===

    +Mind how you throw insults. You're as much branded now as I am.
        ->cutDownGaspar_1fa
    +Whatever. Counting rescuing you in the mine, I've saved your life twice now. That means you'll address us with respect if you have any sense of gratitude.
        \*Gáspár looks like he wants to argue with you, but instead begins to cough again. Eventually, he manages to nod.*
        ->1c

=== cutDownGaspar_1fa ===

\*Gáspár meets your eyes with loathing.* I am no branded.

    +No? You've got a neck scar, and you're a criminal that the Lovashi want dead. That sounds like a branded to me.
        \*Gáspár looks like he wants to argue with you, but instead begins to cough again. Eventually, he manages to shake his head.*
        ->1c

=== gasparDies ===

\*Weft watches Gáspár struggle a moment longer, then turns away.*

setNPCFacing({weftIndex},NW)
changeCamTarget({gasparHangingIndex})

disableDialogueUI()

wait(3)

enableDialogueUI()

setFacing(NW)
setNPCFacing({weftIndex},SW)

Gáspár's fate aside, I'm overjoyed to still be among the living... er, among the dead, but alive, I mean. 

->1c

=== 1c ===

VAR askedAboutVada = false

{
    -true:
    +The woman with the spider for a head. Who was she? I heard Adéla call her 'Vada'.
        ~askedAboutVada = true
        {
            -gasparSavedFromNoose:
                ->1ca
            -else:
                ->1cb
        }
}
{
-askedAboutVada or not gasparSavedFromNoose:
    +{gasparSaysDirectorDidNotBetrayPlayer}If the Director didn't betray us, then why not give us the pardon?
        ->1caa
    +{not gasparSaysDirectorDidNotBetrayPlayer}What I don't understand is why would the Director betray us? He had plenty of opportunities to do it before now.
        ->1caa
}

    +How are we going to get out of this crevice? It looks like the Lovashi have removed the ladder back to the camp.
        ->1f

=== 1caa ===

        {
            -gasparSavedFromNoose:
                ->1eb
            -else:
                ->1e
        }

=== 1ca ===

changeCamTarget({gasparCutDownIndex})

What woman? A Vada? Here?

    +After you were pushed into the ravine, the Director continued his speech. Before he announced our pardon, a woman with a headdress shaped like the head of a spider appeared and ordered the Lovashi to attack us. That's why we're down here. 
        ->1cb

=== 1cb ===

changeCamTarget({weftIndex})

Do you really not know? The Vada are spirits that the Lovashi use to terrorize the Craft Folk on their lands. Sew discord, foment chaos, keep the serfs from trusting each other too much to rebel.

Each one takes after a particular beast. That one must have been the Spider... Tawh-cache or something. 'The Weaver', they call her. I've heard she can take the form of other people, cast spells that make you unable to tell friend from foe, even curse you or your village just by being in her presence. Maybe all the Vada can. I don't know. 

{
-gasparSavedFromNoose:
changeCamTarget({gasparCutDownIndex})
Takács. She can do all of that and more besides. The Vada were Táltos. *Gáspás searches for the words to explain.* Religious leaders. Like priests, but gifted with abilities beyond what we would consider normal. 

}

->1d

=== 1d ===

    {
    -gasparSavedFromNoose:
    +'Were'? What happened to them?
        changeCamTarget({gasparCutDownIndex})
        I don't know, but what is clear is they are more spirit than human now; burden entities carried along bloodlines. A child born of the line of a Vada may become possessed by them, turning into one themselves. The possessed gains the knowledge of all of the memories of the Vada that takes hold of them, in essence <i>becoming</i> them.
        ->1d
    -else:
    +She looked mostly human to me. What makes her so dangerous?
        ->1da
    }
    +How many other Vada are there?
    {
    -gasparSavedFromNoose:
        changeCamTarget({gasparCutDownIndex})
    -else:
        changeCamTarget({weftIndex})
    }
        I've heard of a few; Hound, Owl, Bear... Otter, Crow... Crane. I probably could think of a dozen if I had the time. A sage would be able to recall more; I doubt I've heard stories about them all.
        ->1d
    +What was she doing here? This seems to be a strange place to meet something like her.
    {
    -gasparSavedFromNoose:
        changeCamTarget({gasparCutDownIndex})
        The counts have many Vada at their beck and call. Our mission at this camp was important; so important that it's true purpose is known only to the Director and a select few others. Maybe it was important enough to warrant the supervision of one of the Vada as well.
    -else:
        changeCamTarget({weftIndex})
        How should I know? The counts can command them somehow, that's why they're always bothering the Craft Folk and mostly leave the Lovashi be. Maybe one of the counts sent her here. For what purpose, I cannot say.
    }
        ->1d
    +She did something to the Lovashi. They weren't near so belligerent before she showed up.
        changeCamTarget({weftIndex})
        Yeah, you're right. I actually thought that we were going to get our pardon before the Director started talking about crushing the Lovashi's enemies. I'm not sure why, but she really did not want us to leave the camp alive.
    {
    -gasparSavedFromNoose:
        changeCamTarget({gasparCutDownIndex})
        I assumed you would not have been allowed to leave this camp until it's purpose was complete. If the Director meant to let you leave before then... *Gáspár looks troubled, and lost in thought.*
    }
        ->1d
    +We have more important things to discuss right now.
        changeCamTarget({weftIndex})
        Yeah, like how to get out of here.
        ->1c

=== 1da ===

changeCamTarget({weftIndex})

Don't be fooled. The Vada are anything but human: they can't be killed. If you do manage to harm one, they just come back later. I've heard stories of when the Craft Folk killed a few back in the day; the Vada would eventually crawl out of the graves they were buried in to haunt the children of their slayer.

    +I saw a part of a face under that headdress she was wearing. How can you be sure they are the same creature, and not just some other person in a mask?
        In all of the stories, the Vada always remembers all of the ways they've died. Even when there are no witnesses. Even when there are no tales told of their defeat. They just know. That's part of what makes them so horrible: they can't be beaten the same way twice.
        ->1d

=== 1e ===

changeCamTarget({weftIndex})

Maybe he wanted to wait for the camp to be present? That way his guards could overwhelm you with their numbers. He probably was scared of us after we defeated the worms for him.

{
-acceptedDirectorVidraLetterJob:
    +But he gave me this letter to deliver. I can't give it to his friend for him if I'm dead.
        Most perplexing. I really can't say. Perhaps he changed his mind?
        ->1ea
-else:
    +But he offered me a job after he told me he would give me a pardon. I couldn't have done it for him if he was going to betray me.
        Most perplexing. Perhaps he changed his mind <i>because</i> you turned him down? I really can't say. 
        ->1ea
}

=== 1ea ===

    +Or that Vada changed it for him. Either way, we can't ask him now.

        \*Weft looks up at the wall of the ravine.* Too true.
        ->1c

=== 1eb ===

changeCamTarget({gasparCutDownIndex})

A man like the Director has no need for lies. {gasparSaysDirectorDidNotBetrayPlayer: |He did not betray you. }If he wanted to kill you, he'd look you in the eye while he did it.

    +Then why not order his troops to stand down?
        A Vada carries the weight of a count's authority with their words. Even the Director could not go against them without good reason. They must have opposed the decision, and ordered your death instead.
        ->1c

=== 1f ===

changeCamTarget({weftIndex})

activateQuestStep(Leave the Body Pile,Enter the pool.)

While I was in the pool, I saw some tunnels leading in a few different directions. I'm not certain they lead anywhere, but they might be worth exploring.

    +That's not very promising, but I guess swimming will be easier than climbing back up to the camp. Let's give it a try.
        {
            -gasparSavedFromNoose:
            ->1fa
            -else:
            ->deactivateExtras
        }

=== 1fa === 

changeCamTarget({gasparCutDownIndex})

\*Gáspár looks up at the sky, and the brim of the ravine. He lingers there for some time.*

    +You can't be considering climbing back up. Even if you made the climb, they'd kill you.
        My oaths demand I do. But... they have already declared me an oathbreaker. For now, I will follow you. Until I can bring myself to return.
        ->deactivateExtras
    +\*Say nothing and watch him.*
        \*Gáspár shakes his head.* Let us go, then. 
        ->deactivateExtras


=== deactivateExtras ===

fadeToBlack()

deactivate({weftIndex})
deactivate({thatchIndex})

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE

{
-askedAboutVada or not gasparSavedFromNoose:
    +{gasparSaysDirectorDidNotBetrayPlayer:If the Director didn't betray us, then why not give us the pardon?|What I don't understand is why would the Director betray us? He had plenty of opportunities to do it before now.}
        {
            -gasparSavedFromNoose:
                ->1eb
            -else:
                ->1e
        }
}

    +How are we going to get out of this crevice? It looks like the Lovashi have removed the ladder back to the camp.
        ->1f

=== 1ca ===

changeCamTarget({gasparCutDownIndex})

What woman? A Vada? Here?

    +After you were pushed into the ravine, the Director continued his speech. Before he announced our pardon, a woman with a headdress shaped like the head of a spider appeared and ordered the Lovashi to attack us. That's why we're down here. 
        ->1cb

*/