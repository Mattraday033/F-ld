VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR givenTutorialQuest = false
VAR foundSlate = false
VAR thatchRemovedTutorialRubble = false
VAR toldThatchAboutSlate = false
VAR toldKastorOfThatchsFate = false

VAR metKastor = false
VAR gotBroglinKilledByGuard = false
VAR spokeToGarchaAboutPlan = false
VAR askedKastorWhoHeIs = false
VAR gaveKastorYourName = false
VAR knowRevolutionPassword = false
VAR askedKastorToHelpEscape = false
VAR knowsAboutTheMine = false
VAR gaveKastorThePassword = false
VAR askedAboutGuardMarcos = false
VAR saidIKnowWhatImDoing = false
VAR gotThePlanFromKastor = false
VAR toldToAidJanos = false
VAR finishedBalintsTask = false
VAR finishedErvinsTask = false
VAR obtainedMineArmoryKey = false
VAR toldKastorFinishedErvinsTask = false
VAR givenTaskByErvin = false
VAR imreWontSpeakToPlayer = false
VAR terrifiedImre = false
VAR convincedImre = false
VAR toldKastorFinishedBalintsTask = false
VAR toldKastorObtainedMineArmoryKey = false
VAR refusedToWorkWithJanos = false
VAR gotKeyFromJanos = false
VAR saidJanosIsATraitor = false
VAR kastorExplainedWhereToFindAnotherKey = false
VAR gotArmoryKeyFromGuardHouse = false
VAR killedChiefIren = false
VAR mineLvl2BatBossKilled = false
VAR mentionedBadReasonForGoingInsideMine = false
VAR learnedAboutMuzsasSweetToothFromKastor = false
VAR discussingNandor = false
VAR toldToFindNandor = false
VAR toldKastorErvinIsDead = false
VAR toldKastorBalintIsDead = false
VAR toldKastorJanosIsDead = false
VAR toldToFindTools = false
VAR hasToolBundle = false
VAR gaveKastorToolBundle = false
VAR mineLvl3CarterAndNandorInParty = false
VAR mineLvl3KilledGuards = false
VAR mineLvl3DealtWithGaspar = false
VAR kastorReadyToStartRevolt = false
VAR mineLvl3MarcosAgreedToIgniteJelly = false
VAR broughtNandorToKastor = false
VAR kastorStartedRevolt = false
VAR toldCarterPassword = false
VAR toldCarterWrongPassword = false
VAR learnedCartersIdentity = false
VAR learnedPagesIdentity = false
VAR toldDirectorIsAWarHero = false

VAR explainingPlan = false
VAR backTo6cza = false
VAR backTo3c = false
VAR aboutToStartRevolt = false

VAR restAfterDeactivatingExtras = false

VAR deathFlagBálint = false
VAR deathFlagImre = false
VAR deathFlagErvin = false
VAR deathFlagAndrás = false
VAR deathFlagJanos = false
VAR deathFlagNándor = false
VAR deathFlagGuardMárcos = false

VAR formationFlagNándor = false
VAR formationFlagCarter = false

VAR kastorIndex = 1
VAR nandorIndex = 2
VAR carterIndex = 3
VAR marcosIndex = 4

VAR itemTutorialKey = "Item Tutorial"

VAR southEasternCampSceneName = "SECamp"
VAR manseKitchensSceneName = "Manse-1F-Kitchens"
VAR manseStockroomSceneName = "Manse-2F-Stockroom"

VAR choseStrengthAtStart = false
VAR choseDexterityAtStart = false
VAR choseWisdomAtStart = false
VAR choseCharismaAtStart = false

VAR weaponListIndex = 1
VAR cudgelIndex = 0
VAR shivIndex = 1
VAR plankIndex = 18
VAR sharpRockIndex = 19

VAR playerName = ""

searchInventoryFor(obtainedMineArmoryKey,Key,0)
searchInventoryFor(hasToolBundle,Tool Bundle)

{
-kastorStartedRevolt:
    ->8a
-kastorReadyToStartRevolt:
    ->7ab
-not metKastor and mineLvl3CarterAndNandorInParty and not broughtNandorToKastor and not gotThePlanFromKastor:
    ->6c
-gotThePlanFromKastor:
    {
    -mineLvl3CarterAndNandorInParty and playerFinishedErvinsTask() and not toldKastorFinishedErvinsTask:
        ->3a
    -(toldToFindTools and hasToolBundle) or mineLvl3CarterAndNandorInParty or gaveKastorToolBundle or deathFlagNándor:
        ->6a
    }
    ->3a
-givenTutorialQuest and not toldKastorOfThatchsFate:
    ->T1d
-gaveKastorThePassword and not givenTutorialQuest and not toldKastorOfThatchsFate:
    You're back. What do you need? 
    ->2aa
-else:
    ~metKastor = true
    setToTrue(metKastor)
    ->1a
}

=== 1a ===
{
 -  gaveKastorThePassword:
    You're back. What do you need? 
    ->2aa
 -  gaveKastorYourName:
    keepDialogue()
    You're back. What do you need? 
    ->1c
 -  not gaveKastorYourName:
    Who are you? How are you able to walk around during the lockdown?
    ->1aa
}

=== 1aa ===

    {not askedKastorWhoHeIs:
    *Are you Kastor?
        ->1b
    } 
    +I'm {playerName}. Broglin helped me gain the trust of the guards.
        ~gaveKastorYourName = true
        setToTrue(gaveKastorYourName)
        ->1c
    +Who I am doesn't matter. Garcha sent me.
        ->1c

=== 1b ===

~askedKastorWhoHeIs = true

setToTrue(askedKastorWhoHeIs)

I am. But I ask again, who are you?

->1aa

=== 1c ===

{gaveKastorYourName:Broglin you say? That is very interesting...|Garcha? What does he want from me?}

//    {
//        -not askedKastorToHelpEscape:
//        *I'm planning to escape. Want in?
//        ->1d
//    }
    {
        -knowRevolutionPassword and not gaveKastorThePassword:
        *Which way is the wind blowing?
        ->2a
    }
    
    +Why is your hut so much bigger than Broglin and Garcha's?
        ->keepDialogueB4HutSizeExplanation(->1c)
    +Do you know what happened to the mine?
        ->mineExplanationDisclaimer(true,->1c)
    +I have to leave now.
        ->Close

=== keepDialogueB4HutSizeExplanation(->divert) ===

keepDialogue()

->hutSizeExplanation(divert)

=== hutSizeExplanation(->divert) ===

This hut houses the miners that are forced to work the lowest levels of the mines. It was built like this since it was easier to corral us if we were all in one place.

->divert


/*
{

 -  gotThePlanFromKastor:
    keepDialogue()

    This hut houses the miners that are forced to work the lowest levels of the mines. It was built like this since it was easier to corral us if we were all in one place.
    {
        -hasToolBundle:
        ->6a
        -else:
        ->3a
    }

 - 

 -  gaveKastorThePassword:
     This hut houses the miners that are forced to work the lowest levels of the mines. It was built like this since it was easier to corral us if we were all in one place.
    ->2aa
 -  not gaveKastorThePassword:
    keepDialogue()

    This hut houses the miners that are forced to work the lowest levels of the mines. It was built like this since it was easier to corral us if we were all in one place.
    ->1c
}*/
    
=== 1d ===
~askedKastorToHelpEscape = true

setToTrue(askedKastorToHelpEscape)

\*Kastors eyes bulge and a sharp intake of breath causes him to begin to cough* By the Gods, you're going to get me killed! Please leave now before the guards hear you.
->Close

=== mineExplanationDisclaimer(needsKeepDialogueLine,->divert) ===

Recounting it brings back hard memories, but I will do it. It is also a story not quickly told.

    +I would still hear it, if you would be willing to tell it.
        ->mineExplanation1(needsKeepDialogueLine,divert)
    +In that case maybe it would be best if I heard it another time.
        {
        -needsKeepDialogueLine:
            keepDialogue()
        }
        Very well.
            ->divert

=== mineExplanation1(needsKeepDialogueLine,->divert) ===

My team and I were tasked with excavating a new shaft, deep within the mine. I was close to the tunnel entrance so I did not see how it all started, but I heard another slave shout from farther in that he had hit a pocket.

    +A pocket?
        ->mineExplanation2(needsKeepDialogueLine,divert)

=== mineExplanation2(needsKeepDialogueLine,->divert)===

Yes, you see, when working beneath the ground sometimes dig teams can burrow into small caverns, or pockets, that can be filled with air. Or, if you're unlucky, these pockets can be filled with breathable poison. Or worse.

I continued my work removing stones from the tunnel when I heard more shouting from farther ahead. When I looked up, I saw some of my team fleeing back towards me. They were being chased by a pack of sickly white worms, each one the size of a hound!

    +Worms?
        ->mineExplanation3(needsKeepDialogueLine,divert)

=== mineExplanation3(needsKeepDialogueLine,->divert) ===

I don't know how else to describe them; I've never seen anything like them before. They had eyeless, round faces with rows of teeth that burrowed back deeper into their gullets, and long white bodies that held no legs.

They moved deceptively fast for beings born without feet. Had Guard Márcos not stepped between us, you and I would not be discussing this.

    +What happened next?
        ->mineExplanation4(needsKeepDialogueLine,divert)
    +What became of Márcos?
        ->mineExplanation3a(needsKeepDialogueLine,divert)

=== mineExplanation3a(needsKeepDialogueLine,->divert) ===

setToTrue(askedAboutGuardMarcos)

He slew the beasts, but not before one bit into his leg. Last I saw him, he was shouting for me to alert those on the levels above us while limping towards where the worms had come from.

    +A brave man.
        Indeed. There are few like him.
        ->mineExplanation4Pref(needsKeepDialogueLine,divert)
    +Sounds like one less guard to deal with.
        I suppose thats one way to look at it.
        ->mineExplanation4Pref(needsKeepDialogueLine,divert)
    +Would that they feasted on his guts. *Spits*
        If they had, I would have died with him. I can forgive much of those who save my life.
        ->mineExplanation4Pref(needsKeepDialogueLine,divert)

=== mineExplanation4Pref(needsKeepDialogueLine,->divert) ===

combineDialogue()

Anyways, that's the last I saw of him. 

    ->mineExplanation4(needsKeepDialogueLine,divert)

=== mineExplanation4(needsKeepDialogueLine,->divert) ===

I followed Márcos's instructions and fled with the other survivors. We reached the level above without interruption and explained what had happened to the overseer there. He rallied the rest of the guards and slaves and led us out of the mine.
    
    +And the lockdown came after?
        ->mineExplanation4a(needsKeepDialogueLine,divert)
    +You were very fortunate.
        setToTrue(knowsAboutTheMine)
        {
        -needsKeepDialogueLine:
            keepDialogue()
        }

        {
        -divert != ->4d:
            That I was. That I was...
        }

            ->divert


=== mineExplanation4a(needsKeepDialogueLine,->divert) ===

keepDialogue()

Correct. I suppose the lockdown was to prevent the news from spreading, and causing a panic. Or perhaps it was to prevent alerting the slaves to how undermanned the guards are at the moment.

    ->mineExplanation4(needsKeepDialogueLine,divert)

=== 1n ===

{
 -  gotThePlanFromKastor:
    
    keepDialogue()

    That I was. That I was...
    {
        -hasToolBundle:
        ->6a
        -else:
        ->3a
    }

 -  gaveKastorThePassword:
    That I was. That I was...
    ->2aa
 -  not gaveKastorThePassword:
    keepDialogue()

    That I was. That I was...
    ->1c
}


=== 2a === 

~gaveKastorThePassword = true

setToTrue(gaveKastorThePassword)

prepForItem()

\*Kastor smiles.* East, friend.

addXP(250)

->2aa

=== 2aa ===

    /*
    *I can move past the guards freely. What do you need of me?
        ->2f
    */

    {
    -not givenTutorialQuest and not toldKastorOfThatchsFate:
    *I can move past the guards freely. What do you need of me?
        ->T1a
    }
    +Broglin has been taken by the guards.
        ->2b
    +Why is your hut so much bigger than Broglin and Garcha's?
        ->hutSizeExplanation(->2aa)
    +Do you know what happened to the mine?
        ->mineExplanationDisclaimer(false,->2aa)
    +I have to leave now.
        ->Close
    
=== 2b ===

That is alarming news. I would not wish the pit on anyone.

    *What are we going to do about it?
        ->2e
    *I just thought you should know
        ->2e 

=== 2c ===

keepDialogue()

I have learned over my time as a slave not to get too attached to my fellow workers. These things happen enough to teach you that lesson quickly.
        ->1c


=== 2d ===

keepDialogue()

What else?

->1c


=== 2e ===

I appreciate you telling me this. Once we have dealt with the guards, we'll free all those that are stuck in the pit.

    +Do we know where it is?
        No, but I can make a guess that it's location lies somewhere under the Director's residence. What everyone in the camp calls 'the Manse'. For now we must focus on other, more pressing, matters.
        ->2aa

=== T1a ===

\*Kastor considers you for a moment.* You seem no less capable than anyone else, I suppose. But I was not present for how you impressed Broglin. You'll need to prove you can be trusted to handle yourself out there before I divulge any more of the plan to you.

    +I understand. What do I need to do?
        ->T1b

=== T1b ===

Before you arrived, I was considering who else among the slaves could be recruited to be our runner between the huts. There really is only one other candidate: a branded named Thatch. From my doorway, I have seen the guards assigning him to manual labor even during the lockdown; they'll be used to seeing him outside. And from my interactions with him, he seems to have a level head.

His hut is very close by, only just across the street to the northeast. If I wasn't stuck in here, I would have recruited him already. But with your arrival, you can go in my stead and speak with him. There's just one small wrinkle. 

+Naturally. What's the catch.
    ->T1c

=== T1c ===

Last night, I heard screams coming from the direction of his hut. I'm not sure what happened, and I dared not investigate. I need you to enter his hut and see what has become of Thatch. If he is well, recruit him and bring him back here. If he isn't... learn what you can and then return to me.

    +I'll see what has become of him.

        setToTrue(givenTutorialQuest)
        activateQuestStep(Look for Thatch, 0)
        activateQuestStep(The Plan,5)
        
        startTutorial({itemTutorialKey})

        prepForItem()
        
        Stay safe. And take this. It may become necessary to use it if things do not go your way. Should events take a turn for the worse, I will not blame you if you are forced to defend yourself.
        
        {
        -choseStrengthAtStart:
            giveItem({weaponListIndex},{cudgelIndex},1)
        -choseDexterityAtStart:
            giveItem({weaponListIndex},{shivIndex},1)
        -choseWisdomAtStart:
            giveItem({weaponListIndex},{plankIndex},1)
        -else:
            giveItem({weaponListIndex},{sharpRockIndex},1)
        }
        
        ->Close

=== T1d ===

Were you able to recruit Thatch?

{
-foundSlate and (thatchRemovedTutorialRubble or toldThatchAboutSlate):
    +Yes. He is with us.
        ->T1e
-else:
    +Not yet. I wished to discuss something with you first.
        keepDialogue()
        What is it?
        ->1c
}

=== T1e ===

finishQuest(Look for Thatch, true, 6)

Well done. What were the screams about. Is he hurt?

    +No, he was on a work detail last night. The screams were from his hutmate, Slate.
        ->T1f

=== T1f ===

What is Slate's condition?

    +He died from his injuries.
        ->T1g

=== T1g ===

I see. May his hearth keep him warm, then. But we must mourn the lost in stride, there is too much to do before we are free.

->2f

=== 2f ===
~explainingPlan = true

You performed well, and have earned my trust. You are now the lynchpin of the entire plan. Without you, the scattered pieces of our conspiracy cannot communicate. Take care not to bring attention to yourself, or else you will jeopardize everything.

    +Understood.
        ->2h
    +Of course. I know what I'm doing.
    ~saidIKnowWhatImDoing = true
        ->2h

=== 2h ===

{saidIKnowWhatImDoing:I'm glad to hear that. |}There are some parts of the plan that still need completing. I've assigned each task to a different hut. You'll need to check in on their progress and aid them with whatever they need. Are you ready?

    +Yes. Lay it on me.
        ->2i

=== 2i ===
The first thing we need to do is figure out where we are. The guards never talk about where this camp is located, and there are no landmarks that we can see past the forest.
    
There is no point in escape if we cannot reach some place where the Lovashi cannot follow. We cannot hold this place forever against whatever count owns these lands.

{
-not mineLvl3CarterAndNandorInParty:
    ->2ia
-else:
    ->2ib
}

=== 2ia ===

I have tasked Bálint with divining our location. He is the most learned of us, as he was a scholar before his branding. He lives in the hut just north of the gate, in the center of the camp.
    
    +I understand. What else?
    activateQuestStep(Aiding Bálint,0)
        ->2m


=== 2ib ===

changeCamTarget({carterIndex})

setToTrue(learnedCampLocationFromCarter)

prepForItem()

Actually Kastor, I think I may be able to help with this one. I've already told Nándor and Márcos about this while we were stuck in the mine, but since I trust everyone here and it will help our cause, I'll tell you and {playerName} about it as well.

addXP(250)

->2iba

=== 2iba ===

The plan was established with the belief that the camp resided somewhere within the Lovashi Confederation. But that isn't true. At least, not unless we've been invaded since I was planted at this camp three months ago. I obviously haven't been able to learn much of anything happening outside the camp since I arrived here, being a prisoner and all.

    +I don't understand: 'planted'? Someone sent you here from the outside?
        ~learnedCartersIdentity = true
        setToTrue(learnedCartersIdentity)
        ->2ic

=== 2ic ===

Correct. I am not Carter, the Confederation serf branded for horse-smuggling, but Carter, agent of the Kingdom of Masons. It was reported to Masonic Command by another agent that the Lovashi were operating a mining camp within our borders without our knowledge or consent. I was sent here to link up with that agent and assist in discovering why. Everything was going smoothly, too, until the lockdown occured. 

    +Kingdom of Masons? Where is that?
        ->2ica
    +What is this 'Masonic Command'?
        ->2icc
    +Who is this other agent?
        ->2icd
    +Does that mean you volunteered for the branding?
        ->2ice
    +If the Kingdom of Masons knows the location of the camp, why haven't they come to free us yet?
        ->2icf
    +Is your mission ruined if we free ourselves?
        ->2icg
    +Did you ever find out why the Lovashi built the camp?
        ->2ich
    +So where do we go from here?
    
        We still follow the plan Nándor, Kastor and I have put together, with two minor adjustments. First, if possible we should look out for my contact, Page, inside the Director's Manse. Her help may prove invaluable and she is essential to the completion of my mission. 
        
        And second, since we are within the Kingdom of Masons, when we have freed all of the slaves and are ready to leave the camp Page and I can lead you through the forest to the closest settlement. There you can be provided with food, water, shelter, medical treatment; whatever you should require.
    
        {
        -explainingPlan:
            changeCamTarget({kastorIndex})
            ->2m
        -backTo6cza:
            ~backTo6cza = false
            ->6cza
        -backTo3c:
            ~backTo3c = false
            changeCamTarget({kastorIndex})
            
            keepDialogue()
            
            Was there anything else?
            ->3c
        -aboutToStartRevolt:
            But all of that will come later. Let us focus on the present. Kastor?
            ~aboutToStartRevolt = false
            changeCamTarget({kastorIndex})
            ->7c
        -else:
            ->Close
        }
        
        

=== 2ica ===

    It's south of the Confederation. Much of the Confederation's southern border is protected by a mountain range known as the Waking Mountains. The Kingdom is just on the other side of those mountains.
    
    +I know very little of your homeland. What's it like?
        ->2icb
    +I see. I have other questions.
        keepDialogue()
        Ask me anything and I shall do my best to answer.
        ->2ic
        
=== 2icb ===

    I'm not surprised. Márcos was passingly familiar with the Kingdom, but Nándor knew nothing of it beyond it's name. The Confederation must purposefully suppress information about the outside world to keep their serfs uninformed about other ways of life. Or avenues of escape.
 
    changeCamTarget({kastorIndex})
    
    Nothing so overt. Many of us simply have no reason to learn about the world outside our borders. What use is geography to a serf farmer who will live and die on the same plot of land? I know where the Kingdom of Mason's resides, and I've actually met traders from your Kingdom. And I'm certain I'm not alone among the other slaves of this camp.
    
    changeCamTarget({carterIndex})
    
    I meant no offense, Kastor. Anyways, lets start at the basics. As you know, the majority of the serfs in the Confederation are descended from the Folk of the Craft, and most of the lords and landowners in the Confederation are descended from the Folk of the Saddle, or the Lovashi as they refer to themselves.
        
    Both the subjects and lords of the Kingdom of Masons, myself included, share blood with your people. Infact, almost all of the land now controlled by the Confederation was once owned by one Craft Folk Kingdom or another, though most of them succumbed to the Lovashi decades ago. We Masons are one of the last Kingdoms, alongside the Smiths and Jewellers.
    
    As for what the Kingdom is like itself? Much of the northern portion was ravaged when we repelled the last Lovashi incursion. After that, and the years of famine that followed, we've only recently begun resettling. Much of the north is overgrown or abandoned; a frontier within our own kingdom. But the south wasn't as affected, which is where most of the cities are. 
    
    setToTrue(carterSaidBrandedWouldBeTreatedLikeGuests)
    
    If we can make it out of this camp, you would be treated much better than you were in the Confederation. You would not be the first escaped branded to seek asylum in the Kingdom, and should you survive the journey we would make a point of treating you like an honored guest. In my home city of Efesus, it was considered a privilege and symbol of status to host a branded in your house and teach them your trade.
    
    +Anywhere would be better than here.
        keepDialogue()
        \*Carter chuckles.* I certainly understand the sentiment.
        ->2ic
    +It sounds a little too good to be true.
        keepDialogue()
        I don't mean to make it sound like your life is going to be a paradise forever after. And we still need to get out of this camp.
        ->2ic
    +Then it is a boon for us that the camp should be located inside your borders.
        keepDialogue()
        For you? Yes, I think it certainly is. But we have yet to understand why the Lovashi chose such a remote location for this mining camp. I don't think it's for any neighborly reason.
        ->2ic

=== 2icc ===

keepDialogue()

Masonic Command is the council of generals that advises the King on military matters. The scope of it's responsibilities range from raising and maintaining the King's hosts in times of war, to conducting espionage in times of peace. The latter, of course, being where I come in.
    ->2ic

=== 2icd ===

~learnedCartersIdentity = true
setToTrue(learnedPagesIdentity)

The other agent is a nonbranded slave in the service of the Director named 'Page'. She has been a spy in the Director's household for years; since well before this camp was built, and the Director became the Director. She's the one who got word back to Command about the camp's existence. I've only spoken a few times, since meeting within the camp walls was risky, but she seems to have a cool head on her shoulders.

    +It sounds like she would have to, considering how long she has been undercover.
        keepDialogue()
        You're right, you don't go deepcover like she has without knowing what you're about. But still, we should make it a priority to keep her safe. It would be disastrous for the Kingdom if she were killed in the confusion while we're trying to break out of here.
    
        ->2ic

=== 2ice ===

Yes it does. And don't start thinking my brand is fake. I was subjected to the full use of a captured branding tool, to ensure the authenticity of my appearance as one of the branded.

    +Why was that necessary? Surely a branded slave would have less mobility than a guard, or even one of the nonbranded.
    
    The mission briefing mentioned my contact inside the camp was having difficulty discovering it's purpose. From the messages my contact sent, we knew that the Director kept it all very discrete, even with members of his own household. A branded slave would be subject to worse abuses and labor, sure, but they would also have access to areas of the mine it would be suspicious to see a nonbranded slave inside. Hence why I maneuvered my way into getting placed on the bottom floor. It was easier to get close to what they were doing down there that way.
    
    keepDialogue()
    
    Also the urgency of the mission lent itself to inserting an agent as a branded. The Director was very picky about what guards he manned the camp with, as well as what nonbranded slaves he brought from his estates. It would have taken months, or more, to create a false identity, get that identity noticed by the Director, then brought here to the camp, all the while not arousing his suspicions. Much easier to spy on the shipments of new slaves making their way into our territory, and swap out one lucky branded for my unlucky self before the guards noticed anything amiss. 
        ->2ic

=== 2icf === //If the Kingdom of Masons knows the location of the camp, why haven't they come to free us yet? 

I don't know all of the details, but I was told by my superior officer that Command was split on the issue. Some of its highest members did favor sending troops to capture the camp, but many others wanted to use covert means to learn more about it. The thinking went: storming the camp might destroy evidence of it's purpose, or give the enemy time to destroy it themselves. 

~toldDirectorIsAWarHero = true
setToTrue(toldDirectorIsAWarHero)

keepDialogue()

There's also the fact that the Director is something of a war hero back in the Confederation. It wasn't obvious to Command whether his death or capture would result in the Confederation using an attack on the camp as an excuse for war. Command is deathly afraid of provoking another conflict with the Lovashi, and will do pretty much anything to avoid it if they can, including permitting the temporary colonization of the King's holdings, apparently.
    ->2ic

=== 2icg === //+Is your mission ruined if we free ourselves?

Perhaps, perhaps not. I obviously have been out of contact with the agent I was sent to work with since I got locked inside the mine. She may have made progress on determining the purpose of this camp since the lockdown began. And even if she hasn't, if during the slave revolt we were able to take the Director alive, we may be able to interogate him. Or bring him back to Command and have them do it. 
    {
    -toldDirectorIsAWarHero:
        +Didn't you say that capturing the Director could start a war? What's changed?
            
            My superiors didn't favor a military intervention because they didn't want to start a war. But I believe that the Lovashi went to all the effort to create this camp because they are already looking to start one. That's just the way they are.
            
            keepDialogue()
            
            It won't make much of a difference if that war comes tomorrow or a few months from now. But if we can take the Director alive and extract what he knows, that may give us an understanding of the Lovashi's attack strategy. That could give us an edge.
            ->2icg
    }

    +If helping us puts your mission in jeopardy even a little, why would you?
        
        keepDialogue()
        
        When Nándor approached Kastor and I with his plan to escape, that same question crossed my mind. But as Nándor began to explain his plan in detail, I felt it was more and more likely to succeed. I also knew that if I were to refuse to help I would be dashing the hopes of men that I had become close with. I believed that my connection to Masonic Command would see me eventually leaving the camp, but they had no such hopes outside the plan of theirs. It would take a colder man than I to snuff that out.
        
        ->2ic

=== 2ich === //Did you ever find out why the Lovashi built the camp?

Sort of. The way I understand it, the Lovashi were looking for something. When we hit the ruins that would become the second floor of the mine, they seemed ecstatic. Briefly, progress picked up and we received a major shipment of new slaves soon afterwards. But once the majority of the second floor was excavated, they no longer seemed happy, and kept digging.

{
-deathFlagGuardMárcos:

I asked Márcos while we were stuck on the third floor what it was all about and he admitted he didn't even know. He explained that some of the Chiefs and Overseers spoke like they knew more, but they wouldn't tell any of the low ranking guards.

-else:
changeCamTarget({marcosIndex})

Carter and I have discussed this at length. Aside from some of the Chiefs and Overseers, and the Director himself, none of the guards know what the purpose of this camp is. We know we're camped on Mason land, and not to discuss that with any of the slaves, but otherwise the officers don't give away much else.

changeCamTarget({carterIndex})

}

keepDialogue()

Between what Márcos told me, and what I could piece together with my contact's help, I suspect there's another set of ruins somewhere deeper that they are still looking for. What they'll do when they find it remains a mystery.

    ->2ic

=== 2j ===

Lastly, we need to make contact with the slaves that work the Manse and guards' quarters. They aren't branded like us. We need to gauge if they'll fight with us, stand aside, or fight alongside their masters.
    
    +Why wouldn't they fight with us?
        ->2k

=== 2k ===

Non-branded slaves view themselves as superior to the branded. They serve their sentences for noncapital crimes, and their punishment isn't to be worked to death like us. Often their sentences are finite where ours only end with our lives.

A branded slave's only chances at freedom are to fight or escape, but the brandless may simply choose to wait out their sentence and return to society a free citizen afterwards. We need to know if the non-branded slaves will choose to fight with us for freedom, or back their masters.

    +I see.
        ->2l

=== 2l ===

activateQuestStep(Aiding Ervin,0)

For this, you'll need to speak to Ervin in the hut directly across the street from this one. He is the only slave to receive his brand within the camp. He should know how to approach the situation.

    +Was that it?
        ->2n

=== 2m ===

The next thing we must do is arm ourselves. The guards protect the keys to their armory well, of course, but the mine's closing affords us a rare opportunity. There is another armory within the mine, and I doubt the guards had time to raid it before they evacuated.


I gave the job of obtaining a key to the armory to Janos. He lives in the hut to the west of Broglin's. Originally, he was supposed to acquire the key to the armory within the guardhouse. Inform him that the plan has changed and work with him to obtain the mine armory's key.

{
-hasToolBundle and not gaveKastorToolBundle:
    +Actually, I was able to raid the armory while I was in the mine.
        ->6b
-obtainedMineArmoryKey:
    +Actually, I already have the key to the mine's armory.
        ->2ma
-else:
   +Got it.
        setToTrue(toldToAidJanos)
        activateQuestStep(Aiding Janos,0)
        ->2j
}

=== 2ma ===

setToTrue(toldKastorObtainedMineArmoryKey)
Extraordinary. When did you have time to do that?

{
-gotArmoryKeyFromGuardHouse:
    +I was sneaking around the Guardhouse and found it on the top floor.
    ->2mb
-mineLvl2BatBossKilled:
    +Someone left one down in the mines.
    ->2mb
-else:
    +It doesn't matter where I got it. We have it now.
    ->2mb
}

=== 2mb ===

prepItem()

Very well done. This means we can skip this part of the plan.

addXP(250)

->2j

=== 2n ===

That's it for now. There is one final part that comes after, but it doesn't matter unless the rest is achieved. We'll discuss it when you return. Farewell.

setToTrue(gotThePlanFromKastor)

activateQuestStep(The Plan,6)

~explainingPlan = false
~gotThePlanFromKastor = true

{
- broughtNandorToKastor:
    ->deactivateExtras
-else:
    ->Close
}

=== 3a ===

You're back. Were you successful?

{
- (refusedToWorkWithJanos or (playerFinishedErvinsTask() and not toldKastorFinishedErvinsTask) or (deathFlagImre and not toldKastorFinishedErvinsTask) or (deathFlagErvin and not givenTaskByErvin) or (deathFlagJanos and not obtainedMineArmoryKey) or (finishedBalintsTask and not toldKastorFinishedBalintsTask) or (deathFlagBálint and not toldKastorFinishedBalintsTask) or (mentionedBadReasonForGoingInsideMine and not learnedAboutMuzsasSweetToothFromKastor) or (obtainedMineArmoryKey and not toldKastorObtainedMineArmoryKey) and not (toldKastorErvinIsDead and toldKastorBalintIsDead and toldKastorJanosIsDead)):

    +I have something to report.
        ->3c
}
    +Not yet.
        ->3b
    +Why is your hut so much bigger than Broglin and Garcha's?
        ->keepDialogueB4HutSizeExplanation(->3a)
    +Tell me about what happened to the mine.
        ->mineExplanationDisclaimer(true,->3a)

=== 3b ===

Please hurry. We need this over quickly or else the guards might catch on.
    ->Close

=== 3ba ===

~ learnedAboutMuzsasSweetToothFromKastor = true
setToTrue(learnedAboutMuzsasSweetToothFromKastor)

activateQuestStep(Múzsa's Sweet Tooth, 3)

I see. I've never spoken with her, but I've overheard some of the guards say she's always on the lookout for sweets. Must be hard to get them in the camp, since we never let outsiders in here.

prepForItem()

The cook in the mess hall may have some in stock, or know how to get some. I'm not sure how you'll convince him to help you, so you may have to barter. I don't have much money, but I managed to swipe some gold a long time ago when I was sent to work inside the guard barracks for a day. Take it with you, it could be useful.

giveCoins(30)

    ->Close


=== 3c ===

What happened?

{
-mentionedBadReasonForGoingInsideMine and not learnedAboutMuzsasSweetToothFromKastor:
    +I spoke to Guard Múzsa at her post in front of the mine. She won't let me inside.
    ->3ba
}

{
-deathFlagErvin and not toldKastorFinishedErvinsTask and not toldKastorErvinIsDead:
    +Ervin was dead when I arrived at his hut.
        setToTrue(toldKastorErvinIsDead)
        ->3db

-playerFinishedErvinsTask() and not toldKastorFinishedErvinsTask:
    {
        
        -convincedImre and deathFlagImre:
            +I tried to convince the Manse slaves, but deemed them untrustworthy. We can't rely on their support.
                ~toldKastorFinishedErvinsTask = true
                setToTrue(finishedErvinsTask)
                setToTrue(toldKastorFinishedErvinsTask)
                finishQuest(Aiding Ervin, true, 7)
                    ->3d
    
        -convincedImre:
            +I did what Ervin asked of me. The Manse slaves will aid us.
                ~toldKastorFinishedErvinsTask = true
                setToTrue(finishedErvinsTask)
                setToTrue(toldKastorFinishedErvinsTask)
                finishQuest(Aiding Ervin, true, 7)
                
                prepForItem()
                
                This is good! Very good. There aren't as many of them as us, but their help will certainly make a difference.
                
                addXP(250)
                
                keepDialogue()
                
                When we attack we will need to make our way to the Manse to set them free. If left on their own, they will not be able to help us or make their own escape.
                
                {
                -mineLvl3CarterAndNandorInParty:
                    ->7aa
                -else:
                    ->3c
                }
        -imreWontSpeakToPlayer and not deathFlagImre:
            +I couldn't convince the Manse slaves.
                ~toldKastorFinishedErvinsTask = true
                setToTrue(finishedErvinsTask)
                setToTrue(toldKastorFinishedErvinsTask)
                finishQuest(Aiding Ervin, true, 7)
                    ->3d
        
        -deathFlagImre:
            +I couldn't convince the Manse slaves. I had to kill one of them to keep our secret.
                ~toldKastorFinishedErvinsTask = true
                setToTrue(finishedErvinsTask)
                setToTrue(toldKastorFinishedErvinsTask)
                finishQuest(Aiding Ervin, true, 7)
                    ->3d
        -terrifiedImre and not convincedImre:
            +I applied some pressure to the Manse slaves. They will aid us when the time comes.
                ~toldKastorFinishedErvinsTask = true
                setToTrue(finishedErvinsTask)
                setToTrue(toldKastorFinishedErvinsTask)
                finishQuest(Aiding Ervin, true, 7)
                
                prepForItem()
                
                This is good! Very good. There aren't as many of them as us, but their help will certainly make a difference.
                
                addXP(250)
                
                keepDialogue()
                
                When we attack we will need to make our way to the Manse to set them free. If left on their own, they will not be able to help us or make their own escape.
                    ->3c
        -else:
            Error: This dialogue with Kastor shouldn't be reachable
                ->Close
    }
}

{
-finishedBalintsTask and not toldKastorFinishedBalintsTask:
    +I spoke with Bálint. He has determined the camp's location.
    ~toldKastorFinishedBalintsTask = true
    setToTrue(toldKastorFinishedBalintsTask)
        ->3e
}    

{
-deathFlagJanos and not toldKastorObtainedMineArmoryKey and not toldKastorJanosIsDead:
    +Janos was dead when I entered his hut. I was unsuccessful in acquiring the key to the mine's armory.
        setToTrue(toldKastorJanosIsDead)
        ->3ga

-refusedToWorkWithJanos and not obtainedMineArmoryKey and not kastorExplainedWhereToFindAnotherKey:

    +Janos's plan involves talking with a guard. I refused to cooperate with them.
        ->3m

-obtainedMineArmoryKey and not toldKastorObtainedMineArmoryKey:
        ->3ca
}    

{
- broughtNandorToKastor and gaveKastorToolBundle and toldKastorFinishedErvinsTask and toldKastorFinishedBalintsTask:
    +I've completed everything. I believe we can now start this revolution.
        ->7a

-(toldKastorFinishedErvinsTask + toldKastorFinishedBalintsTask  + toldKastorObtainedMineArmoryKey < 3) and (toldKastorFinishedErvinsTask + toldKastorFinishedBalintsTask  + toldKastorObtainedMineArmoryKey > 0) :
    +That's it for now.
        You're making good progress. Keep to it.
        ->Close

-toldKastorFinishedErvinsTask and toldKastorFinishedBalintsTask and toldKastorObtainedMineArmoryKey and not toldToFindNandor:

    +I've completed everything. What is our next move?
        ->4a
-else:
    +Nevermind. I'll be back.
        ->Close
}
    

        
=== 3ca ===

{
-toldToFindTools and hasToolBundle and not gaveKastorToolBundle:
    +I have the tools.
        ~backTo3c = true
        ~toldKastorObtainedMineArmoryKey = true
        setToTrue(toldKastorObtainedMineArmoryKey)
        ->6b
}

{
-obtainedMineArmoryKey and gotArmoryKeyFromGuardHouse:
    +I have returned from the guardhouse, where I found the armory key.
        ~toldKastorObtainedMineArmoryKey = true
        setToTrue(toldKastorObtainedMineArmoryKey)
        ->3g
}

{
-obtainedMineArmoryKey and mineLvl2BatBossKilled:
    +You need not worry about the key, I found a spare within the mine itself.
        ~toldKastorObtainedMineArmoryKey = true
        setToTrue(toldKastorObtainedMineArmoryKey)
        ->3g
}

{
-obtainedMineArmoryKey and gotKeyFromJanos:
    +I have met with Janos, and obtained the key to the mine's armory.
        ~toldKastorObtainedMineArmoryKey = true
        setToTrue(toldKastorObtainedMineArmoryKey)
        ->3g
}
    

=== 3d ===

    prepForItem()

    Damn it all. That means we have less time than I would have liked to get this plan moving. Did you leave any witnesses to what you were discussing?
    
    addXP(250)
    
    {
        -deathFlagImre:
        +No, they were dealt with.
            ->3dd
        -else:
        +Yes, I was unwilling to kill another slave.
            ->3da
        +No, they were dealt with. <Lie>
            ->3dd
    }
            
=== 3da ===
    I understand why you did what you did, but if they were not inclined to side with us then the Manse slaves may be forced to fight for their masters before the plan concludes. Or some of the branded may take revenge after the guards are dealt with. You may have doomed us, and certainedly did not guarentee their safety.
        
        +None of that matters. I saved a life in the moment and I am content with that.
        {
        -mineLvl3CarterAndNandorInParty:
            Lets not debate the point further. We're short on time as it is.
                ->7aa
        -else:
            keepDialogue()
            Lets not debate the point further. We're short on time as it is.
                ->3c
        }

        +I... see. Nothing can be done about it now however.
        {
        -mineLvl3CarterAndNandorInParty:
            Lets not debate the point further. We're short on time as it is.
                ->7aa
        -else:
            keepDialogue()
            Lets not debate the point further. We're short on time as it is.
                ->3c
        }

=== 3db ===
    
    \*Kastor solemnly takes in the news.* I did not know Ervin well, but no one deserves to die a death like that. Alone and maimed with no one to mourn you. At least he is no longer in pain.
    
        +\*Say nothing.*
                ->3dc
        +Truly an unworthy death.
            Indeed...
                ->3dc

=== 3dc ===

    finishQuest(Aiding Ervin, true, 8)
    
    prepForItem()
    
    Continue on with the other tasks you have been given. Do not worry, when the time comes we'll pay the guards back, blood for blood, tooth for tooth.
    
    addXP(100)
        ->Close


=== 3dd ===
{
-mineLvl3CarterAndNandorInParty:
//empty on purpose
-else:
keepDialogue()
}

It is unfortunate that you were forced to take the life of a slave, but what we are doing is tricky. One failure and the entire plan could collapse. Hiding your tracks as you did will save lives in the future.

    {
    -mineLvl3CarterAndNandorInParty:
        ->7aa
    -else:
        ->3c
    }

=== 3e ===

Excellent! Where are we?
    
    +He said we're beyond the Waking Mountains, in the northern portion of the Kingdom of Masons. 
    
        {
        -mineLvl3CarterAndNandorInParty or broughtNandorToKastor:
            finishQuest(Aiding Bálint, 4)
        
            changeCamTarget({carterIndex})
            
            prepForItem()
    
            Not to take away from what you've accomplished, {playerName}, but I need to talk to everyone about that.
    
            addXP(500)
            
            keepDialogue()
            
            The plan was established with the belief that the camp resided somewhere within the Lovashi Confederation. But as {playerName} pointed out, that isn't true. At least, not unless we've been invaded since I was planted at this camp three months ago. I obviously haven't been able to learn much of anything happening outside the camp since I arrived here, being a prisoner and all.
                ~backTo3c = true
                ->2iba
        }
    
        ->3f

=== 3f ===

finishQuest(Aiding Bálint, true, 4)

prepForItem()

That is... That is not what I was hoping for.

addXP(250)

    +Bálint seemed to think it was fantastic news.
        
        keepDialogue()
        It's good news for the plan; we won't need to worry about a thousand mounted Lovashi barrelling down on us. But I was hoping to see my two sons again one day. That will be much trickier now. But enough of that. Let us dwell on the present. Have you made headway on the rest of it?
        ->3c

=== 3g ===

Well done. Can I see it?

    +Here. *Show Kastor the Mine Armory Key.*
        ->3h

=== 3ga ===

The guards killed Janos? But he was always so friendly, and he never gave them a reason to hurt him. Why would they kill him?

    +\*Say nothing.*
        ->3gb
    +I don't know. Do slavers need a reason to hurt a slave?
        No, you're right. They don't. They're animals to the last, and we'll make them pay when our moment comes.
        ->3gb

=== 3gb ===

finishQuest(Aiding Janos, true, 9)

prepForItem()

Without the key to the mine's armory, accessing it will be difficult. But perhaps there is another way that we do not know about. For now, keep moving with the plan; let me know when you've made more headway and we'll discuss the next steps.

addXP(100)

    ->Close

=== 3h ===

prepForItem()

\*Kastor examines the key and then hands it back to you.* You should keep it. Not much I could do with it from in here anyways. Were there any complications with acquiring it?

addXP(250)

{
-killedChiefIren and gotArmoryKeyFromGuardHouse:

    +While I was grabbing the key I was forced to kill Chief Irén.
        {
        -not explainingPlan:
        finishQuest(Aiding Janos, true, 12)
        }
        ->3i
}
{
-gotArmoryKeyFromGuardHouse:

    +None.
    {
    -explainingPlan:
        In and out, quick and clean. Exceptional.
            ->2j
    -else:
        keepDialogue()
        In and out, quick and clean. Very good. Anything else?
            ->3c
    }
}
{
-deathFlagAndrás:
    +While following Janos's plan, I was forced to kill a guard.
        finishQuest(Aiding Janos, true, 10)
        ->3i
}
{
-gotKeyFromJanos:

    +Janos's plan to get the key included letting a guard in on the plan.
        ->3j
    +None worth mentioning. <Lie>
        finishQuest(Aiding Janos, true, 10)
        keepDialogue()
        Incredible. You performed your part to perfection. Continue like this and we'll all be free in no time.
            ->3c
}

=== 3i ===

That is certainly bad news, but if I'm being honest I expected that we would have to kill a guard eventually. Were you able to remain discreet?

{
-killedChiefIren and gotArmoryKeyFromGuardHouse:

    +I killed her on the second floor of the guardhouse and left no witnesses to where I stashed the body.
        {
        -explainingPlan:
            At least we have that going for us. It does mean however that we'll need to move quicker than I would have liked. But it can't be helped now.
            ->2j
        }
        keepDialogue()
        At least we have that going for us. It does mean however that we'll need to move quicker than I would have liked. But it can't be helped now. Anything else?
            ->3c

-deathFlagAndrás:
    +I killed him during a private meeting with Janos, and Janos hid the body. No one will miss him for some time.
        keepDialogue()
        At least we have that going for us. It does mean however that we'll need to move quicker than I would have liked. But it can't be helped now. Anything else?  
            ->3c
}

=== 3j ===

Tell the guards? Did Janos betray us?

    +Almost certainly. We should proceed with caution.
        finishQuest(Aiding Janos, true, 9)
        setToTrue(saidJanosIsATraitor)
        keepDialogue()
        The guards have yet to take me to the pit, so we shall continue as if this hasn't happened yet. But should they come for me, I shall at least know who to curse with my final breath.
            ->3c
    
    +I do not believe so. Evidenced by the fact that they haven't come for us yet.
    finishQuest(Aiding Janos, true, 11)
    {
    -deathFlagAndrás:
        keepDialogue()
        Hmmm, very true. Well, the death of a guard is certainly bad news, but if I'm being honest I expected that to happen eventually. Were you able to remain discreet?
            ->3i
    -else:
            ->3k
    }
    
=== 3k ===

Hmmm, very true. What possessed Janos to want to put his trust in one of the guards?

    +Love. Janos cares for András, and András for Janos.
        keepDialogue()
        A strange thing to find in this camp, but I am forced to trust his judgement no matter how little I want to. Let us just hope he is not as mad as this makes him sound.
            ->3c
            
    +I spoke with the guard and I could see we could rely on him. He is different from the rest.
        ->3l
            
=== 3l ===

You- Th- What? You told a guard our plan just because he seemed nice?

    +You weren't there, so you don't get to judge. All you can do now is trust Janos, and trust me.
        keepDialogue()
        I am forced to trust both of your judgements no matter how little I want to. Let us just hope neither of you are as mad as this makes you sound.
            ->3c

=== 3m ===

setToTrue(kastorExplainedWhereToFindAnotherKey)
activateQuestStep(Aiding Janos,4)

I see. I'm not sure what Janos was thinking, but I understand your decision to not go through with that. We'll have to proceed with the backup plan.

There are a few guards entrusted to hold the keys to the various store rooms and armories throughout the camp. One of these guards is Irén, the Chief Surveyor of the mines. She's the one who plans where we dig and how deep, surveys the rock to see if it's safe to excavate, that sort of thing. Rumors were that she was sick before the lockdown, so she's probably still confined to her quarters somewhere inside the guardhouse.

It may be easy to enter the ground floor of the guardhouse, since before the lockdown it wasn't uncommon for slaves to need to run an errand for a guard there. But should your search take you to the second floor, be very careful. It's off limits to anyone but the guards. 

->Close

=== 4a === //I've completed everything. What is our next move?

Our next move is the most dangerous. Not to diminish everything you've done already, but now you must walk through fire to save us.

    +What could be more dangerous than risking discovery by the guards?
        ->4b

=== 4b ===

I am not the original architect of the escape plan, I was just the first person to be entrusted with it's details. The one who set it into motion, and the one who convinced the rest of us that it was worth the risk, was another branded named Nándor. It was his words that gave us the hope to fight.

    +I see. Which hut is he in?
        ->4c

=== 4c ===

~toldToFindTools = true
setToTrue(toldToFindTools)

Unfortunately, he's not in any hut. The last I saw him, he was working with me on the third level of the mine. When the mine was shut down, I lost most of my team, including Nándor.

{
-knowsAboutTheMine:
    +I'm starting to think you want me to look for Nándor.
        keepDialogue()
        The final thing we need to do before rallying the other slaves is sneak into the mine to access the armory there. It's on the second level, near the access point to the third. But once you're back with the weapons, having Nándor with us will be a massive morale gain for our cause. The others will need to think we can actually win this if they're going to risk their lives.
        ->4d
        
    +What happened to the mine?
    ~discussingNandor = true
        ->mineExplanation1(false,->4d)
-else:
    +What happened to the mine?
    ~discussingNandor = true
        ->mineExplanation1(false,->4d)
}

=== 4d ===
~discussingNandor = false

That I was. But back to the plan. The final thing we need to do before rallying the other slaves is sneak into the mine to access the armory there. It's on the second level, near the access point to the third. But once you're back with the weapons, having Nándor with us will be a massive morale gain for our cause. People will need to think we can actually win this if they're going to risk their lives.

{
-hasToolBundle and not gaveKastorToolBundle:
    +I actually have already retrieved the tools from the armory.

    activateQuestStep(The Plan,10)
    ~gaveKastorToolBundle = true
    setToTrue(gaveKastorToolBundle)
    
    prepForItem()

    \*Kastor's eyes go wide.* By all the Gods, you move fast. Incredible work.

    addXP(400)

    prepForItem()

    I will hide the tools for now. When we are ready to begin the revolt, I will retrieve them.

    takeAllOfItem(Tool Bundle)

    keepDialogue()

    All that is left to do is rescue Nándor. Since you seem extremely comfortable traversing the mines, I think you'll have no trouble finding him.

        ->4d
}
    +From what you've told me, it doesn't sound like Nándor is still alive.
        ->4e

=== 4e ===

The chances that he is still breathing are slim. But the last time I saw Guard Márcos was when he was trying to save those trapped on the third level. And there were other guards in the tunnel when those beasts appeared. They may have been able to fight their way to a safe place within the mine. If you're already going down there, attempting to save him is worth the risk.

 
    +I'll see what I can do, but if it's too dangerous I'm coming right back up.
        ->4f
    +If it will help us escape, I'll do it.
        ->4f
    +Easy for you to say, I will be the one risking myself by going into that hellhole. 
        ->4g
    +I'm not going down there. This sounds like certain death.
        ->4fa

=== 4fa ===

I understand your apprehension, but you're the only one capable of leaving their hut without being stopped by the guards. If you do not do this, no one can. The whole plan will fall apart.

    +I understand what is at risk. I'll do it.
        ->4f
    +I said no. I'm not getting myself killed.
        ->4faa

=== 4faa === //if it will help us escape, I'll do it. / I'll see what I can do,

{
- gaveKastorToolBundle:
activateQuestStep(The Plan, 11)
-else:
activateQuestStep(The Plan, 7)
}


If you need some time to think about it, take it. But once this lockdown ends, the guards will have been reinforced and we'll have lost our opportunity. Please, do not take too long.

    +Don't hold your breath.
        ->Close


=== 4f === //if it will help us escape, I'll do it. / I'll see what I can do,

Thank you. I must stress that the task I've laid before you is not easy by any means. The mine was a hostile place even before the guards abandoned it. Now... if you are unprepared it may prove deadly. 
        ->4h

=== 4g ===

I apologize, what I'm asking of you is an enormous undertaking. But because of your status with the guards, you are the only one able to carry it out. 
        ->4h

=== 4h ===

setToTrue(toldToFindNandor)
activateQuestStep(The Plan, 8)
activateQuestStep(Finding Nándor, 0)
activateQuestStep(Explore the Mine, 0)

If you do not believe you are ready to face what is down there, I would recommend looking around the camp for any advantage you can bring with you. I believe the guards will have barricaded the mine entrance as well, so you may need to talk or sneak your way past them to get inside. If you need assistance, you can come back to me and I will help however I can. Take care.
    ->Close
    
=== 5a ===

Balint, Ervin and Janos are dead. All dead! The plan is in shambles. The guards have been one step ahead of us at every turn and I no longer know how to proceed. I wish Nándor was here, he would know what to do.

    +Nándor? Who is he?
        ->5b

=== 5b ===

Nándor was the one who came up with the escape plan. He convinced us all to take part in it. He's the real leader here, I've only been putting together the pieces to his plan. But I haven't seen him since the lockdown. He was a part of the work team that was excavating the lowest level of the mine with me, so he must still be trapped down there. Or he's already dead. 

    +If Nándor was here, would he know how we should proceed?
        ->5c

=== 5c ===

I don't know, but if anyone would it would be him.

    +I could go looking for him.
        ->5d
    +It's a moot point: if he's been trapped down there all this time he's already dead.
        ->5ca

=== 5ca ===

You're right. I want in my heart to believe he's alive, but I'm shaken by all these recent deaths. I can no longer muster the hope to believe he still breathes. I think I need some time alone. Please, leave me.

    +As you wish.
        ->Close

=== 5d ===

setToTrue(toldToFindNandor)

activateQuestStep(The Plan, 9)
activateQuestStep(Finding Nándor, 0)

I was going to ask you to enter the mine once we had all of the other pieces in place. But now that everything has fallen apart you volunteer to go? If you would do this for us, you would be a hero to all of the branded, even more than you already are. 

The chances that Nándor is still breathing are slim. But our chances of escaping without him to lead us are slimmer still. The other branded will not rise up if they believe the plan won't succeed, and he is the only one who can convince them to fight for their lives.

If you do not believe you are ready to face what is down there, I would recommend looking around the camp for any advantage you can bring with you. Perhaps if you find a way to convince the guards to let you into the mine, they may let you bring an ally or two with you. However, if you cannot, sneaking yourself and others inside may be your only option. And thank you.

->Close


=== 5e ===



->Close


=== 5f ===



->Close

=== 6a ===

You're back. Were you successful?

{
-hasToolBundle and not gaveKastorToolBundle:
    +I have the tools.
        ->6b
}
{
-mineLvl3CarterAndNandorInParty and not deathFlagNándor and not broughtNandorToKastor:
    +I have found Nándor.
        ->6c
}

    +Not yet.
        ->3b
    +Why is your hut so much bigger than Broglin and Garcha's?
        ->keepDialogueB4HutSizeExplanation(->6a)
    +Do you know what happened to the mine?
        ->mineExplanationDisclaimer(true,->6a)

=== 6b ===
~gaveKastorToolBundle = true
setToTrue(gaveKastorToolBundle)

prepForItem()

Amazing! There's enough here to arm a good deal of us. The rest will have to take arms from any guards we are able to slay.

takeAllOfItem(Tool Bundle)

{
-explainingPlan:

prepForItem()

With these tools, we stand an actual chance of facing the guards in a brawl. And the other slaves will know it too. This may convince more of them to join our cause than any simple combination of words could ever do. Outstanding work.

addXP(650)

{
-backTo3c:
    ~backTo3c = false
    keepDialogue()
    
    Was there anything else?
    
    ->3c
-else:
    ->2j
}



-broughtNandorToKastor:
prepForItem()

You have done everything I have asked of you, and I believe that we are ready to begin this revolt. And with you and Nándor at our sides, the branded will no doubt rally to our cause.

addXP(400)

    ->7a

-else:

prepForItem()

You have done everything I have asked of you, and I believe that we are ready to begin this revolt. But what of Nándor? Were you able to find him?

addXP(400)

    {
    -mineLvl3CarterAndNandorInParty and not deathFlagNándor and not broughtNandorToKastor:
        +I also have found Nándor.
            ->6c
    -else:// Nandor is not with you but you have found the tools
        +I have not found Nándor yet, but I will continue searching.
            activateQuestStep(The Plan,10)
            Good. I will hide the tools and await your return. When that happens, we can finally rise up and make the masters pay.
            
            ->Close
    }
}


=== 6c ===

fadeToBlack(true, false)

hideTrain()

setToTrue(broughtNandorToKastor)
~broughtNandorToKastor = true

activate({nandorIndex})
activate({carterIndex})

{
-not deathFlagGuardMárcos:
    activate({marcosIndex})
}

fadeBackIn(60)

{
-metKastor:

prepForItem()
    
Nándor! Carter! Thank the Mother you've returned.
            
addXP(400)

    ->6cz
-else:

prepForItem()
    
Nándor! Carter! Thank the Mother you've returned. But who is this with you?
            
addXP(400)

}

changeCamTarget({nandorIndex})
    
This is {playerName}. They ventured into the mine when none of the guards dared return there, and freed us.

changeCamTarget({kastorIndex})

{playerName}, we are in your debt. More so than you may realize.

{
-deathFlagGuardMárcos:        
    And what of Guard Márcos? I had hoped to thank him for saving our life.

    changeCamTarget({carterIndex})

    He chose to seal the pocket that the worms came from with some blasting jelly. We won't need to worry about them at our backs when we face the guards. And all thanks to him.

    changeCamTarget({kastorIndex})

    Even in death, he continues to provide for us. I am once more in his debt as well.
    
    +'Face the guards'? I was aware Kastor was involved with the plan that Broglin mentioned, but you two are as well?
    
        changeCamTarget({nandorIndex})
        
        Yes, and infact, Kastor, Carter, and myself were the first to set the plan in motion. We decided who to recruit, and what jobs they should perform.
        
        changeCamTarget({kastorIndex})
        
        With that out of the way, we should get right to discussing how to procede with the plan. A guard could enter at any moment and disrupt us, and we would have a hard time explaining Nándor, Carter, and Márcos's sudden reappearance.
            ->6cab
}

->6ca

=== 6cz ===

changeCamTarget({nandorIndex})

Kastor, I am glad you were able make it out alright. It is good to be back above ground again, and it's all thanks to {playerName}'s heroism.

finishQuest(Finding Nándor, true, 3)

changeCamTarget({kastorIndex})

I am the one who should be glad that you survived. You were down there for days!

{
-not deathFlagGuardMárcos:
And Márcos! You live as well? I can't imagine what it would take to kill you after something like this!

changeCamTarget({marcosIndex})

Do not rejoice just yet, Kastor. I am liable to collapse at any moment.

changeCamTarget({kastorIndex})

I can see that. Here, sit. I will tend to you as best I can when I am able.
-deathFlagGuardMárcos and mineLvl3MarcosAgreedToIgniteJelly:
And what of Guard Márcos? I had hoped to thank him for saving our life.

changeCamTarget({carterIndex})

He chose to seal the pocket that the worms came from with some blasting jelly. We won't need to worry about them at our backs when we face the guards. And all thanks to him.

changeCamTarget({kastorIndex})

Even in death, he helps our cause. I am once more in his debt.
}

->6cza

//VAR toldKastorObtainedMineArmoryKey = false
//VAR toldKastorFinishedErvinsTask = false

=== 6cza ===

{
-hasToolBundle and not gaveKastorToolBundle:
    +If I may interject, I also have these tools from the mine's armory. They should help our cause. *Hand over the Tool Bundle.*
        ->6b
/*
-finishedBalintsTask and not toldKastorFinishedBalintsTask:
    +I'd like to bring up that I helped Bálint learn that the camp is located within the Kingdom of Masons.
    
        finishQuest(Aiding Bálint, 4)
        
        changeCamTarget({carterIndex})
        
        prepForItem()

        Not to take away from what you've accomplished, {playerName}, but I need to talk to everyone about that.

        addXP(250)
        
        keepDialogue()
        
        The plan was established with the belief that the camp resided somewhere within the Lovashi Confederation. But as {playerName} pointed out, that isn't true. At least, not unless we've been invaded since I was planted at this camp three months ago. I obviously haven't been able to learn much of anything happening outside the camp since I arrived here, being a prisoner and all.
        ~backTo6cza = true
        ->2iba

-finishedErvinsTask and not toldKastorFinishedBalintsTask:
    +I'd like to say that I 
    
        finishQuest(Aiding Bálint, 4)
        
        changeCamTarget({carterIndex})
        
        prepForItem()

        Not to take away from what you've accomplished, {playerName}, but I need to talk to everyone about that.

        addXP(250)
        
        keepDialogue()
        
        The plan was established with the belief that the camp resided somewhere within the Lovashi Confederation. But as {playerName} pointed out, that isn't true. At least, not unless we've been invaded since I was planted at this camp three months ago. I obviously haven't been able to learn much of anything happening outside the camp since I arrived here, being a prisoner and all.
        ~backTo6cza = true
        ->2iba
*/
-else:
    +I think it is about time we made our move before the guards catch on to our return.
        ->6d
}

=== 6ca ===

{
-toldCarterPassword:

    changeCamTarget({carterIndex})
    
    {playerName} is in on the plan. They provided the passphrase when we first met.
    
    changeCamTarget({kastorIndex})
    
    Excellent. Then if you haven't been informed already, Nándor was the first among us to work on the plan. He enlisted my and Carter's help in the days leading up to the lockdown, and we decided between the three of us who else to recruit. Then I was the one to recruit them to the cause and set them with on their individual tasks. Who recruited you?

    +Broglin and Garcha. They needed a way to communicate during the lockdown and helped me gain the trust of Guard László so I could leave my hut. *Show badge*
        ->6caa    
                
-else:

    changeCamTarget({carterIndex})
    
    I think we should let {playerName} in on the plan. Anyone who has bled alongside us like they have is worth trusting.
    
    changeCamTarget({nandorIndex})
    
    I agree.
    
    {
    -spokeToGarchaAboutPlan:
        +Actually, Broglin and Garcha already filled me in on the plan.
        {
        -toldCarterWrongPassword:
            ->6cb
        -else:
            ->6caa
        }
    -else:
        +Plan? What plan.
            ->Close
    }
}

=== 6caa ===

    ~gaveKastorThePassword = true
    setToTrue(gaveKastorThePassword)
    
    changeCamTarget({kastorIndex})
    
    We certainly were lucky that they were able to recruit such a capable member to our cause, then. Not many could have braved the mine and brought Nándor back to us.
    ->6cab

=== 6cab ===
    
    changeCamTarget({kastorIndex})
    
    keepDialogue()
    {playerName}, you are now the lynchpin of the entire plan. Without you, the scattered pieces of our conspiracy cannot communicate. Take care not to bring attention to yourself, or else you will jeopardize everything.
        ->2f


=== 6cb ===

    changeCamTarget({carterIndex})

    Then why didn't you respond with the correct direction when I asked you for the passphrase?
{
-wisdom <= 1:
    +I hope the plan doesn't hinge on me remembering things like passphrases. If it does you should probably rewrite it. <Wis {wisdom}/2>
        
        \*Carter groans.* Alright, fine. We'll talk slow and use small words for you until you get it. Ask questions if we lose you.
        
        ->6cab
}
    +I had only just met you and it seemed like too much of a coincidence I would find you down in the mine like I did.
        ->6cba //then 6cc with plan explanation
    +I wanted to see if I could trust you first. I'm new to the camp and don't know who to put my trust in yet.
        ->6cba //then 6cc with plan explanation

=== 6cba ===

    changeCamTarget({carterIndex})

    I suppose if I wasn't so desperate after being stuck in the mine for so long, I may have thought the same thing. I can't fault you for being overly cautious.
        ->6cab

=== 6d ===

{
-gotThePlanFromKastor and (not toldKastorFinishedErvinsTask or not toldKastorObtainedMineArmoryKey or not toldKastorFinishedErvinsTask):
    changeCamTarget({kastorIndex})
    
    keepDialogue()

    Before we begin, we must finish the rest of the plan. Have you made any more progress on what we've discussed?
    
        ->3c
-not gaveKastorToolBundle and not hasToolBundle:
    changeCamTarget({kastorIndex})
    
    With Nándor back, the only thing we need before we can begin are weapons to supply the other slaves with. Without them, any revolt won't get very far.
    
    +I passed through the mine's armory to get to the third level of the mine. I can go back and loot it for anything that could serve as a weapon.
            ->6da
-else:
    ->7a
}

=== 6da ===

{
-gotThePlanFromKastor:
It would have been better if you had done that the first time, but considering you seem to be able to move through the mine with ease it should be no issue for you to return.
}

changeCamTarget({nandorIndex})
        
I would loath to go back underground, but I know that without those weapons the revolt will falter. I will continue to assist you if you are heading down there again. Kastor, you stay here. If anyone were to come looking for you during the lockdown, they might start asking too many questions if you leave.

        ->deactivateExtras


=== 7aa ===

changeCamTarget({kastorIndex})

It looks like everything is finished. We are ready to move on to the final part of the plan: starting the revolution.

    ->7a
     
=== 7a ===

setToTrue(kastorReadyToStartRevolt)
~kastorReadyToStartRevolt = true

Are you ready to begin the revolt? After we kick things off, we won't be able to turn back.

    +I'm ready. Let us finally start this thing.
            ->7ac
    +I just thought of something I must take care of. I will be back shortly.
        The longer you take, the less likely we are to catch the guards by surprise. Prepare as you need, but hurry!
        ->deactivateExtras

=== 7ab ===

Are you ready to begin the revolt? After we kick things off, we won't be able to turn back.

    +I'm ready. Let us finally start this thing.
        ->7ac
    +I just thought of something I must take care of. I will be back shortly.
        The longer you take, the less likely we are to catch the guards by surprise. Prepare as you need, but hurry!
        ->Close

=== 7ac ===
        fadeToBlack()

        hideTrain()
        
        setToTrue(broughtNandorToKastor)
        ~broughtNandorToKastor = true
        
        activate({nandorIndex})
        activate({carterIndex})
        
        {
        -not deathFlagGuardMárcos:
            activate({marcosIndex})
        }
        
        fadeBackIn(60)
            
        changeCamTarget({nandorIndex})
->7b

=== 7b ===

The next stage of the plan for the revolt was to fight our way to the northern section of the camp, kill the overseer holding the slaves there, and then use our combined numbers to move on the Manse itself.

{
-not terrifiedImre and not convincedImre:
    +Why do we need to assault the Manse at all? The Manse slaves never agreed to join us. Couldn't we just leave once the branded are freed?
        ->7ba
-else:
    +What's the point of assaulting the Manse? Once we take the gates and free the branded couldn't we just leave?
        ->7bb
}

    +I'm ready to take the fight to the guards. What can you tell me of their tactics?
    
    {
    -learnedCartersIdentity:
        ->7c
    -else:
    changeCamTarget({carterIndex})
    
    Actually, before we go any further, I have something I need to share. I've already told Nándor and Márcos about this while we were stuck in the mine, but since I trust everyone here I will explain it to you as well.
    
        ~learnedCartersIdentity = true
        ~aboutToStartRevolt = true
        setToTrue(learnedCartersIdentity)
        keepDialogue()
        
        You know me as Carter, the Confederation serf branded for horse-smuggling, but that is a fabrication. Instead, I am Carter, agent of the Kingdom of Masons. It was reported to Masonic Command by another agent that the Lovashi were operating a mining camp within our borders without our consent. I was sent here to link up with that agent and assist in discovering why. Everything was going smoothly, too, until the lockdown occured. 
    
        ->2ic
    }
=== 7ba ===

changeCamTarget({kastorIndex})

activateQuestStep(Rescue Broglin,1)

That is true, but the Manse is almost certainly where the Pit and it's prisoners reside. I would not leave Broglin and any other slaves held there to whatever fate the guards have in store for them.

->7bc

=== 7bb ===

changeCamTarget({kastorIndex})

activateQuestStep(Rescue Broglin,1)

We could, but the Manse is almost certainly where the Pit and it's prisoners reside. We also swore to help free the Manse slaves. We owe it to anyone trapped there to at least attempt to free them.

->7bc

=== 7bc ===

{
-learnedCartersIdentity:
changeCamTarget({carterIndex})

Also my contact, Page, is probably still stuck within the Manse. Connecting with her is a priority if we want to learn why the Lovashi built this camp. And while she has proven many times she can handle herself, it is my duty as a fellow agent to aid her in any way I can.

}

changeCamTarget({nandorIndex})

keepDialogue()

And on more practical grounds, the Manse contains most of the camp's food stores. I don't want to free ourselves, and then get stuck feeding the rest of the slaves on whatever we can forage or hunt from the forest while we try to make it to civilization.

->7b

=== 7c ===

When we leave here and rally the slaves on the southern side of the camp, the guards will deploy in force to stop us. They may erect barricades to slow or channel us towards areas it's easier to deal with us. 

They will also have guards trained to signal the arrow towers spread around the camp. Killing these guards quickly should be our highest priority, to lower our casualties.

changeCamTarget({carterIndex})

The slaves we free will rally around you, but it won't be easy to direct the mob in combat. Try to keep them alive and they will clear the path for you. Fail to protect them, and we may find ourselves fighting alone.

I would also pick your battles. Not every guard needs to die in the actual riot to achieve victory. Once we take the Manse and defeat the Director, the rest of the guards will know that the battle is lost.

    +Very well. Anything else?
        ->7d

=== 7d ===

changeCamTarget({nandorIndex})

When I was coming up with the plan, I thought myself something of a leader to the other workers. But you have been the one to put everything in motion while I was trapped beneath the surface. On the precipice of battle, let there be no confusion: you have earned my respect, and I will follow your lead during the revolt to whatever end it brings us.

    +Good, two leaders would only cause disruption when trying to direct the other branded.
    setToTrue(agreedToBeLeader)
        ->7f
    +Kastor said you would be instrumental in rallying the other slaves to our cause. They will need a familiar face to put their faith in, not a newcomer's.
        ->7db
    +Thank you, Nándor. You understate your part in what we've accomplished, but I am ready to lead the branded to freedom.
    setToTrue(agreedToBeLeader)
        ->7f
    +A position I do not want. I am no leader.
        ->7da

=== 7da ===

You may think that, but from what I have seen of you I beg to differ. I also broach this topic now because we are about to rally the other slaves and convince them to fight the guards with us. Having agreed upon a single figurehead to lead the revolution would make addressing the crowd easier.

    +I do not care for the trials of leadership, or even the glory. You may address the crowd in my place.
        changeCamTarget({nandorIndex})
        
        Very well, I will speak when we address the slaves.
        ->7f
    +I see. In that case I will address the crowd myself.
        setToTrue(agreedToBeLeader)
        ->7f

=== 7db ===

I will be there when the time comes but when it comes to combat, I am no tactician. A compromise then, I can serve as the face of the revolution when we address the other slaves if you continue to direct us in battle.

    +Agreed. I would much rather leave the big speeches to someone else.
        changeCamTarget({nandorIndex})
        
        Very well, I will speak when we address the slaves.
        ->7f
    +I will take the burden of speaking to the other slaves. I would rather be introduced to them in such a way.
        setToTrue(agreedToBeLeader)
        ->7f
        
=== 7f ===
changeCamTarget({carterIndex})

With that settled, we're about to charge out there and take it to the guards. Some of us at this gathering may not make it to the next one. If I die before the lot of you, I swear to sing your praises at my hearth before all the Gods. They deserve to hear of the heroes they are about to meet.
    
    +Thank you, Carter, but I intend to live through this, and so should all of you. Let us go. 
        ->revolutionStart
    +Enough talk. My weapon hand itches.
        ->revolutionStart
        
=== revolutionStart ===

{
-convincedImre or terrifiedImre:
setToTrue(imreReadyToHelpPlayer)
activateQuestStep(Assist the Nonbranded,2)
}

setToTrue(kastorStartedRevolt)
activateQuestStep(The Plan,12)

setAreaToHostile({southEasternCampSceneName})
setAreaToHostile({manseKitchensSceneName})
setAreaToHostile({manseStockroomSceneName})

~restAfterDeactivatingExtras = true
    ->deactivateExtras

=== 8a ===

I'm not much of a fighter, but I've tended my fair share of wounds from the mines. I will stay behind and see to any wounded that make their way back here, including yourself if you need some rest.

    +I could use some healing.
        Certainly.
        
        restParty()
        ->Close
    +How is Márcos doing?
        keepDialogue()
        His wounds are substantial, and all the blood he's lost has left him immensely fatigued. He's sleeping now; I'll look after him until he can move around under his own power.
        ->8a
    +I've got to go.
        ->Close

=== function playerFinishedErvinsTask ===

~return (finishedErvinsTask or convincedImre or terrifiedImre or imreWontSpeakToPlayer or deathFlagImre)

=== deactivateExtras ===

{
-formationFlagNándor or formationFlagCarter or restAfterDeactivatingExtras:
    fadeToBlack(false)
}

{
-formationFlagNándor:
deactivate({nandorIndex})
}

{
-formationFlagCarter:
deactivate({carterIndex})
}

{
-restAfterDeactivatingExtras:
    ~restAfterDeactivatingExtras = false
    restParty()
}

fadeBackIn(60,false)

->Close

=== Close ===

close()

->DONE