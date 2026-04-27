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

VAR startedTaborObservationTutorial = false
VAR finishedTaborObservationTutorial = false

VAR startedTaborLeadershipTutorial = false
VAR finishedTaborLeadershipTutorial = false

VAR playerName = ""


{
-finishedTaborCunningTutorial:
    ->1b
-else:
    ->1a
}

=== 1a ===

Don't come back until you've used that crank.

    ->Close

=== 1b ===

activateQuestStep(Chief Tabor,Clear the rubble.)
setToTrue(startedTaborLeadershipTutorial)

While you were dealing with that crank, I was inspecting the rest of the site. The previous team put up this wall wrong, and it's begun to crumble. Removing the rubble looks to me like a two person job.

I've marked the places you and another branded should stand to get the best leverage to clear the rubble. Just have each of you stand in the proper spots and the job should go smoothly.

fadeToBlack()

deactivate({taborIndex})
activate({taborNextIndex})

fadeBackIn(60)

    ->Close

=== Close ===

close()

->DONE