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
VAR toldKendeTaborSentForMeal = false
VAR weftKnowsYouKnowHisSecret = false
VAR toldToGetMealByTabor = false

VAR weftActive = false

VAR kendeIndex = 1
VAR weftIndex = 2

VAR playerName = ""

{
-toldToGetMealByTabor and not toldKendeTaborSentForMeal:
    ->3aa
-insultedKendesCooking:
What is it, slime?
->1aa
-gotMessHallInstructionsFromKende:
You're back? There's no second helpings, so if it's not important then beat it.
->1aa
-else:
->1a(->1aa)
}

=== 1a(->divert) ===

~gotMessHallInstructionsFromKende = true
setToTrue(gotMessHallInstructionsFromKende)

prepForItem()

You new? I guess it doesn't matter, everyone's got the same rules. Take a plate and a cup and wait in line 'til it's your turn for your rations. Don't take a seat, those are for the off-duty guards. Head out to the yard through the big doors and eat quickly; if you have a thirst get some water from the well. When whichever guard brought you here calls for you, stack your plate outside before coming back in. Now, here you go.

giveItem(0,0,1)

->divert

=== 1aa ===

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
        Make it quick.
        ->shop

-learnedAboutMuzsasSweetToothFromKastor:
    +I have a bit of coin, and am in the market for something sweet. What say you?
        ->1c

}
    +I need to be going.
        Then quit wasting my time.
        ->Close

=== 1b ===

setToTrue(kendeWillSellToPlayer)

Augh, that idiot is sending slaves now? Look, buy whatever she sent you to get and then leave. I don't need this attention.

->shop

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
        ->shop
}

    
=== 1d ===

~kendeWillSellToPlayer = true
setToTrue(kendeWillSellToPlayer)

I suppose if you spend *enough* coin, I could let it go. Fine, let me show you what I have.

->shop

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

->shop
}

=== 2a === 

setToTrue(askedKendeWhoHeIs)
~askedKendeWhoHeIs = true

I'm Kende. I'm the cook. I spend all of my time slaving away preparing meals for you branded and before you ask, yes, my parents are very proud. Now if you're done bothering me, get moving.

    +So you are the one who prepares the rations we get?
        ->2b
    +Alright, I'll be going.
        ->Close
    
=== 2b ===

Yeah that's me. What about it?

    +They were good. Thank you.
        ->2c
    +Did you figure out how to make them taste like dirt yourself or did your mother teach you?
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

Wait, you aren't ungrateful like the other slaves. I make a little money on the side selling stuff to the other guards. If you happen to "find" any coins lying around out there, I'll give you something for them.

    +I have some money on me right now.
        ->shop
    +That's good to know. I'll come back if anything comes my way.
        ->Close

=== 3aa ===

fadeToBlack(false, false)

setToTrue(toldKendeTaborSentForMeal)

activateQuestStep(A Situation Brews,Assess the situation.)

~weftActive = true
activate({weftIndex})

fadeBackIn(60)
enableDialogueUI()
{
-insultedKendesCooking:
Another showing from my least favorite branded. Back to give me more grief I suppose?
->3a
-gotMessHallInstructionsFromKende:
You're back. If you're here for another helping, you're in for disappointment. You were lucky you got the first, the workers who are stuck in their huts only get fed at sunup and sundown.
->3a
-else:
->1a(->3a)
}


=== 3a ===

And I see Weft is with you. Has our mutual friend sent you back to me?

changeCamTarget({weftIndex})

I'm on a work detail with my new hutmate here. Chief Tabor has us on break for our midday meal.

changeCamTarget({kendeIndex})

prepForItem()

In that case, here is your helping.

giveItem(0,0,1)

{
-insultedKendesCooking:
->3b
}

combineDialogue()

\*Kende turns back to you.* If you're a friend of Weft's then you must be the trustworthy sort. You know, 
->3ab

=== 3ab ===

I make a tidy bit of money selling hard-to-get goods that come in on the supply wagons. If you <i>find</i> any gold while working in the camp, I'd be happy to give you something for it.

    +I'll see what you have.
        ->shop
    +Maybe later.
        \*Kende shrugs.* Suit yourself.
        ->Close

=== 3b ===

You know Weft, you keep ill company. This one hasn't been here a day and they've already insulted me to my face.

changeCamTarget({weftIndex})

\*Weft frowns.* They certainly seem to get around.

changeCamTarget({kendeIndex})

You and I have made quite a bit of money during your time here. I'd have happily extended the same deal to them, if they were the trustworthy sort.

{// done this way to keep the charisma check at the top of the choices list
-true:
    +I was just having a bit of fun before, nothing was meant by it. What's a little ribbing between friends? <Cha {charisma}/2>
    {
    -charisma >= 2:
        ->3ba
    -else:
        ->3d
    }
}

{
-weftKnowsYouKnowHisSecret:
    +Weft owes me a great deal already. *Glare at Weft.* You'll vouch for me, <i>won't</i> you?
        ->3e
}
    +I didn't realize you were a businessman. Would an apology allow you to put profit before pride?
        ->3c
    +Whatever this deal is, I want no part of it.
        That is good, because I'm glad to be rid of you. Leave me. 
        ->Close

=== 3ba ===

combineDialogue()

\*Kende chews his lip in thought.* Very well, let's put such small words behind us then. 

->3ab

=== 3c ===

I suppose I could be convinced, if it were heartfelt enough.

    +Then allow me to apologize. What I said was uncalled for, and untrue.
        combineDialogue()

        Well said. 
        ->3ab
    +Well, my heart just isn't in it. Get bent. 
        Gah! Begone from here! You won't get another ration from me so long as you live!
        ->Close

=== 3d ===

Us, friends? The thought revolts me, scum. Get gone.

->Close

=== 3e ===

changeCamTarget({weftIndex})

\*Weft's frown deepens.* We've worked together all day and I can already tell they are very... profit minded. I think bringing them into our little circle would be beneficial for everyone.

changeCamTarget({kendeIndex})

->3ba

=== shop ===

{
-weftActive:
->deactivateExtras(->shop)
}

~kendeWillSellToPlayer = true
setToTrue(kendeWillSellToPlayer)
enterShopMode()

->Close

=== deactivateExtras(->divert) ===
fadeToBlack()

~weftActive = false
deactivate({weftIndex})

fadeBackIn(60)
->divert

=== Close ===

{
-weftActive:
->deactivateExtras(->Close)
}

close()

->DONE