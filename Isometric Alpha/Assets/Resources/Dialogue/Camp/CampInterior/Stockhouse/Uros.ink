VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR startledUros = false
VAR gaveKastorThePassword = false
VAR convincedUros = false
VAR intimidatedUros = false
VAR threatenedToSnitchOnUros = false
VAR snitchedOnUros = false
VAR hasIronNugget = false
VAR showedUrosTheNuggetWithoutGivingItBack = false
VAR gaveUrosTheNugget = false
VAR foughtMuzsaAfterSnitchedOnUros = false

VAR lostIronNuggetName = "Lost Iron Nugget"

VAR urosIndex = 1
VAR emeseIndex = 2

VAR playerName = ""

searchInventoryFor(hasIronNugget,{lostIronNuggetName})

{
-snitchedOnUros and foughtMuzsaAfterSnitchedOnUros:
->4b
-snitchedOnUros:
->4a
-gaveUrosTheNugget:
->3a
-showedUrosTheNuggetWithoutGivingItBack:
->3b
-startledUros:
->2a
-else:
->1a
}

=== 1a ===

setNPCFacing({urosIndex},SE)

\*The branded slave mutters under his breath.* Where is it? I know I hid it right here.

    +Looking for something?
        ->1b
    +\*Leave without disturbing him.*
        ->Close

=== 1b ===

~startledUros = true
setToTrue(startledUros)

facePlayer({urosIndex})

Oh! You startled me. I ain't lookin' for nothin', just movin' boxes around.

{
-hasIronNugget:
    +I found this, is it what you were looking for? *Show the Iron Nugget to Uros.*
        ->2b
-charisma >= 2:
    +I won't tell anyone, I'm just curious. <Cha {charisma}/2>
        ~convincedUros = true
        setToTrue(convincedUros)
        activateQuestStep(Stockhouse Stash, The lump of iron.)
        ->1d
}

    +That's not what I heard you say before.
        You heard that? Well I'm not gonna trust no eavesdropper. Get lost.
        ->1c
    +Don't lie to me. What did you hide? Was it valuable?
        I ain't gonna say nothin' to yer like. Now get!
        ->1c
    +Tell me or I'm going to call the quartermaster over. We'll see how interested she is in a slave stashing goods in her stockhouse.
        ->1h
    +Fine, nevermind then.
        activateQuestStep(Stockhouse Stash, Uros is hiding something.)
        ->Close

=== 1c ===

{
-strength >= 2:
    +Tell me or the next thing you'll be looking for is all your missing teeth. <Str {strength}/2>
        ~intimidatedUros = true
        setToTrue(intimidatedUros)
        activateQuestStep(Stockhouse Stash, The lump of iron.1)
        keepDialogue()
        No need for violence! I like my teeth where they are. And it's not like I'm going to find the damn thing anyways. Ok, I'll tell you.
        ->1d
}

    +Fine, I'm going.
        activateQuestStep(Stockhouse Stash, Uros is hiding something.)
        ->Close
    
=== 1d ===

I guess it's not like I can find the damn thing anyways. Ok, I'll tell you.
    ->1e
=== 1e ===

I found a shiny lump of something while digging in the mine. I'm not sure but I think it's a bit of iron. More valuable than copper or tin, much more. I just can't find where I left it, or I'd be rich.

    +Iron? Is it really worth that much?
        
        I hear iron sells for quite a bit. There aren't many places on Föld where it can be mined, so it's coveted by counts and kings alike. It's also harder to work than bronze. I'm no smith so I can't speak to why, but I heard before my branding that it takes a special furnace or some such equipment to melt it down, so you need special know-how to shape it. 
        
        keepDialogue()
        All in all this makes tradin' in iron goods very lucrative, and I had my own little slice of it before I lost it in these blasted crates! 
        ->1e
    +Can't you just wait until the mine opens again and then pocket some more?
        keepDialogue()
        No that's not... You're new so I suppose you wouldn't know, but we don't really mine much ore here, let alone iron ore! We mostly just pull rubble out of the mine, but lately we started finding stuff down there. Furniture, stairs cut into the rock, entire rooms that look like they haven't been lived-in in ages. You wouldn't find any iron unless you get lucky like I did.
        ->1e
    +What use are valuables to a slave? You don't have anywhere to sell them.
        I saw the cook in the mess hall tradin' extra food to slaves who bring him things. Maybe he'd buy it. *Uros absentmindedly scratches his brand.* Besides, it's not like I plan to be a slave forever.
        ->1f


=== 1f ===

{
-gaveKastorThePassword:
    +Which way is the wind blowing?
        Huh? How should I know. I've been working in here all day.
        ->1g
}
    +You're very optimistic for a slave.
        Well you gotta be, otherwise what's the point in keepin' on breathin'?
        ->Close
    +I hope you find what you're looking for.
        ->Close
    +Yeah? Good luck with that.
        ->Close
=== 1g ===

    +Right, of course. Nevermind then.
        ->Close

=== 1h ===

You snitch scum, you wouldn't dare!

{
-charisma >= 2:
    +Try me. <Cha {charisma}/2>
        ~threatenedToSnitchOnUros = true
        setToTrue(threatenedToSnitchOnUros)
        activateQuestStep(Stockhouse Stash, The lump of iron.2)
        keepDialogue()
        Fine, damn you! It's not like I was gonna find the stupid thing anyways.
        ->1d
}

    +Oh quartermaster!
        setFacing(NE)
        ->1i
    +You're right, it's not worth snitching on you for.
        activateQuestStep(Stockhouse Stash, Uros is hiding something.)
        ->Close

=== 1i ===

changeCamTarget({emeseIndex})

\*Quartermaster Emese looks up from her work.* What is it?

    +This slave has a stash hidden away in your stockhouse, he hasn't been working because he's too busy looking for it.
        ->1j

=== 1j ===

changeCamTarget({urosIndex})

changeNPCFacing({urosIndex},NE)

That's a lie, ma'am. I'm working as hard as I can!

changeCamTarget({emeseIndex})

\*Emese looks around the stockhouse.*

You're clearly behind on your tasks. Wait for your lashing outside. If you're not there when I come looking for you, it'll be to the pit with you.

fadeToBlack(true, false)

setToTrue(snitchedOnUros)
deactivate({urosIndex})

fadeBackIn(60)

activateQuestStep(Stockhouse Stash, Uros was hiding something.)

Thank you for bringing this to my attention. If you happen to find whatever he had squirrelled away in here before I get him to talk, and bring it to me, I'll reward you. Just don't let it get in the way of your work; I won't be defending you to some other guard if they come looking for you.

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

Don't bother me, I have work to get to.

{
-hasIronNugget:
    +I found this, is it what you were looking for? *Show the Iron Nugget to Uros.*
        ->2b
}

    +I won't disturb you then.
        ->Close
    
=== 2b ===

\*Uros examines the nugget.* Yes! That's it! Thank you!

    +Don't mention it. *Hand the Iron Nugget to Uros.*
        ->2c
    +I'm just showing you so you can stop searching. I'm keeping it.
        ->2d

=== 2c ===

setToTrue(gaveUrosTheNugget)
finishQuest(Stockhouse Stash, true, The nugget retrieved.)

prepForItem()

You're alright. I won't forget this. Ever!

takeItem(Iron Nugget, 1)&
addXP(200,1)

->Close

=== 2d ===

setToTrue(showedUrosTheNuggetWithoutGivingItBack)
finishQuest(Stockhouse Stash, true, The nugget stolen.)

prepForItem()

You little stain. I won't forget this. Ever!

addXP(100,1)

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

Thanks again, friend. I wish you the best out there.

    +And the same to you.
        ->Close
    +I must be going.
        ->Close

=== 3b ===

Get out of here you rat.

    ->Close

=== 4a ===

You disgust me. I won't forget this. Ever!

    ->Close

=== 4b ===

\*Uros looks at you with wide eyes.* I-I won't tell anyone you killed Múzsa. I swear!

    ->Close

=== Close ===

close()

->DONE