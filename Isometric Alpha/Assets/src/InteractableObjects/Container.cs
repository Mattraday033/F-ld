using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum ContainerType {Chest, Shelf, MattockRack, AxeRack, ShovelRack, SpearRack, SwordTable, PickaxeTable }
public enum ChestState { Closed, OpenFilled, OpenEmpty }

// public interface INonRevealableNameSource: INameSource
// {
//     public bool isRevealable
//     {
//         get;   
//     }

//     public static bool nameSourceIsRevealable(INameSource nameSource)
//     {
//         INonRevealableNameSource nonRevealableNameSource = nameSource as INonRevealableNameSource;

//         if(nonRevealableNameSource == null)
//         {
//             return true;
//         } else
//         {
//             return nonRevealableNameSource.isRevealable;
//         }   
//     }
// }

public class Container : MonoBehaviour, IRevealable, IQuestActivationObject, IAppearanceSource, INameTagSuppressor
{

    #region Chest Sprite Dictionary
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

    private static string getCurrentSprite(Facing facing, ChestState chestState, ContainerType type)
    {
        switch(type)
        {
            case ContainerType.Shelf:
                return shelfSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)];
            case ContainerType.MattockRack:
                return mattockRackSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)];
            case ContainerType.AxeRack:
                return axeRackSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)];
            case ContainerType.ShovelRack:
                return shovelRackSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)];
            case ContainerType.SpearRack:
                return spearRackSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)];
            case ContainerType.SwordTable:
                return swordTableSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)];
            case ContainerType.PickaxeTable:
                return pickaxeTableSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)];
            default:
                return chestSprites[new KeyValuePair<Facing, ChestState>(facing, chestState)];
        }
    }

    public virtual SFXType getChestOpenSFX(ContainerType type)
    {
        switch(type)
        {
            case ContainerType.Shelf:
                return SFXType.OnTransition;
            case ContainerType.Chest:
                return SFXType.ChestOpen;
            default:
                return SFXType.NoSFX;
        }
    }

    private static SFXType getChestTakeSFX(ContainerType type)
    {
        switch(type)
        {
            default:
                return SFXType.PlaceInInventory;
        }
    }

    #endregion

    public PolygonCollider2D mouseHoverCollider;
    public ChestState chestState = ChestState.Closed;
    public ContainerType chestType = ContainerType.Chest;

    public NewAnimationManager animationManager;

    public SpriteLayerRendererList rendererList
    {
        get
        {
            if(animationManager != null)
            {
                return animationManager.rendererList;
            } else
            {
                return null;
            }
        }
    }

    public IAppearance appearance
    {
        get
        {
            return new SpriteDescription(getCurrentSprite(animationManager.characterFacing.currentFacing, chestState, chestType),
                                            large: false,
                                            withScale: chestType.withScale()
                                        );
        }
    }

    private string secretDoorFlag;

    public int chestIndex;

    public DescriptionPanel chestItemDescriptionPanel;

    private QuestStepActivationScript _Script;
    public QuestStepActivationScript script
    {
        get
        {
            return _Script;
        }
        set
        {
            _Script = value;
            OpenChestsSharingIndex.AddListener(openWithoutActivatingScripts);
        }
    }

    public PlayerInteractionScript[] scripts;


    private string _UniqueName = "";
    public string uniqueName { get { return _UniqueName; } }

    private string _NPCName = "";
    public string displayName { get { return _NPCName; } }

    //matches the uniqueName of the ContainerSpawnDetails that spawned this container
    private void setNames()
    {
        _UniqueName = ContainerSpawnDetails.generateName(chestIndex);
        _NPCName = NameSourceExtensions.splitCamelCase(chestType.ToString());
    }

    private void Awake()
    {
        setNames();
        createListeners();
    }

    private void OnDestroy()
    {
        SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(show);

        destroyListeners();
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

    public void onReveal(bool toggleReveal)
    {
        if(rendererList == null)
        {
            return;
        }

        if(toggleReveal && !GateAndChestManager.hasBeenOpened(getChestKey()))
        {
            rendererList.createOutline(getRevealColor());
        } else
        {
            rendererList.removeOutline();
        }
    }

    public Color getRevealColor()
    {
        return ColorList.canBeInteractedWith;
    }

    //Empty on purpose, the OverHeadIconManager beside this container shows its name tag instead
    public void createHoverTag()
    {
    }

    public bool suppressNameTag { get { return hasBeenOpened(); } }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(GateAndChestManager.hasBeenOpened(getChestKey()) || rendererList == null)
        {
            return;
        }

        switch(PlayerStateManager.currentActivity)
        {
            case CurrentActivity.Walking:
            case CurrentActivity.InChestUI:
            case CurrentActivity.Cunning:
            case CurrentActivity.Intimidating:
            case CurrentActivity.Observing:
                break;
            default:
                return;
        }

        PlayerObject.toggleButtonPrompt(false);

        if(!RevealManager.currentlyRevealed)
        {
            rendererList.createOutline(getRevealColor());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(GateAndChestManager.hasBeenOpened(getChestKey()) || rendererList == null)
        {
            return;
        }

        PlayerObject.restoreButtonPrompt();

        switch(PlayerStateManager.currentActivity)
        {
            case CurrentActivity.Walking:
            case CurrentActivity.InChestUI:
            case CurrentActivity.Cunning:
            case CurrentActivity.Intimidating:
            case CurrentActivity.Observing:
                break;
            default:
                return;
        }

        if(!RevealManager.currentlyRevealed)
        {
            rendererList.removeOutline();
        }
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

    public void populate(int index, ContainerType type)
    {
        animationManager = GetComponent<NewAnimationManager>();
        animationManager.appearanceSource = this;

        chestIndex = index;

        chestType = type;
        setNames();
        // setMouseHoverPosition();

        if (GateAndChestManager.hasBeenOpened(getChestKey()))
        {
            setSpriteToOpenEmpty(ignoreSFX: true);
        }
        else
        {
            setSpriteToClosed();
        }
    }
    
    protected virtual void setToCurrentSprite()
    {
        animationManager.handleMovementAnimation();
        // setMouseHoverPosition();
    }

    // protected void setMouseHoverPosition()
    // {
    //     Helpers.updatePolygonCollider(spriteRenderer, mouseHoverCollider);
    //     // GameObjectUtil.updateGameObjectPosition(gameObject);
    // }

    public void playerOpensChest()
    {
        PopUpScreenBlockerManager.spawnPopUpScreenBlocker();

        NotificationManager.OnDeleteAllNotifications.Invoke();

        AudioManager.playAudioClipAsSingleton(getChestOpenSFX(chestType));

        createChestItemUI();

        Inventory.addItem(ChestItemIDList.getChestItem(AreaManager.locationName, chestIndex));

        setSpriteToOpenFilled();

        GateAndChestManager.addKey(getChestKey());

        PlayerInteractionScript.runAllScripts(scripts);

        PlayerStateManager.OnStateChangeFromInChestUI.AddListener(destroyUI);
        PlayerStateManager.OnStateChangeFromInChestUI.AddListener(setSpriteToOpenEmpty);

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
        ChestItemIDList.getChestItem(AreaManager.locationName, chestIndex).describeSelfRow(chestItemDescriptionPanel);
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
        PlayerStateManager.OnStateChangeFromInChestUI.RemoveListener(destroyUI);
        PlayerStateManager.OnStateChangeFromInChestUI.RemoveListener(setSpriteToOpenEmpty);

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
