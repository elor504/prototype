using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    public Texture2D cursorClickSprite, cursorHoldSprite, cursorPointSprite;
    public static CursorState cursorState;

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            if (cursorState == CursorState.Gameplay)
            {
                Cursor.SetCursor(cursorHoldSprite, Vector2.zero, CursorMode.Auto);
            }
            else if (cursorState == CursorState.Menus)
            {
                Cursor.SetCursor(cursorClickSprite, Vector2.zero, CursorMode.Auto);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorPointSprite, Vector2.zero, CursorMode.Auto);
        }
    }
}
public enum CursorState
{ 
    Gameplay,
    Menus
}