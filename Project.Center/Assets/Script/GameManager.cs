using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gm;
    public GameObject prefabCoffee;
    public void Awake()
    {
        gm = this;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        createCoffee(); 
    }

    void createCoffee ()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                Debug.Log(mousePosition);
                Instantiate(prefabCoffee, mousePosition, Quaternion.identity);
            }
            else
            { }
        }
    }

    public static string name
    {
        get
        {
            string[] names = new string[8];

            names[0] = "화용";
            names[1] = "정열";
            names[2] = "인영";
            names[3] = "용훈";
            names[4] = "동원";
            names[5] = "유정";
            names[6] = "덕용";
            names[7] = "형미";

            int r = Random.Range(0, names.Length);
            string s = names[r];

            return s;
        }
    }
}
