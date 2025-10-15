VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR heardTaborsLesson = false
VAR letTaborLive = false
VAR attackedTabor = false
VAR killedTaborInManse = false
VAR acceptedTaborsSurrenderAfterDirectorFight = false
VAR directorDefeated = false
VAR keptDirectorAlive = false

VAR taborIndex = 1
VAR taborFightIndex = 0

VAR playerName = ""

//changeCamTarget(int targetIndex)
//keepDialogue()
//setToTrue(string flagName)
//setToFalse(string flagName)
//activate(int index of gameobject you're activating)
//deactivate(int index of gameobject you're deactivating)
//activateQuestStep(string questTitle, int questStepIndex)
//prepForItem() //used before giveItem/giveItems/takeAllOfItem to add obtained/removed text after next line
//giveItem(int listIndex, int itemIndex, int quantity)
//giveItems(int listIndex1, int itemIndex1, int quantity1 |
//          int listIndex2, int itemIndex2, int quantity2 |
//          ... etc)
//takeAllOfItem(string itemName)
//activateQuestStep(string fullTitleOfQuestFoundInQuestJsonFile,int questStepIndex)
//searchInventoryFor(string nameOfVarSetToTrueInsideInkFile,string itemNameToSearchFor)
//fadeToBlack()
//fadeBackIn(int numberOfFramesToWaitBeforeFadingBackIn)
//moveToLocalPos(float xCoord,float yCoord)

{
-directorDefeated:
->4a
-letTaborLive:
->3a
-else:
->1a
}


=== 1a ===

You're too late, villains. The Director's children have already escaped through the hatch, and their bodyguards will have barred it behind them. By the time you kill me and bust your way through, they will be long gone.

{
-heardTaborsLesson:

I had hoped my lessons would have rubbed off on you, but it seems I have failed you. And for that I am sorry. But it does not matter now. I die knowing that I have prevented you from harming two innocents, and that is enough for me.

-else:

Had you listened to my lessons, perhaps I could have prevented this conflict. Perhaps I could have enlightened you to your evil ways and had you repent. But it does not matter now. I die knowing that I have prevented you from harming two innocents, and that is enough for me.

}

    +You've misjudged us. I never wanted to harm the Director's children.
        ->1b
    +Cut the melodrama, Tabor. You're not the hero here.
        ->2a

=== 1b ===

Like I would take you at your word. A group of armed slaves breaking into the Manse? It's clear you have revenge on your minds. 

    +You are correct, the Director and his goons must pay. But what you speak of is completely different. 
        ->1c
    +I'm not here to bandy words with a slaver. Time to die, 'Chief'. <Combat>
        ~attackedTabor = true
        ->killTabor
        
=== 1c ===

So you say. I've worked with the branded for years, I know your kind. I won't let my guard down for a second, so you might as well stop talking and advance.

    +As you said, the hatch behind you is barred. If we guarded this entrance, you would be our prisoner.
        ->1d
    +Fine, as if I'd lose sleep over another dead slaver. <Combat>
        ~attackedTabor = true
        ->killTabor

=== 1d ===

I fail to see your point.

    +Consider this: guard the hatch until we return, so there is no chance we could pursue the Director's children. Then we take you prisoner without a fight.
        ->1e

=== 1e ===

Why would I do that? And further more, why would you want that?

    +Because it proves that we aren't the monsters you think we are.
        ->1f

=== 1f ===

\*Tabor considers your offer for a moment.* I don't know what the trick is here, but like I said, you can't get to the children without going through me. So go off and do whatever you want, I'll guard this hatch until you realize your blunder.

    +Sure, Tabor. See you then.
        setToTrue(letTaborLive)
        activateQuestStep(An Uneasy Truce, 0)
        ->Close
    +On second thought, I can't stand letting someone like you live. <Combat>
        ~attackedTabor = true
        ->killTabor

=== 2a === 

This isn't about me: it's always been about you and your wicked ways. Could you see past your self-righteousness and repent? Could you look to the future and better yourselves? Ultimately, no, and so you prove that you could never change.

    +The day is not yet over, Tabor. If you are set on not moving from that hatch, then we have no reason to come to blows.
        keepDialogue()
        I do not understand what you mean.
        ->1d
    +Take your twisted rhetoric and shove it up your ass, old man. You die now. <Combat>
        ~attackedTabor = true
        ->killTabor
    
=== 3a ===

You're back, just as I thought. Stay where you are, or we're going to have a problem.

    +I'm just a little turned around in this big house. I'll be going.
        ->Close
    +On second thought, I actually can't stand the thought of letting you live. <Combat>
        ~attackedTabor = true
        ->killTabor

=== 4a ===

You're back. And it sounds as if the fighting has stopped. The Director, is he...?

    {
    -keptDirectorAlive:
        +He's alive. He's more valuable to us that way. It seems there's more going on here than either of us realized.
        
            I'm not sure I believe you. But it doesn't matter. By now, the children will have made good on their escape, and my duty to the Director is fulfilled. 
                ->4b
    -else:
        +Dead. His crimes were too great to let him live. 
            So be it. By now, the children will have made good on their escape, and my duty to the Director is fulfilled. I will have to be content with that.
                ->4b
    }
    
    +The Director is no longer important. The riots are over, and I have returned, as I said I would.
                    The Director was a great man. His legacy will live on well past his death. But by now his children will have made good on their escape. My duty to the Director is fulfilled. I will have to be content with that.
                ->4b

=== 4b ===
    
    +I have come to see our bargain resolved. I have kept the other rioters from harming you. You will now submit to me and become my prisoner.
        ->4c
        
    +Be as contented as you like. I no longer see a purpose in letting scum like you live. <Combat>
        ~attackedTabor = true
        ->killTabor
=== 4c ===

I never agreed to such a deal. But... you did make no attempt to pursue the children. And you say you will make sure I am not harmed?

    +You have my word.
        ->4d

=== 4d ===

I do not understand you, branded. Your kind always discard their morals. The others have been kidnappers, cannibals, and murderers all. And yet you cut deals, and claim it's all an effort to save lives. For a ruse, it's awfully long-winded and far-fetched.

{
-wisdom >= 3:
    +I never understood why you chose such violent methods to teach the branded. It would be hypocritical of me to expect you to understand my methods of teaching you. <Wis {wisdom}/3>
            ->6a
}

    +I have seen many comrades fall today. I won't risk any more of them when we're so close to freedom. I'm willing to let you keep breathing if it means we all get to.
            ->9a
    +I hold all the cards. I've organized the branded, killed dozens of guards that have tried to put us back in chains, and defeated the Director, all to secure my people's freedom. What would be the purpose of such a con at the very end?
            ->8a
    +Is it so far-fetched that I would want to save a life? In all the decades of conflict between our peoples, did you expect no one to stop and think to end it?
            ->7a

=== 6a ===

You don't know of what you speak. And you're mistaken if you think I am in need of correction.

    +Don't I? Am I? Your methods would clearly turn more pupils away than they would get through to.
        ->6b

=== 6b ===

The fault lies in the stubborness of the branded, not in my instruction. Your lot hubristically claim that you are a higher form of life than horses, while simulaneously fearing and hating them.

    +Have you ever taught the branded the horsetongue? If they could speak to horses, surely the branded would learn to respect them.
        ->6c
    
=== 6c ===

This is not an academy. This camp was not erected to provide the branded with an education.

    +But you claim the entire system of branding exists to teach the branded morals. That sounds like an education to me.
        ->6d
    
=== 6d ===

An education in ethics, then! Not in language!

    +But learning the language would help with that. Without it, their only understanding of the goodness and intelligence of horses would come from you: the people beating and enslaving them.
        ->6e
    
=== 6e ===

\*Tabor looks incensed, but does not reply.*

    +Why would they take you at your word that horses are their equals? In their eyes, you work for the horses and you hurt them. To them, it must seem like the horses want them to be in pain. No wonder they fear and loathe them.
        ->6f
    
=== 6f ===

The equality of all intelligent life is self evident. It shouldn't be difficult for them to grasp that!

    +But you treat the branded like dirt. Why aren't you at fault for failing to treat all intelligent beings equally?
        ->6g
    
=== 6g ===

Because the branded aren't intelligent!

    +You and I may never agree on that. But even if that were true you must grant that to them, they are intelligent. So in their eyes you would still be hypocrits for claiming to treat all intelligent beings equally, and then oppressing them. Would you take the word of a hypocrit over your own? 
        ->6h
    
=== 6h ===

This is ridiculous. You condemn our methods without understanding them or their necessity. You know nothing about what you speak.

    +Nothing? I know you think of yourself as a man of morals. What I believe is you abandoned those morals when you chose such detestable methods. I wish to show you that you would have been more successful by sticking to them. Haven't you ever wished there was another way?
            ->9f

=== 7a ===

Everyone would end this conflict if they could! Do you think us so bloodthirsty that we wish to keep it going forever?

    +That was not my meaning. And when you speak of ending the conflict, you would see all the Craft Folk bow to the Lovashi.
        ->7b

=== 7b ===

Because that is the only way it can end! With one side victorious, and the other subservient! If that never comes to pass, we will be locked in a stalement for eternity! 

    +Your Director wasn't so sure. He spoke of a cycle, and he told me he wished for a way to end it. What if I told you my goal is to prove it can be done.
            ->7c
    
=== 7c ===

You quickly stray from reality. The Confederation will never stop until every horse is freed from the Artisans' yoke, and you are all scattered across our lands as serfs: powerless to ever harm us again.

    +There may be a way to free all the horses without continued conflict. But first the cycle must be broken. And that starts here, with both of us finding a way to leave this room alive.
            ->7d
    
=== 7d ===

Idiocy. Plain and pure.

    +Your duty is to the Confederation. And the Confederation fights for the freedom of all horses. If there is a way to free them, however unlikely, your duty is to explore it. Or are your claims of morals just empty words?
            ->7e
    
=== 7e ===

My duty is to prevent you from harming anyone ever again. Not to chase after the mad fantasies of one of the branded. 

    +How can you be so sure it is impossible?
        ->7f
    
=== 7f ===

Because it takes conviction to face the evils of the world head on. To plant your feet and attempt to right them. You blame us for doing what's necessary, while lacking the conviction to do it yourself.

    +I believe we are equal in our conviction. You clearly consider yourself a man of morals. Haven't you ever wished there was another way?
        ->7g
    
=== 7g ===

Of course I have. But your people have proven that there isn't.

    +\*Remove any weapons you are carrying.* Then you are faced with a choice: prove to me that the cycle cannot be broken by attacking while I am unarmed and professing peace, or allow me to prove to you I will not harm you by approaching peaceably.
        ->7h
    
    +Fine. If you're not going to listen to reason then we will have it your way. <Combat>
        ~attackedTabor = true
        ->killTabor
    
=== 7h ===

\*Tabor grips his sword tightly, but his brow is furrowed in contemplation.*
    
    +Your method is pain. Mine is trust. Please, allow me to build it first.
        ->7i
    +\*Pick your weapons back up and sigh.* You don't look like you're ready to accept my olive branch. <Combat>
        ~attackedTabor = true
        ->killTabor

=== 7i ===

\*Tabor hesitates a moment longer, but then lowers his weapon and nods.*
    
    +\*Approach with your hands up.*
        ->9i

=== 8a === //What would be the purpose of such a con at the very end?

I fail to fathom the motives for most of what you people do. This is no different.

    +Have you never been where I am? With overwhelming force at your back, have you never attempted to take a cornered runaway alive?
            ->8b

=== 8b ===

Over my career, I have had cause to hunt down many branded who slipped their bonds. I have always tried to take them alive.

    +And why? Simply to interrogate them for accomplices?
        ->8c

=== 8c ===

That was a portion of it, yes. But I have always firmly believed that a branded who dies before they learned all they could from me is a branded I have failed.

    +Then you and I are of the same mind. If you should die here, I will have failed you.
        ->8d

=== 8d ===

I don't understand your meaning.

        +The Director spoke of a cycle, and he told me he wished for a way to end it. What if I told you my goal is to prove it can be done.
            ->9e

=== 9a ===

Perhaps I could see you taking such a deal. But could you enforce it? With the riot over, what is to prevent the others from killing me when your back is turned?

    +I will place you under guard. There are those I trust among the branded who will hold to my command over their personal grudges.
        ->9b

=== 9b ===

So I am to become your prisoner then? For how long? What humiliations will I suffer once you have me in manacles?

    +Of course, you speak now of humiliations when it is you who is powerless. What of the branded you had in your clutches mere hours ago? Did you wish to prevent humiliations then?
        ->9c
        
    +When we have made it to civilization, and the other branded have gone their separate ways, I would free you. The entire ordeal would be over in weeks at most.
        ->9ba

=== 9ba ===

Even if I could trust you to keep to your word, I cannot bring myself to spend weeks of my life living as a slave to a slave.
        
    +This is not an attempt at revenge, but an attempt to save your life. You would not become a slave.
        ->9bb

=== 9bb ===

And why would you want that? After all of the killing, you come to me claiming to want peace?

    +I never wanted the killing, I simply wanted an end to our torment. And with the Director defeated and our servitude over, I now wish to end the killing too. 
        ->9bc

=== 9bc ===

Then leave. Take the rioters from the Manse and let me return to the Confederation.

    +I doubt the rioters would heed my call to leave you be. Their grudge with you is too great. But if you become my prisoner, I can keep you safe long enough to convince them.
        ->9bb

=== 9bd ===

\*Tabor still looks dubious.* So that's it then? You'd let me live? To go back to the Confederation and start everything over again?

    +Is that what you want? To return to the Confederation and continue as a slavemaster?
        ->9d

=== 9c ===

I used humiliation to teach humility. The pain I inflicted was to imbue you with a sense of right and wrong. Whatever evils I committed on your comrades were committed to make the world a better place.

    +After everything you've done, you still think you were helping us? If you could leave here unimpeded, would you continue to own and 'teach' slaves?
        ->9d

=== 9d ===

Absolutely. My work may never be finished, but it requires doing. I will never give it up.

    +I can not allow that. <Combat>
        ~attackedTabor = true
        ->killTabor
    +The Director spoke of a cycle, and he told me that there was nothing I could do to stop it. What if I told you I am going to prove it can be done.
        ->9e
        
=== 9e ===

A commendable goal, but I doubt you will succeed as you are. You lack the conviction to affect the changes you wish to see.

    +I believe we are equal in our conviction. The difference between you and I is our methods. You clearly consider yourself a man of morals. Haven't you ever wished there was another way?
        ->9f


=== 9f ===

Of course I have. But your people have proven that there isn't.

    +Then prove to me you are nothing like what you see in us. Let calm discourse win the day over bloodshed.
        ->9g

=== 9g ===

\*Tabor grits his teeth, and stays silent for an awkwardly long time.* 

    +\*Remove any weapons you are carrying.* Your method is pain. Mine is trust. Allow me to build it first by approaching, unarmed.
        ->9h
    +Fine. If you're not going to listen to reason then we will have it your way. <Combat>
        ~attackedTabor = true
        ->killTabor
        
=== 9h ===

\*Tabor grips his sword tightly, but nods the affirmative.*
    
    +\*Approach with your hands up.*
        ->9i
    +\*Pick your weapons back up.* You don't look like you're ready to accept my olive branch. <Combat>
        ~attackedTabor = true
        ->killTabor
        
=== 9i ===

fadeToBlack()

moveToLocalPos(1.5,27.125)
changeCamTarget({taborIndex})
setFacing(NorthEast)

fadeBackIn(60)

\*Tabor watches you approach.*

    +\*Extend your hand.* I am defenseless, and in your power. You could harm me, or take me hostage if you like. But the easiest way forward is to give me your sword.
        ->9ia
        
=== 9ia ===

\*Tabor looks from you, to your hand.*

    +The only way we both leave this Manse alive is if you relinquish the power. As I have done.
        ->9j
        
=== 9j ===

\*The Lovashi Chief looks as if every muscle in his body is fighting his mind as he turns his sword over and places it's hilt in your hand.*

    +Thank you. You are now my prisoner. Turn around and allow me to bind your hands. \*Lower your voice.* The others won't understand unless we make it look official.
        ->9k
    +\*Thrust Tabor's sword into his gut.*
        ->9l

=== 9k ===

\*Tabor nods and turns around.*
    ->acceptTaborSurrender

=== 9l ===

\*Tabor's eyes bulge as the sword cuts deep into his stomach. His powerful hands go for your throat.*

    +\*Twist the sword.*
        ->9m

=== 9m ===

\*Your victim's body goes rigid, paralyzed by the pain. As his blood and energy ebb from his wound, his eyes meet yours.* Part of me really thought... \*He coughs, spraying your torso with blood and spittle*... you meant it. Your methods... trust...

    +\*Twist the sword again.*
        \*Another jolt of pain surges through Tabor's body. He then falls away, sliding off the sword and landing wetly at your feet. He does not get up.*
            ~attackedTabor = false
            ->killTabor
    +\*Lean closer.* How could I have? 
            \*Tabor tries to renew his attack, but his body no longer can muster the strength. He accomplishes nothing more than soiling your clothing with his blood, before falling into a slump at your feet. He does not get up.*
            ~attackedTabor = false
            ->killTabor

=== acceptTaborSurrender === 

    ~letTaborLive = true
    setToTrue(letTaborLive)
    setToTrue(acceptedTaborsSurrenderAfterDirectorFight)

    activateQuestStep(An Uneasy Truce, 4)

    fadeToBlack()
    
    deactivate({taborIndex})
    
    fadeBackIn(60)

    ->Close

=== killTabor === 

setToFalse(letTaborLive)

{
-directorDefeated:
    finishQuest(An Uneasy Truce, true, 3, true)
-else:    
    finishQuest(An Uneasy Truce, true, 1, true)
}

kill({taborIndex})
setToTrue(killedTaborInManse)

{
-attackedTabor:
    ->Combat
-else:
    ->Close
}

=== Combat === 

enterCombat({taborFightIndex})

->Close


=== Close ===

close()

->DONE