using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public enum ShopMode { Buy = 0, Sell = 1, BuyBack = 2, Junk = 3 }

public class ShopPopUpWindow : PopUpWindow, ITabParent
{
    public readonly static Dictionary<string, Item> junkDestinationPocket = null; //junk that gets sold gets send to the void

    public TextMeshProUGUI shopNameTag;
    public TextMeshProUGUI totalPlayerDiscount;
    public TextMeshProUGUI totalPlayerGold;


    public Button sellAllJunkButton;
    public Image sellAllJunkIconImageBackground;
    public Image sellAllJunkIconImage;
    public TextMeshProUGUI sellAllJunkText;

    private Shopkeeper currentShopkeeper;

    public static ShopMode currentShopMode;
    public static DescribableList currentDescribableList;

    private static ShopPopUpWindow instance;

    public static ShopPopUpWindow getInstance()
    {
        return instance;
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeShopPopUpWindow()
    {
        currentShopMode = ShopMode.Sell;
        currentDescribableList = DescribableList.Unnecessary;
        instance = null;
        PlayerOOCStateManager.OnStateChangeFromInShopUI.AddListener(onLeavingShopUI);
    }

    private void Awake()
    {
        if (instance != null)
        {
            throw new IOException("Duplicate instances of ShopPopUpWindow exist");
        }

        instance = this;
    }

    public static bool currentlyShopping()
    {
        return instance != null;
    }

    public static Shopkeeper getCurrentShopkeeper()
    {
        if (getInstance() == null)
        {
            return null;
        }

        return getInstance().currentShopkeeper;
    }

    public void setCurrentShopkeeper(Shopkeeper shopkeeper)
    {
        currentShopkeeper = shopkeeper;

        shopNameTag.text = shopkeeper.getShopkeeperInventoryKey() + "'s Shop";

        setShopMode(ShopMode.Buy);
    }

    public override void closeButtonPress()
    {
        EscapeStack.handleEscapePress();
    }

    public static void onLeavingShopUI()
    {
        currentShopMode = ShopMode.Sell;
        currentDescribableList = DescribableList.Unnecessary;
    }

    public void setShopMode(ShopMode newMode)
    {
        currentShopMode = newMode;

        int currentListCount = Tab.getList(currentDescribableList).Count();

        if(currentListCount <= 0)
        {
            currentDescribableList = getDefaultDescribableList();
            AbilityGridSideTab.setCurrentTabDict(this, currentDescribableList);
        }

        ScreenManager.OnScreenInteriorUpdate.Invoke();
    }
    private void updateSellAllJunkButtonInteractability()
    {
        if (State.junkPocket.Count > 0)
        {
            sellAllJunkButton.interactable = true;
        }
        else
        {
            sellAllJunkButton.interactable = false;
        }
    }

    public static void buyItem(Item item)
    {
        exchangeItem(item, getCurrentShopkeeper().getInventory(), State.inventory);
    }

    public static void sellItem(Item item)
    {
        Dictionary<string, Item> startPocket = State.inventory;

        if (item.isJunk())
        {
            startPocket = State.junkPocket;
        }

        exchangeItem(item, startPocket, getCurrentShopkeeper().getInventory());
    }

    private static void handleMoneyExchange(Item item)
    {
        if (currentShopMode == ShopMode.Buy || currentShopMode == ShopMode.BuyBack)
        {
            Purse.removeCoins(Item.getTotalWorth(item, currentShopMode));
        }
        else
        {
            Purse.addCoins(Item.getTotalWorth(item, currentShopMode));
        }

        ScreenManager.OnScreenInteriorUpdate.Invoke();
    }

    private static void exchangeItem(Item item, Dictionary<string, Item> startPocket, Dictionary<string, Item> destinationPocket)
    {
        handleMoneyExchange(item);

        Inventory.removeItem(item, item.getQuantity(), startPocket);
        Item newItem = item.clone();
        newItem.setQuantity(item.getQuantity());

        Inventory.addItem(newItem, destinationPocket);

        ShopItemQuestChecker.QuestStepActivationOnItemTransation(item);

        ScreenManager.OnScreenInteriorUpdate.Invoke();
    }

    public void sellAllJunkButtonPress()
    {
        IEnumerable<IDescribable> junkList = Tab.getList(DescribableList.Junk);

        foreach (Item item in junkList)
        {
            exchangeItem(item, State.junkPocket, junkDestinationPocket);
        }

        ScreenManager.OnScreenInteriorUpdate.Invoke();
    }

    #region ITabParent/ICounter

    public List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(ScreenManager.OnScreenInteriorUpdate);
        listOfEvents.Add(AbilityGridSideTab.OnSideTabChosen);

        return listOfEvents;
    }
    public DescribableList getDefaultDescribableList()
    {
        for(int index = (int) DescribableList.ShopKeeperMainHandWeapons; index <= (int) DescribableList.ShopKeeperEssentialItems; index++)
        {
            if(Tab.getList( (DescribableList) index).Count() <= 0)
            {
                continue;
            }

            return (DescribableList) index;
        }

        return DescribableList.ShopKeeperMainHandWeapons;
    }

    public void updateCounter()
    {
        totalPlayerGold.text = Purse.getCoinsInPurseForDisplay();
        totalPlayerDiscount.text = PartyStats.getDiscountForDisplay();
        updateSellAllJunkButtonInteractability();
    }

    private void OnEnable()
    {
        addListeners();
    }

    private void OnDestroy()
    {
        removeListeners();
    }

    public void addListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.AddListener(updateCounter);
        }
    }
    public void removeListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.RemoveListener(updateCounter);
        }
    }

    #endregion
}

public static class ShopItemQuestChecker
{
    public const bool questSuccessful = true;

    public static void QuestStepActivationOnItemTransation(Item item)
    {
        switch (item.getKey())
        {
            case "Candy":
                if(Flags.getFlag(FlagNameList.givenTaskByMuzsa))
                {
                    QuestList.activateQuestStep(QuestNameList.muzsasSweetToothQuestTitle, QuestNameList.muzsasSweetToothStepTitleFour);
                }
                break;
            case "Lost Iron Nugget":
                QuestList.finishQuest("Stockhouse Stash", QuestNameList.stockhouseStashStepTitleTwelve, questSuccessful);
                break;
            default:
                return;
        }
    }



}

