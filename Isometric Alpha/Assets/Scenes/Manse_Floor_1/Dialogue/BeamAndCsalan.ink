VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR metBeam = false
VAR askedBeamAboutWhittling = false
VAR pissedOffBeam = false
VAR askedAboutMangledName = false
VAR givenHorsetongueGuideFromBeam = false

VAR gotThePlanFromKastor = false
VAR spokeToBalint = false
VAR spokeToJanos = false
VAR spokeToErvin = false
VAR spokeToGarchaAboutPlan = false
VAR metKastor = false

VAR foughtHorsesInManse = false
VAR askedAboutSurrender = false

VAR beamIndex = 1
VAR csalanIndex = 2

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

/*
changeCamTarget({csalanIndex})

<>

changeCamTarget({beamIndex})
*/

->1a

=== 1a ===

changeCamTarget({beamIndex})

Hail, branded. I have been beseeched by the glorious Csalan, steed to the Director, to serve as his translator while he parlays with you.

    +I will hear his words.
        ->csalanSpeaksAndBeamTranslates("\*The horse speaks aloud in the horsetongue.* <Excellent. I had doubts you would be so agreeable.>", "Csalan appreciates your willingness to speak.", ->1b)
    +Parlay? With a horse? You must be joking.
        ->csalanSpeaksAndBeamTranslates("\*The horse snorts, then speaks aloud in the horsetongue.* <I am not one to joke. This regards a matter most dire.>", "Csalan says he is not joking. He needs to speak to you, earnestly.", ->1b)
    +The revolution won't be delayed. Not by you, or your horse. Attack! <Combat>
        ->Combat

=== 1b ===

    +You don't need to translate my words back to him?
        Horses can understand our language, but Beast did not see fit to allow their throats to speak it. He does not need me to translate what you say.
        ->1b
    +What did he want to discuss?
        ->csalanSpeaksAndBeamTranslates("<I am aware of the progress you've made through the camp. That you should penetrate this far into the Manse... does not bode well for our prospects of victory.>","Csalan says that the victories you have won thus far against the guards are known to him. He sees you as a powerful and worthy opponent.",->1c)

=== 1ba ===

    +You don't need to translate my words back to him?
        Horses can understand our language, but Beast did not see fit to allow their throats to speak it. He does not need me to translate what you say.
        ->1ba
    +What did he want to discuss?
        ->csalanSpeaksAndBeamTranslates("<I am aware of the progress you've made through the camp. That you should penetrate this far into the Manse... does not bode well for our prospects of victory.>","Csalan says that the victories you have won thus far against the guards are known to him. He sees you as a powerful and worthy opponent.",->1c)


=== 1c ===

->csalanSpeaksAndBeamTranslates("<Every horse has heard the stories of the fates of those steeds that fall in battle against the Craft Folk. I wish to gauge your character, and determine what shall befall my form if you were to defeat me here.>","The horses of the Confederation know that it is common for Craft Folk to feast on the flesh of the horses they slay in battle. Csalan wishes to learn if such a fate awaits him and the horses under his charge.",->1ca)

=== 1ca ===

    {
    -askedAboutSurrender:
        +Why does this worry him?
            ->csalanSpeaksAndBeamTranslates("<It is not dignified for a horse to allow themselves to be eaten like chattle.>","Csalan says the indignity of it concerns them. He does not wish such a fate for himself or his subordinates.",->1ca)
        +I have made my decision
            ->1d
        +If my decision makes no difference, then what is the point in making it? <Combat>
            ->Combat
    -else:
        +Perhaps we don't need to fight at all. We will gladly accept your surrender.
            ~askedAboutSurrender = true
            ->csalanSpeaksAndBeamTranslates("\*If horses can scoff, then this one did.* <We are resolved to fight to the death, no matter your answer.>","Csalan says no horse would allow themselves to be captured. The horses will fight you, whatever your decision is.",->1ca)
        +Why does this worry him?
            ->csalanSpeaksAndBeamTranslates("<It is not dignified for a horse to allow themselves to be eaten like chattle.>","Csalan says the indignity of it concerns them. He does not wish such a fate for himself or his subordinates.",->1ca)
        +I have made my decision
            ->1d
    }

=== 1d ===

changeCamTarget({beamIndex})

What have you decided?

    +You have misjudged us: that will not happen. I will see to it. 
        ->csalanSpeaksAndBeamTranslates("<I would hear you swear this to me. Swear on whatever you hold most dear that you will burn our bodies to ash rather than eat from them.>", "Csalan wants to hear an oath from you declaring you will allow fire to consume their bodies rather than consume them yourselves. Swear it, lest whatever you hold most dear be forfeit to the Gods.", ->1e)
    +I have no intention of allowing a being I have spoken with be eaten. 
        ->csalanSpeaksAndBeamTranslates("<I would hear you swear this to me. Swear on whatever you hold most dear that you will burn our bodies to ash rather than eat from them.>", "Csalan wants to hear an oath from you declaring you will allow fire to consume their bodies rather than consume them yourselves. Swear it, lest whatever you hold most dear be forfeit to the Gods.", ->1e)
    +I won't partake, but my fellows are starving. I will not order them to go without the meat your bodies would provide.
        ->csalanSpeaksAndBeamTranslates("<You disgust me branded. Never have I been more sure that our cause is righteous.>", "Csalan says that you disgust him, and your words only serve to prove him superior to you.", ->Combat)
    +And let a delicacy like him go to waste? I won't let that happen.
        ->csalanSpeaksAndBeamTranslates("<You disgust me branded. Never have I been more sure that our cause is righteous.>", "Csalan says that you disgust him, and your words only serve to prove him superior to you.", ->Combat)
    +I will do what I like, and you'll be too dead to stop me. <Combat>
        ->Combat

=== 1e ===

    +I swear it before the Gods, lest...
        ->1f
    +I owe you no oaths. Let us get this over with. <Combat>
        ->Combat

=== 1f ===

setToTrue(sworeToBurnCsalansBody)

changeCamTarget({csalanIndex})

\*The horse listens intently to your oath.*

    +... my life be forfeit. //total health minus
        setToTrue(csalanLifeOathMade)
        ->1g
    +... I suffer for the rest of my days. // increase to incoming damage
        setToTrue(csalanSufferingOathMade)
        ->1g
    +... my name be forever stained. //synergy minus
        setToTrue(csalanStainedNameOathMade)
        ->1g
    +... my family dissolve our bonds of kinship. //
        setToTrue(csalanFamilyOathMade)
        ->1g
    +... I never return to the land I call home. //minus to retreat chance
        setToTrue(csalanHomeOathMade)
        ->1g
    +... I never know wealth. //percentage minus to gold gain
        setToTrue(csalanWealthOathMade)
        ->1g
    +... my sanity wither from the shame. //mental resist minus
        setToTrue(csalanSanityOathMade)
        ->1g

=== 1g ===

    ->csalanSpeaksAndBeamTranslates("\*Csalan bows his head low, bending his front legs to add to it's depth.* <I thank you, branded. Now, battle can be postponed no longer. Show me the strength that has felled so many of my guards.>", "The great Csalan thanks you. He now invites you to battle. He wishes to test the strength that you wield.", ->1h)

=== 1h ===

    +I am curious to fight him as well. We are ready. <Combat>
        ->Combat
    +Finally! Let it be done! <Combat>
        ->Combat
    +\*Grin wickedly.* Good thing I had my fingers crossed. <Combat>
        ->Combat

=== csalanSpeaksAndBeamTranslates(csalanDialogueLine, beamDialogueLine, ->divert) ===

changeCamTarget({csalanIndex})

{csalanDialogueLine}

changeCamTarget({beamIndex})

{beamDialogueLine}

->divert

=== Combat ===

setToTrue(foughtHorsesInManse)

enterCombat(0)

->Close

=== Close ===

close()

->DONE