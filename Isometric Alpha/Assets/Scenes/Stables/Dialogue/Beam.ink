VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR metBeam = false
VAR askedBeamAboutWhittling = false
VAR pissedOffBeam = false
VAR askedAboutMangledName = false
VAR givenHorsetongueGuideFromBeam = false

VAR gotThePlanFromKastor = false
VAR spokeToBalint = false
VAR spokeToJanos = false
VAR spokeToErvin = false
VAR spokeToGarchaAboutPlan = false
VAR metKastor = false



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
-pissedOffBeam:
->pissedOffBeamConvoEnd
-metBeam:
->1aa
-else:
->1a
}
=== 1a ===

setToTrue(metBeam)

\*This man leans against a fence post, whittling a small figurine from a chunk of wood. He doesn't look up from his work until you are directly next to him.* Hello, can I help you?

->1b

=== 1aa ===

\*Beam continues his whittling, waiting for you to speak.*

{
-askedBeamAboutWhittling:
    ->2a
-else:
    ->1b
}

=== 1b ===

+What have you got there?
    setToTrue(askedBeamAboutWhittling)
    ->1c
+I have to get back to work.
    ->Close

=== 1c ===

This? *The man shows you his work. The figurine is of a man astride a horse, with armor in the Lovashi style. The legs of the horse have yet to be hewn from the rest of the block.* It's the Director, on his horse Csalan.
    ->1ca

=== 1ca ===

    +I've never met the man. What's he like?
        ->1d
    +Is this your job? To sit around and whittle?
        keepDialogue()
        I don't appreciate your tone. But I agree with you that it can appear that way. My job is to wait on the horses of the camp. If they need something, I'm supposed to be there to be their hands. And they don't mind the whittling, there isn't much to do when I'm not serving them. Horses are stoic creatures to the last, they don't require much. 
        ->1c
    +Does he know you make these figurines?
        ->1f
    
=== 1d ===

He's well known in the Confederation. He fought in the Emancipation Conflict between the Lovashi and the Craft kingdoms. His warriors cut deep into the western regions of the Kingdom of Masons, going so far as to set seige to the city of Rudra, on the Wandering Roil river.

Another story I heard tells that at the end of the war, when things weren't going so well for us, his soldiers held out in a captured Craft Folk fort for many weeks against a much larger force. I've heard it said the Craft Folk made over a dozen attempts take the fort back, and each time they were turned away.
    ->1da

=== 1da ===

    +I meant more of what is he like as a person.
        ->1e
    +How did he make it out?
        The war ended. The story goes that we negotiated a peace that allowed for our soldiers to come home, and that included the warriors under his command.
        ->1da
    +Why is it called the Emancipation Conflict?
        The Lovashi fight to emancipate all horses in bondage in Craft Folk lands. I've heard that the horses that the Craft Folk raise can't even speak. They're treated like dumb animals, attached to yokes like oxen and made to plow their fields or pull their wagons.
        ->1da

=== 1e ===

Oh, well I don't know him personally. I'm just a servant that was hired to tend to the horses of his household. But he pays well, and always on time. And Csalan holds him in high regard, of course, so I expect he is as great as the stories say.

->1fb

=== 1f ===

No, but Csalan has seen them. He says my figures do him and the Director credit.
    ->1fb

=== 1fb ===

    +You speak as if you can talk to the horses. Why is that?
        ->1g
    +Then what is Csalan like? You're closer with him, I assume?
        ->1k

=== 1g ===

I'm fluent in the horsetongue. My family has served horses for three generations. The Gods gave the Lovashi horses, and the horsetongue to speak to them, but anyone can learn it with enough patience.

    +But 'Beam' is a Craft Folk name, isn't it? How is it that you know the horsetongue?
        ->1h

=== 1h ===

Well spied. Since you brought it up, I will say I always thought it was funny how the Craft Folk name themselves after such material things. 'Beam', 'Thatch', 'Clay', all just things you might make a house out of. 

The Lovashi have a much more complex naming system, and are mostly named after some famous person from their history. But you can usually tell a name is Lovashi because of all the accented vowels they borrow from the horsetongue.

As for why I can speak it? My family have been servants to the Lovashi for a long time. Some enterprising family member a long time ago saw the writing on the wall and learned the language to get a leg up, and it's served us well ever since.

->1ha

=== 1ha ===

    +You seem proud of being a servant of the Lovashi. It would bring me shame to work for them.
        ->1j
    +You know much about language. If I asked you about some names I've heard, could you tell me about them?
        ->1i
    +I need to get back to work.
        ->Close

=== 1i ===

Of course! This is a topic I enjoy, so ask away.

{
-askedAboutMangledName:
    +What would cause the name to be changed like that?
        It could be for a number of reasons. The Lovashi have been intermingling with the Craft Folk for a long time; well before they started fighting one another. Some Craft Folk find the Lovashi names and words to be hard to pronounce, so they change them. Sometimes accidentally, sometimes not.

        keepDialogue()
        
        Other Craft Folk purposefully take Lovashi names because they think the Lovashi will look more favorably on them. I'm not sure if that really works, especially if they end up pronouncing it wrong, but that doesn't stop them from trying it.
        ->1i
}

{
-gotThePlanFromKastor or spokeToBalint:
    +Bálint?
        ->lovashiName
}

{
-true:
    +Broglin?
        keepDialogue()
        Oh, I'm not familiar with that name. It's not Lovashi, that's for sure. It doesn't sound Craft Folk, either. Song Folk perhaps? Or Farm Folk?  
        ->1i
}

{
-gotThePlanFromKastor or spokeToErvin:
    +Ervin?
        ->lovashiName
}

{
-true:
    +Garcha?
        setToTrue(askedAboutMangledName)
        keepDialogue()
        I'm not sure, but I would guess that's a Lovashi name, but it's almost unrecognizable. If pressed, I would have to say it came from Géza. 
        ->1i
}

{
-spokeToGarchaAboutPlan or metKastor:
    +Kastor?
        setToTrue(askedAboutMangledName)
        keepDialogue()
        That seems to be a Craft Folk name, but it's been modified to sound more Lovashi. Many Lovashi names, 'Gábor', 'Tabor', 'Nándor', end in 'or'. I think the original name is 'Caster', like to cast something in a mold. Or perhaps to cast a net, as in to fish.
        ->1i
}

{
-gotThePlanFromKastor or spokeToJanos:
    +Janos?
        setToTrue(askedAboutMangledName)
        keepDialogue()
        Hmm, I believe that's a Lovashi name, but it's been mangled some. János is the correct form. 
        ->1i
}


    +I think that's all my questions
        I hope I was helpful.
        ->1ha

=== lovashiName ===

keepDialogue()

Easy, that's a Lovashi name.

    ->1i

=== 1j ===

I would not expect a branded to understand it. The Lovashi won't be leaving any time soon. My family eats well off the work the Lovashi provide. If you had sense, you'd have learned that before you were branded. Now, it's too late for you.
    ->pissedOffBeamConvoEnd


=== 1k === //Then what is Csalan like? You're closer with him, I assume?

It would be very presumptuous of me to claim we are 'close'. Csalan is a horse of high standing: many of his family line have been the steeds of counts and heroes. I'm just his servant. As for what he is like, I have nothing but praise for him. He is a powerful warhorse, an excellent leader, and an accomplished poet.
    
    +It sounds like you admire him.
        ->1l
    +The Director's horse writes poetry in his spare time? Now this I've got to hear.
        keepDialogue()
        It would be quiet gauche for a servant to betray their lord's musings without their permission. You won't hear them from me.
        ->1k
    +He's not here, you can tell me how you really feel.
        I've been nothing but truthful. Just because you cannot muster such praise about anyone in your sad life doesn't mean others cannot.
        ->pissedOffBeamConvoEnd

=== 1l ===

Very much so. There is little he cannot accomplish when he sets him mind to it.

{
-not givenHorsetongueGuideFromBeam:
    +Could you teach me the horsetongue?
        ->1m
}

    +I need to get going now.
        ->Close

=== 1m ===

setToTrue(givenHorsetongueGuideFromBeam)

prepItem()

I could, but I'm not certain that is the best use of either of our time. I can, however, provide you with this. It's a pronouciation guide for the horsetongue. It should at least prevent you from making a fool of yourself when you say someone's name.

giveItem(7,6,1)

Now if you'll excuse me, I'm sure you have important things you need to get back to. Please leave me to my whittling.

->Close

=== 2a ===

+Can I the progress you've made?
    ->2b
+I have to get back to work.
    ->Close

=== 2b ===

\*The man shows you his work. The figurine is of a man astride a horse, with armor in the Lovashi style. The legs of the horse have yet to be hewn from the rest of the block. He's only made a little progress.* I've finished the Director, and now I'm working on his horse, Csalan.
    ->1ca

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

=== pissedOffBeamConvoEnd ===
setToTrue(pissedOffBeam)

Leave me. You've soured the conversation, and I'm sure you have work you need to get back to.

->Close

=== Close ===

close()

->DONE