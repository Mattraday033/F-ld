using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum ShopMode { Buy = 0, Sell = 1, BuyBack = 2, Junk = 3 }

public class ShopPopUpWindow : PopUpWindow
{
    public static Dictionary<string, Item> junkDestinationPocket = null; //junk that gets sold gets send to the void
    public TabCollection itemTypeTabs;
    public TabCollection buySellTabs;

    public TextMeshProUGUI shopNameTag;
    public TextMeshProUGUI totalPlayerDiscount;
    public TextMeshProUGUI totalPlayerGold;


    public Button sellAllJunkButton;
    public Image sellAllJunkIconImageBackground;
    public Image sellAllJunkIconImage;
    public TextMeshProUGUI sellAllJunkText;

    public ScrollableUIElement shopInventoryGrid;

    private Shopkeeper currentShopkeeper;

    private static ShopPopUpWindow instance;

    public static ShopPopUpWindow getInstance()
    {
        return instance;
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

    public static ShopMode getCurrentMode()
    {
        if (getInstance() == null)
        {
            return ShopMode.Sell;
        }

        return (ShopMode)getInstance().buySellTabs.getCurrentTabIndex();
    }

    public void setCurrentShopkeeper(Shopkeeper shopkeeper)
    {
        currentShopkeeper = shopkeeper;

        shopNameTag.text = shopkeeper.getShopkeeperInventoryKey() + "'s Shop";

        setShopMode((int)ShopMode.Buy);

        setToFirstVisibleTab();
    }

    public void updateGold()
    {
        totalPlayerGold.text = Purse.getCoinsInPurseForDisplay();
    }

    public override void closeButtonPress()
    {
        EscapeStack.handleEscapePress();
    }


    public void setShopMode(ShopMode newMode)
    {
        setShopMode((int)newMode);
    }

    public void setShopMode(int newMode)
    {
        buySellTabs.selectTab(newMode);

        hideUnnecessaryTabs();

        if (!currentTabStillVisible())
        {
            setToFirstVisibleTab();
        }

        populateGrid();
    }

    private void setToFirstVisibleTab()
    {
        if (mainHandTabVisible())
        {
            itemTypeTabs.selectAndClickTab(0);
            return;
        }

        if (useItemTabVisible())
        {
            itemTypeTabs.selectAndClickTab(1);
            return;
        }

        if (offHandTabVisible())
        {
            itemTypeTabs.selectAndClickTab(2);
            return;
        }

        if (armorTabVisible())
        {
            itemTypeTabs.selectAndClickTab(3);
            return;
        }

        if (essentialTabVisible())
        {
            itemTypeTabs.selectAndClickTab(4);
            return;
        }

        if (junkTabVisible())
        {
            itemTypeTabs.selectAndClickTab(5);
            return;
        }

        itemTypeTabs.collection[0].button.gameObject.SetActive(true);
        itemTypeTabs.selectAndClickTab(0);
    }

    private bool currentTabStillVisible()
    {
        switch (itemTypeTabs.getCurrentTabIndex())
        {
            case 0:
                return mainHandTabVisible();
            case 1:
                return useItemTabVisible();
            case 2:
                return offHandTabVisible();
            case 3:
                return armorTabVisible();
            case 4:
                return essentialTabVisible();
            case 5:
                return junkTabVisible();
            default:
                return false;
        }
    }

    public static void populateGrid()
    {
        getInstance().shopInventoryGrid.populatePanels(getInstance().getCurrentInventory());
        getInstance().updateGold();
        getInstance().updateSellAllJunkButtonInteractability();
    }

    private void updateSellAllJunkButtonInteractability()
    {
        if (State.junkPocket.Count > 0)
        {
            sellAllJunkButton.interactable = true;
            sellAllJunkIconImageBackground.color = Color.black;
            sellAllJunkIconImage.color = Color.white;
            sellAllJunkText.color = new Color32(25, 25, 25, 255);
        }
        else
        {
            sellAllJunkButton.interactable = false;
            sellAllJunkIconImageBackground.color = new Color32(100, 100, 100, 255);
            sellAllJunkIconImage.color = new Color32(155, 155, 155, 255);
            sellAllJunkText.color = new Color32(155, 155, 155, 255);
        }
    }

    private ArrayList getCurrentInventory()
    {
        Dictionary<string, Item> currentPocket;

        switch (getCurrentMode())
        {
            case ShopMode.Buy:

                currentPocket = currentShopkeeper.getInventory();
                break;

            case ShopMode.Sell:

                currentPocket = State.inventory;
                break;
        }

        return Tab.getList(itemTypeTabs.getCurrentTab().list);
    }

    public void hideUnnecessaryTabs()
    {
        itemTypeTabs.collection[0].button.gameObject.SetActive(mainHandTabVisible());
        itemTypeTabs.collection[1].button.gameObject.SetActive(useItemTabVisible());
        itemTypeTabs.collection[2].button.gameObject.SetActive(offHandTabVisible());
        itemTypeTabs.collection[3].button.gameObject.SetActive(armorTabVisible());
        itemTypeTabs.collection[4].button.gameObject.SetActive(essentialTabVisible());
        itemTypeTabs.collection[5].button.gameObject.SetActive(junkTabVisible());
    }

    private bool mainHandTabVisible()
    {
        if (getCurrentMode() == ShopMode.Sell)
        {
            return true;
        }

        return Tab.getList(DescribableList.ShopKeeperMainHandWeapons).Count > 0;
    }

    private bool useItemTabVisible()
    {
        if (getCurrentMode() == ShopMode.Sell)
        {
            return true;
        }

        return Tab.getList(DescribableList.ShopKeeperUseItems).Count > 0;
    }

    private bool offHandTabVisible()
    {
        if (getCurrentMode() == ShopMode.Sell)
        {
            return true;
        }

        return Tab.getList(DescribableList.ShopKeeperOffHandWeapons).Count > 0;
    }

    private bool armorTabVisible()
    {
        if (getCurrentMode() == ShopMode.Sell)
        {
            return true;
        }

        return Tab.getList(DescribableList.ShopKeeperArmor).Count > 0;
    }

    private bool essentialTabVisible()
    {
        if (getCurrentMode() == ShopMode.Sell)
        {
            return false;
        }

        return Tab.getList(DescribableList.ShopKeeperEssentialItems).Count > 0;
    }

    private bool junkTabVisible()
    {
        return getCurrentMode() == ShopMode.Sell;
    }

    public static void buyItem(Item item)
    {
        exchangeItem(item, getCurrentShopkeeper().getInventory(), State.inventory);
        populateGrid();
    }

    public static void sellItem(Item item)
    {
        Dictionary<string, Item> startPocket = State.inventory;

        if (item.isJunk())
        {
            startPocket = State.junkPocket;
        }

        exchangeItem(item, startPocket, getCurrentShopkeeper().getInventory());
        populateGrid();
    }

    private static void handleMoneyExchange(Item item)
    {
        handleMoneyExchange(getCurrentMode(), item);
    }

    private static void handleMoneyExchange(ShopMode shopMode, Item item)
    {
        if (shopMode == ShopMode.Buy || shopMode == ShopMode.BuyBack)
        {
            Purse.removeCoins(Item.getTotalWorth(item, shopMode));
        }
        else
        {
            Purse.addCoins(Item.getTotalWorth(item, shopMode));
        }

        if (instance != null)
        {
            instance.updateGold();
        }
    }

    private static void exchangeItem(Item item, Dictionary<string, Item> startPocket, Dictionary<string, Item> destinationPocket)
    {
        exchangeItem(item, getCurrentMode(), startPocket, destinationPocket);
    }

    private static void exchangeItem(Item item, ShopMode shopMode, Dictionary<string, Item> startPocket, Dictionary<string, Item> destinationPocket)
    {
        handleMoneyExchange(shopMode, item);

        Inventory.removeItem(item, item.getQuantity(), startPocket);
        Item newItem = item.clone();
        newItem.setQuantity(item.getQuantity());

        Inventory.addItem(newItem, destinationPocket);

        ShopItemQuestChecker.QuestStepActivationOnItemTransation(item);
    }

    public void sellAllJunkButtonPress()
    {
        ArrayList junkList = Tab.getList(DescribableList.Junk);

        foreach (Item item in junkList)
        {
            exchangeItem(item, ShopMode.Sell, State.junkPocket, junkDestinationPocket);
        }

        populateGrid();
    }

}

public static class ShopItemQuestChecker
{
    public const bool questSuccessful = true;

    public static void QuestStepActivationOnItemTransation(Item item)
    {
        switch (item.getKey())
        {
            case "Candy":
                QuestList.activateQuestStep("Múzsa's Sweet Tooth", 4);
                break;
            case "Lost Iron Nugget":
                QuestList.finishQuest("Stockhouse Stash", 12, questSuccessful);
                break;
            default:
                return;
        }
    }



}

