VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR snitchedOnUros = false
VAR hasIronNugget = false
VAR metQuartermasterEmese = false
VAR gaveIronNuggetToEmese = false

VAR taborMentionedRewardForHostages = false
VAR receivedTaborRewardFromEmese = false
VAR sentIntoMineByDirector = false
VAR trainedByEmeseToUseBlasingJelly = false
VAR toldToFindNandor = false
VAR mineLvl3KilledGuards = false

VAR playerName = ""

searchInventoryFor(hasIronNugget,Lost Iron Nugget)

->1a

=== 1a ===

setToTrue(metQuartermasterEmese)

Hello slave. Got an order for me?

->1aa

=== 1aa ===

{
-hasIronNugget:
    +I found what Uros was hiding. *Hand the Iron Nugget to Emese.*
        ->1e
}

{
-taborMentionedRewardForHostages and not receivedTaborRewardFromEmese:
    +Chief Tabor said you would have something for me?
        ->1f
}

{
-sentIntoMineByDirector and not trainedByEmeseToUseBlasingJelly:
    +I'm here to be trained to use blasting jelly. *Show seal.*
        ->1g
}

    +How is the lockdown treating you?
        The lockdown? I barely notice it. Supply tallies and requisitions don't stop just because the slaves aren't working. The biggest difference is now that the branded are stuck inside all day they're begging for stockhouse duty just to have something to do. I've never had so many volunteers! *Emese chuckles to herself.*
        ->1aa
    +If I need supplies, can I get them from you?
        Not without a guard's approval you can't. It would be best if you didn't concern yourself with such things and get back to work.
        ->1aa
    +\*Leave.*
        ->Close

=== 1e ===

~gaveIronNuggetToEmese = true

setToTrue(gaveIronNuggetToEmese)

finishQuest(Stockhouse Stash, true, The nugget retrieved.1)

prepForItem()

\*Quartermaster Emese examines the Iron Nugget.* Iron? Very interesting. When I tell the Director about this, Uros will be interogated to the fullest extent to find where he got it. You've done an excellent job.

takeItem(Lost Iron Nugget, 1)&
addXP(100,1)

\*Quartermaster Emese rummages around in a crate she keeps below her desk.*

prepForItem()

These gloves were meant to be distributed among the guards, but I think you've earned them. They should help keep blisters off your hands after a hard day of swinging a pick. If any guard or slave tries to take them, tell them I gave them to you and they'll be answering to me if they wind up stolen.

giveItem(2,5,1)

->1aa

=== 1f ===

setToTrue(receivedTaborRewardFromEmese)
finishQuest(Tabor's Reward,true,Reward received.)

prepItem()

Indeed. The chief told me to give you these. A clean robe, usually reserved for the kitchen servants. It should keep you warm much better than the rags they usually give the branded.

giveItem(2,29,1)

prepItem()

As well as a nice new pair of boots. Walking around barefoot you're bound to get your feet all cut up, but these will protect them nicely.

giveItem(2,11,1)

->1aa

=== 1g ===

setToTrue(trainedByEmeseToUseBlasingJelly)

\*Emese eyes the seal, then looks you up and down.* So you are. The Director sent a runner. It's quite unusual for one of the branded to be assigned for explosives training, but who am I to question the powers that be?

The blasting jelly is a mixture of two components: the inert explosive gel, and the primer. I'll show you how to mix in the primer to make the gel volatile. It's not too hard.

fadeToBlack(true, false)

wait(1)

fadeBackIn(60)

Now for the actual detonation. This is the tricky part, and the most dangerous, so listen well. 

The way you ignite the gel once it's been made volatile is to add water. The hard part is doing it from a safe distance.

To give you time to get away from the jelly before it goes off, you must use a water clock. Every barrel of blasting jelly comes with it's own water clock.

A water clock has two components: a big cup with a spout, and a little cup. First, place the small cup on top of the barrel. Then add water to the big cup, and tip it like so to keep the water from flowing out of the spout.

Now, place the larger cup on the barrel, with the spout pouring into the small cup. Do not let the water spill into the barrel! This will ignite the jelly, and you'll just be a smear on the wall. Have the good send to place the barrel where you want it to go off before setting the timer, so that you don't spill the water while you move the barrel, and the worm problem will be solved if you make a mistake.

If you did it right, you have until the water from the big cup fills the small cup to overflowing before the barrel will ignite, or about five minutes. Use that time to get somewhere safe. 

prepItem()
{
-wisdom >= 2: 
You seem pretty attentive, but I've been wrong before. Take these instructions for if you need a refresher before you set the water clock.
-else:
By the way your eyes are glazing over, you look like you'll need a refresher before you set the water clock. Take these instructions for when you need to use the jelly.
}

giveItem(7,13,1)

    +Thanks. Which one of these barrels should I take with me?
        ->1h

=== 1h ===

Oh, we only keep the training kits in the camp. The blasting jelly that we use for excavation is stored in the stockroom on the third floor of the mine. You'll need to make it all the way down there before you'll have access to one.

    +Understood.
        ->1i
    +Typical. Very well, I'll make do.
        ->1i

=== 1i ===

{
-mineLvl3KilledGuards:
activateQuestStep(No Good Deed,Make for the breach.)
-else:
activateQuestStep(No Good Deed,Find the blasting jelly.)
}

{
-not toldToFindNandor:
activateQuestStep(Explore the Mine,Enter the mine.)
}

Good luck... and be careful. I overheard some of the guards from the bottom floor talk about those worm things they encountered down there. I wouldn't wish them on anyone.

->1aa


=== Close ===

close()

->DONE