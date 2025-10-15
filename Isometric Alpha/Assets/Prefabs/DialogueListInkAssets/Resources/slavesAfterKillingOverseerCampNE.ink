VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerName = ""

VAR kastorStartedRevolt = false
VAR agreedToBeLeader = false
VAR andrasAttackedPlayer = false
VAR declaredAndrasMustDie = false
VAR knowsAboutTheMine = false
VAR gaveKastorToolBundle = false
VAR metClay = false
VAR gotKnifeFromClay = false
VAR convincedImre = false
VAR terrifiedImre = false
VAR gotThePlanFromKastor = false
VAR andrasLeftInHut = false
VAR obtainedMineArmoryKey = false
VAR mineLvl3CarterAndNandorInParty = false
VAR failedToConvinceClay = false

VAR partyFlagNándor = false

VAR deathFlagGuardAndrás = false
VAR deathFlagJanos = false
VAR deathFlagClay = false
VAR deathFlagThatch = false
VAR deathFlagGarcha = false

VAR crowdFervor = 0
VAR crowdAppeasementThreshold = 3

VAR gainedFervorFromLockdownExplanation = false
VAR gainedFervorFromToolsMention = false
VAR gainedFervorFromNandorExplanation = false
VAR gainedFervorFromChaOption = false
VAR gainedFervorFromStrOption = false

VAR clayInterjected = false
VAR janosInterjected = false

VAR playerIndex = 0
VAR nandorIndex = 1
VAR carterIndex = 2
VAR garchaIndex = 3
VAR janosIndex  = 4
VAR clayIndex   = 5
VAR slave1Index = 6
VAR slave2Index = 7
VAR slave3Index = 8
VAR slave4Index = 9
VAR theCrowdIndex = 10
VAR afterOverseerParentIndex = 11

//depricated
VAR thatchIndex = 11 


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

->1a

=== 1a ===

moveToLocalPos(2,2.4)

changeCamTarget({nandorIndex})

Hear me! We have slain the overseers in this section, so you do not need to fear the guards during this gathering. But time is short, and we have much to explain if we are to move quickly enough to take advantage of their confusion.

{
-agreedToBeLeader:

You all know me, but you may not know my friend here. They are the one who has secured the equipment you see before you. They are the one who has already slain many guards. And they are the one who personally saved myself and others from being trapped below the earth. Hear them now and know they have more than earned your trust!

    ->1b

-else:

You all have suffered under this lockdown, but you may not know for what reason. Days ago, while digging on the lowest level of the mine, my team and I ran afoul of a cavern beneath the earth filled with packs of deadly monsters. A horde of enormous, ravenous worms flooded the mine, which forced the guards to call for the evacuation you all remember.

During the initial confusion many guards were slain, or trapped as I had been, deep within the mine. *Nándor grabs your wrist and raises it high for all to see.* And had it not been for the efforts of {playerName}, I would still be trapped there. 

Their accomplishments are many: they are the one who personally saved myself and others from being trapped below the earth. They are the one who has secured the equipment you see before you. They have already slain many guards, and will be leading our assault on the Manse.

I would not ask what I ask of you now if I was not sure it would succeed. The guards' strength is depleted, but ours is bolstered! With {playerName}'s might, we can break any barricade the guards put in our path. We can smash our way into the Manse, take retribution on the Director, and eat from their stores as kings! 
    
    ~gainedFervorFromNandorExplanation = true
    ~crowdFervor+=5
    
    ->1b
}

=== 1b ===

    +I am {playerName}. Everything Nándor said is true. We have come to liberate the branded, and to break the guards' hold on the camp.
        
        changeCamTarget({theCrowdIndex})
        
        \*Some of the slaves exchange glances. There is a mood of doubtfulness about them.*
        
         changeCamTarget({slave1Index})
         
        ->1ba
    +We have slain some guards, but there are more. Who here will fight to save themselves?
        
        changeCamTarget({theCrowdIndex})
        
        \*A few halfhearted hands linger in the air, but the rest of the slaves look to you with pitiful expressions.*
        
        ->1ba
        
=== 1ba ===
    //{
    //-crowdFervor >= 1 and not clayInterjected and ((gotKnifeFromClay and not deathFlagThatch) or not deathFlagClay):
    //    ~clayInterjected = true
    //    ->6a
    //-else:
         changeCamTarget({slave1Index})
        ->3a
    //}

=== 2a ===

~janosInterjected = true
changeCamTarget({janosIndex})

Heed not {playerName}'s words! They would use you as fodder to enable their own escape!

+Janos? You oppose the plan you helped with? Why are you doing this?
    You know why, villain.  I do what I do for András.
        ->2b
+\*Say nothing.*
    ->2b

=== 2b ===

I was a part of this plan from the very beginning, when Nándor devised it before the lockdown. But back then we never planned to rely on this newcomer! Listen to me, they have murdered those that would aid our plan in cold blood, and seek only their own liberation! Your interests are not theirs!

    +He only tells half the tale! I killed a guard who I suspected of treachery, one whom Janos was bedding!
        ->2c
=== 2c ===

This is true, but who do you trust more as a judge of character? The man who has dug and bled beside you for the past few months, or this newcomer, who knows not of your toils? 

    +If I were so self-centered, would I have fought through the mine to claim the weapons you see before you?
        changeCamTarget({janosIndex})
        keepDialogue()
         I believe you only did that because you thought you had no choice. It falls to me to reveal you for the uncaring opportunist that you are. You would lead these people to ruin if it meant you could slink away.
            ->2c
   +Are you seriously suggesting that after killing the guards and freeing this portion of the camp, that they should simply go back to their huts?
        changeCamTarget({janosIndex})
        keepDialogue()
        This is a glorious moment, but one perched upon a blade's edge. I believe we must fight, but what we decide here will either result in our freedom or deaths. I will not have everyone gamble their lives away with you in the lead!
            ->2c
    +Proof of my intentions stands before you: I risked my own hide to save the lives of Nándor and Carter.
        ->2ca
    +\*Turn and address the gathered slaves.*
        ->2d


=== 2ca ===

\*Janos looks to Nándor.* Nándor, please. Give context to what you said before. Tell me you are not truly so indebted to this murderer.

changeCamTarget({nandorIndex})

keepDialogue()

It's true Janos. There can be no mistaking it. Myself and the others trapped in the mine owe our freedom to {playerName}. I am sorry for what our friend had to do, but I trust their judgement. Many more guards will die before we are free, and I cannot blame them for what they felt they had to do.

->2c

=== 2d ===

changeCamTarget({theCrowdIndex})

\*The slaves await your words.*

    +I hold no secrets! I did kill Guard András, and I would do so again! The man was a slaver, a torturer and a murderer, and in any civilized land he would have been put to death! Follow me if you wish the same on his compatriots!
        keepDialogue()
        \*The slaves erupt in cheers. Their blood is up, and they bay for more. Having lost the crowd, Janos slinks back out of sight.*
        ->3a
    +Forgive Janos, for the accusations he has laid at my feet are misguided by love. Do not judge him too harshly for clinging to such a precious thing! Mourn it's loss, but swear to never forget it's memory!
        keepDialogue()
        \*The slaves nod their heads solemnly. Some of them look at Janos with pity. Janos looks at you with malicious intent.*
        ->3a
    +There can be no room for doubts now. I call on Janos to renouce their love for Guard András before the gathered people, and swear their loyalty to the cause. Any less would mean they still hold traitorous thoughts in their heart!
        keepDialogue()
        \*The slaves wait with bated breath, looking at Janos in anticipation. Janos looks from them, to you, and then back to the crowd. He looks as if he is about to vomit, but speaks instead.*
        
        I was mistaken before. I was not in my right mind to hold any love for a guard. {playerName} has brought me to my senses, and I am ready to follow them.
        ->3a
    +Janos is a traitor! My only mistake was to leave them alive so that they may continue to thwart the revolution! Let us now rectify that mistake! <Attack Janos>
        kill({janosIndex})
        
        keepDialogue()
        \*Those in the crowd near Janos grab him and begin to beat him. Soon he is lost beneath the sea of bodies, his screams giving way to the sickening rhythm of landed blows.*
        ->3a

=== 2e ===

~janosInterjected = true

{
-not deathFlagClay and not failedToConvinceClay:
changeCamTarget({janosIndex})

I would fight! But I must know: what will become of the guards who surrender to us?

changeCamTarget({clayIndex})

Gods damn it, is this really the time for this? We can decide that when we've won, not just before battle!

changeCamTarget({janosIndex})

By then it will be too late. Some may take justice into their own hands, or refuse their surrender entirely. I would have it said now before all present that we would not descend to their level!

    +Enough of this! I have come to a decision!
        ->2f

-else:

changeCamTarget({janosIndex})

I would fight! But I must know: what will become of the guards who surrender to us? During the chaos of battle some may take justice into their own hands, or refuse their surrender entirely. I wish to have it said now before all present that we would not descend to their level.

    ->2f
}


    
=== 2f ===

changeCamTarget({theCrowdIndex})

\*Everyone waits to hear what you decide.*

    +\*Address the crowd.* Any guards who surrender are under my protection! I agree with Janos: we will not descend to an equivalence with slavers!
    setToTrue(acceptingGuardPrisoners)
    
    keepDialogue()
    
    \*The crowd gives a cheer, but it is yet to be seen if they will hold to your command.*
        ->3a
    +\*Address the crowd.* Any guards who surrender should be protected! If they think they may live by surrendering, we may be able to defeat them without cost!
    setToTrue(acceptingGuardPrisoners)
    
    keepDialogue()
    
    \*The crowd gives a cheer, but it is yet to be seen if they will hold to your command.*
        ->3a
    +\*Address the crowd.* Do not delay your revenge to save another's conscience! Slavers deserve the axe, and not your mercy!
    setToTrue(notAcceptingGuardPrisoners)
    
    keepDialogue()
    
    \*The crowd gives a cheer. It seems like they don't need to be told twice.*
        ->3a

=== 3a ===

    \*An older slave speaks up.* What does it matter if you have slain some guards. There are so many of them, and they are better armed. None of us know how to wield a weapon, let alone fight as a group! We are not soldiers. 

    /*{
    -crowdFervor >= 1 and not clayInterjected and ((gotKnifeFromClay and not deathFlagThatch) or not deathFlagClay):
        ~clayInterjected = true
        ->6a
    }*/

    {
    -not deathFlagJanos and not janosInterjected and crowdFervor >= crowdAppeasementThreshold && deathFlagGuardAndrás && not andrasAttackedPlayer and not declaredAndrasMustDie:
        ->2a 
    -not deathFlagJanos and not janosInterjected and crowdFervor >= crowdAppeasementThreshold:
        ->2e
    }
    
    {
    -crowdFervor >= crowdAppeasementThreshold:
        ->4a
    }

    {
    -(knowsAboutTheMine or mineLvl3CarterAndNandorInParty) and (agreedToBeLeader or not partyFlagNándor):
    
        *It was hard to see during the lockdown, but the guards are undermanned. We outnumber them greatly!
            {
            -not gainedFervorFromLockdownExplanation:
                ~gainedFervorFromLockdownExplanation = true
                ~crowdFervor++
            }
            ->3b
    }
    
    {
    -gaveKastorToolBundle:
        *Who among you can say that you do not know a pick backwards and forwards? Or a shovel, or a mattock? The tools in front of you can be mighty in even a slave's hands. Perhaps especially a slave's!
            {
            -not gainedFervorFromToolsMention:
                ~gainedFervorFromToolsMention = true
                ~crowdFervor++
            }
            ->3c
    }
    
    {
    -charisma >= 3:
        *\*Address the crowd.* Look at what we have here. The choosiest beggar, come to tell you that the guards are too mighty and handsome to be taken down by the likes of us! <Cha {charisma}/3>
            {
            -not gainedFervorFromChaOption:
                ~gainedFervorFromChaOption = true
                ~crowdFervor++
            }
            ->3e
    }
    {
    -strength >= 3:
        *\*Address the crowd.* I have personally slain the Overseer of this portion of the camp! Look at how his blood spatters my clothes and tell me the guards are so skilled! So mighty! <Str {strength}/3>
            {
            -not gainedFervorFromStrOption:
                ~gainedFervorFromStrOption = true
                ~crowdFervor++
            }
            ->3f
    }
    
    {
    -partyFlagNándor && agreedToBeLeader:
        *\*Look to Nándor.*
            {
            -not gainedFervorFromNandorExplanation:
                ~gainedFervorFromNandorExplanation = true
                ~crowdFervor+=5
            }
            ->3d
    }
    
    {
    -not knowsAboutTheMine and not gaveKastorToolBundle and not partyFlagNándor and charisma < 3 and strength < 3 and crowdFervor < 3:
        *Uh... well, I just thought-
            ->3aa
    }
    
=== 3aa ===

Just thought what? That you could take the whole camp by yourself? That we might come along to get slaughtered with? I'm not sticking around to be seen with you, but don't worry. I'm sure the guards will force me to watch your execution.
    
    ->deactivateExtras

=== 3b === //It was hard to see during the lockdown, but the guards are undermanned. We outnumber them greatly!

changeCamTarget({slave1Index})

What do you mean? Where did the other guards go? How greatly?

    +The guards instated the lockdown because they did not have the personnel to maintain security in the camp. They were killed when a group of miners delved into a nest of monsters, causing the evacuation that happened a few days ago.
        ->3ba

=== 3ba === 

{
-not partyFlagNándor:
keepDialogue()
}

\*The older slaves strokes his chin.* Hmm, that would explain why the work hasn't resumed yet.

{
-partyFlagNándor:

changeCamTarget({nandorIndex})

keepDialogue()

It's true. And this one right here fought their way deep into the mine, braving what the guards could not to rescue Carter and I. For that, they have my utmost loyalty.
}

->3a

=== 3c === //Who among you can say that you do not know a pick backwards and forwards? Or a shovel, or a mattock? The tools in front of you can be mighty in even a slave's hands. Especially a slave's.

changeCamTarget({slave2Index})

There are those of us who aren't so old as to cling to life so desperately. I'm sick of sitting around. Put a tool my hand and I'll swing it at a guard. No questions asked.

changeCamTarget({theCrowdIndex})

keepDialogue()

\*Murmurs of ascent rise out of the crowd.*

->3a

=== 3d ===

changeCamTarget({nandorIndex})

You all have suffered under this lockdown, but you may not know for what reason. Days ago, while digging on the lowest level of the mine, my team and I ran afoul of a cavern beneath the earth filled with packs of deadly monsters. A horde of enormous, ravenous worms flooded the mine, which forced the guards to call for the evacuation you all remember.

During the initial confusion, many guards were slain or trapped as I had been deep within the mine. *Nándor grabs your wrist and raises it high for all to see.* And had it not been for the efforts of the one you see before you here, I would still be trapped there. 

Their accomplishments are many: They are the one who personally saved myself and others from being trapped below the earth. They are the one who has secured the equipment you see before you. They have already slain many guards, and will be leading our assault on the Manse.

I would not ask what I ask of you now if I was not sure it would succeed. The guards' strength is depleted, but ours is bolstered! With {playerName}'s might, we can break any barricade the guards put in our path. We can smash our way into the Manse, take retribution on the Director, and eat from their stores as kings! 

keepDialogue()

\*The crowd gives a cheer. Nándor has whipped this weary group of slaves into an excitement you would not have expected at the start of his speech.*

->3a

=== 3e === //\*Address the crowd.* Look at what we have here. The choosiest beggar, come to tell you that the guards are too mighty and handsome to be taken down by the likes of us! <Cha {charisma}/3>

\*The slave looks back at you with surprise.* No, you misundersta-

    +No, we understand you plainly enough: you have been a slave long enough that you have grown used to your shackles. Just make sure the next time you want to speak up for the guards you don't slander the rest of us in the same breath!
        keepDialogue()
        \*The older man looks around sheepishly at the glares he is receiving from the crowd.*
        -> 3a

=== 3f === // \*Address the crowd.* I have personally slain the Overseer of this portion of the camp! Look at how his blood spatters my clothes and tell me the guards are so skilled! So mighty!

keepDialogue()

\*The crowd marvels at your figure. The difference between your toned physique and the malnourished figure of the slaves near you makes you look all the more powerful.* 

->3a

=== 4a ===

setToTrue(convincedSlavesToHelpYou)

changeCamTarget({theCrowdIndex})

\*The crowd has grown excited. They are abuzz with energy; energy which now can be channeled towards violence against the guards.*

    +Our time of meekness is at an end! No longer will the guards eat while we starve! No longer will they rest while we labor!
        ->5a
    +It is time! You have been slaves too long! To the Manse! To freedom! <Leave>
        ->deactivateExtras

=== 5a ===

\*The gathered slaves stand enraptured, hanging on your every word.*

//    +Our time of meekness is at an end! No longer will the guards eat while we starve! No longer will they rest while we labor! <Cha {charisma}/3>

    +It is against the natural order for humans to own humans! Fill your hearts with the knowledge that you fight on the side of the righteous!
        \*The slaves nod at your words with solemn but palpable purpose.*
        ->5b
    +Sunder your shackles! Break their barricades! Show them that you are not animals to be caged, but men to be feared!
        \*The slaves begin to clench their fists, and look to the tools at their feet with determination.*
        ->5b
    +With each flogging, they have begged you to think them invincible. The guards I have already slain put this lie to rest!
        \*The slaves look to you with an envious wonder.*
        ->5b

=== 5b ===

    +With these tools, we can build a better future! The time has come to put right what they have done with your own hands!
        \*The slaves bristle with a hopeful energy. For the first time, they look to the future with longing.*
        ->5c
    +When I look at you, I do not see slaves. I see a people with the will to take back their pride!
        \*Your words summon the vestiges of the slaves pride. They all stand a little straighter.*
        ->5c
    +This uprising was inevitable! Our victory is inevitable! The guards are not our victims, but the unwitting casualties of their own actions!
        \*There is no mistaking it. The slaves look ready for war.*
        ->5c

=== 5c ===
    
    +They have subjected you to every torture in the book! Now is your opportunity to write the sequel in their blood!
        \*The slaves lunge at the pile of weapons. The riot begins in earnest!*
        ->deactivateExtras  
    +The time of the masters is over! *Raise your weapon in the air.* They have taught us not to fear death! Now we shall teach that fear to the masters!
        \*A chant goes up as the slaves grab their weapons.* Death to the masters! Death to the masters!
        ->deactivateExtras
    +I pledge before the night is through you will be free! The dawning sun brings with it the first day of your new lives!
        \*The slaves steel their hearts for battle as they choose their weapons. The revolution begins.*
        ->deactivateExtras

=== 6a ===

{
-gotKnifeFromClay and not deathFlagThatch:
    changeCamTarget({thatchIndex})
    
    Good people! I... *The crowd turns to look at Thatch, and he looks like he immediately regrets speaking up.* I know t-this is a d-decision none of us can make lightly. And you k-know me: I look tough, but I have a coward's heart. 
    
    \*Thatch points at you.* But {playerName} helped this coward when they didn't have to. They would not lead you astray, not over something this important! This coward will fight with them, and if you won't fight for yourselves, what does that make you?
    
    {
    -not deathFlagClay:
        changeCamTarget({clayIndex})
        
        \*Clay looks from you, then to Thatch, then back to you.* I will not have it be said that I was indolent while other's bled for me. I also will fight with {playerName}. And besides, I'd rather be flogged 'til I quit this life than spend one more minute in lockdown.
    }
    
    ~crowdFervor++
    changeCamTarget({theCrowdIndex})
    
    keepDialogue()
    
    \*The other slaves murmur their assent.*
    ->3a
-gotKnifeFromClay && not deathFlagClay:
changeCamTarget({clayIndex})

 keepDialogue()

\*Clay steps forward and addresses the crowd.* I know this one. In the short time they have been in this camp, they have proven they have the guts to pull this off. If any of you have doubts, forget them; follow {playerName} with me and claim your freedom with us.

~crowdFervor++

->3a
-not deathFlagClay:

changeCamTarget({clayIndex})

\*Clay steps forward and addresses the crowd.* I've walked the world enough to know not to trust the providence of strangers. If someone comes along and tells you they're going to solve all your problems they're either trying to sell you something, deranged, or both!
    
    +Does your own paranoia blind you to all the dead guards at your feet?
        ->6b
    +If someone was selling me the chance to crack guard skulls, I'd buy two.
        ->6c
}

=== 6b ===

Aye, you've killed some guards. But there are many more. How do we know you won't cut and run if things begin to turn for the worst?
    {
    -wisdom >= 3:
    +You don't, and neither do I about you. But I'm the one putting aside my fears and working towards the common good. <Wis {wisdom}/3>
        changeCamTarget({slave2Index})
        
        \*A young man puts his hand on Clay's shoulder.* Anyone who fights together needs to trust each other. Let us return their trust with a little of our own to start.
        
        changeCamTarget({clayIndex})
        
        keepDialogue()
        
        Fine. We'll work together. But we'll all be watching how you lead us. Don't disappoint.
        
        ~crowdFervor++
        ->3a
    }

    {
    -gaveKastorToolBundle:
        +The tools at your feet prove I took many risks before I ever asked you to take any.
            keepDialogue()
            \*Clay scans your face, but finally nods.* I guess I'll have to accept that. But I'll be watching how you lead us. We all will be.
            ~crowdFervor++
        ->3a
    }
    
    +If you're going to be like this after a triumph. I can only imagine what you'll be like if defeat looms. The revolt can do without your help.
        ~failedToConvinceClay = true
        setToTrue(failedToConvinceClay)
        keepDialogue()
        That's good, because I'm not going to be sticking around long enough to find out how it ends up for you.
        ~crowdFervor--
        ->3a

=== 6c ===

changeCamTarget({theCrowdIndex})

\*Your retort earns a round of chuckles from the crowd.*

changeCamTarget({clayIndex})

Ha! Well, perhaps you're right. But I want to know you've got a bigger plan going than just charging their barricades face first.

    {
    -gotThePlanFromKastor and charisma >= 3:
        +Worry not. The plan has been in motion long before I arrived at the camp. <Cha {charisma}/3>
            keepDialogue()
            Good! Very good. Clearly we are in good hands.
            ~crowdFervor++
            ->3a
    }
    {
    -terrifiedImre or convincedImre:
    +We have friends among the Manse slaves. They will aid us from within when the time comes.
        keepDialogue()
        \*Clay raises an eyebrow at this.* You get around. But if it is as you say, then the guards are going to have a hard time on their hands.
        ~crowdFervor++
        ->3a
    }
    +I give my word that if any barricades need charging, my face will be at the fore.
        ~failedToConvinceClay = true
        setToTrue(failedToConvinceClay)
        keepDialogue()
        \*Clay visibly balks at this.*
        ~crowdFervor--
        ->3a

=== 7a ===

changeCamTarget({garchaIndex})

It looks like everything is set. Remember, if things get tough in there you can always come back here and Kastor and I will patch you up. If you're in need of something a little more material, go find Uros by the barricade. I put him in charge of collecting things from the stores and dead guards in case we need them later. And don't you dare forget about rescuing Broglin!

fadeToBlack()

deactivate({garchaIndex})
activate({afterOverseerParentIndex})

fadeBackIn(60)    

->Close

=== deactivateExtras ===

{
-not deathFlagGarcha and kastorStartedRevolt and crowdFervor >= crowdAppeasementThreshold:
    fadeToBlack(true, false)
-else:
    fadeToBlack()
}


deactivate({nandorIndex})
deactivate({carterIndex})
deactivate({janosIndex})
deactivate({clayIndex})
deactivate({slave1Index})
deactivate({slave2Index})
deactivate({slave3Index})
deactivate({slave4Index})
deactivate({theCrowdIndex})

fadeBackIn(60)    

{
-crowdFervor >= crowdAppeasementThreshold:
activateQuestStep(The Plan,13)
-else:
activateQuestStep(The Plan,14)
}

{
-not deathFlagGarcha and kastorStartedRevolt and crowdFervor >= crowdAppeasementThreshold:
->7a
}

->Close

=== Close ===

close()

->DONE
