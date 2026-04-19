using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private static CursorController instance;

    public Texture2D cursorDefaultSprite;
    public Texture2D cursorClickedSprite;

    private bool clicked = false;

    private Vector2 clickPosition = Vector2.zero;

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
}
