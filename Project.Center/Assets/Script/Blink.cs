using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Blink : MonoBehaviour
{
    Text blink_Text;

    // Start is called before the first frame update
    void Start()
    {
        blink_Text = GetComponent<Text>();
        StartCoroutine(BlinkText());
    }

    public IEnumerator BlinkText()
    {
        while(true)
        {
            blink_Text.text = "";
            yield return new WaitForSeconds(.5f);
            blink_Text.text = "Press to start";
            yield return new WaitForSeconds(.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
