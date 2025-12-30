VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR description = ""
VAR destinationName = ""

VAR playerName = ""

->1a

=== 1a ===

{description}

{
-dexterity >= 2:
    +\*Climb the ladder* <Dex {dexterity}/2>
        ->1b
    +\*Leave.*
        ->Close
        
-else:

    +\*Leave.* <Dex {dexterity}/2>
        ->Close
}

=== 1b ===

You successfully climb the ladder.

changeLocation({destinationName})
    
    ->Close

=== Close ===

close()

->DONE