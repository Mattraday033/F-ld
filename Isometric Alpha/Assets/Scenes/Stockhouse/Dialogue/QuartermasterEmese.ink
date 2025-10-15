VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR snitchedOnUros = false
VAR hasIronNugget = false
VAR metQuartermasterEmese = false
VAR gaveIronNuggetToEmese = false

VAR playerName = ""

//changeCamTarget(int targetIndex)
//keepDialogue()
//setToTrue(string flagName)
//setToFalse(string flagName)
//activate(int index of gameobject you're activating)
//deactivate(int index of gameobject you're deactivating)
//activateQuestStep(string questTitle, int questStepIndex)
//prepForItem() //used before giveItem/giveItems/takeAllOfItem to add obtained/removed text after next line
//giveItem(int listIndex, int itemIndex, int quantity)
//giveItems(int listIndex1, int itemIndex1, int quantity1 |
//          int listIndex2, int itemIndex2, int quantity2 |
//          ... etc)
//takeAllOfItem(string itemName)
//activateQuestStep(string fullTitleOfQuestFoundInQuestJsonFile,int questStepIndex)
//searchInventoryFor(string nameOfVarSetToTrueInsideInkFile,string itemNameToSearchFor)
//fadeToBlack()
//fadeBackIn(int numberOfFramesToWaitBeforeFadingBackIn)
//moveToLocalPos(float xCoord,float yCoord)

{
-snitchedOnUros and not gaveIronNuggetToEmese:
->1e
-else:
    {
    -metQuartermasterEmese:
        ->1d
    -else:
        ->1a
    }
}

=== 1a ===

Hello slave. Got an order for me?

    +No ma'am
        ->1b

=== 1b ===
~metQuartermasterEmese = true
setToTrue(metQuartermasterEmese)

Then why are you bothering me?

    +I'm looking for Guard László. He is having me run an errand for him. *Show badge*
        ->1c

=== 1c ===

I haven't seen him, but you should check the guard hut near the main entrance. That's where he is posted when he doesn't have duties to attend to.

    +How is the lockdown treating you?
        keepDialogue()
        The lockdown? I barely notice it. Supply tallies and requisitions don't stop just because the slaves aren't working. The biggest difference is now that the branded are stuck inside all day they're begging for stockhouse duty just to have something to do. I've never had so many volunteers! *Emese chuckles to herself.*
        ->1c
    +If I need supplies, can I get them from you?
        keepDialogue()
        Not without a guard's approval you can't. It would be best if you didn't concern yourself with such things and get back to work.
        ->1c
    +Can you remind me where I can find Guard László? I'm looking for him.
        keepDialogue()
        I haven't seen him, but you should check the guard hut near the main entrance. That's where he is posted when he doesn't have duties to attend to.
        ->1c
    +Thank you for your time.
        ->Close
    
=== 1d ===

keepDialogue()

You're back. Did you need something?
    ->1c

=== 1e ===
searchJunkFor(hasIronNugget,Iron Nugget)

Did you find whatever Uros hid in here?

{
-hasIronNugget:
    +Yes, here you go. *Hand the Iron Nugget to Emese.*
        ->1f
}
    +No, not yet.
        keepDialogue()
        When you do, let me know.
        ->1c


=== 1f ===

~gaveIronNuggetToEmese = true

setToTrue(gaveIronNuggetToEmese)

finishQuest(Stockhouse Stash, true, 10)

prepForItem()

\*Quartermaster Emese examines the Iron Nugget.* Iron? Very interesting. When I tell the Director about this, Uros will be interogated to the fullest extent to find where he got it and if there's any more where it came from. You've done an excellent job.

takeJunk(Iron Nugget, 1)&
addXP(100)

\*Quartermaster Emese rummages around in a crate she keeps below her desk.*

prepForItem()

These gloves were meant to be distributed among the guards, but I think you've earned them. They should help keep blisters off your hands after a hard day of swinging a pick. If any guard or slave tries to take them, tell them I gave them to you and they'll be answering to me if they wind up stolen.

giveItem(2,5,1)

->Close

=== 1g ===

->Close

=== 1h ===

->Close

=== 1i ===

->Close

=== 1j ===

->Close

=== 1k ===

->Close

=== 1l ===

->Close

=== 1m ===

->Close

=== 1n ===

->Close

=== 2a === 

->Close
    
=== 2b ===

->Close

=== 2c ===

->Close

=== 2d ===

->Close

=== 2e ===

->Close

=== 2f ===

->Close

=== 2h ===

->Close

=== 2i ===

->Close

=== 2j ===

->Close

=== 2k ===

->Close

=== 2l ===

->Close

=== 2m ===

->Close

=== 2n ===

->Close

=== 3a ===

->Close

=== 3b ===

->Close

=== Close ===

close()

->DONE