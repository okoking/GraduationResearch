using UnityEngine;

public class BossHand : MonoBehaviour
{
    //�^�[�Q�b�g(�v���C���[)
    Transform player;

    //��(�{�X�̎���o�������̂��)
    public Transform bossHandSpawn;

    //�r�[��(�U��)
    public GameObject beam;

    public float orbitRadius    = 5f;   //�{�X����̋���
    public float orbitSpeed     = 30f;  //��]�X�s�[�h
    public float floatAmplitude = 1f;   //�㉺�̗h�ꕝ
    public float floatSpeed     = 2f;   //�㉺�̗h��X�s�[�h

    public float beamInterval   = 5f;   //�r�[�����ˊԊu
    public float beamSpeed      = 10f;  //�r�[���̑���

    private float angle;
    private float beamTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �V�[����� Player �������ŒT��
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (bossHandSpawn == null || player == null) return;

        //�{�X�̎��͂��΂�
        angle += orbitSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(
            Mathf.Cos(rad) * orbitRadius,
            Mathf.Sin(Time.time * floatSpeed) * floatAmplitude,
            Mathf.Sin(rad) * orbitRadius
        );

        transform.position = bossHandSpawn.position + offset;

        //��Ƀv���C���[�̕���������
        transform.LookAt(player);

        //����I�Ƀr�[������������
        beamTimer += Time.deltaTime;
        if (beamTimer >= beamInterval)
        {
            beamTimer = 0f;
        }
    }
}