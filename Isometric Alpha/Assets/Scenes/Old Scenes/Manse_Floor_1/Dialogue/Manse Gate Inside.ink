VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR kastorStartedRevolt = false

VAR playerName = ""



    ->1a

=== 1a ===

These doors lead out into the camp.

    +Open the doors.
        ->1b
    +Leave the doors alone.
        ->Close

=== 1b ===

fadeToBlack()

setToTrue(manseDoorsOpenedRevolt)

openGate()

fadeBackIn(60)

You open the doors.

    ->Close

=== Close ===

close()

->DONE