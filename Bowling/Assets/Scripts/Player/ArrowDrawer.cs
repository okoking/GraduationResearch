using UnityEngine;

public class ArrowDrawer : MonoBehaviour
{
    // private Transform spawnPoint;   // 出す位置
    private BallMovement ballMovement;
    //private GameObject Ball;
    // 出したい3Dモデル
    public GameObject ArrowPrefab;
    //public GameObject ArrowPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Instantiate(Arrow, transform.transform);
        ballMovement = GetComponent<BallMovement>();

        GameObject arrow = Instantiate(ArrowPrefab, transform);
        arrow.transform.localPosition = new Vector3(0f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //float h = Input.GetAxis("Horizontal"); // A/D, ←/→, スティックX
        //float v = Input.GetAxis("Vertical");   // W/S, ↑/↓, スティックY
        //h = Mathf.Abs(h);
        //if (v < 0f && h < .5f)
        //{
        //    ArrowPrefab.SetActive(true);
        //    Debug.Log("出現");
        //}
        //else
        //{
        //    ArrowPrefab.SetActive(false);
        //    Debug.Log("出現sinai");
        //}
        if (ballMovement.InputPower != Vector3.zero)
        {
            ArrowPrefab.SetActive(true);
            Debug.Log("出現");
        }
        else
        {
            ArrowPrefab.SetActive(false);
            Debug.Log("出現sinai");
        }
    }
}
