VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR guardIndex = 1
VAR taborIndex = 2
VAR taborAtAnnouncementIndex = 3
VAR directorIndex = 4
VAR adelaIndex = 5
VAR crowdIndex = 6

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
        ->readyCheck_1b
    +\*Leave.*
        ->Close

=== readyCheck_1b ===

setToTrue(startedDirectorAnnouncement)

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
    +I've taken on worms five times your size. You'll apologize if you know what's good for you.
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

VAR doneGreatService = "You have done the Confederation a worthy service and for that the Director has pardoned you, but in a way, I see it as you've graduated from this camp."
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
    +I'm proof that the brand should not exist. If the branded were not consigned to die, they more easily reveal to you they are worthy of life.
        Perhaps that is true. I'm not convinced it doesn't have its purposes, but there are so many branded. Your unceasing numbers bode ill for the future of the Confederation.
        ->keepingDirectorWaiting_1a
    +I've been here for less than a day. You're awfully full of yourself to take credit for anything I've done.
        You have a point. The teacher could use his own lesson in humility, then.
        ->keepingDirectorWaiting_1a
    +I'm done with this conversation. Goodbye Tabor.
        ->keepingDirectorWaiting_1b

=== keepingDirectorWaiting_1a ===

But we are keeping the Director waiting. It would be best if we move along.

->proceedToDirectorSpeech

=== keepingDirectorWaiting_1b ===

Farewell, {playerName}. I expect not to see your like again.

->proceedToDirectorSpeech

=== proceedToDirectorSpeech ===

fadeToBlack(true, false)

deactivate({taborIndex})
movePlayerPos(-9,14)
setFacing(SE)
changeCamTarget({directorIndex})

fadeBackIn(60)

\*The Director clears his throat.*

changeCamTarget({adelaIndex})

Listen up! The Director speaks!

changeCamTarget({crowdIndex})

\*The gathered guards quiet down.*

changeCamTarget{directorIndex}



->Close

=== Close ===

close()

->DONE