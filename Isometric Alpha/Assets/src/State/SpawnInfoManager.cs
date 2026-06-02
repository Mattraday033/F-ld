using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class SpawnInfoManager
{

    public static SaveBlueprint lastSaveBlueprint;
    public static List<GameObject> allSpawnedObjects;


    [RuntimeInitializeOnLoadMethod]
    private static void initializeSpawnInfoManager()
    {
        lastSaveBlueprint = null;
        allSpawnedObjects = null;
        AreaManager.OnAreaSpawn.AddListener(spawnDetails);
        SecretDoorFlags.OnSecretDoorDiscovery.AddListener(spawnHiddenTerrain);
        LoadSaveFile.OnLoadReadBlueprint.AddListener(readSaveBlueprint);
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

    public static bool wipingSlate = false;

    private static void wipeSlate()
    {
        wipingSlate = true;

        if (allSpawnedObjects == null)
        {
            return;
        }

        foreach (GameObject spawnedObject in allSpawnedObjects)
        {
            if (spawnedObject == null)
            {
                continue;
            }

            GameObject.DestroyImmediate(spawnedObject);
        }

        wipingSlate = false;
    }

    public static void spawnDetails()
    {
        wipeSlate();

        allSpawnedObjects = new List<GameObject>();

        allSpawnedObjects.AddRange(spawnBackground());

        allSpawnedObjects.AddRange(spawnPlayer());

        allSpawnedObjects.AddRange(spawnAllInteractables());

        spawnAllTransitions();

        allSpawnedObjects.AddRange(instantiateAllAxisSpawnDetails());

        PartyMemberTrainManager.createPartyMemberTrain();

        performButtonScriptStartingAction();

        spawnAllMonsters();

        TrapAndButtonStateManager.setTrapsAndButtons();

        if(lastSaveBlueprint != null)
        {
            PartyMemberPlacer.placeAllPartyMembers();
            lastSaveBlueprint = null;
        } else if(TrapAndButtonStateManager.trapKeyCount() <= 0)
        {
            setDefaultTrapStates();
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

    public static void addGameObject(GameObject spawnedObject)
    {
        allSpawnedObjects.Add(spawnedObject);
    }

    private static List<GameObject> spawnBackground()
    {
        List<GameObject> spawnedObjects = new List<GameObject>();

        GameObject background = GameObject.Instantiate(Resources.Load<GameObject>(AreaManager.locationName), AreaManager.getGridParent());
        spawnedObjects.Add(background);

        return spawnedObjects;
    }

    private static List<GameObject> spawnPlayer()
    {
        List<GameObject> spawnedObjects = new List<GameObject>();

        Transform player = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.playerPrefab), AreaManager.getPlayerParent()).transform;

        if (AreaManager.saveBlueprint != null)
        {
            player.position = AreaManager.getMasterGrid().GetCellCenterWorld(AreaManager.saveBlueprint.playerCell);
            AreaManager.saveBlueprint = null;
        }
        else
        {
            player.position = AreaManager.getMasterGrid().GetCellCenterWorld(getDefaultCell());
        }

        Helpers.updateGameObjectPosition(player);

        spawnedObjects.Add(player.gameObject);

        return spawnedObjects;
    }

    private static List<GameObject> spawnAllInteractables()
    {
        List<OOCSpawnDetails> oocSpawnDetailsList = OOCSpawnDetailsList.getOOCSpawnDetails(AreaManager.locationName);
        List<GameObject> spawnedObjects = new List<GameObject>();

        foreach (OOCSpawnDetails details in oocSpawnDetailsList)
        {
            SpawnParams spawnParams = details.getSpawnParams();

            GameObject interactable = spawnInteractable(details);

            if (spawnParams != null && interactable != null && !spawnParams.canSpawn(details.npcName))
            {
                interactable.SetActive(false);
            }

            spawnedObjects.Add(interactable);
        }

        return spawnedObjects;
    }

    private static void spawnHiddenTerrain(string secretDoorFlag)
    {
        List<OOCSpawnDetails> oocSpawnDetailsList = OOCSpawnDetailsList.getOOCSpawnDetails(AreaManager.locationName);

        foreach (OOCSpawnDetails details in oocSpawnDetailsList)
        {
            if(!details.spawnsOnSecretDoorActivation())
            {
                continue;
            } 

            HiddenTerrainSpawnDetails hiddenTerrainDetails = details as HiddenTerrainSpawnDetails;

            if (hiddenTerrainDetails.secretDoorKeys.Contains(secretDoorFlag))
            {
                GameObject spawnedObject = spawnInteractable(details);
                allSpawnedObjects.Add(spawnedObject);
                return;
            }
        }
    }

    public static GameObject spawnInteractable(OOCSpawnDetails details)
    {
        GameObject interactable = GameObject.Instantiate(Resources.Load<GameObject>(details.getPrefabName()), details.getParent());

        Canvas.ForceUpdateCanvases();

        Transform transform = interactable.transform;

        transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(details.cellCoords);

        Helpers.updateGameObjectPosition(interactable);

        details.setGameObjectName(interactable);

        details.spawnActions(interactable);

        return interactable;
    }

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
        TransitionSpace transitionSpace = transitionGameObject.GetComponent<TransitionSpace>();

        transitionSpace.setTransition(transition);

        transitionGameObject.transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(transition.cellCoords);

        addGameObject(transitionGameObject);

        return transitionSpace;
    }

    private static List<GameObject> instantiateAllAxisSpawnDetails()
    {
        List<AxisSpawnInfo> listOfSpawnInfo = new List<AxisSpawnInfo>();

        listOfSpawnInfo.AddRange(GateSpawnInfoList.getGateSpawnInfo(AreaManager.locationName));
        listOfSpawnInfo.AddRange(SecretDoorSpawnInfoList.getSecretDoorSpawnDetails(AreaManager.locationName));
        listOfSpawnInfo.AddRange(TutorialColliderSpawnDetailsList.getTutorialColliderSpawnDetails(AreaManager.locationName));

        List<GameObject> spawnedObjects = new List<GameObject>();

        foreach (AxisSpawnInfo spawnInfo in listOfSpawnInfo)
        {
            if(!spawnInfo.shouldSpawn())
            {
                continue;
            }

            List<OOCSpawnDetails> allSpawnsAlongAxis = spawnInfo.getSpawnDetails();

            foreach (OOCSpawnDetails spawnDetails in allSpawnsAlongAxis)
            {
                spawnedObjects.Add(spawnInteractable(spawnDetails));
            }
        }

        return spawnedObjects;
    }

    private static void spawnAllMonsters()
    {
        if(!AreaList.currentAreaIsHostile())
        {
            return;
        }

        List<MonsterSpawnDetails> monsterDetailsList = MonsterSpawnDetailsList.getMonsterSpawnDetails();

        int index = 0;
        foreach (MonsterSpawnDetails details in monsterDetailsList)
        {
            spawnMonster(details, index);

            index++;
        }
    }

    public static Transform spawnMonster(MonsterSpawnDetails details, int index)
    {
        GameObject monsterGameObject = GameObject.Instantiate(Resources.Load<GameObject>(details.getPrefabName()), details.getParent());
        EnemyMovement monsterMovement = monsterGameObject.GetComponent<EnemyMovement>();

        monsterMovement.setMonsterPackIndex(index);

        details.spawnActions(monsterGameObject);

        InteractableSpawnParams spawnParams = SpawnParamsList.getMonsterSpawnParams(AreaManager.locationName, index.ToString());

        string key = MonsterDefeatKeysList.generateMonsterDefeatKey(monsterMovement.getMonsterPackIndex());

        if (!spawnParams.canSpawn(key))
        {
            monsterMovement.setToDefeatedMode();
        } 

        details.spawnActions(monsterMovement);

        if (lastSaveBlueprint != null)
        {
            if (lastSaveBlueprint.monsterLocations.Length > index)
            {
                monsterMovement.setFromWrapper(lastSaveBlueprint.monsterLocations[index]);
            }
        }
        else
        {   
            Vector3 newPos = AreaManager.getMasterGrid().GetCellCenterWorld(details.cellCoords);
            newPos.z = Helpers.calculateColliderZPosition(details.cellCoords);
            monsterGameObject.transform.position = newPos;
        }

        addGameObject(monsterGameObject);

        return monsterGameObject.transform;
    }

}
