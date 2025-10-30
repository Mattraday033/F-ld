VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR coins = 0

VAR givenTaskByMuzsa = false
VAR insultedKendesCooking = false
VAR askedKendeWhoHeIs = false
VAR kendeWillSellToPlayer = false
VAR learnedAboutMuzsasSweetToothFromKastor = false
VAR gotMessHallInstructionsFromKende = false

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

->1a

=== 1a ===

{
-insultedKendesCooking:
What is it, slime?
-gotMessHallInstructionsFromKende:
You're back? There's no second helpings, so if it's not important then beat it.
-else:
~gotMessHallInstructionsFromKende = true
setToTrue(gotMessHallInstructionsFromKende)

prepForItem()

You new? I guess it doesn't matter, everyone's got the same rules. Take a plate and a cup and wait in line 'til it's your turn for your ration. Don't take a seat, those are for the off-duty guards. Head out to the yard through the big doors and eat quickly; if you have a thirst get some water from the well. When whichever guard brought you here calls for you, stack your plate outside before coming back in. Now, here you go.

giveItem(0,0,1)
}

{
-givenTaskByMuzsa:
    +Guard Múzsa sent me. She said you had some things for sale she would like to buy.
        ->1b
}
{
-not insultedKendesCooking and not kendeWillSellToPlayer:
    +Who are you?
        ->2a
}

{
-kendeWillSellToPlayer:
    +I have some coin to spend if you're still selling.
        Certainly.
        enterShopMode()
        ->Close

-learnedAboutMuzsasSweetToothFromKastor:
    +I have a bit of coin, and am in the market for something sweet. What say you?
        ->1c

}
    +Nothing, I need to be going.
        Then quit wasting my time.
        ->Close

=== 1b ===

//activateQuestStep(Múzsa's Sweet Tooth, 3)
setToTrue(kendeWillSellToPlayer)

//prepForItem()

Augh, that idiot is sending slaves now? Look, buy whatever she sent you to get and then leave. I don't need this attention.

enterShopMode()

//giveItem(3,3,1)

->Close

=== 1c ===

{
- insultedKendesCooking:
    \*Kende's ears perk up at your mention of coin.* I may have what you want, but my ego is still recovering from what you said about my cooking. 
    
    {
    -charisma >= 2:
        +Surely an astute business man like yourself wouldn't let a bruised ego get in the way of making a bit of gold? <Cha {charisma}/2>
            ->1d
    }
    
    +Would 25 gold speed it's recovery?
        ->1e
    +I'm not going to pay you for the right to pay you. I'm leaving.
        ->Close
-else:

A slave? With coin? *Kende ponders this for a moment, but then relents.* How about I don't ask you where you got your coin, and you don't mention where you got anything you buy here. Deal?
    
    +Deal.
        ~kendeWillSellToPlayer = true
        setToTrue(kendeWillSellToPlayer)
        enterShopMode()
        ->Close
}

    
=== 1d ===

~kendeWillSellToPlayer = true
setToTrue(kendeWillSellToPlayer)

I suppose if you spend *enough* coin, I could let it go. Fine, let me show you what I have.

enterShopMode()

->Close

=== 1e ===

{
-coins < 25:

It would, if you had that much. Come back when you're a little... hmmmm, richer.

->Close

-else:
~kendeWillSellToPlayer = true
setToTrue(kendeWillSellToPlayer)

prepForItem()

I think it will manage to limp on for that much. Let me show you what I have.

takeCoins(25)

enterShopMode()

->Close
}

=== 1f ===

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

setToTrue(askedKendeWhoHeIs)
~askedKendeWhoHeIs = true

I'm Kende. I'm the cook. I spend all of my time slaving away preparing meals for you branded and before you ask, yes, my parents are very proud. Now if you're done bothering me, get moving.

    +So you are the one who makes the rations we get?
        ->2b
    +Alright, I'll be going.
        ->Close
    
=== 2b ===

Yeah that's me. What about it?

    +They were good. Thank you.
        ->2c
    +Do you put the maggots in yourself or do they find their way there after?
        setToTrue(insultedKendesCooking)
        You little shit! Get out of here before I decide you're worth caning.
        ->Close

=== 2c ===

Oh, um... thanks.

    +Don't mention it. Good bye.
    {
    -kendeWillSellToPlayer:
        ->Close
    -else:
        ->2d
    }
=== 2d ===

setToTrue(kendeWillSellToPlayer)

Wait, you aren't ungrateful like all of the other slaves. I make a little money on the side selling stuff to the other guards. If you happen to "find" any coins lying around out there, I'll give you something for them.

    +I have some money on me right now.
        enterShopMode()
        ->Close
    +That's good to know. I'll come back if anything comes my way.
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