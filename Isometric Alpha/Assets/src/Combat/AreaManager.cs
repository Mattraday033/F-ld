using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaManager : MonoBehaviour
{
    public readonly static UnityEvent OnAreaSpawn = new UnityEvent();

    public static string locationName;
    private static AreaManager instance;

    public static SaveBlueprint saveBlueprint;

    #region Parent Transforms
    public Transform gridParent;
    public Transform playerParent;
    public Transform npcParent;
    public Transform transitionParent;
    public Transform monsterParent;
    public Transform movableObjectParent;
    public Transform nonTransitionColliderParent;
    #endregion

    public NotificationManager notificationManager;
    public HeartBeatManager[] heartBeatManagers;

    public QuestStepActivationScript[] scripts;

    [SerializeField]
    public List<PlayerInteractionScript> onEntryScripts;

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateAreaManager()
    {
        locationName = null;
        instance = null;
        saveBlueprint = null;
    }

    public static AreaManager getInstance()
    {
        return instance;
    }

    public static void changeArea(string newArea)
    {
        locationName = newArea;

        instance.Awake();
    }

    private void Awake()
    {
        instance = this;
        notificationManager.Awake();
        
        foreach(HeartBeatManager heartBeatManager in heartBeatManagers)
        {
            heartBeatManager.Awake();
        }

        runAllScriptsOnLocationEntry();

        addMapData();

        OnAreaSpawn.Invoke();
    }

    private static void runAllScriptsOnLocationEntry()
    {
        List<PlayerInteractionScript> listOfScripts = ScriptOnLocationEntryList.getScriptsOnLocationEntry();

        foreach(PlayerInteractionScript script in listOfScripts)
        {
            script.runScript();
        }
    }

    public IMapObject getMapData()
    {
        return MapObjectList.getMapObject(locationName);
    }

    public IDescribable getAreaDescription()
    {
        return new AreaDescription(getMapData().getNotificationDisplayName());
    }

    public void addMapData()
    {
        List<string> knownLocationsInZone;
        IMapObject mapObject = getMapData();

        if (State.allKnownMapData.ContainsKey(mapObject.getZoneKey()))
        {
            knownLocationsInZone = State.allKnownMapData[mapObject.getZoneKey()]; 
        }
        else
        {
            knownLocationsInZone = new List<string>();
        }        

        if (!knownLocationsInZone.Contains(mapObject.getLocationName()))
        {
            knownLocationsInZone.Add(mapObject.getLocationName());
        }

        State.allKnownMapData[mapObject.getZoneKey()] = knownLocationsInZone;
    }
    
    public static MovementManager getMovementManager()
    {
        return getPlayerParent().GetComponent<MovementManager>();
    }

    public static Grid getMasterGrid()
    {
        return getPlayerParent().GetComponent<MovementManager>().grid;
    }

    public static Transform getGridParent()
    {
        return instance.gridParent;
    }

    public static Transform getPlayerParent()
    {
        return instance.playerParent;
    }

    public static Transform getNPCParent()
    {
        return instance.npcParent;
    }

    public static Transform getTransitionParent()
    {
        return instance.transitionParent;
    }

    public static Transform getMonsterParent()
    {
        return instance.monsterParent;
    }

    public static Transform getMovableObjectParent()
    {
        return instance.movableObjectParent;
    }

    public static Transform getNonTransitionColliderParent()
    {
        return instance.nonTransitionColliderParent;
    }
}

public class AreaDescription : IDescribable
{
    private string areaName;

    public AreaDescription(string areaName)
    {
        this.areaName = areaName;
    }

    public string getName()
    {
        return areaName;
    }

    public void describeSelfFull(DescriptionPanel panel)
    {
        panel.setObjectBeingDescribed(this);

        DescriptionPanel.setText(panel.nameText, areaName);
        DescriptionPanel.setText(panel.notificationNameText, areaName);
    }

    public void describeSelfRow(DescriptionPanel panel)
    {
        //empty on purpose
    }

    public GameObject getDecisionPanel()
    {
        return null;
    }

    public GameObject getDescriptionPanelFull()
    {
        return getDescriptionPanelFull(PanelType.Standard);
    }

    public GameObject getDescriptionPanelFull(PanelType type)
    {
        return Resources.Load<GameObject>(PrefabNames.areaNameDescriptionPanel);
    }

	public List<IDescribable> getRelatedDescribables()
	{
		return new List<IDescribable>();
	}

	public bool buildableWithBlocks()
    {
        return false;
    }

	public bool buildableWithBlocksRows()
    {
        return false;
    }

    public GameObject getRowType(RowType rowType)
    {
        return null;
    }

    public bool ineligible()
    {
        return false;
    }
    public void setUpDecisionPanel(IDecisionPanel descisionPanel)
    {
        //empty on purpose
    }

    public bool withinFilter(string[] filterParameters)
    {
        return true;
    }
}

