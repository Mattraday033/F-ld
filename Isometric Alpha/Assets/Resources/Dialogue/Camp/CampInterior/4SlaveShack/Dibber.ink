
VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerName = ""

VAR dibberIndex = 1
VAR dibberSafeIndex = 2
VAR kastorIndex = 3
VAR kastorWisIndex = 4
VAR kastorByDibberIndex = 5
VAR thatchIndex = 6

VAR savedDibber = false

VAR saveDibberQuestName = "Save Dibber!"

{
-savedDibber:
    ->2a
-else:
    ->1a
}

=== 1a ===

setToTrue(savedDibber)
setToTrue(finishedKastorObservationTutorial)
setToFalse(duringKastorSkillTutorial)

\*Dibber lies on the ground, barely moving. His breaths are labored and shallow. His face is streaked with blood from a heavily scabbed wound across his temple.*

    +Kastor, get in here! Dibber's alive but he's badly hurt!
        ->1b

=== 1b ===

finishQuest(Save Dibber!, true, Dibber is safe.)

fadeToBlack(true, false)

deactivate({kastorWisIndex})
activate({kastorByDibberIndex})
movePlayerPos(5,17)

fadeBackIn(60)

changeCamTarget({kastorByDibberIndex})

His breathing is poor, but not obstructed. His neck seems fine, as far as I can tell, but obviously his head wound is problematic. Normally, I'd want to move him as little as possible in a state like this but I can't rule out the rest of the roof collapsing in on him again.

setNPCFacing({kastorByDibberIndex},SW)

There's nothing for it, we'll have to carry him out of here. I'll grab his top half, you get his legs. I'll show you how to carry him so we twist him as little as possible.

fadeToBlack(true, false)

deactivate({kastorByDibberIndex})
deactivate({dibberIndex})
movePlayerPos(9,14)
activate({kastorIndex})
activate({dibberSafeIndex})
activate({thatchIndex})
changeCamTarget({kastorIndex})

fadeBackIn(60)

getNewDialogueFromList(4SlaveShackKastor,false,justSavedDibber)

->Close

=== 2a ===

\*Dibber is sleeping soundly.*

    +\*Leave*
        ->Close

=== Close ===

close()

->DONE