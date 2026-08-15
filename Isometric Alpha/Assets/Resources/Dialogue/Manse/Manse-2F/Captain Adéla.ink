VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR playerIndex = 0
VAR adelaIndex = 1

VAR adelaFightIndex = 0

VAR playerName = ""

->1a

=== 1a ===

changeCamTarget({adelaIndex})

See what events have escalated to: dirty, ragged slaves marching about the Manse, looting and killing as they go. Your lot are always but one step from a riot. 

playAnimation({adelaIndex},Idle_Back)

We grew lax and unclamped our boots from your necks for but an instant, and you took the oppurtunity to cause havoc. And as Captain of the Guard, it falls to me to put things right.

    ->1b

=== 1b ===

    +I don't suppose I could convince you to surrender?
        Of course not. I was hand picked by the Director himself to lead his honorguard. To give in to you is unthinkable.
        ->1b

    +How evil you make our freedom seem.

        I wish freedom on you like I wish it upon a wildfire, or a plague. I'd rather die than allow your kind the freedom to spread as you please.

        ->haveItYourWay

    +If a system demands constant vigilance, then it is doomed to fail. No one can be on their guard forever.
        ->1c

    +Let our servitude end! Die slaver! <Combat>
        ->Combat

    +Your rhetoric means nothing to me. Let the inevitable commence. <Combat>
        ->Combat

=== 1c === 

Violence is inevitable in all parts of life. A warrior knows this and prepares accordingly.

    +What a lame excuse. What other slice of your life comes close to this? 
        This is a waste of time! While you blather on, your compatriots continue their rampage! I'm done talking, raise your weapons!

        ->haveItYourWay

    +Even if that were true, surely the brand amplifies that violence. Where else is conflict so bloody, so desparate?
        ->1ca

=== 1ca === 

Nowhere, for the brand is applied to the worst of the worst. It is obvious that the cruelest criminals will behave in the most savage manner. 

    +If we are so savage, then why not just kill us? That would be easier than branding, collecting, and confining us, and then whipping and starving us to keep us in line. For the sake of punishment, you've chosen to perpetually add heat to a bubbling pot, and suffer sitting on the lid to prevent it boiling over.
        ->1d

=== 1d ===

An... apt metaphor. But you underestimate the value of justice. This 'boiling pot' is its own reward. The brand provides us a punishment fit for cannibals and murderers; it is worth the inevitable conflict.

    +I won't fight you on the field of morals. We would both grow old before the other relented. But if you care an ounce for the lives of those you command, you should lament a system that endangers them so.
        ->1e

    +Admit it, Captain: the lords you serve have set quotas, and demand results. To their tune you spin pain into industry and you care not whether it's for justice or greed.
        ->1da

=== 1da ===

\*Captain Adéla grinds her teeth.* You know nothing. I'm done with useless talk. Fight me or shut your damned hole.

    ->haveItYourWay

=== 1e ===

I- *Captain Adéla pauses for a moment, but then raises her weapon.* Fool, why would I lament that? All it is is an excuse to get my hands dirty.

    +The truth comes out. <Combat>
        ->Combat


=== haveItYourWay ===

    +Have it your way. <Combat>
        ->Combat

=== Combat ===

enterCombat({adelaFightIndex})

->Close


=== Close ===

close()

->DONE