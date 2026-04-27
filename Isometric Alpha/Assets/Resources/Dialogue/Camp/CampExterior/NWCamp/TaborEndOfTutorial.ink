VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR taborIndex = 1
VAR weftIndex = 2
VAR guardIndex = 3
VAR previousTaborIndex = 4

VAR skippedTutorialInNWCamp = false

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
-skippedTutorialInNWCamp or finishedTaborObservationTutorial:
->1a
-else:
->1c
}

=== 1a ===

disableDialogueUI()
manualFadeToBlack()
stopAllFades()

setTutorialToSeen(intimidateTutorialSequenceEntered)
setTutorialToSeen(secondCunningTutorialSequenceEntered)
setTutorialToSeen(observationTutorialSequenceEntered)
setTutorialToSeen(leadershipTutorialSequenceEntered)

deactivate({previousTaborIndex})

defeatMonster(NWCamp,0,true)
movePlayer(-9,0)
setFacing(NE)
activate({weftIndex})
activate({taborIndex})
activate({guardIndex})
changeCamTarget({taborIndex})
setNPCFacing({taborIndex},SW)

slowFadeBackIn(2)
wait(1)
enableDialogueUI()

That was good work for this morning. I'll escort you down to the mess hall for lunch, and then we'll start our afternoon session.

setNPCFacing({guardIndex},NE)
changeCamTarget({guardIndex})

Chief Tabor! Captain Adéla sent a runner with a message for you! A situation is brewing in the northeast section of the camp. Your presence is requested.

setToTrue(toldToGetMealByTabor)
changeCamTarget({taborIndex})

Of course there is. I'd best go see what that is about. You two, go directly to the mess hall, and make no other stops. When you finish your meal, meet me in the northeast part of the camp. 

    +Where is the mess hall?
        ->1b

=== 1b ===

You did your work well today, I forgot you're new. It's the large building with the wooden roof in the southeast part of the camp. It's the one that opens up into a large yard with a well. Just head south from the center of camp and you can't miss it.

fadeToBlack()

activateQuestStep(Chief Tabor,To the mess hall.)

deactivate({taborIndex})
deactivate({guardIndex})
deactivate({weftIndex})

fadeBackIn(60)

->Close

=== 1c ===

Don't come back until you've removed those boards.

->Close

=== Close ===

close()

->DONE