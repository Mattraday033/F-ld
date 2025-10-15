VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR directorIndex = 1
VAR pageIndex = 2
VAR carterIndex = 3
VAR nandorIndex = 4

VAR directorDefeated = false
VAR directorSaidAbsurdityQuote = false
VAR allowedDirectorToSpeak = false

VAR deathFlagCarter = false

VAR sitting = true

VAR dialogueKeyForAfterDefeatingDirector = "directorDefeatedConvo"

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

->1a


=== 1a ===

fadeToBlack()

changeCamTarget({directorIndex})

activate({carterIndex})
activate({nandorIndex})

fadeBackIn(60)

    \*A man, his hair grey, his armor made for someone larger, sits behind a desk. He stares at a large banner, hung on the wall and made of cloth as green as steppe grass with the golden symbol of the Lovashi Confederation emblazoned upon it. After a moment, he turns to you.* To have made it past my guards is impressive. You'll forgive me if I don't stand to greet you. It's my leg, you see. It took a spear beneath the walls at Rudra, and it's been flaring up again.

        +You're the Director? I expected someone a bit more... regal.
            ->1aa
        +I suppose after all you've put me through to get here, a little courtesy would have been too much to ask.
            ->1b
        +Don't trouble yourself. You'll be dead in a moment anyways. <Combat>
            ->lastLineB4Combat

=== 1aa ===

    That word may have described me at one time. Now, I am old and haggard, but my countrymen are too polite to stop using it. I understand you are here to kill me, but if you would indulge me a few more moments of life, I would speak with you.
        
    +What is there to discuss? You are going to surrender to me, without conditions, or you are going to die.
        ->2a
    +I owe you nothing. Let's just get this over with. <Combat>
        ->lastLineB4Combat

=== 1b ===
~sitting = false
\*The Director closes his eyes for a moment, and takes a deep breath. Then, with some effort, he stands up from his chair and faces you, clearly favoring his right leg. He gives you a well practiced bow, and the way his body wobbles you expect he bowed as low as he could manage.* I meant no discourtesy. I am Lord Gábor Kálnoky, uncle to Count Béla Kálnoky. From one warrior to another, you have done well to make it this far.

    +A lord, bowing to a slave? You are an absurd man.
        ->1c
    +\*Return the bow.* Your manners, despite the circumstances, do you credit. 
        And yours only serve to elevate my opinion of you. 

        combineDialogue()

        I would not rob you of your prize.
        ->1d

=== 1c ===

setToTrue(directorSaidAbsurdityQuote)

Over my life, I've often found that calling something absurd simply betrays your ignorance on the subject. I bow to you because you have defeated my guards. Such an accomplishment should be recognized.

    +Whatever the case, it won't delay the inevitable. Surrender, or we will attack.
        ->1d
    +Your guards, and now you. Defend yourself, Director. <Combat> 
        ->lastLineB4Combat

=== 1d ===

We will fight. Be certain of that. But there is no rush. I cannot leave this office, except through you. Would you mind if I sat down again?

    +I see little harm in it. Sit if you wish.
        ~sitting = true
        combineDialogue()
        Thank you. *The Director relents back into his chair.* 
        ->2a
    +I will hear you out, but you will stand. A little discomfort before your defeat may teach you some humility.
        ->1da
    +If fighting is inevitable, then I choose to strike the first blow. <Combat>
        ->lastLineB4Combat
    +The sooner you die, the sooner your guards' will to fight will be broken. Let's get this over with. <Combat>
        ->lastLineB4Combat

=== 1da ===

combineDialogue()

\*The Director frowns, but does not sit.* Your terms are harsh, but fair. To begin, 

    ->2a

=== 2a ===

 I don't think it is any secret that we are not evenly matched. I am alone, and I am old. It appears to me that you have won the day. How does it feel?

    +The branded will no longer suffer. I am contented with that.
        A heroic sentiment. It can be an intoxicating feeling, to fight for what you see as right. When one's people look to you to guard them from suffering, I find few can turn away.
        ->2b
    +It's but one more struggle the Gods have put in my way. Nothing more.
        combineDialogue()
        You make it sound as if the day's events aren't out of the ordinary for you. Do you fight your way clear of prison camps regularly? Whatever the case, soon your people will no longer be imprisoned here.
        ->2b
    +Wonderful. Freedom waits, but for one windy geezer.
        {
        -sitting:
            \*A brief chuckle escapes the Director.* Your impertanence is rare in my circle. I'll cleave to the point, then.
        -else:
        
            combineDialogue()
            Fine. I'll cleave to the point, then.
        }
            ->2b
    +I ache from my wounds. I am ready to see an end to the fighting.
        I know that feeling well. It is universal among warriors. One inevitably dies in battle, or fights long enough to wish it to end.
        ->2b



=== 2b ===

All my life, I've fought to keep my people safe from those that would do them harm. If you'll forgive the familiarity, I see much of myself in you. 

    +You sit here, as the head of a camp of slaves, and liken yourself to me? How can you believe that?
        ->2c

=== 2c ===

    I understand your apprehension. I couldn't fathom it either, in yesteryears, the similarities between the Craft and Riding Folks. Only recently, did it dawn on me. 

    I fought in the last war between our peoples. The Confederation, despite it's faults, spends all of it's energies, makes each of it's decisions, with one goal in mind. The freeing of every horse that the Craft Folk keep as slaves on their farms, or in their armies. The Emancipation Conflict is what we call the series of wars we have waged to that end.

    Fifteen years ago, I had the privilege of leading a horde of our riders against the Kingdom of Masons. We struck deep into their land, freeing the feral horses they kept as we went. The ones that were too old to rehabilitate, we sent to sanctuaries, to live out their days as honored guests of the Counts. Those that could be taught to speak, we welcomed home as freed prisoners of war.
        ->2ca

=== 2ca ===

    +What do you mean by 'rehabilitate'?
        ->2cc
    +I would hear more of this conflict from someone who was there.
        ->2cb
    +But you took many slaves for your troubles, I'm sure.
        ->2d

=== 2cb ===

At first, the war went well. We had struck with little warning, and swept aside the meager resistance the Masons could offer. My horde had traveled west, and got as far as the great city of Rudra, on the Wandering Roil river. Then, things began to go wrong.

My force could not take the city. We were kept from fully encircling it by the great river the city was built upon. Without encircling it, we couldn't prevent the city from restocking it's provisions. We were stuck beneath it's walls, forced to take the time to build the war machines necessary to break them. And while we waited, the Masons sprung their trap.

A clever Mason general attacked our camp at night, and somehow had alerted Rudra to his plans. Rudra's defenders joined the fighting and, assaulted on both sides, my horde was badly pressed. I managed to extract a large portion of them from the jaws of the trap, but not before I received the wound that still plagues my left leg. Gone was any chance of taking the city. Bloodied and demoralized, I was forced to retreat.

Never again would our forces venture that far west. The Kingdom of Smiths, neighbor to the Masons, entered the war shortly after. They quickly took the Masonic Gap from us, and trapped our hordes inside the Kingdom of Masons. With no way home for my riders, and enemies on all sides, we hid away like rats within a captured Mason fort. After weeks of horrific fighting, the news came that our side had sued for peace, and we were allowed to return home.

    ->2ca

=== 2cc ===

All horses have the potential for intelligence. But the horses the Craft Folk keep have the intelligence of unspeaking beasts. This is because they are not taught the horsetongue, the language the god Beast gave the Lovashi to speak with horses.

Human children and horse foals, if they are not taught to speak by a certain age, forever lose the ability to learn. And without language, they cannot learn to become truly sapient. When the Lovashi rescue horses from the Craft Folk, they are immediately brought before teachers who attempt to impart the horsetongue to them. If they cannot learn it, they are sent to special santuaries, to be cared for until they pass away.

    ->2ca

=== 2d ===

setToTrue(allowedDirectorToSpeak)

Yes, many prisoners received their brand in the wake of my horde. But I think you'll find my complicity in this as the final similarity between us, in time. I expect you've already learned this camp rests on Mason land? You'll leave this camp victorious, and join their kingdom as a lost cousin returning in triumph. They'll welcome you into their homes, feed you from their larders, and ask you to tell them stories of your travails, as my people once asked of me.

Enjoy it while it lasts, that feeling of heroism is well earned. But you are familiar with the signs of oppression now; I'm sure my guards have taught you well. You will one day look up from the tables of your hosts to spy the horses in their fields, and you will see the harnesses hitched to their flesh. When that time comes, you will know the true worth of your sentiments: whether you speak up for them, or look away.

    +I will fight for the oppressed no matter where I find them.
        ->2e
    +I am not responsible for all the world's woes. There will be others who take up that calling.
        ->2g
    +That decision will be made when I have heard both sides. For now, all I have is your twisted perspective.
        The decision is yours to make. One difference between us is I will not judge you for yours, as you seem to have done for mine. But should you choose to continue to fight slavery among the Masons? 
        ->2e
    +I will not be lectured on morality by a slaver. This conversation is over. <Combat>
        ->lastLineB4Combat

=== 2e ===

Then I will salute you as a comrade. For that's how the Masons will see it. You may not know their perspective on the matter, but I have learned it well from my discussions with the prisoners we took. They view horses as tools, or even as weapons. They use them in their agriculture, or as mounts in their armies. For them, losing horses means starvation, or surrendering to the Confederation. How does one fight for such a thing without sounding like a Lovashi spy?

    +I am one of the branded. They will know I am no spy.
        ->2f
    +I don't need a solution now. I will find one after I have beaten you.
        ->2fa

=== 2f ===

    combineDialogue()

    That may give you an advantage, because no one would ever be so foolish as to brand themselves to go where they are not welcome. 
        ->2fb

=== 2fa ===

    combineDialogue()

    You speak true. 
        ->2fb

=== 2fb ===

And whether you believe it or not, hear me when I say that I hope you find the solution to the cycle of conflict I have fought my entire life. But should you fail, and my children take your head in your old age, I will be waiting at the hearth-here-after to ask you why. But we have put the inevitable off long enough. 
    ->lastLineB4Combat

=== 2g ===

If that is your answer, then perhaps I have misjudged you. You are more like your countrymen than I had realized, and that fills me with a great sadness. But we have put the inevitable off long enough.
    ->lastLineB4Combat

=== lastLineB4Combat ===

My friends gather at my hearth. I have kept them waiting all these years... to make them wait much longer would be the height of rudeness. Come at me.

    ->Combat

/*
=== 1a ===

fadeToBlack()

changeCamTarget({directorIndex})

activate({carterIndex})
activate({nandorIndex})

fadeBackIn(60)
//fadeBackIn(60, true)

//enableDialogueUI()

\*The Director sits behind his desk; his eyes are closed in pensive thought. He opens them as you enter the room.* I see you have defeated my guards. Did you leave any of them alive?

    +I tried to kill as few as possible.
        ->1b
    +Some.
        ->1b
    +Not if I could help it.
        ->1c
    +No.
        ->1c

=== 1b ===

Hmph, of course you did. A gentle pause in the cycle. But your compassion won't stop the wheel from turning, and the conflict starting anew later.

    +What are you talking about?
        ->1d

=== 1c ===

Then all is as it has always been. The cycle continues, despite all of the Confederation's attempts to the contrary.

    +What are you talking about?
        ->1d
    
=== 1d ===

I'm talking about the conflict between our peoples. The great conquest of the Artisan Kingdoms, the freeing from bondage of those they stole! You are the inheritor of a grudge going back generations, one which you perpetuate as you try to buck your own yoke.

    +You're not making any sense. It is natural that all slaves would want to be free.
        ->1e
    +I'm not going to engage with your lunacy. Time to die, villain. <Combat>
        ->1h

=== 1e ===

That is a given, but through your selfishness you think only of yourself, not of the greater good. Your people incurred a debt to us long ago, when they snuck into the Lovashi's settlements and stole horses to work their lands. They perpetrated the same slavery you suffer upon horsekind, kept the horsetongue from them and turned them into beasts of burden.

My people war to save all horses from that slavery, and see it as equal recompense that you and your children should work until the children of the horses you stole are returned to us. All of the industries of our Confederation, powered by your labors, are put towards the conquest of lands that use horses as laborers. 

You state it is natural that slaves should want to be free, but myopically you exclude equine slavery. I say, is it not natural that all sapient beings should want to extend that freedom to all other sapient beings?
->1ea

=== 1ea ===

    +"Equine slavery"? You're saying you think you should be able to own me because someone else owns a horse?
        ->1eb
    +I cannot be held responsible for the acts of my countrymen, or my forebearers.
        ->1f
    +I'm not going to engage with your lunacy. Time to die, villain. <Combat>
        ->1h
        
=== 1eb ===

As usual, you fail to see them as our equals. You as Craftsfolk may only know the muted animals you keep as beasts of burden, and think us insane for wanting them to be treated as a person. But you don't remember how your maltreatment caused them to be this way.

Do you know what happens to a child who never learns language? They remain forever stunted in their growth, unable to truly learn to communicate after a certain point in their development. So too have you treated horses, having stolen them for use in your fields and deprived them of ever learning the horsetongue.

Should you remain unconvinced, go to the stables in the camp. There you will meet true horses, who speak the horsetongue and can think and feel as you do. Perhaps then you will learn the compassion for their kind as my people have always known.

->1ea

=== 1f ===

Perhaps not, but all peoples of the world should be able to answer the call to end the evils of slavery, which your people invented. Those that act against the Confederation's wishes, those that are deviant to it's designs to end slavery, act in support of slavery. We of the Confederation see it as a fitting punishment to put into bonds those that would keep others in theirs.
    
    +You're allowing yourselves to perpetuate that same evil! You're all hypocrits!
        ->1g

=== 1g ===

I can live with committing evil against evildoers. What is punishment but righteous violence? Should the murderer escape the hangman? Should the thief leave the prison with both his hands? You are lenient because you have sinned, but those without sin have no need for leniency.

Enough of this. You have me cornered, but while I still have my wits and my sword I promise I will make you pay.

->1h

=== 1h ===

->Close

=== 1i ===

->Close

=== 1j ===

->Close

=== 1k ===

->Close

=== 1l ===

->Close

=== 1m ===

->Close

=== 1n ===

->Close

=== 2a === 

->Close
    
=== 2b ===

->Close

=== 2c ===

->Close

=== 2d ===

->Close

=== 2e ===

->Close

=== 2f ===

->Close

=== 2h ===

->Close

=== 2i ===

->Close

=== 2j ===

->Close

=== 2k ===

->Close

=== 2l ===

->Close

=== 2m ===

->Close

=== 2n ===

->Close

=== 3a ===

->Close

=== 3b ===

->Close
/*
=== 1c ===

The Lovashi cannot brook a slave revolt, even one as far flung as this. The only way you leave this camp without the possibility of my retribution is if you kill me now.

    +Spoken almost like a man who wants me to kill him.
        ->1d
    
=== 1d ===

changeCamTarget({pageIndex})

He does. If he does not die in the revolt, his superiors will surely hunt him down and do far worse to him.
    
{
-not deathFlagCarter:

changeCamTarget({carterIndex})

Page! It's good to see you still alive!

changeCamTarget({pageIndex})

Likewise, Carter. I see you've held up your end of the mission.

changeCamTarget({directorIndex})

Page? What is going on? Who is this slave and what are you talking about?

changeCamTarget({pageIndex})

Neither of us were ever yours, Director. Carter and I were inserted into your household long ago by the Kingdom of Masons to monitor highly placed Lovashi officers. You've been very useful, but now it seems that use is at an end.

-else:

changeCamTarget({directorIndex})

Page, do not speak. This is betw-

changeCamTarget({pageIndex})

No, my time taking your orders is at an end. Stay silent now, while those of import speak.

You there. I am an agent of the Kingdom of Masons, supplanted into the Director's household years ago to monitor his and other Lovashi officer's movement's. We fight for the freeing of all slaves from bondage, and you can trust my intentions.
}

If you kill him, you free him from a life hunted by the Lovashi. If you keep him alive, he could be a valuable asset to the Kingdom in our opposition to the Confederation.

->1da

=== 1da ===

    +Keeping him alive would rob every slave here of their revenge.
        It would, but his death may spell the demise of many slaves in the future. Surely postponing it for a time is worth that?
        ->1da
    +I need no excuse to prevent an execution. You can have him.
        ->1e
    +I've waited a long time to see him dead. I won't be thwarted at the last moment by you.
        ->Close
    +May you take him with my blessing, so long as you remember who allowed you to have him.
        ->Close

=== 1e ===

You've made a wise decision. I'm not in any place to give orders to those you lead, so would you please place the sla-, sorry, former slaves you trust the most on guard to prevent any of the others from taking their revenge before he can be moved?

changeCamTarget({directorIndex})

No wait a moment! I would rather die than submit to your custody. And while I am still armed, you won't take me quietly!

->Close
*/

=== Combat ===

setToTrue(directorDefeated)

enterCombat(0,{dialogueKeyForAfterDefeatingDirector})

    ->Close

=== Close ===

close()

->DONE