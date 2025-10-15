using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintPosition : MonoBehaviour
{
	public string name;
	
    private void Awake()
    {
        //Debug.Log(name + "'s Awake localPosition: " + transform.localPosition);
    }
	
    void Start()
    {
        //Debug.Log(name + "'s Start localPosition: " + transform.localPosition);
    }

    void Update()
    {
        //Debug.Log(name + "'s Update localPosition: " + transform.localPosition);
    }
}
