VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR guardIndex = 1
VAR taborIndex = 2

VAR skipTutorialIndex = 0
VAR intimidateTutorialIndex = 1
VAR cunningTutorialIndexIndex = 2

VAR toldByTaborToBuildHouses = false
VAR acceptedTaborSkillTutorial = false

VAR playerName = ""

->1a

=== 1a ===

changeCamTarget({guardIndex})

You're not allowed in here. Head back the way you came.

{
-toldByTaborToBuildHouses:
    +I'm under orders from Chief Tabor to report to this area.
        ->1b
    +\*Leave.*
        ->Close
-else:
    +\*Leave.*
        ->Close
}

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

getNewDialogueFromList(NWCampChief Tabor,true,skippedTutorialInNWCamp)

->Close

=== Close ===

close()

->DONE