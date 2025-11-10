using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OOCSpawnDetails
{

    private const string gameObjectNameSuffix = "'s GameObject";
    private const string gameObjectPlaceHolderName = "PlaceHolder GameObject";

    public string tutorialTargetHash = "";

    public string npcName = "";
    public Vector3Int cellCoords;

    public OOCSpawnDetails()
    {
        this.cellCoords = Vector3Int.zero;
    }

    public OOCSpawnDetails(Vector3Int cellCoords)
    {
        this.cellCoords = cellCoords;
    }

    public OOCSpawnDetails(string npcName, Vector3Int cellCoords)
    {
        this.npcName = npcName;
        this.cellCoords = cellCoords;
    }

    public virtual string getSpriteName()
    {
        return null;
    }

    public virtual string getPrefabName()
    {
        return null;
    }

    public virtual bool determineSpriteAtSpawn()
    {
        return true;
    }

    public virtual Transform getParent()
    {
        return null;
    }

    public virtual bool spawnsOnSecretDoorActivation()
    {
        return false;
    }

    public virtual SpawnParams getSpawnParams()
    {
        return SpawnParamList.getSpawnParams(AreaManager.locationName, npcName);
    }

    public bool hasTutorialTargetHash()
    {
        return tutorialTargetHash != null && tutorialTargetHash.Length > 0;
    }

    public virtual void setGameObjectName(GameObject gameObject)
    {
        if (npcName.Length > 0)
        {
            gameObject.name = npcName + gameObjectNameSuffix;
        }
        else
        {
            gameObject.name = gameObjectPlaceHolderName;
        }
    }

    public virtual void spawnActions(GameObject interactable)
    {
        SpriteRenderer spriteRenderer = interactable.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Helpers.loadSpriteFromResources(getSpriteName());
    }

    public static void addTutorialTargetComponent(GameObject gameObject, string tutorialTargetHash)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        addTutorialTargetComponent(gameObject, spriteRenderer, tutorialTargetHash);
    }

    public static void addTutorialTargetComponent(GameObject gameObject, SpriteRenderer spriteRenderer, string tutorialTargetHash)
    {
        gameObject.AddComponent<RectTransform>();

        GameObject targetRect = GameObject.Instantiate(new GameObject("Target Rect"), gameObject.transform);

        RectTransform rectTransform = targetRect.AddComponent<RectTransform>();

        rectTransform.anchorMin = Vector2Int.zero;
        rectTransform.anchorMax = Vector2Int.one;
        rectTransform.pivot = new Vector2(.5f, .5f);

        rectTransform.offsetMin = Vector2Int.zero;
        rectTransform.offsetMax = Vector2Int.zero;

        Helpers.updateColliderPosition(targetRect);

        TutorialSequenceStepTargetSprite targetSprite = targetRect.AddComponent<TutorialSequenceStepTargetSprite>();
        targetSprite.tutorialHash = tutorialTargetHash;

        targetSprite.spriteRenderer = spriteRenderer;
    }

    public static void addTutorialTargetComponent(ITutorialSequenceTarget target, string tutorialTargetHash)
    {
        target.setTutorialHash(tutorialTargetHash);
        target.getGameObject().AddComponent<RectTransform>();
    }

}

public class TutorialColliderSpawnDetails : OOCSpawnDetails
{

    public string tutorialKey;
    public string seenFlagName;
    public StartSpawningAllTrueFlagList startSpawningFlagList;
    public int monsterDefeatKeyIndex;

    public TutorialColliderSpawnDetails(Vector3Int cellCoords, string tutorialKey, string seenFlagName) :
    base(cellCoords)
    {
        this.tutorialKey = tutorialKey;
        this.seenFlagName = seenFlagName;
        this.startSpawningFlagList = new StartSpawningAllTrueFlagList();
        this.monsterDefeatKeyIndex = -1;
    }

    public TutorialColliderSpawnDetails(Vector3Int cellCoords, string tutorialKey, string seenFlagName, StartSpawningAllTrueFlagList startSpawningFlagList) :
    base(cellCoords)
    {
        this.tutorialKey = tutorialKey;
        this.seenFlagName = seenFlagName;
        this.startSpawningFlagList = startSpawningFlagList;
        this.monsterDefeatKeyIndex = -1;
    }

    public TutorialColliderSpawnDetails(Vector3Int cellCoords, string tutorialKey, string seenFlagName, StartSpawningAllTrueFlagList startSpawningFlagList, int monsterDefeatKeyIndex) :
    base(cellCoords)
    {
        this.tutorialKey = tutorialKey;
        this.seenFlagName = seenFlagName;
        this.startSpawningFlagList = startSpawningFlagList;
        this.monsterDefeatKeyIndex = monsterDefeatKeyIndex;
    }


    public override string getSpriteName()
    {
        return PrefabNames.defaultNPCSprite;
    }

    public override string getPrefabName()
    {
        return PrefabNames.tutorialCollider;
    }

    public override Transform getParent()
    {
        return AreaManager.getNonTransitionColliderParent();
    }

    public override void spawnActions(GameObject tutorialColliderGameObject)
    {
        if (shouldNotSpawn())
        {
            GameObject.DestroyImmediate(tutorialColliderGameObject);
            return;
        }

        TutorialTriggerCollider tutorialCollider = tutorialColliderGameObject.GetComponent<TutorialTriggerCollider>();
        tutorialCollider.tutorialSequenceKey = tutorialKey;
    }

    private bool shouldNotSpawn()
    {
        return Flags.getFlag(seenFlagName) ||
                (startSpawningFlagList != null && !startSpawningFlagList.evaluateFlags())
                || (monsterDefeatKeyIndex >= 0 && !MonsterDefeatKeysList.monsterIsDefeated(monsterDefeatKeyIndex));
    }

}

public abstract class CunningObjectSpawnDetails : OOCSpawnDetails
{
    public Facing startFacing;
    public Facing endFacing;
    public CunningObjectSpriteCategory category;

    public CunningObjectSpawnDetails(Vector3Int cellCoords, Facing facing, CunningObjectSpriteCategory category) :
    base(category.ToString(), cellCoords)
    {
        this.startFacing = facing;
        this.endFacing = facing;

        this.category = category;
    }

    public CunningObjectSpawnDetails(Vector3Int cellCoords, Facing startFacing, Facing endFacing, CunningObjectSpriteCategory category) :
    base(category.ToString(), cellCoords)
    {
        this.startFacing = startFacing;
        this.endFacing = endFacing;

        this.category = category;
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParent();
    }

    public override void spawnActions(GameObject gameObject)
    {
        CunningObject cunningObject = gameObject.GetComponent<CunningObject>();

        spawnActions(cunningObject);
    }

    public abstract void spawnActions(CunningObject cunningObject);

}

public class CunningBlockerSpawnDetails : CunningObjectSpawnDetails
{

    private ObstacleSpawnDetails blockerSpawnDetails;

    public CunningBlockerSpawnDetails(Vector3Int cellCoords, Facing facing, CunningObjectSpriteCategory category, ObstacleSpawnDetails blockerSpawnDetails) :
    base(cellCoords, facing, category)
    {
        this.blockerSpawnDetails = blockerSpawnDetails;
    }

    public CunningBlockerSpawnDetails(Vector3Int cellCoords, Facing startFacing, Facing endFacing, CunningObjectSpriteCategory category, ObstacleSpawnDetails blockerSpawnDetails) :
    base(cellCoords, startFacing, endFacing, category)
    {
        this.blockerSpawnDetails = blockerSpawnDetails;
    }

    public CunningBlockerSpawnDetails(Vector3Int cellCoords, Facing facing, CunningObjectSpriteCategory category, ObstacleSpawnDetails blockerSpawnDetails, string tutorialTargetHash) :
    base(cellCoords, facing, category)
    {
        this.blockerSpawnDetails = blockerSpawnDetails;
        this.tutorialTargetHash = tutorialTargetHash;
    }

    public CunningBlockerSpawnDetails(Vector3Int cellCoords, Facing startFacing, Facing endFacing, CunningObjectSpriteCategory category, ObstacleSpawnDetails blockerSpawnDetails, string tutorialTargetHash) :
    base(cellCoords, startFacing, endFacing, category)
    {
        this.blockerSpawnDetails = blockerSpawnDetails;
        this.tutorialTargetHash = tutorialTargetHash;
    }

    public override string getPrefabName()
    {
        return PrefabNames.cunningBlocker;
    }

    // public override void spawnActions(GameObject cunningBlocker)
    // {
    //     base.spawnActions(cunningBlocker);


    // }

    public override void spawnActions(CunningObject cunningObject)
    {
        CunningBlocker cunningBlocker = cunningObject as CunningBlocker;
        GameObject blocker = SpawnInfoManager.spawnInteractable(blockerSpawnDetails);

        cunningBlocker.build(startFacing, endFacing, category, blocker);
        SpawnInfoManager.addGameObject(blocker);

        if (hasTutorialTargetHash())
        {
            addTutorialTargetComponent(cunningBlocker.gameObject, cunningBlocker.spriteRenderer, tutorialTargetHash);
        }
    }
}

public class ObstacleSpawnDetails : OOCSpawnDetails
{
    private string spriteName;
    private Color tint;

    public ObstacleSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName) :
    base(npcName, cellCoords)
    {
        this.spriteName = spriteName;
        this.tint = Color.white;
    }

    public ObstacleSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName, Color tint) :
    base(npcName, cellCoords)
    {
        this.spriteName = spriteName;
        this.tint = tint;
    }

    public override string getSpriteName()
    {
        return spriteName;
    }

    public override string getPrefabName()
    {
        return PrefabNames.oocObstacle;
    }

    public override bool determineSpriteAtSpawn()
    {
        return true;
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParent();
    }

    public override void spawnActions(GameObject interactable)
    {
        Obstacle obstacle = interactable.GetComponent<Obstacle>();
        obstacle.setObstacleName(npcName);

        spawnActions(interactable.GetComponent<SpriteRenderer>());
    }

    public virtual void spawnActions(SpriteRenderer spriteRenderer)
    {
        if(spriteRenderer == null)
        {
            return;
        }

        spriteRenderer.sprite = Helpers.loadSpriteFromResources(getSpriteName());
        spriteRenderer.color = tint;
    }

}

public class RubbleObstacleSpawnDetails : ObstacleSpawnDetails
{
    public RubbleObstacleSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName) :
    base(npcName, cellCoords, spriteName)
    {
        
    }

    public override void spawnActions(SpriteRenderer spriteRenderer)
    {
        base.spawnActions(spriteRenderer);

        spriteRenderer.color = ColorList.getRubbleColorFromLocationName();
    }

}

public class ButtonSpawnDetails : OOCSpawnDetails
{

    private int index;

    public ButtonSpawnDetails(string npcName, Vector3Int cellCoords) :
    base(npcName, cellCoords)
    {
        this.index = 0;
    }

    public ButtonSpawnDetails(string npcName, Vector3Int cellCoords, string tutorialTargetHash) :
    base(npcName, cellCoords)
    {
        this.tutorialTargetHash = tutorialTargetHash;
        this.index = 0;
    }

    public override string getPrefabName()
    {
        return PrefabNames.floorButton;
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParent();
    }

    public override void spawnActions(GameObject button)
    {
        if (hasTutorialTargetHash())
        {
            SpriteRenderer spriteRenderer = button.GetComponent<SpriteRenderer>();

            addTutorialTargetComponent(button, spriteRenderer, tutorialTargetHash);
        }

        FloorButton floorButton = button.GetComponent<FloorButton>();
        floorButton.index = index;
    }

}

public class NPCSpawnDetails : OOCSpawnDetails
{

    public const string extraSpaceNameSuffix = "'s Extra Space GameObject";

    public bool activated;
    public Vector3Int[] extraSpaces = new Vector3Int[0];
    public Dialogue dialogue;
    public SpeakAtStartScript speakAtStartScript;

    public NPCSpawnDetails(string npcName, Vector3Int cellCoords) :
    base(npcName, cellCoords)
    {
        this.activated = true;

        this.dialogue = getDialogue();
    }

    public NPCSpawnDetails(string npcName, Vector3Int cellCoords, string areaName) :
    base(npcName, cellCoords)
    {
        this.activated = true;
        this.dialogue = getDialogue(areaName);
    }

    public NPCSpawnDetails(string npcName, Vector3Int cellCoords, string areaName, Vector3Int[] extraSpaces) :
    base(npcName, cellCoords)
    {
        this.activated = true;
        this.dialogue = getDialogue(areaName);

        this.extraSpaces = extraSpaces;
    }

    public NPCSpawnDetails(string npcName, Vector3Int cellCoords, string areaName, SpeakAtStartScript speakAtStartScript) :
    base(npcName, cellCoords)
    {
        this.activated = true;
        this.dialogue = getDialogue(areaName);
        this.speakAtStartScript = speakAtStartScript;
    }

    public NPCSpawnDetails(string npcName, Vector3Int cellCoords, bool activated) :
    base(npcName, cellCoords)
    {
        this.activated = activated;
        this.dialogue = null;
    }

    public NPCSpawnDetails(string npcName, Vector3Int cellCoords, bool activated, string areaName) :
    base(npcName, cellCoords)
    {
        this.activated = activated;
        this.dialogue = getDialogue(areaName);
    }

    public Dialogue getDialogue()
    {
        return getDialogue(Constants.emptyString);
    }

    public virtual Dialogue getDialogue(string areaName)
    {
        return DialogueList.getDialogue(npcName, areaName);
    }

    public virtual bool interactable()
    {
        return dialogue != null;
    }

    public override string getSpriteName()
    {
        return PrefabNames.defaultNPCSprite;
    }

    public override string getPrefabName()
    {
        return PrefabNames.NPC;
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParent();
    }

    public override void spawnActions(GameObject npc)
    {
        base.spawnActions(npc);

        npc.SetActive(activated);

        DialogueTrigger dialogueTrigger = npc.GetComponent<DialogueTrigger>();

        if (interactable())
        {
            dialogueTrigger.dialogue = dialogue;
            dialogueTrigger.speakAtStartScript = speakAtStartScript;
        }
        else
        {
            dialogueTrigger.dialogue = new Dialogue(npcName, npc);
        }

        spawnActions(dialogueTrigger);
    }

    public virtual void spawnActions(DialogueTrigger mainTrigger)
    {
        List<GameObject> listOfExtraSpaces = new List<GameObject>();

        int index = 0;
        foreach (Vector3Int extraSpace in extraSpaces)
        {
            GameObject extraSpaceGameObject = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.npcExtraSpace), getParent());

            extraSpaceGameObject.name = npcName + extraSpaceNameSuffix + " #" + (index + 1);

            DialogueTriggerLink linkTrigger = extraSpaceGameObject.GetComponent<DialogueTriggerLink>();

            linkTrigger.linkedDialogue = mainTrigger;

            extraSpaceGameObject.transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(extraSpace);

            Helpers.updateColliderPosition(extraSpaceGameObject);

            SpawnInfoManager.addGameObject(extraSpaceGameObject);

            listOfExtraSpaces.Add(extraSpaceGameObject);

            index++;
        }

        mainTrigger.extraSpaces = listOfExtraSpaces.ToArray();
    }
}

public class GateSpawnDetails : NPCSpawnDetails
{
    private Sprite sprite;
    private bool skewed;

    public GateSpawnDetails(string npcName, Vector3Int cellCoords, string currentArea, string spriteName, string tutorialTargetHash, bool skewed) :
    base(npcName, cellCoords, currentArea)
    {
        this.sprite = Helpers.loadSpriteFromResources(spriteName);
        this.tutorialTargetHash = tutorialTargetHash;
        this.skewed = skewed;
    }

    public override Transform getParent()
    {
        if (skewed)
        {
            return base.getParent();
        } else
        {
            return null;
        }
    }

    public override void spawnActions(GameObject gateGameObject)
    {
        base.spawnActions(gateGameObject);

        Gate gate = gateGameObject.AddComponent<Gate>();
        gate.setKey(npcName);
        gate.spriteRenderer.sprite = sprite;

        if (hasTutorialTargetHash())
        {
            addTutorialTargetComponent(gateGameObject, gate.spriteRenderer, tutorialTargetHash);
        }
    }
}

public class ShopkeeperSpawnDetails : NPCSpawnDetails
{

    public ShopkeeperSpawnDetails(string npcName, Vector3Int cellCoords, string areaName) :
    base(npcName, cellCoords, areaName)
    {

    }

    public ShopkeeperSpawnDetails(string npcName, Vector3Int cellCoords, string areaName, Vector3Int[] extraSpaces) :
    base(npcName, cellCoords, areaName, extraSpaces)
    {

    }

    public override bool interactable()
    {
        return true;
    }

    public override void spawnActions(GameObject npc)
    {
        base.spawnActions(npc);

        Shopkeeper shopkeeper = npc.AddComponent<Shopkeeper>();

        shopkeeper.shopkeeperInventoryKey = npcName;
    }
}

public class SecretDoorSpawnDetails : NPCSpawnDetails
{
    private SecretDoorInfo secretDoorInfo;
    private string spriteNamePath;

    public SecretDoorSpawnDetails(string npcName, Vector3Int cellCoords, string areaName, SecretDoorInfo secretDoorInfo, string tutorialTargetHash, string spriteNamePath) :
    base(npcName, cellCoords, areaName)
    {
        this.secretDoorInfo = secretDoorInfo;
        this.spriteNamePath = spriteNamePath;

        this.tutorialTargetHash = tutorialTargetHash;
    }
    
    public override bool interactable()
    {
        return true;
    }

    public override string getSpriteName()
    {
        Sprite sprite = Helpers.loadSpriteFromResources(spriteNamePath);

        if(sprite != null)
        {
            return spriteNamePath;
        } else
        {
            return PrefabNames.defaultNPCSprite;
        }        
    }

    public override string getPrefabName()
    {
        return PrefabNames.secretDoor;
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParent();
    }

    public override void spawnActions(GameObject secretDoor)
    {
        if(secretDoorInfo.hasBeenDiscovered())
        {
            GameObject.DestroyImmediate(secretDoor);
        }

        base.spawnActions(secretDoor);

        ObservableObject observableObject = secretDoor.GetComponent<ObservableObject>();

        observableObject.secretDoorKey = secretDoorInfo.secretDoorKey;

        if(hasTutorialTargetHash())
        {
            SpriteRenderer spriteRenderer = secretDoor.GetComponent<SpriteRenderer>();
            addTutorialTargetComponent(secretDoor, spriteRenderer, tutorialTargetHash);
        }
    }

    public override void spawnActions(DialogueTrigger dialogueTrigger)
    {
        Dialogue dialogue = dialogueTrigger.dialogue;

        dialogue.variableSources.Add(secretDoorInfo);
    }
}

public class VaultableObjectSpawnDetails : NPCSpawnDetails
{

    public VaultableObject vaultableObject;

    public VaultableObjectSpawnDetails(string npcName, Vector3Int cellCoords, VaultableObject vaultableObject) :
    base(npcName, cellCoords)
    {
        this.vaultableObject = vaultableObject;
    }

    public VaultableObjectSpawnDetails(string npcName, Vector3Int cellCoords, VaultableObject vaultableObject, string tutorialTargetHash) :
    base(npcName, cellCoords)
    {
        this.vaultableObject = vaultableObject;
        this.tutorialTargetHash = tutorialTargetHash;
    }

    public override Dialogue getDialogue(string areaName)
    {
        return DialogueList.getVaultableObjectDialogue();
    }

    public override bool interactable()
    {
        return true;
    }

    public override string getSpriteName()
    {
        switch (vaultableObject.objectName)
        {
            case VaultableObject.barrelName:
                return PrefabNames.vaultableBarrels;
            default:
                return null;
        }
    }

    public override string getPrefabName()
    {
        return PrefabNames.vaultableObject;
    }

    public override void spawnActions(GameObject gameObject)
    {
        base.spawnActions(gameObject);

        if (hasTutorialTargetHash())
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            addTutorialTargetComponent(gameObject, spriteRenderer, tutorialTargetHash);
        }
    }

    public override void spawnActions(DialogueTrigger dialogueTrigger)
    {
        dialogueTrigger.dialogue.variableSources.Add(vaultableObject);
    }

}

public class VaultableRubbleSpawnDetails : VaultableObjectSpawnDetails
{

    public VaultableRubbleSpawnDetails(string npcName, Vector3Int cellCoords, int vaultDistance) :
    base(npcName, cellCoords, new VaultableObject(vaultDistance, VaultableObject.isPlural, VaultableObject.rockName))
    {

    }

    public override string getSpriteName()
    {
        return PrefabNames.vaultableRocks;
    }

    public override void spawnActions(GameObject gameObject)
    {
        base.spawnActions(gameObject);

        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if(spriteRenderer != null)
        {
            spriteRenderer.color = ColorList.getRubbleColorFromLocationName();
        }
    }
}

public class ChestSpawnDetails : OOCSpawnDetails
{
    private int index;
    private Facing facing;


    public ChestSpawnDetails(int index, Vector3Int cellCoords, Facing facing) :
    base(generateName(index), cellCoords)
    {
        this.index = index;
        this.facing = facing;
    }

    public override string getPrefabName()
    {
        return PrefabNames.chest;
    }

    private static string generateName(int index)
    {
        return NPCNameList.chest + "-" + index;
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParent();
    }

    public override bool determineSpriteAtSpawn()
    {
        return false;
    }

    public virtual ChestType getType()
    {
        return ChestType.Chest;
    }

    public override void spawnActions(GameObject chestGameObject)
    {
        Chest chest = chestGameObject.GetComponent<Chest>();

        chest.populate(index, facing, getType());
    }
}


public class ShelfSpawnDetails : ChestSpawnDetails
{

    public ShelfSpawnDetails(int index, Vector3Int cellCoords, Facing facing) :
    base(index, cellCoords, facing)
    {

    }

    public override ChestType getType()
    {
        return ChestType.Shelf;
    }
}

public class HiddenTerrainSpawnDetails : OOCSpawnDetails
{

    public string secretDoorFlag;
    private string areaName;
    private string sectionName;

    private string locationName;

    private int index;

    public HiddenTerrainSpawnDetails(string secretDoorFlag, string locationName, int index) :
    base()
    {
        this.secretDoorFlag = secretDoorFlag;

        this.locationName = locationName;

        this.index = index;

        this.areaName = null;
        this.sectionName = null;
    }

    public HiddenTerrainSpawnDetails(string secretDoorFlag, string areaName, string sectionName, int index) :
    base()
    {
        this.secretDoorFlag = secretDoorFlag;

        this.areaName = areaName;
        this.sectionName = sectionName;

        this.index = index;
        
        this.locationName = null;
    }

    public override bool spawnsOnSecretDoorActivation()
    {
        return true;
    }

    public override string getPrefabName()
    {
        if (locationName == null)
        {
            return HiddenTerrainList.getHiddenTerrainFolderPath(areaName, sectionName, index);
        }
        else
        {
            return HiddenTerrainList.getHiddenTerrainFolderPath(locationName, index);
        }
    }

    public override SpawnParams getSpawnParams()
    {
        return new HiddenTerrainSpawnParams(secretDoorFlag);
    }

    public override bool determineSpriteAtSpawn()
    {
        return false;
    }

    public override Transform getParent()
    {
        return AreaManager.getGridParent();
    }

    public override void spawnActions(GameObject interactable)
    {
        interactable.transform.localPosition = Vector3.zero;

        Helpers.updateColliderPosition(interactable);
    }

}