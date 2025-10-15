using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NPCParent : MonoBehaviour
{
	private static NPCParent instance;

	private void Awake()
	{
		if(instance != null)
		{
			throw new IOException("Another NPCParent exists");
		}
		
		instance = this;
	}

	public static NPCParent getInstance()
	{
		return instance;
	}
}
