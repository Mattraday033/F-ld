
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

activateQuestStep({saveDibberQuestName},Kill the bats.)

As I suspected, our path is blocked by a group of bats. I've fought a few during my time in the mines, they can be nasty in a scrap. But you'll have to face far worse before we are free.

->STR_1b

=== STR_1b ===

When you're ready, approach the bats with caution. I will observe how you and Thatch handle this.

    +Neat
        ->Close

=== DEX_1a ===

DEX PH 1

    +Neat
        ->Close

=== DEX_1b ===

DEX PH 2

    +Neat
        ->Close

=== CHA_1a ===

CHA PH 1

    +Neat
        ->Close

=== CHA_1b ===

CHA PH 2

    +Neat
        ->Close

=== WIS_1a ===

WIS PH 1

    +Neat
        ->Close

=== WIS_1b ===

WIS PH 2

    +Neat
        ->Close

=== Close ===

close()

->DONE