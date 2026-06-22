
VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerName = ""

VAR kastorIndex = 1
VAR kastorStrIndex = 2
VAR kastorDexIndex = 3
VAR kastorChaIndex = 4
VAR kastorWisIndex = 5

VAR duringKastorSkillTutorial = false
VAR startedKastorIntimidateTutorial = false
VAR finishedKastorIntimidateTutorial = false
VAR startedKastorCunningTutorial = false
VAR finishedKastorCunningTutorial = false
VAR startedKastorLeadershipTutorial = false
VAR finishedKastorLeadershipTutorial = false
VAR startedKastorObservationTutorial = false
VAR finishedKastorObservationTutorial = false

VAR savedDibber = false

VAR saveDibberQuestName = "Save Dibber!"

{
-startedKastorObservationTutorial:
    ->WIS_1b
-finishedKastorLeadershipTutorial:
    ->WIS_1a
-startedKastorLeadershipTutorial:
    ->CHA_1b
-finishedKastorCunningTutorial:
    ->CHA_1a
-startedKastorCunningTutorial:
    ->DEX_1b
-finishedKastorIntimidateTutorial:
    ->DEX_1a
-startedKastorIntimidateTutorial:
    ->STR_1b
-duringKastorSkillTutorial:
    ->STR_1a
}

=== STR_1a ===

setToTrue(startedKastorIntimidateTutorial)
activateQuestStep({saveDibberQuestName},Kill the bats.)

As I suspected, our path is blocked by a group of bats. I've fought a few during my time in the mines, they can be nasty in a scrap. But you'll have to face far worse before we are free.

->STR_1b

=== STR_1b ===

When you're ready, approach the bats with caution. I will observe how you and Thatch handle this.

    +On it.
        ->Close

=== DEX_1a ===

resetAllSkills()
setToTrue(startedKastorCunningTutorial)
activateQuestStep({saveDibberQuestName},Use the crank.)

Good work with those bats. I'm glad to see you two can work together in a fight. 

The next patch of rubble looks like we could get over it ourselves, but I don't know what state Dibber is in. We'll want this rubble moved in case we have to carry him out.

When the guards were trying to patch up this hut before the lockdown, they set up a crank towards the back wall. You should be able to use it to clear us a path while I clamber over the rubble and help from the other side.

    +Understood.
        ->activateDeactivate(kastorChaIndex,kastorDexIndex)

=== DEX_1b ===

Use the crank to clear the rubble out of the way so we can get Dibber out of there.

    ->Close

=== CHA_1a ===

setToTrue(startedKastorLeadershipTutorial)
activateQuestStep({saveDibberQuestName},Clear the rubble.)

This next bit of debris is going to be tricky. Trust me, after working in the mine for months you acquire a knack for these things.

The rubble in front of us is too large and bulky for the crank. You'll need to get on either side of it and work together to move it without hurting yourselves.

Thatch is much bigger than I am, and you haven't worked a shift yet, so you two will do the brunt of the lifting from this side. I'll squeeze through and do what I can from the other.

    +On it.
        ->activateDeactivate(kastorWisIndex,kastorChaIndex)

=== CHA_1b ===

This is nothing compared to working in the mine. Just have each of you stand on opposite sides of the rubble and take one chunk down at a time.

    +\*Leave*
        ->Close

=== WIS_1a ===

activateQuestStep({saveDibberQuestName},Remove the wall patch.)

We're almost there. It looks like the collapse that trapped Dibber covers the entire entrance to his bedding. And as I feared, this debris pile is the only thing keeping the central supports from falling apart completely.

To get him out of there, we will need to remove one of the wall patches we installed a few weeks ago. It's the boarded up section of the wall just past that chair, there. Temple is some sort of wizard with boards; he put them up so a smart individual could remove them safely and easily.

->WIS_1b

=== WIS_1b ===

When the patch is down, assess how badly hurt Dibber is. I'll be right behind you.

    +Understood.
        ->Close

=== activateDeactivate(onIndex, offIndex) ===

fadeToBlack()

activate({onIndex})
deactivate({offIndex})

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE