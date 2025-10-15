using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum DragDrogItemSlotType {Junk = 0, Inventory = 1, Buy = 2, Sell = 3}

public class EquipmentDisplayEditorSlot : SlotIconHover
{

    private EquippableItem itemInSlot;
    private int combatActionSlotIndex = -1;

    private static Color availableGrey = new Color32(100, 100, 100, 255);
    private static Color unavailableGrey = new Color32(35, 35, 35, 255);
    private static Color availableIconFadeOutLevel = new Color32(0, 0, 0, 85);
    private static Color unavailableIconFadeOutLevel = new Color32(125, 125, 125, 85);
    private static Color filledIconFadeOutLevel = new Color32(160, 160, 160, 210);

    public Button unequipButton;
    public Collider2D collider;

    public int slotIndex;
    public int displayIndex;

    public Image outline;

    public DragDrogItemSlotType slotType;

    public override void Awake()
    {
        base.Awake();
    }

    public EquippableItem getItemInSlot()
    {
        if (combatActionSlotIndex >= 0)
        {
            CombatAction actionInSlot = OverallUIManager.getCurrentActionArray().getActionInSlot(combatActionSlotIndex);

            if (actionInSlot != null)
            {
                return OverallUIManager.getCurrentActionArray().getActionInSlot(combatActionSlotIndex).getSourceItem() as EquippableItem;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return itemInSlot;
        }
    }

    public override IDescribable getObjectBeingDescribed()
    {
        if (isFilled())
        {
            return getItemInSlot();
        }
        else
        {
            return this;
        }
    }

    public void addItemToSlot(EquippableItem item)
    {
        if (item.getSlotID() >= 0 && item.getSlotID() < Weapon.mainHandSlotIndex)
        {
            itemInSlot = item;
            setToFilledAndUsable();
        }
        else if (item.getSlotID() == Weapon.mainHandSlotIndex)
        {
            int currentAttackIndex = OverallUIManager.getCurrentActionArray().getActionIndex(new Attack(item as Weapon));

            if (currentAttackIndex >= 0 && currentAttackIndex < CombatActionArray.numberOfActivatablePlayerCombatActions)
            {
                setToFilledAndUsable(currentAttackIndex);
            }
            else
            {
                setToFilledAndUnusable(currentAttackIndex);
            }

            iconImage.sprite = Helpers.loadSpriteFromResources(getItemInSlot().getIconName());
        }

        if (!item.isUnequippable())
        {
            unequipButton.enabled = false;
        }
        else
        {
            unequipButton.enabled = true;
        }
    }

    private void setIconImage()
    {
        if (isFilled())
        {
            if (getItemInSlot().getSubtype().Equals(Weapon.subtype))
            {
                iconImage.sprite = Helpers.loadSpriteFromResources(getItemInSlot().getIconName());
            }
            else
            {
                iconImage.sprite = Helpers.loadSpriteFromResources(getItemInSlot().getSlotIconName());
            }
        }
    }

    public override GameObject getDescriptionPanelType()
    {
        if (isFilled())
        {
            return Resources.Load<GameObject>(PrefabNames.hoverIconCombatActionDescriptionPanel);
        }
        else
        {
            return Resources.Load<GameObject>(PrefabNames.hoverIconDescriptionPanel);
        }

    }

    public void resetUI()
    {
        itemInSlot = null;
        setToAvailableAndUsable();
        setToSlotSprite();

        // if (isFilled() && combatActionSlotIndex < 0)
        // {
        //     setIconImage();
        //     unequipButton.enabled = true;
        // }
        // else if (isFilled() && combatActionSlotIndex >= 0)
        // {
        //     if (combatActionSlotIndex < CombatActionArray.numberOfActivatablePlayerCombatActions)
        //     {
        //         setToAvailableAndUsable(combatActionSlotIndex);
        //     }
        //     else if (combatActionSlotIndex < CombatActionArray.maxPlayerCombatActions)
        //     {
        //         setToFilledAndUnusable(combatActionSlotIndex);
        //     }

        //     iconImage.sprite = Helpers.loadSpriteFromResources(getItemInSlot().getIconName());

        //     unequipButton.enabled = true;
        // } else
        // {
        //     setToSlotSprite();

        //     setToAvailableAndUsable();

        //     unequipButton.enabled = false;
        // }
    }

    public void unequipInCurrentSlot()
    {
        if (isFilled())
        {
            if (combatActionSlotIndex >= 0)
            {
                OverallUIManager.getCurrentActionArray().unequipCombatAction(combatActionSlotIndex);
            }
            else
            {
                OverallUIManager.getCurrentEquippedItems().unequipItem(itemInSlot);
            }

            combatActionSlotIndex = -1;

            destroyHoverIcon();
        }
    }

    public void setToFilledAndUsable()
    {
        setToFilledAndUsable(-1);
    }

    //has weapon, in activatable range
    public void setToFilledAndUsable(int combatActionSlotIndex)
    {
        this.combatActionSlotIndex = combatActionSlotIndex;

        backgroundImage.color = Color.black;
        iconImage.color = Color.white;

        if (getItemInSlot() != null && getItemInSlot().getSlotID() == Weapon.offHandSlotIndex)
        {
            iconImage.sprite = Helpers.loadSpriteFromResources(getItemInSlot().getIconName());
        }

        collider.enabled = true;
    }

    public void setToFilledAndUnusable()
    {
        setToFilledAndUnusable(-1);
    }

    //has weapon, in passive range
    public void setToFilledAndUnusable(int combatActionSlotIndex)
    {
        this.combatActionSlotIndex = combatActionSlotIndex;

        backgroundImage.color = Color.black;
        iconImage.color = filledIconFadeOutLevel;

        if (getItemInSlot() != null && getItemInSlot().getSlotID() == Weapon.offHandSlotIndex)
        {
            iconImage.sprite = Helpers.loadSpriteFromResources(getItemInSlot().getIconName());
        }

        collider.enabled = true;
    }

    public void setToAvailableAndUsable()
    {
        setToAvailableAndUsable(-1);
    }

    //no weapon, has slot open
    public void setToAvailableAndUsable(int combatActionSlotIndex)
    {
        this.combatActionSlotIndex = combatActionSlotIndex;

        backgroundImage.color = availableGrey;
        iconImage.color = availableIconFadeOutLevel;

        setToSlotSprite();

        collider.enabled = true;
    }

    public void setToUnavailableAndUnusable()
    {
        setToUnavailableAndUnusable(-1);
    }

    //no weapon, no slot open
    public void setToUnavailableAndUnusable(int combatActionSlotIndex)
    {
        this.combatActionSlotIndex = combatActionSlotIndex;

        backgroundImage.color = unavailableGrey;
        iconImage.color = unavailableIconFadeOutLevel;

        setToSlotSprite();

        collider.enabled = false;
    }

    public bool isFilled()
    {
        return getItemInSlot() != null;
    }

    public bool sendToPocketSlot()
    {
        return slotIndex < 0;
    }

    public void setToJunk()
    {
        slotType = DragDrogItemSlotType.Junk;
        iconImage.sprite = Helpers.loadSpriteFromResources(IconList.junkIconName);

        setHoverMessage(HoverMessageList.getMessage(HoverMessageList.junkSlotKey));
    }

    public void setToInventory()
    {
        slotType = DragDrogItemSlotType.Inventory;
        iconImage.sprite = Helpers.loadSpriteFromResources(IconList.amountIconName);

        setHoverMessage(HoverMessageList.getMessage(HoverMessageList.toInvSlotKey));
    }

    public void moveAllItemToJunk(Item item)
    {
        if (item.isJunk())
        {
            return;
        }

        Inventory.removeItem(item, State.inventory);
        Inventory.addItem(item, State.junkPocket);

        UsableItem usableItem = item as UsableItem;

        if ((usableItem) != null)
        {
            OverallUIManager.getCurrentPartyMember().getActionArray().unequipCombatAction(usableItem.getKey());
        }

        EquippedItems.OnEquipmentChange.Invoke();
    }

    public void moveAllItemOutOfJunk(Item item)
    {
        if (!item.isJunk())
        {
            return;
        }

        Inventory.removeItem(item, State.junkPocket);
        Inventory.addItem(item, State.inventory);

        EquippedItems.OnEquipmentChange.Invoke();
    }

    public void setToSlotSprite()
    {
        if (slotIndex >= Armor.offHandSlotIndex && slotIndex <= Armor.trinketSlotIndex)
        {
            iconImage.sprite = Helpers.loadSpriteFromResources(Armor.getSlotIconName(slotIndex));
        }
        else if (slotIndex >= Weapon.mainHandSlotIndex)
        {
            iconImage.sprite = Helpers.loadSpriteFromResources(Weapon.getSlotIconName(slotIndex));
        }

    }

    public void buyItem(Item item)
    {
        ShopPopUpWindow.buyItem(item);
    }

    public void sellItem(Item item)
    {
        ShopPopUpWindow.sellItem(item);
    }

    public void highlight(IDescribable describable)
    {
        Item itemBeingDragged = describable as Item;

        if (itemBeingDragged != null && itemBeingDragged.getSlotID() >= 0 &&
            itemBeingDragged.getSlotID() == slotIndex)
        {
            outline.color = Color.yellow;
        }
    }

    public void unhighlight(IDescribable describable)
    {
        outline.color = Color.black;
    }

    private void OnEnable()
    {
        DragAndDropManager.OnDragAndDropCreated.AddListener(highlight);
        DragAndDropManager.OnDragAndDropDestroyed.AddListener(unhighlight);
    }

    private void OnDisable()
    {
        DragAndDropManager.OnDragAndDropCreated.RemoveListener(highlight);
        DragAndDropManager.OnDragAndDropDestroyed.RemoveListener(unhighlight);
    }
}