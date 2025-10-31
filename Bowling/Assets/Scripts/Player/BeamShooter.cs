using UnityEngine;

public class BeamShooter : MonoBehaviour
{
    [SerializeField] private GameObject beamPrefab;   // ビームのプレハブ
    //[SerializeField] private Transform beamOrigin;    // 手の位置など発射位置
    [SerializeField] private Camera playerCamera;     // メインカメラ
    private BeamCamera beamCamera;
    private LockOnSystem lockOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        beamCamera = GetComponent<BeamCamera>();
        lockOn = GetComponent<LockOnSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("joystick button 5"))
        {
            if (beamCamera)
            {
                ShootBeam();
            }
            else
            {
                ShootMiniBeam();
            }
        }
    }

    void ShootBeam()
    {
        if (!beamCamera) return;

        // カメラ中央からまっすぐ伸ばした方向を取得
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        // ビームを生成
        Vector3 vec3 = transform.position;
        vec3.y += 1f;

        GameObject beam = Instantiate(beamPrefab, vec3, Quaternion.identity);

        // 発射方向を設定
        beam.transform.rotation = Quaternion.LookRotation(ray.direction);
    }

    void ShootMiniBeam()
    {
        Vector3 targetDir;

        if (lockOn.lockOnTarget != null)
        {
            targetDir = (lockOn.lockOnTarget.position - transform.position).normalized;
        }
        else
        {
            targetDir = transform.forward; // ロックオンしていなければ正面
        }

        var beam = Instantiate(beamPrefab, transform.position, Quaternion.LookRotation(targetDir));
    }
}
