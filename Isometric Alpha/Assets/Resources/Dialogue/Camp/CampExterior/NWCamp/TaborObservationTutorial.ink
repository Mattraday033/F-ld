VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR taborIndex = 1
VAR weftIndex = 2
VAR guardIndex = 3
VAR taborNextIndex = 4

VAR startedTaborIntimidateTutorial = false
VAR finishedTaborIntimidateTutorial = false

VAR startedTaborCunningTutorial = false
VAR finishedTaborCunningTutorial = false

VAR startedTaborLeadershipTutorial = false
VAR finishedTaborLeadershipTutorial = false

VAR startedTaborObservationTutorial = false
VAR finishedTaborObservationTutorial = false

VAR playerName = ""


{
-finishedTaborLeadershipTutorial:
    ->1b
-else:
    ->1a
}

=== 1a ===

Don't come back until you've cleared that rubble.

    ->Close

=== 1b ===

activateQuestStep(Chief Tabor,Remove the wall patch.)
setToTrue(startedTaborObservationTutorial)
setSecretDoorsObservable()

Now that the rubble has been removed, I've noticed that this patch holding up the back wall of the hut was shoddily applied. If we put the roof in and the patch doesn't hold, the entire back part of the hut will collapse.

Your next job is to remove the boards, but you will need to be clever about how you do it or you will knock over the beams that the wall patch is supporting. Be careful, and come find me when you're finished.

fadeToBlack()

deactivate({taborIndex})
activate({taborNextIndex})

fadeBackIn(60)

    ->Close

=== Close ===

close()

->DONE