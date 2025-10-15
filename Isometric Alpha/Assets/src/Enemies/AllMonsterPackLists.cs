using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

// [System.Serializable]
public class MonsterPackList: ICloneable
{
	public string sceneName;

	public MonsterPack[] monsterPacks;

	public MonsterSpawnScript[] monsterSpawnScripts;
	public MonsterPack[][] alternateMonsterPacks;

	public MonsterPackList(string sceneName)
	{
		this.sceneName = sceneName;
		this.monsterPacks = new MonsterPack[0];

		this.monsterSpawnScripts = null;
		this.alternateMonsterPacks = new MonsterPack[0][];
	}

	public MonsterPackList(string sceneName, MonsterPack[] mP)
	{
		this.sceneName = sceneName;
		this.monsterPacks = mP;

		this.monsterSpawnScripts = null;
		this.alternateMonsterPacks = new MonsterPack[0][];
	}
	
	public MonsterPackList(string sceneName, MonsterPack[] mP, MonsterSpawnScript monsterSpawnScript, MonsterPack[] alternateMonsterPacks)
	{
		this.sceneName = sceneName;
		monsterPacks = mP;

		this.monsterSpawnScripts = new MonsterSpawnScript[1] { monsterSpawnScript };
		this.alternateMonsterPacks = new MonsterPack[1][] { alternateMonsterPacks };
	}

	public MonsterPackList(string sceneName, MonsterPack[] mP, MonsterSpawnScript[] monsterSpawnScripts, MonsterPack[][] alternateMonsterPacks)
	{
		this.sceneName = sceneName;
		monsterPacks = mP;

		this.monsterSpawnScripts = monsterSpawnScripts;
		this.alternateMonsterPacks = alternateMonsterPacks;
	}

	public void setMonsterPackList()
	{

		if (monsterSpawnScripts == null || alternateMonsterPacks == null ||
			monsterSpawnScripts.Length <= 0 || alternateMonsterPacks.Length <= 0)
		{
			return;
		}

		int indexToUse = PlayerInteractionScript.getIndexOfFirstScriptToEvaluate(monsterSpawnScripts);

		if (indexToUse >= 0 && indexToUse < alternateMonsterPacks.Length)
		{
			monsterPacks = alternateMonsterPacks[indexToUse];
		}
	}

	public object Clone()
	{
		return this.MemberwiseClone();
	}
	
	public MonsterPackList clone()
	{
		MonsterPack[] monsterPackClones = new MonsterPack[monsterPacks.Length];
		
		for(int packIndex = 0; packIndex < monsterPacks.Length; packIndex++)
		{
			monsterPackClones[packIndex] = new MonsterPack(monsterPacks[packIndex]);
		}


		MonsterPack[][] alternateMonsterPackClones = new MonsterPack[alternateMonsterPacks.Length][];

		for (int listIndex = 0; listIndex < alternateMonsterPacks.Length; listIndex++)
		{
			alternateMonsterPackClones[listIndex] = new MonsterPack[alternateMonsterPacks[listIndex].Length];
			
			for(int packIndex = 0; packIndex < alternateMonsterPacks[listIndex].Length; packIndex++)
			{
				alternateMonsterPackClones[listIndex][packIndex] = new MonsterPack(alternateMonsterPacks[listIndex][packIndex]);
			}
		}
		
		return new MonsterPackList(sceneName, monsterPackClones, monsterSpawnScripts, alternateMonsterPackClones);
	}

}

// [System.Serializable]
public struct MonsterPack : IJSONConvertable
{

	private const string saveStartPositionXElementName = "startPositionX";
	private const string saveStartPositionYElementName = "startPositionY";
	private const string saveCurrentPositionXElementName = "currentPositionX";
	private const string saveCurrentPositionYElementName = "currentPositionY";

	public string spriteName;
	
	public int index;

	public int cunningCounter;
	public int intimidateCounter;
	public int retreatCounter;

    public Vector3 startPosition;
	public Vector3 currentPosition;

    public Facing facingDirection;

    public GameObject monsterSprite;
	
	public MonsterPack(int i, string sN, bool d, int cunningCounter, int intimidateCounter, int retreatCounter, Vector3 sP, Vector3 cP, Facing facingDirection)
	{
		index = i;

		spriteName = sN;

		this.cunningCounter = cunningCounter;
		this.intimidateCounter = intimidateCounter;
		this.retreatCounter = retreatCounter;

		startPosition = sP;
		currentPosition = cP;
		monsterSprite = null;
		this.facingDirection = facingDirection;
	}
	
	public MonsterPack(int i, string sN)
	{
		index = i;
		
		spriteName = sN;

        cunningCounter = 0;
        intimidateCounter = 0;
        retreatCounter = 0;

        startPosition = new Vector3(0f,0f,0f);
		currentPosition = new Vector3(0f,0f,0f);
		monsterSprite = null;
		this.facingDirection = CharacterFacing.getRandomFacing();
	}
	
	public MonsterPack(int i, string sN, Facing facingDirection)
	{
		index = i;
		
		spriteName = sN;

        cunningCounter = 0;
        intimidateCounter = 0;
        retreatCounter = 0;

        startPosition = new Vector3(0f,0f,0f);
		currentPosition = new Vector3(0f,0f,0f);
		monsterSprite = null;
		this.facingDirection = facingDirection;
	}
	
	public MonsterPack(MonsterPack packToClone)
	{
		index = packToClone.index;
		
		spriteName = packToClone.spriteName;

        cunningCounter = packToClone.cunningCounter;
        intimidateCounter = packToClone.intimidateCounter;
        retreatCounter = packToClone.retreatCounter;

        startPosition = new Vector3(packToClone.startPosition.x,
									packToClone.startPosition.y,
									0f);
		
		currentPosition = new Vector3(packToClone.startPosition.x,
									  packToClone.startPosition.y,
									  0f);
		monsterSprite = packToClone.monsterSprite;
		facingDirection = packToClone.facingDirection;
	}
	
	private string getSpriteNameAndIndex()
	{
		return spriteName + "-" + index;
	}

	public void setMovementManager(MovementManager mM)
	{
		monsterSprite.GetComponent<EnemyMovement>().movementManager = mM;
	}
	
	public string convertToJson()
	{
		string json = "";
		
		if(monsterSprite == null || monsterSprite is null)
		{
			json = "{\"index\":\"" + index + "\"," +
					"\"spriteName\":\"" + spriteName + "\"," + 
                    "\"cunningCounter\":\"" + cunningCounter + "\"," +
                    "\"intimidateCounter\":\"" + intimidateCounter + "\"," +
                    "\"retreatCounter\":\"" + retreatCounter + "\"," +
                    "\""+ saveStartPositionXElementName + "\":\"" + startPosition.x + "\"," +
					"\""+ saveStartPositionYElementName + "\":\"" + startPosition.y + "\"," +
					"\""+ saveCurrentPositionXElementName + "\":\"" + 0.0 + "\"," +
					"\""+ saveCurrentPositionYElementName + "\":\"" + 0.0 + "\"" +
					"}";
		} else
		{
			json = "{\"index\":\"" + index + "\"," +
					"\"spriteName\":\"" + spriteName + "\"," +
                    "\"cunningCounter\":\"" + cunningCounter + "\"," +
                    "\"intimidateCounter\":\"" + intimidateCounter + "\"," +
                    "\"retreatCounter\":\"" + retreatCounter + "\"," +
                    "\""+ saveStartPositionXElementName + "\":\"" + startPosition.x + "\"," +
					"\""+ saveStartPositionYElementName + "\":\"" + startPosition.y + "\"," +
					"\""+ saveCurrentPositionXElementName + "\":\"" + monsterSprite.transform.localPosition.x + "\"," +
					"\""+ saveCurrentPositionYElementName + "\":\"" + monsterSprite.transform.localPosition.y + "\"," +
					"\"facingDirection\":\"" + ((int) facingDirection) + "\"" +
					"}";
		}
		
		return json;
		
	}
	
	public static MonsterPack extractFromJson(string json)
	{
		dynamic jsonDynamic = JsonConvert.DeserializeObject<dynamic>(json);

		MonsterPack monsterPack = new MonsterPack();

		monsterPack.index = GetFromJson.getElementFromJson(SaveDefaultValues.badMPName, nameof(index), jsonDynamic, SaveDefaultValues.defaultStatOne);
		monsterPack.spriteName = GetFromJson.getElementFromJson(monsterPack.index + "", nameof(spriteName), jsonDynamic, SaveDefaultValues.missingSpriteName);

		monsterPack.cunningCounter = GetFromJson.getElementFromJson(monsterPack.getSpriteNameAndIndex(), nameof(cunningCounter), jsonDynamic, SaveDefaultValues.defaultStatZero);
		monsterPack.intimidateCounter = GetFromJson.getElementFromJson(monsterPack.getSpriteNameAndIndex(), nameof(intimidateCounter), jsonDynamic, SaveDefaultValues.defaultStatZero);
		monsterPack.retreatCounter = GetFromJson.getElementFromJson(monsterPack.getSpriteNameAndIndex(), nameof(retreatCounter), jsonDynamic, SaveDefaultValues.defaultStatZero);

		float startingX = GetFromJson.getElementFromJson(monsterPack.getSpriteNameAndIndex(), saveStartPositionXElementName, jsonDynamic, SaveDefaultValues.defaultMonsterPackStartingPosition);
		float startingY = GetFromJson.getElementFromJson(monsterPack.getSpriteNameAndIndex(), saveStartPositionYElementName, jsonDynamic, SaveDefaultValues.defaultMonsterPackStartingPosition);

		monsterPack.startPosition = new Vector3(startingX, startingY, 0f);
		
		float currentX = GetFromJson.getElementFromJson(monsterPack.getSpriteNameAndIndex(), saveCurrentPositionXElementName, jsonDynamic, SaveDefaultValues.defaultMonsterPackStartingPosition);
		float currentY = GetFromJson.getElementFromJson(monsterPack.getSpriteNameAndIndex(), saveCurrentPositionYElementName, jsonDynamic, SaveDefaultValues.defaultMonsterPackStartingPosition);

		monsterPack.currentPosition = new Vector3(currentX, currentY, 0f);

		monsterPack.facingDirection = (Facing) GetFromJson.getElementFromJson(monsterPack.getSpriteNameAndIndex(), nameof(facingDirection), jsonDynamic, SaveDefaultValues.defaultFacing);

		return monsterPack;
	}
	
}

public static class AllMonsterPackLists
{
	//moveable objects
	public const string crate = "Crate";
	public const string tutorialCrate = "Tutorial Crate";

	//camp packs 
	public const string guards3CampGuards = "Guards3CampGuards";
	public const string guards1CampGuard2Slingers = "Guards1CampGuard2Slingers";
	public const string tutorialBat2Master2Minions1 = "TutorialBat2Master2Minions 1";
	public const string tutorialBat2Master2Minions2 = "TutorialBat2Master2Minions 2";
	public const string guardsChiefIren2CampGuards = "GuardsChiefIren2CampGuards";
	public const string guards2Spear1Axe2Slinger = "Guards2Spear1Axe2Slinger";
	public const string guards1Spear1Axe6Slinger = "Guards1Spear1Axe6Slinger";
	public const string guards1Axe2Slinger1Overseer = "Guards1Axe2Slinger1Overseer";
	public const string guards2Spear1Axe2Slinger1Overseer1Captain = "Guards2Spear1Axe2Slinger1Overseer1Captain";
	public const string guards1Axe2Spear1Overseer1Captain1Signalier = "Guards1Axe2Spear1Overseer1Captain1Signalier";
	public const string guards2Spear1Axe1Signalier1Overseer = "Guards2Spear1Axe1Signalier1Overseer";
	public const string guardsCampNEOverseerBoss = "CampNEOverseerBoss";
	
	//manse 1F packs
	
	public const string guards1Spear1Axe1Captain1Overseer1Linebreaker = "Guards1Spear1Axe1Captain1Overseer1Linebreaker";
	public const string guards1Spear1Axe2Overseer1Lancer = "Guards1Spear1Axe2Overseer1Lancer";
	public const string guards2Spear1Axe1Captain1Lieutenant = "Guards2Spear1Axe1Captain1Lieutenant";
	public const string guards2Spear1Axe1Linebreaker = "Guards2Spear1Axe1Linebreaker";
	public const string guards2Spear2Slinger1Lancer = "Guards2Spear2Slinger1Lancer";
	public const string guards2Spear1Axe1Bloodletter1Captain = "Guards2Spear1Axe1Bloodletter1Captain";
	public const string guards3Overseers1Lieutenant = "Guards3Overseers1Lieutenant";
	
	//manse 2F packs
	
	public const string honorguards1Bloodletter1Lancer1Executioner = "Guards1Bloodletter1Lancer1Executioner";
	public const string honorguards1Bloodletter1Lancer1Linebreaker = "Guards1Bloodletter1Lancer1Linebreaker";
	public const string honorguards1Bloodletter1Executioner1Linebreaker = "Guards1Bloodletter1Executioner1Linebreaker";
	public const string honorguards1Lancer1Executioner1Linebreaker = "Guards1Lancer1Executioner1Linebreaker";
	public const string honorguardsHGCaptain = "GuardsHGCaptain";

	//Pit packs

	public const string stoneSaint = "StoneSaintPack";

	public const string bat1Master2Minions = "Bat1Master2Minions";
	public const string bat2Master2Minions = "Bat2Master2Minions";
	public const string bat1Charged1Master2Minions = "Bat1Charged1Master2Minions";
	public const string bat1Master1Spawner2ExplosiveMinions = "Bat1Master1Spawner2ExplosiveMinions";
	
	public const string bat1Spawner1Charged2ExplosiveMinions = "Bat1Spawner1Charged2ExplosiveMinions";
	public const string bat2Armored2Minions = "Bat2Armored2Minions";
	public const string bat2Charged2Minions = "Bat2Charged2Minions";
	public const string bat1Spawner1Armored1Master1ExplosiveMinion = "Bat1Spawner1Armored1Master1ExplosiveMinion";
	public const string bat1Armored1Charged1Minion = "Bat1Armored1Charged1Minion";
	public const string bat3Master1Armored2Minions = "Bat3Master1Armored2Minions";
	public const string batArmoredBoss = "BatArmoredBoss";
	
	public const string worm1Spawner1Linked2Minions = "Worm1Spawner1Linked2Minions";
	public const string worm1Spawner1Linked4Minions = "Worm1Spawner1Linked4Minions";
	public const string wormBoss2Spawner1Linked4Minions = "WormBoss2Spawner1Linked4Minions";
	public const string worm4Master1Carapace = "Worm4Master1Carapace";
	public const string worm4Masters2Spawner = "Worm4Masters2Spawner";
	public const string worm1BigMaster = "Worm1BigMaster";
	public const string worm1BigMaster2Master2Fumes = "Worm1BigMaster2Master2Fumes";
	public const string worm3Master1Spawner2Linked = "Worm3Master1Spawner2Linked";
	public const string worm3Masters2Fumes1Restorative = "Worm3Masters2Fumes1Restorative";
	public const string worm1BigMaster2Master2Spawner2Restorative = "Worm1BigMaster2Master2Spawner2Restorative";
	public const string wormMine3Boss = "WormMine3Boss";
	public const string worm2BigMaster3Master1Spawner1Restorative = "Worm2BigMaster3Master1Spawner1Restorative";
	
	private static Dictionary<string, MonsterPackList> allMonsterPackLists = new Dictionary<string, MonsterPackList>();
	
	public static MonsterPackList getMonsterPackList(string sceneName)
	{
		if(!allMonsterPackLists.ContainsKey(sceneName))
		{
			return null;
		}

		return allMonsterPackLists[sceneName].clone();
	}

	static AllMonsterPackLists()
	{
		instantiateCamp();
		instantiateManseFirstFloor();
		instantiateManseSecondFloor();
		instantiatePit();
		instantiateMineLvl_1();
		instantiateMineLvl_2();
		instantiateMineLvl_3();
	}

	private static void instantiateCamp()
	{
		MonsterPackList slaveShack6 = new MonsterPackList("6SlaveShack", 	new MonsterPack[]{	new MonsterPack(0, tutorialBat2Master2Minions1, Facing.SouthEast),
																								new MonsterPack(1, tutorialBat2Master2Minions2, Facing.NorthEast)
																						 	},
																new TutorialGuardSpawn(),
																new MonsterPack[]			{
																								new MonsterPack(0, tutorialBat2Master2Minions1, Facing.SouthEast)
																							}
																							);

		MonsterPackList neCamp = new MonsterPackList("NECamp", 	new MonsterPack[]{	new MonsterPack(0, guards2Spear1Axe2Slinger),
																					new MonsterPack(1, guards1Axe2Slinger1Overseer),
																					new MonsterPack(2, guards1Axe2Spear1Overseer1Captain1Signalier),
																					new MonsterPack(3, guards2Spear1Axe1Signalier1Overseer),
																					new MonsterPack(4, guardsCampNEOverseerBoss, Facing.SouthWest)
																				 });
		
		MonsterPackList centerCamp = new MonsterPackList("CenterCamp", 	new MonsterPack[]{	new MonsterPack(0, guards1Axe2Slinger1Overseer),
																							new MonsterPack(1, guards2Spear1Axe2Slinger),
																							new MonsterPack(2, guards2Spear1Axe2Slinger1Overseer1Captain),
																							new MonsterPack(3, guards2Spear1Axe1Signalier1Overseer),
																							new MonsterPack(4, guards2Spear1Axe2Slinger1Overseer1Captain )
																						 });
		
		MonsterPackList mineEntranceCamp = new MonsterPackList("MineEntranceCamp", 	new MonsterPack[]{	new MonsterPack(0, guards2Spear1Axe2Slinger),
																										new MonsterPack(1, guards2Spear1Axe2Slinger)
																									 },
																new CampRevoltWormSpawn(),
																new MonsterPack[]			{
																								new MonsterPack(0, worm2BigMaster3Master1Spawner1Restorative),
																								new MonsterPack(1, worm2BigMaster3Master1Spawner1Restorative)
																							});

		
		MonsterPackList manseCamp = new MonsterPackList("ManseCamp", 	new MonsterPack[]{	new MonsterPack(0, guards2Spear1Axe2Slinger),
																							new MonsterPack(1, guards2Spear1Axe2Slinger)
																						 });
		
		MonsterPackList guardHouseTopFloor = new MonsterPackList("GuardHouseTopFloor", 	new MonsterPack[]{	new MonsterPack(0, tutorialCrate),
																											new MonsterPack(1, crate),
																											new MonsterPack(2, crate),
																											new MonsterPack(3, crate),
																											new MonsterPack(4, crate),
																											new MonsterPack(5, crate),
																											new MonsterPack(6, guards3CampGuards),
																											new MonsterPack(7, guards3CampGuards),
																											new MonsterPack(8, guardsChiefIren2CampGuards, Facing.SouthWest)
																										 });

		MonsterPackList seCamp = new MonsterPackList("SECamp", new MonsterPack[]{   new MonsterPack(0, guards2Spear1Axe2Slinger),
																					new MonsterPack(1, guards1Axe2Slinger1Overseer),
																					new MonsterPack(2, guards2Spear1Axe2Slinger),
																					new MonsterPack(3, guards1Axe2Slinger1Overseer),
																					new MonsterPack(4, guards2Spear1Axe2Slinger),
																					new MonsterPack(5, guards2Spear1Axe2Slinger1Overseer1Captain)
																				 },
													new CampRevoltWormSpawn(),
													new MonsterPack[]			{
																					new MonsterPack(0, worm2BigMaster3Master1Spawner1Restorative),
																					new MonsterPack(1, worm2BigMaster3Master1Spawner1Restorative),
																					new MonsterPack(2, worm2BigMaster3Master1Spawner1Restorative),
																					new MonsterPack(3, worm2BigMaster3Master1Spawner1Restorative),
																					new MonsterPack(4, worm2BigMaster3Master1Spawner1Restorative),
																					new MonsterPack(5, worm2BigMaster3Master1Spawner1Restorative)
																				 }
													);

		allMonsterPackLists.Add(slaveShack6.sceneName, slaveShack6);			
		allMonsterPackLists.Add(neCamp.sceneName, neCamp);		
		allMonsterPackLists.Add(centerCamp.sceneName, centerCamp);
		allMonsterPackLists.Add(seCamp.sceneName, seCamp);	
		allMonsterPackLists.Add(manseCamp.sceneName, manseCamp);
		allMonsterPackLists.Add(guardHouseTopFloor.sceneName, guardHouseTopFloor);	
		allMonsterPackLists.Add(mineEntranceCamp.sceneName, mineEntranceCamp);
	}

	private static void instantiateManseFirstFloor()
	{
		
		MonsterPackList manse_1F_1b = new MonsterPackList("Manse-1F-1b", new MonsterPack[]{	new MonsterPack(0, guards1Spear1Axe1Captain1Overseer1Linebreaker),
																							new MonsterPack(1, guards1Spear1Axe2Overseer1Lancer),
																							new MonsterPack(2, guards2Spear1Axe1Captain1Lieutenant)
																						  });	
		
		MonsterPackList manse_1F_1c = new MonsterPackList("Manse-1F-1c", new MonsterPack[]{	new MonsterPack(0, crate),
																							new MonsterPack(1, guards2Spear1Axe1Linebreaker),
																							new MonsterPack(2, guards1Spear1Axe1Captain1Overseer1Linebreaker)
																						  });	
		
		MonsterPackList manse_1F_Dining_Room= new MonsterPackList("Manse-1F-Dining Room", new MonsterPack[]{ new MonsterPack(0, crate),
																											 new MonsterPack(1, guards2Spear2Slinger1Lancer)
																										   });	
		
		MonsterPackList manse_1F_2a = new MonsterPackList("Manse-1F-2a", new MonsterPack[]{new MonsterPack(0, crate),
																						   new MonsterPack(1, crate),
																						   new MonsterPack(2, crate),
																						   new MonsterPack(3, guards2Spear1Axe1Linebreaker),
																						   new MonsterPack(4, guards1Spear1Axe1Captain1Overseer1Linebreaker),
																						   new MonsterPack(5, guards2Spear1Axe1Captain1Lieutenant),
																						   new MonsterPack(6, guards1Spear1Axe2Overseer1Lancer)
																						  });	
																				 
		MonsterPackList manse_1F_2b = new MonsterPackList("Manse-1F-2b", new MonsterPack[]{new MonsterPack(0, crate),
																						   new MonsterPack(1, crate),
																						   new MonsterPack(2, crate),
																						   new MonsterPack(3, crate),
																						   new MonsterPack(4, crate),
																						   new MonsterPack(5, crate),
																						   new MonsterPack(6, crate),
																						   new MonsterPack(7, crate),
																						   new MonsterPack(8, guards2Spear2Slinger1Lancer, Facing.NorthWest),
																						   new MonsterPack(9, guards1Spear1Axe2Overseer1Lancer, Facing.SouthEast),
																						   new MonsterPack(10, guards2Spear1Axe1Bloodletter1Captain, Facing.NorthWest),
																						   new MonsterPack(11, guards1Spear1Axe1Captain1Overseer1Linebreaker, Facing.SouthEast)
																						  });	
																						  
		MonsterPackList manse_1F_3a = new MonsterPackList("Manse-1F-3a", new MonsterPack[]{new MonsterPack(0, crate),
																						   new MonsterPack(1, guards2Spear1Axe1Linebreaker),
																					       new MonsterPack(2, guards2Spear1Axe1Bloodletter1Captain)
																						  });
		
		// MonsterPackList manse_1F_3b = new MonsterPackList("Manse-1F-3b", new MonsterPack[]{new MonsterPack(0, guards2Spear1Axe1Bloodletter1Captain),
		// 																				   new MonsterPack(1, guards2Spear1Axe1Captain1Lieutenant),
		// 																			       new MonsterPack(2, guards1Spear1Axe1Captain1Overseer1Linebreaker),
		// 																			       new MonsterPack(3, guards3Overseers1Lieutenant)
		// 																				  });	
																						  
		allMonsterPackLists.Add(manse_1F_1b.sceneName, manse_1F_1b);
		
		allMonsterPackLists.Add(manse_1F_1c.sceneName, manse_1F_1c);
		
		allMonsterPackLists.Add(manse_1F_Dining_Room.sceneName, manse_1F_Dining_Room);
		
		allMonsterPackLists.Add(manse_1F_2a.sceneName, manse_1F_2a);
		
		allMonsterPackLists.Add(manse_1F_2b.sceneName, manse_1F_2b);
		
		allMonsterPackLists.Add(manse_1F_3a.sceneName, manse_1F_3a);
		
		// allMonsterPackLists.Add(manse_1F_3b.sceneName, manse_1F_3b);
	}
	
	private static void instantiateManseSecondFloor()
	{
		MonsterPackList manse_2F_2a = new MonsterPackList("Manse-2F-2a", new MonsterPack[]{	new MonsterPack(0, crate),
																							new MonsterPack(1, crate),
																							new MonsterPack(2, honorguards1Bloodletter1Lancer1Executioner),
																							new MonsterPack(3, honorguards1Bloodletter1Lancer1Linebreaker),
																							new MonsterPack(4, honorguards1Bloodletter1Executioner1Linebreaker)
																						  });	
		MonsterPackList manse_2F_3a = new MonsterPackList("Manse-2F-3a", new MonsterPack[]{	new MonsterPack(0, crate),
																							new MonsterPack(1, crate),
																							new MonsterPack(2, crate),
																							new MonsterPack(3, crate),
																							new MonsterPack(4, honorguards1Bloodletter1Executioner1Linebreaker),
																							new MonsterPack(5, honorguards1Lancer1Executioner1Linebreaker)
																						  });	

		MonsterPackList manse_2F_3b = new MonsterPackList("Manse-2F-3b", new MonsterPack[]{	new MonsterPack(0, honorguards1Bloodletter1Lancer1Executioner),
																							new MonsterPack(1, honorguards1Bloodletter1Lancer1Linebreaker),
																							new MonsterPack(2, honorguardsHGCaptain, Facing.NorthEast)
																						  });	
		
		MonsterPackList manse_2F_3c = new MonsterPackList("Manse-2F-3c", new MonsterPack[]{	new MonsterPack(0, honorguards1Bloodletter1Lancer1Executioner),
																							new MonsterPack(1, honorguards1Bloodletter1Lancer1Linebreaker)
																						  });	
		
		MonsterPackList manse_2F_Stockroom = new MonsterPackList("Manse-2F-Stockroom", new MonsterPack[]{	new MonsterPack(0, crate),
																											new MonsterPack(1, crate),
																											new MonsterPack(2, crate),
																											new MonsterPack(3, crate),
																											new MonsterPack(4, honorguards1Bloodletter1Executioner1Linebreaker),
																											new MonsterPack(5, honorguards1Bloodletter1Lancer1Linebreaker)
																										});	
																						  
		allMonsterPackLists.Add(manse_2F_2a.sceneName, manse_2F_2a);

		allMonsterPackLists.Add(manse_2F_3a.sceneName, manse_2F_3a);
		
		allMonsterPackLists.Add(manse_2F_3b.sceneName, manse_2F_3b);
		
		allMonsterPackLists.Add(manse_2F_3c.sceneName, manse_2F_3c);
		
		allMonsterPackLists.Add(manse_2F_Stockroom.sceneName, manse_2F_Stockroom);
	}
	
	private static void instantiatePit()
	{
		MonsterPackList pit_1b = new MonsterPackList("Pit-1b", new MonsterPack[]{	new MonsterPack(0, guards1Axe2Slinger1Overseer)
																				});	
		
		MonsterPackList pit_2c = new MonsterPackList("Pit-2c", new MonsterPack[]{	new MonsterPack(0, crate),
																					new MonsterPack(1, crate),
																					new MonsterPack(2, crate),
																					new MonsterPack(3, crate)
																				});	
		
		MonsterPackList pit_2d = new MonsterPackList("Pit-2d", new MonsterPack[]{	new MonsterPack(0, stoneSaint, Facing.SouthEast)
																				});	
		
		allMonsterPackLists.Add(pit_1b.sceneName, pit_1b);

		allMonsterPackLists.Add(pit_2c.sceneName, pit_2c);
		
		allMonsterPackLists.Add(pit_2d.sceneName, pit_2d);
	}

	private static void instantiateMineLvl_1()
	{
		MonsterPackList mineLvl_1_1b = new MonsterPackList("MineLvl_1-1b", new MonsterPack[]{	new MonsterPack(0, bat1Master1Spawner2ExplosiveMinions),
																								new MonsterPack(1, bat1Charged1Master2Minions)
																							});
																							
		MonsterPackList mineLvl_1_2a = new MonsterPackList("MineLvl_1-2a", new MonsterPack[]{	new MonsterPack(0, bat1Master2Minions),
																								new MonsterPack(1, bat1Master2Minions),
																								new MonsterPack(2, bat1Master2Minions)
																								});
		
		MonsterPackList mineLvl_1_2b = new MonsterPackList("MineLvl_1-2b", new MonsterPack[]{	new MonsterPack(0, bat2Master2Minions),
																								new MonsterPack(1, bat2Master2Minions),
																								new MonsterPack(2, bat2Master2Minions)
																							});
		
		MonsterPackList mineLvl_1_3a = new MonsterPackList("MineLvl_1-3a", new MonsterPack[]{	new MonsterPack(0, bat1Charged1Master2Minions),
																								new MonsterPack(1, bat1Charged1Master2Minions)
																								});
		
		MonsterPackList mineLvl_1_4a = new MonsterPackList("MineLvl_1-4a", new MonsterPack[]{	new MonsterPack(0, bat1Master1Spawner2ExplosiveMinions),
																								new MonsterPack(1, bat1Master1Spawner2ExplosiveMinions),
																								new MonsterPack(2, bat1Master1Spawner2ExplosiveMinions)
																							});
		
		allMonsterPackLists.Add(mineLvl_1_1b.sceneName, mineLvl_1_1b);
		allMonsterPackLists.Add(mineLvl_1_2a.sceneName, mineLvl_1_2a);
		
		allMonsterPackLists.Add(mineLvl_1_2b.sceneName, mineLvl_1_2b);

		allMonsterPackLists.Add(mineLvl_1_3a.sceneName, mineLvl_1_3a);
		
		allMonsterPackLists.Add(mineLvl_1_4a.sceneName, mineLvl_1_4a);
	}

	private static void instantiateMineLvl_2()
	{
		MonsterPackList mineLvl_2_1a = new MonsterPackList("MineLvl_2-1a", new MonsterPack[]{	new MonsterPack(0, tutorialCrate),
																								new MonsterPack(1, crate),
																								new MonsterPack(2, crate)
																							});
																							
		MonsterPackList mineLvl_2_1b = new MonsterPackList("MineLvl_2-1b", new MonsterPack[]{	new MonsterPack(0, bat2Armored2Minions),
																								new MonsterPack(1, bat1Spawner1Charged2ExplosiveMinions),
																								new MonsterPack(2, bat1Spawner1Charged2ExplosiveMinions)
																							});
																							
		MonsterPackList mineLvl_2_1c = new MonsterPackList("MineLvl_2-1c", new MonsterPack[]{	new MonsterPack(0, bat1Spawner1Charged2ExplosiveMinions),
																								new MonsterPack(1, bat2Armored2Minions),
																								new MonsterPack(2, bat1Spawner1Charged2ExplosiveMinions),
																								new MonsterPack(3, bat1Spawner1Charged2ExplosiveMinions)
																							});
		
		MonsterPackList mineLvl_2_2b = new MonsterPackList("MineLvl_2-2b", new MonsterPack[]{	new MonsterPack(0, bat2Armored2Minions),
																								new MonsterPack(1, bat2Charged2Minions)
																							});

		MonsterPackList mineLvl_2_3a = new MonsterPackList("MineLvl_2-3a", new MonsterPack[]{	new MonsterPack(0, bat1Spawner1Armored1Master1ExplosiveMinion),
																								new MonsterPack(1, bat1Armored1Charged1Minion)
																							});
		
		MonsterPackList mineLvl_2_3b = new MonsterPackList("MineLvl_2-3b", new MonsterPack[]{	new MonsterPack(0, crate),
																								new MonsterPack(1, crate),
																								new MonsterPack(2, crate)
																							});
																							
		MonsterPackList mineLvl_2_4 = new MonsterPackList("MineLvl_2-4",   new MonsterPack[]{	new MonsterPack(0, worm1Spawner1Linked2Minions),
																								new MonsterPack(1, worm1Spawner1Linked4Minions),
																								new MonsterPack(2, wormBoss2Spawner1Linked4Minions, Facing.NorthWest)
																							});
																							
		MonsterPackList mineLvl_2_6 = new MonsterPackList("MineLvl_2-6",   new MonsterPack[]{	new MonsterPack(0, bat2Master2Minions)
																							}); // Wisdom
																							
		MonsterPackList mineLvl_2_7b = new MonsterPackList("MineLvl_2-7b", new MonsterPack[]{	
																								new MonsterPack(0, bat1Spawner1Charged2ExplosiveMinions),
																								new MonsterPack(1, bat2Armored2Minions),
																								new MonsterPack(2, bat1Armored1Charged1Minion),
																								new MonsterPack(3, bat1Master1Spawner2ExplosiveMinions),
																								new MonsterPack(4, bat2Charged2Minions),
																								new MonsterPack(5, bat3Master1Armored2Minions),
																								new MonsterPack(6, batArmoredBoss, Facing.SouthWest) //boss
																							});
		
		allMonsterPackLists.Add(mineLvl_2_1a.sceneName, mineLvl_2_1a);
		allMonsterPackLists.Add(mineLvl_2_1b.sceneName, mineLvl_2_1b);
		allMonsterPackLists.Add(mineLvl_2_1c.sceneName, mineLvl_2_1c);
		
		allMonsterPackLists.Add(mineLvl_2_2b.sceneName, mineLvl_2_2b);
		
		allMonsterPackLists.Add(mineLvl_2_3a.sceneName, mineLvl_2_3a);
		allMonsterPackLists.Add(mineLvl_2_3b.sceneName, mineLvl_2_3b);
		
		allMonsterPackLists.Add(mineLvl_2_4.sceneName, mineLvl_2_4);
		
		allMonsterPackLists.Add(mineLvl_2_6.sceneName, mineLvl_2_6);
		
		allMonsterPackLists.Add(mineLvl_2_7b.sceneName, mineLvl_2_7b);
	}
	
	private static void instantiateMineLvl_3()
	{
		MonsterPackList mineLvl_3_1a = new MonsterPackList("MineLvl_3-1a", new MonsterPack[]{	new MonsterPack(0, worm4Master1Carapace),
																								new MonsterPack(1, worm4Master1Carapace),
																								new MonsterPack(2, worm4Master1Carapace)
																							});	
		MonsterPackList mineLvl_3_1b = new MonsterPackList("MineLvl_3-1b", new MonsterPack[]{
																							});	
		
		MonsterPackList mineLvl_3_2a = new MonsterPackList("MineLvl_3-2a", new MonsterPack[]{	new MonsterPack(0, worm4Masters2Spawner),
																								new MonsterPack(1, worm4Masters2Spawner)
																							});	
		
		MonsterPackList mineLvl_3_2b = new MonsterPackList("MineLvl_3-2b", new MonsterPack[]{	new MonsterPack(0, crate),
																								new MonsterPack(1, crate),
																								new MonsterPack(2, crate),
																								new MonsterPack(3, crate),
																								new MonsterPack(4, worm3Master1Spawner2Linked),
																								new MonsterPack(5, worm3Master1Spawner2Linked),
																								new MonsterPack(6, worm3Master1Spawner2Linked),
																								new MonsterPack(7, worm3Master1Spawner2Linked)
																							});		
		
		//MonsterPackList mineLvl_3_3a = new MonsterPackList("MineLvl_3-3a", new MonsterPack[]{	new MonsterPack(0, bat1Master2Minions),
		//																						new MonsterPack(1, bat1Master2Minions)
		//																					});		
		
		MonsterPackList mineLvl_3_4a = new MonsterPackList("MineLvl_3-4a", new MonsterPack[]{	new MonsterPack(0, crate),
																								new MonsterPack(1, worm1BigMaster),
																								new MonsterPack(2, worm1BigMaster),
																								new MonsterPack(3, worm1BigMaster)
																							});
																							
		MonsterPackList mineLvl_3_4b = new MonsterPackList("MineLvl_3-4b", new MonsterPack[]{	new MonsterPack(0, worm3Masters2Fumes1Restorative),
																								new MonsterPack(1, worm3Masters2Fumes1Restorative),
																								new MonsterPack(2, worm3Masters2Fumes1Restorative)
																							});
																							
		MonsterPackList mineLvl_3_5a = new MonsterPackList("MineLvl_3-5", new MonsterPack[]{	new MonsterPack(0, worm3Masters2Fumes1Restorative)
																							});
		
		MonsterPackList mineLvl_3_6a = new MonsterPackList("MineLvl_3-6a", new MonsterPack[]{	new MonsterPack(0, worm1BigMaster2Master2Fumes),
																								new MonsterPack(1, worm1BigMaster2Master2Fumes)
																							});	
																							
		MonsterPackList mineLvl_3_7 = new MonsterPackList("MineLvl_3-7", new MonsterPack[]{		new MonsterPack(0, worm1BigMaster2Master2Spawner2Restorative),
																								new MonsterPack(1, worm1BigMaster2Master2Spawner2Restorative),
																								new MonsterPack(2, worm1BigMaster2Master2Spawner2Restorative),
																								new MonsterPack(3, wormMine3Boss, Facing.SouthEast)
																							});	
		
		allMonsterPackLists.Add(mineLvl_3_1a.sceneName, mineLvl_3_1a);
		allMonsterPackLists.Add(mineLvl_3_1b.sceneName, mineLvl_3_1b);					
		allMonsterPackLists.Add(mineLvl_3_2a.sceneName, mineLvl_3_2a);
		allMonsterPackLists.Add(mineLvl_3_2b.sceneName, mineLvl_3_2b);
		//allMonsterPackLists.Add(mineLvl_3_3a.sceneName, mineLvl_3_3a);						
		allMonsterPackLists.Add(mineLvl_3_4a.sceneName, mineLvl_3_4a);
		allMonsterPackLists.Add(mineLvl_3_4b.sceneName, mineLvl_3_4b);
		allMonsterPackLists.Add(mineLvl_3_5a.sceneName, mineLvl_3_5a);
		allMonsterPackLists.Add(mineLvl_3_6a.sceneName, mineLvl_3_6a);
		allMonsterPackLists.Add(mineLvl_3_7.sceneName, mineLvl_3_7);
	}

	/*
	public static void setAllMonsterPackListsToReset()
	{
		foreach (KeyValuePair<string, MonsterPackList> monsterPackListKVP in allMonsterPackLists)
		{
			monsterPackListKVP.Value.shouldReset = true;
		}
	}

	/*public static void markAllPacksAsUndefeated()
	{
		foreach (KeyValuePair<string, MonsterPackList> monsterPackListKVP in allMonsterPackLists)
		{
			for(int j = 0; j < monsterPackListKVP.Value.monsterPacks.Length; j++)
			{
				monsterPackListKVP.Value.monsterPacks[j].defeated = false;
			}
		}
	}*/
}
