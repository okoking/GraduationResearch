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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(WallPrefab, spawnPoint.position, Quaternion.identity);
            Debug.LogWarning("WallPrefab �܂��� spawnPoint ���N���[������܂����B");

        }
        
    }
}
