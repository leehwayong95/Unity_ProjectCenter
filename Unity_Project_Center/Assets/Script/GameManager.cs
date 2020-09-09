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
using MySql.Data.MySqlClient;
using System.Text;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public long data = 0;

    public static bool collect = false; //true : 자료수집 , false : 커피

    public GameObject prefabCoffee;
    public Image Panel_Gauge; //Data 수집 UI

    public List<Mate> emps;
    public string savePath;
    public GameObject prefabEmployee;

    public Image Panel_TimeGauge; //남은 Time UI
    public float limitTime = 60;

    public static float[] StagePurpose = new float[] { 13f, 23f, 33f, 43f, 53f, 63f, 73f };
    public int stage = 0;

    public Sprite[] ProjectName;
    public Image Project_Name;
    public Sprite[] ButtonStates;
    public Image Button_img;

    //Penalty UI object
    public GameObject Fail;
    public GameObject BP;
    public GameObject Conflict;
    public GameObject Late;


    //DB에 저장하는것들
    public static String userName;
    public int playTime = 0;
    public int createcoffeeCount = 0;
    public static int penalty = 0;
    public int tryCount = 0; //재시도 횟수

    //DBConnector
    DBConnector dbConnector;
    bool doneFlag = false;

    //light Control
    introlightControl intropinLight;

    private void Awake()
    {
        gm = this;
        DontDestroyOnLoad(this);
        intropinLight = GameObject.Find("Directional Light").GetComponent<introlightControl>();
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
        ChangeButtonImage();
    }

    void Update()
    {
        //UI 위에서 마우스(터치)가 이루어지지 않을 때
        if (!EventSystem.current.IsPointerOverGameObject() && stage < 7)
        {
            if (collect == true)
                collectData();   //함수실행

            else
                createCoffee();
            showDataRate();
            showTimeRate();
        }
        else if(stage == 7) //7stage 클리어시
        {
            StopCoroutine(countPlayTime());
            inGamelightControl light = GameObject.Find("Directional Light").GetComponent<inGamelightControl>();
            //여기에 DB push
            if (!doneFlag)
            {
                StartCoroutine(callMysql());
                light.restartScene();
            }
            else
                StopCoroutine(callMysql());
        }

        if (penalty < 0)
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
                EmployeeControl victim = hit.transform.GetComponent<EmployeeControl>();
                victim.playWhipSound();
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
            createcoffeeCount++;
        }
    }

    public void ChangeButtonImage()
    {
        //자료수집 : true
        //커피 : false
        if (collect)
        {
            collect = false;
            Button_img.sprite = ButtonStates[0];
        }
        else
        {
            collect = true;
            Button_img.sprite = ButtonStates[1];
        }

    }

    void showDataRate()
    {
        Panel_Gauge.fillAmount = data / StagePurpose[stage];
    }

    void showTimeRate()
    {
        if (EmployeeControl.startflag)
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
                    Debug.Log(GameManager.penalty);
                    Late.SetActive(false);
                    limitTime = 50;
                    Time.timeScale = 1.0f;
                }
            }
        }
    }

    void penaltyControl()
    {
        if(penalty <= -20 && Fail.activeSelf == false)
        {
            Fail.SetActive(true);
            //Time.timeScale = 0.0f;
        }
    }

    void showProjectName()
    {
        Project_Name.sprite = ProjectName[stage];
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
            string[] names = new string[9];
            names[0] = "화용";
            names[1] = "정열";
            names[2] = "인영";
            names[3] = "용훈";
            names[4] = "동원";
            names[5] = "유정";
            names[6] = "덕용";
            names[7] = "대우";
            names[8] = "동혁";

            return names[UnityEngine.Random.Range(0, names.Length)];
        }
    }

    public void changeNickname()
    {
        //GameObject Gcanvas = GameObject.Find("InputNicknameCanvas");
        Canvas canvas = GameObject.Find("InputNicknameCanvas").GetComponent<Canvas>();
        //Canvas nextCanvas = GameObject.Find("Logo").GetComponent<Canvas>();
        Text Nickname = canvas.GetComponentInChildren<InputField>().GetComponentInChildren<Text>();
        //VideoStop();
        userName = Nickname.text.ToString().Trim();
        canvas.enabled = false;

        //EmployeeControl.gameStart();
        //PlayerControl.gameStart();
            intropinLight.closeLight();//SceneLoad 포함
        //nextCanvas.enabled = true;
        StartCoroutine(countPlayTime());
    }

    public void Restart()//재시작 기능
    {
        PlayerControl.gameStop();
        EmployeeControl.gameStop();
        Canvas nextCanvas = GameObject.Find("Logo").GetComponent<Canvas>();
        //inGamelightControl light = GameObject.Find("Directional Light").GetComponent<inGamelightControl>();
        nextCanvas.enabled = false;
        penalty = 0;
        Fail.SetActive(false);
        Time.timeScale = 1.0f;
        limitTime = 60;
        data = 0;
        stage = 0;
        tryCount++;
        //StopCoroutine(countPlayTime());
        //StartCoroutine(light.closeScene());
        SceneManager.LoadScene(0);
    }

    public static void VideoStop()
    {
        VideoPlayer videoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
        videoPlayer.Stop();
    }

    IEnumerator countPlayTime()
    {
        while (true)
        {
            playTime++;
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator callMysql()
    {
        //String 인코딩
        byte[] bytesForEncoding = UTF8Encoding.UTF8.GetBytes(userName);
        string encodedString = UTF8Encoding.UTF8.GetString(bytesForEncoding);
        MySqlDataReader reader = DBConnector.Instance.doQuery
            ("insert into leaderboard values " +
            "(0,\"" + encodedString + "\"," + playTime + "," + createcoffeeCount + "," + penalty + "," + tryCount + ");"
            );
        doneFlag = true;
        DBConnector.closeSqlConnection();
        yield return null;
    }
}