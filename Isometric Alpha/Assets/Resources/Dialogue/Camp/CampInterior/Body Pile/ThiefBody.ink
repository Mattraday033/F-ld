VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR taborIndex = 1
VAR weftIndex = 2

VAR skipTutorialIndex = 0
VAR intimidateTutorialIndex = 1
VAR cunningTutorialIndexIndex = 2

VAR toldByTaborToBuildHouses = false
VAR acceptedTaborSkillTutorial = false

VAR playerName = ""

->1a

=== 1a ===

fadeToBlack(true,false)

wait(1.5)
activateQuestStep(Comb the Bodies,Exit the body pile.)
setToTrue(foundThiefsRing)

movePlayerPos(4,-2)
setFacing(SW)
activate({taborIndex})
activate({weftIndex})

fadeBackIn(60)

prepItem()

That's the ring we were told to find. I'll take that off of you.

exchangeItemForXP(Lovashi Ring,1,250)

You did well. Those larger bats can tear a person in half if you can't hold your own. I've seen it more than once.

You may have picked up some things off of the rest of these poor souls. Don't worry, I won't search your pockets, their executioners should have taken any contraband they were caught with off their bodies before they dumped them. Whatever else they had is yours to keep.

I'll meet you back at the top of the ladder. Don't spend too long down here, or I'll take it out of your meal time.

fadeToBlack()

deactivate({taborIndex})
deactivate({weftIndex})

fadeBackIn(60)

->Close

=== 1b ===

    ->1c

=== 1c ===

->Close

=== 1d ===

->Close

=== Close ===

close()

->DONE