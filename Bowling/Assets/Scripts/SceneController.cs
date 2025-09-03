using UnityEngine;
//シーン管理機能
using UnityEngine.SceneManagement;
//型安全なコレクション
using System.Collections.Generic;
//非同期処理や並列処理管理機能
using System.Threading.Tasks;

public class SceneController : MonoBehaviour
{
    //シングルトン
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        //インスタンスが空であれば
        if(Instance == null)
        {
            Instance = this; DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
