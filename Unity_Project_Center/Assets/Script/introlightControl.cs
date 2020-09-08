using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class introlightControl : MonoBehaviour
{
    public Light pinlight;
    // Start is called before the first frame update
    void Start()
    {
        pinlight = GetComponent<Light>();
    }

    public void closeLight()
    {
        StartCoroutine(closeScene());
    }
    IEnumerator closeScene()
    {
        float incresement = 0.3f;
        while (true)
        {
            pinlight.spotAngle -= 1;
            if (pinlight.spotAngle == 1)
                break;
            yield return new WaitForSeconds(incresement--);
        }
        pinlight.enabled = false;
        StopCoroutine(closeScene());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
}
