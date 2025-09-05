using UnityEngine;

public class EnemyUi : MonoBehaviour
{
    private EnemyBase enemy;

    [SerializeField] private Transform bar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponentInParent<EnemyBase>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null) return;

        // �J�����̕���������
        transform.forward = Camera.main.transform.forward;

        // HP�������v�Z
        float ratio = (float)enemy.GetEnemyHp().GetCurrentHp() / enemy.GetEnemyHp().MaxHp;
        ratio = Mathf.Clamp01(ratio);

        // �o�[���k�߂�
        transform.localScale = new Vector3(ratio, 0.1f, 0.1f);
        transform.localPosition = new Vector3((ratio - 1f) * 0.5f, 2f, 0f);
    }
}