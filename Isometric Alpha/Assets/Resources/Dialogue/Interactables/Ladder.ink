VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR description = ""
VAR destinationName = ""

VAR dexDifficulty = 2

VAR playerName = ""

->1a

=== 1a ===

{description}

{
-dexDifficulty <= 1:
    +\*Climb the ladder*
        ->1b
    +\*Leave.*
        ->Close
-dexterity >= dexDifficulty:
    +\*Climb the ladder* <Dex {dexterity}/{dexDifficulty}>
        ->1b
    +\*Leave.*
        ->Close
        
-else:

    +\*Leave.* <Dex {dexterity}/{dexDifficulty}>
        ->Close
}

=== 1b ===

You successfully climb the ladder.

changeLocation({destinationName})
    
    ->Close

=== Close ===

close()

->DONE