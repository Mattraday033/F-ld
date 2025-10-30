VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR spokeWithPazmanAtPunishment = false

VAR gaveAGuardToTheCrowd = false
VAR executedAnyGuard = false

VAR didNotExecutePazman = false
VAR gavePazmanToTheCrowd = false
VAR gavePazmanFiftyLashes = false
VAR executedPazman = false

VAR allowedErvinToSpeakAtPazmansTrial = false

VAR pazmanIndex = 1
VAR crowdIndex = 2
VAR ervinIndex = 3
VAR ervinCloserIndex = 4

VAR playerName = ""

//changeCamTarget(int targetIndex)
//keepDialogue()
//setToTrue(string flagName)
//setToFalse(string flagName)
//activate(int index of gameobject you're activating)
//deactivate(int index of gameobject you're deactivating)
//activateQuestStep(string questTitle, int questStepIndex)
//prepForItem()
//giveItem(int listIndex, int itemIndex, int quantity)
//giveItems(int listIndex1, int itemIndex1, int quantity1 |
//          int listIndex2, int itemIndex2, int quantity2 |
//          ... etc)
//takeAllOfItem(string itemName)


changeCamTarget({pazmanIndex})

{
-spokeWithPazmanAtPunishment:

You have returned. Is it time?

    ->1c
}

setToTrue(spokeWithPazmanAtPunishment)

{
-gaveAGuardToTheCrowd:
->1ab
-executedAnyGuard:
->1aa
-else:
->1a
}

=== 1a ===

Gáspár wouldn't be surprised to hear that when I surrendered to you, I didn't think this far ahead. Are you going to let me go? Or am I to be the first to be executed?
    ->1b


=== 1aa ===

A guard is dead, and everyone cheers. I wonder if we're about to witness another execution.

    ->1b

=== 1ab ===

\*Pázmán keeps a strong face, but his breathing is quick and shallow. He bears a weak smile.* Any chance I can convince you to give me a head start before you chase me down and rip me to pieces? No? Worth a try I guess.
    ->1b

=== 1b ===

{
-gaveAGuardToTheCrowd: 
//1ab
    +A head start would do you no good. All it would give you is false hope.
        \*Pázmán chuckles meekly.* Wouldn't want that.
        ->1c
-executedAnyGuard:
//1aa
    +What you say next may yet sway my decision. Choose your words with care.
        Yes, of course. Sage advice.
        ->1c
-else:
//1a
    +I am still forming my verdict.
        Form away. But if you ask me, letting me go looks like an excellent option.
        ->1c
}

    +Quiet. You will only speak when addressed.
        \*Pázmán silently nods.*
        ->1c

=== 1c ===

    +I have some questions I must ask you.
        ->1d
    +When we offered you peace, you accepted it. For your actions, you may leave here with your life.
        ->2aa(->2f)
    +A moment of cooperation does not wipe away a career of oppression. Your punishment is due.
        ->2aa(->3a)
    +I will be back.
        ->Close

=== 1d ===

Ask anything you like. You'll find my answers as agreeable as I can make them.

    +What are your crimes as you see them?
        ->1e
    +How did you come to be a guard at this camp?
        ->1f
    +Why did you accept Overseer Gáspár's orders to abandon the branded to the worms?
        ->1g
    +If granted your freedom, what will you do with it?
        ->1h
    +Do you have any regrets?
        ->1i
    +Will any of the freed vouch for your life?
        ->1j
    +I am finished with my questions.
        What is your verdict to be?
        ->1c

=== 1e ===

keepDialogue()
I served under the Director, handling the branded and keeping them in line. I punished those who rejected their duties, certainly, but I always resolved to never punish them too severely. My father taught me that compassion is the best tool for teaching.

->1d

=== 1f ===

At one time, a long time ago, I trained to be a priest, as my father had been. But I had great difficulty learning my letters. Or perhaps it was that I never really cared to. In either case, eventually my father grew tired of failing to teach me anything and threw me out. I became a soldier soon afterwards, and many years later here I am, serving the Director as one of his house guard.

    +A priest? Of which God?
        keepDialogue()
        Most priests do not revere only one God. I learned prayers and sacrifices to them all: Beast, Sun, the Mother, etc. But my father gave services at the local temple to Harmony. Looking back, maybe that's why I became a soldier: a father who worshipped the Goddess of peace and cooperation wouldn't have approved of his son learning the warrior's trade.
        ->1d
    +I have more questions.
        ->1d

=== 1g ===

What could I do? I was but one man, and I knew the others would follow his example. If I had spoken out I could have been punished, or killed!
 
    +Guard Márcos disobeyed Gáspár's orders. He wasn't killed.
        keepDialogue()
        Uh, yes, that's true. But Márcos is special. He's a great fighter, and an upstanding man. Gáspár probably let him get away with it to save face.
        ->1d
    +I have more questions.
        ->1d

=== 1h === //If granted your freedom, what will you do with it?

keepDialogue()
I think I'll go back to the temple of Harmony in Carnassus and return to my studies as a priest. I'm sick of all this fighting. I just want it to end.
    ->1d

=== 1i ===

keepDialogue()
I have so many regrets. Working as a slaver has made me realize I've been wasting my life. I know now that it was wrong, and I just want to move on from it.

    ->1d

=== 1j === //Will any of the freed vouch for your life?

{
-wisdom >= 3:

\*Pázmán scans the crowd for moment, lingering for a moment at a slave behind you and to your left.* No one comes to mind.
    
    +Hmm. You seem to recognize Ervin. Perhaps he has something to add? <Wis {wisdom}/3>
        ->1ja
    +I see. I have more questions then.
        ->1d
-else:

keepDialogue()
\*Pázmán scans the crowd for moment.* No one comes to mind.
    
    ->1d
}

=== 1ja ===

\*Pázmán begins to sweat.* Maybe he does, but I'm sure it would be nothing important. Best to skip it.

    +\*Address the Crowd.* Everyone, be quiet. Ervin has difficulty speaking and I must hear what he has to say. Ervin, step forward.
        ->2a

=== 2aa(->divert) === 

changeCamTarget({ervinIndex})

\*Ervin waves his hands frantically to get your attention above the noise of the assembled freed prisoners.* Wait! I would speak!

    +\*Address the Crowd.* Everyone, be quiet. Ervin has difficulty speaking and I must hear what he has to say. Ervin, step forward.
        ->2a
    +Ervin, be still. You are not a part of these proceedings.
        changeCamTarget({ervinIndex})

        \*Ervin glares at you venomously, but does not continue his interruption.*
        ->divert

=== 2a === 

fadeToBlack(true, false)

setToTrue(allowedErvinToSpeakAtPazmansTrial)
deactivate({ervinIndex})
activate({ervinCloserIndex})
changeCamTarget({ervinCloserIndex})

fadeBackIn(60)

\*Ervin enunciates as well as his neck injury will allow. He takes his time speaking, putting emphasis on every raspy syllable, despite the clear pain it brings him.* Thank you. This is the guard who performed my branding. I spoke out when Pázmán punished another brandless slave for bringing his food late, and cold. 

Under Lovashi law, it is a guard's right to discipline a slave, but those same laws state the brandless must be protected from the worst punishments. Pázmán had his victim hung up with chains and whipped until he could no longer protest. The brandless lost much of the use of both his arms, and could no longer work in the kitchens. 

When I explained what had happened to his Overseer, Pázmán was placed on a slave's rations for a month. A paltry punishment if you ask me. Soon after it was handed down, Pázmán had another guard fabricate a claim that I had attempted to escape. Pázmán gained approval to have me branded, and performed the act himself. His branding left me maimed, so that for the rest of my days whenever I speak, I will remember the consequences for speaking out.

Please. Do what is right. Kill this man. Allow me the peace of mind that his death will bring. Remind me that there are some out there who respect justice.

changeCamTarget({pazmanIndex})

These are all lies! Ervin gained his brand not because I lied about an escape attempt, but because he was trying to poison the Director. Ervin was a cook in the Manse's kitchens before his branding. I found him slipping a strange liquid into the Director's meals, and when it was determined to be poison, I had him branded for it.

I never punished anyone to such a degree as he has said, and I didn't perform his branding. I don't know who did, but clearly they had no idea what they were doing because I've never seen a brand so poorly applied.

changeCamTarget({ervinCloserIndex})

Liar! He is a liar!

->2b

=== 2b ===
    
    +Ervin, bring forward the slave who Pázmán hurt. He can corroborate some of your story.
        ->2caa
    +Pázmán, you worked the bottom of the mine. Why were you in the Manse's kitchens while Ervin was preparing meals to witness him adding poison to them?
        ->2cab
    +Pázmán, do you have any witnesses to your side of events?
        ->2cac
    +Speaking earnestly, I don't care if Ervin <i>was</i> poisoning the Director's food. That would have made it easier for us to escape.
        ->2e
    +This behavior may seem startling to a no-brand, but it happened commonly to the branded. I am finished discussing it.
        ->2ea
    +We have heard enough. I have come to a decision.
        changeCamTarget({pazmanIndex})
        ->2e

=== 2caa ===

changeCamTarget({ervinCloserIndex})

Certainly. his name is Reed. Reed, step forward please.

changeCamTarget({crowdIndex})

\*The heads of the crowd swivle about, but no one by that name steps forward.*

changeCamTarget({ervinCloserIndex})

I see... he must have died in the fighting. 

    +Do you have any other witnesses who could speak on your behalf?
        No. I expect the guards who were present for any of it are all dead, and I know of no other slaves who witnessed anything.
        ->2b

=== 2cab ===

changeCamTarget({pazmanIndex})

I was in the kitchens because the miners had finished their work for the day and I was speaking to Kende about a special item I had asked him to smuggle into camp. 
    
    +What was this special item?
        It can get quite cold in the mines and I had asked him to get me wool padding I could put under my armor to keep in the heat.
        ->2b

=== 2cac ===

changeCamTarget({pazmanIndex})

Not unless Kende is still alive. He and I were the only ones to witness Ervin place the poison in the food, and we kept the entire thing hushed up to protect Kende's side business. Kende gave permission for Ervin to be branded and moved into the miners under my care so no one would cause a fuss.

{
-wisdom >= 3:

    +If you and Kende were the only ones to know about the transfer, then you must know who performed his branding. It must be either you or Kende, correct? <Wis {wisdom}/3>
        ->2d
    +That makes sense to me.
        ->2b
-else:
    ->2b
}

=== 2d ===

That's correct.

    +But you said earlier you didn't know who branded him. If it had to be either you or Kende, and you didn't do it, then you know Kende did it.
        ->2da

=== 2da ===

That makes sense to me. It must have been Kende then.

    +Then why did you say you didn't know who branded him?
        \*Pázmán shrugs.* I must have forgotten.
        ->2b

=== 2ea ===

changeCamTarget({ervinCloserIndex})

Has violence become so mundane to you that you will not punish it?

+I merely meant that I needed no convincing. *Address the crowd.* Pázmán shall be punished for what he did to Ervin, and to us!
    ->3a
+The world is a violent place. It is not worth our time to sift through who did what when we can simply move on from it. *Remove Pázmán's bonds.*
    ->2f

=== 2e ===

changeCamTarget({ervinCloserIndex})

Then what is to be your decision?
    
    +I gave Pázmán a promise of safe conduct when I accepted his surrender. He is free to go.
        ->2f
    +\*Address the crowd.* Pázmán's wicked deeds demand an answer.  He shall be punished!
        ->3a

=== 2f ===

changeCamTarget({pazmanIndex})

Thank you. You do your people credit, branded. I will not forget your conduct as long as I live.

changeCamTarget({ervinCloserIndex})

\*Ervin says not a word, but the menace on his face informs you that neither shall he.*

setToTrue(didNotExecutePazman)

activateQuestStep(Deal With the Prisoners, 15)

changeCamTarget({crowdIndex})

\*The crowd begins to boo and jeer as Pázmán is led away.*

fadeToBlack()

deactivate({ervinIndex})
deactivate({ervinCloserIndex})
deactivate({pazmanIndex})

fadeBackIn(60)

->Close

=== 3a ===

changeCamTarget({pazmanIndex})

No! Branded, I beg of you, please let me leave this place. I don't want to die here. I don't- *Pázmán's voice breaks, and he begins to sob.* I don't want to die! 

changeCamTarget({crowdIndex})

\*The Crowd begins to shout and hurl insults at Pázmán.*

changeCamTarget({pazmanIndex})

I have family, in the Confederation. I want to see them again one day. I have a father... a man to whom I owe so much, and have disappointed by coming here. I have a mother too! A mother who I miss more dearly than all of the gold I have ever earned! *Pázmán continues to sob, and he prostrates himself before you, blubbering loudly and unashamedly.*

Don't you have a mother branded? Would she not weep for you as I do now for mine? Please, I will say anything, do anything, to leave this place and find my way home again. Ask it, and I will do it!

->3b

=== 3b ===

{
-allowedErvinToSpeakAtPazmansTrial:
+\*Look to Ervin.* The man weeps for his mother before his enemies. Are you satisfied with this as punishment?
    changeCamTarget({ervinCloserIndex})
    I wept too, after what he did. As my neck will never heal, nor shall he ever receive forgiveness from me. I don't seek his torment, just his death.
    ->3b
+\*Kick Pázmán in the head.* Quiet! You cannot trade your dignity for your life! Your punishment awaits, be silent so I can conduct it.
    changeCamTarget({pazmanIndex})
    \*The blow finds Pázmán's temple, and he flops into the dirt. He does not rise, but still cries loudly and wetly from his prone position.*
    ->3b
+Ervin, Pázmán's fate is yours to decide. Announce your verdict and I shall see it administered.
    changeCamTarget({ervinCloserIndex})

    \*Ervin's voice finally begins to give out on him, but he croaks out a final sentence.* Death by beheading: let it be carried out immediately. 
    ->3d
}

+I have kept Pázmán from his victims long enough. Let the crowd decide his fate!
    ->3c
+Let's get this over with. Nándor, execute him.
    ->3d
+I won't kill a man who begs so pathetically. Fifty lashes, and then set him free.
    ->3e
+\*Sigh, and knead your eyebrows.* I don't wish to admit it, but my heart has heard his sobbing. Let him go.
    ->2f

=== 3c === //gave Pazman to crowd

{
-allowedErvinToSpeakAtPazmansTrial:
changeCamTarget({ervinCloserIndex})

\*Ervin looks at you with slight disdain, but says nothing.*
}

changeCamTarget({pazmanIndex})

No... please! Don't do this!

setToTrue(gavePazmanToTheCrowd)

activateQuestStep(Deal With the Prisoners, 18)

changeCamTarget({crowdIndex})

\*The crowd surges forward and pulls Pázmán within it like a wave submerging a raft. His shrill cries for his mother continue for only moment, and then cease.*

fadeToBlack()

deactivate({ervinIndex})
deactivate({ervinCloserIndex})
kill({pazmanIndex})

fadeBackIn(60)

->3e

=== 3d === //executed Pazman

changeCamTarget({pazmanIndex})

No... please! Don't do this!

setToTrue(executedPazman)

activateQuestStep(Deal With the Prisoners, 17)

changeCamTarget({crowdIndex})

\*The crowd cheers as the executioner's sword rises and falls.*

fadeToBlack()

deactivate({ervinIndex})
deactivate({ervinCloserIndex})
kill({pazmanIndex})

fadeBackIn(60)

->Close

=== 3e ===

changeCamTarget({pazmanIndex})

\*Pázmán gets to his knees and wraps his hands around your shins, kissing the tops of your boots.* Thank you, branded! Thank you! I won't forget this kindness, thank you!

+\*Shake Pázmán off of you.* Nevermind, kill him.
    ->3d
+See that you don't. Now, get off of me.
    ->3ea
+\*Allow Pázmán's display to continue before motioning for some of the branded standing guard to take Pázmán away.*
    ->3ea

=== 3ea ===

setToTrue(gavePazmanFiftyLashes)

activateQuestStep(Deal With the Prisoners, 16)

changeCamTarget({crowdIndex})

\*The crowd cheers as Pázmán is taken away by some of the other branded.*

{
-allowedErvinToSpeakAtPazmansTrial:

changeCamTarget({ervinCloserIndex})

\*Ervin looks at you with contempt before following Pázmán's escort to the flogging post.*

}

fadeToBlack()

deactivate({ervinIndex})
deactivate({ervinCloserIndex})
deactivate({pazmanIndex})

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE