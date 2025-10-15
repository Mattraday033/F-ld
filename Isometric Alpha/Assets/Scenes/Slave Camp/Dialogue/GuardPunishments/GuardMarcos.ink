VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR spokeWithMarcosAtPunishment = false

VAR gaveAGuardToTheCrowd = false
VAR executedAnyGuard = false
VAR didNotExecuteMarcos = false
VAR gaveMarcosFiftyLashes = false

VAR marcosIndex = 1
VAR crowdIndex = 2

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

changeCamTarget({marcosIndex})

{
-spokeWithMarcosAtPunishment:

keepDialogue()

You have returned. Is it time?

    ->1b
}

setToTrue(spokeWithMarcosAtPunishment)

{
-gaveAGuardToTheCrowd:
->1ab
-executedAnyGuard:
->1aa
-else:
->1a
}

=== 1a === 

\*Márcos looks up at you from his position on his knees. He has been stripped of his armor, and you can see fresh bandages have been applied to his many wounds.* Hello, {playerName}. I am glad to see you made it through the riot relatively unhurt. 

    +And I am glad to see you have recovered some from your wounds.
        ->1b
    +Quiet. You will only speak when addressed.
        keepDialogue()
        \*Márcos silently nods.*
        ->1b

=== 1aa === //executedAnyGuard

keepDialogue()

\*Márcos looks up at you from his position on his knees. He has been stripped of his armor, and you can see fresh bandages have been applied to his many wounds.* There was a part of me that knew supporting your plans would lead us here. What is to be my fate, then? I will not shy from it, whatever your decision.

        ->1b

=== 1ab === //gaveAGuardToTheCrowd

\*Márcos looks up at you from his position on his knees. He has been stripped of his armor, and you can see fresh bandages have been applied to his many wounds.* Are all of our fates to be so bloody? Are we to all be thrown to the mob, like meat to dogs?

    +Each of your verdicts will be determined independent of anyone else's actions.
        keepDialogue()
        That may be true, but I fear they will all be the same either way.
        ->1b
    +Quiet. You will only speak when addressed.
        keepDialogue()
        \*Márcos silently nods.*
        ->1b

=== 1b ===

I have regained some of my strength. I have Kastor to thank for that. He knows his craft well. 

    +I have some questions I must ask you.
        ->1c
    +Your actions in the mines have earned my respect. I would not have you on your knees while we celebrate. *Undo Márcos's bonds.*
        ->2a
    +Whatever your actions, you were still responsible for keeping us in chains. That demands punishment.
        ->3a
    +I will be back.
        ->Close

=== 1c ===

\*Márcos awaits your questions.*

    +What are your crimes as you see them?
        ->1ca
    +How did you come to be a guard at this camp?
        ->1d
    +Why did you rush towards the breach after you saved Kastor from the worms?
        ->1e
    +Why did you not follow Overseer Gáspár's orders to abandon the branded?
        ->1f
    +If granted your freedom, what will you do with it?
        ->1g
    +Do you have any regrets?
        ->1h
    +Will any of the freed vouch for your life?
        ->1i
    +I am finished with my questions.
        keepDialogue()
        What is your verdict to be?
        ->1b
    
=== 1ca ===

The list is long. For months before I came to this camp, I enthusiastically served as a disciplinarian. I took pleasure in attempting to ween branded slaves from their evil ways, and would often complain loudly of their stubborness made <i>my</i> work more difficult.

By the time I had arrived here, I was frustrated by my lack of progress. Confused at how so many of my comrades didn't seem to care. In the early months of this camp's existance, I tried to bury myself in my work, helping Chief Tabor and others keep order. I redoubled my efforts to use the punishments I inflicted to make an impact on my charges.

keepDialogue()

But by the time the worms breached the mine, my confusion had overcome me. I no longer knew what to believe in.

    ->1c


=== 1d ===

It is a lengthy tale. Are you certain you want to hear it?

    +I'm certain. Tell me.
        ->1da
    +In that case, let me ask another question.
        ->1c

=== 1da ===

I am the third son of a wealthy landowner back in the Confederation. My family did not expect much from me, nor did I have many obligations to them, so I sought to make a career as a soldier. Since my youth, I had always thought it would be a glorious way to live. Rugged and dangerous, of course, but steeped in heroism and adventure as well. 

My upbringing could have afforded me an officer's commission, but I decided to enlist as a common soldier. I suppose back then I thought it would get me closer to the action. The Confederation had been at peace for years at that point, you see, so there wasn't exactly a lot for a soldier to do.

We spent months drilling and marching about. When the word finally came that rebels were attacking patrols out in the countryside, I was ecstatic. At least, until we came across the remains of another of our county's units. They had been ambushed in camp; we could see from how they had fallen that they hadn't had time to form any kind of cohesion. Some of them didn't even have armor on, or perhaps had been stripped before we had arrived.

The veterans of my formation had warned me of the smell. And it was horrific, but I had prepared myself for it. What I wasn't prepared for were their eyes. Those that hadn't been gotten to by crows had such a stillness to them. A mundanity, like an object. And the ones the crows had been at had no eyes at all. Just gaping holes.

Once we had some time to survey the carnage, my captain pointed out that there were no horse bodies. A formation of this size would have had a compliment of bonded horses, or at least their commander would have been horsed. After we set about looking for them, we could see none had been left behind. Large trails of bent grass told us where they had gone. The rebels had dragged them back to their camps. 

We crept through the brush, and found the rebels celebrating. Drunk on wine, and roasting the horses on spits set over large campfires. This affected me most of all. At my father's estate, we had hosted many horses. I speak the horsetongue fluently. I had ridden them before, befriended them. And these cannibals had singled them out to eat like wolves and fawns.

My captain would later take the credit for leading the charge that routed the rebels, but I was told by my sergeant he had followed me into the fray. After that battle, I thought I understood my lot in life. I thought it was the purpose of all able-bodied Lovashi to fight for the Confederation, to contribute to the Emancipation of all horses. To save them from those that treated them like beasts.

keepDialogue()

Eventually, after the rebellion had been crushed, a messenger came to the garrison looking for volunteers to work as guards on the Director's estate. In the Confederation, during peacetime, wealthy officers can buy soldiers out of their enlistment. I normally wouldn't have volunteered, but the Director is something of a legend. I thought working for him would provide more opportunities to fight for the cause. I signed up and two years later here I am.

    ->1c

=== 1e ===

I don't understand the question. My job was to protect the camp and it's personnel.

    +So that's the only reason? Duty to the Director?
        keepDialogue()
        
        \*Márcos hesitates for a moment.* No, that wasn't the only reason. I knew that there were other guards and slaves at that end of the tunnel. They were in peril. I thought I could help.
    
            ->1e
    +Kastor said you were limping. Why not retreat with him to the upper floors?
        keepDialogue()
        
        I was injured, yes. But I still had the power to save lives. What use is a sword and the knowledge to use it if you flee at the first sign of peril? When the moment comes to run towards or away from conflict, I hope I will always choose the former.
    
            ->1e
            
    +Let me ask another question.
        ->1c
        
=== 1f ===

Overseer Gáspár's decision was callous and self-serving. There was ample food in the stockroom to feed all of the survivors for some time, and should we have run out, we could have taken to catching bats or even attempting to eat the worms we had slain. To immediately sentence all the branded to death was beyond cruelty.

    +But the brand is already a death sentence. Why would a guard care if they lived or died?

    ->1fa

=== 1fa ===

That is true, but as Chief Tabor would say: "the point of the branding is not simply to kill". It is to both force the offender to work towards the goals of the Confederation, and to teach them that those goals are worthy ones. Summary execution robs the branded of the opportunity for atonement.

    +Have you ever seen a branded embrace that 'atonement'?
        ->1fb

=== 1fb ===

No. Not truly. I have seen some attempt to fake it, but their sincerity is always revealed to be false eventually. 

    +Then why continue supporting such a flawed system?
        ->1fc
        
=== 1fc ===

I have asked myself that question many times. After so much time spent observing our system in action, I still believe it is possible to teach the moral of equine equality, but the system forces it's victims to hate their teachers.  And if the student hates the teacher, the student will reject anything their teacher says out of hand, no matter how obvious it may seem to either party.

When Gáspár gave the order to abandon the branded to the worms, it was in conflict with what I understood the purpose of the branding to be. I had always told myself the point of all the pain I had inflicted was to teach. But if we killed the branded before they could accept our truth, what was the point of all the suffering?

While I was stuck down in the mine, I was forced to come to terms with why I rejected his orders. I forced myself to confront the feelings I had felt simmering within me for a long time. If the lessons we taught are correct, but all of the branded rejected our lessons, then perhaps our methods were at fault. And if our methods were the problem, then we had inflicted so much suffering for nothing.

keepDialogue()

This is why when Nándor told me of his plan to escape, I did not attempt to disuade or stop him. It had become so obvious to me why he would want that, and how truly immoral it would have been for me to stand in his way.
        ->1c

=== 1g === //    +If granted your freedom, what will you do with it?

I would like to stay with the branded. Now that I know we are within the Kingdom of Masons, I feel they have a real chance to build new lives, and I wish to be a part of that. But I will admit it is not purely for altruistic reasons. I do not think I can bring myself to return to the Confederation, and I doubt the Kingdom of Masons will be hospitable to someone like me without the branded to vouch for me. 

keepDialogue()

I want to begin to make amends for what I have done to them, and I now find myself reliant on their forgiveness in this new land. From now on, I feel our fates are tied together intrinsically.
        ->1c

=== 1h === //do you have any regrets

keepDialogue()

I have so many. I regret my part in keeping the branded in chains. I regret how long it took for me to understand what I was doing is wrong. I regret how I was unable to make any of my fellow guards see the same truth I see now. And most of all, I regret I was unable to do more to assist in your liberation.
        ->1c

=== 1i ===

keepDialogue()

Yes, I believe Kastor, Carter, and Nándor would. There may be more, but those are the three I am most certain of.
        ->1c

=== 2a === //*Undo Márcos's bonds.*

\*Márcos gets to his feet.* Thank you. I had wondered until this moment whether any of you could find me worthy of forgiveness. I suppose I will never stop wondering if you were right to.

    +\*Address the crowd.* This is Márcos. His actions were integral to our victory. If you hold any esteem for Carter or Nándor, hold that same esteem for him as they owe him their lives.
        
        ~didNotExecuteMarcos = true
        setToTrue(didNotExecuteMarcos)
        
        changeCamTarget({crowdIndex})
        
        \*The crowd erupts in cheers.*
        
        activateQuestStep(Deal With the Prisoners, 3)
        
        fadeToBlack()

        deactivate({marcosIndex})
        
        fadeBackIn(60)
        
        ->Close



=== 3a === //punish Márcos

{
-gaveAGuardToTheCrowd:
\*Márcos hangs his head. Prayers can be heard from under his breath.*
-else:
\*Márcos hangs his head.* I understand. I will not resist whatever punishment you deem fit.
}

    +Fifty lashes should do it. If you're still cogent after that, you can have your freedom.
        ->3b
    +The only punishment befitting a slaver is death. At least for you it will be quick.
        ->3c
    +Let the crowd sort you out. If they decide you should live, then so be it.
        ->3d
    +On second thought, I don't really see the point in punishing you.
        
        keepDialogue()
        
        \*Márcos looks up at you in surprise.*
    
        ->1b


=== 3b ===

setToTrue(didNotExecuteMarcos)
setToTrue(gaveMarcosFiftyLashes)

activateQuestStep(Deal With the Prisoners, 4)

Were our places reversed, and you were in the hands of a younger me, your fate would not have been this merciful. I find your verdict more than fair.

fadeToBlack()

deactivate({marcosIndex})

fadeBackIn(60)

->Close

=== 3c ===

setToTrue(executedMarcos)

activateQuestStep(Deal With the Prisoners, 5)

My death has been a long time coming. I know what I have done to deserve this punishment, although some part of me had hoped it would not come to this. I will not resist.

fadeToBlack()

kill({marcosIndex})

fadeBackIn(60)

->Close

=== 3d ===

setToTrue(gaveMarcosToTheCrowd)

activateQuestStep(Deal With the Prisoners, 6)

\*Márcos looks up at you. You can see true fear in his eyes.*

fadeToBlack()

kill({marcosIndex})

fadeBackIn(60)

->Close

=== 3e ===

->Close
 
=== 3f ===

->Close

=== 3h ===

->Close

=== 3i ===

->Close

=== 3j ===

->Close

=== 3k ===

->Close

=== 3l ===

->Close

=== 3m ===

->Close

=== 3n ===

->Close



=== Close ===

close()

->DONE