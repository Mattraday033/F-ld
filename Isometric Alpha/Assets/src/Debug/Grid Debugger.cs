using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDebugger : MonoBehaviour
{

	public Grid gridToDebug;
	
	public int radius = 25;
	
	private int startRow;
	private int endRow;
	private int startCol;
    private int endCol;

    private GameObject parentObject;

    void Start()
    {
        if (Application.isEditor && State.enableGridDebugger)
        {
            parentObject = new GameObject("Debug Square Parent");
            parentObject.transform.parent = gridToDebug.gameObject.transform;
            parentObject.transform.localPosition = Vector3.zero;
            parentObject.transform.localScale = Vector2.one;

            Vector3Int playerCoords = new Vector3Int(0, 0, 0);

            startRow = playerCoords.x - radius;
            endRow = playerCoords.x + radius;
            startCol = playerCoords.y - radius;
            endCol = playerCoords.y + radius;

            for (int row = startRow; row <= endRow; row++)
            {
                for (int col = startCol; col <= endCol; col++)
                {
                    GameObject currentSquare = Instantiate(Resources.Load<GameObject>("Grid Square Debug"), parentObject.transform);

                    currentSquare.transform.localPosition = gridToDebug.GetCellCenterLocal(new Vector3Int(row, col, 0));
                    currentSquare.GetComponent<DebugSquare>().tmp.text = "(" + row + "," + col + ")";
                }
            }
        }
    }
    
    void Update()
    {
        if(Application.isEditor && parentObject != null && Input.GetKey(KeyCode.LeftAlt) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            parentObject.SetActive(false);
            parentObject = null;
            KeyPressManager.handlingPrimaryKeyPress = true;
        }
    }
}
