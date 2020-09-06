using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public Texture2D[] cursor = new Texture2D[2]; // 0 : 채찍, 1 : 커피
    public bool hotSpotIsCenter = false;
    public Vector2 adjustHotspot = Vector2.zero;
    private Vector2 hotSpot;

    // Start is called before the first frame update
    void Update()
    {
        StartCoroutine(showCursor());
    }

    IEnumerator showCursor()
    {
        yield return new WaitForEndOfFrame();
        if(GameManager.collect)
            Cursor.SetCursor(cursor[0], Vector2.zero, CursorMode.Auto);
        else
            Cursor.SetCursor(cursor[1], Vector2.zero, CursorMode.Auto);
    }
}
