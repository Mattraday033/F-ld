VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR givenFullExplanation = false

VAR givenTutorialQuest = false
VAR toldKastorOfThatchsFate = false
VAR toldToInvestigateScreamingInThatchsHut = false
VAR kastorReactedToHostility = false

VAR metThatch = false
VAR foundSlate = false

VAR thatchRemovedTutorialRubble = false
VAR toldThatchAboutSlate = false

VAR thatchIndex = 1
VAR removableRubbleIndex = 2

VAR playerName = ""

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

=== 1a ===

setToTrue(metThatch)
playAnimation({thatchIndex}, OOC_Idle_Front)
faceOppositePlayer({thatchIndex})

\*This man is toweringly tall, with the hard muscles one acquires after years of labor. He has deep circles under his eyes, and a weariness about him that you must work hard at to keep from catching. He regards you for a moment.* Who are you? 

{
-toldToInvestigateScreamingInThatchsHut:
    +I'm {playerName}. Kastor sent me to investigate all the screaming coming from this hut.
        ->2a
}
    +I'm looking for a man named Thatch. Is that you?
        ->1b


=== 1b ===

I'm Thatch. Am I needed for more work?

    +No, I'm here for something else. Is that why you look like someone rolled you down a craggy hill? All the work?
        ->1ba
    +Good, I've been sent to find you. We have some things to discuss.
        keepDialogue()
        I'm in no mood for discussion. I've never been more exhausted, and the guards may return soon to put me back to work. Ask what you need quickly, and then let me rest.
        ->1bc

=== 1ba ===

\*Thatch glares at you.* You'd look like this too if they put you to work like I've been. The guards can't let too many of us out of our huts during this lockdown so they pick the biggest ones to get the most work done per slave. I just got back from working all night.  

    +Don't complain to me, we've all got it bad right now. 
        //The guards have been working me twice as hard during this lockdown. 
        ->1bb
    +Can't be as bad as sitting inside all day. All I've got to do is sleep and eat. *Yawn loudly.*
        ->1bb
    +I didn't mean to offend, I was just curious what happened to you.
        ->1bc

=== 1bb ===

Look, I'll say or do anything to get a bit of rest. What will get you to shut up and leave me alone the fastest?

{
-toldToInvestigateScreamingInThatchsHut:
    +Fine, I'll cut to it. Kastor has a plan to escape. He want's you in on it.
        ->1d
}
    +Hopefully "anything" includes joining an escape attempt, because that's why I'm here.
        ->1d

=== 1bc ===

I understand, but I'm beyond caring at this point. Ask what you need, and then let me rest.

{
-toldToInvestigateScreamingInThatchsHut:
    +Fine, I'll cut to it. Kastor has a plan to escape. He want's you in on it.
        ->1d
}
    +I'm planning to escape. I need your help with that.
        ->1d

=== 1d ===

Escape? *Thatch considers it.* Maybe it's the lack of sleep talking, but never having to move another stack of rocks around is sounding pretty good right now. But you're going to have to do something for me first.

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
        No, I've been working all night and I got back less than an hour ago. But I can guess what happened.
        ->explanationOfHutState(->2b)
    +Do you know what happened?
        I've been working all night and I got back less than an hour ago. But I can guess what happened.
        ->explanationOfHutState(->2b)

=== 2b ===

+Sounds like you've got as much a reason to hate the guards as anyone. I've actually been sent here to get your help on an escape plan. Interested?
    ->1d

=== explanationOfHutState(->divert) ===

~givenFullExplanation = true

I came back from my labors to find my hut ransacked. That's why there's all this rubble everywhere. These shacks are rickety at the best of times, and whatever the guards were doing in here while I was away has collapsed some of it.

I have a hutmate, Slate, that I'm worried about. He's got a temper, and he's made some enemies among the guards. But the guards haven't retaliated because a lot of them are afraid of me. My guess is that those cowards snuck in here while I was away and did something to Slate, but I'm too tired to do much about it on my own.

->divert

=== explanationOfThatchsTask ===

With the two of us together, we might be able to get to the back of the hut and find out what happened to him. If you're serious about escaping, then clobbering some guards with me shouldn't be much of a request. 

    ->tutorialChoices

=== snoozing ===

\*Thatch is slumped against the hut wall. His eyes are closed and soft snoring can be heard.*

    +\*Wake Thatch.*
        playAnimation({thatchIndex}, OOC_Idle_Front)
        faceOppositePlayer({thatchIndex})
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
-foundSlate:
    +I've actually already found your friend. He was killed by a guard named Vazul.
        ->skippedTutorial
-else:
    +I'll help you get to your friend.
        ->acceptedTutorial
    +I will need to think about it. I'll be back.
        playAnimation({thatchIndex}, Death_Back)
        setNPCFacing({thatchIndex},NW)
        ->Close
}



=== acceptedTutorial ===

Good. I shall move the first bit of rubble so we can get past, then I'll follow your lead. And you'd best be ready for a fight. I'm not about to let them get away with hurting Slate.

setToTrue(thatchRemovedTutorialRubble)
activateQuestStep(Look for Thatch, Slate's fate.)

->addThatchToParty(true)

=== skippedTutorial ===

setToTrue(toldThatchAboutSlate)
I think... I think I already knew that. Somewhere, deep down. But thank you for telling me this. I have no reason to stay any longer.
{
-kastorReactedToHostility:
    finishQuest(Look for Thatch, true, Thatch is willing.)
-toldToInvestigateScreamingInThatchsHut:
    activateQuestStep(Look for Thatch, Return to Kastor.)
}

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