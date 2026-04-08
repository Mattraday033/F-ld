VAR observationLevel = 0

VAR observationDifficulty = 2

VAR hasOfficeKeyFrontHalf = false
VAR hasOfficeKeyBackHalf = false

VAR wisDifficulty = 2

VAR description = "*The wall looks formidable, but you can feel a slight draft.*"

VAR secretDoorKey = ""

VAR playerName = ""

->1a

=== 1a ===

searchInventoryFor(hasOfficeKeyFrontHalf,Key,2)
searchInventoryFor(hasOfficeKeyBackHalf,Key,3)

The wall looks formidable, but you can feel a slight draft. 


    +Search around for anything suspicious. <Observation {observationLevel}/{observationDifficulty}>
        {
        -observationLevel >= observationDifficulty:
            ->1c
        -else:
            ->1b
        }

    +Leave the wall alone.
        ->Close

=== 1b ===

\*You find nothing interesting.*

    +\*Leave.*
        ->Close

=== 1c ===

You search around for anything that catches your eye, and notice that between two of the bricks there is a section of mortar missing in the shape of a keyhole.

{
-hasOfficeKeyFrontHalf and hasOfficeKeyBackHalf:
    +Combine both key halfs and open the door.
        ->1d
}
    +Don't touch anything.
        ->Close

=== 1d ===


fadeToBlack(true, false)

//openGate()
addSecretDoorFlag({secretDoorKey})
finishQuest(Delving Deeper, true, Second passage found.)

fadeBackIn(60)

setToTrue(manseOfficeSecretDoorsOpened)
setToTrue(manseHiddenStairsFound)

You insert the key into the lock and turn it. The wall is pulled to the side by a hidden mechanism.
    ->Close
    
=== Close ===

close()

->DONE