VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR insultedKendesCooking = false
VAR kendeWillSellToPlayer = false
VAR convincedImre = false
VAR terrifiedImre = false
VAR acceptingGuardPrisoners = false

VAR guardSurrendered = false
VAR convincedNonbrandedToHelp = false

VAR kendeIndex = 1
VAR loyalImreIndex = 2
VAR disloyalImreIndex = 3
VAR panIndex = 4
VAR guardIndex = 5
VAR slaveIndex = 6

VAR halfSlavesNoGuardFightIndex = 0
VAR halfSlavesFightIndex = 1
VAR fullSlavesNoGuardFightIndex = 2
VAR fullSlavesFightIndex = 3

VAR playerIndex = 0
VAR playerName = ""

//changeCamTarget(int targetIndex)
//keepDialogue()
//setToTrue(string flagName)
//setToFalse(string flagName)
//activate(int index of gameobject you're activating)
//deactivate(int index of gameobject you're deactivating)
//activateQuestStep(string questTitle, int questStepIndex)
//prepForItem() 
//giveItem(int listIndex, int itemIndex, int quantity)
//giveItems(int listIndex1, int itemIndex1, int quantity1 |
//          int listIndex2, int itemIndex2, int quantity2 |
//          ... etc)
//takeAllOfItem(string itemName)

{
-convincedImre:
    ->1a
-not convincedImre && terrifiedImre:
    ->2a
-else:
    ->3a
}

=== 1a ===

changeCamTarget({kendeIndex})

... and that's what we do to traitors. I will have complete discipline in my kitchen until the Director restores order to the camp. Anyone who speaks out of turn again will get what Pan here got thrice over.

changeCamTarget({panIndex})

Eurghhh...

changeCamTarget({kendeIndex})

Now lets get a head count. At a glance, we look light. Where's Imre?

changeCamTarget({loyalImreIndex})

I'm right here, and I've brought friends. Your time's up, Kende. We're not taking orders from you anymore!

    {
    -acceptingGuardPrisoners && not guardSurrendered:
    +I will only give this offer once. Any guard who surrenders will be taken into custody under my protection. Those that resist, perish.
        ->1b
    }
    +That's right. I've come to fulfill my promise to the unbranded. Your servitude ends today!
        ->1c
    +Quake, slaver! Today you and your guards get the blade!
        ->1d
    +I care not for your grudges. I'm here to kill slavers, so lets get on with it.
        ->1d
    +\*Say nothing.*
        ->1e

=== 1b ===

~guardSurrendered = true

changeCamTarget({guardIndex})

\*The guard throws down his weapons.* I surrender!

changeCamTarget({kendeIndex})

Gods, I'm surrounded by traitors. The next one of you who shows cowardice goes in the stew! I swear it!

->Close

=== 1c ===

changeCamTarget({panIndex})

Yey...

changeCamTarget({kendeIndex})

I see that this treachery has been festering for a while now. Listen up! Any slave that remains loyal to the Director will be given a week's reprieve after the revolt is put down. Any who side with the traitors will be flogged, branded, and then flogged again. To arms!

->Close
    
=== 1d ===

changeCamTarget({kendeIndex})

So what if you have the numbers. I am on the side of the Gods and the right! From where I'm standing, all this means is less mouths to feed.

Listen up! Any slave that remains loyal to the Director will be given a week's reprieve after the revolt is put down. Any who side with the traitors will be flogged, branded, and then flogged again. To arms!

->Close

=== 1e ===

changeCamTarget({kendeIndex})

Imre, you pathetic layabout. All you've done is bring the brand down on you and your friends.

Listen up! Any slave that remains loyal to the Director will be given a week's reprieve after the revolt is put down. Any who side with the traitors will be flogged, branded, and then flogged again. To arms!

->Close

=== 2a === 

changeCamTarget({disloyalImreIndex})

I've brought them, sir! This is the one I told you about!

changeCamTarget({kendeIndex})

Excellent. And thanks to your warning, we're ready for them. Everyone gets double rations after this is over, thanks to Imre's loyalty. But first, stand fast. The traitors come!

    +Imre, you snake. Why would you betray us like this?
        ->2b
    +I see this was a trap. No matter, we'll sweep aside all obstacles on the way to the Director! <Combat>
        ->Close
    
=== 2b ===

changeCamTarget({disloyalImreIndex})

Betrayal? You threatened me! You just wanted more fodder against the guards. At least this way I'll get to live to see freedom one day.

->Close

=== 3a ===

changeCamTarget({kendeIndex})

{
-insultedKendesCooking:
If it isn't the scum that insulted my cooking. I wonder if you'll be so mouthy after I've spitted and roasted you infront of your companions.

-else:
The branded have breached the barricades, and now they've entered my kitchens? Don't come any closer, or you'll get scum on the pots!
}

->3aa

=== 3aa ===

{
-acceptingGuardPrisoners && not guardSurrendered:
    +It's not too late to surrender. If you throw down your weapons, we'll keep you safe in our custody until the fighting is over.
        ->3f
}

{
-charisma >= 4:
    +Any of you nonbranded who decides to fight for your freedom will have it. We have no quarrel that you do not cause. <Cha {charisma}/4>
        ->3d
}

{
-strength >= 4:
    +If you nonbranded stay very still while I peel this cook, I may forget about you. Maybe. <Str {strength}/4>
        ->3e
}

    +Quake, slaver! Today you and your guards get the blade!
        ->3b

    +I take no pleasure in it, but if you want to get in my way then you'll force me to remove you. 
        ->3c
    

=== 3b ===

I've cooked turnips tougher than you. When this is over, you'll just be more scraps on my kitchen's floor.

->Close

=== 3c ===

Don't make me laugh, you couldn't pluck feathers from a duck.

->Close

=== 3d ===

~convincedNonbrandedToHelp = true

changeCamTarget({slaveIndex})

I'm not dying for you, Kende. My years of fixing your slop are at an end!

changeCamTarget({kendeIndex})

Woah, hold on now. Can't we talk about this? Guys?

->Close

=== 3e ===

~convincedNonbrandedToHelp = true

changeCamTarget({slaveIndex})

Eep!

->Close

=== 3f ===

~guardSurrendered = true

changeCamTarget({guardIndex})

\*The guard throws down his weapons.* I surrender!

changeCamTarget({kendeIndex})

Gods, I'm surrounded by traitors. The next one of you who shows cowardice goes in the stew! I swear it!

->3aa

=== Close ===

/*
VAR halfSlavesNoGuardFightIndex = 0
VAR halfSlavesFightIndex = 1
VAR fullSlavesNoGuardFightIndex = 2
VAR fullSlavesFightIndex = 3
*/

setToTrue(foughtKendeInManseKitchen)

{
-(convincedImre or convincedNonbrandedToHelp) and guardSurrendered:

    enterCombat({halfSlavesNoGuardFightIndex})
    
-convincedImre or convincedNonbrandedToHelp:

    enterCombat({halfSlavesFightIndex})
    
-guardSurrendered:

    enterCombat({fullSlavesNoGuardFightIndex})
    
-else:
    enterCombat({fullSlavesFightIndex})
}

close()

->DONE