using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityList
{
	public const int lowestLevelForAbilities = 2;

	public const int minimumNumberOfAbilitiesPerLevel = 1;
    public const int maximumNumberOfAbilitiesPerLevel = 10;

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

    public const string GodSpellAbilityKey = "God Spell";
	public const string moveAllyAbilityKey = "Master Move Ally Ability";

    public const string minorRegenerationName = "Minor Regeneration";

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

    public const string batClawName = "Bat Claw";
	public static DescriptionParams batClawDescription = DescriptionParams.build(batClawName, "The sharp talons of a bat", "Claw");

    public const string harmlessKey = "Harmless";

	public const string punchKey = "Punch";
	public const string swapKey = "Swap";

    public const string weakBatClawKey = "Weak Bat Claw";
	public const string strongBatClawKey = "Strong Bat Claw";
	public const string bossBatClawKey = "Boss Bat Claw";
	public const string diveBombKey = "Dive Bomb";
	public const string flurryKey = "Flurry";
	public const string colonyCrushKey = "Colony Crush";
	public const string spawnPupsKey = "Spawn Pups";

	public const string wallopKey = "Wallop";	
	public const string trampleKey = "Trample";
	public const string splitSpawnWormsKey = "Split Spawn Worms";
	public const string splitBossSpawnWormsKey = "Split Boss Spawn Worms";
	public const string acidVomitKey = "Acid Vomit";
	public const string wormExplosionKey = "Worm Explosion";
	public const string wormBossExplosionKey = "Worm Boss Explosion";
	public const string wormRestorativeKey = "Worm Restorative";
	public const string wormBossRestorativeKey = "Worm Boss Restorative";
	public const string wormFumesKey = "Worm Fumes";
	public const string wormOnDeathFumesKey = "Worm Fumes On Death";

    public const string slashKey = "Slash";
    public const string bladeBlitzKey = "Blade Blitz";
	public const string guardSpearKey = "Spear Thrust";
	public const string guardJavelinKey = "Javalin Throw";
	public const string guardAxeKey = "Axe Swing";
	public const string guardLashKey = "Lash";
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


	public const string chargeKey = "Charge";
	public const string stompKey = "Stomp";
	public const string feedKey = "Feed";

	public static DescriptionParams boulderRollDescription = DescriptionParams.build(boulderRollKey, "A massive rock tumbling quickly towards you.", "BoulderRoll");
    public const string evolveKey = "Evolve";
	public const string boulderRollKey = "Boulder Roll";
	public const string lesserBoulderRollKey = "Lesser Boulder Roll";
	public const string stoneSaintMaterialsSummonKey = "Scavenge Stones";
	
	public const string summonsWhipAttackKey = "Whip Attack";

	public const string redKnifeAcquisitionMethodExplanation = "You will collect this exuberance whenever a Party Member attacks with any weapon.";
	public const string blueShieldAcquisitionMethodExplanation = "You will collect this exuberance whenever a Party Member applies a beneficial trait, as well as whenever a Party Member repositions themselves or an enemy.";
	public const string yellowThornAcquisitionMethodExplanation = "You will collect this exuberance whenever a Party Member applies a harmful trait to a target, as well as whenevera Party Member performs a Critical Hit.";
	public const string greenLeafAcquisitionMethodExplanation = "You will collect this exuberance whenever a Party Member heals or revives one another.";
      

	public const string wormFumesIndicatorName = "WormFumesIndicator";
	public static GroundEffect wormFumesGroundEffect = new GroundEffect("6", 4, GridCoords.getDefaultCoords(), Resources.Load<GameObject>(wormFumesIndicatorName));
	
	public static Dictionary<string,Ability> statAbilityDictionary;
	public static Dictionary<string,Ability> lessonAbilityDictionary;
	public static Dictionary<string,Ability> summonAbilityDictionary;
	public static Dictionary<string,Ability> enemyAbilityDictionary;
	public static Dictionary<string,Ability> miscAbilityDictionary;
	
	public static Dictionary<string,ArrayList> companionAbilityDictionary;
	
	static AbilityList()
	{
		instantiateStatAbilities();
	
		instantiateLessonAbilities();

		instantiateCompanionAbilities();
		
		instantiateSummonAbilities();
		
		instantiateEnemyAbilities();
		
		instantiateMiscAbilities();
	}
	
	private static void instantiateEnemyAbilities()
	{
		enemyAbilityDictionary = new Dictionary<string,Ability>();

        //generic abilities
        enemyAbilityDictionary.Add(harmlessKey, new Ability(CombatActionSettings.build(DescriptionParams.build(harmlessKey, "This creature takes no actions."), DamageParams.build("0", "0"), TargetParams.build(Range.singleTargetIndex))));
		enemyAbilityDictionary.Add(punchKey, new Ability(CombatActionSettings.build(DescriptionParams.build(punchKey, "A punch. Not much to it.", "FistIcon"), DamageParams.build("3", "2"), TargetParams.build(Range.singleTargetIndex))));
		enemyAbilityDictionary.Add(swapKey, new SwapAbility(CombatActionSettings.build(DescriptionParams.build(swapKey, "The creature swaps places with it's target. If successful, the creature heals itself.", swapKey), DamageParams.build("25"))));

		//bat abilities
		enemyAbilityDictionary.Add(flurryKey, new Ability(CombatActionSettings.build(DescriptionParams.build(flurryKey, "A devastating surge of claws and jaws."), DamageParams.build("10", "5"))));
        enemyAbilityDictionary.Add(colonyCrushKey, new Ability(CombatActionSettings.build(DescriptionParams.build(colonyCrushKey, "A torrent of tiny bats is launched at the target."), DamageParams.build("5", "1"), TargetParams.build(Range.quadrupleBoxIndex))));
        enemyAbilityDictionary.Add(spawnPupsKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(spawnPupsKey, "The bat calls forth it's pups to fight for it.")), EnemyStatsList.pupSpawnCombos));
        enemyAbilityDictionary.Add(weakBatClawKey, new Ability(CombatActionSettings.build(weakBatClawKey, batClawDescription, DamageParams.build("3", "2"))));
        enemyAbilityDictionary.Add(strongBatClawKey, new Ability(CombatActionSettings.build(strongBatClawKey, batClawDescription, DamageParams.build("6", "8"))));
        enemyAbilityDictionary.Add(bossBatClawKey, new Ability(CombatActionSettings.build(strongBatClawKey, batClawDescription, DamageParams.build("12", "12"))));
        enemyAbilityDictionary.Add(diveBombKey, new SuicideAbility(CombatActionSettings.build(DescriptionParams.build(diveBombKey, "The bat dives straight for an enemy at lightning speed and collides with it, spraying everyone closeby with viscera and guano.", "DiveBomb"), DamageParams.build("5", "1"), TargetParams.build(Range.quadrupleBoxIndex))));

        //worm abilities
        enemyAbilityDictionary.Add(splitSpawnWormsKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(splitSpawnWormsKey, "The worm splits to spawn two smaller worms.")), EnemyStatsList.wormSplitSpawnCombo));
        enemyAbilityDictionary.Add(wallopKey, new Ability(CombatActionSettings.build(DescriptionParams.build(wallopKey, "The worm drives forward, using the weight of it's body and it's intense bite to rip apart it's foe."), DamageParams.build("2", "1"))));
        enemyAbilityDictionary.Add(trampleKey, new Ability(CombatActionSettings.build(DescriptionParams.build(trampleKey, "The monster crashes into the target, using the size and weight of it's body to crush it's prey."), DamageParams.build("12", "5"))));
        enemyAbilityDictionary.Add(splitBossSpawnWormsKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(splitBossSpawnWormsKey, "The worm splits to spawn four smaller worms."), TargetParams.build(Range.quadrupleBoxIndex)), EnemyStatsList.wormSplitBossSpawnCombo));
        enemyAbilityDictionary.Add(acidVomitKey, new Ability(CombatActionSettings.build(DescriptionParams.build(acidVomitKey, "The worm spits acidic bile at it's enemy, making them more vulnerable to attacks."), DamageParams.build("2", "0"), TraitList.acidVomit)));
        enemyAbilityDictionary.Add(wormExplosionKey, new Ability(CombatActionSettings.build(DescriptionParams.build(wormExplosionKey, "The worm explodes on death, spraying everything around it in burning guts.", "Volatile"), DamageParams.build("5", "0"), TargetParams.build(Range.nontupleBoxIndex, isSelfTargeting), TraitList.acidVomit)));
        enemyAbilityDictionary.Add(wormBossExplosionKey, new Ability(CombatActionSettings.build(DescriptionParams.build(wormBossExplosionKey, "The worm explodes on death, spraying everything around it in burning guts.", "Volatile"), DamageParams.build("30", "0"), TargetParams.build(Range.nontupleBoxIndex, isSelfTargeting), TraitList.acidVomit)));
        enemyAbilityDictionary.Add(wormRestorativeKey, new ReviveAbility(CombatActionSettings.build(DescriptionParams.build(wormRestorativeKey, "The worm disolves into many smaller worms on death, which leave it's carcass in search of new corpses to inhabit.", "Restorative"), DamageParams.build("50"), TargetParams.build(Range.nontupleBoxIndex, isSelfTargeting))));
        enemyAbilityDictionary.Add(wormBossRestorativeKey, new ReviveAbility(CombatActionSettings.build(DescriptionParams.build(wormBossRestorativeKey, "The worm disolves into many smaller worms on death, which leave it's carcass in search of new corpses to inhabit.", "Restorative"), DamageParams.build("100"), TargetParams.build(Range.nontupleBoxIndex, isSelfTargeting))));
        enemyAbilityDictionary.Add(wormFumesKey, new GroundEffectAbility(CombatActionSettings.build(DescriptionParams.build(wormFumesKey, "The worm belches toxic fumes that cloud the tunnel and gnaw at the skin of assailants.", "Fumes")), wormFumesGroundEffect));
        enemyAbilityDictionary.Add(wormOnDeathFumesKey, new GroundEffectAbility(CombatActionSettings.build(DescriptionParams.build(wormOnDeathFumesKey, "The worm belches toxic fumes that cloud the tunnel and gnaw at the skin of assailants.", "Fumes"), TargetParams.build(Range.checkeredLeftIndex)), wormFumesGroundEffect));

        //guard abilities
        enemyAbilityDictionary.Add(slashKey, new Ability(CombatActionSettings.build(DescriptionParams.build(slashKey, "The bite of a sword swung quick."), DamageParams.build("4", "3"))));
        enemyAbilityDictionary.Add(bladeBlitzKey, new Ability(CombatActionSettings.build(DescriptionParams.build(bladeBlitzKey, "Wow Irén, your mom lets you have two axes?"), DamageParams.build("20", "15"), TargetParams.build(Range.tripleHorizontalIndex))));
        enemyAbilityDictionary.Add(guardSpearKey, new Ability(CombatActionSettings.build(DescriptionParams.build(guardSpearKey, "A piercing blow capable of skewering multiple foes."), DamageParams.build("11", "4"), TargetParams.build(Range.doubleVerticalIndex))));
        enemyAbilityDictionary.Add(guardAxeKey, new Ability(CombatActionSettings.build(DescriptionParams.build(guardAxeKey, "A wide sweep from a sharp axe."), DamageParams.build("8", "10"), TargetParams.build(Range.tripleHorizontalIndex))));
        enemyAbilityDictionary.Add(guardArrowBarrageKey, new Ability(CombatActionSettings.build(DescriptionParams.build(guardArrowBarrageKey, "A hail of deadly missles."), DamageParams.build("10", "15"))));
        enemyAbilityDictionary.Add(guardJavelinKey, new Ability(CombatActionSettings.build(DescriptionParams.build(guardJavelinKey, "A missile aimed right at your heart."), DamageParams.build("8", "5"), TargetParams.build(Range.doubleVerticalIndex))));
        enemyAbilityDictionary.Add(guardLashKey, new Ability(CombatActionSettings.build(DescriptionParams.build(guardLashKey, "The bane of slaves everywhere.", "Lashings"), DamageParams.build("8", "40"), TargetParams.build(Range.quadrupleVerticalIndex))));
        enemyAbilityDictionary.Add(guardCoordinateKey, new Ability(CombatActionSettings.build(DescriptionParams.build(guardCoordinateKey, "A leader takes charge and directs their troops in battle, increasing their damage.", "Cohesion"), TargetParams.build(Range.hexadecupleBoxIndex), TraitList.cohesion)));
        enemyAbilityDictionary.Add(guardSlingAttackKey, new Ability(CombatActionSettings.build(DescriptionParams.build(guardSlingAttackKey, "The slinger whips a bullet towards it's target."), DamageParams.build("6", "4"))));
        enemyAbilityDictionary.Add(guardSlaveSummonKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(guardSlaveSummonKey, "The slave driver calls forth more slaves to act as fodder.")), EnemyStatsList.slaveBlockerCombo));
        enemyAbilityDictionary.Add(guardWarriorSummonKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(guardWarriorSummonKey, "The slave driver calls forth more slaves to act as fodder.")), EnemyStatsList.slaveWarriorCombo));


        //Honorguard abilities
        enemyAbilityDictionary.Add(eviscerateKey, new BacklashAbility(CombatActionSettings.build(DescriptionParams.build(eviscerateKey, "A devastating cut capable of unseaming an enemy. This attack hurts the attacker as well as the target.", "MakeItBleed"), DamageParams.build("16", "5"), TargetParams.build(Range.quadrupleBoxIndex), TraitList.vulnerable), oneHundredPercentBacklash));
        enemyAbilityDictionary.Add(skullBashKey, new Ability(CombatActionSettings.build(DescriptionParams.build(skullBashKey, "A blow to the temple that disorients the target.", "Upside the Head"), DamageParams.build("5", "10"), TargetParams.build(Range.doubleVerticalIndex), TraitList.upsideTheHead)));
        enemyAbilityDictionary.Add(squadStrikeKey, new SquadAbility(CombatActionSettings.build(DescriptionParams.build(squadStrikeKey, "An attack that utilizes the cooperation of other squard members. Deals more damage if the attacker is adjacent to one or more allies."), DamageParams.build("3", "5"), TargetParams.build(Range.quadrupleBoxIndex)), "15"));
        enemyAbilityDictionary.Add(skewerKey, new Ability(CombatActionSettings.build(DescriptionParams.build(skewerKey, "The lancer pierces multiple targets in a row."), DamageParams.build("10", "10"), TargetParams.build(Range.quadrupleVerticalIndex))));
        enemyAbilityDictionary.Add(executeKey, new Ability(CombatActionSettings.build(DescriptionParams.build(executeKey, "A chop so quick, it may very well be the last thing you ever see."), DamageParams.build("9", "50"), TargetParams.build(Range.quadrupleHorizontalIndex))));
        enemyAbilityDictionary.Add(turnUpTheHeatKey, new Ability(CombatActionSettings.build(DescriptionParams.build(turnUpTheHeatKey, "Kende cooks his targets until they're seared on the outside but pink in the middle, making them delectable targets for his allies.", "Roasted"), DamageParams.build("2"), TargetParams.build(Range.hexadecupleBoxIndex), TraitList.roasted)));
        enemyAbilityDictionary.Add(shoreUpKey, new MissesArePunishedAbility(CombatActionSettings.build(DescriptionParams.build(shoreUpKey, "The Captain shores up the defenses of her subordinates. If she has a target, she will heal and protect them. If she has no target, she will hurt herself instead.", "Shielded"), DamageParams.build("10", "15"), TraitList.shoredUp)));
        enemyAbilityDictionary.Add(shatterKey, new Ability(CombatActionSettings.build(DescriptionParams.build(shatterKey, "A destructive strike with an enormous area."), DamageParams.build("25", "5"), TargetParams.build(Range.nontupleBoxIndex))));
        enemyAbilityDictionary.Add(frontHandKey, new Ability(CombatActionSettings.build(DescriptionParams.build(frontHandKey, "A torrent of blows that prevents its targets from attacking.", "Lashings"), DamageParams.build("6", "1"), TargetParams.build(Range.quadrupleVerticalIndex), TraitList.whiplash)));
        enemyAbilityDictionary.Add(backHandKey, new Ability(CombatActionSettings.build(DescriptionParams.build(backHandKey, "A painful flurry of lashes.", "Lashings"), DamageParams.build("12", "50"), TargetParams.build(Range.quadrupleHorizontalIndex))));

		//Horse Abilities
		enemyAbilityDictionary.Add(chargeKey, new Ability(CombatActionSettings.build(DescriptionParams.build(chargeKey, "The creature rushes headlong at it's foe, crushing them underfoot."), DamageParams.build("10", "6"), TargetParams.build(Range.quadrupleVerticalIndex))));
		enemyAbilityDictionary.Add(stompKey, new Ability(CombatActionSettings.build(DescriptionParams.build(stompKey, "The creature stamps down on it's target, damaging and stunning it."), DamageParams.build("12", "5"), TraitList.upsideTheHead)));
		enemyAbilityDictionary.Add(feedKey, new HealingAbility(CombatActionSettings.build(DescriptionParams.build(feedKey, "The combatant provides sustenance to their allies, healing them."), DamageParams.build("25"))));

		//Saint Abilities
		enemyAbilityDictionary.Add(boulderRollKey, new Ability(CombatActionSettings.build(DescriptionParams.build(boulderRollKey, "A massive rock tumbling quickly towards you.", "BoulderRoll"), DamageParams.build("12", "10"), TargetParams.build(Range.quadrupleVerticalIndex))));
        enemyAbilityDictionary.Add(lesserBoulderRollKey, new Ability(CombatActionSettings.build(lesserBoulderRollKey, boulderRollDescription, DamageParams.build("6", "5"), TargetParams.build(Range.quadrupleVerticalIndex))));
        enemyAbilityDictionary.Add(evolveKey, new EvolveAbility(CombatActionSettings.build(DescriptionParams.build(evolveKey, "Evolves targets into more powerful versions of themselves."), TargetParams.build(Range.hexadecupleBoxIndex)), enemyAbilityDictionary[boulderRollKey]));
        enemyAbilityDictionary.Add(stoneSaintMaterialsSummonKey, new SummonAbility(CombatActionSettings.build(DescriptionParams.build(stoneSaintMaterialsSummonKey, "The Saint wills more rocks to come to it's aid.")), EnemyStatsList.smallStonesCombo));
	}
	
	private static void instantiateStatAbilities()
	{
		statAbilityDictionary = new Dictionary<string,Ability>();
		string currentKey;

        //start of Str Abilities
        currentKey = generateAbilityKey(strengthKeyChar);
        statAbilityDictionary.Add(currentKey, new KnockBackAbility(CombatActionSettings.build(currentKey, DescriptionParams.build("Send Flying", "Deliver a powerful blow which throws the target backwards into whatever is behind them. Extra damage is dealt depending on how far backwards they travel. If they collide with an enemy, the second enemy also takes damage.", "SendFlying"), DamageParams.build("3S", "D + S"), FrequencyParams.build(twoSlotMax, fiveRoundCooldown)), thirtyPercentPerSquare));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

		currentKey = generateAbilityKey(strengthKeyChar);
        statAbilityDictionary.Add(currentKey, new ActivatedPassive(CombatActionSettings.build(currentKey, TraitList.intimidatingPressence)));
		statAbilityDictionary[currentKey].setStatRequirements(currentKey);

		currentKey = "s-2-3";
        statAbilityDictionary.Add(currentKey, new PassiveAbility(CombatActionSettings.build(currentKey, DescriptionParams.build(minorRegenerationName, "Heal 5% of total health at the end of every combat."), FrequencyParams.build(zeroSlotMax, noCooldown))));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(strengthKeyChar);
        statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build("Make It Bleed", "You impale, bludgeon, or slash your enemy to the point of massive hemorrhaging. The enemy takes initial damage and every hit the enemy takes for the rest of combat deals an additional " + TraitList.vulnerable.getBonusDamageTaken() + " damage.", "MakeItBleed"), DamageParams.build("3S + D", "D"), TargetParams.build(Range.quadrupleBoxIndex), FrequencyParams.build(twoSlotMax, fiveRoundCooldown), TraitList.vulnerable)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(strengthKeyChar);
        statAbilityDictionary.Add(currentKey, new EstablishLinkAbility(CombatActionSettings.build(currentKey, DescriptionParams.build("Chokehold", "Grapple with the enemy, preventing them from acting and making them take half of any damage taken by the caster."), DamageParams.build("S", "D"), FrequencyParams.build(oneSlotMax, sevenRoundCooldown), TraitList.chokehold), TraitList.chokeholdLinkTrait));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(strengthKeyChar);
        statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build("Power Slam", "This potent attack hits an enormous area. Costs three stacks of 'Bloodlust'"), DamageParams.build("5S + 2D", "2S + D"), TargetParams.build(Range.octupleHorizontalIndex), FrequencyParams.build(oneSlotMax, sixRoundCooldown), CostParams.build(ActionCostType.Bloodlust, fourStackCastCost))));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(strengthKeyChar);
        statAbilityDictionary.Add(currentKey, new ActivatedPassive(CombatActionSettings.build(currentKey, TraitList.bloodlust)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        //start of Dex Abilities
        currentKey = generateAbilityKey(dexterityKeyChar);
        statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build(waylayName, "You strike at the perfect moment, bypassing your opponent's unlevied defenses. Guaranteed to Crit in the Surprise Round. Waylay has a long Cooldown."), DamageParams.build("5D + S", "2D+5"), TargetParams.build(Range.quadrupleVerticalIndex), FrequencyParams.build(oneSlotMax, nineRoundCooldown))));
		statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(dexterityKeyChar);
        statAbilityDictionary.Add(currentKey, new ActivatedPassive(CombatActionSettings.build(currentKey, TraitList.devastatingCriticals, new Trait[] { TraitList.afraid })));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(dexterityKeyChar);
        statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build(crippleName, "Your attack leaves permanent damage. The target takes " + TraitList.crippledDamageFormula + " extra damage whenever it receives a debuff."), DamageParams.build("3D + W", "2D"), FrequencyParams.build(twoSlotMax, fiveRoundCooldown), TraitList.crippled)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);
		
		currentKey = generateAbilityKey(dexterityKeyChar);
        statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build(flenseName, "Deal 4D + 2S damage to a target. That target takes 4D damage at the end of the round for the rest of combat."), DamageParams.build("4D + 2S", "2D"), FrequencyParams.build(twoSlotMax, fourRoundCooldown), TraitList.flensed)));
		statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = "d-"+Dexterity.exitStrategy2RoundLevel+"-3";
        statAbilityDictionary.Add(currentKey, new PassiveAbility(currentKey, TraitList.exitStrategy2Round));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(dexterityKeyChar);
        statAbilityDictionary.Add(currentKey, new ActivatedPassive(CombatActionSettings.build(currentKey, TraitList.predation)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(dexterityKeyChar);
        statAbilityDictionary.Add(currentKey, new DoubleStrikeAbility(CombatActionSettings.build(currentKey, DescriptionParams.build("Twice Slice", "You perform a double slice, attacking across a wide swath of the battlefield."), DamageParams.build("4D + 2S", "3D"), TargetParams.build(Range.quadrupleHorizontalIndex), FrequencyParams.build(oneSlotMax, fourRoundCooldown), CostParams.build(ActionCostType.Predation, threeStackCastCost)), Range.quadrupleVerticalIndex));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        //start of Wis Abilities

        currentKey = generateAbilityKey(wisdomKeyChar);
        statAbilityDictionary.Add(currentKey, new Stance(CombatActionSettings.build(currentKey, CostParams.build(ActionCostType.Stance), TraitList.halfHandStance)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(wisdomKeyChar);
        statAbilityDictionary.Add(currentKey, new RepositionEnemyAbility(CombatActionSettings.build(currentKey, DescriptionParams.build("Rolling Throw", "Leverage the enemy's body as a fulcrum and fling them to the ground. The enemy cannot act this turn. Costs two Stacks of any Stance.", "Trip"), DamageParams.build("W + D", "2W + 2D"), FrequencyParams.build(twoSlotMax, threeRoundCooldown), CostParams.build(ActionCostType.Stance, twoStackCastCost), TraitList.tripped)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(wisdomKeyChar);
        statAbilityDictionary.Add(currentKey, new InterruptAbility(CombatActionSettings.build(currentKey, DescriptionParams.build(throatJabName, "A swift jab in the throat that interrupts the enemy's plans. Guaranteed to critically hit if used on an enemy with a '" + TraitList.chargeTraitType + "' type trait. Removes 1 '" + TraitList.chargeTraitType + "' type trait from the target."), DamageParams.build("4W + 2S + 2D"), FrequencyParams.build(oneSlotMax, sixRoundCooldown), TraitList.tripped), TraitList.chargeTraitType));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(wisdomKeyChar);
        statAbilityDictionary.Add(currentKey, new RepetitionAbility(CombatActionSettings.build(currentKey, DescriptionParams.build(doubleStrikeName, "Two quick taps to the gut, one right after the other. Costs one Stack of any Stance."), DamageParams.build("3W"), FrequencyParams.build(twoSlotMax, fourRoundCooldown), CostParams.build(ActionCostType.Stance, oneStackCastCost)), doubleStrikeRepetitions));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = "w-3-3";
        statAbilityDictionary.Add(currentKey, new FistUpgradePassiveAbility(CombatActionSettings.build(currentKey, DescriptionParams.build(improvedStrikesName, "Your fist weapon is replaced with an improved version which deals more damage, crits more often, and hits a larger area than the previous version.", "ImprovedFistIcon"))));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

		currentKey = generateAbilityKey(wisdomKeyChar);
        statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build(crushingBlowName, "A slam that shakes the enemy to their core, leaving them vulnerable. Until the end of the turn they can only muster half of their normal armor value. Costs three Stacks of any Stance."), DamageParams.build("4W + 2S", "W + D"), TargetParams.build(Range.tripleHorizontalIndex), FrequencyParams.build(oneSlotMax, sevenRoundCooldown), CostParams.build(ActionCostType.Stance, fiveStackCastCost), TraitList.crushingBlow)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(wisdomKeyChar);
		statAbilityDictionary.Add(currentKey, new StanceReapplicationAbility(CombatActionSettings.build(currentKey, DescriptionParams.build(battleMeditationName, "Your training has allowed you to realign your energies to immediate affect, healing yourself and your allies and spreading your stance to everyone within range."), DamageParams.build("W"), TargetParams.build(Range.nontupleBoxIndex, isSelfTargeting), FrequencyParams.build(oneSlotMax, eightRoundCooldown))));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        //start of Cha Abilities

        currentKey = generateAbilityKey(charismaKeyChar);
        statAbilityDictionary.Add(currentKey, new PassiveAbility(CombatActionSettings.build(currentKey, DescriptionParams.build(exuberanceName, "Use the energies accumulated by yourself and your allies to activate abilities in combat:\n\nRed Knife: "+redKnifeAcquisitionMethodExplanation+"\n\nBlue Shield: "+blueShieldAcquisitionMethodExplanation+"\n\nYellow Thorn: "+yellowThornAcquisitionMethodExplanation+"\n\nGreen Leaf: "+greenLeafAcquisitionMethodExplanation, IconList.allExuberancesIconName))));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(charismaKeyChar);
        statAbilityDictionary.Add(currentKey, new ExuberanceActivatedPassive(CombatActionSettings.build(currentKey, DescriptionParams.build(unflinchingName, "You are fearless in battle, and your companions know it. Gain "+fourStackBonus+" stacks of the Red Knife Exuberance at the start of every combat.", "Red Knife")), MultiStackProcType.RedKnife, fourStackBonus));	
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = "c-2-3";
        statAbilityDictionary.Add(currentKey, new HealingAbility(CombatActionSettings.build(currentKey, DescriptionParams.build(rallyName, "Encourage an ally to fight on, increasing their damage by " + TraitList.ralliedExtraDamage + " and healing them. Costs 3 Red Knife stacks."), DamageParams.build("2C"), TargetParams.build(Range.singleTargetIndex), FrequencyParams.build(twoSlotMax, sixRoundCooldown), CostParams.build(ActionCostType.RedKnife, threeStackCastCost), TraitList.rallied)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

		currentKey = generateAbilityKey(charismaKeyChar);
        statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build("Victimize", "Your words ring out over the din of combat, alerting your allies to an exploitable weakness in an enemy. Affected targets will take more damage from allied attacks. Costs 2 Red Knife stack and 1 Blue Shield stack."), TargetParams.build(Range.quadrupleBoxIndex), FrequencyParams.build(oneSlotMax, threeRoundCooldown), CostParams.build(new ActionCostType[] { ActionCostType.RedKnife , ActionCostType.BlueShield }, new int[] { twoStackCastCost, oneStackCastCost }), TraitList.insecure)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(charismaKeyChar);
        statAbilityDictionary.Add(currentKey, new RepositionAllyAbility(CombatActionSettings.build(currentKey, DescriptionParams.build("Get Back!", "Order a companion to reposition, healing them in the process. Costs 1 Red Knife stack and 1 Yellow Thorn stack."), DamageParams.build("5C"), TargetParams.build(Range.singleTargetIndex), FrequencyParams.build(oneSlotMax, fourRoundCooldown), CostParams.build(new ActionCostType[] { ActionCostType.RedKnife, ActionCostType.YellowThorn }, new int[] { oneStackCastCost, oneStackCastCost }))));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

		// currentKey = generateAbilityKey(charismaKeyChar);
        // statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build("Isolate", "Direct the flow of battle to sideline a single enemy. This enemy cannot act until it returns to the fray. Taking damage will remove this effect. Only one enemy can be isolated at a time. Costs 1 Red Knife stack and 1 Green Leaf stack."), TargetParams.build(Range.singleTargetIndex), FrequencyParams.build(oneSlotMax, fourRoundCooldown), CostParams.build(new ActionCostType[] { ActionCostType.RedKnife, ActionCostType.GreenLeaf }, new int[] { oneStackCastCost, oneStackCastCost }), TraitList.isolated)));
        // statAbilityDictionary[currentKey].setStatRequirements(currentKey);

		currentKey = "c-3-3";
        statAbilityDictionary.Add(currentKey, new ExuberanceActivatedPassive(CombatActionSettings.build(currentKey, DescriptionParams.build(versatileName, "Your companions have come to rely on you in a variety of situations. Gain "+oneStackBonus+" stack of each Exuberance type at the start of every combat.", "Blue Shield")), new MultiStackProcType[] { MultiStackProcType.RedKnife, MultiStackProcType.BlueShield, MultiStackProcType.YellowThorn, MultiStackProcType.GreenLeaf}, new int[]{oneStackCastCost,oneStackCastCost,oneStackCastCost,oneStackCastCost}));	
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

		currentKey = generateAbilityKey(charismaKeyChar);
        statAbilityDictionary.Add(currentKey, new Ability(CombatActionSettings.build(currentKey, DescriptionParams.build("Demoralize", "Break the enemy's will to fight. All enemies take " + TraitList.demoralizeExtraDamage + " extra damage and act last in the action order."), TargetParams.build(Range.hexadecupleBoxIndex), FrequencyParams.build(oneSlotMax, sixRoundCooldown), CostParams.build(new ActionCostType[] { ActionCostType.RedKnife, ActionCostType.YellowThorn }, new int[] { twoStackCastCost, twoStackCastCost }), TraitList.demoralized)));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey);

        currentKey = generateAbilityKey(charismaKeyChar);
        statAbilityDictionary.Add(currentKey, new RepetitionPerCompanionAbility(CombatActionSettings.build(currentKey, DescriptionParams.build("Volley", "Order all companions to rain missiles on a single foe. Delivers an extra attack per companion on the battlefield. Costs Six Red Knife, 1 Blue Shield, 2 Yellow Thorn, and 1 Green Leaf stack."), DamageParams.build("4C", "2C + D"), TargetParams.build(Range.singleTargetIndex), FrequencyParams.build(oneSlotMax, fourRoundCooldown), CostParams.build(new ActionCostType[] { ActionCostType.RedKnife, ActionCostType.BlueShield, ActionCostType.YellowThorn, ActionCostType.GreenLeaf }, new int[] { sixStackCastCost, oneStackCastCost, twoStackCastCost, oneStackCastCost }))));
        statAbilityDictionary[currentKey].setStatRequirements(currentKey); 
    }
	
	private static void instantiateLessonAbilities()
	{
		lessonAbilityDictionary = new Dictionary<string,Ability>();
		string currentKey;
		
		currentKey = LessonList.clayRemorseKey;

        lessonAbilityDictionary.Add(currentKey, new ActivatedPassive(CombatActionSettings.build(currentKey, TraitList.wearyHeart)));
	}
	
	private static void instantiateCompanionAbilities()
	{
		companionAbilityDictionary = new Dictionary<string,ArrayList>();
		
		ArrayList listOfNandorAbilities = new ArrayList();
        listOfNandorAbilities.Add(new RepositionEnemyAbility(CombatActionSettings.build(NPCNameList.nandor, DescriptionParams.build("Rolling Throw", "Leverage the enemy's body as a fulcrum and fling them to the ground. The enemy cannot act this turn.", "Trip"), DamageParams.build("4 + 3C", "5 + C"), FrequencyParams.build(oneSlotMax, fourRoundCooldown), TraitList.tripped)));

        listOfNandorAbilities.Add(new KnockBackAbility(CombatActionSettings.build(NPCNameList.nandor, DescriptionParams.build("Push", "The companion forces an opponent backwards, dealing damage to the opponent and anyone they are pushed into."), DamageParams.build("4 + 3C", "5 + C"), FrequencyParams.build(oneSlotMax, fiveRoundCooldown)), fiftyPercentPerSquare));

        listOfNandorAbilities.Add(new ActivatedPassive(CombatActionSettings.build(NPCNameList.nandor, TraitList.persistentInfluence)));

        listOfNandorAbilities.Add(new ReviveAbility(CombatActionSettings.build(NPCNameList.nandor, DescriptionParams.build("On Your Feet!", "Cutting an inspiring figure, the companion brings some of the formation back from the brink of submission. Every companion in this ability's area that is downed is healed and put back on their feet.", "OnYourFeet"), DamageParams.build("50"), FrequencyParams.build(oneSlotMax, fiveRoundCooldown))));

		companionAbilityDictionary.Add(NPCNameList.nandor,listOfNandorAbilities);
		
		
		ArrayList listOfRedAbilities = new ArrayList();
		
		listOfRedAbilities.Add(new CompanionAttack(NPCNameList.thatch,"Backhanded Swing","TwoHandedPickReversed",Range.tripleReverseHookIndex, "A swing of Thatch's pick in the opposite direction."));

		listOfRedAbilities.Add(new RepositionAllyAbility(CombatActionSettings.build(NPCNameList.thatch, DescriptionParams.build("Step Between", "Thatch shields an ally from harm, giving them time to reposition. Both Thatch and the target will take 75% less damage for two turns.", "Get Back!"), TargetParams.build(Range.singleTargetIndex), FrequencyParams.build(oneSlotMax, fourRoundCooldown), TraitList.stonewall)));

		//listOfRedAbilities.Add(new Ability(CombatActionSettings.build(NPCNameList.thatch, DescriptionParams.build("Stonewall", "The caster and every ally within the caster's Zone of Influence take 75% less damage until the next turn. Best used early in the turn order. Has a long cooldown."), TargetParams.build(Range.quintupleCrossIndex, isSelfTargeting), FrequencyParams.build(oneSlotMax, sevenRoundCooldown), TraitList.stonewall)));

		listOfRedAbilities.Add(new ActivatedPassive(CombatActionSettings.build(NPCNameList.thatch, TraitList.stalwartInfluence)));

        listOfRedAbilities.Add(new RepositionSelfAbility(CombatActionSettings.build(NPCNameList.thatch, DescriptionParams.build("Daring Sacrifice", "Become invulnerable for one turn. All enemy attack patterns must include this creature when possible, even if they normally would not.", "DaringSacrifice"), FrequencyParams.build(oneSlotMax, sevenRoundCooldown), TraitList.daringSacrifice)));
		
		companionAbilityDictionary.Add(NPCNameList.thatch,listOfRedAbilities);
		
		
		ArrayList listOfCarterAbilities = new ArrayList();

        listOfCarterAbilities.Add(new Ability(CombatActionSettings.build(NPCNameList.carter, DescriptionParams.build("Bristle Bomb", "The caster throws a bomb, damaging the targets and leaving them bristling with needles."), DamageParams.build("2 + 2C", "4 + C"), TargetParams.build(Range.quadrupleBoxIndex), FrequencyParams.build(oneSlotMax, fourRoundCooldown), TraitList.bristled)));

        listOfCarterAbilities.Add(new Ability(CombatActionSettings.build(NPCNameList.carter, DescriptionParams.build("Upside The Head", "The caster gives the target a good thwacking, taking it out of commision for three rounds. Only usable in the suprise round."), DamageParams.build("2 + 2C", "4 + C"), FrequencyParams.build(oneSlotMax, noCooldown, !FrequencyParams.usableOutsideSurpriseRound), TraitList.upsideTheHead)));

        listOfCarterAbilities.Add(new ActivatedPassive(CombatActionSettings.build(NPCNameList.carter, TraitList.cleverInfluence))); 

        listOfCarterAbilities.Add(new TraitBasedDamageAbility(CombatActionSettings.build(NPCNameList.carter, DescriptionParams.build("Bouncing Blade", "The companion throws his blade, striking multiple targets in a line and dealing extra damage per additional trait applied to the target.", "BouncingBlade"), DamageParams.build("25 + 3C", "28"), TargetParams.build(Range.quadrupleVerticalIndex), FrequencyParams.build(oneSlotMax, fiveRoundCooldown)), 0.25));
		
		companionAbilityDictionary.Add(NPCNameList.carter, listOfCarterAbilities);
		
	}
	
	private static void instantiateSummonAbilities()
	{
		summonAbilityDictionary = new Dictionary<string,Ability>();

        summonAbilityDictionary.Add(summonsWhipAttackKey, new Ability(CombatActionSettings.build(summonsWhipAttackKey, DescriptionParams.build("Punishment", "A brutal show of whipwork, displayed by someone who has extensive experience with the tool.", "Lashings"), DamageParams.build("2+6C", "12C"), TargetParams.build(Range.quadrupleVerticalIndex))));
	}
	
	private static void instantiateMiscAbilities()
	{
		miscAbilityDictionary = new Dictionary<string,Ability>();

        miscAbilityDictionary.Add(GodSpellAbilityKey, new Ability(CombatActionSettings.build(GodSpellAbilityKey, DescriptionParams.build("God Spell", "Kills everything on the enemy side of the board.", "Explosion"), DamageParams.build("99S + 99D + 99W + 99C + 1000", "100"), TargetParams.build(Range.hexadecupleBoxIndex))));
        miscAbilityDictionary.Add(moveAllyAbilityKey, new RepositionAllyAbility(CombatActionSettings.build(moveAllyAbilityKey, DescriptionParams.build("Move", "The character hoofs it to the desired space.", "HoofIt"), DamageParams.build("99S + 99D + 99W + 99C + 1000", "100"))));
        miscAbilityDictionary.Add(fearName, new Ability(CombatActionSettings.build(DescriptionParams.build(fearName, "Puts the fear of the Gods in the target, setting their limbs to trembling and turning their bowels to ice water. This renders them stunned and vulnerable.", TraitList.afraid.getIconName()), TraitList.afraid)));
	}
	
	public static CombatAction getCompanionAbility(string name, int abilityIndex) 
	{
		ArrayList listOfAbilities = companionAbilityDictionary[name];
		
		return ((CombatAction) listOfAbilities[abilityIndex]);
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
	
	public static CombatAction getAbility(string key)
	{
		Ability ability = null;
			
		statAbilityDictionary.TryGetValue(key, out ability);
		
		if(ability != null)
		{
			return ability.clone();
		}
		
		lessonAbilityDictionary.TryGetValue(key, out ability);
		
		if(ability != null)
		{
			return ability.clone();
		}
		
		enemyAbilityDictionary.TryGetValue(key, out ability);
		
		if(ability != null)
		{
			return ability.clone();
		}
		
		summonAbilityDictionary.TryGetValue(key, out ability);
		
		if(ability != null)
		{
			return ability.clone();
		}
		
		miscAbilityDictionary.TryGetValue(key, out ability);
		
		if(ability != null)
		{
			return ability.clone();
		}
		
		if(key.Contains(ItemList.fistKey))
		{
			return new FistAttack();
		}
		
		throw new IOException("The key '"+key+"' does not exist."); 
	}
	
	// public static ArrayList getAllAvailableStrengthAbilities()
	// {
	// 	return getAllAvailableAbilitiesOfStat(strengthKeyChar, State.playerStats.getStrength());
	// }
	
	// public static ArrayList getAllAvailableDexterityAbilities()
	// {
	// 	return getAllAvailableAbilitiesOfStat(dexterityKeyChar, State.playerStats.getDexterity());
	// }
	
	// public static ArrayList getAllAvailableWisdomAbilities()
	// {
	// 	return getAllAvailableAbilitiesOfStat(wisdomKeyChar, State.playerStats.getWisdom());
	// }
	
	// public static ArrayList getAllAvailableCharismaAbilities()
	// {
	// 	return getAllAvailableAbilitiesOfStat(charismaKeyChar, State.playerStats.getCharisma());
	// }
	
	public static ArrayList getAllStrengthAbilities()
	{
		return getAllAvailableAbilitiesOfStat(strengthKeyChar, AllyStats.statMaximum);
	}
	
	public static ArrayList getAllDexterityAbilities()
	{
		return getAllAvailableAbilitiesOfStat(dexterityKeyChar, AllyStats.statMaximum);
	}
	
	public static ArrayList getAllWisdomAbilities()
	{
		return getAllAvailableAbilitiesOfStat(wisdomKeyChar, AllyStats.statMaximum);
	}
	
	public static ArrayList getAllCharismaAbilities()
	{
		return getAllAvailableAbilitiesOfStat(charismaKeyChar, AllyStats.statMaximum);
	}

    public static ArrayList getAllAvailableAbilitiesOfStat(char keyChar, int highestLevel)
    {
        return getAllAvailableAbilitiesOfStat(keyChar, lowestLevelForAbilities, highestLevel);
    }

    public static ArrayList getAllAvailableAbilitiesOfStat(StatType type, int lowestLevel, int highestLevel)
	{
        switch (type)
		{
			case StatType.Str:
				return getAllAvailableAbilitiesOfStat(strengthKeyChar, lowestLevel, highestLevel);
            case StatType.Dex:
                return getAllAvailableAbilitiesOfStat(dexterityKeyChar, lowestLevel, highestLevel);
            case StatType.Wis:
                return getAllAvailableAbilitiesOfStat(wisdomKeyChar, lowestLevel, highestLevel);
            case StatType.Cha:
                return getAllAvailableAbilitiesOfStat(charismaKeyChar, lowestLevel, highestLevel);
            default:
				throw new IOException("Unknown StatType: " + type.ToString());
		}
    }

    private static ArrayList getAllAvailableAbilitiesOfStat(char keyChar, int lowestLevel, int highestLevel)
	{
		ArrayList availableAbilities = new ArrayList();
		
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
