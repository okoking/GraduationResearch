using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    // ���O�istring�j�� GameObject ���d��������
    Dictionary<string, GameObject> ballDict = new Dictionary<string, GameObject>();
    // Inspector �œo�^�ł���悤�ɂ���
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject explosionballPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �o�^
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
        // ���łɏ�������Ă���Ȃ珢���ł��Ȃ��悤��
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null) return;

        Instantiate(ballDict[str]);
        // �{�[������������
        GameObject ballObj = GameObject.Find("BallShootManager");
        BallShooter ballShooter = ballObj.GetComponent<BallShooter>();
        ballShooter.BallSelect();
    }

    public void Spawn()
    {
        Instantiate(ballDict["Normal"]);
    }
}
