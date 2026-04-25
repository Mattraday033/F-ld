VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerIndex = 0
VAR laszloIndex = 1
VAR weftIndex = 2

->1a

=== 1a ===

Did you deliver those rations to Weft like I asked?

    +Yes I did.
        Good. I think Weft is assigned to Chief Tabor today, so as his hutmate that means you are too. If you have no other tasks you can go out into the yard in the center of the camp and report to Chief Tabor.
        ->Close
    +Not yet. Where is his hut again?
        It's directly west from here, past the stables. It's the large hut with the thatched roof in front of the manse. Now don't come back until you've done what I told you to!
        ->Close

=== Close ===

close()

->DONE