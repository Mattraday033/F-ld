VAR observationLevel = 0

VAR observationDifficulty = 2

VAR description = "*The wall looks formidable, but you can feel a slight draft.*"
VAR searchChoice = "*Search around for anything suspicious.*"
VAR successDescription = "*You search around for anything that catches your eye, and notice a loose section.*"
VAR successChoice = "*Push it inwards.*"
VAR failureDescription = "*You find nothing interesting.*"
VAR openDescription = "*Applying pressure, you hear a soft click. Suddenly, the wall moves aside.*"

VAR addHostilityIfOutside = false

VAR completeQuest = false
VAR questName = ""
VAR questStepName = ""

VAR secretDoorKey = ""

VAR playerName = ""

->1a

=== 1a ===

{description}

    +\*{searchChoice} <Observation {observationLevel}/{observationDifficulty}>
    
    {
    -observationLevel >= observationDifficulty:
        ->1b
    -else:
        ->1c
    }

    +\*Leave the wall alone.*
        ->Close

=== 1b ===

{successDescription}

    +\*{successChoice}
        ->1d
    +\*Don't touch anything.*
        ->Close

=== 1c ===

{failureDescription}

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

{openDescription}

    ->Close
    
=== Close ===

close()

->DONE