VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR startledUros = false
VAR gotIronNuggetFromBarrels = false
VAR convincedUros = false
VAR intimidatedUros = false
VAR threatenedToSnitchOnUros = false
VAR snitchedOnUros = false

VAR treasureItemListIndex = 5
VAR lostIronNuggetIndex = 3

VAR playerName = ""

->1a

=== 1a ===

{
-not gotIronNuggetFromBarrels:

setToTrue(gotIronNuggetFromBarrels)

prepForItem()

Searching behind some barrels, you find a small scrap of iron.

{
-startledUros:
    {
    -convincedUros or intimidatedUros or threatenedToSnitchOnUros:
    activateQuestStep(Stockhouse Stash, Lost and found.1)
    -snitchedOnUros:
    activateQuestStep(Stockhouse Stash, Lost and found.2)
    -else:
    activateQuestStep(Stockhouse Stash, Lost and found.)
    }
-else:
    activateQuestStep(Stockhouse Stash, I found some iron)
}

giveItem({treasureItemListIndex},{lostIronNuggetIndex},1)

    ->Close
    
-else:
Searching behind some barrels, you find nothing of interest.

->Close
}



=== Close ===

close()

->DONE