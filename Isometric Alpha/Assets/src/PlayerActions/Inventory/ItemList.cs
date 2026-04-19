using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemListID
{
	public const string listIndexElementName = "listIndex";
	public const string itemIndexElementName = "itemIndex";
	public const string quantityElementName = "quantity";

	public int listIndex;
	public int itemIndex;
	public int quantity;
	
	public ItemListID(int listIndex, int itemIndex, int quantity = 1)
	{
		this.listIndex = listIndex;
		this.itemIndex = itemIndex;
		this.quantity = quantity;
	}
}

public class ArmorListID : ItemListID
{
	public ArmorListID(int itemIndex, int quantity = 1):
    base(ItemList.armorListIndex, itemIndex, quantity)
	{
	}
}

public class WeaponListID : ItemListID
{
	public WeaponListID(int itemIndex, int quantity = 1):
    base(ItemList.weaponsListIndex, itemIndex, quantity)
	{
	}
}

public static class ItemList 
{
    public static List<List<Item>> allItems;

    #region Drop Tables
	public static DropTable slaveMineDT;
	public static DropTable lovashiGuardsDT;
    #endregion

	public const int onlyAcceptableEquippedItemQuantity = 1;
	public const string dominantFistKey = "Dominant Fist";
	public const string fistKey = "Fist";

    public const int rationsHealingAmount = 30;
    public const int rockCakeHealingAmount = 40;
	public const int properFoodHealingAmount = 50;
	public const int horseFleshHealingAmount = 75;

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
    public const int rockCakeIndex = 6;
	public const int horseFleshIndex = 7;
	
	public const int cudgelIndex = 0;
	public const int shivIndex = 1;
	public const int mainHandFistIndex = 2;
	// public const int offHandFistIndex = 3;
	public const int malletIndex = 4;
	public const int lightPickIndex = 5;
	public const int heavyPickIndex = 6;
	public const int bronzeBarIndex = 7;
	// public const int bronzeDirkIndex = 8;
	public const int improvedMainHandFistIndex = 9;
	public const int greaterMainHandFistIndex = 10;
	public const int ruinousMainHandFistIndex = 11;
	public const int bronzeGreatspearIndex = 12; // Manse 2F-3b
	public const int wornBowIndex = 13;		 //Manse 2F-2c
	public const int fightingCapeIndex = 14; //Pit 1a
    public const int staffIndex = 15;	//Mine lvl 2 6 (wisdom room)
	// public const int ancientClawIndex = 16; //Pit 2c
	// public const int wickedKnifeIndex = 17; //Mine lvl 2 5b (dexterity room)
	public const int plankIndex = 18;
	public const int sharpRockIndex = 19;
	public const int thinBladeIndex = 20;
	public const int scaldIndex = 21;

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
	public const int luckyTalismanIndex = 18;
	public const int delversDreamIndex = 19;
	public const int cookingPotIndex = 20;
	public const int bronzeBadgeIndex = 21;
	public const int plumedHelmetIndex = 22;
	public const int offHandFistIndex = 23;
	public const int bronzeDirkIndex = 24;
	public const int ancientClawIndex = 25; //Pit 2c
    public const int wickedKnifeIndex = 26; //Mine lvl 2 5b (dexterity room)
    public const int leatherArmorIndex = 27;
    public const int signalTorchIndex = 28;

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
    public const int barracksArmoryKeyIndex = 5;

    public const string barracksArmoryKeyName = "Barracks Armory Key";
	
	public const int ironNuggetIndex = 0;
	public const int goldLocketIndex = 1;	  // x1
	public const int smallCoinPurseIndex = 2; // x2
	public const int urosIronNuggetIndex = 3;
	
	public const int mineGuardsDiaryIndex = 0;
	public const int pageDiaryFirstEntryIndex = 1;
	public const int pageDiarySecondEntryIndex = 2;
    public const int orderTranscriptIndex = 3;
    public const int theInventionOfSinIndex = 4;
	public const int guideToSkillsIndex = 5;
	public const int horsetonguePronunciationGuideIndex = 6;
	public const int nightOfEmptyPlinthsIndex = 7;
	public const int saintsAndSaintsIndex = 8;
	public const int pitSecondEntranceNoteIndex = 9;
	public const int pitClosureNoteIndex = 10;
	public const int directorsJournalIndex = 11;

    #region Weapon Keys

    public const string bronzeDirkKey = bronzeSetPrefix + "Dirk";

    #endregion
	
    #region Armor Set Keys

    private const string helmSuffix = "Helm";
    private const string helmetSuffix = "Helmet";
    private const string armorSuffix = "Armor";
    private const string cuirassSuffix = "Cuirass";
    private const string torsoSuffix = "Torso";
    private const string glovesSuffix = "Gloves";
    private const string guantletsSuffix = "Gauntlets";
    private const string bootsSuffix = "Boots";
    private const string sandalsSuffix = "Sandals";

    private const string cavalryArmorDescriptor = "Cavalry ";

    //starter set
	public const string minersHelmetKey = "Miner's " + helmetSuffix;
	public const string slaveRagsKey = "Slave Rags";
	public const string clothGlovesKey = "Cloth " + glovesSuffix;
    public const string rottenSandalsKey = "Rotten " + sandalsSuffix;
    public const string potLidKey = "Pot Lid";

    private const string salvagedGuardSetPrefix = "Salvaged Guard ";
	public const string salvagedGuardHelmKey = salvagedGuardSetPrefix + helmSuffix;
	public const string salvagedGuardArmorKey = salvagedGuardSetPrefix + armorSuffix;
	public const string salvagedGuardGlovesKey = salvagedGuardSetPrefix + glovesSuffix;
	public const string salvagedGuardBootsKey = salvagedGuardSetPrefix + bootsSuffix;
	
	public const string leatherSetPrefix = "Leather ";
	public const string leatherArmorKey = leatherSetPrefix + armorSuffix;
	public const string leatherGlovesKey = leatherSetPrefix + glovesSuffix;
	public const string leatherBootsKey = leatherSetPrefix + bootsSuffix;

    public const string bronzeSetPrefix = "Bronze ";
    public const string bronzeCuirassKey = bronzeSetPrefix + cuirassSuffix;
    public const string bronzeCavalryHelmetKey = bronzeSetPrefix + cavalryArmorDescriptor + helmetSuffix;

    public const string paddedSetPrefix = "Padded ";
    public const string paddedArmorKey = paddedSetPrefix + armorSuffix;

    #endregion

    public const string wickedKnifeKey = "Wicked Knife";
    public const string ancientClawKey = "Ancient Claw";

    public const string martialArtistsBeltKey = "Martial Artist's Belt";
	public const string wardensShieldKey = "Warden's Shield";
	public const string silverSpoonKey = "Silver Spoon";
	public const string luckyTalismanKey = "Lucky Talisman";
	public const string delversDreamKey = "Delver's Dream";
	public const string bronzeBadgeKey = "Bronze Badge";
    public const string thatchNecklaceKey = "Thatch's Silver Necklace";
    public const string plumedHelmetKey = "Ancient Plumed " + helmetSuffix;
	public const string cookingPotKey = "Cooking Pot";
	public const string signalTorchKey = "Signal Torch";

    public const string toolBundleKey = "Tool Bundle";

	[RuntimeInitializeOnLoadMethod]
	private static void initializeItemList()
	{
        allItems = new List<List<Item>>();

		List<Item> usableItems = new List<Item>();
		List<Item> weapons = new List<Item>();
		List<Item> armor = new List<Item>();
		List<Item> questItems = new List<Item>();
		List<Item> keys = new List<Item>();
		List<Item> treasure = new List<Item>();
		List<Item> partyMemberWeapons = new List<Item>();
		List<Item> books = new List<Item>();

		//HealingItem(string key, string loreDescription, string useDescription, int worth, int amountToHeal)
		usableItems.Add(new HealingItem(new ItemListID(usableItemListIndex, rationsIndex), "Rations", "Moldy bread and grimy pemmican.", ItemSpriteList.rationsSprite, 5, rationsHealingAmount, AudioClipList.playEatingSFX));
		usableItems.Add(new RestorationItem(new ItemListID(usableItemListIndex, bandagesIndex), "Bandages", "Wrappings made from a coarse, firm cloth.", "Used to remove all " + TraitType.Wound.ToString() + " Traits from a friendly target.", ItemSpriteList.bandagesSprite, 15, Range.boxThreeIndex, TraitType.Wound));
		usableItems.Add(new SkillReplenishItem(new ItemListID(usableItemListIndex, thistleTeaIndex), "Thistle Tea", "Tea made from the flower of local thistle plants. Sharpens the senses and reinvigorates the mind.", "Used to replenish a use of the Cunning Skill. Can not be used to increase your cunning uses above your maximum.", ItemSpriteList.teaSprite, 25, AudioClipList.playSipSFX));
		usableItems.Add(new HealingItem(new ItemListID(usableItemListIndex, properFoodIndex), "Proper Food", "Food worth eating. It'll fill you up and more so.", ItemSpriteList.properFoodSprite, 15, properFoodHealingAmount, AudioClipList.playEatingSFX));
		usableItems.Add(new TraitApplicationItem(new ItemListID(usableItemListIndex, chokegrassBombIndex), "Chokegrass Bomb", "A small tin casing filled with a powder that ignites when exposed to air. The smoke from this bomb attacks the eyes and lungs, preventing it's victims from attacking or defending themselves.", "Stuns all targets for 1 round.", ItemSpriteList.smokeBombSprite, 50, Range.boxThreeIndex, TraitList.choking, CombatItem.useDoesRequireAnAction, targetsEnemySection: true));
		usableItems.Add(new TraitApplicationItem(new ItemListID(usableItemListIndex, chewIndex), "Chew", "A leaf that is ground between one's teeth to get at the juices within. These secretions provide an energetic numbness that removes the ache from wounds and fatigue from muscles.", "Heals the user and increases their damage and crit chance.", StatSourceNameList.chewKey, 35, Range.singleTargetIndex, TraitList.chewBuzz, CombatItem.useDoesNotRequireAnAction, healsTarget: true));
        usableItems.Add(new HealingItem(new ItemListID(usableItemListIndex, rockCakeIndex), "Rock Cake", "A brittle roll that looks like a grey stone. Surprisingly, it appears to still be edible.", ItemSpriteList.rockCakeSprite, 8, rockCakeHealingAmount, AudioClipList.playEatingRockCakeSFX));
        usableItems.Add(new HealingItem(new ItemListID(usableItemListIndex, horseFleshIndex), "Horse Flesh", "Strips of salted horsemeat. Chewier than beef, but no less nutritious.", ItemSpriteList.meatSprite, 10, horseFleshHealingAmount, AudioClipList.playEatingSFX));

		//Weapon(string key, string loreDescription, string damageFormula, string critFormula, string iconName, int rangeIndex, int worth, int slotID)
		//Weapon(string key, string loreDescription, string damageFormula, string critFormula, string iconName, int rangeIndex, int worth, int slotID, bool isTwoHanded)

		weapons.Add(new Weapon(new WeaponListID(cudgelIndex), "Cudgel", "A wooden club made from a fallen tree branch.", "2S + 5", "S+D", ItemSpriteList.cudgelSprite, Range.verticalOneIndex, 3, isOneHanded, EffectAnimationType.Blunt));
		weapons.Add(new Weapon(new WeaponListID(shivIndex), "Shiv", "A weapon made from a bronze nail tied to a small piece of wood.", "2D + 7", "3D", ItemSpriteList.shivSprite, Range.singleTargetIndex, 3, isOneHanded, EffectAnimationType.Pierce));
		weapons.Add(new Fist(new WeaponListID(mainHandFistIndex), dominantFistKey, "Good old fashioned meat bludgeons.", "S+D+W", "D+W", "FistIcon", Range.singleTargetIndex));
        weapons.Add(null);
		weapons.Add(new Weapon(new WeaponListID(malletIndex), "Mallet", "A large hammer used to beat pitons into rock walls.", "3S + 6", "D", ItemSpriteList.malletSprite, Range.horizontalOneIndex, 15, isOneHanded, EffectAnimationType.Blunt));
		weapons.Add(new Weapon(new WeaponListID(lightPickIndex), "Light Pick", "A bronze pick meant to be used in one hand.", "3D + 8", "3D", ItemSpriteList.oneHandedPickSprite, Range.horizontalOneIndex, 15, isOneHanded, EffectAnimationType.Pierce));
		weapons.Add(new Weapon(new WeaponListID(heavyPickIndex), "Heavy Pick", "A large bronze pick meant to be used in two hands.", "4S + 7", "D", ItemSpriteList.twoHandedPickSprite, Range.hookOneIndex, 15, isTwoHanded, EffectAnimationType.Pierce));
		weapons.Add(new Weapon(new WeaponListID(bronzeBarIndex), "Bronze Bar", "A long thin bronze ingot. A bit oxidized, but hefty.", "3S + 8", "D + 2", ItemSpriteList.bronzeBarSprite, Range.horizontalThreeIndex, 5, isTwoHanded, EffectAnimationType.Blunt));
		weapons.Add(null);
		weapons.Add(new Fist(new WeaponListID(improvedMainHandFistIndex), dominantFistKey, "Good old fashioned meat bludgeons.", "2W+S+D+4", "W+D+2", "ImprovedFistIcon", Range.verticalOneIndex));
		weapons.Add(new Fist(new WeaponListID(greaterMainHandFistIndex), dominantFistKey, "Good old fashioned meat bludgeons.", "3W+S+D+8", "W+D+4", "GreaterFistIcon", Range.boxOneIndex));
		weapons.Add(new Fist(new WeaponListID(ruinousMainHandFistIndex), dominantFistKey, "Good old fashioned meat bludgeons.", "4W+S+D+16", "W+D+8", "RuinousFistIcon", Range.singleTargetIndex)); //When implementing 6 range selectors, this gets Sextuple Box/Horizontal
		weapons.Add(new Weapon(new WeaponListID(bronzeGreatspearIndex), "Bronze Greatspear", "A long spear with a bronze tip, made to be wielded in two hands.", "6S+12", "S+D", ItemSpriteList.bronzeSpearSprite, Range.verticalThreeIndex, 50, isTwoHanded, EffectAnimationType.Pierce));
		weapons.Add(new Weapon(new WeaponListID(wornBowIndex), "Worn Bow", "This bow is a little weathered, but can still answer the call of it's wielder", "7D+16", "3D", ItemSpriteList.wornBowSprite, Range.verticalOneIndex, 55, isTwoHanded, EffectAnimationType.Pierce));
		weapons.Add(new Weapon(new WeaponListID(fightingCapeIndex), "Fighting Cape", "A cape wrapped around the mainhand, used to both deflect small blows and disorient the opponent. Often paired with a dagger in the offhand.", "3C+4", "C", ItemSpriteList.capeSprite, Range.singleTargetIndex, 35, isOneHanded, EffectAnimationType.Slash));
		weapons.Add(new Staff (new WeaponListID(staffIndex), "Staff", "A weathered length of oak-spar. It would serve as well as a walking aid or a bludgeon. Has high Base Damage.", "W+12", "D+W", ItemSpriteList.staffSprite, Range.horizontalOneIndex, 10, isTwoHanded));
		weapons.Add(null);
		weapons.Add(null);
		weapons.Add(new Staff (new WeaponListID(plankIndex), "Plank", "A long piece of wood, pulled from a shack wall. Poorly balanced, but it'll do in a pinch.", "2W + 2", "W+D", ItemSpriteList.plankSprite, Range.singleTargetIndex, 3, isOneHanded));
		weapons.Add(new Weapon(new WeaponListID(sharpRockIndex), "Sharp Rock", "A stone, chipped to have a meager edge. ", "2C + 3", "C+D", ItemSpriteList.sharpRockSprite, Range.singleTargetIndex, 1, isOneHanded, EffectAnimationType.Pierce));
        weapons.Add(new StanceWeapon(new WeaponListID(thinBladeIndex), "Bronze <nobr>Thin-Blade</nobr>", "A long, slender, double-edged blade with no crossguard. Favored by swordsmen for its long reach, they are either wielded solo or with a matching dagger.", "2W+2D+6", "D+W+1", ItemSpriteList.thinbladeSprite, Range.verticalOneIndex, worth: 55, isOneHanded, EffectAnimationType.Slash));
		weapons.Add(new Weapon(new WeaponListID(scaldIndex), "Scald", "Kende's trusty frying pan. Those struck with it come away burned.", "2S+2D+2C+9", "S+D+C", ItemSpriteList.fryingPanSprite, Range.horizontalOneIndex, worth: 75, isOneHanded, EffectAnimationType.Blunt, traitToApply: TraitList.roasted));

		//Armor(string key, string loreDescription, int worth, int armorRating, int slotID)

		armor.Add(new TierZeroBody(new ItemListID(armorListIndex, slaveRagsIndex), slaveRagsKey, "A set of ratty burlap tunic and pants."));
		armor.Add(new TierZeroHands(new ItemListID(armorListIndex, clothGlovesIndex), clothGlovesKey, "Gloves made of a thick cloth. Useful for hard labor."));
		armor.Add(new TierZeroFeet(new ItemListID(armorListIndex, rottenSandalsIndex), rottenSandalsKey, "A pair of ankle high leather sandals whose soles have seen better days."));
		armor.Add(new TierZeroShield(new ItemListID(armorListIndex, potLidIndex), potLidKey, "The lid to a large bronze cauldron, sufficiently sturdy and wide to be used as a haphazard shield."));
		armor.Add(new TierZeroHelmet(new ItemListID(armorListIndex, minersHelmetIndex), minersHelmetKey, "A copper head cover with a thin layer of cloth padding inside. Useful for protecting against the odd bat or falling rock, but not much else."));
		armor.Add(new TierOneHands(new ItemListID(armorListIndex, leatherGlovesIndex), leatherGlovesKey, "Gloves made to be worn with armor, but still suitable for protecting the hands during hard labor."));
		armor.Add(new TierOneBody(new ItemListID(armorListIndex, paddedArmorIndex), paddedArmorKey, "Armor made of heavy cloth. It feels sturdier than it sounds."));
		armor.Add(new Trinket(new ItemListID(armorListIndex, thatchNecklaceIndex), thatchNecklaceKey, "A necklace made of a silver medalion attached to a thin silver chain. A sun rising over the horizon is etched into the medalion's disk."));
		armor.Add(new Trinket(new ItemListID(armorListIndex, martialArtistsBeltIndex), martialArtistsBeltKey, "A simple belt made of rope. Unadorned and unburdened.", "2W+6"));
		armor.Add(new TierOneShield(new ItemListID(armorListIndex, wardensShieldIndex), wardensShieldKey, "A shield made from bands of bronze fitted over a hard wooden core."));
		armor.Add(new Trinket(new ItemListID(armorListIndex, silverSpoonIndex), silverSpoonKey, "The holder of this spoon accumulates wealth at a faster rate. Monsters drop 20% more gold."));
		armor.Add(new TierOneFeet(new ItemListID(armorListIndex, leatherBootsIndex), leatherBootsKey, "Solid boots made of cowhide. Meant to be worn with armor."));
		armor.Add(new TierOneHelmet(new ItemListID(armorListIndex, bronzeHelmetIndex), bronzeCavalryHelmetKey, "A bronze helmet in the Lovashi style. Well padded and comfortable to wear."));
		armor.Add(new TierTwoBody(new ItemListID(armorListIndex, bronzeCuirassIndex), bronzeCuirassKey, "Armor made of interlocking bronze scales."));
		armor.Add(new TierOneHelmet(new ItemListID(armorListIndex, salvagedGuardHelmIndex), salvagedGuardHelmKey, "A helm taken from a slain guard. Buff out that dent and it's good as new."));
		armor.Add(new TierOneBody(new ItemListID(armorListIndex, salvagedGuardArmorIndex), salvagedGuardArmorKey, "A set of armor stripped off a dead guard. Don't mind whatever that smell is."));
		armor.Add(new TierOneHands(new ItemListID(armorListIndex, salvagedGuardGlovesIndex), salvagedGuardGlovesKey, "A pair of gloves taken from a guard's cold, dead hands."));
		armor.Add(new TierOneFeet(new ItemListID(armorListIndex, salvagedGuardBootsIndex), salvagedGuardBootsKey, "A pair of boots taken from a dead guard. His soles have left his body."));
		armor.Add(new Trinket(new ItemListID(armorListIndex, luckyTalismanIndex), luckyTalismanKey, "András's lucky talisman. Let's hope it serves you better than it served him.", "4", "2"));
		armor.Add(new Trinket(new ItemListID(armorListIndex, delversDreamIndex), delversDreamKey, "A gem, finely cut, with a deep blue hue. The longer you stare, the more certain you become that you do not hold the gem, but it holds you."));
		armor.Add(new TierOneHelmet(new ItemListID(armorListIndex, cookingPotIndex), cookingPotKey, "A pot from the kitchens. Worn about the head, it could provide some meager protection."));
		armor.Add(new Trinket(new ItemListID(armorListIndex, bronzeBadgeIndex), bronzeBadgeKey, "This bronze badge looks bruised and worn, as if it was just pulled from a fire."));
		armor.Add(new TierOneHelmet(new ItemListID(armorListIndex, plumedHelmetIndex), plumedHelmetKey, "A battered helmet, with a ragged plume made of hair from an unknown beast."));
		armor.Add(new OffHandFist(new ItemListID(armorListIndex, offHandFistIndex), fistKey, "Good old fashioned meat bludgeons.", "S+D+W", "D+W"));
        armor.Add(new OffHandWeapon(new ItemListID(armorListIndex, bronzeDirkIndex), bronzeDirkKey, "A curved bronze blade, held in the off hand.", "S + D + 3", "D+2", "CurvedDagger"));
		armor.Add(new OffHandWeapon(new ItemListID(armorListIndex, ancientClawIndex), ancientClawKey, "This battered gauntlet has sharp spikes protruding from its knuckles, which immitate an animal's claw. Its main use is to catch and hold an opponent, helping to line up a strike from your other hand or an ally.", "2S + 2D + 3", "D+2", "Claw"));
		armor.Add(new OffHandWeapon(new ItemListID(armorListIndex, wickedKnifeIndex), wickedKnifeKey, "A barbed knife, meant for gouging.", "2D + 3", "2D + 1", "WickedKnife"));
		armor.Add(new TierOneBody(new ItemListID(armorListIndex, leatherArmorIndex), leatherArmorKey, "Leather pads of tanned oxhide that cover the torso."));
		armor.Add(new Trinket(new ItemListID(armorListIndex, signalTorchIndex), signalTorchKey, "A large torch used to direct archers in combat."));

		//QuestItem(string key, string loreDescription, int ID)

		questItems.Add(new QuestItem(new ItemListID(questItemListIndex, leafSamplesIndex), "Leaf Samples", "An array of green leaves of all shapes and sizes.", leafSamplesIndex));
		questItems.Add(new QuestItem(new ItemListID(questItemListIndex, guardDiaryIndex), "Guard Diary", "This looks to be a journal kept by one of the guards. The final entry reads: " +
		"\"I've seen Ond leaving the second level of the mine many times, but by the time I get to the first level he's gone. Where " +
		"is he going? Maybe he knows something I don't.\"", guardDiaryIndex));
		questItems.Add(new QuestItem(new ItemListID(questItemListIndex, winchIndex), "Winch", "A piece of bent bronze with a wooden handle. Perhaps it fits some mechanism?", winchIndex));
		questItems.Add(new QuestItem(new ItemListID(questItemListIndex, candyIndex), "Candy", "A few pieces of hardened honey and ginger, kept inside a small stoppered clay jar.", 35, candyIndex));
		questItems.Add(new QuestItem(new ItemListID(questItemListIndex, toolBundleIndex), toolBundleKey, "A large amount of picks, mattocks, axes, and shovels. An array of improvised weapons waiting for wielders.", toolBundleIndex));
		questItems.Add(new QuestItem(new ItemListID(questItemListIndex, blastingJellyIndex), "Blasting Jelly", "A small barrel containing a strange smelling, gelatinous mixture. Inert without it's primer and igniting agent.", blastingJellyIndex));
		questItems.Add(new QuestItem(new ItemListID(questItemListIndex, claysNoteIndex), "Clay's Note", "", claysNoteIndex));
		questItems.Add(new QuestItem(new ItemListID(questItemListIndex, laszloBadgeIndex), "Guard László's Badge", "This bronze badge is in the shape of a large coin, with the Lovashi symbol of a horse and it's rider etched into it.", laszloBadgeIndex));

		//Key(string key, string loreDescription, int ID)

		keys.Add(new Key(new ItemListID(keyItemListIndex, mineArmoryKeyIndex), "Mine Armory Key", "The key to the abandoned armory within the camp's mines.", mineArmoryKeyIndex));
		keys.Add(new Key(new ItemListID(keyItemListIndex, pitCellKeyIndex), "Pit Cell Key", "This key opens the door to the pit.", pitCellKeyIndex));
		keys.Add(new Key(new ItemListID(keyItemListIndex, directorsOfficeKeyFrontIndex), "Office Key Front", "This is the front half of a key that opens the door to the Camp Director's Office.", directorsOfficeKeyFrontIndex));
		keys.Add(new Key(new ItemListID(keyItemListIndex, directorsOfficeKeyBackIndex), "Office Key Back", "This is the back half of a key that opens the door to the Camp Director's Office.", directorsOfficeKeyBackIndex));
		keys.Add(new Key(new ItemListID(keyItemListIndex, munitionsKeyIndex), "Munitions Key", "This is key to the munitions storage room on the lowest level of the mine.", munitionsKeyIndex));
        keys.Add(new Key(new ItemListID(keyItemListIndex, barracksArmoryKeyIndex), barracksArmoryKeyName, "The key to the armory where the camp's guards keep all of their gear. The gate this key belongs to is sure to be heavily guarded.", barracksArmoryKeyIndex));
		
		//TreasureItem(string key, string loreDescription, int worth)

		treasure.Add(new TreasureItem(new ItemListID(treasureItemListIndex, ironNuggetIndex), "Iron Nugget", "A small lump of iron. It was possibly once part of some larger object, or is an eroded smaller item such as an old clasp or nail.", 100));
		treasure.Add(new TreasureItem(new ItemListID(treasureItemListIndex, goldLocketIndex), "Gold Locket", "A gold locket worn around the neck. If you squint you can still make out the engravings within the locket.", 150));
		treasure.Add(new TreasureItem(new ItemListID(treasureItemListIndex, smallCoinPurseIndex), "Small Coin Purse", "A coin purse that still has a small collection of bronze and silver coins in it.", 75));
        treasure.Add(new TreasureItem(new ItemListID(treasureItemListIndex, urosIronNuggetIndex), "Lost Iron Nugget", "A small lump of iron. It was possibly once part of some larger object, or is an eroded smaller item such as an old clasp or nail.", 100));
		
		books.Add(new BookItem(new ItemListID(bookListIndex, mineGuardsDiaryIndex), BookList.mineGuardsJournalKey, "This looks to be a journal kept by one of the guards. The pages of this book are thick with writing, most of it about the mundane.", mineGuardsDiaryIndex, new string[] { BookList.mineGuardsJournalReadFlag }, QuestNameList.hiddenAwayQuestTitle, QuestNameList.hiddenAwayStepTitleZero));
		books.Add(new BookItem(new ItemListID(bookListIndex, pageDiaryFirstEntryIndex), BookList.pageFirstDiaryEntryKey, "A piece of parchment serving as a portion of the diary of Page the scholar.", pageDiaryFirstEntryIndex, new string[] { BookList.pageFirstDiaryEntryReadFlag }, QuestNameList.delvingDeeperQuestTitle, QuestNameList.delvingDeeperStepTitleZero));
		books.Add(new BookItem(new ItemListID(bookListIndex, pageDiarySecondEntryIndex), BookList.pageSecondDiaryEntryKey, "A piece of parchment serving as a portion of the diary of Page the scholar.", pageDiarySecondEntryIndex, new string[] { BookList.pageSecondDiaryEntryReadFlag }, QuestNameList.delvingDeeperQuestTitle, QuestNameList.delvingDeeperStepTitleTwo));
		books.Add(new BookItem(new ItemListID(bookListIndex, orderTranscriptIndex), BookList.ordersTranscriptKey, "A long wax tablet with quickly marked characters imprinted upon it, detailing orders to be relayed to the Lovashi guards.", orderTranscriptIndex, new string[] { BookList.ordersTranscriptReadFlag }, QuestNameList.thePlanQuestTitle, QuestNameList.thePlanStepTitleFifteen));
		books.Add(new BookItem(new ItemListID(bookListIndex, theInventionOfSinIndex), BookList.theInventionOfSinKey, "A large treatise, bound in hide and well-leafed.", theInventionOfSinIndex, new string[] { }));
		books.Add(new BookItem(new ItemListID(bookListIndex, guideToSkillsIndex), BookList.guideToSkillsKey, "A description of the various Skills each of your Party Members can learn.", guideToSkillsIndex, new string[] { }));
		books.Add(new BookItem(new ItemListID(bookListIndex, horsetonguePronunciationGuideIndex), BookList.horsetonguePronunciationGuideKey, "A small scrap of animal hide with the phonetic alphabet of the horsetongue scratched into it.", horsetonguePronunciationGuideIndex, new string[] { }));
		books.Add(new BookItem(new ItemListID(bookListIndex, nightOfEmptyPlinthsIndex), BookList.theNightOfEmptyPlinthsKey, "A rolled piece of tanned bark. To read the contents hidden within, it must be unrolled carefully to prevent it from tearing.", nightOfEmptyPlinthsIndex, new string[] { }));
		books.Add(new BookItem(new ItemListID(bookListIndex, saintsAndSaintsIndex), BookList.saintsAndSaintsKey, "A rolled piece of tanned bark. To read the contents hidden within, it must be unrolled carefully to prevent it from tearing.", saintsAndSaintsIndex, new string[] { }));
		books.Add(new BookItem(new ItemListID(bookListIndex, pitSecondEntranceNoteIndex), BookList.pitSecondEntranceNoteKey, "A quick note, jotted in a shaky hand.", pitSecondEntranceNoteIndex, new string[] {BookList.pitSecondEntranceNoteReadFlag}, QuestNameList.rescueBroglinQuestTitle, QuestNameList.rescueBroglinStepTitleThree));
		books.Add(new BookItem(new ItemListID(bookListIndex, pitClosureNoteIndex), BookList.pitClosureNoteKey, "A quick note, jotted in a shaky hand.", pitClosureNoteIndex, new string[] {BookList.pitClosureNoteReadFlag}));
		books.Add(new BookItem(new ItemListID(bookListIndex, directorsJournalIndex), BookList.directorsJournalKey, "A small, tidy journal. The pages are filled with long, caligraphic characters scrawled with much patience.", directorsJournalIndex, new string[] {BookList.directorsJournalReadFlag}));


		allItems.Add(usableItems);          // listIndex = 0
		allItems.Add(weapons);              // listIndex = 1
		allItems.Add(armor);                // listIndex = 2
		allItems.Add(questItems);           // listIndex = 3
		allItems.Add(keys);                 // listIndex = 4
		allItems.Add(treasure);             // listIndex = 5
		allItems.Add(partyMemberWeapons);   // listIndex = 6
		allItems.Add(books);                // listIndex = 7

        slaveMineDT = new DropTable(3, 5, new DropTableEntry[]{
                                                                    new DropTableEntry(getItem(usableItemListIndex, rationsIndex),      .1f),
                                                                    new DropTableEntry(getItem(weaponsListIndex, malletIndex),          .025f),
                                                                    new DropTableEntry(getItem(armorListIndex, clothGlovesIndex),       .025f),
                                                                    new DropTableEntry(getItem(armorListIndex, rottenSandalsIndex),     .025f),
                                                                    new DropTableEntry(getItem(armorListIndex, potLidIndex),            .025f),
                                                                    new DropTableEntry(getItem(armorListIndex, minersHelmetIndex),      .025f),
                                                                    new DropTableEntry(getItem(treasureItemListIndex, ironNuggetIndex), .025f)
                                                            });	


	    lovashiGuardsDT =  new DropTable(6, 10, new DropTableEntry[]{
                                                                        new DropTableEntry(getItem(usableItemListIndex, properFoodIndex),        .05f),
                                                                        new DropTableEntry(getItem(armorListIndex, salvagedGuardArmorIndex),     .03f),
                                                                        new DropTableEntry(getItem(armorListIndex, salvagedGuardHelmIndex),      .03f),
                                                                        new DropTableEntry(getItem(armorListIndex, salvagedGuardBootsIndex),     .03f),
                                                                        new DropTableEntry(getItem(armorListIndex, salvagedGuardGlovesIndex),    .03f),
                                                                        new DropTableEntry(getItem(armorListIndex, bronzeDirkIndex),             .03f),
                                                                        new DropTableEntry(getItem(treasureItemListIndex, smallCoinPurseIndex),  .05f)
                                                                    });	
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
	
	public static Item getItem(int listIndex, int itemIndex, int quantity = 1)
    {
		List<Item> currentItemList = allItems[listIndex];
		
		Item itemTemplate = currentItemList[itemIndex];
		
        if(itemTemplate == null)
        {
            return null;
        }

		Item output = itemTemplate.clone();
		
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
			case (armorListIndex, offHandFistIndex):
			case (weaponsListIndex, improvedMainHandFistIndex):
			case (weaponsListIndex, greaterMainHandFistIndex):
			case (weaponsListIndex, ruinousMainHandFistIndex):
			return false;
		}

		return true;
	}
	
	public static Weapon getMainHandFist(AllyStats targetStats)
    {
        Weapon fist;

        if (targetStats == null || targetStats.getWisdomWithoutBoosts() < Wisdom.improvedStrikesLevel)
        {
            fist = (Weapon)getItem(weaponsListIndex, mainHandFistIndex, 1);

        }
        else if (targetStats.getWisdomWithoutBoosts() >= Wisdom.improvedStrikesLevel && targetStats.getWisdomWithoutBoosts() < Wisdom.greaterStrikesLevel)
        {
            fist = (Weapon)getItem(weaponsListIndex, improvedMainHandFistIndex, 1);

        }
        else if (targetStats.getWisdomWithoutBoosts() >= Wisdom.greaterStrikesLevel && targetStats.getWisdomWithoutBoosts() < Wisdom.ruinousStrikesLevel)
        {
            fist = (Weapon)getItem(weaponsListIndex, greaterMainHandFistIndex, 1);

        }
        else
        {
            fist = (Weapon)getItem(weaponsListIndex, ruinousMainHandFistIndex, 1);
        }

        fist.equipTarget = targetStats;

        return fist;
	}
	
	public static EquippableItem getOffHandFist()
	{
		return (Armor) getItem(armorListIndex, offHandFistIndex, 1);
	}

}

public static class ItemSpriteList
{
    public const string bandagesSprite = "Bandages";
    public const string bronzeBarSprite = "BronzeBar";
    public const string bronzeSpearSprite = "BronzeSpear";
    public const string capeSprite = "Cape";
    public const string cudgelSprite = "Cudgel";
    public const string curvedDaggerSprite = "CurvedDagger";
    public const string fryingPanSprite = "FryingPan";
    public const string malletSprite = "Mallet";
    public const string meatSprite = "Meat";
    public const string oneHandedPickSprite = "OneHandedPick";
    public const string plankSprite = "Plank";
    public const string properFoodSprite = "ProperFood";
    public const string rationsSprite = "Rations";
    public const string rockCakeSprite = "Rock Cake";
    public const string sharpRockSprite = "SharpRock";
    public const string shivSprite = "Shiv";
    public const string smallCoinPurseSprite = "Small Coin Purse";
    public const string smokeBombSprite = "SmokeBomb";
    public const string staffSprite = "Staff";
    public const string teaSprite = "Tea";
    public const string thinbladeSprite = "Thinblade";
    public const string twoHandedPickSprite = "TwoHandedPick";
    public const string wickedKnifeSprite = "WickedKnife";
    public const string wornBowSprite = "WornBow";
}