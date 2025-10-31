using UnityEngine;
using System.Collections;

public class KariBeam : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float beamLength = 50f;
    [SerializeField] private float beamWidth = 0.2f;
    [SerializeField] private float MegabeamDuration = 3f; // ���b�ԏo�������邩
    [SerializeField] private float MinibeamDuration = 0.1f; // ���b�ԏo�������邩
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private Camera mainCam;

    private bool isFiring = false;
    private BeamCamera beamCamera;
    private LockOnSystem lockOn;

    void Start()
    {
        lineRenderer.enabled = false;
        lineRenderer.startWidth = beamWidth;
        lineRenderer.endWidth = beamWidth;
        beamCamera = GetComponent<BeamCamera>();
        lockOn = GetComponent<LockOnSystem>();
    }

    void Update()
    {
        // �{�^����������r�[������
        if (Input.GetKeyDown("joystick button 5") && !isFiring)
        {
            if (beamCamera.isSootBeam)
            {
                StartCoroutine(FireBeam());
            }
            else
            {
                StartCoroutine(FireLockOnBeam());
            }
        }
    }

    IEnumerator FireBeam()
    {
        isFiring = true;
        lineRenderer.enabled = true;

        float timer = 0f;
        while (timer < MegabeamDuration)
        {
            Vector3 start = transform.position;
            start.y += 1;
            Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // ����(0.5,0.5)
            Vector3 end = ray.origin + ray.direction * beamLength;

            //// Raycast�Ŗ�������
            //if (Physics.Raycast(start, transform.forward, out RaycastHit hit, beamLength, hitMask))
            //{
            //    end = hit.point;
            //    // ���������G�ɏ���
            //    if (hit.collider.CompareTag("Enemy"))
            //    {
            //        // Enemy�X�N���v�g��TakeDamage���ĂԂȂ�
            //        // hit.collider.GetComponent<Enemy>()?.TakeDamage(10);
            //    }
            //}

            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);

            timer += Time.deltaTime;
            yield return null;
        }

        lineRenderer.enabled = false;
        isFiring = false;
    }
    
    IEnumerator FireLockOnBeam()
    {
        isFiring = true;
        lineRenderer.enabled = true;

        float timer = 0f;
        while (timer < MinibeamDuration)
        {
            Vector3 start = transform.position;
            start.y += 1;
            Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // ����(0.5,0.5)
            Vector3 end = lockOn.lockOnTarget.position;

            //// Raycast�Ŗ�������
            //if (Physics.Raycast(start, transform.forward, out RaycastHit hit, beamLength, hitMask))
            //{
            //    end = hit.point;
            //    // ���������G�ɏ���
            //    if (hit.collider.CompareTag("Enemy"))
            //    {
            //        // Enemy�X�N���v�g��TakeDamage���ĂԂȂ�
            //        // hit.collider.GetComponent<Enemy>()?.TakeDamage(10);
            //    }
            //}

            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);

            timer += Time.deltaTime;
            yield return null;
        }

        lineRenderer.enabled = false;
        isFiring = false;
    }
}
