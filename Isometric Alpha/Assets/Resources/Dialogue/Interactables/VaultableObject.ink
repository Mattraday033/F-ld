VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR size = 0
VAR plural = false
VAR dexDifficulty = 2
VAR objectName = ""

VAR playerName = ""

->1a

=== 1a ===

{
-plural:
These {objectName} look climbable. A sufficiently dexterous person could vault over them.
-else:
This {objectName} looks traversable. A sufficiently dexterous person could vault over it.
}


{
-dexterity >= dexDifficulty:
    +Vault over the {objectName}. <Dex {dexterity}/{dexDifficulty}>
        ->1b
    +Leave the {objectName} alone.
        ->Close
-else:
    +Leave the {objectName} alone. <Dex {dexterity}/{dexDifficulty}>
        ->Close
}


=== 1b ===

fadeToBlack()

adjustGridSquare({size})

fadeBackIn(60)

->Close

=== 1c ===

->Close

=== 1d ===

->Close

=== Close ===

close()

->DONE