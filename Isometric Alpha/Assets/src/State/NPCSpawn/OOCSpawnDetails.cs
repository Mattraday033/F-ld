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
    protected string spriteName;
    protected Color tint = Color.white;
    private bool flipX = false;
    protected SortingLayerInfo sortingLayerInfo;

    public OOCSpawnDetails()
    {
        this.cellCoords = Vector3Int.zero;
        this.spriteName = null;
    }

    public OOCSpawnDetails(Vector3Int cellCoords)
    {
        this.cellCoords = cellCoords;
        this.spriteName = null;
    }

    public OOCSpawnDetails(string npcName, Vector3Int cellCoords)
    {
        this.npcName = npcName;
        this.cellCoords = cellCoords;
        this.spriteName = null;
    }

    public OOCSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName)
    {
        this.npcName = npcName;
        this.cellCoords = cellCoords;
        this.spriteName = spriteName;
    }

    public OOCSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName, SortingLayerInfo sortingLayerInfo)
    {
        this.npcName = npcName;
        this.cellCoords = cellCoords;
        this.spriteName = spriteName;
        this.sortingLayerInfo = sortingLayerInfo;
    }

    public OOCSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName, bool flipX)
    {
        this.npcName = npcName;
        this.cellCoords = cellCoords;
        this.spriteName = spriteName;
        this.flipX = flipX;
    }


    public virtual string getSpriteName()
    {
        if (spriteName == null)
        {
            return PrefabNames.defaultNPCSprite;
        }
        else
        {
            return spriteName;
        }
    }

    public virtual string getPrefabName()
    {
        return null;
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
        spriteRenderer.flipX = flipX;
        
        spriteRenderer.color = tint;

        if(sortingLayerInfo != null)
        {
            sortingLayerInfo.setSpriteRendererSortingLayer(spriteRenderer);
        }
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
    public int index;

    public CunningObjectSpawnDetails(int index, Vector3Int cellCoords, Facing facing, CunningObjectSpriteCategory category) :
    base(category.ToString(), cellCoords, null)
    {
        this.index = index;

        this.startFacing = facing;
        this.endFacing = facing;

        this.category = category;
    }

    public CunningObjectSpawnDetails(int index, Vector3Int cellCoords, Facing startFacing, Facing endFacing, CunningObjectSpriteCategory category) :
    base(category.ToString(), cellCoords, null)
    {
        this.index = index;

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

    private List<ObstacleSpawnDetails> allBlockerSpawnDetails;

    public CunningBlockerSpawnDetails(int index, Vector3Int cellCoords, Facing facing, CunningObjectSpriteCategory category, List<ObstacleSpawnDetails> allBlockerSpawnDetails) :
    base(index, cellCoords, facing, category)
    {
        this.allBlockerSpawnDetails = allBlockerSpawnDetails;
    }

    public CunningBlockerSpawnDetails(int index, Vector3Int cellCoords, Facing startFacing, Facing endFacing, CunningObjectSpriteCategory category, List<ObstacleSpawnDetails> allBlockerSpawnDetails) :
    base(index, cellCoords, startFacing, endFacing, category)
    {
        this.allBlockerSpawnDetails = allBlockerSpawnDetails;
    }

    public CunningBlockerSpawnDetails(int index, Vector3Int cellCoords, Facing facing, CunningObjectSpriteCategory category, List<ObstacleSpawnDetails> allBlockerSpawnDetails, string tutorialTargetHash) :
    base(index, cellCoords, facing, category)
    {
        this.allBlockerSpawnDetails = allBlockerSpawnDetails;
        this.tutorialTargetHash = tutorialTargetHash;
    }

    public CunningBlockerSpawnDetails(int index, Vector3Int cellCoords, Facing startFacing, Facing endFacing, CunningObjectSpriteCategory category, ObstacleSpawnDetails blockerSpawnDetails, string tutorialTargetHash) :
    base(index, cellCoords, startFacing, endFacing, category)
    {
        this.allBlockerSpawnDetails = new List<ObstacleSpawnDetails>(); 
        allBlockerSpawnDetails.Add(blockerSpawnDetails);
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

        cunningBlocker.index = index;

        cunningBlocker.build(startFacing,endFacing, category);

        buildBlockers(cunningBlocker, allBlockerSpawnDetails);

        if (hasTutorialTargetHash())
        {
            addTutorialTargetComponent(cunningBlocker.gameObject, cunningBlocker.spriteRenderer, tutorialTargetHash);
        }
    }

    protected static void buildBlockers(CunningBlocker cunningBlocker, List<ObstacleSpawnDetails> blockerSpawnDetails)
    {

        foreach (ObstacleSpawnDetails details in blockerSpawnDetails)
        {
            GameObject blocker = SpawnInfoManager.spawnInteractable(details);
            cunningBlocker.addBlocker(blocker.GetComponent<Obstacle>(), details.cellCoords);
            SpawnInfoManager.addGameObject(blocker);
        }

        cunningBlocker.setBlockerStatus();
    }
}

public class LinkedCunningBlockerSpawnDetails : CunningBlockerSpawnDetails
{

    private int linkedIndex;

    public LinkedCunningBlockerSpawnDetails(int index, Vector3Int cellCoords, Facing startFacing, Facing endFacing, CunningObjectSpriteCategory category, List<ObstacleSpawnDetails> allBlockerSpawnDetails, int linkedIndex) :
    base(index, cellCoords, startFacing, endFacing, category, allBlockerSpawnDetails)
    {
        this.linkedIndex = linkedIndex;
    }

    public override void spawnActions(CunningObject cunningObject)
    {
        GameObject gameObject = cunningObject.gameObject;

        LinkedCunningBlocker linkedBlocker = gameObject.AddComponent<LinkedCunningBlocker>();
        linkedBlocker.spriteRenderer = cunningObject.spriteRenderer;
        linkedBlocker.linkedIndex = linkedIndex;

        GameObject.Destroy(cunningObject);

        base.spawnActions(linkedBlocker);
    }
}

public class DoubleCunningBlockerSpawnDetails : CunningBlockerSpawnDetails
{

    private List<ObstacleSpawnDetails> deactivatedBlockerSpawnDetails;

    public DoubleCunningBlockerSpawnDetails(int index, Vector3Int cellCoords, Facing startFacing, Facing endFacing, CunningObjectSpriteCategory category, List<ObstacleSpawnDetails> activatedBlockerSpawnDetails, List<ObstacleSpawnDetails> deactivatedBlockerSpawnDetails) :
    base(index, cellCoords, startFacing, endFacing, category, activatedBlockerSpawnDetails)
    {
        this.deactivatedBlockerSpawnDetails = deactivatedBlockerSpawnDetails;
    }

    public override void spawnActions(CunningObject cunningObject)
    {
        GameObject gameObject = cunningObject.gameObject;

        DoubleCunningBlocker doubleBlocker = gameObject.AddComponent<DoubleCunningBlocker>();
        doubleBlocker.spriteRenderer = cunningObject.spriteRenderer;

        GameObject.Destroy(cunningObject);

        base.spawnActions(doubleBlocker);

        buildBlockers(doubleBlocker, deactivatedBlockerSpawnDetails);
    }
}

public class ObstacleSpawnDetails : OOCSpawnDetails
{

    public ObstacleSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName) :
    base(npcName, cellCoords, spriteName)
    {
    }

    public ObstacleSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName, SortingLayerInfo sortingLayerInfo) :
    base(npcName, cellCoords, spriteName, sortingLayerInfo)
    {
    }

    public ObstacleSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName, Color tint) :
    base(npcName, cellCoords, spriteName)
    {
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
        if (spriteRenderer == null)
        {
            return;
        }

        spriteRenderer.sprite = Helpers.loadSpriteFromResources(getSpriteName());
        spriteRenderer.color = tint;

        if(sortingLayerInfo != null)
        {
            sortingLayerInfo.setSpriteRendererSortingLayer(spriteRenderer);
        }
    }

}

public class ObstacleWithSecretDoorFlagSpawnDetails : ObstacleSpawnDetails
{

    private string secretDoorFlag;

    public ObstacleWithSecretDoorFlagSpawnDetails(Vector3Int cellCoords, string secretDoorFlag) :
    base(NPCNameList.unseenBarrier, cellCoords, null) 
    {
        this.secretDoorFlag = secretDoorFlag;
    }

    public ObstacleWithSecretDoorFlagSpawnDetails(string npcName, Vector3Int cellCoords, string secretDoorFlag) :
    base(npcName, cellCoords, null)
    {
        this.secretDoorFlag = secretDoorFlag;
    }

    public ObstacleWithSecretDoorFlagSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName, string secretDoorFlag) :
    base(npcName, cellCoords, spriteName)
    {
        this.secretDoorFlag = secretDoorFlag;
    }

    public ObstacleWithSecretDoorFlagSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName, SortingLayerInfo sortingLayerInfo, string secretDoorFlag) :
    base(npcName, cellCoords, spriteName, sortingLayerInfo)
    {
        this.secretDoorFlag = secretDoorFlag;
    }

    public override string getPrefabName()
    {
        return PrefabNames.oocObstacle;
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParent();
    }

    public override void spawnActions(GameObject interactable)
    {
        GameObject.Destroy(interactable.GetComponent<Obstacle>());

        ObstacleWithSecretDoorFlag obstacle = interactable.AddComponent<ObstacleWithSecretDoorFlag>();

        obstacle.setObstacleName(npcName);
        obstacle.secretDoorFlag = secretDoorFlag;

        spawnActions(interactable.GetComponent<SpriteRenderer>());
    }

    public override void spawnActions(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null)
        {
            return;
        }

        if(getSpriteName() == null)
        {
            spriteRenderer.sprite = null;
        }
        else
        {
            base.spawnActions(spriteRenderer);
        }
    }

}

public class SpikeSpawnDetails : ObstacleSpawnDetails
{

    public SpikeSpawnDetails(Vector3Int cellCoords) :
    base(NPCNameList.spike, cellCoords, PrefabNames.spikesDown, Color.white)
    {
    }

    public SpikeSpawnDetails(Vector3Int cellCoords, Color tint) :
    base(NPCNameList.spike, cellCoords, PrefabNames.spikesDown, tint)
    {
        
    }

    public override string getPrefabName()
    {
        return PrefabNames.spikes;
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParent();
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
    private int weight;

    public ButtonSpawnDetails(Vector3Int cellCoords) :
    base(NPCNameList.floorButton, cellCoords)
    {
        this.index = 0;
        this.weight = 1;
    }

    public ButtonSpawnDetails(Vector3Int cellCoords, int index) :
    base(NPCNameList.floorButton, cellCoords)
    {
        this.index = index;
        this.weight = 1;
    }

    public ButtonSpawnDetails(int weight, Vector3Int cellCoords) :
    base(NPCNameList.floorButton, cellCoords)
    {
        this.index = 0;
        this.weight = weight;
    }

    public ButtonSpawnDetails(int weight, Vector3Int cellCoords, int index) :
    base(NPCNameList.floorButton, cellCoords)
    {
        this.index = index;
        this.weight = weight;
    }

    public ButtonSpawnDetails(Vector3Int cellCoords, string tutorialTargetHash) :
    base(NPCNameList.floorButton, cellCoords)
    {
        this.tutorialTargetHash = tutorialTargetHash;
        this.index = 0;
        this.weight = 1;
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
        floorButton.weight = weight;
    }

}

public class NPCSpawnDetails : OffSetSpawnDetails
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

    public NPCSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName, SortingLayerInfo sortingLayerInfo) :
    base(npcName, cellCoords, spriteName, sortingLayerInfo, Constants.onTableHeightOffset)
    {
        this.activated = true;
        this.dialogue = getDialogue(npcName);
    }

    public NPCSpawnDetails(string npcName, Vector3Int cellCoords, string areaName, string spriteName) :
    base(npcName, cellCoords, spriteName)
    {
        this.activated = true;
        this.dialogue = getDialogue(areaName);
    }

    public NPCSpawnDetails(string npcName, Vector3Int cellCoords, string areaName, string spriteName, bool flipX, float offset) :
    base(npcName, cellCoords, spriteName, flipX, offset)
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

        if(dialogueTrigger == null)
        {
            return;
        }

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
    private bool skewed;

    public GateSpawnDetails(string npcName, Vector3Int cellCoords, string currentArea, string spriteName, string tutorialTargetHash, bool skewed) :
    base(npcName, cellCoords, currentArea, spriteName)
    {
        this.tutorialTargetHash = tutorialTargetHash;
        this.skewed = skewed;
    }

    public override Transform getParent()
    {
        if (skewed)
        {
            return base.getParent();
        }
        else
        {
            return null;
        }
    }

    public virtual Gate addGate(GameObject gateGameObject)
    {
        return gateGameObject.AddComponent<Gate>();
    }

    public override void spawnActions(GameObject gateGameObject)
    {
        base.spawnActions(gateGameObject);

        Gate gate = addGate(gateGameObject);
        gate.setKey(npcName);

        if (hasTutorialTargetHash())
        {
            addTutorialTargetComponent(gateGameObject, gate.spriteRenderer, tutorialTargetHash);
        }
    }

    public override void spawnActions(DialogueTrigger dialogueTrigger)
    {
        base.spawnActions(dialogueTrigger);

        Dialogue dialogue = dialogueTrigger.dialogue;

        dialogue.cameraFoci[Constants.indexOne] = dialogueTrigger.gameObject;
    }
}

public class TemporaryGateSpawnDetails : GateSpawnDetails
{
    public TemporaryGateSpawnDetails(string npcName, Vector3Int cellCoords, string currentArea, string spriteName, string tutorialTargetHash, bool skewed) :
    base(npcName, cellCoords, currentArea, spriteName, tutorialTargetHash, skewed)
    {

    }

    public override Dialogue getDialogue(string areaName)
    {
        return DialogueList.getDialogue(DialogueList.scrubNameOfEndNumbers(npcName), areaName);
    }

    public override Gate addGate(GameObject gateGameObject)
    {
        return gateGameObject.AddComponent<TemporaryGate>();
    }

}

public class GateWithHiddenTerrainSpawnDetails : GateSpawnDetails
{
    private string hiddenTerrainFlag;

    public GateWithHiddenTerrainSpawnDetails(string npcName, Vector3Int cellCoords, string currentArea, string spriteName, string tutorialTargetHash, bool skewed, string hiddenTerrainFlag, Color tint) :
    base(npcName, cellCoords, currentArea, spriteName, tutorialTargetHash, skewed)
    {
        this.hiddenTerrainFlag = hiddenTerrainFlag;
        this.tint = tint;
    }

    public override Dialogue getDialogue(string areaName)
    {
        return DialogueList.getDialogue(DialogueList.scrubNameOfEndNumbers(npcName), areaName);
    }

    public override Gate addGate(GameObject gateGameObject)
    {
        GateWithHiddenTerrain gate = gateGameObject.AddComponent<GateWithHiddenTerrain>();
        gate.hiddenTerrainFlag = hiddenTerrainFlag;

        return gate;
    }

    public override void spawnActions(DialogueTrigger dialogueTrigger)
    {
        base.spawnActions(dialogueTrigger);



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

    public SecretDoorSpawnDetails(string npcName, Vector3Int cellCoords, string areaName, SecretDoorInfo secretDoorInfo, string tutorialTargetHash, string spriteName) :
    base(npcName, cellCoords, areaName, spriteName)
    {
        this.secretDoorInfo = secretDoorInfo;

        this.tutorialTargetHash = tutorialTargetHash;
    }
    
    public override bool interactable()
    {
        return true;
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
    base(npcName, cellCoords, npcName)
    {
        this.vaultableObject = vaultableObject;
    }

    public VaultableObjectSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName, SortingLayerInfo sortingLayerInfo, VaultableObject vaultableObject) :
    base(npcName, cellCoords, spriteName, sortingLayerInfo)
    {
        this.vaultableObject = vaultableObject;
    }

    public VaultableObjectSpawnDetails(string npcName, Vector3Int cellCoords, VaultableObject vaultableObject, string tutorialTargetHash) :
    base(npcName, cellCoords, npcName)
    {
        this.vaultableObject = vaultableObject;
        this.tutorialTargetHash = tutorialTargetHash;
    }

    public override Dialogue getDialogue(string npcName)
    {
        return DialogueList.getVaultableObjectDialogue(npcName);
    }

    public override bool interactable()
    {
        return true;
    }

    public override string getSpriteName()
    {
        if(spriteName != null)
        {
            return spriteName;
        }

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


public class OffSetSpawnDetails : OOCSpawnDetails
{
    protected float offset = 0f;

    public OffSetSpawnDetails(string npcName, Vector3Int cellCoords) :
    base(npcName, cellCoords)
    {

    }

    public OffSetSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName) :
    base(npcName, cellCoords, spriteName)
    {

    }

    public OffSetSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName, SortingLayerInfo sortingLayerInfo, float offset) :
    base(npcName, cellCoords, spriteName, sortingLayerInfo)
    {
        this.offset = offset;
    }

    public OffSetSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName, bool flipX) :
    base(npcName, cellCoords, spriteName, flipX)
    {

    }

    public OffSetSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName, bool flipX, float offset) :
    base(npcName, cellCoords, spriteName, flipX)
    {
        this.offset = offset;
    }

    public override void spawnActions(GameObject interactable)
    {
        base.spawnActions(interactable);

        Vector3 currentPosition = interactable.transform.position;

        currentPosition.y -= offset;
        Collider2D collider2D = interactable.GetComponent<Collider2D>();
        collider2D.offset += new Vector2(0f, offset);

        interactable.transform.position = currentPosition;
    }
}

public class BookSpawnDetails : OffSetSpawnDetails
{

    private int bookIndex;

    public BookSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName, int bookIndex) :
    base(npcName, cellCoords, spriteName)
    {
        this.bookIndex = bookIndex;
        offset = Constants.onTableHeightOffset;
    }

    public override string getPrefabName()
    {
        return PrefabNames.book;
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParent();
    }

    public override void spawnActions(GameObject interactable)
    {
        base.spawnActions(interactable);

        WorldBookInfo bookInfo = interactable.GetComponent<WorldBookInfo>();

        bookInfo.bookIndex = bookIndex;
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