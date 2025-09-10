using UnityEngine;

public class ArrowDrawer : MonoBehaviour
{
    // private Transform spawnPoint;   // �o���ʒu
    //private BallMovement ballMovement;
    public GameObject Ball;
    // �o������3D���f��
    public GameObject ArrowPrefab;
    //public GameObject ArrowPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Instantiate(Arrow, transform.transform);
        Ball = GetComponent<GameObject>();
        GameObject arrow = Instantiate(ArrowPrefab, Ball.transform);
        arrow.transform.localPosition = new Vector3(0f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal"); // A/D, ��/��, �X�e�B�b�NX
        float v = Input.GetAxis("Vertical");   // W/S, ��/��, �X�e�B�b�NY
        h = Mathf.Abs(h);
        if (v < 0f && h < .5f)
        {
            ArrowPrefab.SetActive(true);
            Debug.Log("�o��");
        }
        else
        {
            ArrowPrefab.SetActive(false);
        }
    }
}
