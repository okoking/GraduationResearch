using TMPro;
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

    //public GameObject ArrowPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // カメラの参照
        Camera cam = Camera.main;

        //Instantiate(Arrow, transform.transform);
        ballMovement = GetComponent<BallMovement>();

        arrow = Instantiate(ArrowPrefab, transform.position, cam.transform.rotation);
        arrow.transform.localPosition = new Vector3(0f, 1f, -6f);
        
        renderer = arrow.GetComponent<Renderer>();
        renderer.material = NormalMaterial;

        uiText = Instantiate(uiTextPrefab);
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

            if (angle > ballMovement.SHOT_ANGLE_RANGE)
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
