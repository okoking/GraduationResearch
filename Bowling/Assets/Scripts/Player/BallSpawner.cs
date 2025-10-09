using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    // ���O�istring�j�� GameObject ���d��������
    Dictionary<string, GameObject> ballDict = new Dictionary<string, GameObject>();
    // Inspector �œo�^�ł���悤�ɂ���
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject explosionballPrefab;

    [SerializeField] private Vector3 SPAWN_POS = new(0f, .5f, 0f);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �o�^
        ballDict.Add("ball1", ballPrefab);
        ballDict.Add("ball2", explosionballPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("joystick button 4"))
        {
            Spawn("ball1");
        }
        else if (Input.GetKeyDown("joystick button 5"))
        {
            Spawn("ball2");
        }
    }

    public void Spawn(string str)
    {
        // ���łɏ�������Ă���Ȃ珢���ł��Ȃ��悤��
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null) return;

        Instantiate(ballDict[str], SPAWN_POS, Quaternion.identity);
        // �{�[������������
        GameObject ballObj = GameObject.Find("BallShootManager");
        BallShooter ballShooter = ballObj.GetComponent<BallShooter>();
        ballShooter.BallSelect();
    }
}
