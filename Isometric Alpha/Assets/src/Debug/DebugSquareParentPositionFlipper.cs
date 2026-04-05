using UnityEngine;
using System;
using System.Collections;

[ExecuteAlways]
public class DebugSquareParentPositionFlipper : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(flip());
    }

    private IEnumerator flip()
    {
        yield return null;

        Debug.LogError("transform.parent.name = " + transform.parent.name);
        Debug.LogError("transform.parent.localPosition = " + transform.parent.localPosition);

        transform.localPosition = Vector3.Scale(transform.parent.localPosition, new Vector3(-1f, -1f, -1f));
    }
}
