VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR speakerIndex = 1
VAR barricadeParentIndex = 2

VAR defeatFlag = ""

VAR barricadeGuardDeathFlagNameIndex = 2

VAR withBarricadeFightIndex = 0
VAR withoutBarricadeFightIndex = 1

VAR playerName = ""

VAR facingNE = false
VAR facingNW = false
VAR facingSW = false
VAR facingSE = false

{
-facingNE:
    ->1a
-else:
    ->1b
}

->1a

=== 1a ===

Halt, slave! Riot control is in effect. Any slave that is found in their huts without a weapon after order is restored to the camp will be pardoned. Anyone who continues to resist will be put to death. What say you?

    +I'm getting through this barricade whether you man it or not. For freedom! <Attack>
        ->combat(withBarricadeFightIndex)
    +\*Leave without fighting.*
        ->Close

=== 1b ===

Blast, the rioters got behind us! To arms!

->combat(withoutBarricadeFightIndex)
    
=== combat(fightIndex) ===

setToTrue({defeatFlag})
enterCombat({fightIndex})

->Close

=== Close ===

close()

->DONE