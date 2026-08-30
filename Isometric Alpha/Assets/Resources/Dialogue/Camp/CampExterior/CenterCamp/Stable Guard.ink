VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR guardIndex = 1

->1a

=== 1a ===

You slaves are always complaining about how much work you have. When we got here, this entire place was an overgrown ruin. We had to clear the entire camp of trees, brush and rocks, before we could even begin to build the camp walls.

Then, when all of that was done, we had to clear even more of the forest so we could see any assailants coming. Now that was work! And not a guard among us complained even once. Consider that the next time you want to moan about going in the mine again.

    +Have you ever had to work in the mine?
        ->1b
    +Who would assault a work camp in the heart of the confederation?
        Err... Don't worry about that. Get back to work.
        ->Close
    +I don't have time for this. *Leave.*
        ->Close
    
=== 1b ===

No. What does that have to do with anything?

    +Then how do you know which work is worse?
        \*The guard is silent for a moment.*

        setNPCFacing({guardIndex},SW)

        Get out of here, slave. Quit wasting my time.
        ->Close

=== Close ===

close()

->DONE