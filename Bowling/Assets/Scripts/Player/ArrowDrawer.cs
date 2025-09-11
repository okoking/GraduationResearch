using UnityEngine;
using UnityEngine.Windows;

public class ArrowDrawer : MonoBehaviour
{
    // private Transform spawnPoint;   // �o���ʒu
    private BallMovement ballMovement;
    //private GameObject Ball;
    // �o������3D���f��
    public GameObject ArrowPrefab;
    private GameObject arrow;

    //public GameObject ArrowPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Instantiate(Arrow, transform.transform);
        ballMovement = GetComponent<BallMovement>();

        arrow = Instantiate(ArrowPrefab, transform);
        arrow.transform.localPosition = new Vector3(0f, 0f, -3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (ballMovement.InputPower.z < 0f && ballMovement.InputPower.x < .5f)
        {
            arrow.transform.localScale = new(.5f, .5f, -ballMovement.InputPower.z);
            float angle = Mathf.Atan2(ballMovement.InputPower.x, ballMovement.InputPower.z) * Mathf.Rad2Deg;
            Vector3 currentEuler = arrow.transform.eulerAngles;
            // Y�����X�V
            currentEuler.y = angle;
            // ���f
            arrow.transform.eulerAngles = currentEuler; 
            arrow.SetActive(true);
        }
        else
        {
            arrow.SetActive(false);
        }
    }
}
