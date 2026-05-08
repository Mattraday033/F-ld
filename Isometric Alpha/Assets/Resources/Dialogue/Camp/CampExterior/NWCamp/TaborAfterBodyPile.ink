VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR taborIndex = 1
VAR weftIndex = 2
VAR guardIndex = 3

VAR kendeWillSellToPlayer = false

VAR playerName = ""

->1a

=== 1a ===

stopAllFades()

movePlayerPos(-7, 14)
setFacing(SE)
activate({taborIndex})
activate({weftIndex})

finishQuest(Comb the Bodies,true,Found the ring.)
wait(.5)

fadeBackIn(60)

A brutal task for anyone, to rifle through a pile of your own dead. But as always, there is a lesson here. Do not become too attached to this life; you branded can quickly leave it at any time. 

Instead, look to achieving the next life. Think of what you will say to those you've wronged when next you will meet them. Inner reflection and humility will see your spirit quickly through the flames of your hearth into your next cycle.

    +You really believe you're doing us a favor, don't you?
        I'm doing every human on Föld a favor. Or, at least the ones who have met you two.
        ->1aa
    +Humility is a trait we should all aspire to. It's not just something you preach to criminals.
        Too right. Maybe you'll be easier to teach than the others. You seem to have a grasp of it already.
        ->1aa
    +I'd work twice as hard if you'd just shut up while I did it.
        The labors aren't the point of this ordeal, slave. They're just a tool to teach you what you've done is wrong.
        ->1aa

=== 1aa ===

    +Have you ever met a branded who actually wanted to learn whatever you're trying to teach them?
        Not many of the branded are receptive to morality, no. But that doesn't mean it isn't a glorious calling to attempt to teach it to them.
        ->1ab

=== 1ab ==

But enough lessons. You'll learn better after you've eaten. I'll escort you down to the mess hall for lunch, and then we'll start our afternoon session.

fadeToBlack(true, false)
setNPCFacing({guardIndex},NW)
setNPCFacing({taborIndex},SE)
changeCamTarget({guardIndex})
activate({guardIndex})
fadeBackIn(60)

Chief Tabor! Captain Adéla sent a runner with a message for you! A situation is brewing in the northeast quarter. Your presence is requested.

setToTrue(toldToGetMealByTabor)
setToTrue(situationStartedInNECamp)
changeCamTarget({taborIndex})
setNPCFacing({taborIndex},NW)

{
-kendeWillSellToPlayer:
->2a
}

Of course there is. I'd best go see what that is about. You two, go directly to the mess hall, and make no other stops. When you finish your meal, meet me in the northeast part of the camp. 

    +Where is the mess hall?
        ->1b

=== 1b ===

activateQuestStep(A Situation Brews,Midday meal.)

You did your work well today, I forgot you're new. It's the large building with the wooden roof in the southeast part of the camp. It's the one that opens up into a large yard with a well. Just head south from the center of camp and you can't miss it.

->deactivateExtras

=== 2a ===

activateQuestStep(A Situation Brews,Assess the situation.)

Of course there is. Looks like lunch will have to wait, Adéla wouldn't have sent for me if it wasn't urgent. I will run ahead and see what needs my attention. Meet me in the camp's northeast quarter and I'll put you to work on whatever has come up.

->deactivateExtras

=== 1c ===

Don't come back until you've removed those boards.

->Close

=== deactivateExtras ===

fadeToBlack()

deactivate({taborIndex})
deactivate({guardIndex})
deactivate({weftIndex})

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE