VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0
VAR playerName = ""

VAR pageIndex = 1
VAR carterIndex = 2
VAR debrisIndex = 3

VAR keptDirectorAlive = false

VAR pageSaidReadyToLeave = false
VAR spokeWithPageBeforePrisoners = false

VAR spokeWithNandorAfterPrisoners = false

{
-pageSaidReadyToLeave:
	->3a
-spokeWithNandorAfterPrisoners:
	->2a
-else:
	->1a
}


=== 1a ===

I am ready to leave this camp behind, but Carter has asked me to allow you time to rest. When you feel recuperated and you have taken care of all of your business here, return to me and we will discuss our next move.

{
-not spokeWithPageBeforePrisoners:

activateQuestStep(Leave the Camp, 0)
setToTrue(spokeWithPageBeforePrisoners)
~spokeWithPageBeforePrisoners = true

}

->Close

=== 2a ===

changeCamTarget({pageIndex})

Carter and I are both ready to make our way through the forest. Are you ready to accompany us?

+Yes, lets discuss our route.
	->2b

+Not yet. Give me a few more moments to prepare.
	Of course. Take as much time as you need.
	->Close

=== 2b ===

fadeToBlack()

changeCamTarget({pageIndex})
activate({carterIndex})

moveToLocalPos(-11,-10.65)
setFacing(NorthWest)

fadeBackIn(60)

As I have been out of touch with my homeland for some time, Carter will explain what comes next.

changeCamTarget({carterIndex})

The closest settlement within the Kingdom of Masons is the soldier's colony at Rice Hill, located some distance east-southeast of us. My team stayed there for a few days while we prepared to plant me at this camp.

->2ba

=== 2ba ===


+What do you mean by 'soldier's colony'?

	A soldier's colony is a fort or town settled by veterans of His Majesty's hosts, who were given a charter and stipend for that purpose. At the end of the last war with the Lovashi, the Kingdom of Masons was faced with many problems. Two of the biggest were large swathes of devastated, abandoned frontier, and a large number of veterans whose homes had been destroyed in the fighting.

	The king chartered many of these soldier's colonies to attempt to solve both of those issues. The soldiers were given large areas of the frontier to tame and police, which both helped to resettle the lands we had abandoned and kept them from idleness. Idle, destitute soldiers are only ever a few steps removed from banditry, after all.

	->2ba
+Can they accommodate all of the branded?

	Rice Hill should be large enough to provide for you all in the short term, and the majority of you in the long. Those that don't wish to stay at the colony can, of course, follow Page and I further south when we continue on to deliver {keptDirectorAlive:the Director and }our report to Masonic Command.

	->2ba
+How do we get there?
	->2c

=== 2c ===

We follow the trail that leads out of the camp to the east for as far as it will go. Then when it tapers out I can guide us further southeast until we hit the Grand Road, which bisects the kingdom running northeast to southwest. After that, it should just be a matter of following the road markers until we arrive at Rice Hill. Once we get there, we can send a small party back to show anyone who stayed behind the way through.

+Sounds easy enough. Is there anything I should know about the forest?
	->2d

=== 2d ===

It's home to wolves, Stick Saints, the odd hermit. Nothing you can't handle after facing down a camp full of Lovashi guards. But journey too far out of the woods and you may run into trouble. 

The forest we're in is situated on the northern tip of the Kingdom of Masons. Go much farther north and you'll run up against the Waking Mountains; consider that way blocked. Northeast is the Masonic Gap, which leads around the Waking Mountains into the Confederation. Obviously you'll want to steer clear of there.

East of us is more frontier, not much there but old abandoned forts and burnt out hamlets. Rice Hill is the closest thing to a settlement for miles around, and may be the most northern settlement in the entire kingdom. Go farther south, however, and you'll run into the kingdom's heartlands. Farther south than that and you'll get to see real cities too.

I should warn you now against going west. If you go that way, you'll hit the Wandering Roil or one of it's tributaries coming down from the mountains. Manage to cross it, and you'll be in the Burrow Mire. That's where the Expelled live. And they're not too fond of us Masons right now. Not sure they ever were, if I'm being honest.

->2da

=== 2da ===

+What's a Stick Saint? I've never heard of those.
	changeCamTarget({carterIndex})
	Nature spirits that animate bundles of dead wood they find in the forest. They like to wear cloaks of leaves and jump passersby who linger too long on the trail. If you're ready for them and travel in numbers, they're more a nuisance than anything. Like I said, nothing you can't handle.
	->2da
+Who are the Expelled and why do they hate you?
	changeCamTarget({pageIndex})
	They're a tribe of people that was exiled from their homeland a long time ago. They were nomads before they came to the Kingdom of Masons, but when they arrived here the king at the time gave them all of the land west of the Roil. Every so often they get into a spat with the kingdom because the course of the Roil 'wanders', hence the name, taking land or bestowing it to either side. When the Expelled don't have enough land or the Craft Folk lose too much, it leads to squabbling and things can get very tense.

	When the Lovashi invaded, the Expelled didn't lend a hand despite what our treaties with them say. And the Lovashi ended up leaving them mostly untouched, which made a lot of people in the kingdom suspect treachery. But we won the war and the king never held them to account, so there wasn't much either side could do but sit across the river and curse eachother. Unless something's changed recently, of course.
	->2da
+Stick to the woods until I get to the Grand Road, then make my way to Rice Hill. Got it.
	changeCamTarget({pageIndex})
	->2e
+I'll plan my own route, thank you.
	changeCamTarget({pageIndex})

	You can take whichever route you please, so long as we get to Rice Hill in one piece. But our best bet is to follow the trail, then keep heading southeast.
	->2e

=== 2e ===
~pageSaidReadyToLeave = true
setToTrue(pageSaidReadyToLeave)

activateQuestStep(Leave the Camp, 4)

Gather what you need. When you're ready, leave the camp and I'll follow you out. 

fadeToBlack()

deactivate({carterIndex})
deactivate({debrisIndex})

fadeBackIn(60)

->Close

=== 3a ===

Whenever you're ready, leave the camp. I'll be right behind you. 

->Close

=== Close ===

close()

->DONE