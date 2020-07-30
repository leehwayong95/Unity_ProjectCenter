﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Monetization;
using UnityEngine.UI;

public class EmployeeControl : MonoBehaviour
{
    public Rigidbody enemy;
    public Canvas canvas;
    public Text enemyName;
    mate info = new mate();
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

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = canvas.transform.position - Camera.main.transform.position;
        dir.Normalize();
        Quaternion q = Quaternion.identity;
        q.y = dir.y;
        canvas.transform.rotation = q;


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

    public class mate
    {
        public int gender;
        public string name;
        public float hp;
        public float decrease_speed;
    }
}
