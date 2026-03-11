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
    Wound = 14
}

public static class TraitList
{
	public readonly static GridCoords[] fourCornersEnemySide = new GridCoords[]
                                                                                { 
                                                                                    new GridCoords(3,0),
                                                                                    new GridCoords(0,0),
                                                                                    new GridCoords(0,3),
                                                                                    new GridCoords(3,3)
                                                                                };

	public const bool isBuff = true;
	public const bool isDebuff = false;

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
	public readonly static Trait master = new Trait(StatSourceNameList.masterKey, TraitType.FoeType, "A creature that leads other creatures. All Master creatures must be dead to win.", "Crown"); 
	public readonly static Trait minion = new Trait(StatSourceNameList.minionKey, TraitType.FoeType, "A creature that takes orders from a Master. Most die in one hit.", "Collar");
	public readonly static Trait summoned = new Trait(StatSourceNameList.summonedKey, TraitType.FoeType, "A creature that is here at the behest of another, but cannot be controlled directly.", "Summoned");

	public readonly static Trait frontLine = new PositioningTrait(StatSourceNameList.frontLineKey, TraitType.Positioning, "This creature always spawns at the front of the enemy field.", "Front Line", PositioningType.Frontline);
	public readonly static Trait backLine = new PositioningTrait(StatSourceNameList.backLineKey, TraitType.Positioning, "This creature always spawns at the back of the enemy field.", "Back Line", PositioningType.Backline);

	public readonly static Trait evolutionary = new Trait(StatSourceNameList.evolutionaryKey, TraitType.Interaction, "Can be evolved into a better version of itself.", "Evolve");
	public readonly static Trait immobile = new Trait(StatSourceNameList.immobileKey, TraitType.Interaction, "Takes no actions. Cannot be moved.", "Immobile", preventsMovementTrait, isPacifist);
	public readonly static Trait large = new Trait(StatSourceNameList.largeKey, TraitType.Size, "And in charge. This creature takes up multiple spaces, and will take damage each time one of its spaces is hit by the same attack. Cannot be moved.", "Large", preventsMovementTrait);

	//all specific target priorities
	public readonly static SpecificTargetPriorityTrait specificCheckeredLeftAlliedSide = new SpecificTargetPriorityTrait("(6,2)", "SpecificTargetPriorityTrait", "", new GridCoords(6, 2));
	public readonly static SpecificTargetPriorityTrait specificHexadecupleBoxEnemySide = new SpecificTargetPriorityTrait("(2,2)", "SpecificTargetPriorityTrait", "", new GridCoords(2, 2));

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

	public readonly static Trait spawner = new Trait(StatSourceNameList.spawnerKey, TraitType.Interaction, "This creature creates minions.", "Egg");
	public readonly static Trait fodder = new Trait(StatSourceNameList.fodderKey, TraitType.Interaction, "This creature dies after it attacks", "Fodder");

	public readonly static Trait charged = new Trait(StatSourceNameList.chargedKey, TraitType.Charge, "This creature is capable of delivering a devastating attack.", "Charged", roundsLeft: Constants.oneRoundDuration, permanent: false);
	public readonly static Trait shielded = new ShieldTrait(StatSourceNameList.shieldedKey, TraitType.Charge, "This creature takes reduced damage.", StatSourceNameList.shieldedKey, roundsLeft: Constants.oneRoundDuration, permanent: false);
    public readonly static Trait extraShielded = new ShieldTrait(StatSourceNameList.protectedKey, TraitType.Charge, "This creature takes a reduced damage. It will lose this trait if it is the last enemy alive.", StatSourceNameList.shieldedKey, roundsLeft: Constants.oneRoundDuration, permanent: false);
	public readonly static Trait signaling = new Trait(StatSourceNameList.signalingKey, TraitType.Charge, "This guard is going to call signal an arrow tower to fire upon their assailants.", "Signaling");
	public readonly static Trait coordinated = new Trait(StatSourceNameList.coordinatedKey, TraitType.Charge, "This creature is taking part in a plan directed by another creature.", StatSourceNameList.coordinatedKey, roundsLeft: Constants.twoRoundDuration, permanent: false);
    public readonly static Trait observing = new Trait(StatSourceNameList.observingKey, TraitType.Charge, "The creature is observing it's troops and developing a strategy.", "Observing");


	//on Death Effects
	public readonly static Trait wormSplits = new SummonOnDeathTrait(AbilityList.splitSpawnWormsKey, (GeneratedTargetPriorityTrait)emptyGenerated2);
	public readonly static Trait wormBossSplits = new SummonOnDeathTrait(AbilityList.splitBossSpawnWormsKey, (EmptyTargetSpecificPriorityTrait)bottomRightEnemySideEmptyTargetingTrait, preventsResurrection);
	public readonly static Trait wormExplodes = new SelfTargetAOEOnDeathTrait(StatSourceNameList.volatileKey, "When this creature is killed, it damages all creates near it.", "Volatile", AbilityList.wormExplosionKey);
	public readonly static Trait wormBossExplodes = new SelfTargetAOEOnDeathTrait(StatSourceNameList.volatileKey, "When this creature is killed, it damages all creates near it.", "Volatile", AbilityList.wormBossExplosionKey);
	public readonly static Trait wormRevive = new SelfTargetAOEOnDeathTrait(StatSourceNameList.restorativeKey, "When this creature is killed, it brings back all downed creatures near it.", "Restorative", AbilityList.wormRestorativeKey, preventsResurrection);
	public readonly static Trait wormBossRevive = new SelfTargetAOEOnDeathTrait(StatSourceNameList.restorativeKey, "When this creature is killed, it brings back all downed creatures near it.", "Restorative", AbilityList.wormBossRestorativeKey, preventsResurrection);
	public readonly static Trait wormBossFumesOnDeath = new OnDeathEffectTrait(StatSourceNameList.miasmicKey, "When this creature is killed, it releases a toxic gas as a final retribution against it's enemies.", "DeathFumes", AbilityList.wormOnDeathFumesKey, specificCheckeredLeftAlliedSide);


	public readonly static Trait mobLinked = new Trait(StatSourceNameList.weaklyLinkedKey, TraitType.Passive, "This creature takes a percentage of it's total health as damage when a minion dies.", "Chain");
	public readonly static Trait bossLinked = new Trait(StatSourceNameList.powerLinkedKey, TraitType.Passive, "This creature takes a percentage of it's total health as damage when a minion dies.", "Chain");

	//temporary buffs
	public readonly static Trait daringSacrifice = new MandatoryTargetTrait(StatSourceNameList.daringSacrificeKey, TraitType.Protection, "Become invulnerable for one turn. All enemy attack patterns must include this creature when possible, even if they normally would not.", "DaringSacrifice", Constants.endOfRoundDuration, daringSacrificeDamageReduction);
	public readonly static Trait cohesion = new DamageBoostTrait(StatSourceNameList.cohesionKey, iconName:  "Cohesion", roundsLeft: Constants.twoRoundDuration);
	// public readonly static Trait shoredUp = new ShieldTrait(StatSourceNameList.shoredUpKey, TraitType.Boost, "This creature only takes half of any damage dealt to it.", "Shielded", roundsLeft: Constants.twoRoundDuration, shieldedDamageReduction);
	public readonly static Trait exitStrategy = new Trait(StatSourceNameList.exitStrategyKey, TraitType.Protection, "This creature and all of it's allies take reduced damage until one round after the surprise round.", "ExitStrategy", roundsLeft: Constants.oneRoundDuration, permanent: false); //exception to round duration rule because it's applied at the top of the first round and thus doesn't need to compensate for the first tick down.
	public readonly static LinkTrait chokeholdLinkTrait = new LinkTrait(StatSourceNameList.chokeholdKey, "This creature deals half of all damage received to whoever it is linked to.", "Chokehold", Constants.twoRoundDuration, chokeholdDamagePercentage);
	public readonly static Trait rallied = new DamageBoostTrait(StatSourceNameList.ralliedKey, iconName: "Rally", roundsLeft: Constants.fourRoundDuration, permanent: false);
	public readonly static Trait chewBuzz = new DamageBoostTrait(StatSourceNameList.chewBuzzKey, iconName: StatSourceNameList.chewBuzzKey, roundsLeft: Constants.threeRoundDuration);


	//temporary debuffs
	public readonly static Trait wounded = new Trait(StatSourceNameList.woundedKey, TraitType.Wound, "This creature takes extra damage whenever it is hit", "MakeItBleed");
	public readonly static Trait bristled = new Trait(StatSourceNameList.bristledKey, TraitType.Wound, "This creature takes extra damage whenever it is hit", "Bristled", roundsLeft: Constants.fourRoundDuration);
	public readonly static Trait upsideTheHead = new CrowdControlTrait(StatSourceNameList.upsideTheHeadKey, TraitType.Wound, "This creature is stunned, and cannot complete any actions until this trait is removed.", "UpsideTheHead", roundsLeft: Constants.oneRoundDuration);
	public readonly static Trait tripped = new CrowdControlTrait(StatSourceNameList.tripKey, TraitType.Wound, "This creature is stunned, and cannot complete any actions until this trait is removed.", "Trip", roundsLeft: Constants.endOfRoundDuration);
	public readonly static Trait countered = new CrowdControlTrait(StatSourceNameList.counteredKey, TraitType.Wound, "This creature is stunned, and cannot complete any actions until this trait is removed.", "Trip", roundsLeft: Constants.endOfRoundDuration);
	public readonly static Trait acidVomit = new Trait(StatSourceNameList.acidVomitKey, TraitType.Wound, "This creature takes extra damage whenever it is hit", "Acid Vomit", roundsLeft: Constants.threeRoundDuration);
	public readonly static Trait roasted = new StackableTrait(StatSourceNameList.roastedKey, TraitType.Wound, "Roasted to perfection. This creature takes an extra point of damage per stack", StatSourceNameList.roastedKey, startingStacks: Constants.oneStackAtStart, stacksAppliedPerApplication: Constants.oneStackPerApplication);
	public readonly static Trait crippled = new DamageOnFutureTraitApplicationTrait(StatSourceNameList.crippledKey, TraitType.Wound, "This creature has suffered a crippling blow and takes " + crippledDamageFormula + " whenever a debuff is applied to it.", "Cripple", crippledDamageFormula, TriggerType.Debuff);
	public readonly static Trait whiplash = new CrowdControlTrait(StatSourceNameList.whiplashKey, TraitType.Wound, "This creature is stunned until the end of the round", "Lashings", roundsLeft: Constants.endOfRoundDuration);
	public readonly static Trait afraid = new CrowdControlTrait(StatSourceNameList.afraidKey, TraitType.Wound, "This creature is stunned, and cannot complete any actions until this trait is removed.", "Afraid", roundsLeft: Constants.oneRoundDuration);
	public readonly static Trait crushingBlow = new Trait(AbilityList.crushingBlowName, TraitType.Wound, "Until the end of the turn the damage reduction from this creature's armor is reduced.", AbilityList.crushingBlowName, roundsLeft: Constants.endOfRoundDuration, permanent: true);
	public readonly static Trait chokehold = new CrowdControlTrait(StatSourceNameList.chokeholdKey, TraitType.Interaction, "This creature is stunned and receives half of all damage dealt to whoever stunned it.", "Chokehold", roundsLeft: Constants.twoRoundDuration);
	public readonly static Trait insecure = new Trait(StatSourceNameList.insecureKey, TraitType.Mental, "This creature is no longer sure of it's own defenses. This creature takes extra damage whenever it is hit.", "Victimize");
	public readonly static Trait demoralized = new SlowingTrait(StatSourceNameList.demoralizedKey, TraitType.Mental, "This creature is reluctant to fight. It takes extra damage and always attacks last in the action order.", "Demoralize", Constants.fourRoundDuration);
	public readonly static Trait choking = new CrowdControlTrait(StatSourceNameList.chokingKey, TraitType.Wound, "This creature is stunned, and cannot complete any actions until this trait is removed.", "SmokeBomb", roundsLeft: Constants.oneRoundDuration);
    public readonly static Trait caveMadness = new SlowingTrait(StatSourceNameList.caveMadnessKey, TraitType.Mental, "The ringing won't stop! The afflicted creature always moves last in the action order, and takes " + caveMadnessExtraDamage + " extra damage when struck.", StatSourceNameList.caveMadnessKey, Constants.twoRoundDuration);
	

	//permanent debuffs
	public readonly static Trait flensed = new DamageOverTimeTrait(StatSourceNameList.flensedKey, TraitType.Wound, "This creature takes damage at the end of every round for the rest of combat.", "Flense", "3D + 5");
	public readonly static Trait isolated = new BreakableCrowdControlTrait(StatSourceNameList.isolatedKey, TraitType.Mental, "This creature has been removed from battle and cannot act until it is dealt damage.", "Isolate");

	//EquippedPassiveBuffs
	public readonly static Trait wearyHeart = new Trait(StatSourceNameList.wearyHeartKey, TraitType.EquippedPassive, "This creature's Armor is increased by 5 and your chance to successfully retreat is increased by 20%.", "WearyHeart");
	public readonly static Trait devastatingCriticals = new TraitWithRelatedTraits(StatSourceNameList.devastatingCriticalsKey, TraitType.EquippedPassive, new List<IDescribable>(){ afraid }, "This creature's critical hits deal D% of the victim's health as extra damage normally, and 2D% during a surprise round. Critical hits caused by single target actions can cause a random enemy to receive the '" + afraid.getName() + "' trait.", StatSourceNameList.devastatingCriticalsKey);
	public readonly static Trait intimidatingPressence = new MandatoryTargetTrait(StatSourceNameList.intimidatingPressenceKey, TraitType.EquippedPassive, "All enemy attack patterns must include this creature when possible. Useful for preventing enemies from attacking weaker or hurt allies.", "InitmidatingPressence");
	public readonly static Trait bloodlust = new StackableTrait(StatSourceNameList.bloodlustKey, TraitType.EquippedPassive, "The red mist descends, causing the creature to deal more damage per stack. Gain a stack at the start of every turn, and whenever you slay a minion or summoned enemy. Maximum of " + bloodlustMaximumStacks + " stacks.", StatSourceNameList.bloodlustKey, startingStacks: Constants.oneStackAtStart, stacksAppliedPerApplication: Constants.oneStackPerApplication, costType: ActionCostType.Bloodlust, maximumStacks: bloodlustMaximumStacks, personalReapplicationEvents: new List<UnityEvent>() { EnemyStats.OnMinionSummonDeath }, impersonalReapplicationEvents: new List<UnityEvent>() { CombatStateManager.OnNewTurn });
    public readonly static Trait halfHandStance = new StackableTrait(StatSourceNameList.halfHandStanceKey, TraitType.Stance, "A balanced stance, increasing damage dealt by " + halfHandStanceExtraDamage + " and decreasing damage taken by " + halfHandStanceExtraDamage + " per stack. Starts with " + halfHandStanceStartingStacks + " stacks. Gain stacks by attacking with fists or staffs. Only one stance can be active at a time.", StatSourceNameList.halfHandStanceKey, startingStacks: Constants.fourStacksAtStart, stacksAppliedPerApplication: Constants.oneStackPerApplication, costType: ActionCostType.Stance, personalReapplicationEvents: new List<UnityEvent>() { Stance.OnStanceApplyingWeaponAttack });
    public readonly static Trait predation = new StackableTrait(StatSourceNameList.predationKey, TraitType.EquippedPassive, "Your brutal strikes reinvigorate you. Whenever you deal 100% or more of a Master enemy's health in one hit, you heal for D/2 health and gain 10% Armor Penetration and 4 extra damage per attack. The enemy does not need to be at full health to activate Predation.", StatSourceNameList.predationKey, startingStacks: Constants.zeroStacksAtStart, stacksAppliedPerApplication: Constants.oneStackPerApplication, costType: ActionCostType.Predation, personalReapplicationEvents: new List<UnityEvent>(){ Stats.PredationProc });

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

    private readonly static Trait charismaPassivesBaseTrait = new Trait(StatSourceNameList.exuberanceKey, TraitType.Charge, "The energies that draw others to you, and inspire them to follow your example.", "");
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
		
		dictionaryOfTraits.Add(evolutionary.getName(), evolutionary);
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
