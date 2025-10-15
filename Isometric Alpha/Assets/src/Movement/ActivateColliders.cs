using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateColliders : MonoBehaviour
{
	public GameObject colliders;
	
    // Start is called before the first frame update
    void Start()
    {
        colliders.SetActive(true);
    }

}
