VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR hasBlastingJelly = false
VAR toldToFindNandor = false
VAR mineLvl3ClearedCratesToMiners = false
VAR mineLvl3MetGaspar = false
VAR mineLvl3MarcosAgreedToIgniteJelly = false
VAR takingCarterNandorWithYou = false

VAR broughtNandorToKastor = false

VAR startingFromMinersDialogue = false
VAR minersCrateDialogueIndex = 1

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
-broughtNandorToKastor:
    ->3a
-startingFromMinersDialogue:
    ->2a
-mineLvl3MarcosAgreedToIgniteJelly:
    ->1e
-else:
    ->1a
}


=== 1a ===

searchInventoryFor(hasBlastingJelly,Blasting Jelly)

changeCamTarget(1)

I see you're back. Were you successful in enlisting the other guards?
    
{
- hasBlastingJelly:
    +The Guards were uncooperative. But I have the blasting jelly.
        ->1c
}
    +Not yet.
        ->1b

=== 1b ===

In that case, maybe you should rest while you're here. I will stand guard while you recover.

    +\*Rest*
        restParty()
        ->Close
    +I cannot rest right now. I need to keep moving.
        ->Close

=== 1c ===

That is unfortunate. I will attempt the ignition then.

    +You have lost a lot of blood. You shouldn't be handling explosives if you can't keep yourself from trembling.
        ->1d
    +Understood. I will escort you to the breach.
        setToTrue(mineLvl3MarcosAgreedToIgniteJelly)
        ->Close
=== 1d ===

That does not matter, I am the only one here who has been trained in how to mix the jelly with the primer, and how to set the ignition timer. Without me, the breach cannot be sealed.
    
{
-wisdom >= 2:
    +I am a quick learner. If you explained it to me, I could perform the mixing and set the timer. <Wis {wisdom}/2>
        ->2a
}
    +I understand. May the Gods guide your hands.
        setToTrue(mineLvl3MarcosAgreedToIgniteJelly)
        activateQuestStep(Sealing the Breach, 5)
        ->2c
    +Fine. Lets get a move on then.
        setToTrue(mineLvl3MarcosAgreedToIgniteJelly)
        activateQuestStep(Sealing the Breach, 5)
        ->2c

=== 1e ===

You're back. Are you in need of rest?

+\*Rest*
    restParty()
    ->Close
+I cannot rest right now. I need to keep moving.
    ->Close

->Close

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

If you are set on it, then I won't stop you. Let us go over some of the basics and see if you have an aptitude for it. 

fadeToBlack(true, false)

fadeBackIn(60)

I think you're getting the hang of how to measure out the primer. So long as you continue mixing with the pattern I've shown you, you shouldn't have any trouble getting the right consistancy. Now, for the timer.

The ignition method for blasting jelly is the application of water. Even a single drop is enough to cause the mixed jelly and primer to ignite, resulting in a large explosion.

    +Water? But the guards were storing the blasting jelly in a cavern with a stream flowing through it. Isn't that risky?
        ->2b

=== 2b ===

So long as the blasting jelly isn't mixed with the primer, neither can ignite even if submerged. Though the guards still store the primer and jelly on separate ends of the cavern, to prevent any accidental mixture. But nevermind that, let us get back to the matter at hand.

The timer itself is actually very simple, the difficulty comes from the risk involved if the instructions are inproperly followed. There are two parts to the timer, a large cup with a small spout in the side, and a smaller cup. First, fill the larger cup half of the way with water, away from the mixture. Then carry it slanted on it's side so that the water does not drip out of the spout, or the top.

When you reach the mixed blasting jelly, place the smaller cup on top of the barrel with the jelly in it. Then place the larger cup on the barrel so that the water starts to drip out of the side spout into the smaller cup. Then hurry away from the mixture. You have until the dripping fills the smaller cup to overflowing before the barrel will ignite. 

setToTrue(mineLvl3MarcosAgreedToIgniteJelly)
setToTrue(mineLvl3MarcosTaughtHowToIgniteJelly)
activateQuestStep(Sealing the Breach, 3)

Be careful not to knock over either of the cups as you hurry away from the barrel. We've lost more than one guard to that mistake.

    +I understand. Thank you.
        ->2c
    +Let's get moving. I want to get out of this damned mine.
        ->2c

=== 2c ===

You lead. I'll be right behind you.

{
-startingFromMinersDialogue:
swapInkFiles({minersCrateDialogueIndex},backFromMarcosDialogue)
}

fadeToBlack()

deactivate(1)

fadeBackIn(60)

->Close

=== 3a ===

\*Someone has applied clean bandages to Márcos's wounds. He is sleeping soundly.*

    +\*Leave.*
    ->Close
=== 3b ===

->Close

=== Close ===

close()

->DONE