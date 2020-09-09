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
    }

    public void restartScene()
    {
        StartCoroutine(closeScene());
    }

    public IEnumerator closeScene()
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
        
        yield return new WaitForSeconds(1f);
        if (GameManager.gm.stage < 7)
        {
            Debug.Log("u r loser");
            //SceneManager.LoadScene(2);
        }
        else
        {
            Debug.Log("u r winner");
            Canvas canvas = GameObject.Find("Logo").GetComponent<Canvas>();
            canvas.enabled = false;
            SceneManager.LoadScene(3);
        }
    }
}
