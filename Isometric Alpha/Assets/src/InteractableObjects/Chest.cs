using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

public class Chest : MonoBehaviour, INonRevealableNameSource
{

    private const string tagText = "Chest";

    private readonly static Vector2 mouseHoverOffsetNE = new Vector2(0.11f, -0.025f);
    private readonly static Vector2 mouseHoverOffsetNW = new Vector2(-0.11f,-0.025f);
    private readonly static Vector2 mouseHoverOffsetSE = new Vector2(-0.075f,0.075f);
    private readonly static Vector2 mouseHoverOffsetSW = new Vector2(0.075f,0.075f);

    private readonly static Vector2 mouseHoverOffsetLarge = new Vector2(-0.075f,0.475f);

    private readonly static Vector2 mouseHoverSmallSize = new Vector2(0.65f,0.65f);
    private readonly static Vector2 mouseHoverLargeSize = new Vector2(0.7f,1.35f);

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

    public BoxCollider2D mouseHoverCollider;
    public Facing facing = Facing.NorthEast;
    public ChestState chestState = ChestState.Closed;
    public ChestType chestType = ChestType.Chest;

    public SpriteRenderer spriteRenderer;
    public SpriteOutline outline;

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

    private void OnEnable()
    {
        // createListeners();
        outline = new SpriteOutline();
        outline.setSpriteRenderer(spriteRenderer);
    }

    // private void OnDisable()
    // {
    //     destroyListeners();
    // }

    public void populate(int index, Facing facing, ChestType type)
    {
        chestIndex = index;

        this.facing = facing;
        chestType = type;
        setMouseHoverPosition();

        chestContents = ChestItemIDList.getChestItem(AreaManager.locationName, chestIndex);

        if (GateAndChestManager.hasBeenOpened(getChestKey()))
        {
            setSpriteToOpenEmpty();
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
        switch(chestType)
        {
            case ChestType.Shelf:
                mouseHoverCollider.size = mouseHoverLargeSize;
                mouseHoverCollider.offset = mouseHoverOffsetLarge;
                break;
            default:
                mouseHoverCollider.size = mouseHoverSmallSize;
                switch(facing)
                {
                    case Facing.NorthEast:
                        mouseHoverCollider.offset = mouseHoverOffsetNE;
                        break;
                    case Facing.NorthWest:
                        mouseHoverCollider.offset = mouseHoverOffsetNW;
                        break;
                    case Facing.SouthEast:
                        mouseHoverCollider.offset = mouseHoverOffsetSE;
                        break;
                    case Facing.SouthWest:
                        mouseHoverCollider.offset = mouseHoverOffsetSW;
                        break;
                }
                break;;
        }

        Helpers.updateGameObjectPosition(gameObject);
    }

    public void playerOpensChest()
    {
        PopUpScreenBlockerManager.spawnPopUpScreenBlocker();

        NotificationManager.OnDeleteAllNotifications.Invoke();

        createChestItemUI();

        Inventory.addItem(chestContents);

        setSpriteToOpenFilled();
        outline.removeOutline();

        GateAndChestManager.addKey(getChestKey());

        PlayerInteractionScript.runAllScripts(scripts);

        if (questName != null && !questName.Equals("") && (activateQuestOnPickup || QuestList.getQuest(questName).active))
        {
            QuestList.activateQuestStep(questName, questStep);
        }

        if (flagOnPickUp != null && flagOnPickUp.Length > 0)
        {
            Flags.setFlag(flagOnPickUp, true);
        }
    }

    private void createChestItemUI()
    {
        chestItemDescriptionPanel = Instantiate(Resources.Load<GameObject>(PrefabNames.chestDescriptionPanel), PlayerMovement.getUIParentTransform()).GetComponent<DescriptionPanel>();
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
        chestState = ChestState.OpenEmpty;
        setToCurrentSprite();
    }

    private string getChestKey()
    {
        return AreaManager.locationName + "-chest-" + chestIndex;
    }

    public bool hasBeenOpened()
    {
        return chestState != ChestState.Closed;
    }

    //IRevealable interface methods

    // public SpriteOutline getSpriteOutline()
    // {
    //     return outline;
    // }

    // public void createListeners()
    // {
    //     RevealManager.OnReveal.AddListener(onReveal);
    // }

    // public void destroyListeners()
    // {
    //     RevealManager.OnReveal.RemoveListener(onReveal);
    // }

    // public void onReveal(bool toggleReveal)
    // {
    //     if (!hasBeenOpened())
    //     {
    //         if(toggleReveal)
    //         {
    //             outline.createOutline(getRevealColor(), getOutlineSize());
    //         } else
    //         {
    //             outline.removeOutline();
    //         }
    //     }
    // }

    // public Color getRevealColor()
    // {
    //     return ColorList.canBeInteractedWith;
    // }

	// public OutlineMode getOutlineSize()
    // {
    //     return OutlineMode.Bold;
    // }

    // public void createHoverTag()
    // {
    //     MouseHoverManager.getMouseHoverBase();
    //     MouseHoverManager.createHoverTag(tagText);
    // }

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     if (eventData != null && eventData.used)
    //     {
    //         return;
    //     }

    //     if (!RevealManager.currentlyRevealed && !hasBeenOpened())
    //     {
    //         outline.createOutline(getRevealColor(), getOutlineSize());
    //     }

    //     if (!hasBeenOpened())
    //     {
    //         createHoverTag();
    //     }
    // }

    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     if (eventData != null && eventData.used)
    //     {
    //         return;
    //     }

    //     if (!RevealManager.currentlyRevealed && !hasBeenOpened())
    //     {
    //         outline.removeOutline();
    //     }

    //     if (!hasBeenOpened())
    //     {
    //         MouseHoverManager.destroyMouseHoverBase();
    //     }
    // }
}
