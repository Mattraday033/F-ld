using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public enum TraitType
{
    Boost = 0,
    Charge = 1,
    FoeType = 2,
    EquippedPassive = 3,
    Influence = 4,
    Interaction = 5,
    Mental = 6,
    OnDeath = 7,
    Passive = 8,
    Positioning = 9,
    Protection = 10,
    Size = 11,
    Stance = 12,
    TargetPriority = 13,
    Wound = 14,
    InteractionBuff = 15,
    
    InteractionDebuff = 16
}

public static class TraitList
{
    #region TraitType Names

    public const string boostName = "Boost";
    public const string chargeName = "Charge";
    public const string foeTypeName = "Foe Type";
    public const string equippedPassiveName = "Equipped Passive";
    public const string influenceName = "Influence";
    public const string interactionName = "Interaction";
    public const string mentalName = "Mental";
    public const string onDeathName = "On Death";
    public const string passiveName = "Passive";
    public const string positioningName = "Positioning";
    public const string protectionName = "Protection";
    public const string sizeName = "Size";
    public const string targetPriorityName = "Target Priority";
    public const string woundName = "Wound";
    #endregion

	public readonly static GridCoords[] fourCornersEnemySide = new GridCoords[]
                                                                                { 
                                                                                    new GridCoords(3,0),
                                                                                    new GridCoords(0,0),
                                                                                    new GridCoords(0,3),
                                                                                    new GridCoords(3,3)
                                                                                };

    public static Dictionary<string, Trait> dictionaryOfTraits;
    public static Dictionary<string, Trait> dictionaryOfHiddenTraits;

	private const bool isPacifist = true;
	private const bool preventsMovementTrait = true;
	private const bool preventsResurrection = true;

	private const bool isUntargetable = true;

	private const int halfHandStanceStartingStacks = 4;

	public const int chewHealing = 8;
	public const int caveMadnessExtraDamage = 3;
	public const int demoralizeExtraDamage = 5;
	private const int predationExtraDamage = 4;
	private const int halfHandStanceExtraDamage = 1;
	private const double daringSacrificeDamageReduction = 1.0;
	private const double chokeholdDamagePercentage = .5;

	private const int bloodlustMaximumStacks = 6;

	public const string crippledDamageFormula = "2D + 2W";


	//permanent/mandatory monster traits
	public readonly static Trait master = new Trait(StatSourceNameList.masterKey, TraitType.FoeType, "A Creature that leads other Creatures. All Master Creatures must be defeated to win.", iconName: IconList.masterIcon); 
	public readonly static Trait minion = new Trait(StatSourceNameList.minionKey, TraitType.FoeType, "A Creature that takes orders from a Master. Minions do not need to be defeated to win.", iconName: IconList.minionIcon);
	public readonly static Trait summoned = new Trait(StatSourceNameList.summonedKey, TraitType.FoeType, "A Creature that is here at the behest of another, but cannot be controlled directly.", iconName: "Summoned");

	public readonly static Trait frontLine = new PositioningTrait(StatSourceNameList.frontLineKey, TraitType.Positioning, "This creature always spawns at the front of the enemy field.", "Front Line", PositioningType.Frontline);
	public readonly static Trait backLine = new PositioningTrait(StatSourceNameList.backLineKey, TraitType.Positioning, "This creature always spawns at the back of the enemy field.", "Back Line", PositioningType.Backline);

	public readonly static Trait catalytic = new Trait(StatSourceNameList.catalyticKey, TraitType.Interaction, "Can be evolved into a better version of itself.", iconName: "Evolve");
	public readonly static Trait immobile = new Trait(StatSourceNameList.immobileKey, TraitType.Interaction, "Takes no actions. Cannot be moved.", iconName: "Immobile", preventsMovementTrait, isPacifist);
	public readonly static Trait large = new Trait(StatSourceNameList.largeKey, TraitType.Size, "And in charge. This creature takes up multiple spaces, and will take damage each time one of its spaces is hit by the same attack. Cannot be moved.", iconName: "Large", preventsMovementTrait);
    public readonly static Trait indomitable = new PreventStunTrait(StatSourceNameList.indomitableKey, "This creature is immune to stuns. Traits that stun their targets can still be applied, but will not prevent this creature from acting.", StatSourceNameList.indomitableKey, permanent: true);


	//all specific target priorities
	public readonly static SpecificTargetPriorityTrait specificCheckeredLeftAlliedSide = new SpecificTargetPriorityTrait("(6,2)", "SpecificTargetPriorityTrait", "", new GridCoords(6, 2));
	public readonly static SpecificTargetPriorityTrait specificHexadecupleBoxEnemySide = new SpecificTargetPriorityTrait("(2,2)", "SpecificTargetPriorityTrait", "", new GridCoords(2, 2));
	public readonly static SpecificTargetPriorityTrait specificBoxTwoTwoRightCornerEnemySide = new SpecificTargetPriorityTrait("(1,2)", "SpecificTargetPriorityTrait", "", new GridCoords(1, 2));

	public readonly static Trait chaotic = new ChaoticTargetPriorityTrait();
	public readonly static Trait nonMasterChaotic = new NonMasterChaoticTargetPriorityTrait();
	public readonly static Trait clockwiseFourCornersEnemySide = new ClockwiseTargetPriorityTrait(fourCornersEnemySide);
	public readonly static Trait empty = new EmptyTargetPriorityTrait();
	public readonly static Trait emptyGenerated2 = new GeneratedTargetPriorityTrait((TargetPriorityTrait)empty, 2);
	public readonly static Trait emptyGenerated3 = new GeneratedTargetPriorityTrait((TargetPriorityTrait)empty, 3);
	public readonly static Trait bottomRightMiddleEnemySideEmptyTargetingTrait = new EmptyTargetSpecificPriorityTrait(new GridCoords(Constants.indexTwo, Constants.indexTwo));
	public readonly static Trait territorial = new TerritorialTargetPriorityTrait();
	public readonly static Trait predatory = new PredatoryTargetPriorityTrait();
	public readonly static Trait closeRanged = new CloseRangedTargetPriorityTrait();
	public readonly static Trait inaccurateBombardment = new BombardmentTargetPriorityTrait(25, 5);
	public readonly static Trait rapidInaccurateBombardment = new BombardmentTargetPriorityTrait(25, 8);
	public readonly static Trait blocker = new BlockerTrait();
	public readonly static Trait buffer = new BufferTargetPriorityTrait(specificHexadecupleBoxEnemySide);
	public readonly static Trait healer = new BufferTargetPriorityTrait(new LowestHealthEnemySideTargetPriorityTrait());
	public readonly static Trait singleTargetBuffer = new BufferTargetPriorityTrait(new RandomEnemyBesidesSelfTargetPriorityTrait());
	public readonly static Trait saintly = new CatalystTargetPriorityTrait(specificHexadecupleBoxEnemySide, new ChaoticTargetPriorityTrait());

	public readonly static Trait spawner = new Trait(StatSourceNameList.spawnerKey, TraitType.Interaction, "This creature creates minions.", iconName: "Egg");
	public readonly static Trait fodder = new Trait(StatSourceNameList.fodderKey, TraitType.Interaction, "This creature dies after it attacks", iconName: "Fodder");

    public readonly static HiddenTrait cannotSummon = new HiddenTrait(StatSourceNameList.cannotSummonKey, untargetable: false);

	public readonly static Trait charged = new Trait(StatSourceNameList.chargedKey, TraitType.Charge, "This creature is capable of delivering a devastating attack.", iconName: "Charged", roundsLeft: Constants.oneRoundDuration, permanent: false);
	public readonly static Trait shielded = new ShieldTrait(StatSourceNameList.shieldedKey, TraitType.Charge, "This creature takes reduced damage.", StatSourceNameList.shieldedKey, roundsLeft: Constants.oneRoundDuration, permanent: false);
    public readonly static Trait extraShielded = new CaveMatronShieldTrait(StatSourceNameList.protectedKey, TraitType.Charge, "This creature takes a reduced damage. <B>It will lose this trait if it is the last enemy alive.</B>", StatSourceNameList.protectedKey, roundsLeft: Constants.oneRoundDuration);
	public readonly static Trait signaling = new Trait(StatSourceNameList.signalingKey, TraitType.Charge, "This guard is going to call signal an arrow tower to fire upon their assailants.", iconName: "Signaling");
	public readonly static Trait coordinated = new Trait(StatSourceNameList.coordinatedKey, TraitType.Charge, "This creature is taking part in a plan directed by another creature.", iconName: StatSourceNameList.coordinatedKey, roundsLeft: Constants.twoRoundDuration, permanent: false);
    public readonly static Trait observing = new Trait(StatSourceNameList.observingKey, TraitType.Charge, "The creature is observing it's troops and developing a strategy.", iconName: "Observing"); 


	//on Death Effects
	public readonly static Trait wormSplits = new SummonOnDeathTrait(AbilityList.splitSpawnWormsKey, (GeneratedTargetPriorityTrait)emptyGenerated2);
	public readonly static Trait wormBossSplits = new SummonOnDeathTrait(AbilityList.splitBossSpawnWormsKey, (EmptyTargetSpecificPriorityTrait)bottomRightMiddleEnemySideEmptyTargetingTrait);
	public readonly static Trait wormExplodes = new SelfTargetAOEOnDeathTrait(StatSourceNameList.volatileKey, "When this creature is killed, it damages all creatures near it.", "Volatile", AbilityList.wormExplosionKey);
	public readonly static Trait wormBossExplodes = new SelfTargetAOEOnDeathTrait(StatSourceNameList.volatileKey, "When this creature is killed, it damages all creatures near it.", "Volatile", AbilityList.wormBossExplosionKey);
	public readonly static Trait wormRevive = new SelfTargetAOEOnDeathTrait(StatSourceNameList.restorativeKey, "When this creature is killed, it brings back all downed creatures near it.", "Restorative", AbilityList.wormRestorativeKey);
	public readonly static Trait wormBossRevive = new SelfTargetAOEOnDeathTrait(StatSourceNameList.restorativeKey, "When this creature is killed, it brings back all downed creatures near it.", "Restorative", AbilityList.wormBossRestorativeKey, targetPriority: specificBoxTwoTwoRightCornerEnemySide);
	public readonly static Trait wormBossFumesOnDeath = new OnDeathEffectTrait(StatSourceNameList.miasmicKey, "When this creature is killed, it releases a toxic gas as a final retribution against it's enemies.", "DeathFumes", AbilityList.bossWormFumesKey, specificCheckeredLeftAlliedSide);


	public readonly static Trait mobLinked = new Trait(StatSourceNameList.weaklyLinkedKey, TraitType.Interaction, "This creature takes a percentage of it's total health as damage when a minion dies.", iconName: "Chain");
	public readonly static Trait bossLinked = new Trait(StatSourceNameList.powerLinkedKey, TraitType.Interaction, "This creature takes a percentage of it's total health as damage when a minion dies.", iconName: "Chain");

	//temporary buffs
	public readonly static Trait daringSacrifice = new MandatoryTargetTrait(StatSourceNameList.daringSacrificeKey, TraitType.Protection, "Become invulnerable for one turn. All enemy attack patterns must include this creature when possible, even if they normally would not.", "DaringSacrifice", Constants.endOfRoundDuration, daringSacrificeDamageReduction);
	public readonly static Trait cohesion = new DamageBoostTrait(StatSourceNameList.cohesionKey, iconName:  "Cohesion", roundsLeft: Constants.twoRoundDuration);
	// public readonly static Trait shoredUp = new ShieldTrait(StatSourceNameList.shoredUpKey, TraitType.Boost, "This creature only takes half of any damage dealt to it.", "Shielded", roundsLeft: Constants.twoRoundDuration, shieldedDamageReduction);
	public readonly static Trait exitStrategy = new Trait(StatSourceNameList.exitStrategyKey, TraitType.Protection, "This creature and all of it's allies take reduced damage until one round after the surprise round.", iconName: "ExitStrategy", roundsLeft: Constants.oneRoundDuration, permanent: false); //exception to round duration rule because it's applied at the top of the first round and thus doesn't need to compensate for the first tick down.
	public readonly static LinkTrait chokeholdLinkTrait = new LinkTrait(StatSourceNameList.chokeholdKey, "This creature deals half of all damage received to whoever it is linked to.", "Chokehold", Constants.twoRoundDuration, chokeholdDamagePercentage, stuns: true);
	public readonly static Trait rallied = new DamageBoostTrait(StatSourceNameList.ralliedKey, iconName: "Rally", roundsLeft: Constants.fourRoundDuration, permanent: false);
	public readonly static Trait chewBuzz = new DamageBoostTrait(StatSourceNameList.chewBuzzKey, iconName: StatSourceNameList.chewBuzzKey, roundsLeft: Constants.threeRoundDuration);
    public readonly static Trait standTogether = new Trait(StatSourceNameList.standTogetherKey, TraitType.Boost ,"This Creature attacks in unison with it's allies, dealing extra damage.", iconName: StatSourceNameList.standTogetherKey, permanent: true);


	//temporary debuffs
	public readonly static Trait wounded = new Trait(StatSourceNameList.woundedKey, TraitType.Wound, "This creature takes extra damage whenever it is hit", iconName: "MakeItBleed");
	public readonly static Trait bristled = new Trait(StatSourceNameList.bristledKey, TraitType.Wound, "This creature takes extra damage whenever it is hit", iconName: "Bristled", roundsLeft: Constants.fourRoundDuration);
	public readonly static Trait upsideTheHead = new CrowdControlTrait(StatSourceNameList.upsideTheHeadKey, TraitType.Wound, "This creature is stunned, and cannot complete any actions until this trait is removed.", "UpsideTheHead", roundsLeft: Constants.oneRoundDuration);
	public readonly static Trait tripped = new CrowdControlTrait(StatSourceNameList.tripKey, TraitType.Wound, "This creature is stunned, and cannot complete any actions until this trait is removed.", "Trip", roundsLeft: Constants.endOfRoundDuration);
	public readonly static Trait aliveBarely = new CrowdControlTrait(StatSourceNameList.aliveBarelyKey, TraitType.Wound, "This creature is stunned, and cannot complete any actions until this trait is removed.", "Rip Apart", roundsLeft: Constants.endOfRoundDuration);
	public readonly static Trait countered = new CrowdControlTrait(StatSourceNameList.counteredKey, TraitType.Wound, "This creature is stunned, and cannot complete any actions until this trait is removed.", AbilityList.throatJabName, roundsLeft: Constants.endOfRoundDuration);
	public readonly static Trait acidVomit = new Trait(StatSourceNameList.acidVomitKey, TraitType.Wound, "This creature takes extra damage whenever it is hit", iconName: "Acid Vomit", roundsLeft: Constants.threeRoundDuration);
	public readonly static Trait roasted = new StackableTrait(StatSourceNameList.roastedKey, TraitType.Wound, "This creature takes an extra point of damage per stack", loreDescription: "Roasted to perfection.", iconName: StatSourceNameList.roastedKey, startingStacks: Constants.oneStackAtStart, stacksAppliedPerApplication: Constants.oneStackPerApplication);
	public readonly static Trait riled = new StackableTrait(StatSourceNameList.riledKey, TraitType.Boost, "Provoked and ready to do something about it. This creature deals increased damage.", AbilityList.rileKey, startingStacks: Constants.oneStackAtStart, stacksAppliedPerApplication: Constants.oneStackPerApplication);
    public readonly static Trait crippled = new DamageOnFutureTraitApplicationTrait(StatSourceNameList.crippledKey, TraitType.Wound, "This creature takes " + crippledDamageFormula + " whenever a debuff is applied to it.", "Cripple", crippledDamageFormula, TriggerType.Debuff);
	public readonly static Trait whiplash = new CrowdControlTrait(StatSourceNameList.whiplashKey, TraitType.Wound, "This creature is stunned until the end of the round", "Lashings", roundsLeft: Constants.oneRoundDuration);
	public readonly static Trait afraid = new CrowdControlTrait(StatSourceNameList.afraidKey, TraitType.Wound, "This creature is stunned, and cannot complete any actions until this trait is removed.", "Afraid", roundsLeft: Constants.oneRoundDuration);
	public readonly static Trait crushingBlow = new Trait(AbilityList.crushingBlowName, TraitType.Wound, "The damage reduction offered by this creature's Armor is reduced.", iconName: AbilityList.crushingBlowName, roundsLeft: Constants.oneRoundDuration, permanent: false);
	public readonly static Trait chokehold = new CrowdControlTrait(StatSourceNameList.chokeholdKey, TraitType.InteractionDebuff, "This creature is stunned and receives half of all damage dealt to whoever stunned it.", "Chokehold", roundsLeft: Constants.twoRoundDuration);
	public readonly static Trait insecure = new Trait(StatSourceNameList.insecureKey, TraitType.Mental, "This creature takes extra damage whenever it is hit.", iconName: "Victimize");
	public readonly static Trait demoralized = new SlowingTrait(StatSourceNameList.demoralizedKey, TraitType.Mental, "This creature takes extra damage and always attacks last in the Action Order.", "Demoralize", Constants.fourRoundDuration);
	public readonly static Trait choking = new CrowdControlTrait(StatSourceNameList.chokingKey, TraitType.Wound, "This creature is stunned, and cannot complete any actions until this trait is removed.", "SmokeBomb", roundsLeft: Constants.oneRoundDuration);
    public readonly static Trait caveMadness = new SlowingTrait(StatSourceNameList.caveMadnessKey, TraitType.Mental, "The afflicted creature always moves last in the action order, and takes " + caveMadnessExtraDamage + " extra damage when struck.", StatSourceNameList.caveMadnessKey, Constants.twoRoundDuration, loreDescription: "The ringing won't stop!");
	
    public readonly static Trait collectivePunishment = new CrowdControlTrait(StatSourceNameList.collectivePunishmentKey, TraitType.InteractionBuff, "This creature receives half of all damage dealt to whoever it is linked to.", StatSourceNameList.collectivePunishmentKey, roundsLeft: Constants.sixRoundDuration);
	public readonly static LinkTrait collectivePunishmentLinkTrait = new LinkTrait(StatSourceNameList.collectivePunishmentKey, "This creature deals half of all damage received to whoever it is linked to.", StatSourceNameList.collectivePunishmentKey, Constants.sixRoundDuration, chokeholdDamagePercentage);
	

	//permanent debuffs
	public readonly static Trait flensed = new DamageOverTimeTrait(StatSourceNameList.flensedKey, TraitType.Wound, "This creature takes damage at the end of every round for the rest of combat.", "Flense", "3D + 5");
	public readonly static Trait isolated = new BreakableCrowdControlTrait(StatSourceNameList.isolatedKey, TraitType.Mental, "This creature has been removed from battle and cannot act until it is dealt damage.", "Isolate");

	//EquippedPassiveBuffs
	public readonly static Trait wearyHeart = new Trait(StatSourceNameList.wearyHeartKey, TraitType.EquippedPassive, "This creature's Armor is increased by 5 and your chance to successfully retreat is increased by 20%.", iconName: "WearyHeart");
	public readonly static Trait devastatingCriticals = new TraitWithRelatedTraits(StatSourceNameList.devastatingCriticalsKey, TraitType.EquippedPassive, new List<IDescribable>(){ afraid }, "This creature's critical hits deal D% of the victim's health as extra damage normally, and 2D% during a surprise round. Critical hits caused by single target actions can cause a random enemy to receive the '" + afraid.getName() + "' trait.", StatSourceNameList.devastatingCriticalsKey);
	public readonly static Trait intimidatingPressence = new Trait(StatSourceNameList.intimidatingPressenceKey, TraitType.EquippedPassive, "Attacks by Territorial Enemies must include this creature when possible. Useful for preventing enemies from attacking weaker or hurt allies.", iconName: TerritorialTargetPriorityTrait.initialTraitIconName);
    public readonly static Trait protectTheWeak = new Trait(StatSourceNameList.protectTheWeakKey, TraitType.EquippedPassive, "Attacks by Predatory Enemies must include this creature when possible. Useful for preventing enemies from attacking weaker or hurt allies.", iconName: PredatoryTargetPriorityTrait.initialTraitIconName);
    public readonly static Trait avertBlame = new Trait(AbilityList.avertBlameName, TraitType.EquippedPassive, "Attacks by Chaotic Enemies must include this creature when possible. Useful for preventing enemies from attacking weaker or hurt allies.", iconName: AbilityList.avertBlameName);
    public readonly static Trait bloodlust = new StackableTrait(StatSourceNameList.bloodlustKey, TraitType.EquippedPassive, "The red mist descends, causing the creature to deal more damage per stack. Gain a stack at the start of every turn, and whenever you slay a minion or summoned enemy. Maximum of " + bloodlustMaximumStacks + " stacks.", StatSourceNameList.bloodlustKey, startingStacks: Constants.oneStackAtStart, stacksAppliedPerApplication: Constants.oneStackPerApplication, costType: ActionCostType.Bloodlust, maximumStacks: bloodlustMaximumStacks, personalReapplicationEvents: new List<UnityEvent>() { EnemyStats.OnMinionSummonDeath }, impersonalReapplicationEvents: new List<UnityEvent>() { CombatStateManager.OnNewTurn });
    public readonly static Trait halfHandStance = new StackableTrait(StatSourceNameList.halfHandStanceKey, TraitType.Stance, "A balanced stance, increasing damage dealt by " + halfHandStanceExtraDamage + " and decreasing damage taken by " + halfHandStanceExtraDamage + " per stack. Starts with " + halfHandStanceStartingStacks + " stacks. Gain stacks by attacking with fists or staffs. Only one stance can be active at a time.", StatSourceNameList.halfHandStanceKey, startingStacks: Constants.fourStacksAtStart, stacksAppliedPerApplication: Constants.oneStackPerApplication, costType: ActionCostType.Stance, personalReapplicationEvents: new List<UnityEvent>() { Stance.OnStanceApplyingWeaponAttack });
    public readonly static Trait predation = new StackableTrait(StatSourceNameList.predationKey, TraitType.EquippedPassive, "Your brutal strikes reinvigorate you. Whenever you deal 100% or more of a Master enemy's total health in one hit, you heal for D/2 health and gain 10% Armor Penetration and 4 extra damage per attack. The enemy does not need to be at full health to activate Predation.", StatSourceNameList.predationKey, startingStacks: Constants.zeroStacksAtStart, stacksAppliedPerApplication: Constants.oneStackPerApplication, costType: ActionCostType.Predation, personalReapplicationEvents: new List<UnityEvent>(){ Stats.PredationProc });

	//Charisma passive stackable traits
	// private readonly static Trait redKnifeBaseTrait = new Trait(StatSourceNameList.redKnifeKey, TraitType.Charge, "The will to harm. " + AbilityList.redKnifeAcquisitionMethodExplanation, "Red Knife");
	// private readonly static StackableTrait redKnife = new StackableTrait(Constants.zeroStacksAtStart, Constants.oneStackPerApplication, ActionCostType.RedKnife, redKnifeBaseTrait);

    // private readonly static Trait blueShieldBaseTrait = new Trait(StatSourceNameList.blueShieldKey, TraitType.Charge, "The will to help. " + AbilityList.blueShieldAcquisitionMethodExplanation, "Blue Shield");
    // private readonly static StackableTrait blueShield = new StackableTrait(Constants.zeroStacksAtStart, Constants.oneStackPerApplication, ActionCostType.BlueShield, blueShieldBaseTrait);

    // private readonly static Trait yellowThornBaseTrait = new Trait(StatSourceNameList.yellowThornKey, TraitType.Charge, "The will to hinder. " + AbilityList.yellowThornAcquisitionMethodExplanation, "Yellow Thorn");
    // private readonly static StackableTrait yellowThorn = new StackableTrait(Constants.zeroStacksAtStart, Constants.oneStackPerApplication, ActionCostType.YellowThorn, yellowThornBaseTrait);

    // private readonly static Trait greenLeafBaseTrait = new Trait(StatSourceNameList.greenLeafKey, TraitType.Charge, "The will to heal. " + AbilityList.greenLeafAcquisitionMethodExplanation, "Green Leaf");
    // private readonly static StackableTrait greenLeaf = new StackableTrait(Constants.zeroStacksAtStart, Constants.oneStackPerApplication, ActionCostType.GreenLeaf, greenLeafBaseTrait);

    // private readonly static StackableTrait[] charismaPassiveStackableTraits = new StackableTrait[] { redKnife, blueShield, yellowThorn, greenLeaf };

    private readonly static Trait charismaPassivesBaseTrait = new Trait(StatSourceNameList.exuberanceKey, TraitType.Charge, "The energies that draw others to you, and inspire them to follow your example.", iconName: "");
    // public readonly static MultiStackTrait charismaPassives = new MultiStackTrait(charismaPassivesBaseTrait, charismaPassiveStackableTraits);

	public readonly static Trait stonewall = new ShieldTrait(StatSourceNameList.stonewallKey, TraitType.Protection, "This creature will take 75% less damage until the following turn.", "Stonewall", Constants.twoRoundDuration, false);

	public readonly static Trait repositioningInvulnerability = new HiddenShieldTrait(StatSourceNameList.repositioningInvulnerabilityKey, TraitType.Protection, "Become invulnerable until you reposition", "Default");

    public readonly static Trait untargetable = new HiddenTrait(StatSourceNameList.untargetableKey, isUntargetable);
	
    static TraitList()
    {
        initializeTraitList();
    }
    
    [RuntimeInitializeOnLoadMethod]
	private static void initializeTraitList()
    {
        dictionaryOfTraits = new Dictionary<string,Trait>();
        dictionaryOfHiddenTraits = new Dictionary<string, Trait>();

		dictionaryOfTraits.Add(master.getName(), master);
		dictionaryOfTraits.Add(minion.getName(), minion);
		dictionaryOfTraits.Add(summoned.getName(), summoned);
		
		dictionaryOfTraits.Add(frontLine.getName(), frontLine);
		dictionaryOfTraits.Add(backLine.getName(), backLine);
		
		dictionaryOfTraits.Add(catalytic.getName(), catalytic);
		dictionaryOfTraits.Add(immobile.getName(), immobile);
		dictionaryOfTraits.Add(large.getName(), large);
		
		dictionaryOfTraits.Add(chaotic.getName(), chaotic);
		dictionaryOfTraits.Add(StatSourceNameList.clockwiseFourCornersEnemySideKey, clockwiseFourCornersEnemySide);
		dictionaryOfTraits.Add(empty.getName(), empty);
		dictionaryOfTraits.Add(emptyGenerated2.getName() + StatSourceNameList.generatedSuffix + 2, emptyGenerated2);
		dictionaryOfTraits.Add(emptyGenerated3.getName() + StatSourceNameList.generatedSuffix + 3, emptyGenerated3);
		dictionaryOfTraits.Add(territorial.getName(), territorial);
		dictionaryOfTraits.Add(predatory.getName(), predatory);
		dictionaryOfTraits.Add(closeRanged.getName(), closeRanged);
		dictionaryOfTraits.Add(StatSourceNameList.inaccurateBombardmentKey, inaccurateBombardment);
		dictionaryOfTraits.Add(StatSourceNameList.rapidInaccurateBombardmentKey, rapidInaccurateBombardment);
		dictionaryOfTraits.Add(blocker.getName(), blocker);
		dictionaryOfTraits.Add(buffer.getName(), buffer); //of the Support Targeting Priority subtypes, asking for "Support" gets you the buffer trait
		dictionaryOfTraits.Add(StatSourceNameList.bufferKey, buffer);
		dictionaryOfTraits.Add(StatSourceNameList.healerKey, healer);
		dictionaryOfTraits.Add(StatSourceNameList.singleTargetBuffKey, singleTargetBuffer);
		dictionaryOfTraits.Add(saintly.getName(), saintly);
		
		dictionaryOfTraits.Add(charged.getName(), charged);
		dictionaryOfTraits.Add(shielded.getName(), shielded);
		dictionaryOfTraits.Add(StatSourceNameList.extraShieldedKey, extraShielded);
		dictionaryOfTraits.Add(signaling.getName(), signaling);
		dictionaryOfTraits.Add(observing.getName(), observing);
		dictionaryOfTraits.Add(StatSourceNameList.chewBuzzKey, chewBuzz);
		
		dictionaryOfTraits.Add(spawner.getName(), spawner);
		dictionaryOfTraits.Add(fodder.getName(), fodder);
		
		dictionaryOfTraits.Add(StatSourceNameList.wormSplitsTraitKey, wormSplits);
		dictionaryOfTraits.Add(StatSourceNameList.wormBossSplitsTraitKey, wormBossSplits);
		dictionaryOfTraits.Add(StatSourceNameList.wormExplodesTraitKey, wormExplodes);
		dictionaryOfTraits.Add(StatSourceNameList.wormBossExplodesTraitKey,wormBossExplodes);
		dictionaryOfTraits.Add(StatSourceNameList.wormReviveTraitKey, wormRevive);
		dictionaryOfTraits.Add(StatSourceNameList.wormBossReviveTraitKey, wormBossRevive);
		dictionaryOfTraits.Add(wormBossFumesOnDeath.getName(), wormBossFumesOnDeath);
		
		mobLinked.setLinkedPercentage(.15);
		bossLinked.setLinkedPercentage(.075);
		
		dictionaryOfTraits.Add(StatSourceNameList.chokingKey, choking);

		dictionaryOfTraits.Add(mobLinked.getName(), mobLinked);
		dictionaryOfTraits.Add(bossLinked.getName(), bossLinked);
		
		dictionaryOfTraits.Add(wearyHeart.getName(), wearyHeart);
		dictionaryOfTraits.Add(stonewall.getName(), stonewall);
        
		dictionaryOfTraits.Add(bloodlust.getName(), bloodlust);
		dictionaryOfTraits.Add(predation.getName(), predation);
		
		dictionaryOfHiddenTraits.Add(untargetable.getName(), untargetable);
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
