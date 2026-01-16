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


VAR questItemListIndex = 3
VAR claysNoteIndex = 6
VAR thatchsNecklaceKey = "Thatch's Silver Necklace"

VAR playerName = ""

->1a

=== 1a ===

Good luck out there. We're all behind you now... Don't give me that look, someone has to stay behind and make sure you have somewhere to fall back to. Besides, if I die, who's gonna patch you up?

    +I'm wounded. Can you heal me?
        Alright, let me take a look at you.

        restParty()
        ->Close
    +I'm off.
        ->Close

=== Close ===

close()

->DONE