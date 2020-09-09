using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardControl : MonoBehaviour
{
    public Material learboardMaterial;
    public MeshRenderer planeMaterial;
    public Light pinlight;
    public Text playtime;
    public Text penalty;
    public Text coffee;
    public Text trycount;

    bool dbflag =false ;

    // Start is called before the first frame update
    void Start()
    {
        planeMaterial = GameObject.Find("Plane").GetComponent<MeshRenderer>();
        pinlight = GameObject.Find("Directional Light").GetComponent<Light>();
        playtime = GameObject.Find("PlaytimeText").GetComponent<Text>();
        penalty = GameObject.Find("PenaltyText").GetComponent<Text>();
        coffee = GameObject.Find("CoffeeText").GetComponent<Text>();
        trycount = GameObject.Find("TryCountText").GetComponent<Text>();
        StartCoroutine(openScene());
        StartCoroutine(callMysql(playtime,"playtime","초","select name, playtime, @curRank := @curRank + 1 AS rank from leaderboard l, (Select @curRank:= 0) r order by playtime desc limit 5;"));
        StartCoroutine(callMysql(penalty,"penalty","점" , "select name, penalty, @curRank := @curRank + 1 AS rank from leaderboard l, (Select @curRank:= 0) r order by penalty limit 5;"));
        StartCoroutine(callMysql(coffee,"coffee","번" , "select name, coffee, @curRank := @curRank + 1 AS rank from leaderboard l, (Select @curRank:= 0) r order by coffee desc limit 5;"));
        StartCoroutine(callMysql(trycount,"trycount","번", "select name, trycount, @curRank := @curRank + 1 AS rank from leaderboard l, (Select @curRank:= 0) r order by trycount desc limit 5;"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(closeScene());
        }
        else if (pinlight.enabled == false && dbflag == false)
        {
            planeMaterial.material = learboardMaterial;
            StartCoroutine(openScene());
            dbflag = true;
        }
        else if (pinlight.spotAngle >= 140 && dbflag == true)
        {
            playtime.enabled = true;
            penalty.enabled = true;
            coffee.enabled = true;
            trycount.enabled = true;
        }
    }

    IEnumerator openScene()
    {
        float incresement = 0.3f;
        pinlight.enabled = true;
        while (true)
        {
            pinlight.spotAngle += 1;
            if (pinlight.spotAngle > 140f)
                break;
            else if (pinlight.spotAngle == 45 && !dbflag)
                yield return new WaitForSeconds(2f);
            yield return new WaitForSeconds(incresement--);
        }
        StopCoroutine(openScene());
    }

    IEnumerator closeScene()
    {
        float incresement = 0.3f;
        while (true)
        {
            pinlight.spotAngle -= 1;
            if (pinlight.spotAngle == 1)
                break;
            yield return new WaitForSeconds(incresement--);
        }
        pinlight.enabled = false;
        yield return new WaitForSeconds(1f);
        StopCoroutine(closeScene());
    }

    IEnumerator callMysql(Text text,string target, string unit, string query)
    {
        MySqlDataReader reader;
        try
        {
            reader = DBConnector.Instance.doQuery(query);
            while(reader.Read())
                text.text += (reader["rank"] + "위\t" +reader["name"] + "\t : \t" + reader[target] + unit+"\n");
            reader.Close();
        }
        catch (MySqlException ex)
        {
            Debug.Log("DB Load Fail");
        }
        yield return null;
    }
}
