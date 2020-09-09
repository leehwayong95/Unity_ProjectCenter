using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class inGamelightControl : MonoBehaviour
{
    public Light pinlight;
    // Start is called before the first frame update
    private void Awake()
    {
        pinlight = GetComponent<Light>();
        pinlight.enabled = true;
    }
    void Start()
    {
        StartCoroutine(openScene()); 
    }

    IEnumerator openScene()
    {
        float incresement = 0.3f;
        while (true)
        {
            pinlight.spotAngle += 1;
            if (pinlight.spotAngle > 99f)
                break;
            yield return new WaitForSeconds(incresement--);
        }
        StopCoroutine(openScene());
    }

    public void restartScene()
    {
        StartCoroutine(closeScene());
    }

    IEnumerator closeScene()
    {
        float incresement = 0.3f;
        while (true)
        {
            pinlight.spotAngle -= 1;
            if (pinlight.spotAngle <= 1)
                break;
            yield return new WaitForSeconds(incresement--);
        }
        pinlight.enabled = false;
        StopCoroutine(closeScene());
        yield return new WaitForSeconds(1f);
        if (GameManager.gm.stage < 7)
            SceneManager.LoadScene(0);
        else
            SceneManager.LoadScene(3);

    }
}
