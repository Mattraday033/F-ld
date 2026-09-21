using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OOCSpawnDetails: IAppearanceSource
{
    public string tutorialTargetHash = "";
    public string npcName;
    public Vector3Int cellCoords;
    public Vector3Int[] extraSpaces = new Vector3Int[0];
    protected Facing facing;
    protected bool ignoresSecretDoors;
    protected Dictionary<Type, IExtraSpawnBehaviour> aestheticSpawnBehaviours = new();
    protected Dictionary<Type, IExtraSpawnBehaviour> universalSpawnBehaviours = new();
    
    private IAppearance _Appearance;
    public IAppearance appearance { get { return _Appearance; } }

    protected virtual string tag { get { return LayerAndTagManager.untaggedTag; } }
    protected virtual int layer { get { return LayerAndTagManager.defaultLayer; } }

    public virtual string prefabName { get { return PrefabNames.creaturePrefab; } }

    public virtual Transform parent { 
                                        get { 
                                                if(appearance.withScale) 
                                                { 
                                                    return AreaManager.getNPCParentWithScale(); 
                                                } else 
                                                { 
                                                    return AreaManager.getNPCParentWithoutScale(); 
                                                } 
                                            } 
                                    }

                            // ,
                            // List<IExtraSpawnBehaviour> universalSpawnBehaviours = null,
                            // List<IExtraSpawnBehaviour> aestheticSpawnBehaviours = null

    public OOCSpawnDetails( string npcName = "",
                            IAppearance appearance = null,
                            Vector3Int cellCoords = new Vector3Int(),
                            Facing facing = Facing.Random,
                            string tutorialTargetHash = "",
                            bool ignoresSecretDoors = false,
                            Vector3Int[] extraSpaces = null
                            )
    {
        this.npcName = npcName;
        this._Appearance = appearance ?? Costume.getDefaultCostume();
        this.cellCoords = cellCoords;
        this.ignoresSecretDoors = ignoresSecretDoors;
        this.facing = facing;

        if(extraSpaces != null)
        {
            this.extraSpaces = extraSpaces;
        }

        if(tutorialTargetHash == null)
        {
            this.tutorialTargetHash = "";
        } else
        {
            this.tutorialTargetHash = tutorialTargetHash;
        }
        
        // this.universalSpawnBehaviours = universalSpawnBehaviours ?? new List<IExtraSpawnBehaviour>();
        // this.aestheticSpawnBehaviours = aestheticSpawnBehaviours ?? new List<IExtraSpawnBehaviour>();
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

    private GameObject generateBlankPrefab(Vector3Int cell, bool extra = false)
    {
        GameObject blank = GameObject.Instantiate(Resources.Load<GameObject>(prefabName), parent);
        setGameObjectName(blank, extra);

        Transform transform = blank.transform;
        transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(cell);
        Helpers.updateGameObjectPosition(blank);

        return blank;
    }

    public List<GameObject> spawnInteractables()
    {
        GameObject main = generateBlankPrefab(cellCoords);
        List<GameObject> allInteractables = new List<GameObject>() { main };

        foreach(IExtraSpawnBehaviour behaviour in aestheticSpawnBehaviours.Values)
        {
            behaviour.addBehaviour(main);
        }

        appearance.applyAppearance(main.GetComponent<SpriteLayerRendererList>(), updateColors: true);

        foreach(Vector3Int cell in extraSpaces)
        {
            GameObject extraSpace = generateBlankPrefab(cell, extra: true);
            allInteractables.Add(extraSpace);
        }

        // setIgnoresSecretDoors(interactable);

        foreach(GameObject interactable in allInteractables)
        {
            foreach(IExtraSpawnBehaviour behaviour in universalSpawnBehaviours.Values)
            {
                behaviour.addBehaviour(interactable);
            }

            interactable.layer = layer;
            interactable.tag = tag;
        }
        
        Canvas.ForceUpdateCanvases();

        return allInteractables;
    }

    // protected virtual void setIgnoresSecretDoors(GameObject interactable)
    // {
    //     NameTagGenerator nameTagGenerator = interactable.GetComponent<NameTagGenerator>();

    //     if(nameTagGenerator != null && ignoresSecretDoors)
    //     {
    //         nameTagGenerator.setToIgnoreSecretDoors();
    //     }
    // }

    public static void addTutorialTargetComponent(GameObject gameObject, string tutorialTargetHash)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        addTutorialTargetComponent(gameObject, spriteRenderer, tutorialTargetHash);
    }

    public static void addTutorialTargetComponent(GameObject gameObject, SpriteRenderer spriteRenderer, string tutorialTargetHash, IRevealable revealable = null)
    {
        if(gameObject == null || gameObject.transform == null)
        {
            return;
        }

        if(gameObject.GetComponent<RectTransform>() == null)
        {
            gameObject.AddComponent<RectTransform>();
        }

        GameObject template = new GameObject("Target Rect");

        GameObject targetRect = GameObject.Instantiate(template, gameObject.transform);

        GameObject.Destroy(template);

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

    public IAppearance getAppearance()
    {
        return appearance;
    }
    
    public const string gameObjectNameSuffix = "'s GameObject";
    public const string extraSpaceNameSuffix = "'s Extra Space GameObject";
    private const string gameObjectPlaceHolderName = "PlaceHolder GameObject";

    public void setGameObjectName(GameObject gameObject, bool extra = false)
    {
        if (npcName.Length > 0)
        {
            if(extra)
            {
                gameObject.name = npcName + extraSpaceNameSuffix;
            } else
            {
                gameObject.name = npcName + gameObjectNameSuffix;
            }
        }
        else
        {
            gameObject.name = gameObjectPlaceHolderName;
        }
    }


}

public class TutorialColliderSpawnDetails : OOCSpawnDetails
{

    public string tutorialKey;
    public string seenFlagName;
    public StartSpawningAllTrueFlagList startSpawningFlagList;
    public int monsterDefeatKeyIndex;
    public bool alwaysSpawn;

    public TutorialColliderSpawnDetails(Vector3Int cellCoords, string tutorialKey, string seenFlagName, StartSpawningAllTrueFlagList startSpawningFlagList = null, bool alwaysSpawn = false, IAppearance appearance = null) :
    base(appearance: appearance, cellCoords: cellCoords)
    {
        this.tutorialKey = tutorialKey;
        this.seenFlagName = seenFlagName;
        this.startSpawningFlagList = startSpawningFlagList ?? new StartSpawningAllTrueFlagList();
        this.monsterDefeatKeyIndex = -1;
        this.alwaysSpawn = alwaysSpawn;
    }

    // public override string getPrefabName()
    // {
    //     return PrefabNames.tutorialCollider;
    // }

    // public override Transform getParent()
    // {
    //     return AreaManager.getNonTransitionColliderParent();
    // }

    // public override void spawnActions(GameObject tutorialColliderGameObject)
    // {
    //     if (shouldNotSpawn())
    //     {
    //         GameObject.DestroyImmediate(tutorialColliderGameObject);
    //         return;
    //     }

    //     TutorialTriggerCollider tutorialCollider = tutorialColliderGameObject.GetComponent<TutorialTriggerCollider>();
    //     tutorialCollider.tutorialSequenceKey = tutorialKey;
    // }

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

public class NPCSpawnDetails : OOCSpawnDetails
{
    protected override string tag { get { return LayerAndTagManager.npcTag; } }
    protected override int layer { get { return LayerAndTagManager.npcLayer; } }

    public NPCSpawnDetails( string npcName,
                            Vector3Int cellCoords,
                            Facing facing = Facing.Random,
                            Vector3Int[] extraSpaces = null,
                            SpeakAtStartScript speakAtStartScript = null,
                            CharacterAnimationType animationType = CharacterAnimationType.None,
                            string tutorialTargetHash = "",
                            bool ignoresSecretDoors = true,
                            bool sleepingDialogueIntro = false,
                            IAppearance appearance = null) :
    base(npcName, facing: facing, appearance: appearance, cellCoords: cellCoords, tutorialTargetHash: tutorialTargetHash, ignoresSecretDoors: ignoresSecretDoors, extraSpaces: extraSpaces)
    {
        aestheticSpawnBehaviours[typeof(AnimationManagerSpawnBehaviour)] = new AnimationManagerSpawnBehaviour(this, facing, animationType);

        universalSpawnBehaviours[typeof(DialogueTriggerSpawnBehaviour)] = new DialogueTriggerSpawnBehaviour(npcName,
                                                                            AudioClipList.getDialogueIntroSFXLogic(npcName, sleepingDialogueIntro),
                                                                            speakAtStartScript);

        if(PartyMemberList.characterIsPartyMember(npcName))
        {
            universalSpawnBehaviours[typeof(PartyMemberDespawnListenerSpawnBehaviour)] = new PartyMemberDespawnListenerSpawnBehaviour(npcName);
        }
    }
}

public class VaultableObjectSpawnDetails : NPCSpawnDetails
{

    public VaultableObjectSpawnDetails(string npcName,
                                        Vector3Int cellCoords,
                                        VaultableObject vaultableObject,
                                        string tutorialTargetHash = "",
                                        IAppearance appearance = null) :
    base(npcName,
         cellCoords,
         tutorialTargetHash: tutorialTargetHash,
         appearance: appearance)
    {
        DialogueTriggerSpawnBehaviour dialogueTriggerSpawnBehaviour = universalSpawnBehaviours[typeof(DialogueTriggerSpawnBehaviour)] as DialogueTriggerSpawnBehaviour;
        dialogueTriggerSpawnBehaviour.dialogueSource = vaultableObject;

        this.tutorialTargetHash = tutorialTargetHash;
    }

}


public class HorseSpawnDetails : NPCSpawnDetails
{
    public HorseSpawnDetails(string npcName,
                                Vector3Int cellCoords, 
                                Facing facing,
                                Vector3Int[] extraSpaces = null,
                                SpeakAtStartScript speakAtStartScript = null,
                                CharacterAnimationType animationType = CharacterAnimationType.None, 
                                bool ignoresSecretDoors = true,
                                IAppearance appearance = null) :
    base(npcName, cellCoords, facing: facing, extraSpaces: generateRumpExtraSpace(extraSpaces ?? new Vector3Int[0], cellCoords, facing), 
            speakAtStartScript: speakAtStartScript, ignoresSecretDoors: ignoresSecretDoors, animationType: animationType, appearance: appearance)
    {

    }

    private static Vector3Int[] generateRumpExtraSpace(Vector3Int[] extraSpaces, Vector3Int cellCoords, Facing facing)
    {
        Vector3Int rumpSpace;

        switch(facing)
        {
            case Facing.NorthEast:
                rumpSpace = cellCoords + MovementManager.distance1TileSouthWestGrid;
                break;
            case Facing.NorthWest:
                rumpSpace = cellCoords + MovementManager.distance1TileSouthEastGrid;
                break;
            case Facing.SouthEast:
                rumpSpace = cellCoords + MovementManager.distance1TileNorthWestGrid;
                break;
            default:
                rumpSpace = cellCoords + MovementManager.distance1TileNorthEastGrid;
                break;
        }

        List<Vector3Int> finalExtraSpaces = new List<Vector3Int>(extraSpaces);

        finalExtraSpaces.Add(rumpSpace);

        return finalExtraSpaces.ToArray();
    }
}

public interface IQuestActivationObject
{
    public QuestStepActivationScript script
    {
        set;
        get;
    }
}

public class ContainerSpawnDetails : OOCSpawnDetails
{
    private ChestType type;
    protected override int layer { get { return LayerAndTagManager.chestLayer; } }
    public override Transform parent { get {
                                                if(type.withScaleByChestType()) 
                                                { 
                                                    return AreaManager.getNPCParentWithScale(); 
                                                } else 
                                                { 
                                                    return AreaManager.getNPCParentWithoutScale(); 
                                                } 
                                            }
                                     }


    public ContainerSpawnDetails(int index,
                             Vector3Int cellCoords,
                             Facing facing,
                             ChestType type,
                             QuestStepActivationScript script = null,
                             string secretDoorFlag = null,
                             IAppearance appearance = null) :
    base(npcName: generateName(index), facing: facing, appearance: appearance, cellCoords: cellCoords)
    {
        this.type = type;

        aestheticSpawnBehaviours[typeof(AnimationManagerSpawnBehaviour)] = new AnimationManagerSpawnBehaviour(this, facing);
        universalSpawnBehaviours[typeof(ContainerSpawnBehaviour)] = new ContainerSpawnBehaviour(index, script, secretDoorFlag, type);
    }

    public static string generateName(int index)
    {
        return NPCNameList.chest + "-" + index;
    }

    // public bool isSingleSpriteChest()
    // {
    //     return spriteName != null;
    // }

    // public virtual ChestType getType()
    // {
    //     return type;
    // }

    // public override void spawnActions(GameObject chestGameObject)
    // {
    //     if(isSingleSpriteChest())
    //     {
    //         spawnSingleSpriteChest(chestGameObject);
    //         return;
    //     }

    //     Container chest = chestGameObject.GetComponent<Container>();

    //     chest.populate(index, facing, getType());

    //     setScript(chest);

    //     chest.setSecretDoorFlag(secretDoorFlag);
    // }

    // private Sprite getSprite()
    // {
    //     if(deadBody)
    //     {
    //         string folderPath = EnemyTypeFolderPathList.getEnemyTypeFolderPath(spriteName);

    //         if(weaponless)
    //         {
    //             switch(facing)
    //             {
    //                 case Facing.NorthEast:
    //                 case Facing.NorthWest:
    //                     return SpriteUtil.loadSpriteFromResources(folderPath + CharacterAnimationType.Death_Back_Weaponless);
    //                 default:
    //                     return SpriteUtil.loadSpriteFromResources(folderPath + CharacterAnimationType.Death_Front_Weaponless);
    //             }
    //         } else
    //         {
    //             Sprite[] deathSprites = null;

    //             switch(facing)
    //             {
    //                 case Facing.NorthEast:
    //                 case Facing.NorthWest:
    //                     deathSprites = Resources.LoadAll<Sprite>(folderPath + CharacterAnimationType.Death_Back);
    //                     break;
    //                 default:
    //                     deathSprites = Resources.LoadAll<Sprite>(folderPath + CharacterAnimationType.Death_Front);
    //                     break;
    //             }

    //             if(deathSprites == null || deathSprites.Length <= 0)
    //             {
    //                 deathSprites = Resources.LoadAll<Sprite>(folderPath + CharacterAnimationType.Death);
    //             }

    //             return deathSprites[deathSprites.Length - 1];
    //         }
    //     }

    //     return SpriteUtil.loadSpriteFromResources(spriteName);
    // }

    // private void spawnSingleSpriteChest(GameObject chestGameObject)
    // {
    //     Container chest = chestGameObject.GetComponent<Container>();

    //     SingleSpriteChest singleSpriteChest = chestGameObject.AddComponent<SingleSpriteChest>();

    //     // singleSpriteChest.sprite = getSprite();
    //     singleSpriteChest.chestName = chestName;
    //     singleSpriteChest.mouseHoverCollider = chest.mouseHoverCollider;

    //     NameTagGenerator nameTagGenerator = chestGameObject.GetComponent<NameTagGenerator>();

    //     if(nameTagGenerator != null)
    //     {
    //         nameTagGenerator.nameSource = singleSpriteChest;
    //     }

    //     if(chest != null)
    //     {
    //         GameObject.Destroy(chest);
    //     }

    //     singleSpriteChest.populate(index, facing, getType());

    //     setScript(singleSpriteChest);

    //     singleSpriteChest.setSecretDoorFlag(secretDoorFlag);
    // }
}


public abstract class CunningObjectSpawnDetails : OOCSpawnDetails
{
    public Facing startFacing;
    public Facing endFacing;
    public CunningObjectSpriteCategory category;
    public int index;

    public CunningObjectSpawnDetails(int index,
                                     Vector3Int cellCoords, 
                                     Facing startFacing, 
                                     CunningObjectSpriteCategory category, 
                                     Facing endFacing = Facing.Random, 
                                     string tutorialTargetHash = null,
                                     QuestStepActivationScript script = null,
                                     IAppearance appearance = null) :
    base(category.ToString(), appearance: appearance, cellCoords: cellCoords, tutorialTargetHash: tutorialTargetHash)
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

    // public override void spawnActions(GameObject gameObject)
    // {
    //     CunningObject cunningObject = gameObject.GetComponent<CunningObject>();

    //     spawnActions(cunningObject);
    // }

    public abstract void spawnActions(CunningObject cunningObject);

}

public class CunningBlockerSpawnDetails : CunningObjectSpawnDetails
{

    private List<ObstacleSpawnDetails> allBlockerSpawnDetails;

    public CunningBlockerSpawnDetails(int index, 
                                        Vector3Int cellCoords, 
                                        Facing startFacing,
                                        CunningObjectSpriteCategory category, 
                                        List<ObstacleSpawnDetails> allBlockerSpawnDetails = null, 
                                        ObstacleSpawnDetails blockerSpawnDetails = null, 
                                        Facing endFacing = Facing.Random, 
                                        QuestStepActivationScript script = null, 
                                        string tutorialTargetHash = null,
                                        IAppearance appearance = null) :
    base(index, cellCoords, startFacing, category, endFacing: endFacing, tutorialTargetHash: tutorialTargetHash, script: script, appearance: appearance)
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
    }

    // public override string getPrefabName()
    // {
    //     return PrefabNames.cunningBlocker;
    // }

    public override void spawnActions(CunningObject cunningObject)
    {
        CunningBlocker cunningBlocker = cunningObject as CunningBlocker;

        cunningBlocker.index = index;

        cunningBlocker.build(startFacing,endFacing, category);
        // cunningBlocker.script = script;

        addNameTagGenerator(cunningBlocker.gameObject, cunningBlocker, cunningTarget: true);

        List<GameObject> blockers = buildBlockers(cunningBlocker, allBlockerSpawnDetails);

        foreach(GameObject blocker in blockers)
        {
            blocker.SetActive(getSpawnParams().canSpawn(npcName));
        }

        if (hasTutorialTargetHash())
        {
            // addTutorialTargetComponent(cunningBlocker.gameObject, cunningBlocker.spriteRenderer, tutorialTargetHash, cunningBlocker);
        }
    }

    protected static List<GameObject> buildBlockers(CunningBlocker cunningBlocker, List<ObstacleSpawnDetails> blockerSpawnDetails)
    {
        List<GameObject> blockers = new List<GameObject>();            

        // foreach (ObstacleSpawnDetails details in blockerSpawnDetails)
        // {
        //     GameObject blocker = SpawnInfoManager.spawnInteractable(details);
        //     cunningBlocker.addBlocker(blocker.GetComponent<Obstacle>(), details.cellCoords);
        //     SpawnInfoManager.addGameObject(blocker);
        //     blockers.Add(blocker);
        // }

        // cunningBlocker.setBlockerStatus();

        return blockers;
    }
}

public class LinkedCunningBlockerSpawnDetails : CunningBlockerSpawnDetails
{

    private int linkedIndex;

    public LinkedCunningBlockerSpawnDetails(int index, 
                                            Vector3Int cellCoords, 
                                            Facing startFacing, 
                                            Facing endFacing, 
                                            CunningObjectSpriteCategory category, 
                                            List<ObstacleSpawnDetails> allBlockerSpawnDetails, 
                                            int linkedIndex,
                                            string tutorialTargetHash = null,
                                            IAppearance appearance = null) :
    base(index, cellCoords, startFacing, category, allBlockerSpawnDetails, endFacing: endFacing, tutorialTargetHash: tutorialTargetHash, appearance: appearance)
    {
        this.linkedIndex = linkedIndex;
    }

    public override void spawnActions(CunningObject cunningObject)
    {
        GameObject gameObject = cunningObject.gameObject;

        LinkedCunningBlocker linkedBlocker = gameObject.AddComponent<LinkedCunningBlocker>();
        // linkedBlocker.spriteRenderer = cunningObject.spriteRenderer;
        linkedBlocker.linkedIndex = linkedIndex;

        addNameTagGenerator(gameObject, linkedBlocker, cunningTarget: true);

        GameObject.Destroy(cunningObject);

        base.spawnActions(linkedBlocker);
    }
}

public class DoubleCunningBlockerSpawnDetails : CunningBlockerSpawnDetails
{

    private List<ObstacleSpawnDetails> deactivatedBlockerSpawnDetails;

    public DoubleCunningBlockerSpawnDetails(int index, 
                                            Vector3Int cellCoords, 
                                            Facing startFacing, 
                                            Facing endFacing, 
                                            CunningObjectSpriteCategory category, 
                                            List<ObstacleSpawnDetails> activatedBlockerSpawnDetails, 
                                            List<ObstacleSpawnDetails> deactivatedBlockerSpawnDetails,
                                            IAppearance appearance = null) :
    base(index, cellCoords, startFacing, category, activatedBlockerSpawnDetails, endFacing: endFacing, appearance: appearance)
    {
        this.deactivatedBlockerSpawnDetails = deactivatedBlockerSpawnDetails;
    }

    public override void spawnActions(CunningObject cunningObject)
    {
        GameObject gameObject = cunningObject.gameObject;

        DoubleCunningBlocker doubleBlocker = gameObject.AddComponent<DoubleCunningBlocker>();
        // doubleBlocker.spriteRenderer = cunningObject.spriteRenderer;

        addNameTagGenerator(gameObject, doubleBlocker, cunningTarget: true);

        GameObject.Destroy(cunningObject);

        base.spawnActions(doubleBlocker);

        buildBlockers(doubleBlocker, deactivatedBlockerSpawnDetails);
    }
}

public class ObstacleSpawnDetails : OOCSpawnDetails
{

    public ObstacleSpawnDetails(string npcName, 
                                Vector3Int cellCoords,
                                Facing facing = Facing.Random, 
                                bool ignoresSecretDoors = true,
                                IAppearance appearance = null) :
    base(npcName, facing: facing, appearance: appearance, cellCoords: cellCoords, ignoresSecretDoors: ignoresSecretDoors)
    {
        
    }

    // public override string getPrefabName()
    // {
    //     return PrefabNames.oocObstacle;
    // }

    // public override void spawnActions(GameObject interactable)
    // {
    //     base.spawnActions(interactable);

    //     Obstacle obstacle = interactable.GetComponent<Obstacle>();
    //     obstacle.setObstacleName(npcName);

    //     spawnActions(interactable.GetComponent<SpriteRenderer>());
    // }

    // public override void spawnActions(SpriteRenderer spriteRenderer)
    // {
    //     if (spriteRenderer == null)
    //     {
    //         return;
    //     }

    //     base.spawnActions(spriteRenderer);

    //     spriteRenderer.sprite = SpriteUtil.loadSpriteFromResources(getSpriteName());

    //     if(sortingLayerInfo != null)
    //     {
    //         sortingLayerInfo.setRendererSortingLayer(spriteRenderer);
    //     }

    //     spriteRenderer.flipX = flipSprite();
    // }

    // protected override void setIgnoresSecretDoors(GameObject interactable)
    // {
    //     Obstacle obstacle = interactable.GetComponent<Obstacle>();

    //     if(obstacle != null && ignoresSecretDoors)
    //     {
    //         obstacle.setToIgnoreSecretDoors();
    //     }
    // }

}

public class DeadBodySpawnDetails : ObstacleSpawnDetails
{

    private bool weaponless;

    public DeadBodySpawnDetails(string npcName, 
                                Vector3Int cellCoords, 
                                Facing facing = Facing.NorthEast, 
                                bool ignoresSecretDoors = true, 
                                bool weaponless = false,
                                IAppearance appearance = null) :
    base(npcName, cellCoords, facing: facing, ignoresSecretDoors: ignoresSecretDoors, appearance: appearance)
    {
        this.weaponless = weaponless;
    }

    // public Sprite getSprite()
    // {
    //     string path = EnemyTypeFolderPathList.getEnemyTypeFolderPath(spriteName);
    //     string sheetName = "";

    //     switch(facing)
    //     {
    //         case Facing.SouthEast:
    //         case Facing.SouthWest:
    //             sheetName = CharacterAnimationType.Death_Front.ToString();
    //             break;
    //         default:
    //             sheetName = CharacterAnimationType.Death_Back.ToString();
    //             break;
    //     }

    //     if(weaponless)
    //     {
    //         sheetName += "_Weaponless";
    //     }

    //     Sprite[] sprites = Resources.LoadAll<Sprite>(path+sheetName);

    //     if(sprites == null || sprites.Length <= 0)
    //     {
    //         sprites = Resources.LoadAll<Sprite>(path+CharacterAnimationType.Death.ToString());
    //     }

    //     return sprites[sprites.Length - 1];
    // }

    // public override void spawnActions(SpriteRenderer spriteRenderer)
    // {
    //     if (spriteRenderer == null)
    //     {
    //         return;
    //     }

    //     if(sortingLayerInfo != null) 
    //     {
    //         sortingLayerInfo.setRendererSortingLayer(spriteRenderer);
    //     }

    //     spriteRenderer.sprite = getSprite();
    // }
}

public class ObstacleWithSecretDoorFlagSpawnDetails : ObstacleSpawnDetails
{
    protected string secretDoorFlag;

    public ObstacleWithSecretDoorFlagSpawnDetails(  string npcName, 
                                                    Vector3Int cellCoords, 
                                                    string secretDoorFlag = "", 
                                                    IAppearance appearance = null) :
    base(npcName, cellCoords, appearance: appearance)
    {
        this.secretDoorFlag = secretDoorFlag;
    }

    protected void setOffset(Transform transform)
    {
        // switch(spriteName)
        // {
        //     case PrefabNames.water:
        //         transform.position = new Vector3(transform.position.x, transform.position.y - Constants.onTableHeightOffset*2);
        //         break;
        //     case PrefabNames.waterShort:
        //         transform.position = new Vector3(transform.position.x, transform.position.y - Constants.waterShortOffset);
        //         break;
        //     default:
        //         break;
        // }
    }

    // public override string getPrefabName()
    // {
    //     return PrefabNames.oocObstacle;
    // }

    public override SpawnParams getSpawnParams()
    {
        return new SecretDoorObstacleSpawnParams(secretDoorFlag);
    }

    // public override void spawnActions(GameObject interactable)
    // {
    //     GameObject.Destroy(interactable.GetComponent<Obstacle>());

    //     ObstacleWithSecretDoorFlag obstacle = interactable.AddComponent<ObstacleWithSecretDoorFlag>();

    //     obstacle.setObstacleName(npcName);
    //     obstacle.secretDoorFlag = secretDoorFlag;

    //     setOffset(interactable.transform);

    //     spawnActions(interactable.GetComponent<SpriteRenderer>());
    // }

    // public override void spawnActions(SpriteRenderer spriteRenderer)
    // {
    //     if (spriteRenderer == null)
    //     {
    //         return;
    //     }

    //     if(getSpriteName() == null)
    //     {
    //         spriteRenderer.sprite = null;
    //     }
    //     else
    //     {
    //         base.spawnActions(spriteRenderer);
    //     }
    // }

}


public class Wave : ObstacleWithSecretDoorFlagSpawnDetails 
{
    public Wave(string npcName, 
                Vector3Int cellCoords, 
                string secretDoorFlag = "",
                IAppearance appearance = null) :
    base(npcName, cellCoords, secretDoorFlag: secretDoorFlag, appearance: appearance)
    {
    }

    // public override string getPrefabName()
    // {
    //     return PrefabNames.wave;
    // }

    // public override void spawnActions(GameObject interactable)
    // {
    //     ObstacleWithSecretDoorFlag obstacle = interactable.AddComponent<ObstacleWithSecretDoorFlag>();

    //     obstacle.setObstacleName(npcName);
    //     obstacle.secretDoorFlag = secretDoorFlag;

    //     setOffset(interactable.transform);

    //     spawnActions(interactable.GetComponent<Tilemap>());
    //     spawnActions(interactable.GetComponent<TilemapRenderer>());
    // }

    public virtual void spawnActions(Tilemap tilemap)
    {
        // if(tilemap != null && spriteName != null)
        // {
        //     AnimatedTile tile = Resources.Load<AnimatedTile>(spriteName);
        //     tilemap.SetTile(Vector3Int.zero, tile);
        // }
    }
    public virtual void spawnActions(TilemapRenderer tilemapRenderer)
    {
        // if(tilemapRenderer != null && sortingLayerInfo != null)
        // {
        //     sortingLayerInfo.setRendererSortingLayer(tilemapRenderer);
        // }
    }
}

public class SpikeSpawnDetails : ObstacleSpawnDetails
{

    public SpikeSpawnDetails(Vector3Int cellCoords,
                            IAppearance appearance = null) :
    base(NPCNameList.spike, cellCoords, appearance: appearance)
    {
    }

    // public override string getPrefabName()
    // {
    //     return PrefabNames.spikes;
    // }

}

public class RubbleObstacleSpawnDetails : ObstacleSpawnDetails
{
    public RubbleObstacleSpawnDetails(string npcName, 
                                        Vector3Int cellCoords,
                                        IAppearance appearance = null) :
    base(npcName, cellCoords, appearance: appearance)
    {
    }
}

public class ButtonSpawnDetails : OOCSpawnDetails
{

    private int index;
    private int weight;
    private int charismaRequirement;

    public ButtonSpawnDetails(Vector3Int cellCoords, 
                                int index = 0, 
                                int weight = 1, 
                                int charismaRequirement = 1, 
                                string tutorialTargetHash = null,
                                IAppearance appearance = null) :
    base(NPCNameList.button, appearance: appearance, cellCoords: cellCoords, ignoresSecretDoors: true)
    {
        this.index = index;
        this.weight = weight;
        this.charismaRequirement = charismaRequirement;
        this.tutorialTargetHash = tutorialTargetHash;
    }

    // public override string getPrefabName()
    // {
    //     return PrefabNames.floorButton;
    // }

    // public override void spawnActions(GameObject button)
    // {
    //     base.spawnActions(button);

    //     if (hasTutorialTargetHash())
    //     {
    //         SpriteRenderer spriteRenderer = button.GetComponent<SpriteRenderer>();

    //         addTutorialTargetComponent(button, spriteRenderer, tutorialTargetHash);
    //     }

    //     spawnActions(button.GetComponent<FloorButton>());
    // }

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

    public HiddenButtonSpawnDetails(Vector3Int cellCoords, 
                                    string secretDoorFlag, 
                                    int index = 0,
                                    IAppearance appearance = null) :
    base(cellCoords, index: index, appearance: appearance)
    {
        this.secretDoorFlag = secretDoorFlag;
    }

    // public override void spawnActions(GameObject button)
    // {
    //     base.spawnActions(button);

    //     if(!SecretDoorFlags.secretDoorHasBeenDiscovered(secretDoorFlag))
    //     {
    //         button.SetActive(false);
    //     }
    // }

    public override void spawnActions(FloorButton floorButton)
    {
        base.spawnActions(floorButton);
        floorButton.secretDoorFlag = secretDoorFlag;

    }
}

public class NonDialogueNPCSpawnDetails : NPCSpawnDetails
{

    public NonDialogueNPCSpawnDetails(string npcName,
                                        Vector3Int cellCoords,
                                        Facing facing = Facing.Random,
                                        bool ignoresSecretDoors = true,
                                        CharacterAnimationType animationType = CharacterAnimationType.None,
                                        IAppearance appearance = null) :
    base(npcName, cellCoords, facing, ignoresSecretDoors: ignoresSecretDoors, animationType: animationType, appearance: appearance)
    {

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

public class DependantSpawnDetails : NPCSpawnDetails
{

    private string parentName;
    private Transform parent;
    private bool normalScale;

    public DependantSpawnDetails(string npcName,
                                    Vector3Int cellCoords,
                                    string parentName,
                                    Facing facing = Facing.Random,
                                    bool normalScale = false,
                                    CharacterAnimationType animationType = CharacterAnimationType.None,
                                    IAppearance appearance = null) :
    base(npcName, cellCoords, facing: facing, animationType: animationType, appearance: appearance)
    {
        this.parentName = parentName;
        this.normalScale = normalScale;
    }

    // public override Transform getParent()
    // {
    //     return parent;
    // }

    // public override void spawnActions(GameObject npc)
    // {
    //     GameObject parentObject = DialogueManager.findNPCGameObject(parentName);

    //     if(parentObject != null)
    //     {
    //         parent = parentObject.GetComponent<RectTransform>();

    //         if(parent == null)
    //         {
    //             parent = parentObject.AddComponent<RectTransform>();
    //         }
    //     }

    //     // Vector3 worldPos = npc.transform.position;

    //     npc.AddComponent<RectTransform>();

    //     // npc.transform.SetParent(getParent());

    //     if(normalScale)
    //     {
    //         npc.transform.localScale = Vector3.one;
    //     } else
    //     {
    //         npc.transform.localScale = Constants.scaleChange;
    //     }

    //     NameTagGenerator nameTagGenerator = npc.GetComponent<NameTagGenerator>();

    //     // nameTagGenerator.getSpriteOutline().normalZPos = -5f;

    //     // npc.transform.position = worldPos;

    //     base.spawnActions(npc);
    // }

}

public class GateSpawnBehaviour : IExtraSpawnBehaviour
{

    public Dialogue dialogue;

    protected string gateKey;

    private Dictionary<string, int> statDifficulties;

    public GateSpawnBehaviour(string gateKey, Dictionary<string, int> statDifficulties = null)
    {
        this.gateKey = gateKey;
        this.statDifficulties = statDifficulties ?? new Dictionary<string, int>();
    }

    public void addBehaviour(GameObject gateGameObject)
    {
        Gate gate = gateGameObject.AddComponent<Gate>();

        gate.setKey(gateKey);

        if (dialogue == null)
        {
            return;
        }

        dialogue.cameraFoci[Constants.indexOne] = gateGameObject;

        dialogue.variableSources.Add(new StoryStatRequirementVariableSource(statDifficulties));
    }
}


// public class GateSpawnDetails : NPCSpawnDetails
// {
//     private bool showSprite;
//     private Axis axis;

//     public GateSpawnDetails(string npcName,
//                             Vector3Int cellCoords,
//                             string currentArea,
//                             string tutorialTargetHash,
//                             bool showSprite,
//                             Axis axis,
//                             IAppearance appearance = null) :
//     base(npcName, cellCoords, currentArea, appearance: appearance)
//     {
//         this.tutorialTargetHash = tutorialTargetHash;
//         this.showSprite = showSprite;
//         this.axis = axis;
//     }
// }

// public class GateWithKeySpawnDetails : GateSpawnDetails
// {
//     public GateWithKeySpawnDetails(string npcName, Vector3Int cellCoords, string currentArea, bool showSprite, Axis axis, GateKeyDetails gateKeyDetails, IAppearance appearance = null) :
//     base(npcName, cellCoords, currentArea, noTutorialTargetHash, showSprite, axis, new Dictionary<string, int>(), appearance: appearance,
//          gateSpawnBehaviour: new GateWithKeySpawnBehaviour(npcName, noTutorialTargetHash, new Dictionary<string, int>(), gateKeyDetails))
//     {
//     }

//     public override Dialogue getDialogue(string areaName)
//     {
//         return new SingleCharacterDialogue(npcName, InkAssetList.getInkJSON(DialogueKey.GateWithKey));
//     }
// }

// public class GateWithKeySpawnBehaviour : GateSpawnBehaviour
// {
//     private GateKeyDetails gateKeyDetails;

//     public GateWithKeySpawnBehaviour(string gateKey, string tutorialTargetHash, Dictionary<string, int> statDifficulties, GateKeyDetails gateKeyDetails) :
//     base(gateKey, tutorialTargetHash, statDifficulties)
//     {
//         this.gateKeyDetails = gateKeyDetails;
//     }

//     protected override void addDialogueBehaviour(GameObject gateGameObject)
//     {
//         base.addDialogueBehaviour(gateGameObject);

//         if (dialogue == null)
//         {
//             return;
//         }

//         dialogue.variableSources.Add(gateKeyDetails);
//     }
// }

// public class TemporaryGateSpawnDetails : GateSpawnDetails
// {
//     public TemporaryGateSpawnDetails(string npcName, Vector3Int cellCoords, string currentArea, Axis axis, Dictionary<string, int> statDifficulties, IAppearance appearance = null) :
//     base(npcName, cellCoords, currentArea, true, axis, statDifficulties, appearance: appearance,
//          gateSpawnBehaviour: new TemporaryGateSpawnBehaviour(npcName, statDifficulties))
//     {

//     }

//     public override Dialogue getDialogue(string areaName)
//     {
//         return DialogueList.getDialogue(DialogueList.scrubNameOfEndNumbers(npcName), areaName);
//     }

// }

// public class TemporaryGateSpawnBehaviour : GateSpawnBehaviour
// {
//     public TemporaryGateSpawnBehaviour(string gateKey, string tutorialTargetHash, Dictionary<string, int> statDifficulties) :
//     base(gateKey, tutorialTargetHash, statDifficulties)
//     {
//     }

//     protected override Gate addGate(GameObject gateGameObject)
//     {
//         return gateGameObject.AddComponent<TemporaryGate>();
//     }
// }

// public class GateWithHiddenTerrainSpawnDetails : GateSpawnDetails
// {
//     public GateWithHiddenTerrainSpawnDetails(string npcName, Vector3Int cellCoords, string currentArea, string spriteName, string tutorialTargetHash, Dictionary<string, int> statDifficulties, string hiddenTerrainFlag, IAppearance appearance = null) :
//     base(npcName, cellCoords, currentArea, spriteName, tutorialTargetHash, true, Axis.DescendingX, statDifficulties, appearance: appearance,
//          gateSpawnBehaviour: new GateWithHiddenTerrainSpawnBehaviour(npcName, tutorialTargetHash, statDifficulties, hiddenTerrainFlag))
//     {
//     }

//     public override Dialogue getDialogue(string areaName)
//     {
//         return DialogueList.getDialogue(DialogueList.scrubNameOfEndNumbers(npcName), areaName);
//     }
// }

// public class GateWithHiddenTerrainSpawnBehaviour : GateSpawnBehaviour
// {
//     private string hiddenTerrainFlag;

//     public GateWithHiddenTerrainSpawnBehaviour(string gateKey, string tutorialTargetHash, Dictionary<string, int> statDifficulties, string hiddenTerrainFlag) :
//     base(gateKey, tutorialTargetHash, statDifficulties)
//     {
//         this.hiddenTerrainFlag = hiddenTerrainFlag;
//     }

//     protected override Gate addGate(GameObject gateGameObject)
//     {
//         GateWithHiddenTerrain gate = gateGameObject.AddComponent<GateWithHiddenTerrain>();
//         gate.hiddenTerrainFlag = hiddenTerrainFlag;
//         NameTagGenerator nameTagGenerator = gateGameObject.GetComponent<NameTagGenerator>();

//         nameTagGenerator.nameSource = gate;

//         return gate;
//     }
// }

public class RestStopAndShopkeeperSpawnDetails : NPCSpawnDetails
{

    private bool isShopkeeper = false;
    private bool isRestStop = false;

    public RestStopAndShopkeeperSpawnDetails(string npcName,
                                             Vector3Int cellCoords,
                                             Vector3Int[] extraSpaces = null,
                                             bool ignoresSecretDoors = true, 
                                             Facing facing = Facing.Random, 
                                             bool isShopkeeper = false,
                                             bool isRestStop = false,
                                             IAppearance appearance = null) :
    base(npcName, cellCoords, facing: facing, extraSpaces: extraSpaces, ignoresSecretDoors: ignoresSecretDoors, appearance: appearance)
    {
        this.isShopkeeper = isShopkeeper;
        this.isRestStop = isRestStop;
    }

    // public override void spawnActions(GameObject npc)
    // {
    //     base.spawnActions(npc);

    //     if(isShopkeeper)
    //     {
    //         Shopkeeper shopkeeper = npc.AddComponent<Shopkeeper>();

    //         shopkeeper.shopkeeperInventoryKey = npcName;
    //     }

    //     if(isRestStop)
    //     {
    //         npc.AddComponent<RestStop>();
    //     }

    // }
}

public class SecretDoorSpawnDetails : NPCSpawnDetails
{
    private SecretDoorInfo secretDoorInfo;
    private string terrainSpriteName;
    private ObservableDelegate observable;

    public SecretDoorSpawnDetails(string npcName,
                                    Vector3Int cellCoords,
                                    SecretDoorInfo secretDoorInfo,
                                    string tutorialTargetHash, 
                                    string terrainSpriteName, 
                                    ObservableDelegate observable = null, 
                                    QuestStepActivationScript script = null, 
                                    IAppearance appearance = null) :
    base(npcName, cellCoords, appearance: appearance)
    {
        this.secretDoorInfo = secretDoorInfo;

        this.tutorialTargetHash = tutorialTargetHash;
        this.terrainSpriteName = terrainSpriteName;

        // dialogue = getDialogue(areaName);
    
        this.observable = observable;
    }

    // public override Dialogue getDialogue(string areaName)
    // {
    //     if(secretDoorInfo != null && secretDoorInfo.customDialogueKey != DialogueKey.NoDialogue)
    //     {
    //         return new Dialogue(new string[] { Constants.emptyString, NPCNameList.suspiciousWall }, InkAssetList.getInkJSON(secretDoorInfo.customDialogueKey));
    //     }

    //     return new Dialogue(new string[] { Constants.emptyString, NPCNameList.suspiciousWall }, InkAssetList.getInkJSON(DialogueKey.SuspiciousWall));
    // }

    // public override bool interactable()
    // {
    //     return true;
    // }

    // public override string getPrefabName()
    // {
    //     return PrefabNames.secretDoor;
    // }

    // public override void spawnActions(GameObject secretDoor)
    // {
    //     if(secretDoorInfo.hasBeenDiscovered())
    //     {
    //         GameObject.DestroyImmediate(secretDoor);
    //     }

    //     base.spawnActions(secretDoor);

    //     ObservableObject observableObject = secretDoor.GetComponent<ObservableObject>();

    //     observableObject.secretDoorKeys = secretDoorInfo.secretDoorKeys;

    //     if(terrainSpriteName != null && terrainSpriteName.Length > 0)
    //     {
    //         observableObject.terrainSprite = SpriteUtil.loadSpriteFromResources(terrainSpriteName);
    //     }

    //     if(hasTutorialTargetHash())
    //     {
    //         SpriteRenderer spriteRenderer = secretDoor.GetComponent<SpriteRenderer>();
    //         addTutorialTargetComponent(secretDoor, spriteRenderer, tutorialTargetHash);
    //     }

    //     if(observable == null || observable())
    //     {
    //         secretDoor.layer = LayerAndTagManager.observableLayer;
    //     } else
    //     {
    //         secretDoor.layer = LayerAndTagManager.objectLayer;
    //     }

    //     // observableObject.script = script;
    // }

    // public override void spawnActions(DialogueTrigger dialogueTrigger)
    // {
    //     Dialogue dialogue = dialogueTrigger.dialogue;

    //     dialogue.variableSources.Add(secretDoorInfo);

    //     // dialogueTrigger.introAudioClipLogic = getDialogueIntroSFXLogic();
    // }
}

public class LadderSpawnDetails : NPCSpawnDetails
{
    public const float offsetY = .1f;
    public const bool doNotFlipX = false;

    public Ladder ladder;

    public LadderSpawnDetails(Vector3Int cellCoords, Ladder ladder, IAppearance appearance = null) :
    base(NPCNameList.ladder, cellCoords, appearance: appearance)
    {
        this.ladder = ladder;
    }

    // public override Dialogue getDialogue(string areaName)
    // {
    //     return Ladder.dialogue;
    // }

    // public override void spawnActions(GameObject npc)
    // {
    //     base.spawnActions(npc);
    // }

    // public override void spawnActions(DialogueTrigger mainTrigger)
    // {
    //     base.spawnActions(mainTrigger);

    //     mainTrigger.dialogue.variableSources.Add(ladder);

    //     SpawnInfoManager.spawnTransitionSpace(ladder.locationName, ladder.destinationName, cellCoords, ladder.facing);
    // }
}

public class VaultableOrDestroyableObjectSpawnDetails : VaultableObjectSpawnDetails
{

    public int index;

    public VaultableOrDestroyableObjectSpawnDetails(string npcName, Vector3Int cellCoords, VaultableOrDestroyableObject vaultableOrDestroyableObject, int index = 0, bool withScale = true, IAppearance appearance = null) :
    base(npcName, cellCoords, vaultableOrDestroyableObject, appearance: appearance)
    {
        this.index = index;
    }

    // public override void spawnActions(GameObject gameObject)
    // {
    //     base.spawnActions(gameObject);

    //     Gate gate = gameObject.AddComponent<Gate>();

    //     gate.setKey(VaultableOrDestroyableObject.gateKey+index);
    // }
}

public class BookSpawnDetails : OOCSpawnDetails
{

    private int bookIndex;

    public BookSpawnDetails(string npcName, Vector3Int cellCoords, int bookIndex, IAppearance appearance = null) :
    base(npcName, appearance: appearance, cellCoords: cellCoords, ignoresSecretDoors: true)
    {
        this.bookIndex = bookIndex;
    }

    // public override string getPrefabName()
    // {
    //     return PrefabNames.book;
    // }

    // public override void spawnActions(GameObject interactable)
    // {
    //     base.spawnActions(interactable);

    //     WorldBookInfo bookInfo = interactable.GetComponent<WorldBookInfo>();

    //     bookInfo.bookIndex = bookIndex;

    //     BookItem book = ItemList.getItem(ItemList.bookListIndex, bookIndex) as BookItem;

    //     StopSpawningFlagList stopSpawningFlagList = new StopSpawningFlagList(book.flagsFlippedWhenRead);

    //     if(book != null && stopSpawningFlagList.evaluateFlags())
    //     {
    //         interactable.SetActive(false);
    //     }
    // }
}

public class HiddenTerrainSpawnDetails : OOCSpawnDetails
{


    public List<string> secretDoorKeys = new List<string>();

    protected string areaName;
    protected string sectionName;

    protected string locationName;

    protected int index;

    public HiddenTerrainSpawnDetails(string secretDoorKey = null, List<string> secretDoorKeys = null, string locationName = "", int index = 0, bool withScale = true, IAppearance appearance = null) :
    base(appearance: appearance)
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

    public HiddenTerrainSpawnDetails(string secretDoorKey = null, List<string> secretDoorKeys = null, string areaName = "", string sectionName = "", int index = 0, IAppearance appearance = null) :
    base(appearance: appearance)
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

    // public override string getPrefabName()
    // {
    //     if (locationName == null)
    //     {        
    //         return HiddenTerrainList.getHiddenTerrainFolderPath(areaName, sectionName, index);
    //     }
    //     else
    //     {
    //         return HiddenTerrainList.getHiddenTerrainFolderPath(locationName, index);
    //     }
    // }

    public override SpawnParams getSpawnParams()
    {
        return new HiddenTerrainSpawnParams(secretDoorKeys);
    }

    // public override Transform getParent()
    // {
    //     return AreaManager.getGridParent();
    // }

    // public override void spawnActions(GameObject interactable)
    // {
    //     interactable.transform.localPosition = Vector3.zero;

    //     Helpers.updateColliderPosition(interactable);
    // }

}

public class HostilityTerrainSpawnDetails : HiddenTerrainSpawnDetails
{

    private const string hostilitySecretDoorFlagPlaceholder = "Hostility-";

    public HostilityTerrainSpawnDetails(string locationName, int index, bool withScale = true, IAppearance appearance = null) :
    base(hostilitySecretDoorFlagPlaceholder+index, locationName: locationName, index: index, withScale: withScale, appearance: appearance)
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