VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR speakerIndex = 1
VAR barricadeParentIndex = 2
VAR andrasIndex = 3

VAR barricadeGuardDeathFlagNameIndex = 2

VAR barricadeEnemyInfoIndex = 0

VAR wisdomBarricadePassUsed = false
VAR strengthBarricadePassUsed = false
VAR charismaBarricadePassUsed = false
VAR andrasBarricadePassUsed = false

VAR andrasLeftInHut = false
VAR gotKeyFromJanos = false
VAR acceptingGuardPrisoners = false

VAR deathFlagGuardAndrás = false
VAR deathFlagGuardJanos = false

VAR playerName = ""

//changeCamTarget(int targetIndex)
//keepDialogue()
//setToTrue(string flagName)
//setToFalse(string flagName)
//activate(int index of gameobject you're activating)
//deactivate(int index of gameobject you're deactivating)
//activateQuestStep(string questTitle, int questStepIndex)
//prepForItem()
//giveItem(int listIndex, int itemIndex, int quantity)
//giveItems(int listIndex1, int itemIndex1, int quantity1 |
//          int listIndex2, int itemIndex2, int quantity2 |
//          ... etc)
//takeAllOfItem(string itemName)

->1a

=== 1a ===

Halt! Approach the barricade at your own peril!

    {
    -not strengthBarricadePassUsed && strength >= 3:
    +The last guard felt confident in his barricade too, until I fed him his tongue. How confident do you feel? <Str {strength}/3>
        ->Str
    }

    {
    -not wisdomBarricadePassUsed && acceptingGuardPrisoners && wisdom >= 3:
    +We have you outmanned by an enormous margin. Surrender, and we will leave you unhurt. <Wis {wisdom}/3>
        ->Wis
    }

    {
    -not charismaBarricadePassUsed && acceptingGuardPrisoners && charisma >= 3:
    +No need to throw your lives away in this Mother-forsaken camp. Surrender, and I swear to give you protection. <Cha {charisma}/3>
        ->Cha
    }
    
    {
    -not andrasBarricadePassUsed and not deathFlagGuardAndrás and not deathFlagGuardJanos and acceptingGuardPrisoners and (gotKeyFromJanos or andrasLeftInHut):
    +Janos, go get András. Have him negotiate.
        ->And
    }    

    +I'm getting through this barricade whether you man it or not. For freedom! <Attack>
        enterCombat({barricadeEnemyInfoIndex})
        ->deactivateExtras
    +\*Leave without fighting.*
        ->Close

=== Str ===

setToTrue(strengthBarricadePassUsed)

\*Gulp* L-less by the second. I'm getting outta here!

kill({barricadeGuardDeathFlagNameIndex})
    ->deactivateExtras
    
=== Wis ===

setToTrue(wisdomBarricadePassUsed)

\*Sigh* I can see that. Fine, we'll enter into your custody. Lower your weapons, we're coming out.

kill({barricadeGuardDeathFlagNameIndex})
    ->deactivateExtras

=== Cha ===

setToTrue(charismaBarricadePassUsed)

Dying for your boss isn't all it's cracked up to be anyways. Lower your weapons, we're coming out.

kill({barricadeGuardDeathFlagNameIndex})
    ->deactivateExtras

=== And ===

activate({andrasIndex})

setToTrue(andrasBarricadePassUsed)

changeCamTarget({andrasIndex})

Ferenc, that you over there?

changeCamTarget({speakerIndex})

Yeah it's me. You with them now, András?

changeCamTarget({andrasIndex})

The Director's days are numbered. Anyone can see that. Wouldn't you rather not be remembered as someone who died to keep people in chains?

changeCamTarget({speakerIndex})

...

changeCamTarget({andrasIndex})

Sun's dipping, Ferenc. We're gonna need an answer.

changeCamTarget({speakerIndex})

They gonna kill us if we drop our weapons?

changeCamTarget({andrasIndex})

\*András looks to you for an answer.*

    +I swear before the Gods that if you relinquish this barricade you will be treated well.
        changeCamTarget({speakerIndex})

        That'll do. Lower your weapons, we're coming out.
        deactivate({andrasIndex})
        kill({barricadeGuardDeathFlagNameIndex})
        ->Close
    +On second thought, I think I'd rather just kill you guys. <Combat>
        deactivate({andrasIndex})
        //kill({barricadeGuardDeathFlagNameIndex})
        enterCombat({barricadeEnemyInfoIndex})
        ->Close

=== deactivateExtras === 

deactivate({speakerIndex})
deactivate({barricadeParentIndex})
deactivate({andrasIndex})

->Close

=== Close ===

close()

->DONE