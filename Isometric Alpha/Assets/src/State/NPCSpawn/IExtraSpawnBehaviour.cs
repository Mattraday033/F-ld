using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public interface IExtraSpawnBehaviour
{
    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements { get; }

    public Component addBehaviour(GameObject gameObject);
}

public static class IExtraSpawnBehaviourExtensions
{
    public static void applyActivationRequirements(this IExtraSpawnBehaviour behaviour, ActivationListener listener)
    {
        foreach(KeyValuePair<ActivationDesignatorType, ActivationCategory> kvp in behaviour.activationRequirements)
        {
            switch(kvp.Key)
            {
                case ActivationDesignatorType.Name:
                    listener.listenForActivationByName(kvp.Value);
                    break;
                case ActivationDesignatorType.Index:
                    listener.listenForActivationByIndex(kvp.Value);
                    break;
            }
        }
    }
}

public class ActivationListenerSpawnBehaviour : IExtraSpawnBehaviour
{
    //for requirements that belong to the spawned object itself rather than to one of its behaviours
    public ActivationListenerSpawnBehaviour(KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements = null)
    {
        this._ActivationRequirements = activationRequirements ?? _ActivationRequirements;
    }

    private KeyValuePair<ActivationDesignatorType, ActivationCategory>[] _ActivationRequirements = new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];
    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => _ActivationRequirements;

    public Component addBehaviour(GameObject gameObject)
    {
        return gameObject.AddComponent<ActivationListener>();
    }

    public static Component addActivationListener(GameObject gameObject)
    {
        return gameObject.AddComponent<ActivationListener>();
    }
}

public class AnimationManagerSpawnBehaviour : IExtraSpawnBehaviour
{
    
    private IAppearanceSource appearanceSource;
    private Facing facing;
    private CharacterAnimationType animationType;

    public AnimationManagerSpawnBehaviour(IAppearanceSource appearanceSource, Facing facing, CharacterAnimationType animationType = CharacterAnimationType.None)
    {
        this.appearanceSource = appearanceSource;
        this.facing = facing;
        this.animationType = animationType;
    }

    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

    public Component addBehaviour(GameObject gameObject)
    {
        NewAnimationManager animationManager = gameObject.AddComponent<NewAnimationManager>();

        animationManager.appearanceSource = appearanceSource;
        animationManager.characterFacing.currentFacing = facing;

        if(animationType != CharacterAnimationType.None)
        {
            animationManager.playAnimation(animationType);
        } else
        {
            animationManager.handleMovementAnimation();
        }

        return animationManager;
    }
}

public class CombatantSpawnBehaviour : IExtraSpawnBehaviour
{
    private CombatantSpawnDetails spawnDetails;

    public CombatantSpawnBehaviour(CombatantSpawnDetails spawnDetails)
    {
        this.spawnDetails = spawnDetails;
    }

    //combatants don't listen for activation yet; a requirement added here would apply to every GameObject of the combatant
    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

    public Component addBehaviour(GameObject gameObject)
    {
        Combatant combatant = gameObject.AddComponent<Combatant>();

        GridCoords cell = spawnDetails.getCell(gameObject);

        combatant.initialize(spawnDetails.stats, cell, spawnDetails.isPrimaryCell(cell), spawnDetails.isPlaceholder);

        return combatant;
    }
}

public class CombatantHoverSpawnBehaviour : IExtraSpawnBehaviour
{
    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

    //the hover reacts to OnMouseEnter, so it has to share a GameObject with the body's collider
    public Component addBehaviour(GameObject gameObject)
    {
        SpriteLayerRendererList rendererList = gameObject.GetComponent<SpriteLayerRendererList>();

        GameObject hoverObject = rendererList != null && rendererList.bodyCollider != null ? rendererList.bodyCollider.gameObject : gameObject;

        return hoverObject.AddComponent<CombatantHover>();
    }
}

public class ContainerSpawnBehaviour: IExtraSpawnBehaviour
{
    private int index;
    private QuestStepActivationScript script;
    private string secretDoorFlag;
    private ContainerType type;
    // private string chestName;

    public ContainerSpawnBehaviour(int index,
                                    QuestStepActivationScript script = null,
                                    string secretDoorFlag = null,
                                    ContainerType type = ContainerType.Chest,
                                    string chestName = null)
    {
        this.index = index;
        this.script = script;
        this.secretDoorFlag = secretDoorFlag;
        this.type = type;
        // this.chestName = chestName;
    }

    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

    public Component addBehaviour(GameObject gameObject)
    {
        Container chest = gameObject.AddComponent<Container>();

        chest.populate(index, type);

        chest.script = script;

        chest.setSecretDoorFlag(secretDoorFlag);

        return chest;
    }
}

public class CunningObjectSpawnBehaviour : IExtraSpawnBehaviour
{
    private int index;
    private CunningAction cunningAction;
    private CunningObjectSpriteCategory category;
    private QuestStepActivationScript script;

    public CunningObjectSpawnBehaviour(int index,
                                        CunningAction cunningAction,
                                        CunningObjectSpriteCategory category,
                                        QuestStepActivationScript script = null)
    {
        this.index = index;
        this.category = category;
        this.script = script;
        this.cunningAction = cunningAction;
    }

    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

    public Component addBehaviour(GameObject gameObject)
    {
        CunningObject cunningObject = gameObject.AddComponent<CunningObject>();

        cunningObject.rendererList = gameObject.GetComponent<SpriteLayerRendererList>();
        cunningObject.index = index;
        cunningObject.script = script;
        cunningObject.cunningAction = cunningAction;
        cunningObject.category = category;

        //matches the saved state CunningObjectAppearance already drew the sprite from
        cunningObject.evenActivation = TrapAndButtonStateManager.contains(cunningObject.getKey());

        return cunningObject;
    }

}

public class DialogueTriggerSpawnBehaviour : IExtraSpawnBehaviour
{
    private string uniqueName;
    private SpeakAtStartScript speakAtStartScript;
    private PlaySFXLogic introSFX;
    private bool hasExtraSpaces;

    private IDialogueSource _DialogueSource;
    public IDialogueSource dialogueSource
    {
        set
        {
           _DialogueSource = value;
        }
    }

    public DialogueTriggerSpawnBehaviour(string uniqueName, IDialogueSource dialogueSource = null, PlaySFXLogic introSFX = null, bool hasExtraSpaces = false, SpeakAtStartScript speakAtStartScript = null)
    {
        this.uniqueName = uniqueName;
        this.speakAtStartScript = speakAtStartScript;
        this.introSFX = introSFX ?? AudioClipList.getDialogueIntroSFXLogic(uniqueName);
        this.hasExtraSpaces = hasExtraSpaces;

        this.dialogueSource = dialogueSource;
    }

    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[]
    {
        new KeyValuePair<ActivationDesignatorType, ActivationCategory>(ActivationDesignatorType.Name, ActivationCategory.Dialogue)
    };

    public Component addBehaviour(GameObject gameObject)
    {
        DialogueTrigger dialogueTrigger = gameObject.AddComponent<DialogueTrigger>();

        dialogueTrigger.uniqueName = uniqueName;
        dialogueTrigger.speakAtStartScript = speakAtStartScript;
        dialogueTrigger.introAudioClipLogic = introSFX;
        dialogueTrigger.dialogueSource = _DialogueSource;

        return dialogueTrigger;
    }
}

public class EnemyMovementSpawnBehaviour : IExtraSpawnBehaviour
{
    private MonsterSpawnDetails spawnDetails;

    //the details are read when the monster spawns, as its pack index is only known once its place in the area's list is
    public EnemyMovementSpawnBehaviour(MonsterSpawnDetails spawnDetails)
    {
        this.spawnDetails = spawnDetails;
    }

    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

    public Component addBehaviour(GameObject gameObject)
    {
        EnemyMovement enemyMovement = addMovement(gameObject);

        //the movement index and the pack lookups both come from the pack index, so it has to be in place first
        enemyMovement.setMonsterPackIndex(spawnDetails.index);

        enemyMovement.rendererList = gameObject.GetComponent<SpriteLayerRendererList>();
        enemyMovement.iconManager = gameObject.GetComponent<OverHeadIconManager>();
        enemyMovement.attachedCollider2D = gameObject.GetComponent<TilemapCollider2D>();

        enemyMovement.movementType = spawnDetails.movementType;

        //the animation manager arrives with the aesthetic behaviours, so its OnEnable ran before this tracker existed to be found
        NewAnimationManager animationManager = gameObject.GetComponent<NewAnimationManager>();

        if(animationManager != null)
        {
            enemyMovement.animationManager = animationManager;
            animationManager.movementTracker = enemyMovement;
        }

        MovementManager.addMovementTracker(enemyMovement);

        enemyMovement.applyRetreatStun();

        return enemyMovement;
    }

    protected virtual EnemyMovement addMovement(GameObject gameObject)
    {
        return gameObject.AddComponent<EnemyMovement>();
    }
}

public class MovableObjectMovementSpawnBehaviour : EnemyMovementSpawnBehaviour
{
    public MovableObjectMovementSpawnBehaviour(MonsterSpawnDetails spawnDetails) :
    base(spawnDetails)
    {
    }

    protected override EnemyMovement addMovement(GameObject gameObject)
    {
        return gameObject.AddComponent<MovableObjectMovement>();
    }
}

public class FloorButtonSpawnBehaviour : IExtraSpawnBehaviour
{
    private int index;
    private int weight;
    private int charismaRequirement;
    private string secretDoorFlag;

    public FloorButtonSpawnBehaviour(int index, int weight, int charismaRequirement, string secretDoorFlag = null)
    {
        this.index = index;
        this.weight = weight;
        this.charismaRequirement = charismaRequirement;
        this.secretDoorFlag = secretDoorFlag;
    }

    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

    public Component addBehaviour(GameObject gameObject)
    {
        FloorButton floorButton = gameObject.AddComponent<FloorButton>();

        floorButton.index = index;
        floorButton.weight = weight;
        floorButton.charismaRequirement = charismaRequirement;
        floorButton.secretDoorFlag = secretDoorFlag;

        floorButton.rendererList = gameObject.GetComponent<SpriteLayerRendererList>();

        //the prefab's tile collider covers the button's cell, so anything standing on it presses the button
        floorButton.collider = gameObject.GetComponent<TilemapCollider2D>();

        return floorButton;
    }
}

public class GateSpawnBehaviour : IExtraSpawnBehaviour
{
    private string gateKey;
    private string hiddenTerrainFlag;

    public GateSpawnBehaviour(string gateKey, string hiddenTerrainFlag = null)
    {
        this.gateKey = gateKey;
        this.hiddenTerrainFlag = hiddenTerrainFlag;
    }

    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

    public Component addBehaviour(GameObject gameObject)
    {
        Gate gate = gameObject.AddComponent<Gate>();

        //setKey checks whether the gate is already open, which needs the flag in place first
        gate.hiddenTerrainFlag = hiddenTerrainFlag;
        gate.setKey(gateKey);

        return gate;
    }
}

public class NPCMouseHoverSpawnBehaviour : IExtraSpawnBehaviour
{
    public NPCMouseHoverSpawnBehaviour()
    {
    }

    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

    //the hover component is only added next frame, so there is nothing to return yet
    public Component addBehaviour(GameObject gameObject)
    {
        MonoBehaviour monoBehaviour = gameObject.GetComponent<MonoBehaviour>();

        if(monoBehaviour == null)
        {
            return null;
        }

        monoBehaviour.StartCoroutine(addBehaviourAtEndOfFrame(gameObject));

        return null;
    }

    private IEnumerator addBehaviourAtEndOfFrame(GameObject gameObject)
    {
        yield return null;

        SpriteLayerRendererList rendererList = gameObject.GetComponent<SpriteLayerRendererList>();

        if(rendererList == null || rendererList.bodyCollider == null)
        {
            yield break;
        }

        NPCMouseHover mouseHover = addMouseHover(gameObject, rendererList.bodyCollider.gameObject);

        mouseHover.rendererList = rendererList;
        mouseHover.revealables = gameObject.GetComponents<IRevealable>();
    }

    protected virtual NPCMouseHover addMouseHover(GameObject gameObject, GameObject bodyColliderObject)
    {
        return bodyColliderObject.AddComponent<NPCMouseHover>();
    }

}

public class ObservableObjectSpawnBehaviour : IExtraSpawnBehaviour
{
    private List<string> secretDoorKeys;
    private string terrainSpriteName;

    public ObservableObjectSpawnBehaviour(List<string> secretDoorKeys, string terrainSpriteName = null)
    {
        this.secretDoorKeys = secretDoorKeys;
        this.terrainSpriteName = terrainSpriteName;
    }

    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

    public Component addBehaviour(GameObject gameObject)
    {
        ObservableObject observableObject = gameObject.AddComponent<ObservableObject>();

        observableObject.secretDoorKeys = secretDoorKeys;

        if(!string.IsNullOrEmpty(terrainSpriteName))
        {
            observableObject.terrainSprite = SpriteUtil.loadSpriteFromResources(terrainSpriteName);
        }

        return observableObject;
    }
}

public class ObstacleSpawnBehaviour : IExtraSpawnBehaviour
{
    private string uniqueName;
    private bool ignoresSecretDoors;

    public ObstacleSpawnBehaviour(string uniqueName, bool ignoresSecretDoors = true, KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements = null)
    {
        this.uniqueName = uniqueName;
        this.ignoresSecretDoors = ignoresSecretDoors;

        this._ActivationRequirements = activationRequirements ?? _ActivationRequirements;
    }

    private KeyValuePair<ActivationDesignatorType, ActivationCategory>[] _ActivationRequirements = new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];
    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => _ActivationRequirements;

    public Component addBehaviour(GameObject gameObject)
    {
        Obstacle obstacle = addObstacle(gameObject);

        obstacle.uniqueName = uniqueName;

        //Awake has already hooked the obstacle up to the discovery event, so this unhooks it again
        if(ignoresSecretDoors)
        {
            obstacle.setToIgnoreSecretDoors();
        }

        return obstacle;
    }

    protected virtual Obstacle addObstacle(GameObject gameObject)
    {
        return gameObject.AddComponent<Obstacle>();
    }
}

public class OverHeadIconManagerSpawnBehaviour : IExtraSpawnBehaviour
{
    private bool ignoresSecretDoors;

    //objects that manage their own visibility, like containers hidden behind a secret door, should not be toggled by the icon manager's spawn params check
    public OverHeadIconManagerSpawnBehaviour(bool ignoresSecretDoors = false)
    {
        this.ignoresSecretDoors = ignoresSecretDoors;
    }

    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

    public Component addBehaviour(GameObject gameObject)
    {
        OverHeadIconManager iconManager = gameObject.AddComponent<OverHeadIconManager>();

        //Awake has already hooked the manager up to the discovery event, so this unhooks it again
        if(ignoresSecretDoors)
        {
            iconManager.setToIgnoreSecretDoors();
        }

        GameObject formatter = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.overHeadIconFormatter), gameObject.transform);
        formatter.transform.localPosition = Vector3.zero;
        GameObjectUtil.updateGameObjectPosition(formatter);

        if(gameObject.GetComponent<GraphicRaycasterShield>() == null)
        {
            gameObject.AddComponent<GraphicRaycasterShield>();
        }

        iconManager.iconParent = formatter.transform;
        iconManager.canvas = formatter.GetComponent<Canvas>();

        iconManager.setRendererList(gameObject.GetComponent<SpriteLayerRendererList>());

        iconManager.StartCoroutine(assignNameSourceAtEndOfFrame(iconManager));

        return iconManager;
    }

    //the name source arrives with the universal spawn behaviours, which run after the aesthetic ones
    private IEnumerator assignNameSourceAtEndOfFrame(OverHeadIconManager iconManager)
    {
        yield return new WaitForEndOfFrame();

        foreach(INameSource nameSource in iconManager.GetComponents<INameSource>())
        {
            //the manager is an INameSource itself, and delegates to whichever one sits beside it
            if(!ReferenceEquals(nameSource, iconManager))
            {
                iconManager.nameSource = nameSource;
                yield break;
            }
        }
    }
}

public class PartyMemberMovementSpawnBehaviour : IExtraSpawnBehaviour
{
    private PartyMember partyMember;
    private int placeInTrain;

    public PartyMemberMovementSpawnBehaviour(PartyMember partyMember, int placeInTrain)
    {
        this.partyMember = partyMember;
        this.placeInTrain = placeInTrain;
    }

    //a follower only belongs in the train while its party member is in the formation
    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[]
    {
        ActivationRequirementList.leftFormationByName
    };

    public Component addBehaviour(GameObject gameObject)
    {
        PartyMemberMovement partyMemberMovement = gameObject.AddComponent<PartyMemberMovement>();

        partyMemberMovement.partyMember = partyMember;
        partyMemberMovement.placeInTrain = placeInTrain;

        //the animation manager arrives with the aesthetic behaviours, so its OnEnable ran before this tracker existed to be found
        NewAnimationManager animationManager = gameObject.GetComponent<NewAnimationManager>();

        if(animationManager != null)
        {
            animationManager.movementTracker = partyMemberMovement;
        }

        return partyMemberMovement;
    }
}

public class PlacedPartyMemberMouseHoverSpawnBehaviour : NPCMouseHoverSpawnBehaviour
{
    //the hover has to know when its party member is hidden, so it does not bring the body collider back under the player
    protected override NPCMouseHover addMouseHover(GameObject gameObject, GameObject bodyColliderObject)
    {
        PlacedPartyMemberMouseHover mouseHover = bodyColliderObject.AddComponent<PlacedPartyMemberMouseHover>();

        mouseHover.placedPartyMember = gameObject.GetComponent<PlacedPartyMember>();

        return mouseHover;
    }
}

public class PlacedPartyMemberSpawnBehaviour : IExtraSpawnBehaviour
{
    private PartyMember partyMember;

    public PlacedPartyMemberSpawnBehaviour(PartyMember partyMember)
    {
        this.partyMember = partyMember;
    }

    //has to stay empty: the placed party member shares its name with its NPC, whose dialogue would otherwise switch it on and off
    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

    public Component addBehaviour(GameObject gameObject)
    {
        PlacedPartyMember placedPartyMember = gameObject.AddComponent<PlacedPartyMember>();

        placedPartyMember.partyMember = partyMember;

        return placedPartyMember;
    }
}

public class RestStopSpawnBehaviour : IExtraSpawnBehaviour
{
    public RestStopSpawnBehaviour()
    {
    }

    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

    public Component addBehaviour(GameObject gameObject)
    {
        return gameObject.AddComponent<RestStop>();
    }
}

public class ShopkeeperSpawnBehaviour : IExtraSpawnBehaviour
{
    private string shopkeeperInventoryKey;

    public ShopkeeperSpawnBehaviour(string shopkeeperInventoryKey)
    {
        this.shopkeeperInventoryKey = shopkeeperInventoryKey;
    }

    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

    public Component addBehaviour(GameObject gameObject)
    {
        Shopkeeper shopkeeper = gameObject.AddComponent<Shopkeeper>();

        shopkeeper.shopkeeperInventoryKey = shopkeeperInventoryKey;

        return shopkeeper;
    }
}

public class TutorialTargetSpawnBehaviour : IExtraSpawnBehaviour
{
    private string tutorialTargetHash;
    
    public TutorialTargetSpawnBehaviour(string tutorialTargetHash)
    {
        this.tutorialTargetHash = tutorialTargetHash;
    }

    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

    public Component addBehaviour(GameObject gameObject)
    {
        GameObject targetRect = GameObjectUtil.createBlankGameObject(gameObject);

        RectTransform rectTransform = targetRect.AddComponent<RectTransform>();

        rectTransform.anchorMin = Vector2Int.zero;
        rectTransform.anchorMax = Vector2Int.one;
        rectTransform.pivot = new Vector2(.5f, .5f);

        rectTransform.offsetMin = Vector2Int.zero;
        rectTransform.offsetMax = Vector2Int.zero;

        GameObjectUtil.updateGameObjectPosition(targetRect);

        TutorialSequenceStepTargetSprite targetSprite = targetRect.AddComponent<TutorialSequenceStepTargetSprite>();
        targetSprite.tutorialHash = tutorialTargetHash;
        targetSprite.rendererList = gameObject.GetComponent<SpriteLayerRendererList>();
        targetSprite.disableArrow = true;

        return targetSprite;
    }
}

public class TutorialTriggerColliderSpawnBehaviour : IExtraSpawnBehaviour
{
    private string tutorialKey;
    
    public TutorialTriggerColliderSpawnBehaviour(string tutorialKey)
    {
        this.tutorialKey = tutorialKey;
    }

    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

    public Component addBehaviour(GameObject gameObject)
    {
        TutorialTriggerCollider tutorialCollider = gameObject.GetComponent<TutorialTriggerCollider>();
        tutorialCollider.tutorialSequenceKey = tutorialKey;

        return tutorialCollider;
    }
}

public class TilemapOffsetSpawnBehaviour : IExtraSpawnBehaviour
{
    private float offset;

    public TilemapOffsetSpawnBehaviour(float offset)
    {
        this.offset = offset;
    }

    public KeyValuePair<ActivationDesignatorType, ActivationCategory>[] activationRequirements => new KeyValuePair<ActivationDesignatorType, ActivationCategory>[0];

    public Component addBehaviour(GameObject gameObject)
    {
        TilemapCollider2D tilemapCollider = gameObject.GetComponent<TilemapCollider2D>();

        if(tilemapCollider != null)
        {
            tilemapCollider.offset = new Vector2(tilemapCollider.offset.x, offset);
        }

        return tilemapCollider;
    }
}

    // public override void spawnActions(GameObject tutorialColliderGameObject)
    // {
    //     if (shouldNotSpawn())
    //     {
    //         GameObject.Destroy(tutorialColliderGameObject);
    //         return;
    //     }

    //     TutorialTriggerCollider tutorialCollider = tutorialColliderGameObject.GetComponent<TutorialTriggerCollider>();
    //     tutorialCollider.tutorialSequenceKey = tutorialKey;
    // }

    //     //the icons live on their own world space canvas so they can sort above everything the character stands in front of
    // private void buildIconParent(GameObject gameObject, OverHeadIconManager iconManager)
    // {
    //     GameObject iconParent = GameObjectUtil.createBlankGameObject(gameObject, iconParentObjectName);

    //     iconParent.layer = LayerAndTagManager.UILayer;

    //     RectTransform rectTransform = iconParent.AddComponent<RectTransform>();

    //     rectTransform.anchorMin = Vector2Int.zero;
    //     rectTransform.anchorMax = Vector2Int.one;
    //     rectTransform.pivot = new Vector2(.5f, .5f);

    //     rectTransform.offsetMin = Vector2Int.zero;
    //     rectTransform.offsetMax = Vector2Int.zero;

    //     Canvas canvas = iconParent.AddComponent<Canvas>();

    //     canvas.renderMode = RenderMode.WorldSpace;
    //     canvas.overrideSorting = true;
    //     canvas.sortingLayerName = LayerAndTagManager.thirteenthSortingLayerName;
    //     canvas.sortingOrder = Constants.indexOne;

    //     iconParent.AddComponent<GraphicRaycaster>();

    //     //stops a pointer that is over an icon from also reaching the character underneath it
    //     iconParent.AddComponent<GraphicRaycasterShield>();

    //     HorizontalLayoutGroup layoutGroup = iconParent.AddComponent<HorizontalLayoutGroup>();

    //     layoutGroup.spacing = iconSpacing;
    //     layoutGroup.childAlignment = TextAnchor.UpperLeft;
    //     layoutGroup.childControlWidth = true;
    //     layoutGroup.childControlHeight = true;
    //     layoutGroup.childForceExpandWidth = true;
    //     layoutGroup.childForceExpandHeight = true;

    //     ContentSizeFitter sizeFitter = iconParent.AddComponent<ContentSizeFitter>();

    //     sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
    //     sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

    //     GameObjectUtil.updateGameObjectPosition(iconParent);

    //     iconManager.iconParent = rectTransform;
    //     iconManager.canvas = canvas;
    // }