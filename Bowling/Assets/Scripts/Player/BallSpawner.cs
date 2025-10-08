using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    // 名前（string）で GameObject を仕分けられる
    Dictionary<string, GameObject> ballDict = new Dictionary<string, GameObject>();
    // Inspector で登録できるようにする
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject explosionballPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 登録
        ballDict.Add("Normal", ballPrefab);
        ballDict.Add("Explosion", explosionballPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("joystick button 4"))
        {
            Spawn("Normal");
        }
        else if (Input.GetKeyDown("joystick button 5"))
        {
            Spawn("Explosion");
        }
    }

    void Spawn(string str)
    {
        // すでに召喚されているなら召喚できないように
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null) return;

        Instantiate(ballDict[str]);
        // ボールを召喚する
        GameObject ballObj = GameObject.Find("BallShootManager");
        BallShooter ballShooter = ballObj.GetComponent<BallShooter>();
        ballShooter.BallSelect();
    }

    public void Spawn()
    {
        Instantiate(ballDict["Normal"]);
    }
}
