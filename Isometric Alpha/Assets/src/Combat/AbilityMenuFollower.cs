using UnityEngine;

//Places the shared ability menu, which lives on a screen space canvas, over the combatant it's opened for.
//The combat camera doesn't move, so the menu is placed once each time it's shown rather than every frame
[RequireComponent(typeof(RectTransform), typeof(AbilityMenuManager))]
public class AbilityMenuFollower : MonoBehaviour
{
    public Vector3 worldOffset = Vector3.zero;

    private RectTransform rect;
    private Canvas canvas;
    private Transform target;

    private void Awake()
    {
        rect = (RectTransform)transform;
        canvas = GetComponentInParent<Canvas>().rootCanvas;
    }

    public void setTarget(Transform target)
    {
        this.target = target;
    }

    //called by the AbilityMenuManager before it reveals the menu
    public void moveToTarget()
    {
        if(target == null)
        {
            return;
        }

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position + worldOffset);

        if(RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)rect.parent, screenPoint, canvas.worldCamera, out Vector2 localPoint))
        {
            //localPosition is in the parent's space like localPoint, so the menu's anchors don't offset it
            rect.localPosition = new Vector3(localPoint.x, localPoint.y, rect.localPosition.z);
        }
    }
}
