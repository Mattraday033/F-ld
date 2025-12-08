VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR showedUrosTheNuggetWithoutGivingItBack = false
VAR gaveUrosTheNugget = false
VAR snitchedOnUros = false
VAR threatenedToSnitchOnUros = false
VAR intimidatedUros = false

VAR spokeToUrosShop = false
VAR urosBadAttitude = false
VAR urosGoodAttitude = false

VAR urosBestPrices = false
VAR urosNormalPrices = false
VAR urosBadPrices = false
VAR urosWorstPrices = false

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
-urosBadAttitude:
    ->1g
-urosGoodAttitude:
    ->1f
-spokeToUrosShop:
    ->1e
-gaveUrosTheNugget:
    ->setAttitudeToGood(->1d)
-threatenedToSnitchOnUros:
    ->setAttitudeToBad(->1b)
-snitchedOnUros:
    ->setAttitudeToBad(->1b)
-showedUrosTheNuggetWithoutGivingItBack:
    ->setAttitudeToBad(->1c)
-else:
    ->1a
}

=== 1a ===

setToTrue(spokeToUrosShop)
setToTrue(urosNormalPrices)

Hey, how's it going? While you were out there fighting the guards, some of us took the opportunity to liberate some of their stuff. My haul is yours for the perusal; only take what you can pay for though!

    +Fine, let me take a look.
        enterShopMode()
        ->Close
    +I don't have time for this.
        ->Close
        
=== 1b ===

setToTrue(urosWorstPrices)

You may have everyone else lookin' up to you, but I remember what you did to me earlier. Snitches pay double. And no hagglin'.

{
-charisma >= 3:
    +I'm fighting to free us all, you included. That's not worth a little forgiveness? <Cha {charisma}/3>
        ->2a
}
    +\*Sigh* Fine, show me what you've got.
        enterShopMode()
        ->Close
    +Screw this. I'll be fine on my own.
        ->Close

=== 1c ===

setToTrue(urosBadPrices)

Well if it ain't the nugget thief. Hope you got good money for that thing, 'cause all my prices just shot up when I saw you comin'.

{
-charisma >= 3:
    +I'm fighting to free us all, you included. That's not worth a little forgiveness? <Cha {charisma}/3>
        ->2a
}
    +\*Sigh* Fine, show me what you've got.
        enterShopMode()
        ->Close
    +Screw this. I'll be fine on my own.
        ->Close
    
=== 1d ===

setToTrue(urosBestPrices)

My friend! Good to see you! I haggled that iron away to another slave for a bit of his loot, so I've got lots of good stuff for sale. Everything you see here is for buying. Don't worry about the cost, I save my best prices for my friends.

    +\That's very generous. Let me see what you found.
        enterShopMode()
        ->Close
    +I must be going.
        ->Close

=== 1e ===

The fighting must be fierce out there. I've collected plenty of rations: buy some to keep your strength up.

->1h

=== 1f ===

Thanks to you, the looting has been going well. It's only right that you share in the spoils.

->1h

=== 1g ===

Oh, you're back. Buy something quick and then leave me be.

->1h

=== 1h ===

    +Show me what you have for sale.
        enterShopMode()
        ->Close
    +I must be going.
        ->Close
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
    {
    -urosWorstPrices:
        setToTrue(urosBadPrices)
    -urosBadPrices:
        setToTrue(urosNormalPrices)
    }
    \*Uros grits his teeth as he thinks on what you said.* I guess that's worth somethin'. I'll drop my prices a little, but it ain't cause I like you.
        enterShopMode()
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

=== setAttitudeToBad(->divert) ===

setToTrue(spokeToUros)
setToTrue(urosBadAttitude)

->divert

=== setAttitudeToGood(->divert) ===

setToTrue(spokeToUros)
setToTrue(urosGoodAttitude)

->divert

=== metUrosNoAttitude(->divert) ===

setToTrue(spokeToUros)

->divert


=== Close ===

close()

->DONE