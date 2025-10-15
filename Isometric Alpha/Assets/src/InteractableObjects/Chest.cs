using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum ChestState {Closed, OpenFilled, OpenEmpty }

public class Chest : MonoBehaviour, IRevealable
{

    private static Dictionary<KeyValuePair<Facing, ChestState>, string> chestSprites;

    private const string tagText = "Chest";

    private static Vector2 mouseHoverOffsetNE = new Vector2(0.11f, -0.025f);
    private static Vector2 mouseHoverOffsetNW = new Vector2(-0.11f,-0.025f);
    private static Vector2 mouseHoverOffsetSE = new Vector2(-0.075f,0.075f);
    private static Vector2 mouseHoverOffsetSW = new Vector2(0.075f,0.075f);

    static Chest()
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
    }

    private static Sprite getCurrentSprite(Facing facing, ChestState chestState)
    {
        return Helpers.loadSpriteFromResources(chestSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)]);
    }

    public BoxCollider2D mouseHoverCollider;
    public Facing facing = Facing.NorthEast;
    public ChestState chestState = ChestState.Closed;

    public SpriteRenderer spriteRenderer;
    public Collider2D collider;

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

    private void Awake()
    {
        spawnTargetCanvas();
    }

    private void OnEnable()
    {
        createListeners();
    }

    private void OnDisable()
    {
        destroyListeners();
    }

    void Start()
    {

    }

    public void populate(int index, Facing facing)
    {
        this.facing = facing;
        setMouseHoverOffset();

        chestContents = ChestItemIDList.getChestItem(AreaManager.locationName, index);

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
        spriteRenderer.sprite = getCurrentSprite(facing, chestState);

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
    }

    private void setMouseHoverOffset()
    {
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

    }

    public void playerOpensChest()
    {
        PopUpBlocker.spawnPopUpScreenBlocker();

        NotificationManager.OnDeleteAllNotifications.Invoke();

        createChestItemUI();

        Inventory.addItem(chestContents);

        setSpriteToOpenFilled();
        RevealManager.setOutlineColorToDefault(gameObject);

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
        PopUpBlocker.destroyPopUpScreenBlocker();
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

    public void createListeners()
    {
        RevealManager.OnReveal.AddListener(onReveal);
    }

    public void destroyListeners()
    {
        RevealManager.OnReveal.RemoveListener(onReveal);
    }

    public void onReveal()
    {
        if (!hasBeenOpened())
        {
            RevealManager.setRevealForGameObject(gameObject, getRevealColor());
        }
    }

    public Color getRevealColor()
    {
        return RevealManager.canBeInteractedWith;
    }

    public void spawnTargetCanvas()
    {
        // if (targetCanvas == null)
        // {
        //     gameObject.AddComponent<RectTransform>();
        //     targetCanvas = Instantiate(Resources.Load<GameObject>(PrefabNames.targetBox), transform);
        // }
    }

    public void createHoverTag()
    {
        MouseHoverManager.getMouseHoverBase();
        MouseHoverManager.createHoverTag(tagText);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData != null && eventData.used)
        {
            return;
        }

        if (!RevealManager.currentlyRevealed && !hasBeenOpened())
        {
            RevealManager.setOutlineColor(gameObject, getRevealColor());
        }

        if (!hasBeenOpened())
        {
            createHoverTag();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData != null && eventData.used)
        {
            return;
        }

        if (!RevealManager.currentlyRevealed && !hasBeenOpened())
        {
            RevealManager.setOutlineColorToDefault(gameObject);
        }

        if (!hasBeenOpened())
        {
            MouseHoverManager.destroyMouseHoverBase();
        }
    }
}
