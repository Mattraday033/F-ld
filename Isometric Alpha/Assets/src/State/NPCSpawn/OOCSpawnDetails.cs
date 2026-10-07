using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OOCSpawnDetails: IAppearanceSource
{
    public string displayName;
    public int index;
    public Vector3Int cellCoords;
    public Vector3Int[] extraSpaces = new Vector3Int[0];
    protected Facing facing;
    protected bool ignoresSecretDoors;
    protected Dictionary<Type, IExtraSpawnBehaviour> aestheticSpawnBehaviours = new();
    protected Dictionary<Type, IExtraSpawnBehaviour> universalSpawnBehaviours = new();
    protected ActivationListenerSpawnBehaviour activationListenerSpawnBehaviour = new();

    private IAppearance _Appearance;
    public IAppearance appearance { get { return _Appearance; } }

    //spawn details sharing an displayName are told apart by their index, which is appended to form the key their dialogue, spawn params and GameObject are found by
    public virtual string uniqueName { get { return index == 0 ? displayName : displayName + index; } }

    public virtual SpawnParams spawnParams { get { return SpawnParamsList.getSpawnParams(AreaManager.locationName, uniqueName); } }
    protected virtual string tag { get { return LayerAndTagManager.untaggedTag; } }
    protected virtual int layer { get { return LayerAndTagManager.defaultLayer; } }

    public virtual bool spawnsOnSecretDoorActivation { get { return false; } }

    public virtual string prefabName { get { return PrefabNames.creaturePrefab; } }

    public virtual Vector3 localScale { get { return Vector3.one; } }

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

    public OOCSpawnDetails( string displayName = "",
                            IAppearance appearance = null,
                            Vector3Int cellCoords = new Vector3Int(),
                            Facing facing = Facing.Random,
                            string tutorialTargetHash = "",
                            bool ignoresSecretDoors = false,
                            Vector3Int[] extraSpaces = null,
                            float colliderOffset = -2f,
                            int index = 0
                            )
    {
        this.displayName = displayName;
        this.index = index;
        this._Appearance = appearance ?? Costume.getDefaultCostume();
        this.cellCoords = cellCoords;
        this.ignoresSecretDoors = ignoresSecretDoors;
        this.facing = facing;

        if(extraSpaces != null)
        {
            this.extraSpaces = extraSpaces;
        }

        if(!string.IsNullOrEmpty(tutorialTargetHash))
        {
            aestheticSpawnBehaviours[typeof(TutorialTargetSpawnBehaviour)] = new TutorialTargetSpawnBehaviour(tutorialTargetHash);
        }

        if(colliderOffset >= -1f && colliderOffset <= 1f)
        {
            universalSpawnBehaviours[typeof(TilemapOffsetSpawnBehaviour)] = new TilemapOffsetSpawnBehaviour(colliderOffset);
        }

        // this.universalSpawnBehaviours = universalSpawnBehaviours ?? new List<IExtraSpawnBehaviour>();
        // this.aestheticSpawnBehaviours = aestheticSpawnBehaviours ?? new List<IExtraSpawnBehaviour>();
    }

    //when true, every extra space gets the full prefab, appearance and aesthetic behaviours instead of an invisible Extra Space
    protected virtual bool extraSpacesAreVisible { get { return false; } }

    public List<GameObject> spawnInteractables()
    {
        GameObject main = spawnVisible(cellCoords);
        List<GameObject> allInteractables = new List<GameObject>() { main };

        foreach(Vector3Int cell in extraSpaces)
        {
            GameObject extraSpace = extraSpacesAreVisible ? spawnVisible(cell, extra: true) : generateBlankPrefab(cell, PrefabNames.extraSpace, extra: true);
            allInteractables.Add(extraSpace);
        }

        // setIgnoresSecretDoors(interactable);

        foreach(GameObject interactable in allInteractables)
        {
            ActivationListener listener = activationListenerSpawnBehaviour.addBehaviour(interactable) as ActivationListener;
            listener.index = index;
            activationListenerSpawnBehaviour.applyActivationRequirements(listener);

            foreach(IExtraSpawnBehaviour behaviour in universalSpawnBehaviours.Values)
            {
                Component component = behaviour.addBehaviour(interactable);
                INameSource nameSource = component as INameSource;

                listener.nameSource = nameSource ?? listener.nameSource;
                behaviour.applyActivationRequirements(listener);
            }

            interactable.layer = layer;
            interactable.tag = tag;
        }
        
        // Canvas.ForceUpdateCanvases();

        return allInteractables;
    }

    public const string gameObjectNameSuffix = "'s GameObject";
    public const string extraSpaceNameSuffix = "'s Extra Space GameObject";
    private const string gameObjectPlaceHolderName = "PlaceHolder GameObject";

    public void setGameObjectName(GameObject gameObject, bool extra = false)
    {
        if (displayName.Length > 0)
        {
            if(extra)
            {
                gameObject.name = uniqueName + extraSpaceNameSuffix;
            } else
            {
                gameObject.name = uniqueName + gameObjectNameSuffix;
            }
        }
        else
        {
            gameObject.name = gameObjectPlaceHolderName;
        }
    }

    private GameObject spawnVisible(Vector3Int cell, bool extra = false)
    {
        GameObject visible = generateBlankPrefab(cell, prefabName, extra);

        appearance.applyAppearance(visible.GetComponent<SpriteLayerRendererList>(), updateColors: true);

        foreach(IExtraSpawnBehaviour behaviour in aestheticSpawnBehaviours.Values)
        {
            behaviour.addBehaviour(visible);
        }

        return visible;
    }

    //extra decides the GameObject's name, so a visible extra space is still skipped when looking up the main GameObject by name
    private GameObject generateBlankPrefab(Vector3Int cell, string prefabToLoad, bool extra = false)
    {
        GameObject blank = GameObject.Instantiate(Resources.Load<GameObject>(prefabToLoad), parent);
        setGameObjectName(blank, extra);
        blank.transform.localScale = localScale;

        setBlankPrefabPosition(cell, blank.transform);
        GameObjectUtil.updateGameObjectPosition(blank);

        return blank;
    }

    protected virtual void setBlankPrefabPosition(Vector3Int cell, Transform transform)
    {
        transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(cell);
    }
}

public class CunningObjectSpawnDetails : OOCSpawnDetails
{
    //the index identifies the cunning object itself, not a variant of its name
    public override string uniqueName { get { return displayName; } }

    protected override int layer { get { return LayerAndTagManager.cunningableObjectLayer; } }

    public CunningObjectSpawnDetails(int index,
                                        Vector3Int cellCoords,
                                        CunningObjectSpriteCategory category,
                                        CunningAction cunningAction,
                                        QuestStepActivationScript script = null,
                                        string tutorialTargetHash = null,
                                        int linkedIndex = CunningObject.noLinkedIndex) :
    base(category.ToString(), appearance: new CunningObjectAppearance(index, category), cellCoords: cellCoords, tutorialTargetHash: tutorialTargetHash, index: index)
    {
        aestheticSpawnBehaviours[typeof(NPCMouseHoverSpawnBehaviour)] = new NPCMouseHoverSpawnBehaviour();

        universalSpawnBehaviours[typeof(CunningObjectSpawnBehaviour)] = new CunningObjectSpawnBehaviour(index, cunningAction, category, script, linkedIndex);
    }

}

public class ObstacleSpawnDetails : OOCSpawnDetails
{
    protected override int layer { get { return LayerAndTagManager.objectLayer; } }

    private KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements;

    //children override obstacleSpawnParams instead, so the cunning check below always runs first
    public sealed override SpawnParams spawnParams
    {
        get
        {
            if(deactivatedByCunningObject())
            {
                return new NeverSpawnParams();
            }

            return obstacleSpawnParams;
        }
    }

    protected virtual SpawnParams obstacleSpawnParams { get { return base.spawnParams; } }

    public ObstacleSpawnDetails(string displayName,
                                Vector3Int cellCoords,
                                Facing facing = Facing.Random,
                                bool ignoresSecretDoors = true,
                                IAppearance appearance = null,
                                int index = 0,
                                KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements = null) :
    base(displayName, facing: facing, appearance: appearance, cellCoords: cellCoords, ignoresSecretDoors: ignoresSecretDoors, index: index)
    {
        this.activationRequirements = activationRequirements ?? new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

        universalSpawnBehaviours[typeof(ObstacleSpawnBehaviour)] = new ObstacleSpawnBehaviour(uniqueName, ignoresSecretDoors, this.activationRequirements);
    }

    private bool deactivatedByCunningObject()
    {
        if(Array.IndexOf(activationRequirements, ActivationRequirementList.cunningByIndex) < 0)
        {
            return false;
        }

        return TrapAndButtonStateManager.contains(CunningObject.generateKey(AreaManager.locationName, index));
    }

}

public class NPCSpawnDetails : OOCSpawnDetails, IDialogueSource
{
    protected override string tag { get { return LayerAndTagManager.npcTag; } }
    protected override int layer { get { return LayerAndTagManager.npcLayer; } }

    public virtual Dialogue dialogue
    {
        get
        {
            return DialogueList.getDialogue(AreaManager.locationName, uniqueName);
        }
    }

    public NPCSpawnDetails( string displayName,
                            Vector3Int cellCoords,
                            Facing facing = Facing.Random,
                            Vector3Int[] extraSpaces = null,
                            SpeakAtStartScript speakAtStartScript = null,
                            CharacterAnimationType animationType = CharacterAnimationType.None,
                            string tutorialTargetHash = "",
                            bool ignoresSecretDoors = true,
                            bool sleepingDialogueIntro = false,
                            IAppearance appearance = null,
                            float colliderOffset = -2f,
                            int index = 0) :
    base(displayName, facing: facing, appearance: appearance, cellCoords: cellCoords, tutorialTargetHash: tutorialTargetHash, ignoresSecretDoors: ignoresSecretDoors, extraSpaces: extraSpaces, colliderOffset: colliderOffset, index: index)
    {
        aestheticSpawnBehaviours[typeof(AnimationManagerSpawnBehaviour)] = new AnimationManagerSpawnBehaviour(this, facing, animationType);
        aestheticSpawnBehaviours[typeof(NPCMouseHoverSpawnBehaviour)] = new NPCMouseHoverSpawnBehaviour();
        aestheticSpawnBehaviours[typeof(OverHeadIconManagerSpawnBehaviour)] = new OverHeadIconManagerSpawnBehaviour();

        universalSpawnBehaviours[typeof(DialogueTriggerSpawnBehaviour)] = new DialogueTriggerSpawnBehaviour(uniqueName,
                                                                            dialogueSource: this,
                                                                            AudioClipList.getDialogueIntroSFXLogic(displayName, sleepingDialogueIntro),
                                                                            speakAtStartScript: speakAtStartScript);

        if(PartyMemberList.characterIsPartyMember(displayName))
        {
            //the NPC stands aside once the party member it plays is following the player instead
            activationListenerSpawnBehaviour = new ActivationListenerSpawnBehaviour(new KeyValuePair<ActivationDesignatorType, ActivationCategory>[]
            {
                ActivationRequirementList.joinedFormationByName
            });
        }
    }
}

public class RestStopAndShopkeeperSpawnDetails : NPCSpawnDetails
{

    public RestStopAndShopkeeperSpawnDetails(string displayName,
                                             Vector3Int cellCoords,
                                             Vector3Int[] extraSpaces = null,
                                             bool ignoresSecretDoors = true, 
                                             Facing facing = Facing.Random, 
                                             bool isShopkeeper = false,
                                             bool isRestStop = false,
                                             IAppearance appearance = null,
                                             float colliderOffset = -2f,
                                             int index = 0) :
    base(displayName, cellCoords, facing: facing, extraSpaces: extraSpaces, ignoresSecretDoors: ignoresSecretDoors, appearance: appearance, colliderOffset: colliderOffset, index: index)
    {
        if(isShopkeeper)
        {
            universalSpawnBehaviours[typeof(ShopkeeperSpawnBehaviour)] = new ShopkeeperSpawnBehaviour(displayName);
        }

        if(isRestStop)
        {
            universalSpawnBehaviours[typeof(RestStopSpawnBehaviour)] = new RestStopSpawnBehaviour();
        }
    }
}

public enum Axis { DescendingX = 0, DescendingY = 1}

public enum GateSpriteLayout { Single, PerCell }

public delegate bool ObservableDelegate();

//spawn details that stretch from cellCoords along an axis, with the remaining cells filled by extra spaces
public class AxisSpawnDetails : NPCSpawnDetails
{
    private GateSpriteLayout layout;

    protected override bool extraSpacesAreVisible { get { return layout == GateSpriteLayout.PerCell; } }

    public AxisSpawnDetails(IAppearance appearance,
                            string displayName,
                            int index = 0,
                            Vector3Int cellCoords = default,
                            int size = 1,
                            Axis axis = Axis.DescendingX,
                            string tutorialTargetHash = "",
                            GateSpriteLayout layout = GateSpriteLayout.Single) :
    base(displayName, cellCoords, extraSpaces: generateExtraSpaces(cellCoords, size, axis), appearance: appearance, tutorialTargetHash: tutorialTargetHash, index: index)
    {
        this.layout = layout;

        if(layout == GateSpriteLayout.Single && aestheticSpawnBehaviours.ContainsKey(typeof(AnimationManagerSpawnBehaviour)))
        {
            aestheticSpawnBehaviours.Remove(typeof(AnimationManagerSpawnBehaviour));
        }
    }

    private static Vector3Int[] generateExtraSpaces(Vector3Int cellCoords, int size, Axis axis)
    {
        List<Vector3Int> extraSpaces = new List<Vector3Int>();

        for (int index = 1; index < size; index++)
        {
            Vector3Int currentCell = cellCoords;

            if (axis == Axis.DescendingX)
            {
                currentCell.x -= index;
            }
            else if (axis == Axis.DescendingY)
            {
                currentCell.y -= index;
            }

            extraSpaces.Add(currentCell);
        }

        return extraSpaces.ToArray();
    }
}

public class GateSpawnDetails : AxisSpawnDetails
{
    protected Dictionary<string, int> statDifficulties = new Dictionary<string, int>();

    public GateSpawnDetails(IAppearance appearance,
                            string displayName,
                            int index = 0,
                            Vector3Int cellCoords = default,
                            int size = 1,
                            Axis axis = Axis.DescendingX,
                            string tutorialTargetHash = "",
                            KeyValuePair<string, int> statDifficulty = new KeyValuePair<string, int>(),
                            GateSpriteLayout layout = GateSpriteLayout.Single) :
    base(appearance, displayName, index, cellCoords, size, axis, tutorialTargetHash, layout)
    {
        universalSpawnBehaviours[typeof(GateSpawnBehaviour)] = new GateSpawnBehaviour(uniqueName);

        if(statDifficulty.Key != null && statDifficulty.Key.Length > 0)
        {
            statDifficulties.Add(statDifficulty.Key, statDifficulty.Value);
        }
    }
}

public class LadderSpawnDetails : NPCSpawnDetails
{
    public const float offsetY = .1f;
    public const bool doNotFlipX = false;

    public override Dialogue dialogue
    {
        get
        {
            return Ladder.getDialogue();
        }
    }

    public Ladder ladder;

    public LadderSpawnDetails(Vector3Int cellCoords, Ladder ladder, IAppearance appearance = null, float colliderOffset = -2f, int index = 0) :
    base(NPCNameList.ladder, cellCoords, appearance: appearance, colliderOffset: colliderOffset, index: index)
    {
        this.ladder = ladder;
    }

}

public class NonDialogueNPCSpawnDetails : NPCSpawnDetails
{
    public override SpawnParams spawnParams { 
                                                get 
                                                { 
                                                    InteractableSpawnParams _SpawnParams = SpawnParamsList.getSpawnParams(AreaManager.locationName, uniqueName);

                                                    if(_SpawnParams.startSpawningFlagList.flags.Length == 0 && 
                                                        _SpawnParams.stopSpawningFlagList.flags.Length == 0)
                                                    {
                                                        return new NeverSpawnParams();
                                                    } else
                                                    {
                                                        return _SpawnParams;
                                                    }
                                                }
                                            }

    public NonDialogueNPCSpawnDetails(string displayName,
                                        Vector3Int cellCoords,
                                        Facing facing = Facing.Random,
                                        bool ignoresSecretDoors = true,
                                        CharacterAnimationType animationType = CharacterAnimationType.None,
                                        IAppearance appearance = null,
                                        float colliderOffset = -2f,
                                        int index = 0) :
    base(displayName, cellCoords, facing, ignoresSecretDoors: ignoresSecretDoors, animationType: animationType, appearance: appearance, colliderOffset: colliderOffset, index: index)
    {
        universalSpawnBehaviours[typeof(DialogueTriggerSpawnBehaviour)] = new DialogueTriggerSpawnBehaviour(uniqueName);
    }
}

public class VaultableObjectSpawnDetails : NPCSpawnDetails
{

    public VaultableObjectSpawnDetails(string displayName,
                                        Vector3Int cellCoords,
                                        VaultableObject vaultableObject,
                                        string tutorialTargetHash = "",
                                        IAppearance appearance = null,
                                        float colliderOffset = -2f,
                                        int index = 0) :
    base(displayName,
         cellCoords,
         tutorialTargetHash: tutorialTargetHash,
         appearance: appearance,
         colliderOffset: colliderOffset,
         index: index)
    {
        DialogueTriggerSpawnBehaviour dialogueTriggerSpawnBehaviour = universalSpawnBehaviours[typeof(DialogueTriggerSpawnBehaviour)] as DialogueTriggerSpawnBehaviour;
        dialogueTriggerSpawnBehaviour.dialogueSource = vaultableObject;
    }

}


public class HorseSpawnDetails : NPCSpawnDetails
{
    public HorseSpawnDetails(string displayName,
                                Vector3Int cellCoords, 
                                Facing facing,
                                Vector3Int[] extraSpaces = null,
                                SpeakAtStartScript speakAtStartScript = null,
                                CharacterAnimationType animationType = CharacterAnimationType.None, 
                                bool ignoresSecretDoors = true,
                                IAppearance appearance = null,
                                float colliderOffset = -2f,
                                int index = 0) :
    base(displayName, cellCoords, facing: facing, extraSpaces: generateRumpExtraSpace(extraSpaces ?? new Vector3Int[0], cellCoords, facing),
            speakAtStartScript: speakAtStartScript, ignoresSecretDoors: ignoresSecretDoors, animationType: animationType, appearance: appearance, colliderOffset: colliderOffset, index: index)
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
    private ContainerType type;
    protected override int layer { get { return LayerAndTagManager.chestLayer; } }
    public override Transform parent { get {
                                                if(type.withScale()) 
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
                             ContainerType type,
                             QuestStepActivationScript script = null,
                             string secretDoorFlag = null,
                             IAppearance appearance = null) :
    base(displayName: generateName(index), facing: facing, appearance: appearance, cellCoords: cellCoords, index: index)
    {
        this.type = type;

        aestheticSpawnBehaviours[typeof(AnimationManagerSpawnBehaviour)] = new AnimationManagerSpawnBehaviour(this, facing);
        aestheticSpawnBehaviours[typeof(NPCMouseHoverSpawnBehaviour)] = new NPCMouseHoverSpawnBehaviour();
        aestheticSpawnBehaviours[typeof(OverHeadIconManagerSpawnBehaviour)] = new OverHeadIconManagerSpawnBehaviour(ignoresSecretDoors: true);
        universalSpawnBehaviours[typeof(ContainerSpawnBehaviour)] = new ContainerSpawnBehaviour(index, script, secretDoorFlag, type);
    }

    //generateName already puts the index into the name
    public override string uniqueName { get { return displayName; } }

    public static string generateName(int index)
    {
        return NPCNameList.chest + "-" + index;
    }

    // public bool isSingleSpriteChest()
    // {
    //     return spriteName != null;
    // }

    // public virtual ContainerType getType()
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

public class DeadBodySpawnDetails : ObstacleSpawnDetails
{

    private bool weaponless;

    public DeadBodySpawnDetails(string displayName, 
                                Vector3Int cellCoords, 
                                Facing facing = Facing.NorthEast, 
                                bool ignoresSecretDoors = true, 
                                bool weaponless = false,
                                IAppearance appearance = null,
                                int index = 0,
                                KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements = null) :
    base(displayName, cellCoords, facing: facing, ignoresSecretDoors: ignoresSecretDoors, appearance: appearance, index: index, activationRequirements: activationRequirements)
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
    protected override SpawnParams obstacleSpawnParams { get { return new SecretDoorObstacleSpawnParams(secretDoorFlag); } }

    protected string secretDoorFlag;

    public ObstacleWithSecretDoorFlagSpawnDetails(  string displayName, 
                                                    Vector3Int cellCoords, 
                                                    string secretDoorFlag = "",
                                                    IAppearance appearance = null,
                                                    int index = 0,
                                                    KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements = null) :
    base(displayName, cellCoords, appearance: appearance, index: index, activationRequirements: activationRequirements)
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

    // public override void spawnActions(GameObject interactable)
    // {
    //     GameObject.Destroy(interactable.GetComponent<Obstacle>());

    //     ObstacleWithSecretDoorFlag obstacle = interactable.AddComponent<ObstacleWithSecretDoorFlag>();

    //     obstacle.setObstacleName(displayName);
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
    public Wave(string displayName, 
                Vector3Int cellCoords, 
                string secretDoorFlag = "",
                IAppearance appearance = null,
                int index = 0,
                KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements = null) :
    base(displayName, cellCoords, secretDoorFlag: secretDoorFlag, appearance: appearance, index: index, activationRequirements: activationRequirements)
    {
    }

    // public override string getPrefabName()
    // {
    //     return PrefabNames.wave;
    // }

    // public override void spawnActions(GameObject interactable)
    // {
    //     ObstacleWithSecretDoorFlag obstacle = interactable.AddComponent<ObstacleWithSecretDoorFlag>();

    //     obstacle.setObstacleName(displayName);
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

//the index is the index of the cunning object that raises and lowers the spike
public class SpikeSpawnDetails : ObstacleSpawnDetails
{
    public readonly bool raisedWhenCranked;

    public SpikeSpawnDetails(Vector3Int cellCoords,
                            IAppearance appearance = null,
                            int index = 0,
                            bool raisedWhenCranked = false) :
    base(NPCNameList.spike, cellCoords, appearance: appearance, index: index)
    {
        this.raisedWhenCranked = raisedWhenCranked;

        universalSpawnBehaviours[typeof(ObstacleSpawnBehaviour)] = new SpikeSpawnBehaviour(uniqueName, index, raisedWhenCranked);
    }

    public bool isRaised()
    {
        return Spike.isRaised(TrapAndButtonStateManager.contains(CunningObject.generateKey(AreaManager.locationName, index)), raisedWhenCranked);
    }

    // public override string getPrefabName()
    // {
    //     return PrefabNames.spikes;
    // }

}

public class RubbleObstacleSpawnDetails : ObstacleSpawnDetails
{
    public RubbleObstacleSpawnDetails(string displayName, 
                                        Vector3Int cellCoords,
                                        IAppearance appearance = null,
                                        int index = 0,
                                        KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements = null) :
    base(displayName, cellCoords, appearance: appearance, index: index, activationRequirements: activationRequirements)
    {
    }
}

public class ButtonSpawnDetails : OOCSpawnDetails
{

    private int weight;
    private int charismaRequirement;

    public override Transform parent { get { return AreaManager.getNPCParentWithScale(); } }

    public override string uniqueName { get { return displayName; } }

    private const float buttonColliderOffset = -0.15f;

    public ButtonSpawnDetails(Vector3Int cellCoords,
                                int index = 0,
                                int weight = 1,
                                int charismaRequirement = 1,
                                string tutorialTargetHash = "",
                                IAppearance appearance = null,
                                string secretDoorFlag = null) :
    base(NPCNameList.button, appearance: appearance ?? SpriteDescriptionList.buttonUpStone, cellCoords: cellCoords, ignoresSecretDoors: true, tutorialTargetHash: tutorialTargetHash, colliderOffset: buttonColliderOffset, index: index)
    {
        this.weight = weight;
        this.charismaRequirement = charismaRequirement;

        aestheticSpawnBehaviours[typeof(NPCMouseHoverSpawnBehaviour)] = new NPCMouseHoverSpawnBehaviour();
        aestheticSpawnBehaviours[typeof(OverHeadIconManagerSpawnBehaviour)] = new OverHeadIconManagerSpawnBehaviour(ignoresSecretDoors: true);

        universalSpawnBehaviours[typeof(FloorButtonSpawnBehaviour)] = new FloorButtonSpawnBehaviour(index, weight, charismaRequirement, secretDoorFlag);
    }

}

public class HiddenButtonSpawnDetails : ButtonSpawnDetails
{
    private string secretDoorFlag;

    public HiddenButtonSpawnDetails(Vector3Int cellCoords, 
                                    string secretDoorFlag, 
                                    int index = 0,
                                    IAppearance appearance = null) :
    base(cellCoords, index: index, appearance: appearance, secretDoorFlag: secretDoorFlag)
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

    // public override void spawnActions(FloorButton floorButton)
    // {
    //     base.spawnActions(floorButton);
    //     floorButton.secretDoorFlag = secretDoorFlag;

    // }
}

public class DependantSpawnDetails : NPCSpawnDetails
{

    private string parentName;
    private Transform parent;
    private bool normalScale;

    public DependantSpawnDetails(string displayName,
                                    Vector3Int cellCoords,
                                    string parentName,
                                    Facing facing = Facing.Random,
                                    bool normalScale = false,
                                    CharacterAnimationType animationType = CharacterAnimationType.None,
                                    IAppearance appearance = null,
                                    float colliderOffset = -2f,
                                    int index = 0) :
    base(displayName, cellCoords, facing: facing, animationType: animationType, appearance: appearance, colliderOffset: colliderOffset, index: index)
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

public class SecretDoorSpawnDetails : AxisSpawnDetails
{

    protected override int layer => LayerAndTagManager.observableLayer;
    protected override string tag => LayerAndTagManager.observableTag;

    protected SecretDoorInfo secretDoorInfo;
    private string terrainSpriteName;
    private ObservableDelegate observable;
    private QuestStepActivationScript script;

    //a secret door stops spawning once it has been discovered
    public override SpawnParams spawnParams { get { return new SecretDoorObstacleSpawnParams(secretDoorInfo.secretDoorKeys); } }

    public override Dialogue dialogue
    {
        get
        {
            DialogueKey dialogueKey = DialogueKey.SuspiciousWall;

            if(secretDoorInfo != null && secretDoorInfo.customDialogueKey != DialogueKey.NoDialogue)
            {
                dialogueKey = secretDoorInfo.customDialogueKey;
            }

            Dialogue secretDoorDialogue = new Dialogue(new string[] { Constants.emptyString, displayName }, InkAssetList.getInkJSON(dialogueKey));

            if(secretDoorInfo != null)
            {
                secretDoorDialogue.variableSources.Add(secretDoorInfo);
            }

            return secretDoorDialogue;
        }
    }

    //every cell of a secret door shows its own sprite, as the old SecretDoorSpawnInfo spawned one door per cell
    public SecretDoorSpawnDetails(IAppearance appearance,
                                    string displayName,
                                    Vector3Int cellCoords,
                                    SecretDoorInfo secretDoorInfo,
                                    int size = 1,
                                    Axis axis = Axis.DescendingX,
                                    string tutorialTargetHash = "",
                                    string terrainSpriteName = "",
                                    ObservableDelegate observable = null,
                                    QuestStepActivationScript script = null,
                                    int index = 0,
                                    GateSpriteLayout layout = GateSpriteLayout.PerCell) :
    base(appearance, displayName, index, cellCoords, size, axis, tutorialTargetHash, layout)
    {
        this.secretDoorInfo = secretDoorInfo;
        this.terrainSpriteName = terrainSpriteName;
        this.observable = observable;
        this.script = script;

        //a secret door is a static wall: an animation manager would turn it to face the player when spoken to,
        //and re-applying the appearance on that turn wipes the observed tint
        aestheticSpawnBehaviours.Remove(typeof(AnimationManagerSpawnBehaviour));

        //secret doors get no mouse hover
        aestheticSpawnBehaviours.Remove(typeof(NPCMouseHoverSpawnBehaviour));

        //the icon manager outlines on every reveal key press, which would give the door away before it is observed;
        //ObservableObject handles both the observed outline and hiding the door once it is discovered
        aestheticSpawnBehaviours.Remove(typeof(OverHeadIconManagerSpawnBehaviour));

        aestheticSpawnBehaviours[typeof(ObservableObjectSpawnBehaviour)] = new ObservableObjectSpawnBehaviour(secretDoorInfo.secretDoorKeys, terrainSpriteName);
    }

    public string getPrimarySecretDoorKey()
    {
        if(secretDoorInfo == null || secretDoorInfo.secretDoorKeys.Count == 0)
        {
            return null;
        }

        return secretDoorInfo.secretDoorKeys[0];
    }


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
    //         GameObject.Destroy(secretDoor);
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
}

public class WallPatchSpawnDetails : SecretDoorSpawnDetails
{
    public WallPatchSpawnDetails(Vector3Int cellCoords,
                                    SecretDoorInfo secretDoorInfo,
                                    int size = 1,
                                    Axis axis = Axis.DescendingX,
                                    string tutorialTargetHash = "",
                                    string terrainSpriteName = "",
                                    ObservableDelegate observable = null,
                                    QuestStepActivationScript script = null,
                                    bool tall = false,
                                    int index = 0) :
    base(tall ? SpriteDescriptionList.wallPatchTall : SpriteDescriptionList.wallPatch,
            NPCNameList.wallPatch,
            cellCoords,
            secretDoorInfo,
            size,
            axis,
            tutorialTargetHash,
            terrainSpriteName,
            observable,
            script,
            index)
    {
        secretDoorInfo.description = "*This section of boards holds up the structure's ceiling. Redundancies allow a determined individual to remove some of the boards without collapsing the roof.";
        secretDoorInfo.searchChoice = "Attempt to make a hole in the wall.*";
        secretDoorInfo.successDescription = "*After a moment of planning, you understand the safest order to remove the boards.*";
        secretDoorInfo.successChoice = "Start removing boards.*";
        secretDoorInfo.failureDescription = "*You are unable to safely remove the boards.*";
        secretDoorInfo.openDescription = "*The way is open.*";
    }
}

public class VaultableOrDestroyableObjectSpawnDetails : VaultableObjectSpawnDetails
{

    //the index identifies the vaultable or destroyable object's gate, not a variant of its name
    public override string uniqueName { get { return displayName; } }

    public VaultableOrDestroyableObjectSpawnDetails(string displayName, Vector3Int cellCoords, VaultableOrDestroyableObject vaultableOrDestroyableObject, int index = 0, IAppearance appearance = null, float colliderOffset = -2f) :
    base(displayName, cellCoords, vaultableOrDestroyableObject, appearance: appearance, colliderOffset: colliderOffset, index: index)
    {
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

    public BookSpawnDetails(string displayName, Vector3Int cellCoords, int bookIndex, IAppearance appearance = null, int index = 0) :
    base(displayName, appearance: appearance, cellCoords: cellCoords, ignoresSecretDoors: true, index: index)
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

    public override SpawnParams spawnParams { get { return new HiddenTerrainSpawnParams(secretDoorKeys); } }
    public override bool spawnsOnSecretDoorActivation { get { return true; } }

    public List<string> secretDoorKeys = new List<string>();

    //the index picks the hidden terrain prefab, not a variant of its name
    public override string uniqueName { get { return displayName; } }

    //spawn details are only spawned in the area they are listed under, so the current location names the prefab's folder
    public override string prefabName { get { return HiddenTerrainList.getHiddenTerrainFolderPath(index); } }

    public HiddenTerrainSpawnDetails(string secretDoorKey = null, List<string> secretDoorKeys = null, int index = 0) :
    base(index: index)
    {
        if(secretDoorKey != null)
        {
            this.secretDoorKeys.Add(secretDoorKey);
        }

        if(secretDoorKeys != null)
        {
            this.secretDoorKeys.AddRange(secretDoorKeys);
        }
    }

    //hidden terrain is laid out like the area's own tilemaps, so it sits with them under the grid
    public override Transform parent { get { return AreaManager.getGridParent(); } }

    // public override void spawnActions(GameObject interactable)
    // {
    //     interactable.transform.localPosition = Vector3.zero;

    //     GameObjectUtil.updateGameObjectPosition(interactable);
    // }

    protected override void setBlankPrefabPosition(Vector3Int cell, Transform transform)
    {
        transform.localPosition = Vector3.zero;
    }
}
 
public class HostilityTerrainSpawnDetails : HiddenTerrainSpawnDetails
{

    private const string hostilitySecretDoorFlagPlaceholder = "Hostility-";

    //the area whose hostility decides whether this terrain shows
    public override SpawnParams spawnParams { get { return new HostilitySpawnParams(AreaManager.locationName); } }
    public override bool spawnsOnSecretDoorActivation { get { return false; } }

    public HostilityTerrainSpawnDetails(int index) :
    base(hostilitySecretDoorFlagPlaceholder+index, index: index)
    {
    }

}

public class TutorialColliderSpawnDetails : OOCSpawnDetails
{

    public override string prefabName { get { return PrefabNames.tutorialCollider; } }
    public override Transform parent { get { return AreaManager.getNonTransitionColliderParent(); } }
    protected override int layer { get { return LayerAndTagManager.tutorialLayer; } }

    public override SpawnParams spawnParams { 
                                                get
                                                { 
                                                    if(!TutorialFlags.getFlag(seenFlagName) && 
                                                        ((monsterDefeatKeyIndex >= 0 && !MonsterDefeatKeysList.monsterIsDefeated(monsterDefeatKeyIndex)) || monsterDefeatKeyIndex < 0))
                                                    {
                                                        return new InteractableSpawnParams(startSpawningFlagList);
                                                    } else
                                                    {
                                                        return new NeverSpawnParams();           
                                                    }
                                                }
                                            }

    private string seenFlagName;
    private StartSpawningAllTrueFlagList startSpawningFlagList;
    private int monsterDefeatKeyIndex;

    public TutorialColliderSpawnDetails(Vector3Int cellCoords, 
                                        string tutorialKey, 
                                        string seenFlagName, 
                                        StartSpawningAllTrueFlagList startSpawningFlagList = null,
                                        int index = 0) :
    base(cellCoords: cellCoords, index: index)
    {
        universalSpawnBehaviours[typeof(TutorialTriggerColliderSpawnBehaviour)] = new TutorialTriggerColliderSpawnBehaviour(tutorialKey);

        this.seenFlagName = seenFlagName;
        this.startSpawningFlagList = startSpawningFlagList ?? new StartSpawningAllTrueFlagList();
        this.monsterDefeatKeyIndex = -1;
    }
}

//a party member following the player, spawned on the player's cell and facing the way the player faces
public class PartyMemberTrainSpawnDetails : OOCSpawnDetails
{
    protected override int layer { get { return LayerAndTagManager.trainLayer; } }

    public override Transform parent { get { return AreaManager.getPlayerParent(); } }

    public override Vector3 localScale { get { return Constants.scaleChange; } }

    public override string uniqueName { get { return partyMember.uniqueName; } }

    //followers are created by PartyMemberTrainManager rather than an area's spawn list, so their NPC spawn params do not apply
    public override SpawnParams spawnParams { get { return new InteractableSpawnParams(); } }

    public readonly PartyMember partyMember;
    public readonly int placeInTrain;

    public PartyMemberTrainSpawnDetails(PartyMember partyMember, int placeInTrain) :
    base(partyMember.displayName,
         appearance: partyMember.stats.appearance,
         cellCoords: PlayerMovement.getInstance().getCell(),
         facing: State.playerFacing.getFacing(),
         ignoresSecretDoors: true)
    {
        this.partyMember = partyMember;
        this.placeInTrain = placeInTrain;

        //the stats are the appearance source, so a change of equipment shows on the follower
        aestheticSpawnBehaviours[typeof(AnimationManagerSpawnBehaviour)] = new AnimationManagerSpawnBehaviour(partyMember.stats, facing);

        universalSpawnBehaviours[typeof(PartyMemberMovementSpawnBehaviour)] = new PartyMemberMovementSpawnBehaviour(partyMember, placeInTrain);
    }
}

//a party member left standing on a cell by the Leadership skill, facing the way the player faces
public class PlacedPartyMemberSpawnDetails : OOCSpawnDetails
{
    //the party member tag keeps it from being talked to like the NPC it shares a layer with
    protected override string tag { get { return LayerAndTagManager.partyMemberTag; } }
    protected override int layer { get { return LayerAndTagManager.npcLayer; } }

    public override string uniqueName { get { return partyMember.uniqueName; } }

    //placed party members are created by PartyMemberPlacer rather than an area's spawn list, so their NPC spawn params do not apply
    public override SpawnParams spawnParams { get { return new InteractableSpawnParams(); } }

    public readonly PartyMember partyMember;

    public PlacedPartyMemberSpawnDetails(PartyMember partyMember, Vector3Int cellCoords) :
    base(partyMember.displayName,
         appearance: partyMember.stats.appearance,
         cellCoords: cellCoords,
         facing: State.playerFacing.getFacing(),
         tutorialTargetHash: TutorialSequenceList.placedCharacterTargetHash)
    {
        this.partyMember = partyMember;

        aestheticSpawnBehaviours[typeof(AnimationManagerSpawnBehaviour)] = new AnimationManagerSpawnBehaviour(partyMember.stats, facing);
        aestheticSpawnBehaviours[typeof(NPCMouseHoverSpawnBehaviour)] = new PlacedPartyMemberMouseHoverSpawnBehaviour();
        //the spawn params under this name belong to the party member's NPC, so the icon manager must not check them
        aestheticSpawnBehaviours[typeof(OverHeadIconManagerSpawnBehaviour)] = new OverHeadIconManagerSpawnBehaviour(ignoresSecretDoors: true);

        universalSpawnBehaviours[typeof(PlacedPartyMemberSpawnBehaviour)] = new PlacedPartyMemberSpawnBehaviour(partyMember);
    }
}

// public class GateWithKeySpawnDetails : GateSpawnDetails
// {
//     public GateWithKeySpawnDetails(string displayName, Vector3Int cellCoords, string currentArea, bool showSprite, Axis axis, GateKeyDetails gateKeyDetails, IAppearance appearance = null) :
//     base(displayName, cellCoords, currentArea, noTutorialTargetHash, showSprite, axis, new Dictionary<string, int>(), appearance: appearance,
//          gateSpawnBehaviour: new GateWithKeySpawnBehaviour(displayName, noTutorialTargetHash, new Dictionary<string, int>(), gateKeyDetails))
//     {
//     }

//     public override Dialogue getDialogue(string areaName)
//     {
//         return new SingleCharacterDialogue(displayName, InkAssetList.getInkJSON(DialogueKey.GateWithKey));
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
//     public TemporaryGateSpawnDetails(string displayName, Vector3Int cellCoords, string currentArea, Axis axis, Dictionary<string, int> statDifficulties, IAppearance appearance = null) :
//     base(displayName, cellCoords, currentArea, true, axis, statDifficulties, appearance: appearance,
//          gateSpawnBehaviour: new TemporaryGateSpawnBehaviour(displayName, statDifficulties))
//     {

//     }

//     public override Dialogue getDialogue(string areaName)
//     {
//         return DialogueList.getDialogue(DialogueList.scrubNameOfEndNumbers(displayName), areaName);
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
//     public GateWithHiddenTerrainSpawnDetails(string displayName, Vector3Int cellCoords, string currentArea, string spriteName, string tutorialTargetHash, Dictionary<string, int> statDifficulties, string hiddenTerrainFlag, IAppearance appearance = null) :
//     base(displayName, cellCoords, currentArea, spriteName, tutorialTargetHash, true, Axis.DescendingX, statDifficulties, appearance: appearance,
//          gateSpawnBehaviour: new GateWithHiddenTerrainSpawnBehaviour(displayName, tutorialTargetHash, statDifficulties, hiddenTerrainFlag))
//     {
//     }

//     public override Dialogue getDialogue(string areaName)
//     {
//         return DialogueList.getDialogue(DialogueList.scrubNameOfEndNumbers(displayName), areaName);
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

/// <summary>
///  Moved GateSpawnInfo Child Classes to here safe keeping
/// </summary>

// public class TemporaryGateSpawnInfo : GateSpawnInfo
// {

//     public TemporaryGateSpawnInfo(int gateIndex, string displayName, string currentArea, Vector3Int startCell, int size, Axis axis) :
//     base(gateIndex, displayName, currentArea, startCell, PrefabNames.portcullis1x1Path, size, axis)
//     {
//     }

//     protected override string getGateName()
//     {
//         return displayName + gateIndex;
//     }

//     public override GateSpawnDetails createSpawnDetails(Vector3Int currentCell, int index)
//     {
//         return new TemporaryGateSpawnDetails(getGateName(), currentCell, currentArea, getSpriteName(axis, index), tutorialTargetHash, //skewed(),
//          axis, statDifficulties);
//     }

// }

// public class GateWithHiddenTerrainSpawnInfo : GateSpawnInfo
// {
//     private string hiddenTerrainFlag;

//     public GateWithHiddenTerrainSpawnInfo(int gateIndex, string displayName, string currentArea, string spriteName, Vector3Int startCell, string hiddenTerrainFlag, KeyValuePair<string, int> statDifficulty) :
//     base(gateIndex, displayName, currentArea, startCell, spriteName, statDifficulty: statDifficulty)
//     {
//         this.hiddenTerrainFlag = hiddenTerrainFlag;        
//     }

//     public override GateSpawnDetails createSpawnDetails(Vector3Int currentCell, int index)
//     {
//         return new GateWithHiddenTerrainSpawnDetails(getGateName(), currentCell, currentArea, getSpriteName(axis, index), tutorialTargetHash, statDifficulties, hiddenTerrainFlag);
//     }

// }


// public class GateKeyDetails : IStoryVariableSource
// {

//     public string description = "";
//     public string keyName = "";

//     public string hostileAreaName = "";
//     public string hostilityScriptKey = "";

//     public GateKeyDetails(string description, string keyName)
//     {
//         this.description = description;
//         this.keyName = keyName;
//     }

//     public GateKeyDetails(string description, string keyName, string hostilityScriptKey, string hostileAreaName)
//     {
//         this.description = description;
//         this.keyName = keyName;

//         this.hostilityScriptKey = hostilityScriptKey;
//         this.hostileAreaName = hostileAreaName;
//     }

//     public Story addVariables(Story story)
//     {
//         story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.description, description);
//         story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.keyName, keyName);

//         story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.hostileAreaName, hostileAreaName);
//         story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.hostilityScriptKey, hostilityScriptKey);
  
//         return story;
//     }

// }

// public class GateWithKeySpawnInfo : GateSpawnInfo
// {
//     private GateKeyDetails gateKeyDetails;

//     public GateWithKeySpawnInfo(int gateIndex, string displayName, string currentArea, string spriteName, Vector3Int startCell, int size, Axis axis, GateKeyDetails gateKeyDetails) :
//     base(gateIndex, displayName, currentArea, startCell, spriteName, size, axis)
//     {
//         this.gateKeyDetails = gateKeyDetails;
//     }

//     public override GateSpawnDetails createSpawnDetails(Vector3Int currentCell, int index)
//     {
//         return new GateWithKeySpawnDetails(getGateName(), currentCell, currentArea, getSpriteName(axis, index), //skewed(),  
//         indexHasSprite(spriteName, index), axis, gateKeyDetails);
//     }
// }

// public class TemporaryGateSpawnInfo : GateSpawnInfo
// {

//     public TemporaryGateSpawnInfo(int gateIndex, string displayName, string currentArea, Vector3Int startCell, int size, Axis axis) :
//     base(gateIndex, displayName, currentArea, startCell, PrefabNames.portcullis1x1Path, size, axis)
//     {
//     }

//     protected override string getGateName()
//     {
//         return displayName + gateIndex;
//     }

//     public override GateSpawnDetails createSpawnDetails(Vector3Int currentCell, int index)
//     {
//         return new TemporaryGateSpawnDetails(getGateName(), currentCell, currentArea, getSpriteName(axis, index), tutorialTargetHash, //skewed(),
//          axis, statDifficulties);
//     }

// }

// public class GateWithHiddenTerrainSpawnInfo : GateSpawnInfo
// {
//     private string hiddenTerrainFlag;

//     public GateWithHiddenTerrainSpawnInfo(int gateIndex, string displayName, string currentArea, string spriteName, Vector3Int startCell, string hiddenTerrainFlag, KeyValuePair<string, int> statDifficulty) :
//     base(gateIndex, displayName, currentArea, startCell, spriteName, statDifficulty: statDifficulty)
//     {
//         this.hiddenTerrainFlag = hiddenTerrainFlag;        
//     }

//     public override GateSpawnDetails createSpawnDetails(Vector3Int currentCell, int index)
//     {
//         return new GateWithHiddenTerrainSpawnDetails(getGateName(), currentCell, currentArea, getSpriteName(axis, index), tutorialTargetHash, statDifficulties, hiddenTerrainFlag);
//     }

// }


// public class GateKeyDetails : IStoryVariableSource
// {

//     public string description = "";
//     public string keyName = "";

//     public string hostileAreaName = "";
//     public string hostilityScriptKey = "";

//     public GateKeyDetails(string description, string keyName)
//     {
//         this.description = description;
//         this.keyName = keyName;
//     }

//     public GateKeyDetails(string description, string keyName, string hostilityScriptKey, string hostileAreaName)
//     {
//         this.description = description;
//         this.keyName = keyName;

//         this.hostilityScriptKey = hostilityScriptKey;
//         this.hostileAreaName = hostileAreaName;
//     }

//     public Story addVariables(Story story)
//     {
//         story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.description, description);
//         story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.keyName, keyName);

//         story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.hostileAreaName, hostileAreaName);
//         story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.hostilityScriptKey, hostilityScriptKey);
  
//         return story;
//     }

// }

// public class GateWithKeySpawnInfo : GateSpawnInfo
// {
//     private GateKeyDetails gateKeyDetails;

//     public GateWithKeySpawnInfo(int gateIndex, string displayName, string currentArea, string spriteName, Vector3Int startCell, int size, Axis axis, GateKeyDetails gateKeyDetails) :
//     base(gateIndex, displayName, currentArea, startCell, spriteName, size, axis)
//     {
//         this.gateKeyDetails = gateKeyDetails;
//     }

//     public override GateSpawnDetails createSpawnDetails(Vector3Int currentCell, int index)
//     {
//         return new GateWithKeySpawnDetails(getGateName(), currentCell, currentArea, getSpriteName(axis, index), //skewed(),  
//         indexHasSprite(spriteName, index), axis, gateKeyDetails);
//     }
// }