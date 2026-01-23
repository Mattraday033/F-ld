VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR playerName = ""

VAR strDifficulty = 2

VAR gateKey = ""

VAR rubbleIndex = 1 

->1a

=== 1a ===

This pillar is about to give way. A strong enough push would cause it to tumble, providing a way forward.


    +Remove the rubble. <Str {strength}/{strDifficulty}>
    {
    -strength >= strDifficulty:
        ->1b
    -else:
        ->1c
    }
    +\*Leave.*
        ->Close

=== 1b ===

fadeToBlack(true, false)

openGate()

fadeBackIn(60)

You push with all your strength, sending the pillar crashing down. The path is now clear.
    ->Close

=== 1c ===

Your attempts are futile. You haven't the muscles to break the pillar.
    ->Close

=== Close ===

close()

->DONE