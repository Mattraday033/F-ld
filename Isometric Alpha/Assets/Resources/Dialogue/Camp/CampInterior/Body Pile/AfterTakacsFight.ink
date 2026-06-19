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

Hard to believe those slaves could survive a fall like that.

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
setToTrue(gasparSavedFromNoose)
resetIdleDictionaryEntry({gasparCutDownIndex})
setFacing(NE)
setNPCFacing({weftIndex},SW)
setNPCFacing({gasparCutDownIndex},NW)

playAnimation({gasparCutDownIndex},OOC_Idle_Front)
changeCamTarget({gasparCutDownIndex})

wait(1)

fadeBackIn(60)

enableDialogueUI()
VAR gaveGasparNeckAdvice = false
VAR askedIfItWasHorrible = false
VAR andWorseForIt = false

\*Gáspár coughs heavily. His entire body shivers, and he sways from side to side. More than once does he reach for his own neck, just to jerk his hand away upon touching the wound inflicted by the grinding rope.*

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

\*Eventually, he manages to get some words out as his breathing finds some pained caricature of normalcy.* {gaveGasparNeckAdvice:That helped a little. }{askedIfItWasHorrible:Your jokes are tasteless, branded. }{askedIfItWasHorrible:And all the worse for it. }Why did you cut me down? They'll kill you for that, pardon be damned.

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
    + What I don't understand is{gasparSaysDirectorDidNotBetrayPlayer:if the Director didn't betray us, then why not give us the pardon?| why would the Director betray us? He had plenty of opportunities to do it before now.}
        {
            -gasparSavedFromNoose:
                ->1eb
            -else:
                ->1e
        }
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
setNPCFacing({gasparCutDownIndex},SE)

\*Gáspár looks up at the sky, and the brim of the ravine. He lingers there for some time.*

    +You can't be considering climbing back up. Even if you made the climb, they'd kill you.
        setNPCFacing({gasparCutDownIndex},NW)
        My oaths demand I do. But... they have already declared me an oathbreaker. For now, I will follow you, until I can bring myself to return.
        ->deactivateExtras
    +\*Say nothing and watch him.*
        setNPCFacing({gasparCutDownIndex},NW)
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