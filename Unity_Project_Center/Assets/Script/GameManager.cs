using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public static long data = 0;

    bool collect = false;
    public Text textButton;

    public GameObject prefabCoffee;
    public Image Panel_Gauge;
    public Text ProjectName;

    public static float[] StagePurpose = new float[] { 13f, 23f, 33f, 43f, 53f, 63f, 73f };
    int stage = 0;
    public static string[] StageName = new string[]
        {"팀프로젝트1 : 인사정책", "팀프로젝트2 : 마케팅전략", "팀프로젝트3 : 상품전략", "팀프로젝트4 : 재무전략",
        "팀프로젝트5 : 마을사업", "팀프로젝트6 : 원물사업", "팀프로젝트7 : 지역정책"};

    private void Awake()
    {
        gm = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Panel_Gauge = GetComponent<UnityEngine.UI.Image>();
        ChangeButtonText();
    }

    // Update is called once per frame
    void Update()
    {
        //UI 위에서 마우스(터치)가 이루어지지 않을 때
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (collect == true)
                collectData();   //함수실행

            else
                createCoffee();
        }

        showDataRate();
    }

    public void collectData()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (data == StagePurpose[stage])
            {
                data = 0;
                stage++;
                showProjectName();
            }
            else
            {
                data += 1;
            }
            Debug.Log(data);
        }
    }

    void createCoffee()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            Debug.Log(Input.mousePosition);
            Instantiate(prefabCoffee, mousePosition, Quaternion.identity); ;
        }
    }

    public void ChangeButtonText()
    {
        //자료수집 : true
        //커피 : false
        if (collect)
        {
            collect = false;
            textButton.text = "커피";
        }
        else
        {
            collect = true;
            textButton.text = "자료수집";
        }

    }

    void showDataRate()
    {
        //Image img = transform.Find("Logo").GetComponentInChildren<Canvas>.GetComponentInChildren<>
        //Image img = transform.Find("Logo").transform.Find("Panel").transform.Find("Image_Gauge").GetComponent<Image>();
        Panel_Gauge.fillAmount = data / StagePurpose[stage];
    }

    void showProjectName()
    {
        ProjectName.text = StageName[stage];
    }

    public static string enemy_name
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

            return names[UnityEngine.Random.Range(0, names.Length)];
        }
    }

    public void changeNickname()
    {
        GameObject Gcanvas = GameObject.Find("InputNicknameCanvas");
        Canvas canvas = GameObject.Find("InputNicknameCanvas").GetComponent<Canvas>();
        Text Nickname = canvas.GetComponentInChildren<InputField>().GetComponentInChildren<Text>();
        EmployeeControl player = GameObject.Find("Player").GetComponent<EmployeeControl>();
        Canvas nextCanvs = GameObject.Find("Logo").GetComponent<Canvas>();
        
        player.setName(Nickname.text);

        Gcanvas.SetActive(false);
        nextCanvs.enabled = true;
    }
}
