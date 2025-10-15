VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR metTemple = false
VAR templeMentionedBackground = false
VAR templeExplainedPatches = false
VAR askedTempleAboutSampson = false
VAR heardTaborsLesson = false
VAR campCenterFirstHiddenPassageFound = false

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
-metTemple:
    ->2a
-else:
    ->1a
}

=== 1a ===

{
-campCenterFirstHiddenPassageFound:
\*Temple grumbles under his breath.* Some bastard wants to tear a hole in the wall while my back is turned? If I catch 'em I'm gonna skin 'em alive, I swears it.
-else:
Are you looking for somethin', kid?
}

    +I'm looking for Sampson. Do you know where he is?
        ->1b
    +Nothing specific, just curious what you're doing out during lockdown by yourself.
        ->1d
    +No, just passing by.
        ->Close

=== 1b ===

~askedTempleAboutSampson = true
setToTrue(askedTempleAboutSampson)

Sampson? Why are you looking for that pisstain? 
    
    +I was told to by Guard László. 
        ->1ca
    +That's between myself and Sampson.
        ->1cb

=== 1c ===

    +Thanks. By the way, how come you get to be out of your hut without a guard?
        ->1d
    +Thank you. *Leave.*
        ->Close

=== 1ca ===

Well in that case, he's up by the Manse. Turn west, away from the gate, and keep going until you see the big shack on your right. Can't miss it.
        ->1c

=== 1cb ===

I suppose that's true, just be careful around that roach. He's up by the Manse. Turn west, away from the gate, and keep going until you see the big shack on your right. Can't miss it.
        ->1c

=== 1d ===

~templeExplainedPatches = true
setToTrue(templeExplainedPatches)

{
-heardTaborsLesson:
Oh, I've got a guard on me, he's just not the brightest firefly in the field. Don't worry, he seems pretty enthralled with pickin' his nose from where I'm standing, so I don't think he'll chase us back to work any time soon.
-else:
Oh, I've got a guard on me, he's just over there watching ol' Tabor whip the hide off of someone. Don't worry, he seems pretty enthralled from where I'm standing so I don't think he'll chase us back to work any time soon.
}

{
-campCenterFirstHiddenPassageFound:
As for what I'm doing, I just finished patching up the big hole in the wall of the stables here when someone came along and tore it down. Now I'm gonna be stuck our here for twice as long fixin' someone else's shoddy work.

These saddlehumpers thinks they know more about putting boards up than I do, despite my three decades of experience. Always got me chasin' after their mistakes when they break somethin'.
-else:
As for what I'm doing, I just finished patching up the big hole in the wall of the stables here. Ain't my best, but it's hard to get that out of me with the way my hands are gettin'.

The job's easy enough, but the trick of it is to build the patches in a way that the boards can be removed easily. That way if some saddlehumper thinks he knows more about putting boards up than your three decades of experience and tries to take it apart, the roof doesn't collapse on him and get you in trouble.
}

    +You're a carpenter?
        ->1e

=== 1e ===

    ~templeMentionedBackground = true
    setToTrue(templeMentionedBackground)

    keepDialogue()

    I'm not just some carpenter, I'm a Master Woodworker. Used to be one of the best in all the Stolen Cities too, but that was a long time ago.
    
    ->2a
=== 1f ===


Aye, what some call many of the cities of the Confederation. The Lovashi stole them when they conquered what used to be Craft Folk land and made them into their counties' capitols. 

I'm from Pharos, and it's... well I'm not sure how far it is from here, on account of no one has actually done me the courtesy of explaining where this camp sits on a map. But Pharos lies down south, where the Waking Mountains melt into the Masonic Gap, pretty close to where the Confederation ends and the Craft Kingdoms begin.

A beautiful city, full of red-blooded folk of the Craft clinging to our traditions just as hard as the Lovashi try to peel 'em off us. Last of the Stolen Cities to bow too, and I'll remind anyone who listens of that fact until the day I die.

But I'm rambling, and it's dangerous for you to be idle. I don't get to talk much so I relish it when I can, but you'd best be off. If you're interested in learning more, seek out Bálint in the invalid's hut just north of the gate over there. He's a sage of some kind, he can wax on about it as long as you like.
    ->2a

=== 1g ===

I suppose a wise enough individual could do it rather quickly, but the guards would get mighty angry if you just went around doing it all day. Best to only rip one down if you had to.
    ->2a

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

{
-campCenterFirstHiddenPassageFound:
\*Temple grumbles under his breath.* Some bastard wants to tear a hole in the wall while my back is turned? If I catch 'em I'm gonna skin 'em alive, I swears it.
-else:
You're back. Did you get turned around?
}

{
-templeMentionedBackground:
    +The Stolen Cities?
        ->1f
}

{
-templeExplainedPatches and wisdom >= 2:
    +Could I remove the patch jobs if I wanted to? <Wis {wisdom}/2>
        ->1g
}

{
-not askedTempleAboutSampson:
    +I'm looking for Sampson. Do you know where he is?
        ->1b
}

    +Well, it's been nice but I've gotta get going.
        Watch yourself out there.
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