using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    private Transform playerTransform;
    private readonly List<EnemyAI> enemies = new();
    void Awake() => Instance = this;

    public void Register(EnemyAI enemy)
    {
        enemies.Add(enemy);
        if (playerTransform == null)
        {
            Debug.Log("プレイヤーがありません");
        }
        //プレイヤーが既に存在していれば EnemyAI にセット
        if (playerTransform != null)
            Debug.Log("プレイヤーを取得成功");
        enemy.SetPlayer(playerTransform);
    }
    public void SetPlayer(Transform player)
    {
        playerTransform = player;

        //登録済み EnemyAI に player を通知
        foreach (var e in enemies)
            e.SetPlayer(playerTransform);
    }
    public void Unregister(EnemyAI enemy) => enemies.Remove(enemy);

    public Transform GetPlayerTransform() => playerTransform;

    //近くの敵を取得（Boids用）
    public List<EnemyAI> GetNearbyEnemies(EnemyAI self, float radius)
    {
        List<EnemyAI> nearby = new();
        foreach (var e in enemies)
        {
            if (e == self) continue;
            if (Vector3.Distance(e.transform.position, self.transform.position) < radius)
                nearby.Add(e);
        }
        return nearby;
    }

    //警報共有：周囲の敵に追跡を通知
    public void AlertNearbyEnemies(EnemyAI sender, float alertRadius)
    {
        foreach (var e in enemies)
        {
            if (e == sender) continue;
            if (Vector3.Distance(e.transform.position, sender.transform.position) <= alertRadius)
            {
                e.OnAlerted();
            }
        }
    }

    void Update()
    {
        if (playerTransform == null)
        {
            Debug.Log("プレイヤー情報がないため処理を終了しました");
            SetPlayer(GameObject.Find("Player").transform);

            if(playerTransform != null)
                Debug.Log("プレイヤー情報を取得しました");
            else
                Debug.Log("プレイヤー情報を取得できませんでした");
            return;
        }

        foreach (var e in enemies)
        {
            float dist = Vector3.Distance(playerTransform.position, e.transform.position);
            if (dist < 15f) e.ManagedUpdate();       //近距離は毎フレーム
            else if (dist < 30f) { if (Time.frameCount % 2 == 0) e.ManagedUpdate(); }  //中距離
            else { if (Time.frameCount % 5 == 0) e.ManagedUpdate(); }                  //遠距離
        }
    }
}
