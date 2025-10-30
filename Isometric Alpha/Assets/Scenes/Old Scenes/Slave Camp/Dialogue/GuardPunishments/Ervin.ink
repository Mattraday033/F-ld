VAR spokeToErvin = false
VAR askedErvinAboutBrand = false
VAR gotThePlanFromKastor = false
VAR gavePasswordToErvin = false
VAR givenTaskByErvin = false
VAR convincedImre = false
VAR terrifiedImre = false
VAR deathFlagImre = false
VAR imreWontSpeakToPlayer = false
VAR finishedErvinsTask = false

VAR guardPazmanAndRekaAtTrial = false
VAR rekaNeedsHandling = false
VAR pazmanNeedsHandling = false

VAR playerName = ""

//changeCamTarget(int targetIndex)
//keepDialogue()
//setToTrue(string flagName)
//setToFalse(string flagName)

->1a

=== 1a ===

You did well. Thank you.

{
-guardPazmanAndRekaAtTrial and pazmanNeedsHandling:
    +Are there any prisoners you would vouch for?
        ->1b
    +I must be going.
        ->Close
}

->Close

=== 1b ===
        
\*Ervin's eyes narrow.* None. But Pazman was one who gave me brand. Kill him. Or let me do it.

    +I promise you, justice will be served.
        \*Ervin nods.*
        ->Close
    +Why did he brand you?
        ->1c

=== 1c ===

I witnessed him mistreat another slave. Guards not allowed to harrass no-brands. Lovashi laws say no-brands have protections.

He branded me to keep me from telling. Tried to silence me. He failed.

    +I'll give it some thought.
        ->Close
    +He'll get what he deserves.
        ->Close

=== Close ===

close()

->DONE