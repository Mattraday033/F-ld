VAR spokeToErvin = false
VAR askedErvinAboutBrand = false
VAR gotThePlanFromKastor = false
VAR gavePasswordToErvin = false
VAR givenTaskByErvin = false
VAR convincedImre = false
VAR terrifiedImre = false
VAR deathFlagImre = false
VAR imreWontSpeakToPlayer = false
VAR finishedErvinsTask = false
VAR askedQuestion1 = 0
VAR askedQuestion2 = 0
VAR askedQuestion3 = 0


VAR playerName = ""

//changeCamTarget(int targetIndex)
//keepDialogue()
//setToTrue(string flagName)
//setToFalse(string flagName)

{
-givenTaskByErvin and not finishedErvinsTask:
->3a
-finishedErvinsTask:
->4a
-spokeToErvin:
keepDialogue()
\*Ervin waits expectantly*
->1a
-else:
setToTrue(spokeToErvin)
->1a
}

=== 1a ===

\*The first thing you notice about this man is his brand. Unlike others you have seen, his is further down his neck, and his scars are thicker and more pronounced. This clearly causes him incredible discomfort. His voice is pained and raspy.* What you need?
    
{
-gotThePlanFromKastor && askedErvinAboutBrand:
    +I won't bother you.
        ->Close
    +Your brand. What happened?
        ->1ba
    +What did you do before your brand?        
        ->1c
    +Which way is the wind blowing?
        ->2a
-gotThePlanFromKastor:
    +I won't bother you.
        ->Close
    +Your brand. What happened?
        ->1ba
    +Which way is the wind blowing?
        ->2a
-askedErvinAboutBrand:
    +I won't bother you.
        ->Close
    +Your brand. What happened?
        ->1ba
    +What did you do before your brand?
        ->1c
-else:
    +I won't bother you.
        ->Close
    +Your brand. What happened?
        ->1ba
}

=== 1ba ===
~askedErvinAboutBrand = true
setToTrue(askedErvinAboutBrand)
    ->1b

=== 1b ===

Before brand, I was slave. Guards didn't have tools to brand. Did it themselves. Sloppy.
    
    +What did you do to receive your branding?
        keepDialogue()
        Told wrong guard of another's abuses. He made up story about me trying to escape. Gave me bad branding to keep me quiet. Stop me talking. Now talking hurts. That why I skip words, save me pain.
        ->1b
//    +So how come your brand is so much worse than mine?
//        keepDialogue()
//        Brand done with special tool. They don't have here. Had to improvise.
//        ->1b
    +That's horrible. I'm so sorry.
        keepDialogue()
        Keep your pity
            {finishedErvinsTask:->4a|{givenTaskByErvin:->3a|->1a}}
    +But you don't die easy.
        keepDialogue()
        \*Ervin smirks slightly and nods*
            {finishedErvinsTask:->4a|{givenTaskByErvin:->3a|->1a}}
    +They'll all pay soon. Just wait a little.
        keepDialogue()
        When they do, come get me. I want to be there.
            {finishedErvinsTask:->4a|{givenTaskByErvin:->3a|->1a}}

=== 1c ===

I was cook. Even before slave. Now I pick up rocks in the dark.

    +Did you ever cook anything interesting?
        ->1d
    +Lets talk about something else.
        keepDialogue()
        \*Ervin waits expectantly.*
        {finishedErvinsTask:->4a|{givenTaskByErvin:->3a|->1a}}
=== 1d ===

Please, go away. Talking hurts.
    +Oh, I'm sorry. I'll go.
        ->Close
    +Fine. Nevermind then.
        ->Close

=== 2a === 

~gavePasswordToErvin = true
setToTrue(gavePasswordToErvin)

\*Ervin smiles.* East.
    
    +What do you require?
        ->2b

=== 2b ===

Kastor didn't say?

    +I'm caught up, I just need specifics.
        ->2d
    +It's been a little bit since we spoke. Can you remind me?
        ->2c

=== 2c ===

combineDialogue()

I know Manse slaves. Was one. We need get them to help when fighting starts. 

->2d

=== 2d ===

No-brands take breaks in alley on west side of barracks, in front of the Manse. You talk there.

->2e

=== 2e ===

    +Where is the Manse?
        The Manse is big house near bottom of cliff. Follow path west from gate in center of camp.
            ->2e
    +How should I approach this?
        Up to you. If they think plan succeed they'll go with it; they hate guards like us. Don't know what else Kastor wants, but if he gave other tasks you should do them first. If you tell brandless plan already done deal, they trust it more.
        Chat with them. Butter them up. They need think we take care of them and not get them killed by guards.
            ->2e
    +Can they be trusted?
        Liars don't make good Manse slaves. They will tell you straight yes or no.
            ->2e
    +Are you still on good terms with them? Your opinion might help sway them.
        They don't like me. They think I'm trouble. I remind of consequences.
            ->2e
    +Got it. I'll return when I've spoken to them.
        ->2f

=== 2f ===

setToTrue(givenTaskByErvin)

activateQuestStep(Aiding Ervin, 1)

Wait. There's chance you can't convince them. That happen, you be ready to kill. A missing slave gives time before they are noticed. A slave who knows our plans threatens now.

    +I'm not going to kill a fellow slave.
        Unwise. If you're caught, help us by dying before you can betray us.
            ->Close
    +Understood.
        ->Close
    +They'll know betrayal means death.
        \*Ervin nods somberly.*
            ->Close

=== 2h ===

->Close

=== 2i ===

->Close

=== 2j ===

->Close

=== 2k ===

->Close

=== 2l ===

->Close

=== 2m ===

->Close

=== 2n ===

->Close

=== 3a ===

\*Ervin waits expectantly.*

{
-askedErvinAboutBrand:
    +I won't bother you.
        ->Close
    +Your brand. What happened?
        ->1ba
    +What did you do before your brand?        
        ->1c
    +We need to talk about what you asked of me.
        ->3b

-else:
    +I won't bother you.
        ->Close
    +Your brand. What happened?
        ->1ba
    +We need to talk about what you asked of me.
        ->3b
}

=== 3b ===

What do you need?

{
-deathFlagImre:

    +Nevermind.
        ->3a
    +Imre wouldn't help us. I had to kill him.
        ->3e

-convincedImre:
    
    +Nevermind.
        ->3a
    +I convinced Imre to help us.
        ->3c

-imreWontSpeakToPlayer:

    +Nevermind.
        ->3a
    +Imre won't help us. But I didn't kill him.
        ->3d

-terrifiedImre:

    +Nevermind.
        ->3a
    +I convinced Imre to help us.
        ->3c
-else:

    +Nevermind.
        ->3a

}

=== 3c ===

That is good. Imre has a good head. You did well.

    +He didn't seem like a bad guy.
        ->3f
    +I wasn't impressed with him. I'm glad you have faith in him at least.
        ->3f

=== 3d ===

You are a fool. I hope Imre knows to keep his mouth shut.

    ->3f

=== 3e ===

That is sad, but you had to do it. Do not weep for what you did. Hate the masters that made it happen.

    ->3f

=== 3f ===

activateQuestStep(Aiding Ervin, 6)

setToTrue(finishedErvinsTask)

Return to Kastor. Tell him what happened. 

    +I will. Take care.
        ->Close

=== 4a ===

\*Ervin waits expectantly.*

{
-askedErvinAboutBrand:
    +I won't bother you.
        ->Close
    +Your brand. What happened?
        ->1ba
    +What did you do before your brand?        
        ->1c

-else:
    +I won't bother you.
        ->Close
    +Your brand. What happened?
        ->1ba
}


=== Close ===

close()

->DONE