﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeControl : MonoBehaviour
{
    public Rigidbody enemy;
    public Canvas canvas;
    public Text enemyName;
    public mate info = new mate();
    public Image hp_Gauge; 

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

    void HpDecreaseAuto()
    {
        if (info.hp > 0)
        {
            info.hp -= Time.deltaTime * info.decrease_speed;      
        }
        else
        {
            GameManager.gm.Conflict.SetActive(true);
            Time.timeScale = 0.0f;

            if(Input.GetMouseButtonDown(0))
            {
                GameManager.gm.penalty -= 3;
                Debug.Log(GameManager.gm.penalty);
                GameManager.gm.Conflict.SetActive(false);
                info.hp = 50;
                Time.timeScale = 1.0f;
            }


        }
    }

    void Update()
    {
        Vector3 dir = canvas.transform.position - Camera.main.transform.position;
        dir.Normalize();
        Quaternion q = Quaternion.identity;
        q.y = dir.y;
        canvas.transform.rotation = q;

        //Info 반영
        showInfo();
        HpDecreaseAuto();
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Coffee"))
        {
            Debug.Log(this.name + "가 커피를 마셨다!");
            GameObject.Destroy(collision.gameObject);
            if(!(info.hp >= 100))
                info.hp += 5;
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
        hp_Gauge.fillAmount = info.hp / 100;
    }

    public void setName(string name)
    {
        info.name = name;
    }

    public class mate
    {
        public int gender;
        public string name;
        public float hp;
        public float decrease_speed;
    }
}