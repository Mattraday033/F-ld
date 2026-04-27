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
-finishedTaborIntimidateTutorial:
    ->1b
-else:
    ->1a
}

=== 1a ===

Don't come back until you've killed those bats.

    ->Close

=== 1b ===

activateQuestStep(Chief Tabor,Use the crank.)
setToTrue(startedTaborCunningTutorial)

With that out of the way, we can start working on the shack. The previous team laid these supports all wrong, and they're going to have to be ripped out before we can put them back up correctly.

Use this crank here to remove the parts of the wall that need removing. When you're done with that, meet me inside the hut.

fadeToBlack()

deactivate({taborIndex})
activate({taborNextIndex})

fadeBackIn(60)

    ->Close

=== Close ===

close()

->DONE