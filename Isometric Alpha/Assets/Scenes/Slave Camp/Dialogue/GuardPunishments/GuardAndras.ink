VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR spokeWithAndrasAtPunishment = false

VAR gaveAGuardToTheCrowd = false
VAR executedAnyGuard = false
VAR didNotExecuteAndras = false
VAR gaveAndrasFiftyLashes = false

VAR intimidatedAndras = false
VAR gotKeyFromJanos = false
VAR andrasBarricadePassUsed = false

VAR andrasAgreedToBeBranded = false
VAR andrasAgreedToMonthSeparation = false
VAR andrasSworeGodOath = false

VAR andrasIndex = 1
VAR crowdIndex = 2
VAR janosIndex = 3

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

changeCamTarget({andrasIndex})

{
-spokeWithAndrasAtPunishment:
keepDialogue()

What is to be my fate?
    ->1b
}

setToTrue(spokeWithAndrasAtPunishment)

{
-gaveAGuardToTheCrowd:
->1ab
-executedAnyGuard:
->1aa
-else:
->1a
}

=== 1a === 

\*As you approach, András forces himself to break eye contact with Janos and looks up at you.* {playerName}. Please, assure me this is all a formality, and not what it looks like.

    +It's necessary, András. It'll only take a moment.
        ->1b
    +Quiet. You will only speak when addressed.
        keepDialogue()
        \*András's eyes widen, and he looks fearfully back towards Janos.*
        ->1b

=== 1aa === //executedAnyGuard

{playerName}, please, whats going on? What's going to happen to me?

    +Calm yourself, András. We have things to discuss.
        keepDialogue()
        \*András steadies his breathing.* What things?
        ->1b
    +Quiet. You will only speak when addressed.
        keepDialogue()
        \*András's eyes widen, and he looks fearfully back towards Janos.*
        ->1b

=== 1ab === //gaveAGuardToTheCrowd

Oh Gods, they ripped them to shreds. Please, {playerName}, don't do this. I don't want to die like that!

    +Calm yourself, András. We have things to discuss.
        keepDialogue()
        \*András tries to stready his breathing.* What things?
        ->1b
    +Quiet. You will only speak when addressed.
        keepDialogue()
        \*András's eyes widen, and he looks fearfully back towards Janos.*
        ->1b

=== 1b ===

Ok... ok. What do we need to do?

    +I have some questions I must ask you.
        ->1c
    +Your aid to the rebellion has earned you your freedom. Rise as a friend, András. *Undo András's bonds.*
        ->2a
    +Whatever your actions, you were still responsible for keeping us in chains. That demands punishment.
        ->3a
    +I will be back.
        ->Close

=== 1c ===

Ask them then.

    +What are your crimes as you see them?
        ->1e
    +How did you come to be a guard at this camp?
        ->1d
    +What were your contributions to the rebellion?
        ->1f
    +Do you have any regrets?
        ->1g
    +Will any of the freed vouch for your life?
        ->1h
    +I am finished with my questions.
        keepDialogue()
        What is to be my fate?
        ->1b
    
=== 1d ===

It isn't much of a story. My father and mother had both been in the service of the Director since before my birth. My mother had fought with him as a fellow officer during the Emancipation Conflict, and my father had ridden with him as a part of the Director's bodyguard. Since a young age, I had wished to serve as one of his household guard.

When the Director came to this camp, because of my family's history of service, I was entrusted to come along as a guard. And again, because of the respect the Director held for my family, I was entrusted to guard a set of keys to many locations within the camp. It was also why I could sneak away for long periods: my reputation would often gain me the benefit of the doubt.

    +And you did nothing yourself to gain this reputation for trustworthiness? 
        ->1da

=== 1da ===

I simply did nothing to ruin it.

    +But you still betrayed the Director and joined our cause. Some would call you a traitor for that.
        ->1db

=== 1db ===

\*András grimaces.* I certainly think my parents would agree with you.

    +Had Janos and you never met, what would you have done during the rebellion?
        ->1dc

=== 1dc ===

keepDialogue()

I would have died at my post. Had I never met Janos, I would not have had the cause I did to question my actions. And had we not been struck down in the initial riots, or trapped somewhere, my squad would have been deployed to the second floor of the Manse. I would not be here speaking with you now; my body would instead lie moldering with the others.

    ->1c

=== 1e === //What are your crimes as you see them?

    keepDialogue()

    I contributed to the suffering of the branded. I supported their imprisonment, I punished them when they slowed their work, and I... ignored their cries when they asked for mercy. *András hangs his head.*

    ->1c
        
=== 1f ===
keepDialogue()

{
-gotKeyFromJanos and andrasBarricadePassUsed:

I relinquished the key to the mine's armory without a fight. I also convinced a group of guards to leave their barricade and surrender. Finally, I stayed with Janos and protected them during the riots.

-gotKeyFromJanos:

I relinquished the key to the mine's armory without a fight. I also stayed with Janos and protected them during the riots.

}

    ->1c

=== 1g === //Do you have any regrets?

keepDialogue()

I regret that I did not do more to help the cause. I confessed to Janos before you and I had ever met that I wished we could both be free of the camp. I regret I took no action towards that goal before you came to us. 

        ->1c

=== 1h === 

keepDialogue()

Janos will, I'm sure of it. And I suppose I had hoped you would as well.
        ->1c

=== 2a === //Your aid to the rebellion has earned you your freedom. Rise as a friend, András. *Undo András's bonds.*

\*András gets to his feet.* Oh thank the Mother. I thought... I'm just glad it's all over.

{
-andrasAgreedToBeBranded:
    ->2aa
-else:
    ->2ab
}

=== 2aa ===
+\*Address the crowd.* This is András. He helped in gathering the tools that you used against the guards. When I questioned his quality, he agreed to become one of the branded rather than give up on the one he loves. Mark him as a friend, and as a brother!
        ->2z

=== 2ab ===
    
+\*Address the crowd.* This is András. He helped in gathering the tools that you used against the guards. Remember his contributions and give him your gratitude.
        ->2z

=== 2z ===

~didNotExecuteAndras = true

setToTrue(didNotExecuteAndras)

changeCamTarget({crowdIndex})

activateQuestStep(Deal With the Prisoners, 7)

\*The crowd erupts in cheers.*

fadeToBlack()

deactivate({andrasIndex})

fadeBackIn(60)
    ->Close

=== 3a === 

{
- executedAnyGuard or gaveAGuardToTheCrowd:
\*András looks to Janos. Tears well in his eyes.*
-else:
Wait! Wait, please! 
}
    
    +Fifty lashes, for crimes against the branded, and to remind you of what shall be in store for you if you should ever mistreat Janos.
        ->3b
    +Execution, to be carried out immediately. May it be swift and painless.
        ->3c
    +The crowd will be your judge. May they not find you wanting.
        ->3d
    +On second thought, I don't really see the point in punishing you.
        
        keepDialogue()
        
        {
        - executedAnyGuard or gaveAGuardToTheCrowd:
            \*András's head snaps back towards you.* What?
        -else:
            \*András lets the tension leave his gut.* Thank the Gods.
        }
    
        ->1b


=== 3b ===

setToTrue(didNotExecuteAndras)
setToTrue(gaveAndrasFiftyLashes)

activateQuestStep(Deal With the Prisoners, 8)

changeCamTarget({janosIndex})

This is unnecessary! Don't do this!

changeCamTarget({andrasIndex})

\*András mutters under his breath.* You can take this. You can take this.

fadeToBlack()

deactivate({andrasIndex})

fadeBackIn(60)

->Close

=== 3c ===

setToTrue(executedAndras)

activateQuestStep(Deal With the Prisoners, 9)

changeCamTarget({janosIndex})

Please! I'll vouch for him! Don't kill him!

fadeToBlack()

kill({andrasIndex})

fadeBackIn(60)

->Close

=== 3d ===

setToTrue(gaveAndrasToTheCrowd)

activateQuestStep(Deal With the Prisoners, 10)

changeCamTarget({janosIndex})

Don't you dare touch him! Get your hands off him! Get-

fadeToBlack()

kill({andrasIndex})
kill({janosIndex})

fadeBackIn(60)

->Close

=== 3e ===

->Close

=== 3f ===

->Close

=== 3h ===

->Close

=== 3i ===

->Close

=== 3j ===

->Close

=== 3k ===

->Close

=== 3l ===

->Close

=== 3m ===

->Close

=== 3n ===

->Close



=== Close ===

close()

->DONE