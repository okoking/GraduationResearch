using UnityEngine;
using System;

public class DoorDeath : MonoBehaviour
{
    private BossHp doorHp;

    private bool isOpened = false;

    public static event Action OnDoorOpened;

    void Start()
    {
        doorHp = GetComponent<BossHp>();
    }

    void Update()
    {
        if (!isOpened && doorHp.GetIsDeath())
        {
            isOpened = true;

            // ドアが開いた通知
            OnDoorOpened?.Invoke();

            // ドアを消す（開く）
            Destroy(gameObject);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            doorHp.TakeDamage(10);
        }
    }
}