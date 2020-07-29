using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class convert2 : MonoBehaviour
{
    // Start is called before the first frame update
    Canvas canvas2;
    void Start()
    {
        canvas2 = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enable()
    {
        canvas2.enabled = true;
    }
}
