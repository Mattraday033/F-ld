using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class SpawnInfoManager
{

    public static SaveBlueprint lastSaveBlueprint;

    public static bool wipingSlate;

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        lastSaveBlueprint = null;
        wipingSlate = false;
        AreaManager.OnAreaSpawn.AddListener(spawnDetails);
        SecretDoorFlags.OnSecretDoorDiscovery.AddListener(spawnHiddenTerrain);
        LoadSaveFile.OnLoadReadBlueprint.AddListener(readSaveBlueprint);
    }

    private static void wipeSlate()
    {
        wipingSlate = true;

        EventList.DestroyAllSpawnedGameObjects.Invoke();

        wipingSlate = false;
    }


    private static void readSaveBlueprint(SaveBlueprint blueprint)
    {
        lastSaveBlueprint = blueprint;
    }

    public static Vector3Int getDefaultCell()
    {
        if(CombatStateManager.hasReturnCell)
        {
            return CombatStateManager.useReturnCell();
        } else
        {
            return new Vector3Int(7, 1);
        }
    }

    public static void spawnDetails()
    {
        wipeSlate();

        spawnBackground();

        spawnPlayer();

        spawnAllInteractables();

        spawnAllTransitions();

        //needs the player to be standing in the area, and has to come before the train so it can leave the placed party members out
        PartyMemberPlacer.restorePlacedPartyMembers();

        PartyMemberTrainManager.createPartyMemberTrain();

        // performButtonScriptStartingAction();

        spawnAllMonsters();

        // TrapAndButtonStateManager.setTrapsAndButtons();

        if(lastSaveBlueprint != null)
        {
            lastSaveBlueprint = null;
        } else if(TrapAndButtonStateManager.trapKeyCount() <= 0)
        {
            // setDefaultTrapStates();
        }
    }

    private static void setDefaultTrapStates()
    {
        List<KeyValuePair<string, bool>> defaultTrapStates = TrapStateList.getDefaultTrapStates();

        foreach(KeyValuePair<string, bool> kvp in defaultTrapStates)
        {
            TrapAndButtonStateManager.setKey(kvp.Key, kvp.Value);
        }
    }

    public static void performButtonScriptStartingAction()
    {
        List<ButtonLogicScript> buttonLogicScripts = ButtonScriptList.getButtonScripts(AreaManager.locationName);

        foreach (ButtonLogicScript script in buttonLogicScripts)
        {
            script.startingAction();
        }
    }

    private static List<GameObject> spawnBackground()
    {
        List<GameObject> spawnedObjects = new List<GameObject>();

        GameObject background = GameObject.Instantiate(Resources.Load<GameObject>(AreaManager.locationName), AreaManager.getGridParent());
        ActivationListenerSpawnBehaviour.addActivationListener(background);
        spawnedObjects.Add(background);

        return spawnedObjects;
    }

    private static List<GameObject> spawnPlayer()
    {
        List<GameObject> spawnedObjects = new List<GameObject>();

        Transform player = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.playerPrefab), AreaManager.getPlayerParent()).transform;
        ActivationListenerSpawnBehaviour.addActivationListener(player.gameObject);

        NewAnimationManager animationManager = player.GetComponent<NewAnimationManager>();

        if (AreaManager.saveBlueprint != null)
        {
            player.position = AreaManager.getMasterGrid().GetCellCenterWorld(AreaManager.saveBlueprint.playerCell);
            AreaManager.saveBlueprint = null;
        }
        else
        {
            player.position = AreaManager.getMasterGrid().GetCellCenterWorld(getDefaultCell());
        }

        animationManager.characterFacing = State.playerFacing;
        animationManager.setAppearanceSource(PartyManager.getPlayerStats());

        GameObjectUtil.updateGameObjectPosition(player);

        spawnedObjects.Add(player.gameObject);

        return spawnedObjects;
    }

    private static List<GameObject> spawnAllInteractables()
    {
        List<OOCSpawnDetails> oocSpawnDetailsList = OOCSpawnDetailsList.getOOCSpawnDetails(AreaManager.locationName);
        List<GameObject> spawnedObjects = new List<GameObject>();

        foreach (OOCSpawnDetails details in oocSpawnDetailsList)
        {
            SpawnParams spawnParams = details.spawnParams;

            List<GameObject> interactables = details.spawnInteractables();

            if (spawnParams != null && !spawnParams.canSpawn(details.displayName))
            {
                foreach(GameObject interactable in interactables)
                {
                    interactable.SetActive(false);
                }
            }

            spawnedObjects.AddRange(interactables);
        }

        return spawnedObjects;
    }

    private static void spawnHiddenTerrain(string secretDoorFlag)
    {
        List<OOCSpawnDetails> oocSpawnDetailsList = OOCSpawnDetailsList.getOOCSpawnDetails(AreaManager.locationName);

        foreach (OOCSpawnDetails details in oocSpawnDetailsList)
        {
            if(!details.spawnsOnSecretDoorActivation)
            {
                continue;
            } 

            HiddenTerrainSpawnDetails hiddenTerrainDetails = details as HiddenTerrainSpawnDetails;

            if (hiddenTerrainDetails.secretDoorKeys.Contains(secretDoorFlag))
            {
                details.spawnInteractables();
                return;
            }
        }
    }

    // public static GameObject spawnInteractable(OOCSpawnDetails details)
    // {
    //     GameObject interactable = GameObject.Instantiate(Resources.Load<GameObject>(details.prefabName), details.parent);

    //     Canvas.ForceUpdateCanvases();

    //     Transform transform = interactable.transform;

    //     transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(details.cellCoords);

    //     GameObjectUtil.updateGameObjectPosition(interactable);

    //     return interactable;
    // }

    private static void spawnAllTransitions()
    {
        List<TransitionSpawnInfo> transitionSpawnInfoList = TransitionSpawnInfoList.getTransitionSpawnInfo(AreaManager.locationName);
        List<GameObject> spawnedObjects = new List<GameObject>();

        foreach (TransitionSpawnInfo spawnInfo in transitionSpawnInfoList)
        {
            List<Transition> transitionList = spawnInfo.getTransitions();

            foreach (Transition transition in transitionList)
            {
                spawnTransitionSpace(transition);
            }
        }
    }
  
    public static TransitionSpace spawnTransitionSpace(string locationName, string destinationName, Vector3Int cellCoords, Facing facing)
    {
        return spawnTransitionSpace(new LadderTransition(locationName, destinationName, cellCoords, facing));
    }

    public static TransitionSpace spawnTransitionSpace(Transition transition)
    {
        GameObject transitionGameObject = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.transitionSpace), AreaManager.getTransitionParent());
        ActivationListenerSpawnBehaviour.addActivationListener(transitionGameObject);
        TransitionSpace transitionSpace = transitionGameObject.GetComponent<TransitionSpace>();

        transitionSpace.setTransition(transition);

        transitionGameObject.transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(transition.cellCoords);

        return transitionSpace;
    }

    private static List<GameObject> instantiateAllAxisSpawnDetails()
    {
        // List<AxisSpawnInfo> listOfSpawnInfo = new List<AxisSpawnInfo>();

        // listOfSpawnInfo.AddRange(TutorialColliderSpawnDetailsList.getTutorialColliderSpawnDetails(AreaManager.locationName));

        List<GameObject> spawnedObjects = new List<GameObject>();

        // foreach (AxisSpawnInfo spawnInfo in listOfSpawnInfo)
        // {
        //     if(!spawnInfo.shouldSpawn())
        //     {
        //         continue;
        //     }

        //     List<OOCSpawnDetails> allSpawnsAlongAxis = spawnInfo.getSpawnDetails();

        //     foreach (OOCSpawnDetails spawnDetails in allSpawnsAlongAxis)
        //     {
        //         spawnedObjects.Add(spawnInteractable(spawnDetails));
        //     }
        // }

        return spawnedObjects;
    }

    public static void spawnAllMonsters()
    {
        if(!AreaList.currentAreaIsHostile())
        {
            return;
        }

        List<MonsterSpawnDetails> monsterDetailsList = MonsterSpawnDetailsList.getMonsterSpawnDetails();

        foreach (MonsterSpawnDetails details in monsterDetailsList)
        {
            spawnMonster(details);
        }
    }

    //the details carry the monster's pack index, and their spawn behaviours add and set up its EnemyMovement
    public static Transform spawnMonster(MonsterSpawnDetails details)
    {
        GameObject monsterGameObject = details.spawnInteractables()[0];
        EnemyMovement monsterMovement = monsterGameObject.GetComponent<EnemyMovement>();

        string key = MonsterDefeatKeysList.generateMonsterDefeatKey(monsterMovement.getMonsterPackIndex());

        if (!details.spawnParams.canSpawn(key))
        {
            monsterMovement.setToDefeatedMode();
        }

        if (lastSaveBlueprint != null)
        {
            if (lastSaveBlueprint.monsterLocations.Length > details.index)
            {
                monsterMovement.setFromWrapper(lastSaveBlueprint.monsterLocations[details.index]);
            }
        }

        return monsterGameObject.transform;
    }

}
