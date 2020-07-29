using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inputName : MonoBehaviour
{
    [SerializeField] private InputField input_Name;

    public void sendName()
    {
        Debug.Log(input_Name.text);
    }
}
