using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private static CursorController instance;

    public Texture2D cursorDefaultSprite;
    public Texture2D cursorClickedSprite;

    private bool clicked = false;

    private Vector2 clickPosition = new Vector2(0f, 18f);

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        Cursor.SetCursor(cursorDefaultSprite, clickPosition, CursorMode.Auto);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0) && !clicked)
        {
            clicked = true;
            Cursor.SetCursor(cursorClickedSprite, clickPosition, CursorMode.Auto);
            return;
        }

        if(!Input.GetKey(KeyCode.Mouse0) && clicked)
        {
            clicked = false;
            Cursor.SetCursor(cursorDefaultSprite, clickPosition, CursorMode.Auto);
            return;
        }
    }

    private void OnDrawGizmos()
    {
        if (Camera.main == null) return;

        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane + 1f));

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(worldPos, Constants.detectionSize);
    }
}
