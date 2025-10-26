VAR gotBroglinKilledByGuard = false
VAR spokeToGarchaAboutPlan = false
VAR kastorStartedRevolt = false
VAR wisdom = 0
VAR charisma = 0

{
-kastorStartedRevolt:
->4a
-else:
->3a
}


=== 1b ===

{gotBroglinKilledByGuard:Ask what you need, but make it quick.|I think you've earned some answers. What were you looking to know?}
    

    +What is the pit?
        ->1d
    +How long have you been here?
        ->1c
    +What happened to the mine?
        ->1e
    +Never mind.
        ->1f

=== 1c ===

I've been here for four months, or about that. It's a little hard to keep track of time here. I know it certainly feels like much longer.

keepDialogue()

The slaves that have been here that long helped build the camp. The Manse and guard quarters were here before we arrived. I think they were apart of some earlier settlement, but I can't say who lived here before us.

{spokeToGarchaAboutPlan:->3d|->1b}

=== 1d ===

keepDialogue()

I've never been there, but it's where the guards send the slaves that cause the most trouble. Those that attempt to escape or foment revolt, that sort of thing. I'm not sure where it is but I think it's underneath the Manse where the camp's Director lives. It's not out in the camp and I never saw it when I worked the mine.

{spokeToGarchaAboutPlan:->3d|->1b}

=== 1e ===

keepDialogue()

I'm not sure. I was on the top level when it happened. All of a sudden, the guards from the lower levels were running out, herding all the slaves towards the entrance. I heard some rumors afterwards that said some guards and slaves were trapped farther down, but the lockdown started shortly after that and I couldn't get more information.

{spokeToGarchaAboutPlan:->3d|->1b}

=== 1f ===

keepDialogue()

Alright then.
    ->3a


=== 3a ===

You're back. Was there something else you needed?

    +I need rest.
        ->3e
    +Where was Kastor again?
        ->3b
    +What was the passphrase again?
        ->3c
    +Can you answer some questions for me?
        ->3d
    +Nothing else.
        ->Close

=== 3b ===

keepDialogue()

You're not inspiring much confidence in me when you have me repeating myself like this. He's in one of the other huts, down in the south east section of the camp. It's the biggest hut in that part of the camp, along the southern wall. Don't forget it this time.

->3a


->Close

=== 3c ===

keepDialogue()

You gotta ask Kastor which way the wind is blowing. He won't talk to you about the plan otherwise.

->3a

=== 3d ===

Ask what you need, but make it quick.
    
    +What is the pit?
        ->1d
    +How long have you been here?
        ->1c
    +What happened to the mine?
        ->1e
    +Never mind.
        ->1f

=== 3e ===

restParty()

->DONE

=== 4a ===

Alright, finally! 

->Close

=== Close ===

close()

->DONE