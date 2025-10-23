using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    private Transform playerTransform;
    private readonly List<EnemyAI> enemies = new();
    void Awake() => Instance = this;

    public void Register(EnemyAI enemy)
    {
        enemies.Add(enemy);
        if (playerTransform == null)
        {
            Debug.Log("�v���C���[������܂���");
        }
        //�v���C���[�����ɑ��݂��Ă���� EnemyAI �ɃZ�b�g
        if (playerTransform != null)
            Debug.Log("�v���C���[���擾����");
        enemy.SetPlayer(playerTransform);
    }
    public void SetPlayer(Transform player)
    {
        playerTransform = player;

        //�o�^�ς� EnemyAI �� player ��ʒm
        foreach (var e in enemies)
            e.SetPlayer(playerTransform);
    }
    public void Unregister(EnemyAI enemy) => enemies.Remove(enemy);

    public Transform GetPlayerTransform() => playerTransform;

    //�߂��̓G���擾�iBoids�p�j
    public List<EnemyAI> GetNearbyEnemies(EnemyAI self, float radius)
    {
        List<EnemyAI> nearby = new();
        foreach (var e in enemies)
        {
            if (e == self) continue;
            if (Vector3.Distance(e.transform.position, self.transform.position) < radius)
                nearby.Add(e);
        }
        return nearby;
    }

    //�x�񋤗L�F���͂̓G�ɒǐՂ�ʒm
    public void AlertNearbyEnemies(EnemyAI sender, float alertRadius)
    {
        foreach (var e in enemies)
        {
            if (e == sender) continue;
            if (Vector3.Distance(e.transform.position, sender.transform.position) <= alertRadius)
            {
                e.OnAlerted();
            }
        }
    }

    void Update()
    {
        if (playerTransform == null)
        {
            Debug.Log("�v���C���[��񂪂Ȃ����ߏ������I�����܂���");
            SetPlayer(GameObject.Find("Player").transform);

            if(playerTransform != null)
                Debug.Log("�v���C���[�����擾���܂���");
            else
                Debug.Log("�v���C���[�����擾�ł��܂���ł���");
            return;
        }

        foreach (var e in enemies)
        {
            float dist = Vector3.Distance(playerTransform.position, e.transform.position);
            if (dist < 15f) e.ManagedUpdate();       //�ߋ����͖��t���[��
            else if (dist < 30f) { if (Time.frameCount % 2 == 0) e.ManagedUpdate(); }  //������
            else { if (Time.frameCount % 5 == 0) e.ManagedUpdate(); }                  //������
        }
    }
}
