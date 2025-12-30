VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerName = ""
VAR heardTaborsLesson = false

->1a

=== 1a ===

{
-heardTaborsLesson:
You'd better get back where you're supposed to be, 'fore I reach for my whip.
-else:
Go bother someone else, I'm trying to watch the Chief at work.
}
    ->Close

=== Close ===

close()

->DONE