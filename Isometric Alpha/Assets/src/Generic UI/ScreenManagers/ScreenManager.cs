using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.Profiling;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public enum DescribableList
{
    Unnecessary = 0,
    Inventory = 1,
    Junk = 2,
    Equipment = 3,
    Strength = 4,
    Dexterity = 5,
    Wisdom = 6,
    Charisma = 7,
    Saves = 8,
    PartyMembers = 9,
    Quests = 10,
    Glossary = 11,
    MainHandWeaponsAsActions = 13,
    Armor = 14,
    Usable = 15,
    QuestItems = 16,
    OffHandWeapons = 17,
    PartyMembersWithPlayer = 18,
    CombatUsableItems = 19,
    MainHandWeaponsAsItems = 20,
    ShopKeeperMainHandWeapons = 21,
    ShopKeeperUseItems = 22,
    ShopKeeperOffHandWeapons = 23,
    ShopKeeperArmor = 24,
    ShopKeeperEssentialItems = 25,
    CharacterSpecificAbilities = 26,
    AllItems = 27,
    
    ShopKeeperAllItems = 28
}

[System.Serializable]
public struct Tab
{
    public readonly static UnityEvent<DescribableList> OnListRetrieved = new UnityEvent<DescribableList>();

    public static IEnumerable<IDescribable> getList(DescribableList describableList)
    {
		return getList(describableList, null);
    }

    public static IEnumerable<IDescribable> getList(DescribableList describableList, string[] filterParameters)
    {
        OnListRetrieved.Invoke(describableList);
        
        switch (describableList)
        {
            case DescribableList.Unnecessary:

                return new List<IDescribable>();

            case DescribableList.Inventory:

                return Inventory.getPocketForDisplayGenericUI(State.inventory, filterParameters, new NameComparer());

            case DescribableList.Junk:

                return Inventory.getPocketForDisplayGenericUI(State.junkPocket, filterParameters, new NameComparer());

            case DescribableList.Equipment:

                return OverallUIManager.getCurrentEquippedItems().createEquippedItemList();

            case DescribableList.Strength:

                return AbilityList.getAllStrengthAbilities();

            case DescribableList.Dexterity:

                return AbilityList.getAllDexterityAbilities();

            case DescribableList.Wisdom:

                return AbilityList.getAllWisdomAbilities();

            case DescribableList.Charisma:

                return AbilityList.getAllCharismaAbilities();

            case DescribableList.Saves:

                return SaveHandler.getSaveGameList();

            case DescribableList.PartyMembers:

                return new List<IDescribable>(PartyManager.getAllJoinablePartyMembers());

            case DescribableList.Quests:

                return QuestList.getActiveQuests();

            case DescribableList.Glossary:

                return GlossaryCategoryList.getAllGlossaryCategories();

            case DescribableList.MainHandWeaponsAsActions:

                return Inventory.getAllMainHandWeaponsInPocketAsCombatActions(State.inventory);
            case DescribableList.Armor:

                return Inventory.getAllArmorInPocket(State.inventory);
            case DescribableList.Usable:
                
                return Inventory.getPocketForDisplayGenericUI(State.inventory, new string[]{UsableItem.type}, new NameComparer());
            case DescribableList.QuestItems:
                
                return Inventory.getPocketForDisplayGenericUI(State.inventory, new string[]{QuestItem.subtype, Key.subtype}, new NameComparer());
            case DescribableList.OffHandWeapons:
                
                return Inventory.getAllOffHandWeaponsInPocket(State.inventory);
            case DescribableList.PartyMembersWithPlayer:

                return new List<IDescribable>(PartyManager.getAllJoinablePartyMembers());
            case DescribableList.CombatUsableItems:
                
                return Inventory.getAllItemsUsableInCombat();
            case DescribableList.MainHandWeaponsAsItems:
                
                return Inventory.getAllMainHandWeaponsInPocket(State.inventory);

            case DescribableList.ShopKeeperMainHandWeapons:

                if (ShopPopUpWindow.currentShopMode == ShopMode.Buy)
                {
                    return Inventory.getAllMainHandWeaponsInPocket(ShopPopUpWindow.getCurrentShopkeeper().getInventory());
                }
                else
                {
                    return getList(DescribableList.MainHandWeaponsAsItems);
                }
            case DescribableList.ShopKeeperUseItems:

                if (ShopPopUpWindow.currentShopMode == ShopMode.Buy)
                {
                    return Inventory.getAllItemsOfTypeInPocket(ShopPopUpWindow.getCurrentShopkeeper().getInventory(), UsableItem.type);
                }
                else
                {
                    return getList(DescribableList.Inventory, new string[] { UsableItem.type });
                }
            case DescribableList.ShopKeeperOffHandWeapons:

                if (ShopPopUpWindow.currentShopMode == ShopMode.Buy)
                {
                    return Inventory.getAllOffHandWeaponsInPocket(ShopPopUpWindow.getCurrentShopkeeper().getInventory());
                }
                else
                {
                    return getList(DescribableList.OffHandWeapons);
                }
            case DescribableList.ShopKeeperArmor:

                if (ShopPopUpWindow.currentShopMode == ShopMode.Buy)
                {
                    return Inventory.getAllArmorInPocket(ShopPopUpWindow.getCurrentShopkeeper().getInventory());
                }
                else
                {
                    return getList(DescribableList.Armor);
                }
            case DescribableList.ShopKeeperEssentialItems:

                if (ShopPopUpWindow.currentShopMode == ShopMode.Buy)
                {
                    return Inventory.getPocketForDisplayGenericUI(ShopPopUpWindow.getCurrentShopkeeper().getInventory(), new string[]{EssentialItem.type}, new NameComparer());
                }
                else
                {
                    return new List<IDescribable>(); //can't sell essential items
                }
            case DescribableList.CharacterSpecificAbilities:

                if(OverallUIManager.getCurrentPartyMember() == null)
                {
                    return new List<IDescribable>();
                } else
                {
                    return AbilityList.getCompanionAbilities(OverallUIManager.getCurrentPartyMember().uniqueName);
                }
            case DescribableList.AllItems:
                    return State.inventory.Values;
            case DescribableList.ShopKeeperAllItems:
                if (ShopPopUpWindow.currentShopMode == ShopMode.Buy)
                {
                    return ShopPopUpWindow.getCurrentShopkeeper().getInventory().Values;
                }
                else
                {
                    return State.inventory.Values.Where(item => !item.getSubtype().Equals(QuestItem.subtype) && !item.getSubtype().Equals(Key.subtype));
                }
            default:

                throw new IOException("Unknown DescribableList = " + describableList.ToString());
        }
    }
}

public abstract class ScreenManager : MonoBehaviour, ITabParent
{
    #region Events
    public readonly static UnityEvent<ScreenManager> OnScreenDeclaration = new UnityEvent<ScreenManager>();
    public readonly static UnityEvent OnScreenInteriorUpdate = new UnityEvent();
    #endregion

    private static AllyStats _CurrentPartyMember;
    public static AllyStats currentPartyMember
    {
        get
        {
            if (_CurrentPartyMember == null)
            {
                _CurrentPartyMember = PartyManager.getPlayerStats();
            }

            return _CurrentPartyMember;
        }
        set
        {
            _CurrentPartyMember = value;
        }
    }

    public virtual void Awake()
    {
        OverallUIManager.currentScreenManager = this; 
        OnScreenDeclaration.Invoke(this);
        addListeners();
    }

    protected virtual void Start()
    {        
        //a prebuilt screen gets this from show, and its Start can come a frame after it was first shown
        if (isPrebuilt)
        {
            return;
        }

        OnScreenInteriorUpdate.Invoke();
    }

    #region Prebuilt screens

    private static readonly ProfilerMarker showMarker = new ProfilerMarker("ScreenManager.show");

    //set on the copies PrebuiltScreenManager makes. Those are shown and hidden, where every other copy is made and destroyed
    public bool isPrebuilt { get; private set; } = false;

    //where each scroll area sat when the screen was made, which is where one that had just been instantiated would sit
    private Dictionary<RectTransform, Vector2> scrollStartPositions = new Dictionary<RectTransform, Vector2>();

    //a prebuilt screen that has been hidden is still alive, so "it exists" no longer means "it is on screen"
    public bool isShowing
    {
        get
        {
            return gameObject.activeInHierarchy;
        }
    }

    //for the pieces inside a screen. Something with no screen above it, like the shop or the party strip, is never hidden this way
    public static bool isHidden(ScreenManager owner)
    {
        return owner != null && !owner.isShowing;
    }

    //Editor only. A piece of a hidden screen doing real work means one of its listeners wasn't told the screen was hidden
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void reportWorkWhileHidden(Component piece, string work)
    {
        ScreenManager owner = piece.GetComponentInParent<ScreenManager>(true);

        if (isHidden(owner))
        {
            Debug.LogError(piece.name + " " + work + " while its screen, " + owner.name + ", was hidden");
        }
    }

    public void markAsPrebuilt()
    {
        isPrebuilt = true;

        onPrebuilt();

        foreach (ScrollRect scrollRect in GetComponentsInChildren<ScrollRect>(true))
        {
            if (scrollRect.content != null)
            {
                scrollStartPositions[scrollRect.content] = scrollRect.content.anchoredPosition;
            }
        }
    }

    //for anything a screen only builds the first time part of it is switched on
    protected virtual void onPrebuilt()
    {
        //Empty on purpose
    }

    //does for a prebuilt screen what Awake and Start do for one that was just instantiated
    public void show(Transform parent)
    {
        using (showMarker.Auto())
        {
            transform.SetParent(parent, false);

            //before the screen is switched on, because pieces inside read it as they enable
            OverallUIManager.currentScreenManager = this;

            //a screen that was just made had no remembered tab, so it opened on its default
            AbilityGridSideTab.forgetTab(this);

            foreach (KeyValuePair<RectTransform, Vector2> scrollStartPosition in scrollStartPositions)
            {
                if (scrollStartPosition.Key != null)
                {
                    scrollStartPosition.Key.anchoredPosition = scrollStartPosition.Value;
                }
            }

            OnScreenDeclaration.Invoke(this);

            gameObject.SetActive(true);

            prepareToShow();

            OnScreenInteriorUpdate.Invoke();
        }
    }

    //whatever a screen's own Awake set up that has to be set up again each time it is shown
    protected virtual void prepareToShow()
    {
        //Empty on purpose
    }

    //puts a prebuilt screen away, leaving things the way destroying it used to
    public void hide(Transform holder)
    {
        //first, while the screen can still hear a drag being dropped and put back what was being dragged
        MouseHoverManager.destroyMouseHoverBase();

        MouseHoverManager.destroyHoverIconInside(transform);

        cleanUpToHide();

        park(holder);

        if (OverallUIManager.currentScreenManager == this)
        {
            OverallUIManager.removeCurrentScreenType();
        }
    }

    //whatever destroying the screen used to take with it that hiding leaves behind
    protected virtual void cleanUpToHide()
    {
        //a tutorial step pointing at something in here would otherwise leave it tinted, with its cut-out, for the next time it is shown
        foreach (TutorialSequenceStepTargetUIObject tutorialTarget in GetComponentsInChildren<TutorialSequenceStepTargetUIObject>(true))
        {
            tutorialTarget.clearHighlight();
        }

        //rows and built descriptions listen to things themselves, and a screen that was just made has none of either yet.
        //Each grid is filled again, and each slot described again, by the update that showing the screen fires
        foreach (UIListenerGrid listenerGrid in GetComponentsInChildren<UIListenerGrid>(true))
        {
            if (listenerGrid.grid != null)
            {
                listenerGrid.grid.deleteAllPanels();
            }
        }

        foreach (DescriptionPanelSlot slot in GetComponentsInChildren<DescriptionPanelSlot>(true))
        {
            slot.clearAllDescribables();
        }
    }

    //to the holder with none of the tidying, which hide has already done by the time it calls this
    private void park(Transform holder)
    {
        gameObject.SetActive(false);

        transform.SetParent(holder, false);
    }

    #endregion

    public virtual bool enableSpriteRowDragAndDrop()
    {
        return false;
    }

    public abstract bool requiresPartyMemberSelectionGrid();

    public abstract List<UnityEvent> getUpdateEvents();

    public abstract DescribableList getDefaultDescribableList();

    public abstract void updateCounter();

    public virtual void addListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.AddListener(updateCounterIfShowing);
        }
    }
    public virtual void removeListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.RemoveListener(updateCounterIfShowing);
        }
    }

    //a hidden screen is brought up to date when it is next shown, so it lets the events in between go by
    private void updateCounterIfShowing()
    {
        if (!isShowing)
        {
            return;
        }

        updateCounter();
    }

    public abstract KeyCode getExitKeyCode();


    public static void resetCurrentPartyMember()
    {
        currentPartyMember = null;
    }

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        LoadSaveFile.OnLoadResetData.AddListener(resetCurrentPartyMember);
    }
}