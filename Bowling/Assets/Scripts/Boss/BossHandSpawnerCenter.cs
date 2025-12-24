using UnityEngine;

public class BossHandSpawnerCenter : MonoBehaviour
{
    //ボスの手のオブジェクト
    public GameObject bossHandSpawerPrefab;

    //何体の手を生み出すか
    public int handNum = 2;

    public float offsetX = 3f;   //横の距離
    public float offsetY = 2f;   //上下の距離

    float halfWidth;
    float halfHeight;

    void Start()
    {
        //Renderer renderer = GetComponent<Renderer>();
        //halfWidth = renderer.bounds.extents.x;
        //halfHeight = renderer.bounds.extents.y;

        SpawnHands();
    }

    void SpawnHands()
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
                    offset = new Vector3(
                        (i == 0 ? -(halfWidth + offsetX) : (halfWidth + offsetX)),
                        offsetY,
                        0
                    );
                    break;
            }

            Instantiate(bossHandSpawerPrefab, transform.position + offset, Quaternion.identity);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}