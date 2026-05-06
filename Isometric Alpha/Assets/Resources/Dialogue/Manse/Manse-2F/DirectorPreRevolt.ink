VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerIndex = 0
VAR directorIndex = 1
VAR takacsFirstIndex = 2
VAR takacsSecondIndex = 3

VAR playerName = ""

->1a

=== 1a ===

duckMusic()
setToTrue(hadTakacsConvoInDirectorsOffice)
setNPCFacing({directorIndex},SW)

stopAllFades()
deactivate({playerIndex})
disableDialogueUI()
changeCamTarget({directorIndex})
slowFadeBackIn(5)
wait(2.5)

createEffect(SmokeBomb,-1,-1)
wait(0.25)
activate({takacsFirstIndex})

wait(2.5)

enableDialogueUI()

Ah, Takács. You've made quite the journey to speak to me in person. What news from my nephew?

changeCamTarget({takacsFirstIndex})

\*With a voice like a thousand chittering insects, Takács speaks.* This is not a social visit, Lord Kálnoky. The rider you sent reached Pharos yesterday morning. I am here to investigate your lack of progress, and give you my master's reply.

changeCamTarget({directorIndex})

The situation is poor. We have halted all digging in the mines. We will not be able to continue until the count releases to me the requested soldiers.

changeCamTarget({takacsFirstIndex})

The Confederation's movements are being watched by Mason spies. Count Kálnoky is reluctant to increase traffic across the border. 

changeCamTarget({directorIndex})

Stones cannot be bled, Vada. We will make no more gains until we are given the means to produce them: more swords, and more slaves. Unless you have been sent in their place, of course.

changeCamTarget({takacsFirstIndex})

I have not been given permission to assist you in that manner.

changeCamTarget({directorIndex})

Béla certainly keeps you on a short leash. Do the counts hold no love for their pets anymore?

disableDialogueUI()

createEffect(SmokeBomb,-1,-1)
wait(0.33)
deactivate({takacsFirstIndex})

wait(.5)

createEffect(SmokeBomb,2,0)
wait(0.33)
activate({takacsSecondIndex})
wait(0.5)

changeCamTarget({takacsSecondIndex})
enableDialogueUI()

You feign toughness, old lord, but I can see the beads of sweat on your neck. Fear fills your stomach like rain does a grave.

setNPCFacing({directorIndex},NW)
changeCamTarget({directorIndex})

Leave me be, spirit! Be useless to me somewhere else!

changeCamTarget({takacsSecondIndex})

\*A noise unlike laughter escapes the figure's headdress.* My predecessor remembers you well. Even as a boy, you've always been afraid of spiders.

disableDialogueUI()
createEffect(SmokeBomb,2,0)
wait(0.33)
deactivate({takacsSecondIndex})
wait(2.5)
enableDialogueUI()
changeCamTarget({directorIndex})

setNPCFacing({directorIndex},SW)

Blasted thing. Why send it at all if not with news of aid? 

fadeToBlack()

changeCamTarget({playerIndex})
activate({playerIndex})

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE