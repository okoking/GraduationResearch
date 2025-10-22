using UnityEngine;

public class BossHandSpawer : MonoBehaviour
{

    //�{�X�̎�̃I�u�W�F�N�g
    public GameObject bossHandPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //��𐶐�
        GameObject hand = Instantiate(bossHandPrefab, transform.position, Quaternion.identity);

        //��̃X�N���v�g���擾���Ď���ɑΉ�������
        BossHand handScript = hand.GetComponent<BossHand>();
        if (handScript != null)
        {
            handScript.bossHandSpawn = this.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
