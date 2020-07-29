using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class movePlayer : MonoBehaviour
{
    public Rigidbody enemy;
    public Canvas canvas;
    public Text playerName;
    public Text inputName;

    mate info = new mate();
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        playerName = canvas.GetComponentInChildren<Text>();
        //inputName = GameObject.Find("Canvas").GetComponentInChildren<InputField>();
        if (string.IsNullOrEmpty(info.name))
        {
            SetInfo();
        }

        StartCoroutine(MoveObject());
        
    }
    IEnumerator MoveObject()
    {
        enemy = GetComponent<Rigidbody>();
        while(true)
        {
            int dir1 = Random.Range(-4, 4);
            int dir2 = Random.Range(-4, 4);
            yield return new WaitForSeconds(Random.Range(2,8));
            enemy.velocity = new Vector3(dir1, 3, dir2);
        }
    }

    void Update()
    {
        //카메라 각도에 따라 캔버스 y축만 회전시키기)
        Vector3 dir = canvas.transform.position - Camera.main.transform.position;
        dir.Normalize();
        Quaternion q = Quaternion.identity;
        q.y = dir.y;
        //q.x = 0.3f;
        canvas.transform.rotation = q;

        //UI반영
        showInfo();

            //LookAt(-(new Vector3(0, Camera.main.transform.position.y, 0)));
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Coffee"))
        {
            Debug.Log(this.name + "가 커피를 마셨다!");
            other.gameObject.SetActive(false);
        }
    }
    
    void SetInfo()
    {
        info.name = GameManager.name;
        info.hp = 100;
        info.decrease_speed = Random.Range(1, 8);
        info.gender = Random.Range(0, 1);
    }
    
    void showInfo()
    {
        playerName.text = info.name;
        //체력부분 UI 추가하기
    }
    public class mate
    {
        public int gender;
        public string name;
        public float hp;
        public float decrease_speed;
    }
    public void changeName()
    {
        Text UserInput = GameObject.Find("Canvas").GetComponentInChildren<Text>();

        Debug.Log("Input Name : " + UserInput.text);
        //Debug.Log("Current Name : " + playerName.text);
        //playerName.text = inputName.text;
    }
}
