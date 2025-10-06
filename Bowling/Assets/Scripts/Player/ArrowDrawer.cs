using UnityEngine;

public class ArrowDrawer : MonoBehaviour
{
    public Material NormalMaterial; // �C���X�y�N�^�[�Ŏw��
    public Material OutsideMaterial; // �C���X�y�N�^�[�Ŏw��

    public GameObject uiTextPrefab; // TextMeshPro�̃I�u�W�F�N�g������
    private GameObject uiText; // TextMeshPro�̃I�u�W�F�N�g������

    private new Renderer renderer;

    // private Transform spawnPoint;   // �o���ʒu
    private BallMovement ballMovement;
    //private GameObject Ball;
    // �o������3D���f��
    public GameObject ArrowPrefab;
    private GameObject arrow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �J�����̎Q��
        Camera cam = Camera.main;

        //Instantiate(Arrow, transform.transform);
        ballMovement = GetComponent<BallMovement>();

        arrow = Instantiate(ArrowPrefab, transform.position, cam.transform.rotation, transform);
        //// ��: ��ʂ̒��S�̍��W�����[���h�ɕϊ�����
        //Vector3 screenPos = new Vector3(Screen.width / 2, Screen.height / 2, 10f);
        //// z�̓J��������̋����i������w�肵�Ȃ��Ɛ��������[���h�ʒu�ɂȂ�Ȃ��j
        //Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        //// �I�u�W�F�N�g�����̈ʒu�ɔz�u
        //arrow.transform.position = worldPos;

        // �J�����̂ق�����������
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
            // Y�����X�V
            float angle = -Mathf.Atan2(ballMovement.GetInputPower().x, ballMovement.GetInputPower().z) * Mathf.Rad2Deg;
            currentEuler.z = angle;
            // ���f
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
