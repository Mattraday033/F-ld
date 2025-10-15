using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public struct ItemListID
{
	public const string listIndexElementName = "listIndex";
	public const string itemIndexElementName = "itemIndex";
	public const string quantityElementName = "quantity";

	public int listIndex;
	public int itemIndex;
	public int quantity;
	
	public ItemListID(int listIndex, int itemIndex)
	{
		this.listIndex = listIndex;
		this.itemIndex = itemIndex;
		this.quantity = 1;
	}
	
	public ItemListID(int listIndex, int itemIndex, int quantity)
	{
		this.listIndex = listIndex;
		this.itemIndex = itemIndex;
		this.quantity = quantity;
	}
}

public static class ItemList 
{
	public static ArrayList allItems = new ArrayList();

	public const int onlyAcceptableEquippedItemQuantity = 1;
	public const string dominantFistKey = "Dominant Fist";
	public const string fistKey = "Fist";

	public const int rationsHealingAmount = 25;
	public const int properFoodHealingAmount = 40;

	public const bool isOneHanded = false; //what you in the constructor when you want a weapon to be one handed
	public const bool isTwoHanded = true; 	//what you in the constructor when you want a weapon to be two handed

	public const int itemHasNoWorth = 0;

	public const int usableItemListIndex = 0;
	public const int weaponsListIndex = 1;
	public const int armorListIndex = 2;
	public const int questItemListIndex = 3;
	public const int keyItemListIndex = 4;
	public const int treasureItemListIndex = 5;
	public const int partyMemberWeaponListIndex = 6;
	public const int bookListIndex = 7;
	
	public const int rationsIndex = 0;
	public const int bandagesIndex = 1;
	public const int thistleTeaIndex = 2;
	public const int properFoodIndex = 3;
	public const int chokegrassBombIndex = 4;
	public const int chewIndex = 5;
	
	public const int cudgelIndex = 0;
	public const int shivIndex = 1;
	public const int mainHandFistIndex = 2;
	public const int offHandFistIndex = 3;
	public const int malletIndex = 4;
	public const int lightPickIndex = 5;
	public const int heavyPickIndex = 6;
	public const int bronzeBarIndex = 7;
	public const int bronzeDirkIndex = 8;
	public const int improvedMainHandFistIndex = 9;
	public const int greaterMainHandFistIndex = 10;
	public const int ruinousMainHandFistIndex = 11;
	public const int bronzeGreatspearIndex = 12; // Manse 2F-3b
	public const int wornBowIndex = 13;		 //Manse 2F-2c
	public const int fightingCapeIndex = 14; //Pit 1a
    public const int staffIndex = 15;	//Mine lvl 2 6 (wisdom room)
	public const int ancientClawIndex = 16; //Pit 2c
	public const int wickedKnifeIndex = 17; //Mine lvl 2 5b (dexterity room)
	public const int plankIndex = 18;
	public const int sharpRockIndex = 19;

    public const int slaveRagsIndex = 0;
	public const int clothGlovesIndex = 1;
	public const int rottenSandalsIndex = 2;
	public const int potLidIndex = 3;
	public const int minersHelmetIndex = 4;
	public const int leatherGlovesIndex = 5;
	public const int paddedArmorIndex = 6;
	public const int thatchNecklaceIndex = 7;
	public const int martialArtistsBeltIndex = 8; // Manse 1F-3e
	public const int wardensShieldIndex = 9; //Pit 1b
	public const int silverSpoonIndex = 10;  //Pit 1a
	public const int leatherBootsIndex = 11; // Manse 1F-2a
	public const int bronzeHelmetIndex = 12; // Manse 2f-Stockroom
	public const int bronzeCuirassIndex = 13; // Manse 1F-1c
	public const int salvagedGuardHelmIndex = 14; 
	public const int salvagedGuardArmorIndex = 15;
	public const int salvagedGuardGlovesIndex = 16;
	public const int salvagedGuardBootsIndex = 17;
	public const int andrasLuckyTalismanIndex = 18;
	public const int delversDreamIndex = 19;
	public const int cookingPotIndex = 20;
	public const int bronzeBadgeIndex = 21;
	
	public const int leafSamplesIndex = 0;
	public const int guardDiaryIndex = 1;
	public const int winchIndex = 2;
	public const int candyIndex = 3;
	public const int toolBundleIndex = 4;
	public const int blastingJellyIndex = 5;
	public const int claysNoteIndex = 6;
	public const int laszloBadgeIndex = 7;
	
	public const int mineArmoryKeyIndex = 0;
	public const int pitCellKeyIndex = 1;
	public const int directorsOfficeKeyFrontIndex = 2;
	public const int directorsOfficeKeyBackIndex = 3;
	public const int munitionsKeyIndex = 4;
	
	public const int ironNuggetIndex = 0;
	public const int goldLocketIndex = 1;	  // x1
	public const int smallCoinPurseIndex = 2; // x2
	public const int urosIronNuggetIndex = 3;
	
	public const int redPickLevel1Index = 0;
	public const int redPickLevel2Index = 1;
	public const int redPickLevel3Index = 2;
	public const int redPickLevel4Index = 3;
	public const int redPickLevel5Index = 4;
	public const int nandorCudgelLevel1Index = 5;
	public const int nandorCudgelLevel2Index = 6;
	public const int nandorCudgelLevel3Index = 7;
	public const int nandorCudgelLevel4Index = 8;
	public const int nandorCudgelLevel5Index = 9;
	public const int carterShivLevel1Index = 10;
	public const int carterShivLevel2Index = 11;
	public const int carterShivLevel3Index = 12;
	public const int carterShivLevel4Index = 13;
	public const int carterShivLevel5Index = 14;
	
	public const int mineGuardsJournalIndex = 0;
	public const int pageDiaryFirstEntryIndex = 1;
	public const int pageDiarySecondEntryIndex = 2;
    public const int orderTranscriptIndex = 3;
    public const int theInventionOfSinIndex = 4;
	public const int keybindingInfoIndex = 5;
	public const int horsetonguePronunciationGuideIndex = 6;
	public const int nightOfEmptyPlinthsIndex = 7;
	public const int saintsAndSaintsIndex = 8;
	public const int pitSecondEntranceNoteIndex = 9;
	public const int pitClosureNoteIndex = 10;

    public const string martialArtistsBeltKey = "Martial Artist's Belt";
	public const string wardensShieldKey = "Warden's Shield";
	public const string silverSpoonKey = "Silver Spoon";
	
	public const string salvagedGuardHelmKey = "Salvaged Guard Helm";
	public const string salvagedGuardArmorKey = "Salvaged Guard Armor";
	public const string salvagedGuardGlovesKey = "Salvaged Guard Gloves";
	public const string salvagedGuardBootsKey = "Salvaged Guard Boots";
	
	public const string luckyTalismanKey = "Lucky Talisman";
	public const string delversDreamKey = "Delver's Dream";
	public const string bronzeBadgeKey = "Bronze Badge";

	public const string cookingPotKey = "Cooking Pot";

	private const int givesNoArmor = 0;

	//head slot armor values
	public const int minersHelmetArmorValue = 1;
	public const int bronzeHelmetArmorValue = 5;
	public const int salvagedGuardHelmArmorValue = 3; 
	public const int cookingPotArmorValue = 2; 

	//chest slot armor values
	public const int slaveRagsArmorValue = 1;
	public const int paddedArmorArmorValue = 5;
	public const int bronzeCuirassArmorValue = 10;
	public const int salvagedGuardArmorArmorValue = 10;

	//hands slot armor values
	public const int clothGlovesArmorValue = 1;
	public const int leatherGlovesArmorValue = 3;
	public const int salvagedGuardGlovesArmorValue = 3;

	//feet slot armor values
	public const int rottenSandalsArmorValue = 1;
	public const int leatherBootsArmorValue = 3;
	public const int salvagedGuardBootsArmorValue = 3;

	//trinket slot armor values
	public const int thatchNecklaceArmorValue = 4;
	public const int martialArtistsBeltArmorValue = 6; //plus 2W
	public const int delversDreamArmorValue = 4;
	public const int bronzeBadgeArmorValue = 4;

	//shield armor values
	public const int potLidArmorValue = 8;
	public const int wardensShieldArmorValue = 10; //plus 2S


	static ItemList()
	{

		ArrayList usableItems = new ArrayList();
		ArrayList weapons = new ArrayList();
		ArrayList armor = new ArrayList();
		ArrayList questItems = new ArrayList();
		ArrayList keys = new ArrayList();
		ArrayList treasure = new ArrayList();
		ArrayList monsterWeapons = new ArrayList();
		ArrayList partyMemberWeapons = new ArrayList();
		ArrayList books = new ArrayList();

		//HealingItem(string key, string loreDescription, string useDescription, int worth, int amountToHeal)
		usableItems.Add(new HealingItem(new ItemListID(usableItemListIndex, rationsIndex), "Rations", "Moldy bread and grimy pemmican.", "Heals " + rationsHealingAmount + " hp.", "Rations", 5, rationsHealingAmount));
		usableItems.Add(new RestorationItem(new ItemListID(usableItemListIndex, bandagesIndex), "Bandages", "Wrappings made from a coarse, firm cloth.", "Used to remove all " + TraitList.woundTraitType + " Traits from a friendly target.", "Bandages", 15, Range.hexadecupleBoxIndex, TraitList.woundTraitType));
		usableItems.Add(new SkillReplenishItem(new ItemListID(usableItemListIndex, thistleTeaIndex), "Thistle Tea", "Tea made from the flower of local thistle plants. Sharpens the senses and reinvigorates the mind.", "Used to replenish a use of the Cunning Skill. Can not be used to increase your cunning uses above your maximum.", "Tea", 25));
		usableItems.Add(new HealingItem(new ItemListID(usableItemListIndex, properFoodIndex), "Proper Food", "Food worth eating. It'll fill you up and more so.", "Heals " + properFoodHealingAmount + " hp.", "Rations", 15, properFoodHealingAmount));
		usableItems.Add(new TraitApplicationItem(new ItemListID(usableItemListIndex, chokegrassBombIndex), "Chokegrass Bomb", "A small tin casing filled with a powder that ignites when exposed to air. The smoke from this bomb attacks the eyes and lungs, preventing it's victims from attacking or defending themselves.", "Stuns all targets for 1 round.", "SmokeBomb", 50, Range.hexadecupleBoxIndex, TraitList.chokingKey, CombatItem.useDoesRequireAnAction));
		usableItems.Add(new TraitApplicationItem(new ItemListID(usableItemListIndex, chewIndex), "Chew", "A leaf that is ground between ones teeth to get at the juices within. These secretions provide an energetic numbness that removes the ache from wounds and fatigue from muscles.", "Heals the user for "+TraitList.chewHealing+" health, increases their damage by "+TraitList.chewExtraDamage+", and their crit chance by "+TraitList.chewExtraCritPercent+"%.", "Chew", 35, Range.singleTargetIndex, TraitList.chewKey, CombatItem.useDoesNotRequireAnAction));


		//Weapon(string key, string loreDescription, string damageFormula, string critFormula, string iconName, int rangeIndex, int worth, int slotID)
		//Weapon(string key, string loreDescription, string damageFormula, string critFormula, string iconName, int rangeIndex, int worth, int slotID, bool isTwoHanded)

		weapons.Add(new Weapon(new ItemListID(weaponsListIndex, cudgelIndex), "Cudgel", "A wooden club made from a fallen tree branch.", "2S + 5", "S+D", "Cudgel", Range.doubleVerticalIndex, 3, Weapon.mainHandSlotIndex, isOneHanded));
		weapons.Add(new Weapon(new ItemListID(weaponsListIndex, shivIndex), "Shiv", "A weapon made from a bronze nail tied to a small piece of wood.", "2D + 7", "3D", "Shiv", Range.singleTargetIndex, 3, Weapon.mainHandSlotIndex, isOneHanded));
		weapons.Add(new Fist(new ItemListID(weaponsListIndex, mainHandFistIndex), dominantFistKey, "Good old fashioned meat bludgeons.", "S+D+W", "D+W", "FistIcon", Range.singleTargetIndex, Weapon.mainHandSlotIndex));
		weapons.Add(new OffHandFist(new ItemListID(weaponsListIndex, offHandFistIndex), fistKey, "Good old fashioned meat bludgeons.", "S+D+W", "D+W", "FistIcon"));
		weapons.Add(new Weapon(new ItemListID(weaponsListIndex, malletIndex), "Mallet", "A large hammer used to beat pitons into rock walls.", "3S + 6", "D", "MalletIcon", Range.doubleHorizontalIndex, 15, Weapon.mainHandSlotIndex, isOneHanded));
		weapons.Add(new Weapon(new ItemListID(weaponsListIndex, lightPickIndex), "Light Pick", "A bronze pick meant to be used to in one hand.", "3D + 8", "3D", "OneHandedPick", Range.doubleHorizontalIndex, 15, Weapon.mainHandSlotIndex, isOneHanded));
		weapons.Add(new Weapon(new ItemListID(weaponsListIndex, heavyPickIndex), "Heavy Pick", "A large bronze pick meant to be used in two hands.", "4S + 7", "D", "TwoHandedPick", Range.tripleHookIndex, 15, Weapon.mainHandSlotIndex, isTwoHanded));
		weapons.Add(new Weapon(new ItemListID(weaponsListIndex, bronzeBarIndex), "Bronze Bar", "A long thin bronze ingot. A bit oxidized, but hefty.", "2S + 5", "D", "BronzeBar", Range.quadrupleHorizontalIndex, 5, Weapon.mainHandSlotIndex, isTwoHanded));
		weapons.Add(new Weapon(new ItemListID(weaponsListIndex, bronzeDirkIndex), "Bronze Dirk", "A curved bronze blade, held in the off hand.", "S + D + 3", "D+2", "CurvedDagger", Range.singleTargetIndex, 5, Weapon.offHandSlotIndex, isTwoHanded));
		weapons.Add(new Fist(new ItemListID(weaponsListIndex, improvedMainHandFistIndex), dominantFistKey, "Good old fashioned meat bludgeons.", "2W+S+D+4", "W+D+2", "ImprovedFistIcon", Range.doubleVerticalIndex, Weapon.mainHandSlotIndex));
		weapons.Add(new Fist(new ItemListID(weaponsListIndex, greaterMainHandFistIndex), dominantFistKey, "Good old fashioned meat bludgeons.", "3W+S+D+8", "W+D+4", "GreaterFistIcon", Range.quadrupleBoxIndex, Weapon.mainHandSlotIndex));
		weapons.Add(new Fist(new ItemListID(weaponsListIndex, ruinousMainHandFistIndex), dominantFistKey, "Good old fashioned meat bludgeons.", "4W+S+D+16", "W+D+8", "RuinousFistIcon", Range.singleTargetIndex, Weapon.mainHandSlotIndex)); //When implementing 6 range selectors, this gets Sextuple Box/Horizontal
		weapons.Add(new Weapon(new ItemListID(weaponsListIndex, bronzeGreatspearIndex), "Bronze Greatspear", "A long spear with a bronze tip, made to be wielded in two hands.", "6S+12", "S+D", "BronzeSpear", Range.quadrupleVerticalIndex, 50, Weapon.mainHandSlotIndex, isTwoHanded));
		weapons.Add(new Weapon(new ItemListID(weaponsListIndex, wornBowIndex), "Worn Bow", "This bow is a little weathered, but can still answer the call of it's wielder", "7D+16", "3D", "WornBow", Range.doubleVerticalIndex, 55, Weapon.mainHandSlotIndex, isTwoHanded));
		weapons.Add(new Weapon(new ItemListID(weaponsListIndex, fightingCapeIndex), "Fighting Cape", "A cape rapped around the offhand, used to both deflect small blows and disorient the opponent.", "3C", "C", "Cape", Range.singleTargetIndex, 35, Weapon.offHandSlotIndex, isTwoHanded));
		weapons.Add(new Staff (new ItemListID(weaponsListIndex, staffIndex), "Staff", "A weathered length of oak-spar. It would serve as well as a walking aid or a bludgeon. Has high Base Damage.", "10", "D+W", "Staff", Range.doubleHorizontalIndex, 10, Weapon.mainHandSlotIndex, isTwoHanded));
		weapons.Add(new Weapon(new ItemListID(weaponsListIndex, ancientClawIndex), "Ancient Claw", "This battered gauntlet has sharp spikes protruding from its knuckles, which immitate an animal's claw. Its main use is to catch and hold an opponent, helping to line up a strike from your other hand or an ally.", "2S + 2D + 3", "D+2", "Claw", Range.singleTargetIndex, 15, Weapon.offHandSlotIndex, isOneHanded));
		weapons.Add(new Weapon(new ItemListID(weaponsListIndex, wickedKnifeIndex), "Wicked Knife", "A barbed knife, meant for gouging.", "2D + 3", "2D + 1", "CurvedDagger", Range.singleTargetIndex, 15, Weapon.offHandSlotIndex, isOneHanded));
		weapons.Add(new Staff (new ItemListID(weaponsListIndex, plankIndex), "Plank", "A long piece of wood, pulled from a shack wall. Poorly balanced, but it'll do in a pinch.", "2W + 2", "W+D", "Plank", Range.singleTargetIndex, 3, Weapon.mainHandSlotIndex, isOneHanded));
		weapons.Add(new Weapon(new ItemListID(weaponsListIndex, sharpRockIndex), "Sharp Rock", "A stone, chipped to have a meager edge. ", "2C + 3", "C+D", "SharpRock", Range.singleTargetIndex, 1, Weapon.mainHandSlotIndex, isOneHanded));
		

		//Armor(string key, string loreDescription, int worth, int armorRating, int slotID)

		armor.Add(new Armor(new ItemListID(armorListIndex, slaveRagsIndex), "Slave Rags", "A set of ratty burlap tunic and pants.", slaveRagsArmorValue, Armor.bodySlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, clothGlovesIndex), "Cloth Gloves", "Gloves made of a thick cloth. Useful for hard labor.", clothGlovesArmorValue, Armor.handsSlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, rottenSandalsIndex), "Rotten Sandals", "A pair of ankle high leather sandals whose soles have seen better days.", rottenSandalsArmorValue, Armor.feetSlotIndex));
		armor.Add(new Shield(new ItemListID(armorListIndex, potLidIndex), "Pot Lid", "The lid to a large bronze cauldron, sufficiently sturdy and wide to be used as a haphazard shield.", potLidArmorValue, Armor.offHandSlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, minersHelmetIndex), "Miner's Helmet", "A cheaply made copper head cover with a thin layer of cloth padding inside and a scrap of leather for a chin strap. Useful for protecting against the odd bat or falling rock, but not much else.", minersHelmetArmorValue, Armor.headSlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, leatherGlovesIndex), "Leather Gloves", "Gloves made to be worn with armor, but still suitable for protecting the hands during hard labor.", leatherGlovesArmorValue, Armor.handsSlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, paddedArmorIndex), "Padded Armor", "Armor made of heavy cloth.", paddedArmorArmorValue, Armor.bodySlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, thatchNecklaceIndex), "Thatch's Silver Necklace", "A necklace made of a silver medalion attached to a thin silver chain. A sun rising over the horizon is etched into the medalion.", thatchNecklaceArmorValue, Armor.trinketSlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, martialArtistsBeltIndex), martialArtistsBeltKey, "A simple belt made of rope. This belt gives you extra armor equal to twice your wisdom score + 6.", martialArtistsBeltArmorValue, Armor.trinketSlotIndex));
		armor.Add(new Shield(new ItemListID(armorListIndex, wardensShieldIndex), wardensShieldKey, "A shield made from bands of bronze fitted over a hard wood core. This shield provides armor equal to twice your strength + 10", wardensShieldArmorValue, Armor.offHandSlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, silverSpoonIndex), silverSpoonKey, "The holder of this spoon accumulates wealth at a faster rate. Monsters drop 20% more gold.", givesNoArmor, Armor.trinketSlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, leatherBootsIndex), "Leather Boots", "Solid boots made of cowhide. Meant to be worn with armor.", leatherBootsArmorValue, Armor.feetSlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, bronzeHelmetIndex), "Bronze Cavalry Helmet", "A Bronze Helmet in the Lovashi style. Well padded and comfortable to wear.", bronzeHelmetArmorValue, Armor.headSlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, bronzeCuirassIndex), "Bronze Cuirass", "Armor made of interlocking bronze scales.", bronzeCuirassArmorValue, Armor.bodySlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, salvagedGuardHelmIndex), salvagedGuardHelmKey, "A helm taken from a slain guard. Buff out that dent and it's good as new.", salvagedGuardHelmArmorValue, Armor.headSlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, salvagedGuardArmorIndex), salvagedGuardArmorKey, "A set of armor stripped off a dead guard. Don't mind whatever that smell is.", salvagedGuardArmorArmorValue, Armor.bodySlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, salvagedGuardGlovesIndex), salvagedGuardGlovesKey, "A pair of gloves taken from a dead guard. Their literal death grip provides a bonus equal to your dexterity to crit chance.", salvagedGuardGlovesArmorValue, Armor.handsSlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, salvagedGuardBootsIndex), salvagedGuardBootsKey, "A pair of boots taken from a dead guard. His soles have left his body.", salvagedGuardBootsArmorValue, Armor.feetSlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, andrasLuckyTalismanIndex), luckyTalismanKey, "András's lucky talisman. Let's hope it serves you better than it served him.", givesNoArmor, Armor.trinketSlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, delversDreamIndex), delversDreamKey, "A gem, finely cut, with a deep blue hue. The longer you stare, the more certain you become that you do not hold the gem, but it holds you. 10% bonus mental resistance when equipped.", delversDreamArmorValue, Armor.trinketSlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, cookingPotIndex), cookingPotKey, "A pot from the kitchens. Worn about the head, it could provide some meager protection.", cookingPotArmorValue, Armor.headSlotIndex));
		armor.Add(new Armor(new ItemListID(armorListIndex, bronzeBadgeIndex), bronzeBadgeKey, "This bronze badge looks bruised and worn, as if it was just pulled from a fire.", bronzeBadgeArmorValue, Armor.trinketSlotIndex));
		

		//QuestItem(string key, string loreDescription, int ID)

		questItems.Add(new QuestItem(new ItemListID(questItemListIndex, leafSamplesIndex), "Leaf Samples", "An array of green leaves of all shapes and sizes.", leafSamplesIndex));
		questItems.Add(new QuestItem(new ItemListID(questItemListIndex, guardDiaryIndex), "Guard Diary", "This looks to be a journal kept by one of the guards. The final entry reads: " +
		"\"I've seen Ond leaving the second level of the mine many times, but by the time I get to the first level he's gone. Where " +
		"is he going? Maybe he knows something I don't.\"", guardDiaryIndex));
		questItems.Add(new QuestItem(new ItemListID(questItemListIndex, winchIndex), "Winch", "A piece of bent bronze with a wooden handle. Perhaps it fits some mechanism?", winchIndex));
		questItems.Add(new QuestItem(new ItemListID(questItemListIndex, candyIndex), "Candy", "A few pieces of hardened honey and ginger, kept inside a small stoppered clay jar.", 35, candyIndex));
		questItems.Add(new QuestItem(new ItemListID(questItemListIndex, toolBundleIndex), "Tool Bundle", "A large amount of picks, mattocks, axes, and shovels. An array of improvised weapons waiting for wielders.", toolBundleIndex));
		questItems.Add(new QuestItem(new ItemListID(questItemListIndex, blastingJellyIndex), "Blasting Jelly", "A small barrel containing a strange smelling, gelatinous mixture. Inert without it's primer and igniting agent.", blastingJellyIndex));
		questItems.Add(new QuestItem(new ItemListID(questItemListIndex, claysNoteIndex), "Clay's Note", "", claysNoteIndex));
		questItems.Add(new QuestItem(new ItemListID(questItemListIndex, laszloBadgeIndex), "Guard László's Badge", "This bronze badge is in the shape of a large coin, with the Lovashi symbol of a horse and it's rider etched into it.", laszloBadgeIndex));

		//Key(string key, string loreDescription, int ID)

		keys.Add(new Key(new ItemListID(keyItemListIndex, mineArmoryKeyIndex), "Mine Armory Key", "The key to the abandoned armory within the camp's mines.", mineArmoryKeyIndex));
		keys.Add(new Key(new ItemListID(keyItemListIndex, pitCellKeyIndex), "Pit Cell Key", "This key opens the door to the pit.", pitCellKeyIndex));
		keys.Add(new Key(new ItemListID(keyItemListIndex, directorsOfficeKeyFrontIndex), "Office Key Front", "This is the front half of a key that opens the door to the Camp Director's Office.", directorsOfficeKeyFrontIndex));
		keys.Add(new Key(new ItemListID(keyItemListIndex, directorsOfficeKeyBackIndex), "Office Key Back", "This is the back half of a key that opens the door to the Camp Director's Office.", directorsOfficeKeyBackIndex));
		keys.Add(new Key(new ItemListID(keyItemListIndex, munitionsKeyIndex), "Munitions Key", "This is key to the munitions storage room on the lowest level of the mine.", munitionsKeyIndex));

		//TreasureItem(string key, string loreDescription, int worth)

		treasure.Add(new TreasureItem(new ItemListID(treasureItemListIndex, ironNuggetIndex), "Iron Nugget", "A small lump of iron. It was possibly once part of some larger object, or is an eroded smaller item such as an old clasp or nail.", 100));
		treasure.Add(new TreasureItem(new ItemListID(treasureItemListIndex, goldLocketIndex), "Gold Locket", "A gold locket worn around the neck. If you squint you can still make out the engravings within the locket.", 150));
		treasure.Add(new TreasureItem(new ItemListID(treasureItemListIndex, smallCoinPurseIndex), "Small Coin Purse", "A coin purse that still has a small collection of bronze and silver coins in it.", 75));
        treasure.Add(new TreasureItem(new ItemListID(treasureItemListIndex, urosIronNuggetIndex), "Lost Iron Nugget", "A small lump of iron. It was possibly once part of some larger object, or is an eroded smaller item such as an old clasp or nail.", 100));
		
		partyMemberWeapons.Add(new Weapon(new ItemListID(partyMemberWeaponListIndex, redPickLevel1Index), NPCNameList.thatch + "'s Heavy Pick", "A large bronze pick meant to be used in two hands.", "8 + C", "2", "TwoHandedPick", Range.doubleVerticalIndex, 15, Weapon.mainHandSlotIndex, isTwoHanded));
		partyMemberWeapons.Add(new Weapon(new ItemListID(partyMemberWeaponListIndex, redPickLevel2Index), NPCNameList.thatch + "'s Heavy Pick", "A large bronze pick meant to be used in two hands.", "11 + 2C", "4", "TwoHandedPick", Range.tripleHookIndex, 15, Weapon.mainHandSlotIndex, isTwoHanded));
		partyMemberWeapons.Add(new Weapon(new ItemListID(partyMemberWeaponListIndex, redPickLevel3Index), NPCNameList.thatch + "'s Heavy Pick", "A large bronze pick meant to be used in two hands.", "16 + 3C", "6", "TwoHandedPick", Range.tripleHookIndex, 15, Weapon.mainHandSlotIndex, isTwoHanded));
		partyMemberWeapons.Add(new Weapon(new ItemListID(partyMemberWeaponListIndex, redPickLevel4Index), NPCNameList.thatch + "'s Heavy Pick", "A large bronze pick meant to be used in two hands.", "23 + 4C", "8", "TwoHandedPick", Range.tripleHookIndex, 15, Weapon.mainHandSlotIndex, isTwoHanded));
		partyMemberWeapons.Add(new Weapon(new ItemListID(partyMemberWeaponListIndex, redPickLevel5Index), NPCNameList.thatch + "'s Heavy Pick", "A large bronze pick meant to be used in two hands.", "32 + 5C", "12", "TwoHandedPick", Range.tripleHookIndex, 15, Weapon.mainHandSlotIndex, isTwoHanded));
		partyMemberWeapons.Add(new Weapon(new ItemListID(partyMemberWeaponListIndex, nandorCudgelLevel1Index), NPCNameList.nandor + "'s Cudgel", "A wooden club made from a fallen tree branch.", "6 + C", "4", "Cudgel", Range.singleTargetIndex, 3, Weapon.mainHandSlotIndex, isTwoHanded));
		partyMemberWeapons.Add(new Weapon(new ItemListID(partyMemberWeaponListIndex, nandorCudgelLevel2Index), NPCNameList.nandor + "'s Cudgel", "A wooden club made from a fallen tree branch.", "8 + 2C", "6", "Cudgel", Range.doubleVerticalIndex, 3, Weapon.mainHandSlotIndex, isTwoHanded));
		partyMemberWeapons.Add(new Weapon(new ItemListID(partyMemberWeaponListIndex, nandorCudgelLevel3Index), NPCNameList.nandor + "'s Cudgel", "A wooden club made from a fallen tree branch.", "11 + 3C", "8", "Cudgel", Range.doubleVerticalIndex, 3, Weapon.mainHandSlotIndex, isTwoHanded));
		partyMemberWeapons.Add(new Weapon(new ItemListID(partyMemberWeaponListIndex, nandorCudgelLevel4Index), NPCNameList.nandor + "'s Cudgel", "A wooden club made from a fallen tree branch.", "15 + 4C", "10", "Cudgel", Range.quadrupleBoxIndex, 3, Weapon.mainHandSlotIndex, isTwoHanded));
		partyMemberWeapons.Add(new Weapon(new ItemListID(partyMemberWeaponListIndex, nandorCudgelLevel5Index), NPCNameList.nandor + "'s Cudgel", "A wooden club made from a fallen tree branch.", "20 + 5C", "14", "Cudgel", Range.quadrupleBoxIndex, 3, Weapon.mainHandSlotIndex, isTwoHanded));
		partyMemberWeapons.Add(new Weapon(new ItemListID(partyMemberWeaponListIndex, carterShivLevel1Index), NPCNameList.carter + "'s Shiv", "A weapon made from a bronze nail tied to a small piece of wood.", "14 + C", "8", "Shiv", Range.singleTargetIndex, 3, Weapon.mainHandSlotIndex, isTwoHanded));
		partyMemberWeapons.Add(new Weapon(new ItemListID(partyMemberWeaponListIndex, carterShivLevel2Index), NPCNameList.carter + "'s Shiv", "A weapon made from a bronze nail tied to a small piece of wood.", "22 + 2C", "12", "Shiv", Range.singleTargetIndex, 3, Weapon.mainHandSlotIndex, isTwoHanded));
		partyMemberWeapons.Add(new Weapon(new ItemListID(partyMemberWeaponListIndex, carterShivLevel3Index), NPCNameList.carter + "'s Shiv", "A weapon made from a bronze nail tied to a small piece of wood.", "28 + 3C", "16", "Shiv", Range.singleTargetIndex, 3, Weapon.mainHandSlotIndex, isTwoHanded));
		partyMemberWeapons.Add(new Weapon(new ItemListID(partyMemberWeaponListIndex, carterShivLevel4Index), NPCNameList.carter + "'s Shiv", "A weapon made from a bronze nail tied to a small piece of wood.", "34 + 4C", "20", "Shiv", Range.singleTargetIndex, 3, Weapon.mainHandSlotIndex, isTwoHanded));
		partyMemberWeapons.Add(new Weapon(new ItemListID(partyMemberWeaponListIndex, carterShivLevel5Index), NPCNameList.carter + "'s Shiv", "A weapon made from a bronze nail tied to a small piece of wood.", "44 + 5C", "28", "Shiv", Range.doubleVerticalIndex, 3, Weapon.mainHandSlotIndex, isTwoHanded));

		books.Add(new BookItem(new ItemListID(bookListIndex, mineGuardsJournalIndex), BookList.mineGuardsJournalKey, "This looks to be a journal kept by one of the guards. The pages of this book are thick with writing, most of it about the mundane.", mineGuardsJournalIndex, new string[] { BookList.mineGuardsJournalReadFlag }, "Hidden Away", 0));
		books.Add(new BookItem(new ItemListID(bookListIndex, pageDiaryFirstEntryIndex), BookList.pageFirstDiaryEntryKey, "A piece of parchment serving as a portion of the diary of Page the scholar.", pageDiaryFirstEntryIndex, new string[] { BookList.pageFirstDiaryEntryReadFlag }, "Delving Deeper", 0));
		books.Add(new BookItem(new ItemListID(bookListIndex, pageDiarySecondEntryIndex), BookList.pageSecondDiaryEntryKey, "A piece of parchment serving as a portion of the diary of Page the scholar.", pageDiarySecondEntryIndex, new string[] { BookList.pageSecondDiaryEntryReadFlag }, "Delving Deeper", 2));
		books.Add(new BookItem(new ItemListID(bookListIndex, orderTranscriptIndex), BookList.ordersTranscriptKey, "A long wax tablet with quickly marked characters imprinted upon it, detailing orders to be relayed to the Lovashi guards.", orderTranscriptIndex, new string[] { BookList.ordersTranscriptReadFlag }, "The Plan", 15));
		books.Add(new BookItem(new ItemListID(bookListIndex, theInventionOfSinIndex), BookList.theInventionOfSinKey, "A large treatise, bound in hide and well-leafed.", theInventionOfSinIndex, new string[] { }));
		books.Add(new BookItem(new ItemListID(bookListIndex, keybindingInfoIndex), BookList.keybindingInfoKey, "All keybindings, laid out.", keybindingInfoIndex, new string[] { }));
		books.Add(new BookItem(new ItemListID(bookListIndex, horsetonguePronunciationGuideIndex), BookList.horsetonguePronunciationGuideKey, "A small scrap of animal hide with the phonetic alphabet of the horsetongue scratched into it.", horsetonguePronunciationGuideIndex, new string[] { }));
		books.Add(new BookItem(new ItemListID(bookListIndex, nightOfEmptyPlinthsIndex), BookList.theNightOfEmptyPlinthsKey, "A rolled piece of tanned bark. To read the contents hidden within, it must be unrolled carefully to prevent it from tearing.", nightOfEmptyPlinthsIndex, new string[] { }));
		books.Add(new BookItem(new ItemListID(bookListIndex, saintsAndSaintsIndex), BookList.saintsAndSaintsKey, "A rolled piece of tanned bark. To read the contents hidden within, it must be unrolled carefully to prevent it from tearing.", saintsAndSaintsIndex, new string[] { }));
		books.Add(new BookItem(new ItemListID(bookListIndex, pitSecondEntranceNoteIndex), BookList.pitSecondEntranceNoteKey, "A quick note, jotted in a shaky hand.", pitSecondEntranceNoteIndex, new string[] {BookList.pitSecondEntranceNoteReadFlag}, "Rescue Broglin", 3));
		books.Add(new BookItem(new ItemListID(bookListIndex, pitClosureNoteIndex), BookList.pitClosureNoteKey, "A quick note, jotted in a shaky hand.", pitClosureNoteIndex, new string[] {BookList.pitClosureNoteReadFlag}));


		allItems.Add(usableItems);          // listIndex = 0
		allItems.Add(weapons);              // listIndex = 1
		allItems.Add(armor);                // listIndex = 2
		allItems.Add(questItems);           // listIndex = 3
		allItems.Add(keys);                 // listIndex = 4
		allItems.Add(treasure);             // listIndex = 5
		allItems.Add(partyMemberWeapons);   // listIndex = 6
		allItems.Add(books);                // listIndex = 7
	}
	
	public static Item getItem(string listIndex, string itemIndex)
	{
		
		int intListIndex = int.Parse(listIndex);
		int intItemIndex = int.Parse(itemIndex);
		
		return getItem(intListIndex, intItemIndex, 1);
	}
	
	public static Item getItem(ItemListID itemListID)
	{
		return getItem(itemListID.listIndex, itemListID.itemIndex, itemListID.quantity);
	}
	
	public static Item getItem(string listIndex, string itemIndex, string quantity)
	{
		
		int intListIndex = int.Parse(listIndex);
		int intItemIndex = int.Parse(itemIndex);
		int intQuantity	 = int.Parse(quantity);
		
		return getItem(intListIndex, intItemIndex, intQuantity);
	}
	
	public static Item getItem(int listIndex, int itemIndex)
	{
		return getItem(listIndex, itemIndex, 1);
	}
	
	public static Item getItem(int listIndex, int itemIndex, int quantity){
		
		ArrayList currentItemList = (ArrayList) allItems[listIndex];
		
		Item itemTemplate = (Item) currentItemList[itemIndex];
		
		Item output = (Item) itemTemplate.Clone();
		
		output.setQuantity(quantity);
		
		return output;
	}
	
	public static bool addableToInventory(Item item)
	{
		int listIndex = item.getItemListID().listIndex;
		int itemIndex = item.getItemListID().itemIndex;

		switch (listIndex, itemIndex)
		{
			case (weaponsListIndex, mainHandFistIndex):
			case (weaponsListIndex, offHandFistIndex):
			case (weaponsListIndex, improvedMainHandFistIndex):
			case (weaponsListIndex, greaterMainHandFistIndex):
			case (weaponsListIndex, ruinousMainHandFistIndex):
			return false;
		}

		return true;
	}
	
	public static Weapon getMainHandFist(AllyStats targetStats)
	{
		if (targetStats == null || targetStats.getWisdomWithoutBoosts() < Wisdom.improvedStrikesLevel)
		{
			return (Weapon)getItem(weaponsListIndex, mainHandFistIndex, 1);

		}
		else if (targetStats.getWisdomWithoutBoosts() >= Wisdom.improvedStrikesLevel && targetStats.getWisdomWithoutBoosts() < Wisdom.greaterStrikesLevel)
		{
			return (Weapon)getItem(weaponsListIndex, improvedMainHandFistIndex, 1);

		}
		else if (targetStats.getWisdomWithoutBoosts() >= Wisdom.greaterStrikesLevel && targetStats.getWisdomWithoutBoosts() < Wisdom.ruinousStrikesLevel)
		{
			return (Weapon)getItem(weaponsListIndex, greaterMainHandFistIndex, 1);

		}
		else
		{
			return (Weapon)getItem(weaponsListIndex, ruinousMainHandFistIndex, 1);
		}
	}
	
	public static Weapon getOffHandFist()
	{
		return (Weapon) getItem(weaponsListIndex, offHandFistIndex, 1);
	}

}
