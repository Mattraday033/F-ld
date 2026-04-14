using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BookList
{
	public const string mineGuardsJournalKey = "Mine Guard's Diary";
	public const string mineGuardsJournalReadFlag = "mineLvl2ReadNoteAboutHiddenPassage";

	public const string pageFirstDiaryEntryKey = "Page's Diary #1";
	public const string pageFirstDiaryEntryReadFlag = "pickedUpPagesDiary#1";
	public const string pageSecondDiaryEntryKey = "Page's Diary #2";
	public const string pageSecondDiaryEntryReadFlag = "pickedUpPagesDiary#2";

    public const string ordersTranscriptKey = "Orders from the Director";
    public const string ordersTranscriptReadFlag = "pickedUpOrdersTranscript";

    public const string theInventionOfSinKey = "The Invention of Sin";
    public const string memoRegardingTheBrandedKey = "Memo Regarding the Branded";
	
	public const string guideToSkillsKey = "Guide to Skills";

	public const string horsetonguePronunciationGuideKey = "Horsetongue Pronunciation Guide";
	public const string theNightOfEmptyPlinthsKey = "The Night of Empty Plinths";
	public const string saintsAndSaintsKey = "Saints & Saints";

	public const string pitSecondEntranceNoteKey = "Pit Note";
	public const string pitSecondEntranceNoteReadFlag = "manse1FReadPit";

	public const string pitClosureNoteKey = "Pit Closure Note";
	public const string pitClosureNoteReadFlag = "pitClosureNoteRead";

	public const string directorsJournalKey = "Director's Journal";
	public const string directorsJournalReadFlag = "directorJournalRead";

	public static string getBookContents(string key)
	{
		switch (key)
		{
			case mineGuardsJournalKey:
				return "<i>You leaf through the journal and linger a while on the final entry: </i>" +
					   "\"I've seen Ond leaving the second level of the mine many times, but " +
					   "when I get to the first level he\'s gone. Where is he going? " +
					   "Maybe he knows something I don\'t.\"";
			case pageFirstDiaryEntryKey:
				return "...the architecture of the mansion seems to be of Delver origin. Perhaps " +
						"an above ground outpost to be used for trading with the surface? It would" +
						" explain why there was an overgrown road leading to the site. The only other" +
						" structure in the area was the large stone building now serving as the " +
						"guard barracks. Any other buildings must have been made of wood or another similarly less " +
						"stalwart material, and decayed to the point of invisibility since the site was abandoned.\n" +
						"\tMasonic Command has given Carter and myself complete autonomy on this venture, so I think I'm " +
						"going to take my time familiarizing myself with the mansion. Should an attack be warranted from " +
						"outside the camp, it would be good to know exactly how to exploit its layout. From my brief time studying other Delving Folk complexes, " +
						"I know how much they liked to place hidden passages within their underground networks. I expect their " +
						"aboveground structures to be no different. The room the Director has turned into a library seems to be a good place" +
						" to start looking. I'll be spending long hours in there tutoring the kids, so I will have plenty of oppurtunities to " +
						"continue my search.";
			case pageSecondDiaryEntryKey:
				return "I was correct! There was a section of bookshelves that had been around since before the Lovashi had moved in. " +
						"After some time I was able to find a hidden passage that led to a small room. " +
						"Within, I found a trove of records from the original builders of this place. Most of the scrolls would only have value" +
						" to someone interested in the minutiate of everyday Delver life (like myself), but included in these records were the " +
						"original building designs. Rifling through them revealed another passage that leads from the northeastern most corner of the meeting room " +
						"on the first floor to the Director's office on the second! So well hidden was this route that I doubt even the Director" +
						" himself is aware of it's existence.\n" +
						"\tWorking at night, I have explored the passage and discovered how the entry mechanism works. While it was deceptively " +
						"simple, it did include a lock which I lacked the key to. It took me three nights of toiling in secret to remove " +
						"the lock from the door. Would that I had Carter with me; he would have done it in a fraction of the time. " +
						"After I removed the lock, I replaced it with one whose tumblers I had set to accept my copy of the key to the Director's office. " +
						"This way I won't have to explain to anyone why I'm carrying around so many keys.";
			case ordersTranscriptKey:
				return "Let it be known that the Director considers all areas of the camp not within the immediate vicinity of the Manse to be lost, including " +
					   "the barracks and gates. All squad leaders are to report to either Captain Adéla on the second floor of the southern wing, or Chief Tabor " +
					   "on the second floor of the northern wing, for their posts.\n" +
					   "\tThe Director has confined himself to his Office during the sun hours and his Quarters during rest hours for the duration of the crisis, in " +
					   "order to minimize access to him by the rioters. All keys to the Director's Office and Quarters have been confiscated save for one, which has " +
					   "been split between Captain Adéla and Chief Tabor. The Captain and Chief will use the key to gain access to the Director's Office each morning to " +
					   "brief him on the current situation and receive their orders. No other access to the Director is permitted and anyone caught even lingering near his " +
					   "apartments without orders to do so will be subject to immediate seizure and imprisonment.\n" +
					   "\tAddendum: The Director wishes for the following statement to be read to the guards: 'Soldiers, those of you old enough to have served with me " +
					   "during the Emancipation Conflict will recognize our predicament. We're surrounded, outnumbered, and many days from relief. There are throngs of baying " +
					   "drudges probing the pickets. The odds and Gods alike look to be on the side of our enemies.\n" +
					   "\tBut we survived the worst of that war, and we'll survive this. Our walls are thick. Our supplies are plentiful. Our comrades were already sent for even " +
					   "before the riot began. The sons and daughters of the saddle do not buckle before such paltry foes as these cobblers in rags! These horse-eating mongrels! " +
					   "Man your barricades, steel your hearts, and remember: every second you stand firm brings our inevitable victory ever closer.'";
			case theInventionOfSinKey:
				return "... but what may not be commonly known, is that the first accounts of humans on Föld committing the sins of slavery and cannibalism were recorded during the early years of " +
						"what we now call 'The Emancipation Conflict.' At the outset of this inter-folk war between the peoples of the saddle and craft, the Craft Kingdoms were at " +
						"a clear disadvantage. They fielded slow moving, foot-mobile armies, which were often outmaneuvered by the more nimble, horse-mobile Riding Folk. The " +
						"connection the Riding Folk had with their mounts, via fluency in the horsetongue, allowed them to be almost everywhere at once: at one moment harrying " +
						"their enemy's flanks, then wheeling to assault from another angle once the Craft Folk had attempted a defense.\n" +
						"\tThe Craft Folk, aware of their predicament, stole into the camps of the Riding Folk and made off with the children of their horses. " +
						"This enabled the Artisan Kings to raise crude imitations of the Riding Folk hosts, in an effort to counter their foe's mobility. However, the Craft Folk " +
						"were unlearned in the ways of the rider, and lacked the necessary understanding of the horsetongue to teach it to their captives. The foals in their care " +
						"grew up diminished in their intellect, as we might expect a human child raised without language to be. Becoming little more than unintelligent beasts ridden by " +
						"ignorant masters, it took a long time for the horsemen of the Craft Folk hosts to be acknowledged as the serious and legitimate threat they are today. " +
						"The Lovashi consider the stealing of those first horses, and the pressing of them into the service of the Craft Kingdom's militaries, to be the origin of " +
						"the sin of slavery.\n" +
						"\tWhile this atrocity was being perpetrated, most of the Riding Folk leaders were busy dealing with a different impedement: the walls of the Craft Folk settlements. " +
						"Where as early pitched battles were mostly one-sided Lovashi victories, the folk of the saddle had little answer for the massive fortifications erected around " +
						"Craft Folk cities. The first Artisan Kingdom seat to fall was Jawan, capital to the Kingdom of Dams, and must be considered an anomaly, given that the defenders " +
						"released the Loam Flow river upon the seiging and beseiged alike rather than surrender it. However, the Craft Folk defenders willingness to fight to the death, along with the Lovashi's own ineptitude for seige warfare, " +
						"does explain the Rider Lords' apprehensions toward storming Craft Folk battlements. Instead, a different approach was adopted. The Riding Folk would harass Craft Folk armies, but " +
						"never cut them off from retreat. Eventually, the Craft Folk would be forced to cede the field, and flee back to their cities for shelter. In this way, the Riding Folk " +
						"would channel their enemies into one place, as they did with herds of deer upon the High Steppes. Then they would camp outside their walls, and wait for the great numbers of trapped " +
						"Artisan soldiers to starve. Eventually, the Craft Folk would be forced to give up their walled cities without considerable loss of Lovashi life.\n" +
						"\tAs the Riding Folk forces entered the surrendering cities, they came across an appalling sight. Whenever a trapped host contained any of the Craft Folk's fledgling cavalry, " +
						"the townspeople would take to eating the horses. When interrogated, the Craft Folk revealed they would even eat them early in the seiges, well before other, more perishable food " +
						"sources were depleted. The reasoning for this being horses would contribute little to the defense of the city, and require food in much larger proportions than a human. This logic " +
						"horrified the Lovashi, who considered any horse's life equal to that of a human. Early punishments for what the Lovashi would deem to be cannibalism were simple executions, " +
						"but the ubiquity of evidence for the crime when accepting the surrender of Craft Folk fortresses caused many Lovashi to conduct massacres of the surrendering populaces, sometimes " +
						"even before that evidence had presented itself.\n" +
						"\tAs the Lovashi perfected their methods for conducting seiges, the Craft Folk began to become wise to their tactics. The Artisans learned to stockpile food in their strong places, " +
						"well in advance of the approaching Lovashi hordes. As such, the pace of the war ground to a halt, while the Lovashi waited longer and longer for the Craft Folk settlements to run out " +
						"of provisions. The Riding Folk were faced with a dilemma: continue to fear the Craft Folk's bastions and give their foes time to rest and perfect the art of mounted combat, or " +
						"attempt to carry the enemy's walls and suffer the hideous attrition that would require. They eventually were forced to choose the latter, but would find the means to make that " +
						"choice more palatable by learning from the crimes of their nemeses.\n" +
						"\tThe Riding Folk began conscripting captured Artisan smiths and carpenters to create for them great machines of war, starting with the " +
						"prisoners they had initially wished to execute for crimes against horsekind. Many of the schematics which were designed to aid in the defense of the cities, great bolt-throwers, catapults, " +
						"and slings, were scaled up and made to smash through those same defenses. As the conflict dragged on, the needs for skilled laborers grew, and the Lovashi enlisted more and more of the Craft Folk, criminals or not. " +
						"The Riding Folk eventually created a system to differentiate between the conscripts which had been found guilty of 'owning' a feral horse, or had eaten from their flesh, and those whose " +
						"expertise was the only factor that necessitated their conscription. The conscripts that were innocent of these sins would only work for a period of five years, or the end of the conflict, " +
						"whichever came first. Those that were found guilty would work until they died from the exertion, and were given a burning brand about their face and necks to forever mark them as such. This " +
						"is believed to be the origin of the use of branded slaves in Lovashi industry, and the act which marks the transition from the Early Period of the Emancipation Conflict to the Middle Period...";
			case memoRegardingTheBrandedKey:
				return "To all camp personnel of every rank:\n" +
						"The Director has asked me to disseminate the following orders regarding conduct when interacting with the branded of this camp. It has come to his attention that when bestowing tasks, punishments, " +
						"and rewards to the branded, personnel are giving more favorable outcomes to the branded of Lovashi ethnicity. This is to be stopped immediately, for the following reasons.\n\n" +
						"1) The brand is not meant to be a pleasant experience. It is a tool for correcting the mentalities and behaviors that lead to the crimes these prisoners are guilty of. This includes those that result in " +
						"traitorous activities that Lovashi prisoners have performed. Any leniency provided to a Lovashi branded is a disservice both to the prisoner and to the Confederation.\n\n" +
						"2)In the majority of cases it is difficult to determine whether a prisoner is actually of Lovashi decent. Many Craft Folk have taken to giving their children Lovashi names because they think it will incentivize their " +
						"lords to offer them better treatment. As there is little way to tell the difference between someone of Craft Folk and Lovashi decent aside from accent, and even that can be faked or misinterpreted, it is camp policy to " +
						"treat all prisoners the same, regardless of lineage.\n\n" +
						"Any camp personnel suspected of continuing this practice will be given severe repercussions. If I have to flog you in front of the entire camp, the slaves included, I will do so. Test my resolve at your peril.\n\n" +
						"- Chief Correctional Officer Tabor";
			case guideToSkillsKey:
				return "Skills are Abilities that are usable outside of Combat. A Party Member unlocks a Skill by having a 2 or higher in that Skill's corresponding Primary Stat. The four Skills are Intimidate (Strength), Cunning (Dexterity), Observation (Wisdom), " + 
                        "and Leadership (Charisma). If multiple Party Members have unlocked a Skill, then the highest Primary Stat among all Party Members will be used. To use a Skill, press ' "+ KeyBindingList.skillKey.ToString() +" ' while out of Combat. " + 
                        "To change the currently selected Skill, press  ' " + KeyBindingList.cycleSkillAscendingKey.ToString() + " ' or ' " + KeyBindingList.cycleSkillDescendingKey.ToString() + " '.\n\n" + 
                        "Intimidate: Use Intimidate to scare packs of Enemies while in the Overworld. Intimidated Enemies cannot Surprise you or be Surprised by you. Use this to prevent Enemies from Surprising you around corners. " + 
                        "All Enemies within the target area will be Intimidated. A use of Intimidate will cost a single charge, regardless of how many packs of Enemies were affected. You regain charges when you enter a new area.\n\n" + 
                        "Cunning: Cunning can be used to distract Enemies while in the Overworld. A distracted Enemy cannot move for a short time and will turn around, exposing their back. Use this to either Surprise your Enemies or slip by them unnoticed. " + 
                        "Distracting an Enemy costs two charges of Cunning. Regain charges of Cunning by entering a new area. Cunning can also be used to activate certain objects in the Overworld. If Cunning can be used on an object, then that object will have a yellow " + 
                        "highlight when you press '" + KeyBindingList.revealKey.ToString() + " '. Using Cunning on an object only costs a single charge.\n\n" + 
                        "Observation: Observation helps find hidden doors and objects. If a hidden object is found, it will be shaded pink. Hidden objects are not shown when you press '" + KeyBindingList.revealKey.ToString() + " ' so be on the lookout " + 
                        "for anything that looks suspicious. Observation requires no charges to use.\n\n" + 
                        "Leadership: Leadership allows the Party to place its Members on specific tiles. This can be useful for holding down buttons or blocking passageways. To remove a Party Member from a tile, face that Party Member and press ' " 
                        + KeyBindingList.removePlacedCompanionMovableObjectKey.ToString() + " '. The better your Leadership Skill, the more Party Members you can place at once. You can never place more Party Members than the amount of characters in your Party minus one.";
			case horsetonguePronunciationGuideKey:
				return "This short guide is meant to help someone pronounce the words of the horsetongue.\n\n" +
						"A: like the 'o' in 'honest'\n" +
						"Á: like the 'a' in 'apple'\n" +
						"B: like the 'b' in 'bark'\n" +
						"C: like the 'ts' in 'tsar'\n" +
						"Cs: like the 'ch' in 'child'\n" +
						"D: like the 'd' in 'dog'\n" +
						"Dz: like the 'ds' in 'suds'\n" +
						"Dzs: like the 'j' in 'job'\n" +
						"E: like the 'e' in 'bed'\n" +
						"É: like the 'ay' in 'hay'\n" +
						"F: like the 'f' in 'father'\n" +
						"G: like the 'g' in 'goat'\n" +
						"Gy: like the 'j' in 'job', held into the y in 'yup', to make 'jyuh'\n" +
						"H: like the 'h' in 'home'\n" +
						"I: like the 'ee' in 'see'\n" +
						"Í: same as I, but twice as long.\n" +
						"J: like the 'y' in 'yes'\n" +
						"K: like the 'k' in 'kit'\n" +
						"L: like the 'l' in 'love'\n" +
						"Ly: like the 'y' in 'yes'\n" +
						"M: like the 'm' in 'mother'\n" +
						"N: like the 'n' in 'nurse'\n" +
						"Ny: like the 'n' in 'nurse', held into the y in 'yup', to make 'nyuh'\n" +
						"O: like the 'o' in 'go'\n" +
						"Ó: same as O, but twice as long\n" +
						"Ö: a vowel sound, half way between 'eh', and 'uh'\n" +
						"Ő: same as Ö, but twice as long\n" +
						"P: like the 'p' in 'puppy'\n" +
						"Q: like the k in 'kit', held into the 'v' in 'vase', to make 'kvuh'\n" +
						"R: like the 'r' in 'red'. Rolled if you can.\n" +
						"S: like the 'sh' in 'share'\n" +
						"Sz: like the 's' in 'sun'\n" +
						"T: like the 't' in 'top'\n" +
						"Ty: like the 't' in 'top', held into the 'y' in 'yup', to make 'tyuh'\n" +
						"U: like the 'oo' in 'boot'\n" +
						"Ú: same as U, but twice as long\n" +
						"Ü: a vowel sound, half way between 'uh', and 'oo'\n" +
						"Ű: same as Ü, but twice as long\n" +
						"V: like the 'v' in 'vase'\n" +
						"W: also like the 'v' in 'vase'\n" +
						"X: like the 'k' in 'kit', held into the 's' in 'sun', to make 'ksuh'\n" +
						"Y: like the 'ee' in 'see'\n" +
						"Z: like the 'z' in 'zest'\n" +
						"Zs: like the 's' in 'pleasure'\n";
			case theNightOfEmptyPlinthsKey:
				return "Each of the Craft Kingdoms, save one, took their name from the Ordained Craft they were given by the Gods to perfect. The Kingdom of Sculptors, however, was founded by the Saint-King Lysop, who attained Sainthood by learning the Craft of Sculpting from the Földkölyök, or Kobolds. " +
						"After his canonization, Lysop was gifted land and subjects by the King of Weavers to found his own kingdom, wherein he could teach his people the Craft of Sculpting. The tales tell that so great was Lysop's connection to stone, he could cleave an opus from marble in mere weeks. " +
						"Over his life he would create scores of works, which he would display alongside those of his disciples within his throne-city of Carnassus.\n" +
						"\tMany legends describe the life of Lysop, but this treatise concerns itself purely with his death. Decades after Lysop's coronation, the King of Weavers would call upon Lysop to aid him in the Emancipation Conflict. Eventually, after the Weavers' defeat and subjugation, Lysop " +
						"fled with his armies back to Carnassus to await the inevitable Lovashi incursion. When the Lovashi arrived to beseige the City of Statues, they found it's defenders in high spirits, singing songs and shouting insults at their foes from the city's ramparts. Undeterred, the Lovashi " +
						"set to work building the necessary equipment for their assault. One night, to their surprise, Carnassus' defenders sallied from it's walls and torched the Lovashi camps. It made little diffence, so greatly did the seigers outnumber the beseiged, but the Lovashi were " +
						"taken aback by how fearlessly the Sculptors conducted themselves in the face of their hopeless predicament.\n" +
						"\tEventually, the inevitable occured: the Lovashi breached the outer ringwall, and the fighting spilled into the city's streets. The Lovashi found that, as they spread deeper into the city, the Craft Folk fought to beat them back harder and harder. Finally, as their progress brought them " +
						"to Carnassus' main plaza, they found the source of the Artisan's fervor. A great statue adorned the plaza, depicting a fallen rider trapped beneath his dead mount, with a Craft Folk warrior standing atop the beast, ready to thrust his spear into the rider's neck. So truly was this depiction realized that " +
						"the two combatants looked to be in motion; it is said that an onlooker had to continuously blink to realize the Artisan had not slain his prey before one's eyes.\n" +
						"\tThe Sculptors would not relinquish the plaza while they still had life to defend it. Twice, the Lovashi crashed against their ranks, the second time even managing to break their formation down the middle, but again and again the defenders rallied. Only when the Riding Folk finally surrounded the plaza and " +
						"set it alight did it's protector's fury wither. The plaza and it's statue were one of the final places to cease it's resistance, falling just before the capture of the last civilian shelters, and Lysop's throneroom. But when the Lovashi entered the Sculptor King's palace, they found no trace of him or his throne.\n" +
						"\tNo story holds the truth of where the Sculptor King disappeared to, but the remnants of his people say he gave his life to his final masterpiece. They say only by bestowing his energy into the stone could he create such a beauty, and as his like will never grace Föld again, nor will another statue like it be born " +
						"from Föld's stones. As for the throne, a plan was hatched for it to be broken down into many pieces, and secreted away across Carnassus. When the embers of war cooled in that city, the pieces were to be carried to safety in one of the Craft Kingdoms yet to bow to the Lovashi. This plan would " +
						"eventually succeed, despite the odds, and the Throne of Lysop now rests in Phesus, capitol of the Kingdom of Masons, where it serves as the seat of their ruler's consort. The Craft Folk still long for the day when it can be returned to Carnassus, and a new Sculptor King crowned.\n" +
						"\tAfter Carnassus was truly conquered, the Lovashi grew incensed at the toll in blood the city had cost them. They bore witness to Lysop's last work, and fearing it's power over his subjects, took to dismantling it. So complete was their retribution, and so great was their malice, " +
						"that after the statue had been reduced to rubble they began to spread through the city, toppling every statue of any prominence. The Lovashi took their revenge on Lysop's disciples as well, crushing them beneath their largest works. This moment of vengeance would later come " +
						"to be called 'The Night of Empty Plinths', and indeed only every fifth statue was saved from the pillage. To this day, sculpting is a forbidden art within the Confederation, for the Lovashi still fear it's power to rally a people against them.";

			case saintsAndSaintsKey:
				return "The Gods, when they created the many Folks that live atop Föld, did so each time with a special purpose in mind. The Craft Folk are no exception, for their purpose was to learn the many crafts of civilization, and to teach them to the other Folks. Such are most of the Craft Kingdoms named for: " +
						"the Masons study masonry, the Smiths master smithing, the Weaver's weave, and so on. In their wisdom, the Gods chose the crafts that would best assist in humanity's growth, but they did not deem every Craft worthy of it's own kingdom, nor did they teach every craft to one of the Craft Folk. " +
						"The crafts that were handed down by the Gods were deemed to be 'Ordained', and are held above the others. So it goes that over the ages new crafts have been discovered. When this occurs, it is a cause for much celebration among the Craft Folk, and should the new craft be deemed wonderous " +
						"enough to be truly unique from the others, a Sainthood is bestowed upon it's discoverer. This process, known as 'canonization', is one of the most holy procedures in Craft Folk ritual, and can only be conducted by royalty.\n" +
						"\tFor a Sainthood to be handed down, first the craft in question must be considered against the other 'canon' crafts. Is it unique from them? Is it as useful as them? Is it as complex as them? These and many other questions must be considered by one of the Craft Kings before he approaches, or " +
						"perhaps after he is approached by, the candidate for Sainthood. If the king is thoroughly impressed enough by what has been discovered, he will then have to petition the royal families of two other kingdoms before a quorum can be reach. If the candidate appears worthy, " +
						"then the kings or their representatives will meet to discuss the matter. If all three parties can agree on the legitimacy of the new craft, then it's discovery is determined to have been granted by the Gods themselves, and the new craft is " +
						"added to the canon. As one can guess, this process has only been completed a handful of times since the founding of the kingdoms. The most famous example perhaps is the discovery of Sculpting, which so wildly impressed the King of Weavers that he relinquished a portion of his domain to form " +
						"the Kingdom of Sculptors after it's canonization. Some other prominent examples include Erid, Saint of Glass, and Kai, Saint of Paper.\n" +
						"\tA common question about the Artisan Saints is regarding the Saint spirits, and how they factor into the process. This is a misconception, as the origin of the term 'Saint' in regards to these spirits is actually derivative and colloquial. Take for example the mischievous Stick Saint, a spirit that " +
						"plagues the northern woodlands of the Kingdom of Masons, among other parts of Föld. The body of a Stick Saint consists of a skeletal figure made from discarded wooden sticks collected from the forest, and is usually found draped in a cloak made of leaves plucked from low hanging branches. Though hard " +
						"to verify, it is a frequently held belief that these Saint spirits 'build' these bodies from their titular element. This has given rise to the notion that these spirits are as industrious as the Saints themselves, which then lent itself to the name.";
			case pitSecondEntranceNoteKey:
				return "With the riot going on, all work has ceased on this section of the Manse. We should bar the doors to this area, it's dangerous in here. Oszkár was up on the second floor when he slipped and fell. Someone found his body all the way down in the Pit: there wasn't much left of him to scrape off the stones.";
			case pitClosureNoteKey:
				return "The Pit's warden has been saying he's been hearing movement down in the Pit, and it's not the slaves. No one's seen anything, but we're all on edge after what happened in the mine. With the slaves rioting upstairs, the warden ordered this section sealed up incase the worms managed to break in. " +
						"Not sure whats going on up there: I've been stuck down here watching for worms for a while, and no one's come to relieve me yet.";
			case directorsJournalKey:
				return "... the count's chose me for this task for my adherence to our cause. I know my Confederation requires this act of sacrifice should a new campaign against the Masons ever be renewed. The mountains act as better walls than they could ever build. A way beneath them would be nothing less than transformative in our war planning. " + 
                        "However, I confess my thoughts turn yet again to what the Great Mother would think of this expedition. A camp so close to Trónszék? Is it not some variety of sacrilege? We don't seek her domain at the top of the mountain, meerly to travel beneath it along the roads the Delver's made, but that excuse feels more and more threadbare with each passing week. " + 
                        "I can feel Her eyes on us. Angyal knows why we are here and surely must disapprove. She expelled the Delvers of this mountain and took their home for it's remoteness, and yet here we are all the same. The worms that assaulted our dig teams can be nothing but a clear sign that we have ridden where we should not.\n\n" +
                        "My leg has been hurting again. This place... it feels all too familiar. This morning I smelled smoke as I awoke. I lept from my bed and meant to race to the door, shouting for the guard to be raised and for my mount to be sent for. Had my leg not given out before I could make it across the room, my household may well have seen me " +
                        "race about the Manse, shrieking as if I was still at Wudra. I don't think the sentries outside my doors heard what I was saying, they seemed more confused than alarmed as I picked myself off the floor. Perhaps it would be better if I <I>was</I> still there. At least then I could retreat. Here I am stuck under the Great Mother's wrathful gaze.\n\n" + 
                        "Trapped against giant walls. Again the defenders come forth. We are not welcome.";
		}

		return "Book Contents Not Found.";
	}

}
