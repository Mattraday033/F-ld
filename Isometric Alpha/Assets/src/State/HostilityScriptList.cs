using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class HostilityScriptList
{
    private static Dictionary<string, PlayerInteractionScript> hostilityScriptDict;

    public const string openBarracksGateScriptKey = "OpenedBarracksGate";

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateHostilityScriptList()
    {
        hostilityScriptDict = new Dictionary<string, PlayerInteractionScript>();

        hostilityScriptDict.Add(openBarracksGateScriptKey, new OpenBarracksGateScript());        
    }

    public static void runScript(string key)
    {
        if(!hostilityScriptDict.ContainsKey(key))
        {
            Debug.LogError("No hosility script at key: " + key);

            return;
        }

        hostilityScriptDict[key].runScript();        
    }

}

public class OpenBarracksGateScript : PlayerInteractionScript
{
    private static Vector3Int transitionSpawnCoords = new Vector3Int(6, -2);
    private static TransitionSpace transitionSpace;

    public void openGate()
    {
        GateAndChestManager.addKey(AreaManager.locationName + NPCNameList.barracksArmoryGate+1);
        TransitionManager.BeforeTransition.RemoveListener(openGate);
    }

    public override void runScript()
    {
        AreaList.setAreaToHostile(LocationNameList.guardHouseSouthWest);
        AreaList.setAreaToHostile(LocationNameList.campCenter);
        AreaList.setAreaToHostile(ZoneKeyList.manseFirstFloor + LocationNameList.section1a);
        AreaList.setAreaToHostile(ZoneKeyList.manseSecondFloor + LocationNameList.section1a);

        transitionSpawnCoords = MovementManager.getPlayerCell() + MovementManager.distance1TileSouthEastGrid;

        TransitionManager.CollectTransitionSpaces.AddListener(createTransitionCopy);
        TransitionManager.BeforeTransition.AddListener(openGate);

        TransitionManager.changeLocation(new LadderTransition(LocationNameList.guardHouseSouthWest, LocationNameList.guardHouseSouthWest, transitionSpawnCoords, Facing.NorthWest), Constants.skipAutosave);
    }

    private void createTransitionCopy()
    {
        transitionSpace = SpawnInfoManager.spawnTransitionSpace(LocationNameList.guardHouseSouthWest, LocationNameList.guardHouseSouthWest, transitionSpawnCoords, Facing.NorthWest);
        transitionSpace.updateCounter();

        TransitionManager.CollectTransitionSpaces.RemoveListener(createTransitionCopy);
        TransitionManager.AfterTransition.AddListener(destroyTransition);
    }

    private void destroyTransition()
    {
        if(transitionSpace != null)
        {
            GameObject.Destroy(transitionSpace.gameObject);
        }

        TransitionManager.AfterTransition.RemoveListener(destroyTransition);
    }
}