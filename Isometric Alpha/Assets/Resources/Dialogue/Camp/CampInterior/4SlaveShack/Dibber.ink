
VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerName = ""

VAR dibberIndex = 1
VAR dibberSafeIndex = 2
VAR kastorIndex = 3
VAR kastorWisIndex = 4

VAR savedDibber = false

VAR saveDibberQuestName = "Save Dibber!"

{
-savedDibber:
    ->2a
-else:
    ->1a
}

=== 1a ===

Thanks for saving me!

    +Neat
        ->Close

=== 2a ===

Being safe is great!

    +Neat
        ->Close

=== Close ===

close()

->DONE