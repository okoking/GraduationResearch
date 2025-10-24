using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class BeamCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private CinemachineCamera normalCam;
    [SerializeField] private CinemachineCamera beamCam;
    bool isSootBeam = false;

    private Rigidbody rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        normalCam.gameObject.SetActive(!isSootBeam);
        beamCam.gameObject.SetActive(isSootBeam);

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            isSootBeam = !isSootBeam;
            normalCam.gameObject.SetActive(!isSootBeam);
            beamCam.gameObject.SetActive(isSootBeam);
            Debug.Log("X");
        }
    }


    void FixedUpdate()
    {
        if (!isSootBeam) return;

        // �J�����̑O��������ɁAY���݂̂Ō��������킹��
        Vector3 lookDirection = cameraTransform.forward;
        lookDirection.y = 0f; // �㉺�͖������Đ�����]�̂�
        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            rb.transform.rotation = Quaternion.Lerp(rb.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
