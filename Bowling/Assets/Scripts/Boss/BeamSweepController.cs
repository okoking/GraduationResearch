using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BeamSweepController : MonoBehaviour
{
    public float duration = 2f;        // �r�[�����o�Ă��鎞��
    public float sweepAngle = 90f;     // �U�蕝
    public float sweepSpeed = 90f;     // ��]���x
    public float beamLength = 50f;     // �ő咷��
    public float beamWidth = 0.2f;     // ����
    public LayerMask groundLayer;      // �n�ʃ��C���[��ݒ�i��FDefault�j

    private LineRenderer line;
    private float timer;
    private float currentAngle;
    private bool sweepingRight = true;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.startWidth = beamWidth;
        line.endWidth = beamWidth;
        line.useWorldSpace = true;

        currentAngle = -sweepAngle / 2f;

        Destroy(gameObject, duration);
    }

    void Update()
    {
        timer += Time.deltaTime;

        // ��]�p�x�X�V
        float step = sweepSpeed * Time.deltaTime * (sweepingRight ? 1 : -1);
        currentAngle += step;
        if (Mathf.Abs(currentAngle) >= sweepAngle / 2f)
            sweepingRight = !sweepingRight;

        // ��]�����v�Z
        Quaternion rot = Quaternion.Euler(0, currentAngle, 0);
        Vector3 dir = rot * transform.forward;

        // �n�ʃq�b�g�`�F�b�N
        Vector3 start = transform.position;
        Vector3 hitPoint = start + dir * beamLength; // �f�t�H���g�͍ő勗��

        if (Physics.Raycast(start, dir, out RaycastHit hit, beamLength, groundLayer))
        {
            hitPoint = hit.point;
        }

        // LineRenderer�X�V
        line.SetPosition(0, start);
        line.SetPosition(1, hitPoint);
    }
}