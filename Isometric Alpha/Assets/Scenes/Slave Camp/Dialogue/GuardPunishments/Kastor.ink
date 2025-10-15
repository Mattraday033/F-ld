VAR playerName = ""

VAR spokeWithNandorAfterPrisoners = false

VAR didNotExecuteMarcos = false
VAR gaveMarcosToTheCrowd = false
VAR gaveMarcosFiftyLashes = false
VAR executedMarcos = false

VAR canSpeakToKastorAboutFoodShortage = false
VAR spokeToKastorAboutFoodShortage = false
VAR kastorDiscussingFoodShortage = false

{
-canSpeakToKastorAboutFoodShortage and not spokeToKastorAboutFoodShortage:
    ->1b
-else:
    ->1a
}


=== 1a ===

{
- didNotExecuteMarcos:
Thank you for setting Márcos free. It will be much easier on him and the rest of us now that everyone seems to have started accepting his part in things.
- gaveMarcosToTheCrowd:
I would not have wished Márcos's fate on anyone, let alone the man who saved my life. You should hang your head in shame after what you did.
- executedMarcos:
Márcos's death is a travesty. How could you condemn him like that, after all he did for us?
- gaveMarcosFiftyLashes:
Of all the guards to hand a punishment, I think Márcos was the least worthy. But don't think I judge you for what you did.
-else:
Need something?

    +Will you vouch for any of the prisoners?
        I trust you'll do the right thing. Let Márcos go free.
        ->Close
    +I must be going.
        ->Close
}

->Close

=== 1b ===

Yes?

    +What needs to be done before the freed slaves are ready to leave the camp?
        swapInkFile(0, kastorDiscussingFoodShortage)
        ->Close
    +Nevermind. *Leave.*
        ->Close



=== Close ===

close()

->DONE