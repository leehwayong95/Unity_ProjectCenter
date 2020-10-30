using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialLight : MonoBehaviour
{

    public Material[] tutorial = new Material[11];
    public MeshRenderer plane;
    public Light pinlight;
    public int touchcount = 0;

    public Sprite[] click = new Sprite[2];
    public Image mouse;
    // Start is called before the first frame update
    void Start()
    {
        plane = GameObject.Find("Plane").GetComponent<MeshRenderer>();
        plane.material = tutorial[touchcount++];

        pinlight = GetComponent<Light>();
        pinlight.enabled = true;
        StartCoroutine(openScene());

        mouse = GameObject.Find("Image").GetComponent<Image>();
        StartCoroutine(clickPulse());
    }

    // Update is called once per frame
    void Update()
    {
        if (touchcount >= 10 )
        {
            if (pinlight.enabled == false)
            {
                SceneManager.LoadScene(2);
                Canvas nextCanvas = GameObject.Find("Logo").GetComponent<Canvas>();
                EmployeeControl.gameStart();
                PlayerControl.gameStart();
                nextCanvas.enabled = true;
            }
            else if(pinlight.spotAngle >= 170)
                StartCoroutine(closeScene());
        }
        else if (Input.GetMouseButtonUp(0) && pinlight.spotAngle >= 170)
        {
                plane.material = tutorial[touchcount++];
        }
    }
    IEnumerator openScene()
    {
        float incresement = 0.3f;
        while (true)
        {
            pinlight.spotAngle += 1;
            if (pinlight.spotAngle > 170f)
                break;
            else if (pinlight.spotAngle == 23)
                yield return new WaitForSeconds(2f);
            yield return new WaitForSeconds(incresement--);
        }
        StopCoroutine(openScene());
    }
    IEnumerator closeScene()
    {
        float incresement = 0.3f;
        while (true)
        {
            pinlight.spotAngle -= 1;
            if (pinlight.spotAngle == 1)
                break;
            else if (pinlight.spotAngle == 23)
                yield return new WaitForSeconds(2f);
            yield return new WaitForSeconds(incresement--);
        }
        pinlight.enabled = false;
        yield return new WaitForSeconds(1f);
        StopCoroutine(closeScene());
    }

    IEnumerator clickPulse()
    {
        while (true)
        {
            mouse.sprite = click[0];
            yield return new WaitForSeconds(1f);
            mouse.sprite = click[1];
            yield return new WaitForSeconds(1f);
        }
    }
}
