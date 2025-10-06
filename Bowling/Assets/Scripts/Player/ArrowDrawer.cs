using UnityEngine;

public class ArrowDrawer : MonoBehaviour
{
    public Material NormalMaterial; // インスペクターで指定
    public Material OutsideMaterial; // インスペクターで指定

    public GameObject uiTextPrefab; // TextMeshProのオブジェクトを入れる
    private GameObject uiText; // TextMeshProのオブジェクトを入れる

    private new Renderer renderer;

    // private Transform spawnPoint;   // 出す位置
    private BallMovement ballMovement;
    //private GameObject Ball;
    // 出したい3Dモデル
    public GameObject ArrowPrefab;
    private GameObject arrow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // カメラの参照
        Camera cam = Camera.main;

        //Instantiate(Arrow, transform.transform);
        ballMovement = GetComponent<BallMovement>();

        arrow = Instantiate(ArrowPrefab, transform.position, cam.transform.rotation, transform);
        //// 例: 画面の中心の座標をワールドに変換する
        //Vector3 screenPos = new Vector3(Screen.width / 2, Screen.height / 2, 10f);
        //// zはカメラからの距離（これを指定しないと正しいワールド位置にならない）
        //Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        //// オブジェクトをその位置に配置
        //arrow.transform.position = worldPos;

        // カメラのほうを向かせる
        arrow.transform.LookAt(2 * transform.position - cam.transform.position);

        renderer = arrow.GetComponent<Renderer>();
        renderer.material = NormalMaterial;

        uiText = Instantiate(uiTextPrefab, transform);

        arrow.SetActive(false);
        uiText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ballMovement.GetisableShot())
        {
            //float angle = Mathf.Abs(Mathf.Atan2(ballMovement.GetInputPower().x, ballMovement.GetInputPower().z) * Mathf.Rad2Deg);
            arrow.transform.localScale = new(.5f, -ballMovement.GetInputPower().magnitude * 1.5f, .5f);

            Vector3 currentEuler = arrow.transform.eulerAngles;
            // Yだけ更新
            float angle = -Mathf.Atan2(ballMovement.GetInputPower().x, ballMovement.GetInputPower().z) * Mathf.Rad2Deg;
            currentEuler.z = angle;
            // 反映
            arrow.transform.eulerAngles = currentEuler;
            angle = Mathf.Abs(Mathf.Atan2(ballMovement.GetInputPower().x, ballMovement.GetInputPower().z) * Mathf.Rad2Deg);

            if (angle > ballMovement.SHOT_ANGLE_RANGE && ballMovement.GetInputPower().magnitude > .5f)
            {
                arrow.SetActive(true);
                uiText.SetActive(false);
                renderer.material = NormalMaterial;
            }
            else
            {
                arrow.SetActive(true);
                uiText.SetActive(true);
                renderer.material = OutsideMaterial;
            }

            if (ballMovement.GetInputPower().magnitude == 0)
            {
                arrow.SetActive(false);
                uiText.SetActive(false);
            }
        }
        else
        {
            arrow.SetActive(false);
            uiText.SetActive(false);
        }
    }
}
