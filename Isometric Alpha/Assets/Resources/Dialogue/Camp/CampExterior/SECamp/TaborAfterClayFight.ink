VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR taborIndex = 1

VAR gaveAGuardToTheCrowd = false
VAR foughtCrowdForTabor = false


VAR playerName = ""

->1a


=== 1a ===

setToTrue(foughtCrowdForTabor)

changeCamTarget({taborIndex})

\*Tabor wriggles back to his position in the lineup from where he hid during the fighting.* You killed them! You killed your fellow slaves to save my life. I don't understand. Why would you do that?

{
-not gaveAGuardToTheCrowd:
    +What they would have done to you... No one deserves that.
        ->swapInkFile4
}
    +I tried to prevent this, but they were too stubborn.
        ->swapInkFile3
    +I actually thought they were bluffing.
        ->swapInkFile2
    +\*Wipe the blood from your weapon.* Still think this is a ruse?
        ->swapInkFile1

=== swapInkFile1 ===

swapInkFile(0, fromFightDialogueFlag1)

->Close

=== swapInkFile2 ===

swapInkFile(0, fromFightDialogueFlag2)

->Close

=== swapInkFile3 ===

swapInkFile(0, fromFightDialogueFlag3)

->Close

=== swapInkFile4 ===

swapInkFile(0, fromFightDialogueFlag4)

->Close


=== Close ===

close()

->DONE