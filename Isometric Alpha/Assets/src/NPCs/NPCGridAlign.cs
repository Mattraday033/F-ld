using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGridAlign : MonoBehaviour
{
	private Grid grid;



    void Start()
    {
		grid = transform.parent.GetComponent<Grid>();
		
        transform.localPosition = colliderLocalPosition();
    }

	private Vector3 colliderLocalPosition()
	{
		return grid.GetCellCenterLocal(grid.LocalToCell(transform.localPosition));
	}

}
