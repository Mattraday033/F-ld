using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Ink.Runtime;
using Newtonsoft.Json;

public static class Flags 
{

	private static Dictionary<string, bool> newGameFlags;
	private static Dictionary<string, bool> flags = new Dictionary<string, bool>();



    static Flags()
    {
        flags[FlagNameList.newGameFlagName] = true; //is a new game 
        // flags["skipFirstDialogue"] = false; //Set to true if you want to skip the dialogue with broglin and co at the beginning. Only used for testing		
        // flags["finishedFirstDialogue"] = false; //if the player finished the first dialogue with broglin/garcha normally

        // //Tutorial Flags
        // flags["seenCombatTutorial"] = false; //Used to check if the Combat Tutorial should appear at start of combat
        // flags["seenHostilityTutorial"] = false; //Used to check if the Hostility Tutorial should appear in the mine or 2nd floor of barracks
        // flags["seenQuestTutorial"] = false; //Used to check if the player saw the Journal/Quest Tutorial
        // flags["seenAbilityWheelTutorial"] = false; //Used to check if the Ability Wheel Tutorial should appear at after first level up

        // //Tutorial Pack Flags
        // flags["secondTutorialBatPackDefeated"] = false;

        // //Tutorial Sequence Flags
        // flags[TutorialSequenceList.skipThatchShackTutorialsFlag] = false;
        // flags[TutorialSequenceList.firstHostitilityTutorialSeenFlag] = false;
        // flags[TutorialSequenceList.intimidateTutorialSeenFlag] = false;
        // flags[TutorialSequenceList.cunningTutorialSeenFlag] = false;
        // flags[TutorialSequenceList.secondCunningTutorialSeenFlag] = false;
        // flags[TutorialSequenceList.observationTutorialSeenFlag] = false;
        // flags[TutorialSequenceList.leadershipTutorialSeenFlag] = false;
        // flags[TutorialSequenceList.interactableObjectTutorialSeenFlag] = false;
        // flags[TutorialSequenceList.hiddenObjectsTutorialSeenFlag] = false;
        // flags[TutorialSequenceList.combatTutorialSeenFlag] = false;
        // flags[TutorialSequenceList.movableObjectTutorialSeenFlag] = false;
        // flags[TutorialSequenceList.playerLevelUpTutorialSeenFlag] = false;

        // //Pop Up Tutorial Sequence Flags
        // flags[TutorialSequenceList.equippableItemTutorialSeenFlag] = false;
        // flags[TutorialSequenceList.formationPopUpTutorialSeenFlag] = false;
        // flags[TutorialSequenceList.addingAbilitiesTutorialSeenFlag] = false;
        // flags[TutorialSequenceList.traitTutorialSeenFlag] = false;
        // flags[TutorialSequenceList.questCounterTutorialSeenFlag] = false;
        // flags[TutorialSequenceList.partyMemberUpgradeTutorialSeenFlag] = false;

        // //Tutorial Quest Flags
        // flags["choseStrengthAtStart"] = false; //Used to determine which tutorial quest objects spawn in slaveshack6
        // flags["choseDexterityAtStart"] = false; //Used to determine which tutorial quest objects spawn in slaveshack6
        // flags["choseWisdomAtStart"] = false; //Used to determine which tutorial quest objects spawn in slaveshack6
        // flags["choseCharismaAtStart"] = false; //Used to determine which tutorial quest objects spawn in slaveshack6
        // flags["givenTutorialQuest"] = false; //given tutorial quest by Kastor, can talk to Thatch now
        // flags["thatchRemovedTutorialRubble"] = false;
        // flags["wisdomTutorialWallsFound"] = false;
        // flags["foundSlate"] = false; //Got past guards in thatch's hut, not need to make it back to Kastor
        // flags["toldKastorOfThatchsFate"] = false; //Finished Tutorial quest

        // //first convo flags
        // flags["goesWithBroglinsPlan"] = false; //if player chose to go along with broglin's plan in the first conversation
        // flags["gotBroglinKilledByGuard"] = false; // if the player got Broglin killed by guard.
        // flags["impressedGuardLaszlo"] = false;// if the player impressed Guard László
        // flags["rattedOutBroglin"] = false; // if the player ratted out broglin's plan to the guards
        // flags["sentToThePit"] = false;//if the player was sent to the pit in the first conversation
        // flags["killedBroglinAndGarcha"] = false; //if the player killed Broglin and Garcha in the first conversation
        // flags["gotPasswordFromGarcha"] = false; //if Garcha told the player about the "which way is the wind blowing/east" call and response
        // flags["killedGuardLaszlo"] = false; //if the player killed Guard László in the first conversation
        // flags["spokeToGarchaAboutPlan"] = false; //if the player has spoken to Garcha after the first conversation
        // flags["givenTaskByLaszlo"] = false; //if Guard László has told you to get Sampson his rations.

        // //kastor convo flags
        // flags["metKastor"] = false; //if the player has spoken to Kastor before
        // flags["askedKastorWhoHeIs"] = false; //if the player asks kastor's name
        // flags["gaveKastorYourName"] = false; //if the player gave their name to Kastor
        // flags["askedKastorToHelpEscape"] = false; //if the player asks Kastor to help him escape before giving the password
        // flags["gaveKastorThePassword"] = false; //gave kastor the password
        // flags["knowsAboutTheMine"] = false; //if the player learns what happened to the mine
        // flags["askedAboutGuardMarcos"] = false; //if the player asks what happened to guard marcos
        // flags["gotThePlanFromKastor"] = false; //if the player got the plan from kastor
        // flags["toldToAidJanos"] = false; //if player doesn't have key when getting plan from kastor, so is told to get it from Janos
        // flags["toldKastorFinishedErvinsTask"] = false; //true when explained to kastor what happened with Ervin/Imre
        // flags["toldKastorFinishedBalintsTask"] = false; //true when explained to kastor what happened with Balint
        // flags["toldKastorObtainedMineArmoryKey"] = false; //true when explained to kastor what happened with Janos/Andras
        // flags["toldKastorErvinIsDead"] = false; //true when explained to kastor that Ervin is dead
        // flags["toldKastorBalintIsDead"] = false; //true when explained to kastor that Balint is dead
        // flags["toldKastorJanosIsDead"] = false; //true when explained to kastor that Janos is dead
        // flags["saidJanosIsATraitor"] = false; //explicitly tell Kastor Janos is a traitor when explaining what happened with Guard Andras.
        // flags["learnedAboutMuszasSweetToothFromKastor"] = false; //needed to be told about M�zsa's sweet tooth from Kastor because you gave her a bad reason to enter the mine
        // flags["toldToFindNandor"] = false; //if you've been told to find Nandor by kastor 
        // flags["toldToFindTools"] = false; //If kastor has told you to find the tools in the armory
        // flags["gaveKastorToolBundle"] = false; //if you've given Kastor the tools from the armory
        // flags["broughtNandorToKastor"] = false; //if you've brought Nandor to Kastor after being told to
        // flags["kastorReadyToStartRevolt"] = false; //if Kastor is ready to move the plan to the actual revolt
        // flags["agreedToBeLeader"] = false; //told Nandor you would lead the revolt
        // flags["kastorStartedRevolt"] = false; //kastor has started the revolt
        // flags["convincedSlavesToHelpYou"] = false; // convinced slaves to help you during convo after killing slavedriver in CampNE
        // flags["acceptingGuardPrisoners"] = false; // accepted Janos's call to accept prisoners during convo after killing slavedriver in CampNE
        // flags["notAcceptingGuardPrisoners"] = false; // rejected Janos's call to accept prisoners during convo after killing slavedriver in CampNE
        // flags["imreReadyToHelpPlayer"] = false;
        // flags["kastorExplainedWhereToFindAnotherKey"] = false; //kastor told player about the guardhouse armory key

        // //Revolt flags
        // flags["spawnWormsInsteadOfGuards"] = false; //if the revolt has started and you opened the way to the third level of the mine, but didn't seal the breach

        // //Nándor Flags
        // flags["spokeWithNandorAfterPrisoners"] = false; //finished prisoner punishments and ready to leave camp
        // flags["nandorLeftParty"] = false; //main left party flag
        // flags["nandorLeftPartyOverPrisonerPunishment"] = false; //gave a guard to the crowd and couldn't keep Nandor from leaving
        // flags["orderedTheHorsesBurned"] = false;
        // flags["orderedTheHorsesEaten"] = false;
        // flags["celebratedWithTheHorseMeat"] = false;
        // flags["hoardedTheHorseMeat"] = false;
        // flags["sharedTheHorseMeat"] = false;
        // flags["canSpeakToKastorAboutFoodShortage"] = false;
        // flags["spokeToKastorAboutFoodShortage"] = false;
        // flags["kastorDiscussingFoodShortage"] = false;
        // flags["letNandorDecideGuardPunishments"] = false;
        // flags["afterNandorDecidesGuardPunishments"] = false;
        // flags["executedAllPrisoners"] = false;


        // //Temple Flags
        // flags["metTemple"] = false; //if the player met Temple
        // flags["templeMentionedBackground"] = false;
        // flags["templeExplainedPatches"] = false;
        // flags["askedTempleAboutSampson"] = false;

        // //Tabor Flags
        // flags["heardTaborsLesson"] = false;
        // flags["letTaborLive"] = false; //walked into Tabor's room and didn't kill him
        // flags["killedTaborInManse"] = false; //killed Tabor during conversation in Manse
        // flags["acceptedTaborsSurrenderAfterDirectorFight"] = false; //went back to get Tabor after defeating the Director and accepted his surrender

        // //seb flags
        // flags["spokeToSeb"] = false; //if the player has spoken to Seb.
        // flags["gaveNoteToSeb"] = false; //if the player has given Clay's Note to Seb's presence

        // //balint flags
        // flags["spokeToBalint"] = false; //if the player has spoken to Balint
        // flags["knowBalintIsFromCarnassus"] = false; //if balint has explained he is from Carnassus
        // flags["gaveBalintThePassword"] = false; //if you've talked to balint about the escape plan
        // flags["givenTaskByBalint"] = false; //been tasked with gather leaves by balint
        // flags["gotLeavesForBalint"] = false; //have leaves to give to balint in inventory
        // flags["finishedBalintsTask"] = false; //handed leaves to balint so he can examine them. player is told to report back to Kastor
        // flags["givenAdviceFromBalint"] = false; //handed leaves to balint so he can examine them. player is told to report back to Kastor

        // //Janos flags
        // flags["spokeToJanos"] = false; //has met Janos.
        // flags["refusedToWorkWithJanos"] = false; //Refused to work with Janos. Different from saying you need more time before waiting with Janos 
        // flags["canWaitWithJanos"] = false; //said they needed more time before waiting with Janos
        // flags["declaredAndrasMustDie"] = false; //told Janos that you are going to kill Guard Andras
        // flags["intimidatedAndras"] = false; //intimidated Andras
        // flags["andrasAttackedPlayer"] = false; //andras struck first
        // flags["andrasIsDead"] = false; //killed andras 
        // flags["janosIsCrying"] = false; //janos is crying
        // flags["obtainedMineArmoryKey"] = false; //got key for the Mine Armory
        // flags["andrasLeftInHut"] = false; //andras should appear in janos's hut
        // flags["gotKeyFromJanos"] = false; //janos specifically gave the key
        // flags["janosExplainedHowHeMetAndras"] = false;
        // flags["andrasAgreedToBeBranded"] = false;
        // flags["andrasAgreedToMonthSeparation"] = false;
        // flags["andrasSworeGodOath"] = false;

        // //Ervin flags
        // flags["spokeToErvin"] = false; //have spoken to Ervin before
        // flags["gavePasswordToErvin"] = false; //gave password to Ervin to get his quest
        // flags["askedErvinAboutBrand"] = false; //asked Ervin about his brand
        // flags["givenTaskByErvin"] = false; //Ervin told player who to talk to.
        // flags["finishedErvinsTask"] = false; //completed talking to imre
        // flags["allowedErvinToSpeakAtPazmansTrial"] = false;

        // //Imre flags
        // flags["imreWontSpeakToPlayer"] = false; //pissed off imre
        // flags["terrifiedImre"] = false; //chose scaring imre path
        // flags["convincedImre"] = false; //imre will follow the plan
        // flags["spokenToLoyalImre"] = false; //had a convo with Imre after killing kende
        // flags["askedImreToLeadTheWay"] = false; //spoke with Imre during revolt b4 entering the kitchens
        // flags["toldImreNeededToRest"] = false; //spoke with Imre during revolt but needed to leave
        // flags["foughtKendeInManseKitchen"] = false; //finished dialogue with Imre/Kende in kitchen

        // //Clay flags
        // flags["metClay"] = false;   //have interacted with Clay
        // flags["clayExplainedCrime"] = false; //Clay said what Thatch saw.
        // flags["clayExplainedReward"] = false; //Clay explained that he would give a knife as a reward
        // flags["clayExplainedJob"] = false; //Clay explained the job was to kill Thatch
        // flags["gotKnifeFromClay"] = false; //completed first job for Clay
        // flags["toldClaySpokeToSeb"] = false; //told clay you spoke to Seb
        // flags["failedToConvinceClay"] = false; //clay wasn't convinced to follow you into battle

        // //Thatch/Slate/Vazul flags
        // flags["metThatch"] = false;
        // flags["saidKilledForLessThanChew"] = false;
        // flags["vazulMentionedSlatesFate"] = false;
        // flags["toldThatchAboutSlate"] = false;
        // flags["thatchBeginsStranglingVazul"] = false;

        // //Broglin Flags
        // flags["toldAboutCellKey"] = false; //Broglin has explained where the cell key is
        // flags["freedBroglin"] = false; //Broglin has been freed from the pit

        // //Muzsa flags
        // flags["metMuzsa"] = false;  //have interacted with Guard Muzsa before
        // flags["givenTaskByMuzsa"] = false;  //told to buy candy from Kende
        // flags["turnedDownMuzsasTask"] = false;  //Said you didn't want to buy Muzsa candy
        // flags["mineCratesCleared"] = false; //Said you didn't want to buy Muzsa candy
        // flags["mentionedBadReasonForGoingInsideMine"] = false; //gave bad reason for going inside the mine.
        // flags["gaveCandyToMuzsa"] = false; //Finished Muzsa's Sweet Tooth by handing her candy.
        // flags["gaveSnipeHuntExcuseToMuzsa"] = false;

        // //Kende the Cook flags
        // flags["gotMessHallInstructionsFromKende"] = false; //spoke with Kende
        // flags["turnedDownMuzsasTask"] = false;  //Said you didn't want to buy Muzsa candy
        // flags["knowsAboutKendesShop"] = false; //(unimplemented) Learned about Kende's shop from someone besides him or Muzsa (like Clay). Enables dialogue to ask about shop, but not shop itself
        // flags["kendeWillSellToPlayer"] = false; //Kende the cook has the option to sell items to the player
        // flags["askedKendeWhoHeIs"] = false; //chose the dialogue option about asking kende who he was


        // //Quartermaster Emese flags
        // flags["metQuartermasterEmese"] = false; //have interacted with Emese before
        // flags["gaveIronNuggetToEmese"] = false;

        // //Uros flags
        // flags["startledUros"] = false; //You spoke to Uros so he's no longer talking about looking for something.
        // flags["convincedUros"] = false; //convinced Uros to tell you about what he has hidden away.
        // flags["intimidatedUros"] = false; //intimidated Uros into telling you about what he has hidden away.
        // flags["threatenedToSnitchOnUros"] = false; //Uros told you about what he has hidden because you told him you'd snitch on him but didn't end up snitching.
        // flags["snitchedOnUros"] = false; //Told Quartermaster Emese about Uros hiding something in the stockhouse
        // flags["gaveUrosTheNugget"] = false; //finished Uros's quest by giving his nugget back.
        // flags["showedUrosTheNuggetWithoutGivingItBack"] = false; //finished Uros's quest by showing him the nugget and then not giving it back to him.
        // flags["gotIronNuggetFromBarrels"] = false; //whether you've picked up the iron nugget from behind the barrels yet in the stockhouse
        // flags["spokeToUrosShop"] = false;
        // flags["urosBadAttitude"] = false;
        // flags["urosGoodAttitude"] = false;
        // flags["urosWorstPrices"] = false;
        // flags["urosBadPrices"] = false;
        // flags["urosNormalPrices"] = false;
        // flags["urosBestPrices"] = false;

        // //Carter flags
        // flags["toldCarterPassword"] = false; // gave Carter the password on first meeting him 
        // flags["toldCarterWrongPassword"] = false; //If you said anything but 'East' to Carter when asked the password and you got the call/response from Garcha
        // flags["learnedCartersIdentity"] = false; //Told about where Carter comes from and his mission in the camp 
        // flags["learnedPagesIdentity"] = false; //Told about where Page comes from and her mission in the camp 
        // flags["toldDirectorIsAWarHero"] = false; //Told capturing the Director may start a war. Prompts a question about that later when talking to Carter
        // flags["learnedCampLocationFromCarter"] = false; //Carter was the one who told the player where the camp is located
        // flags["carterSaidBrandedWouldBeTreatedLikeGuests"] = false; //Player read dialogue where Carter said the branded would be treated like guests by the Masons

        // //Camp Flags
        // flags["campSEHiddenPassageFound1"] = false; //You opened the right passage between 6SlaveShack and CampSE
        // flags["campSEHiddenPassageFound2"] = false; //You opened the passage left between 6SlaveShack and CampSE
        // flags["campSEHiddenPassageFound3"] = false; //You opened the passage between 5SlaveShack and CampSE
        // flags["campCenterFirstHiddenPassageFound"] = false; //You opened the passage between the Stables and CampCenter
        // flags["campCenterSecondHiddenPassageFound"] = false; //You opened the passage between the Temple and CampCenter

        // //Minelvl2 flags
        // flags["mineLvl2BrokenGateLifted"] = false; //if you've passed the strength check to lift the gate on the east side of Mine Lvl 2
        // flags[BookList.mineGuardsJournalReadFlag] = false; //if you've read about the secret passage on mine lvl 2
        // flags["mineLvl2HiddenPassageFound"] = false; //if you've opened the secret doors in the mine lvl 2
        // flags["mineLvl2ArmoryGateOpened"] = false; //if you've opened the armory gate in mine lvl 2
        // flags["mineLvl2GateToLevel3Opened"] = false; //if you've opened the gate to mine lvl 3
        // flags["mineLvl2HiddenDoor7b"] = false; //if you've opened the wisdom wall in 7b
        // flags["mineLvl2GuardsMovedToSecondLevelGate"] = false; //if you got the guards to move to the gate to the second level
        // flags["mineLvl2GuardsFinishedMove"] = false; //used to tell GuardsCrate.ink to not deactivate guards

        // //Minelvl3 flags
        // flags["mineLvl3ClearedCratesToMiners"] = false; //if you've had the Miners clear the crates to their room
        // flags["mineLvl3CarterAndNandorInParty"] = false; //if you've met added Carter and Nandor to the party after meeting them near Guard Marcos
        // flags["mineLvl3ToldAboutJelly"] = false; //Nandor has explained that you need to seal the breech in the mines third level to keep the worms from overwhelming the camp
        // flags["mineLvl3ClearedCratesToGuards"] = false; //if you've had the Guards clear the crates to their room
        // flags["mineLvl3SpeakingFromBrokenGate"] = false; //Lifted the broken gate that leads to the guards
        // flags["mineLvl3SpeakingFromGuardCrates"] = false; //Speaking from crate side of guards
        // flags["mineLvl3MetGaspar"] = false; //if you've met Gaspar
        // flags["mineLvl3KilledGuards"] = false;//if you've killed the guards on mine lvl 3 for any reason
        // flags["mineLvl3GuardsInParty"] = false;//if you've convinced or coerced the guards to join your party
        // flags["mineLvl3-1bHiddenPassageFound"] = false;
        // flags["mineLvl3MarcosAgreedToIgniteJelly"] = false;
        // flags["mineLvl3MarcosTaughtHowToIgniteJelly"] = false;
        // flags["mineLvl3MarcosDiedSealingBreach"] = false;
        // flags["mineLvl3BreachSealed"] = false;
        // flags["mineLvl3RefusedToFightGaspar"] = false;
        // flags["mineLvl3DealtWithGaspar"] = false; //if you've killed gaspar and co after sealing the breach 
        // flags["mineLvl3ToldPazmanToEatShit"] = false;
        // flags["mineLvl3GuardsBackToSurface"] = false;
        // flags["mineLvl3SlavesBackToSurface"] = false;
        // flags["mineLvl3ToldToFindMarcos"] = false;
        // flags["mineLvl3ThreatenedGaspar"] = false; //took the strength dialogue option when convincing Overseer Gaspar to help you in the mine
        // flags["mineLvl3ConvincedRekaAndPazman"] = false; //Convinced Guards Reka and Pazman to lay down their weapons
        // flags["mineLvl3PromisedToProtectRekaAndPazman"] = false; //Threatened Guard Reka and Pazman when taking them prisoner
        // flags["mineLvl3ThreatenedRekaAndPazmanAsPrisoners"] = false; //Threatened Guard Reka and Pazman when taking them prisoner 

        // //Manse Flags
        // flags["manseDiningRoomPermButtonScriptActivated"] = false;
        // flags["manse1FLibraryHiddenRoomFound"] = false;
        // flags["manseOfficeSecretDoorsOpened"] = false;
        // flags["manseMeetingRoomSecretDoorsOpened"] = false;
        // flags["manseHiddenStairsFound"] = false;
        // flags["unlockedThePitGate"] = false;
        // flags["manseDoorsOpenedRevolt"] = false;
        // flags[BookList.pageFirstDiaryEntryReadFlag] = false;
        // flags[BookList.pageSecondDiaryEntryReadFlag] = false;
        // flags[BookList.ordersTranscriptReadFlag] = false;
        // flags[BookList.pitSecondEntranceNoteReadFlag] = false;

        // //Pit Flags
        // flags[BookList.pitClosureNoteReadFlag] = false;
        // flags[EnteredPit1b.enteredPit1bFlag] = false;
        // flags[EnteredPit2a.enteredPit2aFlag] = false;

        // //Director Convo
        // flags["directorDefeated"] = false;
        // flags["allowedDirectorToSpeak"] = false;
        // flags["directorSaidAbsurdityQuote"] = false;
        // flags["directorConvoFinished"] = false;
        // flags["keptDirectorAlive"] = false;

        // //Page flags
        // flags["toldPageRevengeFeltFantastic"] = false;
        // flags["spokeWithPageBeforePrisoners"] = false; //if you speak to page at the center of camp after director is defeated but b4 dealing with the prisoners at the mess hall
        // flags["pageSaidReadyToLeave"] = false; //disables debris at the gate after speaking with page

        // //boss kill flags
        // flags["mineLvl2BatBossKilled"] = false;
        // flags["mineLvl2WormStrBossKilled"] = false;
        // flags["mineLvl3WormFinalBossKilled"] = false;
        // flags["neCampOverseerKilled"] = false;
        // flags["killedChiefIren"] = false;

        // //puzzle flags
        // flags["wisdomPuzzleMineLvl3Completed"] = false;
        // flags["wisdomPuzzleMineLvl3TwoObjects"] = false;
        // flags["wisdomPuzzleMineLvl3ThreeObjects"] = false;
        // flags["wisdomPuzzleMineLvl3FourObjects"] = false;
        // flags["wisdomPuzzleMineLvl3FiveObjects"] = false;

        // flags["prevWTRoundRubble"] = false;
        // flags["prevWTSingleStalagmite"] = false;
        // flags["prevWTTripleStalagmite"] = false;
        // flags["prevWTBushRock"] = false;

        // //Camp Barricade Pass Flags
        // flags["wisdomBarricadePassUsed"] = false;
        // flags["strengthBarricadePassUsed"] = false;
        // flags["charismaBarricadePassUsed"] = false;
        // flags["andrasBarricadePassUsed"] = false;

        // //beam flags
        // flags["metBeam"] = false;
        // flags["askedBeamAboutWhittling"] = false;
        // flags["pissedOffBeam"] = false;
        // flags["givenHorsetongueGuideFromBeam"] = false;
        // flags["askedAboutMangledName"] = false;
        // flags["sworeToBurnCsalansBody"] = false;
        // flags["foughtHorsesInManse"] = false;

        // //Csalan Oath Flags
        // flags["csalanLifeOathMade"] = false;
        // flags["csalanSufferingOathMade"] = false;
        // flags["csalanStainedNameOathMade"] = false;
        // flags["csalanFamilyOathMade"] = false;
        // flags["csalanHomeOathMade"] = false;
        // flags["csalanWealthOathMade"] = false;
        // flags["csalanSanityOathMade"] = false;


        // //Guard Punishment Flags
        // flags["noPrisoners"] = false;

        // flags["spokeWithMarcosAtPunishment"] = false;
        // flags["spokeWithAndrasAtPunishment"] = false;
        // flags["spokeWithRekaAtPunishment"] = false;

        // flags["didNotExecuteMarcos"] = false;
        // flags["gaveMarcosToTheCrowd"] = false;
        // flags["gaveMarcosFiftyLashes"] = false;
        // flags["executedMarcos"] = false;

        // flags["didNotExecuteAndras"] = false;
        // flags["gaveAndrasToTheCrowd"] = false;
        // flags["gaveAndrasFiftyLashes"] = false;
        // flags["executedAndras"] = false;

        // flags["didNotExecuteReka"] = false;
        // flags["gaveRekaToTheCrowd"] = false;
        // flags["gaveRekaFiftyLashes"] = false;
        // flags["executedReka"] = false;

        // flags["didNotExecutePazman"] = false;
        // flags["gavePazmanToTheCrowd"] = false;
        // flags["gavePazmanFiftyLashes"] = false;
        // flags["executedPazman"] = false;

        // flags["didNotExecuteTabor"] = false;
        // flags["gaveTaborToTheCrowd"] = false;
        // flags["crowdForcedTaborExecution"] = false;
        // flags["executedTabor"] = false;

        // flags["foughtCrowdForTabor"] = false; //failed to convince Clay and the crowd to let Tabor live, and had to fight them
        // flags["clayWillSeekOutTabor"] = false; //failed to convince Clay, but not the crowd, to let Tabor live, and Clay decides to attack you later

        // flags["crowdDispersed"] = false;

        // //Dialogue Upon Entry Flags
        // flags["kendeUponEnteringKitchens"] = false;
        // flags["taborManse-2F-2B"] = false;

        // //Chest Item Pickup Flags
        // flags["gotArmoryKeyFromGuardHouse"] = false; //picked up the mine armory key from the 2nd floor of the camp guard house

        // //Stop Party Train Spawning Flags
        // flags["disablePartyTrain"] = false;

        // //Entered Area flags (used for advancing time/quests usually)
        // flags["enteredMineLvl1"] = false;
        // flags["enteredMineLvl2"] = false;
        // flags["enteredMineLvl2-2a"] = false;
        // flags["enteredMineLvl3"] = false;
        // flags["enteredMessHallYardAfterRevolt"] = false;
        // flags["enteredCivilizationAfterLeavingCamp"] = false;

        newGameFlags = flags.ToDictionary(entry => entry.Key,
                                          entry => entry.Value);

        //printAll();
    }

	public static bool getFlag(string flagName)
	{
		if (!flags.ContainsKey(flagName))
		{
			flags.Add(flagName, false);
		}
            
        return flags[flagName];
	}

	public static void setFlag(string flagName, bool flagStatus)
	{
		if (!flags.ContainsKey(flagName))
		{
			flags.Add(flagName, flagStatus);
		}
		else
		{
			flags[flagName] = flagStatus;
		}
	}

    public static void printAll()
    {   // Get and display values  
        //Dictionary<string,bool>.KeyCollection keys = flags.Keys;  
        //Dictionary<string,bool>.ValueCollection values = flags.Values;  
        foreach (KeyValuePair<string, bool> kvp in flags)
        {
            Debug.Log(kvp.Key + " is " + kvp.Value);
        }
    }

	public static Story addAllVariables(Story story)
	{
		foreach (KeyValuePair<string, bool> kvp in flags)
		{
			if (story.variablesState[kvp.Key] != null)
			{
				story.variablesState[kvp.Key] = kvp.Value;
			}
		}

		return story;
	}

	public static void overwriteFlags(Dictionary<string, bool> newFlags)
	{
		flags = newGameFlags.ToDictionary(entry => entry.Key,
										  entry => entry.Value);

		if (newFlags == null)
		{
			return;
		}

		foreach (KeyValuePair<string, bool> flag in newFlags)
		{
			flags[flag.Key] = newFlags[flag.Key];
		}
	}
	//assumes string is a json that can be deserialized into a Dictionary<string,bool>();
	public static void overwriteFlags(string newFlags)
	{
		overwriteFlags(JsonConvert.DeserializeObject<Dictionary<string, bool>>(newFlags));
	}

	public static void resetAllFlags()
	{
		resetAllFlags(false);
	}

	public static void resetAllFlags(bool newGame)
	{
		foreach (var key in flags.Keys.ToList())
		{
			flags[key] = false;
		}

		flags["newGame"] = newGame;
	}

	public static string getFlagsForSave()
	{

		return JsonConvert.SerializeObject(flags, Formatting.Indented);

	}

	public static bool isInNewGameMode()
	{
		return flags[FlagNameList.newGameFlagName];
	}

	public static void exitNewGameMode()
	{
		flags[FlagNameList.newGameFlagName] = false;
	}

	public static void stopPartyTrainSpawning()
	{
		flags["disablePartyTrain"] = true;
	}

	public static void allowPartyTrainSpawning()
	{
		flags["disablePartyTrain"] = false;
	}

	public static bool shouldStopPartyTrainSpawning()
	{
		if (flags["disablePartyTrain"])
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public static void setStatTutorialFlag() //only use when starting new game
    {
        PrimaryStat chosenStat = PartyManager.getPlayerStats().getHighestPrimaryStats()[0];

        switch (chosenStat)
        {
            case PrimaryStat.Strength:
		        Flags.flags["choseStrengthAtStart"] = true;
                return;
            case PrimaryStat.Dexterity:
            	Flags.flags["choseDexterityAtStart"] = true;
                return;
            case PrimaryStat.Wisdom:
                Flags.flags["choseWisdomAtStart"] = true;
                return;
            case PrimaryStat.Charisma:
                Flags.flags["choseCharismaAtStart"] = true;
                return;
        }
    }

}
