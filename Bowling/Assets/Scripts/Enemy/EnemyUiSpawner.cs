using UnityEngine;

public class EnemyUiSpawner : MonoBehaviour
{

    [SerializeField] private GameObject hpBarPrefab;
    private GameObject hpBarInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //HP�o�[�𐶐����Ă��̓G�̎q�ɂ���
        hpBarInstance = Instantiate(hpBarPrefab, transform);

        //���̏�ɔz�u
        hpBarInstance.transform.localPosition = new Vector3(0, 2f, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
