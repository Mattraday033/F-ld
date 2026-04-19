VAR hostileAreaName = ""
VAR hostilityScriptKey = ""

VAR fadeOutAndIn = true

VAR hasKey = false

VAR keyName = ""

VAR description = ""
VAR gateKey = ""

{
-keyName != "":
searchInventoryFor(hasKey,{keyName})
    ->1a
-else:
    ->1a
}

=== 1a ===

{
-description != "":
    {description}
-else:
    \*This gate is locked. You will need a key to get through it.*
}

{
-hasKey:
    +\*Open the gate.* <{keyName}>
        {
        -hostilityScriptKey != "":
            ->1c
        -else:
            ->1b
        }
    +\*Leave.*
        ->Close
-else:
    +\*Leave.*
        ->Close
}

=== 1b ===

fadeToBlack(true, false)

openGate()

{
-hostilityScriptKey != "":
    activateHostilityScript({hostilityScriptKey})
}

fadeBackIn(60)

\*The gate swings open.*

->Close

=== 1c ===

\*Unlocking this gate will turn {hostileAreaName} hostile. This will affect the outcome of certain quests, cause enemies to attack you in previously safe areas, and may be irreversible. Are you certain you wish to proceed?*

    +\*Open the gate anyways.* <{keyName}>
        activateHostilityScript({hostilityScriptKey})
        ->Close
    +\*Leave.*
        ->Close

->Close

=== Close ===

close()

->DONE