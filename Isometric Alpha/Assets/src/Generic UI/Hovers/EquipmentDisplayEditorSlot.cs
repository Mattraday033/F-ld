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

    public Button unequipButton;
    public Collider2D boxCollider;

    public int slotIndex;
    public int displayIndex;

    public Image outline;

    public DragDrogItemSlotType slotType;

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
            int currentAttackIndex = OverallUIManager.getCurrentActionArray().getActionIndex(new Attack(OverallUIManager.getCurrentPartyMember(), item as Weapon));

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

        boxCollider.enabled = true;
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
        iconImage.color = ColorList.filledIconFadeOutLevel;

        if (getItemInSlot() != null && getItemInSlot().getSlotID() == Weapon.offHandSlotIndex)
        {
            iconImage.sprite = Helpers.loadSpriteFromResources(getItemInSlot().getIconName());
        }

        boxCollider.enabled = true;
    }

    public void setToAvailableAndUsable()
    {
        setToAvailableAndUsable(-1);
    }

    //no weapon, has slot open
    public void setToAvailableAndUsable(int combatActionSlotIndex)
    {
        this.combatActionSlotIndex = combatActionSlotIndex;

        backgroundImage.color = ColorList.availableEquipmentIcon;
        iconImage.color = ColorList.availableIconFadeOutLevel;

        setToSlotSprite();

        boxCollider.enabled = true;
    }

    public void setToUnavailableAndUnusable()
    {
        setToUnavailableAndUnusable(-1);
    }

    //no weapon, no slot open
    public void setToUnavailableAndUnusable(int combatActionSlotIndex)
    {
        this.combatActionSlotIndex = combatActionSlotIndex;

        backgroundImage.color = ColorList.unavailableEquipmentIcon;
        iconImage.color = ColorList.unavailableIconFadeOutLevel;

        setToSlotSprite();

        boxCollider.enabled = false;
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
    }

    public void moveAllItemOutOfJunk(Item item)
    {
        if (!item.isJunk())
        {
            return;
        }

        Inventory.removeItem(item, State.junkPocket);
        Inventory.addItem(item, State.inventory);
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

        int slotID = itemBeingDragged.getSlotID();

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

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if ((hoverText != null && hoverText.Length > 0) || isFilled())
        {
            MouseHoverManager.startCoroutine(this, MouseHoverManager.waitToHandleDescriptionPanel(this, MouseHoverManager.shouldSpawnHoverIcon));
        }
    }
}