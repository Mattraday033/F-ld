using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnInfoManager
{

    public static List<GameObject> allSpawnedObjects;
    private const string playerPrefab = "PlayerOOC";
    public static Vector3Int defaultCell = new Vector3Int(7,1);

    static SpawnInfoManager()
    {
        AreaManager.OnAreaSpawn.AddListener(spawnDetails);
    }

    public static void initialize()
    {
        
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

        List<GameObject> spawnedObjects = new List<GameObject>();

        spawnedObjects.AddRange(spawnBackground());

        spawnedObjects.AddRange(spawnPlayer());

        spawnedObjects.AddRange(spawnAllInteractables());

        spawnedObjects.AddRange(spawnAllTransitions());

        allSpawnedObjects = spawnedObjects;
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

        if(AreaManager.saveBlueprint != null)
        {
            Debug.LogError("1");
            float[] savePos = AreaManager.saveBlueprint.playerPosition;
            player.position = new Vector3(savePos[0], savePos[1]);
            AreaManager.saveBlueprint = null;
        } else
        {
            Debug.LogError("2");
            player.position = AreaManager.getMasterGrid().GetCellCenterWorld(defaultCell);
        }        

        Helpers.updateGameObjectPosition(player);

        spawnedObjects.Add(player.gameObject);

        return spawnedObjects;
    }

    private static List<GameObject> spawnAllInteractables()
    {
        List<OOCSpawnDetails> oocSpawnDetailsList = OOCSpawnInfoList.getOOCSpawnDetails(AreaManager.locationName); 
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

    private static GameObject spawnInteractable(OOCSpawnDetails details)
    {
        GameObject interactable = GameObject.Instantiate(Resources.Load<GameObject>(details.getPrefabName()), AreaManager.getNPCParent());

        Transform transform = interactable.transform;

        transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(details.cellCoords);

        Helpers.updateGameObjectPosition(interactable);

        details.spawnActions(interactable);

        details.setGameObjectName(interactable);
        
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

                transitionSpace.transition = transition;

                transitionGameObject.transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(transition.cellCoords);

                spawnedObjects.Add(transitionGameObject);
            }
        }

        return spawnedObjects;
    }


}
