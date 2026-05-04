using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ChestType {Chest, Shelf, MattockRack, AxeRack, ShovelRack, SpearRack, SwordTable, PickaxeTable }
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

    public const string chestKeyMarker = "-chest-";

    public readonly static UnityEvent<int> OpenChestsSharingIndex = new UnityEvent<int>();

    private static Dictionary<KeyValuePair<Facing, ChestState>, string> chestSprites;
    private static Dictionary<KeyValuePair<Facing, ChestState>, string> shelfSprites;
    private static Dictionary<KeyValuePair<Facing, ChestState>, string> mattockRackSprites;
    private static Dictionary<KeyValuePair<Facing, ChestState>, string> axeRackSprites;
    private static Dictionary<KeyValuePair<Facing, ChestState>, string> shovelRackSprites;
    private static Dictionary<KeyValuePair<Facing, ChestState>, string> spearRackSprites;
    private static Dictionary<KeyValuePair<Facing, ChestState>, string> swordTableSprites;
    private static Dictionary<KeyValuePair<Facing, ChestState>, string> pickaxeTableSprites;


    [RuntimeInitializeOnLoadMethod]
    private static void instantiateSprites()
    {
        #region Chest
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

        #endregion

        #region Shelf

        shelfSprites = new Dictionary<KeyValuePair<Facing, ChestState>, string>();

        shelfSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.Closed), PrefabNames.shelfFrontFull);
        shelfSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenFilled), PrefabNames.shelfFrontFull);
        shelfSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenEmpty), PrefabNames.shelfFrontEmpty);

        shelfSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.Closed), PrefabNames.shelfFrontFull);
        shelfSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenFilled), PrefabNames.shelfFrontFull);
        shelfSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenEmpty), PrefabNames.shelfFrontEmpty);

        #endregion
    
        #region MattockRack

        mattockRackSprites = new Dictionary<KeyValuePair<Facing, ChestState>, string>();

        mattockRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.Closed), PrefabNames.mattockRack);
        mattockRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenFilled), PrefabNames.mattockRack);
        mattockRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenEmpty), PrefabNames.emptyHorizontalRack);

        mattockRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.Closed), PrefabNames.mattockRack);
        mattockRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenFilled), PrefabNames.mattockRack);
        mattockRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenEmpty), PrefabNames.emptyHorizontalRack);

        #endregion

        #region AxeRack

        axeRackSprites = new Dictionary<KeyValuePair<Facing, ChestState>, string>();

        axeRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.Closed), PrefabNames.axeRack);
        axeRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenFilled), PrefabNames.axeRack);
        axeRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenEmpty), PrefabNames.emptyHorizontalRack);

        axeRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.Closed), PrefabNames.axeRack);
        axeRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenFilled), PrefabNames.axeRack);
        axeRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenEmpty), PrefabNames.emptyHorizontalRack);

        #endregion

        #region ShovelRack

        shovelRackSprites = new Dictionary<KeyValuePair<Facing, ChestState>, string>();

        shovelRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.Closed), PrefabNames.shovelRack);
        shovelRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenFilled), PrefabNames.shovelRack);
        shovelRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenEmpty), PrefabNames.emptyPolearmRack);

        shovelRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.Closed), PrefabNames.shovelRack);
        shovelRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenFilled), PrefabNames.shovelRack);
        shovelRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenEmpty), PrefabNames.emptyPolearmRack);

        #endregion

        #region SpearRack

        spearRackSprites = new Dictionary<KeyValuePair<Facing, ChestState>, string>();

        spearRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.Closed), PrefabNames.spearRack);
        spearRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenFilled), PrefabNames.emptyPolearmRack);
        spearRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenEmpty), PrefabNames.emptyPolearmRack);

        spearRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.Closed), PrefabNames.emptyPolearmRack);
        spearRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenFilled), PrefabNames.emptyPolearmRack);
        spearRackSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenEmpty), PrefabNames.emptyPolearmRack);

        #endregion

        #region SwordTable

        swordTableSprites = new Dictionary<KeyValuePair<Facing, ChestState>, string>();

        swordTableSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.Closed), PrefabNames.swordTable);
        swordTableSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenFilled), PrefabNames.swordTable);
        swordTableSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenEmpty), PrefabNames.emptyWeaponTable);

        swordTableSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.Closed), PrefabNames.swordTable);
        swordTableSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenFilled), PrefabNames.swordTable);
        swordTableSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenEmpty), PrefabNames.emptyWeaponTable);

        #endregion
        
        #region PickaxeTable

        pickaxeTableSprites = new Dictionary<KeyValuePair<Facing, ChestState>, string>();

        pickaxeTableSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.Closed), PrefabNames.pickaxeTable);
        pickaxeTableSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenFilled), PrefabNames.pickaxeTable);
        pickaxeTableSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthEast, ChestState.OpenEmpty), PrefabNames.emptyWeaponTable);

        pickaxeTableSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.Closed), PrefabNames.pickaxeTable);
        pickaxeTableSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenFilled), PrefabNames.pickaxeTable);
        pickaxeTableSprites.Add(new KeyValuePair<Facing, ChestState>(Facing.SouthWest, ChestState.OpenEmpty), PrefabNames.emptyWeaponTable);

        #endregion
    }

    private static Sprite getCurrentSprite(Facing facing, ChestState chestState, ChestType type)
    {
        switch(type)
        {
            case ChestType.Shelf:
                return Helpers.loadSpriteFromResources(shelfSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)]);
            case ChestType.MattockRack:
                return Helpers.loadSpriteFromResources(mattockRackSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)]);
            case ChestType.AxeRack:
                return Helpers.loadSpriteFromResources(axeRackSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)]);
            case ChestType.ShovelRack:
                return Helpers.loadSpriteFromResources(shovelRackSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)]);
            case ChestType.SpearRack:
                return Helpers.loadSpriteFromResources(spearRackSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)]);
            case ChestType.SwordTable:
                return Helpers.loadSpriteFromResources(swordTableSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)]);
            case ChestType.PickaxeTable:
                return Helpers.loadSpriteFromResources(pickaxeTableSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)]);
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
            case ChestType.Chest:
                return AudioClipList.chestOpen;
            default:
                return "";
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

    private string secretDoorFlag;

    public int chestIndex;

    private Item chestContents;

    public DescriptionPanel chestItemDescriptionPanel;

    private QuestStepActivationScript script;

    public PlayerInteractionScript[] scripts;


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

    public void setSecretDoorFlag(string secretDoorFlag)
    {
        if(secretDoorFlag == null || secretDoorFlag.Length <= 0)
        {
            return;
        }

        this.secretDoorFlag = secretDoorFlag;

        if(!SecretDoorFlags.secretDoorHasBeenDiscovered(secretDoorFlag))
        {
            gameObject.SetActive(false);
            SecretDoorFlags.OnSecretDoorDiscovery.AddListener(show);
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

        AudioManager.playAudioClipAsSingleton(getChestOpenSFX(chestType));

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
            AudioManager.playAudioClipAsSingleton(getChestTakeSFX(chestType));
        }

        OpenChestsSharingIndex.RemoveListener(openWithoutActivatingScripts);
        OpenChestsSharingIndex.Invoke(chestIndex);
    }

    private string getChestKey()
    {
        return AreaManager.locationName + chestKeyMarker  + chestIndex;
    }

    public bool hasBeenOpened()
    {
        return chestState != ChestState.Closed;
    }

    public void setScript(QuestStepActivationScript script)
    {
        this.script = script;
        OpenChestsSharingIndex.AddListener(openWithoutActivatingScripts);
    }

    private void openWithoutActivatingScripts(int index)
    {
        if(chestIndex != index)
        {
            return;
        }

        setSpriteToOpenEmpty(ignoreSFX: true);
        OpenChestsSharingIndex.RemoveListener(openWithoutActivatingScripts);
    }

}
