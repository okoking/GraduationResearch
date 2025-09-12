using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [ExecuteAlways] // �G�f�B�^��ł������悤�ɂ���
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [Header("�ړ��͈͐ݒ�")]
    public float minX = -5f;
    public float maxX = 5f;

    [Header("Gizmo�̕\���ݒ�")]
    public float gizmoHeight = 2f;
    public Color edgeColor = Color.red;
    public Color lineColor = Color.yellow;

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;

        float left = pos.x + minX;
        float right = pos.x + maxX;
        //���[�̏㉺�̓_��ݒ�
        Vector3 bottomLeft = new Vector3(left, pos.y - gizmoHeight / 2f, pos.z);
        Vector3 topLeft = new Vector3(left, pos.y + gizmoHeight / 2f, pos.z);
        //�E�[�̏㉺�̓_��ݒ�
        Vector3 bottomRight = new Vector3(right, pos.y - gizmoHeight / 2f, pos.z);
        Vector3 topRight = new Vector3(right, pos.y + gizmoHeight / 2f, pos.z);

        // ���̃��C��
        Gizmos.color = edgeColor;
        Gizmos.DrawLine(bottomLeft, topLeft);

        // �E�̃��C��
        Gizmos.DrawLine(bottomRight, topRight);

        // �͈͂��Ȃ����C��
        //x,y,z�̏��łȂ��ł���
        Gizmos.color = lineColor;
        Gizmos.DrawLine(
            new Vector3(left, pos.y, pos.z),
            new Vector3(right, pos.y, pos.z)
        );
    }
}
