using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class OOCSpawnDetails
{

    public const string gameObjectNameSuffix = "'s GameObject";
    private const string gameObjectPlaceHolderName = "PlaceHolder GameObject";
    protected const string noTutorialTargetHash = Constants.emptyString;

    public string tutorialTargetHash = "";

    protected bool useRubbleColor;

    public string npcName;
    public Vector3Int cellCoords;
    protected string spriteName;
    protected Color tint = Color.white;
    private bool flipX = false;
    protected SortingLayerInfo sortingLayerInfo;

    public OOCSpawnDetails( string npcName = "", 
                            Vector3Int cellCoords = new Vector3Int(), 
                            string spriteName = null, 
                            SortingLayerInfo sortingLayerInfo = null, 
                            bool flipX = false, 
                            string tutorialTargetHash = "")
    {
        this.npcName = npcName;
        this.cellCoords = cellCoords;
        this.spriteName = spriteName;
        this.sortingLayerInfo = sortingLayerInfo;
        this.flipX = flipX;

        if(tutorialTargetHash == null)
        {
            this.tutorialTargetHash = "";
        } else
        {
            this.tutorialTargetHash = tutorialTargetHash;
        }
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

    public virtual bool flipSprite()
    {
        switch(spriteName)
        {
            default:
                return flipX;
        }
    }

    public virtual string getPrefabName()
    {
        return null;
    }

    public virtual Transform getParent()
    {
        return AreaManager.getNPCParentWithoutScale();
    }

    public virtual bool spawnsOnSecretDoorActivation()
    {
        return false;
    }

    public virtual SpawnParams getSpawnParams()
    {
        return SpawnParamsList.getSpawnParams(AreaManager.locationName, npcName);
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
        if(flipSprite())
        {
            interactable.transform.localScale = Constants.flippedXScale;
        }

        SpriteRenderer spriteRenderer = interactable.GetComponent<SpriteRenderer>();

        spawnActions(spriteRenderer);
    }

    public virtual void spawnActions(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null)
        {
            return;
        }

        spriteRenderer.sprite = Helpers.loadSpriteFromResources(getSpriteName());

        if(useRubbleColor)
        {
            spriteRenderer.color = ColorList.getRubbleColorFromLocationName();
        } else
        {
            spriteRenderer.color = tint;
        }

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

    public static void addTutorialTargetComponent(GameObject gameObject, SpriteRenderer spriteRenderer, string tutorialTargetHash, IRevealable revealable = null)
    {
        if(gameObject == null)
        {
            return;
        }

        if(gameObject.GetComponent<RectTransform>() == null)
        {
            gameObject.AddComponent<RectTransform>();
        }

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
        targetSprite.revealable = revealable;
    }

    public static void addTutorialTargetComponent(ITutorialSequenceTarget target, string tutorialTargetHash)
    {
        target.setTutorialHash(tutorialTargetHash);
        target.getGameObject().AddComponent<RectTransform>();
    }

    protected static void addNameTagGenerator(GameObject targetObject, INameSource nameSource, bool cunningTarget = false)
    {
        NameTagGenerator nameTagGenerator;
        
        if(cunningTarget) 
        {
            nameTagGenerator = targetObject.GetComponent<CunningNameTagGenerator>();
        } else
        {
            nameTagGenerator = targetObject.GetComponent<NameTagGenerator>();
        }

        nameTagGenerator.nameSource = nameSource;
    }

    // public static void setMouseHoverTileMap(string spriteName, Transform transform)
    // {
    //     foreach(Transform child in transform)
    //     {
    //         if(child.GetComponent<NPCMouseHover>() != null)
    //         {
    //             GameObject.Destroy(child.gameObject);
    //         }
    //     }

    //     Tilemap npcMouseHover = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.mouseHoverTileMap), transform).GetComponent<Tilemap>();

    //     Tile tile = ScriptableObject.CreateInstance<Tile>();

    //     tile.sprite = Resources.Load<Sprite>(spriteName);

    //     npcMouseHover.SetTile(new Vector3Int(-1, -1), tile);
    // }
}

public interface IQuestActivationObject
{
    public void setScript(QuestStepActivationScript script);
}
public abstract class QuestActivationObjectSpawnDetails : OOCSpawnDetails
{

    protected QuestStepActivationScript script;

    public QuestActivationObjectSpawnDetails(string npcName, Vector3Int cellCoords, QuestStepActivationScript script = null) :
    base(npcName, cellCoords)
    {
        this.script = script;
    }

    protected abstract void setScript(IQuestActivationObject questActivationObject);
}

public class TutorialColliderSpawnDetails : OOCSpawnDetails
{

    public string tutorialKey;
    public string seenFlagName;
    public StartSpawningAllTrueFlagList startSpawningFlagList;
    public int monsterDefeatKeyIndex;
    public bool alwaysSpawn;

    public TutorialColliderSpawnDetails(Vector3Int cellCoords, string tutorialKey, string seenFlagName, bool alwaysSpawn = false) :
    base(cellCoords: cellCoords)
    {
        this.tutorialKey = tutorialKey;
        this.seenFlagName = seenFlagName;
        this.startSpawningFlagList = new StartSpawningAllTrueFlagList();
        this.monsterDefeatKeyIndex = -1;
        this.alwaysSpawn = alwaysSpawn;
    }

    public TutorialColliderSpawnDetails(Vector3Int cellCoords, string tutorialKey, string seenFlagName, StartSpawningAllTrueFlagList startSpawningFlagList, bool alwaysSpawn = false) :
    base(cellCoords: cellCoords)
    {
        this.tutorialKey = tutorialKey;
        this.seenFlagName = seenFlagName;
        this.startSpawningFlagList = startSpawningFlagList;
        this.monsterDefeatKeyIndex = -1;
        this.alwaysSpawn = alwaysSpawn;
    }

    public TutorialColliderSpawnDetails(Vector3Int cellCoords, string tutorialKey, string seenFlagName, StartSpawningAllTrueFlagList startSpawningFlagList, int monsterDefeatKeyIndex, bool alwaysSpawn = false) :
    base(cellCoords: cellCoords)
    {
        this.tutorialKey = tutorialKey;
        this.seenFlagName = seenFlagName;
        this.startSpawningFlagList = startSpawningFlagList;
        this.monsterDefeatKeyIndex = monsterDefeatKeyIndex;
        this.alwaysSpawn = alwaysSpawn;
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
        if(alwaysSpawn)
        {
            return false;
        }

        return TutorialFlags.getFlag(seenFlagName) ||
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

    public CunningObjectSpawnDetails(int index, Vector3Int cellCoords, Facing startFacing, CunningObjectSpriteCategory category, Facing endFacing = Facing.Random, string tutorialTargetHash = null) :
    base(category.ToString(), cellCoords, tutorialTargetHash: tutorialTargetHash)
    {
        this.index = index;

        this.startFacing = startFacing;

        if(endFacing == Facing.Random)
        {
            this.endFacing = startFacing;
        } 
        else
        {
            this.endFacing = endFacing;
        }

        this.category = category;
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParentWithoutScale();
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

    private QuestStepActivationScript script;
    private List<ObstacleSpawnDetails> allBlockerSpawnDetails;

    public CunningBlockerSpawnDetails(int index, Vector3Int cellCoords, Facing startFacing, CunningObjectSpriteCategory category, List<ObstacleSpawnDetails> allBlockerSpawnDetails = null, ObstacleSpawnDetails blockerSpawnDetails = null, Facing endFacing = Facing.Random, QuestStepActivationScript script = null, string tutorialTargetHash = null) :
    base(index, cellCoords, startFacing, category, endFacing: endFacing, tutorialTargetHash: tutorialTargetHash)
    {
        if(allBlockerSpawnDetails == null)
        {
            this.allBlockerSpawnDetails = new List<ObstacleSpawnDetails>();
        } else
        {
            this.allBlockerSpawnDetails = allBlockerSpawnDetails;
        }

        if(blockerSpawnDetails != null)
        {
            this.allBlockerSpawnDetails.Add(blockerSpawnDetails);
        } 

        this.script = script;
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
        cunningBlocker.script = script;

        addNameTagGenerator(cunningBlocker.gameObject, cunningBlocker, cunningTarget: true);

        List<GameObject> blockers = buildBlockers(cunningBlocker, allBlockerSpawnDetails);

        foreach(GameObject blocker in blockers)
        {
            blocker.SetActive(getSpawnParams().canSpawn(npcName));
        }

        if (hasTutorialTargetHash())
        {
            addTutorialTargetComponent(cunningBlocker.gameObject, cunningBlocker.spriteRenderer, tutorialTargetHash, cunningBlocker);
        }
    }

    protected static List<GameObject> buildBlockers(CunningBlocker cunningBlocker, List<ObstacleSpawnDetails> blockerSpawnDetails)
    {
        List<GameObject> blockers = new List<GameObject>();            

        foreach (ObstacleSpawnDetails details in blockerSpawnDetails)
        {
            GameObject blocker = SpawnInfoManager.spawnInteractable(details);
            cunningBlocker.addBlocker(blocker.GetComponent<Obstacle>(), details.cellCoords);
            SpawnInfoManager.addGameObject(blocker);
            blockers.Add(blocker);
        }

        cunningBlocker.setBlockerStatus();

        return blockers;
    }
}

public class LinkedCunningBlockerSpawnDetails : CunningBlockerSpawnDetails
{

    private int linkedIndex;

    public LinkedCunningBlockerSpawnDetails(int index, Vector3Int cellCoords, Facing startFacing, Facing endFacing, CunningObjectSpriteCategory category, List<ObstacleSpawnDetails> allBlockerSpawnDetails, int linkedIndex, string tutorialTargetHash = null) :
    base(index, cellCoords, startFacing, category, allBlockerSpawnDetails, endFacing: endFacing, tutorialTargetHash: tutorialTargetHash)
    {
        this.linkedIndex = linkedIndex;
    }

    public override void spawnActions(CunningObject cunningObject)
    {
        GameObject gameObject = cunningObject.gameObject;

        LinkedCunningBlocker linkedBlocker = gameObject.AddComponent<LinkedCunningBlocker>();
        linkedBlocker.spriteRenderer = cunningObject.spriteRenderer;
        linkedBlocker.linkedIndex = linkedIndex;

        addNameTagGenerator(gameObject, linkedBlocker, cunningTarget: true);

        GameObject.Destroy(cunningObject);

        base.spawnActions(linkedBlocker);
    }
}

public class DoubleCunningBlockerSpawnDetails : CunningBlockerSpawnDetails
{

    private List<ObstacleSpawnDetails> deactivatedBlockerSpawnDetails;

    public DoubleCunningBlockerSpawnDetails(int index, Vector3Int cellCoords, Facing startFacing, Facing endFacing, CunningObjectSpriteCategory category, List<ObstacleSpawnDetails> activatedBlockerSpawnDetails, List<ObstacleSpawnDetails> deactivatedBlockerSpawnDetails) :
    base(index, cellCoords, startFacing, category, activatedBlockerSpawnDetails, endFacing: endFacing)
    {
        this.deactivatedBlockerSpawnDetails = deactivatedBlockerSpawnDetails;
    }

    public override void spawnActions(CunningObject cunningObject)
    {
        GameObject gameObject = cunningObject.gameObject;

        DoubleCunningBlocker doubleBlocker = gameObject.AddComponent<DoubleCunningBlocker>();
        doubleBlocker.spriteRenderer = cunningObject.spriteRenderer;

        addNameTagGenerator(gameObject, doubleBlocker, cunningTarget: true);

        GameObject.Destroy(cunningObject);

        base.spawnActions(doubleBlocker);

        buildBlockers(doubleBlocker, deactivatedBlockerSpawnDetails);
    }
}

public class ObstacleSpawnDetails : OffSetSpawnDetails
{

    private bool withScale;

    public ObstacleSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName, SortingLayerInfo sortingLayerInfo = null, float offset = 0f, bool flipX = false, bool withScale = true, bool ignoresSecretDoors = true) :
    base(npcName, cellCoords, spriteName, sortingLayerInfo, offset, flipX, ignoresSecretDoors: ignoresSecretDoors)
    {
        this.tint = Color.white;
        this.withScale = withScale;
    }

    public ObstacleSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName, Color tint, bool ignoresSecretDoors = true) :
    base(npcName, cellCoords, spriteName, ignoresSecretDoors: ignoresSecretDoors)
    {
        useRubbleColor = false;
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
        if(withScale)
        {
            return AreaManager.getNPCParentWithScale();
        }
        else
        {
            return AreaManager.getNPCParentWithoutScale();
        }
    }

    public override void spawnActions(GameObject interactable)
    {
        base.spawnActions(interactable);

        Obstacle obstacle = interactable.GetComponent<Obstacle>();
        obstacle.setObstacleName(npcName);

        spawnActions(interactable.GetComponent<SpriteRenderer>());

        if(flipSprite())
        {
            interactable.transform.localScale = Constants.flippedXScale;
        }
    }

    public override void spawnActions(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null)
        {
            return;
        }

        base.spawnActions(spriteRenderer);

        spriteRenderer.sprite = Helpers.loadSpriteFromResources(getSpriteName());

        if(sortingLayerInfo != null)
        {
            sortingLayerInfo.setSpriteRendererSortingLayer(spriteRenderer);
        }
    }

    protected override void setIgnoresSecretDoors(GameObject interactable)
    {
        Obstacle obstacle = interactable.GetComponent<Obstacle>();

        if(obstacle != null && ignoresSecretDoors)
        {
            obstacle.setToIgnoreSecretDoors();
        }
    }

}

public class DeadBodySpawnDetails : ObstacleSpawnDetails
{

    private Facing facing;

    public DeadBodySpawnDetails(string npcName, Vector3Int cellCoords, string spriteName, Facing facing = Facing.NorthEast, SortingLayerInfo sortingLayerInfo = null, float offset = 0f, bool flipX = false, bool withScale = false, bool ignoresSecretDoors = true) :
    base(npcName, cellCoords, spriteName, sortingLayerInfo, offset, flipX, withScale: withScale, ignoresSecretDoors: ignoresSecretDoors)
    {
        this.facing = facing;
    }

    public Sprite getSprite()
    {
        string path = EnemyTypeFolderPathList.getEnemyTypeFolderPath(spriteName);
        string sheetName = "";

        switch(facing)
        {
            case Facing.SouthEast:
            case Facing.SouthWest:
                sheetName = CharacterAnimationType.Death_Front.ToString();
                break;
            default:
                sheetName = CharacterAnimationType.Death_Back.ToString();
                break;
        }

        Sprite[] sprites = Resources.LoadAll<Sprite>(path+sheetName);

        if(sprites == null || sprites.Length <= 0)
        {
            sprites = Resources.LoadAll<Sprite>(path+CharacterAnimationType.Death.ToString());
        }

        return sprites[sprites.Length - 1];
    }

    public override void spawnActions(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null)
        {
            return;
        }

        if(sortingLayerInfo != null) 
        {
            sortingLayerInfo.setSpriteRendererSortingLayer(spriteRenderer);
        }

        spriteRenderer.sprite = getSprite();
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

    private void setOffset(Transform transform)
    {
        switch(spriteName)
        {
            case PrefabNames.water:
                transform.position = new Vector3(transform.position.x, transform.position.y - Constants.onTableHeightOffset*2);
                break;
            default:
                break;
        }
    }

    public override string getPrefabName()
    {
        return PrefabNames.oocObstacle;
    }

    public override SpawnParams getSpawnParams()
    {
        return new SecretDoorObstacleSpawnParams(secretDoorFlag);
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParentWithScale();
    }

    public override void spawnActions(GameObject interactable)
    {
        GameObject.Destroy(interactable.GetComponent<Obstacle>());

        ObstacleWithSecretDoorFlag obstacle = interactable.AddComponent<ObstacleWithSecretDoorFlag>();

        obstacle.setObstacleName(npcName);
        obstacle.secretDoorFlag = secretDoorFlag;

        setOffset(interactable.transform);

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
        return AreaManager.getNPCParentWithScale();
    }

}

public class RubbleObstacleSpawnDetails : ObstacleSpawnDetails
{
    public RubbleObstacleSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName) :
    base(npcName, cellCoords, spriteName)
    {
        useRubbleColor = true;
    }
}

public class ButtonSpawnDetails : OffSetSpawnDetails
{

    private int index;
    private int weight;
    private int charismaRequirement;

    public ButtonSpawnDetails(Vector3Int cellCoords, int index = 0, int weight = 1, int charismaRequirement = 1, string tutorialTargetHash = null) :
    base(NPCNameList.button, cellCoords, offset: Constants.onTableHeightOffset*-3)
    {
        this.index = index;
        this.weight = weight;
        this.charismaRequirement = charismaRequirement;
        this.tutorialTargetHash = tutorialTargetHash;
    }

    public override string getPrefabName()
    {
        return PrefabNames.floorButton;
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParentWithScale();
    }

    public override void spawnActions(GameObject button)
    {
        base.spawnActions(button);

        if (hasTutorialTargetHash())
        {
            SpriteRenderer spriteRenderer = button.GetComponent<SpriteRenderer>();

            addTutorialTargetComponent(button, spriteRenderer, tutorialTargetHash);
        }

        spawnActions(button.GetComponent<FloorButton>());
    }

    public virtual void spawnActions(FloorButton floorButton)
    {
        floorButton.index = index;
        floorButton.weight = weight;
        floorButton.charismaRequirement = charismaRequirement;

        floorButton.setSprite(Constants.indexZero);

        floorButton.transform.position = new Vector3(floorButton.transform.position.x, floorButton.transform.position.y, 0f);

        NPCMouseHover mouseHover = floorButton.transform.GetComponentInChildren<NPCMouseHover>();
        Helpers.updatePolygonCollider(mouseHover.spriteRenderer, mouseHover.polygonCollider2D);
    }

}

public class HiddenButtonSpawnDetails : ButtonSpawnDetails
{
    private string secretDoorFlag;

    public HiddenButtonSpawnDetails(Vector3Int cellCoords, string secretDoorFlag, int index = 0) :
    base(cellCoords, index: index)
    {
        this.secretDoorFlag = secretDoorFlag;
    }

    public override void spawnActions(GameObject button)
    {
        base.spawnActions(button);

        if(!SecretDoorFlags.secretDoorHasBeenDiscovered(secretDoorFlag))
        {
            button.SetActive(false);
        }
    }

    public override void spawnActions(FloorButton floorButton)
    {
        base.spawnActions(floorButton);
        floorButton.secretDoorFlag = secretDoorFlag;

    }
}



public class NPCSpawnDetails : OffSetSpawnDetails
{

    public const string extraSpaceNameSuffix = "'s Extra Space GameObject";

    public Vector3Int[] extraSpaces = new Vector3Int[0];
    public Dialogue dialogue;
    public SpeakAtStartScript speakAtStartScript;

    public bool sleepingDialogueIntro;

    public NPCSpawnDetails( string npcName, 
                            Vector3Int cellCoords, 
                            string areaName = null, 
                            string spriteName = null, 
                            SortingLayerInfo sortingLayerInfo = null, 
                            float offset = 0f, 
                            bool flipX = false,
                            Vector3Int[] extraSpaces = null,
                            SpeakAtStartScript speakAtStartScript = null,
                            string tutorialTargetHash = "",
                            bool ignoresSecretDoors = true,
                            bool sleepingDialogueIntro = false) :
    base(npcName, cellCoords, spriteName, sortingLayerInfo, offset, flipX, ignoresSecretDoors: ignoresSecretDoors, tutorialTargetHash: tutorialTargetHash)
    {
        if(areaName == null)
        {
            this.dialogue = getDialogue();
        } else
        {
            this.dialogue = getDialogue(areaName);
        }

        if(sortingLayerInfo != null && offset == 0f)
        {
            this.offset = Constants.onTableHeightOffset;
        }

        if(extraSpaces != null)
        {
            this.extraSpaces = extraSpaces;
        }

        this.speakAtStartScript = speakAtStartScript;
        this.sleepingDialogueIntro = sleepingDialogueIntro;
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
        return AreaManager.getNPCParentWithScale();
    }

    public PlaySFXLogic getDialogueIntroSFXLogic()
    {
        return AudioClipList.getDialogueIntroSFXLogic(npcName, sleepingDialogueIntro);
    }

    public override void spawnActions(GameObject npc)
    {
        base.spawnActions(npc);

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

        mainTrigger.introAudioClipLogic = getDialogueIntroSFXLogic();

        mainTrigger.extraSpaces = listOfExtraSpaces.ToArray();
    }
}

public class NPCWithAnimationsSpawnDetails : NPCSpawnDetails
{

    private string animationName;
    protected Facing facing;
    private CharacterAnimationType animationType;

    public NPCWithAnimationsSpawnDetails(string npcName,
                                         Vector3Int cellCoords, 
                                         string areaName = "", 
                                         string animationName = null,
                                         Facing facing = Facing.Random,
                                         Vector3Int[] extraSpaces = null,
                                         SpeakAtStartScript speakAtStartScript = null,
                                         CharacterAnimationType animationType = CharacterAnimationType.None, 
                                         bool ignoresSecretDoors = true,
                                         float offset = 0f,
                                         bool sleepingDialogueIntro = false) :
    base(npcName, cellCoords, areaName, extraSpaces: extraSpaces, speakAtStartScript: speakAtStartScript, ignoresSecretDoors: ignoresSecretDoors, offset: offset, sleepingDialogueIntro: sleepingDialogueIntro) 
    {
        if(animationName == null)
        {
            this.animationName = npcName;
        } else
        {
            this.animationName = animationName;
        }

        this.facing = facing;
        this.animationType = animationType;
    }

    public override Transform getParent()
    {
        string npcNameScrubbed = DialogueList.scrubNameOfEndNumbers(npcName);

        if(npcNameScrubbed.Equals(NPCNameList.barricade))
        {
            return AreaManager.getNPCParentWithScale();
        }

        return AreaManager.getNPCParentWithoutScale();
    }

    public override void spawnActions(GameObject npc)
    {
        base.spawnActions(npc);

        spawnActions(npc.GetComponent<AnimationManager>());

        if(PartyMemberList.characterIsPartyMember(npcName))
        {
            PartyMemberDespawnListener listener = npc.AddComponent<PartyMemberDespawnListener>();

            listener.partyMemberName = npcName;
        }

        // npc.transform.localScale = Constants.antiAngleAdjustmentScale;
    }

    public virtual void spawnActions(AnimationManager animationManager)
    {
        if(animationName == null)
        {
            return;
        }

        animationManager.setAnimations(animationName);
        animationManager.setFacing(facing);

        if(animationType != CharacterAnimationType.None)
        {
            string characterToAnimate = "";

            if(animationName != null)
            {
                characterToAnimate = animationName;
            } else
            {
                characterToAnimate = npcName;
            }

            animationManager.setCurrentIdle(AnimationManager.getFallBackIdleType(characterToAnimate, animationType));
        }
    }
}

public class HorseSpawnDetails : NPCWithAnimationsSpawnDetails
{

    public HorseSpawnDetails(string npcName,
                                         Vector3Int cellCoords, 
                                         Facing facing,
                                         string areaName = "", 
                                         string animationName = null,
                                         Vector3Int[] extraSpaces = null,
                                         SpeakAtStartScript speakAtStartScript = null,
                                         CharacterAnimationType animationType = CharacterAnimationType.None, 
                                         bool ignoresSecretDoors = true,
                                         float offset = 0f) :
    base(npcName, cellCoords, areaName, extraSpaces: extraSpaces, speakAtStartScript: speakAtStartScript, ignoresSecretDoors: ignoresSecretDoors, offset: offset, animationName: animationName, facing: facing, animationType: animationType) 
    {
        List<Vector3Int> extraCoords = new List<Vector3Int>();

        if(extraSpaces != null)
        {
            extraCoords.AddRange(extraSpaces);
        }

        extraCoords.Add(getExtraSpace());

        this.extraSpaces = extraCoords.ToArray();
    }

    private Vector3Int getExtraSpace()
    {
        switch(facing)
        {
            case Facing.NorthEast:
                return cellCoords + MovementManager.distance1TileSouthWestGrid;
            case Facing.NorthWest:
                return cellCoords + MovementManager.distance1TileSouthEastGrid;
            case Facing.SouthEast:
                return cellCoords + MovementManager.distance1TileNorthWestGrid;
            default:
                return cellCoords + MovementManager.distance1TileNorthEastGrid;
        }
    }

    // public override void spawnActions(GameObject npc)
    // {
    //     base.spawnActions(npc);

    //     spawnActions(npc.GetComponent<AnimationManager>());

    //     if(PartyMemberList.characterIsPartyMember(npcName))
    //     {
    //         PartyMemberDespawnListener listener = npc.AddComponent<PartyMemberDespawnListener>();

    //         listener.partyMemberName = npcName;
    //     }

    //     // npc.transform.localScale = Constants.antiAngleAdjustmentScale;
    // }

    public override void spawnActions(AnimationManager animationManager)
    {
        base.spawnActions(animationManager);
        
        if(animationManager != null)
        {
            animationManager.changesFacing = false;
            // animationManager.disableExtras();
        }
    }
}

public class NonDialogueNPCSpawnDetails : NPCWithAnimationsSpawnDetails
{

    public NonDialogueNPCSpawnDetails(string npcName, 
                                        Vector3Int cellCoords, 
                                        string animationName = null,
                                        Facing facing = Facing.Random, 
                                        bool ignoresSecretDoors = true,
                                         CharacterAnimationType animationType = CharacterAnimationType.None) :
    base(npcName, cellCoords, "", animationName, facing, ignoresSecretDoors: ignoresSecretDoors, animationType: animationType)
    {

    }

    public override Dialogue getDialogue(string areaName)
    {
        return null;
    }

    public override SpawnParams getSpawnParams()
    {
        InteractableSpawnParams spawnParams = SpawnParamsList.getSpawnParams(AreaManager.locationName, npcName);

        if(spawnParams.startSpawningFlagList.flags.Length == 0 && 
            spawnParams.stopSpawningFlagList.flags.Length == 0)
        {
            return new NeverSpawnParams();
        } else
        {
            return spawnParams;
        }
    }
}

public class DependantSpawnDetails : NPCWithAnimationsSpawnDetails
{

    private string parentName;
    private Transform parent;
    private bool normalScale;

    public DependantSpawnDetails(string npcName, 
                                    Vector3Int cellCoords, 
                                    string areaName, 
                                    string parentName, 
                                    Facing facing = Facing.Random, 
                                    bool normalScale = false,
                                    CharacterAnimationType animationType = CharacterAnimationType.None) :
    base(npcName, cellCoords, areaName, facing: facing, animationType: animationType)
    {
        this.parentName = parentName;
        this.normalScale = normalScale;
    }

    public override Transform getParent()
    {
        return parent;
    }

    public override void spawnActions(GameObject npc)
    {
        GameObject parentObject = DialogueManager.findNPCGameObject(parentName);

        if(parentObject != null)
        {
            parent = parentObject.AddComponent<RectTransform>();
        }

        // Vector3 worldPos = npc.transform.position;

        npc.AddComponent<RectTransform>();

        npc.transform.SetParent(getParent());

        if(normalScale)
        {
            npc.transform.localScale = Vector3.one;
        } else
        {
            npc.transform.localScale = Constants.scaleChange;
        }

        // npc.transform.position = worldPos;

        base.spawnActions(npc);
    }

}



public class NPCOffGridSpawnDetails : NPCSpawnDetails
{

    public NPCOffGridSpawnDetails(string npcName, Vector3Int cellCoords, string areaName, string spriteName) :
    base(npcName, cellCoords, areaName, spriteName)
    {
    }

    public NPCOffGridSpawnDetails(string npcName, Vector3Int cellCoords, string areaName, string spriteName, bool flipX, float offset) :
    base(npcName, cellCoords, areaName, spriteName, flipX: flipX, offset :offset)
    {
    }

    public NPCOffGridSpawnDetails(string npcName, Vector3Int cellCoords, string areaName, string spriteName, Vector3Int[] extraSpaces) :
    base(npcName, cellCoords, areaName, spriteName, extraSpaces: extraSpaces)
    {
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParentWithoutScale();
    }
}

public class CustomMouseHoverNPCSpawnDetails : NPCSpawnDetails
{

    public CustomMouseHoverNPCSpawnDetails(string npcName, Vector3Int cellCoords, string currentArea, string spriteName):
    base(npcName, cellCoords, currentArea, spriteName)
    {
        
    }

    public CustomMouseHoverNPCSpawnDetails(string npcName, Vector3Int cellCoords, string areaName, string spriteName, bool flipX, float offset) :
    base(npcName, cellCoords, areaName, spriteName, flipX: flipX, offset: offset)
    {
        
    }

    public virtual bool hasSprite()
    {
        return true;
    }

    public void setUpMouseHover(GameObject gameObject)
    {

        // foreach(Transform child in gameObject.transform)
        // {
        //     GameObject.Destroy(child.gameObject);
        // }

        if(!hasSprite())
        {
            NameTagGenerator nameTagGenerator = gameObject.GetComponent<NameTagGenerator>();

            if(nameTagGenerator != null)
            {
                GameObject.Destroy(nameTagGenerator);
            }
        }
    }

    public override void spawnActions(GameObject gameObject)
    {
        base.spawnActions(gameObject);

        setUpMouseHover(gameObject);
    }

}

public class GateSpawnDetails : CustomMouseHoverNPCSpawnDetails
{
    // private bool skewed;
    private bool showSprite;
    private Axis axis;
    private Dictionary<string, int> statDifficulties;

    public GateSpawnDetails(string npcName, 
                            Vector3Int cellCoords, 
                            string currentArea, 
                            string spriteName, 
                            string tutorialTargetHash, 
                            // bool skewed, 
                            bool showSprite, 
                            Axis axis, 
                            Dictionary<string, int> statDifficulties, 
                            bool useRubbleColor) :
    base(npcName, cellCoords, currentArea, spriteName)
    {
        this.tutorialTargetHash = tutorialTargetHash;
        // this.skewed = skewed;
        this.showSprite = showSprite;
        this.axis = axis;
        this.statDifficulties = statDifficulties;
        this.useRubbleColor = useRubbleColor;

        switch(spriteName)
        {
            case PrefabNames.portcullis3x1Path:
                offset = Constants.onTableHeightOffset*5;
                break;
            default:
                break;
        }
    }

    public override Transform getParent()
    {
        // if (skewed)
        // {
            return AreaManager.getNPCParentWithScale();
        // }
        // else
        // {
        //     return AreaManager.getNPCParentWithoutScale();
        // }
    }

    public override bool hasSprite()
    {
        return showSprite;
    }

    public override bool flipSprite()
    {
        switch(spriteName)
        {
            case PrefabNames.portcullis1x1Path:
            case PrefabNames.portcullis2x1Path:
            case PrefabNames.portcullis3x1Path:
                return axis == Axis.DescendingX;
            default:
                return false;
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

        AnimationManager animationManager = gateGameObject.GetComponent<AnimationManager>();

        if(animationManager != null)
        {
            animationManager.disableExtras();
        }

        if(!hasSprite())
        {
            gate.spriteRenderer.enabled = false;

            foreach(Transform child in gateGameObject.transform)
            {
                Collider2D childCollider = child.GetComponent<Collider2D>();
                if(childCollider != null)
                {
                    childCollider.enabled = false;
                }
            }

        } else if(flipSprite())
        {
            gateGameObject.transform.localScale = Constants.flippedXScale;
        }
    }

    public override void spawnActions(DialogueTrigger dialogueTrigger)
    {
        base.spawnActions(dialogueTrigger);

        Dialogue dialogue = dialogueTrigger.dialogue;

        dialogue.cameraFoci[Constants.indexOne] = dialogueTrigger.gameObject;

        dialogue.variableSources.Add(new StoryStatRequirementVariableSource(statDifficulties));
    }
}

public class GateWithKeySpawnDetails : GateSpawnDetails
{
    private GateKeyDetails gateKeyDetails;

    public GateWithKeySpawnDetails(string npcName, Vector3Int cellCoords, string currentArea, string spriteName,// bool skewed,
     bool showSprite, Axis axis, GateKeyDetails gateKeyDetails) :
    base(npcName, cellCoords, currentArea, spriteName, noTutorialTargetHash, //skewed, 
    showSprite, axis, new Dictionary<string, int>(), useRubbleColor: false)
    {
        this.gateKeyDetails = gateKeyDetails;
    }

    public override Dialogue getDialogue(string areaName)
    {
        return new SingleCharacterDialogue(npcName, Resources.Load<TextAsset>(DialogueNameList.gateWithKeyPath));
    }

    public override void spawnActions(DialogueTrigger dialogueTrigger)
    {
        base.spawnActions(dialogueTrigger);

        dialogueTrigger.dialogue.variableSources.Add(gateKeyDetails);
    }
}

public class TemporaryGateSpawnDetails : GateSpawnDetails
{
    public TemporaryGateSpawnDetails(string npcName, Vector3Int cellCoords, string currentArea, string spriteName, string tutorialTargetHash, //bool skewed,
     Axis axis, Dictionary<string, int> statDifficulties) :
    base(npcName, cellCoords, currentArea, spriteName, tutorialTargetHash, //skewed,
     true, axis, statDifficulties, useRubbleColor: false)
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

    public GateWithHiddenTerrainSpawnDetails(string npcName, Vector3Int cellCoords, string currentArea, string spriteName, string tutorialTargetHash, //bool skewed, 
    Dictionary<string, int> statDifficulties, string hiddenTerrainFlag, Color tint) :
    base(npcName, cellCoords, currentArea, spriteName, tutorialTargetHash, //skewed, 
    true, Axis.DescendingX, statDifficulties, useRubbleColor: false)
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
        NameTagGenerator nameTagGenerator = gateGameObject.GetComponent<NameTagGenerator>();

        nameTagGenerator.nameSource = gate;

        return gate;
    }
}

public class ShopkeeperSpawnDetails : NPCWithAnimationsSpawnDetails
{

    public ShopkeeperSpawnDetails(string npcName, Vector3Int cellCoords, string areaName, string animationName = null, Vector3Int[] extraSpaces = null, bool ignoresSecretDoors = true, Facing facing = Facing.Random) :
    base(npcName, cellCoords, areaName, animationName: animationName, extraSpaces: extraSpaces, ignoresSecretDoors: ignoresSecretDoors, facing: facing)
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
    private string terrainSpriteName;
    private ObservableDelegate observable;
    private QuestStepActivationScript script;

    public SecretDoorSpawnDetails(string npcName, Vector3Int cellCoords, string areaName, SecretDoorInfo secretDoorInfo, string tutorialTargetHash, string spriteName, string terrainSpriteName, ObservableDelegate observable = null, QuestStepActivationScript script = null) :
    base(npcName, cellCoords, areaName, spriteName)
    {
        this.secretDoorInfo = secretDoorInfo;

        this.tutorialTargetHash = tutorialTargetHash;
        this.terrainSpriteName = terrainSpriteName;

        dialogue = getDialogue(areaName);
    
        this.observable = observable;
        this.script = script;
    }

    public override Dialogue getDialogue(string areaName)
    {
        if(secretDoorInfo != null && secretDoorInfo.customDialoguePath != null)
        {
            return new Dialogue(new string[] { Constants.emptyString, NPCNameList.suspiciousWall }, Resources.Load<TextAsset>(secretDoorInfo.customDialoguePath));
        }

        return new Dialogue(new string[] { Constants.emptyString, NPCNameList.suspiciousWall }, Resources.Load<TextAsset>(DialogueNameList.suspiciousWallPath));
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
        return AreaManager.getNPCParentWithScale();
    }

    public override void spawnActions(GameObject secretDoor)
    {
        if(secretDoorInfo.hasBeenDiscovered())
        {
            GameObject.DestroyImmediate(secretDoor);
        }

        base.spawnActions(secretDoor);

        ObservableObject observableObject = secretDoor.GetComponent<ObservableObject>();

        observableObject.secretDoorKeys = secretDoorInfo.secretDoorKeys;

        if(terrainSpriteName != null && terrainSpriteName.Length > 0)
        {
            observableObject.terrainSprite = Helpers.loadSpriteFromResources(terrainSpriteName);
        }

        if(hasTutorialTargetHash())
        {
            SpriteRenderer spriteRenderer = secretDoor.GetComponent<SpriteRenderer>();
            addTutorialTargetComponent(secretDoor, spriteRenderer, tutorialTargetHash);
        }

        if(observable == null || observable())
        {
            secretDoor.layer = LayerAndTagManager.observableLayer;
        } else
        {
            secretDoor.layer = LayerAndTagManager.objectLayer;
        }

        observableObject.script = script;
    }

    public override void spawnActions(DialogueTrigger dialogueTrigger)
    {
        Dialogue dialogue = dialogueTrigger.dialogue;

        dialogue.variableSources.Add(secretDoorInfo);

        dialogueTrigger.introAudioClipLogic = getDialogueIntroSFXLogic();
    }
}

public class LadderSpawnDetails : NPCSpawnDetails
{
    public const float offsetY = .1f;
    public const bool doNotFlipX = false;

    public Ladder ladder;

    public LadderSpawnDetails(Vector3Int cellCoords, string spriteName, Ladder ladder, bool flipX = doNotFlipX, float offset = offsetY) :
    base(NPCNameList.ladder, cellCoords, Constants.emptyString, spriteName, flipX: flipX, offset: offset)
    {
        this.ladder = ladder;
    }

    public override Dialogue getDialogue(string areaName)
    {
        return Ladder.getDialogue();
    }

    public override void spawnActions(GameObject npc)
    {
        base.spawnActions(npc);

        AnimationManager animationManager = npc.GetComponent<AnimationManager>();

        if(animationManager != null)
        {
            animationManager.disableExtras();
        }
    }

    public override void spawnActions(DialogueTrigger mainTrigger)
    {
        base.spawnActions(mainTrigger);

        mainTrigger.dialogue.variableSources.Add(ladder);

        SpawnInfoManager.spawnTransitionSpace(ladder.locationName, ladder.destinationName, cellCoords, ladder.facing);
    }
}

public class VaultableObjectSpawnDetails : NPCSpawnDetails
{

    public VaultableObject vaultableObject;

    public VaultableObjectSpawnDetails(string npcName, 
                                        Vector3Int cellCoords, 
                                        VaultableObject vaultableObject, 
                                        string spriteName = null, 
                                        float offset = 0f, 
                                        SortingLayerInfo sortingLayerInfo = null, 
                                        string tutorialTargetHash = "") :
    base(npcName, 
         cellCoords, 
         spriteName: spriteName, 
         offset: offset, 
         sortingLayerInfo: sortingLayerInfo, 
         tutorialTargetHash: tutorialTargetHash)
    {
        this.vaultableObject = vaultableObject;
        this.dialogue = getDialogue(npcName);

        this.tutorialTargetHash = tutorialTargetHash;
    }

    public override Dialogue getDialogue(string npcName)
    {
        if(vaultableObject == null)
        {
            return null;
        }

        return vaultableObject.getDialogue(npcName);
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
            case NPCNameList.barricade:
                return PrefabNames.destroyableBarricade;
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

        // setMouseHoverTileMap(getSpriteName(), gameObject.transform);
    }

    public override void spawnActions(DialogueTrigger dialogueTrigger)
    {
        dialogueTrigger.dialogue.variableSources.Add(vaultableObject);
        
        dialogueTrigger.introAudioClipLogic = getDialogueIntroSFXLogic();
    }

}

public class VaultableRubbleSpawnDetails : VaultableObjectSpawnDetails
{

    public VaultableRubbleSpawnDetails(string npcName, Vector3Int cellCoords, int difficulty, int vaultDistance, string spriteName = null) :
    base(npcName, cellCoords, new VaultableObject(difficulty, vaultDistance, VaultableObject.isPlural, VaultableObject.rockName), spriteName: spriteName)
    {
        useRubbleColor = true;
    }

    public override string getSpriteName()
    {
        return PrefabNames.vaultableRocks;
    }
}


public class VaultableOrDestroyableObjectSpawnDetails : VaultableObjectSpawnDetails
{

    public int index;

    public VaultableOrDestroyableObjectSpawnDetails(string npcName, Vector3Int cellCoords, VaultableOrDestroyableObject vaultableOrDestroyableObject, string spriteName = null, int index = 0) :
    base(npcName, cellCoords, vaultableOrDestroyableObject, spriteName: spriteName)
    {
        this.index = index;
    }

    public override void spawnActions(GameObject gameObject)
    {
        base.spawnActions(gameObject);

        Gate gate = gameObject.AddComponent<Gate>();

        gate.setKey(VaultableOrDestroyableObject.gateKey+index);
    }
}

public class ChestSpawnDetails : QuestActivationObjectSpawnDetails
{
    private int index;
    private Facing facing;
    private string secretDoorFlag;

    public ChestSpawnDetails(int index, Vector3Int cellCoords, Facing facing, QuestStepActivationScript script = null, string secretDoorFlag = null) :
    base(generateName(index), cellCoords, script)
    {
        this.index = index;
        this.facing = facing;
        this.secretDoorFlag = secretDoorFlag;
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
        return AreaManager.getNPCParentWithScale();
    }

    public virtual ChestType getType()
    {
        return ChestType.Chest;
    }

    public override void spawnActions(GameObject chestGameObject)
    {
        Chest chest = chestGameObject.GetComponent<Chest>();

        chest.populate(index, facing, getType());

        setScript(chest);

        chest.setSecretDoorFlag(secretDoorFlag);
    }

    protected override void setScript(IQuestActivationObject questActivationObject)
    {
        questActivationObject.setScript(script);
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

public class WeaponRackSpawnDetails : ChestSpawnDetails
{
    private ChestType type;

    public WeaponRackSpawnDetails(int index, Vector3Int cellCoords, Facing facing, ChestType type, QuestStepActivationScript script = null) :
    base(index, cellCoords, facing, script)
    {
        this.type = type;
    }

    public override ChestType getType()
    {
        return type;
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParentWithScale();
    }
}


public class OffSetSpawnDetails : OOCSpawnDetails
{
    protected float offset = 0f;
    protected bool ignoresSecretDoors;

    //npcName, cellCoords, spriteName, sortingLayerInfo, offset, flipX

    public OffSetSpawnDetails(string npcName, 
                              Vector3Int cellCoords, 
                              string spriteName = null, 
                              SortingLayerInfo sortingLayerInfo = null, 
                              float offset = 0f, 
                              bool flipX = false, 
                              bool ignoresSecretDoors = true,
                              string tutorialTargetHash = "") :
    base(npcName, cellCoords, spriteName, sortingLayerInfo, flipX, tutorialTargetHash: tutorialTargetHash)
    {
        this.offset = offset;
        this.ignoresSecretDoors = ignoresSecretDoors;
    }

    public override void spawnActions(GameObject interactable)
    {
        base.spawnActions(interactable);

        Vector3 currentPosition = interactable.transform.position;

        currentPosition.y -= offset;
        Collider2D collider2D = interactable.GetComponent<Collider2D>();
        collider2D.offset += new Vector2(0f, offset);

        interactable.transform.position = currentPosition;

        setIgnoresSecretDoors(interactable);
    }

    protected virtual void setIgnoresSecretDoors(GameObject interactable)
    {
        NameTagGenerator nameTagGenerator = interactable.GetComponent<NameTagGenerator>();

        if(nameTagGenerator != null && ignoresSecretDoors)
        {
            nameTagGenerator.setToIgnoreSecretDoors();
        }
    }
}

public class BookSpawnDetails : OffSetSpawnDetails
{

    private int bookIndex;

    public BookSpawnDetails(string npcName, Vector3Int cellCoords, string spriteName, int bookIndex, float offset = Constants.onTableHeightOffset*2) :
    base(npcName, cellCoords, spriteName)
    {
        this.bookIndex = bookIndex;
        this.offset = offset;
    }

    public override string getPrefabName()
    {
        return PrefabNames.book;
    }

    public override Transform getParent()
    {
        return AreaManager.getNPCParentWithScale();
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


    public List<string> secretDoorKeys = new List<string>();

    protected string areaName;
    protected string sectionName;

    protected string locationName;

    protected int index;

    public HiddenTerrainSpawnDetails(string secretDoorKey = null, List<string> secretDoorKeys = null, string locationName = "", int index = 0) :
    base()
    {
        if(secretDoorKey != null)
        {
            this.secretDoorKeys.Add(secretDoorKey);
        }

        if(secretDoorKeys != null)
        {
            this.secretDoorKeys.AddRange(secretDoorKeys);
        }

        this.locationName = locationName;

        this.index = index;

        this.areaName = null;
        this.sectionName = null;
    }

    public HiddenTerrainSpawnDetails(string secretDoorKey = null, List<string> secretDoorKeys = null, string areaName = "", string sectionName = "", int index = 0) :
    base()
    {

        if(secretDoorKey != null)
        {
            this.secretDoorKeys.Add(secretDoorKey);
        }

        if(secretDoorKeys != null)
        {
            this.secretDoorKeys.AddRange(secretDoorKeys);
        }

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
        return new HiddenTerrainSpawnParams(secretDoorKeys);
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

public class HostilityTerrainSpawnDetails : HiddenTerrainSpawnDetails
{

    private const string hostilitySecretDoorFlagPlaceholder = "Hostility-";

    public HostilityTerrainSpawnDetails(string locationName, int index) :
    base(hostilitySecretDoorFlagPlaceholder+index, locationName: locationName, index: index)
    {
    }

    public override bool spawnsOnSecretDoorActivation()
    {
        return false;
    }
    public override SpawnParams getSpawnParams()
    {
        // HostilitySpawnParams spawnParams = 
        return new HostilitySpawnParams(locationName);

        // if(!spawnParams.canSpawn(locationName))
        // {
        //     return spawnParams;
        // } else
        // {
        //     return null;
        // }
    }

}