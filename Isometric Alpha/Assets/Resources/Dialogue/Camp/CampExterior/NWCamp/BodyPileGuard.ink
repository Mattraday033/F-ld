VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerIndex = 0
VAR guardIndex = 1

VAR madeRavineGuardTripAttempt = false
VAR succeededRavineGuardTripAttempt = false

->1a

=== 1a ===

All you branded make me sick. Wish the Director'd just order us to toss rest of you down the ravine and go home. It's what the lot of you deserve.

    +After you. *Trip the guard into the ravine* <Dex {dexterity}/2>
        setToTrue(madeRavineGuardTripAttempt)
        {
            -dexterity >= 2:
                ->1b
            -else:
                ->1c
        }
    +\*Ignore the guard and leave.*
        ->Close

=== 1b ===

setToTrue(succeededRavineGuardTripAttempt)

->Close

=== 1c ===



->Close

=== Close ===

close()

->DONE
