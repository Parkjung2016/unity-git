using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMouseCursor : MonoBehaviour
{
    public Texture2D cursorImg;
    public Texture2D cursorClickImg;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
        Cursor.SetCursor(cursorClickImg, Vector2.zero, CursorMode.ForceSoftware);

        }
        else
        {
        Cursor.SetCursor(cursorImg, Vector2.zero, CursorMode.ForceSoftware);

        }
    }
}
