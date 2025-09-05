using UnityEngine;

public class EnemyUiSpawner : MonoBehaviour
{

    [SerializeField] private GameObject hpBarPrefab;
    private GameObject hpBarInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //HPバーを生成してこの敵の子にする
        hpBarInstance = Instantiate(hpBarPrefab, transform);

        //頭の上に配置
        hpBarInstance.transform.localPosition = new Vector3(0, 2f, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
