using UnityEngine;

public class BeamSweepController : MonoBehaviour
{
    public float duration = 2f;        // �r�[�����o�Ă��鎞��
    public float sweepAngle = 90f;     // �U�蕝�i���E�ɉ��x��]���邩�j
    public float sweepSpeed = 90f;     // ��]���x�i�x/�b�j
    public float beamLength = 10f;     // �r�[���̒���
    public float beamWidth = 0.3f;     // �r�[���̑���

    private LineRenderer line;
    private float timer;
    private float currentAngle;
    private bool sweepingRight = true;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.startWidth = beamWidth;
        line.endWidth = beamWidth;
        line.positionCount = 2;
        line.useWorldSpace = true;

        //���������ݒ�
        currentAngle = -sweepAngle / 2f;

        //��莞�Ԍ�ɏ���
        Destroy(gameObject, duration);
    }

    void Update()
    {
        timer += Time.deltaTime;
        //�r�[�����E�������ɂ���Đi�ފp�x��ς���
        float step = sweepSpeed * Time.deltaTime * (sweepingRight ? 1 : -1);
        currentAngle += step;

        // �U�肫�����甽�Ε�����
        if (Mathf.Abs(currentAngle) >= sweepAngle / 2f)
            sweepingRight = !sweepingRight;

        // �����X�V
        Quaternion rot = Quaternion.Euler(0, currentAngle, 0);
        Vector3 dir = rot * transform.forward;

        // LineRenderer�Ő���`��
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + dir * beamLength);
    }
}