using UnityEngine;

public class BossHandSpawner : MonoBehaviour
{
    //�{�X�̎�̃I�u�W�F�N�g
    public GameObject bossHandPrefab;

    //���̂̎�𐶂ݏo����
    public int handNum = 2;

    public float offsetX = 3f;   // ���̋���
    public float offsetY = 2f;   // �㉺�̋���

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < handNum; i++)
        {
            Vector3 offset = Vector3.zero;

            switch (handNum)
            {
                case 1:
                    offset = Vector3.zero;
                    break;
                case 2:
                    offset = new Vector3((i == 0 ? -offsetX : offsetX), 0, 0);
                    break;
                case 3:
                    if (i == 0) offset = new Vector3(-offsetX, 0, 0);
                    else if (i == 1) offset = new Vector3(offsetX, 0, 0);
                    else offset = new Vector3(0, offsetY, 0);
                    break;
                case 4:
                    if (i == 0) offset = new Vector3(-offsetX, 0, 0);
                    else if (i == 1) offset = new Vector3(offsetX, 0, 0);
                    else if (i == 2) offset = new Vector3(-offsetX / 2, offsetY, 0);
                    else offset = new Vector3(offsetX / 2, offsetY, 0);
                    break;
            }

            GameObject hand = Instantiate(bossHandPrefab, transform.position + offset, Quaternion.identity);
            hand.transform.SetParent(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
