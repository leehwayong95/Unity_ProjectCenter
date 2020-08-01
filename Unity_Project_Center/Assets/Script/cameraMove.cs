using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    bool focusMode = false;
    Vector3 quarterView = new Vector3(11, 12, -11);
    // Start is called before the first frame update
    void Start()
    {
        
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
        focusMode = true;
    }

    public void CallquarterView()
    {
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
        Vector3 targetPosition = new Vector3(player.position.x + 3, player.position.y + 4.5f, player.position.z - 3 );
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, 0.1f);
        yield return null;
    }
}
