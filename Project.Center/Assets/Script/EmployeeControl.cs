using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void showDataRate()
    {
        Image img = transform.Find("Logo/Panel/Image").GetComponent<Image>();
        img.fillAmount = GameManager.data / 14f;
    }
}
