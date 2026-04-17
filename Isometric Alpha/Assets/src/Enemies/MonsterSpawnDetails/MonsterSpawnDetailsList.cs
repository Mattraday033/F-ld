using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterSpawnDetailsList
{

    private static Dictionary<string, List<MonsterSpawnDetails>> monsterSpawnDetailsDict;


    public static List<MonsterSpawnDetails> getMonsterSpawnDetails()
    {
        string key = AreaManager.locationName;

        List<MonsterSpawnDetails> conditionalList = getConditionalMonsterSpawnList(key);

        if(conditionalList != null)
        {
            return conditionalList;
        }

        if (!monsterSpawnDetailsDict.ContainsKey(key))
        {
            return new List<MonsterSpawnDetails>();
        }

        return monsterSpawnDetailsDict[key];
    }

    public static List<MonsterSpawnDetails> getConditionalMonsterSpawnList(string locationName)
    {
        List<MonsterSpawnDetails> conditionalList = new List<MonsterSpawnDetails>();        

        switch(locationName)
        {       
            case LocationNameList.campMineEntrance:
                
                if(!MonsterSpawnConditionsList.wormsSpawnInsideCamp())
                {
                    return null;
                }

                conditionalList.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(9, 8), movementType: MonsterMovementType.Chases));
                conditionalList.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(8, -12), movementType: MonsterMovementType.Chases));

                return conditionalList;

            case LocationNameList.campSouthEast:
                
                if(!MonsterSpawnConditionsList.wormsSpawnInsideCamp())
                {
                    return null;
                }

                conditionalList.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(10, 0), movementType: MonsterMovementType.Chases));
                conditionalList.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(11, 18), movementType: MonsterMovementType.Chases));                
                conditionalList.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(4, 17), movementType: MonsterMovementType.Chases));
                conditionalList.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(21, -1), movementType: MonsterMovementType.Chases));

                return conditionalList;

            case LocationNameList.campCenter:

                if(!MonsterSpawnConditionsList.wormsSpawnInsideCamp())
                {
                    return null;
                }

                conditionalList.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-7, 3), movementType: MonsterMovementType.Chases));
                conditionalList.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-3, 2), movementType: MonsterMovementType.Chases));                
                conditionalList.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(11, 6), movementType: MonsterMovementType.Chases));
                conditionalList.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(2, 10), movementType: MonsterMovementType.Chases));
                conditionalList.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-5, 19), movementType: MonsterMovementType.Chases));

                return conditionalList;
                
            default:
                return null;
        }
    }

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateMonsterSpawnDetailsList()
    {
        monsterSpawnDetailsDict = new Dictionary<string, List<MonsterSpawnDetails>>();
        List<MonsterSpawnDetails> list;

        #region Camp

        #region 6SlaveShack
        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(-1, -5), Facing.SouthEast, tutorialTargetHash: TutorialSequenceList.firstTutorialEnemyTargetHash));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(4, -2), Facing.NorthEast, tutorialTargetHash: TutorialSequenceList.secondTutorialEnemyTargetHash));

        monsterSpawnDetailsDict.Add(LocationNameList.slaveShackSix, list);
        #endregion

        #region GuardHouse Top Floor
        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(-14, -5), movementType: MonsterMovementType.Chases));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(-7, -4), movementType: MonsterMovementType.Chases));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(4, -5), movementType: MonsterMovementType.Chases));

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(2, 4), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(2, 3), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(5, 3), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(5, 2), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(6, 2), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(4, 2), PrefabNames.pushableCrate));


        monsterSpawnDetailsDict.Add(LocationNameList.guardHouseTopFloor, list);
        #endregion    

        #region GuardHouse NE
        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(2, -1), Facing.SouthWest, MonsterMovementType.Chases));

        monsterSpawnDetailsDict.Add(LocationNameList.guardHouseNorthEast, list);
        #endregion    

        #region GuardHouse SW
        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(-8, -2), Facing.NorthWest, MonsterMovementType.Chases));

        monsterSpawnDetailsDict.Add(LocationNameList.guardHouseSouthWest, list);
        #endregion    

        #region NECamp
        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(-7, 1), Facing.SouthWest, MonsterMovementType.Stationary));

        monsterSpawnDetailsDict.Add(LocationNameList.campNorthEast, list);
        #endregion

        #region CenterCamp
        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(1, 3), Facing.SouthEast, MonsterMovementType.Stationary));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(12, 6), Facing.SouthEast, MonsterMovementType.Stationary));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(19, 8), Facing.SouthWest, MonsterMovementType.Stationary));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(1, 15), Facing.SouthWest, MonsterMovementType.Stationary));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(-5, 19), Facing.NorthWest, MonsterMovementType.Chases));

        monsterSpawnDetailsDict.Add(LocationNameList.campCenter, list);
        #endregion

        #region SECamp
        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(4, 19), movementType: MonsterMovementType.Chases));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(13, 13), movementType: MonsterMovementType.Chases));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(9, 7), Facing.NorthWest, MonsterMovementType.Stationary));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(14, 0), Facing.SouthEast, MonsterMovementType.Stationary));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(23, 0), Facing.SouthEast, MonsterMovementType.Stationary));

        monsterSpawnDetailsDict.Add(LocationNameList.campSouthEast, list);
        #endregion

        #region ManseCamp
        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(4, -13), Facing.SouthWest, MonsterMovementType.Stationary));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(3, -8), Facing.SouthWest, MonsterMovementType.Stationary));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(0, -7), Facing.SouthEast, MonsterMovementType.Stationary));

        monsterSpawnDetailsDict.Add(LocationNameList.campManse, list);
        #endregion

        #endregion

        #region Mine Levels 1-3

        #region MineLvl_1-1b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(8, 7)));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl1 + LocationNameList.section1b, list);

        #endregion

        #region MineLvl_2
        #region MineLvl_2-1a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(2, -1), PrefabNames.pushableCrate, TutorialSequenceList.tutorialCrateTargetHash));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(1, 2), PrefabNames.pushableCrate));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section1a, list);

        #endregion

        #region MineLvl_2-1b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(1, 6)));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section1b, list);

        #endregion

        #region MineLvl_2-1c

        list = new List<MonsterSpawnDetails>();

        // list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(0, -1)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(1, 13)));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section1c, list);

        #endregion

        #region MineLvl_2-2b

        list = new List<MonsterSpawnDetails>();

        // list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(-1, 9)));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section2b, list);

        #endregion

        #region MineLvl_2-3a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(8, 12)));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section3a, list);

        #endregion

        #region MineLvl_2-3b

        list = new List<MonsterSpawnDetails>();

        // list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(3, 1), Facing.SouthEast));
        // list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(0, 0), Facing.SouthEast));
        // list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(-4, 0), Facing.SouthWest));

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(2, -10), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-1, -10), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-5, -10), PrefabNames.pushableCrate));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section3b, list);

        #endregion

        #region MineLvl_2-4

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(5, 9)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(4, 2)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(-2, -5), Facing.NorthWest));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section4, list);

        #endregion

        #region MineLvl_2-5

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(9, 2), movementType: MonsterMovementType.Chases));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(9, 16), movementType: MonsterMovementType.Chases));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section5, list);

        #endregion

        #region MineLvl_2-7b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(1, -11)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(-3, 2)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(3, 9)));

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(-3, 9), Facing.SouthWest, movementType: MonsterMovementType.Stationary));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section7b, list);

        #endregion
        #endregion
        
        #region MineLvl_3-1a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-11, 13), movementType: MonsterMovementType.Chases));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(1, 13), movementType: MonsterMovementType.Chases));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section1a, list);

        #endregion

        #region MineLvl_3-1b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(4, -2), Facing.NorthWest, MonsterMovementType.Chases));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section1b, list);

        #endregion


        #region MineLvl_3-2a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-1, -2), movementType: MonsterMovementType.Chases));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section2a, list);

        #endregion

        #region MineLvl_3-2b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(1, 2)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(5, 3)));

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(9, 3), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(2, -2), PrefabNames.pushableCrate));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section2b, list);

        #endregion

        #region MineLvl_3-3a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(1, 6), Facing.NorthEast));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section3a, list);

        #endregion

        #region MineLvl_3-4a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-7, 4), Facing.SouthWest));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section4a, list);

        #endregion

        #region MineLvl_3-4b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-4, -1), Facing.SouthWest, MonsterMovementType.Chases));

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(2, 13), PrefabNames.pushableCrate));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section4b, list);

        #endregion

        #region MineLvl_3-5

        list = new List<MonsterSpawnDetails>();

        // list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(1, 1), movementType: MonsterMovementType.Chases));
        // list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(16, 3), movementType: MonsterMovementType.Chases));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(15, -1), movementType: MonsterMovementType.Chases));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section5, list);

        #endregion

        #region MineLvl_3-6a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-4, 17)));
        // list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-16, 9)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-14, -4)));
        
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-6, 13), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-7, 12), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-5, 11), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-5, 10), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-5, 9), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-4, 12), PrefabNames.pushableCrate));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section6a, list);

        #endregion

        #region MineLvl_3-7

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-7, 7), Facing.SouthEast, MonsterMovementType.Stationary)); //boss

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-5, -13), Facing.NorthEast, MonsterMovementType.Chases));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-16, -2), Facing.SouthEast, MonsterMovementType.Chases));
        
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(8, 0), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(6, 2), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(6, -2), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(4, -1), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(3, -3), PrefabNames.pushableCrate));

        monsterSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section7, list);

        #endregion

        #endregion

        #region Manse

        #region Manse-1F

        #region Manse-1F-1c

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(14, 7), Facing.SouthEast, movementType: MonsterMovementType.Chases));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(4, -2), Facing.NorthEast, movementType: MonsterMovementType.Chases));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(14, -12), Facing.NorthWest, movementType: MonsterMovementType.Chases));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(23, -4), Facing.SouthWest, movementType: MonsterMovementType.Chases));

        monsterSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section1c, list);

        #endregion

        #region Manse-1F-Dining Room

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(-1, 8), Facing.SouthEast, movementType: MonsterMovementType.Chases));

        monsterSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.diningRoom, list);

        #endregion

        #region Manse-1F-2a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(-2, 7), Facing.SouthEast, movementType: MonsterMovementType.Chases));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(5, 2), movementType: MonsterMovementType.Chases));

        monsterSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section2a, list);

        #endregion

        #region Manse-1F-2b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(6, 2), movementType: MonsterMovementType.Chases));

        monsterSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section2b, list);

        #endregion 

        #region Manse-1F-3a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(4, 0), facing: Facing.SouthWest, movementType: MonsterMovementType.Chases));

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(1, 0), PrefabNames.pushableCrate));

        monsterSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section3a, list);

        #endregion

        #endregion

        #region Manse-2F

        #region Manse-2F-2a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(1, -2), Facing.SouthEast, movementType: MonsterMovementType.Chases));

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(2, -3), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-3, -2), PrefabNames.pushableCrate));

        monsterSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section2a, list);

        #endregion

        #region Manse-2F-3a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(-1, -3), Facing.SouthWest, movementType: MonsterMovementType.Stationary));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(2, 2), Facing.NorthWest, movementType: MonsterMovementType.Chases));

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-6, -3), PrefabNames.pushableCrate));

        monsterSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section3a, list);

        #endregion

        #region Manse-2F-3b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(-8, 5), Facing.NorthEast, movementType: MonsterMovementType.Stationary));

        monsterSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section3b, list);

        #endregion
        #region Manse-2F-3c

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(3, -2), Facing.SouthEast, movementType: MonsterMovementType.Stationary));

        monsterSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section3c, list);

        #endregion
        #region Manse-2F-Stockroom

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(-5, -12), Facing.NorthWest, movementType: MonsterMovementType.Chases));

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-5, -10), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-2, -6), PrefabNames.pushableCrate));

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-5, -5), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-6, -5), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-6, -4), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-5, -3), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-4, -2), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-6, -2), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-5, -1), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-6, -1), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-7, -1), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-4, 0), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-5, 0), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-6, 0), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-8, 0), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-7, 1), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-6, 2), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-7, 2), PrefabNames.pushableCrate));

        monsterSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, list);

        #endregion

        #region Manse-2F-Office

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(-5, -2), Facing.SouthWest, movementType: MonsterMovementType.Chases));

        monsterSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.office, list);

        #endregion

        #endregion

        #region Pit

        #region Pit-1a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(-2, -1), Facing.Random, movementType: MonsterMovementType.Chases));

        monsterSpawnDetailsDict.Add(ZoneKeyList.pit + LocationNameList.section1a, list);

        #endregion

        #region Pit-2c

        list = new List<MonsterSpawnDetails>();

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(15, -8), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-4, 14), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-7, -8), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(14, 11), PrefabNames.pushableCrate));

        monsterSpawnDetailsDict.Add(ZoneKeyList.pit + LocationNameList.section2c, list);

        #endregion

        #region Pit-2d

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.saints, new Vector3Int(-6, 4), Facing.SouthEast, movementType: MonsterMovementType.Stationary));

        monsterSpawnDetailsDict.Add(ZoneKeyList.pit + LocationNameList.section2d, list);

        #endregion

        #endregion

        #endregion

    }


}
