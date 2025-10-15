using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlagNameList
{
    	public const string newGameFlagName = "newGame";
        public const string skipFirstDialogue = "skipFirstDialogue"; //Set to true if you want to skip the dialogue with broglin and co at the beginning. Only used for testing		
        public const string finishedFirstDialogue = "finishedFirstDialogue"; //if the player finished the first dialogue with broglin/garcha normally

        //Tutorial Flags
        public const string seenCombatTutorial = "seenCombatTutorial"; //Used to check if the Combat Tutorial should appear at start of combat
        public const string seenHostilityTutorial = "seenHostilityTutorial"; //Used to check if the Hostility Tutorial should appear in the mine or 2nd floor of barracks
        public const string seenQuestTutorial = "seenQuestTutorial"; //Used to check if the player saw the Journal/Quest Tutorial
        public const string seenAbilityWheelTutorial = "seenAbilityWheelTutorial"; //Used to check if the Ability Wheel Tutorial should appear at after first level up

        //Tutorial Pack Flags
        public const string secondTutorialBatPackDefeated = "secondTutorialBatPackDefeated";

        //Tutorial Quest Flags
        public const string choseStrengthAtStart = "choseStrengthAtStart"; //Used to determine which tutorial quest objects spawn in slaveshack6
        public const string choseDexterityAtStart = "choseDexterityAtStart"; //Used to determine which tutorial quest objects spawn in slaveshack6
        public const string choseWisdomAtStart = "choseWisdomAtStart"; //Used to determine which tutorial quest objects spawn in slaveshack6
        public const string choseCharismaAtStart = "choseCharismaAtStart"; //Used to determine which tutorial quest objects spawn in slaveshack6
        public const string givenTutorialQuest = "givenTutorialQuest"; //given tutorial quest by Kastor, can talk to Thatch now
        public const string thatchRemovedTutorialRubble = "thatchRemovedTutorialRubble";
        public const string wisdomTutorialWallsFound = "wisdomTutorialWallsFound";
        public const string foundSlate = "foundSlate"; //Got past guards in thatch's hut, not need to make it back to Kastor
        public const string toldKastorOfThatchsFate = "toldKastorOfThatchsFate"; //Finished Tutorial quest

        //first convo flags
        public const string goesWithBroglinsPlan = "goesWithBroglinsPlan"; //if player chose to go along with broglin's plan in the first conversation
        public const string gotBroglinKilledByGuard = "gotBroglinKilledByGuard"; // if the player got Broglin killed by guard.
        public const string impressedGuardLaszlo = "impressedGuardLaszlo";// if the player impressed Guard László
        public const string rattedOutBroglin = "rattedOutBroglin"; // if the player ratted out broglin's plan to the guards
        public const string sentToThePit = "sentToThePit";//if the player was sent to the pit in the first conversation
        public const string killedBroglinAndGarcha = "killedBroglinAndGarcha"; //if the player killed Broglin and Garcha in the first conversation
        public const string gotPasswordFromGarcha = "gotPasswordFromGarcha"; //if Garcha told the player about the "which way is the wind blowing/east" call and response
        public const string killedGuardLaszlo = "killedGuardLaszlo"; //if the player killed Guard László in the first conversation
        public const string spokeToGarchaAboutPlan = "spokeToGarchaAboutPlan"; //if the player has spoken to Garcha after the first conversation
        public const string givenTaskByLaszlo = "givenTaskByLaszlo"; //if Guard László has told you to get Sampson his rations.

        //kastor convo flags
        public const string metKastor = "metKastor"; //if the player has spoken to Kastor before
        public const string askedKastorWhoHeIs = "askedKastorWhoHeIs"; //if the player asks kastor's name
        public const string gaveKastorYourName = "gaveKastorYourName"; //if the player gave their name to Kastor
        public const string askedKastorToHelpEscape = "askedKastorToHelpEscape"; //if the player asks Kastor to help him escape before giving the password
        public const string gaveKastorThePassword = "gaveKastorThePassword"; //gave kastor the password
        public const string knowsAboutTheMine = "knowsAboutTheMine"; //if the player learns what happened to the mine
        public const string askedAboutGuardMarcos = "askedAboutGuardMarcos"; //if the player asks what happened to guard marcos
        public const string gotThePlanFromKastor = "gotThePlanFromKastor"; //if the player got the plan from kastor
        public const string toldToAidJanos = "toldToAidJanos"; //if player doesn't have key when getting plan from kastor, so is told to get it from Janos
        public const string toldKastorFinishedErvinsTask = "toldKastorFinishedErvinsTask"; //true when explained to kastor what happened with Ervin/Imre
        public const string toldKastorFinishedBalintsTask = "toldKastorFinishedBalintsTask"; //true when explained to kastor what happened with Balint
        public const string toldKastorObtainedMineArmoryKey = "toldKastorObtainedMineArmoryKey"; //true when explained to kastor what happened with Janos/Andras
        public const string toldKastorErvinIsDead = "toldKastorErvinIsDead"; //true when explained to kastor that Ervin is dead
        public const string toldKastorBalintIsDead = "toldKastorBalintIsDead"; //true when explained to kastor that Balint is dead
        public const string toldKastorJanosIsDead = "toldKastorJanosIsDead"; //true when explained to kastor that Janos is dead
        public const string saidJanosIsATraitor = "saidJanosIsATraitor"; //explicitly tell Kastor Janos is a traitor when explaining what happened with Guard Andras.
        public const string learnedAboutMuszasSweetToothFromKastor = "learnedAboutMuszasSweetToothFromKastor"; //needed to be told about M�zsa's sweet tooth from Kastor because you gave her a bad reason to enter the mine
        public const string toldToFindNandor = "toldToFindNandor"; //if you've been told to find Nandor by kastor 
        public const string toldToFindTools = "toldToFindTools"; //If kastor has told you to find the tools in the armory
        public const string gaveKastorToolBundle = "gaveKastorToolBundle"; //if you've given Kastor the tools from the armory
        public const string broughtNandorToKastor = "broughtNandorToKastor"; //if you've brought Nandor to Kastor after being told to
        public const string kastorReadyToStartRevolt = "kastorReadyToStartRevolt"; //if Kastor is ready to move the plan to the actual revolt
        public const string agreedToBeLeader = "agreedToBeLeader"; //told Nandor you would lead the revolt
        public const string kastorStartedRevolt = "kastorStartedRevolt"; //kastor has started the revolt
        public const string convincedSlavesToHelpYou = "convincedSlavesToHelpYou"; // convinced slaves to help you during convo after killing slavedriver in CampNE
        public const string acceptingGuardPrisoners = "acceptingGuardPrisoners"; // accepted Janos's call to accept prisoners during convo after killing slavedriver in CampNE
        public const string notAcceptingGuardPrisoners = "notAcceptingGuardPrisoners"; // rejected Janos's call to accept prisoners during convo after killing slavedriver in CampNE
        public const string imreReadyToHelpPlayer = "imreReadyToHelpPlayer";
        public const string kastorExplainedWhereToFindAnotherKey = "kastorExplainedWhereToFindAnotherKey"; //kastor told player about the guardhouse armory key

        //Revolt flags
        public const string spawnWormsInsteadOfGuards = "spawnWormsInsteadOfGuards"; //if the revolt has started and you opened the way to the third level of the mine, but didn't seal the breach

        //Nándor Flags
        public const string spokeWithNandorAfterPrisoners = "spokeWithNandorAfterPrisoners"; //finished prisoner punishments and ready to leave camp
        public const string nandorLeftParty = "nandorLeftParty"; //main left party flag
        public const string nandorLeftPartyOverPrisonerPunishment = "nandorLeftPartyOverPrisonerPunishment"; //gave a guard to the crowd and couldn't keep Nandor from leaving
        public const string orderedTheHorsesBurned = "orderedTheHorsesBurned";
        public const string orderedTheHorsesEaten = "orderedTheHorsesEaten";
        public const string celebratedWithTheHorseMeat = "celebratedWithTheHorseMeat";
        public const string hoardedTheHorseMeat = "hoardedTheHorseMeat";
        public const string sharedTheHorseMeat = "sharedTheHorseMeat";
        public const string canSpeakToKastorAboutFoodShortage = "canSpeakToKastorAboutFoodShortage";
        public const string spokeToKastorAboutFoodShortage = "spokeToKastorAboutFoodShortage";
        public const string kastorDiscussingFoodShortage = "kastorDiscussingFoodShortage";
        public const string letNandorDecideGuardPunishments = "letNandorDecideGuardPunishments";
        public const string afterNandorDecidesGuardPunishments = "afterNandorDecidesGuardPunishments";
        public const string executedAllPrisoners = "executedAllPrisoners";


        //Temple Flags
        public const string metTemple = "metTemple"; //if the player met Temple
        public const string templeMentionedBackground = "templeMentionedBackground";
        public const string templeExplainedPatches = "templeExplainedPatches";
        public const string askedTempleAboutSampson = "askedTempleAboutSampson";

        //Tabor Flags
        public const string heardTaborsLesson = "heardTaborsLesson";
        public const string letTaborLive = "letTaborLive"; //walked into Tabor's room and didn't kill him
        public const string killedTaborInManse = "killedTaborInManse"; //killed Tabor during conversation in Manse
        public const string acceptedTaborsSurrenderAfterDirectorFight = "acceptedTaborsSurrenderAfterDirectorFight"; //went back to get Tabor after defeating the Director and accepted his surrender

        //seb flags
        public const string spokeToSeb = "spokeToSeb"; //if the player has spoken to Seb.
        public const string gaveNoteToSeb = "gaveNoteToSeb"; //if the player has given Clay's Note to Seb's presence

        //balint flags
        public const string spokeToBalint = "spokeToBalint"; //if the player has spoken to Balint
        public const string knowBalintIsFromCarnassus = "knowBalintIsFromCarnassus"; //if balint has explained he is from Carnassus
        public const string gaveBalintThePassword = "gaveBalintThePassword"; //if you've talked to balint about the escape plan
        public const string givenTaskByBalint = "givenTaskByBalint"; //been tasked with gather leaves by balint
        public const string gotLeavesForBalint = "gotLeavesForBalint"; //have leaves to give to balint in inventory
        public const string finishedBalintsTask = "finishedBalintsTask"; //handed leaves to balint so he can examine them. player is told to report back to Kastor
        public const string givenAdviceFromBalint = "givenAdviceFromBalint"; //handed leaves to balint so he can examine them. player is told to report back to Kastor

        //Janos flags
        public const string spokeToJanos = "spokeToJanos"; //has met Janos.
        public const string refusedToWorkWithJanos = "refusedToWorkWithJanos"; //Refused to work with Janos. Different from saying you need more time before waiting with Janos 
        public const string canWaitWithJanos = "canWaitWithJanos"; //said they needed more time before waiting with Janos
        public const string declaredAndrasMustDie = "declaredAndrasMustDie"; //told Janos that you are going to kill Guard Andras
        public const string intimidatedAndras = "intimidatedAndras"; //intimidated Andras
        public const string andrasAttackedPlayer = "andrasAttackedPlayer"; //andras struck first
        public const string andrasIsDead = "andrasIsDead"; //killed andras 
        public const string janosIsCrying = "janosIsCrying"; //janos is crying
        public const string obtainedMineArmoryKey = "obtainedMineArmoryKey"; //got key for the Mine Armory
        public const string andrasLeftInHut = "andrasLeftInHut"; //andras should appear in janos's hut
        public const string gotKeyFromJanos = "gotKeyFromJanos"; //janos specifically gave the key
        public const string janosExplainedHowHeMetAndras = "janosExplainedHowHeMetAndras";
        public const string andrasAgreedToBeBranded = "andrasAgreedToBeBranded";
        public const string andrasAgreedToMonthSeparation = "andrasAgreedToMonthSeparation";
        public const string andrasSworeGodOath = "andrasSworeGodOath";

        //Ervin flags
        public const string spokeToErvin = "spokeToErvin"; //have spoken to Ervin before
        public const string gavePasswordToErvin = "gavePasswordToErvin"; //gave password to Ervin to get his quest
        public const string askedErvinAboutBrand = "askedErvinAboutBrand"; //asked Ervin about his brand
        public const string givenTaskByErvin = "givenTaskByErvin"; //Ervin told player who to talk to.
        public const string finishedErvinsTask = "finishedErvinsTask"; //completed talking to imre
        public const string allowedErvinToSpeakAtPazmansTrial = "allowedErvinToSpeakAtPazmansTrial";

        //Imre flags
        public const string imreWontSpeakToPlayer = "imreWontSpeakToPlayer"; //pissed off imre
        public const string terrifiedImre = "terrifiedImre"; //chose scaring imre path
        public const string convincedImre = "convincedImre"; //imre will follow the plan
        public const string spokenToLoyalImre = "spokenToLoyalImre"; //had a convo with Imre after killing kende
        public const string askedImreToLeadTheWay = "askedImreToLeadTheWay"; //spoke with Imre during revolt b4 entering the kitchens
        public const string toldImreNeededToRest = "toldImreNeededToRest"; //spoke with Imre during revolt but needed to leave
        public const string foughtKendeInManseKitchen = "foughtKendeInManseKitchen"; //finished dialogue with Imre/Kende in kitchen

        //Clay flags
        public const string metClay = "metClay";   //have interacted with Clay
        public const string clayExplainedCrime = "clayExplainedCrime"; //Clay said what Thatch saw.
        public const string clayExplainedReward = "clayExplainedReward"; //Clay explained that he would give a knife as a reward
        public const string clayExplainedJob = "clayExplainedJob"; //Clay explained the job was to kill Thatch
        public const string gotKnifeFromClay = "gotKnifeFromClay"; //completed first job for Clay
        public const string toldClaySpokeToSeb = "toldClaySpokeToSeb"; //told clay you spoke to Seb
        public const string failedToConvinceClay = "failedToConvinceClay"; //clay wasn't convinced to follow you into battle

        //Thatch/Slate/Vazul flags
        public const string metThatch = "metThatch";
        public const string saidKilledForLessThanChew = "saidKilledForLessThanChew";
        public const string vazulMentionedSlatesFate = "vazulMentionedSlatesFate";
        public const string toldThatchAboutSlate = "toldThatchAboutSlate";
        public const string thatchBeginsStranglingVazul = "thatchBeginsStranglingVazul";

        //Broglin Flags
        public const string toldAboutCellKey = "toldAboutCellKey"; //Broglin has explained where the cell key is
        public const string freedBroglin = "freedBroglin"; //Broglin has been freed from the pit

        //Muzsa flags
        public const string metMuzsa = "metMuzsa";  //have interacted with Guard Muzsa before
        public const string givenTaskByMuzsa = "givenTaskByMuzsa";  //told to buy candy from Kende
        public const string turnedDownMuzsasTask = "turnedDownMuzsasTask";  //Said you didn't want to buy Muzsa candy
        public const string mineCratesCleared = "mineCratesCleared"; //Said you didn't want to buy Muzsa candy
        public const string mentionedBadReasonForGoingInsideMine = "mentionedBadReasonForGoingInsideMine"; //gave bad reason for going inside the mine.
        public const string gaveCandyToMuzsa = "gaveCandyToMuzsa"; //Finished Muzsa's Sweet Tooth by handing her candy.
        public const string gaveSnipeHuntExcuseToMuzsa = "gaveSnipeHuntExcuseToMuzsa";

        //Kende the Cook flags
        public const string gotMessHallInstructionsFromKende = "gotMessHallInstructionsFromKende"; //spoke with Kende
        public const string knowsAboutKendesShop = "knowsAboutKendesShop"; //(unimplemented) Learned about Kende's shop from someone besides him or Muzsa (like Clay). Enables dialogue to ask about shop, but not shop itself
        public const string kendeWillSellToPlayer = "kendeWillSellToPlayer"; //Kende the cook has the option to sell items to the player
        public const string askedKendeWhoHeIs = "askedKendeWhoHeIs"; //chose the dialogue option about asking kende who he was


        //Quartermaster Emese flags
        public const string metQuartermasterEmese = "metQuartermasterEmese"; //have interacted with Emese before
        public const string gaveIronNuggetToEmese = "gaveIronNuggetToEmese";

        //Uros flags
        public const string startledUros = "startledUros"; //You spoke to Uros so he's no longer talking about looking for something.
        public const string convincedUros = "convincedUros"; //convinced Uros to tell you about what he has hidden away.
        public const string intimidatedUros = "intimidatedUros"; //intimidated Uros into telling you about what he has hidden away.
        public const string threatenedToSnitchOnUros = "threatenedToSnitchOnUros"; //Uros told you about what he has hidden because you told him you'd snitch on him but didn't end up snitching.
        public const string snitchedOnUros = "snitchedOnUros"; //Told Quartermaster Emese about Uros hiding something in the stockhouse
        public const string gaveUrosTheNugget = "gaveUrosTheNugget"; //finished Uros's quest by giving his nugget back.
        public const string showedUrosTheNuggetWithoutGivingItBack = "showedUrosTheNuggetWithoutGivingItBack"; //finished Uros's quest by showing him the nugget and then not giving it back to him.
        public const string gotIronNuggetFromBarrels = "gotIronNuggetFromBarrels"; //whether you've picked up the iron nugget from behind the barrels yet in the stockhouse
        public const string spokeToUrosShop = "spokeToUrosShop";
        public const string urosBadAttitude = "urosBadAttitude";
        public const string urosGoodAttitude = "urosGoodAttitude";
        public const string urosWorstPrices = "urosWorstPrices";
        public const string urosBadPrices = "urosBadPrices";
        public const string urosNormalPrices = "urosNormalPrices";
        public const string urosBestPrices = "urosBestPrices";

        //Carter flags
        public const string toldCarterPassword = "toldCarterPassword"; // gave Carter the password on first meeting him 
        public const string toldCarterWrongPassword = "toldCarterWrongPassword"; //If you said anything but 'East' to Carter when asked the password and you got the call/response from Garcha
        public const string learnedCartersIdentity = "learnedCartersIdentity"; //Told about where Carter comes from and his mission in the camp 
        public const string learnedPagesIdentity = "learnedPagesIdentity"; //Told about where Page comes from and her mission in the camp 
        public const string toldDirectorIsAWarHero = "toldDirectorIsAWarHero"; //Told capturing the Director may start a war. Prompts a question about that later when talking to Carter
        public const string learnedCampLocationFromCarter = "learnedCampLocationFromCarter"; //Carter was the one who told the player where the camp is located
        public const string carterSaidBrandedWouldBeTreatedLikeGuests = "carterSaidBrandedWouldBeTreatedLikeGuests"; //Player read dialogue where Carter said the branded would be treated like guests by the Masons

        //Camp Flags
        public const string campSEHiddenPassageFound1 = "campSEHiddenPassageFound1"; //You opened the right passage between 6SlaveShack and CampSE
        public const string campSEHiddenPassageFound2 = "campSEHiddenPassageFound2"; //You opened the passage left between 6SlaveShack and CampSE
        public const string campSEHiddenPassageFound3 = "campSEHiddenPassageFound3"; //You opened the passage between 5SlaveShack and CampSE
        public const string campCenterFirstHiddenPassageFound = "campCenterFirstHiddenPassageFound"; //You opened the passage between the Stables and CampCenter
        public const string campCenterSecondHiddenPassageFound = "campCenterSecondHiddenPassageFound"; //You opened the passage between the Temple and CampCenter

        //Minelvl2 flags
        public const string mineLvl2BrokenGateLifted = "mineLvl2BrokenGateLifted"; //if you've passed the strength check to lift the gate on the east side of Mine Lvl 2
        public const string mineLvl2HiddenPassageFound = "mineLvl2HiddenPassageFound"; //if you've opened the secret doors in the mine lvl 2
        public const string mineLvl2ArmoryGateOpened = "mineLvl2ArmoryGateOpened"; //if you've opened the armory gate in mine lvl 2
        public const string mineLvl2GateToLevel3Opened = "mineLvl2GateToLevel3Opened"; //if you've opened the gate to mine lvl 3
        public const string mineLvl2HiddenDoor7b = "mineLvl2HiddenDoor7b"; //if you've opened the wisdom wall in 7b
        public const string mineLvl2GuardsMovedToSecondLevelGate = "mineLvl2GuardsMovedToSecondLevelGate"; //if you got the guards to move to the gate to the second level
        public const string mineLvl2GuardsFinishedMove = "mineLvl2GuardsFinishedMove"; //used to tell GuardsCrate.ink to not deactivate guards

        //Minelvl3 flags
        public const string mineLvl3ClearedCratesToMiners = "mineLvl3ClearedCratesToMiners"; //if you've had the Miners clear the crates to their room
        public const string mineLvl3CarterAndNandorInParty = "mineLvl3CarterAndNandorInParty"; //if you've met added Carter and Nandor to the party after meeting them near Guard Marcos
        public const string mineLvl3ToldAboutJelly = "mineLvl3ToldAboutJelly"; //Nandor has explained that you need to seal the breech in the mines third level to keep the worms from overwhelming the camp
        public const string mineLvl3ClearedCratesToGuards = "mineLvl3ClearedCratesToGuards"; //if you've had the Guards clear the crates to their room
        public const string mineLvl3SpeakingFromBrokenGate = "mineLvl3SpeakingFromBrokenGate"; //Lifted the broken gate that leads to the guards
        public const string mineLvl3SpeakingFromGuardCrates = "mineLvl3SpeakingFromGuardCrates"; //Speaking from crate side of guards
        public const string mineLvl3MetGaspar = "mineLvl3MetGaspar"; //if you've met Gaspar
        public const string mineLvl3KilledGuards = "mineLvl3KilledGuards";//if you've killed the guards on mine lvl 3 for any reason
        public const string mineLvl3GuardsInParty = "mineLvl3GuardsInParty";//if you've convinced or coerced the guards to join your party
        public const string mineLvl31bHiddenPassageFound = "mineLvl3-1bHiddenPassageFound";
        public const string mineLvl3MarcosAgreedToIgniteJelly = "mineLvl3MarcosAgreedToIgniteJelly";
        public const string mineLvl3MarcosTaughtHowToIgniteJelly = "mineLvl3MarcosTaughtHowToIgniteJelly";
        public const string mineLvl3MarcosDiedSealingBreach = "mineLvl3MarcosDiedSealingBreach";
        public const string mineLvl3BreachSealed = "mineLvl3BreachSealed";
        public const string mineLvl3RefusedToFightGaspar = "mineLvl3RefusedToFightGaspar";
        public const string mineLvl3DealtWithGaspar = "mineLvl3DealtWithGaspar"; //if you've killed gaspar and co after sealing the breach 
        public const string mineLvl3ToldPazmanToEatShit = "mineLvl3ToldPazmanToEatShit";
        public const string mineLvl3GuardsBackToSurface = "mineLvl3GuardsBackToSurface";
        public const string mineLvl3SlavesBackToSurface = "mineLvl3SlavesBackToSurface";
        public const string mineLvl3ToldToFindMarcos = "mineLvl3ToldToFindMarcos";
        public const string mineLvl3ThreatenedGaspar = "mineLvl3ThreatenedGaspar"; //took the strength dialogue option when convincing Overseer Gaspar to help you in the mine
        public const string mineLvl3ConvincedRekaAndPazman = "mineLvl3ConvincedRekaAndPazman"; //Convinced Guards Reka and Pazman to lay down their weapons
        public const string mineLvl3PromisedToProtectRekaAndPazman = "mineLvl3PromisedToProtectRekaAndPazman"; //Threatened Guard Reka and Pazman when taking them prisoner
        public const string mineLvl3ThreatenedRekaAndPazmanAsPrisoners = "mineLvl3ThreatenedRekaAndPazmanAsPrisoners"; //Threatened Guard Reka and Pazman when taking them prisoner 

        //Manse Flags
        public const string manseDiningRoomPermButtonScriptActivated = "manseDiningRoomPermButtonScriptActivated";
        public const string manse1FLibraryHiddenRoomFound = "manse1FLibraryHiddenRoomFound";
        public const string manseOfficeSecretDoorsOpened = "manseOfficeSecretDoorsOpened";
        public const string manseMeetingRoomSecretDoorsOpened = "manseMeetingRoomSecretDoorsOpened";
        public const string manseHiddenStairsFound = "manseHiddenStairsFound";
        public const string unlockedThePitGate = "unlockedThePitGate";
        public const string manseDoorsOpenedRevolt = "manseDoorsOpenedRevolt";

        //Pit Flags

        //Director Convo
        public const string directorDefeated = "directorDefeated";
        public const string allowedDirectorToSpeak = "allowedDirectorToSpeak";
        public const string directorSaidAbsurdityQuote = "directorSaidAbsurdityQuote";
        public const string directorConvoFinished = "directorConvoFinished";
        public const string keptDirectorAlive = "keptDirectorAlive";

        //Page flags
        public const string toldPageRevengeFeltFantastic = "toldPageRevengeFeltFantastic";
        public const string spokeWithPageBeforePrisoners = "spokeWithPageBeforePrisoners"; //if you speak to page at the center of camp after director is defeated but b4 dealing with the prisoners at the mess hall
        public const string pageSaidReadyToLeave = "pageSaidReadyToLeave"; //disables debris at the gate after speaking with page

        //boss kill flags
        public const string mineLvl2BatBossKilled = "mineLvl2BatBossKilled";
        public const string mineLvl2WormStrBossKilled = "mineLvl2WormStrBossKilled";
        public const string mineLvl3WormFinalBossKilled = "mineLvl3WormFinalBossKilled";
        public const string neCampOverseerKilled = "neCampOverseerKilled";
        public const string killedChiefIren = "killedChiefIren";

        //puzzle flags
        public const string wisdomPuzzleMineLvl3Completed = "wisdomPuzzleMineLvl3Completed";
        public const string wisdomPuzzleMineLvl3TwoObjects = "wisdomPuzzleMineLvl3TwoObjects";
        public const string wisdomPuzzleMineLvl3ThreeObjects = "wisdomPuzzleMineLvl3ThreeObjects";
        public const string wisdomPuzzleMineLvl3FourObjects = "wisdomPuzzleMineLvl3FourObjects";
        public const string wisdomPuzzleMineLvl3FiveObjects = "wisdomPuzzleMineLvl3FiveObjects";

        public const string prevWTRoundRubble = "prevWTRoundRubble";
        public const string prevWTSingleStalagmite = "prevWTSingleStalagmite";
        public const string prevWTTripleStalagmite = "prevWTTripleStalagmite";
        public const string prevWTBushRock = "prevWTBushRock";

        //Camp Barricade Pass Flags
        public const string wisdomBarricadePassUsed = "wisdomBarricadePassUsed";
        public const string strengthBarricadePassUsed = "strengthBarricadePassUsed";
        public const string charismaBarricadePassUsed = "charismaBarricadePassUsed";
        public const string andrasBarricadePassUsed = "andrasBarricadePassUsed";

        //beam flags
        public const string metBeam = "metBeam";
        public const string askedBeamAboutWhittling = "askedBeamAboutWhittling";
        public const string pissedOffBeam = "pissedOffBeam";
        public const string givenHorsetongueGuideFromBeam = "givenHorsetongueGuideFromBeam";
        public const string askedAboutMangledName = "askedAboutMangledName";
        public const string sworeToBurnCsalansBody = "sworeToBurnCsalansBody";
        public const string foughtHorsesInManse = "foughtHorsesInManse";

        //Csalan Oath Flags
        public const string csalanLifeOathMade = "csalanLifeOathMade";
        public const string csalanSufferingOathMade = "csalanSufferingOathMade";
        public const string csalanStainedNameOathMade = "csalanStainedNameOathMade";
        public const string csalanFamilyOathMade = "csalanFamilyOathMade";
        public const string csalanHomeOathMade = "csalanHomeOathMade";
        public const string csalanWealthOathMade = "csalanWealthOathMade";
        public const string csalanSanityOathMade = "csalanSanityOathMade";


        //Guard Punishment Flags
        public const string noPrisoners = "noPrisoners";

        public const string spokeWithMarcosAtPunishment = "spokeWithMarcosAtPunishment";
        public const string spokeWithAndrasAtPunishment = "spokeWithAndrasAtPunishment";
        public const string spokeWithRekaAtPunishment = "spokeWithRekaAtPunishment";

        public const string didNotExecuteMarcos = "didNotExecuteMarcos";
        public const string gaveMarcosToTheCrowd = "gaveMarcosToTheCrowd";
        public const string gaveMarcosFiftyLashes = "gaveMarcosFiftyLashes";
        public const string executedMarcos = "executedMarcos";

        public const string didNotExecuteAndras = "didNotExecuteAndras";
        public const string gaveAndrasToTheCrowd = "gaveAndrasToTheCrowd";
        public const string gaveAndrasFiftyLashes = "gaveAndrasFiftyLashes";
        public const string executedAndras = "executedAndras";

        public const string didNotExecuteReka = "didNotExecuteReka";
        public const string gaveRekaToTheCrowd = "gaveRekaToTheCrowd";
        public const string gaveRekaFiftyLashes = "gaveRekaFiftyLashes";
        public const string executedReka = "executedReka";

        public const string didNotExecutePazman = "didNotExecutePazman";
        public const string gavePazmanToTheCrowd = "gavePazmanToTheCrowd";
        public const string gavePazmanFiftyLashes = "gavePazmanFiftyLashes";
        public const string executedPazman = "executedPazman";

        public const string didNotExecuteTabor = "didNotExecuteTabor";
        public const string gaveTaborToTheCrowd = "gaveTaborToTheCrowd";
        public const string crowdForcedTaborExecution = "crowdForcedTaborExecution";
        public const string executedTabor = "executedTabor";

        public const string foughtCrowdForTabor = "foughtCrowdForTabor"; //failed to convince Clay and the crowd to let Tabor live, and had to fight them
        public const string clayWillSeekOutTabor = "clayWillSeekOutTabor"; //failed to convince Clay, but not the crowd, to let Tabor live, and Clay decides to attack you later

        public const string crowdDispersed = "crowdDispersed";

        //Dialogue Upon Entry Flags
        public const string kendeUponEnteringKitchens = "kendeUponEnteringKitchens";
        public const string taborManse2F2B = "taborManse-2F-2B";

        //Chest Item Pickup Flags
        public const string gotArmoryKeyFromGuardHouse = "gotArmoryKeyFromGuardHouse"; //picked up the mine armory key from the 2nd floor of the camp guard house

        //Stop Party Train Spawning Flags
        public const string disablePartyTrain = "disablePartyTrain";

        //Entered Area flags (used for advancing time/quests usually)
        public const string enteredMineLvl1 = "enteredMineLvl1";
        public const string enteredMineLvl2 = "enteredMineLvl2";
        public const string enteredMineLvl2_2a = "enteredMineLvl2-2a";
        public const string enteredMineLvl3 = "enteredMineLvl3";
        public const string enteredMessHallYardAfterRevolt = "enteredMessHallYardAfterRevolt";
        public const string enteredCivilizationAfterLeavingCamp = "enteredCivilizationAfterLeavingCamp";

}
