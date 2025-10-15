VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR spokeWithRekaAtPunishment = false

VAR gaveAGuardToTheCrowd = false
VAR executedAnyGuard = false
VAR didNotExecuteReka = false

VAR rekaIndex = 1
VAR crowdIndex = 2

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


changeCamTarget({rekaIndex})

{
-spokeWithRekaAtPunishment:

You have returned. Is it time?

    ->1c
}

setToTrue(spokeWithRekaAtPunishment)

{
-gaveAGuardToTheCrowd:
->1ab
-executedAnyGuard:
->1aa
-else:
->1a
}

=== 1a ===

\*Réka looks down the line at the others, then back up at you.* What is to become of us? We surrendered, didn't we? That means you are honor-bound to keep us from harm.

    ->1b

=== 1aa ===

\*Réka stares at the spot where her former comrade was executed.* You bastards, we surrendered, didn't we? You disgrace your honor by harming us.

    ->1b

=== 1ab ===

\*Réka's eyes are wide and trembling.* We surrendered, and you ripped him to pieces! You lot are monsters!

    ->1b

=== 1b ===

    +Deals struck with slavers need not be kept. Be quite.

        {
        -gaveAGuardToTheCrowd:
            \*Réka leans as far away from you as her bonds will allow.* The others were right to die fighting. What a fool I was to trust you.
        -else:
            \*Réka grits her teeth defiantly and says nothing.*
        }

        ->1c
    +The deal between us still stands. You and I have matters to discuss.

        {
        -gaveAGuardToTheCrowd or executedAnyGuard:
            Your word means nothing now. Ask what you wish, but keep your distance.
        -else:
            Say what you wish, but keep your distance.
        }
        ->1c

=== 1c ===

    +I have some questions I must ask you.
        ->1d
    +When we offered you peace, you accepted it. For your actions, you may leave here with your life. *Undo Réka's bonds.*
        ->2a
    +A moment of cooperation does not wipe away a career of oppression. Your punishment is due.
        ->3a
    +I will be back.
        ->Close

=== 1d ===

Ask them then.

    +What are your crimes as you see them?
        ->1e
    +Why did you accept Overseer Gáspár's order to abandon the branded?
        ->1f
    +How did you come to be a guard at this camp?
        ->1g
    +If granted your freedom, what will you do with it?
        ->1h
    +Do you have any regrets?
        ->1i
    +Will any of the freed vouch for your life?
        ->1j
    +I am finished with my questions.
        \*Réka waits for you to continue.*
        ->1c


=== 1e ===

keepDialogue()
I oversaw the branded as they worked in the mine. I kept them to the pace the Overseer set for our work, and hurt them when they slowed. And I did nothing when Gáspár told me to leave them to the worms.

    ->1d

=== 1f ===

As a guard, I was trained to follow my superior's orders, no matter the circumstances. If I had disobeyed Gáspár, I would have either been executed or left to the worms like the branded. And... I was scared.

    +But Márcos disobeyed those same orders. He lead some of the other slaves to safety. Why did you not go with him?
        keepDialogue()
        I thought Márcos had gone mad when he stood up to the Overseer. I'm not sure why Gáspár didn't have him killed. And I was even more surprised to find Márcos and the branded had survived. Frankly, it was a miracle.
        ->1d
    +I have more questions.
        ->1d

=== 1g ===

I am the daughter of a cobbler from Pharos. My family had too many mouths to feed, and there was too little work to go around. When I was old enough, I signed up with the city guard and would send what extra coin I earned back to my family.

Pharos is a city constantly at war with itself. Life in the city guard can be short, but this also means there are many avenues of advancement. I became a sergeant in my fourth year, and it was not uncommon to see others rise to that rank even earlier.

My dedication to Pharos and County Kálnoky caught the eye of one of the Director's honorguard. When the Director set out for this camp, I came with him. That was four months ago now... but it feels like much longer.

    +You are known for your dedication to your superiors, and yet you surrendered to us. How would the Director describe you now?
        keepDialogue()
        I doubt he would use as kind a word as that. But should I live through today, I will not regret my decision. You were not trapped as we were. You do not know the fear that had soaked through me. Do not judge the dedication of a soldier discarded in the dirt like a tool.
            ->1d
    +I have more questions.
        ->1d

=== 1h ===

keepDialogue()
I would like to return to Pharos, some day. My family is there. My duty is to protect that city. I never should have left it.
    ->1d

=== 1i ===

keepDialogue()
I regret my conduct towards the branded. I regret that I couldn't save Virág and Gáspár from their fates. And I regret ever leaving Pharos. I had a surety of purpose serving the people of that city that I could never find serving the Director.
    ->1d

=== 1j ===

keepDialogue()
\*Réka thinks for a long time in silence.* No. I don't think I can think of a single one.
    ->1d

=== 2a === 

    {
    -gaveAGuardToTheCrowd or executedAnyGuard:
        You're letting me go? But- \*Réka abruptly reconsiders whatever she was about to say, and simply nods instead.*
    -else:
        \*Réka gets to her feet.* Forgive my lack of faith. Your actions have been most gracious.
    }

    +\*Address the crowd.* This is Guard Réka. She assisted in ending the worm threat to the camp, and surrendered rather than fight our emancipation. Her words helped convince another to do the same, and she stood aside rather than help her comrades return us to our toils. For these reasons, I set her free.
        ->2b

=== 2b ===

setToTrue(didNotExecuteReka)
changeCamTarget({crowdIndex})

\*Your words are met with encouragement from a portion of the crowd. Others look about in mild confusion, or shake their heads.*

activateQuestStep(Deal With the Prisoners, 11)
        
    fadeToBlack()

    deactivate({rekaIndex})
    
    fadeBackIn(60)
        
        ->Close

=== 2c ===

->Close

=== 2d ===

->Close

=== 2e ===

->Close

=== 2f ===

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
{
-gaveAGuardToTheCrowd:
\*Réka whispers to herself.* I've been a fool. I've been a fool.
-executedAnyGuard:
\*Réka grimaces as she looks up into your face.* Get it over with then. After all I've been through, I cannot muster fear for the likes of you.
-else:
You bastard. If Gáspár could see me now, I know he would laugh.
}

    +Fifty lashes should do it. If you're still cogent after that, you can have your freedom.
        ->3b
    +The only punishment befitting a slaver is death. At least for you it will be quick.
        ->3c
    +Let the crowd sort you out. If they decide you should live, then so be it.
        ->3d
    +On second thought, I don't really see the point in punishing you.
        
        Don't toy with me! Take this seriously!
    
        ->1b

=== 3b ===

setToTrue(didNotExecuteReka)
setToTrue(gaveRekaFiftyLashes)

activateQuestStep(Deal With the Prisoners, 12)

{
-gaveAGuardToTheCrowd:
That is... that is more than fair. Just keep that crowd of animals away from me.
-else:
I am forced to accept that. Let us finish it quickly then.
}

changeCamTarget({crowdIndex})

\*The crowd raises a cheer as Réka is led to the flogging post.*

fadeToBlack()

deactivate({rekaIndex})

fadeBackIn(60)

->Close

=== 3c ===

setToTrue(executedReka)

activateQuestStep(Deal With the Prisoners, 13)

{
-gaveAGuardToTheCrowd:
That it could be worse is no comfort. I have no doubt Gáspár will be laughing when I see him soon.
-else:
\*Réka hangs her head in defeat.* If I still had my weapon, I would bury it in your lying face. That I discarded it willingly just adds to my folly.
}

fadeToBlack()

deactivate({rekaIndex})

fadeBackIn(60)

->Close

=== 3d ===

setToTrue(gaveRekaToTheCrowd)

activateQuestStep(Deal With the Prisoners, 14)

Stay back you scum, stay back! The first to approach I will bleed with my teeth!

fadeToBlack()

kill({rekaIndex})

fadeBackIn(60)

->Close

=== 3e ===

->Close

=== 3f ===

->Close

=== 3g ===

->Close

=== 3h ===

->Close

=== 3i ===

->Close

=== 3j ===

->Close

=== 3k ===

->Close

=== 3l ===

->Close

=== Close ===

close()

->DONE
