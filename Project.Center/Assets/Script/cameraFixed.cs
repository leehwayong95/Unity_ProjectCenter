using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraFixed : MonoBehaviour
{
    public Canvas coworker_canvas;
    public Camera quater;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = quater.WorldToViewportPoint(target.position);

        coworker_canvas.transform.position =
            new Vector3(screenPos.x, screenPos.y, coworker_canvas.transform.position.z);
    }
}
