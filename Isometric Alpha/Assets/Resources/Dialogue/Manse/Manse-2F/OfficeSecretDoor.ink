VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

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


    +Search around for anything suspicious. <Wis {wisdom}/{wisDifficulty}>
        {
        -wisdom >=wisDifficulty:
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

openGate()
addSecretDoorFlag({secretDoorKey})

fadeBackIn(60)

setToTrue(manseOfficeSecretDoorsOpened)
setToTrue(manseHiddenStairsFound)

You insert the key into the lock and turn it. The wall is pulled to the side by a hidden mechanism.
    ->Close
    
=== Close ===

close()

->DONE