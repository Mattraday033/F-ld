VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR taborIndex = 1
VAR weftIndex = 2
VAR guardIndex = 3
VAR taborNextIndex = 4

VAR startedTaborIntimidateTutorial = false
VAR finishedTaborIntimidateTutorial = false

VAR startedTaborCunningTutorial = false
VAR finishedTaborCunningTutorial = false

VAR startedTaborObservationTutorial = false
VAR finishedTaborObservationTutorial = false

VAR startedTaborLeadershipTutorial = false
VAR finishedTaborLeadershipTutorial = false

VAR playerName = ""


->1a

=== 1a ===

setToTrue(startedTaborIntimidateTutorial)
activateQuestStep(Chief Tabor,Kill the bat.)

I've taken some time to survey the site and it looks like yesterday's team made a mess of things, so you'll have to set right what they did wrong. You'll be doing the work, but if you don't have the Skills required for any of the tasks I give you, I'll step in and show you how it's done. But only because it's your first day, slave. Don't expect such kindness from me on the morrow. 

You'll notice that there are some bats trapped in the construction site behind me. It happens sometimes, they usually fly out of the mine around sunset and return at sunrise, but now and again they get lost and roost in the huts or other structures of the camp.

The one behind me probably thought the shade afforded by this half built shack was a good place to spend the night and was rudely awakened when the sun invaded it's nesting area, so it must be disoriented and angry beyond belief.

You will have to learn to fight the beasts eventually, so you might as well start now. Just look big and scary and hit it a few times and it will go down. If you disgrace yourself in there I will bail you out, but you'll have wished the bat had gotten the better of you. Get to it.

fadeToBlack()

deactivate({taborIndex})
deactivate({weftIndex})
activate({taborNextIndex})
fadeBackIn()

->Close

=== Close ===

close()

->DONE