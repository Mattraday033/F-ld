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

All you branded make me sick. I wish the Director would order us to toss the rest of you down the ravine and go home. It's what the lot of you deserve.

{
    -strength >= 2:
    +Fortunately for you, I have a cure for that. *Push the guard over the side of the ravine.* <Str {strength}/2>
        setToTrue(madeRavineGuardTripAttempt)
        {
            -strength >= 2:
                ->1b
            -else:
                ->1c
        }
}

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

What are y- AAAAAAAAAAHHH!!!

->deactivateGuard

=== 1c ===

addHostilitytoCurrentArea()

\*You can't find a way to catch the guard by surprise.*

You've got some lip on you, branded. I'm off to tell Kende not to give you any rations for a few days. We'll see if you're still mouthy after that.

->deactivateGuard

=== deactivateGuard ===

fadeToBlack()

deactivate({guardIndex})
wait(1)

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE
