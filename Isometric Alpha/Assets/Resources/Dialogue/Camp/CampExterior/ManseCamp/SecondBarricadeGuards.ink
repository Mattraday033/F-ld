VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR speakerIndex = 1
VAR andrasIndex = 2

VAR withBarricadeFightIndex = 0
VAR withoutBarricadeFightIndex = 1

VAR defeatFlag = ""

VAR facingNE = false
VAR facingNW = false
VAR facingSW = false
VAR facingSE = false

VAR wisdomBarricadePassUsed = false
VAR strengthBarricadePassUsed = false
VAR charismaBarricadePassUsed = false
VAR andrasBarricadePassUsed = false

VAR andrasLeftInHut = false
VAR gotKeyFromJanos = false
VAR acceptingGuardPrisoners = false

VAR deathFlagGuardAndrás = false
VAR deathFlagJanos = false

VAR playerName = ""


{
-facingNW:
    ->1a
-else:
    ->1b
}

=== 1a ===

Halt! Approach the barricade at your own peril!

    {
    -not strengthBarricadePassUsed && strength >= 3:
    +The last guard felt confident in his barricade too, until I fed him his tongue. How confident do you feel? <Str {strength}/3>
        ->Str
    }

    //{
    //-not wisdomBarricadePassUsed && acceptingGuardPrisoners && wisdom >= 3:
    //+We have you outmanned by an enormous margin. Surrender, and we will leave you unhurt. <Wis {wisdom}/3>
    //    ->Wis
    //}

    {
    -not charismaBarricadePassUsed && acceptingGuardPrisoners && charisma >= 3:
    +No need to throw your lives away in this Mother-forsaken camp. Surrender, and I swear to give you protection. <Cha {charisma}/3>
        ->Cha
    }
    
    {
    -not andrasBarricadePassUsed and not deathFlagGuardAndrás and not deathFlagJanos and acceptingGuardPrisoners and (gotKeyFromJanos or andrasLeftInHut):
    +Janos, go get András. Have him negotiate.
        ->And
    }    

    +I'm getting through this barricade whether you man it or not. For freedom! <Attack>
        setToTrue({defeatFlag})
        setToTrue(attackedBarricadeHeadOn)
        enterCombat({withBarricadeFightIndex})
        ->Close
    +\*Leave without fighting.*
        ->Close

=== 1b ===

Blast, the rioters got behind us! To arms!

setToTrue({defeatFlag})

enterCombat({withoutBarricadeFightIndex})
->Close

=== Str ===

setToTrue(strengthBarricadePassUsed)

\*Gulp* L-less by the second. I'm getting outta here!

    ->deactivateExtras
    
=== Wis ===

setToTrue(wisdomBarricadePassUsed)

\*Sigh* I can see that. Fine, we'll enter into your custody. Lower your weapons, we're coming out.

    ->deactivateExtras

=== Cha ===

setToTrue(charismaBarricadePassUsed)

Dying for your boss isn't all it's cracked up to be. Lower your weapons, we're coming out.

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

Sun's dipping, Ferenc. We need an answer.

changeCamTarget({speakerIndex})

Will they kill us if we drop our weapons?

changeCamTarget({andrasIndex})

\*András looks to you for an answer.*

    +I swear before the Gods that if you relinquish this barricade you will be treated well.
        changeCamTarget({speakerIndex})

        That'll do. Lower your weapons, we're coming out.
        ->deactivateExtras
    +On second thought, I think I'd rather just kill you guys. <Combat>
        setToTrue({defeatFlag})
        enterCombat({withBarricadeFightIndex})
        ->Close

=== deactivateExtras === 

fadeToBlack()

setToTrue({defeatFlag})

updateNPCVisibility()

deactivate({andrasIndex})

fadeBackIn(60)

->Close

=== Close ===

close()

->DONE