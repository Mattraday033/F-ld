VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerName = ""

VAR spokeToTaborAtBeginningOfSituation = false

{
-spokeToTaborAtBeginningOfSituation:
->1b
-else:
->1a
}

=== 1a ===

getNewDialogueFromList(NECampChief Tabor,spokeToAdelaFirst,true)

    ->Close

=== 1b ===

Let's see how you do, slave. *Adéla chuckles.* Just don't get yourself killed, 'cause my guards won't be quick enough to save you.

    ->Close

=== Close ===

close()

->DONE