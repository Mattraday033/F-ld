VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerName = ""

VAR spokeToTaborAtBeginningOfSituation = false

VAR taborIndex = 1
VAR weftIndex = 2

VAR hostageTakersStandardPunishment = false
VAR hostageTakersNoPunishment = false
VAR hostageTakersLeaderPunished = false
VAR hostageTakersLaborPunishment = false

VAR gaveWeftCreditAfterHostages = false
VAR tookBlameForHostageDeath = false
VAR blamedWeftForHostageDeath = false

VAR mentionedStoneMan = false
VAR hostageSituationGuardsLeft = false
VAR insultedWeftAfterHostages = false

VAR hostagesDead = false
VAR declaredHostagesDead = false
VAR concludedHostageNegotiations = false


{
-concludedHostageNegotiations:

    activateQuestStep(No Good Deed,Go to the Director's office.)
    
    openGateFromKey(Manse Front Door)
    {
    -hostagesDead:
        ->setUpSpeakers(->2a)
    -else:
        ->setUpSpeakers(->1a)
    }
}

=== setUpSpeakers(->divert) ===

changeCamTarget({taborIndex})

disableDialogueUI()

fadeToBlack(true, false)

activate({weftIndex})
movePlayer(4,-16)
setFacing(NE)

fadeBackIn(60)
enableDialogueUI()

->divert

=== 1a ===

Good, you're here. I have given my report to the Director, and he was impressed with how you two managed during the hostage negotiations.

The Director has asked to meet both of you. He has a task of some import that he needs you to perform for him.

    +Will we be given our reward before or after we finish this errand?
        Your conduct so far has earned my patience, but do not ask such things of the Director. He will not be as forgiving as I am. 

        However, your question is a fair one. Before, certainly. I will have it made ready while the Director explains what he needs done, and then I will give it to you afterwards.
        ->1b
    +\*Say nothing.*  
        ->1b

=== 1b ===

The Director's office is inside the Manse. Enter through the main door, then proceed up the stairs. His office will be the second door on the right. Do not deviate from this course: branded slaves are not allowed to wander the halls of the Manse unsupervised.
    ->deactivateExtras

=== 2a ===

Good, you're here. Captain Adéla and I have both given our reports to the Director on your participation in the deaths of the hostages. While I was able to keep him from ordering you punished, the captain was persuasive on a different matter.

The Director has asked to meet both of you. He has a task that he believes you two well suited for.

    +That does not bode well.
        How it bodes is of no concern to you. And make no mention of your thoughts to the Director, or I will be unable to shield you from punishment again.
        ->1b
    +\*Say nothing.*  
        ->1b

=== deactivateExtras===

setToTrue(summonedToDirectorsOffice)

fadeToBlack()

deactivate({taborIndex})
deactivate({weftIndex})

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE