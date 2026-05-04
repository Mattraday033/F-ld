VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerIndex = 0
VAR directorIndex = 1
VAR takacsIndex = 2

VAR playerName = ""

->1a

=== 1a ===

setToTrue(hadTakacsConvoInDirectorsOffice)

stopAllFades()
deactivate({playerIndex})
disableDialogueUI()
changeCamTarget({directorIndex})
slowFadeBackIn(5)
wait(4)

createEffect(SmokeBomb,-1,-1)
wait(0.25)
activate({takacsIndex})

wait(3)

enableDialogueUI()

Ah, Takács. You've made quite the journey to speak to me in person. What news from my nephew?

changeCamTarget({takacsIndex})

\*With a voice like a thousand chittering insects, Takács speaks.* This is not a social visit, Lord Kálnoky. The rider you sent reached Pharos yesterday morning. I am here to investigate your lack of progress, and give you my master's reply.

changeCamTarget({directorIndex})

The situation is poor. We have halted all digging in the mines. We will not be able to continue until the count releases to me the requested soldiers.

changeCamTarget({takacsIndex})

The Confederation's movements are being watched by Mason spies. Count Kálnoky is reluctant to increase traffic across the border. 

changeCamTarget({directorIndex})

Stones cannot be bled, Vada. We will make no more gains unless we are given the means to produce them: more slaves, and more swords. Unless you have been sent in their place, of course.

changeCamTarget({takacsIndex})

I have not been given permission to assist you in that manner.

changeCamTarget({directorIndex})

Béla certainly keeps you on a short leash. Do the counts hold no love for their pets anymore?

changeCamTarget({takacsIndex})

You play tough, old lord, but I can see the beads of sweat on your neck. Fear fills your stomach like rain does a grave.

changeCamTarget({directorIndex})

Leave me be, spirit! Be useless to me somewhere else!

changeCamTarget({takacsIndex})

\*A noise unlike laughter escapes the figure's headdress.* My predecessor remembers you well. Since you were a boy, you've always been afraid of spiders.

disableDialogueUI()
createEffect(SmokeBomb,-1,-1)
wait(0.33)
deactivate({takacsIndex})
wait(3)
enableDialogueUI()
changeCamTarget({directorIndex})

Blasted thing. Why send it at all if not with news of aid? 

fadeToBlack()

changeCamTarget({playerIndex})
activate({playerIndex})

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE