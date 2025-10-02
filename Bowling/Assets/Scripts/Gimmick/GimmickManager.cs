using UnityEngine;

public class GimmickManager : MonoBehaviour
{
    [SerializeField] private GameObject WallPrefab;
    [SerializeField] private Transform spawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WallAdd();
    }
    void WallAdd()
    {
        bool WallIsUse = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            WallIsUse = true;

        }
        if (WallPrefab != null && spawnPoint != null)
        {
            Instantiate(WallPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("WallPrefab Ç‹ÇΩÇÕ spawnPoint Ç™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒÅB");
        }
    }
}
