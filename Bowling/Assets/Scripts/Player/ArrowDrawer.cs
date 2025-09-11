using UnityEngine;

public class ArrowDrawer : MonoBehaviour
{
    // private Transform spawnPoint;   // �o���ʒu
    private BallMovement ballMovement;
    //private GameObject Ball;
    // �o������3D���f��
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
        //float h = Input.GetAxis("Horizontal"); // A/D, ��/��, �X�e�B�b�NX
        //float v = Input.GetAxis("Vertical");   // W/S, ��/��, �X�e�B�b�NY
        //h = Mathf.Abs(h);
        //if (v < 0f && h < .5f)
        //{
        //    ArrowPrefab.SetActive(true);
        //    Debug.Log("�o��");
        //}
        //else
        //{
        //    ArrowPrefab.SetActive(false);
        //    Debug.Log("�o��sinai");
        //}
        if (ballMovement.InputPower != Vector3.zero)
        {
            ArrowPrefab.SetActive(true);
            Debug.Log("�o��");
        }
        else
        {
            ArrowPrefab.SetActive(false);
            Debug.Log("�o��sinai");
        }
    }
}
