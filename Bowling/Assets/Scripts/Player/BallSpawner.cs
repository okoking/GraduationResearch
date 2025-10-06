using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    // ñºëOÅistringÅjÇ≈ GameObject Çédï™ÇØÇÁÇÍÇÈ
    Dictionary<string, GameObject> ballDict = new Dictionary<string, GameObject>();
    // Inspector Ç≈ìoò^Ç≈Ç´ÇÈÇÊÇ§Ç…Ç∑ÇÈ
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject explosionballPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ìoò^
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
        Instantiate(ballDict[str]);
    }
}
