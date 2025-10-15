VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR heardTaborsLesson = false

VAR taborIndex = 1
VAR feherIndex = 2
VAR otherSlavesIndex = 3

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
-heardTaborsLesson:
->2a
-else:
->1a
}

=== 1a ===

... and that's why it's imperative that the lesson be delivered in this manner. The pain conveys import, the blood pounding in your ears lets in only the sound of the teacher's voic- what's this? Another student here to learn at the feet of the teacher? Sit with the others, and don't make a sound.

    +Yessir!
        ->1b

    +My apologies, sir, but I am on a task for Guard László and I can't be late. *Show badge*
        Fine then, move along.
        ->Close


=== 1b ===

fadeToBlack(true, false)

moveToLocalPos(-15.5,-9.9)
setFacing(NE)

fadeBackIn(60)

Now where was I... curses, lost my place. I guess that means I have to start over.

changeCamTarget({feherIndex})

\*Sobs.*

changeCamTarget({taborIndex})

From the top: I am Chief Correctional Officer Tabor. I am in charge of rehabilitation in this camp. I am not a lowly guard, nor do I oversee any work in the mines. As such, you will not address me as 'Guard' Tabor, or 'Overseer' Tabor, but 'Chief' Tabor. Is that clear?

    +Yes, Chief Tabor!
        ->1c
    +\*Say nothing*.
        ->1ba

=== 1ba ===

changeCamTarget({otherSlavesIndex})

Yes, Chief Tabor!

changeCamTarget({taborIndex})

->1c

=== 1c ===

Good! Now I know what you must be asking. What is the purpose of a Correctional Officer, if the people he is supposed to correct all die before they are released? A short sighted question, but I will still answer it!

The Gods, in their infinite wisdom, have decreed that after we die, we all will be given a home in which to rest before we are reborn. There we will host every person we have ever met, every person we have ever wronged in our life, and will be given the opportunity to reconcile our differences. 

You may all know this as 'the Posthumous Hearth', or simply 'going to one's hearth' after we die. I imagine my hearth will look something like Grammy Tabor's stead, and will smell of her wonderful coney stew. Yours, if you're lucky, will be similarly homey and warm!

But therein lies the rub. If we allow you murderers and miscreants to go to your hearth's as ill-mannered malefactors, we'll be clogging up the afterlife with evil men and poor conversation. You may be less than nothing now, but I promise by the time you leave this camp you will know the error of your ways and be able to be reborn as goodly, contributing members of society.

\*Tabor cracks his whip, and Feher suffers another lash across his back.*

Look at Feher here. Poor Feher. His latest crime was reminiscing about horsemeat stew. Not your first time on the flogging block, is it Feher?

changeCamTarget({feherIndex})

\*Sobs.*

changeCamTarget({taborIndex})

No it is not. Now, I could just keep flogging Feher, but I would remiss if I didn't use this as a teachable moment. Who here can tell me what Feher did wrong?

->1ca

=== 1ca ===

    +He ate horsemeat?
        ->1cb
    +He wants to eat more horsemeat?
        ->1cd
    +He was caught?
        ->1cc
    +\*Say nothing.*
        ->1ce

=== 1cb ===

Yes, it was a sin to eat one of the Beast God's favorite creations. But he did that before he ever came to the camp. I'm talking about what he did wrong recently.

->1ca

=== 1cc ===

No, and I'll forgive your lip because you're new. Never let it be said that I am not merciful.

changeCamTarget({feherIndex})

\*Sobs.*

changeCamTarget({taborIndex})

Anyone else have the answer?

->1ca

=== 1cd ===
prepForItem()

Exactly right! Extra rations for you.

giveItem(0,0,1)

->1d

=== 1ce ===

changeCamTarget({otherSlavesIndex})

He wants to eat a horse?

changeCamTarget({taborIndex})

Exactly right! Extra rations for you.

->1d

=== 1d ===

Feher still lusts in his despicable heart for horseflesh. Despite the fact that the Gods have blessed horses, as they did us, with the gift of language. Despite the fact that horses have helped build the glorious Lovashi Confederation as much as humans have. Despite the fact that we have horses in this very camp! He thought only of his own hunger, and not of the merits of others.

\*Tabor cracks his whip and strikes Feher again.*

changeCamTarget({feherIndex})

\*Sobs.*

changeCamTarget({taborIndex})

I don't care what you did before coming to this camp. That is of no consequence to me. What I care about is that you learn from the mistakes of your past and go to your hearth a better person. One who can see all of the Gods creations as their equal. 

setToTrue(heardTaborsLesson)

Feher is going to spend the rest of the day tied to this post unless, Gods forbid, one of you needs a lesson yourself. Now, Priest Rikard has asked me to remind you that his sermons are open to any volunteers. The Temple is behind you and on your right. Otherwise, stay with me and I will return you to your quarters. Dismissed!

->Close

=== 1e ===

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

What are you doing back here. Get back to work!

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