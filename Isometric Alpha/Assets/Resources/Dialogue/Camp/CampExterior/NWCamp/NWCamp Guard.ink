VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR guardIndex = 1
VAR taborIndex = 2
VAR taborAtAnnouncementIndex = 3

VAR skipTutorialIndex = 0
VAR intimidateTutorialIndex = 1
VAR cunningTutorialIndexIndex = 2

VAR toldByTaborToBuildHouses = false
VAR acceptedTaborSkillTutorial = false
VAR skippedTutorialInNWCamp = false
VAR gasparBroughtToExecution = false

VAR directorMentionedAnnouncement = false
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

State your business, branded.

    +The Director gave me orders to report here. I'm to be pardoned.
{
-gasparBroughtToExecution:
    ->3a
-else:
    ->2a
}

=== 2a ===

So you're the branded who cleared the mine. You're not much to look at. If we weren't so busy making sure your lot kept in line, we'd have done it ourselves.

->Close

=== 3a ===

So I've heard. *The guard shakes his head.* Gáspár's to be hanged and you get to go free? The whole world's gone upside down.

    +Are you going to let me past or not?
        ->3b
    +Take it up with the Director. Now let me by. 
        ->3b

=== 3b ===

And what if I don't? 

fadeToBlack(true, false)

activate({taborAtAnnouncementIndex})
changeCamTarget({taborAtAnnouncementIndex})
playAnimation({taborAtAnnouncementIndex},Idle_Front)

fadeBackIn(60)

Then you'll get a lashing for harrassing a free citizen.

changeCamTarget({guardIndex})
setNPCFacing({guardIndex},NE)

Chief! I uh... didn't see you there.

changeCamTarget({taborAtAnnouncementIndex})

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
playAnimation({taborAtAnnouncementIndex},OOC_Idle_Front)

fadeBackIn(60)

It is a confusing time for all of us. The Director has always been one to think laterally, but a pardon is a bit much for the others to swallow. I'm still trying to get my head around it myself.

Whether I agree with the Director's decision or not, I will admit your service was exemplary. And so, because you are unlikely to hear it from anyone else today... thank you. Your work saved many of the guards' lives, even if they will never acknowledge it.

    +I didn't do it for them. In fact, I almost wish I didn't now.
        I know you didn't, but that doesn't diminish what you did for them. It's worth recognizing all the same.
        ->3d
    +Er... you're welcome?
        \*Tabor chuckles.* It feels awkward for me as well, just so you know. But decorum is a virtue in of itself.
        ->3d
    +You're a strange one, Tabor. 
        As are you. Putting aside your talents, you're certainly the first branded I've ever corrected that survived the ordeal. I want to understand you better. What did we do that reached you? How can we better teach the other branded?
        ->3e

=== 3d ===

    +Your mix of courtesy and sadism is certainly unique, Chief. Can't say I'll miss it.
        ->Close
    +This charade is getting old. You expect me to believe your sincerity after I've seen how quick you are to use your whip?
        ->Close
    +It's bizarre to hear this from you, but I'll take whatever thanks I can get.
        ->Close

=== 3e ===

    +I've been here for less than a day. You're awfully full of yourself to take credit for anything I've done.
        ->Close
    +Your methods are barbaric. You will never reach an understanding with the other branded if you continue to degrade and abuse them as you do.
        ->Close

=== 3f ===

    +I won't 

=== Close ===

close()

->DONE