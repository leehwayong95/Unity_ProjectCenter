using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextCanvas : MonoBehaviour
{
    public Canvas canvas1;
    Canvas this_canvas;
    void Start()
    {
        this_canvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            canvas1.enabled = true;
            this_canvas.enabled = false;
        }
    }
}
