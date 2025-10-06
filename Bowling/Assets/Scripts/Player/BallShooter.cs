using UnityEngine;

public class BallShooter : MonoBehaviour
{
    [SerializeField] private Material NormalMaterial; // �C���X�y�N�^�[�Ŏw��
    [SerializeField] private Material OutsideMaterial;
    [SerializeField] private float SHOT_ANGLE_RANGE = 135f;
    [SerializeField] private GameObject ArrowPrefab;
    [SerializeField] private GameObject uiTextPrefab;

    // �{�[���𔭎˂���t�F�[�Y���ǂ���
    private bool isShootScene;

    private GameObject uiText; // TextMeshPro�̃I�u�W�F�N�g������

    private new Renderer renderer;

    // private Transform spawnPoint;   // �o���ʒu
    //private GameObject Ball;
    // �o������3D���f��
    private GameObject arrow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isShootScene = true;
        // �J�����̎Q��
        Camera cam = Camera.main;

        //Instantiate(Arrow, transform.transform);

        arrow = Instantiate(ArrowPrefab/*, transform.position, cam.transform.rotation, transform*/);

        arrow.transform.position = cam.transform.position + cam.transform.forward * 2f;
        
        // �J�����̂ق�����������
        arrow.transform.LookAt(2f * transform.position - cam.transform.position);

        renderer = arrow.GetComponent<Renderer>();
        renderer.material = NormalMaterial;

        uiText = Instantiate(uiTextPrefab, transform);

        arrow.SetActive(false);
        uiText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (ballMovement.GetisableShot())
        //{
        //    //float angle = Mathf.Abs(Mathf.Atan2(ballMovement.GetInputPower().x, ballMovement.GetInputPower().z) * Mathf.Rad2Deg);
        //    arrow.transform.localScale = new(.5f, -ballMovement.GetInputPower().magnitude * 1.5f, .5f);

        //    Vector3 currentEuler = arrow.transform.eulerAngles;
        //    // Y�����X�V
        //    float angle = -Mathf.Atan2(ballMovement.GetInputPower().x, ballMovement.GetInputPower().z) * Mathf.Rad2Deg;
        //    currentEuler.z = angle;
        //    // ���f
        //    arrow.transform.eulerAngles = currentEuler;
        //    angle = Mathf.Abs(Mathf.Atan2(ballMovement.GetInputPower().x, ballMovement.GetInputPower().z) * Mathf.Rad2Deg);

        //    if (angle > ballMovement.SHOT_ANGLE_RANGE && ballMovement.GetInputPower().magnitude > .5f)
        //    {
        //        arrow.SetActive(true);
        //        uiText.SetActive(false);
        //        renderer.material = NormalMaterial;
        //    }
        //    else
        //    {
        //        arrow.SetActive(true);
        //        uiText.SetActive(true);
        //        renderer.material = OutsideMaterial;
        //    }

        //    if (ballMovement.GetInputPower().magnitude == 0)
        //    {
        //        arrow.SetActive(false);
        //        uiText.SetActive(false);
        //    }
        //}
        //else
        //{
        //    arrow.SetActive(false);
        //    uiText.SetActive(false);
        //}

        Shot();
    }

    void Shot()
    {
        if (!isShootScene) return;

        float h = Input.GetAxis("Horizontal"); // A/D, ��/��, �X�e�B�b�NX
        float v = Input.GetAxis("Vertical");   // W/S, ��/��, �X�e�B�b�NY

        float shotAngle = Mathf.Atan2(h, v);

        Vector3 InputPower = new(h, 0f, v);

        // ����������̕\���ɂ��Ă̂��
        //// ���˂ł���͈͂܂ŃX�e�B�b�N��|���Ă��邩
        //bool canShoot = false;

        //if (shotAngle > SHOT_ANGLE_RANGE && InputPower.magnitude > .5f)
        //{
        //    canShoot = true;
        //}

        if (InputPower.magnitude > 0f)
        {
            //float angle = Mathf.Abs(Mathf.Atan2(ballMovement.GetInputPower().x, ballMovement.GetInputPower().z) * Mathf.Rad2Deg);
            arrow.transform.localScale = new(.5f, InputPower.magnitude * 1.5f, .5f);

            Vector3 currentEuler = arrow.transform.eulerAngles;
            // z�����X�V
            float angle = -shotAngle * Mathf.Rad2Deg;
            currentEuler.z = angle;
            // ���f
            arrow.transform.eulerAngles = currentEuler;
            angle = Mathf.Abs(shotAngle * Mathf.Rad2Deg);

            if (angle > SHOT_ANGLE_RANGE && InputPower.magnitude > .5f)
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

            if (InputPower.magnitude == 0)
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




        //// X�Ń��Z�b�g
        //if (Input.GetKeyDown("joystick button 2"))
        //{
        //    if (isableShot) // ���ˏ���
        //    {
        //        float angle = Mathf.Abs(Mathf.Atan2(h, v) * Mathf.Rad2Deg);
        //        if (angle > SHOT_ANGLE_RANGE && InputPower.magnitude > .5f)
        //        {
        //            isShot = true;
        //            isableShot = false;
        //        }
        //    }
        //    else           // ���Z�b�g
        //    {
        //        isableShot = true;
        //        rb.linearVelocity = Vector3.zero;
        //        rb.rotation = Quaternion.identity;
        //        rb.angularVelocity = Vector3.zero;
        //        rb.position = new(0f, 0.5f, -5f);
        //    }
        //}
    }

}
