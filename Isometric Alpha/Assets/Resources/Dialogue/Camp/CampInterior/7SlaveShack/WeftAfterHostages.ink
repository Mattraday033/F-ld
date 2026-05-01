VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerName = ""

VAR weftIndex = 1

VAR dezsoAndSlavesFightIndex = 0
VAR dezsoOnlyFightIndex = 1

VAR declaredHostagesDead = false
VAR savedHostages = false
VAR hostagesDead = false
VAR foughtDezsoAndLoam = false

VAR toldNotAllowedToLeave = false

VAR concludedHostageNegotiations = false
VAR spokeToTaborAtBeginningOfSituation = false

VAR mentionedStoneMan = false
VAR failedRushDezso = false

VAR hostageTakersStandardPunishment = false
VAR hostageTakersNoPunishment = false
VAR hostageTakersLeaderPunished = false
VAR hostageTakersLaborPunishment = false

->1a

=== 1a ===

You aren't supposed to be here. Leave before the guards get the wrong impression.

        ->Close

=== deactivateExtras === 

fadeToBlack()

deactivate({weftIndex})

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE