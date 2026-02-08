VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR wisDifficulty = 2

VAR description = "*The wall looks formidable, but you can feel a slight draft.*"

VAR secretDoorKey = ""

VAR playerName = ""

->1a

=== 1a ===

{description}

    +Search around for anything suspicious. <Wis {wisdom}/{wisDifficulty}>
    
    {
    -wisdom >= wisDifficulty:
        ->1b
    -else:
        ->1c
    }

    +\*Leave the wall alone.*
        ->Close

=== 1b ===

\*You search around for anything that catches your eye, and notice a loose section.*

    +\*Push it inwards.*
        ->1d
    +\*Don't touch anything.*
        ->Close

=== 1c ===

\*You find nothing interesting.*

    +\*Leave.*
        ->Close

=== 1d ===

fadeToBlack(true, false)

addSecretDoorFlag({secretDoorKey})

fadeBackIn(60)

\*Applying pressure, you hear a soft click. Suddenly, the wall moves aside.*

    ->Close
    
=== Close ===

close()

->DONE