using System.Collections;
using System.Collections.Generic;
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
                if (collision.gameObject.tag.Equals(getTargetTag()) ||
                    (collision.gameObject.tag.Equals(LayerAndTagManager.junkSlotTargetTag) && handlesJunkSlot()))
                {
                    handleTargetObject(collision);
                }
            }
        }
    }

    public virtual void handleTargetObject(Collider2D collision)
    {
        //empty on purpose
    }

    public virtual string getTargetTag()
    {
        return "";
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

}
