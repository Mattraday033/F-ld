VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR playerName = ""

VAR strengthRequirement = 2

VAR gateKey = ""

VAR rubbleIndex = 1 

->1a

=== 1a ===

This pillar is about to give way. A strong enough push would cause it to tumble, providing a way forward.


    +Remove the rubble. <Str {strength}/{strengthRequirement}>
    {
    -strength >= strengthRequirement:
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