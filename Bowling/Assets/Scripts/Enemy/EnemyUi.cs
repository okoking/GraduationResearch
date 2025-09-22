using UnityEngine;

public class EnemyUi : MonoBehaviour
{
    private EnemyBase enemy;

    private Transform enemyTransform;

    public float height;

    private Vector3 initPos;

    Camera mainCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponentInParent<EnemyBase>();

        if (enemy != null)
        {
            enemyTransform = enemy.transform;
        }

        initPos = enemyTransform.position;

        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null) return;

        if (mainCam != null)
        {
            // �J�����̕���������
            transform.forward = mainCam.transform.forward;
        }

        // HP�������v�Z
        float ratio = (float)enemy.GetEnemyHp().GetCurrentHp() / enemy.GetEnemyHp().MaxHp;
        ratio = Mathf.Clamp01(ratio);

        // �o�[���k�߂�
        transform.localScale = new Vector3(ratio, height, 0.1f);
        transform.position = initPos + new Vector3((ratio - 1f) * 0.5f, 1f, 0f);
    }
}