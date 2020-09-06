using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    bool focusMode = false;
    Vector3 quarterView;
    Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GetComponent<Transform>();
        quarterView = new Vector3(cameraTransform.transform.position.x, cameraTransform.transform.position.y, cameraTransform.transform.position.z);    
    }

    // Update is called once per frame
    void Update()
    {
        if (focusMode)
        {
            StopCoroutine(focusQuartorview());
            StartCoroutine(focusPlayer());
        }
        else
        {
            StopCoroutine(focusPlayer());
            StartCoroutine(focusQuartorview());
        }
    }

    public void CallfocusPlayer()
    {
        Time.timeScale = 0.0f;
        focusMode = true;
    }

    public void CallquarterView()
    {
        Time.timeScale = 1.0f;
        focusMode = false;
    }

    IEnumerator focusQuartorview()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, quarterView, 0.1f);
        yield return null;
    }
    IEnumerator focusPlayer()
    {
        Transform player = GameObject.Find("Player").GetComponent<Transform>();
        Vector3 targetPosition = new Vector3(player.position.x + 2, player.position.y + 5.5f, player.position.z - 3 );
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, 0.1f);
        yield return null;
    }
}
