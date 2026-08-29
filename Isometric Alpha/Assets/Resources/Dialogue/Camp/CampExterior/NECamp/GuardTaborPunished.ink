VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerName = ""

VAR playerIndex = 0
VAR guardIndex = 1

->1a

=== 1a ===

setNPCFacing({guardIndex},NW)

\*The guard pays you no mind, and mutters to himself.* Stupid Tabor. Who hasn't gotten a little carried away while laying into a slave? Chief 'Holier-Than-Thou' should be docking everyone's rations, not just mine!
        ->Close

=== Close ===

close()

->DONE