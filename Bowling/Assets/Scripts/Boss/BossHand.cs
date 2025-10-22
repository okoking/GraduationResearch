using UnityEngine;

public class BossHand : MonoBehaviour
{
    //ターゲット(プレイヤー)
    Transform player;

    //主(ボスの手を出す透明のやつ)
    public Transform bossHandSpawn;

    //ビーム(攻撃)
    public GameObject beamSweepPrefab;

    public float orbitRadius    = 5f;   //ボスからの距離
    public float orbitSpeed     = 30f;  //回転スピード
    public float floatAmplitude = 1f;   //上下の揺れ幅
    public float floatSpeed     = 2f;   //上下の揺れスピード

    public float beamInterval   = 5f;   //ビームのインターバル
    public float beamSpeed      = 10f;  //ビームのスピード

    private float angle;
    private float beamTimer;

    private bool isFiringBeam = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //プレイヤーとタグが付いたものを探す
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (bossHandSpawn == null || player == null) return;

        //ビーム中は動かさないようにする
        if (!isFiringBeam)
        {
            //中心を軸に動く
            angle += orbitSpeed * Time.deltaTime;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(
                Mathf.Cos(rad) * orbitRadius,
                Mathf.Sin(Time.time * floatSpeed) * floatAmplitude,
                Mathf.Sin(rad) * orbitRadius
            );
            transform.position = bossHandSpawn.position + offset;

            //プレイヤーを向く
            transform.LookAt(player);
        }

        //一定間隔でビーム
        beamTimer += Time.deltaTime;
        if (beamTimer >= beamInterval && !isFiringBeam)
        {
            beamTimer = 0f;
            //ビームを呼び出す
            StartCoroutine(ShootSweepBeam());
        }
    }

    private System.Collections.IEnumerator ShootSweepBeam()
    {
        isFiringBeam = true;

        //発射
        GameObject beam = Instantiate(beamSweepPrefab, transform.position, transform.rotation);
        float beamDuration = beam.GetComponent<BeamSweepController>().duration;

        //撃ってる時間は手を固定
        yield return new WaitForSeconds(beamDuration);

        isFiringBeam = false;
    }
}