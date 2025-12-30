VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR revoltStarted = false
VAR convincedSlavesToHelpYou = false

VAR playerName = ""

    ->1a

=== 1a ===

These doors lead into the Manse. They are closed and barred.

{
-strength >= 4:
    +\*Break down the doors.*
        ->1b
-revoltStarted && convincedSlavesToHelpYou:
    +Alright everyone! Break down these doors!
        ->1c
}
    +Leave the doors alone.
        ->Close

=== 1b ===
fadeToBlack(true, false)

setToTrue(manseDoorsOpenedRevolt)

openGate()

fadeBackIn(60)

You break down the doors.

    ->Close

=== 1c ===

fadeToBlack(true, false)

setToTrue(manseDoorsOpenedRevolt)

openGate()

fadeBackIn(60)

The slaves surge forward and clear the door with pick and mattock.

    ->Close

=== Close ===

close()

->DONE