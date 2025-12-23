using UnityEngine;
using System;

public class DoorDeath : MonoBehaviour
{
    public int doorIndex; // 0,1,2...

    private BossHp doorHp;
    private bool isOpened = false;

    // どのドアが開いたか通知
    public static event Action<int> OnDoorOpened;

    void Start()
    {
        doorHp = GetComponent<BossHp>();
    }

    void Update()
    {
        if (!isOpened && doorHp.GetIsDeath())
        {
            isOpened = true;

            // 自分の番号を通知
            OnDoorOpened?.Invoke(doorIndex);

            Destroy(gameObject);
        }
    }
}
