using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public static class TraitList
{
	public readonly static GridCoords[] fourCornersEnemySide = new GridCoords[]{ new GridCoords(3,0),
																		new GridCoords(0,0),
																		new GridCoords(0,3),
																		new GridCoords(3,3)
																		};

	//Permanent Trait Types
	public const string creatureTypeTraitType = "Creature Type";
	public const string interactionTypeTraitType = "Interaction";
	public const string positioningTraitType = "Positioning";
	public const string passiveTraitType = "Passive";
	public const string activatedPassiveTraitType = "Equipped Passive";
	public const string stanceTraitType = "Stance";
	public const string sizeTraitType = "Size";

	//Targeting traits
	public const string generatedSuffix = "Generated";
	public const string inaccurateBombardmentKey = "Inaccurate Bombardment";
	public const string rapidInaccurateBombardmentKey = "Rapid Inaccurate Bombardment";
	public const string clockwiseFourCornersEnemySideKey = "Four Corners Enemy Side";

	//Debuff Trait Types
	public const string woundTraitType = "Wound";
	public const string mentalTraitType = "Mental";

	//Buff Trait Types
	public const string protectionTraitType = "Protection";
	public const string boostTraitType = "Boost";
	public const string chargeTraitType = "Charge";

	public const bool isBuff = true;
	public const bool isDebuff = false;
    public static Dictionary<string, bool> buffDebuffTraitTypes;

	private const int zeroStacksAtStart = 0;
	private const int oneStackAtStart = 1;
	private const int twoStacksAtStart = 2;
	private const int threeStacksAtStart = 3;
	private const int fourStacksAtStart = 4;

	private const double crushingBlowArmorShred = .5;

	private const int oneStackPerApplication = 1;

	public const string extraShieldedKey = "Extra Shielded";

	public const string wormSplitsTraitKey = "Worm Splits";
	public const string wormBossSplitsTraitKey = "Worm Boss Splits";
	public const string wormExplodesTraitKey = "Worm Explodes";
	public const string wormBossExplodesTraitKey = "Worm Boss Explodes";
	public const string wormReviveTraitKey = "Worm Revive";
	public const string wormBossReviveTraitKey = "Worm Boss Revive";

	public const string roastedKey = "Roasted";

	public const string devastatingCriticalsKey = "Devastating Criticals";

    public const string caveMadnessKey = "Cave Madness";
	public const string chokingKey = "Choking";
	public const string chewKey = "Chew";

	public const string bufferKey = "Buffer";
	public const string healerKey = "Healer";
	public const string singleTargetBuffKey = "Single Target Buff";

    public static Dictionary<string, Trait> dictionaryOfTraits;
    public static Dictionary<string, Trait> dictionaryOfHiddenTraits;

	private const bool isPacificstic = true;
	private const bool isMandatoryTrait = true;
	private const bool preventsMovementTrait = true;
	private const bool preventsResurrection = true;

	private const bool isUntargetable = true;

	private const int halfHandStanceStartingStacks = 4;

	private const int vulnerableExtraDamage = 5;
	private const int acidVomitExtraDamage = 2;
	private const int roastedExtraDamage = 1;
	private const int bristledExtraDamage = 5;
	private const int insecureExtraDamage = 7;
	private const int cohesionExtraDamage = 6;
	public const int ralliedExtraDamage = 8;
	public const int chewHealing = 8;
	public const int chewExtraDamage = 4;
	public const int chewExtraCritPercent = 4;
	public const int caveMadnessExtraDamage = 3;
	public const int demoralizeExtraDamage = 5;
	private const int bloodlustExtraDamage = 1;
	private const int predationExtraDamage = 4;
	private const int halfHandStanceExtraDamage = 1;
	private const double shieldedDamageReduction = .5;
	private const double extraShieldedDamageReduction = .75;
	private const double stonewallDamageReduction = .75;
	private const double daringSacrificeDamageReduction = 1.0;
	private const double exitStrategyDamageReduction = .6;
	private const double chokeholdDamagePercentage = .5;

	private const int bloodlustMaximumStacks = 6;

	public const string crippledDamageFormula = "2D + 2W";

	private const int endOfRoundDuration = 1; //duration will always tick down once before the player/enemy gets to exploit
	private const int oneRoundDuration = 2; //so a 1 round duration is really just for the rest of that turns resolution
	private const int twoRoundDuration = 3;
	private const int threeRoundDuration = 4;
	private const int fourRoundDuration = 5;


	//permanent/mandatory monster traits
	public readonly static Trait master = new Trait("Master", creatureTypeTraitType, "A creature that leads other creatures. All Master creatures must be dead to win.", "Crown", Color.blue, isMandatoryTrait);
	public readonly static Trait minion = new Trait("Minion", creatureTypeTraitType, "A creature that takes orders from a Master. Most die in one hit.", "Collar", Color.red, isMandatoryTrait);
	public readonly static Trait summoned = new Trait("Summoned", creatureTypeTraitType, "A creature that is here at the behest of another, but cannot be controlled directly.", "Summoned", Color.black, isMandatoryTrait);

	public readonly static Trait frontLine = new PositioningTrait("Front Line", positioningTraitType, "This creature always spawns at the front of the enemy field.", "Front Line", Color.black, PositioningType.Frontline);
	public readonly static Trait backLine = new PositioningTrait("Back Line", positioningTraitType, "This creature always spawns at the back of the enemy field.", "Back Line", Color.black, PositioningType.Backline);

	public readonly static Trait evolutionary = new Trait("Evolutionary", interactionTypeTraitType, "Can be evolved into a better version of itself.", "Evolve", Color.black, isMandatoryTrait);
	public readonly static Trait immobile = new Trait("Immobile", interactionTypeTraitType, "Takes no actions. Cannot be moved.", "Immobile", Color.black, isMandatoryTrait, preventsMovementTrait, isPacificstic);
	public readonly static Trait large = new Trait("Large", sizeTraitType, "And in charge. This creature takes up multiple spaces and will take damage each time it is hit by the same attack. Cannot be moved.", "Large", Color.black, isMandatoryTrait, preventsMovementTrait);

	//all specific target priorities
	public readonly static SpecificTargetPriorityTrait specificCheckeredLeftAlliedSide = new SpecificTargetPriorityTrait("(6,2)", "SpecificTargetPriorityTrait", "", Color.black, new GridCoords(6, 2));
	public readonly static SpecificTargetPriorityTrait specificHexadecupleBoxEnemySide = new SpecificTargetPriorityTrait("(2,2)", "SpecificTargetPriorityTrait", "", Color.black, new GridCoords(2, 2));


	public readonly static Trait chaotic = new ChaoticTargetPriorityTrait();
	public readonly static Trait clockwiseFourCornersEnemySide = new ClockwiseTargetPriorityTrait(fourCornersEnemySide);
	public readonly static Trait empty = new EmptyTargetPriorityTrait();
	public readonly static Trait emptyGenerated2 = new GeneratedTargetPriorityTrait((TargetPriorityTrait)empty, 2);
	public readonly static Trait emptyGenerated3 = new GeneratedTargetPriorityTrait((TargetPriorityTrait)empty, 3);
	public readonly static Trait bottomRightEnemySideEmptyTargetingTrait = new EmptyTargetSpecificPriorityTrait(new GridCoords(CombatGrid.enemyRowLowerBounds, CombatGrid.colRightBounds));
	public readonly static Trait territorial = new TerritorialTargetPriorityTrait();
	public readonly static Trait predatory = new PredatoryTargetPriorityTrait();
	public readonly static Trait closeRanged = new CloseRangedTargetPriorityTrait();
	public readonly static Trait inaccurateBombardment = new BombardmentTargetPriorityTrait(25, 5);
	public readonly static Trait rapidInaccurateBombardment = new BombardmentTargetPriorityTrait(25, 8);
	public readonly static Trait blocker = new BlockerTrait();
	public readonly static Trait buffer = new BufferTargetPriorityTrait(specificHexadecupleBoxEnemySide);
	public readonly static Trait healer = new BufferTargetPriorityTrait(new LowestHealthEnemySideTargetPriorityTrait());
	public readonly static Trait singleTargetBuffer = new BufferTargetPriorityTrait(new RandomEnemyTargetPriorityTrait());
	public readonly static Trait saintly = new CatalystTargetPriorityTrait(specificHexadecupleBoxEnemySide, new ChaoticTargetPriorityTrait());

	public readonly static Trait spawner = new Trait("Spawner", interactionTypeTraitType, "This creature creates minions.", "Egg", Color.green, isMandatoryTrait);
	public readonly static Trait fodder = new Trait("Fodder", interactionTypeTraitType, "This creature dies after it attacks", "Fodder", Color.red, isMandatoryTrait);

	public readonly static Trait charged = new Trait("Charged", chargeTraitType, "This creature is capable of delivering a devastating attack.", "Charged", Color.blue);
	public readonly static Trait shielded = new ShieldTrait("Shielded", chargeTraitType, "This creature is shielded and takes half damage.", "Shielded", oneRoundDuration, Color.blue, shieldedDamageReduction);
	public readonly static Trait extraShielded = new ShieldTrait("Shielded", chargeTraitType, "This creature is shielded and takes half damage.", "Shielded", oneRoundDuration, Color.blue, extraShieldedDamageReduction);
	public readonly static Trait signaling = new Trait("Signaling", chargeTraitType, "This guard is going to call signal an arrow tower to fire upon their assailants.", "Signaling", oneRoundDuration, Color.blue);
	public readonly static Trait observing = new Trait("Observing", chargeTraitType, "The creature is observing it's troops and developing a strategy.", "Observing", oneRoundDuration, Color.blue);


	//on Death Effects
	public readonly static Trait wormSplits = new SummonOnDeathTrait(AbilityList.splitSpawnWormsKey, (GeneratedTargetPriorityTrait)emptyGenerated2);
	public readonly static Trait wormBossSplits = new SummonOnDeathTrait(AbilityList.splitBossSpawnWormsKey, (EmptyTargetSpecificPriorityTrait)bottomRightEnemySideEmptyTargetingTrait, preventsResurrection);
	public readonly static Trait wormExplodes = new SelfTargetAOEOnDeathTrait("Volatile", "On Death", "When this creature is killed, it damages all creates near it.", "Volatile", AbilityList.wormExplosionKey);
	public readonly static Trait wormBossExplodes = new SelfTargetAOEOnDeathTrait("Volatile", "On Death", "When this creature is killed, it damages all creates near it.", "Volatile", AbilityList.wormBossExplosionKey);
	public readonly static Trait wormRevive = new SelfTargetAOEOnDeathTrait("Restorative", "On Death", "When this creature is killed, it brings back all downed creatures near it.", "Restorative", AbilityList.wormRestorativeKey, preventsResurrection);
	public readonly static Trait wormBossRevive = new SelfTargetAOEOnDeathTrait("Restorative", "On Death", "When this creature is killed, it brings back all downed creatures near it.", "Restorative", AbilityList.wormBossRestorativeKey, preventsResurrection);
	public readonly static Trait wormBossFumesOnDeath = new OnDeathEffectTrait("Miasmic", "On Death", "When this creature is killed, it releases a toxic gas as a final retribution against it's enemies.", "DeathFumes", Color.black, AbilityList.wormOnDeathFumesKey, specificCheckeredLeftAlliedSide);


	public readonly static Trait mobLinked = new Trait("Weakly Linked", passiveTraitType, "This creature takes a percentage of it's total health as damage when a minion dies.", "Chain", Color.blue, isMandatoryTrait);
	public readonly static Trait bossLinked = new Trait("Power Linked", passiveTraitType, "This creature takes a percentage of it's total health as damage when a minion dies.", "Chain", Color.red, isMandatoryTrait);

	//temporary buffs
	public readonly static Trait daringSacrifice = new MandatoryTargetTrait("Daring Sacrifice", protectionTraitType, "Become invulnerable for one turn. All enemy attack patterns must include this creature when possible, even if they normally would not.", "DaringSacrifice", endOfRoundDuration, Color.black, daringSacrificeDamageReduction);
	public readonly static Trait cohesion = new DamageBoostTrait("Cohesion", boostTraitType, "This creature deals " + cohesionExtraDamage + " extra damage whenever it attacks", "Cohesion", twoRoundDuration, Color.black, cohesionExtraDamage);
	public readonly static Trait shoredUp = new ShieldTrait("Shored Up", boostTraitType, "This creature only takes half of any damage dealt to it.", "Shielded", twoRoundDuration, Color.black, shieldedDamageReduction);
	public readonly static Trait exitStrategy = new ShieldTrait("Exit Strategy", protectionTraitType, "This creature and all of it's allies take 60% less damage until one round after the surprise round.", "ExitStrategy", oneRoundDuration, Color.black, exitStrategyDamageReduction); //exception to round duration rule because it's applied at the top of the first round and thus doesn't need to compensate for the first tick down.
	public readonly static LinkTrait chokeholdLinkTrait = new LinkTrait("Chokehold", "This creature deals half of all damage received to whoever it is linked to.", "Chokehold", twoRoundDuration, Color.black, chokeholdDamagePercentage);
	public readonly static Trait rallied = new DamageBoostTrait("Rallied", boostTraitType, "This creature deals " + ralliedExtraDamage + " extra damage whenever it attacks", "Rally", fourRoundDuration, Color.black, ralliedExtraDamage);
	public readonly static Trait chew = new DamageBoostTrait(chewKey, boostTraitType, "This creature deals " + chewExtraDamage + " extra damage whenever it attacks. It also has an extra " + chewExtraCritPercent + "% chance to crit.", "Chew", threeRoundDuration, Color.black, chewExtraDamage, chewExtraCritPercent);


	//temporary debuffs
	public readonly static Trait vulnerable = new VulnerabilityTrait("Vulnerable", woundTraitType, "This creature takes " + vulnerableExtraDamage + " extra damage whenever it is hit", "MakeItBleed", Color.black, vulnerableExtraDamage);
	public readonly static Trait bristled = new VulnerabilityTrait("Bristled", woundTraitType, "This creature takes " + bristledExtraDamage + " extra damage whenever it is hit", "Bristled", fourRoundDuration, Color.black, bristledExtraDamage);
	public readonly static Trait upsideTheHead = new CrowdControlTrait("Upside The Head", woundTraitType, "This creature is stunned, and cannot complete any actions until this trait is removed.", "UpsideTheHead", oneRoundDuration, Color.black);
	public readonly static Trait tripped = new CrowdControlTrait("Trip", woundTraitType, "This creature is stunned, and cannot complete any actions until this trait is removed.", "Trip", endOfRoundDuration, Color.black);
	public readonly static Trait countered = new CrowdControlTrait("Countered", woundTraitType, "This creature is stunned, and cannot complete any actions until this trait is removed.", "Trip", endOfRoundDuration, Color.black);
	public readonly static Trait acidVomit = new VulnerabilityTrait("Acid Vomit", woundTraitType, "This creature takes " + acidVomitExtraDamage + " extra damage whenever it is hit", "Acid Vomit", threeRoundDuration, Color.black, acidVomitExtraDamage);
	private readonly static Trait roastedBaseTrait = new VulnerabilityTrait(roastedKey, woundTraitType, "Roasted to perfection. This creature takes an extra point damage per stack of '" + roastedKey + "'", roastedKey, Color.black, roastedExtraDamage);
	public readonly static Trait roasted = new StackableTrait(oneStackAtStart, oneStackPerApplication, roastedBaseTrait);
	public readonly static Trait crippled = new DamageOnFutureTraitApplicationTrait("Crippled", woundTraitType, "This creature has suffered a crippling blow and takes " + crippledDamageFormula + " whenever a debuff is applied to it.", "Cripple", Color.black, crippledDamageFormula, TriggerType.Debuff);
	public readonly static Trait whiplash = new CrowdControlTrait("Whiplash", woundTraitType, "This creature is stunned until the end of the round", "Lashings", endOfRoundDuration, Color.black);
	public readonly static Trait afraid = new CrowdControlTrait("Afraid", woundTraitType, "This creature is stunned, and cannot complete any actions until this trait is removed.", "Afraid", oneRoundDuration, Color.black);
	public readonly static Trait crushingBlow = new ArmorShredTrait(AbilityList.crushingBlowName, woundTraitType, "Until the end of the turn this creature can only muster half of their normal armor value.", AbilityList.crushingBlowName, endOfRoundDuration, Color.black, crushingBlowArmorShred);
	public readonly static Trait chokehold = new CrowdControlTrait("Chokehold", LinkTrait.linkTraitType, "This creature is stunned and receives half of all damage dealt to whoever stunned it.", "Chokehold", twoRoundDuration, Color.black);
	public readonly static Trait insecure = new VulnerabilityTrait("Insecure", mentalTraitType, "This creature is no longer sure of it's own defenses. This creature takes " + insecureExtraDamage + " extra damage whenever it is hit.", "Victimize", Color.black, insecureExtraDamage);
	public readonly static Trait demoralized = new SlowingTrait("Demoralized", mentalTraitType, "This creature is reluctant to fight. It takes " + demoralizeExtraDamage + " extra damage and always attacks last in the action order.", "Demoralize", fourRoundDuration, Color.black, demoralizeExtraDamage);
	public readonly static Trait choking = new CrowdControlTrait(chokingKey, woundTraitType, "This creature is stunned, and cannot complete any actions until this trait is removed.", "SmokeBomb", oneRoundDuration, Color.black);
    public readonly static Trait caveMadness = new SlowingTrait(caveMadnessKey, mentalTraitType, "The screech of the bat's still rings in this one's ears. The afflicted creature is slowed and takes " + caveMadnessExtraDamage + " extra damage.", caveMadnessKey, twoRoundDuration, Color.black, caveMadnessExtraDamage);
	

	//permanent debuffs
	public readonly static Trait flensed = new DamageOverTimeTrait("Flensed", woundTraitType, "This creature takes 3D + 5 damage at the end of every round for the rest of combat.", "Flense", Color.black, "3D + 5");
	public readonly static Trait isolated = new BreakableCrowdControlTrait("Isolated", mentalTraitType, "This creature has been removed from battle and cannot act until it is dealt damage.", "Isolate", Color.black);

	//ActivatedPassiveBuffs
	public readonly static Trait wearyHeart = new Trait("Weary Heart", activatedPassiveTraitType, "This creature's Armor is increased by 5 and your chance to successfully retreat is increased by 20%.", "WearyHeart", Color.black);
	public readonly static Trait devastatingCriticals = new Trait(devastatingCriticalsKey, activatedPassiveTraitType, "This creature's critical hits deal D% of the victim's health as extra damage normally, and 2D% during a surprise round. Critical hits caused by single target actions can cause a random enemy to receive the '" + afraid.getName() + "' trait.", devastatingCriticalsKey, Color.black);
	public readonly static Trait intimidatingPressence = new MandatoryTargetTrait("Intimidating Pressence", activatedPassiveTraitType, "All enemy attack patterns must include this creature when possible. Useful for preventing enemies from attacking weaker or hurt allies.", "InitmidatingPressence", Color.black);
	private readonly static Trait bloodlustBaseTrait = new DamageBoostTrait("Bloodlust", activatedPassiveTraitType, "The red mist descends, causing the creature to deal " + bloodlustExtraDamage + " more damage per stack. Gain a stack at the start of every turn, and whenever you slay a minion or summoned enemy. Maximum of " + bloodlustMaximumStacks + " stacks.", "Bloodlust", Color.black, bloodlustExtraDamage);
	public readonly static Trait bloodlust = new StackOnKillOrNewTurnTrait(new UnityEvent[] { EnemyStats.OnMinionSummonDeath, CombatStateManager.OnNewTurn }, oneStackAtStart, oneStackPerApplication, bloodlustMaximumStacks, ActionCostType.Bloodlust, bloodlustBaseTrait);
    private readonly static Trait halfHandStanceBaseTrait = new DamageBoostTrait("Half Hand Stance", stanceTraitType, "A balanced stance, increasing damage dealt by " + halfHandStanceExtraDamage + " and decreasing damage taken by " + halfHandStanceExtraDamage + " per stack. Starts with " + halfHandStanceStartingStacks + " stacks. Gain stacks by attacking with fists or staffs. Only one stance can be active at a time.", "Half Hand Stance", Color.black, halfHandStanceExtraDamage);
    private readonly static Trait halfHandStanceBaseTrait2 = new VulnerabilityTrait("Half Hand Stance", stanceTraitType, "", "Bloodlust", Color.black, -1*halfHandStanceExtraDamage);
    public readonly static Trait halfHandStance = new StackableTrait(Stance.OnStanceApplyingWeaponAttack, fourStacksAtStart, oneStackPerApplication, ActionCostType.Stance, new Trait[] { halfHandStanceBaseTrait, halfHandStanceBaseTrait2 });
    private readonly static Trait predationBaseTrait = new DamageBoostTrait("Predation", activatedPassiveTraitType, "Your brutal strikes reinvigorate you. Whenever you deal 100% or more of a Master enemy's health in one hit, you heal for D/2 health and gain 10% Armor Penetration and 4 extra damage per attack. The enemy does not need to be at full health to activate Predation.", "Predation", Color.black, predationExtraDamage);
    public readonly static Trait predation = new StackOnKillTrait(Stats.PredationProc, zeroStacksAtStart, oneStackPerApplication, ActionCostType.Predation, predationBaseTrait);

	//Charisma passive stackable traits
	private readonly static Trait redKnifeBaseTrait = new Trait("Red Knife", chargeTraitType, "The will to harm. " + AbilityList.redKnifeAcquisitionMethodExplanation, "Red Knife", Color.red);
	private readonly static StackableTrait redKnife = new StackableTrait(zeroStacksAtStart, oneStackPerApplication, ActionCostType.RedKnife, redKnifeBaseTrait);

    private readonly static Trait blueShieldBaseTrait = new Trait("Blue Shield", chargeTraitType, "The will to help. " + AbilityList.blueShieldAcquisitionMethodExplanation, "Blue Shield", ColorList.blueShieldTextColor);
    private readonly static StackableTrait blueShield = new StackableTrait(zeroStacksAtStart, oneStackPerApplication, ActionCostType.BlueShield, blueShieldBaseTrait);

    private readonly static Trait yellowThornBaseTrait = new Trait("Yellow Thorn", chargeTraitType, "The will to hinder. " + AbilityList.yellowThornAcquisitionMethodExplanation, "Yellow Thorn", Color.yellow);
    private readonly static StackableTrait yellowThorn = new StackableTrait(zeroStacksAtStart, oneStackPerApplication, ActionCostType.YellowThorn, yellowThornBaseTrait);

    private readonly static Trait greenLeafBaseTrait = new Trait("Green Leaf", chargeTraitType, "The will to heal. " + AbilityList.greenLeafAcquisitionMethodExplanation, "Green Leaf", ColorList.greenLeafTextColor); 
    private readonly static StackableTrait greenLeaf = new StackableTrait(zeroStacksAtStart, oneStackPerApplication, ActionCostType.GreenLeaf, greenLeafBaseTrait);

    private readonly static StackableTrait[] charismaPassiveStackableTraits = new StackableTrait[] { redKnife, blueShield, yellowThorn, greenLeaf };

    private readonly static Trait charismaPassivesBaseTrait = new Trait("Exuberance", chargeTraitType, "The energies that draw others to you, and inspire them to follow your example.", "", Color.black);
    // public readonly static MultiStackTrait charismaPassives = new MultiStackTrait(charismaPassivesBaseTrait, charismaPassiveStackableTraits);

    //Zone Of Influence Traits
    public readonly static Trait stalwartInfluence = new ZoneOfInfluenceTrait("Stalwart Influence", "The companion's influence now protects even more. The amount of armor gained by being in their Zone of Influence is doubled.", "Stalwart Influence", new string[]{"redStalwartInfluence"});
	public readonly static Trait cleverInfluence = new ZoneOfInfluenceTrait("Clever Influence", "The companion's influence provides underhanded solutions to violent problems. The amount of extra damage in a surprise round provided by their Zone of Influence is doubled", "CleverInfluence", new string[]{AllyStats.carterCleverInfluenceStatBoostKey});
	public readonly static Trait persistentInfluence = new ZoneOfInfluenceTrait("Persistent Influence", "The companion's influence is the light that guards the morale of those around them. +20% Mental Resistance to all allies within the companion's Zone of Influence", "PersistentInfluence", new string[]{AllyStats.nandorPersistentInfluenceStatBoostKey});
	
	public readonly static Trait stonewall = new ShieldTrait("Stonewall", protectionTraitType, "This creature will take 75% less damage until the following turn.", "Stonewall", twoRoundDuration, Color.black, stonewallDamageReduction);
	
	public readonly static Trait repositioningInvulnerability = new HiddenShieldTrait("Repositioning Invulnerability", protectionTraitType, "Become invulnerable until you reposition", "Default", Color.blue, 100.0);

    public readonly static Trait untargetable = new HiddenTrait("Untargetable", isUntargetable);
	
    static TraitList()
    {
        initializeTraitList();
    }
    
    [RuntimeInitializeOnLoadMethod]
	private static void initializeTraitList()
    {
        dictionaryOfTraits = new Dictionary<string,Trait>();
        dictionaryOfHiddenTraits = new Dictionary<string, Trait>();
        buffDebuffTraitTypes = new Dictionary<string, bool>();

		dictionaryOfTraits.Add(master.getName(), master);
		dictionaryOfTraits.Add(minion.getName(), minion);
		dictionaryOfTraits.Add(summoned.getName(), summoned);
		
		dictionaryOfTraits.Add(frontLine.getName(), frontLine);
		dictionaryOfTraits.Add(backLine.getName(), backLine);
		
		dictionaryOfTraits.Add(evolutionary.getName(), evolutionary);
		dictionaryOfTraits.Add(immobile.getName(), immobile);
		dictionaryOfTraits.Add(large.getName(), large);
		
		dictionaryOfTraits.Add(chaotic.getName(), chaotic);
		dictionaryOfTraits.Add(clockwiseFourCornersEnemySideKey, clockwiseFourCornersEnemySide);
		dictionaryOfTraits.Add(empty.getName(), empty);
		dictionaryOfTraits.Add(emptyGenerated2.getName() + generatedSuffix + 2, emptyGenerated2);
		dictionaryOfTraits.Add(emptyGenerated3.getName() + generatedSuffix + 3, emptyGenerated3);
		dictionaryOfTraits.Add(territorial.getName(), territorial);
		dictionaryOfTraits.Add(predatory.getName(), predatory);
		dictionaryOfTraits.Add(closeRanged.getName(), closeRanged);
		dictionaryOfTraits.Add(inaccurateBombardmentKey, inaccurateBombardment);
		dictionaryOfTraits.Add(rapidInaccurateBombardmentKey, rapidInaccurateBombardment);
		dictionaryOfTraits.Add(blocker.getName(), blocker);
		dictionaryOfTraits.Add(buffer.getName(), buffer); //of the Support Targeting Priority subtypes, asking for "Support" gets you the buffer trait
		dictionaryOfTraits.Add(bufferKey, buffer);
		dictionaryOfTraits.Add(healerKey, healer);
		dictionaryOfTraits.Add(singleTargetBuffKey, singleTargetBuffer);
		dictionaryOfTraits.Add(saintly.getName(), saintly);
		
		dictionaryOfTraits.Add(charged.getName(), charged);
		dictionaryOfTraits.Add(shielded.getName(), shielded);
		dictionaryOfTraits.Add(extraShieldedKey, extraShielded);
		dictionaryOfTraits.Add(signaling.getName(), signaling);
		dictionaryOfTraits.Add(observing.getName(), observing);
		dictionaryOfTraits.Add(chewKey, chew);
		
		dictionaryOfTraits.Add(spawner.getName(), spawner);
		dictionaryOfTraits.Add(fodder.getName(), fodder);
		
		dictionaryOfTraits.Add(wormSplitsTraitKey, wormSplits);
		dictionaryOfTraits.Add(wormBossSplitsTraitKey, wormBossSplits);
		dictionaryOfTraits.Add(wormExplodesTraitKey, wormExplodes);
		dictionaryOfTraits.Add(wormBossExplodesTraitKey,wormBossExplodes);
		dictionaryOfTraits.Add(wormReviveTraitKey, wormRevive);
		dictionaryOfTraits.Add(wormBossReviveTraitKey, wormBossRevive);
		dictionaryOfTraits.Add(wormBossFumesOnDeath.getName(), wormBossFumesOnDeath);
		
		mobLinked.setLinkedPercentage(.40);
		bossLinked.setLinkedPercentage(.075);
		
		dictionaryOfTraits.Add(chokingKey, choking);

		dictionaryOfTraits.Add(mobLinked.getName(), mobLinked);
		dictionaryOfTraits.Add(bossLinked.getName(), bossLinked);
		
		dictionaryOfTraits.Add(wearyHeart.getName(), wearyHeart);
		dictionaryOfTraits.Add(stonewall.getName(), stonewall);
		dictionaryOfTraits.Add(stalwartInfluence.getName(), stalwartInfluence);
		
		buffDebuffTraitTypes.Add(woundTraitType, isDebuff);
        buffDebuffTraitTypes.Add(mentalTraitType, isDebuff);

        buffDebuffTraitTypes.Add(protectionTraitType, isBuff);
		buffDebuffTraitTypes.Add(boostTraitType, isBuff);
		buffDebuffTraitTypes.Add(chargeTraitType, isBuff);
		
		dictionaryOfHiddenTraits.Add(untargetable.getName(), untargetable);
	}

	public static Trait[] getListOfTraits(string[] traitNames)
	{
		if(traitNames == null)
		{
			return new Trait[0];
		}
		
		Trait[] traits = new Trait[traitNames.Length];
		
		for(int nameIndex = 0; nameIndex < traitNames.Length; nameIndex++)
		{
			traits[nameIndex] = dictionaryOfTraits[traitNames[nameIndex]];
		}
		
		return traits;
	}
	
	public static Trait getTrait(string traitName)
	{
		return dictionaryOfTraits[traitName];
	}

	public static Trait getHiddenTrait(string traitName)
	{
		return dictionaryOfHiddenTraits[traitName];
	}

    /*
        public static int getNextFreeTraitSlot(Trait[] traitList)
        {
            for(int traitIndex = 0; traitIndex < traitList.Length; traitIndex++)
            {
                if(traitList[traitIndex] == null)
                {
                    return traitIndex;
                }
            }

            for(int traitIndex = 0; traitIndex < traitList.Length; traitIndex++)
            {
                if(!traitList[traitIndex].isPermanent())
                {
                    return traitIndex;
                }
            }

            return -1;
        }
    */
}
