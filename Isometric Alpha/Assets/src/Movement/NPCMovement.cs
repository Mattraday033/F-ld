using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{

	private CharacterFacing npcFacing;
	
	private void Awake()
	{
		if(npcFacing == null)
		{
            npcFacing = new CharacterFacing();
        }
	}
	
	public CharacterFacing getFacing()
	{
        if (npcFacing == null)
        {
            npcFacing = new CharacterFacing();
        }

        return npcFacing;
	}

}
