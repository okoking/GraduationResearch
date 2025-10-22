using UnityEngine;

public class BossHand : MonoBehaviour
{
    //ターゲット(プレイヤー)
    Transform player;

    //主(ボスの手を出す透明のやつ)
    public Transform bossHandSpawn;

    //ビーム(攻撃)
    public GameObject beam;

    public float orbitRadius    = 5f;   //ボスからの距離
    public float orbitSpeed     = 30f;  //回転スピード
    public float floatAmplitude = 1f;   //上下の揺れ幅
    public float floatSpeed     = 2f;   //上下の揺れスピード

    public float beamInterval   = 5f;   //ビーム発射間隔
    public float beamSpeed      = 10f;  //ビームの速さ

    private float angle;
    private float beamTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // シーン上の Player を自動で探す
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (bossHandSpawn == null || player == null) return;

        //ボスの周囲を飛ばす
        angle += orbitSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(
            Mathf.Cos(rad) * orbitRadius,
            Mathf.Sin(Time.time * floatSpeed) * floatAmplitude,
            Mathf.Sin(rad) * orbitRadius
        );

        transform.position = bossHandSpawn.position + offset;

        //常にプレイヤーの方向を向く
        transform.LookAt(player);

        //定期的にビームを撃たせる
        beamTimer += Time.deltaTime;
        if (beamTimer >= beamInterval)
        {
            beamTimer = 0f;
        }
    }
}