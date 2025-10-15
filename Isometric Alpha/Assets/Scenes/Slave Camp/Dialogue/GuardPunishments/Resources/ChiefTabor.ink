VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR attitude = 0

VAR clayWillSeekOutTabor = false

VAR gaveAGuardToTheCrowd = false
VAR executedAnyGuard = false

VAR didNotExecuteMarcos = false
VAR gaveMarcosToTheCrowd = false
VAR gaveMarcosFiftyLashes = false
VAR executedMarcos = false

VAR didNotExecuteAndras = false
VAR gaveAndrasToTheCrowd = false
VAR gaveAndrasFiftyLashes = false
VAR executedAndras = false

VAR directorSaidAbsurdityQuote = false
VAR allowedDirectorToSpeak = false
VAR keptDirectorAlive = false
VAR letTaborLive = false
VAR acceptedTaborsSurrenderAfterDirectorFight = false

VAR heardTaborsLesson = false
VAR threatenedTaborForInformation = false
VAR saidYouWishToTeachTabor = false
VAR attitudeModFromAskingQuestions = false
VAR attitudeModDrivelRedemptionChoice = false
VAR attitudeModRedemptionAnswerReply = false
VAR clayChoicesMade = 0

VAR currentKnot = ->3a

VAR taborAfterClayFightKey = "taborAfterClayFight"


CONST rageThreshold = 3
CONST rageName = "enraged"
CONST angryThreshold = 2
CONST angryName = "angry"
CONST agitatedThreshold = 1
CONST agitatedName = "agitated"
CONST calmedThreshold = 0
CONST calmName = "calm"
CONST calmedName = "calmed"

CONST crowdMoreEnragedPrefix = "*The crowd has become even more "
CONST crowdIncreasedPrefix = "*The crowd has become "
CONST crowdStillPrefix = "*The crowd is still "
CONST crowdDecreasedPrefix = "*The crowd is now only "
CONST crowdCalmedPrefix = "*The crowd has been "

CONST crowdWontAcceptSuffix = ". They don't wish to let Tabor live.*"
CONST crowdWillAcceptSuffix = ". They will accept your verdict.*"

VAR crowdStatus = calmedThreshold
VAR crowdStatusDescription = ""

VAR executedTabor = false
VAR didNotExecuteTabor = false
VAR gaveTaborToTheCrowd = false
VAR crowdForcedTaborExecution = false

VAR letNandorDecideGuardPunishments = false
VAR mineLvl3ConvincedRekaAndPazman = false

VAR deathFlagGuardMárcos = false

VAR deathFlagGuardAndrás = false
VAR acceptingGuardPrisoners = false
VAR gotKeyFromJanos = false

VAR crowdDispersed = false

VAR marcosIsAtTrial = false
VAR andrasIsAtTrial = false
VAR guardPazmanAndRekaAtTrial = false
VAR janosIsAtTrial = false

VAR marcosNeedsHandling = false
VAR andrasNeedsHandling = false
VAR rekaNeedsHandling = false
VAR pazmanNeedsHandling = false


VAR fromFightDialogueFlag1 = false
VAR fromFightDialogueFlag2 = false
VAR fromFightDialogueFlag3 = false
VAR fromFightDialogueFlag4 = false

VAR taborIndex = 1
VAR crowdIndex = 2
VAR clayIndex = 3

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

changeCamTarget({taborIndex})

{
-fromFightDialogueFlag1:
    ->fromFightDialogueKnot1
-fromFightDialogueFlag2:
    ->fromFightDialogueKnot2
-fromFightDialogueFlag3:
    ->fromFightDialogueKnot3
-fromFightDialogueFlag4:
    ->fromFightDialogueKnot4
}



{
-not taborIsNext():
    ->1ac
-letTaborLive and not acceptedTaborsSurrenderAfterDirectorFight:
    ->1a
-gaveAGuardToTheCrowd or executedAnyGuard:
    ->1aa
-else:
    ->1ab
}

=== 1a === 

\*Tabor is shirtless, and his entire upper torso is covered in bruises. Some bandages have been applied to his stomach and face, and he holds himself as if some of his ribs are broken. His breathing is obviously labored as he speaks* Come to finish me off yourself, you dogsbody scum?

    +I wish we had the opportunity to speak before you were attacked. Alas, it was not meant to be.
        ->1b
    +Quiet. You will only speak when addressed.
        ->1ba

=== 1aa === 

You trick me into surrendering without a fight, and then you commence with the executions? How very devious of you, branded. You had even myself fooled, but you showed your true colors eventually, didn't you?

    +Our deal still holds. Wait a moment as I announce your fate.
        \*Tabor closes his eyes* Forgive me if I don't raise my hopes too high.
        ->2a
    +The others' crimes have no bearing on my verdict for you. 
        ->2a
    +Quiet. You will only speak when addressed.
        keepDialogue()
        \*Tabor stares up at you, silently. His eyes sear with his disdain.*
        ->2a

=== 1ab === 

Your words in the Manse affected me greatly, branded. Yet here we all are, lined up in front of the other escapees. Much of me wonders if you are going back on your word.

    +\*Whisper below the noise of the crowd* Our deal still holds. Wait a moment as I announce your fate.
        keepDialogue()
        \*Tabor closes his eyes* Get on with it. Whatever happens, I am eager to see this concluded.
        ->2a
    +Quiet. You will only speak when addressed. *Wink at Tabor*
        keepDialogue()
        \*Tabor studies you, but does not speak*
        ->2a

=== 1ac === 

Ah, is it my time on the block?

    +We're saving you for last, Tabor. Keep quiet until then.
        ->Close

=== 1b ===

If you are here to gloat, save it for after my head leaves my shoulders.

    {
    -not acceptedTaborsSurrenderAfterDirectorFight:
    +Do not be in such a hurry to die. There is still much I wish to know about you.
        ->1c
    }
    +No gloating: the crowd awaits my verdict.
        ->2a

=== 1ba ===

\*Tabor stares up at you, his face contorted in the firmest grimace it can muster. His bloodshot eyes stare hatefully from behind swollen eyelids*

    {
    -not acceptedTaborsSurrenderAfterDirectorFight:
    +Do not be in such a hurry to die. There is still much I wish to know about you.
        ->1c
    }
    +Lets get on with it. The crowd awaits my verdict.
        keepDialogue()
        \*Tabor looks like he wishes to say something derisive, but keeps his mouth closed.*
        ->2a

=== 1c ===

Mocking me is no better. You try my patience, branded.
    
    +Do not mistake my interest for jest. I simply wish to know who I have been at odds with.
        ->9a
    +I offer no mockery. I will not wonder what could have been gleaned before your passing.
        ->9a
        
=== 2a ===

A verdict? The criminal is now a judge? What absurdity. *Tabor spits*

    +\*Address the crowd:* Hear me, my friends! I have decided Tabor's fate!
        ->2b

=== 2b ===

changeCamTarget({crowdIndex})

\*The crowd quiets down, awaiting your decision*


    +I declare Chief Tabor's life belongs to me! As the one who has bled the most for this rebellion, I suspend his sentence! 
        ->3a
    
    +There can be no just punishment for his crimes but death! Let it be a swift execution: may his death serve as an instruction in the mercy he lacked in life!
        ->6a
    
    +The worst of Tabor's crimes were committed against the lot of you! Therefore, the lot of you will take part in his lashing! Each slave shall serve him a lash for every month they had been his captive!
        ->7a

=== 3a ===

changeCamTarget({crowdIndex})

\*A sense of shock ripples through the crowd. Shouts and boos erupt as they let their ire be known*

changeCamTarget({clayIndex})

We must not understand your meaning. You're not letting him go... are you?

    +Of course not. I will keep him in my custody. But he will wear no chains, and receive no branding.
            ->3aa.starting_dialogue_section

=== 3aa ===

= starting_dialogue_section
~currentKnot = ->3aa.choice_section

    changeCamTarget({crowdIndex})
    
    ~crowdStatus = rageThreshold
    \*The noise of the crowd builds. They make their rage known.*
    
    changeCamTarget({clayIndex})
    
    You have no right! You've only been here for a day, while we've suffered at his hands for months! Kill him! Or let us!

    ->currentKnot
= choice_section

{
-clayChoicesMade >= 4 or crowdStatus <= calmedThreshold:
    +\*Address the crowd.* Enough! I am done discussing it. Tabor is my responsibility. None of you need even consider him after today!
        {
        -crowdStatus > calmedThreshold:
            ->5a
        -else:
            ->4a
        }
}

{
-charisma >= 4:
    +As the one who accepted his surrender, it is my right to chose his fate. If any of you harm him, I will take it as a personal insult. <Cha {charisma}/4>
        ->madeHighCharismaCheck1a
    +\*Address the crowd.* I give you my word that I will answer all of your questions, but first we must have some peace so I can hear them! <Cha {charisma}/2>
        ->madeLowCharismaCheck1a
-charisma >= 2:
    +\*Address the crowd.* I give you my word that I will answer all of your questions, but first we must have some peace so I can hear them! <Cha {charisma}/2>
        ->madeLowCharismaCheck1a
    *No right? Without my contributions, you would still be in chains!  
        ~clayChoicesMade++
        ->3ab
-else:
    *No right? Without my contributions, you would still be in chains!  
    ~clayChoicesMade++
        ->3ab
}

    *It's true, he has done nothing to me. That's what allows me to keep a clear head about this.
    ~clayChoicesMade++
        ->3ac
        
    *His punishment will be for me to administer. I will make certain he will work for the good of all branded.
    ~clayChoicesMade++
        ->3ad

    *You all know what I have done to realize this rebellion. *Point at Clay* But I know nothing of this one's struggles. What claim do you have to Tabor's life?
    ~clayChoicesMade++
        ->3ae

=== madeLowCharismaCheck1a ===

    changeCamTarget({crowdIndex})
    
    \*Your words have the desired effect. The crowd quiets down some to listen to your words.*

        ->returnTo3aaSuccess

=== madeHighCharismaCheck1a ===

    changeCamTarget({crowdIndex})
    
    \*The crowd, in an attempt to prevent from insulting you, stops their shouting. More than one of the branded awkwardly refuse to meet your gaze.*

    changeCamTarget({clayIndex})

    Your contributions are without question. But I don't understand: why would you choose to keep him alive after all he's done to us?

{
-not gaveAGuardToTheCrowd and not executedAnyGuard:
    +There has been too much blood shed today. Peace will reign while I am around to keep it.
        ->madeHighCharismaCheck1e
}

    +My reasons are my own. That will have to suffice.
        ->madeHighCharismaCheck1c
    +I could kill him, and he would be powerless to stop me. But consider that <i>not</i> killing him exercises even greater power.
        ->madeHighCharismaCheck1d
    +After all I've done for you, I now owe you his life too? What else must I give you, my clothes? My firstborn?
        ->madeHighCharismaCheck1b

=== madeHighCharismaCheck1b ===

    changeCamTarget({clayIndex})

    No, of course not. I meant no ingratitude by what I asked, I only meant to express my confusion regarding your decision...

    +All is forgiven. Now is not the time for us to quibble over minor details; we should instead celebrate our victory!
        ->clayGivesUpWithSuspicion(->crowdDispersesToCelebrate)
    +\*Laugh loudly.* The sky is blue, the leaves are green, and Clay is confused. I see nothing out of place. 
        ->crowdLaughsAtClay
    +My apologies. I had not realized that I needed your approval on how to conduct this revolution. Worry not, the next time I free you from your shackles I will remember to ask your permission first.
        ->madeHighCharismaCheck1ca

=== madeHighCharismaCheck1c ===

    changeCamTarget({clayIndex})

    You want us to let this monster live and offer no explanation?

    +You trusted me to lead you against the guards, and I fulfilled all of my promises then. Does that not prove my benevolence?
        ->madeHighCharismaCheck1cc
    +It would take too long to explain. I offer no explanation because I would no waste your time with it.
        ->madeHighCharismaCheck1ca
    +You are not owed an explanation. I will do what I like with my own prisoner.
        ->madeHighCharismaCheck1ca

=== madeHighCharismaCheck1ca ===

    setToTrue(clayWillSeekOutTabor)

    changeCamTarget({clayIndex})

    \*Clay begins to say something, but is prevented from speaking by another slave jostling him with their elbow. Instead, Clay gives you a silent look of suspicion.*

    ->madeHighCharismaCheck1cb

=== madeHighCharismaCheck1cb ===

    +The rest of you that have faith in me, let me express to you my gratitude. I have nothing more to say; you are all free to leave.
        ->crowdDisperses
    +Please, do not let me waste your time on this minor detail that you could be using to celebrate your freedom. This meeting is adjourned.
        ->crowdDispersesToCelebrate

=== madeHighCharismaCheck1cc ===

    changeCamTarget({clayIndex})

    You are right. We placed our faith in you and you did not disappoint. Tabor has hurt so many of us, and my own grudge against him runs deep, but if you ask it of me, I will leave it behind.

    +Thank you. I know that was not easy. With that settled, we have nothing more to discuss. Everyone is free to leave.
        ->crowdDisperses
    +Good. With that settled, I will not waste any more of everyone's time on this minor detail. This meeting is adjourned.
        ->crowdDisperses

=== madeHighCharismaCheck1d === 

    changeCamTarget({clayIndex})

    I don't understand. What greater power?

    +Even Tabor could not pardon a slave who raised a sword against him. But if I pardon him, I prove us more powerful than he ever was.
        ->madeHighCharismaCheck1da

=== madeHighCharismaCheck1da === 

    changeCamTarget({clayIndex})

    \*Clay ponders your words for a moment.* To do what he could not is power, in a way. But what use is it? How does it benefit us?

    +To pardon Tabor is to no longer have to spend another second thinking about him. Discard him, and live beyond him.
    
        Your words are wise. He is but discarded trash now: no longer worth my time. I retract my objections.
            ->madeHighCharismaCheck1db
    +It is the ultimate insult. He could torture us all he likes, and for all his effort he has made no more mark on our minds than a storm against a mountain.
        
        "A storm against a mountain." I like that. We have defeated the Lovashi. To them, we <i>are</i> like mountains: indomitable.
            ->madeHighCharismaCheck1db

=== madeHighCharismaCheck1db ===

    +Thank you. I have nothing more to say; you are all free to leave.
        ->crowdDisperses
    +I'm glad you agree. Now, let us no longer dwell on this. Instead, we should be celebrating your freedom! This meeting is adjourned.
        ->crowdDispersesToCelebrate

=== madeHighCharismaCheck1e ===

//There has been too much blood shed today. Peace will reign while I am around to keep it.

    changeCamTarget({clayIndex})

    What is but a little more blood in the face of everything that has happened? He has tormented so many, why not let the last one his deeds have hurt be himself?

    +As the one who has personally shed much of that blood, I am the one who decides when it ends.
        ->madeHighCharismaCheck1ea
    +There will always be an excuse to continue the violence. I won't hear any more of them.
        ->madeHighCharismaCheck1ea

=== madeHighCharismaCheck1ea ===

    changeCamTarget({crowdIndex})
    
    \*Some of the crowd begins to nod along to what you say. Your words have earned their agreement.*

    changeCamTarget({clayIndex})

    \*Clay begins to say something, but is prevented from speaking by another slave jostling him with their elbow.* I see I am alone in my opinion. I retract my objections.

    ->madeHighCharismaCheck1db
    
=== crowdLaughsAtClay ===
    
    setToTrue(clayWillSeekOutTabor)

    changeCamTarget({crowdIndex})
    
    \*Your joke earns a round of laughter from the crowd at Clay's expense.*

    changeCamTarget({clayIndex})

    \*Clay's face turns red and he begins to say something, but is prevented from speaking by another slave jostling him with their elbow. Instead, Clay gives you a silent look of anger.*

    ->madeHighCharismaCheck1cb

=== clayGivesUpPeacefully(->divert) ===

    changeCamTarget({clayIndex})

    \*Clay looks like he wishes to press the point, but thinks better of it.*

    ->divert

=== clayGivesUpWithSuspicion(->divert) ===

    setToTrue(clayWillSeekOutTabor)
    changeCamTarget({clayIndex})

    \*Clay looks like he wishes to press the point, but thinks better of it. Instead, he regards you with a silent look of suspicion.*

    ->divert

=== crowdDispersesToCelebrate ===

    changeCamTarget({crowdIndex})
    
    \*The crowd seems happy enough to move on, and raises a cheer. They follow your directions and start to celebrate, dispersing as they do.*

    fadeToBlack(true, false)

    setToTrue(crowdDispersed)
    deactivate({crowdIndex})
    deactivate({clayIndex})

    fadeBackIn(60)
    ->4ba

=== crowdDisperses ===

    changeCamTarget({crowdIndex})
    
    \*The crowd seems happy enough to move on, and they begin to disperse.*

    fadeToBlack(true, false)

    setToTrue(crowdDispersed)
    deactivate({crowdIndex})
    deactivate({clayIndex})

    fadeBackIn(60)
    ->4ba

=== 3ab ===

= starting_dialogue_section
~currentKnot = ->3ab.choice_section

    changeCamTarget({clayIndex})

    Your contributions are without question. But that does not mean you can disregard our recompense. His life belongs to those he's harmed!

    ->currentKnot
= choice_section

    +Do not so quickly dismiss what I have done. I am owed by each of you. This is the manner in which I will collect that debt.
            changeCamTarget({clayIndex})
        
            Our debt to you is of a different nature. We owe you our own lives, not the life of our greatest antagonist.  *Clay grimaces, as if his rebuke has left a bad taste in his mouth*
                ->returnTo3aaSuccess
        
    +He has abused too many for any of you to claim him individually. What, you wish to each harm a scrap of him? Isn't that kind of pathetic?
            
            changeCamTarget({clayIndex})
    
            You don't get to judge us! We've waited four long months for this moment, we won't be denyed it!
                ->returnTo3aaFailure

    +Is this how we settle our grudges? Lynchings? If we kill him in this manner, we prove his view of us right!
    
        {
        - gaveAGuardToTheCrowd:
            
            changeCamTarget({clayIndex})
        
            One moment you encourage us to seek our revenge, and the next you condemn us for it? You disgust me.
                ->returnTo3aaFailure
        - else:
            ->3aba
        }

=== 3aba ===

= starting_dialogue_section
~currentKnot = ->3aba.choice_section

    changeCamTarget({clayIndex})

    But... letting him live? *Clay clenches and unclenches his fists* Part of me wonders if I even care if we grant him that victory before he dies, so long as he dies.

    ->currentKnot
= choice_section

    +We have our whole lives ahead of us again. Don't give in now, at the final moment. You may never forgive yourself.
            ->returnTo3aaSuccess


=== 3ac === //It's true, he has done nothing to me. That's what allows me to keep a clear head about this.

= starting_dialogue_section
~currentKnot = ->3ac.choice_section

    changeCamTarget({clayIndex})

    Then stay out of it. His fate should not concern you!

    ->currentKnot
= choice_section

    +Guard Márcos was given a similar order by his Overseer. Had he followed it, Nándor and Carter would be dead right now.
        {
        - gaveMarcosToTheCrowd or executedMarcos:
            
            changeCamTarget({clayIndex})
        
            \*Clay scoffs* You shouldn't expect us to follow the example of one of the guards. Especially one that was so recently executed.
                ->returnTo3aaFailure
        - else:
                ->3aca
        }
    +I've already given you my verdict. Are you challenging my authority?
            ->3acb
{
-not executedAnyGuard and not gaveAGuardToTheCrowd:
    +I can't stay out of it. After all the death I've seen, I won't sit idle when I could have prevented it from continuing.
            ->3acc
}


=== 3aca === //It's true, he has done nothing to me. That's what allows me to keep a clear head about this.

= starting_dialogue_section
~currentKnot = ->3aca.choice_section

    changeCamTarget({clayIndex})

    This is different!

    ->currentKnot
= choice_section

    +I'm not so certain. And it's that uncertainty that leads to me preserve him from the finality of death.
            ->returnTo3aaSuccess

=== 3acb === // I have already claimed his life as mine. His fate now concerns me, no matter how much you argue. Unless... you're challenging my authority?

= starting_dialogue_section
~currentKnot = ->3acb.choice_section

    changeCamTarget({clayIndex})

    And what if I am?

    ->currentKnot
= choice_section

{
-strength >= 3:
    +\*Crack your knuckles and grin* Then you and I are gonna have a problem. <Str {strength}/3>
        changeCamTarget({clayIndex})
    
        No no no, no problem here. We can keep this conversation civil, can't we?
            ->returnTo3aaSuccess
-strength < 3:
    +Then y-you're lucky there's a waist-high fence between us or I-I'd show you the move I t-took out the Director with. <Str {strength}/3>
        changeCamTarget({clayIndex})
    
        Sure, friend. If you fight as pathetic as you sound I could take three of you.
            ->returnTo3aaFailure
}

{
-charisma >= 3:
    +That would make you an 'enemy of the revolution'. Are you looking to join the line up? <Cha {charisma}/3>
        changeCamTarget({clayIndex})
    
        Of course not! Don't you remember when you gave us the tools? No one believed in you then more than I did!
            ->returnTo3aaSuccess
}
    
    +I'd respect it, but I wouldn't bow to it. We're all equals now, after all. That's what we fought for.
        changeCamTarget({clayIndex})
    
        That so? Then hear us when we say Tabor can't be allowed to live. Its an insult to everyone that died in the fighting, its an insult to his victims, and its an insult to justice.
            ->returnTo3aaFailure

=== 3acc === //    +I can't stay out of it. After all the death I've seen, I won't sit idle when I could have prevented it from continuing.

= starting_dialogue_section
~currentKnot = ->3acc.choice_section

    changeCamTarget({clayIndex})

    Do you feel no shame? What of his crimes against us? You speak as if he is a victim! 
    
    ->currentKnot
= choice_section

    +He is not a victim unless you make him yours. I will not allow you to become even one step closer to being like him.
        changeCamTarget({clayIndex})
    
        \*Clay looks as if he wants to speak, but no retort is forth coming*
            ->returnTo3aaSuccess

=== 3ad === //His punishment will be for me to administer. I will make certain he will work for the good of all branded.

= starting_dialogue_section
~currentKnot = ->3ad.choice_section

    changeCamTarget({clayIndex})

    If you plan to punish him, then why delay it? We can finally be rid of him, here and now.
    
    ->currentKnot
= choice_section
{
-not gaveAGuardToTheCrowd:
    +I will punish him when he longer is useful to me. To do so before would be counterproductive.
            ->3ada
}

    +His punishment will be carried out when I can confirm it will be humane. I don't believe anyone here but myself can ensure that.
            ->3ada
    +I suspended his sentence because I believe his actions were committed as an earnest attempt to teach morality to his victims. I wish to see if I can teach him the morality he himself lacked.
        ~saidYouWishToTeachTabor = true
            ->3ada

=== 3ada === //His punishment will be for me to administer. I will make certain he will work for the good of all branded.

= starting_dialogue_section
~currentKnot = ->3ada.choice_section

    changeCamTarget({clayIndex})

    What utter nonsense. The man is a sadist. He delights in the suffering of others. Don't waste your time on him! Give him to us and have done with it.
    
    ->currentKnot
= choice_section

{
-saidYouWishToTeachTabor:

    +You're not seeing the opportunity this provides. If he can be taught to coexist with us, maybe other Lovashi can as well. This could be the first step towards a more peaceful future!
            ->3add
}

    +Nothing about his demeanor has made me believe he enjoyed your suffering. However, it seems like you would yourself enjoy watching him suffer.
            ->3adc

    +Why would you wish to waste your own time on him? Do you love this camp so much you cannot bring yourself to leave it?
            ->3adb

=== 3adb === //Why would you wish to waste your own time on him? Do you love this camp so much you cannot bring yourself to leave it?

= starting_dialogue_section
~currentKnot = ->3adb.choice_section

    changeCamTarget({clayIndex})

    \*Clay looks back at you confused* Love? For this camp? This is the last place I wish to be.
    
    ->currentKnot
= choice_section

    +Then allow me to remove this final burden from you. I shall punish him in the way I see fit, and you can start your new lives that much quicker.
        changeCamTarget({clayIndex})
    
        Only hours ago I would have done anything to never spend another second in this camp. \*Clay's rigid expression cracks for a brief moment as he looks at Tabor* Perhaps that includes never giving you another thought.
            ->returnTo3aaSuccess

=== 3adc === //Nothing about his demeanor has made me believe he enjoyed your suffering. However, it seems like you would yourself enjoy watching him suffer.

= starting_dialogue_section
~currentKnot = ->3adc.choice_section

    changeCamTarget({clayIndex})

    Oh yes. Very much so. 
    
    ->currentKnot
= choice_section

{
-wisdom >= 3:
    +Then it seems that Tabor has seeded within you the very thing he wished to stamp out. It now strangles your thoughts as a parasitic vine chokes a great oak. If you do not leave that aspect of yourself behind in this camp, it may become you entirely. <Wis {wisdom}/3>
    
        changeCamTarget({clayIndex})
    
        Become... like him? *Clay looks down at his own hands with transparent horror*
            ->returnTo3aaSuccess
-wisdom < 3:
    +Would you believe me if I said that's a bad sign for your state of mind? <Wis {wisdom}/3>
    
        changeCamTarget({clayIndex})
    
        Does it look to you like I care about such a thing? When we're through with him, we can look to ourselves for the rest of your lives.
            ->returnTo3aaFailure
}

    +Are you so far gone that you can't hear how villainous you sound?
        changeCamTarget({clayIndex})
    
        Not only do you see Tabor as a victim, but now you see the branded as the villains? Your true colors are becoming more apparent by the minute.
            ->returnTo3aaFailure

=== 3add === //You're not seeing the opportunity this provides. If he can be taught to coexist with us, maybe other Lovashi can as well. This could be the first step towards a more peaceful future!

= starting_dialogue_section
~currentKnot = ->3add.choice_section

    changeCamTarget({clayIndex})

    Who would be so irresponsible as to wish to coexist with the Lovashi? What would be the point? So they can stab us in the back once we lay down our arms?
    
    ->currentKnot
= choice_section

    +This is the exact thinking I hope to stamp out. If we can teach each other to trust again, we won't need to fight them at all!
        changeCamTarget({clayIndex})
    
        \Clay laughs harshly* Your ideals are more ludicrous than I had realized! Do you have a fever? Or have you been eating strange plants lately, perhaps?
            ->returnTo3aaFailure
            
    +My time as a slave has shown me the worst abuses humanity can inflict. I will walk every path, explore every possibility to keep them from happening to others. And if your mind cannot understand that, then you were lost well before you ever arrived at this camp.
        changeCamTarget({clayIndex})
    
        \*Clay meets your gaze, but then turns away. His face reddens in shame*
            ->returnTo3aaSuccess

=== 3ae === //You all know what I have done to realize this rebellion. *Point at Clay* But I know nothing of this one's struggles. What claim do you have to Tabor's life?

= starting_dialogue_section
~currentKnot = ->3ae.choice_section

    changeCamTarget({clayIndex})

    \*Clay smirks* If you weren't so new, you'd know my struggles like everyone else. I made it my mission to show the others that they could keep hope alive, by rebelling whenever I got the chance. I became something of a pet project of Tabor's. I've been on the flogging block in front of the whole camp more than anyone else! My back has the web of scars to prove it!
    
    ->currentKnot
= choice_section

    +Your mission was a noble one, and I'm sorry that you were put through all of that. But I am forced to wonder: do you wish to see justice be done, or do you wish to finally win the game going between you and Tabor?
            ->3aea
    +I agree that you have earned the right to revenge. But the best revenge against an enemy is to live well. If you kill Tabor, he won't live to see you prosper despite all his efforts.
            ->3aeb

=== 3aea === //win the game

= starting_dialogue_section
~currentKnot = ->3aea.choice_section

    changeCamTarget({clayIndex})

    This was no game! My back is not a score tally! That man flogs for sport, but I did not suffer his attentions for the fun of it! Every scar I carry, every lash I suffered, was my way of showing the others how powerless the guards were to crush my spirit!
    
    ->currentKnot
= choice_section

    +If that is the case, then let this go. You did your part: the spark of hope was kept alive until it could ignite the riot that overthrew the Director. Now reap your reward, and leave this vendetta in the past.
        changeCamTarget({clayIndex})
    
        \*Clay shakes with fury. He punches the fence in front him hard enough to crack it, but the effort seems to relieve some of the tension built up in his frame.*
            ->returnTo3aaSuccess

=== 3aeb === //I agree that you have earned the right to revenge. But the best revenge against an enemy is to live well. If you kill Tabor, he won't live to see you prosper despite all his efforts.

= starting_dialogue_section
~currentKnot = ->3aeb.choice_section

    changeCamTarget({clayIndex})

    Thats just a cliché. The real best revenge is to impart back the same pain your enemy did, twice over. And to get to look them in the eyes while you do it. 
    
    ->currentKnot
= choice_section

    +You appear to be quite familiar with all the facets of revenge. It makes your claim to take Tabor's beatings for the good of the slaves seem rather doubtful.
            ->3aeba
    +So the best revenge is just revenge? Where's the moral in that?
        changeCamTarget({clayIndex})
    
        I don't care about any moral! He hurt me, and I finally get to hurt him back! That's enough for me!
            ->returnTo3aaFailure
    +'The same pain', you say? You'll forgive me if I hold no trust for someone who longs to hold the whip as the masters did.
        changeCamTarget({clayIndex})
    
        No, you misunderstand! I don't- *Clay turns around to see many pairs of eyes glaring at him from the crowd.*
            ->returnTo3aaSuccess
    
=== 3aeba === //I agree that you have earned the right to revenge. But the best revenge against an enemy is to live well. If you kill Tabor, he won't live to see you prosper despite all his efforts.

= starting_dialogue_section
~currentKnot = ->3aeba.choice_section

    changeCamTarget({clayIndex})

    Slander! I've done my part for the rebellion, and now I merely want to do my part administering the guards' just desserts.
    
    ->currentKnot
= choice_section

    +I don't remember you arguing this loud to do your part after we had freed your section of the camp. Didn't you say I was trying to sell you something?
        changeCamTarget({clayIndex})
    
        \*Clays flashes a sheepish expression.* Well, uh, I did say that. But I was just trying to make sure you knew what you were doing!
            ->returnTo3aaSuccess


=== 4a === //finished conversation with clay, succeeded in convincing crowd

setToTrue(clayWillSeekOutTabor)

changeCamTarget({clayIndex})

I won't let this breach of justice come to pass. If you're going to take the side of one of the masters over your own people, then-

changeCamTarget({crowdIndex})

\*The crowd begins to hiss and boo at Clay. The slaves closest to Clay shove him about, stiffling any further response he could muster*

    +Thank you. I understand this is unorthodox, but your trust in me is not misplaced. I will make sure Tabor is held to account for what he has done. Killing him would be a needless waste, but letting him live, despite everything you've suffered, is a powerful example of the decency you never lost.
        ->4b

=== 4b === 

changeCamTarget({crowdIndex})

\*Unable to bring themselves to cheer for your decision to let Tabor live, the former slaves simply nod along to your words soberly.*

changeCamTarget({clayIndex})

\*Clay looks from you to Tabor, then back to you. Your eyes meet, and he matches your gaze with a glare. He then turns and makes his way back into the crowd.*

    +\*Turn towards Tabor.* Your life is mine now. I've kept my word to you.
    
        fadeToBlack(true, false)
        deactivate({clayIndex})
        fadeBackIn(90)
        ->4c
        

=== 4ba === //coming from cha 4 check to prevent a fight with the crowd.
            // Crowd celebrates and disperses.

    changeCamTarget({taborIndex})
    
    You have faced down your people to save my life. I... I don't understand. Why? Why save the likes of me?

->4ca


=== 4c === 

changeCamTarget({taborIndex})

I recognize that. But... I don't understand. Why? Why face down your own people to save the likes of me?
    ->4ca

=== fromFightDialogueKnot1 === //\*Wipe the blood from your weapon.* Still think this is a ruse?

Not anymore. I finally see that you meant everything you said. However, while you are no trickster, how you can believe what you've said still puzzles me.

    ->4ca

=== fromFightDialogueKnot2 === //I actually thought they were bluffing.

Perhaps they thought the same of you. None are more shocked than I that you weren't, of course. What would cause someone to fight their allies to save an enemy? 

    ->4ca


=== fromFightDialogueKnot3 === //I tried to prevent this, but they were too stubborn.

I heard your words; I believe you. What I can't believe is that you would actually fight your comrades to save me.

    ->4ca

=== fromFightDialogueKnot4 === //What they would have done to you... No one deserves that.

    Your perspective is rare. Don't mistake my meaning: none are happier than I that you believe that. I just can't understand why you would.

    ->4ca

=== 4ca ===

{
-fromFightDialogue():
    +When we encountered you in the Manse, I witnessed a man who stood ready to die between the innocent and what he thought were monsters. Like I said then, I want to teach that man that we aren't the monsters he thought we were.
    
        \*Tabor looks down at the ground. He seems unwilling to meet your eyes.* I don't know what to say... but I owe you my life. What is to become of me now?
            ->4d
        
    +I hold no love for you, Tabor. But the struggle between our peoples is too dire to let run to it's conclusion. If you and I can come to an understanding, then there may be hope for our peoples too.
    
        I can grasp some of that. The hope for a better tomorrow can be an intoxicating thing. It has yet to be seen if your plan has any merit beyond it's ideals, however. In the mean time, what is to become of me?
            ->4d
-else:

    +When we encountered you in the Manse, I witnessed a man who stood ready to die between the innocent and what he thought were monsters. Like I said then, I want to teach that man that we aren't the monsters he thought we were.
    
        \*Tabor looks down at the ground. He seems unwilling to meet your eyes.* I don't know what to say... but I owe you my life. What is to become of me now?
            ->4d
        
    +I hold no love for you, Tabor. But the struggle between our peoples is too dire to let run to it's conclusion. If you and I can come to an understanding, then there may be hope for our peoples too.
    
        I can grasp some of that. The hope for a better tomorrow can be an intoxicating thing. It has yet to be seen if your plan has any merit beyond it's ideals, however. In the mean time, what is to become of me?
            ->4d
        
    +I'm not entirely sure myself. But it feels good to stop the killing, even momentarily. That's enough for me.
    
        Well, when you learn your own motives, please see fit to inform me. In the mean time, what is to become of me?
            ->4d

}

=== 4d ===

+You will be treated like any of the other prisoners for now. When we are ready to leave camp, you will venture forth with me. I think it's best I keep a close eye on you, for your safety.
    ->4e

=== 4e ===

finishQuest(An Uneasy Truce, true, 7)

setToTrue(didNotExecuteTabor)

\*Tabor nods, but says nothing. He appears lost in thought.*

fadeToBlack()

deactivate({taborIndex})

fadeBackIn(60)

->Close

=== 5a === //finished conversation with clay, failed to convince crowd

changeCamTarget({crowdIndex})

\*The crowd is too incensed to heed your words.*

changeCamTarget({clayIndex})

I won't let this breach of justice come to pass. If you're going to take the side of one of the masters over your own people, then we will treat you as one of them! 

    +It saddens me that you are so committed to violence, but if you try to lay a hand on Tabor, I will fight for what I think is right. <Combat>
        ->5b
    +I see no way forward that doesn't end in conflict. I'm sorry Tabor, but your execution can't be put off any longer. *Execute Tabor.*
        setToTrue(crowdForcedTaborExecution)
        ->6b

=== 5b ===
finishQuest(An Uneasy Truce, true, 11, true)

changeCamTarget({crowdIndex})

\*The crowd follows Clay's lead and begins to swarm over the fence.*

enterCombat(0, {taborAfterClayFightKey})
->Close

=== 6a === //Executing Tabor

changeCamTarget({crowdIndex})

\*The crowd lets out a mixture of cheers and boos. It seems some of the audience had hoped to take part in delivering Tabor's sentence.*

changeCamTarget({taborIndex})

{
- acceptedTaborsSurrenderAfterDirectorFight:

    I should have known your words held no merit. Little difference it makes in the end, though. May your life be short and full of hardship, branded.
- else:
    It beats getting stomped to death, I suppose. Get on with it then.
}

    ->6c(->Close)

=== 6b ===

changeCamTarget({crowdIndex})

\*The crowd's anger turns to cheers as you face Tabor.*

changeCamTarget({taborIndex})

You put up more of a fight than I thought you would. It didn't matter in the end, though. Thank you... for your effort, and for the few extra moments of breath.

    ->6c(->6ca)

=== 6c(->divert) ===

finishQuest(An Uneasy Truce, true, 8)

setToTrue(executedTabor)

fadeToBlack(true, false)

kill({taborIndex})

fadeBackIn(60)

->divert

=== 6ca===

changeCamTarget({crowdIndex}))

\*With Tabor's head separated from his shoulders, the crowd gradually calms from their jubilation and begins to disperse.*

fadeToBlack()

setToTrue(crowdDispersed)

deactivate({clayIndex})
deactivate({crowdIndex})

fadeBackIn(60)

->Close

=== 7a === //finished conversation with clay, failed to convince crowd

changeCamTarget({crowdIndex})

\*The crowd begins to cheer and sing. Those that looted the whips of dead guards begin to pass them about.*

changeCamTarget({taborIndex})

{
- acceptedTaborsSurrenderAfterDirectorFight:

    I should have known your words held no merit. Whipped to death? Your cruelty cannot be understated, branded. If the Gods are good they will find an equally brutal fate for the likes of you.
- else:
    Whipped to death? Your cruelty cannot be understated, branded. If the Gods are good they will find an equally brutal fate for the likes of you.
}

finishQuest(An Uneasy Truce, true, 9)

setToTrue(gaveTaborToTheCrowd)

fadeToBlack()

kill({taborIndex})

fadeBackIn(60)

->Close

=== 9a ===

{
    - not attitudeModFromAskingQuestions:
    ~attitude += 1
    ~attitudeModFromAskingQuestions = true
}


\*Tabor thinks for a moment, then relents* Bah, if it does not betray the memory of the Director and will get you to quicken my passing, I will answer. Ask your questions, slave.
    
    /*
    +What was the purpose of this camp? The guards never told us what our work was for.
        ->9b
    */
    +Why did the Director choose you to be his Chief Correctional Officer?
        ->9c
    +Do you have any regrets for what you've done?
        ->9d
    +Will any of the freed vouch for your life?
        ->9e
    +My questions are finished. I have come to a verdict.
        ->2a

=== 9b ===

I was the Chief Correctional Officer. My duty here was much the same as any other camp. That information was given only to the officers who needed to know it.
    
    +But the Director ordered you to guard his children. He trusted you. He never told you anything?
        ->9ba
    
    +Blast. Fine, another question then.
    
        keepDialogue()
        
        Ask.
        
        ->9a

=== 9ba ===

We only had the briefest of exchanges: a rudimentary understanding so I understood if any of the slaves had gleaned some information they should not know.

    +Ah, so you do know something. Tell me, anything will help.
        ->9bb
        
        
=== 9bb ===

No. I said I would not betray the Director. You will not have it.

{
-strength >= 4:
    +You don't seem to understand the position you're in. I'm asking you nicely now. The next time, I'll be asking whats left of you. <Str {strength}/4>
        {
        -not threatenedTaborForInformation:
            ~attitude -= 2
            ~threatenedTaborForInformation = true
        }
        ->9bc
-strength < 4:
    +I could lay into you for a while. We'll see if you're still loyal after that. <Str {strength}/4>
        {
        -not threatenedTaborForInformation:
            ~attitude -= 2
            ~threatenedTaborForInformation = true
        }

        
        keepDialogue()
        
        Your threats are hollow. You won't find my loyalties so easily discarded.
                
        ->9a
}

{
-keptDirectorAlive:
    +We kept the Director alive, we're going to hand him over to the Masons. Any information you give us may save him some suffering at their hands.
        ->9bc
}

    +I won't ask you to dishonor him. Another question then.
    
        keepDialogue()
        
        Ask.
        
        ->9a

=== 9bc ===

\*Tabor closes his eyes and thinks for a moment* I only know that they think there is a passage beneath the mountain. For what purpose, I do not know. The Delver's built that place a long time ago. It's got something in it that the Director's benefactors wanted, back in the Confederation.

    +Something valuable? And do they want to have it, or do they want the Masons not to have it?
    
        keepDialogue()
        
        Like I said, the Director never told me what they wanted. I'm not sure even the Director knows everything about it, but if he does he wouldn't tell me. 
        ->9bc
    +You mentioned benefactors. Who are they? A count? Wealthy merchants?
    
        keepDialogue()
        
        I never met them. There's a bunch of them, and from what the Director let slip at least a few of them had noble titles. Could have been a count, or part of a count's family. They were always hounding the Director for results, and he complained once or twice that they asked too much while providing him with too little.
            ->9bc
            
    +Who are the Delvers?
    
        keepDialogue()
        
        They're another of the Folks, like the Craft or Riding Folk. They are miners and architects, and have a lot in common with the Craft Folk, but they live most of their lives underground. We think they had some settlement or project they built out here a long time ago, but abandoned it at some point.
            ->9bc
    
    +That's everything, for now. I have some other questions.
        keepDialogue()
        
        Ask.
        
        ->9a

=== 9c ===

The Director had a keen eye for earnest talent. A man must not simply know his chosen profession to excel at it: he must believe in it. Of all my comrades, I was the one who believed most strongly in the branding as a instrument for redemption.

    +Redemption? But you work us to death. Where's the redemption in that?
        {
        -not attitudeModDrivelRedemptionChoice:
            ~attitude += 1
            ~attitudeModDrivelRedemptionChoice = true
        }
        ->9ca
        
    {
    -heardTaborsLesson:
    
        +I've heard this drivel before. No more.
        
            {
            -not attitudeModDrivelRedemptionChoice:
                ~attitude -= 1
                ~attitudeModDrivelRedemptionChoice = true
            }
            
            keepDialogue()
            
            \*Tabor shrugs* If you care not for my answer, then why waste my time with the question?
            
            ->9a
    } 
        
=== 9ca ===

When each of us die, we must make amends with those we've wronged before the Gods will let us pass from the afterlife into our next lives on Föld. My duty as a Correctional Officer was to teach you to revere all life, including the horses that your people despised. Even condemned as you are, it is pragmatic to try to teach you the error of your ways so that your soul won't harass your fellows in the afterlife.

Although, in truth, I have always believed my work to be for the benefit of the branded as much as it is for the Confederation. Should the student accept the lessons I teach, their time as a branded would have been filled with less punishment, and more soul searching.

    +I suppose there is a strange nobility to that. A mote of morality in an ocean of evil, but still visible.
        
        {
        -not attitudeModRedemptionAnswerReply:
            ~attitude += 1
            ~attitudeModRedemptionAnswerReply = true
        }
        
        keepDialogue()
        
        \*Tabor flashes the weakest of smirks, but does not reply*

        ->9a
        
    +Yes, you're a real bleeding-heart. Now, I have more questions.
        
        {
        -not attitudeModRedemptionAnswerReply:
            ~attitude -= 1
            ~attitudeModRedemptionAnswerReply = true
        }
        
        keepDialogue()
        
        Ask them then.

        ->9a

=== 9d === //Do you have any regrets for what you've done?

Regrets? *Tabor's eyes study your face* What a strange sensation it is, to finally be called to answer that question, when I have asked it of others so many times.

Of course I have regrets. Had I succeeded in teaching you the futility of your sinful ways... had I been better, had I not been complacent, I could have prevented your rioting. You would not be loose to hurt the subjects of the Confederation. You would not still be lost, unable to see yourselves for the malevolent actors that you are. Had I done my duty, my comrades... might still be alive.

    +You misunderstand. I wish to know if you regretted your actions against the branded. The tortures you put us through.
        ->9da

=== 9da === //Do you have any regrets for what you've done?

My lessons used pain to keep a slaves attentions. To show the consequences of their actions in a way they could not ignore. They were necessary parts of my role in this camp. I hold no regrets regarding my conduct in fulfilling my obligations to the Director.

    +Your warped sense of morality told you such measures were necessary. But you can't even bring yourself to regret the 'necessity' of your actions? You never wished there was another way?
        ->9db
        
=== 9db === 

Every day. But my work with the branded has consistently shown there was no other choice. Without the pain, the branded never allow the lessons to keep in their minds. They simply go back to their cells without considering the magnitude of our discussions.

    +Then in a future where you returned to the Confederation, you would continue to antagonize the branded? 
    
        keepDialogue()
        
        I would continue my work with a renewed vigor. Whatever my failings now, I would learn from them and keep striving to perfect my abilities as a teacher.
        
            -> 9a

=== 9e === 

keepDialogue()

\*Tabor lets out a {not acceptedTaborsSurrenderAfterDirectorFight:pained }chuckle* Wait, you're serious? No, none of the branded come to mind.

    ->9a

=== returnTo3aaFailure ===

    ~currentKnot = ->3aa.choice_section
    ->moveToCrowdStatus(crowdStatus)

=== returnTo3aaSuccess ===

    ~currentKnot = ->3aa.choice_section
    ->moveToCrowdStatus(crowdStatus-1)

=== moveToCrowdStatus(newCrowdStatus) ===

~crowdStatusDescription = ""

{
-crowdStatus < newCrowdStatus:
    {
    
    -crowdStatus >= rageThreshold:
        ~crowdStatusDescription += crowdMoreEnragedPrefix
    -else:
        ~crowdStatusDescription += crowdIncreasedPrefix
    }

-crowdStatus == newCrowdStatus:

~crowdStatusDescription += crowdStillPrefix

-newCrowdStatus <= calmedThreshold:

~crowdStatusDescription += crowdCalmedPrefix

-crowdStatus > newCrowdStatus:

~crowdStatusDescription += crowdDecreasedPrefix
}


{
- newCrowdStatus >= rageThreshold:

~crowdStatusDescription += rageName + crowdWontAcceptSuffix

- newCrowdStatus == angryThreshold:

~crowdStatusDescription += angryName + crowdWontAcceptSuffix

- newCrowdStatus == agitatedThreshold:

~crowdStatusDescription += agitatedName + crowdWontAcceptSuffix

- newCrowdStatus <= calmedThreshold and crowdStatus <= calmedThreshold:

~crowdStatusDescription += calmName + crowdWillAcceptSuffix

- newCrowdStatus <= calmedThreshold:

~crowdStatusDescription += calmedName + crowdWillAcceptSuffix

}

~crowdStatus = newCrowdStatus

changeCamTarget({crowdIndex})

{crowdStatusDescription}

->currentKnot

=== function fromFightDialogue() ===

~return (fromFightDialogueKnot1 or fromFightDialogueKnot2 or fromFightDialogueKnot3 or fromFightDialogueKnot4)

=== function taborIsNext() ===

~return not (marcosNeedsHandling or andrasNeedsHandling)

=== Close ===

close()

->DONE

/*

=== 2c ===

changeCamTarget({crowdIndex})

\*A sense of shock ripples through the crowd. Shouts and boos erupt as they let their ire be known*

changeCamTarget({clayIndex})

You have no right! You're too new to understand what we've been through! Kill him! Or let us!

changeCamTarget({crowdIndex})

\*The crowd cheers its assent*

    +I had no choice! What he knows is too valuable to let die with him! 
    
        changeCamTarget({crowdIndex})

        \*The crowd seems split. They bicker among themselves as much as they shout back rebuttals*
        ->2ca
    +What would you have had me do? Turn their assistance away? Left you to rot in chains?
    
        changeCamTarget({crowdIndex})

        \*The crowd seems split. They bicker among themselves as much as they shout back rebuttals*
        ->2d
    +Do you think the Masons inept in the ways of torture? His days will not be pleasant, I guarantee you that!
    
        changeCamTarget({crowdIndex})

        \*The crowd seems split. They bicker among themselves as much as they shout back rebuttals*
        ->2e

=== 2ca ===

changeCamTarget({clayIndex})

I don't care what he knows! I care about what he's done! Done to me!
    
    +You let your hate blind you to the pragmatic path! The Masons need his knowledge to fight the entire Confederation!
        ->2cb
    +You weigh yourself more important than countless slaves?
        ->2cc
        
=== 2cb ===

changeCamTarget({clayIndex})

If you had been here to suffer his lessons you would share my feelings!
    
    +\*Address the crowd:* Does this one man speak for all of you? Would you cripple your only allies to sate your grudges?
        ->2z
    +I will always make the choice that hurts the Lovashi the most. If your feelings were as potent as mine, you would choose that too.
        ->2cd

=== 2cc ===

changeCamTarget({clayIndex})

No! I only wish to see justice done!
    
    +\*Address the crowd:* Does this one man speak for all of you? Would you cripple your only allies to sate your grudges?
        ->2z
    +Justice will be done. Just not by you. Let the Masons handle it.
        ->2cd

=== 2cd ===

changeCamTarget({clayIndex})

\*Clay looks from you to Tabor, then back to you. His eyes smolder darkly. He then turns and makes his way back into the crowd*
    
    +\*Address the crowd:* Tabor will be under my protection until we are safely behind the Masons' walls. After that, their interrogators will make sure he gets the treatment he deserves!
        ->2za

=== 2d ===

changeCamTarget({clayIndex})

\*Clay looks back at the crowd uncertainly*

    +This was their price. Had I not paid it, you would still be slaves!
        ->2da

=== 2da ===

changeCamTarget({clayIndex})

Will they at least make him suffer? The bastard deserves that much.

    +The Masons hold no love for the Lovashi. He won't be comfortable where he's going.
    
        ->2db
        
=== 2db ===

changeCamTarget({clayIndex})

Very well. I shall have to live with that.

    +\*Address the crowd:* Tabor will be under my protection until we are safely behind the Masons' walls. After that, their interrogators will make sure he gets the treatment he deserves!
        ->2za
        
=== 2e ===

changeCamTarget({clayIndex})

I know nothing about these Masons. What could they 

    +\*Address the crowd:* Tabor will be under my protection until we are safely behind the Masons' walls. After that, their interrogators will make sure he gets the treatment he deserves!
        ->2za

=== 2z ===

changeCamTarget({crowdIndex})

\*The crowd begins to hiss and boo at Clay. The slaves closest to Clay shove him about, stiffling any response he could muster*
    
    +\*Address the crowd:* Tabor will be under my protection until we are safely behind the Masons' walls. After that, their interrogators will make sure he gets the treatment he deserves!
        ->2za

=== 2za ===

changeCamTarget({crowdIndex})

\*The crowd lets out a cheer*
    
    +\*Turn back to Tabor* Tabor will be under my watch and protection until we are safely behind the Masons' walls. Once there, he will be handed over. Anyone who tries to end his life will join him in the Masons' dungeons!
        ->Close
        */