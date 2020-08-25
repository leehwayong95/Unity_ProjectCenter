using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.UNetWeaver;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public static long data = 0;

    bool collect = false;
    public Text textButton;

    public GameObject prefabCoffee;
    public Image Panel_Gauge; //Data 수집 UI
    public Text ProjectName;

    public List<Mate> emps;
    public string savePath;
    public GameObject prefabEmployee;

    public Image Panel_TimeGauge; //남은 Time UI
    public float limitTime = 60;
    public int penalty = 0;

    public int tryCount = 0; //재시도 횟수

    public static float[] StagePurpose = new float[] { 13f, 23f, 33f, 43f, 53f, 63f, 73f };
    public int stage = 0;
    public static string[] StageName = new string[]
        {"팀프로젝트1 : 인사정책", "팀프로젝트2 : 마케팅전략", "팀프로젝트3 : 상품전략", "팀프로젝트4 : 재무전략",
        "팀프로젝트5 : 마을사업", "팀프로젝트6 : 원물사업", "팀프로젝트7 : 지역정책"};
    //Penalty UI object
    public GameObject Fail;
    public GameObject BP;
    public GameObject Conflict;
    public GameObject Late;
    public static String userName;

    private void Awake()
    {
        gm = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        emps = new List<Mate>();
        if (System.IO.File.Exists(savePath))
        {
            Load();
            foreach (var a in emps)
            {
                InitializeEmployee(a);
            }
        }
        ChangeButtonText();
    }

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
        showTimeRate();

        if(penalty < 0)
            penaltyControl();
    }

    public void collectData()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit, 100); 
            if (data == StagePurpose[stage])
            {
                data = 0;
                limitTime = 60;
                stage++;
                showProjectName();
            }
            else if(hit.transform.tag == "Coworker")
            {
                data += 1;
                hit.rigidbody.velocity = new Vector3(UnityEngine.Random.Range(-4, 4), 3, UnityEngine.Random.Range(-4, 4));
                Debug.Log("click coworker");
            }
            else if(hit.transform.tag == "Player")
            {
                cameraMove camera = GameObject.Find("Main Camera").GetComponent<cameraMove>();
                camera.CallfocusPlayer();
                PlayerControl player = GameObject.Find("Player").GetComponent<PlayerControl>();
                player.showuiInfo();
            }
            else
                Debug.Log("click nothing");
        }
    }

    void createCoffee()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //레이져 쏴서 닿는 좌표구하기
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 1000);
            Debug.Log(hit.point);
            Vector3 hitpoint = new Vector3(hit.point.x, 2, hit.point.z);
            Instantiate(prefabCoffee, hitpoint, Quaternion.identity); ;
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
        Panel_Gauge.fillAmount = data / StagePurpose[stage];
    }

    void showTimeRate()
    {
        if (limitTime > 0)
        {
            limitTime -= Time.deltaTime;
            Panel_TimeGauge.fillAmount = limitTime / 60;
        }

        else
        {
            Late.SetActive(true);
            //Time.timeScale = 0.0f;

            if (Input.GetMouseButtonDown(0))
            {
                penalty -= 3;
                Debug.Log(GameManager.gm.penalty);
                Late.SetActive(false);
                limitTime = 50;
                Time.timeScale = 1.0f;
            }
        }
    }

    void penaltyControl()
    {
        if(penalty <= -20)
        {
            Fail.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    void showProjectName()
    {
        ProjectName.text = StageName[stage];
    }

    public void Save()
    {
        //PlayerPrefs.SetInt("MONEY", (int)money);
        SaveData sd = new SaveData();
        sd.data = data;
        sd.penalty = penalty;
        sd.stage = stage;
        sd.limitTime = limitTime;
        sd.empList = emps;
    }

    public void Load()
    {
        //money = PlayerPrefs.GetInt("MONEY", 1000);        Save();
        SaveData sd = XmlManager.XmlLoad<SaveData>(savePath);
        data = sd.data;
        penalty = sd.penalty;
        stage = sd.stage;
        limitTime = sd.limitTime;
        emps = sd.empList;
    }

    public void InitializeEmployee(Mate e)
    {
        GameObject obj = Instantiate(prefabEmployee, Vector3.zero, Quaternion.identity);
        obj.GetComponent<EmployeeControl>().info = e;
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

        
        //GameObject Gcanvas = GameObject.Find("InputNicknameCanvas");
        Canvas canvas = GameObject.Find("InputNicknameCanvas").GetComponent<Canvas>();
        Canvas nextCanvas = GameObject.Find("Logo").GetComponent<Canvas>();
        Text Nickname = canvas.GetComponentInChildren<InputField>().GetComponentInChildren<Text>();
        userName = Nickname.text.ToString();

        SceneManager.LoadScene(1);
        nextCanvas.enabled = true;

        //씬 불러오고, Gamemanager distory 안되게끔 
        /*
        PlayerControl player = GameObject.Find("Player").GetComponent<PlayerControl>();
        Canvas nextCanvs = GameObject.Find("Logo").GetComponent<Canvas>();
        
        player.setName(Nickname.text);

        Gcanvas.SetActive(false);
        nextCanvs.enabled = true;
        Time.timeScale = 1f;
        */

    }

    public void Restart()
    {
        /************ 수정해야함 ************
        GameObject popupMessage = GameObject.Find("Fail");
        Canvas Logo = GameObject.Find("Logo").GetComponent<Canvas>();

        this.limitTime = 60;
        this.stage = 0;

        Logo.enabled = true;
        popupMessage.SetActive(false);
        ************ 수정해야함 *************/
    }

    public static void VideoStop()
    {
        VideoPlayer videoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
        videoPlayer.Stop();
    }
}
