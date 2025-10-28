using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnInfoManager
{

    public static SaveBlueprint lastSaveBlueprint;
    public static List<GameObject> allSpawnedObjects;
    private const string playerPrefab = "PlayerOOC";

    [RuntimeInitializeOnLoadMethod]
    private static void initializeSpawnInfoManager()
    {
        lastSaveBlueprint = null;
        allSpawnedObjects = null;
        AreaManager.OnAreaSpawn.AddListener(spawnDetails);
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

    private static void wipeSlate()
    {
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
    }

    public static void spawnDetails()
    {
        wipeSlate();

        allSpawnedObjects = new List<GameObject>();

        allSpawnedObjects.AddRange(spawnBackground());

        allSpawnedObjects.AddRange(spawnPlayer());

        allSpawnedObjects.AddRange(spawnAllInteractables());

        allSpawnedObjects.AddRange(spawnAllTransitions());

        allSpawnedObjects.AddRange(instantiateAllAxisSpawnDetails());

        spawnAllMonsters();

        TrapAndButtonStateManager.setTrapsAndButtons();

        lastSaveBlueprint = null;
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

        Transform player = GameObject.Instantiate(Resources.Load<GameObject>(playerPrefab), AreaManager.getPlayerParent()).transform;

        if (AreaManager.saveBlueprint != null)
        {
            float[] savePos = AreaManager.saveBlueprint.playerPosition;
            player.position = new Vector3(savePos[0], savePos[1]);
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
            NPCSpawnParams spawnParams = NPCSpawnParamList.getNPCSpawnParams(AreaManager.locationName, details.npcName);

            if (spawnParams == null)
            {
                continue;
            }

            if (spawnParams.canSpawn(details.npcName))
            {
                spawnedObjects.Add(spawnInteractable(details));
            }
        }

        return spawnedObjects;
    }

    public static GameObject spawnInteractable(OOCSpawnDetails details)
    {
        GameObject interactable = GameObject.Instantiate(Resources.Load<GameObject>(details.getPrefabName()), details.getParent());

        Transform transform = interactable.transform;

        transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(details.cellCoords);

        Helpers.updateGameObjectPosition(interactable);

        details.setGameObjectName(interactable);

        details.spawnActions(interactable);

        return interactable;
    }

    private static List<GameObject> spawnAllTransitions()
    {
        List<TransitionSpawnInfo> transitionSpawnInfoList = TransitionSpawnInfoList.getTransitionSpawnInfo(AreaManager.locationName);
        List<GameObject> spawnedObjects = new List<GameObject>();

        foreach (TransitionSpawnInfo spawnInfo in transitionSpawnInfoList)
        {
            List<Transition> transitionList = spawnInfo.getTransitions();

            foreach (Transition transition in transitionList)
            {
                GameObject transitionGameObject = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.transitionSpace), AreaManager.getTransitionParent());
                TransitionSpace transitionSpace = transitionGameObject.GetComponent<TransitionSpace>();

                transitionSpace.setTransition(transition);

                transitionGameObject.transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(transition.cellCoords);

                spawnedObjects.Add(transitionGameObject);
            }
        }

        return spawnedObjects;
    }

    private static List<GameObject> instantiateAllAxisSpawnDetails()
    {
        List<AxisSpawnInfo> listOfSpawnInfo = new List<AxisSpawnInfo>();

        listOfSpawnInfo.AddRange(GateSpawnInfoList.getGateSpawnInfo(AreaManager.locationName));
        listOfSpawnInfo.AddRange(SecretDoorSpawnInfoList.getSecretDoorSpawnDetails(AreaManager.locationName));

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

        if (lastSaveBlueprint != null)
        {
            if (lastSaveBlueprint.monsterLocations.Length > index)
            {
                monsterGameObject.transform.position = lastSaveBlueprint.monsterLocations[index].getPosition();
            }
        }
        else
        {
            monsterGameObject.transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(details.cellCoords);
        }

        details.spawnActions(monsterGameObject);

        NPCSpawnParams spawnParams = NPCSpawnParamList.getMonsterSpawnParams(AreaManager.locationName, index.ToString());

        string key = MonsterDefeatKeysList.generateMonsterDefeatKey(monsterMovement.getMonsterPackIndex());

        if (!spawnParams.canSpawn(key))
        {
            monsterGameObject.SetActive(false);
        } else
        {
            details.spawnActions(monsterMovement);
        }

        addGameObject(monsterGameObject);

        return monsterGameObject.transform;
    }

}
