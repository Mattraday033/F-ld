VAR observationLevel = 0

VAR observationDifficulty = 2

VAR description = "*The wall looks formidable, but you can feel a slight draft.*"

VAR addHostilityIfOutside = false

VAR completeQuest = false
VAR questName = ""
VAR questStepName = ""

VAR secretDoorKey = ""

VAR playerName = ""

->1a

=== 1a ===

{description}

    +Search around for anything suspicious. <Observation {observationLevel}/{observationDifficulty}>
    
    {
    -observationLevel >= observationDifficulty:
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

{
-addHostilityIfOutside:
    addHostilityToCurrentArea()
}

{
-questName != "" && questStepName != "":
    {
    -completeQuest:
        finishQuest({questName}, true, {questStepName})
    -else:
        activateQuestStep({questName},{questStepName})
    }
}

fadeBackIn(60)

\*Applying pressure, you hear a soft click. Suddenly, the wall moves aside.*

    ->Close
    
=== Close ===

close()

->DONE