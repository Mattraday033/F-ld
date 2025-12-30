VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR hasOfficeKeyFrontHalf = false
VAR hasOfficeKeyBackHalf = false

VAR playerName = ""


->1a

=== 1a ===

searchInventoryFor(hasOfficeKeyFrontHalf,Key,2)
searchInventoryFor(hasOfficeKeyBackHalf,Key,3)

//hasOfficeKeyFrontHalf = {hasOfficeKeyFrontHalf}
//hasOfficeKeyBackHalf = {hasOfficeKeyBackHalf}

This locked door is made of solid wood reinforced with bronze. It would take ages to hack through it. 

{
-hasOfficeKeyFrontHalf and hasOfficeKeyBackHalf:
    +Combine both key halfs and open the door.
        ->1b
}

    +Leave the door alone.
        ->Close

=== 1b ===

fadeToBlack(true, false)

openGate()

fadeBackIn(60)

You insert the key into the lock and unlock the door.
    ->Close


=== 1c ===

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