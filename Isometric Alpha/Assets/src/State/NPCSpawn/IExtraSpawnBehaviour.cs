using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public interface IExtraSpawnBehaviour
{
    public void addBehaviour(GameObject gameObject);

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

    public void addBehaviour(GameObject gameObject)
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
    }
}

public class ContainerSpawnBehaviour: IExtraSpawnBehaviour
{
    private int index;
    private QuestStepActivationScript script;
    private string secretDoorFlag;
    private ChestType type;
    // private string chestName;

    public ContainerSpawnBehaviour(int index,
                                    QuestStepActivationScript script = null,
                                    string secretDoorFlag = null,
                                    ChestType type = ChestType.Chest,
                                    string chestName = null)
    {
        this.index = index;
        this.script = script;
        this.secretDoorFlag = secretDoorFlag;
        this.type = type;
        // this.chestName = chestName;
    }

    public void addBehaviour(GameObject gameObject)
    {
        Container chest = gameObject.AddComponent<Container>();

        chest.populate(index, type);

        chest.script = script;

        chest.setSecretDoorFlag(secretDoorFlag);
    }
}

public class DialogueTriggerSpawnBehaviour : IExtraSpawnBehaviour
{
    private string npcName;
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

    public DialogueTriggerSpawnBehaviour(string npcName, IDialogueSource dialogueSource = null, PlaySFXLogic introSFX = null, bool hasExtraSpaces = false, SpeakAtStartScript speakAtStartScript = null)
    {
        this.npcName = npcName;
        this.speakAtStartScript = speakAtStartScript;
        this.introSFX = introSFX ?? AudioClipList.getDialogueIntroSFXLogic(npcName);
        this.hasExtraSpaces = hasExtraSpaces;

        this.dialogueSource = dialogueSource;
    }

    public void addBehaviour(GameObject gameObject)
    {
        DialogueTrigger dialogueTrigger = gameObject.AddComponent<DialogueTrigger>();

        dialogueTrigger.npcName = npcName;
        dialogueTrigger.speakAtStartScript = speakAtStartScript;
        dialogueTrigger.introAudioClipLogic = introSFX;
        dialogueTrigger.dialogueSource = _DialogueSource;

        if(hasExtraSpaces)
        {
            dialogueTrigger.listenForActivationByName();
        }
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

    public void addBehaviour(GameObject gameObject)
    {
        Gate gate = gameObject.AddComponent<Gate>();

        //setKey checks whether the gate is already open, which needs the flag in place first
        gate.hiddenTerrainFlag = hiddenTerrainFlag;
        gate.setKey(gateKey);
    }
}

public class NPCMouseHoverSpawnBehaviour : IExtraSpawnBehaviour
{
    public NPCMouseHoverSpawnBehaviour()
    {
    }

    public void addBehaviour(GameObject gameObject)
    {
        MonoBehaviour monoBehaviour = gameObject.GetComponent<MonoBehaviour>();

        if(monoBehaviour == null)
        {
            return;
        }

        monoBehaviour.StartCoroutine(addBehaviourAtEndOfFrame(gameObject));
    }

    private IEnumerator addBehaviourAtEndOfFrame(GameObject gameObject)
    {
        yield return null;

        SpriteLayerRendererList rendererList = gameObject.GetComponent<SpriteLayerRendererList>();

        if(rendererList == null || rendererList.bodyCollider == null)
        {
            yield break;
        }

        NPCMouseHover mouseHover = rendererList.bodyCollider.gameObject.AddComponent<NPCMouseHover>();

        mouseHover.rendererList = rendererList;
        mouseHover.revealables = gameObject.GetComponents<IRevealable>();
    }

}

public class ObstacleSpawnBehaviour : IExtraSpawnBehaviour
{
    private string obstacleName;
    private bool ignoresSecretDoors;

    public ObstacleSpawnBehaviour(string obstacleName, bool ignoresSecretDoors = true)
    {
        this.obstacleName = obstacleName;
        this.ignoresSecretDoors = ignoresSecretDoors;
    }

    public void addBehaviour(GameObject gameObject)
    {
        Obstacle obstacle = addObstacle(gameObject);

        obstacle.obstacleName = obstacleName;

        //Awake has already hooked the obstacle up to the discovery event, so this unhooks it again
        if(ignoresSecretDoors)
        {
            obstacle.setToIgnoreSecretDoors();
        }
    }

    protected virtual Obstacle addObstacle(GameObject gameObject)
    {
        return gameObject.AddComponent<Obstacle>();
    }
}

public class OverHeadIconManagerSpawnBehaviour : IExtraSpawnBehaviour
{
    public OverHeadIconManagerSpawnBehaviour()
    {
    }

    public void addBehaviour(GameObject gameObject)
    {
        OverHeadIconManager iconManager = gameObject.AddComponent<OverHeadIconManager>();

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

public class PartyMemberDespawnListenerSpawnBehaviour : IExtraSpawnBehaviour
{
    private string npcName;
    
    public PartyMemberDespawnListenerSpawnBehaviour(string npcName)
    {
        this.npcName = npcName;
    }

    public void addBehaviour(GameObject gameObject)
    {
        PartyMemberDespawnListener listener = gameObject.AddComponent<PartyMemberDespawnListener>();

        listener.partyMemberName = npcName;
    }
}

public class TutorialTargetSpawnBehaviour : IExtraSpawnBehaviour
{
    private string tutorialTargetHash;
    
    public TutorialTargetSpawnBehaviour(string tutorialTargetHash)
    {
        this.tutorialTargetHash = tutorialTargetHash;
    }

    public void addBehaviour(GameObject gameObject)
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

    }
}

public class TutorialTriggerColliderSpawnBehaviour : IExtraSpawnBehaviour
{
    private string tutorialKey;
    
    public TutorialTriggerColliderSpawnBehaviour(string tutorialKey)
    {
        this.tutorialKey = tutorialKey;
    }

    public void addBehaviour(GameObject gameObject)
    {
        TutorialTriggerCollider tutorialCollider = gameObject.GetComponent<TutorialTriggerCollider>();
        tutorialCollider.tutorialSequenceKey = tutorialKey;
    }
}

public class TilemapOffsetSpawnBehaviour : IExtraSpawnBehaviour
{
    private float offset;

    public TilemapOffsetSpawnBehaviour(float offset)
    {
        this.offset = offset;
    }

    public void addBehaviour(GameObject gameObject)
    {
        TilemapCollider2D tilemapCollider = gameObject.GetComponent<TilemapCollider2D>();

        if(tilemapCollider != null)
        {
            tilemapCollider.offset = new Vector2(tilemapCollider.offset.x, offset);
        }
    }
}

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