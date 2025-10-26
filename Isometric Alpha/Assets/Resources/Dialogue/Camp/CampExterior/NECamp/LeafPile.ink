VAR givenTaskByBalint = false
VAR gotLeavesForBalint = false

{ 
-gotLeavesForBalint:
    ->1a
-givenTaskByBalint:
    ->2a
-else:
    ->1a
}

=== 1a ===

A pile of leaves that have been blown over the wall by the wind.

    ->Close

=== 2a ===

A pile of leaves that have been blown over the wall by the wind.

    +Take a few of the green ones.
        ->2b
    +Leave them alone.    
        ->Close

=== 2b ===

setToTrue(gotLeavesForBalint)

activateQuestStep(Aiding Bálint, 2)

prepForItem()

You take a few of the leaves.

giveItem(3,0,1)

    ->Close


=== Close ===

close()

->DONE
