using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IDragAndDropContainer
{
    public void setObjectBeingDragged(IDescribable objectBeingDragged);
    public IDescribable getObjectBeingDragged();

    public void OnDestroy();
}

public class DragAndDropUIObject : MonoBehaviour, IDragAndDropContainer
{

    public List<Collider2D> colliders;

    public DescriptionPanel descriptionPanel;

    public IDescribable objectBeingDragged;

    void Update()
    {
        if (!Input.GetKey(KeyCode.Mouse0))
        {
            handleMouseUp();
        }
    }

    public IDescribable getObjectBeingDragged()
    {
        return objectBeingDragged;
    }

    public Item getItemBeingDragged()
    {
        ItemCombatAction itemCombatAction = getObjectBeingDragged() as ItemCombatAction;

        if(itemCombatAction != null)
        {
            return itemCombatAction.getSourceItem();
        } else
        {
            return getObjectBeingDragged() as Item;
        }
    }

    public virtual void setObjectBeingDragged(IDescribable objectBeingDragged)
    {
        this.objectBeingDragged = objectBeingDragged;
        descriptionPanel.setObjectBeingDescribed(objectBeingDragged);
        objectBeingDragged.describeSelfFull(descriptionPanel);
    }

    public void handleMouseUp()
    {
        checkForTargetObject();

        MouseHoverManager.destroyMouseHoverBase();
    }

    public virtual void checkForTargetObject()
    {
        ContactFilter2D filter2D = new ContactFilter2D();
        filter2D.SetLayerMask(LayerAndTagManager.uiLayerMask);
        filter2D.useLayerMask = true;

        foreach (Collider2D collider in colliders)
        {
            Collider2D[] collisions = Helpers.getCollisions(collider, filter2D);

            foreach (Collider2D collision in collisions)
            {
                if (getTargetTags().Contains(collision.gameObject.tag) ||
                    (collision.gameObject.tag.Equals(LayerAndTagManager.junkSlotTargetTag) && handlesJunkSlot()))
                {
                    if(handleTargetObject(collision, collision.gameObject.tag))
                    {
                        return;
                    }
                }
            }
        }
    }

    public virtual bool handleTargetObject(Collider2D collision, string tag)
    {
        return false;
    }

    public virtual string[] getTargetTags()
    {
        return new string[]{};
    }

    public virtual bool handlesJunkSlot()
    {
        return false;
    }

    public void OnDisable()
    {
        DragAndDropManager.OnDragAndDropDestroyed.Invoke(objectBeingDragged);
    }

    public void OnDestroy()
    {
        DragAndDropManager.OnDragAndDropDestroyed.Invoke(objectBeingDragged);
    }

    public bool handleUsableItemDrop(GameObject target)
    {
        DescriptionPanel partyMemberGridRow = target.GetComponent<DescriptionPanel>();

        UsableItem item = getItemBeingDragged() as UsableItem;

        if (item == null)
        {
            return false;
        }

        Stats targetStats = Stats.convertIDescribableToStats(partyMemberGridRow.getObjectBeingDescribed());

        if (!item.fitsUseCriteria(targetStats))
        {
            return false;
        }

        item.use(targetStats);

        if (!item.infiniteUses())
        {
            Inventory.removeItem(item, 1);
        }

        return true;
    }

}
