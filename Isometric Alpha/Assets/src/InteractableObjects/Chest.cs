using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ChestType {Chest, Shelf }
public enum ChestState { Closed, OpenFilled, OpenEmpty }

public interface INonRevealableNameSource: INameSource
{
    public bool isRevealable();

    public static bool nameSourceIsRevealable(INameSource nameSource)
    {
        INonRevealableNameSource nonRevealableNameSource = nameSource as INonRevealableNameSource;

        if(nonRevealableNameSource == null)
        {
            return true;
        } else
        {
            return nonRevealableNameSource.isRevealable();
        }   
    }
}

public class Chest : MonoBehaviour, INonRevealableNameSource, IQuestActivationObject
{

    private static Dictionary<KeyValuePair<Facing, ChestState>, string> chestSprites;
    private static Dictionary<KeyValuePair<Facing, ChestState>, string> shelfSprites;

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateSprites()
    {
        chestSprites = new Dictionary<KeyValuePair<Facing, ChestState>, string>();

        chestSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.NorthEast, ChestState.Closed), PrefabNames.chestBackClosed);
        chestSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.NorthEast, ChestState.OpenFilled), PrefabNames.chestBackOpenFilled);
        chestSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.NorthEast, ChestState.OpenEmpty), PrefabNames.chestBackOpenEmpty);

        chestSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.NorthWest, ChestState.Closed), PrefabNames.chestBackClosed);
        chestSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.NorthWest, ChestState.OpenFilled), PrefabNames.chestBackOpenFilled);
        chestSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.NorthWest, ChestState.OpenEmpty), PrefabNames.chestBackOpenEmpty);

        chestSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.Closed), PrefabNames.chestFrontClosed);
        chestSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenFilled), PrefabNames.chestFrontOpenFilled);
        chestSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenEmpty), PrefabNames.chestFrontOpenEmpty);

        chestSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.Closed), PrefabNames.chestFrontClosed);
        chestSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenFilled), PrefabNames.chestFrontOpenFilled);
        chestSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenEmpty), PrefabNames.chestFrontOpenEmpty);

        shelfSprites = new Dictionary<KeyValuePair<Facing, ChestState>, string>();

        shelfSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.Closed), PrefabNames.shelfFrontFull);
        shelfSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenFilled), PrefabNames.shelfFrontFull);
        shelfSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenEmpty), PrefabNames.shelfFrontEmpty);

        shelfSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.Closed), PrefabNames.shelfFrontFull);
        shelfSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenFilled), PrefabNames.shelfFrontFull);
        shelfSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenEmpty), PrefabNames.shelfFrontEmpty);
    }

    private static Sprite getCurrentSprite(Facing facing, ChestState chestState, ChestType type)
    {
        switch(type)
        {
            case ChestType.Shelf:
                return Helpers.loadSpriteFromResources(shelfSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)]);
            default:
                return Helpers.loadSpriteFromResources(chestSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)]);
        }
    }

    private static string getChestOpenSFX(ChestType type)
    {
        switch(type)
        {
            case ChestType.Shelf:
                return "";
            default:
                return AudioClipList.chestOpen;
        }
    }

    private static string getChestTakeSFX(ChestType type)
    {
        switch(type)
        {
            default:
                return AudioClipList.placeInInventory;
        }
    }

    public PolygonCollider2D mouseHoverCollider;
    public Facing facing = Facing.NorthEast;
    public ChestState chestState = ChestState.Closed;
    public ChestType chestType = ChestType.Chest;

    public SpriteRenderer spriteRenderer;
    public SpriteOutline outline;

    public string secretDoorFlag;

    public int chestIndex;
    //public bool FooBar { get; protected set; }

    public int chestContentsItemType;
    public int chestContentsItemID;
    public int chestContentsItemQuantity;
    private Item chestContents;

    public Vector3 position;

    public DescriptionPanel chestItemDescriptionPanel;

    public bool activateQuestOnPickup;
    public string questName;
    public int questStep;
    public string flagOnPickUp;

    private QuestStepActivationScript script;

    public PlayerInteractionScript[] scripts;

    public GameObject targetCanvas;

    public string getName()
    {
        return chestType.ToString();
    }

    public bool isRevealable()
    {
        return !GateAndChestManager.hasBeenOpened(getChestKey());
    }

    private void Awake()
    {
        SecretDoorFlags.OnSecretDoorDiscovery.AddListener(show);
    }

    private void OnEnable()
    {
        outline = new SpriteOutline();
        outline.setSpriteRenderer(spriteRenderer);
    }

    private void OnDestroy()
    {
        SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(show);
    }

    private void show(string secretDoorFlag)
    {
        if(this.secretDoorFlag != null && this.secretDoorFlag.Equals(secretDoorFlag))
        {
            gameObject.SetActive(true);
        }
    }

    public void populate(int index, Facing facing, ChestType type)
    {
        chestIndex = index;

        this.facing = facing;
        chestType = type;
        setMouseHoverPosition();

        chestContents = ChestItemIDList.getChestItem(AreaManager.locationName, chestIndex);

        if (GateAndChestManager.hasBeenOpened(getChestKey()))
        {
            setSpriteToOpenEmpty(ignoreSFX: true);
        }
        else
        {
            setSpriteToClosed();
        }
    }
    
    private void setToCurrentSprite()
    {
        spriteRenderer.sprite = getCurrentSprite(facing, chestState, chestType);

        switch(facing)
        {
            case Facing.NorthEast:
            case Facing.SouthWest:
                spriteRenderer.flipX = true;
                break;
            case Facing.NorthWest:
            case Facing.SouthEast:
                spriteRenderer.flipX = false;
                break;
        }    

        setMouseHoverPosition();
    }

    private void setMouseHoverPosition()
    {
        Helpers.updatePolygonCollider(spriteRenderer, mouseHoverCollider);
        // Helpers.updateGameObjectPosition(gameObject);
    }

    public void playerOpensChest()
    {
        PopUpScreenBlockerManager.spawnPopUpScreenBlocker();

        NotificationManager.OnDeleteAllNotifications.Invoke();

        AudioManager.playSFX(getChestOpenSFX(chestType));

        createChestItemUI();

        Inventory.addItem(chestContents);

        setSpriteToOpenFilled();
        outline.removeOutline();

        GateAndChestManager.addKey(getChestKey());

        PlayerInteractionScript.runAllScripts(scripts);

        PlayerOOCStateManager.OnStateChangeFromInChestUI.AddListener(destroyUI);
        PlayerOOCStateManager.OnStateChangeFromInChestUI.AddListener(setSpriteToOpenEmpty);

        if(script != null)
        {
            script.runScript();
        }
    }

    private void createChestItemUI()
    {
        RectTransform rectTransform = Instantiate(Resources.Load<GameObject>(PrefabNames.descriptionPanelBuildingBlockItem), PlayerObject.getUIParentTransform()).GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(.0075f, .0075f);

        chestItemDescriptionPanel = rectTransform.GetComponent<DescriptionPanel>();
        chestContents.describeSelfRow(chestItemDescriptionPanel);
    }

    public void destroyUI()
    {
        DestroyImmediate(chestItemDescriptionPanel.gameObject);
        PopUpScreenBlockerManager.destroyPopUpScreenBlocker();
    }

    private void setSpriteToClosed()
    {
        chestState = ChestState.Closed;
        setToCurrentSprite();
    }

    private void setSpriteToOpenFilled()
    {
        chestState = ChestState.OpenFilled;
        setToCurrentSprite();
    }

    public void setSpriteToOpenEmpty()
    {
        setSpriteToOpenEmpty(false);
    }

    public void setSpriteToOpenEmpty(bool ignoreSFX = false)
    {
        chestState = ChestState.OpenEmpty;
        setToCurrentSprite();
        PlayerOOCStateManager.OnStateChangeFromInChestUI.RemoveListener(destroyUI);
        PlayerOOCStateManager.OnStateChangeFromInChestUI.RemoveListener(setSpriteToOpenEmpty);

        if(!ignoreSFX)
        {
            AudioManager.playSFX(getChestTakeSFX(chestType));
        }
    }

    private string getChestKey()
    {
        return AreaManager.locationName + "-chest-" + chestIndex;
    }

    public bool hasBeenOpened()
    {
        return chestState != ChestState.Closed;
    }

    public void setScript(QuestStepActivationScript script)
    {
        this.script = script;
    }

}
