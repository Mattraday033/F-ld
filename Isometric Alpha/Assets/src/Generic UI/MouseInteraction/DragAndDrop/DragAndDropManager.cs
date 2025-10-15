using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class DragAndDropManager
{
    public const float timeToWait = .5f;
    public static UnityEvent<IDescribable> OnDragAndDropCreated = new UnityEvent<IDescribable>();
    public static UnityEvent<IDescribable> OnDragAndDropDestroyed = new UnityEvent<IDescribable>();

    public static void createDragAndDrop(IDragAndDropSource source, IDescribable objectBeingDragged)
    {
        IDragAndDropContainer dragAndDrop = GameObject.Instantiate(Resources.Load<GameObject>(source.getDragAndDropPrefabName()), MouseHoverManager.getDragAndDropBase().transform).GetComponent<IDragAndDropContainer>();

        dragAndDrop.setObjectBeingDragged(objectBeingDragged);

        OnDragAndDropCreated.Invoke(objectBeingDragged);
    }

    public static IEnumerator waitForMouseRelease(IDragAndDropSource source, IDescribable objectBeingDragged)
    {
        float timeWaited = 0f;

        while (Input.GetKey(KeyCode.Mouse0) && timeWaited < timeToWait/1.5f)
        {
            yield return null;

            timeWaited += Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            // Debug.LogError("Time Passed: " + timeWaited);

            createDragAndDrop(source, objectBeingDragged);
        }
    }

}
