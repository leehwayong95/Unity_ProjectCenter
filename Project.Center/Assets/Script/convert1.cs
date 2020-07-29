using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Collections.LowLevel.Unsafe;
using TMPro;

public class convert1 : MonoBehaviour
{
    public Canvas canvas1;
    public Canvas canvas2;
    
    // Start is called before the first frame update
    void Start()
    {
        canvas1 = GetComponent<Canvas>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            nextCanvas1();
    }

    public void nextCanvas1()
    {
        //convert2.enabled 
        canvas2.enabled = true;
        canvas1.enabled = false;
    }
}
