VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR askedImreToLeadTheWay = false

VAR playerName = ""

    ->1a

=== 1a ===

{
-askedImreToLeadTheWay:

These doors provide access to the Manse's kitchens. They are unbarred, and stand slightly ajar.

-else:

These doors provide access to the Manse's kitchens. They are closed and barred.

}


{
-askedImreToLeadTheWay:
    +Proceed through the door.
        fadeToBlack()

        openGate()

        fadeBackIn(60)

        ->Close
}
    +Leave the doors alone.
        ->Close

=== Close ===

close()

->DONE