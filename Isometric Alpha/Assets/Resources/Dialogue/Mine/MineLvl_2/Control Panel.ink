VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR mineLvl2GateToLevel3Opened = false
VAR hasWinch = false

VAR gateKey = "Ancient Portcullis"

VAR playerName = ""


{
-mineLvl2GateToLevel3Opened:
    ->1c
-else:
    ->1a
}

=== 1a ===

searchInventoryFor(hasWinch,Winch)

This panel once controlled the gate. An empty slot indicates it to be missing some mechanism integral to it's function.

{
-hasWinch:
    +Insert the winch into the slot.
        ->1b
    +\*Leave.*
        ->Close
-else:
    +\*Leave.*
        ->Close
}

=== 1b ===

fadeToBlack(true, false)

setToTrue(mineLvl2GateToLevel3Opened)

openGateFromKey({gateKey})

fadeBackIn(60)

You insert the winch and begin to crank. After some effort, the portculis begins to rise. You then remove the winch and place it back among your belongings.

->Close

=== 1c ===

The gate is raised. The panel lacks any other purpose.

->Close
    
=== 1d ===

->Close

=== 1e ===

->Close

=== 1f ===

->Close

=== 1g ===

->Close

=== 1h ===

->Close

=== 1i ===

->Close

=== 1j ===

->Close

=== 1k ===

->Close

=== 1l ===

->Close

=== 1m ===

->Close

=== 1n ===

->Close

=== 2a === 

->Close
    
=== 2b ===

->Close

=== 2c ===

->Close

=== 2d ===

->Close

=== 2e ===

->Close

=== 2f ===

->Close

=== 2h ===

->Close

=== 2i ===

->Close

=== 2j ===

->Close

=== 2k ===

->Close

=== 2l ===

->Close

=== 2m ===

->Close

=== 2n ===

->Close

=== 3a ===

->Close

=== 3b ===

->Close

=== Close ===

close()

->DONE