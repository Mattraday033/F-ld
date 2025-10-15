using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutOutMaskPositionSetter : MonoBehaviour
{
    private void Awake()
    {
        transform.localPosition = new Vector3(0, 0, 0);
    }
}
