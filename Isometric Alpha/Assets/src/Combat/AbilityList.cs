using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityList
{
    #region ActionType Names

    public const string abilityActionTypeName = "Ability";
    public const string attackActionTypeName = "Attack";
    public const string itemActionTypeName = "Item";
    public const string passiveActionTypeName = "Passive";
    public const string equippedPassiveActionTypeName = "Equipped Passive";

    #endregion

	public const int lowestLevelForAbilities = 2;

	public const int minimumNumberOfAbilitiesPerLevel = 1;
    public const int maximumNumberOfAbilitiesPerLevel = 10;

	public const char levelKeyChar 	= 'l';
    public const char strengthKeyChar 	= 's';
	public const char dexterityKeyChar 	= 'd';
	public const char wisdomKeyChar 	= 'w';
	public const char charismaKeyChar 	= 'c';

    public  const int zeroSlotMax  = 0; //for passives
    private const int oneSlotMax   = 1;
	private const int twoSlotMax   = 2;
	private const int threeSlotMax = 3;
	private const int noSlotMax    = 8;
	
	public  const int noCooldown 		 = 1;
	private const int oneRoundCooldown   = 2;
	private const int twoRoundCooldown   = 3; 
	private const int threeRoundCooldown = 4;
	private const int fourRoundCooldown  = 5;
	private const int fiveRoundCooldown  = 6;
	private const int sixRoundCooldown 	 = 7;
	private const int sevenRoundCooldown = 8;
	private const int eightRoundCooldown = 9; 
	private const int nineRoundCooldown  = 10;

    private const int oneStackCastCost = 1;
    private const int twoStackCastCost = 2;
    private const int threeStackCastCost = 3;
    private const int fourStackCastCost = 4;
    private const int fiveStackCastCost = 5;
    private const int sixStackCastCost = 6;

    private const int oneStackBonus = 1;
    private const int twoStackBonus = 2;
    private const int threeStackBonus = 3;
    private const int fourStackBonus = 4;
    private const int fiveStackBonus = 5;
    private const int sixStackBonus = 6;

    private static int strAbilityLevel = 2;
	private static int dexAbilityLevel = 2;
	private static int wisAbilityLevel = 2;
	private static int chaAbilityLevel = 2;
	
	private static int strAbilityIndex = 1;
	private static int dexAbilityIndex = 1;
	private static int wisAbilityIndex = 1;
	private static int chaAbilityIndex = 1;
	
	private const double thirtyPercentPerSquare = .3;
	private const double fiftyPercentPerSquare = .5;
	private const double oneHundredPercentBacklash = 1.0;
	private const bool isSelfTargeting = true;
	private const bool targetsEnemy = true;

    public const string godSpellAbilityKey = "God Spell";
	public const string moveAllyAbilityKey = "Master Move Ally Ability";

    public const string waylayName = "Waylay";
	public const string crippleName = "Cripple";
	public const string flenseName = "Flense";
	public const string fearName = "Fear";

    public const string throatJabName = "Throat Jab";
	public const string doubleStrikeName = "Double Strike";
	public const int doubleStrikeRepetitions = 2;
	public const string improvedStrikesName = "Improved Strikes";
    public const string crushingBlowName = "Crushing Blow";
    public const string battleMeditationName = "Battle Meditation";

    public const string rallyName = "Rally";
    public const string exuberanceName = "Exuberance";
    public const string unflinchingName = "Unflinching";
	public const string versatileName = "Versatile";

    public const string avertBlameName = "Avert Blame";

    public const string batClawName = "Bat Claw";
	public readonly static DescriptionParams batClawDescription = DescriptionParams.build(batClawName, iconName: "Claw",loreDescription: "The sharp talons of a bat.");
    public const string swarmRushKey = "Swarm Rush";

    public const string harmlessKey = "Harmless";

	public const string punchKey = "Punch";

	public const string swapKey = "Swap";
	public const string gutKey = "Gut";
	public const string rileKey = "Rile";
	public const string growMobKey = "Grow Mob";


	public const string bossBatClawKey = "Boss Bat Claw";
	public const string diveBombKey = "Dive Bomb";
	public const string flurryKey = "Flurry";
	public const string colonyCrushKey = "Colony Crush";
	public const string screechKey = "Screech";
	public const string spawnPupsKey = "Spawn Pups";
	public const string rouseColonyKey = "Rouse Colony";

	public const string wallopKey = "Wallop";	
    public const string slamKey = "Slam";	
	public const string trampleKey = "Trample";
    public const string spawnBroodlingKey = "Spawn Broodling";
	public const string splitSpawnWormsKey = "Split Spawn Worms";
	public const string splitBossSpawnWormsKey = "Split Boss Spawn Worms";
	public const string acidVomitKey = "Acid Vomit";
	public const string wormExplosionKey = "Worm Explosion";
	public const string wormBossExplosionKey = "Worm Boss Explosion";
	public const string wormRestorativeKey = "Worm Restorative";
	public const string wormBossRestorativeKey = "Worm Boss Restorative";
	public const string wormAcidBarrageKey = "Acid Spit";
	public const string bossWormFumesKey = "Acid Barrage";

    public const string slashKey = "Slash";
    public const string bladeBlitzKey = "Blade Blitz";
	public const string guardSpearKey = "Spear Thrust";
	public const string guardJavelinKey = "Javelin Throw";
	public const string guardAxeKey = "Axe Swing";
	public const string guardLashKey = "Lash";
	public const string taborsWhipKey = "Tabor's Whip";
	public const string guardArrowBarrageKey = "Arrow Barrage";
	public const string guardCoordinateKey = "Coordinate";
	public const string guardSlingAttackKey = "Bullet";
	public const string guardSlaveSummonKey = "Call Slaves";
	public const string guardWarriorSummonKey = "More Fuel";
	
	public const string eviscerateKey = "Eviscerate";
	public const string skullBashKey = "Skull Bash";
	public const string squadStrikeKey = "Squad Strike";
	public const string skewerKey = "Skewer";
	public const string executeKey = "Execute";
	public const string turnUpTheHeatKey = "Turn Up The Heat";
	public const string shoreUpKey = "Shore Up";
	public const string shatterKey = "Shatter";

	public const string frontHandKey = "Front Hand";
	public const string backHandKey = "Back Hand";
	public const string takeHostageKey = "Take Hostage";


	public const string brandedVolleyKey = "Branded Volley";

	public const string chargeKey = "Charge";
	public const string stompKey = "Stomp";
	public const string feedKey = "Feed";

	public readonly static DescriptionParams boulderRollDescription = DescriptionParams.build(boulderRollKey, useDescription: "A massive rock tumbling quickly towards you.", iconName: "BoulderRoll");
    public const string evolveKey = "Evolve";
	public const string boulderRollKey = "Boulder Roll";
	public const string lesserBoulderRollKey = "Lesser Boulder Roll";
	public const string stoneSaintMaterialsSummonKey = "Scavenge Stones";

	public const string summonAxemanPuppetsKey = "Summon Axeman Puppets";
	public const string summonSpearmanPuppetsKey = "Summon Spearman Puppets";
	public const string summonDisciplinarianPuppetsKey = "Summon Disciplinarian Puppets";
	public const string summonJavelineerPuppetsKey = "Summon Javelineer Puppets";
	
	public const string summonsWhipAttackKey = "Whip Attack";

	public const string redKnifeAcquisitionMethodExplanation = "You will collect the Red Knife Exuberance whenever a Party Member attacks with any Weapon.";
	public const string blueShieldAcquisitionMethodExplanation = "You will collect the Blue Shield Exuberance whenever a Party Member applies a beneficial Trait, as well as whenever a Party Member repositions an Ally or an Enemy.";
	public const string yellowThornAcquisitionMethodExplanation = "You will collect the Yellow Thorn Exuberance whenever a Party Member applies a harmful Trait to a target, as well as whenever a Party Member performs a Critical Hit.";
	public const string greenLeafAcquisitionMethodExplanation = "You will collect the Green Leaf Exuberance whenever a Party Member heals or revives another Party Member.";
      

	public const string wormFumesIndicatorName = "AcidPoolIndicator";
	public readonly static GroundEffect wormFumesGroundEffect = new GroundEffect("5", 4, GridCoords.getDefaultCoords(), Resources.Load<GameObject>(wormFumesIndicatorName));
	public readonly static GroundEffect bossWormFumesGroundEffect = new GroundEffect("3", 4, GridCoords.getDefaultCoords(), Resources.Load<GameObject>(wormFumesIndicatorName));
	
    public const string chokeholdKey = "Chokehold";

	public static Dictionary<string,Ability> statAbilityDictionary;
	public static Dictionary<string,Ability> summonAbilityDictionary;
	public static Dictionary<string,Ability> enemyAbilityDictionary;
	public static Dictionary<string,Ability> miscAbilityDictionary;

    public static Dictionary<string, List<CombatAction>> companionAbilityDictionary;

    public static void initialize()
    {
        if(statAbilityDictionary != null &&
            summonAbilityDictionary != null &&
            enemyAbilityDictionary != null &&
            miscAbilityDictionary != null &&
            companionAbilityDictionary != null)
        {
            return;
        }

        SelectorList.init();

        instantiateStatAbilities();
	
		instantiateSummonAbilities();
		
		instantiateEnemyAbilities();
		
		instantiateMiscAbilities();

		instantiateCompanionAbilities();
    }
	
	private static void instantiateEnemyAbilities()
	{
		enemyAbilityDictionary = new Dictionary<string,Ability>();

        //generic abilities
        enemyAbilityDictionary.Add(harmlessKey, new Ability(CombatActionSettings.build(DescriptionParams.build(harmlessKey, useDescription: "This creature takes no actions."), DamageParams.build("0", "0"), TargetParams.build(SelectorList.singleName))));
		enemyAbilityDictionary.Add(punchKey, new Ability(CombatActionSettings.build(DescriptionParams.build(punchKey, iconName: "FistIcon", loreDescription: "A punch. Not much to it."), DamageParams.build("3", "2"), TargetParams.build(SelectorList.singleName))));
		
        //branded abilities
        enemyAbilityDictionary.Add(swapKey, new SwapAbility(CombatActionSettings.build(DescriptionParams.build(swapKey, iconName: swapKey, useDescription: "The creature swaps places with it's target. If successful, the creature heals itself."), DamageParams.build("25"), animationParams: AnimationParams.build(EffectAnimationType.SmokeBomb))));
        enemyAbilityDictionary.Add(gutKey, new Ability(CombatActionSettings.build(gutKey, DescriptionParams.build(gutKey, loreDescription: "A knife in the stomach that leaves lasting damage."), DamageParams.build("45", "15"), TargetParams.build(SelectorList.singleName), animationParams: AnimationParams.build(EffectAnimationType.Pierce), appliedTrait: TraitList.wounded)));
        enemyAbilityDictionary.Add(rileKey, new Ability(CombatActionSettings.build(rileKey, DescriptionParams.build(rileKey, loreDescription: "Agitate your allies, increasing their furiosity."), targetParams: TargetParams.build(SelectorList.boxThreeName), appliedTrait: TraitList.riled)));
        enemyAbilityDictionary.Add(brandedVolleyKey, new Ability(CombatActionSettings.build(DescriptionParams.build(brandedVolleyKey, loreDescription: "The thrown projectiles of an angry mob."), DamageParams.build("25", "20"), TargetParams.build(SelectorList.verticalOneName))));
        
        
        string[] threeAngryBranded = new string[]{MonsterNameList.angryBranded, MonsterNameList.angryBranded, MonsterNameList.angryBranded};
        enemyAbilityDictionary.Add(growMobKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(growMobKey, loreDescription: "More branded join the fight.")), threeAngryBranded));
        
		//bat abilities
		enemyAbilityDictionary.Add(flurryKey, new Ability(CombatActionSettings.build(DescriptionParams.build(flurryKey, loreDescription: "A devastating surge of claws and jaws."), DamageParams.build("18", "12"), TargetParams.build(SelectorList.boxOneName), animationParams: AnimationParams.build(EffectAnimationType.BatSwarm))));
        enemyAbilityDictionary.Add(screechKey, new Ability(CombatActionSettings.build(DescriptionParams.build(screechKey, iconName: StatSourceNameList.caveMadnessKey, loreDescription: "A howl so loud it draws blood."), DamageParams.build("15", "8"), TargetParams.build(SelectorList.boxOneName), appliedTrait: TraitList.caveMadness)));
        enemyAbilityDictionary.Add(spawnPupsKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(spawnPupsKey, loreDescription: "The bat calls forth its pups to fight for it.")), MonsterNameList.batSwarm));
        enemyAbilityDictionary.Add(swarmRushKey, new Ability(CombatActionSettings.build(DescriptionParams.build(swarmRushKey, iconName: colonyCrushKey, loreDescription: "The swarm flutters about their target, clawing and biting ferociously."), DamageParams.build("8", "2"), animationParams: AnimationParams.build(EffectAnimationType.BatSwarm))));
        enemyAbilityDictionary.Add(batClawName, new Ability(CombatActionSettings.build(batClawName, batClawDescription, DamageParams.build("10", "8"), animationParams: AnimationParams.build(EffectAnimationType.Pierce))));
        enemyAbilityDictionary.Add(bossBatClawKey, new Ability(CombatActionSettings.build(batClawName, batClawDescription, DamageParams.build("12", "12"), animationParams: AnimationParams.build(EffectAnimationType.Pierce))));
        enemyAbilityDictionary.Add(diveBombKey, new SuicideAbility(CombatActionSettings.build(DescriptionParams.build(diveBombKey, iconName: "DiveBomb", loreDescription: "The bat dives straight for an enemy at lightning speed and collides with it, spraying everyone close by with viscera and guano."), DamageParams.build("5", "1"), TargetParams.build(SelectorList.boxOneName))));
        enemyAbilityDictionary.Add(rouseColonyKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(rouseColonyKey, useDescription: "The Cave Matron summons a random grouping of bats to fight for it, and then protects itself. Killing the Matron's children will make it vulnerable again.", loreDescription: "The Matron calls her children to war."), animationParams: AnimationParams.build(EffectAnimationType.Positive)), 
                                                                                                new string[][]
                                                                                                            {
                                                                                                                new string[]{MonsterNameList.armoredBat, MonsterNameList.screecher},
                                                                                                                new string[]{MonsterNameList.armoredBat, MonsterNameList.denMother},
                                                                                                                new string[]{MonsterNameList.giantBat, MonsterNameList.batSwarm},
                                                                                                                new string[]{MonsterNameList.screecher, MonsterNameList.batSwarm},
                                                                                                                new string[]{MonsterNameList.screecher, MonsterNameList.giantBat},
                                                                                                                new string[]{MonsterNameList.giantBat, MonsterNameList.denMother}
                                                                                                            }));
        
        //worm abilities

        enemyAbilityDictionary.Add(wallopKey, new Ability(CombatActionSettings.build(DescriptionParams.build(wallopKey, loreDescription: "The worm drives forward, using the weight of its body and an intense bite to rip apart its foe."), DamageParams.build("17", "5"))));
        enemyAbilityDictionary.Add(trampleKey, new Ability(CombatActionSettings.build(DescriptionParams.build(trampleKey, loreDescription: "The creature crashes into the target, using the size and weight of it's body to crush it's prey."), DamageParams.build("22", "10"), TargetParams.build(SelectorList.verticalThreeName))));
        enemyAbilityDictionary.Add(slamKey, new Ability(CombatActionSettings.build(DescriptionParams.build(slamKey, loreDescription: "The creature crashes into the target, using the size and weight of it's body to crush it's prey."), DamageParams.build("27", "15"), TargetParams.build(SelectorList.boxOneName), animationParams: AnimationParams.build(EffectAnimationType.Blunt), appliedTrait: TraitList.wounded)));
        enemyAbilityDictionary.Add(acidVomitKey, new Ability(CombatActionSettings.build(DescriptionParams.build(acidVomitKey, loreDescription: "The worm spits acidic bile at it's enemy, making them more vulnerable to attacks."), DamageParams.build("14", "0"), animationParams: AnimationParams.build(EffectAnimationType.Acid), appliedTrait: TraitList.acidVomit)));
        enemyAbilityDictionary.Add(wormExplosionKey, new Ability(CombatActionSettings.build(DescriptionParams.build(wormExplosionKey, iconName: "Volatile", loreDescription: "The worm explodes on death, spraying everything around it in burning guts."), DamageParams.build("10", "0"), TargetParams.build(SelectorList.boxTwoName, isSelfTargeting), animationParams: AnimationParams.build(EffectAnimationType.Acid, AnimationParams.useSpecialAttack), appliedTrait: TraitList.acidVomit)));
        enemyAbilityDictionary.Add(wormBossExplosionKey, new Ability(CombatActionSettings.build(DescriptionParams.build(wormBossExplosionKey, iconName: "Volatile", loreDescription: "The worm explodes on death, spraying everything around it in burning guts."), DamageParams.build("35", "0"), TargetParams.build(SelectorList.boxTwoName, isSelfTargeting), animationParams: AnimationParams.build(EffectAnimationType.Acid, AnimationParams.useSpecialAttack), appliedTrait: TraitList.acidVomit)));
        enemyAbilityDictionary.Add(wormRestorativeKey, new ReviveAbility(CombatActionSettings.build(DescriptionParams.build(wormRestorativeKey, iconName: "Restorative", loreDescription: "The worm disolves into many smaller worms on death, which leave it's carcass in search of new corpses to inhabit."), DamageParams.build("50"), TargetParams.build(SelectorList.boxTwoName, isSelfTargeting), animationParams: AnimationParams.build(AnimationParams.useSpecialAttack))));
        enemyAbilityDictionary.Add(wormBossRestorativeKey, new ReviveAbility(CombatActionSettings.build(DescriptionParams.build(wormBossRestorativeKey, iconName: "Restorative", loreDescription: "The worm disolves into many smaller worms on death, which leave it's carcass in search of new corpses to inhabit."), DamageParams.build("100"), TargetParams.build(SelectorList.boxTwoName), animationParams: AnimationParams.build(AnimationParams.useSpecialAttack))));
        enemyAbilityDictionary.Add(wormAcidBarrageKey, new GroundEffectAbility(CombatActionSettings.build(DescriptionParams.build(wormAcidBarrageKey, iconName: "DeathFumes", loreDescription: "The worm belches toxic acid that gnaws at the skin of prey."), animationParams: AnimationParams.build(EffectAnimationType.Acid)), wormFumesGroundEffect));
        enemyAbilityDictionary.Add(bossWormFumesKey, new GroundEffectAbility(CombatActionSettings.build(DescriptionParams.build(bossWormFumesKey, iconName: "DeathFumes", loreDescription: "The worm belches toxic acid that gnaws at the skin of prey."), animationParams: AnimationParams.build(EffectAnimationType.Acid)), bossWormFumesGroundEffect));
        
        string[] pairOfBroodlings = new string[]{MonsterNameList.broodling, MonsterNameList.broodling};
        enemyAbilityDictionary.Add(splitSpawnWormsKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(splitSpawnWormsKey, useDescription: "The worm splits into two smaller worms."), animationParams: AnimationParams.build(AnimationParams.useSpecialAttack)), pairOfBroodlings));
        enemyAbilityDictionary.Add(spawnBroodlingKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(spawnBroodlingKey, useDescription:"The Herald summons broodlings to fight its battles.")), pairOfBroodlings));
        string[] twoArmoredWorms = new string[]{MonsterNameList.armoredWorm, MonsterNameList.armoredWorm};
        enemyAbilityDictionary.Add(splitBossSpawnWormsKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(splitBossSpawnWormsKey, useDescription: "The worm splits to spawn two smaller worms."), targetParams: TargetParams.build(SelectorList.horizontalOneName), animationParams: AnimationParams.build(AnimationParams.useSpecialAttack)), twoArmoredWorms, activatesAfterDeath: true));

        //guard abilities
        enemyAbilityDictionary.Add(slashKey, new Ability(CombatActionSettings.build(DescriptionParams.build(slashKey, loreDescription: "The bite of a sword swung quick."), DamageParams.build("20", "15"))));
        enemyAbilityDictionary.Add(bladeBlitzKey, new Ability(CombatActionSettings.build(DescriptionParams.build(bladeBlitzKey, iconName: executeKey, loreDescription: "Two axe strikes, lightning fast."), DamageParams.build("25", "15"), TargetParams.build(SelectorList.horizontalTwoName))));
        enemyAbilityDictionary.Add(guardSpearKey, new Ability(CombatActionSettings.build(DescriptionParams.build(guardSpearKey, loreDescription: "A piercing blow capable of skewering multiple targets."), DamageParams.build("25", "15"), TargetParams.build(SelectorList.verticalOneName), animationParams: AnimationParams.build(EffectAnimationType.Pierce))));
        enemyAbilityDictionary.Add(guardAxeKey, new Ability(CombatActionSettings.build(DescriptionParams.build(guardAxeKey, loreDescription: "A wide sweep from a sharp axe."), DamageParams.build("23", "10"), TargetParams.build(SelectorList.horizontalTwoName))));
        enemyAbilityDictionary.Add(guardArrowBarrageKey, new Ability(CombatActionSettings.build(DescriptionParams.build(guardArrowBarrageKey, loreDescription: "A hail of deadly missles called from nearby arrow towers."), DamageParams.build("32", "15"))));
        enemyAbilityDictionary.Add(guardJavelinKey, new Ability(CombatActionSettings.build(DescriptionParams.build(guardJavelinKey, loreDescription: "A missile aimed right at your heart."), DamageParams.build("17", "5"), TargetParams.build(SelectorList.verticalOneName))));
        enemyAbilityDictionary.Add(guardLashKey, new Ability(CombatActionSettings.build(DescriptionParams.build(guardLashKey, iconName: "Lashings", loreDescription: "The bane of slaves everywhere."), DamageParams.build("21", "40"), TargetParams.build(SelectorList.verticalThreeName))));
        enemyAbilityDictionary.Add(taborsWhipKey, new Ability(CombatActionSettings.build(DescriptionParams.build(taborsWhipKey, iconName: "Lashings"), DamageParams.build("11", "15"), TargetParams.build(SelectorList.verticalThreeName))));
        enemyAbilityDictionary.Add(guardCoordinateKey, new Ability(CombatActionSettings.build(DescriptionParams.build(guardCoordinateKey, iconName: "Cohesion", loreDescription: "A leader takes charge and directs their troops in battle, increasing their damage."), targetParams: TargetParams.build(SelectorList.boxThreeName), appliedTrait: TraitList.cohesion)));
        enemyAbilityDictionary.Add(guardSlingAttackKey, new Ability(CombatActionSettings.build(DescriptionParams.build(guardSlingAttackKey, loreDescription:"The slinger whips a bullet towards it's target."), DamageParams.build("6", "4"))));
        enemyAbilityDictionary.Add(guardSlaveSummonKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(guardSlaveSummonKey, useDescription: "The slave driver calls forth Branded Conscripts, which force their enemies to attack them instead of their masters.")), MonsterNameList.brandedConscript));
        enemyAbilityDictionary.Add(guardWarriorSummonKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(guardWarriorSummonKey, useDescription: "The slave driver calls forth more slaves to act as fodder.")), new string[]{MonsterNameList.noBrandLoyalist, MonsterNameList.noBrandLoyalist}));

        //Honorguard abilities
        enemyAbilityDictionary.Add(eviscerateKey, new BacklashAbility(CombatActionSettings.build(DescriptionParams.build(eviscerateKey, iconName: "MakeItBleed", useDescription: "This attack hurts the attacker as well as the target.", loreDescription: "A devastating cut capable of unseaming an enemy."), DamageParams.build("30", "5"), TargetParams.build(SelectorList.boxOneName), appliedTrait: TraitList.wounded), oneHundredPercentBacklash));
        enemyAbilityDictionary.Add(skullBashKey, new Ability(CombatActionSettings.build(DescriptionParams.build(skullBashKey, iconName: "Upside the Head", loreDescription: "A blow to the temple that disorients the target."), DamageParams.build("24", "10"), TargetParams.build(SelectorList.verticalOneName), appliedTrait: TraitList.upsideTheHead)));
        enemyAbilityDictionary.Add(squadStrikeKey, new SquadAbility(CombatActionSettings.build(DescriptionParams.build(squadStrikeKey, useDescription: "Deals more damage if the attacker is adjacent to one or more allies."), DamageParams.build("10", "5"), TargetParams.build(SelectorList.boxOneName)), "31"));
        enemyAbilityDictionary.Add(skewerKey, new Ability(CombatActionSettings.build(DescriptionParams.build(skewerKey, loreDescription: "The lancer pierces multiple targets in a row."), DamageParams.build("37", "10"), TargetParams.build(SelectorList.verticalThreeName), animationParams: AnimationParams.build(EffectAnimationType.Pierce))));
        enemyAbilityDictionary.Add(executeKey, new Ability(CombatActionSettings.build(DescriptionParams.build(executeKey, loreDescription: "A decapitating strike."), DamageParams.build("28", "50"), TargetParams.build(SelectorList.horizontalThreeName))));
        enemyAbilityDictionary.Add(turnUpTheHeatKey, new Ability(CombatActionSettings.build(DescriptionParams.build(turnUpTheHeatKey, iconName: "Roasted", loreDescription: "Kende cooks his targets until they're seared on the outside but pink in the middle, making them delectable targets for his allies."), DamageParams.build("12"), TargetParams.build(SelectorList.boxThreeName), appliedTrait: TraitList.roasted)));
        // enemyAbilityDictionary.Add(shoreUpKey, new MissesArePunishedAbility(CombatActionSettings.build(DescriptionParams.build(shoreUpKey, "The Captain shores up the defenses of her subordinates. If she has a target, she will heal and protect them. If she has no target, she will hurt herself instead.", "Shielded"), DamageParams.build("10", "15"), TraitList.shoredUp)));
        enemyAbilityDictionary.Add(shatterKey, new Ability(CombatActionSettings.build(DescriptionParams.build(shatterKey, loreDescription: "A destructive strike with an enormous area."), DamageParams.build("38", "5"), TargetParams.build(SelectorList.boxTwoName), animationParams: AnimationParams.build(EffectAnimationType.Blunt))));
        enemyAbilityDictionary.Add(frontHandKey, new Ability(CombatActionSettings.build(DescriptionParams.build(frontHandKey, iconName: "Lashings", loreDescription: "A torrent of blows that prevents its targets from attacking."), DamageParams.build("35", "10"), TargetParams.build(SelectorList.verticalThreeName), appliedTrait: TraitList.whiplash)));
        enemyAbilityDictionary.Add(backHandKey, new Ability(CombatActionSettings.build(DescriptionParams.build(backHandKey, iconName: "Lashings", loreDescription: "A painful flurry of lashes."), DamageParams.build("25", "50"), TargetParams.build(SelectorList.horizontalThreeName), appliedTrait: TraitList.wounded)));
        enemyAbilityDictionary.Add(takeHostageKey, new TakeHostageAbility(CombatActionSettings.build(DescriptionParams.build(takeHostageKey, iconName: "Lashings", useDescription: "This Ability targets a single Minion Creature and kills it outright. The Attacker then summons a Hostage Minion to their side of the field which forces it's enemies to attack it instead of it's master."), DamageParams.build("60", "50"))));

		//Horse Abilities
		enemyAbilityDictionary.Add(chargeKey, new Ability(CombatActionSettings.build(DescriptionParams.build(chargeKey, loreDescription: "The creature rushes headlong at it's foe, crushing them underfoot."), DamageParams.build("26", "20"), TargetParams.build(SelectorList.verticalThreeName), animationParams: AnimationParams.build(EffectAnimationType.Blunt))));
		enemyAbilityDictionary.Add(stompKey, new Ability(CombatActionSettings.build(DescriptionParams.build(stompKey, loreDescription: "The creature stamps down on it's target, damaging and stunning it."), DamageParams.build("31", "5"), appliedTrait: TraitList.upsideTheHead)));
		enemyAbilityDictionary.Add(feedKey, new HealingAbility(CombatActionSettings.build(DescriptionParams.build(feedKey, useDescription: "The combatant provides sustenance to their allies, healing them."), DamageParams.build("22"))));

		//Saint Abilities
		enemyAbilityDictionary.Add(boulderRollKey, new Ability(CombatActionSettings.build(DescriptionParams.build(boulderRollKey, iconName: "BoulderRoll", loreDescription: "A massive rock tumbling quickly towards you."), DamageParams.build("31", "10"), TargetParams.build(SelectorList.verticalThreeName), animationParams: AnimationParams.build(EffectAnimationType.Blunt))));
        enemyAbilityDictionary.Add(lesserBoulderRollKey, new Ability(CombatActionSettings.build(lesserBoulderRollKey, boulderRollDescription, DamageParams.build("25", "10"), TargetParams.build(SelectorList.verticalThreeName), animationParams: AnimationParams.build(EffectAnimationType.Blunt))));
        enemyAbilityDictionary.Add(evolveKey, new EvolveAbility(CombatActionSettings.build(DescriptionParams.build(evolveKey, useDescription: "Evolves targets into more powerful versions of themselves."), targetParams: TargetParams.build(SelectorList.boxThreeName)), enemyAbilityDictionary[boulderRollKey]));
        enemyAbilityDictionary.Add(stoneSaintMaterialsSummonKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(stoneSaintMaterialsSummonKey, useDescription: "The Saint summons rock Minions which do not attack but can be Evolved into more Stone Saints.")), 
                                                                                    new string[]{MonsterNameList.smallRock, MonsterNameList.smallRock}));

        //Vada Abilities
        string vadaSummonDescription = "The Vada calls forth more puppets.";
        enemyAbilityDictionary.Add(summonAxemanPuppetsKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(summonSpearmanPuppetsKey, useDescription: vadaSummonDescription)), 
                                                                                    new string[]{MonsterNameList.puppetedPrefix + MonsterNameList.axeman, MonsterNameList.puppetedPrefix + MonsterNameList.axeman, 
                                                                                                 MonsterNameList.puppetedPrefix + MonsterNameList.axeman, MonsterNameList.puppetedPrefix + MonsterNameList.axeman}));
        enemyAbilityDictionary.Add(summonSpearmanPuppetsKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(summonSpearmanPuppetsKey, useDescription: vadaSummonDescription)), 
                                                                                    new string[]{MonsterNameList.puppetedPrefix + MonsterNameList.spearman, MonsterNameList.puppetedPrefix + MonsterNameList.spearman, 
                                                                                                 MonsterNameList.puppetedPrefix + MonsterNameList.spearman, MonsterNameList.puppetedPrefix + MonsterNameList.spearman}));
        enemyAbilityDictionary.Add(summonDisciplinarianPuppetsKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(summonSpearmanPuppetsKey, useDescription: vadaSummonDescription)), 
                                                                                    new string[]{MonsterNameList.puppetedPrefix + MonsterNameList.disciplinarian, MonsterNameList.puppetedPrefix + MonsterNameList.disciplinarian, 
                                                                                                 MonsterNameList.puppetedPrefix + MonsterNameList.disciplinarian, MonsterNameList.puppetedPrefix + MonsterNameList.disciplinarian}));
        enemyAbilityDictionary.Add(summonJavelineerPuppetsKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(summonSpearmanPuppetsKey, useDescription: vadaSummonDescription)), 
                                                                                    new string[]{MonsterNameList.puppetedPrefix + MonsterNameList.javelineer, MonsterNameList.puppetedPrefix + MonsterNameList.javelineer, 
                                                                                                 MonsterNameList.puppetedPrefix + MonsterNameList.javelineer, MonsterNameList.puppetedPrefix + MonsterNameList.javelineer}));
	}
	
	private static void instantiateStatAbilities()
	{
		statAbilityDictionary = new Dictionary<string,Ability>();  
		string currentKey;

        //start of Str Abilities
        currentKey = generateAbilityKey(strengthKeyChar);
        statAbilityDictionary.Add(currentKey, new KnockBackAbility(CombatActionSettings.build(currentKey, DescriptionParams.build("Send Flying", iconName: "SendFlying", useDescription: "Deliver a powerful blow which throws the target backwards into whatever is behind them. Extra damage is dealt depending on how far backwards they travel. If they collide with an enemy, the second enemy also takes damage."), DamageParams.build("3S", "D + S"), frequencyParams: FrequencyParams.build(twoSlotMax, fiveRoundCooldown)), thirtyPercentPerSquare));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

		currentKey = generateAbilityKey(strengthKeyChar);
        statAbilityDictionary.Add(currentKey, new EquippedPassive(CombatActionSettings.build(currentKey, appliedTrait: TraitList.intimidatingPressence)));
		statAbilityDictionary[currentKey].setStatRequirements(currentKey);

		currentKey = strengthKeyChar + "-2-3";
        statAbilityDictionary.Add(currentKey, new EquippedPassive(CombatActionSettings.build(currentKey, appliedTrait: TraitList.protectTheWeak)));
		statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(strengthKeyChar);
        statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build("Make It Bleed", iconName: "MakeItBleed", useDescription: "The enemy takes initial damage and every hit the enemy takes for the rest of Combat deals additional damage.", loreDescription: "You impale, bludgeon, or slash your enemy to the point of massive hemorrhaging."), DamageParams.build("3S + D", "D"), TargetParams.build(SelectorList.boxOneName), FrequencyParams.build(twoSlotMax, fiveRoundCooldown), animationParams: AnimationParams.build(EffectAnimationType.Blunt), appliedTrait: TraitList.wounded)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(strengthKeyChar);
        statAbilityDictionary.Add(currentKey, new EquippedPassive(CombatActionSettings.build(currentKey, appliedTrait: TraitList.bloodlust)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(strengthKeyChar);
        statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build("Power Slam", loreDescription: "A potent assault that hits a wide area."), DamageParams.build("5S + 2D", "2S + D"), TargetParams.build(SelectorList.horizontalFourName), FrequencyParams.build(oneSlotMax, sixRoundCooldown), CostParams.build(ActionCostType.Bloodlust, fourStackCastCost), AnimationParams.build(EffectAnimationType.Blunt))));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(strengthKeyChar);
        statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build("Rip Apart", useDescription: "Deals massive damage. If the target survives, they cannot act until the next turn."), DamageParams.build("9S+D+W", "10S"), TargetParams.build(SelectorList.singleName), FrequencyParams.build(oneSlotMax, sevenRoundCooldown), CostParams.build(ActionCostType.Bloodlust, fiveStackCastCost), AnimationParams.build(EffectAnimationType.Slash), TraitList.aliveBarely)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        //start of Dex Abilities
        currentKey = generateAbilityKey(dexterityKeyChar);
        statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build(waylayName, useDescription: "Guaranteed to Crit in the Surprise Round. Waylay has a long Cooldown.", loreDescription: "You strike at the perfect moment, bypassing your opponent's unlevied defenses."), DamageParams.build("5D + S", "2D+5"), TargetParams.build(SelectorList.verticalThreeName), FrequencyParams.build(oneSlotMax, nineRoundCooldown), animationParams: AnimationParams.build(EffectAnimationType.Pierce))));
		statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(dexterityKeyChar);
        statAbilityDictionary.Add(currentKey, new EquippedPassive(CombatActionSettings.build(currentKey, appliedTrait: TraitList.devastatingCriticals, relatedTraits: new Trait[] { TraitList.afraid })));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(dexterityKeyChar);
        statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build(crippleName, loreDescription: "Your attack leaves permanent damage."), DamageParams.build("3D + W", "2D"), frequencyParams: FrequencyParams.build(twoSlotMax, fiveRoundCooldown), appliedTrait: TraitList.crippled)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);
		
		currentKey = generateAbilityKey(dexterityKeyChar);
        statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build(flenseName), DamageParams.build("4D + 2S", "2D"), frequencyParams: FrequencyParams.build(twoSlotMax, fourRoundCooldown), appliedTrait: TraitList.flensed)));
		statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = "d-"+Dexterity.exitStrategyLevel+"-3";
        statAbilityDictionary.Add(currentKey, new PassiveAbility(currentKey, TraitList.exitStrategy));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(dexterityKeyChar);
        statAbilityDictionary.Add(currentKey, new EquippedPassive(CombatActionSettings.build(currentKey, appliedTrait: TraitList.predation)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(dexterityKeyChar);
        statAbilityDictionary.Add(currentKey, new DoubleStrikeAbility(CombatActionSettings.build(currentKey, DescriptionParams.build("Twice Slice", useDescription: "You perform two attacks across a large area."), DamageParams.build("4D + 2S", "3D"), TargetParams.build(SelectorList.horizontalThreeName), FrequencyParams.build(oneSlotMax, fourRoundCooldown), CostParams.build(ActionCostType.Predation, threeStackCastCost)), SelectorList.verticalThreeName));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        //start of Wis Abilities

        currentKey = generateAbilityKey(wisdomKeyChar);
        statAbilityDictionary.Add(currentKey, new Stance(CombatActionSettings.build(currentKey, costParams: CostParams.build(ActionCostType.Stance), appliedTrait: TraitList.halfHandStance)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(wisdomKeyChar);
        statAbilityDictionary.Add(currentKey, new RepositionEnemyAbility(CombatActionSettings.build(currentKey, DescriptionParams.build("Rolling Throw", iconName: "Trip", useDescription: "Throw an Enemy to a location of your choice."), DamageParams.build("W + D", "2W + 2D"), frequencyParams: FrequencyParams.build(twoSlotMax, threeRoundCooldown), costParams: CostParams.build(ActionCostType.Stance, twoStackCastCost), animationParams: AnimationParams.build(EffectAnimationType.Blunt), appliedTrait: TraitList.tripped)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(wisdomKeyChar);
        statAbilityDictionary.Add(currentKey, new InterruptAbility(CombatActionSettings.build(currentKey, DescriptionParams.build(throatJabName, useDescription: "Guaranteed to critically hit if used on an enemy with a '" + TraitType.Charge.ToString() + "' type trait. Removes 1 '" + TraitType.Charge.ToString() + "' type trait from the target. If a Trait is removed in this way, the target has the Countered Trait applied to them.", loreDescription: "A swift jab to the throat that interrupts the enemy's plans."), DamageParams.build("4W + 2S + 2D"), frequencyParams: FrequencyParams.build(oneSlotMax, sixRoundCooldown), animationParams: AnimationParams.build(EffectAnimationType.Blunt)), TraitType.Charge));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(wisdomKeyChar);
        statAbilityDictionary.Add(currentKey, new RepetitionAbility(CombatActionSettings.build(currentKey, DescriptionParams.build(doubleStrikeName, useDescription: "The Caster Damages their opponent twice.", loreDescription: "Two quick taps to the gut, one right after the other."), DamageParams.build("3W"), frequencyParams: FrequencyParams.build(twoSlotMax, fourRoundCooldown), costParams: CostParams.build(ActionCostType.Stance, oneStackCastCost), animationParams: AnimationParams.build(EffectAnimationType.Blunt)), doubleStrikeRepetitions));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = "w-3-3";
        statAbilityDictionary.Add(currentKey, new FistUpgradePassiveAbility(CombatActionSettings.build(currentKey, DescriptionParams.build(improvedStrikesName, iconName: "ImprovedFistIcon", useDescription: "Your Fist weapon is replaced with an improved version which deals more damage, crits more often, and hits a larger area than the previous version."))));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

		currentKey = generateAbilityKey(wisdomKeyChar);
        statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build(crushingBlowName), DamageParams.build("4W + 2S", "W + D"), TargetParams.build(SelectorList.horizontalTwoName), FrequencyParams.build(oneSlotMax, sevenRoundCooldown), CostParams.build(ActionCostType.Stance, fiveStackCastCost), AnimationParams.build(EffectAnimationType.Blunt), TraitList.crushingBlow)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(wisdomKeyChar);
		statAbilityDictionary.Add(currentKey, new StanceReapplicationAbility(CombatActionSettings.build(currentKey, DescriptionParams.build(battleMeditationName, useDescription: "Heal yourself and your allies, and spread your Stance to everyone within range."), DamageParams.build("W"), TargetParams.build(SelectorList.boxTwoName, isSelfTargeting), FrequencyParams.build(oneSlotMax, eightRoundCooldown))));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        //start of Cha Abilities

        currentKey = generateAbilityKey(charismaKeyChar);
        statAbilityDictionary.Add(currentKey, new PassiveAbility(CombatActionSettings.build(currentKey, DescriptionParams.build(exuberanceName, useDescription: "Use the energies accumulated by yourself and your allies to activate abilities in Combat:\n\nRed Knife: "+redKnifeAcquisitionMethodExplanation+"\n\nBlue Shield: "+blueShieldAcquisitionMethodExplanation+"\n\nYellow Thorn: "+yellowThornAcquisitionMethodExplanation+"\n\nGreen Leaf: "+greenLeafAcquisitionMethodExplanation, iconName: IconList.allExuberancesIconName))));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(charismaKeyChar);
        statAbilityDictionary.Add(currentKey, new ExuberanceEquippedPassive(CombatActionSettings.build(currentKey, DescriptionParams.build(unflinchingName, useDescription: "You are fearless in battle, and your companions know it. Gain "+fourStackBonus+" stacks of the Red Knife Exuberance at the start of every Combat.", iconName: "Red Knife")), MultiStackProcType.RedKnife, fourStackBonus));	
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = "c-2-3";
        statAbilityDictionary.Add(currentKey, new HealingAbility(CombatActionSettings.build(currentKey, DescriptionParams.build(rallyName, useDescription: "Encourage an ally to fight on, increasing their damage and healing them."), DamageParams.build("2C"), TargetParams.build(SelectorList.singleName), FrequencyParams.build(twoSlotMax, sixRoundCooldown), CostParams.build(ActionCostType.RedKnife, threeStackCastCost), appliedTrait: TraitList.rallied)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

		currentKey = generateAbilityKey(charismaKeyChar);
        statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build("Victimize", useDescription: "Affected targets will take more damage from allied attacks. Costs 2 Red Knife stack and 1 Blue Shield stack.", loreDescription: "Your words ring out over the din of Combat, alerting your allies to exploitable weaknesses."), targetParams: TargetParams.build(SelectorList.boxOneName), frequencyParams: FrequencyParams.build(oneSlotMax, threeRoundCooldown), costParams: CostParams.build(new ActionCostType[] { ActionCostType.RedKnife , ActionCostType.BlueShield }, new int[] { twoStackCastCost, oneStackCastCost }), appliedTrait: TraitList.insecure)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(charismaKeyChar);
        statAbilityDictionary.Add(currentKey, new RepositionAllyAbility(CombatActionSettings.build(currentKey, DescriptionParams.build("Get Back!", useDescription: "Order a Companion to reposition, healing them in the process."), DamageParams.build("5C"), TargetParams.build(SelectorList.singleName), FrequencyParams.build(oneSlotMax, fourRoundCooldown), CostParams.build(new ActionCostType[] { ActionCostType.RedKnife, ActionCostType.YellowThorn }, new int[] { oneStackCastCost, oneStackCastCost }))));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

		// currentKey = generateAbilityKey(charismaKeyChar);
        // statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build("Isolate", "Direct the flow of battle to sideline a single enemy. This enemy cannot act until it returns to the fray. Taking damage will remove this effect. Only one enemy can be isolated at a time. Costs 1 Red Knife stack and 1 Green Leaf stack."), TargetParams.build(SelectorList.singleName), FrequencyParams.build(oneSlotMax, fourRoundCooldown), CostParams.build(new ActionCostType[] { ActionCostType.RedKnife, ActionCostType.GreenLeaf }, new int[] { oneStackCastCost, oneStackCastCost }), TraitList.isolated)));
        // statAbilityDictionary[currentKey].setStatRequirements(currentKey);

		currentKey = "c-3-3";
        statAbilityDictionary.Add(currentKey, new ExuberanceEquippedPassive(CombatActionSettings.build(currentKey, DescriptionParams.build(versatileName, iconName: "Blue Shield", useDescription: "Your companions have come to rely on you in a variety of situations. Gain "+oneStackBonus+" stack of each Exuberance type at the start of every Combat.")), new MultiStackProcType[] { MultiStackProcType.RedKnife, MultiStackProcType.BlueShield, MultiStackProcType.YellowThorn, MultiStackProcType.GreenLeaf}, new int[]{oneStackCastCost,oneStackCastCost,oneStackCastCost,oneStackCastCost}));	
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

		currentKey = generateAbilityKey(charismaKeyChar);
        statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build("Demoralize", useDescription: "Break the enemy's will to fight. All enemies take " + TraitList.demoralizeExtraDamage + " extra damage and act last in the action order."), targetParams: TargetParams.build(SelectorList.boxThreeName), frequencyParams: FrequencyParams.build(oneSlotMax, sixRoundCooldown), costParams: CostParams.build(new ActionCostType[] { ActionCostType.RedKnife, ActionCostType.YellowThorn }, new int[] { twoStackCastCost, twoStackCastCost }), appliedTrait: TraitList.demoralized)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(charismaKeyChar);
        statAbilityDictionary.Add(currentKey, new RepetitionPerCompanionAbility(CombatActionSettings.build(currentKey, DescriptionParams.build("Barrage", useDescription: "Order all companions to rain missiles on a single foe. Delivers an extra attack per Companion on the battlefield."), DamageParams.build("4C", "2C + D"), TargetParams.build(SelectorList.singleName), FrequencyParams.build(oneSlotMax, fourRoundCooldown), CostParams.build(new ActionCostType[] { ActionCostType.RedKnife, ActionCostType.BlueShield, ActionCostType.YellowThorn, ActionCostType.GreenLeaf }, new int[] { sixStackCastCost, oneStackCastCost, twoStackCastCost, oneStackCastCost }))));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey); 
    }
	
	private static void instantiateCompanionAbilities()
	{
		companionAbilityDictionary = new Dictionary<string,List<CombatAction>>();
		
		List<CombatAction> listOfNandorAbilities = new List<CombatAction>();
        // listOfNandorAbilities.Add(new RepositionEnemyAbility(CombatActionSettings.build(NPCNameList.nandor, DescriptionParams.build("Rolling Throw", "Leverage the enemy's body as a fulcrum and fling them to the ground. The enemy cannot act this turn.", "Trip"), DamageParams.build("4 + 3C", "5 + C"), FrequencyParams.build(oneSlotMax, fourRoundCooldown), TraitList.tripped)));

        // listOfNandorAbilities.Add(new KnockBackAbility(CombatActionSettings.build(NPCNameList.nandor, DescriptionParams.build("Push", "The companion forces an opponent backwards, dealing damage to the opponent and anyone they are pushed into."), DamageParams.build("4 + 3C", "5 + C"), FrequencyParams.build(oneSlotMax, fiveRoundCooldown)), fiftyPercentPerSquare));

        // listOfNandorAbilities.Add(new EquippedPassive(CombatActionSettings.build(NPCNameList.nandor, TraitList.persistentInfluence)));

        // ReviveAbility nandorRevive = new ReviveAbility(CombatActionSettings.build(NPCNameList.nandor+1, DescriptionParams.build("On Your Feet!", "Cutting an inspiring figure, the companion brings some of the formation back from the brink of submission. Every companion in this ability's area that is downed is healed and put back on their feet.", "OnYourFeet"), DamageParams.build("50"), FrequencyParams.build(oneSlotMax, fiveRoundCooldown)));
        // miscAbilityDictionary.Add(nandorRevive.getKey(), nandorRevive);
        // listOfNandorAbilities.Add(nandorRevive);

        Ability standTogether = new Ability(CombatActionSettings.build(StatSourceNameList.standTogetherKey, DescriptionParams.build(StatSourceNameList.standTogetherKey, useDescription: "The caster calls for all of his allies to act as one. Allies in the area deal extra damage for the rest of Combat"), targetParams: TargetParams.build(SelectorList.boxThreeName, targetsOnlyAllies: true), frequencyParams: FrequencyParams.build(oneSlotMax, eightRoundCooldown), appliedTrait: TraitList.standTogether));
        standTogether.setStatRequirements(levelKeyChar + "-3");
        listOfNandorAbilities.Add(standTogether);
        miscAbilityDictionary.Add(standTogether.getKey(), standTogether); // here for loading, if not here then this ability will be replaced with default fist ability on load

		companionAbilityDictionary.Add(NPCNameList.nandor,listOfNandorAbilities);
		
		

		List<CombatAction> listOfThatchAbilities = new List<CombatAction>();
		
		// listOfThatchAbilities.Add(new CompanionAttack(NPCNameList.thatch,"Backhanded Swing","TwoHandedPickReversed",SelectorList.reverseHookOneName, "A swing of Thatch's pick in the opposite direction."));

		// listOfThatchAbilities.Add(new RepositionAllyAbility(CombatActionSettings.build(NPCNameList.thatch, DescriptionParams.build("Step Between", "Thatch shields an ally from harm, giving them time to reposition. Both Thatch and the target will take 75% less damage for two turns.", "Get Back!"), TargetParams.build(SelectorList.singleName), FrequencyParams.build(oneSlotMax, fourRoundCooldown), TraitList.stonewall)));

		//listOfThatchAbilities.Add(new Ability(CombatActionSettings.build(NPCNameList.thatch, DescriptionParams.build("Stonewall", "The caster and every ally within the caster's Zone of Influence take 75% less damage until the next turn. Best used early in the turn order. Has a long cooldown."), TargetParams.build(SelectorList.crossName, isSelfTargeting), FrequencyParams.build(oneSlotMax, sevenRoundCooldown), TraitList.stonewall)));

		// listOfThatchAbilities.Add(new EquippedPassive(CombatActionSettings.build(NPCNameList.thatch, TraitList.stalwartInfluence)));

        // RepositionSelfAbility thatchSacrifice = new RepositionSelfAbility(CombatActionSettings.build(NPCNameList.thatch+1, DescriptionParams.build("Daring Sacrifice", "This creature repositions themselves and becomes invulnerable for one turn. All enemy attack patterns must include this creature when possible, even if they normally would not.", "DaringSacrifice"), FrequencyParams.build(oneSlotMax, sevenRoundCooldown), TraitList.daringSacrifice));
        // miscAbilityDictionary.Add(thatchSacrifice.getKey(), thatchSacrifice);
        // listOfThatchAbilities.Add(thatchSacrifice);
		
        PassiveAbility influentialStrength = new PassiveAbility(CombatActionSettings.build(DescriptionParams.build("Influential Strength", useDescription: "This Companion's Zone of Influence scales with either their Strength or Charisma, whichever is higher.")));
        influentialStrength.setStatRequirements(levelKeyChar + "-1");
        listOfThatchAbilities.Add(influentialStrength);

        EstablishLinkAbility chokehold = new EstablishLinkAbility(CombatActionSettings.build(chokeholdKey, DescriptionParams.build(chokeholdKey, useDescription: "You grapple with the enemy, preventing both yourself and the target from acting. Whenever you take damage while stunned in this way, you only take half of that damage and the target takes the other half."), DamageParams.build("S", "D"), frequencyParams: FrequencyParams.build(oneSlotMax, eightRoundCooldown), appliedTrait: TraitList.chokehold), TraitList.chokeholdLinkTrait);
        chokehold.setStatRequirements(levelKeyChar + "-3");
        listOfThatchAbilities.Add(chokehold);
        miscAbilityDictionary.Add(chokehold.getKey(), chokehold); // here for loading, if not here then this ability will be replaced with default fist ability on load

		companionAbilityDictionary.Add(NPCNameList.thatch,listOfThatchAbilities);
		

		
		List<CombatAction> listOfCarterAbilities = new List<CombatAction>();

        // Ability carterBomb = new Ability(CombatActionSettings.build(NPCNameList.carter+1, DescriptionParams.build("Bristle Bomb", "The caster throws a bomb, damaging the targets and leaving them bristling with needles."), DamageParams.build("2 + 2C", "4 + C"), TargetParams.build(SelectorList.boxOneName), FrequencyParams.build(oneSlotMax, fourRoundCooldown), TraitList.bristled));
        // miscAbilityDictionary.Add(carterBomb.getKey(), carterBomb);
        // listOfCarterAbilities.Add(carterBomb);

        // listOfCarterAbilities.Add(new Ability(CombatActionSettings.build(NPCNameList.carter, DescriptionParams.build("Upside The Head", "The caster gives the target a good thwacking, taking it out of commision for three rounds. Only usable in the suprise round."), DamageParams.build("2 + 2C", "4 + C"), FrequencyParams.build(oneSlotMax, noCooldown, !FrequencyParams.usableOutsideSurpriseRound), TraitList.upsideTheHead)));

        // listOfCarterAbilities.Add(new EquippedPassive(CombatActionSettings.build(NPCNameList.carter, TraitList.cleverInfluence))); 

        TraitBasedDamageAbility bouncingBlade = new TraitBasedDamageAbility(CombatActionSettings.build("Bouncing Blade", DescriptionParams.build("Bouncing Blade", useDescription: "The caster throws their blade, striking multiple targets in a line and dealing extra damage per additional trait applied to the target.", iconName: "BouncingBlade"), DamageParams.build("6D+W+C", "4D"), TargetParams.build(SelectorList.verticalThreeName), FrequencyParams.build(oneSlotMax, fiveRoundCooldown)), 0.25);
        bouncingBlade.setStatRequirements(levelKeyChar + "-3");
        listOfCarterAbilities.Add(bouncingBlade);
        miscAbilityDictionary.Add(bouncingBlade.getKey(), bouncingBlade);

		companionAbilityDictionary.Add(NPCNameList.carter, listOfCarterAbilities);
		

		List<CombatAction> listOfWeftAbilities = new List<CombatAction>();

        HealingAbility avertBlame = new HealingAbility(CombatActionSettings.build(avertBlameName, DescriptionParams.build(avertBlameName, useDescription: "The caster uses an ally as a shield, healing the ally and causing all Chaotic enemies to consider that ally a Mandatory Target."), DamageParams.build("C", "0"), TargetParams.build(SelectorList.singleName), FrequencyParams.build(oneSlotMax, fiveRoundCooldown)));
        avertBlame.setStatRequirements(levelKeyChar + "-3");
        listOfWeftAbilities.Add(avertBlame);
        miscAbilityDictionary.Add(avertBlame.getKey(), avertBlame);

		companionAbilityDictionary.Add(NPCNameList.weft, listOfWeftAbilities);


		List<CombatAction> listOfGasparAbilities = new List<CombatAction>();

        EstablishLinkAbility collectivePunishment = new EstablishLinkAbility(CombatActionSettings.build(StatSourceNameList.collectivePunishmentKey, DescriptionParams.build(StatSourceNameList.collectivePunishmentKey, useDescription: "The caster motivates his allies through shared pain, causing half of damage dealt to the caster to be dealt to an ally instead. Both the caster and the ally receive a boost to the damage they deal."), frequencyParams: FrequencyParams.build(oneSlotMax, eightRoundCooldown), appliedTrait: TraitList.collectivePunishment), TraitList.collectivePunishmentLinkTrait);
        collectivePunishment.setStatRequirements(levelKeyChar + "-3");
        listOfGasparAbilities.Add(collectivePunishment);
        miscAbilityDictionary.Add(collectivePunishment.getKey(), collectivePunishment);

		companionAbilityDictionary.Add(NPCNameList.gaspar, listOfGasparAbilities);
	}
	
	private static void instantiateSummonAbilities()
	{
		summonAbilityDictionary = new Dictionary<string,Ability>();

        summonAbilityDictionary.Add(summonsWhipAttackKey, new Ability(CombatActionSettings.build(summonsWhipAttackKey, DescriptionParams.build("Punishment", useDescription: "A brutal show of whipwork, displayed by someone who has extensive experience with the tool.", iconName: "Lashings"), DamageParams.build("2+6C", "12C"), TargetParams.build(SelectorList.verticalThreeName))));
	}
	
	private static void instantiateMiscAbilities()
	{
		miscAbilityDictionary = new Dictionary<string,Ability>();

        miscAbilityDictionary.Add(godSpellAbilityKey, new Ability(CombatActionSettings.build(godSpellAbilityKey, DescriptionParams.build("God Spell", useDescription: "Kills everything on the enemy side of the board.", iconName: "Explosion"), DamageParams.build("99S + 99D + 99W + 99C + 1000", "100"), TargetParams.build(SelectorList.boxThreeName), animationParams: AnimationParams.build(CombatAnimationType.Effect))));
        miscAbilityDictionary.Add(moveAllyAbilityKey, new RepositionAllyAbility(CombatActionSettings.build(moveAllyAbilityKey, DescriptionParams.build("Move", useDescription: "The character hoofs it to the desired space.", iconName: "HoofIt"), DamageParams.build("99S + 99D + 99W + 99C + 1000", "100"))));
        miscAbilityDictionary.Add(fearName, new Ability(CombatActionSettings.build(DescriptionParams.build(fearName, useDescription: "Puts the fear of the Gods in the target, setting their limbs to trembling and turning their bowels to ice water. This renders them stunned and vulnerable.", iconName: TraitList.afraid.getIconName()), appliedTrait: TraitList.afraid)));
	}
	
	public static List<CombatAction> getCompanionAbilities(string name) 
	{
        List<CombatAction> combatActions = new List<CombatAction>();

        combatActions.Add(new ZoneOfInfluenceDescriptorAbility(name, PartyManager.getPartyMember(name).stats.getZoneOfInfluenceTrait()));

        if(companionAbilityDictionary.ContainsKey(name))
        {
            combatActions.AddRange(companionAbilityDictionary[name]);
        }

		return combatActions;
	}

	private static string generateAbilityKey(char abilityKeyChar)
	{
		string key = "";
		
		switch(abilityKeyChar)
		{
			case strengthKeyChar:
				key = "" + abilityKeyChar + "-" + strAbilityLevel + "-" + strAbilityIndex;
				
				if(strAbilityIndex == 1)
				{
					strAbilityIndex++;
				} else
				{
					strAbilityIndex = 1;
					strAbilityLevel++;
				}
				return key; 
			case dexterityKeyChar:
				key = "" + abilityKeyChar + "-" + dexAbilityLevel + "-" + dexAbilityIndex;
				
				if(dexAbilityIndex == 1)
				{
					dexAbilityIndex++;
				} else
				{
					dexAbilityIndex = 1;
					dexAbilityLevel++;
				}
				
				return key; 
			case wisdomKeyChar:
				key = "" + abilityKeyChar + "-" + wisAbilityLevel + "-" + wisAbilityIndex;
				
				if(wisAbilityIndex == 1)
				{
					wisAbilityIndex++;
				} else
				{
					wisAbilityIndex = 1;
					wisAbilityLevel++;
				}
				
				return key; 
			case charismaKeyChar:
				key = "" + abilityKeyChar + "-" + chaAbilityLevel + "-" + chaAbilityIndex;
				
				if(chaAbilityIndex == 1)
				{
					chaAbilityIndex++;
				} else
				{
					chaAbilityIndex = 1;
					chaAbilityLevel++;
				}
				
				return key; 
			default:
				throw new IOException("Unknown abilityKeyChar: " + abilityKeyChar);
		}
	}
	
	public static CombatAction getAbility(Stats statSource, string key)
    {
        initialize();

		if(statAbilityDictionary.ContainsKey(key))
		{
            return statAbilityDictionary[key].clone(statSource);
		} else if(enemyAbilityDictionary.ContainsKey(key))
		{
            return enemyAbilityDictionary[key].clone(statSource);
		}else if(summonAbilityDictionary.ContainsKey(key))
		{
            return summonAbilityDictionary[key].clone(statSource);
		}else if(miscAbilityDictionary.ContainsKey(key))
		{
            return miscAbilityDictionary[key].clone(statSource);
		}
		
		if(key.Contains(ItemList.fistKey))
		{
			return new FistAttack(statSource);
		}
		
		return new FistAttack(statSource);
	}
	
	public static List<CombatAction> getAllStrengthAbilities()
	{
		return getAllAvailableAbilitiesOfStat(strengthKeyChar, AllyStats.statMaximum);
	}
	
	public static List<CombatAction> getAllDexterityAbilities()
	{
		return getAllAvailableAbilitiesOfStat(dexterityKeyChar, AllyStats.statMaximum);
	}
	
	public static List<CombatAction> getAllWisdomAbilities()
	{
		return getAllAvailableAbilitiesOfStat(wisdomKeyChar, AllyStats.statMaximum);
	}
	
	public static List<CombatAction> getAllCharismaAbilities()
	{
		return getAllAvailableAbilitiesOfStat(charismaKeyChar, AllyStats.statMaximum);
	}

    public static List<CombatAction> getAllAvailableAbilitiesOfStat(char keyChar, int highestLevel)
    {
        return getAllAvailableAbilitiesOfStat(keyChar, lowestLevelForAbilities, highestLevel);
    }

    public static char getPrimaryStatCharacter(PrimaryStat stat)
    {
        switch(stat)
        {
            case PrimaryStat.Strength:
                return strengthKeyChar;
            case PrimaryStat.Dexterity:
                return dexterityKeyChar;
            case PrimaryStat.Wisdom:
                return wisdomKeyChar;
            case PrimaryStat.Charisma:
                return charismaKeyChar;
            default:
                return levelKeyChar;
        }
    }

    public static List<CombatAction> getAllAvailableAbilitiesOfStat(PrimaryStat type, int lowestLevel, int highestLevel)
	{
        switch (type)
		{
			case PrimaryStat.Strength:
				return getAllAvailableAbilitiesOfStat(strengthKeyChar, lowestLevel, highestLevel);
            case PrimaryStat.Dexterity:
                return getAllAvailableAbilitiesOfStat(dexterityKeyChar, lowestLevel, highestLevel);
            case PrimaryStat.Wisdom:
                return getAllAvailableAbilitiesOfStat(wisdomKeyChar, lowestLevel, highestLevel);
            case PrimaryStat.Charisma:
                return getAllAvailableAbilitiesOfStat(charismaKeyChar, lowestLevel, highestLevel);
            default:
				throw new IOException("Unknown PrimaryStat: " + type.ToString());
		}
    }

    private static List<CombatAction> getAllAvailableAbilitiesOfStat(char keyChar, int lowestLevel, int highestLevel)
	{
		List<CombatAction> availableAbilities = new List<CombatAction>();
		
		for(int currentLevel = lowestLevel; currentLevel <= highestLevel; currentLevel++)
		{
			for(int currentAbilityIndex = minimumNumberOfAbilitiesPerLevel; currentAbilityIndex < maximumNumberOfAbilitiesPerLevel; currentAbilityIndex++)
			{
				Ability currentAbility = null;

                statAbilityDictionary.TryGetValue(keyChar + "-" + currentLevel + "-" + currentAbilityIndex, out currentAbility);

				if(currentAbility != null)
				{
                    availableAbilities.Add(currentAbility);
                } else
				{
					break;
				}
            }
		}
		
		return availableAbilities;
	}
}
