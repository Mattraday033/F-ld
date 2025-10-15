VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR playerName = ""

VAR rubbleIndex = 1 
VAR tutorialColliderIndex = 2 
VAR rubbleIndex2 = 3 

deactivate({tutorialColliderIndex})

->1a

=== 1a ===

This rubble blocks your path, but with enough strength and effort it can be moved.

{
-strength >= 2:
    +Remove the rubble. <Str {strength}/2>
        ->1b
}
    +Leave the rubble alone.
        ->Close

=== 1b ===

fadeToBlack()

deactivate({rubbleIndex})
deactivate({rubbleIndex2})

fadeBackIn(60)

openGate()

You move the rubble. The path is now clear.
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