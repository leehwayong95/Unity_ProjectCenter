using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody enemy;
    public Canvas canvas;
    public Text enemyName;
    public mate info = new mate();

    // Start is called before the first frame update
    void Start()
    {

        canvas = GetComponentInChildren<Canvas>();
        enemyName = canvas.GetComponentInChildren<Text>();
        if (string.IsNullOrEmpty(info.name))
        {
            setInfo();
        }
        StartCoroutine(moveEnemy());
        //StartCoroutine(HpDecreaseAuto());
    }

    IEnumerator moveEnemy()
    {
        enemy = GetComponent<Rigidbody>();
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2, 8));
            enemy.velocity = new Vector3(Random.Range(-4, 4), 3, Random.Range(-4, 4));
        }
    }

    IEnumerator HpDecreaseAuto()
    {
        while(true)
        {
            info.hp -= info.decrease_speed;
            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = canvas.transform.position - Camera.main.transform.position;
        dir.Normalize();
        Quaternion q = Quaternion.identity;
        q.y = dir.y;
        canvas.transform.rotation = q;

        //Info 반영
        showInfo();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Coffee"))
        {
            Debug.Log(this.name + "가 커피를 마셨다!");
            GameObject.Destroy(collision.gameObject);
        }

    }
    void setInfo()
    {
        //info 정보 지정
        info.name = GameManager.enemy_name;
        info.hp = 100;
        info.decrease_speed = Random.Range(1, 8);
        info.gender = Random.Range(0, 1);
    }

    void showInfo()
    {
        enemyName.text = info.name;
    }

    public void setName(string name)
    {
        info.name = name;
    }

    public void showuiInfo()
    {
        Canvas canvas = GetComponentInChildren<Canvas>();
        Transform infoGroup = canvas.transform.GetChild(1);
        Text name = infoGroup.transform.GetChild(3).GetComponent<Text>();
        Text penalty = infoGroup.transform.GetChild(4).GetComponent<Text>();
        Text gender = infoGroup.transform.GetChild(5).GetComponent<Text>();

        name.text = info.name;
        penalty.text = info.hp.ToString(); // 패널티 관련함수 추가
        if (info.gender == 0)
            gender.text = "여자";
        else
            gender.text = "남자";
        infoGroup.gameObject.SetActive(true);
    }

    public void closeuiInfo()
    {
        GameObject infoGroup = GameObject.Find("Info");
        cameraMove camera = GameObject.Find("Main Camera").GetComponent<cameraMove>();
        camera.CallquarterView();
        infoGroup.SetActive(false);
    }

    public class mate
    {
        public int gender;
        public string name;
        public float hp;
        public float decrease_speed;
    }
}