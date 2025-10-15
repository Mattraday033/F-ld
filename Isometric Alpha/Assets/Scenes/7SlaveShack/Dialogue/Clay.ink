VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR metClay = false
VAR spokeToSeb = false
VAR clayExplainedCrime = false
VAR clayExplainedReward = false
VAR clayExplainedJob = false
VAR acceptedClaysFirstJob = false
VAR acceptedClaysSecondJob = false
VAR hasThatchsNecklace = false
VAR threatenedThatch = false
VAR knowsAboutKendesShop = false
VAR gotKnifeFromClay = false
VAR toldClaySpokeToSeb = false
VAR gaveNoteToSeb = false

VAR deathFlagThatch = false

VAR clayRemorseKey = "A Weary Heart"
VAR clayFrontalAssaultKey = "The Frontal Assault"
VAR clayStealthKey = "Stay Unseen"
VAR clayKeptNecklaceKey = "Keepsake Kept"
VAR clayPacifistKey = "Slipping Upward"
VAR clayHeroKey = "A Hero, Actually"


VAR weaponListIndex = 1
VAR bronzeDirkIndex = 8
VAR questItemListIndex = 3
VAR claysNoteIndex = 6
VAR thatchsNecklaceKey = "Thatch's Silver Necklace"

VAR playerName = ""

//changeCamTarget(int targetIndex)
//keepDialogue()
//setToTrue(string flagName)
//setToFalse(string flagName)
//activate(int index of gameobject you're activating)
//deactivate(int index of gameobject you're deactivating)
//activateQuestStep(string questTitle, int questStepIndex)
//prepForItem() //used before giveItem/giveItems/takeAllOfItem to add obtained/removed text after next line
//giveItem(int listIndex, int itemIndex, int quantity)
//giveItems(int listIndex1, int itemIndex1, int quantity1 |
//          int listIndex2, int itemIndex2, int quantity2 |
//          ... etc)
//takeAllOfItem(string itemName)
//activateQuestStep(string fullTitleOfQuestFoundInQuestJsonFile,int questStepIndex)
//searchInventoryFor(string nameOfVarSetToTrueInsideInkFile,string itemNameToSearchFor)
//fadeToBlack()
//fadeBackIn(int numberOfFramesToWaitBeforeFadingBackIn)
//moveToLocalPos(float xCoord,float yCoord)

{
-acceptedClaysSecondJob:
->5a
-gotKnifeFromClay:
->3a
-acceptedClaysFirstJob:
->2a
-metClay and not acceptedClaysFirstJob:
->1i
-else:
->1a
}


=== 1a ===

~metClay = true
setToTrue(metClay)

New blood, eh? You come to tell me the lockdown's over?

    +I've been let out of my hut to run a job for someone. I'm just getting my bearings.
        ->1b

=== 1b ===

If you're running errands for the guards, then perhaps you could run one for me. I could use someone who doesn't let their scruples get in the way of earning a fat reward.
    
    +Give me the details.
        ->1d
    +Depends on how fat the reward is, I suppose.
        ->1da
    +You'll find me more scrupulous than scruple-less. Don't get me involved in whatever you have planned.
        \*Clay chuckles to himself as you turn to leave.* So you say, but you're nothing but the guards' pet if they let you run around like that.
        ->Close
        
=== 1c ===


    {
    -not clayExplainedJob:
        +Give me the details.
        ->1d
    }
    {
    -not clayExplainedReward:
        +What are you offering for the job?
        ->1da
    }
    {
    -clayExplainedJob:
    +I'll remove this gossip for you. Who's the target?
        ->1g
    +What kind of rumors? You've piqued my interest now.
        ->1e
    }
    {
    -clayExplainedJob or clayExplainedCrime:
    +I'm not liking the sound of this. I'm out.
        ->1f
    -else:
    +This isn't worth the risks. Keep your knife, I want no part of this.
            ->Close
    }
=== 1d ===
    ~clayExplainedJob = true
    setToTrue(clayExplainedJob)
        
        It goes like this: a certain individual may or may not have seen me commit an act that the guards would very much like to know about. And the individual in question just so happens to be the 'to honest for his own good' type. So I need you to remove this guy before he has a lapse in judgement and starts to spread rumors about me to the guards.
    
    ->1c

=== 1da ===

    ~clayExplainedReward = true
    setToTrue(clayExplainedReward)

    I was gonna do the job myself before the lockdown, so I lifted this sticker here off of one of the guards. It's sharp, finely made; would fetch a tidy sum if you didn't have a use for it. It's yours after the jobs done, as I won't be needing it.
        ->1c

=== 1e ===

I have to keep the amount of people who know about this to a minimum. However, if you do the job and get back without anyone noticing, I'll know you're the discrete type and I will fill you in then.
    
{
-charisma >= 2:
    +You can't expect me to do this job without any context, you're asking me to take a life. <Cha {charisma}/2>
        ->1h
}
    +Whatever, it's none of my business anyways.
        That's right, it's not.
        ->1c

=== 1f ===

Can't have you telling some guard about this conversation. Should have taken the job, friend.

    +\*Defend yourself.* <Combat>
        ->Close

=== 1g ===

~acceptedClaysFirstJob = true
setToTrue(acceptedClaysFirstJob)
{
-clayExplainedCrime:
activateQuestStep(Shut Up The Gossip,1)
-else:
activateQuestStep(Shut Up The Gossip,0)
}

The unlucky sap is named Thatch. He lives down in the southern part of the camp, in the hut directly across from the Mess Hall. He's an alright sort, but I've worked with him long enough to know that big head of his has no room for a secret of any kind. So the way I see it, you need to take him out before he does something stupid like letting slip to the guards.

He's big, but he ain't the kind of guy to get into fights. If you can come at him from the sides or get around his back, you might be able to surprise him and lay him flat before he can hit you with those big mitts of his.

He has a necklace he keeps hidden from the guards. I've seen him thumbing it when he thinks no one's looking. Bring it back and I'll trade it for the knife.

->Close

=== 1h ===

~clayExplainedCrime = true
setToTrue(clayExplainedCrime)

Fine then. If it matters so much to you, then I will explain.

    ->4a

=== 1i ===

You're back. Shouldn't you be off licking the guards' boots?

    +I've reconsidered and I want to hear what that job was.
        keepDialogue()
        Couldn't leave the promise of a nice payout behind, could ya?
        ->1b
    +Whatever.
        ->Close

=== 1j ===

->Close

=== 1k ===

->Close

=== 1l ===

->Close

=== 1m ===

->Close

=== 1n ===

->Close

=== 2a === 

searchInventoryFor(hasThatchsNecklace,{thatchsNecklaceKey})

You're back. Any luck with Thatch?

{
-acceptedClaysFirstJob and hasThatchsNecklace:
    +The deed is done. You mentioned a reward?
        ->2b
}
{
-acceptedClaysFirstJob:
    +I've decided that I'm not going to kill Thatch. You can find someone else to do your dirty work.
        keepDialogue()
        Unwise. \*Clay draws a bronze knife and begins to attack.*
        ->1f
}
    +I haven't been to see him yet. I'll return when I have.
        Very good.
        ->Close

=== 2aa ===

~clayExplainedCrime = true
setToTrue(clayExplainedCrime)

Some guard was gonna lay into a buddy of mine, so I removed him. Got him good in the neck and stuffed his body before anyone was the wiser. Or that was the plan, but I was sloppy and someone saw me moving the body. And I didn't even stop the guards from punishing my friend.

    
{
-acceptedClaysFirstJob and hasThatchsNecklace:
    +The deed is done. You mentioned a reward?
        ->2b
}
    +I haven't been to see him yet. I'll return when I have.
        Very good.
        ->Close

=== 2b ===

{
-deathFlagThatch:
    finishQuest(Shut Up The Gossip, true, 4)
    
    {
    -threatenedThatch:
        learnLesson({clayRemorseKey})
    }
    
    learnLesson({clayFrontalAssaultKey})
    learnLesson({clayStealthKey})

-else:
    finishQuest(Shut Up The Gossip, true, 5)

    learnLesson({clayPacifistKey})
    learnLesson({clayHeroKey})
}

prepForItem()

Not so fast, you'll need to hand over that necklace of his first. I've been around long enough to know to ask for proof a job is done before paying for it.

addXP(100)

{
-charisma >= 2:
    +\*Show the necklace.* It's right here, but what use is it to you if you're stuck in here? <Cha {charisma}/2>
        learnLesson({clayKeptNecklaceKey})
        ->2c
}

{
-wisdom >= 2:
    +\*Show the necklace.* It's right here, but what use is it to you if you're stuck in here? <Wis {wisdom}/2>
        ->2c
}

    +It's right here. *Give Clay the necklace.*
        prepForItem()
        
        I'll take that. I didn't know Thatch very well, but it was clear this meant something to him. Shame he had to go, but when it comes down to you or the other guy, it's always gotta be the other guy. Remember that while you're in here. 
        
        takeAllOfItem({thatchsNecklaceKey})
        ->2d

=== 2c ===

~knowsAboutKendesShop = true
setToTrue(knowsAboutKendesShop)

\*Clay squints at you wryly.* You make a perceptive point. Fine, keep the damned thing. I was thinking I could pawn it off to that cook in the Mess Hall after the lockdown ended, but he never seemed to like me anyways. Maybe you'll have better luck getting a fair price from him.

->2d

=== 2d ===

~gotKnifeFromClay = true
setToTrue(gotKnifeFromClay)

prepForItem()

And now the knife, as promised. May you burrow it in more of the guards than I was able to.

giveItem({weaponListIndex},{bronzeDirkIndex},1)

You do good work, and I could use some more of your help. But if you gotta get back to your other duties I understand.

->2da

=== 2da ===

    +So what did Thatch catch you doing anyways?
        ->4a
    +What's the job?
        ->2e
    +Another time, perhaps.
        ->Close

=== 2e ===

This one's more of a personal matter. A friend of mine, Seb, recently was given the mother of all floggings by that shitheel Tabor. I haven't seen him in a while; I'm guessing they beat him so bad they moved him to the hut they keep the invalids in. That is, unless he's dead, or in the Pit. Can you go check in on him for me and give him this note? Or otherwise find out what happened to him?


{
-spokeToSeb:
    +I've actually seen Seb. He was beaten severely and is alive but unresponsive.
        ~toldClaySpokeToSeb = true
        setToTrue(toldClaySpokeToSeb)
        keepDialogue()
        So it's like that, is it? May Sun scorch all of the guards eyes from their sockets. Every last one of them deserves no less. But they'll get theirs in time. In the meanwhile, I'd like you to go back to Seb and read him the contents of the note. I don't know if there's enough left of him to hear it, but it'd at least cheer me up some to know he might have.
        ->2e
}

    +Which hut would he be in?
        keepDialogue()
        We used to share this hut, but seeing as he ain't here, the guards probably moved him to the hut where they keep the slaves that can't work anymore. It's the one that's just left of the gate when you're facing east, in the center of camp.
        ->2e
    +What's the pay?
        keepDialogue()
        Not much, just the few coins I scrounged up. But it should only take a moment of your time, so the way I see it it's fair compensation. Just don't think about what I had to do to keep the guards from finding them.
        ->2e
    +What's on the note?
        ->2f
    +I'm game.
        ->2h
    +I'm not interested.
        Come back if you change your mind.
        ->Close
=== 2f ===

    Nothing too important, I don't care if you read it. It's just a short poem I wrote; we used to spend our time coming up with them in the few hours of rest we got before the lockdown.

        +Poetry? You didn't strike me as the type.
            keepDialogue()
            What can I say, I'm a man of many talents. When the two things you do all day are swing a pick or stare at a wall, you got a lot of time to think. Seb liked to spend that time coming up with little rhymes, songs, riddles, jokes, you name it. It caught on and now I do it some too.
            ->2e

=== 2h ===

~acceptedClaysSecondJob = true
setToTrue(acceptedClaysSecondJob)

{
-toldClaySpokeToSeb:
activateQuestStep(Note Worthy,1)
-else:
activateQuestStep(Note Worthy,0)
}

prepForItem()

Good, here's the note. Off with you now.

giveItem({questItemListIndex},{claysNoteIndex},1)

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

Are you looking to take me up on that job now?

    +What job was that again?
        ->2e
    +So what did Thatch catch you doing anyways?
        ->4a
    +Not right now.
        ->Close

=== 3b ===

->Close


=== 4a ===

A guard was giving a buddy of mine some trouble, so I removed him. Got him good in the neck and stuffed his body before anyone was the wiser. Or that was the plan, but I was sloppy and someone saw me moving the body. And the guards ended up punishing my friend over the matter instead of me.
    
    +You talk about this all very casually.
        keepDialogue()
        You're new here, so I'll spell it out for you. The branded ain't all out maneuvered bureaucrats or farmers who pissed off their tax collector. Some of us got here the way the system expects us to, by doing things out there that were so unsavory that the law says we don't get to go back to.
        ->4a
    +Who is this buddy of yours? You sound very protective of him.
        ->4b
    +Lets talk about something else.
        Sure.
        {
        -gotKnifeFromClay:
            ->2da
        -else:
            ->1c
        }
        
=== 4b ===

He used to share this hut with me, before the guards messed him up good. He's an alright kinda guy, reminded me of some of the guys I used to run with before I got put away in here. And the one rule we had was you look out for the crew.

    +And that includes killing a guard for him?
        keepDialogue()
        You might find this hard to understand, because you ain't been through the mines like we have. You'll quickly find that the guards ain't gonna treat you like people, you lost that luxury when you got the brand. So you gotta stop thinking of them like people too. Would I put down an animal for a friend? Sure as shit I would.
        ->4a
        
=== 5a ===

How'd it go? Were you able to find Seb?

    {
    -gaveNoteToSeb:
    +I got the note to Seb. 
        ->5b
    }
    +Not yet.
        ->Close
        
=== 5b ===

finishQuest(Note Worthy,true,4)

How's he looking?

    +It's not good. The guards flogged him into some kind of shock.
        ->5c
    +They beat him until his mind fled, and then kept going. There isn't much of him left.
        ->5c

=== 5c ===

prepForItem()

Those whoresons. Each of them is all big and mighty when you're unarmed and tied to a post. But they bleed just like the rest of us with a knife sticking out of them. If the chance arrises, show them no mercy. They wouldn't show you any if it was reversed.

giveCoins(50)&
addXP(100)

    ->Close

=== Close ===

close()

->DONE