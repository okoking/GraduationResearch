using UnityEngine;

public class BeamShooter : MonoBehaviour
{
    [SerializeField] private GameObject beamPrefab;   // �r�[���̃v���n�u
    //[SerializeField] private Transform beamOrigin;    // ��̈ʒu�Ȃǔ��ˈʒu
    [SerializeField] private Camera playerCamera;     // ���C���J����
    private BeamCamera beamCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        beamCamera = GetComponent<BeamCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("joystick button 5"))
        {
            ShootBeam();
        }
    }

    void ShootBeam()
    {
        if (!beamCamera) return;

        // �J������������܂������L�΂����������擾
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        // �r�[���𐶐�
        Vector3 vec3 = transform.position;
        vec3.y += 1f;

        GameObject beam = Instantiate(beamPrefab, vec3, Quaternion.identity);

        // ���˕�����ݒ�
        beam.transform.rotation = Quaternion.LookRotation(ray.direction);
    }
}
