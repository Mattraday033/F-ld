VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerName = ""

VAR weftIndex = 1
VAR weftHurtIndex = 2

VAR dezsoAndSlavesFightIndex = 0
VAR dezsoOnlyFightIndex = 1

VAR declaredHostagesDead = false
VAR savedHostages = false
VAR hostagesDead = false
VAR foughtDezsoAndLoam = false

VAR toldNotAllowedToLeave = false

VAR concludedHostageNegotiations = false
VAR spokeToTaborAtBeginningOfSituation = false

VAR allowedYourselfToBeTakenHostage = false
VAR mentionedStoneMan = false
VAR failedRushDezso = false

VAR liedToWeftAboutHearingExtortion = false

VAR hostageTakersStandardPunishment = false
VAR hostageTakersNoPunishment = false
VAR hostageTakersLeaderPunished = false
VAR hostageTakersLaborPunishment = false

activateQuestStep(A Situation Brews,Return to Tabor.)

setToTrue(concludedHostageNegotiations)
setToTrue(spokeToWeftAfterHostageSituation)

{
-allowedYourselfToBeTakenHostage:
    ->2a
-savedHostages:
    ->speakingToNormalWeft(->1a)
-declaredHostagesDead:
    ->speakingToNormalWeft(->4a)
-hostagesDead:
    ->speakingToNormalWeft(->3a)
}

->speakingToNormalWeft(->1a)

=== speakingToNormalWeft(->divert) ===

movePlayer(3,5)

changeCamTarget({weftIndex})
activate({weftIndex})
setNPCFacing({weftIndex},SE)
setPlayerFacing(NW)

->divert

=== 1a ===

What relief. I thought they would kill us for certain. Or the Lovashi would when they came charging in.

    +But we managed our task anyways. 
        ->1b
    +That didn't stop you from sucking up to Adéla and Tabor before.
        ->1e

=== 1b ===

You handled that masterfully. Had the hostages died, there'd have been a melee for certain. With us caught in the middle!

    +I noticed you let me do the talking. Why was that?
        I'm not well liked by the branded. I thought that keeping my mouth shut would be the most helpful thing I could do.
        ->1c
    +And you didn't say a word. You were next to useless!
        I did not choose to come on this task with you! Considering the how little I am tolerated by the branded, I thought that keeping my mouth shut would be the most helpful thing I could do.
        ->1c

=== 1c ===

{
-wisdom >= 2:
    +\*Nod sagely.* There is a prudence to knowing when to speak and when to be still. <Wis <{wisdom}/2>
        
        I'm glad you approve. You're quite the learned one. For one of the branded I mean.
        ->1d
}
    +Maybe that was the right call. It worked out for us in the end.
        combineDialogue()
        And I'm ever grateful it did! 
        ->1d
    +A clever excuse to let me attract the branded's ire.
        ->1da

=== 1d ===

Your words saved many lives, ours included. I doubt anyone else could have pulled that off.

    +Sucking up to me now, are you? Save your prostrations for the Lovashi.
        ->1da
    +Your words are kind, but we'd best get back to Chief Tabor before he comes looking for us.
        Quite right. I'll follow you out.
        ->deactivateExtras
    +Don't think surviving this makes us comrades. I detest how you conduct yourself in front of the guards.
        ->1e

=== 1da ===

setToTrue(insultedWeftAfterHostages)
No, I-... damn it. Let's just head outside and get our next task.
->deactivateExtras

=== 1e ===

The Lovashi are fickle, malicious creatures. You will learn as I have that, to keep them from attacking, you must make yourself as an appealing a servant as possible.

    +The same excuse all collaborators give.
        setToTrue(insultedWeftAfterHostages)
        You've probably become used to hearing it because collaborators tend to live longer. 
        ->deactivateExtras
    +That approach will serve you well, until they grow bored of you.
        The same can be said of rebelling. All that is different is how quickly the Lovashi will react.
        ->deactivateExtras


=== 2a ===

changeCamTarget({weftHurtIndex})
movePlayer(4,0)

deactivate({weftIndex})
activate({weftHurtIndex})
setNPCFacing({weftIndex},SW)
setPlayerFacing(NE)
setPartyMemberHealth(Weft,1)

\*Weft gives out a pained moan.*

    +Gods, the hate those branded felt for you was fierce. When the guards assaulted the hut and the hostages were killed, the branded guarding us stabbed you at least a few dozen times. 
        ->2b
    +Let me help you up. We need to get you some medical attention.
        ->deactivateExtras

=== 2b ===

I-I-Is it over? Oh... why am I so cold...

    +He didn't even look at me, he just kept going until the guards pulled him off of you.
        ->2c
    +Let me help you up. We need to get you some medical attention.
        ->deactivateExtras

=== 2c ===

\*Weft doesn't reply, but merely lays there on the ground, slowly breathing.*

    +We'd best get you to the guards. Perhaps they can do something for you.
        ->deactivateExtras


=== 3a ===

We failed... oh by the Mother of All, our punishment for this is going to be horrific beyond compare.

    +What do you mean? Tabor said we would be safe no matter the outcome.
        ->3f
    +Calm yourself, nothing has been decided yet.
        ->3e
    +Well at least I tried something. You didn't say a word!
        ->3b

=== 3b ===

\*Panic colors Weft's voice.* The branded hate me! I thought it was best to keep mute!

    +I'm going to make sure Tabor knows you meant to say nothing.
        setToTrue(insultedWeftAfterHostages)
        ->3c
    +Well, that's probably true. Let's just get back to the Chief before he think's we're trying to hide.
        ->deactivateExtras

=== 3c ===

He's known me a lot longer. He'll take my side over yours!

    +Not if he learns you instigated the mess hall heist.
        ->3d
    +We'll see who he favors after he hears how useless you were.
        ->deactivateExtras

=== 3d ===

{
-liedToWeftAboutHearingExtortion:
setToTrue(weftKnowsYouLiedAboutHearingExtortion)
So you did hear us talking! You lying sack of shit! If you tell that to anyone, the guards I have been useful to will find ways to make you pay.
-else:
I should have known you'd prove a backstabber. If you tell that to anyone, the guards I have been useful to will find ways to make you pay.
}

->deactivateExtras

=== 3e ===

\*Weft looks unsure.* You're right, you're right... but Captain Adéla will surely find a way... oh Gods...

->deactivateExtras

=== 3f ===

Chief Tabor will keep to his word. He may even defend us to the Director. But Captain Adéla will be scheming her revenge on us both. And I'm sure those hostages had friends too.

    +One danger at a time. Let us return to Chief Tabor before he suspects we're trying to hide from him.
        ->3e
    +Well at least I tried something. You didn't say a word!
        ->3b

=== 4a ===

You- \*Weft looks about at all the guards and keeps his voice down.* You called the guards in before the hostages were killed. What were you thinking?

    +The way I see it, more dead guards is a good thing.
        ->4b
    +Be quiet! The only way they'll learn that is if you keep yapping about it.
        ->4ba

=== 4b ===

You're insane. They'll find out about this, and then we'll be punished for sure. Chief Tabor isn't going to protect us once he learns we failed on purpose!

    +They aren't going to find out. Nobody will have noticed when exactly the hostages died in the confusion. We're in the clear.
        ->4ba
    +The guards will only find out about this if you tell them. So just shut up about it and we'll be fine.
        ->4ba

=== 4ba ===
        
keepDialogue()

Maybe you're right, but we still failed... Oh by the Mother of All, our punishment for this is going to be horrific beyond compare.
    ->3a

=== deactivateExtras === 

fadeToBlack()

deactivate({weftIndex})
deactivate({weftHurtIndex})

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE