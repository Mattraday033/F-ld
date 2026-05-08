VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR taborIndex = 1
VAR weftIndex = 2
VAR guardIndex = 3
VAR previousTaborIndex = 4

VAR skippedTutorialInNWCamp = false
VAR kendeWillSellToPlayer = false
VAR situationStartedInNECamp = false

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

finishQuest(Chief Tabor,true,Work complete.)

deactivate({previousTaborIndex})

defeatMonster(NWCamp,0,true)
movePlayer(-9,0)
setFacing(NE)
activate({weftIndex})
activate({taborIndex})
//activate({guardIndex})
changeCamTarget({taborIndex})
setNPCFacing({taborIndex},SW)

slowFadeBackIn(2)
wait(1)
enableDialogueUI()

prepItem()

That was good work for this morning. The sun is high in the sky now, we'll be able to take our midday break soon.

addXP(200,1)

setToTrue(orderedIntoBodyPile)
activateQuestStep(Comb the Bodies,Enter the body pile.)

Before we do, however, we have a task we must complete that is a little less... comfortable.

The lockdown has only been going for a few days, but already one of the branded has tested our willingness to enforce it. He snuck out after sundown last night, but was caught by last night's watch.

He resisted arrest, and was killed. In the morning it was discovered that a ring of some value was burgled from the Director's household, but by then the guards that killed our only suspect had already thrown his body in the body pile.

You may be able to guess what our next task is: we have unfortunately been ordered to comb the body pile for the thief's body and check it for the ring. 

I waited until now to start our search because the body pile tends to attract scavengers, but the sun should be shining fully into the crevace where we dispose of the dead branded by now, so there will be fewer bats about. And I'll be coming with you, just in case.

The ladder down to the body pile is just to the west of us. All work is over the quicker it is begun, so let's get to it and afterwards we'll break for our midday meal.

->deactivateExtras

/*

I'll escort you down to the mess hall for lunch, and then we'll start our afternoon session.

setNPCFacing({guardIndex},NE)
changeCamTarget({guardIndex})

Chief Tabor! Captain Adéla sent a runner with a message for you! A situation is brewing in the camp's northeast section. Your presence is requested.

setToTrue(toldToGetMealByTabor)
setToTrue(situationStartedInNECamp)
changeCamTarget({taborIndex})

{
-kendeWillSellToPlayer:
->2a
}

Of course there is. I'd best go see what that is about. You two, go directly to the mess hall, and make no other stops. When you finish your meal, meet me in the northeast part of the camp. 

    +Where is the mess hall?
        ->1b

=== 1b ===

activateQuestStep(A Situation Brews,Midday meal.)

You did your work well today, I forgot you're new. It's the large building with the wooden roof in the southeast part of the camp. It's the one that opens up into a large yard with a well. Just head south from the center of camp and you can't miss it.

->deactivateExtras

=== 2a ===

activateQuestStep(A Situation Brews,Assess the situation.)

Of course there is. Looks like lunch will have to wait, Adéla wouldn't have sent for me if it wasn't urgent. I will run ahead and see what needs my attention. Meet me in the camp's northeast quarter and I'll put you to work on whatever has come up.

->deactivateExtras

*/

=== 1c ===

Don't come back until you've removed those boards.

->Close

=== deactivateExtras ===

fadeToBlack()

deactivate({taborIndex})
deactivate({guardIndex})
deactivate({weftIndex})

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE