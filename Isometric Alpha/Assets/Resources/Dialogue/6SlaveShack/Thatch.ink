VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR givenFullExplanation = false

VAR givenTutorialQuest = false
VAR toldKastorOfThatchsFate = false

VAR metThatch = false
VAR slateFound = false

VAR thatchRemovedTutorialRubble = false
VAR toldThatchAboutSlate = false

VAR thatchIndex = 1
VAR removableRubbleIndex = 2

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
    -thatchRemovedTutorialRubble or toldThatchAboutSlate:
        ->afterAddingThatchToParty
    -metThatch:
        ->snoozing
    -givenTutorialQuest:
        ->1a
    -else:
        ->deepSnoozing
    }

//Removed old thatch dialogue on 6/22/2025, to get it back try before that in version control

=== 1a ===

setToTrue(metThatch)

\*This man is toweringly tall, with the hard muscles one acquires after years of labor. He has deep circles under his eyes, and a weariness about him that you must work hard at to keep from catching. He regards you for a moment.* Who are you? 

    +I'm looking for a man named Thatch. Is that you?
        ->1b
    +I'm {playerName}. Kastor sent me to investigate all the screaming coming from this hut.
        ->2a

=== 1b ===

I'm Thatch. Am I needed for more work?

    +No, I'm here for something else. Is that why you look like someone rolled you down a craggy hill? All the work?
        //The guards have been working me twice as hard during this lockdown. They can't let too many slaves out of their huts at once so they just pick the biggest ones to get the most out of us. I just got back from working all night, but I'm afraid they're going to  
        ->1ba
    +Good, I've been sent to find you. We have some things to discuss.
        keepDialogue()
        I'm in no mood for discussion. I've never been more exhausted, and the guards may return soon to put me back to work. Ask what you need quickly, and then let me rest.
        ->1bc

=== 1ba ===

\*Thatch glares at you.* You'd look like this too if they put you to work like I've been. The guards can't let too many of us out of our huts during this lockdown so they just pick the biggest ones to get the most out of us. I just got back from working all night.  

    +Don't complain to me, we've all got it bad right now. 
        //The guards have been working me twice as hard during this lockdown. 
        ->1bb
    +Can't be as bad as sitting inside all day. All I've got to do is sleep and eat. *Yawn loudly.*
        ->1bb
    +I didn't mean to offend, I was just curious what happened to you.
        ->1bc

=== 1bb ===

Look, I'll say or do anything to get a bit of rest. What will get you to shut up and leave me alone the fastest.

    +Fine, I'll cut to it. Kastor has a plan to escape. He want's you in on it.
        ->1d
    +Hopefully "anything" includes joining an escape attempt, because that's why I'm here.
        ->1d

=== 1bc ===

I understand, but I'm beyond caring at this point. Ask what you need, and then let me rest.

    +Fine, I'll cut to it. Kastor has a plan to escape. He want's you in on it.
        ->1d

=== 1d ===

Escape? *Thatch considers it.* Maybe it's the lack of sleep talking, but never having to move another stack of rocks around is sounding pretty good right now. But you're going to have to do something for me, first.

    +What is it?
        {
        -givenFullExplanation:

            combineDialogue()
            I want to make these guards pay for whatever they did to Slate. 

            ->explanationOfThatchsTask
        -else:
            ->explanationOfHutState(->explanationOfThatchsTask)
        }


=== 2a ===

Screaming? Damn it all. I didn't... I should have been here.

    +I'm guessing the screams weren't from you then?
        No, I wasn't the one screaming. I've been working all night, I only got back less than an hour ago. But I can guess what happened.
        ->explanationOfHutState(->2b)
    +Do you know what happened?
        I've been working all night, I only got back less than an hour ago, so I wasn't here for it all. But I can guess what happened.
        ->explanationOfHutState(->2b)

=== 2b ===

+Sounds like you've got as much a reason to hate the guards as anyone. I've actually been sent here to get your help on an escape plan. Interested?
    ->1d

=== explanationOfHutState(->divert) ===

~givenFullExplanation = true

I came back from my labors to find my hut ransacked. That's why there's all this rubble everywhere. These shacks are rickety at the best of times, and whatever the guards were doing in here while I was away has collapsed portions of it.

I have a hutmate, Slate, that I'm worried about. He's got a temper, and he's made some enemies among the guards. But the guards haven't done anything about it because a lot of them are afraid of me. My guess is that those cowards snuck in here while I was away and did something to Slate, but I'm too tired to do much about it on my own.

->divert

=== explanationOfThatchsTask ===

With the two of us together, we might be able to get to the back of the hut and find out what happened to him. If you're serious about escaping, then clobbering some guards with me shouldn't be much of a request. 

    ->tutorialChoices

=== snoozing ===

\*Thatch is slumped against the hut wall. His eyes are closed and soft snoring can be heard.*

    +\*Wake Thatch.*
        \*Thatch startles awake and looks up at you.* You're back, and just as I got in a good position, too. Are you ready to begin? 
            ->tutorialChoices
    +\*Leave.*
        ->Close

=== deepSnoozing ===

\*This slave is slumped against a wall, snoring softly. He waves off any attempts to disturb him.*

    +\*Leave.*
        ->Close

=== tutorialChoices ===

{
-slateFound:
    +I've actually already found your friend. He was killed by a guard named Vazul.
        ->skippedTutorial
-else:
    +I'll help you get to your friend.
        ->acceptedTutorial
    +I will need to think about it. I'll be back.
        ->Close
}



=== acceptedTutorial ===

Good. I shall move the first bit of rubble so we can get past, then I'll follow your lead. And you'd best be ready for a fight, I'm not about to let them get away with hurting Slate.

setToTrue(thatchRemovedTutorialRubble)
activateQuestStep(Look for Thatch, 1)

->addThatchToParty(true)

=== skippedTutorial ===

I think... I think I already knew that. Somewhere, deep down. But thank you for telling me this. I have no reason to stay any longer.

activateQuestStep(Look for Thatch, 2)
setToTrue(toldThatchAboutSlate)

->addThatchToParty(false)

=== addThatchToParty(removeRubble) ===

fadeToBlack()

{
-removeRubble:
    deactivate({removableRubbleIndex})
}

addToPartyWithoutPopUp({thatchIndex})
deactivate({thatchIndex})

fadeBackIn(60, false)

->Close

=== afterAddingThatchToParty ===

\*Thatch looks up at you with a fatigued stare.* Do you need my assistance again?

+Yes, follow me.
    ->addThatchToPartyAfterTutorial

+No, stay here and get some rest.
    ->Close


=== addThatchToPartyAfterTutorial ===

fadeToBlack()

deactivate({thatchIndex})
addToParty({thatchIndex})

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE  