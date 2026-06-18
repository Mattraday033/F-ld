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

VAR partyFlagThatch = false
VAR hadAfterTakacsFightConvo = false

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
disableDialogueUI()
changeCamTarget({guardOneIndex})
slowFadeBackIn(5)
wait(3.25)

enableDialogueUI()

Find anything?

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
        ->1ca
    +\*Whisper* Keep it down! The Lovashi don't know that yet.
        \*Weft quiets himself, looking up to the cliffs above to see if any Lovashi heard him.*
        ->1c
    +I knew I was going to make it, but count me just as shocked that you did.
        ->1ca

=== 1ca ===

What? I can't hear you, my heart is pounding too fast in my ears. And what beautiful music it is! *Weft laughs*
    ->1c

=== 1c ===

    +The woman with the spider for a head. Who was she? I heard Adéla call her 'Vada'.
        Do you really not know? The Vada are spirits that the Lovashi use to terrorize the Craft Folk on their lands. Sew discord, foment chaos, keep the serfs from trusting each other too much to rebel.

        Each one takes after a particular beast. That one must have been the Spider... Tawh-cache or something. 'The Weaver', they call her. I've heard she can take the form of other people, cast spells that make you unable to tell friend from foe, even curse you or your village just by being in her presence. Maybe all the Vada can. I don't know. 
        ->1d
    +What I don't understand is why would the Director betray us? He had plenty of opportunities to do it before now.
        ->1e
    +How are we going to get out of this crevice? It looks like the Lovashi have removed the ladder back to the camp.
        ->1f

=== 1d ===

    +She looked mostly human to me. What makes her so dangerous?
        ->1da
    +How many other Vada are there?
        I've heard of a few; Hound, Owl, Bear... Otter, Crow... Crane. I probably could think of a dozen if I had the time. A sage may be able to recall more; I doubt I've heard stories about them all.
        ->1d
    +What was she doing here? This seems to be a strange place to meet something like her.
        How should I know? The counts can command them somehow, that's why they're always bothering the Craft Folk and mostly leave the Lovashi be. Maybe one of the counts sent her here. For what purpose, I cannot say.
        ->1d
    +She did something to the Lovashi. They weren't near so belligerent before she showed up.
        Yeah, you're right. I actually thought that we were going to get our pardon before the Director started talking about crushing the Lovashi's enemies. I'm not sure why, but she really did not want us to leave the camp alive.
        ->1d
    +We have more important things to discuss right now.
        Yeah, like how to get out of here.
        ->1c

=== 1da ===

Don't be fooled. The Vada are anything but human: they can't be killed. If you do manage to harm one, they just come back later. I've heard stories of when the Craft Folk killed a few back in the day; the Vada would eventually crawl out of the graves they were buried in to haunt the children of their slayer.

    +I saw a part of a face under that headdress she was wearing. How can you be sure they are the same creature, and not just some other person in a mask?
        In all of the stories, the Vada always remembers all of the ways they've died. Even when there are no witnesses. Even when there are no tales told of their defeat. They just know. That's part of what makes them so horrible: they can't be beaten the same way twice.
        ->1d

=== 1e ===

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

=== 1f ===

activateQuestStep(Leave the Body Pile,Enter the pool.)

While I was in the pool, I saw some tunnels leading in a few different directions. I'm not certain they lead anywhere, but they might be worth exploring.

    +That's not very promising, but I guess swimming will be easier than climbing back up to the camp. Let's give it a try.
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